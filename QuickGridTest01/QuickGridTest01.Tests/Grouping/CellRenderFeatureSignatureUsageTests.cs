using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.QuickGrid;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Grouping.Components;
using Xunit;

namespace QuickGridTest01.Tests.Grouping;

public class CellRenderFeatureSignatureUsageTests
{
    private sealed class StubColumn<TGridItem> : ColumnBase<TGridItem>
    {
        public override GridSort<TGridItem>? SortBy { get; set; }

        protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
        {
        }
    }

    [Fact]
    public void GroupHeaderHostFeature_ImplementsICellRenderFeatureWithRenderCellSignature()
    {
        static void Consume<T>(ICellRenderFeature<T> feature, T item, FeatureContext<T> context)
            where T : class
        {
            var builder = new RenderTreeBuilder();
            var seq = 0;
            feature.RenderCell(builder, ref seq, item, context, renderNext: static () => { });
        }

        var feature = new GroupHeaderHostFeature<object>();
        var ctx = new FeatureContext<object> { Column = new StubColumn<object>() };

        Consume(feature, new object(), ctx);
    }
}
