using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.QuickGrid;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.RowColumn.Core;

namespace QuickGridTest01.ComposableColumns.Features.Grouping.Components;

public sealed class GroupHeaderHostFeature<TGridItem> : ICellRenderFeature<TGridItem>
    where TGridItem : class
{
    public int Priority => FeaturePriority.Grouping;

    private IGroupingFeature<TGridItem>? _grouping;
    private bool _isHeaderHost;

    private const string ToolbarRenderedStateKey = "Grouping.ToolbarRendered";

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Column is IColumnFeatureProvider<TGridItem> provider)
            _grouping = provider.GetAllFeatures().OfType<IGroupingFeature<TGridItem>>().FirstOrDefault();

        var gridObj = context.Column.GetType().GetProperty("Grid")?.GetValue(context.Column);
        if (_grouping is not null && gridObj is not null)
        {
            var getCoord = gridObj.GetType().GetMethod("GetOrCreateGroupingCoordinator", Type.EmptyTypes);
            var coordObj = getCoord?.Invoke(gridObj, null);
            var headerHostId = coordObj?.GetType().GetProperty("HeaderHostColumnId")?.GetValue(coordObj) as string;
            _isHeaderHost = string.Equals(headerHostId, _grouping.ColumnId, StringComparison.Ordinal);
        }
        else
        {
            _isHeaderHost = false;
        }
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        _grouping = null;
        _isHeaderHost = false;
    }

    public void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext)
    {
        if (_grouping is null || item is not IRowIdentifiable identifiable)
        {
            renderNext();
            return;
        }

        // Synthetic grouping ids use the row's identity. Marker -> overlay. Spacer -> blank. Normal -> default.
        if (!GroupingSyntheticRowId.IsGroupingSynthetic(identifiable.Id))
        {
            renderNext();
            return;
        }

        if (GroupingSyntheticRowId.IsGroupHeaderSpacer(identifiable.Id))
        {
            return;
        }

        // Marker row
        if (!GroupingSyntheticRowId.IsGroupHeaderMarker(identifiable.Id))
            return;

        // Only header-host column renders overlay. All other columns blank marker rows.
        if (!_isHeaderHost)
            return;

        // Current implementation does not yet materialize key/itemCount per marker row.
        // Render header with null key/count (templates can choose to ignore).
        object? key = null;
        var isExpanded = false;
        var itemCount = 0;

        builder.OpenElement(sequence++, "div");
        builder.AddAttribute(sequence++, "class", "qg-group-header-host");

        // Toolbar: render only once per grid, gated to the first marker row encountered.
        if (!context.HasState(ToolbarRenderedStateKey))
        {
            context.SetState(ToolbarRenderedStateKey, true);
        }

        _grouping.RenderGroupHeader(builder, ref sequence, key, itemCount, isExpanded);
        builder.CloseElement();
    }
}
