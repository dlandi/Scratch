using QuickGridTest01.ComposableColumns.Features.Grouping;

namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// Coordinates drag-and-drop reordering operations for a grid.
/// Grid-owned, similar to GroupingCoordinator pattern.
/// Uses <c>where TGridItem : class</c> to match ComposableGrid constraint.
/// IRowIdentifiable is checked at runtime in the feature's OnAttach.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class ReorderCoordinator<TGridItem> : IDisposable
    where TGridItem : class
{
    private bool _disposed;

    /// <summary>
    /// The item currently being dragged, or null if no drag is in progress.
    /// </summary>
    public TGridItem? DraggedItem { get; private set; }

    /// <summary>
    /// The item currently being hovered over as a drop target, or null.
    /// </summary>
    public TGridItem? HoveredTarget { get; private set; }

    /// <summary>
    /// The current drop position relative to the hovered target.
    /// </summary>
    public DropPosition? CurrentDropPosition { get; private set; }

    /// <summary>
    /// Returns true if a drag operation is in progress.
    /// </summary>
    public bool IsDragging => DraggedItem is not null;

    /// <summary>
    /// The data source for order manipulation. User-provided, not coordinator-owned.
    /// Stored as object to avoid IRowIdentifiable constraint; cast by feature.
    /// </summary>
    internal object? DataSource { get; set; }

    /// <summary>
    /// Reference to the grouping coordinator for cross-group validation.
    /// </summary>
    internal GroupingCoordinator<TGridItem>? GroupingCoordinator { get; set; }

    /// <summary>
    /// The feature that registered with this coordinator.
    /// Only one RowReorderFeature is permitted per grid.
    /// Stored as object to avoid IRowIdentifiable constraint; cast by feature.
    /// </summary>
    internal object? Feature { get; set; }

    /// <summary>
    /// Returns true if a RowReorderFeature is registered and enabled.
    /// Used by ComposableGrid to suppress column sorting.
    /// </summary>
    public bool IsReorderingEnabled { get; internal set; }

    /// <summary>
    /// Raised when drag state changes, allowing the UI to refresh.
    /// </summary>
    public event Action? OnStateChanged;

    /// <summary>
    /// Starts a drag operation with the specified item.
    /// </summary>
    /// <param name="item">The item being dragged.</param>
    public void StartDrag(TGridItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        DraggedItem = item;
        HoveredTarget = null;
        CurrentDropPosition = null;

        OnStateChanged?.Invoke();
    }

    /// <summary>
    /// Updates the current hover target and drop position.
    /// </summary>
    /// <param name="target">The item being hovered over, or null.</param>
    /// <param name="position">The drop position relative to the target.</param>
    public void UpdateHover(TGridItem? target, DropPosition? position)
    {
        HoveredTarget = target;
        CurrentDropPosition = position;

        OnStateChanged?.Invoke();
    }

    /// <summary>
    /// Clears the current hover state without ending the drag.
    /// </summary>
    public void ClearHover()
    {
        HoveredTarget = null;
        CurrentDropPosition = null;

        OnStateChanged?.Invoke();
    }

    /// <summary>
    /// Cancels the current drag operation and clears all state.
    /// </summary>
    public void CancelDrag()
    {
        DraggedItem = null;
        HoveredTarget = null;
        CurrentDropPosition = null;

        OnStateChanged?.Invoke();
    }

    /// <summary>
    /// Releases resources used by the coordinator.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        DraggedItem = default;
        HoveredTarget = default;
        CurrentDropPosition = null;
        Feature = null;
        DataSource = null;
        GroupingCoordinator = null;
    }
}
