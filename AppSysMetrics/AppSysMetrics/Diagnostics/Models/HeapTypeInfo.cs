namespace AppSysMetrics.Diagnostics.Models;

public sealed record HeapTypeInfo
{
    public required string TypeName { get; init; }
    public long InstanceCount { get; init; }
    public long TotalSizeBytes { get; init; }
}
