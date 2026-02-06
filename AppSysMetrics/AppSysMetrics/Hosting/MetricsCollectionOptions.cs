namespace AppSysMetrics.Hosting;

public sealed class MetricsCollectionOptions
{
    public TimeSpan CollectionInterval { get; set; } = TimeSpan.FromSeconds(2);
    public int MaxHistorySize { get; set; } = 60;
}
