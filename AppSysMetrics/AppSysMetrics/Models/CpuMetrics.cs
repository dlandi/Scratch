namespace AppSysMetrics.Models;

public sealed record CpuMetrics
{
    public double CpuPercentage { get; init; }
    public TimeSpan TotalProcessorTime { get; init; }
    public int ProcessorCount { get; init; }
}
