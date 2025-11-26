namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// The state of a row's form mode.
/// </summary>
public enum FormRowState
{
    /// <summary>
    /// Normal display mode, not editing.
    /// </summary>
    Reading,

    /// <summary>
    /// Form is open, user is editing.
    /// </summary>
    Editing,

    /// <summary>
    /// Save operation in progress.
    /// </summary>
    Saving
}
