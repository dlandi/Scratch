namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// Specifies where the dragged item should be placed relative to the target item.
/// </summary>
public enum DropPosition
{
    /// <summary>
    /// Place the dragged item before (above) the target item.
    /// </summary>
    Before,

    /// <summary>
    /// Place the dragged item after (below) the target item.
    /// </summary>
    After
}

/// <summary>
/// Specifies how drag reordering is triggered.
/// </summary>
public enum ReorderTriggerMode
{
    /// <summary>
    /// Drag can only be initiated from the drag handle element.
    /// </summary>
    HandleOnly,

    /// <summary>
    /// Drag can be initiated from anywhere in the row.
    /// </summary>
    EntireRow
}

/// <summary>
/// The result of a reorder operation.
/// </summary>
public enum ReorderResult
{
    /// <summary>
    /// The reorder completed successfully.
    /// </summary>
    Success,

    /// <summary>
    /// The reorder was cancelled (by OnBeforeReorder or due to invalid target).
    /// </summary>
    Cancelled,

    /// <summary>
    /// The reorder failed (due to an error during execution).
    /// </summary>
    Failed
}
