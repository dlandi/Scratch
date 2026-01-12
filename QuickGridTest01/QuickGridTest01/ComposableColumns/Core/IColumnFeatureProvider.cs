using Microsoft.AspNetCore.Components;

namespace QuickGridTest01.ComposableColumns.Core;

internal interface IColumnFeatureProvider<TGridItem>
{
    /// <summary>
    /// Gets the unique identifier for this column, typically the column Title.
    /// Used for column ordering and grouping host identification.
    /// </summary>
    string? ColumnId { get; }

    /// <summary>
    /// Gets a RenderFragment that renders this column within a QuickGrid.
    /// </summary>
    RenderFragment RenderColumn { get; }

    IReadOnlyList<IColumnFeature<TGridItem>> GetAllFeatures();
}
