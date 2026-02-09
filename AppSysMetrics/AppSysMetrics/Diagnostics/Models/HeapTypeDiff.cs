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

    // ── Allocation correlation (populated when both dumps have AllocationAtCapture) ──

    /// <summary>Cumulative bytes allocated for this type at baseline dump time.</summary>
    public long? BaselineAllocatedBytes { get; init; }

    /// <summary>Cumulative bytes allocated for this type at current dump time.</summary>
    public long? CurrentAllocatedBytes { get; init; }

    /// <summary>Bytes allocated between the two dumps (current − baseline). Represents throughput.</summary>
    public long? AllocatedBetweenBytes { get; init; }

    /// <summary>
    /// Retention ratio: heap growth / allocation throughput.
    /// Values near 1.0 = nearly everything retained (leak suspect).
    /// Values near 0.0 = healthy churn (most allocations collected).
    /// Null when allocation data is unavailable or throughput is zero.
    /// </summary>
    public double? RetentionRatio { get; init; }
}
