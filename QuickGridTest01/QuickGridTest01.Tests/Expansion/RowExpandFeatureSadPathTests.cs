using Microsoft.AspNetCore.Components;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion;
using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Data;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class RowExpandFeatureSadPathTests
{
    private sealed class Item : IRowIdentifiable
    {
        public int Id { get; set; }
    }

    private static FeatureContext<Item> CreateContext(Func<Func<Task>, Task> invokeAsync, Func<Task> requestRefreshAsync)
        => new FeatureContext<Item>
        {
            Column = new DummyColumn(),
            InvokeAsync = invokeAsync,
            RequestRefreshAsync = requestRefreshAsync
        };

    private sealed class DummyColumn : Microsoft.AspNetCore.Components.QuickGrid.ColumnBase<Item>
    {
        public override Microsoft.AspNetCore.Components.QuickGrid.GridSort<Item>? SortBy { get; set; }

        protected override void CellContent(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder, Item item)
            => builder.AddContent(0, "");
    }

    [Fact]
    public async Task ExpandRowAsync_Null_ThrowsArgumentNullException()
    {
        var feature = new RowExpandFeature<Item>
        {
            ExpandedTemplate = _ => b => b.AddContent(0, "x")
        };

        feature.OnAttach(CreateContext(action => action(), () => Task.CompletedTask));

        await Assert.ThrowsAsync<ArgumentNullException>(() => feature.ExpandRowAsync(null!));
    }

    [Fact]
    public async Task ExpandRowAsync_IdZero_ThrowsArgumentOutOfRangeException()
    {
        var feature = new RowExpandFeature<Item>
        {
            ExpandedTemplate = _ => b => b.AddContent(0, "x")
        };

        feature.OnAttach(CreateContext(action => action(), () => Task.CompletedTask));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => feature.ExpandRowAsync(new Item { Id = 0 }));
    }

    [Fact]
    public async Task ExpandRowAsync_CanceledToken_ThrowsBeforeStateMutation()
    {
        var baseItems = new[] { new Item { Id = 1 }, new Item { Id = 2 } };
        var dataSource = new ExpandableGridDataSource<Item>(baseItems);

        var feature = new RowExpandFeature<Item>
        {
            DataSource = dataSource,
            ExpandedTemplate = _ => b => b.AddContent(0, "x")
        };

        feature.OnAttach(CreateContext(action => action(), () => Task.CompletedTask));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => feature.ExpandRowAsync(new Item { Id = 1 }, cts.Token));

        // No spacer rows should have been inserted
        Assert.DoesNotContain(dataSource.Items, i => i.Id < 0);
    }
}
