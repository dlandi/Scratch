namespace AppSysMetrics.Diagnostics.Models;

/// <summary>
/// Root analysis results for a single type identified as a leak suspect.
/// Contains the top roots retaining instances of this type on the managed heap.
/// </summary>
public sealed record TypeRootAnalysis
{
    /// <summary>Fully-qualified type name that was analyzed.</summary>
    public required string TypeName { get; init; }

    /// <summary>Total number of distinct GC roots found that retain instances of this type.</summary>
    public int TotalRootCount { get; init; }

    /// <summary>
    /// Top roots retaining this type, limited by <c>DumpAnalyzerOptions.MaxRootsPerType</c>.
    /// Sorted by <see cref="GcRootInfo.RetainedInstanceCount"/> descending.
    /// </summary>
    public required IReadOnlyList<GcRootInfo> Roots { get; init; }

    /// <summary>True if analysis was truncated due to timeout or max-root limits.</summary>
    public bool WasTruncated { get; init; }

    /// <summary>Wall-clock time spent analyzing roots for this specific type.</summary>
    public TimeSpan AnalysisDuration { get; init; }
}
