# Edit Event Stream Usage Examples

## Document Information
| Attribute | Value |
|-----------|-------|
| Created | 2025-12-16 |
| Task | A5.1 |
| Purpose | Provide sample code for common event stream usage patterns |

---

## 1. Grid Auto-Rendering Panel with EventPanelPlacement

The simplest approach - let the grid handle everything:

```razor
@* In your page *@
<ComposableGrid TGridItem="Product"
                Items="@_products"
                EventPanelPlacement="EventPanelPlacement.Right">
    
    <ComposableColumn TGridItem="Product" TValue="string"
                      Property="@(p => p.Name)"
                      Title="Name"
                      FeatureCollection="@_nameEditFeatures" />
    
    <ComposableColumn TGridItem="Product" TValue="decimal"
                      Property="@(p => p.Price)"
                      Title="Price"
                      Format="C2"
                      FeatureCollection="@_priceEditFeatures" />
</ComposableGrid>

@code {
    private IQueryable<Product> _products = ...;
    
    private readonly IColumnFeature<Product>[] _nameEditFeatures =
    [
        new InlineEditingFeature<Product, string>
        {
            Editor = EditorKind.Text,
            ShowEvents = true,  // Enable event publishing
            ItemKey = p => p.Id,
            Validators = [new RequiredStringValidator()]
        }
    ];
    
    private readonly IColumnFeature<Product>[] _priceEditFeatures =
    [
        new InlineEditingFeature<Product, decimal>
        {
            Editor = EditorKind.Currency,
            ShowEvents = true,
            ItemKey = p => p.Id,
            Validators = [new RangeValidator<decimal> { Minimum = 0.01m }]
        }
    ];
}
```

### EventPanelPlacement Options

```csharp
public enum EventPanelPlacement
{
    None,   // No auto-rendering (default)
    Top,    // Panel above grid
    Bottom, // Panel below grid
    Left,   // Panel to left of grid
    Right   // Panel to right of grid
}
```

---

## 2. Manual Panel Placement with Cascaded Stream

For more control over layout:

```razor
@* In your page *@
@using QuickGridTest01.ComposableColumns.Features.Editing

<div class="demo-layout-horizontal">
    <div class="grid-section">
        <ComposableGrid TGridItem="Product"
                        Items="@_products"
                        @ref="_grid">
            
            <ComposableColumn TGridItem="Product" TValue="string"
                              Property="@(p => p.Name)"
                              Title="Name"
                              FeatureCollection="@_nameEditFeatures" />
        </ComposableGrid>
    </div>
    
    <div class="event-panel-section">
        @* Consumes the cascaded IEditEventStream from the grid *@
        <EditEventViewer Title="Recent Changes" 
                         MaxDisplayEvents="50"
                         ShowTimestamps="true"
                         ShowPropertyNames="true" />
    </div>
</div>

@code {
    private ComposableGrid<Product>? _grid;
    private IQueryable<Product> _products = ...;
    
    private readonly IColumnFeature<Product>[] _nameEditFeatures =
    [
        new InlineEditingFeature<Product, string>
        {
            Editor = EditorKind.Text,
            ShowEvents = true,
            ItemKey = p => p.Id
        }
    ];
}
```

### CSS for Manual Layout

```css
.demo-layout-horizontal {
    display: flex;
    gap: 1rem;
}

.grid-section {
    flex: 3;
}

.event-panel-section {
    flex: 1;
    min-width: 300px;
    max-height: 500px;
    overflow-y: auto;
}
```

---

## 3. Custom Event Viewer Implementation

Build your own event consumer:

```razor
@* CustomEventViewer.razor *@
@using QuickGridTest01.ComposableColumns.Features.Editing
@implements IDisposable

<div class="custom-event-viewer">
    <h4>@Title (@_events.Count events)</h4>
    
    <div class="event-controls">
        <button @onclick="Clear">Clear</button>
        <select @bind="_filterType">
            <option value="">All Events</option>
            <option value="EditCommitted">Commits Only</option>
            <option value="ValidationFailed">Failures Only</option>
        </select>
    </div>
    
    <ul class="event-list">
        @foreach (var evt in FilteredEvents)
        {
            <li class="event-item @evt.EventType.ToLower()">
                <span class="event-type">@evt.EventType</span>
                <span class="event-property">@evt.PropertyName</span>
                <span class="event-time">@evt.Timestamp.ToString("HH:mm:ss")</span>
                
                @if (evt is EditCommittedEvent committed)
                {
                    <span class="event-values">
                        @committed.OldValue ? @committed.NewValue
                    </span>
                }
                else if (evt is ValidationFailedEvent failed)
                {
                    <span class="event-errors">
                        @string.Join(", ", failed.Errors)
                    </span>
                }
            </li>
        }
    </ul>
</div>

@code {
    [CascadingParameter]
    public IEditEventStream? EventStream { get; set; }
    
    [Parameter]
    public string Title { get; set; } = "Events";
    
    private readonly List<EditEventBase> _events = new();
    private string _filterType = "";
    
    private IEnumerable<EditEventBase> FilteredEvents =>
        string.IsNullOrEmpty(_filterType)
            ? _events.AsEnumerable().Reverse()
            : _events.Where(e => e.EventType == _filterType).Reverse();
    
    protected override void OnInitialized()
    {
        if (EventStream is not null)
        {
            EventStream.EventPublished += OnEventPublished;
            
            // Load existing events
            foreach (var evt in EventStream.RecentEvents)
            {
                _events.Add(evt);
            }
        }
    }
    
    private void OnEventPublished(EditEventBase evt)
    {
        _events.Add(evt);
        
        // Trim to last 100 events
        while (_events.Count > 100)
        {
            _events.RemoveAt(0);
        }
        
        InvokeAsync(StateHasChanged);
    }
    
    private void Clear()
    {
        _events.Clear();
        EventStream?.Clear();
    }
    
    public void Dispose()
    {
        if (EventStream is not null)
        {
            EventStream.EventPublished -= OnEventPublished;
        }
    }
}
```

### CSS for Custom Viewer

```css
.custom-event-viewer {
    padding: 1rem;
    border: 1px solid #ddd;
    border-radius: 4px;
}

.event-list {
    list-style: none;
    padding: 0;
    margin: 0;
}

.event-item {
    display: flex;
    gap: 0.5rem;
    padding: 0.5rem;
    border-bottom: 1px solid #eee;
    font-size: 0.85rem;
}

.event-item.editcommitted {
    background: #e8f5e9;
}

.event-item.validationfailed {
    background: #ffebee;
}

.event-item.editcancelled {
    background: #fff3e0;
}

.event-type {
    font-weight: bold;
    min-width: 120px;
}

.event-time {
    color: #666;
}

.event-errors {
    color: #d32f2f;
}
```

---

## 4. Analytics/Telemetry Integration

Publish events to an external system:

```csharp
// In your service
public class EditTelemetryService : IDisposable
{
    private readonly ILogger<EditTelemetryService> _logger;
    private IEditEventStream? _stream;
    
    public EditTelemetryService(ILogger<EditTelemetryService> logger)
    {
        _logger = logger;
    }
    
    public void AttachToStream(IEditEventStream stream)
    {
        _stream = stream;
        _stream.EventPublished += OnEvent;
    }
    
    private void OnEvent(EditEventBase evt)
    {
        switch (evt)
        {
            case EditCommittedEvent committed:
                _logger.LogInformation(
                    "Edit committed: {Property} changed from {Old} to {New}",
                    committed.PropertyName,
                    committed.OldValue,
                    committed.NewValue);
                break;
                
            case ValidationFailedEvent failed:
                _logger.LogWarning(
                    "Validation failed: {Property} = {Value}, Errors: {Errors}",
                    failed.PropertyName,
                    failed.AttemptedValue,
                    string.Join(", ", failed.Errors));
                break;
                
            case EditCancelledEvent cancelled:
                _logger.LogInformation(
                    "Edit cancelled: {Property}, reverted from {Attempted} to {Original}",
                    cancelled.PropertyName,
                    cancelled.AttemptedValue,
                    cancelled.OriginalValue);
                break;
        }
    }
    
    public void Dispose()
    {
        if (_stream is not null)
        {
            _stream.EventPublished -= OnEvent;
        }
    }
}
```

---

## 5. Event Counters Dashboard

Build a simple metrics display:

```razor
@* EditMetricsDashboard.razor *@
@using QuickGridTest01.ComposableColumns.Features.Editing
@implements IDisposable

<div class="metrics-dashboard">
    <div class="metric">
        <span class="metric-value">@_commitCount</span>
        <span class="metric-label">Commits</span>
    </div>
    <div class="metric">
        <span class="metric-value">@_cancelCount</span>
        <span class="metric-label">Cancels</span>
    </div>
    <div class="metric">
        <span class="metric-value">@_failureCount</span>
        <span class="metric-label">Failures</span>
    </div>
</div>

@code {
    [CascadingParameter]
    public IEditEventStream? EventStream { get; set; }
    
    private int _commitCount;
    private int _cancelCount;
    private int _failureCount;
    
    protected override void OnInitialized()
    {
        if (EventStream is not null)
        {
            EventStream.EventPublished += OnEvent;
        }
    }
    
    private void OnEvent(EditEventBase evt)
    {
        switch (evt)
        {
            case EditCommittedEvent:
                _commitCount++;
                break;
            case EditCancelledEvent:
                _cancelCount++;
                break;
            case ValidationFailedEvent:
                _failureCount++;
                break;
        }
        
        InvokeAsync(StateHasChanged);
    }
    
    public void Dispose()
    {
        if (EventStream is not null)
        {
            EventStream.EventPublished -= OnEvent;
        }
    }
}
```

---

## Summary

| Pattern | Use Case | Complexity |
|---------|----------|------------|
| Auto-rendering with `EventPanelPlacement` | Quick demos, simple UIs | Low |
| Manual placement with `EditEventViewer` | Custom layouts | Medium |
| Custom event viewer | Full control over UI | Medium-High |
| Telemetry integration | Logging, analytics | Medium |
| Metrics dashboard | Monitoring | Low-Medium |
