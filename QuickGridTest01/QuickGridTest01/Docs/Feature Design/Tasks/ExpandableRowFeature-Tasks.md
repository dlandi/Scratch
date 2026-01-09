# Task Execution — Expandable Row Feature (`RowExpandFeature<TGridItem>`)

## Source
- `Docs/Feature Design/ImplementationPlans/Plan_ExpandableRowFeature.md`

## Conventions
- Task Ids are `M<Milestone>.P<Phase>.T<Task>` (e.g., `M1.P1.T1`).
- Legacy code under `QuickGridTest01.RowColumn.*` remains unchanged.

---

## M1 — ComposableColumns plumbing prerequisites

### P1 — Priority + integration prerequisites

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M1.P1.T1 | Update feature priorities | Modify `ComposableColumns/Core/FeaturePriority.cs` to add `FeaturePriority.Expansion = 350`. |
| M1.P1.T2 | Confirm `FeatureContext` dispatcher/refresh invariants | Identify the exact assignment sites in `ComposableColumns/Core/ComposableColumn.cs` (or related) where `FeatureContext.InvokeAsync` and `FeatureContext.RequestRefreshAsync` are set; record the file+method names in the task execution report. |
| M1.P1.T3 | Define required guard failures (sad path) | Produce a small “Guard Failures” list (exception type + message text) for: missing `InvokeAsync`, missing `RequestRefreshAsync`, missing `ExpandedTemplate`, invalid `ExpandedRowSpan`/`RowHeight` (<= 0), duplicate `RowStateManager<TGridItem>` registration. Store this list in the task execution report. |
| M1.P1.T4 | Interface alignment check (Core) | Produce a checklist in the task execution report confirming: `OnAttach/OnDetach` signatures, `RenderCell` signature, nullable `InvokeAsync/RequestRefreshAsync/RequestRefresh/RowKey`, and the decided behavior for missing delegates (throw vs fallback). |

---

## M2 — Create Expansion feature contract types (Composable-only)

### P1 — Core identity + helpers

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P1.T1 | Create `IRowIdentifiable` | Add `ComposableColumns/Features/Expansion/Core/IRowIdentifiable.cs` with `int Id { get; set; }`. |
| M2.P1.T2 | Create spacer helper extension | Add `ComposableColumns/Features/Expansion/Core/RowIdentifiableExtensions.cs` with `IsSpacer()` based on `Id < 0`. |
| M2.P1.T3 | Interface alignment check (Identity) | Produce a checklist in the task execution report stating: `FeatureContext.RowKey` is `Func<TGridItem, object>?`; expansion uses `item.Id` as the canonical int key; non-int row key values are ignored (fall back to `item.Id`). |

### P2 — Enums + contexts + events

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P2.T1 | Create enums | Add `RowTriggerMode`, `ConcurrentExpandBehavior`, `RowExpandedState` under `ComposableColumns/Features/Expansion/`. |
| M2.P2.T2 | Create context objects | Add `RowDisplayContext<TGridItem>` and `RowExpandedContext<TGridItem>` under `ComposableColumns/Features/Expansion/`. |
| M2.P2.T3 | Create event args | Add `ComposableColumns/Features/Expansion/Events/RowEventArgs.cs` containing `RowBeforeExpandEventArgs<TGridItem>`, `RowExpandedEventArgs<TGridItem>`, `RowCollapsedEventArgs<TGridItem>`, `RowStateChangedEventArgs<TGridItem>`. |
| M2.P2.T4 | Interface alignment check (Events/Templates) | Produce a checklist in the task execution report confirming: templates are `RenderFragment<TContext>`; events are `EventCallback<T>`; and event invocation uses `EventCallback<T>.InvokeAsync(...)` (no manual receiver management required for invocation). |

---

## M3 — Create Expansion state + spacer collaboration implementations

### P1 — Spacer encoding + expandable data source

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P1.T1 | Implement `SpacerRowFactory` | Add `ComposableColumns/Features/Expansion/Data/SpacerRowFactory.cs` implementing encode/decode rules and spacer checks. |
| M3.P1.T2 | Implement `ExpandableGridDataSource<TGridItem>` | Add `ComposableColumns/Features/Expansion/Data/ExpandableGridDataSource.cs` with `Items : IQueryable<TGridItem>`, `ExpandRow` inserting `span + 1` spacers, `CollapseRow`, `CollapseAll`, and `OnDataChanged`. |
| M3.P1.T3 | Define spacer/id error conditions (sad path) | Implement and document (in test names) the deterministic behavior for: `rowId <= 0` (throw `ArgumentOutOfRangeException`), `spacerCount < 0` (throw `ArgumentOutOfRangeException`), `spacerCount == 0` (do not insert spacers), expanding a already-expanded row (replace spacer block deterministically), collapsing a non-expanded row (no-op), spacer-id overflow (throw `OverflowException`). |
| M3.P1.T4 | Interface alignment check (Data binding) | Produce a checklist in the task execution report confirming: `Items` is compatible with `QuickGrid.Items` usage in this repo (IQueryable binding), and the refresh mechanism used by the demo page is `FeatureContext.RequestRefreshAsync` (not `OnDataChanged`). |

### P2 — Expansion state manager

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P2.T1 | Implement `RowStateManager<TGridItem>` | Add `ComposableColumns/Features/Expansion/State/RowStateManager.cs` implementing `ConditionalWeakTable` contexts, expanded row tracking, and async add/remove/clear operations with locking. |
| M3.P2.T2 | Interface alignment check (Service lifecycle) | Produce a checklist in the task execution report confirming: `RowStateManager<TGridItem>` is registered via `FeatureContext.RegisterService`, retrieved via `GetService`, and disposed via `FeatureContext.Clear()` when disposable. |

---

## M4 — Implement `RowExpandFeature<TGridItem>`

### P1 — Feature lifecycle + service registration

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P1.T1 | Create feature skeleton | Add `ComposableColumns/Features/Expansion/RowExpandFeature.cs` implementing `ICellRenderFeature<TGridItem>, IDisposable`. |
| M4.P1.T2 | Implement `OnAttach` invariants | Cache context; throw if `InvokeAsync` is null; throw if `RequestRefreshAsync` is null; throw if `RowStateManager<TGridItem>` already registered (sentinel). |
| M4.P1.T3 | Implement lazy service creation | In `RenderCell`, `GetService<RowStateManager<TGridItem>>()` else create and `RegisterService`. |
| M4.P1.T4 | Implement parameter validation (sad path) | Throw `InvalidOperationException` if `ExpandedTemplate` is null. Throw `ArgumentOutOfRangeException` if `ExpandedRowSpan <= 0` or `RowHeight <= 0`. |
| M4.P1.T5 | Implement argument validation (sad path) | Throw `ArgumentNullException` when public methods are called with `item == null`. Treat a default/empty `CancellationToken` as valid; if `cancellationToken.IsCancellationRequested` then honor cancellation by throwing `OperationCanceledException` before mutating state. |
| M4.P1.T6 | Interface alignment check (Feature pipeline) | Produce a checklist in the task execution report confirming: `RowExpandFeature` implements `ICellRenderFeature<TGridItem>` correctly; `RenderCell(...)` is allowed to not call `renderNext()`; and `OnDetach` is invoked by `ComposableColumn.Dispose()`. |

### P2 — Expand/collapse methods + event ordering

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P2.T1 | Implement `ExpandRowAsync` ordering | Add spacer guard, cancellable `OnBeforeExpand`, concurrency enforcement, context creation, optional `DataSource.ExpandRow`, then `OnStateChanged` -> `OnExpanded` -> `RequestRefreshAsync`. |
| M4.P2.T2 | Implement `CollapseRowAsync` ordering | Remove state, optional `DataSource.CollapseRow`, then `OnStateChanged` -> `OnCollapsed` -> `RequestRefreshAsync`. |
| M4.P2.T3 | Implement `CollapseAllAsync` | Clear state, optional `DataSource.CollapseAll`, then `RequestRefreshAsync`. |
| M4.P2.T4 | Enforce dispatcher boundary | Ensure expand/collapse operations and event callbacks execute within `context.InvokeAsync`. |
| M4.P2.T5 | Implement safe no-op rules (sad path) | `ExpandRowAsync` MUST no-op for spacer rows. `CollapseRowAsync` MUST no-op if the row is not expanded. `CollapseAllAsync` MUST no-op if no rows are expanded. |
| M4.P2.T6 | Implement `DataSource` misalignment handling (sad path) | If `DataSource` is supplied but the grid is not bound to `DataSource.Items`, document the supported behavior as: UI may overlap; no exception is thrown; demo and docs require binding to `DataSource.Items`. |
| M4.P2.T7 | Validate row identity assumptions (sad path) | If `item.Id <= 0` (spacer or invalid) then `ExpandRowAsync` MUST no-op (spacer) or throw (`Id == 0` treated as invalid input). |
| M4.P2.T8 | Interface alignment check (Dispatch + callbacks) | Produce a checklist in the task execution report confirming: all operations are wrapped in `FeatureContext.InvokeAsync`; `EventCallback<T>.InvokeAsync` is used for events; and refresh uses `FeatureContext.RequestRefreshAsync`. |

---

## M5 — Create composable `RowCard` component

### P1 — Component parity + close fallback

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P1.T1 | Create composable `RowCard` | Add `ComposableColumns/Features/Expansion/Components/RowCard.razor` with parameters matching legacy: `Title`, `Class`, `HeaderActions`, `FooterContent`, `ShowCloseButton`, `OnClose`, `ChildContent`. |
| M5.P1.T2 | Implement close fallback | If `OnClose` is null, invoke cascaded `RowExpandedContext<TGridItem>.CollapseAsync`. |
| M5.P1.T3 | Interface alignment check (Cascading) | Produce a checklist in the task execution report confirming: `RowExpandedContext<TGridItem>` is cascaded by `RowExpandFeature`; `RowCard` consumes it; and `RowCard` behavior with no cascading context is a safe no-op (close does nothing if `OnClose` is also null). |

---

## M6 — Styling (global stylesheet)

### P1 — Add required CSS selectors

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M6.P1.T1 | Add expansion CSS | Modify `wwwroot/css/qgComposable-refined-minimalism.css` to **reuse existing selector rules when present** and **add only missing selectors** for `.row-cell`, `.row-expanded`, `.row-dimmed`, `.row-overlay`, `.row-click-indicator`, `.row-spacer`. |
| M6.P1.T2 | Interface alignment check (CSS contract) | Produce a checklist in the task execution report confirming: emitted class names match selector names, and the overlay CSS positioning uses `top: 100%` and the configured height behavior. |

---

## M7 — Demo page

### P1 — Create demo page bound to expandable data source

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M7.P1.T1 | Create demo page | Add `Pages/ComposableRowExpandDemo.razor` using `DemoRow : IRowIdentifiable` (`Id`, `Name`, `Email`), bind `QuickGrid.Items` to `ExpandableGridDataSource<DemoRow>.Items`, configure row key as `item => item.Id`, host `RowExpandFeature<DemoRow>` in a `ComposableColumn<DemoRow, string>`, and use composable `RowCard` in `ExpandedTemplate`. |
| M7.P1.T2 | Interface alignment check (Demo wiring) | Produce a checklist in the task execution report confirming: demo binds to `.Items`, row key is set, and the expansion column is rendered via the ComposableColumns feature pipeline. |

---

## M8 — Automated tests (non-UI)

### P1 — Add unit tests for core types

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M8.P1.T1 | Add `SpacerRowFactory` tests | Unit tests validating encode/decode and spacer detection. |
| M8.P1.T2 | Add `ExpandableGridDataSource<T>` tests | Unit tests validating spacer insertion count (`ExpandedRowSpan + 1`), ordering, and collapse behavior. |
| M8.P1.T3 | Add `RowStateManager<T>` tests | Unit tests validating expand state, remove, clear-all, and first-expanded behavior. |
| M8.P1.T4 | Add sad-path tests for non-UI types | Add unit tests for: invalid row ids rejected by `ExpandableGridDataSource`, invalid spacer counts rejected, collapsing non-expanded rows is safe, repeated expand replaces spacer blocks deterministically, and spacer-id overflow throws. |
| M8.P1.T5 | Add sad-path tests for feature inputs | Add unit tests for: `ExpandRowAsync(null)` throws `ArgumentNullException`, `ExpandRowAsync(item with Id==0)` throws `ArgumentOutOfRangeException`, canceled token throws `OperationCanceledException` before state mutation. |
| M8.P1.T6 | Interface alignment check (Test coverage) | Ensure tests include at least one compile-time assertion of the `ICellRenderFeature<T>` method signature usage and explicitly validate nullable delegate guards (`InvokeAsync`, `RequestRefreshAsync`). |

---

## M10 — Completion checklist

### P1 — Final validation against plan

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M10.P1.T1 | Validate acceptance criteria | Verify all acceptance criteria in `Plan_ExpandableRowFeature.md` are satisfied. |
| M10.P1.T2 | Validate sad-path behaviors | Verify guard failures, no-op rules, and `DataSource` misalignment behavior match this tasks document. |
| M10.P1.T3 | Interface alignment check (Final) | Confirm the final implementation compiles cleanly and all feature calls align with current `ComposableColumns` interfaces (`IColumnFeature`, `ICellRenderFeature`, `FeatureContext`), with no unused/assumed members. |