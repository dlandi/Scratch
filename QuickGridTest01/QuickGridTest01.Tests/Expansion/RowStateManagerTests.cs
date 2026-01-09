using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.State;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class RowStateManagerTests
{
    private sealed class Item : IRowIdentifiable
    {
        public int Id { get; set; }
    }

    [Fact]
    public async Task GetOrCreateContextAsync_MarksRowExpanded()
    {
        var mgr = new RowStateManager<Item>();
        var item = new Item { Id = 1 };

        var ctx = await mgr.GetOrCreateContextAsync(item, collapseAsync: () => Task.CompletedTask);

        Assert.NotNull(ctx);
        Assert.True(mgr.IsRowExpanded(item));
        Assert.True(mgr.HasExpandedRows);
        Assert.Equal(1, mgr.ExpandedRowCount);
    }

    [Fact]
    public async Task RemoveRowAsync_RemovesExpandedRow()
    {
        var mgr = new RowStateManager<Item>();
        var item = new Item { Id = 1 };

        await mgr.GetOrCreateContextAsync(item, collapseAsync: () => Task.CompletedTask);
        var removed = await mgr.RemoveRowAsync(item);

        Assert.True(removed);
        Assert.False(mgr.IsRowExpanded(item));
    }

    [Fact]
    public async Task ClearAllAsync_RemovesAllExpandedRows()
    {
        var mgr = new RowStateManager<Item>();
        var a = new Item { Id = 1 };
        var b = new Item { Id = 2 };

        await mgr.GetOrCreateContextAsync(a, collapseAsync: () => Task.CompletedTask);
        await mgr.GetOrCreateContextAsync(b, collapseAsync: () => Task.CompletedTask);

        await mgr.ClearAllAsync();

        Assert.False(mgr.HasExpandedRows);
        Assert.Equal(0, mgr.ExpandedRowCount);
    }

    [Fact]
    public async Task GetFirstExpandedRow_ReturnsFirstOrDefault()
    {
        var mgr = new RowStateManager<Item>();
        var a = new Item { Id = 1 };

        Assert.Null(mgr.GetFirstExpandedRow());

        await mgr.GetOrCreateContextAsync(a, collapseAsync: () => Task.CompletedTask);

        Assert.Same(a, mgr.GetFirstExpandedRow());
    }
}
