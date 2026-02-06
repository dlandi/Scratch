namespace AppSysMetrics.Diagnostics;

public sealed class DumpAnalyzerOptions
{
    public string? WatchFolder { get; set; }
    public int MaxAnalysisHistory { get; set; } = 10;
    public int FileReadyTimeoutSeconds { get; set; } = 30;
    public int FileReadyRetryDelayMs { get; set; } = 500;
    public int TopTypesCount { get; set; } = 50;
}
