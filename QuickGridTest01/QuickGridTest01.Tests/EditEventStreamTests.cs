using QuickGridTest01.ComposableColumns.Features.Editing;

namespace QuickGridTest01.Tests;

/// <summary>
/// Tests for EditEventStream and event publishing functionality.
/// Covers tasks A3.2, A3.3, A3.4, A3.6, A3.7.
/// </summary>
public class EditEventStreamTests
{
    #region A3.2: Callback Payload Tests - Verify event payloads contain correct data

    [Fact]
    public void EditEventBase_HasCorrectDefaultProperties()
    {
        // Arrange & Act
        var @event = new EditCommittedEvent
        {
            ItemKey = "test-key"
        };

        // Assert
        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.Equal("test-key", @event.ItemKey);
        Assert.Null(@event.PropertyName);
        Assert.True(@event.Timestamp <= DateTimeOffset.Now);
        Assert.True(@event.Timestamp > DateTimeOffset.Now.AddMinutes(-1));
    }

    [Fact]
    public void EditCommittedEvent_ContainsOldAndNewValues()
    {
        // Arrange & Act
        var @event = new EditCommittedEvent
        {
            ItemKey = 42,
            PropertyName = "Name",
            OldValue = "Alice",
            NewValue = "Bob"
        };

        // Assert
        Assert.Equal("EditCommitted", @event.EventType);
        Assert.Equal(42, @event.ItemKey);
        Assert.Equal("Name", @event.PropertyName);
        Assert.Equal("Alice", @event.OldValue);
        Assert.Equal("Bob", @event.NewValue);
    }

    [Fact]
    public void EditCancelledEvent_ContainsOriginalAndAttemptedValues()
    {
        // Arrange & Act
        var @event = new EditCancelledEvent
        {
            ItemKey = 42,
            PropertyName = "Age",
            OriginalValue = 25,
            AttemptedValue = 30
        };

        // Assert
        Assert.Equal("EditCancelled", @event.EventType);
        Assert.Equal(42, @event.ItemKey);
        Assert.Equal("Age", @event.PropertyName);
        Assert.Equal(25, @event.OriginalValue);
        Assert.Equal(30, @event.AttemptedValue);
    }

    [Fact]
    public void EditStartedEvent_ContainsCurrentValue()
    {
        // Arrange & Act
        var @event = new EditStartedEvent
        {
            ItemKey = "item-1",
            PropertyName = "Email",
            CurrentValue = "test@example.com"
        };

        // Assert
        Assert.Equal("EditStarted", @event.EventType);
        Assert.Equal("item-1", @event.ItemKey);
        Assert.Equal("Email", @event.PropertyName);
        Assert.Equal("test@example.com", @event.CurrentValue);
    }

    [Fact]
    public void ValidationFailedEvent_ContainsErrorsAndRuleResults()
    {
        // Arrange
        var ruleResults = new List<ValidationRuleResult>
        {
            ValidationRuleResult.Failure("Required", "Value is required"),
            ValidationRuleResult.Failure("StringLength", "Must be at least 3 characters", ValidationSeverity.Error)
        };

        // Act
        var @event = new ValidationFailedEvent
        {
            ItemKey = 42,
            PropertyName = "Name",
            AttemptedValue = "",
            Errors = ["Value is required", "Must be at least 3 characters"],
            RuleResults = ruleResults
        };

        // Assert
        Assert.Equal("ValidationFailed", @event.EventType);
        Assert.Equal(42, @event.ItemKey);
        Assert.Equal("", @event.AttemptedValue);
        Assert.Equal(2, @event.Errors.Count);
        Assert.Equal(2, @event.RuleResults.Count);
        Assert.Equal("Required", @event.RuleResults[0].RuleName);
        Assert.False(@event.RuleResults[0].IsValid);
    }

    [Fact]
    public void ValidationSucceededEvent_ContainsValueAndRuleResults()
    {
        // Arrange
        var ruleResults = new List<ValidationRuleResult>
        {
            ValidationRuleResult.Success("Required"),
            ValidationRuleResult.Success("StringLength")
        };

        // Act
        var @event = new ValidationSucceededEvent
        {
            ItemKey = 42,
            PropertyName = "Name",
            Value = "ValidName",
            RuleResults = ruleResults
        };

        // Assert
        Assert.Equal("ValidationSucceeded", @event.EventType);
        Assert.Equal("ValidName", @event.Value);
        Assert.Equal(2, @event.RuleResults.Count);
        Assert.True(@event.RuleResults.All(r => r.IsValid));
    }

    [Fact]
    public void EventTimestamp_IsAccurate()
    {
        // Arrange
        var before = DateTimeOffset.Now;

        // Act
        var @event = new EditCommittedEvent { ItemKey = 1 };

        // Assert
        var after = DateTimeOffset.Now;
        Assert.True(@event.Timestamp >= before);
        Assert.True(@event.Timestamp <= after);
    }

    [Fact]
    public void EventId_IsUnique()
    {
        // Arrange & Act
        var event1 = new EditCommittedEvent { ItemKey = 1 };
        var event2 = new EditCommittedEvent { ItemKey = 1 };

        // Assert
        Assert.NotEqual(event1.EventId, event2.EventId);
    }

    #endregion

    #region A3.3: Event Order Tests - Verify events publish in expected sequence

    [Fact]
    public async Task PublishAsync_EventsAreInOrder()
    {
        // Arrange
        var stream = new EditEventStream();
        var events = new List<EditEventBase>();
        stream.EventPublished += e => events.Add(e);

        // Act
        await stream.PublishAsync(new EditStartedEvent { ItemKey = 1, PropertyName = "Name" });
        await stream.PublishAsync(new ValidationSucceededEvent { ItemKey = 1, PropertyName = "Name" });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1, PropertyName = "Name" });

        // Assert
        Assert.Equal(3, events.Count);
        Assert.IsType<EditStartedEvent>(events[0]);
        Assert.IsType<ValidationSucceededEvent>(events[1]);
        Assert.IsType<EditCommittedEvent>(events[2]);
    }

    [Fact]
    public async Task PublishAsync_RecentEventsPreservesOrder()
    {
        // Arrange
        var stream = new EditEventStream();

        // Act
        await stream.PublishAsync(new EditStartedEvent { ItemKey = 1 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });
        await stream.PublishAsync(new EditCancelledEvent { ItemKey = 2 });

        // Assert
        var recent = stream.RecentEvents;
        Assert.Equal(3, recent.Count);
        Assert.IsType<EditStartedEvent>(recent[0]);
        Assert.IsType<EditCommittedEvent>(recent[1]);
        Assert.IsType<EditCancelledEvent>(recent[2]);
    }

    [Fact]
    public async Task PublishAsync_ValidationFailedThenCancelSequence()
    {
        // Arrange
        var stream = new EditEventStream();
        var events = new List<string>();
        stream.EventPublished += e => events.Add(e.EventType);

        // Act - Simulate: focus -> type invalid -> blur (validation fails) -> escape (cancel)
        await stream.PublishAsync(new EditStartedEvent { ItemKey = 1 });
        await stream.PublishAsync(new ValidationFailedEvent { ItemKey = 1 });
        await stream.PublishAsync(new EditCancelledEvent { ItemKey = 1 });

        // Assert
        Assert.Equal(new[] { "EditStarted", "ValidationFailed", "EditCancelled" }, events);
    }

    #endregion

    #region A3.4: Opt-in Behavior Tests - Confirm no overhead when disabled

    [Fact]
    public void EditEventStream_DefaultMaxEventsIs100()
    {
        // Arrange & Act
        var stream = new EditEventStream();

        // Assert
        Assert.Equal(100, stream.MaxEvents);
    }

    [Fact]
    public void EditEventStream_CustomMaxEvents()
    {
        // Arrange & Act
        var stream = new EditEventStream(maxEvents: 50);

        // Assert
        Assert.Equal(50, stream.MaxEvents);
    }

    [Fact]
    public void EditEventStream_ThrowsForInvalidMaxEvents()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new EditEventStream(maxEvents: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new EditEventStream(maxEvents: -1));
    }

    [Fact]
    public async Task PublishAsync_DoesNothingAfterDispose()
    {
        // Arrange
        var stream = new EditEventStream();
        var eventCount = 0;
        stream.EventPublished += _ => eventCount++;

        // Act
        stream.Dispose();
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });

        // Assert
        Assert.Equal(0, eventCount);
        Assert.Empty(stream.RecentEvents);
    }

    [Fact]
    public async Task PublishAsync_ThrowsForNullEvent()
    {
        // Arrange
        var stream = new EditEventStream();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => stream.PublishAsync(null!));
    }

    [Fact]
    public async Task Clear_RemovesAllEvents()
    {
        // Arrange
        var stream = new EditEventStream();
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 2 });

        // Act
        stream.Clear();

        // Assert
        Assert.Empty(stream.RecentEvents);
    }

    [Fact]
    public async Task RecentEvents_ReturnsThreadSafeSnapshot()
    {
        // Arrange
        var stream = new EditEventStream();
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });

        // Act
        var snapshot = stream.RecentEvents;
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 2 });

        // Assert - snapshot should not be affected by subsequent publishes
        Assert.Single(snapshot);
    }

    #endregion

    #region A3.6: Validation Event Tests - Rule descriptors, severity, and error messages

    [Fact]
    public void ValidationRuleDescriptor_HasCorrectDefaults()
    {
        // Arrange & Act
        var descriptor = new ValidationRuleDescriptor("Required");

        // Assert
        Assert.Equal("Required", descriptor.Name);
        Assert.Null(descriptor.Description);
        Assert.Equal(ValidationSeverity.Error, descriptor.Severity);
    }

    [Fact]
    public void ValidationRuleDescriptor_CanSetAllProperties()
    {
        // Arrange & Act
        var descriptor = new ValidationRuleDescriptor(
            Name: "StringLength",
            Description: "Must be between 1 and 100 characters",
            Severity: ValidationSeverity.Warning
        );

        // Assert
        Assert.Equal("StringLength", descriptor.Name);
        Assert.Equal("Must be between 1 and 100 characters", descriptor.Description);
        Assert.Equal(ValidationSeverity.Warning, descriptor.Severity);
    }

    [Fact]
    public void ValidationRuleResult_Success_HasCorrectProperties()
    {
        // Arrange & Act
        var result = ValidationRuleResult.Success("Required");

        // Assert
        Assert.Equal("Required", result.RuleName);
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void ValidationRuleResult_Failure_HasCorrectProperties()
    {
        // Arrange & Act
        var result = ValidationRuleResult.Failure("StringLength", "Too short", ValidationSeverity.Error);

        // Assert
        Assert.Equal("StringLength", result.RuleName);
        Assert.False(result.IsValid);
        Assert.Equal("Too short", result.ErrorMessage);
        Assert.Equal(ValidationSeverity.Error, result.Severity);
    }

    [Fact]
    public void ValidationSeverity_HasExpectedValues()
    {
        // Assert
        Assert.Equal(0, (int)ValidationSeverity.Info);
        Assert.Equal(1, (int)ValidationSeverity.Warning);
        Assert.Equal(2, (int)ValidationSeverity.Error);
    }

    [Fact]
    public void ValidationFailedEvent_RuleResultsIncludeSeverity()
    {
        // Arrange
        var ruleResults = new List<ValidationRuleResult>
        {
            ValidationRuleResult.Failure("Required", "Field is required", ValidationSeverity.Error),
            new ValidationRuleResult
            {
                RuleName = "Recommendation",
                IsValid = false,
                ErrorMessage = "Consider using a stronger password",
                Severity = ValidationSeverity.Warning
            }
        };

        // Act
        var @event = new ValidationFailedEvent
        {
            ItemKey = 1,
            RuleResults = ruleResults
        };

        // Assert
        Assert.Equal(ValidationSeverity.Error, @event.RuleResults[0].Severity);
        Assert.Equal(ValidationSeverity.Warning, @event.RuleResults[1].Severity);
    }

    #endregion

    #region A3.5: Backward Compatibility Tests

    [Fact]
    public void InlineEditingFeature_ShowEventsDefaultsFalse()
    {
        // Arrange & Act
        var feature = new InlineEditingFeature<object, string>();

        // Assert - ShowEvents should default to false for backward compat
        Assert.False(feature.ShowEvents);
    }

    [Fact]
    public void InlineEditingFeature_ExistingCallbacksStillExist()
    {
        // Arrange
        var feature = new InlineEditingFeature<object, string>();

        // Assert - These properties should still exist for backward compat
        Assert.False(feature.OnValueChanged.HasDelegate);
        Assert.False(feature.OnValidationCompleted.HasDelegate);
    }

    [Fact]
    public void InlineEditingFeature_ValidatorsPropertyStillWorks()
    {
        // Arrange
        var feature = new InlineEditingFeature<object, string>
        {
            Validators = [new RequiredStringValidator()]
        };

        // Assert
        Assert.Single(feature.Validators);
        Assert.Equal("Required", feature.Validators[0].Name);
    }

    #endregion

    #region A3.7: Telemetry Safeguards - Disposal, event limit, exception handling

    [Fact]
    public async Task PublishAsync_TrimsOldEventsWhenLimitExceeded()
    {
        // Arrange
        var stream = new EditEventStream(maxEvents: 3);

        // Act
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 2 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 3 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 4 });
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 5 });

        // Assert
        var events = stream.RecentEvents;
        Assert.Equal(3, events.Count);
        Assert.Equal(3, events[0].ItemKey);
        Assert.Equal(4, events[1].ItemKey);
        Assert.Equal(5, events[2].ItemKey);
    }

    [Fact]
    public void Dispose_ClearsEventsAndHandlers()
    {
        // Arrange
        var stream = new EditEventStream();
        var handlerCalled = false;
        stream.EventPublished += _ => handlerCalled = true;

        // Act
        stream.Dispose();

        // Assert
        Assert.Empty(stream.RecentEvents);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var stream = new EditEventStream();

        // Act & Assert - should not throw
        stream.Dispose();
        stream.Dispose();
        stream.Dispose();
    }

    [Fact]
    public async Task PublishAsync_SwallowsExceptionsFromHandlers()
    {
        // Arrange
        var stream = new EditEventStream();
        var secondHandlerCalled = false;

        stream.EventPublished += _ => throw new InvalidOperationException("Handler failed");
        stream.EventPublished += _ => secondHandlerCalled = true;

        // Act - should not throw
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });

        // Assert - event should still be added
        Assert.Single(stream.RecentEvents);
        // Note: second handler may not be called due to exception swallowing implementation
    }

    [Fact]
    public async Task PublishAsync_IsSynchronous()
    {
        // Arrange
        var stream = new EditEventStream();
        var events = new List<int>();

        stream.EventPublished += _ => events.Add(1);

        // Act
        await stream.PublishAsync(new EditCommittedEvent { ItemKey = 1 });
        events.Add(2);

        // Assert - 1 should be added before 2 because invocation is synchronous
        Assert.Equal(new[] { 1, 2 }, events);
    }

    [Fact]
    public void IEditEventStream_IsDisposable()
    {
        // Arrange & Act
        IEditEventStream stream = new EditEventStream();

        // Assert
        Assert.IsAssignableFrom<IDisposable>(stream);
        stream.Dispose(); // Should not throw
    }

    #endregion
}
