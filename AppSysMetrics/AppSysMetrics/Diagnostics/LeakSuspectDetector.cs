using AppSysMetrics.Diagnostics.Models;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Two-track leak suspect detection algorithm. Stateless — safe to call from any context.
///
/// Track 1 — High retention (≥ 80%): nearly everything allocated was retained.
/// Track 2 — Large absolute growth (≥ 1 MB or ≥ 20% of heap delta) with positive
///           retention below the Track 1 threshold. Catches diluted leaks like Byte[]
///           shared by framework pooling and user code.
/// </summary>
public static class LeakSuspectDetector
{
    public const double HighRetentionThreshold = 0.8;
    public const long AbsoluteGrowthFloor = 1_048_576; // 1 MB
    public const double HeapShareThreshold = 0.20;
    public const int MaxSuspects = 5;

    /// <summary>
    /// Returns the top leak suspects from a diff result.
    /// </summary>
    /// <param name="diff">The diff to analyze. Must have <see cref="DumpDiffResult.HasAllocationCorrelation"/>.</param>
    /// <param name="typeFilter">
    /// Optional filter that returns <c>true</c> for types to <b>exclude</b>.
    /// Pass <c>null</c> to include all types.
    /// </param>
    public static IReadOnlyList<HeapTypeDiff> Detect(
        DumpDiffResult diff,
        Func<string, bool>? typeFilter = null)
    {
        if (!diff.HasAllocationCorrelation)
            return [];

        var eligible = diff.TypeDiffs
            .Where(t => t.AllocatedBetweenBytes is > 0 && t.DeltaSizeBytes > 0);

        if (typeFilter is not null)
            eligible = eligible.Where(t => !typeFilter(t.TypeName));

        var eligibleList = eligible.ToList();

        // Track 1: High retention ratio
        var highRetention = eligibleList
            .Where(t => t.RetentionRatio >= HighRetentionThreshold);

        // Track 2: Large absolute heap growth with positive but sub-threshold retention
        var totalHeapDelta = Math.Max(1, diff.TotalHeapDelta);

        var largeAbsoluteGrowth = eligibleList
            .Where(t => t.RetentionRatio is > 0 and < HighRetentionThreshold)
            .Where(t => t.DeltaSizeBytes >= AbsoluteGrowthFloor
                     || (totalHeapDelta > 0
                         && (double)t.DeltaSizeBytes / totalHeapDelta >= HeapShareThreshold));

        return highRetention
            .Concat(largeAbsoluteGrowth)
            .DistinctBy(t => t.TypeName)
            .OrderByDescending(t => t.RetentionRatio)
            .ThenByDescending(t => t.DeltaSizeBytes)
            .Take(MaxSuspects)
            .ToList();
    }
}
