# RowExpandFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.3 (Refined) |
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

---

## 4. Context Objects

### 4.1 RowDisplayContext

```csharp
public class RowDisplayContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public bool IsAnyRowExpanded { get; init; }
    public bool CanExpand { get; init; }
    public Func<Task> ExpandAsync { get; init; } = default!;
}
```

### 4.2 RowExpandedContext

```csharp
public class RowExpandedContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public Func<Task> CollapseAsync { get; init; } = default!;
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
        return;
    }
    
    var isExpanded = _stateManager.IsRowExpanded(item);
    var hasAnyExpanded = _stateManager.HasExpandedRows;
    
    // Wrapper div with state classes
    builder.OpenElement(sequence, "div");
    builder.AddAttribute(sequence++, "class", BuildCellClass(isExpanded, hasAnyExpanded));
    
    if (isExpanded)
        RenderExpandedMode(builder, ref sequence, item);
    else
        RenderDisplayMode(builder, ref sequence, item, hasAnyExpanded);
    
    builder.CloseElement();
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

---

## 9. RowCard (demo-aligned)

`RowColumnDemo` uses `RowCard` as a convenient overlay content pattern.

`RowCard` is optional infrastructure: the expansion feature should not depend on it.

**Namespace rule impact:** If `RowCard` is shipped as part of the feature package, its component and any supporting code must live under `QuickGridTest01.ComposableColumns.*`.

**Styling location rule:** If `RowCard` styling is part of the expansion feature UX, its CSS must also live in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (not in a `RowCard.razor.css`).

---

## 10. Usage Patterns

### 10.1 Overlay-only (no spacer injection)

Use when:
- The expanded UI is small
- You don't mind covered rows
- Your item type cannot represent spacers

### 10.2 Overlay + spacer injection (matches demo)

Use when:
- Expanded UI is card-like and should not cover subsequent rows
- Your item type supports spacer rows (`IRowIdentifiable, new()` pattern)
- Your grid is bound to the injected `ExpandableGridDataSource<T>.Items`

---

## 11. Resolved / Updated Open Questions

### 11.1 Spacer row ownership

**Decision:** Keep `ExpandableGridDataSource<T>` as an explicit collaboration object. The feature triggers expand/collapse on it, but the grid owns it.

This matches the demo:
- Grid binds to `DataSource.Items`
- Column/feature calls `DataSource.ExpandRow` and `DataSource.CollapseRow`

### 11.2 RowKey fallback

**Decision:** Throw if `context.RowKey` is missing.

Rationale:
- `GetHashCode`/reference identity is not stable for grid refreshes and can break collapse behavior
- A stable key is necessary for predictable behavior

### 11.3 Multiple expansion features

**Decision:** Prevent multiple expansion features from attaching to the same column by using the context service registration as a sentinel.

---

## 12. Porting Checklist

1. Add `FeaturePriority.Expansion = 350`.
2. Port `RowTriggerMode`, `ConcurrentExpandBehavior`, `RowExpandedState`.
3. Port `RowStateManager<T>` and event args.
4. Move all feature-supporting types under `QuickGridTest01.ComposableColumns.*` namespaces.
5. Implement `RowExpandFeature<T>` using `FeatureContext` for:
   - `RowKey`
   - `RequestRefresh/RequestRefreshAsync`
   - `InvokeAsync` for any off-thread continuations
6. Decide spacer strategy for non-`IRowIdentifiable` types (defer; keep existing for v1).
7. Ensure all required CSS selectors are in `wwwroot/css/qgComposable-refined-minimalism.css` (do not create `*.razor.css` for this feature).

---

## 13. Dependencies

All dependencies listed here are expected to be implemented under `QuickGridTest01.ComposableColumns.*`:

- `FeatureContext<TGridItem>`
- `ICellRenderFeature<TGridItem>`
- `RowStateManager<TGridItem>`
- `ExpandableGridDataSource<TGridItem>` (optional, for spacer injection)
- `SpacerRowFactory` + `IRowIdentifiable` (only when enabling spacers)