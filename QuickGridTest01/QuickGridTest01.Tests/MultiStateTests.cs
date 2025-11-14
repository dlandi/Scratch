using QuickGridTest01.MultiState.Core;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Core;

public class MultiStateTests
{
    [Fact]
    public void Constructor_InitializesWithReadingState()
    {
        // Arrange & Act
        var state = new MultiState<string>();

        // Assert
        Assert.Equal(CellState.Reading, state.CurrentState);
        Assert.Null(state.PreviousState);
        Assert.Equal(0, state.RetryCount);
    }

    [Fact]
    public void OriginalValue_CanBeSet()
    {
        // Arrange
        var state = new MultiState<string>();

        // Act
        state.OriginalValue = "test value";

        // Assert
        Assert.Equal("test value", state.OriginalValue);
    }

    [Fact]
    public void IsModified_ReturnsFalse_WhenValuesAreEqual()
    {
        // Arrange
        var state = new MultiState<string>
        {
            OriginalValue = "test",
            DraftValue = "test"
        };

        // Act & Assert
        Assert.False(state.IsModified);
    }

    [Fact]
    public void IsModified_ReturnsTrue_WhenValuesAreDifferent()
    {
        // Arrange
        var state = new MultiState<string>
        {
            OriginalValue = "original",
            DraftValue = "modified"
        };

        // Act & Assert
        Assert.True(state.IsModified);
    }

    [Fact]
    public void IsModified_ReturnsFalse_WhenBothValuesAreNull()
    {
        // Arrange
        var state = new MultiState<string>
        {
            OriginalValue = null,
            DraftValue = null
        };

        // Act & Assert
        Assert.False(state.IsModified);
    }

    [Fact]
    public void IsModified_ReturnsTrue_WhenOriginalIsNullAndDraftIsNot()
    {
        // Arrange
        var state = new MultiState<string>
        {
            OriginalValue = null,
            DraftValue = "value"
        };

        // Act & Assert
        Assert.True(state.IsModified);
    }

    [Fact]
    public void TransitionTo_UpdatesCurrentState()
    {
        // Arrange
        var state = new MultiState<string>();

        // Act
        state.TransitionTo(CellState.Editing);

        // Assert
        Assert.Equal(CellState.Editing, state.CurrentState);
    }

    [Fact]
    public void TransitionTo_RecordsPreviousState()
    {
        // Arrange
        var state = new MultiState<string>();

        // Act
        state.TransitionTo(CellState.Editing);

        // Assert
        Assert.Equal(CellState.Reading, state.PreviousState);
    }

    [Fact]
    public void TransitionTo_ChainsPreviousStateCorrectly()
    {
        // Arrange
        var state = new MultiState<string>();

        // Act
        state.TransitionTo(CellState.Editing);
        state.TransitionTo(CellState.Loading);
        state.TransitionTo(CellState.Error);

        // Assert
        Assert.Equal(CellState.Error, state.CurrentState);
        Assert.Equal(CellState.Loading, state.PreviousState);
    }

    [Fact]
    public void Reset_RestoresReadingState()
    {
        // Arrange
        var state = new MultiState<string>
        {
            OriginalValue = "original",
            DraftValue = "modified",
            ErrorMessage = "error",
            LoadingStartTime = DateTime.UtcNow,
            RetryCount = 2
        };
        state.TransitionTo(CellState.Error);

        // Act
        state.Reset();

        // Assert
        Assert.Equal(CellState.Reading, state.CurrentState);
        Assert.Equal("original", state.DraftValue);
        Assert.Null(state.ErrorMessage);
        Assert.Null(state.LoadingStartTime);
        Assert.Null(state.PreviousState);
        // Note: RetryCount is NOT reset by design
    }

    [Theory]
    [InlineData(10)]
    [InlineData(42)]
    [InlineData(-5)]
    public void IsModified_WorksWithIntegers(int value)
    {
        // Arrange
        var state = new MultiState<int>
        {
            OriginalValue = value,
            DraftValue = value
        };

        // Act & Assert
        Assert.False(state.IsModified);

        state.DraftValue = value + 1;
        Assert.True(state.IsModified);
    }

    [Fact]
    public void LoadingStartTime_CanBeSet()
    {
        // Arrange
        var state = new MultiState<string>();
        var now = DateTime.UtcNow;

        // Act
        state.LoadingStartTime = now;

        // Assert
        Assert.Equal(now, state.LoadingStartTime);
    }

    [Fact]
    public void RetryCount_CanBeIncremented()
    {
        // Arrange
        var state = new MultiState<string>();

        // Act
        state.RetryCount++;
        state.RetryCount++;

        // Assert
        Assert.Equal(2, state.RetryCount);
    }
}

public class StateTransitionRulesTests
{
    [Theory]
    [InlineData(CellState.Reading, CellState.Editing, true)]
    [InlineData(CellState.Reading, CellState.Loading, false)]
    [InlineData(CellState.Reading, CellState.Error, false)]
    public void IsValidTransition_FromReading(CellState from, CellState to, bool expected)
    {
        // Act
        var result = StateTransitionRules.IsValidTransition(from, to);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(CellState.Editing, CellState.Loading, true)]
    [InlineData(CellState.Editing, CellState.Reading, true)]
    [InlineData(CellState.Editing, CellState.Error, false)]
    public void IsValidTransition_FromEditing(CellState from, CellState to, bool expected)
    {
        // Act
        var result = StateTransitionRules.IsValidTransition(from, to);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(CellState.Loading, CellState.Reading, true)]
    [InlineData(CellState.Loading, CellState.Error, true)]
    [InlineData(CellState.Loading, CellState.Editing, false)]
    public void IsValidTransition_FromLoading(CellState from, CellState to, bool expected)
    {
        // Act
        var result = StateTransitionRules.IsValidTransition(from, to);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(CellState.Error, CellState.Loading, true)]
    [InlineData(CellState.Error, CellState.Reading, true)]
    [InlineData(CellState.Error, CellState.Editing, true)]
    public void IsValidTransition_FromError(CellState from, CellState to, bool expected)
    {
        // Act
        var result = StateTransitionRules.IsValidTransition(from, to);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(CellState.Reading)]
    [InlineData(CellState.Editing)]
    [InlineData(CellState.Loading)]
    [InlineData(CellState.Error)]
    public void IsValidTransition_SameState_IsValid(CellState state)
    {
        // Act
        var result = StateTransitionRules.IsValidTransition(state, state);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetValidNextStates_FromReading_ReturnsCorrectStates()
    {
        // Act
        var validStates = StateTransitionRules.GetValidNextStates(CellState.Reading);

        // Assert
        Assert.Contains(CellState.Reading, validStates);
        Assert.Contains(CellState.Editing, validStates);
        Assert.DoesNotContain(CellState.Loading, validStates);
        Assert.DoesNotContain(CellState.Error, validStates);
    }

    [Fact]
    public void GetValidNextStates_FromEditing_ReturnsCorrectStates()
    {
        // Act
        var validStates = StateTransitionRules.GetValidNextStates(CellState.Editing);

        // Assert
        Assert.Contains(CellState.Editing, validStates);
        Assert.Contains(CellState.Loading, validStates);
        Assert.Contains(CellState.Reading, validStates);
        Assert.DoesNotContain(CellState.Error, validStates);
    }

    [Fact]
    public void GetValidNextStates_FromLoading_ReturnsCorrectStates()
    {
        // Act
        var validStates = StateTransitionRules.GetValidNextStates(CellState.Loading);

        // Assert
        Assert.Contains(CellState.Loading, validStates);
        Assert.Contains(CellState.Reading, validStates);
        Assert.Contains(CellState.Error, validStates);
        Assert.DoesNotContain(CellState.Editing, validStates);
    }

    [Fact]
    public void GetValidNextStates_FromError_ReturnsCorrectStates()
    {
        // Act
        var validStates = StateTransitionRules.GetValidNextStates(CellState.Error);

        // Assert
        Assert.Contains(CellState.Error, validStates);
        Assert.Contains(CellState.Loading, validStates);
        Assert.Contains(CellState.Reading, validStates);
        Assert.Contains(CellState.Editing, validStates);
    }

    [Fact]
    public void CompleteWorkflow_ValidatesCorrectly()
    {
        // Arrange - Simulate a complete edit workflow
        var transitions = new[]
        {
            (CellState.Reading, CellState.Editing),      // User clicks edit
            (CellState.Editing, CellState.Loading),      // User saves
            (CellState.Loading, CellState.Error),        // Save fails
            (CellState.Error, CellState.Loading),        // User retries
            (CellState.Loading, CellState.Reading)       // Save succeeds
        };

        // Act & Assert
        foreach (var (from, to) in transitions)
        {
            Assert.True(StateTransitionRules.IsValidTransition(from, to), 
                $"Expected transition {from} -> {to} to be valid");
        }
    }
}
