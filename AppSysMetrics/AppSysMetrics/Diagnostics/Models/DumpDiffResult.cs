namespace AppSysMetrics.Diagnostics.Models;

public sealed record DumpDiffResult
{
    public required DumpAnalysisResult Baseline { get; init; }
    public required DumpAnalysisResult Current { get; init; }
    public TimeSpan TimeBetweenDumps { get; init; }
    public required IReadOnlyList<HeapTypeDiff> TypeDiffs { get; init; }
    public long TotalHeapDelta { get; init; }
    public long TotalObjectDelta { get; init; }
}
