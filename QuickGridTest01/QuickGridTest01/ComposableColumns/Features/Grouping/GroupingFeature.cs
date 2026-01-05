using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;
using QuickGridTest01.RowColumn.Core;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupingFeature<TGridItem, TValue> : IColumnFeature<TGridItem>, IGroupingFeature<TGridItem>, IDisposable
    where TGridItem : class
{
    public int Priority => FeaturePriority.Grouping;

    public string ColumnId { get; set; } = Guid.NewGuid().ToString("N");

    public bool IsActive { get; set; } = true;

    public Func<TGridItem, TValue>? GroupBy { get; set; }

    public bool InitiallyExpanded { get; set; } = true;

    public int GroupHeaderSlotSpan { get; set; } = 2;

    public RenderFragment<GroupHeaderContext<TGridItem, TValue>>? HeaderTemplate { get; set; }

    public RenderFragment<GroupToolbarContext>? ToolbarTemplate { get; set; }

    public GroupSortDirection GroupOrder { get; set; } = GroupSortDirection.Ascending;

    public IComparer<TValue>? GroupOrderComparer { get; set; }

    public FilterGroupOrder FilterBehavior { get; set; } = FilterGroupOrder.FilterThenGroup;

    public bool HideEmptyGroups { get; set; } = true;

    public NullKeyBehavior NullKeyBehavior { get; set; } = NullKeyBehavior.SeparateGroup;

    public string NullGroupLabel { get; set; } = "(No Value)";

    public IEqualityComparer<TValue>? KeyComparer { get; set; }

    public bool ShowExpandCollapseAllButtons { get; set; } = false;

    private FeatureContext<TGridItem>? _context;
    private Func<TGridItem, object?>? _groupByUntyped;
    private GroupStateManager<TValue>? _state;
    private GroupingCoordinator<TGridItem>? _coordinator;

    public Func<TGridItem, object?> GroupByUntyped
        => _groupByUntyped ?? throw new InvalidOperationException("Grouping feature is not attached.");

    public IEqualityComparer<object?>? KeyComparerUntyped => null;

    IEqualityComparer<object?>? IGroupingFeature<TGridItem>.KeyComparer => null;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.InvokeAsync is null)
            throw new InvalidOperationException("Grouping requires FeatureContext.InvokeAsync to be set (dispatcher was null)." );

        if (FilterBehavior == FilterGroupOrder.GroupThenFilter)
            throw new NotSupportedException("Grouping does not support FilterBehavior.GroupThenFilter.");

        if (GroupHeaderSlotSpan < 1)
            throw new ArgumentOutOfRangeException(nameof(GroupHeaderSlotSpan), "GroupHeaderSlotSpan must be >= 1.");

        _context = context;

        if (IsActive)
        {
            if (typeof(IRowIdentifiable).IsAssignableFrom(typeof(TGridItem)) is false)
                throw new InvalidOperationException("Active grouping requires TGridItem to implement IRowIdentifiable.");
        }

        var selector = GroupBy ?? ((FeatureContext<TGridItem, TValue>)context).GetValue;
        if (selector is null)
            throw new InvalidOperationException("Grouping requires a non-null GroupBy selector.");

        _groupByUntyped = item => selector(item);

        _state = new GroupStateManager<TValue>(KeyComparer);

        var grid = GetGridOrThrow();
        _coordinator = grid.GetOrCreateGroupingCoordinator();
        _coordinator.RegisterColumn(ColumnId, this);

        // First registered grouping column is the header host.
        // Coordinator already pins HeaderHostColumnId in RegisterColumn.
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        _context = null;
    }

    private ComposableGrid<TGridItem> GetGridOrThrow()
    {
        // The grid is cascaded to ComposableColumn only; grouping feature is attached to columns.
        // We rely on FeatureContext state to reach it.
        if (_context is null)
            throw new InvalidOperationException("GroupingFeature must be used inside a ComposableGrid.");

        var grid = (_context as dynamic)?.Grid as ComposableGrid<TGridItem>;
        if (grid is null)
            throw new InvalidOperationException("GroupingFeature must be used inside a ComposableGrid.");

        return grid;
    }

    public bool IsGroupExpanded(object key)
    {
        if (_state is null)
            return false;

        if (key is TValue typed)
            return _state.IsExpanded(typed);

        return false;
    }

    public async Task ToggleGroupAsync(object key)
    {
        if (_state is null)
            return;

        if (key is not TValue typed)
            return;

        await _state.ToggleAsync(typed);
    }

    public Task ExpandAllGroupsAsync() => Task.CompletedTask;

    public Task CollapseAllGroupsAsync() => Task.CompletedTask;

    public void RenderGroupHeader(
        RenderTreeBuilder builder,
        ref int sequence,
        object? key,
        int itemCount,
        bool isExpanded)
    {
        if (HeaderTemplate is not null)
        {
            var ctx = new GroupHeaderContext<TGridItem, TValue>(
                Key: key,
                ColumnId: ColumnId,
                ItemCount: itemCount,
                IsExpanded: isExpanded,
                GroupOrder: GroupOrder,
                HeaderTemplate: HeaderTemplate,
                ToggleAsync: () => ToggleGroupAsync(key ?? ""));

            builder.AddContent(sequence++, HeaderTemplate(ctx));
            return;
        }

        builder.OpenElement(sequence++, "div");
        builder.AddAttribute(sequence++, "class", "qg-group-header");
        builder.AddContent(sequence++, key?.ToString() ?? NullGroupLabel);
        builder.CloseElement();
    }

    public void Dispose()
    {
        _context = null;
        _groupByUntyped = null;
        _coordinator = null;
    }
}
