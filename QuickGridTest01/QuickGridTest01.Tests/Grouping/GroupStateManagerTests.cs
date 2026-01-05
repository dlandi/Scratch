using QuickGridTest01.ComposableColumns.Features.Grouping;
using Xunit;

namespace QuickGridTest01.Tests.Grouping;

public class GroupStateManagerTests
{
    [Fact]
    public async Task InitializeAsync_WhenInitiallyExpandedFalse_DoesNotExpandAnyGroups()
    {
        var manager = new GroupStateManager<int>();

        await manager.InitializeAsync(new[] { 1, 2, 3 }, initiallyExpanded: false);

        Assert.False(manager.HasExpandedGroups);
        Assert.Equal(0, manager.ExpandedGroupCount);
    }

    [Fact]
    public async Task InitializeAsync_WhenInitiallyExpandedTrue_ExpandsAllGroups_IdempotentForDuplicateKeys()
    {
        var manager = new GroupStateManager<int>();

        await manager.InitializeAsync(new[] { 1, 2, 2, 3 }, initiallyExpanded: true);

        Assert.True(manager.IsExpanded(1));
        Assert.True(manager.IsExpanded(2));
        Assert.True(manager.IsExpanded(3));
        Assert.Equal(3, manager.ExpandedGroupCount);
    }

    [Fact]
    public async Task ToggleAsync_WhenKeyNotPresent_AddsKey()
    {
        var manager = new GroupStateManager<int>();

        await manager.ToggleAsync(10);

        Assert.True(manager.IsExpanded(10));
    }

    [Fact]
    public async Task ToggleAsync_WhenKeyPresent_RemovesKey()
    {
        var manager = new GroupStateManager<int>();

        await manager.ToggleAsync(10);
        await manager.ToggleAsync(10);

        Assert.False(manager.IsExpanded(10));
    }

    [Fact]
    public async Task ExpandAllAsync_WithDuplicateKeys_IsIdempotent()
    {
        var manager = new GroupStateManager<int>();

        await manager.ExpandAllAsync(new[] { 1, 2, 2, 3 });

        Assert.Equal(3, manager.ExpandedGroupCount);
    }

    [Fact]
    public async Task CollapseAllAsync_ClearsExpandedGroups()
    {
        var manager = new GroupStateManager<int>();

        await manager.ExpandAllAsync(new[] { 1, 2, 3 });
        await manager.CollapseAllAsync();

        Assert.False(manager.HasExpandedGroups);
        Assert.Equal(0, manager.ExpandedGroupCount);
    }

    [Fact]
    public async Task Concurrency_ToggleAsync_FinalStateIsDeterministicAndCountMatches()
    {
        var manager = new GroupStateManager<int>();

        var toggles = Enumerable.Range(0, 100)
            .Select(_ => manager.ToggleAsync(1));

        await Task.WhenAll(toggles);

        // Even number of toggles should result in not expanded.
        Assert.False(manager.IsExpanded(1));
        Assert.Equal(0, manager.ExpandedGroupCount);
    }
}
