# Task Execution — Row Reordering Feature (`RowReorderFeature<TGridItem>`)

## Source
- `Docs/Feature Design/ImplementationPlans/Plan_RowReorderingFeature.md`

## Conventions
- Task Ids are `M<Milestone>.P<Phase>.T<Task>` (e.g., `M1.P1.T1`).
- Legacy code under `QuickGridTest01.RowColumn.*` remains unchanged.
- All feature logic must live under `QuickGridTest01.ComposableColumns.*` (spec namespace rule).
- All CSS for this feature must be placed in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (no `*.razor.css` for feature styling).
- Only one `RowReorderFeature` is permitted per grid (single-feature constraint).
- `ReorderableDataSource<TGridItem>` is **user-provided**, not coordinator-owned.

---

## M1 — ComposableColumns plumbing prerequisites

### P1 — Priority + integration prerequisites

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M1.P1.T1 | Update feature priorities | Modify `ComposableColumns/Core/FeaturePriority.cs` to add `FeaturePriority.Reordering = 325` (after Styling at 300, before Expansion at 350). |
| M1.P1.T2 | Confirm `FeatureContext` dispatcher/refresh invariants | Identify the exact assignment sites in `ComposableColumns/Core/ComposableColumn.cs` (or related) where `FeatureContext.InvokeAsync` is set; record `file + method` names in the task execution report. Note: reordering uses `InvokeAsync` for event callbacks but does NOT require `RequestRefreshAsync` (coordinator uses `OnStateChanged` event). |
| M1.P1.T3 | Define required guard failures (sad path) | Produce a "Guard Failures" list (exception type + message text) for: duplicate feature registration (`InvalidOperationException`: "Only one RowReorderFeature is permitted per grid"), missing `DataSource` on drag (`InvalidOperationException`: "RowReorderFeature requires a ReorderableDataSource..."), `TGridItem` not implementing `IRowIdentifiable` (`InvalidOperationException`: "Reordering requires TGridItem to implement IRowIdentifiable"), invalid `RowHeight` (<= 0), and data source method errors (see Plan §4.4.1). Store the list in the task execution report. |
| M1.P1.T4 | Interface alignment check (Core) | Produce a checklist in the task execution report confirming: `IColumnFeature.OnAttach/OnDetach` signatures, `ICellRenderFeature.RenderCell` signature, nullable `InvokeAsync` expectations, and that `RenderCell` MUST NOT call `renderNext()` (feature owns entire cell content). |
| M1.P1.T5 | Verify synthetic row detection API | Confirm that `IRowIdentifiable` exists in `ComposableColumns/Features/Expansion/Core/` and that negative `Id` values indicate synthetic rows (grouping markers, expansion spacers). Reordering will reuse this detection pattern. |

---

## M2 — Create Reordering feature contract types

### P1 — Enums + event args

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P1.T1 | Create enums | Add enums under `ComposableColumns/Features/Reordering/`: `DropPosition` (Before, After), `ReorderTriggerMode` (HandleOnly, EntireRow), `ReorderResult` (Success, Cancelled, Failed) per Plan §4.2. |
| M2.P1.T2 | Create event args | Add `RowBeforeReorderEventArgs<TGridItem>`, `RowReorderedEventArgs<TGridItem>`, `RowReorderCancelledEventArgs<TGridItem>` under `ComposableColumns/Features/Reordering/` per Plan §4.1. |
| M2.P1.T3 | Interface alignment check (Events) | Produce a checklist confirming: event args extend `EventArgs`; `RowBeforeReorderEventArgs` has `Cancel` and `CancelReason` properties; events are `EventCallback<T>`; and type constraint is `where TGridItem : class, IRowIdentifiable`. |

### P2 — Helper utilities

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P2.T1 | Create `ReorderingHelpers` | Add `ComposableColumns/Features/Reordering/ReorderingHelpers.cs` implementing `IsSyntheticRow<TGridItem>(item)` returning `item.Id < 0` per Plan §4.5. |
| M2.P2.T2 | Interface alignment check (Helpers) | Produce a checklist confirming: helper is `internal static` class; method uses `IRowIdentifiable` constraint; negative IDs are synthetic (matches grouping/expansion pattern). |

---

## M3 — Implement `ReorderableDataSource<TGridItem>`

### P1 — Data source implementation

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P1.T1 | Implement `ReorderableDataSource<TGridItem>` skeleton | Add `ComposableColumns/Features/Reordering/ReorderableDataSource.cs` implementing `IDisposable` with constructor accepting `IEnumerable<TGridItem>`, `Items` property (`IQueryable<TGridItem>`), `CurrentOrder` property (`IReadOnlyList<TGridItem>`), and `OnOrderChanged` event per Plan §4.4. |
| M3.P1.T2 | Implement order tracking with fractional indices | Implement internal `Dictionary<int, double> _orderIndices` mapping ItemId → OrderIndex. Lower values appear first. Fractional indices enable insertion without renumbering per Plan §4.4.2. |
| M3.P1.T3 | Implement `MoveItem` methods | Implement `MoveItem(int fromIndex, int toIndex)`, `MoveItem(TGridItem item, int toIndex)`, `MoveItemBefore(TGridItem item, TGridItem target)`, `MoveItemAfter(TGridItem item, TGridItem target)` with fractional index insertion algorithm per Plan §4.4.2. Fire `OnOrderChanged` after each mutation. |
| M3.P1.T4 | Implement `IndexOf` method | Implement `IndexOf(TGridItem item)` returning index in `_orderedItems` or `-1` if not found (MUST NOT throw). |
| M3.P1.T5 | Implement persistence methods | Implement `GetOrderIndices()` returning `IReadOnlyList<int>` of Item IDs in current order, `SetOrderIndices(IReadOnlyList<int> indices)` to restore order, and `ResetOrder()` to restore original order. |
| M3.P1.T6 | Implement `UpdateItems` method | Implement `UpdateItems(IEnumerable<TGridItem> items, bool preserveOrder = true)` per Plan §4.4: new items append to end (`MaxOrderIndex + 1`); removed items lose their order indices; re-added items treated as new. |
| M3.P1.T7 | Define error behavior (sad path) | Implement normative error behavior per Plan §4.4.1: `MoveItem(fromIndex, toIndex)` throws `ArgumentOutOfRangeException` for invalid index; `MoveItem(item, toIndex)` throws `ArgumentException` if item not found; `MoveItemBefore/After` throws `ArgumentException` if item or target not found; `SetOrderIndices` throws `ArgumentException` if indices don't match current items; `UpdateItems(null)` throws `ArgumentNullException`. |
| M3.P1.T8 | Interface alignment check (Data source) | Produce a checklist confirming: `Items` is compatible with `QuickGrid.Items` (IQueryable binding); `OnOrderChanged` fires after every successful mutation; fractional indices are transparent to consumers (they see integer indices in `GetOrderIndices`). |

---

## M4 — Implement `ReorderCoordinator<TGridItem>`

### P1 — Coordinator implementation

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P1.T1 | Implement `ReorderCoordinator<TGridItem>` skeleton | Add `ComposableColumns/Features/Reordering/ReorderCoordinator.cs` implementing `IDisposable` with drag state properties (`DraggedItem`, `HoveredTarget`, `CurrentDropPosition`, `IsDragging`), `DataSource` reference, `GroupingCoordinator` reference, `Feature` reference, and `OnStateChanged` event per Plan §4.3. |
| M4.P1.T2 | Implement `IsReorderingEnabled` property | Implement `IsReorderingEnabled => Feature?.Enabled == true` for use by `ComposableGrid` to suppress column sorting. |
| M4.P1.T3 | Implement state management methods | Implement `StartDrag(TGridItem item)`, `UpdateHover(TGridItem? target, DropPosition? position)`, `ClearHover()`, `CancelDrag()` methods. Each method updates state and optionally raises `OnStateChanged`. |
| M4.P1.T4 | Implement `CanDropOnTarget` validation | Implement `CanDropOnTarget(TGridItem source, TGridItem target)` per Plan: return `false` for synthetic rows; if grouping is active and `!AllowCrossGroupReorder`, check same group via `GroupingCoordinator.ActiveGrouping.GetGroupKey`. |
| M4.P1.T5 | Implement `ExecuteReorderAsync` | Implement `ExecuteReorderAsync(args, onBeforeReorder, onRowReordered, onReorderCancelled, ct)` per Plan §5.7: call `onBeforeReorder`; if throws, fire `onReorderCancelled` with reason "Event handler exception"; if `args.Cancel`, fire `onReorderCancelled` with `args.CancelReason`; otherwise mutate data source and fire `onRowReordered`; return appropriate `ReorderResult`. |
| M4.P1.T6 | Implement `Dispose` | Clear all state, unsubscribe from events, set `Feature = null`. |
| M4.P1.T7 | Interface alignment check (Coordinator) | Produce a checklist confirming: coordinator is grid-owned (not feature-owned); `DataSource` is user-provided (set externally); `AllowCrossGroupReorder` is read from `Feature` parameter; `OnStateChanged` is used for UI refresh (not `RequestRefreshAsync`). |

---

## M5 — Implement `RowReorderFeature<TGridItem>`

### P1 — Feature lifecycle + service registration

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P1.T1 | Create feature skeleton | Add `ComposableColumns/Features/Reordering/RowReorderFeature.cs` implementing `ICellRenderFeature<TGridItem>, IDisposable` with `Priority => FeaturePriority.Reordering` (325) and type constraint `where TGridItem : class, IRowIdentifiable`. |
| M5.P1.T2 | Implement parameters | Add all parameters from Plan §3.2: `Enabled`, `TriggerMode`, `AllowCrossGroupReorder`, `RowHeight`, `DragHandleContent`, `DragHandleIcon`, `DragHandleTemplate`, CSS class parameters (`HandleCellClass`, `HandleClass`, etc.), `CanDrag`, `CanDropOn`, and event callbacks (`OnBeforeReorder`, `OnRowReordered`, `OnReorderCancelled`). |
| M5.P1.T3 | Implement `OnAttach` invariants | Cache context; obtain grid via cascade; call `grid.GetOrCreateReorderCoordinator()`; if `coordinator.Feature != null` throw `InvalidOperationException("Only one RowReorderFeature is permitted per grid.")`; register `coordinator.Feature = this`. |
| M5.P1.T4 | Implement `OnDetach` | Unregister from coordinator (`coordinator.Feature = null` if `coordinator.Feature == this`); call `Dispose()`. |
| M5.P1.T5 | Interface alignment check (Feature lifecycle) | Produce a checklist confirming: `OnAttach` enforces single-feature constraint; `OnDetach` cleans up; `InvokeAsync` null behavior is "assume UI thread" (not throw). |

### P2 — Cell rendering

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P2.T1 | Implement `RenderCell` method | Implement per Plan §5.4: check `IsSyntheticRow(item)` → render empty cell with `reorder-cell-empty` class; check `CanDrag` predicate → render disabled handle if false; render drag handle with HTML5 Drag & Drop attributes (`draggable="true"`, event handlers); MUST NOT call `renderNext()`. |
| M5.P2.T2 | Implement drag handle rendering | Render `<div>` with `HandleClass`, `draggable="true"`, ARIA attributes (`role="button"`, `aria-label="Drag to reorder"`, `tabindex="0"`), and content from `DragHandleTemplate` or `DragHandleIcon` or `DragHandleContent` (fallback order). |
| M5.P2.T3 | Implement dynamic CSS class building | Build cell class combining `HandleCellClass`, `DraggingRowClass` (when item is dragged), `DragOverBeforeClass`/`DragOverAfterClass` (when item is hover target). |
| M5.P2.T4 | Interface alignment check (Rendering) | Produce a checklist confirming: `RenderCell` does NOT call `renderNext()`; synthetic rows render empty cells; disabled handles have `DisabledHandleClass`; ARIA attributes are present. |

### P3 — Drag & Drop event handlers

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P3.T1 | Implement `OnDragStart` | Per Plan §5.5: validate DataSource exists (throw if null on first drag); if synthetic row, no-op; if `CanDrag` returns false, no-op; if expansion feature active and item expanded, collapse first; call `coordinator.StartDrag(item)`; raise `OnStateChanged`. |
| M5.P3.T2 | Implement `OnDragOver` | Per Plan §5.6: if not dragging, return; if synthetic row, show invalid cursor, return; if dragged item filtered out during drag, cancel drag, return; if `CanDropOn` returns false, show invalid cursor, return; if grouping constraint violated, show invalid cursor, return; calculate `DropPosition` from `e.ClientY`; call `coordinator.UpdateHover(item, position)`; raise `OnStateChanged`. |
| M5.P3.T3 | Implement `OnDrop` | Per Plan §5.7: if not dragging, return; validate drop target; build `RowBeforeReorderEventArgs`; call `await coordinator.ExecuteReorderAsync(...)`; call `coordinator.CancelDrag()`; raise `OnStateChanged`. |
| M5.P3.T4 | Implement `OnDragEnd` | Per Plan §5.8: call `coordinator.CancelDrag()`; raise `OnStateChanged`. |
| M5.P3.T5 | Implement `OnDragLeave` | Clear hover state if leaving current target: call `coordinator.ClearHover()` if appropriate; raise `OnStateChanged`. |
| M5.P3.T6 | Implement `GetDropPosition` | Per Plan §8.3: calculate `DropPosition.Before` if `e.ClientY % RowHeight < RowHeight / 2`, else `DropPosition.After`. |
| M5.P3.T7 | Define no-op rules (sad path) | Document in test names: `OnDragStart` no-ops for synthetic rows and items where `CanDrag` returns false; `OnDragOver` no-ops when not dragging; `OnDrop` no-ops when not dragging or target invalid; `OnDragEnd` always clears state. |
| M5.P3.T8 | Interface alignment check (Drag & Drop) | Produce a checklist confirming: all handlers use HTML5 Drag & Drop API via `DragEventArgs`; `InvokeAsync` wraps event callbacks; `OnStateChanged` triggers UI refresh; no `RequestRefreshAsync` usage. |

---

## M6 — Integrate reordering into `ComposableGrid<TGridItem>`

### P1 — Coordinator storage + sorting suppression

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M6.P1.T1 | Add grid-owned reorder coordinator | Modify `ComposableColumns/Core/ComposableGrid.razor` (code-behind) to add private `_reorderCoordinator` field and `internal ReorderCoordinator<TGridItem> GetOrCreateReorderCoordinator()` method per Plan §6.1. Throw `InvalidOperationException` if `TGridItem` does not implement `IRowIdentifiable`. |
| M6.P1.T2 | Wire grouping coordinator | In `GetOrCreateReorderCoordinator()`, if `_groupingCoordinator` exists, set `_reorderCoordinator.GroupingCoordinator = _groupingCoordinator`. |
| M6.P1.T3 | Implement sorting suppression | Per Plan §6.2: add `IsReorderingActive` property (`_reorderCoordinator?.IsReorderingEnabled == true`); when active, suppress column sorting by ensuring `SortBy` is not applied (return `FilteredItems` instead of `SortedItems`). |
| M6.P1.T4 | Implement disposal | Per Plan §6.3: in `ComposableGrid.Dispose()`, call `_reorderCoordinator?.Dispose()` and set `_reorderCoordinator = null`. |
| M6.P1.T5 | Interface alignment check (Grid integration) | Produce a checklist confirming: coordinator is grid-owned (mirrors grouping pattern); sorting suppression matches grouping suppression pattern; disposal cleans up coordinator. |

---

## M7 — Styling (global stylesheet)

### P1 — Add required CSS selectors

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M7.P1.T1 | Add reordering CSS | Modify `wwwroot/css/qgComposable-refined-minimalism.css` to add selectors for: `.reorder-handle-cell`, `.reorder-handle`, `.reorder-handle:hover`, `.reorder-handle:active`, `.reorder-handle.reorder-disabled`, `.reorder-dragging`, `.reorder-drop-before`, `.reorder-drop-after`, `.reorder-cell-empty`, `.reorder-drop-target`, `.reorder-drop-invalid` per Plan §11. |
| M7.P1.T2 | Interface alignment check (CSS contract) | Produce a checklist confirming: emitted class names match selector names; `cursor: grab`/`grabbing` for handle; `box-shadow` for drop indicators; `opacity` for dragging row. |

---

## M8 — Demo page

### P1 — Create reordering demo page

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M8.P1.T1 | Create demo model | Define `TaskItem : IRowIdentifiable` with properties: `Id`, `Title`, `Status`, `Category`, `Priority`, `IsLocked`, `IsCompleted` per Plan §7.1. |
| M8.P1.T2 | Create demo page structure | Add `Pages/ComposableReorderDemo.razor` with four sections per Plan §7.3: Basic Reordering, With Validation, With Grouping, Order Persistence. |
| M8.P1.T3 | Implement Scenario 1: Basic Reordering | Grid bound to `ReorderableDataSource<TaskItem>.Items`, `QuickGrid` row key `item => item.Id`, first column with `RowReorderFeature<TaskItem>`, `OnRowReordered` handler that logs the move. |
| M8.P1.T4 | Implement Scenario 2: With Validation | `CanDrag="@(item => !item.IsLocked)"`, `CanDropOn="@((source, target) => source.Category == target.Category)"`, `OnBeforeReorder` handler that cancels if completed task moved above active, `OnReorderCancelled` handler that shows message. |
| M8.P1.T5 | Implement Scenario 3: With Grouping | `GroupingFeature` on Status column with `IsActive="true"`, `AllowCrossGroupReorder="false"` (default), demonstrate visual feedback when attempting cross-group drop. |
| M8.P1.T6 | Implement Scenario 4: Order Persistence | `OnInitializedAsync` loads saved order from localStorage via `SetOrderIndices`, `OnOrderChanged` auto-saves to localStorage via `GetOrderIndices`, demonstrate order survives page refresh. |
| M8.P1.T7 | Interface alignment check (Demo wiring) | Produce a checklist confirming: demo binds to `ReorderableDataSource.Items`; row key is set; reorder column is first column; all four scenarios are implemented. |

---

## M9 — Automated tests (non-UI)

### P1 — Add unit tests for `ReorderableDataSource<TGridItem>`

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M9.P1.T1 | Add `ReorderableDataSource` order manipulation tests | Unit tests validating: `MoveItem_ValidIndices_UpdatesOrder`, `MoveItemBefore_ValidTarget_UpdatesOrder`, `MoveItemAfter_ValidTarget_UpdatesOrder`, `OnOrderChanged_FiresAfterMutation`. |
| M9.P1.T2 | Add `ReorderableDataSource` index lookup tests | Unit tests validating: `IndexOf_ItemExists_ReturnsIndex`, `IndexOf_ItemNotFound_ReturnsMinusOne`. |
| M9.P1.T3 | Add `ReorderableDataSource` persistence tests | Unit tests validating: `GetOrderIndices_ReturnsItemIdsInOrder`, `SetOrderIndices_RestoresOrder`. |
| M9.P1.T4 | Add `ReorderableDataSource` update tests | Unit tests validating: `UpdateItems_NewItems_AppendToEnd`, `UpdateItems_RemovedItems_RemoveFromOrder`. |
| M9.P1.T5 | Add `ReorderableDataSource` sad-path tests | Unit tests validating: `MoveItem_InvalidIndex_ThrowsArgumentOutOfRangeException`, `MoveItem_ItemNotFound_ThrowsArgumentException`, `MoveItemBefore_TargetNotFound_ThrowsArgumentException`, `SetOrderIndices_MismatchedIndices_ThrowsArgumentException`, `UpdateItems_NullItems_ThrowsArgumentNullException`. |

### P2 — Add unit tests for `ReorderCoordinator<TGridItem>`

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M9.P2.T1 | Add `ReorderCoordinator` state management tests | Unit tests validating: `StartDrag_SetsState`, `UpdateHover_SetsTarget`, `CancelDrag_ClearsState`. |
| M9.P2.T2 | Add `ReorderCoordinator` constraint validation tests | Unit tests validating: `CanDropOnTarget_SyntheticRow_ReturnsFalse`, `CanDropOnTarget_DifferentGroup_ReturnsFalse_WhenNotAllowed`, `CanDropOnTarget_SameGroup_ReturnsTrue`. |
| M9.P2.T3 | Clarify coordinator testing without InternalsVisibleTo | Because tests must go through public APIs, test coordinator behavior via public properties and methods. Grouping constraint tests may require mock `GroupingCoordinator` setup. |

### P3 — Add unit tests for `ReorderingHelpers`

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M9.P3.T1 | Add `ReorderingHelpers` tests | Unit tests validating: `IsSyntheticRow_NegativeId_ReturnsTrue`, `IsSyntheticRow_PositiveId_ReturnsFalse`, `IsSyntheticRow_ZeroId_ReturnsFalse`. |

### P4 — Interface alignment check (Test coverage)

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M9.P4.T1 | Interface alignment check (Test coverage) | Ensure tests include at least one compile-time signature usage check for `ICellRenderFeature<TGridItem>` and validate data source error behavior matches Plan §4.4.1. |

---

## M10 — Completion checklist

### P1 — Final validation against plan

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M10.P1.T1 | Validate acceptance criteria | Verify all 16 acceptance criteria in `Plan_RowReorderingFeature.md` §10 are satisfied. |
| M10.P1.T2 | Validate sad-path behaviors | Verify guard failures, no-op rules, and error behavior match this tasks document and Plan §4.4.1, §5.5-5.8. |
| M10.P1.T3 | Interface alignment check (Final) | Confirm the final implementation compiles cleanly and all feature calls align with current `ComposableColumns` interfaces (`IColumnFeature`, `ICellRenderFeature`, `FeatureContext`), with no unused/assumed members. |
| M10.P1.T4 | Validate non-goals are not implemented | Confirm keyboard reorder mode, multi-row drag, cross-grid drag, touch support, and auto-scroll are NOT implemented (per Plan non-goals). |
