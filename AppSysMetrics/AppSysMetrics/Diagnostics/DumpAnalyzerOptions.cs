namespace AppSysMetrics.Diagnostics;

public sealed class DumpAnalyzerOptions
{
    public int MaxAnalysisHistory { get; set; } = 10;
    public int TopTypesCount { get; set; } = 50;
}
