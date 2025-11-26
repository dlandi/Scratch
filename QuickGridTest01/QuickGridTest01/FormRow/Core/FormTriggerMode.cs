namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Defines how the form mode is triggered for a row.
/// </summary>
public enum FormTriggerMode
{
    /// <summary>
    /// Renders an Edit button in the column cell. Click opens form.
    /// </summary>
    Button,

    /// <summary>
    /// Entire row is clickable. Click anywhere opens form.
    /// </summary>
    RowClick,

    /// <summary>
    /// Developer controls via DisplayTemplate. Use context.EnterFormModeAsync().
    /// </summary>
    Custom
}
