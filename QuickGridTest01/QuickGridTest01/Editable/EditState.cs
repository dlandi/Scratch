namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Tracks the editing state for a single cell.
/// </summary>
public class EditState<TValue>
{
    /// <summary>
    /// Whether the cell is currently in edit mode.
    /// </summary>
    public bool IsEditing { get; set; }

    /// <summary>
    /// Whether validation is currently in progress.
    /// </summary>
    public bool IsValidating { get; set; }

    /// <summary>
    /// The current value being edited (may be invalid).
    /// </summary>
    public TValue? CurrentValue { get; set; }

    /// <summary>
    /// The original value before editing started.
    /// </summary>
    public TValue? OriginalValue { get; set; }

    /// <summary>
    /// Validation results from all validators.
    /// </summary>
    public List<ValidationResult> ValidationResults { get; set; } = new();

    /// <summary>
    /// Whether the current value is valid.
    /// </summary>
    public bool IsValid => !ValidationResults.Any(r => !r.IsValid);

    /// <summary>
    /// Whether the value has been modified from the original.
    /// </summary>
    public bool IsDirty => !EqualityComparer<TValue>.Default.Equals(CurrentValue, OriginalValue);

    /// <summary>
    /// Gets all validation error messages.
    /// </summary>
    public IEnumerable<string> ErrorMessages => 
        ValidationResults.Where(r => !r.IsValid).Select(r => r.ErrorMessage!);

    /// <summary>
    /// Whether the cell's original value has been captured (initialization complete).
    /// </summary>
    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Captures the original value and enters edit mode (used for initialization or explicit edit).
    /// </summary>
    public void BeginEdit(TValue? value)
    {
        IsInitialized = true;
        IsEditing = true;
        OriginalValue = value;
        CurrentValue = value;
        ValidationResults.Clear();
    }

    /// <summary>
    /// Cancels edit mode and restores the original value.
    /// </summary>
    public void CancelEdit()
    {
        IsEditing = false;
        CurrentValue = OriginalValue;
        ValidationResults.Clear();
    }

    /// <summary>
    /// Commits the edit if valid.
    /// </summary>
    public bool CommitEdit()
    {
        if (!IsValid)
            return false;

        IsEditing = false;
        OriginalValue = CurrentValue;
        ValidationResults.Clear();
        return true;
    }

    /// <summary>
    /// Resets all state.
    /// </summary>
    public void Reset()
    {
        IsInitialized = false;
        IsEditing = false;
        IsValidating = false;
        CurrentValue = default;
        OriginalValue = default;
        ValidationResults.Clear();
    }
}
