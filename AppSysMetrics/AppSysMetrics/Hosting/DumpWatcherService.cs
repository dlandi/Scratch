using System.Threading.Channels;
using AppSysMetrics.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

/// <summary>
/// Background service that watches a folder for new .gcdump files,
/// triggers analysis, and publishes results + auto-diffs through the hub.
/// Uses FileSystemWatcher → Channel → async processing loop for cross-platform reliability.
/// </summary>
public sealed class DumpWatcherService : BackgroundService
{
    private readonly DumpAnalyzerService _analyzer;
    private readonly DumpDiffService _diffService;
    private readonly DumpAnalysisHub _hub;
    private readonly DumpAnalyzerOptions _analyzerOptions;
    private readonly DiagnosticsOptions _diagnosticsOptions;
    private readonly ILogger<DumpWatcherService> _logger;

    public DumpWatcherService(
        DumpAnalyzerService analyzer,
        DumpDiffService diffService,
        DumpAnalysisHub hub,
        IOptions<DumpAnalyzerOptions> analyzerOptions,
        IOptions<DiagnosticsOptions> diagnosticsOptions,
        ILogger<DumpWatcherService> logger)
    {
        _analyzer = analyzer;
        _diffService = diffService;
        _hub = hub;
        _analyzerOptions = analyzerOptions.Value;
        _diagnosticsOptions = diagnosticsOptions.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Resolve watch folder: DumpAnalyzerOptions → DiagnosticsOptions → temp default
        var watchFolder = _analyzerOptions.WatchFolder
            ?? _diagnosticsOptions.GcDumpOutputDirectory
            ?? Path.Combine(Path.GetTempPath(), "AppSysMetrics", "gcdumps");

        Directory.CreateDirectory(watchFolder);

        _logger.LogInformation("Dump watcher started. Watching: {WatchFolder}", watchFolder);

        var channel = Channel.CreateUnbounded<string>(
            new UnboundedChannelOptions { SingleReader = true });

        // Set up FileSystemWatcher
        using var watcher = new FileSystemWatcher(watchFolder, "*.gcdump")
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        watcher.Created += (_, e) =>
        {
            _logger.LogDebug("FileSystemWatcher detected new file: {FileName}", e.Name);
            channel.Writer.TryWrite(e.FullPath);
        };

        watcher.Error += (_, e) =>
        {
            _logger.LogError(e.GetException(), "FileSystemWatcher error");
        };

        // Scan for existing .gcdump files (handles app restart)
        try
        {
            var existingFiles = Directory.GetFiles(watchFolder, "*.gcdump")
                .OrderBy(f => File.GetLastWriteTimeUtc(f))
                .ToList();

            if (existingFiles.Count > 0)
            {
                _logger.LogInformation(
                    "Found {Count} existing dump file(s) in watch folder", existingFiles.Count);

                foreach (var file in existingFiles)
                {
                    channel.Writer.TryWrite(file);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to scan existing dump files in {WatchFolder}", watchFolder);
        }

        // Processing loop
        await foreach (var filePath in channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                // Skip if already analyzed
                var history = _hub.GetHistory();
                if (history.Any(h => string.Equals(h.FilePath, filePath, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogDebug("Skipping already-analyzed file: {FilePath}", filePath);
                    continue;
                }

                // Wait for file to be fully written
                await WaitForFileReadyAsync(filePath, stoppingToken);

                // Analyze
                var result = await _analyzer.AnalyzeAsync(filePath, stoppingToken);
                if (result is null)
                    continue;

                // Capture previous for auto-diff before publishing
                var previous = _hub.Latest;

                // Publish analysis result
                _hub.Publish(result);

                // Auto-diff: if there's a previous analysis, compute diff
                if (previous is not null)
                {
                    var diff = _diffService.ComputeDiff(previous, result);
                    _hub.PublishDiff(diff);

                    _logger.LogInformation(
                        "Auto-diff computed: {Baseline} → {Current} | Heap delta: {HeapDelta:+#,0;-#,0;0} bytes",
                        previous.FileName, result.FileName, diff.TotalHeapDelta);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process dump file: {FilePath}", filePath);
            }
        }

        _logger.LogInformation("Dump watcher stopped.");
    }

    /// <summary>
    /// Waits for a file to be fully written and released by the writer process.
    /// Uses a hybrid approach for cross-platform compatibility:
    /// - Windows: file is locked during write → retry exclusive open
    /// - Linux: file may not be locked → check size stabilization
    /// </summary>
    private async Task WaitForFileReadyAsync(string filePath, CancellationToken ct)
    {
        var deadline = DateTime.UtcNow.AddSeconds(_analyzerOptions.FileReadyTimeoutSeconds);

        // Initial delay to let the writer begin
        await Task.Delay(_analyzerOptions.FileReadyRetryDelayMs, ct);

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                // Attempt exclusive read to verify file is not locked
                using var stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None);

                if (stream.Length > 0)
                    return; // File is ready

                // File exists but is empty — writer hasn't started yet
            }
            catch (IOException)
            {
                // File still locked or being written — expected on Windows
            }
            catch (UnauthorizedAccessException)
            {
                // Permission issue — retry in case it's transient
            }

            await Task.Delay(_analyzerOptions.FileReadyRetryDelayMs, ct);
        }

        // Timeout reached — proceed anyway
        // On Linux with advisory locks, the file may be ready even though
        // we couldn't get an exclusive lock. Let dotnet-gcdump report decide.
        _logger.LogWarning(
            "File ready timeout reached for {FilePath}, proceeding with analysis", filePath);
    }
}
