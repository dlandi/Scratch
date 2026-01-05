using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.RowColumn.Core;

namespace QuickGridTest01.ComposableColumns.Core;

internal sealed class GroupingSyntheticBlankingFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Grouping;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // No-op. This feature is always present and relies only on context and the injected column reference.
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // No-op.
    }

    public void RenderCell(RenderTreeBuilder builder, ref int sequence, TGridItem item, FeatureContext<TGridItem> context, Action renderNext)
    {
        if (item is not IRowIdentifiable identifiable)
        {
            renderNext();
            return;
        }

        if (!GroupingSyntheticRowId.IsGroupingSynthetic(identifiable.Id))
        {
            renderNext();
            return;
        }

        // For now, blank all grouping synthetic rows in all columns.
        // The header-host column will be permitted to render grouping UI via a dedicated feature.
        // This keeps Core decoupled from Features.Grouping while enforcing safe default behavior.
        builder.AddContent(sequence++, string.Empty);
    }
}
