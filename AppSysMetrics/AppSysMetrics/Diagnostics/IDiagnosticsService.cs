using AppSysMetrics.Models;

namespace AppSysMetrics.Diagnostics;

public interface IDiagnosticsService
{
    ForceGcResult ForceGC();
    Task<GcDumpResult> CaptureGcDumpAsync(CancellationToken cancellationToken = default);
    Task<GcDumpResult> CaptureGcDumpFileAsync(CancellationToken cancellationToken = default);
}

public sealed record ForceGcResult
{
    public required GcMetrics Before { get; init; }
    public required GcMetrics After { get; init; }
    public required TimeSpan Duration { get; init; }
    public required DateTimeOffset PerformedAt { get; init; }
}

public sealed record GcDumpResult
{
    public bool Success { get; init; }
    public string? FilePath { get; init; }
    public string? ErrorMessage { get; init; }
    public long FileSizeBytes { get; init; }
    public DateTimeOffset CapturedAt { get; init; }
}
