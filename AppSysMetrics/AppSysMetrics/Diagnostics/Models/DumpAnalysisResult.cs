using AppSysMetrics.Models;

namespace AppSysMetrics.Diagnostics.Models;

public sealed record DumpAnalysisResult
{
    public required string FilePath { get; init; }
    public required string FileName { get; init; }
    public required DateTimeOffset CapturedAtUtc { get; init; }
    public required DateTimeOffset AnalyzedAtUtc { get; init; }
    public long FileSizeBytes { get; init; }
    public long TotalHeapBytes { get; init; }
    public long TotalObjectCount { get; init; }
    public required IReadOnlyList<HeapTypeInfo> TopTypes { get; init; }

    /// <summary>Number of types whose names could not be resolved (UNKNOWN 0x...).
    /// A non-zero value indicates the dump may be incomplete or the diagnostic port was busy.</summary>
    public int UnresolvedTypeCount { get; init; }

    /// <summary>
    /// Allocation snapshot captured at the time this dump was analyzed.
    /// Enables correlation: "allocated (lifetime) vs retained (on heap)" per type.
    /// Null for dumps loaded from disk on app restart (no live listener at that time).
    /// </summary>
    public AllocationSnapshot? AllocationAtCapture { get; init; }
}
