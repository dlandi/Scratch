namespace QuickGridTest01.ComposableColumns.Features.Editing;

/// <summary>
/// Severity level for validation rules.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>
    /// Informational validation message.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning - value is acceptable but may have issues.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error - value is invalid and cannot be committed.
    /// </summary>
    Error = 2
}

/// <summary>
/// Describes a validation rule for display in UI.
/// </summary>
/// <param name="Name">The name of the validation rule (e.g., "Required", "StringLength").</param>
/// <param name="Description">Human-readable description of the rule.</param>
/// <param name="Severity">The severity level of this rule.</param>
public readonly record struct ValidationRuleDescriptor(
    string Name,
    string? Description = null,
    ValidationSeverity Severity = ValidationSeverity.Error
);

/// <summary>
/// Result of a single validation rule execution.
/// </summary>
public class ValidationRuleResult
{
    /// <summary>
    /// The name of the validator that produced this result.
    /// </summary>
    public required string RuleName { get; init; }

    /// <summary>
    /// Whether the validation passed.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// The error message if validation failed.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// The severity of this validation rule.
    /// </summary>
    public ValidationSeverity Severity { get; init; } = ValidationSeverity.Error;

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationRuleResult Success(string ruleName) => new()
    {
        RuleName = ruleName,
        IsValid = true
    };

    /// <summary>
    /// Creates a failed validation result.
    /// </summary>
    public static ValidationRuleResult Failure(string ruleName, string errorMessage, ValidationSeverity severity = ValidationSeverity.Error) => new()
    {
        RuleName = ruleName,
        IsValid = false,
        ErrorMessage = errorMessage,
        Severity = severity
    };
}

/// <summary>
/// Base class for all edit events published to the event stream.
/// </summary>
public abstract class EditEventBase
{
    /// <summary>
    /// Unique identifier for correlating events within an edit session.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The key identifying the item being edited.
    /// </summary>
    public required object ItemKey { get; init; }

    /// <summary>
    /// The name of the property being edited.
    /// </summary>
    public string? PropertyName { get; init; }

    /// <summary>
    /// When the event occurred.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.Now;

    /// <summary>
    /// The type of event for filtering/display.
    /// </summary>
    public abstract string EventType { get; }
}

/// <summary>
/// Event raised when editing begins on a cell.
/// </summary>
public class EditStartedEvent : EditEventBase
{
    public override string EventType => "EditStarted";

    /// <summary>
    /// The current value when editing started.
    /// </summary>
    public object? CurrentValue { get; init; }
}

/// <summary>
/// Event raised when a value is successfully committed.
/// </summary>
public class EditCommittedEvent : EditEventBase
{
    public override string EventType => "EditCommitted";

    /// <summary>
    /// The value before the edit.
    /// </summary>
    public object? OldValue { get; init; }

    /// <summary>
    /// The new value after the edit.
    /// </summary>
    public object? NewValue { get; init; }
}

/// <summary>
/// Event raised when an edit is cancelled (e.g., Escape key).
/// </summary>
public class EditCancelledEvent : EditEventBase
{
    public override string EventType => "EditCancelled";

    /// <summary>
    /// The original value that was restored.
    /// </summary>
    public object? OriginalValue { get; init; }

    /// <summary>
    /// The value the user had entered before cancelling.
    /// </summary>
    public object? AttemptedValue { get; init; }
}

/// <summary>
/// Event raised when validation fails.
/// </summary>
public class ValidationFailedEvent : EditEventBase
{
    public override string EventType => "ValidationFailed";

    /// <summary>
    /// The value that failed validation.
    /// </summary>
    public object? AttemptedValue { get; init; }

    /// <summary>
    /// The validation errors that occurred.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = [];

    /// <summary>
    /// Detailed validation rule results including rule names and severity.
    /// </summary>
    public IReadOnlyList<ValidationRuleResult> RuleResults { get; init; } = [];
}

/// <summary>
/// Event raised when validation succeeds.
/// </summary>
public class ValidationSucceededEvent : EditEventBase
{
    public override string EventType => "ValidationSucceeded";

    /// <summary>
    /// The value that passed validation.
    /// </summary>
    public object? Value { get; init; }

    /// <summary>
    /// The validation rules that were checked.
    /// </summary>
    public IReadOnlyList<ValidationRuleResult> RuleResults { get; init; } = [];
}

/// <summary>
/// Interface for the grid-level edit event stream.
/// Features publish events to this stream; UI components consume them.
/// </summary>
public interface IEditEventStream : IDisposable
{
    /// <summary>
    /// The most recent events (up to a configured limit).
    /// Oldest events are dropped when the limit is exceeded.
    /// </summary>
    IReadOnlyList<EditEventBase> RecentEvents { get; }

    /// <summary>
    /// Raised when a new event is published to the stream.
    /// </summary>
    event Action<EditEventBase>? EventPublished;

    /// <summary>
    /// Publishes an event to the stream.
    /// </summary>
    /// <param name="event">The event to publish.</param>
    Task PublishAsync(EditEventBase @event);

    /// <summary>
    /// Clears all events from the stream.
    /// </summary>
    void Clear();

    /// <summary>
    /// The maximum number of events to retain.
    /// </summary>
    int MaxEvents { get; }
}

/// <summary>
/// Default implementation of <see cref="IEditEventStream"/>.
/// Maintains a bounded list of recent events with thread-safe access.
/// </summary>
public class EditEventStream : IEditEventStream
{
    private readonly List<EditEventBase> _events = new();
    private readonly object _lock = new();
    private bool _disposed;

    /// <summary>
    /// Creates a new event stream with the specified event limit.
    /// </summary>
    /// <param name="maxEvents">Maximum number of events to retain. Default is 100.</param>
    public EditEventStream(int maxEvents = 100)
    {
        if (maxEvents < 1)
            throw new ArgumentOutOfRangeException(nameof(maxEvents), "Must be at least 1");

        MaxEvents = maxEvents;
    }

    /// <inheritdoc />
    public int MaxEvents { get; }

    /// <inheritdoc />
    public IReadOnlyList<EditEventBase> RecentEvents
    {
        get
        {
            lock (_lock)
            {
                return _events.ToList().AsReadOnly();
            }
        }
    }

    /// <inheritdoc />
    public event Action<EditEventBase>? EventPublished;

    /// <inheritdoc />
    public Task PublishAsync(EditEventBase @event)
    {
        if (_disposed)
            return Task.CompletedTask;

        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        lock (_lock)
        {
            _events.Add(@event);

            // Trim oldest events if we exceed the limit
            while (_events.Count > MaxEvents)
            {
                _events.RemoveAt(0);
            }
        }

        // Invoke event handlers synchronously (Blazor Server requirement)
        // This allows UI components to update immediately
        try
        {
            EventPublished?.Invoke(@event);
        }
        catch
        {
            // Swallow exceptions from event handlers to prevent
            // one bad handler from breaking the stream
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Clear()
    {
        lock (_lock)
        {
            _events.Clear();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        lock (_lock)
        {
            _events.Clear();
        }

        // Clear event handlers to prevent memory leaks
        EventPublished = null;

        GC.SuppressFinalize(this);
    }
}
