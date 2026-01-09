using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// Event args raised before a reorder operation is executed.
/// Set <see cref="Cancel"/> to true to prevent the reorder.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class RowBeforeReorderEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    /// <summary>
    /// The item being dragged.
    /// </summary>
    public required TGridItem DraggedItem { get; init; }

    /// <summary>
    /// The target item where the dragged item will be placed relative to.
    /// </summary>
    public required TGridItem TargetItem { get; init; }

    /// <summary>
    /// The position relative to the target item where the dragged item will be placed.
    /// </summary>
    public required DropPosition Position { get; init; }

    /// <summary>
    /// The original index of the dragged item before the reorder.
    /// </summary>
    public required int OldIndex { get; init; }

    /// <summary>
    /// The new index where the dragged item will be placed.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// Set to true to cancel the reorder operation.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Optional reason for cancelling the reorder. Used in <see cref="RowReorderCancelledEventArgs{TGridItem}"/>.
    /// </summary>
    public string? CancelReason { get; set; }
}

/// <summary>
/// Event args raised after a reorder operation has been executed.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class RowReorderedEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    /// <summary>
    /// The item that was reordered.
    /// </summary>
    public required TGridItem Item { get; init; }

    /// <summary>
    /// The original index of the item before the reorder.
    /// </summary>
    public required int OldIndex { get; init; }

    /// <summary>
    /// The new index of the item after the reorder.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// The complete list of items in their new order.
    /// </summary>
    public required IReadOnlyList<TGridItem> NewOrder { get; init; }
}

/// <summary>
/// Event args raised when a reorder operation is cancelled.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class RowReorderCancelledEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    /// <summary>
    /// The item that was being dragged when the reorder was cancelled.
    /// </summary>
    public required TGridItem Item { get; init; }

    /// <summary>
    /// The reason the reorder was cancelled.
    /// </summary>
    public required string Reason { get; init; }
}
