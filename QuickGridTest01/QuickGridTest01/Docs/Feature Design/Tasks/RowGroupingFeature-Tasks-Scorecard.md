# RowGroupingFeature Tasks — Vetting Scorecard (temporary)

Purpose: rigorously validate `Tasks/RowGroupingFeature-Tasks.md` against `ImplementationPlans/Plan_RowGroupingFeature.md` and `RowGroupingFeature.md` to prevent invented glue and missed requirements.

Legend:
- **PASS**: Tasks explicitly cover the requirement with clear ownership and deliverables.
- **NEEDS CLARIFICATION**: Tasks exist but still leave implementer choices that could cause invention.
- **GAP**: Plan/spec requirement not represented in tasks.
- **POTENTIAL DRIFT**: Task wording conflicts with plan/spec.

---

## 1) Scope + non-goals

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 1.1 | Plan Intro (Non-goals) | PASS | Do tasks avoid nested grouping, drag-to-reorder, persistence? (No tasks added for these.) |
| 1.2 | Spec: no grid row hooks | PASS | Tasks repeatedly assert “no row hooks”; ensure no later tasks introduce row interception. |
| 1.3 | Spec: CSS must be global | PASS | M7 explicitly restricts to `qgComposable-refined-minimalism.css`. |

---

## 2) File structure + namespace constraints

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 2.1 | Plan Intro + Spec §11 | PASS | Tasks place files under `ComposableColumns/Features/Grouping/*` and core under `ComposableColumns/Core/*`. |
| 2.2 | Plan file structure (demo in `Pages/`) | PASS | Demo is now explicitly `Pages/ComposableGroupingDemo.razor` (same folder as other demos). |

---

## 3) Priority + feature pipeline placement

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 3.1 | Plan §2.2 | PASS | M1.P1.T1 sets `FeaturePriority.Grouping = 50`. Verify it is placed before `Core (100)` and matches existing enum ordering expectations. |

---

## 4) Coordinator ownership + access (Filtering pattern)

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 4.1 | Plan §2.4.1 | PASS | M4.P1.T1 enforces grid-owned `_groupingCoordinator` + `GetOrCreateGroupingCoordinator()`.
| 4.2 | Plan §2.3 (header-host = first registered) | PASS | Pinned: coordinator records `HeaderHostColumnId` for the first registered grouping column; non-host blanking is enforced via a core-owned always-present cell feature path (no runtime injection).
| 4.3 | Plan Phase anchors (deterministic activation) | PASS | Tasks now explicitly lock activation to attach-time only (runtime switching requires re-attach; demo must re-render).

---

## 5) Data pipeline + binding rules

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 5.1 | Plan §1.2 (pipeline invariants) | PASS | Plan defines pipeline unambiguously and tasks bind `QuickGrid.Items` via `ItemsForQuickGrid` selecting `SortedItems` vs grouped items.
| 5.2 | Spec §2.4.2.3 (ItemsForQuickGrid must always bind) | PASS | M4.P1.T3 requires binding `QuickGrid.Items` to `ItemsForQuickGrid`.
| 5.3 | Plan sorting interaction (intra-group only) | PASS | Pinned: suppress QuickGrid global sorting by ensuring `ComposableColumn.SortBy` is `null` while grouping is active; sorting is owned by `ComposableGrid` via `SortedItems` from `ISortingFeature` state.
| 5.4 | Plan §2.4.2 (grouping consumes `FilteredItems`) | PASS | By binding to `SortedItems` generated from `FilteredItems`, this is satisfied if `SortedItems` is indeed derived from `FilteredItems`.

---

## 6) Row identity + marker/spacer encoding (Expansion-aligned)

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 6.1 | Plan §2.4.0 + Spec §2.5.1.3.1 | PASS | M2.P2 explicitly implements `GroupHeaderRowId` with encode/decode and sad paths.
| 6.2 | Plan §2.4.0 stability rule | PASS | M3.P2.T3 mandates cached key→groupId mapping.
| 6.3 | Spec: `IRowIdentifiable` enforcement | PASS | M1.P1.T3 + M5.P1.T2 include runtime enforcement. Ensure the exception type/message are pinned in the Guard Failures list.
| 6.4 | Plan §1.2.2 (GroupHeaderSlotSpan height mapping) | PASS | M3 edge cases include `GroupHeaderSlotSpan == 1` and invalid `<1`.

---

## 7) Refresh authority + async ordering

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 7.1 | Plan §2.4.6 | PASS | M4.P1.T2 subscribes to `OnDataChanged` and uses grid `InvokeAsync(StateHasChanged)`.
| 7.2 | Plan §2.4.6 “no fire-and-forget” | PASS | Tasks now explicitly require awaited handlers in the header-host feature (toggle/expand all/collapse all).
| 7.3 | Plan “single refresh authority” | PASS | Tasks explicitly forbid `FeatureContext.RequestRefreshAsync` for expand/collapse.

---

## 8) Rendering responsibilities (cell pipeline)

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 8.1 | Plan §2.4.3 | PASS | Pinned: marker/spacer blanking across non-host columns is enforced via a core-owned always-present `ICellRenderFeature` using a Core detection API + coordinator host identity.
| 8.2 | Plan §1.2.8 toolbar gating | PASS | M6.P1.T1 requires FIRST marker row gating.
| 8.3 | Plan §2.4 row rendering: marker/spacer blank | PASS | M6 explicitly states blank output for spacer and normal rows.
| 8.4 | Plan §6 default template | PASS | Tasks now require `Components/DefaultGroupHeader.razor` as the default rendering path.

---

## 9) Null keys + filtering interaction

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 9.1 | Plan §1.2.6 | PASS | Tasks now enumerate explicit observable outcomes for all `NullKeyBehavior` values.
| 9.2 | Plan §1.2.7 | PASS | Tasks call out `FilterThenGroup` (via consuming `FilteredItems`) and throw for `GroupThenFilter`.
| 9.3 | Plan §1.2.7 HideEmptyGroups | PASS | Tasks now pin: emptiness is evaluated after filtering and after null-key handling.

---

## 10) Disposal + lifecycle leaks

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 10.1 | Plan Phase 7 + Spec §5.6.3 | PASS | Tasks now include explicit disposal/unsubscribe requirements for grid and data source.
| 10.2 | Plan Phase 7 coordinator dispose | PASS | Tasks now explicitly require coordinator disposal semantics.

---

## 11) Tests + feasibility

| Check | Plan/Spec anchor | Status | Notes / Questions to ask |
|------:|------------------|:------:|---------------------------|
| 11.1 | Plan encourages deterministic behaviors + sad paths | PASS | M9 covers id helper, state manager, transform tests, and sad paths.
| 11.2 | Test through public APIs (no InternalsVisibleTo) | PASS | Tasks now require indirect testing of coordinator behavior via public entry points (e.g., `GroupedGridDataSource<TGridItem>`).
| 11.3 | Concurrency tests | PASS | Tasks now specify a deterministic concurrency test approach (idempotent final state; avoid timing-sensitive assertions).

---

## Summary (what to fix in tasks before execution)

All previously open clarifications are now pinned in the tasks/spec/plan.

(Temporary file; delete once vetting is complete.)
