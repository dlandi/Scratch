namespace QuickGridTest01.LogDetail.Core;

/// <summary>
/// Read-only context for an expanded log detail row.
/// Provides the item and collapse action to child templates.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
public class LogDetailContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The grid item being displayed in expanded mode.
    /// </summary>
    public required TGridItem Item { get; init; }

    /// <summary>
    /// Action to collapse this row's detail view.
    /// </summary>
    public required Func<Task> CollapseAsync { get; init; }

    /// <summary>
    /// Whether this row is currently expanded.
    /// </summary>
    public bool IsExpanded { get; set; } = true;
}
