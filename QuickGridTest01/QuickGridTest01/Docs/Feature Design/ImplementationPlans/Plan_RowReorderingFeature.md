# Implementation Plan — `RowReorderFeature<TGridItem>`

## Introduction

This plan translates `Docs/Feature Design/RowReorderingFeature.md` into an executable implementation roadmap for implementing `RowReorderFeature<TGridItem>` within the ComposableColumns architecture.

Scope constraints enforced by the spec:

- Target framework: .NET 9, Blazor Server (QuickGrid)
- Feature namespace root: `QuickGridTest01.ComposableColumns.*`
- Reordering feature namespace: `QuickGridTest01.ComposableColumns.Features.Reordering`
- Styling: **all CSS lives in `wwwroot/css/qgComposable-refined-minimalism.css`** (no feature `*.razor.css`)
- Pattern reference: `GroupingFeature` coordinator pattern, `ExpandableRowFeature` row identity pattern

Non-goals:

- Do not implement keyboard reorder mode (deferred to backlog).
- Do not implement multi-row drag (select and drag multiple rows).
- Do not implement cross-grid drag (drag between different grids).
- Do not implement touch support (`touchstart`, `touchmove`, `touchend`).
- Do not implement auto-scroll during drag for virtualized off-screen targets.

---

## Plan execution anchors (for task generation)

This plan includes implementation details in section `2.x`. For task generation and stable references, use the following lifecycle anchors:

- **Phase 1 (Initialization):** Grid/column initialization and feature attachment prerequisites
- **Phase 2 (Feature attachment):** `RowReorderFeature.OnAttach` creates/registers coordinator + validates single-feature constraint
- **Phase 3 (Grid rendering):** QuickGrid enumerates the bound `Items` sequence (from `ReorderableDataSource.Items`)
- **Phase 4 (Cell rendering):** Feature renders drag handle cells; detects synthetic rows
- **Phase 5 (User interaction):** Drag start, drag over, drop, drag end (HTML5 Drag & Drop API)
- **Phase 6 (Order mutation):** Execute reorder via coordinator → data source → fire events
- **Phase 7 (Disposal):** Dispose feature/coordinator

**Deterministic activation rule:** Reordering is active when a `RowReorderFeature` is attached and `Enabled = true`. Only one `RowReorderFeature` is permitted per grid.

---

## 1. Reference implementation analysis

### 1.1 Behavioral requirements checklist

The new feature must implement these semantics from the spec:

1. **Drag handle rendering**
   - Drag handle cell renders for non-synthetic rows when `Enabled = true` and `CanDrag` returns true
   - Synthetic rows (negative IDs from grouping/expansion) render empty cells
   - Handle is focusable (`tabindex="0"`) with ARIA attributes

2. **HTML5 Drag & Drop integration**
   - `draggable="true"` on handle element
   - Events: `ondragstart`, `ondragend`, `ondragover`, `ondrop`, `ondragleave`
   - Drop position detection via `DragEventArgs.ClientY` modulo `RowHeight`

3. **Order tracking**
   - `ReorderableDataSource<TGridItem>` tracks order by Item ID using fractional `double` indices
   - New items append to end (`MaxOrderIndex + 1`)
   - Removed items lose their order; re-added items treated as new

4. **Event ordering**
   - Drag starts → internal state update
   - Drop occurs → `OnBeforeReorder` (cancellable)
   - If not cancelled → order update → `OnRowReordered`
   - If cancelled → `OnReorderCancelled`

5. **Grouping integration**
   - When `AllowCrossGroupReorder = false` (default), drops are rejected if source and target are in different groups
   - Group headers (synthetic rows) are never valid drop targets

6. **Sorting suppression**
   - When reordering is enabled, column sorting MUST be suppressed (same pattern as grouping)

7. **Filtering integration**
   - Reordering operates on visible `FilteredItems`
   - Order persists by Item ID, so order is preserved when filters change

8. **Expansion integration**
   - Dragging an expanded row collapses it first
   - Spacer rows are not draggable

---

## 2. ComposableColumns integration analysis

### 2.1 Rendering model

Target: `RowReorderFeature<TGridItem>` implements:
- `ICellRenderFeature<TGridItem>` - Renders drag handle in cell
- `IDisposable` - Cleanup

The feature renders drag handles and manages drag state via a grid-owned coordinator.

### 2.2 Priority

`FeaturePriority.Reordering = 325` (after Styling at 300, before Expansion at 350)

**Rationale:** Drag handles are styled cell content that should not conflict with expansion overlays or editing triggers.

### 2.3 Coordinator pattern

The feature uses a **Grid-Owned Coordinator Pattern** (mirrors `GroupingCoordinator`):

1. First `RowReorderFeature` to attach creates `ReorderCoordinator<TGridItem>` via `grid.GetOrCreateReorderCoordinator()`
2. Coordinator is stored on the grid instance
3. Only one `RowReorderFeature` is permitted per grid; second registration throws `InvalidOperationException`
4. Coordinator references (does not own) the user-provided `ReorderableDataSource<TGridItem>`
5. Coordinator tracks drag state and enforces grouping constraints

### 2.4 Data source ownership

**Key distinction from grouping:** The `ReorderableDataSource<TGridItem>` is **user-provided**, not coordinator-owned.

- User creates `ReorderableDataSource<TGridItem>` and binds grid to `dataSource.Items`
- Coordinator references the data source for order manipulation
- If data source is not provided, feature throws on first drag attempt

### 2.5 Sorting suppression mechanism

When reordering is enabled, `ComposableGrid` MUST suppress column sorting:

```csharp
// In ComposableGrid, when determining SortBy for columns:
if (_reorderCoordinator?.IsReorderingEnabled == true)
{
    // Suppress all column sorting
    return null;
}
```

This mirrors the grouping suppression pattern.

### 2.6 Synthetic row detection

Rows with negative IDs (from grouping or expansion features) are synthetic and MUST NOT participate in reordering:

```csharp
internal static bool IsSyntheticRow<TGridItem>(TGridItem item) 
    where TGridItem : IRowIdentifiable
{
    return item.Id < 0;
}
```

---

## 3. Public API to implement

### 3.1 Feature type

Create:

- `QuickGridTest01.ComposableColumns.Features.Reordering.RowReorderFeature<TGridItem>`

Signature:

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

### 3.2 Parameters / properties

1. **Trigger & behavior**
   - `bool Enabled { get; set; } = true;`
   - `ReorderTriggerMode TriggerMode { get; set; } = ReorderTriggerMode.HandleOnly;`
   - `bool AllowCrossGroupReorder { get; set; } = false;`

2. **Row height**
   - `int RowHeight { get; set; } = 48;` (for drop position calculation)

3. **Drag handle appearance**
   - `string DragHandleContent { get; set; } = "⋮⋮";`
   - `string? DragHandleIcon { get; set; }` (CSS class for icon)
   - `RenderFragment<TGridItem>? DragHandleTemplate { get; set; }`

4. **CSS classes**
   - `string HandleCellClass { get; set; } = "reorder-handle-cell";`
   - `string HandleClass { get; set; } = "reorder-handle";`
   - `string DraggingRowClass { get; set; } = "reorder-dragging";`
   - `string DragOverBeforeClass { get; set; } = "reorder-drop-before";`
   - `string DragOverAfterClass { get; set; } = "reorder-drop-after";`
   - `string DisabledHandleClass { get; set; } = "reorder-disabled";`

5. **Drag filtering**
   - `Func<TGridItem, bool>? CanDrag { get; set; }`
   - `Func<TGridItem, TGridItem, bool>? CanDropOn { get; set; }`

6. **Events**
   - `EventCallback<RowBeforeReorderEventArgs<TGridItem>> OnBeforeReorder { get; set; }`
   - `EventCallback<RowReorderedEventArgs<TGridItem>> OnRowReordered { get; set; }`
   - `EventCallback<RowReorderCancelledEventArgs<TGridItem>> OnReorderCancelled { get; set; }`

### 3.3 Internal methods

The feature implements these internal methods (not public API):

- `void OnDragStart(TGridItem item, DragEventArgs e)`
- `void OnDragEnd(TGridItem item, DragEventArgs e)`
- `void OnDragOver(TGridItem item, DragEventArgs e)`
- `void OnDrop(TGridItem item, DragEventArgs e)`
- `void OnDragLeave(TGridItem item, DragEventArgs e)`
- `DropPosition GetDropPosition(DragEventArgs e, double rowHeight)`

---

## 4. Types + contracts to create under `ComposableColumns.Features.Reordering`

### 4.1 Event args

Create under `ComposableColumns.Features.Reordering`:

```csharp
public sealed class RowBeforeReorderEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem DraggedItem { get; init; }
    public required TGridItem TargetItem { get; init; }
    public required DropPosition Position { get; init; }
    public required int OldIndex { get; init; }
    public required int NewIndex { get; init; }
    public bool Cancel { get; set; }
    public string? CancelReason { get; set; }
}

public sealed class RowReorderedEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem Item { get; init; }
    public required int OldIndex { get; init; }
    public required int NewIndex { get; init; }
    public required IReadOnlyList<TGridItem> NewOrder { get; init; }
}

public sealed class RowReorderCancelledEventArgs<TGridItem> : EventArgs
    where TGridItem : class, IRowIdentifiable
{
    public required TGridItem Item { get; init; }
    public required string Reason { get; init; }
}
```

### 4.2 Enums

Create under `ComposableColumns.Features.Reordering`:

```csharp
public enum DropPosition
{
    Before,
    After
}

public enum ReorderTriggerMode
{
    HandleOnly,
    EntireRow
}

public enum ReorderResult
{
    Success,
    Cancelled,
    Failed
}
```

### 4.3 Coordinator

Create `ReorderCoordinator<TGridItem>`:

```csharp
public sealed class ReorderCoordinator<TGridItem> : IDisposable
    where TGridItem : class, IRowIdentifiable
{
    // Drag state
    public TGridItem? DraggedItem { get; private set; }
    public TGridItem? HoveredTarget { get; private set; }
    public DropPosition? CurrentDropPosition { get; private set; }
    public bool IsDragging => DraggedItem is not null;

    // External references
    public ReorderableDataSource<TGridItem>? DataSource { get; set; }
    internal GroupingCoordinator<TGridItem>? GroupingCoordinator { get; set; }

    // Feature reference (for AllowCrossGroupReorder and Enabled check)
    internal RowReorderFeature<TGridItem>? Feature { get; set; }

    /// <summary>
    /// Returns true if a RowReorderFeature is registered and its Enabled property is true.
    /// Used by ComposableGrid to suppress column sorting.
    /// </summary>
    public bool IsReorderingEnabled => Feature?.Enabled == true;

    // State management
    public void StartDrag(TGridItem item);
    public void UpdateHover(TGridItem? target, DropPosition? position);
    public void ClearHover();
    public void CancelDrag();

    // Constraint validation
    public bool CanDropOnTarget(TGridItem source, TGridItem target);

    // Reorder execution
    public Task<ReorderResult> ExecuteReorderAsync(
        RowBeforeReorderEventArgs<TGridItem> args,
        Func<RowBeforeReorderEventArgs<TGridItem>, Task> onBeforeReorder,
        Func<RowReorderedEventArgs<TGridItem>, Task> onRowReordered,
        Func<RowReorderCancelledEventArgs<TGridItem>, Task> onReorderCancelled,
        CancellationToken cancellationToken = default);

    // Events for UI refresh
    public event Action? OnStateChanged;

    public void Dispose();
}
```

### 4.4 Data source

Create `ReorderableDataSource<TGridItem>`:

```csharp
public sealed class ReorderableDataSource<TGridItem> : IDisposable
    where TGridItem : class, IRowIdentifiable
{
    private readonly List<TGridItem> _orderedItems;
    private readonly Dictionary<int, double> _orderIndices; // ItemId → OrderIndex

    public ReorderableDataSource(IEnumerable<TGridItem> items);

    // Grid binding
    public IQueryable<TGridItem> Items => _orderedItems.AsQueryable();
    public IReadOnlyList<TGridItem> CurrentOrder => _orderedItems;

    // Events
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

    public void Dispose();
}
```

### 4.4.1 DataSource Error Behavior (normative)

| Method | Error Condition | Behavior |
|--------|-----------------|----------|
| `MoveItem(fromIndex, toIndex)` | Index out of range | MUST throw `ArgumentOutOfRangeException` |
| `MoveItem(item, toIndex)` | Item not found | MUST throw `ArgumentException` |
| `MoveItemBefore/After` | Item or target not found | MUST throw `ArgumentException` |
| `IndexOf` | Item not found | MUST return `-1` (not throw) |
| `SetOrderIndices` | Indices don't match current items | MUST throw `ArgumentException` with message explaining mismatch |
| `UpdateItems` | `null` items | MUST throw `ArgumentNullException` |

### 4.4.2 Order Tracking Algorithm

Order is tracked by **Item ID** using fractional `double` indices to enable insertion without renumbering:

**Insertion algorithm for `MoveItemBefore(item, target)`:**
```csharp
// Find the item before target (if any)
var targetIndex = _orderIndices[target.Id];
var itemBefore = _orderedItems
    .Where(x => _orderIndices[x.Id] < targetIndex)
    .OrderByDescending(x => _orderIndices[x.Id])
    .FirstOrDefault();

if (itemBefore is null)
{
    // Insert at beginning
    _orderIndices[item.Id] = targetIndex - 1.0;
}
else
{
    // Insert between itemBefore and target
    var beforeIndex = _orderIndices[itemBefore.Id];
    _orderIndices[item.Id] = (beforeIndex + targetIndex) / 2.0;
}
```

### 4.5 Helper utilities

Create under `ComposableColumns.Features.Reordering`:

```csharp
internal static class ReorderingHelpers
{
    public static bool IsSyntheticRow<TGridItem>(TGridItem item) 
        where TGridItem : IRowIdentifiable
    {
        return item.Id < 0;
    }
}
```

---

## 5. `RowReorderFeature` internal design

### 5.1 Service registration + single-feature constraint

- `RowReorderFeature.OnAttach(...)` MUST cache the provided `FeatureContext<TGridItem>` in a private field.
- `RowReorderFeature.OnAttach(...)` MUST obtain the grid via cascaded `ComposableGrid<TGridItem>`.
- `RowReorderFeature.OnAttach(...)` MUST call `grid.GetOrCreateReorderCoordinator()`.
- If the coordinator already has a registered feature (`coordinator.Feature != null`), throw `InvalidOperationException` with message: "Only one RowReorderFeature is permitted per grid."
- Register this feature with the coordinator: `coordinator.Feature = this`.

### 5.2 DataSource validation

- On first drag attempt, if `coordinator.DataSource` is `null`, throw `InvalidOperationException` with message: "RowReorderFeature requires a ReorderableDataSource. Bind the grid's Items to ReorderableDataSource.Items."

### 5.3 `InvokeAsync` rule

- `FeatureContext.InvokeAsync` is required for event callbacks.
- If `InvokeAsync` is `null`, the feature assumes it is already on the UI thread.

### 5.4 Rendering contract

- `RenderCell(...)` MUST check `IsSyntheticRow(item)` and render empty cell if true.
- `RenderCell(...)` MUST check `CanDrag` predicate and render disabled handle if false.
- `RenderCell(...)` renders drag handle with HTML5 Drag & Drop attributes.
- `RenderCell(...)` MUST NOT call `renderNext()` (feature owns the entire cell content).

**ARIA attributes (initial implementation):**
```html
<!-- Default state -->
<div class="reorder-handle"
     role="button"
     aria-label="Drag to reorder"
     tabindex="0">

<!-- During drag (dynamic) -->
<div class="reorder-handle"
     aria-grabbed="true"
     aria-dropeffect="move">
```

### 5.5 Drag start algorithm

`OnDragStart(item, e)`:

1. Validate DataSource is available.
2. If `IsSyntheticRow(item)`, return (no-op).
3. If `CanDrag?.Invoke(item) == false`, return (no-op).
4. If expansion feature is active and item is expanded, collapse it first.
5. `coordinator.StartDrag(item)`.
6. Raise `coordinator.OnStateChanged`.

### 5.6 Drag over algorithm

`OnDragOver(item, e)`:

1. If not dragging, return.
2. If `IsSyntheticRow(item)`, show invalid cursor, return.
3. **If dragged item is no longer in visible items (filtered out during drag), cancel drag and return.**
4. If `CanDropOn?.Invoke(draggedItem, item) == false`, show invalid cursor, return.
5. If grouping is active and `!AllowCrossGroupReorder` and items are in different groups, show invalid cursor, return.
6. Calculate `DropPosition` from `e.ClientY`.
7. `coordinator.UpdateHover(item, position)`.
8. Raise `coordinator.OnStateChanged`.

### 5.7 Drop algorithm

`OnDrop(item, e)`:

1. If not dragging, return.
2. Validate drop target (same checks as OnDragOver).
3. Build `RowBeforeReorderEventArgs`.
4. Execute via `await coordinator.ExecuteReorderAsync(args, ...)`.
5. Coordinator handles event firing and data source mutation:
   - Call `OnBeforeReorder` callback
   - **If `OnBeforeReorder` throws, fire `OnReorderCancelled` with reason "Event handler exception" and return `ReorderResult.Cancelled`**
   - If `args.Cancel == true`, fire `OnReorderCancelled` with `args.CancelReason ?? "Cancelled by event handler"` and return `ReorderResult.Cancelled`
   - Mutate data source order
   - Call `OnRowReordered` callback
6. `coordinator.CancelDrag()` (clears state).
7. Raise `coordinator.OnStateChanged`.

### 5.8 Drag end algorithm

`OnDragEnd(item, e)`:

1. `coordinator.CancelDrag()`.
2. Raise `coordinator.OnStateChanged`.

### 5.9 CSS class model

The feature MUST emit these class names:

- handle cell: `reorder-handle-cell`
- handle element: `reorder-handle`
- disabled handle: `reorder-disabled`
- dragging row: `reorder-dragging`
- drop before: `reorder-drop-before`
- drop after: `reorder-drop-after`
- empty cell (synthetic): `reorder-cell-empty`

All styles MUST live in `wwwroot/css/qgComposable-refined-minimalism.css`.

---

## 6. ComposableGrid integration

### 6.1 Coordinator storage

Add to `ComposableGrid<TGridItem>`:

```csharp
private ReorderCoordinator<TGridItem>? _reorderCoordinator;

internal ReorderCoordinator<TGridItem> GetOrCreateReorderCoordinator()
{
    if (_reorderCoordinator is not null)
        return _reorderCoordinator;
    
    // Reordering requires IRowIdentifiable
    if (!typeof(IRowIdentifiable).IsAssignableFrom(typeof(TGridItem)))
        throw new InvalidOperationException("Reordering requires TGridItem to implement IRowIdentifiable.");
    
    _reorderCoordinator = new ReorderCoordinator<TGridItem>();
    
    // Wire up grouping coordinator if available
    if (_groupingCoordinator is not null)
        _reorderCoordinator.GroupingCoordinator = _groupingCoordinator;
    
    return _reorderCoordinator;
}
```

### 6.2 Sorting suppression

In `ComposableGrid`, when reordering is active, suppress column sorting:

```csharp
private bool IsReorderingActive => _reorderCoordinator?.Feature?.Enabled == true;

// In SortedItems property or wherever SortBy is applied:
if (IsReorderingActive)
{
    // Return FilteredItems without sorting
    return FilteredItems;
}
```

### 6.3 Disposal

In `ComposableGrid.Dispose()`:

```csharp
_reorderCoordinator?.Dispose();
_reorderCoordinator = null;
```

---

## 7. Consumer usage patterns (Blazor)

### 7.1 Column usage (ComposableColumn)

Add a dedicated demo page:

- Create `Pages/ComposableReorderDemo.razor` demonstrating `RowReorderFeature`.

The demo MUST use a concrete demo model type:

```csharp
public sealed class TaskItem : IRowIdentifiable
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Status { get; set; } = "";
    public string Category { get; set; } = "";
    public int Priority { get; set; }
    public bool IsLocked { get; set; }
    public bool IsCompleted { get; set; }
}
```

### 7.2 Required demo scenarios

The demo page MUST include all four scenarios from Spec section 10:

**Scenario 1: Basic Reordering**
- Grid bound to `ReorderableDataSource<TaskItem>.Items`
- `QuickGrid` row key configured to `item => item.Id`
- `ComposableColumn` as first column with `RowReorderFeature<TaskItem>`
- `OnRowReordered` handler that logs the move

**Scenario 2: With Validation (`CanDrag` and `OnBeforeReorder`)**
- `CanDrag="@(item => !item.IsLocked)"` - locked items not draggable
- `CanDropOn="@((source, target) => source.Category == target.Category)"` - same category only
- `OnBeforeReorder` handler that cancels if completed task moved above active task
- `OnReorderCancelled` handler that shows toast notification

**Scenario 3: With Grouping**
- `GroupingFeature` on Status column with `IsActive="true"`
- `AllowCrossGroupReorder="false"` (default)
- Visual feedback when attempting cross-group drop

**Scenario 4: Order Persistence**
- `OnInitializedAsync` loads saved order from localStorage via `SetOrderIndices`
- `OnOrderChanged` auto-saves order to localStorage via `GetOrderIndices`
- Demonstrates order survives page refresh

### 7.3 Demo page structure

```razor
@page "/composable-reorder-demo"

<h1>Row Reordering Demo</h1>

<h2>Basic Reordering</h2>
<!-- Scenario 1 -->

<h2>With Validation</h2>
<!-- Scenario 2 -->

<h2>With Grouping</h2>
<!-- Scenario 3 -->

<h2>Order Persistence</h2>
<!-- Scenario 4 -->
```

---

## 8. Test strategy

Tests MUST be unit tests in `QuickGridTest01.Tests` for the non-UI types only:

- `ReorderableDataSource<TGridItem>`
  - `MoveItem_ValidIndices_UpdatesOrder`
  - `MoveItem_InvalidIndex_ThrowsArgumentOutOfRangeException`
  - `MoveItemBefore_ValidTarget_UpdatesOrder`
  - `MoveItemAfter_ValidTarget_UpdatesOrder`
  - `IndexOf_ItemExists_ReturnsIndex`
  - `IndexOf_ItemNotFound_ReturnsMinusOne`
  - `GetOrderIndices_ReturnsItemIdsInOrder`
  - `SetOrderIndices_RestoresOrder`
  - `UpdateItems_NewItems_AppendToEnd`
  - `UpdateItems_RemovedItems_RemoveFromOrder`
  - `OnOrderChanged_FiresAfterMutation`

- `ReorderCoordinator<TGridItem>`
  - `StartDrag_SetsState`
  - `UpdateHover_SetsTarget`
  - `CancelDrag_ClearsState`
  - `CanDropOnTarget_SyntheticRow_ReturnsFalse`
  - `CanDropOnTarget_DifferentGroup_ReturnsFalse_WhenNotAllowed`
  - `CanDropOnTarget_SameGroup_ReturnsTrue`

- `ReorderingHelpers`
  - `IsSyntheticRow_NegativeId_ReturnsTrue`
  - `IsSyntheticRow_PositiveId_ReturnsFalse`

No component test framework (bUnit) is introduced as part of this feature.

---

## 9. Execution sequence (implementation order)

1. Add `FeaturePriority.Reordering = 325` to `FeaturePriority.cs`.
2. Create enums (`DropPosition`, `ReorderTriggerMode`, `ReorderResult`).
3. Create event args classes.
4. Create `ReorderableDataSource<TGridItem>`.
5. Create `ReorderCoordinator<TGridItem>`.
6. Create `ReorderingHelpers`.
7. Implement `RowReorderFeature<TGridItem>`.
8. Add coordinator support to `ComposableGrid<TGridItem>`.
9. Add sorting suppression logic to `ComposableGrid<TGridItem>`.
10. Add CSS to `wwwroot/css/qgComposable-refined-minimalism.css`.
11. Create `Pages/ComposableReorderDemo.razor`.
12. Add unit tests for non-UI types.
13. Build + run tests.

---

## 10. Acceptance criteria

The feature is complete when:

1. `RowReorderFeature<TGridItem>` compiles and is usable from a `ComposableColumn`.
2. Drag handles render for non-synthetic rows when `Enabled = true`.
3. Synthetic rows (negative IDs) render empty cells.
4. HTML5 Drag & Drop events fire correctly and update coordinator state.
5. Drop position (Before/After) is calculated correctly from mouse position.
6. `OnBeforeReorder` event is cancellable and prevents order mutation when cancelled.
7. `OnRowReordered` event fires with correct `OldIndex`, `NewIndex`, and `NewOrder`.
8. `OnReorderCancelled` event fires with reason when cancelled.
9. Column sorting is suppressed when reordering is enabled.
10. Grouping constraints are enforced when `AllowCrossGroupReorder = false`.
11. `ReorderableDataSource` tracks order by Item ID and supports persistence.
12. New items added via `UpdateItems` append to end of order.
13. All reordering styles are in `wwwroot/css/qgComposable-refined-minimalism.css`.
14. All new types are under `QuickGridTest01.ComposableColumns.Features.Reordering`.
15. The demo page `Pages/ComposableReorderDemo.razor` demonstrates:
    - Basic reordering
    - `CanDrag` predicate (locked items not draggable)
    - `OnBeforeReorder` validation
    - Order persistence to localStorage
    - Reordering with grouping (optional section)
16. Unit tests pass for `ReorderableDataSource` and `ReorderCoordinator`.

---

## 11. Appendix: CSS to add

Add to `wwwroot/css/qgComposable-refined-minimalism.css`:

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

## 12. Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025 | AI Assistant | Initial implementation plan based on spec v0.4 |
| 1.1 | 2025 | AI Assistant | Spec alignment: added section 4.4.1 (DataSource error behavior table), section 4.4.2 (order tracking algorithm); added filtered-out-during-drag edge case to section 5.6; added OnBeforeReorder exception handling to section 5.7; expanded section 7 with all 4 demo scenarios from spec; added `IndexOf` tests; added `IsReorderingEnabled` to coordinator |
