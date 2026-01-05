using QuickGridTest01.ComposableColumns.Features.Grouping;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;
using QuickGridTest01.RowColumn.Core;
using Xunit;

namespace QuickGridTest01.Tests.Grouping;

public class GroupedGridDataSourceTransformTests
{
    private class TestRow : IRowIdentifiable
    {
        public int Id { get; set; }
        public string? Category { get; set; }
    }

    private sealed class TestGroupingFeature : IGroupingFeature<TestRow>
    {
        private readonly HashSet<object> _expanded = new();

        public string ColumnId { get; init; } = "Category";
        public bool IsActive { get; init; } = true;
        public GroupSortDirection GroupOrder { get; init; } = GroupSortDirection.Ascending;
        public FilterGroupOrder FilterBehavior { get; init; } = FilterGroupOrder.FilterThenGroup;
        public NullKeyBehavior NullKeyBehavior { get; init; } = NullKeyBehavior.SeparateGroup;
        public int GroupHeaderSlotSpan { get; init; } = 2;
        public IEqualityComparer<object?>? KeyComparer => null;
        public Func<TestRow, object?> GroupByUntyped { get; init; } = r => r.Category;

        public bool IsGroupExpanded(object key) => _expanded.Contains(key);

        public Task ToggleGroupAsync(object key)
        {
            if (!_expanded.Add(key))
                _expanded.Remove(key);
            return Task.CompletedTask;
        }

        public Task ExpandAllGroupsAsync() => Task.CompletedTask;

        public Task CollapseAllGroupsAsync() => Task.CompletedTask;

        public void RenderGroupHeader(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder, ref int sequence, object? key, int itemCount, bool isExpanded)
        {
        }
    }

    [Fact]
    public void Transform_EmitsMarkerAndSpacerRows_PerGroupHeaderSlotSpan()
    {
        var coord = new GroupingCoordinator<TestRow>();
        coord.RegisterColumn("Category", new TestGroupingFeature { GroupHeaderSlotSpan = 3 });

        var ds = new GroupedGridDataSource<TestRow>(coord);
        ds.SetSourceItems(new[]
        {
            new TestRow { Id = 1, Category = "A" },
            new TestRow { Id = 2, Category = "A" },
            new TestRow { Id = 3, Category = "B" },
        }.AsQueryable());

        var items = ds.Items.ToList();

        // Two groups => 2 marker + (slotSpan-1)*2 spacer = 2 + 4 = 6 synthetic rows
        var synthetic = items.Where(i => i.Id < 0).ToList();
        Assert.Equal(6, synthetic.Count);
    }

    [Fact]
    public void Transform_UsesStableKeyToGroupIdMapping_AcrossRefreshes()
    {
        var feature = new TestGroupingFeature { GroupHeaderSlotSpan = 2 };
        var coord = new GroupingCoordinator<TestRow>();
        coord.RegisterColumn("Category", feature);

        var ds = new GroupedGridDataSource<TestRow>(coord);
        ds.SetSourceItems(new[]
        {
            new TestRow { Id = 1, Category = "A" },
            new TestRow { Id = 2, Category = "B" },
        }.AsQueryable());

        var first = ds.Items.Where(i => i.Id < 0).Select(i => i.Id).ToList();

        // Force rebuild without changing keys.
        ds.SetSourceItems(new[]
        {
            new TestRow { Id = 1, Category = "A" },
            new TestRow { Id = 2, Category = "B" },
            new TestRow { Id = 3, Category = "A" },
        }.AsQueryable());

        var second = ds.Items.Where(i => i.Id < 0).Select(i => i.Id).ToList();

        Assert.Contains(first[0], second);
        Assert.Contains(first[2], second);
    }

    [Theory]
    [InlineData(NullKeyBehavior.ShowAtTop)]
    [InlineData(NullKeyBehavior.ShowAtBottom)]
    [InlineData(NullKeyBehavior.Exclude)]
    public void Transform_NullKeyBehavior_ProducesDeterministicOutcome(NullKeyBehavior behavior)
    {
        var feature = new TestGroupingFeature { NullKeyBehavior = behavior };
        var coord = new GroupingCoordinator<TestRow>();
        coord.RegisterColumn("Category", feature);

        var ds = new GroupedGridDataSource<TestRow>(coord);
        ds.SetSourceItems(new[]
        {
            new TestRow { Id = 1, Category = null },
            new TestRow { Id = 2, Category = "A" },
        }.AsQueryable());

        var items = ds.Items.ToList();

        if (behavior == NullKeyBehavior.Exclude)
        {
            Assert.DoesNotContain(items, i => i.Id == 1);
            return;
        }

        if (behavior == NullKeyBehavior.ShowAtTop)
        {
            Assert.Equal(1, items[0].Id);
        }

        if (behavior == NullKeyBehavior.ShowAtBottom)
        {
            Assert.Equal(1, items[^1].Id);
        }
    }

    [Fact]
    public void Transform_EmptyInputSequence_ReturnsEmpty()
    {
        var coord = new GroupingCoordinator<TestRow>();
        coord.RegisterColumn("Category", new TestGroupingFeature());

        var ds = new GroupedGridDataSource<TestRow>(coord);
        ds.SetSourceItems(Array.Empty<TestRow>().AsQueryable());

        Assert.Empty(ds.Items);
    }

    [Fact]
    public void Transform_GroupHeaderSlotSpanLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var coord = new GroupingCoordinator<TestRow>();
        coord.RegisterColumn("Category", new TestGroupingFeature { GroupHeaderSlotSpan = 0 });

        var ds = new GroupedGridDataSource<TestRow>(coord);
        ds.SetSourceItems(new[] { new TestRow { Id = 1, Category = "A" } }.AsQueryable());

        Assert.Throws<ArgumentOutOfRangeException>(() => ds.Items.ToList());
    }
}
