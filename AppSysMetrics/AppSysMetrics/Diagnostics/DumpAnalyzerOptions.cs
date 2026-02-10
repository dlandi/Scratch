namespace AppSysMetrics.Diagnostics;

public sealed class DumpAnalyzerOptions
{
    public int MaxAnalysisHistory { get; set; } = 10;
    public int TopTypesCount { get; set; } = 50;

    // ── Phase 6: GC Root Analysis ──

    /// <summary>Maximum number of leak-suspect types to run root analysis on per capture.</summary>
    public int MaxRootAnalysisTypes { get; set; } = 5;

    /// <summary>Maximum number of roots to report per type.</summary>
    public int MaxRootsPerType { get; set; } = 10;

    /// <summary>Timeout for root analysis per type. Prevents a single type from blocking the pipeline.</summary>
    public TimeSpan RootAnalysisPerTypeTimeout { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>Global timeout for all root analysis. Hard cap on total analysis time.</summary>
    public TimeSpan RootAnalysisGlobalTimeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Whether to trace retention paths from root to target type.
    /// When false, root analysis still runs but skips the expensive heap-wide parent map.
    /// </summary>
    public bool TraceRetentionPaths { get; set; } = true;

    /// <summary>Maximum depth for backward retention path walk (hops from target to root).</summary>
    public int MaxRetentionPathDepth { get; set; } = 20;
}
