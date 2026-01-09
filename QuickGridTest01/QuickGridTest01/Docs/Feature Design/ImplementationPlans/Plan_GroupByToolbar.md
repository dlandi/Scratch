# Implementation Plan — Grid toolbar: Grouping section

This plan implements the updated `RowGroupingFeature` specification: grouping controls (Group By selector and optional expand/collapse-all controls) are rendered in the **grid toolbar**, directly above the grid and **below any filtering UI**.

## Objectives

1. Provide an internal grouping UI that lets the user select a Group By column at runtime.
2. Keep the existing grouping data model intact (marker/spacer rows + header host cell feature).
3. Preserve deterministic refresh behavior and avoid render loops.
4. Avoid introducing new abstraction layers unless required.

## Constraints and non-goals

- No grid row hooks or alternative row rendering paths.
- The grouping pipeline remains: `FilteredItems → SortedItems → GroupedItems`.
- Group expand/collapse refresh authority remains: `GroupedGridDataSource.OnDataChanged → InvokeAsync(StateHasChanged)`.
- Do not introduce a generic “toolbar contribution” framework at this stage.

## Phase 1 — Inventory current grouping APIs

1. Verify what `GroupingCoordinator<TGridItem>` exposes:
   - registered grouping columns (`columnId` set)
   - active grouping (feature reference and/or active `columnId`)
2. Identify how grouping activation currently occurs:
   - `IsActive=true` “first wins” behavior during registration/attach
   - any existing “disable grouping” path
3. Decide what the Group By selector displays:
   - preferred: column title (if it is available to the grid at registration time)
   - fallback: `columnId`

Deliverable: a minimal, concrete structure for `[(columnId, label)]` + `activeColumnId`.

## Phase 2 — Render the grouping section in `ComposableGrid`

### Placement

Render the grouping section in `ComposableGrid.razor` in the same toolbar region used by filtering:

1. Filter toolbar (existing; when filters are present)
2. **Grid toolbar: Grouping section** (new; when groupable columns are present)
3. `QuickGrid`

### Minimum viable UI

- A `select` control labeled “Group by”:
  - option: “None” (ungrouped)
  - one option per registered grouping column
- Optional controls (only when grouping is active and enabled by the active feature):
  - Expand All
  - Collapse All

### Event behavior

- On selector change:
  - call the coordinator to set active grouping (`columnId` or `null`)
  - rebind `ItemsForQuickGrid` accordingly (grouped vs ungrouped)
  - trigger **exactly one** refresh (`InvokeAsync(StateHasChanged)`) as needed

- On Expand All / Collapse All:
  - call the active feature’s async operation
  - trigger a single grouped refresh (preferably through the existing grouped data source mechanism)

## Phase 3 — Add minimal coordinator metadata needed for UI

Without introducing new abstraction layers, add only the coordinator APIs required by the grid toolbar:

- enumerate registered grouping columns (stable ordering)
- determine active grouping `columnId` (or expose it explicitly)

If the active feature instance does not reliably expose its own `ColumnId`, the coordinator should store it as part of `SetActiveGrouping`.

## Phase 4 — Reconcile runtime selection with initial activation

Maintain deterministic initial activation:

- Startup: `IsActive=true` continues to apply using the existing “first wins” rule.
- After user selection in the toolbar:
  - the user-selected grouping becomes authoritative
  - subsequent registrations/attachments must not override an already-active user selection.

## Phase 5 — Simplify `GroupHeaderHostFeature`

Once grouping controls are rendered in the grid toolbar:

- Remove/retire any “toolbar rendered” state stored by `GroupHeaderHostFeature` that no longer has a functional purpose.
- Keep `GroupHeaderHostFeature` focused on rendering the per-group header UI for marker rows only.

## Phase 6 — Update demos

Update `Pages/ComposableGroupingDemo.razor` so grouping is not effectively hardcoded:

- Attach grouping features to multiple columns (e.g., Category and Status)
- Default to ungrouped or use `IsActive=true` only as an initial default
- Validate the UI-driven selection path end-to-end

## Phase 7 — Styling

Update `wwwroot/css/qgComposable-refined-minimalism.css` to style the grouping section so it visually matches the existing filter toolbar and sits immediately above the grid.

## Phase 8 — Validation

1. `dotnet build`
2. Manual verification:
   - selecting Group By updates grouping without refresh loops
   - expand/collapse all is awaited and refreshes exactly once
   - virtualization remains stable (marker/spacer behavior preserved)
   - QuickGrid header sorting is suppressed while grouping is active
