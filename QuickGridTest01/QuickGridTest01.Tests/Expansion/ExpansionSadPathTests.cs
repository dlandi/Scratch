using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Data;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class ExpansionSadPathTests
{
    private sealed class Item : IRowIdentifiable
    {
        public int Id { get; set; }
    }

    [Fact]
    public void ExpandRow_RowIdZero_Throws()
    {
        var ds = new ExpandableGridDataSource<Item>(new[] { new Item { Id = 1 } });
        Assert.Throws<ArgumentOutOfRangeException>(() => ds.ExpandRow(rowId: 0, spacerCount: 1));
    }

    [Fact]
    public void ExpandRow_SpacerCountNegative_Throws()
    {
        var ds = new ExpandableGridDataSource<Item>(new[] { new Item { Id = 1 } });
        Assert.Throws<ArgumentOutOfRangeException>(() => ds.ExpandRow(rowId: 1, spacerCount: -1));
    }

    [Fact]
    public void ExpandRow_SpacerCountZero_NoSpacersInserted()
    {
        var ds = new ExpandableGridDataSource<Item>(new[] { new Item { Id = 1 }, new Item { Id = 2 } });
        ds.ExpandRow(rowId: 1, spacerCount: 0);
        Assert.Equal(new[] { 1, 2 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void CollapseRow_NotExpanded_NoOp()
    {
        var ds = new ExpandableGridDataSource<Item>(new[] { new Item { Id = 1 }, new Item { Id = 2 } });
        ds.CollapseRow(rowId: 1);
        Assert.Equal(new[] { 1, 2 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void ExpandRow_RepeatedExpand_ReplacesSpacerBlockDeterministically()
    {
        var ds = new ExpandableGridDataSource<Item>(new[] { new Item { Id = 1 }, new Item { Id = 2 }, new Item { Id = 3 } });

        ds.ExpandRow(rowId: 2, spacerCount: 1);
        var first = ds.Items.ToList();
        var firstSpacerCount = first.Count(i => i.Id < 0);

        ds.ExpandRow(rowId: 2, spacerCount: 3);
        var second = ds.Items.ToList();
        var secondSpacerCount = second.Count(i => i.Id < 0);

        Assert.NotEqual(firstSpacerCount, secondSpacerCount);
        Assert.Equal(4, secondSpacerCount); // 3+1
    }

    [Fact]
    public void SpacerIdOverflow_ThrowsOverflowException()
    {
        Assert.Throws<OverflowException>(() => SpacerRowFactory.EncodeSpacerId(int.MaxValue, offset: 999));
    }
}
