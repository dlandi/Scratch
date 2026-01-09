using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Reordering;

namespace QuickGridTest01.Tests.Reordering;

public class ReorderableDataSourceTests
{
    private sealed class TaskItem : IRowIdentifiable
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_NullItems_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ReorderableDataSource<TaskItem>(null!));
    }

    [Fact]
    public void Constructor_EmptyItems_CreatesEmptyDataSource()
    {
        var ds = new ReorderableDataSource<TaskItem>([]);

        Assert.Empty(ds.Items);
        Assert.Empty(ds.CurrentOrder);
    }

    [Fact]
    public void Constructor_WithItems_InitializesInOrder()
    {
        var items = new[]
        {
            new TaskItem { Id = 1, Title = "A" },
            new TaskItem { Id = 2, Title = "B" },
            new TaskItem { Id = 3, Title = "C" }
        };

        var ds = new ReorderableDataSource<TaskItem>(items);

        Assert.Equal(new[] { 1, 2, 3 }, ds.Items.Select(i => i.Id));
        Assert.Equal(new[] { 1, 2, 3 }, ds.CurrentOrder.Select(i => i.Id));
    }

    #endregion

    #region MoveItem(fromIndex, toIndex) Tests

    [Fact]
    public void MoveItem_ByIndex_MovesForward()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);

        ds.MoveItem(fromIndex: 0, toIndex: 3);

        Assert.Equal(new[] { 2, 3, 4, 1, 5 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItem_ByIndex_MovesBackward()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);

        ds.MoveItem(fromIndex: 4, toIndex: 1);

        Assert.Equal(new[] { 1, 5, 2, 3, 4 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItem_ByIndex_SamePosition_NoChange()
    {
        var ds = CreateDataSource(1, 2, 3);

        ds.MoveItem(fromIndex: 1, toIndex: 1);

        Assert.Equal(new[] { 1, 2, 3 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItem_ByIndex_FromIndexNegative_ThrowsArgumentOutOfRange()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => ds.MoveItem(fromIndex: -1, toIndex: 1));
    }

    [Fact]
    public void MoveItem_ByIndex_FromIndexOutOfRange_ThrowsArgumentOutOfRange()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => ds.MoveItem(fromIndex: 5, toIndex: 1));
    }

    [Fact]
    public void MoveItem_ByIndex_ToIndexNegative_ThrowsArgumentOutOfRange()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => ds.MoveItem(fromIndex: 0, toIndex: -1));
    }

    [Fact]
    public void MoveItem_ByIndex_ToIndexOutOfRange_ThrowsArgumentOutOfRange()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => ds.MoveItem(fromIndex: 0, toIndex: 5));
    }

    #endregion

    #region MoveItem(item, toIndex) Tests

    [Fact]
    public void MoveItem_ByItem_MovesToPosition()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);
        var item = ds.CurrentOrder.First(i => i.Id == 1);

        ds.MoveItem(item, toIndex: 3);

        Assert.Equal(new[] { 2, 3, 4, 1, 5 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItem_ByItem_ItemNotFound_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var otherItem = new TaskItem { Id = 99, Title = "Other" };

        var ex = Assert.Throws<ArgumentException>(() => ds.MoveItem(otherItem, toIndex: 1));
        Assert.Contains("not found", ex.Message);
    }

    [Fact]
    public void MoveItem_ByItem_NullItem_ThrowsArgumentNullException()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentNullException>(() => ds.MoveItem(null!, toIndex: 1));
    }

    #endregion

    #region MoveItemBefore Tests

    [Fact]
    public void MoveItemBefore_MovesItemBeforeTarget()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);
        var item = ds.CurrentOrder.First(i => i.Id == 5);
        var target = ds.CurrentOrder.First(i => i.Id == 2);

        ds.MoveItemBefore(item, target);

        Assert.Equal(new[] { 1, 5, 2, 3, 4 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItemBefore_ItemNotFound_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var otherItem = new TaskItem { Id = 99 };
        var target = ds.CurrentOrder.First();

        var ex = Assert.Throws<ArgumentException>(() => ds.MoveItemBefore(otherItem, target));
        Assert.Contains("not found", ex.Message);
    }

    [Fact]
    public void MoveItemBefore_TargetNotFound_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var item = ds.CurrentOrder.First();
        var otherTarget = new TaskItem { Id = 99 };

        var ex = Assert.Throws<ArgumentException>(() => ds.MoveItemBefore(item, otherTarget));
        Assert.Contains("not found", ex.Message);
    }

    #endregion

    #region MoveItemAfter Tests

    [Fact]
    public void MoveItemAfter_MovesItemAfterTarget()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);
        var item = ds.CurrentOrder.First(i => i.Id == 1);
        var target = ds.CurrentOrder.First(i => i.Id == 3);

        ds.MoveItemAfter(item, target);

        Assert.Equal(new[] { 2, 3, 1, 4, 5 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void MoveItemAfter_ItemNotFound_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var otherItem = new TaskItem { Id = 99 };
        var target = ds.CurrentOrder.First();

        var ex = Assert.Throws<ArgumentException>(() => ds.MoveItemAfter(otherItem, target));
        Assert.Contains("not found", ex.Message);
    }

    #endregion

    #region IndexOf Tests

    [Fact]
    public void IndexOf_ReturnsCorrectIndex()
    {
        var ds = CreateDataSource(1, 2, 3);
        var item = ds.CurrentOrder.First(i => i.Id == 2);

        Assert.Equal(1, ds.IndexOf(item));
    }

    [Fact]
    public void IndexOf_ItemNotFound_ReturnsMinusOne()
    {
        var ds = CreateDataSource(1, 2, 3);
        var otherItem = new TaskItem { Id = 99 };

        Assert.Equal(-1, ds.IndexOf(otherItem));
    }

    #endregion

    #region GetOrderIndices / SetOrderIndices Tests

    [Fact]
    public void GetOrderIndices_ReturnsIdsInCurrentOrder()
    {
        var ds = CreateDataSource(1, 2, 3);
        ds.MoveItem(fromIndex: 0, toIndex: 2);

        var indices = ds.GetOrderIndices();

        Assert.Equal(new[] { 2, 3, 1 }, indices);
    }

    [Fact]
    public void SetOrderIndices_RestoresOrder()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);
        var savedOrder = new[] { 3, 1, 5, 2, 4 };

        ds.SetOrderIndices(savedOrder);

        Assert.Equal(savedOrder, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void SetOrderIndices_MismatchedCount_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var wrongOrder = new[] { 1, 2 };

        var ex = Assert.Throws<ArgumentException>(() => ds.SetOrderIndices(wrongOrder));
        Assert.Contains("do not match", ex.Message);
    }

    [Fact]
    public void SetOrderIndices_MissingId_ThrowsArgumentException()
    {
        var ds = CreateDataSource(1, 2, 3);
        var wrongOrder = new[] { 1, 2, 99 };

        var ex = Assert.Throws<ArgumentException>(() => ds.SetOrderIndices(wrongOrder));
        Assert.Contains("do not match", ex.Message);
    }

    #endregion

    #region ResetOrder Tests

    [Fact]
    public void ResetOrder_RestoresOriginalOrder()
    {
        var ds = CreateDataSource(1, 2, 3, 4, 5);
        ds.MoveItem(fromIndex: 0, toIndex: 4);
        ds.MoveItem(fromIndex: 2, toIndex: 0);

        ds.ResetOrder();

        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, ds.Items.Select(i => i.Id));
    }

    #endregion

    #region UpdateItems Tests

    [Fact]
    public void UpdateItems_PreservesOrder_WhenPreserveOrderTrue()
    {
        var ds = CreateDataSource(1, 2, 3);
        ds.MoveItem(fromIndex: 0, toIndex: 2); // Order: 2, 3, 1

        var newItems = new[]
        {
            new TaskItem { Id = 1, Title = "Updated A" },
            new TaskItem { Id = 2, Title = "Updated B" },
            new TaskItem { Id = 3, Title = "Updated C" }
        };

        ds.UpdateItems(newItems, preserveOrder: true);

        Assert.Equal(new[] { 2, 3, 1 }, ds.Items.Select(i => i.Id));
        Assert.Equal("Updated A", ds.Items.First(i => i.Id == 1).Title);
    }

    [Fact]
    public void UpdateItems_ResetsOrder_WhenPreserveOrderFalse()
    {
        var ds = CreateDataSource(1, 2, 3);
        ds.MoveItem(fromIndex: 0, toIndex: 2); // Order: 2, 3, 1

        var newItems = new[]
        {
            new TaskItem { Id = 1, Title = "Updated A" },
            new TaskItem { Id = 2, Title = "Updated B" },
            new TaskItem { Id = 3, Title = "Updated C" }
        };

        ds.UpdateItems(newItems, preserveOrder: false);

        Assert.Equal(new[] { 1, 2, 3 }, ds.Items.Select(i => i.Id));
    }

    [Fact]
    public void UpdateItems_NullItems_ThrowsArgumentNullException()
    {
        var ds = CreateDataSource(1, 2, 3);

        Assert.Throws<ArgumentNullException>(() => ds.UpdateItems(null!));
    }

    #endregion

    #region OnOrderChanged Event Tests

    [Fact]
    public void MoveItem_FiresOnOrderChanged()
    {
        var ds = CreateDataSource(1, 2, 3);
        var eventFired = false;
        ds.OnOrderChanged += () => eventFired = true;

        ds.MoveItem(fromIndex: 0, toIndex: 2);

        Assert.True(eventFired);
    }

    [Fact]
    public void MoveItemBefore_FiresOnOrderChanged()
    {
        var ds = CreateDataSource(1, 2, 3);
        var eventFired = false;
        ds.OnOrderChanged += () => eventFired = true;
        var item = ds.CurrentOrder.Last();
        var target = ds.CurrentOrder.First();

        ds.MoveItemBefore(item, target);

        Assert.True(eventFired);
    }

    [Fact]
    public void SetOrderIndices_FiresOnOrderChanged()
    {
        var ds = CreateDataSource(1, 2, 3);
        var eventFired = false;
        ds.OnOrderChanged += () => eventFired = true;

        ds.SetOrderIndices(new[] { 3, 2, 1 });

        Assert.True(eventFired);
    }

    [Fact]
    public void ResetOrder_FiresOnOrderChanged()
    {
        var ds = CreateDataSource(1, 2, 3);
        ds.MoveItem(fromIndex: 0, toIndex: 2);
        var eventFired = false;
        ds.OnOrderChanged += () => eventFired = true;

        ds.ResetOrder();

        Assert.True(eventFired);
    }

    #endregion

    #region Helper Methods

    private static ReorderableDataSource<TaskItem> CreateDataSource(params int[] ids)
    {
        var items = ids.Select(id => new TaskItem { Id = id, Title = $"Item {id}" }).ToList();
        return new ReorderableDataSource<TaskItem>(items);
    }

    #endregion
}
