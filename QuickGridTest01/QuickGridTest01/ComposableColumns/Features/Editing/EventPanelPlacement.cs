namespace QuickGridTest01.ComposableColumns.Features.Editing;

/// <summary>
/// Specifies where the event panel should be rendered relative to the grid.
/// </summary>
public enum EventPanelPlacement
{
    /// <summary>
    /// No event panel is auto-rendered (default).
    /// The event stream is still available via cascading parameter.
    /// </summary>
    None = 0,

    /// <summary>
    /// Event panel is rendered above the grid.
    /// </summary>
    Top = 1,

    /// <summary>
    /// Event panel is rendered below the grid.
    /// </summary>
    Bottom = 2,

    /// <summary>
    /// Event panel is rendered to the left of the grid.
    /// </summary>
    Left = 3,

    /// <summary>
    /// Event panel is rendered to the right of the grid.
    /// </summary>
    Right = 4
}
