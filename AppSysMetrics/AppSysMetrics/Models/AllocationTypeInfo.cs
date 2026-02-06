namespace AppSysMetrics.Models;

public sealed record AllocationTypeInfo
{
    public required string TypeName { get; init; }
    public long TotalBytes { get; init; }
    public int AllocationCount { get; init; }
    public bool IsLargeObject { get; init; }
}
