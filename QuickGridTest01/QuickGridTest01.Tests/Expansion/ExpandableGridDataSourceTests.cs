using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Data;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class ExpandableGridDataSourceTests
{
    private sealed class Item : IRowIdentifiable
    {
        public int Id { get; set; }
    }

    [Fact]
    public void ExpandRow_InsertsSpanPlusOneSpacersAfterRow()
    {
        var items = new List<Item>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 },
        };

        var ds = new ExpandableGridDataSource<Item>(items);

        ds.ExpandRow(rowId: 2, spacerCount: 3);

        var result = ds.Items.ToList();

        var idx = result.FindIndex(i => i.Id == 2);
        Assert.True(idx >= 0);

        // Expect 3+1 spacers immediately after Id=2
        Assert.Equal(4, result.Skip(idx + 1).Take(4).Count(i => i.Id < 0));
        Assert.Equal(3, result[idx + 1 + 4].Id); // next real row
    }

    [Fact]
    public void CollapseRow_RemovesPreviouslyInsertedSpacers()
    {
        var items = new List<Item>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 },
        };

        var ds = new ExpandableGridDataSource<Item>(items);
        ds.ExpandRow(rowId: 2, spacerCount: 2);
        Assert.Contains(ds.Items, i => i.Id < 0);

        ds.CollapseRow(rowId: 2);

        var result = ds.Items.ToList();
        Assert.DoesNotContain(result, i => i.Id < 0);
        Assert.Equal(new[] { 1, 2, 3 }, result.Select(i => i.Id));
    }

    [Fact]
    public void CollapseAll_ClearsAllExpansions()
    {
        var items = new List<Item>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 },
        };

        var ds = new ExpandableGridDataSource<Item>(items);
        ds.ExpandRow(rowId: 1, spacerCount: 1);
        ds.ExpandRow(rowId: 2, spacerCount: 1);

        ds.CollapseAll();

        var result = ds.Items.ToList();
        Assert.Equal(new[] { 1, 2, 3 }, result.Select(i => i.Id));
    }
}
