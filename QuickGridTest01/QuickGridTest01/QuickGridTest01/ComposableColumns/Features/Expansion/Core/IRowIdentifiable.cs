namespace QuickGridTest01.ComposableColumns.Features.Expansion.Core;

/// <summary>
/// Enables Expansion feature components to identify real vs spacer rows.
/// Spacer rows use negative Id values.
/// </summary>
public interface IRowIdentifiable
{
    /// <summary>
    /// The unique identifier for this row.
    /// Real rows have positive IDs, spacer rows have negative IDs.
    /// </summary>
    int Id { get; set; }
}
