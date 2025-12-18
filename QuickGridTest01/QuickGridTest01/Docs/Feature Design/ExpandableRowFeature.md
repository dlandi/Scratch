# RowExpandFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.4 (Critical Feedback Integrated) |
| Status | Design |
| Created | 2025 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Expansion` |
| Source | Ported from `QuickGridTest01.RowColumn.RowColumn<TGridItem>` and `Pages/RowColumnDemo.razor` |
| Styling | **All CSS for this feature must be placed in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (no `*.razor.css` for feature styling).** |
| Namespace rule | **All logic pertaining to an `IColumnFeature` must live under the `QuickGridTest01.ComposableColumns` namespace (and its sub-namespaces).** |

---

## 1. Overview

### 1.1 Purpose

`RowExpandFeature<TGridItem>` is the **base feature** for row-level expansion within the ComposableColumns architecture.

It provides:
- A rendered cell surface that can display a trigger UI (button, row-click indicator, or custom content)
- An **expanded overlay** that visually spans `ExpandedRowSpan` rows while still rendering inside a single column cell
- Optional **spacer row injection** to push real data rows below the overlay so they remain visible
- A shared, memory-efficient expansion state store consumable by other features (e.g., FormRow, NestedGrid)

### 1.2 Role in Architecture

This feature is the foundation for:
- **FormRowFeature** - Adds form semantics (draft state, validation, save/cancel)
- **NestedGridFeature** - Adds child grid rendering for master-detail patterns

```
RowExpandFeature<TGridItem>                         (Base: trigger + expansion state + overlay)
    ├── FormRowFeature<TGridItem>                   (Adds: draft model + validation + save/cancel)
    └── NestedGridFeature<TGridItem, TChildItem>    (Adds: child grid + selection + load)
```

### 1.3 Migration Context

This feature is a port of the standalone `RowColumn<TGridItem>` component into the `IColumnFeature<TGridItem>` model.

**Namespace rule impact:** All ported types that exist to support `RowExpandFeature` (state manager, event args, RowCard, data source wrappers) must be moved under the `QuickGridTest01.ComposableColumns` namespace tree.

**Key Migration Points (validated from RowColumn implementation):**

| Aspect | RowColumn (Original) | RowExpandFeature (Target) |
|--------|-----------------------|---------------------------|
| Host type | QuickGrid `ColumnBase<TGridItem>` | ComposableColumns `ICellRenderFeature<TGridItem>` |
| State store | Private `_stateManager` per column instance | `RowStateManager<TGridItem>` registered in `FeatureContext` |
| Row identity | `IRowIdentifiable` (`Id`) | `FeatureContext.RowKey` (required for correctness) |
| Spacer rows | `ExpandableGridDataSource<TGridItem>` + encoded negative IDs | Same mechanism, but must be reconciled with `RowKey` (see §5/§12) |
| Re-render | `InvokeAsync(StateHasChanged)` | `context.RequestRefreshAsync()` (preferred) |

---

## 2. Architecture

### 2.1 Interface & Priority

```csharp
public sealed class RowExpandFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class
{
    public int Priority => FeaturePriority.Expansion;
}
```

**Priority rationale:** Expansion should run after general styling features (so it can add dimming/expanded classes) but before editing features (so edit UI can respect expanded state).

> Add `FeaturePriority.Expansion = 350` between `Styling (300)` and `Editing (400)`.

### 2.2 Key Design Decisions (updated)

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Row identification | **`FeatureContext.RowKey` required** | `GetHashCode`/reference identity is unstable across requery/virtualization; stable keys are required for correct expand/collapse |
| State storage | `RowStateManager<TGridItem>` + `ConditionalWeakTable` | Matches `RowColumn` and avoids memory leaks in long-lived grids |
| Overlay rendering | Absolute positioned overlay element inside a `position:relative` wrapper | Identical to `RowColumn` behavior; keeps feature scoped to cell rendering pipeline |
| Spacer injection (optional) | `ExpandableGridDataSource<TGridItem>` | Matches demo: provides visual correctness by pushing rows down |
| Cascading context | Cascade only in expanded mode | Keeps child component contract minimal (only visible when expanded) |
| Styling | Global stylesheet only | This repo's convention requires all feature CSS in `wwwroot/css/qgComposable-refined-minimalism.css` |
| Feature logic location | ComposableColumns only | All `IColumnFeature` logic must live under `QuickGridTest01.ComposableColumns.*` |

### 2.3 Feature Responsibilities vs Grid Responsibilities

`RowExpandFeature` is purely a **cell render feature**. It cannot directly change the grid's `Items` pipeline unless the grid or an upstream feature provides an injectable data source.

Therefore:
- Expansion overlay rendering is always supported
- Spacer row injection is supported **only when the grid is bound to an `ExpandableGridDataSource<TGridItem>`** or an equivalent upstream injection mechanism exists

---

## 3. Parameters (runtime behavior)

> Parameters mirror `RowColumn<TGridItem>`.

### 3.1 Trigger & Behavior

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TriggerMode` | `RowTriggerMode` | `Button` | Trigger strategy: Button, RowClick indicator, or Custom template |
| `ConcurrentBehavior` | `ConcurrentExpandBehavior` | `Block` | What to do if another row is expanded |
| `DimInactiveRows` | `bool` | `true` | When true, non-expanded rows are dimmed and non-interactive |

**ConcurrentExpandBehavior semantics (as implemented in RowColumn):**

| Value | Meaning |
|-------|---------|
| `Block` | If any row is expanded, disallow expanding a different row |
| `CollapseCurrent` | Collapse the first expanded row before expanding a new one |
| `AllowMultiple` | Multiple rows may be expanded at once |

### 3.2 Row Span & Height

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ExpandedRowSpan` | `int` | `3` | Logical number of *row heights* the overlay should cover |
| `RowHeight` | `int` | `48` | Height of a single grid row in pixels |

**Computed:**

```csharp
public int ExpandedHeight => ExpandedRowSpan * RowHeight;
```

**Virtualization constraint:** Spacer rows preserve uniform height so virtualization row measurement remains consistent.

### 3.3 Templates

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `DisplayTemplate` | `RenderFragment<RowDisplayContext<TGridItem>>?` | No | Content when collapsed |
| `ExpandedTemplate` | `RenderFragment<RowExpandedContext<TGridItem>>` | **Yes** | Content when expanded |

Fallback behavior (from RowColumn):
- `Button`: default button when `DisplayTemplate` is null
- `RowClick`: default chevron indicator when `DisplayTemplate` is null
- `Custom`: expects `DisplayTemplate` to provide trigger

### 3.4 Button Customization

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ExpandButtonText` | `string` | `"Edit"` | Button label |
| `ExpandButtonClass` | `string` | `"qg-btn qg-btn-secondary qg-btn-sm"` | Button CSS class |
| `ExpandButtonIcon` | `string?` | `"bi bi-pencil"` | Icon CSS class; demo uses Bootstrap Icons |

### 3.5 Events

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnBeforeExpand` | `EventCallback<RowBeforeExpandEventArgs<TGridItem>>` | Cancellable pre-expand |
| `OnExpanded` | `EventCallback<RowExpandedEventArgs<TGridItem>>` | Fired after expansion |
| `OnCollapsed` | `EventCallback<RowCollapsedEventArgs<TGridItem>>` | Fired after collapse |
| `OnStateChanged` | `EventCallback<RowStateChangedEventArgs<TGridItem>>` | Fired on any state change |

**Event ordering (from RowColumn):**
1. `OnBeforeExpand` (cancellable)
2. Potentially collapse another row (if `CollapseCurrent`)
3. State transition event (`OnStateChanged: Collapsed -> Expanded`)
4. `OnExpanded`

Collapse:
1. State transition event (`OnStateChanged: Expanded -> Collapsed`)
2. `OnCollapsed`

### 3.6 Cancellation Support

All async expand/collapse operations accept an optional `CancellationToken`:

| Method | Signature | Description |
|--------|-----------|-------------|
| `ExpandRowAsync` | `Task ExpandRowAsync(TGridItem item, CancellationToken cancellationToken = default)` | Expand with cancellation support |
| `CollapseRowAsync` | `Task CollapseRowAsync(TGridItem item, CancellationToken cancellationToken = default)` | Collapse with cancellation support |
| `CollapseAllAsync` | `Task CollapseAllAsync(CancellationToken cancellationToken = default)` | Collapse all with cancellation support |

**Cancellation semantics:**
- If cancelled before state change: operation aborts, no events fire
- If cancelled during event callback: `OperationCanceledException` propagates to caller
- Grid refresh still occurs if state was mutated before cancellation

---

## 4. Context Objects

### 4.1 RowDisplayContext

```csharp
public class RowDisplayContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public bool IsAnyRowExpanded { get; init; }
    public bool CanExpand { get; init; }
    
    /// <summary>
    /// Expands the row. Accepts optional cancellation token.
    /// </summary>
    public Func<CancellationToken, Task> ExpandAsync { get; init; } = default!;
    
    /// <summary>
    /// Convenience overload without cancellation token.
    /// </summary>
    public Task ExpandAsync() => ExpandAsync(CancellationToken.None);
}
```

### 4.2 RowExpandedContext

```csharp
public class RowExpandedContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    
    /// <summary>
    /// Collapses the row. Accepts optional cancellation token.
    /// </summary>
    public Func<CancellationToken, Task> CollapseAsync { get; init; } = default!;
    
    /// <summary>
    /// Convenience overload without cancellation token.
    /// </summary>
    public Task CollapseAsync() => CollapseAsync(CancellationToken.None);
}
```

**Cascading contract:** In expanded mode, the feature cascades `RowExpandedContext<TGridItem>` so nested components (forms, nested grids) can invoke `CollapseAsync` without explicit parameter plumbing.

---

## 5. Row Identity & Spacer Row Relationship

### 5.1 RowKey is mandatory

`RowColumn` relied on `IRowIdentifiable.Id` (stable integer key). In ComposableColumns we will key expansion by:

```csharp
context.RowKey!(item)
```

**Design requirement:** `ComposableGrid` must set `RowKey` for any expansion feature usage.

### 5.2 Spacer rows require a representable row item

The existing `ExpandableGridDataSource<T>` injects spacer rows as *instances of `T`* with encoded negative IDs.

That implies one of:
- `TGridItem` must implement `IRowIdentifiable` (as in `RowColumn`)
- or a new spacer representation strategy must be created for ComposableGrid (not in scope for the initial port)

**Current decision:** keep the existing spacer strategy for the initial port.

Implications:
- If you want spacer rows, constrain usage to item types that can represent spacer rows (typically `IRowIdentifiable, new()`)
- If you only want overlay (no spacer injection), any class type can work (but overlay may cover rows)

---

## 6. Spacer Row Data Model

### 6.1 SpacerRowFactory encoding

Spacer IDs are negative and materialize the parent relationship:

`spacerId = -(parentId * 1000 + offset)`

Where:
- `parentId` is the real row Id
- `offset` is 1..N (spacer index)

This ensures:
- Spacer rows never collide with real rows (assuming real IDs are positive)
- Parent row can be derived from spacer row in O(1)

### 6.2 ExpandableGridDataSource semantics

`ExpandableGridDataSource<T>` wraps a source list and injects spacer rows after any expanded item.

Important detail from implementation:
- Stored spacers = `ExpandedRowSpan + 1`
- That +1 accounts for overlay positioning at the row boundary so the next real data row appears below the overlay.

---

## 7. Rendering Pipeline

### 7.1 Cell wrapper & state classes

`RowColumn` wraps every non-spacer cell in a div and adds classes:
- `row-expanded` when expanded
- `row-dimmed` when another row is expanded and `DimInactiveRows == true`

The feature should do the same and then either render expanded overlay or trigger UI.

### 7.2 RenderCell outline (corrected sequencing)

**`renderNext` invocation strategy:** The expansion feature **replaces** the default cell content entirely when rendering—it does not wrap content from downstream features. Therefore, `renderNext` is intentionally **not called** in the expansion column.

**Rationale:**
- The expansion column is a dedicated control column (trigger button or expanded overlay)
- There is no meaningful "inner content" to delegate to downstream features
- Calling `renderNext` would render property values that don't belong in this column

**Alternative designs considered:**
- If future requirements need expansion as a *wrapper* around existing content, a separate `RowExpandWrapperFeature` could be created that calls `renderNext` inside the wrapper div

```csharp
public void RenderCell(
    RenderTreeBuilder builder,
    ref int sequence,
    TGridItem item,
    FeatureContext<TGridItem> context,
    Action renderNext)
{
    // Skip spacer rows - render empty cell
    if (IsSpacerRow(item))
    {
        RenderSpacerCell(builder, ref sequence);
        return; // No renderNext - spacer cells are intentionally empty
    }
    
    var isExpanded = _stateManager.IsRowExpanded(context.RowKey!(item));
    var hasAnyExpanded = _stateManager.HasExpandedRows;
    
    // Wrapper div with state classes
    builder.OpenElement(sequence++, "div");
    builder.AddAttribute(sequence++, "class", BuildCellClass(isExpanded, hasAnyExpanded));
    
    if (isExpanded)
        RenderExpandedMode(builder, ref sequence, item, context);
    else
        RenderDisplayMode(builder, ref sequence, item, context, hasAnyExpanded);
    
    builder.CloseElement();
    
    // NOTE: renderNext is intentionally NOT called - this feature replaces cell content
}
```

**Implementation note:** Use the provided `sequence` ref consistently; do not introduce a separate counter (the render pipeline assumes monotonic sequences).

### 7.3 Display Mode Rendering

```csharp
private void RenderDisplayMode(RenderTreeBuilder builder, ref int seq, 
    TGridItem item, bool hasAnyExpanded)
{
    var canExpand = CanExpandRow(hasAnyExpanded);
    var displayContext = new RowDisplayContext<TGridItem>
    {
        Item = item,
        IsAnyRowExpanded = hasAnyExpanded,
        CanExpand = canExpand,
        ExpandAsync = () => ExpandRowAsync(item)
    };

    if (DisplayTemplate != null)
    {
        builder.AddContent(seq++, DisplayTemplate(displayContext));
    }
    else if (TriggerMode == RowTriggerMode.Button)
    {
        RenderDefaultExpandButton(builder, ref seq, displayContext);
    }
    else if (TriggerMode == RowTriggerMode.RowClick)
    {
        RenderRowClickIndicator(builder, ref seq, displayContext);
    }
}
```

### 7.4 Expanded Mode Rendering

```csharp
private void RenderExpandedMode(RenderTreeBuilder builder, ref int seq, 
    TGridItem item, RowExpandedContext<TGridItem> context)
{
    // Overlay container with calculated height
    builder.OpenElement(seq++, "div");
    builder.AddAttribute(seq++, "class", "row-overlay");
    builder.AddAttribute(seq++, "style", $"height: {ExpandedHeight}px;");

    // Cascade context to child components
    builder.OpenComponent<CascadingValue<RowExpandedContext<TGridItem>>>(seq++);
    builder.AddComponentParameter(seq++, "Value", context);
    builder.AddComponentParameter(seq++, "ChildContent", ExpandedTemplate!(context));
    builder.CloseComponent();

    builder.CloseElement();
}
```

### 7.5 CSS Classes

```csharp
private string BuildCellClass(bool isExpanded, bool hasAnyExpanded)
{
    var classes = new List<string> { "row-cell" };

    if (isExpanded)
        classes.Add("row-expanded");
    else if (hasAnyExpanded && DimInactiveRows)
        classes.Add("row-dimmed");

    return string.Join(" ", classes);
}
```

### 7.6 CSS Overlay Strategy

**Styling location rule:** The selectors for this feature (e.g., `.row-cell`, `.row-overlay`, `.row-dimmed`, spacer row selectors) must live in the global stylesheet:

- `QuickGridTest01/wwwroot/css/qgComposable-refined-minimalism.css`

Do not use `*.razor.css` for this feature.

**Reference selectors (defined in the global stylesheet):**
- `.row-cell`, `.row-cell.row-expanded`, `.row-cell.row-dimmed`
- `.row-overlay`
- `.row-click-indicator`
- `.row-card*`
- spacer-row selectors (e.g., `.row-spacer`, `.row-spacer-row`, `tr:has(.row-spacer)`)

### 7.7 Animation & Transition Guidance

**CSS Transitions (recommended approach):**

The feature supports CSS-based animations via class transitions. No JavaScript interop is required for basic animations.

**Recommended transition selectors in global stylesheet:**

```css
/* Fade transition for dimming inactive rows */
.row-cell {
    transition: opacity 0.2s ease-in-out;
}

.row-cell.row-dimmed {
    opacity: 0.5;
    pointer-events: none;
}

/* Overlay height transition (optional - may conflict with virtualization) */
.row-overlay {
    transition: height 0.3s ease-out;
    overflow: hidden;
}

/* Expand button icon rotation */
.row-click-indicator {
    transition: transform 0.2s ease;
}

.row-expanded .row-click-indicator {
    transform: rotate(90deg);
}
```

**Height animation constraints:**

| Scenario | Animation Support |
|----------|-------------------|
| Fixed `ExpandedRowSpan` | ✅ CSS `height` transition works |
| Dynamic content height | ⚠️ Requires JS measurement; consider `max-height` workaround |
| Virtualized grid | ⚠️ Height transitions may cause scroll jumps; disable or use opacity only |

**No JavaScript interop required for:**
- Opacity/fade transitions
- Transform animations (rotate, scale)
- Fixed-height transitions

**JavaScript interop recommended for:**
- Measuring dynamic content height before animation
- Smooth scroll-to-expanded-row behavior
- Complex choreographed animations

**Implementation note:** If JavaScript interop is added later, it should be optional and the feature must function correctly with CSS-only animations as the default.

---

## 8. Expand/Collapse State Machine

### 8.1 Minimal states

`RowExpandedState`:
- `Collapsed`
- `Expanded`

The state manager tracks:
- A set of expanded row keys
- A `RowExpandedContext<TGridItem>` per expanded row

### 8.2 Concurrency behavior

`RowColumn` collapses only the *first expanded row* in `CollapseCurrent` mode. The port should match that behavior for predictability.

### 8.3 Refresh behavior

In `RowColumn`, expand/collapse ends with `InvokeAsync(StateHasChanged)`.

In `RowExpandFeature`, the equivalent is:
- Prefer `context.RequestRefreshAsync?.Invoke()`
- If not present, fall back to `context.RequestRefresh?.Invoke()`

This keeps the feature host-agnostic (works in components that expose sync or async refresh pathways).

### 8.4 RowStateManager Key-Based Identity (Migration Requirement)

**Issue:** The existing `RowStateManager<TGridItem>` uses `HashSet<TGridItem>` with **reference equality**. This is incompatible with the `FeatureContext.RowKey` requirement for stable identity.

**Required change for ComposableColumns port:**

The ported `RowStateManager` must:
1. Accept a `Func<TGridItem, object> keySelector` in its constructor
2. Use `Dictionary<object, RowExpandedContext<TGridItem>>` instead of `HashSet<TGridItem>`
3. Key all operations by `keySelector(item)` rather than item reference

```csharp
public class RowStateManager<TGridItem> : IDisposable where TGridItem : class
{
    private readonly Func<TGridItem, object> _keySelector;
    private readonly Dictionary<object, RowExpandedContext<TGridItem>> _expandedRows = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    public RowStateManager(Func<TGridItem, object> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }
    
    public bool IsRowExpanded(TGridItem item) => _expandedRows.ContainsKey(_keySelector(item));
    public bool IsRowExpanded(object key) => _expandedRows.ContainsKey(key);
    public bool HasExpandedRows => _expandedRows.Count > 0;
    
    // ... rest of implementation keyed by _keySelector(item)
}
```

**Initialization:** The feature must pass `context.RowKey!` to the state manager constructor during initialization.

### 8.5 Error Handling Strategy

The feature must handle errors gracefully to prevent grid corruption:

| Scenario | Behavior |
|----------|----------|
| `ExpandedTemplate` throws during render | Catch exception, render error placeholder, log to console, do not crash grid |
| `OnBeforeExpand` callback throws | Propagate exception to caller, do not mutate state |
| `OnExpanded`/`OnCollapsed` callback throws | Log warning, state already mutated (cannot rollback), continue with refresh |
| `RowKey` returns null | Throw `InvalidOperationException` with clear message |
| `context.RowKey` is null | Throw `InvalidOperationException("RowKey must be configured for expansion features")` |

**Error placeholder rendering:**

```csharp
private void RenderErrorPlaceholder(RenderTreeBuilder builder, ref int seq, Exception ex)
{
    builder.OpenElement(seq++, "div");
    builder.AddAttribute(seq++, "class", "row-expand-error");
    builder.AddContent(seq++, "Error rendering expanded content");
    builder.CloseElement();
    
    // Log to browser console in development
    Console.Error.WriteLine($"[RowExpandFeature] Render error: {ex.Message}");
}
```

### 8.6 Thread Safety Considerations

**Issue:** The existing `RowStateManager` uses `SemaphoreSlim` for async methods but `IsRowExpanded()` and `HasExpandedRows` are synchronous and not thread-safe.

**Design decision for ComposableColumns port:**

Since Blazor Server uses a single-threaded synchronization context per circuit:
1. **Read operations** (`IsRowExpanded`, `HasExpandedRows`) do not require locking during render
2. **Write operations** (`ExpandRow`, `CollapseRow`) must be async and use `SemaphoreSlim`
3. **Cross-circuit safety** is not a concern (each circuit has its own component tree)

**Implementation guidance:**

```csharp
// Safe - called from render thread
public bool IsRowExpanded(object key) => _expandedRows.ContainsKey(key);
public bool HasExpandedRows => _expandedRows.Count > 0;

// Must be async with locking - mutates state
public async Task ExpandRowAsync(TGridItem item, CancellationToken ct = default)
{
    await _lock.WaitAsync(ct);
    try
    {
        var key = _keySelector(item);
        if (!_expandedRows.ContainsKey(key))
        {
            _expandedRows[key] = CreateContext(item);
        }
    }
    finally
    {
        _lock.Release();
    }
}
```

**Warning:** If the feature is ever used in Blazor WebAssembly with Web Workers or in multi-threaded WASM scenarios, additional synchronization may be required.

---

## 9. RowCard Component

### 9.1 Ownership & Packaging

`RowCard` is **part of the expansion feature package** but its internal definition is **user-customizable**.

| Aspect | Decision |
|--------|----------|
| Shipped with feature | Yes - provides sensible default |
| User can replace | Yes - via `ExpandedTemplate` parameter |
| Internal structure customizable | Yes - via `RowCard` parameters and slots |

**Namespace location:** `QuickGridTest01.ComposableColumns.Features.Expansion.Components.RowCard`

### 9.2 Default RowCard Structure

The default `RowCard` provides a consistent card-like UI for expanded content:

```razor
@typeparam TGridItem where TGridItem : class

<div class="row-card">
    <div class="row-card-header">
        @if (HeaderTemplate != null)
        {
            @HeaderTemplate(Item)
        }
        else
        {
            <span class="row-card-title">@Title</span>
        }
        <button class="row-card-close" @onclick="CloseAsync">
            <i class="bi bi-x-lg"></i>
        </button>
    </div>
    <div class="row-card-body">
        @ChildContent
    </div>
    @if (FooterTemplate != null)
    {
        <div class="row-card-footer">
            @FooterTemplate(Item)
        </div>
    }
</div>

@code {
    [Parameter] public TGridItem Item { get; set; } = default!;
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment<TGridItem>? HeaderTemplate { get; set; }
    [Parameter] public RenderFragment<TGridItem>? FooterTemplate { get; set; }
    [CascadingParameter] public RowExpandedContext<TGridItem>? Context { get; set; }
    
    private Task CloseAsync() => Context?.CollapseAsync() ?? Task.CompletedTask;
}
```

### 9.3 Customization Patterns

**Pattern 1: Use default RowCard with slots**

```razor
<ComposableColumn TGridItem="Employee">
    <RowExpandFeature TGridItem="Employee" 
                      ExpandedTemplate="@(ctx => 
        @<RowCard Item="ctx.Item" Title="Edit Employee">
            <EmployeeEditForm Employee="ctx.Item" />
            <FooterTemplate>
                <button @onclick="() => SaveAsync(ctx.Item)">Save</button>
            </FooterTemplate>
        </RowCard>)" />
</ComposableColumn>
```

**Pattern 2: Completely custom expanded content (no RowCard)**

```razor
<RowExpandFeature TGridItem="Employee" 
                  ExpandedTemplate="@(ctx => 
    @<div class="my-custom-expand-ui">
        <h3>@ctx.Item.Name</h3>
        <button @onclick="ctx.CollapseAsync">Close</button>
    </div>)" />
```

**Pattern 3: Application-specific RowCard replacement**

Users can create their own `MyAppRowCard` component and use it instead of the default.

### 9.4 Styling Rules

**All RowCard CSS must be in the global stylesheet:**

`wwwroot/css/qgComposable-refined-minimalism.css`

**Do not create:** `RowCard.razor.css`

**Required CSS selectors:**
- `.row-card`
- `.row-card-header`, `.row-card-title`, `.row-card-close`
- `.row-card-body`
- `.row-card-footer`

---