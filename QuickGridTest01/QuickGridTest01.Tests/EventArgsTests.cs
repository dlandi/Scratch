using QuickGridTest01.MultiState.Core;
using QuickGridTest01.MultiState.Component;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Component;

public class EventArgsTests
{
    private class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void BeforeEditEventArgs_Constructor_SetsProperties()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var value = "test value";

        // Act
        var args = new BeforeEditEventArgs<TestItem, string>(item, value);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(value, args.Value);
        Assert.False(args.Cancel);
    }

    [Fact]
    public void BeforeEditEventArgs_Constructor_ThrowsOnNullItem()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new BeforeEditEventArgs<TestItem, string>(null!, "value"));
    }

    [Fact]
    public void BeforeEditEventArgs_Cancel_CanBeSet()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var args = new BeforeEditEventArgs<TestItem, string>(item, "value");

        // Act
        args.Cancel = true;

        // Assert
        Assert.True(args.Cancel);
    }

    [Fact]
    public void ValueChangingEventArgs_Constructor_SetsProperties()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var oldValue = "old";
        var newValue = "new";

        // Act
        var args = new ValueChangingEventArgs<TestItem, string>(item, oldValue, newValue);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(oldValue, args.OldValue);
        Assert.Equal(newValue, args.NewValue);
    }

    [Fact]
    public void ValueChangingEventArgs_Constructor_ThrowsOnNullItem()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new ValueChangingEventArgs<TestItem, string>(null!, "old", "new"));
    }

    [Fact]
    public void SaveResultEventArgs_Constructor_SetsProperties_ForSuccess()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var value = "saved value";

        // Act
        var args = new SaveResultEventArgs<TestItem, string>(item, value, success: true);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(value, args.Value);
        Assert.True(args.Success);
        Assert.Null(args.ErrorMessage);
    }

    [Fact]
    public void SaveResultEventArgs_Constructor_SetsProperties_ForFailure()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var value = "failed value";
        var errorMessage = "Save failed";

        // Act
        var args = new SaveResultEventArgs<TestItem, string>(
            item, value, success: false, errorMessage: errorMessage);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(value, args.Value);
        Assert.False(args.Success);
        Assert.Equal(errorMessage, args.ErrorMessage);
    }

    [Fact]
    public void SaveResultEventArgs_Constructor_ThrowsOnNullItem()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new SaveResultEventArgs<TestItem, string>(null!, "value", true));
    }

    [Fact]
    public void StateTransitionEventArgs_Constructor_SetsProperties()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var oldState = CellState.Reading;
        var newState = CellState.Editing;

        // Act
        var args = new StateTransitionEventArgs<TestItem>(item, oldState, newState);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(oldState, args.OldState);
        Assert.Equal(newState, args.NewState);
    }

    [Fact]
    public void StateTransitionEventArgs_Constructor_ThrowsOnNullItem()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new StateTransitionEventArgs<TestItem>(null!, CellState.Reading, CellState.Editing));
    }

    [Fact]
    public void CancelEditEventArgs_Constructor_SetsProperties()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };
        var originalValue = "original";
        var draftValue = "draft";

        // Act
        var args = new CancelEditEventArgs<TestItem, string>(item, originalValue, draftValue);

        // Assert
        Assert.Same(item, args.Item);
        Assert.Equal(originalValue, args.OriginalValue);
        Assert.Equal(draftValue, args.DraftValue);
    }

    [Fact]
    public void CancelEditEventArgs_Constructor_ThrowsOnNullItem()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new CancelEditEventArgs<TestItem, string>(null!, "original", "draft"));
    }

    [Fact]
    public void AllEventArgs_WorkWithDifferentValueTypes()
    {
        // Arrange
        var item = new TestItem { Id = 1, Name = "Test" };

        // Act & Assert - int values
        var intBeforeEdit = new BeforeEditEventArgs<TestItem, int>(item, 42);
        Assert.Equal(42, intBeforeEdit.Value);

        var intValueChanging = new ValueChangingEventArgs<TestItem, int>(item, 10, 20);
        Assert.Equal(10, intValueChanging.OldValue);
        Assert.Equal(20, intValueChanging.NewValue);

        var intSaveResult = new SaveResultEventArgs<TestItem, int>(item, 100, true);
        Assert.Equal(100, intSaveResult.Value);

        var intCancelEdit = new CancelEditEventArgs<TestItem, int>(item, 5, 10);
        Assert.Equal(5, intCancelEdit.OriginalValue);
        Assert.Equal(10, intCancelEdit.DraftValue);

        // Act & Assert - DateTime values
        var date = DateTime.Now;
        var dateBeforeEdit = new BeforeEditEventArgs<TestItem, DateTime>(item, date);
        Assert.Equal(date, dateBeforeEdit.Value);
    }
}