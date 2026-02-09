using System.Diagnostics;
using AppSysMetrics.Collection;
using AppSysMetrics.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

public sealed class DiagnosticsService : IDiagnosticsService
{
    private readonly IMetricsCollector _collector;
    private readonly ClrMdHeapAnalyzer _heapAnalyzer;
    private readonly DumpAnalysisHub _hub;
    private readonly DumpDiffService _diffService;
    private readonly AllocationEventListener _allocationListener;
    private readonly DiagnosticsOptions _options;
    private readonly ILogger<DiagnosticsService> _logger;

    public DiagnosticsService(
        IMetricsCollector collector,
        ClrMdHeapAnalyzer heapAnalyzer,
        DumpAnalysisHub hub,
        DumpDiffService diffService,
        AllocationEventListener allocationListener,
        IOptions<DiagnosticsOptions> options,
        ILogger<DiagnosticsService> logger)
    {
        _collector = collector;
        _heapAnalyzer = heapAnalyzer;
        _hub = hub;
        _diffService = diffService;
        _allocationListener = allocationListener;
        _options = options.Value;
        _logger = logger;
    }

    public ForceGcResult ForceGC()
    {
        _logger.LogInformation("Force GC requested");

        var beforeSnapshot = _collector.Collect();
        var sw = Stopwatch.StartNew();

        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);

        sw.Stop();
        var afterSnapshot = _collector.Collect();

        _logger.LogInformation(
            "Force GC completed in {Duration}ms. Heap before: {Before}, after: {After}",
            sw.ElapsedMilliseconds,
            beforeSnapshot.Gc.HeapSizeBytes,
            afterSnapshot.Gc.HeapSizeBytes);

        return new ForceGcResult
        {
            Before = beforeSnapshot.Gc,
            After = afterSnapshot.Gc,
            Duration = sw.Elapsed,
            PerformedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<GcDumpResult> CaptureGcDumpAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Capture in-process heap snapshot via ClrMD
            var result = await _heapAnalyzer.CaptureAndAnalyzeAsync(cancellationToken);
            if (result is null)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = "Heap snapshot failed. Check logs for details.",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            // 2. Enrich with allocation snapshot for correlation
            var allocSnapshot = _allocationListener.CreateSnapshot();
            result = result with { AllocationAtCapture = allocSnapshot };

            // 3. Capture previous result before publishing (for diff)
            var previous = _hub.Latest;

            // 4. Publish to hub — notifies all UI panels
            _hub.Publish(result);

            // 5. Auto-diff if we have a previous result
            if (previous is not null)
            {
                var diff = _diffService.ComputeDiff(previous, result);
                _hub.PublishDiff(diff);
                _logger.LogInformation(
                    "Auto-diff computed: heap delta {HeapDelta}, {TypeCount} type diffs",
                    diff.TotalHeapDelta,
                    diff.TypeDiffs.Count);
            }

            return new GcDumpResult
            {
                Success = true,
                CapturedAt = result.CapturedAtUtc
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture heap snapshot");
            return new GcDumpResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
    }

    public async Task<GcDumpResult> CaptureGcDumpFileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var processId = Environment.ProcessId;
            var outputDir = _options.GcDumpOutputDirectory
                ?? Path.Combine(Path.GetTempPath(), "AppSysMetrics", "gcdumps");

            Directory.CreateDirectory(outputDir);

            var fileName = $"gcdump_{processId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.gcdump";
            var filePath = Path.Combine(outputDir, fileName);

            _logger.LogInformation("Capturing GC dump for PID {Pid} to {Path}", processId, filePath);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet-gcdump",
                Arguments = $"collect -p {processId} -o \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            if (proc is null)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = "Failed to start dotnet-gcdump. Install with: dotnet tool install -g dotnet-gcdump",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            await proc.WaitForExitAsync(cancellationToken);
            var stderr = await proc.StandardError.ReadToEndAsync(cancellationToken);

            if (proc.ExitCode != 0)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = $"dotnet-gcdump exited with code {proc.ExitCode}: {stderr}",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            var fileInfo = new FileInfo(filePath);
            return new GcDumpResult
            {
                Success = true,
                FilePath = filePath,
                FileSizeBytes = fileInfo.Exists ? fileInfo.Length : 0,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture GC dump file");
            return new GcDumpResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
