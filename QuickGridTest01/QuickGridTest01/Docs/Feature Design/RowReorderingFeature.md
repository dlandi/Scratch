# RowReorderingFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.5 |
| Status | Draft (Spec-Plan Alignment Complete) |
| Created | 2025 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Reordering` |
| Source | Discussion: DevExpress Blazor Grid DragDropRows pattern |
| Styling | **All CSS for this feature must be placed in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (no `*.razor.css` for feature styling).** |
| Namespace rule | **All logic pertaining to an `IColumnFeature` must live under the `QuickGridTest01.ComposableColumns` namespace (and its sub-namespaces).** |
| Encoding | UTF-8 (code page 65001) |

---

## 1. Overview

### 1.1 Purpose

`RowReorderFeature<TGridItem>` provides **drag-and-drop row reordering** within the ComposableColumns architecture.

It provides:
- Drag handle cell rendering for initiating row drags
- Visual feedback during drag operations (drop zones, dragging state)
- Order persistence via `ReorderableDataSource<TGridItem>`
- Integration with grouping (within-group reordering)
- Events for reorder lifecycle (before, completed, cancelled)

### 1.2 Role in Architecture

This feature enables:
- **Manual ordering** - Users can prioritize items by dragging
- **Task boards** - Kanban-style priority management
- **Playlist/queue patterns** - User-defined ordering

```
ComposableColumn Architecture
    └── RowReorderFeature<TGridItem>        (Renders drag handle, fires events)
            ├── ReorderCoordinator           (Grid-level state coordination)
            └── ReorderableDataSource<T>     (Order tracking, persistence)
```

### 1.3 Reference Implementation

This feature provides parity with DevExpress Blazor Grid's DragDropRows:

| DevExpress | RowReorderFeature |
|------------|-------------------|
| `AllowDragRows="true"` | Feature attached to column |
| `DragRowTemplate` | `DragHandleTemplate` |
| `RowDragStart` | `OnBeforeReorder` |
| `RowDropped` | `OnRowReordered` |

---

## 2. Architecture

### 2.1 Interface & Priority

```csharp
public sealed class RowReorderFeature<TGridItem> 
    : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class, IRowIdentifiable
{
    public int Priority => FeaturePriority.Reordering; // 325
}
```

**Type constraints:**
- `TGridItem : class` - Required by ComposableColumns architecture
- `TGridItem : IRowIdentifiable` - Required for stable row identity during drag operations

**Priority rationale:** Reordering renders drag handles as styled cell content. It must run:
- After Styling (300) so conditional CSS applies to handles
- Before Expansion (350) so handles are part of normal rows, not overlays
- Before Editing (400) to avoid conflicts with edit triggers

> `FeaturePriority.Reordering = 325` must be added between `Styling (300)` and `Expansion (350)`.

### 2.2 Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Activation pattern | **Column-first** | Feature activates when attached to a `ComposableColumn`; registers with `ReorderCoordinator` |
| Priority | **325** (after Styling, before Expansion) | Drag handles are styled content that shouldn't conflict with overlays |
| Trigger mode | **Handle column (default)** | Avoids conflicts with editing, selection, expansion |
| State coordination | **Grid-owned `ReorderCoordinator`** | Mirrors `GroupingCoordinator` pattern |
| Order tracking | **`ReorderableDataSource<TGridItem>`** | Wraps items with order indices for persistence |
| Virtualization | **Required** | All ComposableColumn features must support virtualization (rule) |

### 2.3 Hybrid Coordinator Pattern

```
┌─────────────────────────────────────────────────────────────────┐
│ ComposableGrid<TGridItem>                                       │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ ReorderCoordinator<TGridItem> (owned by the grid)        │   │
│  │  - Tracks drag state (dragging item, target, position)   │   │
│  │  - References ReorderableDataSource<TGridItem>           │   │
│  │  - Coordinates events across features                    │   │
│  │  - Enforces grouping constraints                         │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ComposableCol │  │ComposableCol │  │ComposableCol │         │
│  │              │  │              │  │              │         │
│  │ ReorderFeat  │  │ (no reorder) │  │ (data column)│         │
│  │ (drag handle)│  │              │  │              │         │
│  │              │  │              │  │              │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

**Activation Flow:**

1. `RowReorderFeature<T>.OnAttach()` MUST have access to the cascaded `Grid` reference (`ComposableGrid<TGridItem>`).
2. The feature MUST obtain the grid-scoped `ReorderCoordinator<TGridItem>` from the grid (mirrors the existing grouping/filter registration pattern).
3. The coordinator MUST track active drag state and manage the `ReorderableDataSource<TGridItem>`.
4. Only one column MUST have `RowReorderFeature` attached per grid. If multiple columns attach `RowReorderFeature`, the grid MUST throw `InvalidOperationException` on the second registration.

#### 2.3.1 Coordinator storage + access (normative)

To remain consistent with existing feature integration (notably grouping and filtering), the reorder coordinator is **grid-owned** and accessed via the cascaded `ComposableGrid<TGridItem>` instance.

- `ComposableGrid<TGridItem>` must own a private field: `_reorderCoordinator`
- `ComposableGrid<TGridItem>` must expose an `internal` API: `GetOrCreateReorderCoordinator()` returning `ReorderCoordinator<TGridItem>`
- `RowReorderFeature<TGridItem>.OnAttach(...)` must call `grid.GetOrCreateReorderCoordinator()` and register itself

### 2.4 Feature Responsibilities vs Grid Responsibilities

`RowReorderFeature` is a **cell render feature** that coordinates grid-level behavior:

| Responsibility | Owner |
|----------------|-------|
| Drag handle rendering | `RowReorderFeature` |
| Drag/drop event handlers | `RowReorderFeature` |
| Drag state tracking | `ReorderCoordinator` (grid-owned) |
| Order persistence | `ReorderableDataSource<TGridItem>` (user-provided, referenced by coordinator) |
| Grouping constraint enforcement | `ReorderCoordinator` |
| CSS styling | Global stylesheet |

---

## 3. Parameters (runtime behavior)

### 3.1 Trigger & Behavior

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Enabled` | `bool` | `true` | Whether reordering is active |
| `TriggerMode` | `ReorderTriggerMode` | `HandleOnly` | How drag is initiated |
| `AllowCrossGroupReorder` | `bool` | `false` | When grouping active, allow drag between groups |

**ReorderTriggerMode semantics:**

| Value | Meaning |
|-------|---------|
| `HandleOnly` | Only the drag handle cell initiates drag (default, safe) |
| `EntireRow` | Entire row is draggable (conflicts with editing/selection) |

### 3.2 Row Height (for drop position calculation)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `RowHeight` | `int` | `48` | Height of a single grid row in pixels; used for drop position calculation |

**Implementation note (normative):** This follows the same pattern as `RowExpandFeature.RowHeight`. The value MUST match the grid's actual row height (typically QuickGrid's `ItemSize` parameter, which defaults to 50px). The default of 48px accounts for typical cell padding. If the grid uses a different `ItemSize`, this parameter MUST be set accordingly.

### 3.3 Drag Handle Appearance

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `DragHandleContent` | `string` | `"⋮⋮"` | Text/icon content for the handle |
| `DragHandleIcon` | `string?` | `null` | CSS class for icon (e.g., `"bi-grip-vertical"`) |
| `DragHandleTemplate` | `RenderFragment<TGridItem>?` | `null` | Custom handle template |

### 3.4 CSS Classes

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `HandleCellClass` | `string` | `"reorder-handle-cell"` | Class for the handle cell |
| `HandleClass` | `string` | `"reorder-handle"` | Class for the handle element |
| `DraggingRowClass` | `string` | `"reorder-dragging"` | Class applied to row being dragged |
| `DragOverBeforeClass` | `string` | `"reorder-drop-before"` | Class for drop-before indicator |
| `DragOverAfterClass` | `string` | `"reorder-drop-after"` | Class for drop-after indicator |
| `DisabledHandleClass` | `string` | `"reorder-disabled"` | Class when drag is not allowed |

### 3.5 Drag Filtering

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `CanDrag` | `Func<TGridItem, bool>?` | `null` | Predicate to filter draggable rows |
| `CanDropOn` | `Func<TGridItem, TGridItem, bool>?` | `null` | Predicate to validate drop target (source, target) |

**Predicate behavior (normative):**

- When `CanDrag` is `null`, all non-synthetic rows are draggable.
- When `CanDrag` returns `false` for an item, that item's drag handle MUST render with `DisabledHandleClass` and MUST NOT respond to drag events.
- When `CanDropOn` is `null`, all non-synthetic rows are valid drop targets (subject to grouping constraints).
- When `CanDropOn` returns `false` for a (source, target) pair, the target row MUST display invalid drop feedback and MUST NOT accept the drop.

### 3.6 Events

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnBeforeReorder` | `EventCallback<RowBeforeReorderEventArgs<TGridItem>>` | Cancellable pre-reorder |
| `OnRowReordered` | `EventCallback<RowReorderedEventArgs<TGridItem>>` | Fired after reorder completes |
| `OnReorderCancelled` | `EventCallback<RowReorderCancelledEventArgs<TGridItem>>` | Fired when reorder is cancelled |

**Event ordering:**
1. Drag starts → internal state update
2. Drop occurs → `OnBeforeReorder` (cancellable)
3. If not cancelled → order update → `OnRowReordered`
4. If cancelled → `OnReorderCancelled`

---

## 4. Event Args

### 4.1 RowBeforeReorderEventArgs

```csharp
public sealed class RowBeforeReorderEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem DraggedItem { get; init; }
    public required TGridItem TargetItem { get; init; }
    public required DropPosition Position { get; init; }
    public required int OldIndex { get; init; }
    public required int NewIndex { get; init; }

    /// <summary>
    /// Set to true to cancel the reorder operation.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Reason for cancellation. If Cancel is true and CancelReason is null,
    /// the reason defaults to "Cancelled by event handler".
    /// </summary>
    public string? CancelReason { get; set; }
}
```

### 4.2 RowReorderedEventArgs

```csharp
public sealed class RowReorderedEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem Item { get; init; }
    public required int OldIndex { get; init; }
    public required int NewIndex { get; init; }
    public required IReadOnlyList<TGridItem> NewOrder { get; init; }
}
```

### 4.3 RowReorderCancelledEventArgs

```csharp
public sealed class RowReorderCancelledEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem Item { get; init; }
    public required string Reason { get; init; }
}
```

### 4.4 DropPosition

```csharp
public enum DropPosition
{
    /// <summary>
    /// Insert before the target row.
    /// </summary>
    Before,
    
    /// <summary>
    /// Insert after the target row.
    /// </summary>
    After
}
```

---

## 5. ReorderCoordinator

### 5.1 Purpose

Grid-owned coordinator that manages drag state and order tracking.

```csharp
public sealed class ReorderCoordinator<TGridItem> : IDisposable
    where TGridItem : class, IRowIdentifiable
{
    // Current drag state
    public TGridItem? DraggedItem { get; private set; }
    public TGridItem? HoveredTarget { get; private set; }
    public DropPosition? CurrentDropPosition { get; private set; }
    public bool IsDragging => DraggedItem is not null;

    // Feature reference (set during feature registration)
    internal RowReorderFeature<TGridItem>? Feature { get; set; }

    /// <summary>
    /// Returns true if a RowReorderFeature is registered and its Enabled property is true.
    /// Used by ComposableGrid to suppress column sorting.
    /// </summary>
    public bool IsReorderingEnabled => Feature?.Enabled == true;

    // Data source
    public ReorderableDataSource<TGridItem>? DataSource { get; set; }

    // Grouping integration
    internal GroupingCoordinator<TGridItem>? GroupingCoordinator { get; set; }

    // State management
    public void StartDrag(TGridItem item);
    public void UpdateHover(TGridItem? target, DropPosition? position);
    public void ClearHover();
    public void CancelDrag();

    // Reorder execution
    public Task<ReorderResult> ExecuteReorderAsync(
        RowBeforeReorderEventArgs<TGridItem> args,
        CancellationToken cancellationToken = default);

    // Events for UI refresh
    public event Action? OnStateChanged;
}

public enum ReorderResult
{
    Success,
    Cancelled,
    Failed
}
```

### 5.2 Synthetic Row Detection (normative)

Rows from `GroupingFeature` (group header markers, group header spacers) and `RowExpandFeature` (expansion spacers) are synthetic and MUST NOT participate in reordering.

**Detection rule:** A row is synthetic if `item.Id < 0` (negative ID encoding used by grouping and expansion features).

```csharp
internal static bool IsSyntheticRow<TGridItem>(TGridItem item) 
    where TGridItem : IRowIdentifiable
{
    return item.Id < 0;
}
```

### 5.3 Grouping Constraint Enforcement

When grouping is active and `AllowCrossGroupReorder` is false (the default):

```csharp
internal bool CanDropOnTarget(TGridItem source, TGridItem target, bool allowCrossGroup)
{
    // Synthetic rows are never valid drop targets
    if (IsSyntheticRow(target))
        return false;

    if (GroupingCoordinator?.ActiveGrouping is null)
        return true; // No grouping, allow all

    if (allowCrossGroup)
        return true; // Explicitly allowed by feature parameter

    // Check if source and target are in the same group
    var activeGrouping = GroupingCoordinator.ActiveGrouping;
    var sourceGroup = activeGrouping.GetGroupKey(source);
    var targetGroup = activeGrouping.GetGroupKey(target);

    return Equals(sourceGroup, targetGroup);
}
```

**Parameter ownership (normative):** `AllowCrossGroupReorder` is a parameter on `RowReorderFeature`, NOT on `ReorderCoordinator`. The feature MUST pass this value to the coordinator when validating drop targets.

---

## 6. ReorderableDataSource

### 6.1 Purpose

Wraps the grid's items with order tracking, enabling persistence and restoration of user-defined order.

```csharp
public sealed class ReorderableDataSource<TGridItem> : IDisposable
    where TGridItem : class, IRowIdentifiable
{
    private readonly List<TGridItem> _orderedItems;
    private readonly Dictionary<int, double> _orderIndices; // ItemId → OrderIndex (double for fractional insertion)

    public ReorderableDataSource(IEnumerable<TGridItem> items);

        /// <summary>
        /// The items in current order as IQueryable for grid binding.
        /// </summary>
        public IQueryable<TGridItem> Items => _orderedItems.AsQueryable();

        /// <summary>
        /// Current order as a readonly list.
        /// </summary>
        public IReadOnlyList<TGridItem> CurrentOrder => _orderedItems;

        /// <summary>
        /// Fires when order changes. MUST fire after every successful reorder operation.
        /// </summary>
        public event Action? OnOrderChanged;

        // Order manipulation
        public void MoveItem(int fromIndex, int toIndex);
        public void MoveItem(TGridItem item, int toIndex);
        public void MoveItemBefore(TGridItem item, TGridItem target);
        public void MoveItemAfter(TGridItem item, TGridItem target);

        // Index lookup
        public int IndexOf(TGridItem item);

        // Persistence
        public IReadOnlyList<int> GetOrderIndices();
        public void SetOrderIndices(IReadOnlyList<int> indices);
        public void ResetOrder();

        // Refresh from external source
        public void UpdateItems(IEnumerable<TGridItem> items, bool preserveOrder = true);
    }
    ```

    ### 6.2 DataSource Error Behavior (normative)

    | Method | Error Condition | Behavior |
    |--------|-----------------|----------|
| `MoveItem(fromIndex, toIndex)` | Index out of range | MUST throw `ArgumentOutOfRangeException` |
| `MoveItem(item, toIndex)` | Item not found | MUST throw `ArgumentException` |
| `MoveItemBefore/After` | Item or target not found | MUST throw `ArgumentException` |
| `SetOrderIndices` | Indices don't match current items | MUST throw `ArgumentException` with message explaining mismatch |
| `UpdateItems` | `null` items | MUST throw `ArgumentNullException` |

### 6.3 Order Tracking Model (normative)

Order is tracked by **Item ID**, not absolute position. This enables predictable behavior when filtering and adding new items.

**Internal representation:**
```csharp
private readonly Dictionary<int, double> _orderIndices; // ItemId → OrderIndex
```

**Invariants:**
1. Every item in `_orderedItems` MUST have an entry in `_orderIndices`.
2. `OrderIndex` values determine sort order; lower values appear first.
3. `OrderIndex` values MAY be fractional (to insert between existing items without renumbering).

**Reorder within filtered view (normative):**

When a filter is active and the user reorders items, the reorder affects the **full dataset order**, not just the filtered subset.

Example:
```
Full dataset:  [A=0, B=1, C=2, D=3, E=4]  (OrderIndex values)
Filter active: [B, D] visible
User reorders: D before B

Result:
- D.OrderIndex = B.OrderIndex - 0.001 = 0.999
- Full dataset order becomes: [A=0, D=0.999, B=1, C=2, E=4]
- When filter clears: [A, D, B, C, E]
```

**New items (normative):**

When `UpdateItems` is called with new items not present in the current order:
- New items MUST be assigned `OrderIndex = MaxCurrentOrderIndex + 1`
- This appends new items to the **end** of the ordered list
- The `preserveOrder` parameter controls whether existing items retain their order indices

**Removed items:**

When `UpdateItems` is called and some current items are no longer present:
- Their order indices MUST be removed from `_orderIndices`
- If a previously-removed item is re-added later, it is treated as a new item (appended to end)

### 6.4 Order Persistence Pattern

```csharp
// Save order to storage
var orderIndices = dataSource.GetOrderIndices();
await localStorage.SetAsync("taskOrder", orderIndices);

// Restore order on load
var savedOrder = await localStorage.GetAsync<List<int>>("taskOrder");
if (savedOrder is not null)
{
    dataSource.SetOrderIndices(savedOrder);
}
```

---

## 7. Integration with Other Features

### 7.1 Grouping Integration

| Scenario | Behavior |
|----------|----------|
| No grouping active | Reorder freely across entire grid |
| Grouping active, `AllowCrossGroupReorder = false` | Reorder only within same group; cross-group drops MUST be rejected |
| Grouping active, `AllowCrossGroupReorder = true` | Allow cross-group; **does NOT mutate grouping key** |
| Drop on group header | NOT allowed; group headers are synthetic rows and MUST reject drops |

**Implementation note (normative):** Cross-group reordering changes visual position but MUST NOT modify the item's grouping key property. If the user wants to change the group, that is an editing operation, not a reorder operation.

### 7.2 Sorting Integration

**Rule (normative):** When reordering is enabled on a grid, column sorting MUST be suppressed.

| Scenario | Behavior |
|----------|----------|
| Reordering enabled | QuickGrid column sorting MUST be disabled; `SortBy` MUST NOT be set on columns |
| User clicks sortable header | No-op; header MUST NOT respond to sort clicks |
| Reordering disabled | Normal sorting resumes |

**Rationale:** User-defined order is the "sort". Allowing column sort would discard user ordering, causing confusion.

**Implementation (normative):** `ComposableGrid` MUST check for active `ReorderCoordinator` and MUST suppress `SortBy` on all columns when reordering is enabled (same pattern as grouping suppresses global sort).

### 7.3 Filtering Integration

| Scenario | Behavior |
|----------|----------|
| Filter active | User reorders visible (filtered) items |
| Filter cleared | Full dataset shown; reordered items retain positions |
| Item filtered out during drag | Drag MUST cancel automatically |

**Implementation (normative):** Reordering operates on the visible `FilteredItems`. The `ReorderableDataSource` MUST track order against item IDs, so order persists correctly when filters change.

### 7.4 Expansion Integration

| Scenario | Behavior |
|----------|----------|
| Drag expanded row | Allowed; row MUST collapse automatically before drag starts |
| Drop on expanded row | Allowed; drop MUST use row position, not overlay position |
| Drag spacer row | NOT allowed; spacer rows are synthetic and MUST NOT be draggable |

### 7.5 Editing Integration

| Scenario | Behavior |
|----------|----------|
| `TriggerMode = HandleOnly` | No conflict; handle and edit areas are separate |
| `TriggerMode = EntireRow` | Editing MUST take precedence; drag MUST NOT initiate while cell is in edit mode |
| Drag during active edit | NOT allowed; user MUST finish or cancel edit before drag can start |

### 7.6 DataSource Requirement

**Rule (normative):** `RowReorderFeature` MUST have access to a `ReorderableDataSource<TGridItem>` to function.

| Configuration | Behavior |
|---------------|----------|
| `ReorderableDataSource` provided via coordinator | Feature operates normally |
| `ReorderableDataSource` not provided | Feature MUST throw `InvalidOperationException` on first drag attempt with message: "RowReorderFeature requires a ReorderableDataSource. Bind the grid's Items to ReorderableDataSource.Items." |

### 7.7 UI Thread Rule (normative)

**Primary rule:** All reorder operations (including event callbacks) MUST execute on the main Blazor UI thread.

If `FeatureContext.InvokeAsync` is available, it MUST be used as the dispatcher boundary:
- `await context.InvokeAsync(() => OnBeforeReorder.InvokeAsync(args))`
- `await context.InvokeAsync(() => OnRowReordered.InvokeAsync(args))`
- `await context.InvokeAsync(() => OnReorderCancelled.InvokeAsync(args))`

If `FeatureContext.InvokeAsync` is not available, the feature MUST assume it is already executing on the UI thread.

### 7.8 Exception Handling (normative)

- Event handler exceptions MUST propagate to the caller.
- If an exception occurs after order mutation, no rollback is attempted.
- If `OnBeforeReorder` throws, the reorder MUST be cancelled and `OnReorderCancelled` MUST fire with reason "Event handler exception".

---

## 8. Rendering Pipeline

### 8.1 Cell Rendering

```csharp
public void RenderCell(
    RenderTreeBuilder builder,
    ref int sequence,
    TGridItem item,
    FeatureContext<TGridItem> context,
    Action renderNext)
{
    var coordinator = GetCoordinator(context);
    
    // Skip rendering for synthetic rows (group headers, spacers)
    if (IsSyntheticRow(item))
    {
        builder.OpenElement(sequence++, "div");
        builder.AddAttribute(sequence++, "class", "reorder-cell-empty");
        builder.CloseElement();
        return;
    }
    
    var canDrag = CanDrag?.Invoke(item) ?? true;
    var isDragging = ReferenceEquals(coordinator.DraggedItem, item);
    var isDropTarget = ReferenceEquals(coordinator.HoveredTarget, item);
    
    builder.OpenElement(sequence++, "div");
    builder.AddAttribute(sequence++, "class", BuildCellClass(canDrag, isDragging, isDropTarget, coordinator.CurrentDropPosition));
    
    if (canDrag && Enabled)
    {
        RenderDragHandle(builder, ref sequence, item, coordinator);
    }
    
    builder.CloseElement();
}
```

### 8.2 Drag Handle Rendering

```csharp
private void RenderDragHandle(
    RenderTreeBuilder builder,
    ref int seq,
    TGridItem item,
    ReorderCoordinator<TGridItem> coordinator)
{
    builder.OpenElement(seq++, "div");
    builder.AddAttribute(seq++, "class", HandleClass);
    builder.AddAttribute(seq++, "draggable", "true");
    
    // HTML5 Drag & Drop events
    builder.AddAttribute(seq++, "ondragstart",
        EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragStart(item, e)));
    builder.AddAttribute(seq++, "ondragend",
        EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragEnd(item, e)));
    builder.AddAttribute(seq++, "ondragover",
        EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragOver(item, e)));
    builder.AddAttribute(seq++, "ondrop",
        EventCallback.Factory.Create<DragEventArgs>(this, e => OnDrop(item, e)));
    builder.AddAttribute(seq++, "ondragleave",
        EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragLeave(item, e)));
    
    // Accessibility
    builder.AddAttribute(seq++, "role", "button");
    builder.AddAttribute(seq++, "aria-label", "Drag to reorder");
    builder.AddAttribute(seq++, "tabindex", "0");
    
    // Content
    if (DragHandleTemplate is not null)
    {
        builder.AddContent(seq++, DragHandleTemplate(item));
    }
    else if (!string.IsNullOrEmpty(DragHandleIcon))
    {
        builder.OpenElement(seq++, "i");
        builder.AddAttribute(seq++, "class", DragHandleIcon);
        builder.CloseElement();
    }
    else
    {
        builder.AddContent(seq++, DragHandleContent);
    }
    
    builder.CloseElement();
}
```

### 8.3 Drop Position Detection

```csharp
private DropPosition GetDropPosition(DragEventArgs e, double rowHeight)
{
    // Calculate based on mouse Y position relative to row height.
    // This uses clientY modulo row height to determine position within the row.
    // If in upper half: Before, else After.
    var relativeY = e.ClientY % rowHeight;
    return relativeY < (rowHeight / 2) ? DropPosition.Before : DropPosition.After;
}
```

**Implementation note (normative):** Drop position detection MUST use the HTML5 Drag & Drop API's `DragEventArgs.ClientY`. If more precise detection is required in future (e.g., for variable-height rows), JS interop MAY be added, but the initial implementation MUST NOT require JS interop.

---

## 9. CSS Requirements

All styles must be added to `wwwroot/css/qgComposable-refined-minimalism.css`.

```css
/* ===== Row Reordering Feature ===== */

/* Drag handle cell */
.reorder-handle-cell {
    width: 40px;
    min-width: 40px;
    max-width: 40px;
    text-align: center;
    padding: 0;
    user-select: none;
}

/* Drag handle element */
.reorder-handle {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    cursor: grab;
    color: var(--color-text-tertiary);
    font-size: 1.1rem;
    transition: color 0.15s ease, background-color 0.15s ease;
}

.reorder-handle:hover {
    color: var(--color-text-primary);
    background-color: var(--color-bg-subtle);
}

.reorder-handle:active {
    cursor: grabbing;
}

/* Disabled handle */
.reorder-handle.reorder-disabled {
    cursor: not-allowed;
    opacity: 0.4;
}

/* Row being dragged */
tr.reorder-dragging,
.reorder-dragging {
    opacity: 0.5;
    background-color: var(--color-accent-subtle) !important;
}

/* Drop zone indicators */
tr.reorder-drop-before,
.reorder-drop-before {
    box-shadow: inset 0 2px 0 0 var(--color-accent);
}

tr.reorder-drop-after,
.reorder-drop-after {
    box-shadow: inset 0 -2px 0 0 var(--color-accent);
}

/* Empty cell for synthetic rows */
.reorder-cell-empty {
    width: 40px;
    min-width: 40px;
}

/* Cursor during drag over valid target */
.reorder-drop-target {
    cursor: copy;
}

/* Cursor during drag over invalid target */
.reorder-drop-invalid {
    cursor: no-drop;
}
```

---

## 10. Usage Examples

### 10.1 Basic Reordering

```razor
@using QuickGridTest01.ComposableColumns.Features.Reordering

<ComposableGrid TGridItem="TaskItem" Items="@_dataSource.Items">
    <!-- Drag Handle Column (first column) -->
    <ComposableColumn TGridItem="TaskItem" TValue="object">
        <Features>
            <RowReorderFeature TGridItem="TaskItem"
                OnRowReordered="HandleRowReordered" />
        </Features>
    </ComposableColumn>

    <!-- Note: Sortable is ignored when reordering is enabled (see section 7.2) -->
    <PropertyColumn Property="@(x => x.Title)" />
    <PropertyColumn Property="@(x => x.Priority)" />
</ComposableGrid>

@code {
    private ReorderableDataSource<TaskItem> _dataSource = default!;

    protected override void OnInitialized()
    {
        var tasks = GetTasks();
        _dataSource = new ReorderableDataSource<TaskItem>(tasks);
        _dataSource.OnOrderChanged += StateHasChanged;
    }

    private async Task HandleRowReordered(RowReorderedEventArgs<TaskItem> args)
    {
        Console.WriteLine($"Moved '{args.Item.Title}' from {args.OldIndex} to {args.NewIndex}");
        await SaveOrderAsync(args.NewOrder);
    }
}
```

### 10.2 With Validation

```razor
<RowReorderFeature TGridItem="TaskItem"
    CanDrag="@(item => !item.IsLocked)"
    CanDropOn="@((source, target) => source.Category == target.Category)"
    OnBeforeReorder="ValidateReorder"
    OnReorderCancelled="ShowCancelledMessage" />

@code {
    private void ValidateReorder(RowBeforeReorderEventArgs<TaskItem> args)
    {
        if (args.DraggedItem.IsCompleted && !args.TargetItem.IsCompleted)
        {
            args.Cancel = true;
            args.CancelReason = "Cannot move completed tasks above active tasks";
        }
    }
    
    private void ShowCancelledMessage(RowReorderCancelledEventArgs<TaskItem> args)
    {
        _toastService.ShowWarning(args.Reason);
    }
}
```

### 10.3 With Grouping

```razor
<ComposableGrid TGridItem="TaskItem" Items="@_dataSource.Items">
    <!-- Drag Handle -->
    <ComposableColumn TGridItem="TaskItem" TValue="object">
        <Features>
            <RowReorderFeature TGridItem="TaskItem"
                AllowCrossGroupReorder="false" />
        </Features>
    </ComposableColumn>
    
    <!-- Status Column with Grouping -->
    <ComposableColumn TGridItem="TaskItem" TValue="string" Property="@(x => x.Status)">
        <Features>
            <GroupingFeature TGridItem="TaskItem" TValue="string"
                ColumnId="Status"
                GroupBy="@(x => x.Status)"
                IsActive="true" />
        </Features>
    </ComposableColumn>
    
    <PropertyColumn Property="@(x => x.Title)" />
</ComposableGrid>
```

### 10.4 Order Persistence

```razor
@code {
    protected override async Task OnInitializedAsync()
    {
        var tasks = await _taskService.GetTasksAsync();
        _dataSource = new ReorderableDataSource<TaskItem>(tasks);
        
        // Restore saved order
        var savedOrder = await _localStorage.GetAsync<List<int>>("taskOrder");
        if (savedOrder is not null)
        {
            _dataSource.SetOrderIndices(savedOrder);
        }
        
        _dataSource.OnOrderChanged += async () =>
        {
            // Auto-save on change
            var order = _dataSource.GetOrderIndices();
            await _localStorage.SetAsync("taskOrder", order);
            await InvokeAsync(StateHasChanged);
        };
    }
}
```

---

## 11. Accessibility

### 11.1 Keyboard Support (Deferred to Backlog)

**Status:** Keyboard reorder mode is NOT in scope for initial implementation. The drag handle MUST be focusable (`tabindex="0"`) for accessibility compliance, but keyboard-based reordering (arrow keys to move, Enter to confirm) is deferred.

**Initial implementation requirements:**
- Drag handle MUST have `tabindex="0"` for keyboard focus
- Drag handle MUST have `role="button"` and appropriate `aria-label`

**Backlog item:** Full keyboard reorder mode with the following behavior:

| Key | Action |
|-----|--------|
| `Tab` | Focus drag handle |
| `Space` / `Enter` | Begin keyboard reorder mode |
| `↑` / `↓` | Move item up/down (in keyboard mode) |
| `Escape` | Cancel keyboard reorder |
| `Enter` | Confirm new position |

### 11.2 ARIA Attributes (Initial Implementation)

```html
<div class="reorder-handle"
     role="button"
     aria-label="Drag to reorder"
     tabindex="0">
```

During mouse drag (HTML5 Drag & Drop handles this natively):
```html
<div class="reorder-handle"
     aria-grabbed="true"
     aria-dropeffect="move">
```

### 11.3 Screen Reader Announcements (Deferred to Backlog)

Screen reader announcements for position changes are deferred along with keyboard reorder mode. Initial implementation relies on native HTML5 Drag & Drop accessibility.

---

## 12. Testing Requirements

### 12.1 Unit Tests

| Test Case | Description |
|-----------|-------------|
| `Reorder_MoveItemDown_UpdatesOrder` | Moving item from index 0 to 2 updates order correctly |
| `Reorder_MoveItemUp_UpdatesOrder` | Moving item from index 2 to 0 updates order correctly |
| `Reorder_WithGrouping_StaysInGroup` | When `AllowCrossGroupReorder=false`, items stay in group |
| `Reorder_Cancel_PreservesOriginalOrder` | Cancelled reorder doesn't change order |
| `Reorder_CanDrag_RespectsPredicate` | Items failing `CanDrag` predicate aren't draggable |
| `Reorder_CanDropOn_RespectsPredicate` | Invalid drop targets are rejected |
| `DataSource_GetOrderIndices_ReturnsCorrectOrder` | Order indices match current order |
| `DataSource_SetOrderIndices_RestoresOrder` | Setting indices restores previous order |

### 12.2 Integration Tests

| Test Case | Description |
|-----------|-------------|
| `DragHandle_Renders_WhenFeatureAttached` | Drag handle visible in first column |
| `DragStart_UpdatesCoordinatorState` | Coordinator reflects drag state |
| `DragOver_ShowsDropIndicator` | Visual feedback appears on valid targets |
| `Drop_FiresEvents` | `OnBeforeReorder` and `OnRowReordered` fire in order |
| `WithFiltering_ReordersVisibleItems` | Reorder works correctly with active filter |
| `WithVirtualization_MaintainsOrder` | Order persists through scroll virtualization |

---

## 13. Future Considerations

### 13.1 Deferred Features

| Feature | Description | Priority |
|---------|-------------|----------|
| Multi-row drag | Select multiple rows and drag together | Low |
| Cross-grid drag | Drag items between different grids | Low |
| Touch support | `touchstart`, `touchmove`, `touchend` handlers | Medium |
| Animation | Smooth transition animations during reorder | Low |
| Undo/Redo | Track order history and allow reverting | Medium |

### 13.2 Known Limitations

1. **Virtualization edge case:** If dragging to a position that is currently virtualized (off-screen), the drop target is not visible. Auto-scroll during drag is NOT implemented in the initial version. Users MUST scroll manually to reach off-screen targets.

2. **Large datasets:** Order tracking uses an in-memory list. For datasets exceeding 10,000 items, performance degradation is expected. Server-side order tracking is NOT in scope for this feature.

3. **Real-time sync:** If multiple users reorder simultaneously, conflicts require application-level resolution. This feature does NOT provide conflict detection or resolution.

---

## 14. Related Documentation

| Document | Description |
|----------|-------------|
| [RowGroupingFeature.md](RowGroupingFeature.md) | Grouping feature (integration reference) |
| [ExpandableRowFeature.md](ExpandableRowFeature.md) | Row expansion feature (integration reference) |
| [ComposableColumnsParity_01.md](ComposableColumnsParity_01.md) | Overall parity tracking |

---

## 15. Appendix: FeaturePriority Update

Add to `QuickGridTest01\ComposableColumns\Core\FeaturePriority.cs`:

```csharp
/// <summary>
/// Reordering features (drag handle, drop zones).
/// Must run after Styling so handles are styled, before Expansion to avoid overlay conflicts.
/// </summary>
public const int Reordering = 325;
```

---

## 16. Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 0.1 | 2025 | AI Assistant | Initial draft based on discussion |
| 0.2 | 2025 | AI Assistant | Normative language review: replaced fuzzy language (should, may, can) with MUST/MUST NOT; added sections 5.2 (synthetic row detection), 6.2 (error behavior), 7.6-7.8 (DataSource requirement, UI thread rule, exception handling); clarified predicate behavior in 3.4 |
| 0.3 | 2025 | AI Assistant | Added section 3.2 (RowHeight parameter); moved keyboard reorder mode to backlog (section 11); added section 6.3 (order tracking model) defining behavior for filtered reordering and new item insertion |
| 0.4 | 2025 | AI Assistant | Internal consistency fixes: corrected `_orderIndices` type from `int` to `double` in section 6.1; clarified DataSource ownership (user-provided, not coordinator-owned); removed misleading `Sortable="true"` from usage example; updated architecture diagram |
| 0.5 | 2025 | AI Assistant | Plan alignment: added `IndexOf` method to ReorderableDataSource (section 6.1); added `Feature` property and `IsReorderingEnabled` to ReorderCoordinator (section 5.1) for sorting suppression support |
