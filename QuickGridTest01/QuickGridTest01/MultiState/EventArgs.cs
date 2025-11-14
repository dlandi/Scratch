using QuickGridTest01.MultiState.Core;

namespace QuickGridTest01.MultiState.Component;

/// <summary>
/// Event arguments for before editing begins (cancellable).
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of cell value</typeparam>
public class BeforeEditEventArgs<TGridItem, TValue> : EventArgs
{
    /// <summary>
    /// Gets the grid item being edited.
    /// </summary>
    public TGridItem Item { get; }

    /// <summary>
    /// Gets the current value.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets or sets whether the edit should be cancelled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of BeforeEditEventArgs.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="value">The current value</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public BeforeEditEventArgs(TGridItem item, TValue value)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Value = value;
    }
}

/// <summary>
/// Event arguments for when a value is changing during edit.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of cell value</typeparam>
public class ValueChangingEventArgs<TGridItem, TValue> : EventArgs
{
    /// <summary>
    /// Gets the grid item being edited.
    /// </summary>
    public TGridItem Item { get; }

    /// <summary>
    /// Gets the old value.
    /// </summary>
    public TValue OldValue { get; }

    /// <summary>
    /// Gets the new value.
    /// </summary>
    public TValue NewValue { get; }

    /// <summary>
    /// Initializes a new instance of ValueChangingEventArgs.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="oldValue">The old value</param>
    /// <param name="newValue">The new value</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public ValueChangingEventArgs(TGridItem item, TValue oldValue, TValue newValue)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        OldValue = oldValue;
        NewValue = newValue;
    }
}

/// <summary>
/// Event arguments for save operation results.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of cell value</typeparam>
public class SaveResultEventArgs<TGridItem, TValue> : EventArgs
{
    /// <summary>
    /// Gets the grid item that was saved.
    /// </summary>
    public TGridItem Item { get; }

    /// <summary>
    /// Gets the saved value.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets whether the save was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the error message if save failed.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Initializes a new instance of SaveResultEventArgs.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="value">The saved value</param>
    /// <param name="success">Whether the save was successful</param>
    /// <param name="errorMessage">Optional error message</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public SaveResultEventArgs(TGridItem item, TValue value, bool success, string? errorMessage = null)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Value = value;
        Success = success;
        ErrorMessage = errorMessage;
    }
}

/// <summary>
/// Event arguments for state transitions.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
public class StateTransitionEventArgs<TGridItem> : EventArgs
{
    /// <summary>
    /// Gets the grid item whose state changed.
    /// </summary>
    public TGridItem Item { get; }

    /// <summary>
    /// Gets the previous state.
    /// </summary>
    public CellState OldState { get; }

    /// <summary>
    /// Gets the new state.
    /// </summary>
    public CellState NewState { get; }

    /// <summary>
    /// Initializes a new instance of StateTransitionEventArgs.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="oldState">The previous state</param>
    /// <param name="newState">The new state</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public StateTransitionEventArgs(TGridItem item, CellState oldState, CellState newState)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        OldState = oldState;
        NewState = newState;
    }
}

/// <summary>
/// Event arguments for when edit is cancelled.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of cell value</typeparam>
public class CancelEditEventArgs<TGridItem, TValue> : EventArgs
{
    /// <summary>
    /// Gets the grid item being edited.
    /// </summary>
    public TGridItem Item { get; }

    /// <summary>
    /// Gets the original value that was restored.
    /// </summary>
    public TValue OriginalValue { get; }

    /// <summary>
    /// Gets the draft value that was discarded.
    /// </summary>
    public TValue DraftValue { get; }

    /// <summary>
    /// Initializes a new instance of CancelEditEventArgs.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="originalValue">The original value</param>
    /// <param name="draftValue">The draft value</param>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public CancelEditEventArgs(TGridItem item, TValue originalValue, TValue draftValue)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        OriginalValue = originalValue;
        DraftValue = draftValue;
    }
}