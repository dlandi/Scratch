namespace AppSysMetrics.Diagnostics.Models;

public sealed record DumpDiffResult
{
    public required DumpAnalysisResult Baseline { get; init; }
    public required DumpAnalysisResult Current { get; init; }
    public TimeSpan TimeBetweenDumps { get; init; }
    public required IReadOnlyList<HeapTypeDiff> TypeDiffs { get; init; }
    public long TotalHeapDelta { get; init; }
    public long TotalObjectDelta { get; init; }

    /// <summary>True when both dumps carry allocation snapshots, enabling throughput/retention analysis.</summary>
    public bool HasAllocationCorrelation { get; init; }

    /// <summary>Total bytes allocated (app-only) between the two dumps. Null when correlation unavailable.</summary>
    public long? TotalAllocatedBetween { get; init; }

    /// <summary>Total bytes collected between dumps (allocated − heap growth). Null when correlation unavailable.</summary>
    public long? TotalCollectedBetween { get; init; }
}
