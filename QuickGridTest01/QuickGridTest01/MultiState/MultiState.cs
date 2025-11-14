namespace QuickGridTest01.MultiState.Core;

/// <summary>
/// Encapsulates the complete state of an editable cell.
/// </summary>
/// <typeparam name="TValue">The type of value being edited</typeparam>
public class MultiState<TValue>
{
    /// <summary>
    /// The current state of the cell.
    /// </summary>
    public CellState CurrentState { get; set; } = CellState.Reading;

    /// <summary>
    /// The original value before any edits.
    /// </summary>
    public TValue? OriginalValue { get; set; }

    /// <summary>
    /// The draft value being edited.
    /// </summary>
    public TValue? DraftValue { get; set; }

    /// <summary>
    /// Error message from a failed operation.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Collection of validation error messages.
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new();

    /// <summary>
    /// Indicates whether there are any validation errors.
    /// </summary>
    public bool HasValidationErrors => ValidationErrors.Any();

    /// <summary>
    /// When the loading operation started.
    /// </summary>
    public DateTime? LoadingStartTime { get; set; }

    /// <summary>
    /// Number of retry attempts.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// The previous state before the current one.
    /// </summary>
    public CellState? PreviousState { get; private set; }

    /// <summary>
    /// Indicates if the value has been modified.
    /// </summary>
    public bool IsModified => !EqualityComparer<TValue>.Default.Equals(OriginalValue, DraftValue);

    /// <summary>
    /// Transitions to a new state, recording the previous state.
    /// </summary>
    public void TransitionTo(CellState newState)
    {
        PreviousState = CurrentState;
        CurrentState = newState;
    }

    /// <summary>
    /// Commits the edit, making the draft value the new original value.
    /// </summary>
    public void CommitEdit()
    {
        OriginalValue = DraftValue;
        ValidationErrors.Clear();
        ErrorMessage = null;
    }

    /// <summary>
    /// Cancels the edit, restoring the original value to the draft.
    /// </summary>
    public void CancelEdit()
    {
        DraftValue = OriginalValue;
        ValidationErrors.Clear();
        ErrorMessage = null;
    }

    /// <summary>
    /// Resets to Reading state with the original value.
    /// </summary>
    public void Reset()
    {
        CurrentState = CellState.Reading;
        DraftValue = OriginalValue;
        ErrorMessage = null;
        ValidationErrors.Clear();
        LoadingStartTime = null;
        PreviousState = null;
    }
}
