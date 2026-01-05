using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Grouping.Components;
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
    private Func<TGridItem, TValue>? _groupByTyped;
    private IEqualityComparer<object?>? _keyComparerUntyped;

    public Func<TGridItem, object?> GroupByUntyped
        => _groupByUntyped ?? throw new InvalidOperationException("Grouping feature is not attached.");

    IEqualityComparer<object?>? IGroupingFeature<TGridItem>.KeyComparer => _keyComparerUntyped;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (string.IsNullOrWhiteSpace(ColumnId))
            throw new InvalidOperationException("Grouping requires a non-empty ColumnId.");

        if (context.InvokeAsync is null)
            throw new InvalidOperationException("Grouping requires FeatureContext.InvokeAsync to be set (dispatcher was null)." );

        if (FilterBehavior == FilterGroupOrder.GroupThenFilter)
            throw new NotSupportedException("Grouping does not support FilterBehavior.GroupThenFilter.");

        if (GroupHeaderSlotSpan < 1)
            throw new ArgumentOutOfRangeException(nameof(GroupHeaderSlotSpan), "GroupHeaderSlotSpan must be >= 1.");

        _context = context;

        if (IsActive && typeof(IRowIdentifiable).IsAssignableFrom(typeof(TGridItem)) is false)
            throw new InvalidOperationException("Active grouping requires TGridItem to implement IRowIdentifiable.");

        var typedContext = context as FeatureContext<TGridItem, TValue>;
        _groupByTyped = GroupBy ?? typedContext?.GetValue;
        if (_groupByTyped is null)
            throw new InvalidOperationException("Grouping requires a non-null GroupBy selector.");

        _groupByUntyped = item => _groupByTyped(item);
        _keyComparerUntyped = KeyComparer is null ? null : new KeyComparerAdapter(KeyComparer);

        _state = new GroupStateManager<TValue>(KeyComparer);

        var grid = GetGridOrThrow(context);
        _coordinator = grid.GetOrCreateGroupingCoordinator();
        _coordinator.RegisterColumn(ColumnId, this);

        // First registered grouping column is the header host.
        // Coordinator already pins HeaderHostColumnId in RegisterColumn.

        if (string.Equals(_coordinator.HeaderHostColumnId, ColumnId, StringComparison.Ordinal)
            && context.Column is ComposableColumn<TGridItem, TValue> col)
        {
            col.AddFeature(new GroupHeaderHostFeature<TGridItem>());
        }
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        _context = null;
    }

    private static ComposableGrid<TGridItem> GetGridOrThrow(FeatureContext<TGridItem> context)
    {
        if (context.Column is not ComposableColumn<TGridItem, TValue> column || column.Grid is null)
            throw new InvalidOperationException("GroupingFeature must be used inside a ComposableGrid.");

        return column.Grid;
    }

    private sealed class KeyComparerAdapter(IEqualityComparer<TValue> inner) : IEqualityComparer<object?>
    {
        public bool Equals(object? x, object? y)
        {
            if (x is TValue a && y is TValue b)
                return inner.Equals(a, b);
            return object.Equals(x, y);
        }

        public int GetHashCode(object? obj)
        {
            if (obj is TValue v)
                return inner.GetHashCode(v);
            return obj?.GetHashCode() ?? 0;
        }
    }

    public bool IsGroupExpanded(object key)
    {
        if (_state is null)
            return false;

        return key is TValue typed && _state.IsExpanded(typed);
    }

    public async Task ToggleGroupAsync(object key)
    {
        if (_state is null)
            return;

        if (key is not TValue typed)
            return;

        await _state.ToggleAsync(typed);

        // Refresh via grouped data source event (grid-level subscription).
        await RequestDataRefreshAsync();
    }

    public async Task ExpandAllGroupsAsync()
    {
        if (_state is null)
            return;

        if (_coordinator?.ActiveGrouping is null)
            return;

        var keys = _coordinator.GetKnownGroupKeys()
            .OfType<TValue>()
            .ToArray();

        await _state.ExpandAllAsync(keys);
        await RequestDataRefreshAsync();
    }

    public async Task CollapseAllGroupsAsync()
    {
        if (_state is null)
            return;

        await _state.CollapseAllAsync();
        await RequestDataRefreshAsync();
    }

    private async Task RequestDataRefreshAsync()
    {
        if (_context?.InvokeAsync is null || _context.RequestRefreshAsync is null)
            return;

        await _context.InvokeAsync(_context.RequestRefreshAsync);
    }

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
        builder.CloseElement();

        builder.OpenComponent<Components.DefaultGroupHeader>(sequence++);
        builder.AddAttribute(sequence++, nameof(Components.DefaultGroupHeader.Key), key);
        builder.AddAttribute(sequence++, nameof(Components.DefaultGroupHeader.ItemCount), itemCount);
        builder.AddAttribute(sequence++, nameof(Components.DefaultGroupHeader.IsExpanded), isExpanded);
        builder.AddAttribute(sequence++, nameof(Components.DefaultGroupHeader.NullGroupLabel), NullGroupLabel);
        builder.CloseComponent();
    }

    public void Dispose()
    {
        _context = null;
        _groupByUntyped = null;
        _coordinator = null;
    }
}
