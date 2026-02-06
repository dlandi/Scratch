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
}
