namespace AppSysMetrics.Diagnostics.Models;

public sealed record HeapTypeDiff
{
    public required string TypeName { get; init; }
    public long BaselineCount { get; init; }
    public long CurrentCount { get; init; }
    public long DeltaCount { get; init; }
    public long BaselineSizeBytes { get; init; }
    public long CurrentSizeBytes { get; init; }
    public long DeltaSizeBytes { get; init; }
    public double GrowthPercent { get; init; }
}
