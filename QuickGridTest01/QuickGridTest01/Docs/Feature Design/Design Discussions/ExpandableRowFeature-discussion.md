# Discussion of ExpandableRow specification

## Top 10 things you’re still missing (critical)

### 1.	RowStateManager design contradicts your own table in §1.3
•	§1.3 says: state store is “RowStateManager<TGridItem> registered in FeatureContext”.
•	§8.4 now says: RowStateManager must be constructed with context.RowKey!.
•	Missing: an explicit lifecycle + registration contract:
•	When/where is the manager created?
•	What key is used for FeatureContext service registration (type? name?) preventing duplicates (§11.3 mentions sentinel but not the actual mechanism)?
•	Who disposes it (Dispose() vs column vs grid)?
#### Answers:
- **Decision (RowColumn parity):** one `RowStateManager<TGridItem>` per column/feature context.
- **Creation:** created lazily on first `RenderCell` use and placed into `FeatureContext` as a shared service.
- **Registration key:** service type (`context.GetService<RowStateManager<TGridItem>>()` acts as the sentinel).
- **Disposal:** `ComposableColumn.Dispose()` calls `FeatureContext.Clear()`. Therefore `FeatureContext.Clear()` MUST dispose any `IDisposable` services it holds before clearing.
- **Identity constraint:** for now, expansion relies on `TGridItem : IRowIdentifiable` with `int Id` (spacer-compatible).

---

### 2.	Key type is under-specified (risk of broken equality / perf)
#### Answers:
- **Decision:** constrain identity to `TGridItem : IRowIdentifiable` with `int Id` (RowColumn parity + spacer-compatible).
- `FeatureContext.RowKey` (when used) should resolve to `item.Id`.

---

### 3.	Cancellation design is inconsistent in the examples
•	ExpandAsync is Func<CancellationToken, Task>.
•	But §7.3 still uses ExpandAsync = () => ExpandRowAsync(item) (doesn’t match the delegate type).
•	Missing: updated example code in §7.3 to something like:
•	ExpandAsync = ct => ExpandRowAsync(item, ct)
#### Answers:
Given your decisions so far:
- strict adherence to RowColumn semantics
- `TGridItem` must implement `IRowIdentifiable` with `int Id`
- `ComposableColumn.Dispose()` calls `FeatureContext.Clear()`

**Recommendation:** keep cancellation support on the *feature/method* surface (for future async scenarios), but keep the *UI-facing* context delegates RowColumn-simple.

###### Suggested contract (closest to RowColumn)
1.	Keep cancellation tokens for the core operations:
- `ExpandRowAsync(TGridItem item, CancellationToken ct = default)`
- `CollapseRowAsync(TGridItem item, CancellationToken ct = default)`
- `CollapseAllAsync(CancellationToken ct = default)`

2.	Expose tokenless delegates in the template contexts:
- `RowDisplayContext.ExpandAsync` : `Func<Task>`
- `RowExpandedContext.CollapseAsync` : `Func<Task>`

Rationale: Blazor UI events rarely provide a meaningful `CancellationToken`. Forcing token-aware delegates into templates increases friction with no practical benefit in typical usage.

###### If you choose to keep token-aware delegates anyway
Then fix the spec examples to match the delegate type consistently:
- `ExpandAsync = ct => ExpandRowAsync(item, ct)`
- `CollapseAsync = ct => CollapseRowAsync(item, ct)`

And treat `CancellationToken.None` as the standard token passed from UI event handlers.

---

### 4.	RowCard section contradicts the repo’s existing RowCard API
•	Spec §9.2 shows HeaderTemplate/FooterTemplate and a built-in close button calling Context.CollapseAsync().
•	Actual RowColumn RowCard.razor (open in solution) uses HeaderActions, FooterContent, and Func<Task>? OnClose.
•	Missing: either align the spec to the existing component contract, or explicitly call out a new RowCard API for the composable feature package (and what happens to the old one).
#### Answers:
- There needs to be a minimal `RowCard` component in the composable feature package that provides the basic container while allowing a user template to supply the details.

###### Recommendation (strict RowColumn parity)
- Reuse the existing API shape from `QuickGridTest01.RowColumn.Components.RowCard` to avoid spec/API drift.
- Create a new component under `QuickGridTest01.ComposableColumns.Features.Expansion.Components` with the same parameters:
  - `Title` (optional)
  - `Class` (optional)
  - `HeaderActions` (optional `RenderFragment`)
  - `FooterContent` (optional `RenderFragment`)
  - `ShowCloseButton` (bool)
  - `OnClose` (`Func<Task>?`) (optional)
  - `ChildContent` (`RenderFragment?`)

###### Close behavior (expansion-friendly)
- When `OnClose` is not supplied, the default implementation should collapse via the cascaded `RowExpandedContext<TGridItem>`:
  - `OnClose ??= () => Context?.CollapseAsync() ?? Task.CompletedTask;`
- This keeps `RowCard` usable both inside expansion overlays (where a cascading context exists) and outside them (where a caller can supply `OnClose`).
- **Base feature requirement:** even if a user-provided `ExpandedTemplate` does not render its own close UI, the base expansion feature must still provide a consistent way to collapse (e.g., built-in close button in the default `RowCard`, or a standard close affordance rendered by the feature itself when expanded).

###### “User definable internal definition” clarification
- Users do **not** override `RowCard` internals; they customize and/or replace it via:
  1. `ExpandedTemplate` rendering entirely custom markup (full replacement)
  2. providing `HeaderActions` / `FooterContent` / `ChildContent` to the packaged `RowCard` (parameterized customization)

---

### 5.	Overlay positioning/scoping doesn’t line up with your CSS + spec
•	CSS says .row-overlay { position:absolute; top:100%; left:0; right:0; } and .row-cell { position:relative; }.
•	Spec says overlay “spans ExpandedRowSpan rows” and sometimes sounds like it overlays rows; but your CSS positions it below the row (not on top of subsequent rows).
•	Missing: a decisive statement:
•	Is this below-row expansion (push-down UX) or overlapping overlay (cover rows unless spacers)?
•	If below-row is the intent, then the whole “overlay spans N rows” story and spacer rationale should be tightened.
#### Answers:
- **Decision:** below-row expansion (push-down UX) only.
- The overlay MUST start below the "master" row that triggered it (`top: 100%`) and behavior must match `RowColumn`.
- `ExpandedRowSpan` and spacer rows exist to ensure the below-row overlay does not occlude subsequent real rows (virtualization-safe push-down).

---

### 6.	Spacer row rules are incomplete for ComposableColumns
•	You state spacer injection only works with ExpandableGridDataSource<TGridItem>.
•	Missing: the exact integration contract:
•	How does RowExpandFeature discover/access the ExpandableGridDataSource<T> instance? (FeatureContext service? parameter? both?)
•	How does it react to OnDataChanged? (Needs to trigger refresh.)
•	How are spacer rows detected (IsSpacerRow) in a RowKey world?
#### Answers:
- **Decision:** spacer injection will work exactly like the legacy `RowColumn` implementation and `RowColumnDemo`.

###### RowColumn-parity contract
1.	**Opt-in via optional DataSource**
- The expansion feature accepts an optional `ExpandableGridDataSource<TGridItem>` collaboration object (RowColumn `DataSource`-equivalent).
- If `DataSource` is null: expansion is overlay-only (no spacer injection).

2.	**Grid binds to `DataSource.Items`**
- For spacer injection to be visible, the grid must be bound to `DataSource.Items` (not the original data collection).

3.	**Injection/removal is driven by expand/collapse**
- On expand: call `DataSource.ExpandRow(item.Id, ExpandedRowSpan)`
- On collapse: call `DataSource.CollapseRow(item.Id)`
- Spacer count behavior must match `ExpandableGridDataSource` (stores `ExpandedRowSpan + 1`).

4.	**Spacer rows are detected via the item itself**
- Spacer rows are represented as instances of `TGridItem` with encoded negative `Id`.
- Detection uses RowColumn parity method: `item.IsSpacer()`.

5.	**Identity requirement**
- Since spacer rows depend on encoded integer IDs, `TGridItem` must implement `IRowIdentifiable` with `int Id` (and `new()` to allow spacer materialization).

6.	**Refresh pathway**
- `ExpandableGridDataSource` already marks itself dirty / updates its cached `Items` view after expand/collapse.
- Like RowColumn, the feature should refresh the grid after expand/collapse (in ComposableColumns, via `context.RequestRefreshAsync/RequestRefresh`).
- No `OnDataChanged` subscription is required to match RowColumn behavior.

---

### 7.	No formal “state source of truth” when using spacers
•	You currently have two possible state carriers:
•	RowStateManager (expanded keys)
•	ExpandableGridDataSource (expanded row IDs + cache)
•	Missing: explicit precedence and synchronization:
•	If either gets out of sync, which wins?
•	Should feature drive data source, and data source be derived from feature state?
•	Or should expansion state live only in one place?
#### Answers:
- **Decision (RowColumn parity):** `RowStateManager<TGridItem>` is the source of truth for expanded/collapsed state.
- `ExpandableGridDataSource<TGridItem>` is a derived *layout projection* (spacer injection) used to preserve below-row overlay visibility.

###### Precedence rules
1.	**Render logic is driven by `RowStateManager`**
- The feature decides whether to render expanded mode by checking the state manager (`IsRowExpanded(...)`, `HasExpandedRows`, and the stored expanded context).
- Spacer presence must not be used to infer expanded state.

2.	**DataSource updates are driven by expand/collapse transitions**
- Expand:
  - state transition in `RowStateManager`
  - then call `DataSource.ExpandRow(item.Id, ExpandedRowSpan)` if `DataSource != null`
  - then request refresh
- Collapse:
  - state transition in `RowStateManager`
  - then call `DataSource.CollapseRow(item.Id)` if `DataSource != null`
  - then request refresh

###### Out-of-sync behavior
- If the state manager says a row is expanded but spacers are absent:
  - the overlay still renders; spacer injection is best-effort.
- If spacers exist but the state manager says no row is expanded:
  - spacer rows render as empty cells (`item.IsSpacer()`), but no overlay renders.
  - the next expand/collapse operation re-applies the correct projection.

###### Invariant
- After any expand/collapse initiated by the feature, `DataSource` should be consistent with the manager for that row Id.
- No external actor should directly mutate spacer expansion state without also going through the feature/state manager.

---

### 8.	Event semantics lack “failure surface” details for UI consistency
•	§8.5 says callback exceptions may be logged / propagated, but missing:
•	Are events invoked via InvokeAsync to guarantee correct circuit context?
•	What is the policy for exceptions in async void-ish paths (e.g., click handlers)?
•	Should OnStateChanged fire if OnBeforeExpand cancels? (Probably no, but not stated.)
#### Answers:
- **Primary rule:** all expand/collapse work (including event callbacks) must execute on the main Blazor UI thread.

###### UI-thread/dispatcher contract
1.	If `FeatureContext.InvokeAsync` is available, the feature MUST use it as the dispatcher boundary.
- Any expand/collapse entry point that could be invoked from outside a UI event (e.g., nested component calling `CollapseAsync`, timers, background callbacks) must marshal back:
  - `await context.InvokeAsync(() => ExpandRowAsync(item, ct))`
  - `await context.InvokeAsync(() => CollapseRowAsync(item, ct))`
- Event callbacks must be invoked inside the same dispatcher boundary when available:
  - `await context.InvokeAsync(() => OnExpanded.InvokeAsync(args))`
  - `await context.InvokeAsync(() => OnCollapsed.InvokeAsync(args))`

2.	If `FeatureContext.InvokeAsync` is not available, the feature assumes it is already executing on the UI thread (RowColumn parity).

###### Cancellation/ordering rules
- If `OnBeforeExpand` cancels, NO state mutation occurs and `OnStateChanged`/`OnExpanded` MUST NOT fire.
- Collapse ordering remains:
  - `OnStateChanged (Expanded -> Collapsed)` then `OnCollapsed`

###### Exception policy (simple)
- Event handler exceptions propagate to the caller (RowColumn parity).
- The feature must not attempt state rollback if an exception occurs after state mutation.

---

### 9.	Render/error strategy uses Console.Error (not a good fit for Blazor Server)
•	In Blazor Server, Console.Error.WriteLine is server-side console, not browser console.
•	Missing: a logging contract:
•	Use ILogger<RowExpandFeature<TGridItem>> via DI? or service from FeatureContext?
•	Or explicitly say “dev-only: Console on server”.
#### Answers:
- **Decision:** logging must not be feature-specific.
- The app uses the standard ASP.NET Core logging pipeline (Serilog-backed) configured in `Program.cs`.
- ComposableColumns features should log via a shared `ILoggerFactory` obtained through the owning component/context (not `Console.Error`).

###### Minimal contract
1.	`FeatureContext` may optionally expose a shared `ILoggerFactory` service, registered once by `ComposableColumn`/`ComposableGrid`.
2.	Features that need logging should request the shared factory from `FeatureContext` services and create a logger with an appropriate category/scope.
3.	If no factory is available, logging is a no-op (no `Console.Error` for normal operation).

---

### 10.	Prerequisite section references FeaturePriority.cs but doesn’t validate actual current state
•	The spec claims “✅ Exists” for some items, but doesn’t state whether FeaturePriority.Expansion = 350 is already present (it’s still described as a prerequisite + checklist item).
•	Missing: either:
•	Quote the actual FeaturePriority definition (authoritative), or
•	Change the checklist to “verify/add” without asserting status inside the spec.
#### Answers:
- **Status:** `FeaturePriority.Expansion = 350` is NOT currently present.
- The implementation work must add `FeaturePriority.Expansion = 350` between `Styling (300)` and `Editing (400)` and update any docs/checklists to reflect the new constant.

---
