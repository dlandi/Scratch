using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.RowColumn.Core;

/// <summary>
/// Context provided to ExpandedTemplate and cascaded to child components.
/// Provides the item and collapse functionality for the expanded row.
/// </summary>
/// <typeparam name="TGridItem">The type of data item being displayed</typeparam>
public class RowExpandedContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The data item for this row.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// Collapses the row (closes the overlay).
    /// </summary>
    public Func<Task> CollapseAsync { get; init; } = default!;
}
