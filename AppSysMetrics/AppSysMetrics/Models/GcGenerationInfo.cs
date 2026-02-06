namespace AppSysMetrics.Models;

public sealed record GcGenerationInfo
{
    public int Generation { get; init; }
    public long SizeBeforeBytes { get; init; }
    public long SizeAfterBytes { get; init; }
    public long FragmentationBeforeBytes { get; init; }
    public long FragmentationAfterBytes { get; init; }
}
