using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// Internal helper methods for reordering feature.
/// </summary>
internal static class ReorderingHelpers
{
    /// <summary>
    /// Determines whether the item is a synthetic row (group header, expansion spacer, etc.).
    /// Synthetic rows have negative IDs and should not participate in reordering.
    /// </summary>
    /// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item is a synthetic row (negative ID), false otherwise.</returns>
    public static bool IsSyntheticRow<TGridItem>(TGridItem item)
        where TGridItem : IRowIdentifiable
    {
        return item.Id < 0;
    }
}
