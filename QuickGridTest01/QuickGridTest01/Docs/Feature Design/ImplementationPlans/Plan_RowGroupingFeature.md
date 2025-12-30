# Implementation Plan — `GroupingFeature<TGridItem, TValue>`

## Introduction

This plan translates `Docs/Feature Design/RowGroupingFeature.md` into an executable implementation roadmap for implementing `GroupingFeature<TGridItem, TValue>` within the ComposableColumns architecture.

Scope constraints enforced by the spec:

- Target framework: .NET 9, Blazor Server (QuickGrid)
- Feature namespace root: `QuickGridTest01.ComposableColumns.*`
- Grouping feature namespace: `QuickGridTest01.ComposableColumns.Features.Grouping`
- Styling: **all CSS lives in `wwwroot/css/qgComposable-refined-minimalism.css`** (no feature `*.razor.css`)
- Pattern reference: `ExpandableRowFeature.md` and existing `ExpandableGridDataSource<T>` spacer pattern

Non-goals:

- Do not implement nested/multi-level grouping (API has `Level` property for future extensibility).
- Do not implement drag-to-reorder groups.
- Do not implement LocalStorage persistence of group state.

---

## Plan execution anchors (for task generation)

This plan includes implementation details in section `2.4.x`. For task generation and stable references, use the following lifecycle anchors (aligned to Spec Section 2.6):

- **Phase 1 (Initialization):** Grid/column initialization and feature attachment prerequisites
- **Phase 2 (Feature attachment):** `GroupingFeature.OnAttach` creates/registers coordinator + registers column
- **Phase 3 (Grid rendering):** QuickGrid enumerates the bound `Items` sequence (which may be a grouped data source)
- **Phase 4 (Data transformation):** Grouped data source rebuild injects group header marker + spacer rows (as `TGridItem`)
- **Phase 5 (Row rendering):** Existing cell pipeline renders group headers from the header-host column feature (no grid row hooks)
- **Phase 6 (User interaction):** Toggle group, Expand All, Collapse All (async end-to-end + grouped data source `OnDataChanged` → grid `InvokeAsync(StateHasChanged)`)
- **Phase 7 (Disposal):** Dispose feature/state/coordinator

**Deterministic activation rule:** Grouping activation is an internal grid state. `ComposableGrid` treats grouping as active when the grid-scoped coordinator has a non-null `ActiveGrouping`.

## 1. MudBlazor parity analysis

### 1.1 Behavioral parity checklist

The new feature must provide these semantics from MudBlazor's `MudDataGrid` grouping:

| MudBlazor | GroupingFeature | Notes |
|-----------|-----------------|-------|
| `Groupable="true"` | Feature attached to column | Column is groupable |
| `Grouping="true"` | `IsActive = true` | This column's grouping is active |
| `GroupBy` expression | `GroupBy` parameter | Key extraction |
| `GroupTemplate` | `HeaderTemplate` | Custom header rendering |
| `GroupExpanded` | `InitiallyExpanded` | Initial expand state |

### 1.2 Key behaviors to implement

1. **Single-level grouping**
   - Only one column can have active grouping at a time
   - First column with `IsActive = true` wins

2. **Group header rows**
   - The grid uses QuickGrid virtualization with a fixed `ItemSize` (fixed row height)
   - Header height is expressed in *row slots* using `GroupHeaderSlotSpan`
     - Default: `2`
     - Effective pixel height: `GroupHeaderSlotSpan × ItemSize`
   - To remain aligned with existing ComposableColumns virtualization-style features (e.g., Row Expansion), multi-slot headers are represented using **real spacer rows**:
     - A group header occupies `GroupHeaderSlotSpan` *rendered rows* in the Items sequence
     - The first row is the **group header marker row** (renders the header via the header-host column)
     - The remaining `GroupHeaderSlotSpan - 1` rows are **group header spacer rows** (blank output) and exist only for height/virtualization alignment
   - Rendered as an overlay-like full-width header emitted from the header-host column (similar to Expansion overlay rendering)

   **Styling/virtualization alignment requirement:** The rendered pixel height of a group header must match the QuickGrid row virtualization `ItemSize`. The grid is permitted to add minimal markup/attributes **solely** to provide CSS variables for sizing/alignment (Spec §2.5.0), for example:

   - Wrapper: `.qg-grid-wrapper`
   - Style: `style="--qg-item-size: {ItemSize}px; --qg-group-header-slot-span: {GroupHeaderSlotSpan};"`

   This plan's default CSS uses `--qg-item-size` and `--qg-group-header-slot-span` to compute header height.

3. **Expand/collapse state**
   - Feature owns typed `GroupStateManager<TValue>` internally
   - Coordinator delegates to active feature via interface methods
    - No fire-and-forget: toggling is async end-to-end to guarantee "state updated → refresh" ordering

4. **Virtualization support (required)**
   - Collapsed group: contributes `GroupHeaderSlotSpan` to virtual count (header only)
   - Expanded group: contributes `GroupHeaderSlotSpan + itemCount` (header + items)
   - Expand/collapse triggers grouped data source `OnDataChanged` (grid re-renders)

5. **Group ordering**
   - `Ascending`: A-Z, 0-9
   - `Descending`: Z-A, 9-0
   - `FirstOccurrence`: Order by first item occurrence in source

**Sorting interaction (deterministic):** When grouping is active, any column sorting is applied **within each group only**. Group ordering is controlled only by `GroupOrder` / `GroupOrderComparer` (or `FirstOccurrence`).

**Concrete suppression mechanism (normative):** When grouping is active, QuickGrid column sorting must be suppressed by ensuring `ComposableColumn.SortBy` is `null` for all columns. Sorting is instead owned by `ComposableGrid<TGridItem>` as `SortedItems`, derived from active `ISortingFeature<TGridItem>` state.

**Data pipeline (deterministic, identity stages permitted):**

- `Items`
- `FilteredItems` (if no filtering features are present/active, this is `Items`)
- `SortedItems` (if no ComposableColumns sort is active, this is `FilteredItems`)
- `GroupedItems` (only when grouping is active; derived from `SortedItems`)

**Pipeline invariants (stage aliasing):**

- `FilteredItems` always consumes `Items`. If no filters are present/active, `FilteredItems = Items`.
- `SortedItems` always consumes `FilteredItems`. If no ComposableColumns sort is active, `SortedItems = FilteredItems`.
- When grouping is inactive, the grid binds `QuickGrid.Items` to `SortedItems`.
- When grouping is active, the grid binds `QuickGrid.Items` to `GroupedItems(SortedItems)`.

6. **Null key handling**
   - `SeparateGroup`: Null group follows `GroupOrder` (first in Ascending, last in Descending)
   - `ShowAtTop`: Regular rows at top, no group header
   - `ShowAtBottom`: Regular rows at bottom, no group header
   - `Exclude`: Items with null keys not displayed

7. **Filter interaction**
   - `FilterThenGroup`: When used with `ComposableGrid`, grouping receives `FilteredItems` (already filtered)
   - `GroupThenFilter`: Not supported in the current ComposableGrid integration model
   - `HideEmptyGroups`: Groups with 0 items after filtering are hidden

8. **Grouping toolbar controls (Expand All / Collapse All)**
   - Rendered by the header-host column feature as part of the header overlay
   - Rendered **once per grid** and gated by the **FIRST marker row rule** (Spec §2.5.2.2)
   - Default toolbar UI is rendered unless the programmer supplies a `ToolbarTemplate`
   - Commands route to the active feature (`ExpandAllGroupsAsync` / `CollapseAllGroupsAsync`) and are awaited
   - Actions trigger grouped data source `OnDataChanged` (grid re-renders)

---

## 2. ComposableColumns integration analysis

> **Lifecycle Reference:** The spec's Section 2.6 documents the complete feature lifecycle with phase-by-phase timing diagrams. This plan implements that lifecycle. When timing questions arise, consult the spec.

### 2.1 Rendering model

Target: `GroupingFeature<TGridItem, TValue>` implements:
- `IColumnFeature<TGridItem>` - Base feature interface
- `IGridDataTransformer<TGridItem>` - New interface for data transformation features

The grouping feature transforms the grid's data source by injecting group header rows.

### 2.2 Priority

`FeaturePriority.Grouping = 50` (before Core at 100)

**Rationale:** Grouping transforms data shape before other features process it. Group header rows must exist before Core, Filtering, Formatting, Styling, and Editing features run.

### 2.3 Coordinator pattern

The feature uses a **Column-First Coordinator Pattern**:

1. First `GroupingFeature` to attach creates `GroupingCoordinator<TGridItem>`
2. Coordinator is stored in a scope shared by all grouping columns on the same grid
   - **Required:** grid-scoped ownership (via cascaded `ComposableGrid<TGridItem>`), mirroring the existing filter registration pattern
3. **All** grouping-enabled columns register with the coordinator using a column ID (complete set of groupable columns)
4. The **first** registered grouping column becomes the header-host column (renders header UI)
5. Only one column can be active at a time (first wins among `IsActive=true`)
6. Grouped Items are produced by an expansion-style data source that emits marker/spacer rows as `TGridItem`

### 2.4 Integration point (Expansion-style, no ComposableGrid row hooks)

> **Spec Reference:** See Spec Section 2.5.

**Summary:** grouping integrates like `ComposableRowExpandDemo.razor`:

1. The grid binds `QuickGrid.Items` to an `IQueryable<TGridItem>` that may include inserted marker/spacer rows.
2. Group headers are represented using real rows in the sequence:
   - 1x **group header marker row** (the FIRST header row)
   - `GroupHeaderSlotSpan - 1` x **group header spacer rows**
3. A dedicated first column feature (`ICellRenderFeature<TGridItem>`) renders:
   - header UI only for the marker row
   - blank for spacer rows
   - blank (or optional indent) for normal data rows
4. The coordinator provides the set of groupable columns for the header UI.

#### 2.4.0 Marker/spacer identity contract (required)

Before implementing the grouping data source or header-host column feature, define a `SpacerRowFactory`-like helper for group header ids, for example `GroupHeaderRowId`.

Requirements:

- Normal data row: `Id > 0`
- Group header marker row: detectable and unique ("FIRST" header row)
- Group header spacer row: detectable and associated with the correct group id + spacer offset

The header-host column feature must render header UI **only** for the marker row and must render blank output for header spacer rows.

**Stability rule (preferred):** The grouped data source maintains a cached mapping from group key (`object?`) to `groupId` (int). Ids are assigned on first observation and reused across expand/collapse refreshes for deterministic row identity.

#### 2.4.1 Coordinator access

Grouping columns must be able to access the same `GroupingCoordinator<TGridItem>` instance so that subsequent grouping columns contribute to the list of groupable columns.

**Constraint:** The existing `FeatureContext<TGridItem>` is created per `ComposableColumn`, so `FeatureContext.RegisterService(...)` does not provide a grid-wide scope.

**Normative plan rule (Filtering pattern):** The coordinator is stored on the grid instance, and grouping features access it via an `internal` grid API.

- `ComposableGrid<TGridItem>` owns a private field: `_groupingCoordinator`
- `ComposableGrid<TGridItem>` exposes: `internal GroupingCoordinator<TGridItem> GetOrCreateGroupingCoordinator()`
- `GroupingFeature<TGridItem, TValue>.OnAttach(...)` calls `grid.GetOrCreateGroupingCoordinator()` and registers itself.

#### 2.4.6 Toggle/refresh flow (no fire-and-forget)

The grouping toggle path must be async to avoid lost exceptions and stale state during virtualization refresh.

**Pattern reference:** `ExpandableGridDataSource<TGridItem>` in the Expansion feature.

- `GroupHeaderContext.ToggleAsync` is `Func<Task>` and must be awaited.
- The grouped data source exposes `Task ToggleGroupAsync(object key)`.
- The grouped data source updates its internal state/cache deterministically, then raises `OnDataChanged`.
- `ComposableGrid` subscribes to `OnDataChanged` (like it does for filters) and calls `InvokeAsync(StateHasChanged)`.

**Single refresh authority:** Grouping should not also call `FeatureContext.RequestRefreshAsync()` for these state changes; `OnDataChanged` is the single refresh signal (avoids double-refresh loops).

The same async/await rule applies to Expand All / Collapse All:

- Button/template handlers must await `ActiveGrouping.ExpandAllGroupsAsync()` / `ActiveGrouping.CollapseAllGroupsAsync()`.
- The grouped data source raises `OnDataChanged` after the awaited operation completes.

#### 2.4.2 Data source selection

When using an expansion-style grouped data source, the application binds `QuickGrid.Items` to that data source's `Items` (same as `ComposableRowExpandDemo.razor`).

#### 2.4.4.1 Virtualization integration

`ComposableGrid` does not need to implement any custom windowing or `startIndex/count` mapping. Grouping achieves virtualization alignment by emitting real spacer rows for multi-row headers.

QuickGrid virtualization operates over the resulting flattened Items sequence.

#### 2.4.3 Rendering implementation

Rendering is done through the existing `ComposableColumn` cell pipeline.

- The header-host column feature detects marker/spacer rows and renders the header UI only for the FIRST marker row.
- All other columns render empty output for marker/spacer rows.

#### 2.4.4 Virtualization

QuickGrid virtualization remains aligned by representing header height using real rows (spacer-row injection), exactly as in expansion.

#### 2.4.5 Spacer-row virtualization mapping (required for fixed `ItemSize`)

Because QuickGrid virtualization uses a fixed `ItemSize`, grouping must ensure the rendered row sequence reflects the correct physical height.

Definitions:

- `ItemSize` = grid row height (px) used by QuickGrid virtualization.
- `GroupHeaderSlotSpan` = how many item rows a group header occupies (default `2`).
- Data rows always occupy `1` row.

Rules:

1. Each group header contributes `GroupHeaderSlotSpan` rows to the Items sequence:
   - 1× group header marker row (FIRST header row)
   - `GroupHeaderSlotSpan - 1` × group header spacer rows

2. No slot-to-row translation is required; virtualization operates over the flattened row sequence.

**Scope note:** This design does not require `ComposableGrid` row rendering modifications. Grouping is implemented via `TGridItem` marker/spacer rows and `ICellRenderFeature<TGridItem>`.

---

## 3. Public API to implement

### 3.1 Feature type

Create:

- `QuickGridTest01.ComposableColumns.Features.Grouping.GroupingFeature<TGridItem, TValue>`

Signature:

```csharp
public sealed class GroupingFeature<TGridItem, TValue> 
    : IColumnFeature<TGridItem>, IGridDataTransformer<TGridItem>, IDisposable
    where TGridItem : class
{
    public int Priority => FeaturePriority.Grouping; // 50
}
```

**Type constraints:**
- `TGridItem : class` - Required by ComposableColumns architecture
- `TValue` - No constraint (works with reference types, value types, nullable types)

### 3.2 Parameters / properties

1. **Core parameters**
   - `bool IsActive { get; set; } = true;`
   - `Func<TGridItem, TValue>? GroupBy { get; set; }` - Falls back to column's `Property` via `FeatureContext.GetValue`
   - `bool InitiallyExpanded { get; set; } = true;`
    - `int GroupHeaderSlotSpan { get; set; } = 2;`
      - Used for fixed `ItemSize` virtualization. Header height is `GroupHeaderSlotSpan × ItemSize`.
      - Must be `>= 1`.

2. **Templates**
   - `RenderFragment<GroupHeaderContext<TGridItem, TValue>>? HeaderTemplate { get; set; }`
   - `RenderFragment<GroupToolbarContext>? ToolbarTemplate { get; set; }`

3. **Sorting & ordering**
   - `GroupSortDirection GroupOrder { get; set; } = GroupSortDirection.Ascending;`
   - `IComparer<TValue>? GroupOrderComparer { get; set; }`

4. **Filtering interaction**
   - `FilterGroupOrder FilterBehavior { get; set; } = FilterGroupOrder.FilterThenGroup;`
   - `bool HideEmptyGroups { get; set; } = true;`

5. **Null key handling**
   - `NullKeyBehavior NullKeyHandling { get; set; } = NullKeyBehavior.SeparateGroup;`
   - `string NullGroupLabel { get; set; } = "(No Value)";`

6. **Key comparison**
   - `IEqualityComparer<TValue>? KeyComparer { get; set; }`

7. **UI controls**
   - `bool ShowExpandCollapseAllButtons { get; set; } = false;`

### 3.3 Public methods

Expose methods for programmatic control:

- `Task ExpandGroupAsync(TValue key, CancellationToken ct = default)`
- `Task CollapseGroupAsync(TValue key, CancellationToken ct = default)`
- `Task ExpandAllGroupsAsync(CancellationToken ct = default)`
- `Task CollapseAllGroupsAsync(CancellationToken ct = default)`
- `bool IsGroupExpanded(TValue key)`

---

## 4. Types + contracts to create under `ComposableColumns.Features.Grouping`

### 4.1 Context objects

Create `GroupHeaderContext<TGridItem, TValue>`:

```csharp
public record GroupHeaderContext<TGridItem, TValue>(
    TValue? Key,
    IReadOnlyList<TGridItem> Items,
    int Count,
    bool IsExpanded,
    Func<Task> ToggleAsync,
    int Level,
    string NullGroupLabel
) where TGridItem : class;
```

### 4.2 Synthetic row identity helper

Create `GroupHeaderRowId` (SpacerRowFactory-like) to encode and decode group header marker/spacer ids.

### 4.3 Enums

Create under `ComposableColumns.Features.Grouping.Enums`:

```csharp
public enum GroupSortDirection
{
    Ascending,
    Descending,
    FirstOccurrence
}

public enum FilterGroupOrder
{
    FilterThenGroup,
    GroupThenFilter
}

public enum NullKeyBehavior
{
    SeparateGroup,
    ShowAtTop,
    ShowAtBottom,
    Exclude
}
```

### 4.4 Core interface

Create `IGridDataTransformer<TGridItem>` under `ComposableColumns.Core`:

```csharp
public interface IGridDataTransformer<TGridItem> : IColumnFeature<TGridItem>
    where TGridItem : class
{
    bool IsTransformActive { get; }
    string CoordinatorKey { get; }
}
```

### 4.5 Grouping interface

Create `IGroupingFeature<TGridItem>`:

```csharp
public interface IGroupingFeature<TGridItem>
    where TGridItem : class
{
    // Configuration properties (object-typed for coordinator compatibility)
    bool IsActive { get; }
    Func<TGridItem, object>? GroupByUntyped { get; }
    bool InitiallyExpanded { get; }
    GroupSortDirection GroupOrder { get; }
    IComparer<object>? GroupOrderComparerUntyped { get; }
    FilterGroupOrder FilterBehavior { get; }
    bool HideEmptyGroups { get; }
    NullKeyBehavior NullKeyHandling { get; }
    string NullGroupLabel { get; }
    int GroupHeaderSlotSpan { get; }
    IEqualityComparer<object>? KeyComparerUntyped { get; }
    bool ShowExpandCollapseAllButtons { get; }

    RenderFragment<GroupToolbarContext>? ToolbarTemplate { get; }

    // State management methods (delegated from coordinator)
    Task ToggleGroupAsync(object key);
    bool IsGroupExpanded(object key);
    Task ExpandAllGroupsAsync();
    Task CollapseAllGroupsAsync();

    /// <summary>
    /// Render the group header. Called by the header-host column feature when it encounters a group header marker row.
    /// The feature internally casts the object key back to TValue and renders either the custom HeaderTemplate
    /// (if provided) or the default template.
    /// </summary>
    void RenderGroupHeader(
        Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder,
        object? key,
        IReadOnlyList<TGridItem> items,
        int count,
        bool isExpanded,
        int level);
}
```

**Template rendering:** Group header UI is rendered by the header-host column feature (an `ICellRenderFeature<TGridItem>`) when it encounters a group header marker row.

### 4.6 State manager

Create `GroupStateManager<TValue>`:

```csharp
public class GroupStateManager<TValue> : IDisposable
{
    private readonly HashSet<TValue> _expandedGroups;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly IEqualityComparer<TValue> _comparer;

    public GroupStateManager(IEqualityComparer<TValue>? comparer = null)
    {
        _comparer = comparer ?? EqualityComparer<TValue>.Default;
        _expandedGroups = new HashSet<TValue>(_comparer);
    }

    public bool HasExpandedGroups => _expandedGroups.Count > 0;
    public int ExpandedGroupCount => _expandedGroups.Count;

    public bool IsExpanded(TValue key);
    public Task ToggleAsync(TValue key, CancellationToken ct = default);
    public Task ExpandAsync(TValue key, CancellationToken ct = default);
    public Task CollapseAsync(TValue key, CancellationToken ct = default);
    public Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken ct = default);
    public Task CollapseAllAsync(CancellationToken ct = default);
    public Task InitializeAsync(IEnumerable<TValue> allKeys, bool initiallyExpanded, CancellationToken ct = default);

    public void Dispose();
}
```

### 4.7 Coordinator

Create `GroupingCoordinator<TGridItem>`:

```csharp
internal class GroupingCoordinator<TGridItem> : IDisposable
    where TGridItem : class
{
    private readonly Dictionary<string, IGroupingFeature<TGridItem>> _groupableColumns = new();
    private string? _activeColumnId;

    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    // No StateManager property - feature owns typed state, coordinator delegates

    /// <summary>
    /// Register a column's grouping capability.
    /// Throws InvalidOperationException if columnId already registered.
    /// </summary>
    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature)
    {
        ArgumentNullException.ThrowIfNull(columnId);
        ArgumentNullException.ThrowIfNull(feature);

        if (_groupableColumns.ContainsKey(columnId))
            throw new InvalidOperationException($"Column '{columnId}' is already registered.");

        _groupableColumns[columnId] = feature;
    }

    /// <summary>
    /// Set which column's grouping is active.
    /// Pass null to disable grouping. Throws if columnId not registered.
    /// </summary>
    public void SetActiveGrouping(string? columnId)
    {
        if (columnId is null)
        {
            ActiveGrouping = null;
            _activeColumnId = null;
            return;
        }

        if (!_groupableColumns.TryGetValue(columnId, out var feature))
            throw new InvalidOperationException($"Column '{columnId}' is not registered.");

        ActiveGrouping = feature;
        _activeColumnId = columnId;
    }

    /// <summary>
    /// Transform items into a flattened `IQueryable<TGridItem>` sequence containing:
    /// - group header marker rows
    /// - group header spacer rows
    /// - normal data rows
    /// </summary>
    public IQueryable<TGridItem> TransformItems(IQueryable<TGridItem> items);

    public void Dispose()
    {
        _groupableColumns.Clear();
        ActiveGrouping = null;
    }
}
```

### 4.8 Data source wrapper

Create an expansion-style grouped data source wrapping the original data and injecting marker/spacer rows:

```csharp
public class GroupedGridDataSource<TGridItem>
    where TGridItem : class
{
    public IQueryable<TGridItem> Items { get; }
    public event Action? OnDataChanged;
}
```

**Grid subscription (required):** `ComposableGrid` subscribes to `OnDataChanged` and triggers `InvokeAsync(StateHasChanged)`.

**Lifecycle:** Grid-owned and grid-scoped (Spec §5.6.1–§5.6.3). The grid creates/caches/replaces the instance and manages event subscription/unsubscription.

---

## 5. `GroupingFeature` internal design

> **Lifecycle Reference:** See Spec Section 2.6.2 for attachment lifecycle, Section 2.6.4 for data transformation timing, and Section 2.6.8 for timing constraints.

### 5.1 OnAttach implementation

```csharp
public void OnAttach(FeatureContext<TGridItem> context)
{
    _context = context ?? throw new ArgumentNullException(nameof(context));

    // Validate required context capabilities
    if (context.InvokeAsync is null)
        throw new InvalidOperationException("FeatureContext.InvokeAsync is required.");
    // RequestRefreshAsync is not required for grouping state changes.
    // Refresh is driven by GroupedGridDataSource.OnDataChanged -> grid InvokeAsync(StateHasChanged).

    // Resolve effective GroupBy
    if (context is FeatureContext<TGridItem, TValue> typedContext)
    {
        _effectiveGroupBy = GroupBy ?? typedContext.GetValue;
    }
    else
    {
        _effectiveGroupBy = GroupBy;
    }

    if (_effectiveGroupBy is null)
        throw new InvalidOperationException(
            "GroupBy must be specified or column must have a Property.");

    // Coordinator is grid-scoped (mirrors filter registration pattern)
    if (Context.Column is ComposableColumn<TGridItem, TValue> col && col.Grid is not null)
    {
        // feature obtains coordinator from the grid (exact mechanism specified in RowGroupingFeature.md)
    }

    // Register this column
    var columnId = GetColumnId();
    _columnId = columnId;
    _coordinator.RegisterColumn(columnId, this);

    // Set as active if IsActive and no other active grouping (first wins)
    if (IsActive && _coordinator.ActiveGrouping is null)
    {
        _coordinator.SetActiveGrouping(columnId);
        _stateManager = new GroupStateManager<TValue>(KeyComparer);
        // State initialized lazily - see Spec Section 2.6.4
    }
}
```

### 5.1.1 State initialization timing

> **Spec Reference:** Per Spec Section 2.6.4 and 2.6.8, state initialization is lazy because group keys are not available until Items flow through TransformItems.

```csharp
State initialization remains lazy and occurs when keys are first discovered during transformation.
However, the transformation output is a flattened `IQueryable<TGridItem>` with marker/spacer rows, not a discriminated union.
```

### 5.2 Column ID extraction

```csharp
private static int _columnIdCounter;

private string GetColumnId()
{
    var propertyName = GetPropertyNameFromDelegate(_effectiveGroupBy);
    
    if (!string.IsNullOrEmpty(propertyName))
        return propertyName;
    
    return $"GroupingColumn_{Interlocked.Increment(ref _columnIdCounter)}";
}

private static string? GetPropertyNameFromDelegate<T>(Func<TGridItem, T>? func)
{
    if (func?.Target is null) return null;
    var field = func.Target.GetType().GetFields()
        .FirstOrDefault(f => f.FieldType == typeof(Func<TGridItem, T>));
    return field?.Name;
}
```

### 5.3 IGroupingFeature implementation (type bridging)

```csharp
// Typed public API
public GroupStateManager<TValue> StateManager => _stateManager!;

// Untyped interface for coordinator
Func<TGridItem, object>? IGroupingFeature<TGridItem>.GroupByUntyped 
    => item => _effectiveGroupBy!(item)!;

Task IGroupingFeature<TGridItem>.ToggleGroupAsync(object key)
{
    if (key is TValue typedKey)
        return _stateManager!.ToggleAsync(typedKey);
    return Task.CompletedTask;
}

bool IGroupingFeature<TGridItem>.IsGroupExpanded(object key)
{
    return key is TValue typedKey && _stateManager!.IsExpanded(typedKey);
}
```

### 5.4 Virtualization constants

```csharp
/// <summary>
/// Group header height expressed in virtual row slots (not pixels).
/// Effective pixel height is GroupHeaderSlotSpan × QuickGrid ItemSize.
/// </summary>
internal const int DefaultGroupHeaderSlotSpan = 2;
```

### 5.5 CSS class model

The feature MUST emit these class names:

| Class | Purpose |
|-------|---------|
| `.qg-group-header` | Group header row container |
| `.qg-group-header.expanded` | Expanded state |
| `.qg-group-header.collapsed` | Collapsed state |
| `.qg-group-chevron` | Expand/collapse icon |
| `.qg-group-key` | Group key text |
| `.qg-group-count` | Item count text |
| `.qg-grid-wrapper` | Wrapper for toolbar support |
| `.qg-group-toolbar` | Toolbar container |
| `.qg-group-controls` | Button container |
| `.qg-expand-all` | Expand All button |
| `.qg-collapse-all` | Collapse All button |

All styles MUST live in `wwwroot/css/qgComposable-refined-minimalism.css`.

---

## 6. Default header template

Create `DefaultGroupHeader.razor` under `Components/`.

This is used by the header-host column feature when no `HeaderTemplate` is provided.

```razor
@typeparam TGridItem where TGridItem : class
@typeparam TValue

<div class="qg-group-header @(context.IsExpanded ? "expanded" : "collapsed")"
     style="padding-left: @(context.Level * 16)px"
     @onclick="context.ToggleAsync"
     tabindex="0"
     role="button"
     aria-expanded="@context.IsExpanded">
    <span class="qg-group-chevron">@(context.IsExpanded ? "▼" : "▶")</span>
    <span class="qg-group-key">@(context.Key?.ToString() ?? context.NullGroupLabel)</span>
    <span class="qg-group-count">(@context.Count @(context.Count == 1 ? "item" : "items"))</span>
</div>

@code {
    [Parameter] public GroupHeaderContext<TGridItem, TValue> context { get; set; } = default!;
}
```

---

## 7. Consumer usage patterns (Blazor)

### 7.1 Basic usage

```razor
<ComposableGrid Items="@items.AsQueryable()">
    <ComposableColumn Property="x => x.Category">
        <GroupingFeature TGridItem="Product" TValue="string"
                         IsActive="true"
                         InitiallyExpanded="false" />
    </ComposableColumn>
    <ComposableColumn Property="x => x.Name" />
    <ComposableColumn Property="x => x.Price" />
</ComposableGrid>
```

### 7.2 Demo page

The demo requirements are fully specified in the Post-phase Demo section (Section 13).

---

## 8. Test strategy

Tests MUST be unit tests in `QuickGridTest01.Tests` for non-UI types only:

- `GroupStateManager<TValue>`
  - Initial state based on `InitiallyExpanded`
  - Toggle, Expand, Collapse single groups
  - ExpandAll, CollapseAll
  - Thread safety with concurrent access

- `GroupingCoordinator<TGridItem>`
  - Column registration
  - First-wins for multiple `IsActive = true`
  - `SetActiveGrouping` behavior
  - Virtual item count calculation
  - Grouped items transformation (marker/spacer injection)

- `GroupHeaderRowId`
  - Encode/decode marker and spacer ids
  - FIRST marker detection

No component test framework (bUnit) is introduced as part of this feature.

---

## 9. Execution sequence (implementation order)

### Phase 1: Core infrastructure

1. Add `FeaturePriority.Grouping = 50` to `FeaturePriority.cs`
2. Create `IGridDataTransformer<TGridItem>` interface in `Core/`

### Phase 2: Grouping types

3. Create enums: `GroupSortDirection`, `FilterGroupOrder`, `NullKeyBehavior`
4. Create `GroupHeaderContext<TGridItem, TValue>` record
5. Create `GroupHeaderRowId` helper (SpacerRowFactory-like)
6. Create `IGroupingFeature<TGridItem>` interface

### Phase 3: State management

7. Create `GroupStateManager<TValue>`

### Phase 4: Coordinator

8. Create `GroupingCoordinator<TGridItem>`
9. Create `GroupedGridDataSource<TGridItem>`

### Phase 5: Feature implementation

10. Implement `GroupingFeature<TGridItem, TValue>`
11. Create `DefaultGroupHeader.razor` component
12. Create `GroupHeaderHostFeature<TGridItem>` (ICellRenderFeature) to render header UI on marker rows

### Phase 6: Styling

13. Add CSS to `wwwroot/css/qgComposable-refined-minimalism.css`

### Phase 7: Disposal

14. Ensure `GroupingFeature<TGridItem, TValue>.Dispose()` releases all owned state (dispose state manager, clear references)
15. Ensure `GroupingCoordinator<TGridItem>.Dispose()` clears registrations and active grouping state

### 5.6 Complete CSS Implementation

Add the following CSS to `wwwroot/css/qgComposable-refined-minimalism.css`:

```css
/* ============================================================================
   ROW GROUPING FEATURE
   Group header rows and expand/collapse controls
   ============================================================================ */

.qg-grid-wrapper {
    display: flex;
    flex-direction: column;
}

/* ----------------------------------------------------------------------------
   Group Header Row
   Height: GroupHeaderSlotSpan × QuickGrid ItemSize for virtualization alignment
   -------------------------------------------------------------------------- */

.qg-group-header {
    /* Layout: overlay-like full-width header emitted from header-host column */
    display: flex;
    align-items: center;
    gap: var(--space-8, 8px);

    /* Dimensions - GroupHeaderSlotSpan × QuickGrid ItemSize
       Note: QuickGrid ItemSize and GroupHeaderSlotSpan are component parameters.
       Ensure the wrapper sets matching CSS variables:
         --qg-item-size
         --qg-group-header-slot-span
    */
    height: calc(var(--qg-item-size, 40px) * var(--qg-group-header-slot-span, 2));
    min-height: calc(var(--qg-item-size, 40px) * var(--qg-group-header-slot-span, 2));
    padding: var(--space-12, 12px) var(--space-16, 16px);

    /* Appearance */
    background: var(--color-canvas, #f8f9fa);
    border-bottom: 1px solid var(--color-border-default, #dee2e6);

    /* Typography */
    font-size: var(--font-size-sm, 0.875rem);
    color: var(--color-text-primary, #1a1a1a);

    /* Interaction */
    cursor: pointer;
    user-select: none;
    transition: background var(--duration-fast, 150ms) ease;
}

.qg-group-header:hover {
    background: var(--color-surface-hover, #e9ecef);
}

.qg-group-header:focus {
    outline: none;
    box-shadow: inset 0 0 0 2px var(--color-accent-primary, #2563eb);
}

.qg-group-header:focus-visible {
    outline: 2px solid var(--color-accent-primary, #2563eb);
    outline-offset: -2px;
}

.qg-group-header.expanded {
    background: var(--color-surface, #ffffff);
    border-bottom-color: var(--color-border-emphasis, #adb5bd);
}

.qg-group-header.collapsed {
    background: var(--color-canvas, #f8f9fa);
}

/* ----------------------------------------------------------------------------
   Chevron (expand/collapse indicator)
   -------------------------------------------------------------------------- */

.qg-group-chevron {
    flex-shrink: 0;
    width: 20px;
    text-align: center;
    font-size: var(--font-size-sm, 14px);
    color: var(--color-text-tertiary, #6c757d);
    transition: color var(--duration-fast, 150ms) ease;
}

.qg-group-header:hover .qg-group-chevron {
    color: var(--color-text-secondary, #495057);
}

.qg-group-header.expanded .qg-group-chevron {
    color: var(--color-text-primary, #1a1a1a);
}

/* ----------------------------------------------------------------------------
   Group Key and Count
   -------------------------------------------------------------------------- */

.qg-group-key {
    font-weight: var(--font-weight-semibold, 600);
    color: var(--color-text-primary, #1a1a1a);
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.qg-group-count {
    color: var(--color-text-tertiary, #6c757d);
    font-size: var(--font-size-xs, 0.75rem);
    white-space: nowrap;
}

/* ----------------------------------------------------------------------------
   Group Toolbar (Expand All / Collapse All buttons)
   Positioned above grid, right-aligned for visibility regardless of scroll
   -------------------------------------------------------------------------- */

.qg-group-toolbar {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    padding: var(--space-8, 8px) var(--space-12, 12px);
    background: var(--color-canvas, #f8f9fa);
    border: 1px solid var(--color-border-default, #dee2e6);
    border-bottom: none;
    border-radius: var(--card-radius, 4px) var(--card-radius, 4px) 0 0;
}

.qg-group-controls {
    display: flex;
    gap: var(--space-4, 4px);
}

.qg-expand-all,
.qg-collapse-all {
    display: inline-flex;
    align-items: center;
    gap: var(--space-4, 4px);
    padding: var(--space-4, 4px) var(--space-8, 8px);
    font-size: var(--font-size-xs, 0.75rem);
    font-weight: var(--font-weight-medium, 500);
    color: var(--color-text-secondary, #525252);
    background: var(--color-surface, #ffffff);
    border: 1px solid var(--color-border-default, #dee2e6);
    border-radius: var(--card-radius, 4px);
    cursor: pointer;
    transition: all var(--duration-fast, 150ms) ease;
}

.qg-expand-all:hover,
.qg-collapse-all:hover {
    background: var(--color-surface-hover, #e9ecef);
    border-color: var(--color-border-hover, #adb5bd);
    color: var(--color-text-primary, #1a1a1a);
}

.qg-expand-all:focus,
.qg-collapse-all:focus {
    outline: none;
    box-shadow: 0 0 0 2px var(--color-accent-primary-subtle, #eff6ff),
                0 0 0 4px var(--color-accent-primary, #2563eb);
}

.qg-expand-all:active,
.qg-collapse-all:active {
    transform: scale(0.98);
}

/* ----------------------------------------------------------------------------
   Responsive adjustments
   -------------------------------------------------------------------------- */

@media (max-width: 768px) {
    .qg-group-header {
        padding: var(--space-8, 8px) var(--space-12, 12px);
        gap: var(--space-4, 4px);
    }

    .qg-group-toolbar {
        padding: var(--space-4, 4px) var(--space-8, 8px);
    }

    .qg-expand-all,
    .qg-collapse-all {
        padding: var(--space-2, 2px) var(--space-4, 4px);
        font-size: var(--font-size-xs, 0.7rem);
    }
}
```

**Design System Variables Used:**

| Variable | Default | Purpose |
|----------|---------|---------|
| `--space-4` | 4px | Small gap |
| `--space-8` | 8px | Medium gap |
| `--space-12` | 12px | Standard padding |
| `--space-16` | 16px | Large padding |
| `--font-size-xs` | 0.75rem | Count text |
| `--font-size-sm` | 0.875rem | Default text |
| `--font-weight-medium` | 500 | Button text |
| `--font-weight-semibold` | 600 | Key text |
| `--color-surface` | #ffffff | Expanded background |
| `--color-canvas` | #f8f9fa | Collapsed background |
| `--color-surface-hover` | #e9ecef | Hover state |
| `--color-text-primary` | #1a1a1a | Key text |
| `--color-text-secondary` | #525252 | Button text |
| `--color-text-tertiary` | #6c757d | Count text, chevron |
| `--color-border-default` | #dee2e6 | Borders |
| `--color-border-hover` | #adb5bd | Hover borders |
| `--color-accent-primary` | #2563eb | Focus ring |
| `--color-accent-primary-subtle` | #eff6ff | Focus ring outer |
| `--card-radius` | 4px | Border radius |
| `--duration-fast` | 150ms | Transitions |

### Post-phase: Demo

15. Create `Pages/ComposableGroupingDemo.razor`

The demo page must follow the same layout conventions as `Pages/ComposableRowExpandDemo.razor`:

- Wrap content in `.qg-container`
- Page header:
  - `.qg-page-header`, `.qg-page-title`, `.qg-page-subtitle`
- Main demo section:
  - `.qg-section` with `.qg-section-header`, `.qg-section-title`, `.qg-section-description`
- Grid container:
  - `.qg-grid-container`
- Controls area:
  - `.demo-controls` (reuse same control layout)

#### 13.1 Route and imports

- Route: `@page "/composable-grouping-demo"`
- Use QuickGrid
- Use ComposableColumns components
- Use `QuickGridTest01.ComposableColumns.Features.Grouping`

#### 13.2 Demo model and sample data

Define a page-local model and seed at least 25 rows:

- `Product` model:
  - `int Id`
  - `string Name`
  - `string Category`
  - `decimal Price`
  - `string Status` (used to demonstrate grouping toggle between keys)

Seed requirements:

- Categories: 5 distinct values
- At least 5 products per category
- Ensure category names are visually distinct (e.g., Electronics, Clothing, Books, Home, Sports)

#### 13.3 Demo layout: two grids + two event logs

The demo page must render **two separate grids** so both supported UI modes are demonstrated side-by-side without toggles:

1. **Grid A — Default grouping UI (automatic rendering)**
   - Uses grouping with:
     - `HeaderTemplate = null`
     - `ToolbarTemplate = null`
     - `ShowExpandCollapseAllButtons = true`
   - This grid demonstrates:
     - Default group header rendering
     - Default grouping toolbar rendering
     - Expand/collapse interactions

2. **Grid B — Custom grouping UI (template overrides)**
   - Uses grouping with:
     - `HeaderTemplate` provided
     - `ToolbarTemplate` provided
     - `ShowExpandCollapseAllButtons = true`
   - This grid demonstrates:
     - Custom header template rendering using `GroupHeaderContext<TGridItem, TValue>`
     - Custom toolbar template rendering using `GroupToolbarContext`

Each grid must be separated into its own `.qg-section` with:

- A section header showing the grid title and a short description
- A dedicated **Event Log** panel below the grid (same styling patterns as `ComposableRowExpandDemo.razor`)

#### 13.4 Controls

Controls must be minimal and must not be used to switch between default/custom modes (those are shown via Grid A vs Grid B).

Required controls (applies to both grids):

1. `InitiallyExpanded` toggle (checkbox)
   - Label: "Initially expanded"
   - Changing this setting must re-seed/reset the demo state so the initial expand state can be observed.

2. Grouping key selector (dropdown)
   - Label: "Group by"
   - Values:
     - "Category"
     - "Status"
   - This must switch which grouping column is active.
   - Implementation note: because grouping activation is "first wins" during feature attachment, the demo should re-render the grids with new feature instances (or otherwise force re-attachment) when switching between "Category" and "Status" so the coordinator's active grouping is recomputed.

#### 13.5 Grid configuration (applies to both grids)

Use a consistent column set so differences are attributable to templates:

- Columns:
  - Id
  - Name
  - Category
  - Status
  - Price

Each grid must use the same underlying data set (same products) and must use independent feature instances/state so the event logs do not mix.

#### 13.6 Event logs

Provide **two** separate event logs (one per grid) with:

- A "Clear" button
- Display of the last 10 entries (most recent first)

Events to record:

- Group toggled (include key string)
- Expand All invoked
- Collapse All invoked

### Post-phase: Testing

16. Add unit tests for `GroupStateManager<TValue>`
17. Add unit tests for `GroupingCoordinator<TGridItem>`
18. Add unit tests for `GroupHeaderRowId`

### Post-phase: Validation

19. Build solution
20. Run all tests
21. Manual verification of demo page

---

## 10. Acceptance criteria

The feature is complete when:

1. **Compilation**
   - `GroupingFeature<TGridItem, TValue>` compiles and is usable from a `ComposableColumn`

2. **Grouping behavior**
   - Items are grouped by the `GroupBy` expression (or column's `Property` if null)
   - Only one column can have active grouping (first wins)
   - Groups can be expanded/collapsed by clicking the header

3. **Virtualization**
   - Group headers count as `GroupHeaderSlotSpan` virtual rows (height = `GroupHeaderSlotSpan × ItemSize`)
   - Collapsed groups show only header (`GroupHeaderSlotSpan` rows)
   - Expanded groups show header + items (`GroupHeaderSlotSpan + itemCount` rows)
   - Expand/collapse triggers grid refresh

4. **Group ordering**
   - `Ascending`, `Descending`, `FirstOccurrence` work correctly
   - Custom `GroupOrderComparer` overrides `GroupOrder`

5. **Null key handling**
   - All `NullKeyBehavior` options work correctly
   - Null group position follows `GroupOrder` when `SeparateGroup`

6. **UI controls**
   - Expand All / Collapse All buttons appear when `ShowExpandCollapseAllButtons = true`
   - Buttons are above grid, right-aligned

7. **Default template**
   - Default header shows chevron, key, and count
   - Expand/collapse on click
   - ARIA attributes for accessibility

8. **Custom template**
   - `HeaderTemplate` receives correct `GroupHeaderContext<TGridItem, TValue>`

9. **Styling**
   - All CSS in `wwwroot/css/qgComposable-refined-minimalism.css`
   - No `*.razor.css` files

10. **Namespace compliance**
    - All types under `QuickGridTest01.ComposableColumns.Features.Grouping.*`

11. **Demo**
    - `Pages/ComposableGroupingDemo.razor` demonstrates grouping functionality

12. **Tests**
    - Unit tests pass for `GroupStateManager`, `GroupingCoordinator`, `GroupHeaderRowId`

---

## 11. File structure

```
QuickGridTest01/ComposableColumns/
├── Core/
│   ├── FeaturePriority.cs              (add Grouping = 50)
│   └── IGridDataTransformer.cs         (new interface)
└── Features/
    └── Grouping/
        ├── GroupingFeature.cs           (main feature)
        ├── GroupingCoordinator.cs       (coordinator)
        ├── GroupStateManager.cs         (state management)
        ├── IGroupingFeature.cs          (interface)
        ├── GroupHeaderContext.cs        (context record)
        ├── GroupHeaderRowId.cs          (marker/spacer id encoding helper)
        ├── GroupedGridDataSource.cs     (data source wrapper, IQueryable<TGridItem> + marker/spacer injection)
        ├── Enums/
        │   ├── GroupSortDirection.cs
        │   ├── FilterGroupOrder.cs
        │   └── NullKeyBehavior.cs
        └── Components/
            ├── DefaultGroupHeader.razor  (default template)
            └── GroupHeaderHostFeature.cs (header-host column cell feature)

wwwroot/css/
└── qgComposable-refined-minimalism.css  (add grouping styles)

Pages/
└── ComposableGroupingDemo.razor         (demo page)

QuickGridTest01.Tests/
└── Features/
    └── Grouping/
        ├── GroupStateManagerTests.cs
        ├── GroupingCoordinatorTests.cs
        └── GroupHeaderRowIdTests.cs
```

---

## 12. References

- **Specification:** `Docs/Feature Design/RowGroupingFeature.md`
- **Design Decisions:** `Docs/Feature Design/RowGroupingFeature_DesignDecisions.md`
- **Pattern Reference:** `Docs/Feature Design/ImplementationPlans/Plan_ExpandableRowFeature.md`
- **Existing Spacer Pattern:** `ExpandableGridDataSource<T>` in codebase
