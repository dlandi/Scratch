namespace QuickGridTest01.MultiState.Core;

/// <summary>
/// Represents the possible states of an editable cell.
/// </summary>
public enum CellState
{
    /// <summary>
    /// Cell is displaying its value in read-only mode.
    /// </summary>
    Reading = 0,

    /// <summary>
    /// Cell is in edit mode with an input control.
    /// </summary>
    Editing = 1,

    /// <summary>
    /// Cell is performing an asynchronous save operation.
    /// </summary>
    Loading = 2,

    /// <summary>
    /// An error occurred during the save operation.
    /// </summary>
    Error = 3
}
