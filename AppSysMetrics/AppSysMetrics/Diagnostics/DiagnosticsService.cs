using System.Diagnostics;
using AppSysMetrics.Collection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

public sealed class DiagnosticsService : IDiagnosticsService
{
    private readonly IMetricsCollector _collector;
    private readonly ILogger<DiagnosticsService> _logger;
    private readonly DiagnosticsOptions _options;

    public DiagnosticsService(
        IMetricsCollector collector,
        IOptions<DiagnosticsOptions> options,
        ILogger<DiagnosticsService> logger)
    {
        _collector = collector;
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

    public async Task<GcDumpResult> CaptureGcDumpAsync(CancellationToken cancellationToken)
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
            _logger.LogError(ex, "Failed to capture GC dump");
            return new GcDumpResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
