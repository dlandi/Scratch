using QuickGridTest01.FormRow.Core;

namespace QuickGridTest01.FormRow.Events;

/// <summary>
/// Event arguments for successful save operations.
/// </summary>
/// <typeparam name="TGridItem">The type of data item</typeparam>
public class FormSaveEventArgs<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The item that was saved.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// Dictionary of property names and their saved values.
    /// </summary>
    public Dictionary<string, object?> SavedValues { get; init; } = new();
}

/// <summary>
/// Event arguments for cancelled edit operations.
/// </summary>
/// <typeparam name="TGridItem">The type of data item</typeparam>
public class FormCancelEventArgs<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The item whose edit was cancelled.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// True if there were unsaved changes.
    /// </summary>
    public bool WasDirty { get; init; }
}

/// <summary>
/// Event arguments for the before-edit event (cancellable).
/// </summary>
/// <typeparam name="TGridItem">The type of data item</typeparam>
public class FormBeforeEditEventArgs<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The item about to enter edit mode.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// Set to true to prevent entering form mode.
    /// </summary>
    public bool Cancel { get; set; }
}

/// <summary>
/// Event arguments for form state transitions.
/// </summary>
/// <typeparam name="TGridItem">The type of data item</typeparam>
public class FormStateChangedEventArgs<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The item whose state changed.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// The previous state.
    /// </summary>
    public FormRowState OldState { get; init; }

    /// <summary>
    /// The new state.
    /// </summary>
    public FormRowState NewState { get; init; }
}
