namespace QuickGridTest01.RowColumn.Core;

/// <summary>
/// Defines behavior when user attempts to expand another row while one is already open.
/// </summary>
public enum ConcurrentExpandBehavior
{
    /// <summary>
    /// Only one row can be expanded. New expand blocked until current is closed.
    /// </summary>
    Block,

    /// <summary>
    /// Auto-collapse current row and open new row.
    /// </summary>
    CollapseCurrent,

    /// <summary>
    /// Allow multiple rows expanded simultaneously.
    /// </summary>
    AllowMultiple
}
