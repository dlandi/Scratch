namespace QuickGridTest01.RowColumn.Core;

/// <summary>
/// Defines how the expanded mode is triggered for a row.
/// </summary>
public enum RowTriggerMode
{
    /// <summary>
    /// Renders an Edit button in the column cell. Click opens expanded content.
    /// </summary>
    Button,

    /// <summary>
    /// Entire row is clickable. Click anywhere opens expanded content.
    /// </summary>
    RowClick,

    /// <summary>
    /// Developer controls via DisplayTemplate. Use context.ExpandAsync().
    /// </summary>
    Custom
}
