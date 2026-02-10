namespace AppSysMetrics.Diagnostics.Models;

/// <summary>
/// Aggregate GC root analysis result across all analyzed leak-suspect types.
/// Attached to <see cref="DumpAnalysisResult.RootAnalysis"/> as an optional enrichment,
/// following the same nullable pattern as <see cref="DumpAnalysisResult.AllocationAtCapture"/>.
/// </summary>
public sealed record RootAnalysisResult
{
    /// <summary>When the root analysis was performed.</summary>
    public required DateTimeOffset AnalyzedAtUtc { get; init; }

    /// <summary>Per-type root analysis results for each predicted leak-suspect type.</summary>
    public required IReadOnlyList<TypeRootAnalysis> TypeAnalyses { get; init; }

    /// <summary>Total wall-clock time for all root analysis (across all types).</summary>
    public TimeSpan TotalDuration { get; init; }

    /// <summary>True if the overall analysis was cut short by the global timeout.</summary>
    public bool WasTimedOut { get; init; }

    /// <summary>Type names that were requested but could not be analyzed (e.g., not found on heap).</summary>
    public IReadOnlyList<string> SkippedTypes { get; init; } = [];
}
