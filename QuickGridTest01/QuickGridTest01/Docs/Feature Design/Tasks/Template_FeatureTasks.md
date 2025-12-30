# Task Execution — <Feature Display Name> (`<MainTypeName>`)

## Source
- `Docs/Feature Design/ImplementationPlans/<PlanFileName>.md`

## Conventions
- Task Ids are `M<Milestone>.P<Phase>.T<Task>` (e.g., `M1.P1.T1`).
- Legacy code under `<LegacyNamespaceOrFolder>` remains unchanged.
- Keep diffs minimal and follow existing feature patterns.

---

## M1 — ComposableColumns plumbing prerequisites

### P1 — Priority + integration prerequisites

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M1.P1.T1 | Update feature priorities | Modify `ComposableColumns/Core/FeaturePriority.cs` to add `FeaturePriority.<FeatureName> = <PriorityValue>`. |
| M1.P1.T2 | Confirm `FeatureContext` dispatcher/refresh invariants | Identify the exact assignment sites in `ComposableColumns/Core/ComposableColumn.cs` (or related) where `FeatureContext.InvokeAsync` and the relevant refresh delegate(s) for this feature are set; record the `file + method` names in the task execution report. |
| M1.P1.T3 | Define required guard failures (sad path) | Produce a “Guard Failures” list (exception type + message text) for the required invariants and invalid parameters for this feature. Store the list in the task execution report. |
| M1.P1.T4 | Interface alignment check (Core) | Produce a checklist in the task execution report confirming: `OnAttach/OnDetach` signatures, relevant render-feature signatures (e.g., `RenderCell`), nullable delegates (`InvokeAsync`, refresh delegates), and the decided missing-delegate behavior (throw vs fallback). |

---

## M2 — Create <FeatureName> feature contract types (Composable-only)

### P1 — Core contracts + helpers

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P1.T1 | Create core interfaces/contracts | Add contract types under `ComposableColumns/Features/<FeatureName>/` as defined by the spec (e.g., required item contracts, coordinator interfaces). |
| M2.P1.T2 | Create helper utilities | Add helper types (e.g., id encoding helpers, extensions) required by the rendering and data transformation model. |
| M2.P1.T3 | Interface alignment check (Core contracts) | Produce a checklist in the task execution report confirming key contract choices (e.g., identity scheme, key types, comparer behavior, and any integration requirements with `FeatureContext.RowKey`). |

### P2 — Enums + contexts + events

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M2.P2.T1 | Create enums | Add enums under `ComposableColumns/Features/<FeatureName>/Enums/` per the spec. |
| M2.P2.T2 | Create context objects | Add context record(s) consumed by templates/callbacks under `ComposableColumns/Features/<FeatureName>/`. |
| M2.P2.T3 | Create event args (if applicable) | Add event args types under `ComposableColumns/Features/<FeatureName>/Events/` per the spec/plan. |
| M2.P2.T4 | Interface alignment check (Templates/Events) | Produce a checklist confirming: templates are `RenderFragment<TContext>`; events are `EventCallback<T>` (if used); and invocation uses `EventCallback<T>.InvokeAsync(...)`.

---

## M3 — Implement feature state + data collaboration

### P1 — Data transformation / data source

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P1.T1 | Implement encoding/identity helper | Add the helper that implements the deterministic encoding/identity rules (and associated detection/decode). |
| M3.P1.T2 | Implement grid data source / transformer | Add the feature’s data source/transformer type(s) used to produce the `QuickGrid.Items` sequence. |
| M3.P1.T3 | Define deterministic sad-path behavior | Implement and document deterministic behavior for invalid ids/inputs (exception type + message) and any required no-op rules. |
| M3.P1.T4 | Interface alignment check (Data binding + refresh) | Produce a checklist confirming: `Items` is compatible with `QuickGrid.Items` usage (IQueryable binding), and the refresh mechanism is consistent with the spec (e.g., `OnDataChanged` vs `FeatureContext.RequestRefreshAsync`). |

### P2 — State manager (if applicable)

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M3.P2.T1 | Implement feature state manager | Add the state manager type (thread-safe where required) per spec. |
| M3.P2.T2 | Interface alignment check (State lifecycle) | Produce a checklist confirming where state is owned (feature vs grid), how it is initialized (lazy vs eager), and how it is disposed. |

---

## M4 — Implement the main feature type(s)

### P1 — Feature lifecycle + registration

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P1.T1 | Create feature skeleton | Add `<MainFeatureType>.cs` implementing the required feature interfaces and `IDisposable` if needed. |
| M4.P1.T2 | Implement `OnAttach` invariants | Cache context and enforce required invariants (dispatcher, required contracts, unsupported configurations). |
| M4.P1.T3 | Implement registration pattern | Register with grid/coordinator using the established pattern (e.g., grid-owned coordinator, first-wins activation). |
| M4.P1.T4 | Implement parameter validation (sad path) | Throw deterministic exceptions for invalid parameters per the spec.
| M4.P1.T5 | Interface alignment check (Feature pipeline) | Produce a checklist confirming the feature integrates with the existing render pipeline and that `OnDetach`/`Dispose` are invoked through the expected lifecycle.

### P2 — User interaction / commands (if applicable)

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M4.P2.T1 | Implement primary interaction command(s) | Implement the main interaction flow(s) with deterministic ordering and async/await rules.
| M4.P2.T2 | Enforce dispatcher boundary | Ensure UI-affecting operations execute within `context.InvokeAsync` when required.
| M4.P2.T3 | Implement safe no-op rules (sad path) | Implement required no-op behaviors (e.g., ignore synthetic rows) and document them.
| M4.P2.T4 | Interface alignment check (Dispatch + refresh) | Produce a checklist confirming dispatcher usage and refresh authority matches the spec.

---

## M5 — UI components (if any)

### P1 — Default templates/components

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M5.P1.T1 | Create default component(s) | Add default component(s) under `ComposableColumns/Features/<FeatureName>/Components/` per the spec.
| M5.P1.T2 | Interface alignment check (Components) | Produce a checklist confirming component parameters/templates match spec and that fallback behavior is deterministic.

---

## M6 — Styling (global stylesheet)

### P1 — Add required CSS selectors

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M6.P1.T1 | Add CSS | Modify `wwwroot/css/qgComposable-refined-minimalism.css` to reuse existing selector rules and add only missing selectors required by the feature.
| M6.P1.T2 | Interface alignment check (CSS contract) | Produce a checklist confirming emitted class names match selector names and sizing/alignment rules match the spec.

---

## M7 — Demo page

### P1 — Create demo page

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M7.P1.T1 | Create demo page | Add `Pages/<DemoPageName>.razor` demonstrating the feature wired through the ComposableColumns pipeline with correct binding and row key setup.
| M7.P1.T2 | Interface alignment check (Demo wiring) | Produce a checklist confirming demo binds to the correct `Items` sequence and that the feature behaves per spec.

---

## M8 — Automated tests (non-UI)

### P1 — Add unit tests

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M8.P1.T1 | Add helper/encoding tests | Unit tests validating encode/decode rules and detection helpers.
| M8.P1.T2 | Add data source/transform tests | Unit tests validating sequence construction, ordering, and refresh behavior.
| M8.P1.T3 | Add state manager tests (if applicable) | Unit tests validating state transitions and concurrency behavior.
| M8.P1.T4 | Add sad-path tests | Unit tests validating deterministic exception/no-op rules.
| M8.P1.T5 | Interface alignment check (Test coverage) | Ensure tests cover at least one compile-time signature usage check (where applicable) and validate required invariants.

---

## M10 — Completion checklist

### P1 — Final validation against plan

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| M10.P1.T1 | Validate acceptance criteria | Verify all acceptance criteria in `<PlanFileName>.md` are satisfied.
| M10.P1.T2 | Validate sad-path behaviors | Verify guard failures and no-op rules match this tasks document.
| M10.P1.T3 | Interface alignment check (Final) | Confirm the final implementation compiles cleanly and all feature calls align with current `ComposableColumns` interfaces, with no unused/assumed members.
