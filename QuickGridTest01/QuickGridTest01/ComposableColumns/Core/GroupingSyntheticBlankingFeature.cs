using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.RowColumn.Core;
using QuickGridTest01.ComposableColumns.Core.Diagnostics;
using System.Reflection;

namespace QuickGridTest01.ComposableColumns.Core;

internal sealed class GroupingSyntheticBlankingFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Grouping;

    private const string ColumnIdStateKey = "Grouping.ColumnId";

    private bool _loggedAttach;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        if (!_loggedAttach)
        {
            _loggedAttach = true;
            var thisColumnId = context.GetState<string>(ColumnIdStateKey);
            var headerHostColumnId = TryGetHeaderHostColumnId(context);
            QgDebugLog.Write($"BlankingFeature attach: thisColumnId='{thisColumnId ?? "<null>"}', headerHostColumnId='{headerHostColumnId ?? "<null>"}', columnType='{context.Column.GetType().FullName}'");
        }
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
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

        var thisColumnId = context.GetState<string>(ColumnIdStateKey);
        var headerHostColumnId = TryGetHeaderHostColumnId(context);

        var isHost = !string.IsNullOrEmpty(thisColumnId)
            && string.Equals(thisColumnId, headerHostColumnId, StringComparison.Ordinal);

        if (!isHost)
        {
            QgDebugLog.Write($"Blanking synthetic row id={identifiable.Id} for columnId='{thisColumnId ?? "<null>"}', hostId='{headerHostColumnId ?? "<null>"}'");
            builder.AddContent(sequence++, string.Empty);
            return;
        }

        renderNext();
    }

    private static string? TryGetHeaderHostColumnId(FeatureContext<TGridItem> context)
    {
        // Grouping is only supported for grids whose TGridItem provides a stable row identity.
        // Some demos/pages don't use grouping and don't implement IRowIdentifiable; in those cases
        // the blanking feature should behave as a no-op and must not force coordinator creation.
        if (typeof(IRowIdentifiable).IsAssignableFrom(typeof(TGridItem)) is false)
        {
            return null;
        }

        if (context.Grid is null)
        {
            QgDebugLog.Write($"TryGetHeaderHostColumnId: FeatureContext.Grid is null for columnType='{context.Column.GetType().FullName}'");
            return null;
        }

        var gridObj = context.Grid;

        // GetOrCreateGroupingCoordinator is internal; use NonPublic binding flags.
        var getCoord = gridObj.GetType().GetMethod(
            "GetOrCreateGroupingCoordinator",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        object? coordObj;
        try
        {
            coordObj = getCoord?.Invoke(gridObj, null);
        }
        catch (TargetInvocationException)
        {
            // If coordinator creation throws (e.g., grouping not supported for this grid item type),
            // treat grouping as inactive so the column renders normally.
            return null;
        }
        if (coordObj is null)
        {
            QgDebugLog.Write($"TryGetHeaderHostColumnId: coord is null gridType='{gridObj.GetType().FullName}', grid={gridObj.GetHashCode()}");
            return null;
        }

        var host = coordObj.GetType().GetProperty("HeaderHostColumnId", BindingFlags.Instance | BindingFlags.Public)?.GetValue(coordObj) as string;
        var active = coordObj.GetType().GetProperty("ActiveGrouping", BindingFlags.Instance | BindingFlags.Public)?.GetValue(coordObj);

        QgDebugLog.Write($"TryGetHeaderHostColumnId: grid={gridObj.GetHashCode()}, coord={coordObj.GetHashCode()}, host='{host ?? "<null>"}', active={(active is null ? "<null>" : "set")}");
        return host;
    }
}
