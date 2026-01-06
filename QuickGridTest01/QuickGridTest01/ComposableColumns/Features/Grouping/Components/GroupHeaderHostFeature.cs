using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.RowColumn.Core;
using QuickGridTest01.ComposableColumns.Core.Diagnostics;

namespace QuickGridTest01.ComposableColumns.Features.Grouping.Components;

public sealed class GroupHeaderHostFeature<TGridItem> : ICellRenderFeature<TGridItem>
    where TGridItem : class
{
    public int Priority => FeaturePriority.Grouping;

    private IGroupingFeature<TGridItem>? _grouping;
    private bool _loggedAttach;

    private const string ToolbarRenderedStateKey = "Grouping.ToolbarRendered";
    private const string ColumnIdStateKey = "Grouping.ColumnId";

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Column is IColumnFeatureProvider<TGridItem> provider)
            _grouping = provider.GetAllFeatures().OfType<IGroupingFeature<TGridItem>>().FirstOrDefault();

        if (!_loggedAttach)
        {
            _loggedAttach = true;
            var thisColumnId = context.GetState<string>(ColumnIdStateKey);
            var headerHostColumnId = TryGetHeaderHostColumnId(context);
            QgDebugLog.Write($"HostFeature attach: thisColumnId='{thisColumnId ?? "<null>"}', headerHostColumnId='{headerHostColumnId ?? "<null>"}', hasGroupingFeature={_grouping is not null}, columnType='{context.Column.GetType().FullName}'");
        }
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        _grouping = null;
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

        if (!GroupingSyntheticRowId.IsGroupingSynthetic(identifiable.Id))
        {
            renderNext();
            return;
        }

        if (GroupingSyntheticRowId.IsGroupHeaderSpacer(identifiable.Id))
            return;

        if (!GroupingSyntheticRowId.IsGroupHeaderMarker(identifiable.Id))
            return;

        var thisColumnId = context.GetState<string>(ColumnIdStateKey);
        var headerHostColumnId = TryGetHeaderHostColumnId(context);

        if (string.IsNullOrEmpty(thisColumnId))
        {
            renderNext();
            return;
        }

        var isHost = string.Equals(headerHostColumnId, thisColumnId, StringComparison.Ordinal);
        QgDebugLog.Write($"HostFeature marker id={identifiable.Id}, isHost={isHost}, thisColumnId='{thisColumnId}', hostId='{headerHostColumnId}'");

        if (!isHost)
            return;

        // Extract groupId from marker row and query coordinator for metadata
        var groupId = GroupHeaderRowId.GetGroupId(identifiable.Id);
        var coordinator = TryGetCoordinator(context);

        object? key = coordinator?.GetGroupKey(groupId);
        var itemCount = coordinator?.GetGroupItemCount(groupId) ?? 0;
        var isExpanded = coordinator?.IsGroupExpandedById(groupId) ?? false;

        QgDebugLog.Write($"HostFeature groupId={groupId}, key='{key}', itemCount={itemCount}, isExpanded={isExpanded}");

        builder.OpenElement(sequence++, "div");
        builder.AddAttribute(sequence++, "class", "qg-group-header-host");

        if (!context.HasState(ToolbarRenderedStateKey))
            context.SetState(ToolbarRenderedStateKey, true);

        // Create toggle callback that invokes ToggleGroupAsync with the real key
        Func<Task>? onToggle = key is not null || coordinator is not null
            ? () => _grouping.ToggleGroupAsync(key ?? "")
            : null;

        QgDebugLog.Write($"Rendering group header for marker id={identifiable.Id}");
        _grouping.RenderGroupHeader(builder, ref sequence, key, itemCount, isExpanded, onToggle);
        builder.CloseElement();
    }

    private static string? TryGetHeaderHostColumnId(FeatureContext<TGridItem> context)
    {
        var coord = TryGetCoordinator(context);
        if (coord is null)
        {
            QgDebugLog.Write($"HostFeature.TryGetHeaderHostColumnId: coordinator is null for columnType='{context.Column.GetType().FullName}'");
            return null;
        }

        var host = coord.HeaderHostColumnId;
        QgDebugLog.Write($"HostFeature.TryGetHeaderHostColumnId: coord={coord.GetHashCode()}, host='{host ?? "<null>"}', active={(coord.ActiveGrouping is null ? "<null>" : "set")}");
        return host;
    }

    private static GroupingCoordinator<TGridItem>? TryGetCoordinator(FeatureContext<TGridItem> context)
    {
        if (context.Grid is not ComposableGrid<TGridItem> grid)
            return null;

        return grid.GetOrCreateGroupingCoordinator();
    }
}
