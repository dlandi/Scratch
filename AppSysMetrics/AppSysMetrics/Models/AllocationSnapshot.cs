namespace AppSysMetrics.Models;

public sealed record AllocationSnapshot
{
    public required DateTimeOffset CapturedAt { get; init; }
    public required IReadOnlyList<AllocationTypeInfo> TopAllocatingTypes { get; init; }
    public required IReadOnlyList<AllocationTypeInfo> RecentLargeObjectAllocations { get; init; }
    public long TotalTrackedBytes { get; init; }
    public int TotalTrackedCount { get; init; }
}
