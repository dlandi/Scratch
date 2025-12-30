# Task Execution — Row Grouping Feature (`GroupingFeature<TGridItem, TValue>`)

## Source
- `Docs/Feature Design/ImplementationPlans/Plan_RowGroupingFeature.md`

## Conventions
- Task Ids are `M<Milestone>.P<Phase>.T<Task>` (e.g., `M1.P1.T1`).
- Legacy code under `QuickGridTest01.RowColumn.*` remains unchanged.
- All feature logic must live under `QuickGridTest01.ComposableColumns.*` (spec namespace rule).
- All CSS for this feature must be placed in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (no `*.razor.css` for feature styling).
- Activation is **attach-time only** (“first wins” during `OnAttach`). Switching active grouping in the demo must be done by re-rendering with new feature instances (per plan demo notes).
- **No runtime feature injection:** Do not rely on adding features after column initialization. If cross-column behavior is required (e.g., blanking synthetic grouping rows in non-host columns), implement it via a core-owned always-present cell feature path.

---

## M1 — ComposableColumns plumbing prerequisites

### P1 — Priority + integration prerequisites

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M1.P1.T1 | Update feature priorities | Modify `ComposableColumns/Core/FeaturePriority.cs` to add `FeaturePriority.Grouping = 50` (before `Core (100)`). |
| M1.P1.T2 | Confirm `FeatureContext` dispatcher/refresh invariants | Identify the exact assignment sites in `ComposableColumns/Core/ComposableColumn.cs` (or related) where `FeatureContext.InvokeAsync` and `FeatureContext.RequestRefreshAsync/RequestRefresh` are set; record `file + method` names in the task execution report. |
| M1.P1.T3 | Define required guard failures (sad path) | Produce a “Guard Failures” list (exception type + message text) for: missing `InvokeAsync`, unsupported `FilterBehavior = GroupThenFilter`, active grouping with `TGridItem` not implementing `IRowIdentifiable`, invalid `GroupHeaderSlotSpan (< 1)`, invalid `GroupHeaderRowId` inputs (encode/decode), duplicate column id registration, and any required null checks. Store the list in the task execution report. |
| M1.P1.T4 | Interface alignment check (Core) | Produce a checklist in the task execution report confirming: `IColumnFeature.OnAttach/OnDetach` signatures, the cell render feature signature you will use for header-host rendering, nullable delegate expectations (`InvokeAsync`, `RequestRefreshAsync`, `RequestRefresh`), and the feature’s refresh authority rule: grouping uses `GroupedGridDataSource.OnDataChanged` → grid `InvokeAsync(StateHasChanged)` (not `RequestRefreshAsync`) for expand/collapse. |
| M1.P1.T5 | Add Core grouping synthetic-id detection API | Add a minimal detection API under `ComposableColumns/Core/` for identifying grouping marker/spacer synthetic row ids. This must be sufficient for core blanking decisions and should not require `ComposableColumns.Core` to reference `Features.Grouping`. Update spec/plan references as needed. |
| M1.P1.T6 | Add core-owned grouping blanking feature path | Implement an always-present `ICellRenderFeature<TGridItem>` added by `ComposableColumn` prior to initialization that blanks grouping marker/spacer rows for non-host columns. It must detect grouping ids via the Core detection API and consult grid-owned coordinator state to determine the header-host column. |

---

## M2 — Create Grouping feature contract types (Composable-only)

### P1 — Core contracts + helpers

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P1.T1 | Create grouping enums | Add enums under `ComposableColumns/Features/Grouping/Enums/`: `GroupSortDirection`, `FilterGroupOrder`, `NullKeyBehavior` per spec §3.3–§3.5. |
| M2.P1.T2 | Create context records | Add `GroupHeaderContext<TGridItem, TValue>` and `GroupToolbarContext` under `ComposableColumns/Features/Grouping/` per spec §4. |
| M2.P1.T3 | Create `IGroupingFeature<TGridItem>` | Add `ComposableColumns/Features/Grouping/IGroupingFeature.cs` matching spec §5.3 (object-typed coordinator compatibility + header render method). |
| M2.P1.T4 | Create `IGridDataTransformer<TGridItem>` (Core) | Add `ComposableColumns/Core/IGridDataTransformer.cs` per spec §5.4 (if not already present). |
| M2.P1.T5 | Interface alignment check (Contracts/Templates) | Produce a checklist confirming: templates are `RenderFragment<TContext>`; `IGroupingFeature` is non-generic over `TValue` and exposes `GroupByUntyped`/comparers untyped; `RenderGroupHeader` uses `RenderTreeBuilder`; and the header-host column cell feature is the invoker of `RenderGroupHeader` (not the grid). |

### P2 — Synthetic row identity helper

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P2.T1 | Implement `GroupHeaderRowId` encoding helper | Add `ComposableColumns/Features/Grouping/GroupHeaderRowId.cs` implementing the normative bit layout (kind/groupId/offset) and detection/decode API per spec §2.5.1.3.1 + §2.5.1.3. |
| M2.P2.T2 | Define deterministic id error behavior (sad path) | Implement and document (in test names) deterministic behavior for: out-of-range groupId/offset → `ArgumentOutOfRangeException`; decode called on non-synthetic ids → `ArgumentException`; detection methods return `false` for invalid ids; and capacity constraints are enforced. |
| M2.P2.T3 | Interface alignment check (Synthetic ids) | Produce a checklist confirming: marker ids are negative and offset=0; spacer ids are negative and offset>=1; `IsGroupHeaderMarker`/`IsGroupHeaderSpacer` are unambiguous; and `offset <= GroupHeaderSlotSpan-1` is upheld by data generation.

---

## M3 — Implement grouping state + data collaboration

### P1 — Implement `GroupStateManager<TValue>`

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P1.T1 | Implement `GroupStateManager<TValue>` | Add `ComposableColumns/Features/Grouping/GroupStateManager.cs` per spec §5.2 with semaphore-based async locking, `HasExpandedGroups`, `ExpandedGroupCount`, `IsExpanded`, `ToggleAsync`, `ExpandAsync`, `CollapseAsync`, `ExpandAllAsync(allKeys)`, `CollapseAllAsync()`, and `InitializeAsync(allKeys, initiallyExpanded)`. |
| M3.P1.T2 | Define deterministic state semantics | Implement deterministic behavior for: duplicate keys in `InitializeAsync`/`ExpandAllAsync` (idempotent), calling toggle on missing key (allowed; adds/removes), and concurrency safety. Document these semantics in test names. |
| M3.P1.T3 | Interface alignment check (State ownership) | Produce a checklist confirming: state manager is feature-owned and typed; coordinator does not own typed state; initialization is lazy (during first `TransformItems` when keys are known).

### P2 — Implement coordinator + grouped data source

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P2.T1 | Implement `GroupingCoordinator<TGridItem>` | Add `ComposableColumns/Features/Grouping/GroupingCoordinator.cs` per spec §5.1: register columns, manage `ActiveGrouping`, and transform items into a flattened sequence with header marker/spacer rows + data rows (all as `TGridItem`). |
| M3.P2.T2 | Implement `GroupedGridDataSource<TGridItem>` | Add `ComposableColumns/Features/Grouping/GroupedGridDataSource.cs` per spec §5.6 with `Items` binding and `OnDataChanged` refresh event and `ToggleGroupAsync(object key)`.
| M3.P2.T3 | Implement key→groupId caching | Implement cached key→groupId mapping inside the grouping transform (preferred strategy in spec §2.5.1.5) so marker/spacer ids are stable across expand/collapse rebuilds.
| M3.P2.T4 | Define deterministic transform edge cases (sad path) | Implement deterministic behavior for: empty input sequence; all items filtered out; `NullKeyBehavior` variants; `GroupHeaderSlotSpan == 1`; `HideEmptyGroups` handling post-filtering; and invalid `GroupHeaderSlotSpan (<1)` (guard). Document in test names.
| M3.P2.T5 | Interface alignment check (Virtualization + binding) | Produce a checklist confirming: the flattened output is compatible with `QuickGrid.Items` (IQueryable), virtualization alignment is achieved via real marker/spacer rows, and grouped data source refresh uses `OnDataChanged` (not `FeatureContext.RequestRefreshAsync`). |

---

## M4 — Disposal + lifecycle

### P1 — Deterministic cleanup

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P1.T1 | Implement coordinator disposal semantics | Ensure `GroupingCoordinator<TGridItem>.Dispose()` clears registrations and resets `ActiveGrouping` (plan Phase 7). |
| M4.P1.T2 | Implement grid disposal/unsubscribe rules | Ensure `ComposableGrid<TGridItem>` unsubscribes from `GroupedGridDataSource<TGridItem>.OnDataChanged` and clears/disposes any cached grouping data source/coordinator as applicable during grid disposal and when grouping transitions active→inactive (plan/spec lifecycle rules). |
| M4.P1.T3 | Interface alignment check (Disposal) | Produce a checklist confirming: no event handler leaks, grouped data source references are cleared on disable/dispose, and grouping does not retain references preventing GC. |

---

## M5 — Integrate grouping into `ComposableGrid<TGridItem>` (Filtering-pattern)

### P1 — Coordinator storage + ItemsForQuickGrid binding

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P1.T1 | Add grid-owned grouping coordinator | Modify `ComposableColumns/Core/ComposableGrid.razor` (code-behind section) to add private `_groupingCoordinator` field and an `internal GroupingCoordinator<TGridItem> GetOrCreateGroupingCoordinator()` method per spec §2.5.0.1 (Filtering pattern).
| M5.P1.T2 | Add grid-owned grouped data source + event wiring | Modify `ComposableGrid` to hold a single `_groupedDataSource` field (grid-scoped), construct/replace it per spec §5.6.1, and subscribe exactly once to `OnDataChanged` with handler `() => InvokeAsync(StateHasChanged)`; unsubscribe when replacing/disposing. |
| M5.P1.T3 | Implement `ItemsForQuickGrid` selection | Modify `ComposableGrid` so `QuickGrid.Items` binds to a single `ItemsForQuickGrid` property using the normative binding rules (spec §2.4.2.3): inactive → `SortedItems`; active → `GroupedItems(SortedItems)` via `_groupedDataSource.Items`. |
| M5.P1.T4 | Interface alignment check (Grid integration) | Produce a checklist confirming: grouping integrates like filtering (grid-owned coordinator + internal API), no grid row hooks are introduced, and binding is only switched via `ItemsForQuickGrid`.

### P2 — Sorting suppression while grouping is active

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P2.T1 | Implement QuickGrid global-sort suppression (pinned mechanism) | When grouping is active, ensure `ComposableColumn.SortBy` is `null` for all columns (or otherwise not supplied to QuickGrid), disabling QuickGrid click-to-sort behavior so marker/spacer rows are not reordered. |
| M5.P2.T2 | Implement `SortedItems` owned by `ComposableGrid<TGridItem>` | Implement a `SortedItems` pipeline stage derived from `FilteredItems` using the active `ISortingFeature<TGridItem>` state (ComposableColumns sort), independent of QuickGrid global sorting. |
| M5.P2.T3 | Implement intra-group sorting consumption | Ensure the grouping transform consumes the grid’s `SortedItems` as the per-group item order while group ordering follows `GroupOrder`/`GroupOrderComparer` only. |
| M5.P2.T4 | Interface alignment check (Sorting) | Produce a checklist confirming: group ordering is never affected by column sort when grouping is active, intra-group ordering is deterministic, and QuickGrid global sorting is suppressed via `SortBy=null`.

---

## M6 — Implement `GroupingFeature<TGridItem, TValue>` (main feature)

### P1 — Feature lifecycle + validation

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M6.P1.T1 | Create feature skeleton | Add `ComposableColumns/Features/Grouping/GroupingFeature.cs` implementing `IColumnFeature<TGridItem>`, `IGridDataTransformer<TGridItem>`, `IGroupingFeature<TGridItem>`, and `IDisposable` per spec §2.1.
| M6.P1.T2 | Implement `OnAttach` invariants | Enforce: `context.InvokeAsync` must be non-null; `FilterBehavior != GroupThenFilter` (throw `NotSupportedException`); if `IsActive` then require `TGridItem : IRowIdentifiable` at runtime (throw `InvalidOperationException`). Cache typed/untyped group key selectors and comparers. |
| M6.P1.T3 | Implement coordinator registration + attach-time activation | Obtain `ComposableGrid<TGridItem>` via cascade and call `grid.GetOrCreateGroupingCoordinator()`; register this feature by columnId; apply first-wins activation (`SetActiveGrouping`) during `OnAttach` only (no runtime switching without re-attach). |
| M6.P1.T3a | Pin header-host identity in coordinator | Ensure the coordinator records `HeaderHostColumnId` as the first registered grouping column. This must be queryable by the core blanking feature path to determine which column is permitted to render group header UI. |
| M6.P1.T4 | Implement grouping state methods | Implement `ToggleGroupAsync`, `IsGroupExpanded`, `ExpandAllGroupsAsync`, `CollapseAllGroupsAsync` delegating to typed `GroupStateManager<TValue>` and integrating with grouped data source refresh (OnDataChanged path). |
| M6.P1.T5 | Implement header rendering surface | Implement `RenderGroupHeader(...)` to render `HeaderTemplate` when provided; otherwise render the default header via `Components/DefaultGroupHeader.razor` (plan §6). |
| M6.P1.T6 | Interface alignment check (Feature pipeline) | Produce a checklist confirming: feature implements `Priority=50`; `GroupBy` resolution uses explicit `GroupBy` first then `FeatureContext.GetValue`; header rendering is invoked by the header-host column feature; and no use of `FeatureContext.RequestRefreshAsync` for expand/collapse. |

---

## M7 — Header-host column cell feature (rendering overlay)

### P1 — Implement header-host rendering via `ICellRenderFeature<TGridItem>`

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M7.P1.T1 | Implement a group-header host cell feature | Add a cell render feature type (name per plan: `Components/GroupHeaderHostFeature.cs`) under `ComposableColumns/Features/Grouping/Components/` that is attached to the header-host column and:
- detects marker/spacer rows via `GroupHeaderRowId` using `item.Id`
- renders header overlay only for marker rows
- renders blank for spacer rows and blank for normal data rows
- gates toolbar rendering to the FIRST marker row in the flattened sequence
| M7.P1.T2 | Ensure actions are awaited (no fire-and-forget) | Ensure header click, Expand All, and Collapse All handlers are `Task`-returning and are awaited end-to-end (plan §2.4.6). |
| M7.P1.T3 | Wire host feature selection | Implement the rule: the first column registering a grouping feature becomes the header-host and receives/activates the host cell feature; other columns render blank for marker/spacer rows. |
| M7.P1.T4 | Interface alignment check (Rendering) | Produce a checklist confirming: no grid row hooks are used, header UI is emitted from within first column cell surface, spacer rows render blank, and toolbar renders once per grid. |

---

## M7a — Reference and namespace updates (Core detection move)

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M7a.P1.T1 | Update call sites to use Core detection API | Update any code/tests/docs that need to detect grouping marker/spacer ids so they use the new Core detection API, avoiding `ComposableColumns.Core` referencing feature namespaces. |
| M7a.P1.T2 | Keep grouping encoder/decoder contract stable | Ensure grouping’s encoder/decoder helper remains available for grouping feature/data source implementation. If the helper type is moved or split, update namespaces/usings and adjust tests accordingly. |

---

## M8 — Styling (global stylesheet)

### P1 — Add required CSS selectors

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M8.P1.T1 | Add grouping CSS | Modify `wwwroot/css/qgComposable-refined-minimalism.css` to reuse existing selector rules and add only missing selectors for `.qg-group-header`, `.qg-group-header.expanded`, `.qg-group-header.collapsed`, `.qg-group-chevron`, `.qg-group-key`, `.qg-group-count`, `.qg-group-controls`, `.qg-group-toolbar`, and `.qg-grid-wrapper` per spec §7.
| M8.P1.T2 | Implement CSS variable sizing contract | Ensure styles use `--qg-item-size` and `--qg-group-header-slot-span` (spec §2.5.0 + plan §1.2.2) to compute the overlay/header height aligned with virtualization.
| M8.P1.T3 | Interface alignment check (CSS contract) | Produce a checklist confirming emitted class names match selector names and header height equals `GroupHeaderSlotSpan × ItemSize`.

---

## M9 — Demo page

### P1 — Create grouping demo page

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M9.P1.T1 | Create demo page | Add `Pages/ComposableGroupingDemo.razor` (same folder as other demos) matching plan demo requirements (two grids + two event logs) and demonstrating grouping with:
- `DemoRow : IRowIdentifiable`
- at least one grouped column (string/category)
- `GroupHeaderSlotSpan` default and `1`
- toggling expand/collapse and expand all/collapse all
- binding `QuickGrid.Items` through `ComposableGrid`’s `ItemsForQuickGrid` selection
- switching the active grouping key (Category vs Status) by re-rendering with new feature instances (attach-time activation)
| M9.P1.T2 | Interface alignment check (Demo wiring) | Produce a checklist confirming: grouping is activated by first `IsActive=true`, group headers render via marker row detection, virtualization alignment works via spacer rows, and switching group-by forces re-attach.

---

## M10 — Automated tests (non-UI)

### P1 — Add unit tests for core types

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M10.P1.T1 | Add `GroupHeaderRowId` tests | Unit tests validating encode/decode, marker vs spacer detection, invalid inputs, and capacity behavior.
| M10.P1.T2 | Add `GroupStateManager<TValue>` tests | Unit tests validating initialization, toggle, expand/collapse, expand-all/collapse-all, and a deterministic concurrency test strategy (idempotent final state; avoid timing-sensitive assertions).
| M10.P1.T3 | Add grouping transform tests | Unit tests validating:
- header marker/spacer emission count (`GroupHeaderSlotSpan`)
- stable key→groupId mapping
- collapsed vs expanded output
- **NullKeyBehavior explicit outcomes**:
  - `SeparateGroup`: null-key items appear under a dedicated group header
  - `ShowAtTop`: null-key items appear as ungrouped normal rows before any group headers
  - `ShowAtBottom`: null-key items appear as ungrouped normal rows after all groups
  - `Exclude`: null-key items are omitted
- `HideEmptyGroups` handling (emptiness evaluated after filtering and after null-key handling)
| M10.P1.T4 | Add sad-path tests | Unit tests validating deterministic exceptions for unsupported `GroupThenFilter`, invalid `GroupHeaderSlotSpan`, runtime `IRowIdentifiable` enforcement, and invalid id decode usage.
| M10.P1.T5 | Clarify tests for `GroupingCoordinator<TGridItem>` without InternalsVisibleTo | Because tests must go through public APIs and avoid `InternalsVisibleTo`, test coordinator behavior indirectly via `GroupedGridDataSource<TGridItem>` and other public entry points (registration/activation/transform effects). |
| M10.P1.T6 | Interface alignment check (Test coverage) | Ensure tests cover at least one compile-time signature usage check for the chosen cell render feature integration and validate refresh authority assumptions (grouping uses `OnDataChanged`, not `RequestRefreshAsync`).

---

## M11 — Completion checklist

### P1 — Final validation against plan

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M11.P1.T1 | Validate acceptance criteria | Verify all acceptance criteria in `Plan_RowGroupingFeature.md` are satisfied.
| M11.P1.T2 | Validate sad-path behaviors | Verify guard failures, no-op rules, and deterministic ordering match this tasks document.
| M11.P1.T3 | Interface alignment check (Final) | Confirm the final implementation compiles cleanly and all feature calls align with current `ComposableColumns` interfaces and the spec’s deterministic pipeline.
