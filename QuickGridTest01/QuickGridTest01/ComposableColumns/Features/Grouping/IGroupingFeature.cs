using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public interface IGroupingFeature<TGridItem>
{
    string ColumnId { get; }

    bool IsActive { get; }

    GroupSortDirection GroupOrder { get; }

    FilterGroupOrder FilterBehavior { get; }

    NullKeyBehavior NullKeyBehavior { get; }

    int GroupHeaderSlotSpan { get; }

    IEqualityComparer<object?>? KeyComparer { get; }

    Func<TGridItem, object?> GroupByUntyped { get; }

    bool ShowExpandCollapseAllButtons { get; }

    bool IsGroupExpanded(object key);

    Task ToggleGroupAsync(object key);

    Task ExpandAllGroupsAsync();

    Task CollapseAllGroupsAsync();

    void RenderGroupHeader(
        RenderTreeBuilder builder,
        ref int sequence,
        object? key,
        int itemCount,
        bool isExpanded,
        Func<Task>? onToggle);
}
