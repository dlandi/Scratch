# Task B2.1 Placement API – Deep Dive

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 2.0 |
| Status | ? **DECISION MADE: Option 5** |
| Created | 2025-12-16 |
| Updated | 2025-12-16 |
| Related Spec | `InlineEditingPolish.md` |
| Task ID | B2.1 |
| Decision | Grid-Provided Stream + Optional Auto-Panel (Option 5) |

---

## The Core Question

> Should the event-viewer/validation shell render **inside** `ComposableGrid` (as a child slot) or **beside** it (as a sibling container)?

This is fundamentally a question about **ownership** and **layout responsibility**.

---

## Context from Specification

From `InlineEditingPolish.md` Section 4, Goal 3:

> A small shared component (or markup pattern) that renders the active validation rule list plus the latest validation result for the focused cell. It should be **purely presentational** (bound to read-only data supplied by event hooks) so teams can **host it wherever they like** or replace it with their own UX. The shell also needs simple placement options (e.g., top, right, left, bottom of the grid) so the event-viewer/summary overlay can be positioned per layout requirements. Think of it the same way the **legacy `RowColumn` separates column-level triggers from the grid-level overlay**: columns raise events, while the shared shell beside the grid consumes those events and renders once.

### Key Design Principles Extracted
1. **Purely presentational** – Shell is a passive consumer of events
2. **Host it wherever they like** – Maximum placement flexibility
3. **Follows `RowColumn` pattern** – Separation of concerns between feature and overlay
4. **Simple placement options** – Top/Right/Left/Bottom positioning

---

## Option 1: Inside the Grid (Child Slot / RenderFragment Parameter)

### How It Works

```razor
<ComposableGrid Items="@items">
    <Columns>
        <ComposableColumn ... />
    </Columns>
    
    <!-- New slot for the overlay shell -->
    <EventPanel Placement="Placement.Right">
        <EditEventViewer Events="@_eventStream" />
    </EventPanel>
</ComposableGrid>
```

The grid component owns a `RenderFragment` parameter and renders it at the specified position relative to the `<table>` element.

### Pros

| Benefit | Explanation |
|---------|-------------|
| **Encapsulated layout** | Grid controls its own wrapper `<div>` with CSS grid/flex; consumer doesn't worry about positioning |
| **Single source of truth** | Placement enum lives on the grid; no coordination needed |
| **Automatic context** | Shell can access grid's cascading values (e.g., current focused row, validation state) without extra wiring |
| **Cleaner consumer markup** | Everything is "inside" one component tree |

### Cons

| Drawback | Explanation |
|----------|-------------|
| **Grid bloat** | `ComposableGrid` gains new parameters (`EventPanelContent`, `EventPanelPlacement`), increasing its API surface |
| **Limited flexibility** | Hard to position the panel outside the grid's DOM subtree (e.g., in a global sidebar or modal) |
| **Breaking existing layouts** | If the grid currently renders just a `<table>`, adding a wrapper `<div>` for layout could break existing CSS |
| **Coupling** | The shell's existence is tied to the grid—can't reuse it standalone |

### Implementation Sketch

```csharp
// ComposableGrid.razor parameters
[Parameter] public RenderFragment? EventPanelContent { get; set; }
[Parameter] public Placement EventPanelPlacement { get; set; } = Placement.None;

// Render logic
<div class="composable-grid-wrapper @GetLayoutClass()">
    @if (EventPanelPlacement == Placement.Top)
    {
        @EventPanelContent
    }
    
    <table class="composable-grid">...</table>
    
    @if (EventPanelPlacement == Placement.Bottom)
    {
        @EventPanelContent
    }
</div>
```

**CSS Strategy:**
```css
.composable-grid-wrapper {
    display: grid;
    grid-template-columns: 1fr auto;
    gap: var(--space-16);
}

.composable-grid-wrapper.placement-top,
.composable-grid-wrapper.placement-bottom {
    grid-template-columns: 1fr;
}
```

---

## Option 2: Outside the Grid (Sibling Container)

### How It Works

```razor
<div class="grid-with-panel" style="display: flex;">
    <ComposableGrid Items="@items">
        ...
    </ComposableGrid>
    
    <!-- Completely separate component -->
    <EditEventViewer Events="@_eventStream" 
                     Placement="Placement.Right" 
                     Class="event-panel" />
</div>
```

The consumer is responsible for the layout container; the shell is just a standalone component that renders its content.

### Pros

| Benefit | Explanation |
|---------|-------------|
| **Grid stays lean** | No new parameters or wrapper elements in `ComposableGrid` |
| **Maximum flexibility** | Shell can be placed anywhere—sidebar, modal, different page region, or even in a different Blazor component |
| **Reusable** | Same shell works with non-grid scenarios (e.g., form-level validation summary) |
| **No breaking changes** | Existing grid layouts are untouched |
| **Matches legacy pattern** | The spec notes `RowColumn` "separates column-level triggers from the grid-level overlay"—this follows that model |

### Cons

| Drawback | Explanation |
|----------|-------------|
| **Consumer layout burden** | Devs must write their own flex/grid container and CSS |
| **Event wiring** | Shell needs explicit binding to the event stream; no automatic cascading context |
| **Placement enum is "advisory"** | The `Placement` parameter only affects internal rendering (e.g., border direction), not actual positioning—that's on the consumer's CSS |
| **Harder to document** | More examples needed to show various layout configurations |

### Implementation Sketch

```razor
<!-- EditEventViewer.razor -->
<div class="edit-event-viewer @PlacementClass">
    <h4>Event Log</h4>
    @foreach (var evt in Events.TakeLast(10))
    {
        <div class="log-entry">@evt.Summary</div>
    }
</div>

@code {
    [Parameter] public IEnumerable<EditEvent> Events { get; set; } = [];
    [Parameter] public Placement Placement { get; set; } = Placement.Right;
    
    private string PlacementClass => Placement switch
    {
        Placement.Left => "panel-left",
        Placement.Right => "panel-right",
        Placement.Top => "panel-top",
        Placement.Bottom => "panel-bottom",
        _ => ""
    };
}
```

**Consumer Layout Examples:**

```razor
<!-- Right Placement -->
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items" Class="demo-grid">...</ComposableGrid>
    <EditEventViewer Events="@_eventStream" Placement="Placement.Right" />
</div>

<!-- Bottom Placement -->
<div class="demo-layout-vertical">
    <ComposableGrid Items="@items" Class="demo-grid">...</ComposableGrid>
    <EditEventViewer Events="@_eventStream" Placement="Placement.Bottom" />
</div>
```

**CSS Utilities:**

```css
/* Horizontal layout (Left/Right placement) */
.demo-layout-horizontal {
    display: flex;
    gap: var(--space-16);
    align-items: flex-start;
}

.demo-layout-horizontal .edit-event-viewer {
    flex: 0 0 300px;
    max-height: 500px;
    overflow-y: auto;
}

/* Vertical layout (Top/Bottom placement) */
.demo-layout-vertical {
    display: flex;
    flex-direction: column;
    gap: var(--space-12);
}

.demo-layout-vertical .edit-event-viewer {
    max-height: 200px;
    overflow-y: auto;
}

/* Placement-specific styling hints */
.edit-event-viewer.panel-right {
    border-left: 2px solid var(--color-accent-primary);
}

.edit-event-viewer.panel-bottom {
    border-top: 2px solid var(--color-accent-primary);
}
```

---

## Option 3: Hybrid Approach

### How It Works

Provide **both** mechanisms:
1. A lightweight `RenderFragment` slot in the grid for simple cases
2. A standalone `<EditEventViewer>` component for advanced layouts

```razor
<!-- Simple: use the grid slot -->
<ComposableGrid Items="@items" EventPanelPlacement="Placement.Bottom">
    <EventPanelContent>
        <EditEventViewer Events="@_eventStream" />
    </EventPanelContent>
    <Columns>
        ...
    </Columns>
</ComposableGrid>

<!-- Advanced: external placement -->
<div class="custom-layout">
    <aside class="sidebar">
        <EditEventViewer Events="@_eventStream" />
    </aside>
    <main>
        <ComposableGrid Items="@items">...</ComposableGrid>
    </main>
</div>
```

### Pros

| Benefit | Explanation |
|---------|-------------|
| **Progressive disclosure** | Easy things are easy (use grid slot), hard things are possible (external placement) |
| **Covers both scenarios** | Simple demos and complex layouts both supported |
| **Best of both worlds** | Encapsulation when needed, flexibility when needed |

### Cons

| Drawback | Explanation |
|----------|-------------|
| **Two ways to do the same thing** | Documentation overhead, potential confusion |
| **"Which approach should I use?"** | Decision fatigue for consumers |
| **Grid still gains parameters** | Doesn't fully avoid bloat (though parameters are optional) |
| **Implementation complexity** | Must maintain both code paths |

---

## Option 4: Grid-Provided Stream + Manual Panel (Recommended Hybrid)

### How It Works

The grid automatically provides a shared event stream via cascading value. Features opt-in to event broadcasting with `ShowEvents=true`. Consumers manually place the panel component wherever they want, and it automatically binds to the grid's event stream.

```razor
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items">
        <Columns>
            <!-- Feature opts into event broadcasting -->
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" ... />
                </Features>
            </ComposableColumn>
            
            <!-- Another feature also opts in -->
            <ComposableColumn Property="@(p => p.Email)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" ... />
                </Features>
            </ComposableColumn>
            
            <!-- This feature does NOT broadcast events -->
            <ComposableColumn Property="@(p => p.Phone)">
                <Features>
                    <InlineEditingFeature ... />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <!-- Panel manually placed, automatically binds to grid's cascading event stream -->
    <EditEventViewer Placement="Placement.Right" />
</div>
```

### Architecture Layers

| Layer | Responsibility |
|-------|----------------|
| **`ComposableGrid`** | Provides a shared `IEditEventStream` via cascading value. No new parameters. |
| **`InlineEditingFeature`** | Declares `[Parameter] public bool ShowEvents { get; set; }`. When true, publishes events to the cascaded stream. |
| **`IEditEventStream`** | Grid-level service that aggregates events from all opted-in features. |
| **`EditEventViewer`** | Standalone component that consumes the cascaded stream and renders events. Consumer controls placement. |

### Pros

| Benefit | Explanation |
|---------|-------------|
| **Zero grid bloat** | Grid provides stream via cascading value—no new parameters |
| **Automatic aggregation** | All features with `ShowEvents=true` publish to the same stream automatically |
| **Opt-in per column** | Fine-grained control—only certain columns broadcast events |
| **Maximum flexibility** | Consumer controls panel placement (anywhere: sibling, sidebar, modal) |
| **Single panel guarantee** | Grid provides one stream; consumer places one panel |
| **Separation of concerns** | Grid provides infrastructure, features publish events, consumer controls presentation |
| **No breaking changes** | Existing grids and features unaffected; event system is purely additive |

### Cons

| Drawback | Explanation |
|----------|-------------|
| **Cascading value overhead** | Grid must provide cascading context (minor performance cost) |
| **Consumer must wire panel** | Not fully automatic—consumer must place `<EditEventViewer>` component |
| **Stream lifecycle** | Stream must be disposed when grid is disposed |

### Implementation Sketch

**Step 1: Grid provides event stream via cascading value**

```csharp
// ComposableGrid.razor
<CascadingValue Value="@EventStream">
    <table class="composable-grid">
        @ChildContent
    </table>
</CascadingValue>

@code {
    private EditEventStream EventStream { get; } = new();
    
    public override void Dispose()
    {
        EventStream?.Dispose();
        base.Dispose();
    }
}
```

**Step 2: Feature publishes when `ShowEvents=true`**

```csharp
// InlineEditingFeature.cs
[Parameter] public bool ShowEvents { get; set; }

[CascadingParameter] public IEditEventStream? EventStream { get; set; }

private async Task CommitValueAsync(TGridItem item, FeatureContext<TGridItem, TValue> context, TValue? newValue)
{
    // ... existing commit logic ...
    
    // Publish event if opted-in
    if (ShowEvents && EventStream is not null)
    {
        await EventStream.PublishAsync(new EditCommittedEvent
        {
            Item = item,
            OldValue = originalValue,
            NewValue = newValue,
            PropertyName = context.PropertyName,
            Timestamp = DateTimeOffset.Now
        });
    }
    
    // ... rest of commit logic ...
}

private async Task HandleKeyDownAsync(TGridItem item, FeatureContext<TGridItem, TValue> context, KeyboardEventArgs e)
{
    if (e.Key == "Escape")
    {
        // ... revert logic ...
        
        // Publish cancel event if opted-in
        if (ShowEvents && EventStream is not null)
        {
            await EventStream.PublishAsync(new EditCancelledEvent
            {
                Item = item,
                RevertedValue = originalValue,
                PropertyName = context.PropertyName,
                Timestamp = DateTimeOffset.Now
            });
        }
    }
}
```

**Step 3: Event stream interface**

```csharp
// IEditEventStream.cs
namespace QuickGridTest01.ComposableColumns.Features.Editing;

public interface IEditEventStream : IDisposable
{
    /// <summary>
    /// Recent events in chronological order (oldest first).
    /// </summary>
    IReadOnlyList<EditEventBase> RecentEvents { get; }
    
    /// <summary>
    /// Publishes an event to all subscribers.
    /// </summary>
    Task PublishAsync(EditEventBase editEvent);
    
    /// <summary>
    /// Event raised when a new event is published.
    /// </summary>
    event EventHandler<EditEventPublishedArgs>? EventPublished;
}

// EditEventStream.cs
internal sealed class EditEventStream : IEditEventStream
{
    private readonly List<EditEventBase> _events = new();
    private readonly int _maxEvents = 100;
    
    public IReadOnlyList<EditEventBase> RecentEvents => _events.AsReadOnly();
    
    public event EventHandler<EditEventPublishedArgs>? EventPublished;
    
    public Task PublishAsync(EditEventBase editEvent)
    {
        _events.Add(editEvent);
        
        // Keep only recent events
        if (_events.Count > _maxEvents)
        {
            _events.RemoveAt(0);
        }
        
        EventPublished?.Invoke(this, new EditEventPublishedArgs(editEvent));
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _events.Clear();
        EventPublished = null;
    }
}
```

**Step 4: Panel consumes cascaded stream**

```razor
<!-- EditEventViewer.razor -->
<div class="edit-event-viewer @PlacementClass">
    <div class="event-viewer-header">
        <h4>Event Log</h4>
        @if (EventStream?.RecentEvents.Count > 0)
        {
            <button class="qg-btn qg-btn-ghost qg-btn-xs" @onclick="ClearEvents">Clear</button>
        }
    </div>
    
    @if (EventStream is null)
    {
        <div class="event-viewer-empty">No event stream available</div>
    }
    else if (EventStream.RecentEvents.Count == 0)
    {
        <div class="event-viewer-empty">No events yet</div>
    }
    else
    {
        <div class="event-viewer-list">
            @foreach (var evt in EventStream.RecentEvents.TakeLast(10).Reverse())
            {
                <div class="log-entry @GetEventClass(evt)">
                    <span class="event-time">@evt.Timestamp.ToString("HH:mm:ss")</span>
                    <span class="event-summary">@evt.Summary</span>
                </div>
            }
        </div>
    }
</div>

@code {
    [CascadingParameter] public IEditEventStream? EventStream { get; set; }
    [Parameter] public Placement Placement { get; set; } = Placement.Right;
    
    private string PlacementClass => Placement switch
    {
        Placement.Left => "panel-left",
        Placement.Right => "panel-right",
        Placement.Top => "panel-top",
        Placement.Bottom => "panel-bottom",
        _ => ""
    };
    
    private string GetEventClass(EditEventBase evt) => evt switch
    {
        EditCommittedEvent => "event-commit",
        EditCancelledEvent => "event-cancel",
        ValidationFailedEvent => "event-error",
        _ => "event-info"
    };
    
    private void ClearEvents()
    {
        // Could expose a Clear method on IEditEventStream
    }
}
```

**Step 5: Consumer usage patterns**

```razor
<!-- Pattern 1: Right sidebar -->
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items" Class="demo-grid">
        <Columns>
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <EditEventViewer Placement="Placement.Right" />
</div>

<!-- Pattern 2: Bottom panel -->
<div class="demo-layout-vertical">
    <ComposableGrid Items="@items" Class="demo-grid">
        <Columns>
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <EditEventViewer Placement="Placement.Bottom" />
</div>

<!-- Pattern 3: Global sidebar (different component) -->
<MainLayout>
    <Sidebar>
        <EditEventViewer Placement="Placement.Left" />
    </Sidebar>
    <Content>
        <ComposableGrid Items="@items">
            <Columns>
                <ComposableColumn Property="@(p => p.Name)">
                    <Features>
                        <InlineEditingFeature ShowEvents="true" />
                    </Features>
                </ComposableColumn>
            </Columns>
        </ComposableGrid>
    </Content>
</MainLayout>
```

### Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| **Cascading value over DI** | Stream is grid-scoped, not app-scoped. Multiple grids on one page should have separate streams. |
| **`ShowEvents` on feature, not grid** | Opt-in at column level provides fine-grained control. |
| **Panel placement is manual** | Preserves "host it wherever they like" requirement. |
| **Stream is read-only for panel** | Panel is purely presentational; features are the only publishers. |
| **Event limit (100)** | Prevents unbounded memory growth in long-running sessions. |

---

## Option 5: Grid Auto-Renders Panel (Optional Convenience)

### How It Works

The grid provides both the cascading event stream (like Option 4) **AND** optionally renders a built-in panel if the consumer sets a placement parameter. This provides a "batteries included" experience for simple cases while still allowing advanced consumers to use their own panel (Option 4 pattern).

```razor
<!-- Simple: Grid auto-renders panel when placement is specified -->
<ComposableGrid Items="@items" EventPanelPlacement="Placement.Right">
    <Columns>
        <ComposableColumn Property="@(p => p.Name)">
            <Features>
                <InlineEditingFeature ShowEvents="true" ... />
            </Features>
        </ComposableColumn>
    </Columns>
</ComposableGrid>

<!-- Advanced: Consumer uses custom panel -->
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items">
        <Columns>
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" ... />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <!-- Custom panel component -->
    <MyCustomEventViewer Placement="Placement.Right" />
</div>
```

### Architecture Layers

| Layer | Responsibility |
|-------|----------------|
| **`ComposableGrid`** | Provides cascaded `IEditEventStream`. If `EventPanelPlacement != Placement.None`, auto-renders built-in `<EditEventViewer>`. |
| **`InlineEditingFeature`** | Publishes events when `ShowEvents=true`. |
| **`IEditEventStream`** | Grid-level event aggregation service. |
| **`EditEventViewer`** | Standalone component. Can be auto-rendered by grid OR manually placed by consumer. |

### Pros

| Benefit | Explanation |
|---------|-------------|
| **Progressive disclosure** | Simple cases: set placement parameter. Advanced cases: use custom panel. |
| **Zero boilerplate for demos** | No need to manually place panel for quick prototypes |
| **Still supports full flexibility** | Consumer can ignore `EventPanelPlacement` and place their own panel anywhere |
| **Consistent with `RowColumn` pattern** | `RowColumn` auto-renders its overlay; this is similar |
| **Backward compatible with Option 4** | Grid still provides cascading stream; auto-panel is purely additive |

### Cons

| Drawback | Explanation |
|----------|-------------|
| **Grid gains one parameter** | `EventPanelPlacement` adds to grid API surface (though it's optional) |
| **"Two ways" confusion** | Consumers might not know whether to use the parameter or manual placement |
| **Breaking changes for layout** | Grid needs a wrapper `<div>` to position auto-rendered panel |
| **Less discoverable advanced path** | Consumers might not realize they can use custom panels |

### Implementation Sketch

**Step 1: Grid optionally renders panel based on placement**

```csharp
// ComposableGrid.razor
[Parameter] public Placement EventPanelPlacement { get; set; } = Placement.None;

<CascadingValue Value="@EventStream">
    @if (EventPanelPlacement != Placement.None && EventPanelPlacement != Placement.Left && EventPanelPlacement != Placement.Top)
    {
        <!-- Grid-level wrapper only when panel needs positioning -->
        <div class="composable-grid-with-panel @GetPanelLayoutClass()">
            <div class="composable-grid-main">
                <table class="composable-grid">
                    @ChildContent
                </table>
            </div>
            
            @if (EventPanelPlacement == Placement.Right || EventPanelPlacement == Placement.Bottom)
            {
                <div class="composable-grid-panel">
                    <EditEventViewer Placement="@EventPanelPlacement" />
                </div>
            }
        </div>
    }
    else if (EventPanelPlacement == Placement.Left || EventPanelPlacement == Placement.Top)
    {
        <div class="composable-grid-with-panel @GetPanelLayoutClass()">
            <div class="composable-grid-panel">
                <EditEventViewer Placement="@EventPanelPlacement" />
            </div>
            
            <div class="composable-grid-main">
                <table class="composable-grid">
                    @ChildContent
                </table>
            </div>
        </div>
    }
    else
    {
        <!-- No panel: render grid directly -->
        <table class="composable-grid">
            @ChildContent
        </table>
    }
</CascadingValue>

@code {
    private EditEventStream EventStream { get; } = new();
    
    private string GetPanelLayoutClass() => EventPanelPlacement switch
    {
        Placement.Top or Placement.Bottom => "panel-layout-vertical",
        Placement.Left or Placement.Right => "panel-layout-horizontal",
        _ => ""
    };
    
    public override void Dispose()
    {
        EventStream?.Dispose();
        base.Dispose();
    }
}
```

**Step 2: CSS for auto-rendered panel layout**

```css
/* Grid with panel wrapper */
.composable-grid-with-panel {
    display: flex;
    gap: var(--space-16);
    align-items: flex-start;
}

.composable-grid-with-panel.panel-layout-horizontal {
    flex-direction: row;
}

.composable-grid-with-panel.panel-layout-vertical {
    flex-direction: column;
}

.composable-grid-main {
    flex: 1 1 auto;
    min-width: 0; /* Prevent overflow */
}

.composable-grid-panel {
    flex: 0 0 auto;
}

/* Panel sizing based on placement */
.composable-grid-with-panel.panel-layout-horizontal .composable-grid-panel {
    width: 300px;
    max-height: 500px;
    overflow-y: auto;
}

.composable-grid-with-panel.panel-layout-vertical .composable-grid-panel {
    width: 100%;
    max-height: 200px;
    overflow-y: auto;
}
```

**Step 3: Consumer usage patterns**

```razor
<!-- Pattern 1: Auto-rendered panel (simple) -->
<ComposableGrid Items="@items" EventPanelPlacement="Placement.Right">
    <Columns>
        <ComposableColumn Property="@(p => p.Name)">
            <Features>
                <InlineEditingFeature ShowEvents="true" />
            </Features>
        </ComposableColumn>
    </Columns>
</ComposableGrid>

<!-- Pattern 2: Manual panel (advanced) -->
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items">
        <Columns>
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <MyCustomEventViewer Placement="Placement.Right" />
</div>

<!-- Pattern 3: No panel at all -->
<ComposableGrid Items="@items">
    <Columns>
        <ComposableColumn Property="@(p => p.Name)">
            <Features>
                <InlineEditingFeature ... />  <!-- ShowEvents=false -->
            </Features>
        </ComposableColumn>
    </Columns>
</ComposableGrid>
```

### Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| **Optional wrapper div** | Only render wrapper when `EventPanelPlacement != None`; preserves existing grid DOM structure for most users |
| **Auto-render uses same component** | `<EditEventViewer>` works standalone or auto-rendered; no duplication |
| **Placement enum controls both** | Same enum used for auto-rendering (on grid) and styling hints (on manual panels) |
| **Cascading stream always provided** | Stream is available regardless of whether grid auto-renders panel |
| **Advanced users ignore parameter** | Setting `EventPanelPlacement = None` (default) allows manual panel placement without grid interference |

---

## Analysis Matrix

| Criterion | Option 1 (Inside) | Option 2 (Outside) | Option 3 (Hybrid) | Option 4 (Stream + Manual) | Option 5 (Stream + Auto) |
|-----------|-------------------|--------------------|--------------------|----------------------------|--------------------------|
| **Spec Goal: "Host it wherever they like"** | ? Limited | ? Full flexibility | ? Full flexibility | ? Full flexibility | ? **Full flexibility** |
| **Spec Goal: Follows `RowColumn` pattern** | ? Coupled | ? Separated | ? Separated | ? Separated | ? **Separated + Auto-render** |
| **Risk 6.1: Feature bloat in grid** | ? Adds parameters | ? No grid changes | ?? Adds optional parameters | ? No grid parameters | ?? **+1 optional parameter** |
| **Demo simplicity** | ? Minimal markup | ?? Needs wrapper | ? Simple path available | ?? Needs wrapper | ? **Single tag + placement** |
| **Reusability** | ? Grid-only | ? Anywhere | ? Anywhere | ? Anywhere | ? **Anywhere** |
| **Breaking changes** | ?? Grid wrapper needed | ? None | ?? Grid wrapper needed | ? None | ?? **Wrapper when placement set** |
| **Documentation burden** | ? Single pattern | ?? Multiple layout examples | ? Two patterns to explain | ? Single pattern with examples | ?? **Two paths (simple vs advanced)** |
| **Automatic aggregation** | ? Manual | ? Manual | ? Manual | ? Automatic | ? **Automatic** |
| **Opt-in per column** | ? No | ? No | ? No | ? Yes (`ShowEvents`) | ? **Yes (`ShowEvents`)** |
| **Zero-config for demos** | ? No | ? No | ? No | ? No | ? **Yes (set placement only)** |

---

## Recommendation

Based on the specification's explicit goals and the feature bloat risk analysis:

### **? UPDATED RECOMMENDATION: Option 5 (Grid-Provided Stream + Optional Auto-Panel)**

#### Rationale

1. **Best of All Worlds:** Combines automatic event aggregation (Option 4) with optional convenience rendering (Option 1)
2. **Progressive Disclosure:** 
   - Simple demos: `<ComposableGrid EventPanelPlacement="Placement.Right">` ? Done
   - Advanced layouts: Ignore placement parameter, manually place custom panel ? Still works
3. **Aligns with `RowColumn` Precedent:** `RowColumn` auto-renders its overlay based on state; this follows the same pattern
4. **Minimal Grid Bloat:** Only +1 optional parameter (`EventPanelPlacement`)
5. **No Breaking Changes (when not used):** Default `Placement.None` means existing grids render unchanged
6. **Satisfies All Spec Requirements:**
   - ? "Host it wherever they like" – Manual placement still fully supported
   - ? "Purely presentational" – Panel consumes read-only stream
   - ? "Simple placement options" – Placement enum controls both auto-render and styling

#### Why This Beats Option 4

| Advantage | Explanation |
|-----------|-------------|
| **Zero boilerplate for demos** | Option 4 requires wrapper `<div>` + manual `<EditEventViewer>`. Option 5: just add `EventPanelPlacement="Right"` |
| **Still supports advanced cases** | Option 4's manual panel pattern still works—just ignore the placement parameter |
| **Consistent with existing patterns** | `RowColumn` auto-renders overlays; developers already understand this model |
| **Better DX for prototypes** | Getting started is faster; complexity is opt-in |

#### Trade-offs Acknowledged

| Trade-off | Mitigation |
|-----------|------------|
| **+1 Grid Parameter** | Optional parameter with sensible default (`None`). Only used when convenience is desired. |
| **Wrapper `<div>` when placement set** | Only rendered when `EventPanelPlacement != None`. Existing grids unaffected. Provide migration guide if CSS breaks. |
| **Two usage patterns to document** | Clear guidance: "Use placement parameter for quick demos; manually place panel for custom layouts or global sidebars." |

#### Implementation Plan

1. **Get stakeholder approval** on Option 5 (Grid-Provided Stream + Optional Auto-Panel) recommendation
2. **Update Task B2.1 in `InlineEditingPolish.md`** with the decision
3. **Create event contracts** (Task A2.1):
   - `IEditEventStream` interface
   - `EditEventStream` implementation
   - `EditEventBase` and derived event types (`EditCommittedEvent`, `EditCancelledEvent`, `ValidationFailedEvent`)
4. **Update `ComposableGrid`** to:
   - Provide event stream via cascading value
   - Add `EventPanelPlacement` parameter
   - Conditionally render wrapper + auto-panel based on placement
5. **Add `ShowEvents` parameter** to `InlineEditingFeature` (Task A2.2 implementation)
6. **Create `Placement.cs`** enum (Task B2.1.1)
7. **Build `EditEventViewer` component** (Task B3.1) – works both auto-rendered and standalone
8. **Document layout patterns** in Task B2.2 deliverable:
   - CSS for auto-rendered panel layouts (already built into grid)
   - CSS utilities for manual panel placement
9. **Update demo** (`ComposableColumnDemo.razor`) to showcase both patterns (Task B1.1, B1.2):
   - Simple example using `EventPanelPlacement`
   - Advanced example with manual panel placement
10. **Document the pattern** in Task B4.1 with usage guidelines:
    - When to use auto-rendering (demos, prototypes)
    - When to manually place (global sidebars, custom layouts)
    - How to create custom event viewers

---

## Placement Enum Definition

**File:** `QuickGridTest01/ComposableColumns/Features/Editing/Placement.cs`

```csharp
namespace QuickGridTest01.ComposableColumns.Features.Editing;

/// <summary>
/// Indicates the intended visual placement of an event/validation panel 
/// relative to the grid. Used for:
/// - Controlling auto-rendered panel position when set on ComposableGrid.EventPanelPlacement
/// - Providing styling hints for manually-placed panels (border direction, scroll behavior)
/// </summary>
public enum Placement
{
    /// <summary>
    /// No placement specified. 
    /// When set on grid: panel is not auto-rendered.
    /// When set on component: component uses default styling.
    /// </summary>
    None,
    
    /// <summary>Panel is above the grid.</summary>
    Top,
    
    /// <summary>Panel is to the right of the grid.</summary>
    Right,
    
    /// <summary>Panel is below the grid.</summary>
    Bottom,
    
    /// <summary>Panel is to the left of the grid.</summary>
    Left
}
```

### Design Notes

- **`None` is default:** Existing grids unaffected; panel auto-rendering is opt-in
- **Dual-purpose enum:** Controls both auto-rendering (on grid) and styling hints (on manual panels)
- **No `Overlay` value (yet):** Floating/absolute positioning can be added later if needed
- **Future-proof:** Can extend with values like `FloatingTopRight`, `Modal`, etc. without breaking changes

---

## Open Questions

| Question | Priority | Notes |
|----------|----------|-------|
| Should `Placement` support `Overlay` (floating/absolute positioned)? | Medium | Could be added in a future iteration if floating panels are needed |
| Should the shell component accept a `MaxHeight` parameter or rely purely on CSS? | Low | CSS-only keeps component simpler; consumers can override |
| Do we need responsive behavior (e.g., `Right` on desktop ? `Bottom` on mobile)? | Medium | Could use media queries in CSS or add a `ResponsivePlacement` parameter |
| Should we provide a `<GridWithPanel>` wrapper component for common layouts? | Low | Nice-to-have; Option 5 auto-render reduces need for this |
| What happens if consumer sets `EventPanelPlacement` AND manually places a panel? | Medium | Document behavior: both will render (cascading stream feeds both). May want warning in dev mode. |

---

## Next Steps

1. **Get stakeholder approval** on Option 5 (Grid-Provided Stream + Optional Auto-Panel) recommendation
2. **Update Task B2.1 in `InlineEditingPolish.md`** with the decision
3. **Create event contracts** (Task A2.1):
   - `IEditEventStream` interface
   - `EditEventStream` implementation
   - `EditEventBase` and derived event types (`EditCommittedEvent`, `EditCancelledEvent`, `ValidationFailedEvent`)
4. **Update `ComposableGrid`** to:
   - Provide event stream via cascading value
   - Add `EventPanelPlacement` parameter
   - Conditionally render wrapper + auto-panel based on placement
5. **Add `ShowEvents` parameter** to `InlineEditingFeature` (Task A2.2 implementation)
6. **Create `Placement.cs`** enum (Task B2.1.1)
7. **Build `EditEventViewer` component** (Task B3.1) – works both auto-rendered and standalone
8. **Document layout patterns** in Task B2.2 deliverable:
   - CSS for auto-rendered panel layouts (already built into grid)
   - CSS utilities for manual panel placement
9. **Update demo** (`ComposableColumnDemo.razor`) to showcase both patterns (Task B1.1, B1.2):
   - Simple example using `EventPanelPlacement`
   - Advanced example with manual panel placement
10. **Document the pattern** in Task B4.1 with usage guidelines:
    - When to use auto-rendering (demos, prototypes)
    - When to manually place (global sidebars, custom layouts)
    - How to create custom event viewers

---

## Alternatives Considered and Rejected

### Why Not Option 1?

- Violates "host it wherever they like" requirement
- Adds parameters to `ComposableGrid` (bloat risk)
- Limited to grid-adjacent placement
- No automatic event aggregation

### Why Not Option 2?

- Requires manual event stream creation and wiring
- No automatic aggregation across multiple features
- Consumer must manage stream lifecycle
- More boilerplate code for every demo

### Why Not Option 3?

- Provides two solutions to the same problem (confusing)
- Still adds optional parameters to grid (partial bloat)
- Documentation must explain when to use each approach
- YAGNI principle: no evidence consumers need both patterns
- Implementation complexity maintaining two code paths

### Why Not Option 4?

- **Almost perfect**, but requires manual panel placement even for simple demos
- Developers requested a convenience path: "Can the grid just render the panel for me?"
- Doesn't leverage `RowColumn` precedent of auto-rendering overlays
- Option 5 is a strict superset: everything Option 4 does + optional auto-rendering

### Why Option 5 Was Chosen

**Option 5 is Option 4 + optional convenience rendering:**

- ? Grid provides infrastructure (like Option 1 & 4) but via cascading value (minimal bloat)
- ? Panel has full placement flexibility (like Option 2 & 4)
- ? Simple pattern for demos (like Option 3's "simple path")
- ? Automatic event aggregation (like Option 4)
- ? Fine-grained opt-in control via `ShowEvents` (like Option 4)
- ? **NEW:** Zero-config panel rendering for prototypes (unique to Option 5)
- ? **NEW:** Aligns with `RowColumn` auto-rendering pattern (consistent with codebase)

**It addresses every design principle from the spec:**

| Spec Principle | How Option 5 Satisfies |
|----------------|------------------------|
| **"Purely presentational"** | Panel is read-only consumer; features are publishers |
| **"Host it wherever they like"** | Manual placement fully supported; auto-render is optional |
| **"Separation of concerns"** | Grid=infrastructure, Features=publishers, Consumer=presentation (or auto-render) |
| **"Simple placement options"** | `Placement` enum controls both auto-render positioning and manual panel styling |

**Progressive Disclosure in Action:**

```razor
<!-- Day 1: Prototype (auto-render) -->
<ComposableGrid EventPanelPlacement="Right">...</ComposableGrid>

<!-- Day 30: Production (manual placement in global sidebar) -->
<Layout>
    <Sidebar><EditEventViewer /></Sidebar>
    <Main><ComposableGrid>...</ComposableGrid></Main>
</Layout>
```

---

## References

- `InlineEditingPolish.md` Section 4, Goal 3
- `InlineEditingPolish.md` Section 6.1, Feature Bloat analysis
- Existing pattern: `RowColumn.cs` (lines 263-277) – auto-renders overlay in `CellContent`
- Existing pattern: `RowColumnDemo.razor` (lines 162-176) – `ExpandedTemplate` rendered by column
- Existing pattern: `EditableColumnDemo.razor` (lines 65-76) – manual event log placement
