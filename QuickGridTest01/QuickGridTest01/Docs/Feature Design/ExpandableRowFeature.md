# RowExpandFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.5 (Discussion Integrated) |
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
| State store | Private `_stateManager` per column instance | Column-scoped `RowStateManager<TGridItem>` registered in `FeatureContext` |
| Row identity | `IRowIdentifiable` (`Id`) | **`TGridItem : IRowIdentifiable` (`int Id`)** (RowColumn parity; spacer compatible) |
| Spacer rows | `ExpandableGridDataSource<TGridItem>` + encoded negative IDs | Same mechanism (`DataSource.Items` + `IsSpacer()` check) |
| Re-render | `InvokeAsync(StateHasChanged)` | `context.RequestRefreshAsync()` (preferred) |

---

## 2. Architecture

### 2.1 Interface & Priority

```csharp
public sealed class RowExpandFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class, IRowIdentifiable, new()
{
    public int Priority => FeaturePriority.Expansion;
}
```

**Priority rationale:** Expansion should run after general styling features (so it can add dimming/expanded classes) but before editing features (so edit UI can respect expanded state).

> `FeaturePriority.Expansion = 350` is NOT currently present and must be added between `Styling (300)` and `Editing (400)`.

### 2.2 Key Design Decisions (updated)

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Row identification | **`TGridItem : IRowIdentifiable` (`int Id`)** | Strict RowColumn parity and spacer-compatible identity |
| State storage | Column-scoped `RowStateManager<TGridItem>` registered in `FeatureContext` | One manager per column instance; enables downstream feature access |
| Overlay rendering | Below-row overlay (`top: 100%`) inside a `position:relative` wrapper | Must match RowColumn behavior (overlay starts below the master row) |
| Spacer injection (optional) | `ExpandableGridDataSource<TGridItem>` | Matches demo: pushes real rows below the overlay |
| Cascading context | Cascade only in expanded mode | Keeps child component contract minimal |
| Styling | Global stylesheet only | All feature CSS is in `wwwroot/css/qgComposable-refined-minimalism.css` |
| Feature logic location | ComposableColumns only | All `IColumnFeature` logic must live under `QuickGridTest01.ComposableColumns.*` |

### 2.3 Feature Responsibilities vs Grid Responsibilities

`RowExpandFeature` is purely a **cell render feature**.

Therefore:
- Expansion overlay rendering is always supported
- Spacer row injection is supported only when the grid is bound to an `ExpandableGridDataSource<TGridItem>` (i.e., `Items = DataSource.Items`)

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

### 3.3 Templates

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `DisplayTemplate` | `RenderFragment<RowDisplayContext<TGridItem>>?` | No | Content when collapsed |
| `ExpandedTemplate` | `RenderFragment<RowExpandedContext<TGridItem>>` | **Yes** | Content when expanded |

Fallback behavior (from RowColumn):
- `Button`: default button when `DisplayTemplate` is null
- `RowClick`: default chevron indicator when `DisplayTemplate` is null
- `Custom`: expects `DisplayTemplate` to provide trigger

### 3.4 Spacer injection

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `DataSource` | `ExpandableGridDataSource<TGridItem>?` | `null` | Optional collaboration object that performs spacer row injection/removal |

### 3.5 Events

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnBeforeExpand` | `EventCallback<RowBeforeExpandEventArgs<TGridItem>>` | Cancellable pre-expand |
| `OnExpanded` | `EventCallback<RowExpandedEventArgs<TGridItem>>` | Fired after expansion |
| `OnCollapsed` | `EventCallback<RowCollapsedEventArgs<TGridItem>>` | Fired after collapse |
| `OnStateChanged` | `EventCallback<RowStateChangedEventArgs<TGridItem>>` | Fired on any state change |

**Event ordering (RowColumn parity):**
1. `OnBeforeExpand` (cancellable)
2. Potentially collapse another row (if `CollapseCurrent`)
3. State transition event (`OnStateChanged: Collapsed -> Expanded`)
4. `OnExpanded`

Collapse:
1. State transition event (`OnStateChanged: Expanded -> Collapsed`)
2. `OnCollapsed`

### 3.6 Cancellation Support

Cancellation support is kept on the *feature/method* surface (not on UI-facing context delegates):

| Method | Signature | Description |
|--------|-----------|-------------|
| `ExpandRowAsync` | `Task ExpandRowAsync(TGridItem item, CancellationToken cancellationToken = default)` | Expand with cancellation support |
| `CollapseRowAsync` | `Task CollapseRowAsync(TGridItem item, CancellationToken cancellationToken = default)` | Collapse with cancellation support |
| `CollapseAllAsync` | `Task CollapseAllAsync(CancellationToken cancellationToken = default)` | Collapse all with cancellation support |

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

### 5.1 Identity is `IRowIdentifiable.Id` (initial port)

Row identity is constrained to `IRowIdentifiable` (`int Id`). This is required for:
- stable expand/collapse identity
- spacer row encoding/decoding

`FeatureContext.RowKey` (when present) must resolve to `item.Id`.

### 5.2 Spacer rows require a representable row item

The existing `ExpandableGridDataSource<T>` injects spacer rows as *instances of `T`* with encoded negative IDs.

Implications:
- If you want spacer rows, constrain usage to item types that can represent spacer rows: `IRowIdentifiable, new()`.
- Spacer row detection is performed via RowColumn parity method: `item.IsSpacer()`.

---

## 6. Spacer Row Data Model

### 6.1 SpacerRowFactory encoding

Spacer IDs are negative and materialize the parent relationship:

`spacerId = -(parentId * 1000 + offset)`

Where:
- `parentId` is the real row Id
- `offset` is 1..N (spacer index)

### 6.2 ExpandableGridDataSource semantics

Important detail from implementation:
- Stored spacers = `ExpandedRowSpan + 1`

---

## 7. Rendering Pipeline

### 7.3 Display Mode Rendering (corrected example)

The UI-facing delegates are tokenless, so the example must match:

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

---

## 8. Expand/Collapse State Machine

### 8.4 RowStateManager lifecycle & registration

**Decision (RowColumn parity):** one `RowStateManager<TGridItem>` per column/feature context.

Contract:
- **Creation:** created lazily on first `RenderCell` use.
- **Registration key:** service type (stored via `FeatureContext.RegisterService`).
- **Sentinel:** the presence of `RowStateManager<TGridItem>` in `FeatureContext` services prevents duplicate expansion features.
- **Disposal:** `ComposableColumn.Dispose()` calls `FeatureContext.Clear()`. Therefore `FeatureContext.Clear()` must dispose any `IDisposable` services before clearing.

### 8.5 Source of truth when using spacer rows

- `RowStateManager<TGridItem>` is the source of truth for expanded/collapsed state.
- `ExpandableGridDataSource<TGridItem>` is a derived *layout projection* (spacer injection).

Expand/collapse synchronization:
- Expand:
  - mutate `RowStateManager`
  - then call `DataSource.ExpandRow(item.Id, ExpandedRowSpan)` if `DataSource != null`
  - then refresh
- Collapse:
  - mutate `RowStateManager`
  - then call `DataSource.CollapseRow(item.Id)` if `DataSource != null`
  - then refresh

---

## 9. Events & UI Thread

### 9.1 UI-thread rule

**Primary rule:** all expand/collapse work (including event callbacks) must execute on the main Blazor UI thread.

If `FeatureContext.InvokeAsync` is available, it MUST be used as the dispatcher boundary:
- `await context.InvokeAsync(() => ExpandRowAsync(item, ct))`
- `await context.InvokeAsync(() => CollapseRowAsync(item, ct))`
- `await context.InvokeAsync(() => OnExpanded.InvokeAsync(args))`
- `await context.InvokeAsync(() => OnCollapsed.InvokeAsync(args))`

If `FeatureContext.InvokeAsync` is not available, the feature assumes it is already executing on the UI thread.

### 9.2 Cancellation and ordering

- If `OnBeforeExpand` cancels, no state mutation occurs and `OnStateChanged`/`OnExpanded` MUST NOT fire.

### 9.3 Exceptions

- Event handler exceptions propagate to the caller (RowColumn parity).
- No state rollback is attempted if an exception occurs after state mutation.

---

## 10. Logging

### 10.1 Logging contract

- Logging must not be feature-specific.
- The app uses the standard ASP.NET Core logging pipeline (Serilog-backed) configured in `Program.cs`.
- Features should log via a shared `ILoggerFactory` obtained through the owning component/context.

Minimal contract:
1. `FeatureContext` may optionally expose a shared `ILoggerFactory` service, registered once by `ComposableColumn`/`ComposableGrid`.
2. Features that need logging should request the shared factory and create a logger with an appropriate category/scope.
3. If no factory is available, logging is a no-op (no `Console.Error` for normal operation).

---

## 11. RowCard Component

### 11.1 API parity

The default `RowCard` shipped with this package must match RowColumn parity:
- `Title`
- `Class`
- `HeaderActions`
- `FooterContent`
- `ShowCloseButton`
- `OnClose` (`Func<Task>?`)
- `ChildContent`

### 11.2 Base feature close requirement

Even if a user-provided `ExpandedTemplate` does not render close UI, the base expansion feature must still provide a consistent way to collapse (e.g., built-in close button in the default `RowCard`, or a standard close affordance rendered by the feature itself when expanded).

---