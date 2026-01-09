# EditEventStream Implementation Specification

## Document Information
| Attribute | Value |
|-----------|-------|
| Created | 2025-12-16 |
| Task | A2.2 |
| Source File | `QuickGridTest01.ComposableColumns.Features.Editing.EditEventStream.cs` |

---

## 1. Interface Definition: `IEditEventStream`

```csharp
public interface IEditEventStream : IDisposable
{
    IReadOnlyList<EditEventBase> RecentEvents { get; }
    event Action<EditEventBase>? EventPublished;
    Task PublishAsync(EditEventBase @event);
    void Clear();
    int MaxEvents { get; }
}
```

### Member Details

| Member | Type | Description |
|--------|------|-------------|
| `RecentEvents` | `IReadOnlyList<EditEventBase>` | Returns a snapshot of recent events (newest last). Thread-safe copy. |
| `EventPublished` | `event Action<EditEventBase>?` | Raised synchronously when an event is published. UI components subscribe here. |
| `PublishAsync` | `Task` | Adds an event to the stream and notifies subscribers. Returns immediately (synchronous). |
| `Clear` | `void` | Removes all events from the stream. |
| `MaxEvents` | `int` | The configured maximum number of events to retain. |

---

## 2. Implementation: `EditEventStream`

### Constructor

```csharp
public EditEventStream(int maxEvents = 100)
```

| Parameter | Default | Constraints | Description |
|-----------|---------|-------------|-------------|
| `maxEvents` | 100 | Must be ? 1 | Maximum events to retain before oldest are dropped |

### Thread Safety

The implementation uses a simple lock (`object _lock`) for thread-safe access to the event list:

```csharp
private readonly List<EditEventBase> _events = new();
private readonly object _lock = new();
```

**Why not `ConcurrentQueue`?**
- Simple list with lock is sufficient for Blazor Server's single-threaded synchronization context
- Allows easy trimming from the beginning (oldest events)
- `RecentEvents` returns a snapshot copy, avoiding enumeration issues

### Event Limit Enforcement

When `PublishAsync` is called:

1. Event is added to the list
2. While list count exceeds `MaxEvents`, remove from index 0 (oldest)
3. Invoke `EventPublished` event

```csharp
lock (_lock)
{
    _events.Add(@event);
    while (_events.Count > MaxEvents)
    {
        _events.RemoveAt(0);
    }
}
```

**Rationale for 100-event default:**
- Sufficient for typical demo/debugging scenarios
- Low memory footprint (~100KB for typical events)
- Can be increased via constructor for analytics use cases

---

## 3. Threading Guidance

### Synchronous Event Invocation

The `EventPublished` event is invoked **synchronously** after adding the event:

```csharp
try
{
    EventPublished?.Invoke(@event);
}
catch
{
    // Swallow exceptions from event handlers
}
```

**Rationale:**
- Blazor Server components run on the synchronization context
- Immediate notification allows `StateHasChanged()` in handlers
- Async would require marshaling back to the UI thread anyway
- Keeps the implementation simple and predictable

### Exception Handling

Event handler exceptions are **swallowed** to prevent one bad subscriber from breaking the stream:

```csharp
catch
{
    // Swallow exceptions from event handlers
}
```

**Future Enhancement:** Consider logging handler exceptions for debugging.

---

## 4. Grid-Scoped Lifecycle

### Creation
The `EditEventStream` is created by `ComposableGrid` and provided via cascading value:

```razor
@* In ComposableGrid.razor *@
<CascadingValue Value="_editEventStream" IsFixed="true">
    @* Grid content *@
</CascadingValue>

@code {
    private EditEventStream? _editEventStream;

    protected override void OnInitialized()
    {
        _editEventStream = new EditEventStream();
    }
}
```

### Disposal
The grid disposes the stream when the component is disposed:

```csharp
public void Dispose()
{
    _editEventStream?.Dispose();
}
```

**Disposal Behavior:**
1. Sets `_disposed = true` (prevents further publishes)
2. Clears the event list
3. Nulls out `EventPublished` to release handler references
4. Calls `GC.SuppressFinalize(this)`

### Consumer Lifecycle

UI components consuming the stream:

1. Receive stream via `[CascadingParameter]`
2. Subscribe to `EventPublished` in `OnInitialized`
3. **Must** unsubscribe in `Dispose` to prevent memory leaks

```csharp
[CascadingParameter]
public IEditEventStream? EditEventStream { get; set; }

protected override void OnInitialized()
{
    if (EditEventStream is not null)
    {
        EditEventStream.EventPublished += OnEventPublished;
    }
}

private void OnEventPublished(EditEventBase @event)
{
    // Update UI state
    StateHasChanged();
}

public void Dispose()
{
    if (EditEventStream is not null)
    {
        EditEventStream.EventPublished -= OnEventPublished;
    }
}
```

---

## 5. Event Types Summary

| Event Type | Class | When Published |
|------------|-------|----------------|
| EditStarted | `EditStartedEvent` | Focus enters a cell |
| EditCommitted | `EditCommittedEvent` | Value successfully saved (after validation) |
| EditCancelled | `EditCancelledEvent` | User presses Escape |
| ValidationFailed | `ValidationFailedEvent` | Validation fails on blur/enter |
| ValidationSucceeded | `ValidationSucceededEvent` | Validation passes on blur/enter |

---

## 6. Publishing Guidelines for `InlineEditingFeature`

### Opt-In via `ShowEvents` Parameter

```csharp
public bool ShowEvents { get; set; } = false;
```

When `ShowEvents = true` AND a cascaded `IEditEventStream` is available:

| Lifecycle Point | Event to Publish |
|-----------------|------------------|
| `HandleFocus()` | `EditStartedEvent` |
| `HandleKeyDownAsync(Escape)` | `EditCancelledEvent` |
| `ValidateAndCommitAsync()` (fail) | `ValidationFailedEvent` |
| `ValidateAndCommitAsync()` (pass) | `ValidationSucceededEvent` |
| `CommitValueAsync()` (value changed) | `EditCommittedEvent` |

### Guard Pattern

```csharp
private async Task PublishEventIfEnabledAsync(EditEventBase @event)
{
    if (!ShowEvents)
        return;

    if (_editEventStream is null)
        return;

    await _editEventStream.PublishAsync(@event);
}
```

---

## 7. Performance Considerations

### Overhead When Disabled

When `ShowEvents = false`:
- No event objects created
- No stream lookups
- Zero overhead beyond the boolean check

### Memory Usage

With default 100-event limit:
- ~1KB per event (typical)
- ~100KB total for full stream
- Events contain references to item keys (not full items)

### Best Practices

1. **Don't store large objects** in event payloads - use keys instead
2. **Keep `MaxEvents` reasonable** - 100-500 for debugging, 1000+ for analytics
3. **Clear the stream** when navigating away to free memory
4. **Dispose handlers** to prevent memory leaks in long-running grids

---

## 8. Future Enhancements

| Enhancement | Priority | Notes |
|-------------|----------|-------|
| Async event handlers | Low | Would complicate threading model |
| Event filtering API | Medium | `GetEvents<TEvent>()` method |
| Persistence adapter | Low | Save events to localStorage/database |
| Event replay | Low | Re-publish events for testing |
| Telemetry integration | Medium | Publish to Application Insights |
