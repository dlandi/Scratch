namespace AppSysMetrics.Models;

public sealed record AllocationSnapshot
{
    public required DateTimeOffset CapturedAt { get; init; }
    public required IReadOnlyList<AllocationTypeInfo> TopAllocatingTypes { get; init; }
    public required IReadOnlyList<AllocationTypeInfo> RecentLargeObjectAllocations { get; init; }
    public long TotalTrackedBytes { get; init; }
    public int TotalTrackedCount { get; init; }

    /// <summary>App allocations (types NOT in the AppSysMetrics.* namespace).</summary>
    public long AppTrackedBytes { get; init; }
    public int AppTrackedCount { get; init; }

    /// <summary>Library overhead (types in the AppSysMetrics.* namespace).</summary>
    public long LibraryTrackedBytes { get; init; }
    public int LibraryTrackedCount { get; init; }
}
