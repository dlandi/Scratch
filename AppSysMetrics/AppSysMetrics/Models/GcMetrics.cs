namespace AppSysMetrics.Models;

public sealed record GcMetrics
{
    public long HeapSizeBytes { get; init; }
    public long FragmentedBytes { get; init; }
    public long TotalAvailableMemoryBytes { get; init; }
    public double MemoryLoadPercent { get; init; }
    public long TotalMemory { get; init; }
    public long TotalAllocatedBytes { get; init; }
    public double AllocationRateBytesPerSecond { get; init; }
    public double PauseTimePercentage { get; init; }
    public int Gen0Collections { get; init; }
    public int Gen1Collections { get; init; }
    public int Gen2Collections { get; init; }
    public long FinalizationPendingCount { get; init; }
    public IReadOnlyList<GcGenerationInfo> GenerationInfo { get; init; } = [];
}
