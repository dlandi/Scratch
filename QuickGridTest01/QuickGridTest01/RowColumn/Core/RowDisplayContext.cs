namespace QuickGridTest01.RowColumn.Core;

/// <summary>
/// Context provided to DisplayTemplate for custom trigger rendering.
/// </summary>
/// <typeparam name="TGridItem">The type of data item in the grid row</typeparam>
public class RowDisplayContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The data item for this row.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// True if any row in the grid is currently expanded.
    /// </summary>
    public bool IsAnyRowExpanded { get; init; }

    /// <summary>
    /// True if this row can be expanded (based on ConcurrentExpandBehavior).
    /// </summary>
    public bool CanExpand { get; init; }

    /// <summary>
    /// Call to expand this row.
    /// </summary>
    public Func<Task> ExpandAsync { get; init; } = default!;
}
