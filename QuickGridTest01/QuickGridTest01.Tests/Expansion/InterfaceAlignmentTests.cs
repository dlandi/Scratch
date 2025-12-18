using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion;
using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class InterfaceAlignmentTests
{
    private sealed class Item : IRowIdentifiable
    {
        public int Id { get; set; }
    }

    [Fact]
    public void ICellRenderFeature_MethodSignature_Compiles()
    {
        ICellRenderFeature<Item> feature = new RowExpandFeature<Item>();

        var builder = new RenderTreeBuilder();
        var seq = 0;
        var item = new Item { Id = 1 };

        var ctx = new FeatureContext<Item>
        {
            Column = new DummyColumn(),
            InvokeAsync = action => action(),
            RequestRefreshAsync = () => Task.CompletedTask
        };

        feature.OnAttach(ctx);

        Assert.Throws<InvalidOperationException>(() => feature.RenderCell(builder, ref seq, item, ctx, () => { }));
    }

    [Fact]
    public void OnAttach_GuardsMissingDelegates()
    {
        var feature = new RowExpandFeature<Item>();

        var missingInvoke = new FeatureContext<Item> { Column = new DummyColumn(), RequestRefreshAsync = () => Task.CompletedTask };
        Assert.Throws<InvalidOperationException>(() => feature.OnAttach(missingInvoke));

        var missingRefresh = new FeatureContext<Item> { Column = new DummyColumn(), InvokeAsync = action => action() };
        Assert.Throws<InvalidOperationException>(() => feature.OnAttach(missingRefresh));
    }

    private sealed class DummyColumn : Microsoft.AspNetCore.Components.QuickGrid.ColumnBase<Item>
    {
        public override Microsoft.AspNetCore.Components.QuickGrid.GridSort<Item>? SortBy { get; set; }

        protected override void CellContent(RenderTreeBuilder builder, Item item)
            => builder.AddContent(0, "");
    }
}
