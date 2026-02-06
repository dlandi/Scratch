namespace AppSysMetrics.Models;

public sealed record MetricsSnapshot
{
    public required long TimestampTicks { get; init; }
    public required DateTimeOffset CapturedAt { get; init; }
    public required ProcessMetrics Process { get; init; }
    public required CpuMetrics Cpu { get; init; }
    public required GcMetrics Gc { get; init; }
}
