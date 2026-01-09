using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Reordering;

namespace QuickGridTest01.Tests.Reordering;

public class ReorderCoordinatorTests
{
    private sealed class TaskItem : IRowIdentifiable
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    #region Initial State Tests

    [Fact]
    public void Constructor_InitialState_NoDragInProgress()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();

        Assert.False(coordinator.IsDragging);
        Assert.Null(coordinator.DraggedItem);
        Assert.Null(coordinator.HoveredTarget);
        Assert.Null(coordinator.CurrentDropPosition);
    }

    [Fact]
    public void IsReorderingEnabled_Default_False()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();

        Assert.False(coordinator.IsReorderingEnabled);
    }

    #endregion

    #region StartDrag Tests

    [Fact]
    public void StartDrag_SetsDraggedItem()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        var item = new TaskItem { Id = 1, Title = "Test" };

        coordinator.StartDrag(item);

        Assert.True(coordinator.IsDragging);
        Assert.Same(item, coordinator.DraggedItem);
    }

    [Fact]
    public void StartDrag_NullItem_ThrowsArgumentNullException()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();

        Assert.Throws<ArgumentNullException>(() => coordinator.StartDrag(null!));
    }

    [Fact]
    public void StartDrag_FiresOnStateChanged()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        var eventFired = false;
        coordinator.OnStateChanged += () => eventFired = true;
        var item = new TaskItem { Id = 1 };

        coordinator.StartDrag(item);

        Assert.True(eventFired);
    }

    [Fact]
    public void StartDrag_ClearsHoverState()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        var item1 = new TaskItem { Id = 1 };
        var item2 = new TaskItem { Id = 2 };
        coordinator.StartDrag(item1);
        coordinator.UpdateHover(item2, DropPosition.Before);

        coordinator.StartDrag(item1); // Start new drag

        Assert.Null(coordinator.HoveredTarget);
        Assert.Null(coordinator.CurrentDropPosition);
    }

    #endregion

    #region UpdateHover Tests

    [Fact]
    public void UpdateHover_SetsHoverState()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        var dragged = new TaskItem { Id = 1 };
        var target = new TaskItem { Id = 2 };
        coordinator.StartDrag(dragged);

        coordinator.UpdateHover(target, DropPosition.After);

        Assert.Same(target, coordinator.HoveredTarget);
        Assert.Equal(DropPosition.After, coordinator.CurrentDropPosition);
    }

    [Fact]
    public void UpdateHover_FiresOnStateChanged()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });
        var eventFired = false;
        coordinator.OnStateChanged += () => eventFired = true;

        coordinator.UpdateHover(new TaskItem { Id = 2 }, DropPosition.Before);

        Assert.True(eventFired);
    }

    #endregion

    #region ClearHover Tests

    [Fact]
    public void ClearHover_ClearsHoverState()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });
        coordinator.UpdateHover(new TaskItem { Id = 2 }, DropPosition.Before);

        coordinator.ClearHover();

        Assert.Null(coordinator.HoveredTarget);
        Assert.Null(coordinator.CurrentDropPosition);
        Assert.True(coordinator.IsDragging); // Still dragging
    }

    [Fact]
    public void ClearHover_FiresOnStateChanged()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });
        coordinator.UpdateHover(new TaskItem { Id = 2 }, DropPosition.Before);
        var eventFired = false;
        coordinator.OnStateChanged += () => eventFired = true;

        coordinator.ClearHover();

        Assert.True(eventFired);
    }

    #endregion

    #region CancelDrag Tests

    [Fact]
    public void CancelDrag_ClearsAllState()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });
        coordinator.UpdateHover(new TaskItem { Id = 2 }, DropPosition.After);

        coordinator.CancelDrag();

        Assert.False(coordinator.IsDragging);
        Assert.Null(coordinator.DraggedItem);
        Assert.Null(coordinator.HoveredTarget);
        Assert.Null(coordinator.CurrentDropPosition);
    }

    [Fact]
    public void CancelDrag_FiresOnStateChanged()
    {
        using var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });
        var eventFired = false;
        coordinator.OnStateChanged += () => eventFired = true;

        coordinator.CancelDrag();

        Assert.True(eventFired);
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_ClearsState()
    {
        var coordinator = new ReorderCoordinator<TaskItem>();
        coordinator.StartDrag(new TaskItem { Id = 1 });

        coordinator.Dispose();

        Assert.False(coordinator.IsDragging);
        Assert.Null(coordinator.DraggedItem);
    }

    [Fact]
    public void Dispose_MultipleCalls_Safe()
    {
        var coordinator = new ReorderCoordinator<TaskItem>();

        coordinator.Dispose();
        coordinator.Dispose(); // Should not throw
    }

    #endregion
}
