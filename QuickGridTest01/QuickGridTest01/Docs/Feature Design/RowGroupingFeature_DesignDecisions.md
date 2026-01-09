# Row Grouping Feature Design Decisions

This document records **final design decisions** for `GroupingFeature<TGridItem, TValue>` and related grouping infrastructure.

It is intended to capture decisions that are easy to “forget” during implementation (especially those that affect layering, ownership, and cross-column behavior).

## Decision log

### D1 — Single-level grouping only

**Decision:** Implement only single-level grouping. The `Level` property remains reserved for future extensibility.

**Rationale:** Keep initial feature scope aligned with the non-goals and avoid compounding virtualization and identity complexity.

---

### D2 — Header height uses real marker/spacer rows (virtualization alignment)

**Decision:** A group header consumes `GroupHeaderSlotSpan` “row slots” by inserting:

- 1x group header **marker** row
- `GroupHeaderSlotSpan - 1` x group header **spacer** rows

**Rationale:** Matches the proven expansion-style “spacer row injection” pattern and keeps QuickGrid virtualization aligned without custom windowing.

---

### D3 — No runtime feature injection

**Decision:** The implementation must not rely on dynamically adding features after a column has initialized.

**Rationale:** `ComposableColumn.AddFeature(...)` does not attach the feature (`OnAttach` is not invoked), so post-initialization feature injection is not safe/deterministic.

---

### D4 — Non-host columns must render blank for grouping marker/spacer rows (A2.1)

**Decision:** All non-host columns render **blank output** for grouping marker/spacer rows.

**Implementation decision:** This is enforced via a **core-owned always-present cell feature path** added by `ComposableColumn` *prior to initialization*, so it participates in the normal `ICellRenderFeature<TGridItem>` pipeline and can short-circuit rendering.

**Rationale:** This guarantees correctness across all columns without requiring every column to opt into grouping features.

---

### D5 — Header-host identity stored in grid-owned coordinator

**Decision:** The **first** registered grouping-enabled column is the header-host column, and the coordinator records its identity deterministically (e.g., `HeaderHostColumnId`).

**Rationale:** Non-host blanking and header-host rendering require a grid-scoped source of truth.

---

### D6 — Minimal grouping synthetic-id detection API lives in `ComposableColumns.Core`

**Decision:** The minimal API required by the core blanking feature to detect grouping marker/spacer row ids is located in `ComposableColumns.Core`.

**Rationale:** Prevents a `ComposableColumns.Core -> Features.Grouping` dependency. This is an intentional exception to the original “all grouping logic under `Features.Grouping`” file-structure guidance.

**Constraint:** Keep the Core surface small (detection only). Encoding/decoding and range validation remain part of the public grouping row-id contract.

---

### D7 — Sorting while grouping is active

**Decision (direction):** When grouping is active:

- QuickGrid global sorting must be suppressed (it must not reorder marker/spacer rows).
- Sorting applies **within each group only**, using the grid-owned `SortedItems` stage.
- Group ordering is controlled only by `GroupOrder` / `GroupOrderComparer` (or `FirstOccurrence`).

**Pinned mechanism:** Suppress QuickGrid global sorting by ensuring columns do not provide `SortBy` to QuickGrid while grouping is active (i.e., `ComposableColumn.SortBy` is `null` during active grouping). Sorting is owned by `ComposableGrid<TGridItem>` as `SortedItems` derived from `FilteredItems` using `ISortingFeature<TGridItem>` state.

**Rationale:** Deterministic pipeline semantics and stable marker/spacer row placement.

---

### D8 — Refresh authority for expand/collapse

**Decision:** Expand/collapse operations update state and then trigger refresh via:

`GroupedGridDataSource.OnDataChanged` → grid `InvokeAsync(StateHasChanged)`

**Rationale:** Single refresh authority avoids double-refresh loops and ensures async ordering (no fire-and-forget).


