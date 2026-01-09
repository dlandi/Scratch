using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Grouping;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;
using Xunit;

namespace QuickGridTest01.Tests.Grouping;

public class GroupingFeatureSadPathTests
{
    private sealed class StubColumn<TGridItem> : ColumnBase<TGridItem>
    {
        public override GridSort<TGridItem>? SortBy { get; set; }

        protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
        {
        }
    }

    private static FeatureContext<TGridItem> CreateContext<TGridItem>() where TGridItem : class
        => new()
        {
            Column = new StubColumn<TGridItem>(),
            InvokeAsync = _ => Task.CompletedTask,
        };

    private static FeatureContext<TGridItem, TValue> CreateTypedContext<TGridItem, TValue>()
        where TGridItem : class
        => new()
        {
            Column = new StubColumn<TGridItem>(),
            InvokeAsync = _ => Task.CompletedTask,
        };

    [Fact]
    public void OnAttach_WhenInvokeAsyncMissing_ThrowsInvalidOperationException()
    {
        var feature = new GroupingFeature<object, int>();
        var context = new FeatureContext<object> { Column = new StubColumn<object>() };

        var ex = Assert.Throws<InvalidOperationException>(() => feature.OnAttach(context));
        Assert.Equal("Grouping requires FeatureContext.InvokeAsync to be set (dispatcher was null).", ex.Message);
    }

    [Fact]
    public void OnAttach_WhenFilterBehaviorIsGroupThenFilter_ThrowsNotSupportedException()
    {
        var feature = new GroupingFeature<object, int> { FilterBehavior = FilterGroupOrder.GroupThenFilter };
        var context = CreateContext<object>();

        var ex = Assert.Throws<NotSupportedException>(() => feature.OnAttach(context));
        Assert.Equal("Grouping does not support FilterBehavior.GroupThenFilter.", ex.Message);
    }

    [Fact]
    public void OnAttach_WhenGroupHeaderSlotSpanLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var feature = new GroupingFeature<object, int> { GroupHeaderSlotSpan = 0 };
        var context = CreateContext<object>();

        Assert.Throws<ArgumentOutOfRangeException>(() => feature.OnAttach(context));
    }

    [Fact]
    public void OnAttach_WhenIsActiveAndTGridItemIsNotIRowIdentifiable_ThrowsInvalidOperationException()
    {
        var feature = new GroupingFeature<object, int> { IsActive = true };
        var context = CreateContext<object>();

        var ex = Assert.Throws<InvalidOperationException>(() => feature.OnAttach(context));
        Assert.Equal("Active grouping requires TGridItem to implement IRowIdentifiable.", ex.Message);
    }

    [Fact]
    public void OnAttach_WhenGroupByMissingInTypedContext_ThrowsInvalidOperationException()
    {
        var feature = new GroupingFeature<object, int> { IsActive = false, GroupBy = null };
        var context = CreateTypedContext<object, int>();

        var ex = Assert.Throws<InvalidOperationException>(() => feature.OnAttach(context));
        Assert.Equal("Grouping requires a non-null GroupBy selector.", ex.Message);
    }
}
