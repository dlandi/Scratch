# Implementation Plan — `RowExpandFeature<TGridItem>`

## Introduction

This plan translates `Docs/Feature Design/ExpandableRowFeature.md` into an executable implementation roadmap for implementing `RowExpandFeature<TGridItem>` within the ComposableColumns architecture.

Scope constraints enforced by the spec:

- Target framework: .NET 9, Blazor (QuickGrid)
- Feature namespace root: `QuickGridTest01.ComposableColumns.*`
- Expansion feature namespace: `QuickGridTest01.ComposableColumns.Features.Expansion`
- Styling: **all CSS lives in `wwwroot/css/qgComposable-refined-minimalism.css`** (no feature `*.razor.css`)
- Legacy reference implementation: `QuickGridTest01.RowColumn.RowColumn<TGridItem>` and `Pages/RowColumnDemo.razor`

Non-goals:

- Do not implement `FormRowFeature` or `NestedGridFeature` here (only provide the base expansion facility they will build on).

---

## 1. Legacy analysis: legacy reference behavior (`RowColumn<TGridItem>`)

### 1.1 Behavioral parity checklist

The new feature must preserve these semantics from `RowColumn<TGridItem>`:

1. Spacer rows
   - Spacer rows are materialized as `TGridItem` instances with negative `Id`.
   - Spacer rows render an empty cell surface (no trigger, no overlay).
   - Spacer injection count is `ExpandedRowSpan + 1`.
2. Overlay rendering
   - Overlay is rendered inside the same *cell*, but visually begins *below* the master row (`top: 100%`).
   - Height is computed as `ExpandedHeight = ExpandedRowSpan * RowHeight`.
3. Trigger modes
   - If `DisplayTemplate` is provided, it is always used.
   - If `DisplayTemplate` is not provided:
     - `Button`: render the default expand button.
     - `RowClick`: render the chevron indicator.
     - `Custom`: render nothing.
4. Concurrency behavior (`ConcurrentExpandBehavior`)
   - `Block`: if any row is expanded, expanding a different row is disallowed.
   - `CollapseCurrent`: expand collapses the first expanded row first.
   - `AllowMultiple`: multiple expanded rows are allowed.
5. Dimming behavior
   - When any row is expanded and `DimInactiveRows == true`, non-expanded rows receive a dimmed style and become non-interactive.
6. Event ordering
   - Expand:
     1. `OnBeforeExpand` (cancellable)
     2. Potentially collapse another row (`CollapseCurrent`)
     3. `OnStateChanged` (Collapsed -> Expanded)
     4. `OnExpanded`
   - Collapse:
     1. `OnStateChanged` (Expanded -> Collapsed)
     2. `OnCollapsed`
7. State storage
   - Uses a memory-efficient store (`ConditionalWeakTable`) mapping row items to `RowExpandedContext<TGridItem>`.
8. UI thread boundary
   - In the composable version, all public methods + event callbacks must execute on the UI thread via `FeatureContext.InvokeAsync`.

---

## 2. ComposableColumns integration analysis

### 2.1 Rendering model differences

Legacy: `RowColumn` is a `QuickGrid` `ColumnBase<TGridItem>`.

Target: `RowExpandFeature<TGridItem>` is an `ICellRenderFeature<TGridItem>`.

The expansion feature is the cell content owner for its column.

---

## 3. Public API to implement

### 3.1 Feature type

Create:

- `QuickGridTest01.ComposableColumns.Features.Expansion.RowExpandFeature<TGridItem>`

Signature:

- `sealed class RowExpandFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable`
- Constraints: `where TGridItem : class, IRowIdentifiable, new()`

### 3.2 Parameters / properties (match spec)

1. Trigger and behavior
   - `RowTriggerMode TriggerMode { get; set; } = RowTriggerMode.Button;`
   - `ConcurrentExpandBehavior ConcurrentBehavior { get; set; } = ConcurrentExpandBehavior.Block;`
   - `bool DimInactiveRows { get; set; } = true;`

2. Row span and height
   - `int ExpandedRowSpan { get; set; } = 3;`
   - `int RowHeight { get; set; } = 48;`
   - `int ExpandedHeight => ExpandedRowSpan * RowHeight;`

3. Templates
   - `RenderFragment<RowDisplayContext<TGridItem>>? DisplayTemplate { get; set; }`
   - `RenderFragment<RowExpandedContext<TGridItem>> ExpandedTemplate { get; set; }` (required)

4. Spacer injection collaboration
   - `ExpandableGridDataSource<TGridItem>? DataSource { get; set; }`

5. Events
   - `EventCallback<RowBeforeExpandEventArgs<TGridItem>> OnBeforeExpand { get; set; }`
   - `EventCallback<RowExpandedEventArgs<TGridItem>> OnExpanded { get; set; }`
   - `EventCallback<RowCollapsedEventArgs<TGridItem>> OnCollapsed { get; set; }`
   - `EventCallback<RowStateChangedEventArgs<TGridItem>> OnStateChanged { get; set; }`

### 3.3 Public methods (with cancellation)

Expose methods per spec:

- `Task ExpandRowAsync(TGridItem item, CancellationToken cancellationToken = default)`
- `Task CollapseRowAsync(TGridItem item, CancellationToken cancellationToken = default)`
- `Task CollapseAllAsync(CancellationToken cancellationToken = default)`

These methods must:

- execute on UI thread when dispatcher exists (`context.InvokeAsync`)
- propagate handler exceptions
- not perform rollback if exceptions occur

---

## 4. Types + contracts to create under `ComposableColumns.Features.Expansion`

### 4.1 Context objects

Create:

- `RowDisplayContext<TGridItem>`
  - `TGridItem Item { get; init; }`
  - `bool IsAnyRowExpanded { get; init; }`
  - `bool CanExpand { get; init; }`
  - `Func<Task> ExpandAsync { get; init; }`

- `RowExpandedContext<TGridItem>`
  - `TGridItem Item { get; init; }`
  - `Func<Task> CollapseAsync { get; init; }`

Cascading contract:

- In expanded mode, cascade `RowExpandedContext<TGridItem>` via `CascadingValue<T>`.

### 4.2 Enums

Create:

- `RowTriggerMode` (`Button`, `RowClick`, `Custom`)
- `ConcurrentExpandBehavior` (`Block`, `CollapseCurrent`, `AllowMultiple`)
- `RowExpandedState` (`Collapsed`, `Expanded`)

### 4.3 Event args

Create under `ComposableColumns.Features.Expansion.Events`:

- `RowBeforeExpandEventArgs<TGridItem> { TGridItem Item; bool Cancel; }`
- `RowExpandedEventArgs<TGridItem> { TGridItem Item; }`
- `RowCollapsedEventArgs<TGridItem> { TGridItem Item; }`
- `RowStateChangedEventArgs<TGridItem> { TGridItem Item; RowExpandedState OldState; RowExpandedState NewState; }`

### 4.4 State manager

Create `RowStateManager<TGridItem>` under `ComposableColumns.Features.Expansion.State`.

Key requirements:

- Memory-efficient storage: `ConditionalWeakTable<TGridItem, RowExpandedContext<TGridItem>>`
- Expanded rows tracking: `HashSet<TGridItem>`
- Thread safety: `SemaphoreSlim` lock (match legacy)

### 4.5 Spacer collaboration types

Create under `ComposableColumns.Features.Expansion.Data`:

- `ExpandableGridDataSource<TGridItem>`
- `SpacerRowFactory`

Requirements:

- Spacer ids are computed with the negative id encoding:
  - `spacerId = -(parentId * 1000 + offset)`
- `ExpandableGridDataSource.ExpandRow(rowId, ExpandedRowSpan)` stores `ExpandedRowSpan + 1` spacers.
- `ExpandableGridDataSource.Items` is the `IQueryable<TGridItem>` used to bind the grid.

### 4.6 Row identity / RowKey rule

Create `IRowIdentifiable` under `ComposableColumns.Features.Expansion.Core`:

- `int Id { get; set; }`

Row identity is `IRowIdentifiable.Id`.

- `TGridItem : IRowIdentifiable` is the source of truth.
- If `FeatureContext.RowKey` is provided and does not resolve to `int`, `RowExpandFeature` uses `item.Id`.

---

## 5. `RowExpandFeature` internal design

### 5.1 Service registration + sentinel

- `RowExpandFeature.OnAttach(...)` MUST cache the provided `FeatureContext<TGridItem>` in a private field.
- `RowExpandFeature.OnAttach(...)` MUST fail fast by throwing an `InvalidOperationException` if `FeatureContext.GetService<RowStateManager<TGridItem>>()` returns a non-null value.
- `RowExpandFeature.RenderCell(...)` MUST lazily create and register a `RowStateManager<TGridItem>` via `FeatureContext.RegisterService(...)` if and only if it is not already registered.

### 5.2 `InvokeAsync` rule

- `FeatureContext.InvokeAsync` is required.
- `FeatureContext.RequestRefreshAsync` is required.

If either is null, `RowExpandFeature.OnAttach(...)` MUST throw an `InvalidOperationException`.

### 5.3 Rendering contract (`renderNext`)

- `RenderCell(...)` MUST NOT call `renderNext()`.

### 5.4 Expand algorithm

`ExpandRowAsync(item, ct)`:

1. return if spacer row.
2. fire `OnBeforeExpand`; if cancelled return.
3. enforce concurrency:
   - if any expanded and not `AllowMultiple`:
     - get first expanded row; if not same reference:
       - `Block`: return
       - `CollapseCurrent`: collapse first expanded row (await)
4. create expanded context:
   - `stateManager.GetOrCreateContextAsync(item, collapseAsync: () => CollapseRowAsync(item, ct), ct)`
5. if `DataSource != null`: `DataSource.ExpandRow(item.Id, ExpandedRowSpan)`
6. fire `OnStateChanged` (Collapsed->Expanded)
7. fire `OnExpanded`
8. request refresh: `await context.RequestRefreshAsync!()`

### 5.5 Collapse algorithm

`CollapseRowAsync(item, ct)`:

1. return if not expanded.
2. `stateManager.RemoveRowAsync(item, ct)`
3. if `DataSource != null`: `DataSource.CollapseRow(item.Id)`
4. fire `OnStateChanged` (Expanded->Collapsed)
5. fire `OnCollapsed`
6. request refresh: `await context.RequestRefreshAsync!()`

### 5.6 CollapseAll algorithm

`CollapseAllAsync(ct)`:

1. `stateManager.ClearAllAsync(ct)`
2. `DataSource?.CollapseAll()`
3. request refresh: `await context.RequestRefreshAsync!()`

### 5.7 CSS class model

The feature MUST emit these class names (RowColumn parity):

- wrapper: `row-cell`
- when expanded: `row-expanded`
- when dimmed: `row-dimmed`
- spacer: `row-spacer`
- overlay container: `row-overlay`
- row click indicator: `row-click-indicator`

All styles MUST live in `wwwroot/css/qgComposable-refined-minimalism.css`.

### 5.8 Default close requirement + `RowCard` strategy

- `RowExpandFeature` MUST include a default close button in the overlay chrome.
- This default close button MUST call `CollapseRowAsync(item)`.

`RowCard` requirements (strict RowColumn parity):

- New component namespace: `QuickGridTest01.ComposableColumns.Features.Expansion.Components`
- Parameters:
  - `Title` (optional)
  - `Class` (optional)
  - `HeaderActions` (`RenderFragment?`)
  - `FooterContent` (`RenderFragment?`)
  - `ShowCloseButton` (`bool`)
  - `OnClose` (`Func<Task>?`) (optional)
  - `ChildContent` (`RenderFragment?`)

`RowCard` close behavior:

- If `OnClose` is not supplied, the close button MUST call the nearest cascaded `RowExpandedContext<TGridItem>.CollapseAsync`.

---

## 6. Feature implementation plan

Landing page sections:

- **Introduction** (this section)

- **1. Legacy analysis**

- **2. ComposableColumns integration analysis**

- **3. Public API to implement**

- **4. Types + contracts to create under `ComposableColumns.Features.Expansion`**

- **5. `RowExpandFeature` internal design**

- **7. Consumer usage patterns (Blazor)**

- **8. Test strategy**

- **9. Execution sequence (implementation order)**

- **10. Acceptance criteria**

---

## 7. Consumer usage patterns (Blazor)

### 7.2 Column usage (ComposableColumn)

Add a dedicated demo page:

- Create `Pages/ComposableRowExpandDemo.razor` demonstrating `RowExpandFeature`.

The demo MUST use a concrete demo model type defined in the demo page:

- `sealed class DemoRow : IRowIdentifiable`
  - `int Id { get; set; }`
  - `string Name { get; set; }`
  - `string Email { get; set; }`

The demo MUST include:

- a grid bound to `ExpandableGridDataSource<DemoRow>.Items` (spacer injection enabled)
- `QuickGrid` row key configured to `item => item.Id`
- a `ComposableColumn<DemoRow, string>` expansion column that only renders `RowExpandFeature<DemoRow>` in its feature collection
- `ExpandedTemplate` that uses `ComposableColumns.Features.Expansion.Components.RowCard` and can close

---

## 8. Test strategy

Tests MUST be unit tests in `QuickGridTest01.Tests` for the non-UI types only:

- `SpacerRowFactory`
- `ExpandableGridDataSource<TGridItem>`
- `RowStateManager<TGridItem>`

No component test framework (bUnit) is introduced as part of this feature.

---

## 9. Execution sequence (implementation order)

1. Add `FeaturePriority.Expansion = 350`.
2. Create ComposableColumns expansion types (enums, contexts, events, identity).
3. Create spacer injection types (`SpacerRowFactory`, `ExpandableGridDataSource`).
4. Create `RowStateManager<TGridItem>`.
5. Implement `RowExpandFeature<TGridItem>`.
6. Create composable `RowCard`.
7. Add CSS to the global stylesheet.
8. Create `Pages/ComposableRowExpandDemo.razor`.
9. Add unit tests for non-UI types.
10. Build + run tests.

---

## 10. Acceptance criteria

The feature is complete when:

- `RowExpandFeature<TGridItem>` compiles and is usable from a `ComposableColumn`.
- Rows expand/collapse with overlay height equal to `ExpandedRowSpan * RowHeight`.
- Spacer rows are correctly injected when the grid is bound to `ExpandableGridDataSource<TGridItem>.Items`.
- Trigger mode fallback behavior matches the rules in section 1.1.
- Concurrency modes match section 1.1.
- Events fire in the order defined in section 1.1 and `OnBeforeExpand` cancellation prevents state mutation.
- Dimming behavior applies `row-dimmed` to inactive rows and disables interaction via CSS.
- All expansion styling is in `wwwroot/css/qgComposable-refined-minimalism.css`.
- All new expansion types are under `QuickGridTest01.ComposableColumns.Features.Expansion.*`.
- The demo page `Pages/ComposableRowExpandDemo.razor` demonstrates expansion and spacer injection.
