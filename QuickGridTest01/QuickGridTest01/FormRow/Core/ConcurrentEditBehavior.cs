namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Defines behavior when user attempts to edit another row while one is already open.
/// </summary>
public enum ConcurrentEditBehavior
{
    /// <summary>
    /// Only one row can be in form mode. New edit blocked until current is closed.
    /// </summary>
    Block,

    /// <summary>
    /// Auto-cancel current edit (discard changes) and open new row.
    /// </summary>
    CancelCurrent,

    /// <summary>
    /// Auto-save current edit (if valid) and open new row.
    /// </summary>
    SaveCurrent,

    /// <summary>
    /// Allow multiple rows in form mode simultaneously.
    /// </summary>
    AllowMultiple
}
