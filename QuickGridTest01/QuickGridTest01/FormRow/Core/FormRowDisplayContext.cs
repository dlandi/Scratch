namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Context provided to DisplayTemplate for custom trigger rendering.
/// </summary>
/// <typeparam name="TGridItem">The type of data item in the grid row</typeparam>
public class FormRowDisplayContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The data item for this row.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// True if any row in the grid is currently in form mode.
    /// </summary>
    public bool IsAnyRowInFormMode { get; init; }

    /// <summary>
    /// True if this row can enter form mode (based on ConcurrentEditBehavior).
    /// </summary>
    public bool CanEnterFormMode { get; init; }

    /// <summary>
    /// Call to enter form mode for this row.
    /// </summary>
    public Func<Task> EnterFormModeAsync { get; init; } = default!;
}
