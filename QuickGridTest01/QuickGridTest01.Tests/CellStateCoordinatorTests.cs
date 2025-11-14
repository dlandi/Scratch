using QuickGridTest01.MultiState.Core;
using QuickGridTest01.MultiState.Component;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Component;

public class CellStateCoordinatorTests
{
    private class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task GetOrCreateStateAsync_CreatesNewState_WhenNotExists()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };

        // Act
        var state = await coordinator.GetOrCreateStateAsync(item, "initial");

        // Assert
        Assert.NotNull(state);
        Assert.Equal(CellState.Reading, state.CurrentState);
        Assert.Equal("initial", state.OriginalValue);
        Assert.Equal("initial", state.DraftValue);
    }

    [Fact]
    public async Task GetOrCreateStateAsync_ReturnsSameState_WhenCalledTwice()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };

        // Act
        var state1 = await coordinator.GetOrCreateStateAsync(item, "initial");
        var state2 = await coordinator.GetOrCreateStateAsync(item, "different");

        // Assert
        Assert.Same(state1, state2);
        Assert.Equal("initial", state2.OriginalValue); // Should keep original, not "different"
    }

    [Fact]
    public async Task GetOrCreateStateAsync_CreatesDifferentStates_ForDifferentItems()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item1 = new TestItem { Id = 1, Name = "Test1" };
        var item2 = new TestItem { Id = 2, Name = "Test2" };

        // Act
        var state1 = await coordinator.GetOrCreateStateAsync(item1, "value1");
        var state2 = await coordinator.GetOrCreateStateAsync(item2, "value2");

        // Assert
        Assert.NotSame(state1, state2);
        Assert.Equal("value1", state1.OriginalValue);
        Assert.Equal("value2", state2.OriginalValue);
    }

    [Fact]
    public async Task GetOrCreateStateAsync_ThrowsOnNullItem()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => coordinator.GetOrCreateStateAsync(null!, "value"));
    }

    [Fact]
    public void TryGetState_ReturnsFalse_WhenStateDoesNotExist()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };

        // Act
        var found = coordinator.TryGetState(item, out var state);

        // Assert
        Assert.False(found);
        Assert.Null(state);
    }

    [Fact]
    public async Task TryGetState_ReturnsTrue_WhenStateExists()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };
        var createdState = await coordinator.GetOrCreateStateAsync(item, "initial");

        // Act
        var found = coordinator.TryGetState(item, out var state);

        // Assert
        Assert.True(found);
        Assert.Same(createdState, state);
    }

    [Fact]
    public void TryGetState_ThrowsOnNullItem()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => coordinator.TryGetState(null!, out _));
    }

    [Fact]
    public void RemoveState_ReturnsFalse_WhenStateDoesNotExist()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };

        // Act
        var removed = coordinator.RemoveState(item);

        // Assert
        Assert.False(removed);
    }

    [Fact]
    public async Task RemoveState_ReturnsTrue_WhenStateExists()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };
        await coordinator.GetOrCreateStateAsync(item, "initial");

        // Act
        var removed = coordinator.RemoveState(item);

        // Assert
        Assert.True(removed);
        Assert.False(coordinator.TryGetState(item, out _));
    }

    [Fact]
    public void RemoveState_ThrowsOnNullItem()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => coordinator.RemoveState(null!));
    }

    [Fact]
    public async Task StateModifications_ArePreserved()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };
        var state = await coordinator.GetOrCreateStateAsync(item, "initial");

        // Act
        state.CurrentState = CellState.Editing;
        state.DraftValue = "modified";

        var retrievedState = await coordinator.GetOrCreateStateAsync(item, "ignored");

        // Assert
        Assert.Same(state, retrievedState);
        Assert.Equal(CellState.Editing, retrievedState.CurrentState);
        Assert.Equal("modified", retrievedState.DraftValue);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();

        // Act & Assert (should not throw)
        coordinator.Dispose();
        coordinator.Dispose();
    }

    [Fact]
    public async Task ConcurrentAccess_DoesNotCorruptState()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, int>();
        var item = new TestItem { Id = 1, Name = "Test" };
        const int threadCount = 10;
        const int iterationsPerThread = 100;

        // Act
        var tasks = Enumerable.Range(0, threadCount).Select(async threadId =>
        {
            for (int i = 0; i < iterationsPerThread; i++)
            {
                var state = await coordinator.GetOrCreateStateAsync(item, threadId);
                state.DraftValue = threadId * 1000 + i;
                await Task.Yield(); // Force context switch
            }
        });

        await Task.WhenAll(tasks);

        // Assert
        var finalState = await coordinator.GetOrCreateStateAsync(item, -1);
        Assert.NotNull(finalState);
        // State should exist and be valid, though exact value is non-deterministic
        Assert.True(coordinator.TryGetState(item, out _));
    }

    [Fact]
    public async Task WorksWithDifferentValueTypes()
    {
        // Arrange & Act
        var stringCoordinator = new CellStateCoordinator<TestItem, string>();
        var intCoordinator = new CellStateCoordinator<TestItem, int>();
        var dateCoordinator = new CellStateCoordinator<TestItem, DateTime>();

        var item = new TestItem { Id = 1, Name = "Test" };

        var stringState = await stringCoordinator.GetOrCreateStateAsync(item, "test");
        var intState = await intCoordinator.GetOrCreateStateAsync(item, 42);
        var dateState = await dateCoordinator.GetOrCreateStateAsync(item, DateTime.Now);

        // Assert
        Assert.Equal("test", stringState.OriginalValue);
        Assert.Equal(42, intState.OriginalValue);
        Assert.IsType<DateTime>(dateState.OriginalValue);
    }

    [Fact]
    public async Task CancellationToken_IsRespected()
    {
        // Arrange
        var coordinator = new CellStateCoordinator<TestItem, string>();
        var item = new TestItem { Id = 1, Name = "Test" };
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => coordinator.GetOrCreateStateAsync(item, "value", cts.Token));
    }
}