# RowGroupingFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 1.0 |
| Status | Design |
| Created | 2025 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Grouping` |
| Source | Discussion: `Docs/Discussion/discussion-MudBlazorFeaturesImplementation.md` |
| Styling | **All CSS for this feature must be placed in the global stylesheet `wwwroot/css/qgComposable-refined-minimalism.css` (no `*.razor.css` for feature styling).** |
| Namespace rule | **All logic pertaining to an `IColumnFeature` must live under the `QuickGridTest01.ComposableColumns` namespace (and its sub-namespaces).** |
| Encoding | UTF-8 (code page 65001) |

---

## 1. Overview

### 1.1 Purpose

`GroupingFeature<TGridItem, TValue>` provides **row grouping** within the ComposableColumns architecture.

It provides:
- Grouping of grid items by a key expression
- Collapsible/expandable group headers with customizable templates
- Full virtualization support (required rule)
- Configurable sorting, filtering, and null key handling
- Optional Expand All / Collapse All UI controls

### 1.2 Role in Architecture

This feature enables:
- **Data organization** - Group items by category, status, date, or any property
- **Visual hierarchy** - Collapsible sections reduce cognitive load
- **MudBlazor parity** - Enables migration from `MudDataGrid` grouping

```
ComposableColumn Architecture
    └── GroupingFeature<TGridItem, TValue>    (Groups items by key, renders headers)
            ├── GroupingCoordinator           (Grid-level coordination)
            └── GroupStateManager<TValue>     (Expand/collapse state)
```

### 1.3 Migration Context

This feature provides parity with MudBlazor's `MudDataGrid` grouping:

| MudBlazor | GroupingFeature |
|-----------|-----------------|
| `Groupable="true"` | Feature attached to column |
| `Grouping="true"` | `IsActive = true` |
| `GroupBy` expression | `GroupBy` parameter |
| `GroupTemplate` | `HeaderTemplate` |
| `GroupExpanded` | `InitiallyExpanded` |

---

## 2. Architecture

### 2.1 Interface & Priority

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
- `TValue` - No constraint. Works with reference types, value types, and nullable types. Null handling controlled by `NullKeyBehavior` parameter.

**Priority rationale:** Grouping transforms the data shape before other features process it. Group header rows must exist before Core, Filtering, Formatting, Styling, and Editing features run.

> `FeaturePriority.Grouping = 50` must be added before `Core (100)`.

### 2.2 Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Activation pattern | **Column-first** | Feature activates when attached to a `ComposableColumn`; registers with `GroupingCoordinator` |
| Priority | **50** (before Core) | Grouping transforms data shape before other features process |
| Rendering approach | **Expansion-style marker/spacer rows + first-column cell feature** | Matches the proven `ComposableRowExpandDemo` mechanics; avoids requiring grid row hooks |
| Virtualization | **Required** | All ComposableColumn features must support virtualization (rule) |
| Grouping levels | **Single-level only** | API designed for extensibility; nested grouping deferred |
| State management | **GroupStateManager<TValue>** | Reuses `RowStateManager` pattern for expand/collapse |

### 2.3 Column-First Coordinator Pattern

```
┌─────────────────────────────────────────────────────────────────┐
│ ComposableGrid<TGridItem>                                       │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ GroupingCoordinator<TGridItem> (owned by the grid)       │   │
│  │  - Tracks columns with GroupingFeature                   │   │
│  │  - Manages active grouping column                        │   │
│  │  - Delegates to active feature for state management      │   │
│  │  - Transforms Items → grouped items + header rows        │   │
│  │  - Provides virtualization-compatible output             │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ComposableCol │  │ComposableCol │  │ComposableCol │         │
│  │              │  │              │  │              │         │
│  │ GroupingFeat │  │ (no grouping)│  │ GroupingFeat │         │
│  │ GroupBy=Cat  │  │              │  │ GroupBy=Type │         │
│  │ IsActive=true│  │              │  │ IsActive=fals│         │
│  │ (owns state) │  │              │  │              │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

**Activation Flow:**

1. `GroupingFeature<T,V>.OnAttach()` requires access to the cascaded `Grid` reference (`ComposableGrid<TGridItem>`).
2. The feature obtains the grid-scoped `GroupingCoordinator<TGridItem>` from the grid (mirrors the existing filter registration pattern).
3. **All columns** that include `GroupingFeature` register themselves with the coordinator using a column ID (from GroupBy property name).
4. The coordinator therefore represents the complete set of *groupable columns* (used by the header UI to offer "Group by" options).
5. The **first column** that registers a `GroupingFeature` becomes the "Group Header Column" (header-host). It is responsible only for rendering the group header UI. Subsequent grouping-enabled columns do not render the header UI.
6. If `IsActive = true`, the column requests activation. If multiple columns have `IsActive = true`, the first one wins (deterministic based on column order in markup).
7. Coordinator (or feature-owned data source) wraps the grid's `Items` via an expansion-style data source that emits **only `TGridItem`** instances.
8. The transformed sequence injects **group header marker rows** and **group header spacer rows** into `IQueryable<TGridItem>`.
9. The header-host column uses an `ICellRenderFeature<TGridItem>` to:
   - Render the group header UI only for the **FIRST** header marker row
   - Render blank content for header spacer rows
   - Render blank content for normal data rows (or optionally a small indent/glyph)

**Deterministic grouping activation (no indecision):**

`ComposableGrid<TGridItem>` maintains an internal boolean that represents whether grouping is active. Grouping is active when the grid-scoped coordinator has selected an active grouping column:

- `_hasGroupingFeatures`: at least one column registered grouping with the coordinator
- `_isGroupingActive`: `_hasGroupingFeatures` is true and `GroupingCoordinator.ActiveGrouping` is not null

When `_isGroupingActive` is true, the grid binds `QuickGrid.Items` to grouped items derived from `FilteredItems`.

### 2.4 Feature Responsibilities vs Grid Responsibilities

`GroupingFeature` is a **column feature** that coordinates grid-level behavior:

| Responsibility | Owner |
|----------------|-------|
| GroupBy expression | `GroupingFeature` |
| Header template | `GroupingFeature` |
| Expand/collapse state | `GroupStateManager<TValue>` (owned by feature) |
| Data transformation | `GroupingCoordinator` (or feature-owned grouped data source) |
| Sorting while grouping active | `ComposableGrid` pipeline + grouping transform (intra-group only) |
| Row interception | **None required** (QuickGrid renders rows; features render cells) |
| CSS styling | Global stylesheet |

### 2.4.1 Sorting semantics while grouping is active

Grouping introduces a deterministic data pipeline stage (`FilteredItems → GroupedItems`). Because group headers are represented as marker/spacer rows, **global sorting over the flattened sequence is not permitted**.

**Rule:** When grouping is active, column sorting applies **within each group only**.

- Group ordering is controlled exclusively by `GroupOrder` / `GroupOrderComparer` (or `FirstOccurrence`).
- Sorting does not re-order groups.

### 2.4.2 Deterministic data pipeline (Filter → Sort → Group)

`ComposableGrid<TGridItem>` defines a deterministic pipeline. Stages may be identity transforms when the corresponding features are not present/active.

- `Items`: original grid input.
- `FilteredItems`: derived from `Items` when filter features exist/are active; otherwise `FilteredItems = Items`.
- `SortedItems` (**Option A**): derived from `FilteredItems` when a ComposableColumns sort is active; otherwise `SortedItems = FilteredItems`.
- `GroupedItems`: derived from `SortedItems` when grouping is active by injecting group header marker + spacer rows.

When grouping is active, `QuickGrid.Items` binds to `GroupedItems`.

#### Pipeline invariants (stage aliasing)

The following invariants remove conditional/indecisive language. Each stage has a single defined input and may be an identity transform when its corresponding feature set is absent/inactive.

- `FilteredItems` always consumes `Items`. If no filters are present/active, `FilteredItems = Items`.
- `SortedItems` always consumes `FilteredItems`. If no ComposableColumns sort is active, `SortedItems = FilteredItems`.
- When grouping is inactive, `ItemsForQuickGrid = SortedItems`.
- When grouping is active, `ItemsForQuickGrid = GroupedItems(SortedItems)`.

### 2.5 Integration Model (No Grid Row Hooks)

Grouping must integrate using the same model proven by `ComposableRowExpandDemo.razor`:

1. **QuickGrid renders rows** from an `IQueryable<TGridItem>`.
2. Grouping transforms the `Items` sequence by injecting marker/spacer rows that are still `TGridItem` instances.
3. A dedicated first `ComposableColumn` renders the group header UI using an `ICellRenderFeature<TGridItem>`.

No changes are required to `ComposableGrid` to render custom row types. Virtualization remains compatible because header height is represented as additional fixed-height rows (spacer-row injection).

#### 2.5.1 Marker + Spacer Row Identity

Grouping requires a stable identity scheme similar to `SpacerRowFactory`:

- A **group header marker row** represents the **FIRST** row of the header block (this is the only row that renders the header UI).
- **Group header spacer rows** represent additional height for `GroupHeaderSlotSpan - 1` rows.

The feature must be able to detect:

- whether a row is a group header marker row
- whether a row is a group header spacer row

This detection must be deterministic under virtualization.

**Design rule (modelled after Expansion):** The identity scheme must support distinguishing three row kinds at render time:

1. Normal data rows
2. Group header marker rows (**FIRST** header row of a header block)
3. Group header spacer rows (additional height rows)

Because ComposableColumns uses per-cell rendering and QuickGrid renders rows directly from `TGridItem`, the feature must be able to perform this detection from the `TGridItem` instance without requiring grid-level row interception.

##### 2.5.1.1 Required item contract

Grouping requires `TGridItem` to implement the same row identity contract used by the Expansion feature:

```csharp
public interface IRowIdentifiable
{
    int Id { get; set; }
}
```

##### 2.5.1.2 Group header row id encoding

Grouping introduces a dedicated id encoding scheme for group headers (separate from expansion overlay spacer ids):

- **Normal data row:** `Id > 0`
- **Any synthetic grouping row:** `Id < 0`
- **Group header marker row (FIRST):** `Id == EncodeGroupHeaderId(groupId)`
- **Group header spacer row:** `Id == EncodeGroupHeaderSpacerId(groupId, offset)` where `offset >= 1`

`groupId` is an internal, stable integer identifier for the group within a given grouped data source instance.

##### 2.5.1.3 Required helper API

The grouping feature/data source must expose helper methods analogous to `SpacerRowFactory` so `ICellRenderFeature<TGridItem>` can implement deterministic rendering:

```csharp
public static class GroupHeaderRowId
{
    public static int EncodeGroupHeaderId(int groupId);
    public static int EncodeGroupHeaderSpacerId(int groupId, int offset);

    public static bool IsGroupingSynthetic(int id);
    public static bool IsGroupHeaderMarker(int id);
    public static bool IsGroupHeaderSpacer(int id);

    public static int GetGroupId(int syntheticId);
    public static int GetSpacerOffset(int syntheticId);
}
```

**FIRST row rule:** The header-host column must render header content only when `IsGroupHeaderMarker(item.Id)` is true. It must render blank output for `IsGroupHeaderSpacer(item.Id)`.

##### 2.5.1.4 Stability requirement

The `groupId` mapping must be stable for the duration of the grouped data source instance so that:

- row identity is stable across refreshes caused by expand/collapse
- spacer rows remain associated with the correct header marker row

If the active grouping column changes ("Group by" selection), a new grouping transformation may be produced and ids may be reallocated.

##### 2.5.1.5 Preferred id allocation strategy (Expansion-aligned)

To keep behavior deterministic across expand/collapse refreshes, the grouped data source should maintain a **cached key→groupId mapping**:

- Store a dictionary keyed by the untyped group key (`object?`) using the active feature's key comparer.
- Assign a new integer id only when a key is first observed.
- Reuse prior ids for keys already in the dictionary.

This mirrors Expansion's approach of keeping state in the data source instance (not recomputing identity per render).

#### 2.5.2 Full-width Header UI

The header UI is emitted from the first column's cell feature (similar to how `RowExpandFeature` emits an overlay). The global stylesheet may position the header container so it visually spans the grid.

### 2.6 Feature Lifecycle

The grouping feature follows the ComposableColumns lifecycle. Understanding this timeline is critical for correct implementation.

#### 2.6.1 Initialization Phase

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 1: COMPONENT INITIALIZATION                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 1. ComposableGrid.OnInitialized()                                           │
│    └─ Creates EditEventStream                                               │
│    └─ Grid does NOT have Items yet                                          │
│                                                                             │
│ 2. ComposableGrid renders ChildContent                                      │
│    └─ CascadingValue propagates grid reference to columns                   │
│                                                                             │
│ 3. ComposableColumn.OnParametersSetAsync() [for each column]                │
│    └─ Creates FeatureContext<TGridItem, TValue>                             │
│    └─ Compiles Property expression → Context.GetValue                       │
│    └─ Calls Initialize() → feature.OnAttach(Context) for each feature       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.2 GroupingFeature Attachment

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 2: FEATURE ATTACHMENT (inside OnAttach)                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 4. GroupingFeature.OnAttach(context) called                                 │
│    │                                                                        │
│    ├─ Validate context (InvokeAsync, RequestRefreshAsync required)          │
│    │                                                                        │
│    ├─ Resolve effective GroupBy:                                            │
│    │  └─ Use explicit GroupBy parameter if provided                         │
│    │  └─ Otherwise fall back to context.GetValue (from column Property)     │
│    │  └─ Throw if neither available                                         │
│    │                                                                        │
│    ├─ Get or create GroupingCoordinator<TGridItem> (grid-scoped):           │
│    │  └─ Use cascaded Grid reference (ComposableGrid<TGridItem>)             │
│    │  └─ Mirrors existing filter registration pattern                        │
│    │                                                                        │
│    ├─ Register this column with coordinator:                                │
│    │  └─ coordinator.RegisterColumn(columnId, this)                         │
│    │  └─ Throws if columnId already registered                              │
│    │                                                                        │
│    └─ If IsActive && coordinator.ActiveGrouping is null (first wins):       │
│       └─ coordinator.SetActiveGrouping(columnId)                            │
│       └─ Create GroupStateManager<TValue>(KeyComparer)                      │
│       └─ NOTE: State NOT initialized yet (no Items available)               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.3 Rendering Phase

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 3: GRID RENDERING                                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 5. ComposableGrid.FilteredItems property accessed                           │
│    └─ Returns Items with any active filters applied                         │
│                                                                             │
│ 6. Owning component binds QuickGrid.Items to grouped data source:           │
│    └─ dataSource.Items (IQueryable<TGridItem>)                               │
│                                                                             │
│ 7. QuickGrid iterates Items                                                  │
│    └─ Grouped data source lazily rebuilds flattened sequence when dirty      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.4 Data Transformation (First Render)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 4: DATA TRANSFORMATION (inside TransformItems)                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 9. Grouped data source rebuild invoked (dirty -> rebuild)                   │
│    │                                                                        │
│    ├─ If ActiveGrouping is null:                                            │
│    │  └─ Emit original items only (no marker/spacer injection)              │
│    │                                                                        │
│    ├─ Group items by ActiveGrouping.GroupByUntyped:                         │
│    │  └─ items.AsEnumerable().GroupBy(item => GroupByUntyped(item))         │
│    │                                                                        │
│    ├─ FIRST RENDER ONLY - Initialize state:                                 │
│    │  └─ Extract all group keys from groups                                 │
│    │  └─ Feature.InitializeState(allKeys, InitiallyExpanded)                │
│    │  └─ Sets _isInitialized = true                                         │
│    │                                                                        │
│    ├─ Order groups per GroupOrder and NullKeyBehavior                       │
│    │                                                                        │
│    └─ For each group:                                                       │
│       ├─ Emit 1x group header marker row (TGridItem, Id encoded)            │
│       ├─ Emit (GroupHeaderSlotSpan - 1)x header spacer rows (TGridItem)     │
│       └─ If isExpanded: emit original group items                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.5 Row Rendering

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 5: ROW RENDERING (grid's render loop)                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 10. QuickGrid renders each TGridItem row                                     │
│     │                                                                       │
│     ├─ Header-host column cell feature:                                      │
│     │  ├─ If IsGroupHeaderMarker(item.Id): render header UI                  │
│     │  ├─ If IsGroupHeaderSpacer(item.Id): render blank                      │
│     │  └─ Else: render blank (or optional indent)                            │
│     │                                                                       │
│     └─ All other columns: render blank for marker/spacer rows                │
│        and normal content for data rows                                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.6 User Interaction (Expand/Collapse)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 6: USER INTERACTION                                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 11. User clicks group header (expand/collapse)                              │
│     │                                                                       │
│     ├─ GroupHeaderContext.ToggleAsync() invoked                             │
│     │  └─ Calls feature.ToggleGroupAsync(key)                               │
│     │  └─ GroupStateManager.ToggleAsync(typedKey)                           │
│     │  └─ Updates _expandedGroups HashSet                                   │
│     │                                                                       │
│     ├─ Grouped data source marks dirty and raises OnDataChanged             │
│     │  └─ ComposableGrid invokes InvokeAsync(StateHasChanged)               │
│     │                                                                       │
│     └─ Grid re-renders:                                                     │
│        └─ TransformItems called again                                       │
│        └─ IsGroupExpanded returns new state                                 │
│        └─ Collapsed groups: only header emitted                             │
│        └─ Expanded groups: header + data rows emitted                       │
│                                                                             │
│ 12. User clicks Expand All / Collapse All (if ShowExpandCollapseAllButtons) │
│     └─ Similar flow, but ExpandAllGroupsAsync/CollapseAllGroupsAsync called │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.7 Cleanup

```
┌─────────────────────────────────────────────────────────────────────────────┐
│ PHASE 7: DISPOSAL                                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│ 13. Component disposed (page navigation, etc.)                              │
│     │                                                                       │
│     ├─ ComposableColumn.Dispose()                                           │
│     │  └─ Calls feature.Dispose() for each feature                          │
│     │                                                                       │
│     ├─ GroupingFeature.Dispose()                                            │
│     │  └─ Disposes GroupStateManager (releases SemaphoreSlim)               │
│     │  └─ Clears references                                                 │
│     │                                                                       │
│     └─ GroupingCoordinator.Dispose() (via FeatureContext cleanup)           │
│        └─ Clears _groupableColumns dictionary                               │
│        └─ Sets ActiveGrouping = null                                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

#### 2.6.8 Key Timing Constraints

| Constraint | Reason |
|------------|--------|
| State initialization is lazy (Phase 4) | Group keys not available until Items flow through TransformItems |
| Coordinator created by first GroupingFeature | Ensures single coordinator per grid |
| First `IsActive = true` wins | Deterministic based on column order in markup |
| RequestRefreshAsync triggers full re-render | Required for virtualization recalculation |

---

## 3. Parameters

### 3.1 Core Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `IsActive` | `bool` | `true` | Whether this column's grouping is currently active. Only one column can have active grouping; if multiple specify `IsActive = true`, the first column wins. |
| `GroupBy` | `Func<TGridItem, TValue>?` | `null` | Expression to extract group key. If null, uses column's `Property` via `FeatureContext.GetValue`. Throws `InvalidOperationException` if neither is available. |
| `InitiallyExpanded` | `bool` | `true` | Whether groups start expanded or collapsed. |
| `GroupHeaderSlotSpan` | `int` | `2` | Number of fixed-height virtual slots the group header occupies under QuickGrid `ItemSize` virtualization. Must be `>= 1`. |

### 3.2 Templates

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `HeaderTemplate` | `RenderFragment<GroupHeaderContext<TGridItem, TValue>>?` | `null` | Custom template for group headers. If null, uses full-featured default. |
| `ToolbarTemplate` | `RenderFragment<GroupToolbarContext>?` | `null` | Custom template for grouping toolbar controls (Expand All / Collapse All). If null, the grid renders the default grouping toolbar UI when enabled. |

### 3.3 Sorting & Ordering

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `GroupOrder` | `GroupSortDirection` | `Ascending` | How groups are ordered (Ascending, Descending, FirstOccurrence). |
| `GroupOrderComparer` | `IComparer<TValue>?` | `null` | Custom comparer for group ordering. Overrides `GroupOrder`. |

```csharp
public enum GroupSortDirection
{
    /// <summary>Groups ordered by key ascending (A-Z, 0-9).</summary>
    Ascending,
    /// <summary>Groups ordered by key descending (Z-A, 9-0).</summary>
    Descending,
    /// <summary>Groups ordered by first occurrence in source data.</summary>
    FirstOccurrence
}
```

### 3.4 Filtering Interaction

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `FilterBehavior` | `FilterGroupOrder` | `FilterThenGroup` | Order of filter/group operations. Note: `ComposableGrid` applies filtering before providing `Items` to QuickGrid, so `GroupThenFilter` is not supported in the current integration model. |
| `HideEmptyGroups` | `bool` | `true` | Whether to hide groups with 0 items after filtering. |

```csharp
public enum FilterGroupOrder
{
    /// <summary>
    /// Filter items first, then group the filtered results.
    /// Most intuitive for users searching within data.
    /// </summary>
    FilterThenGroup,
    
    /// <summary>
    /// Group all items first, then apply filter within each group.
    /// Useful when group structure must remain visible.
    /// </summary>
    GroupThenFilter
}

**Implementation constraint:** In the current ComposableColumns architecture, grouping receives the sequence that is bound to the grid. When using `ComposableGrid`, that sequence is already filtered (`FilteredItems`). Therefore, `GroupThenFilter` is reserved for future work that would require an explicit, coordinated data pipeline and is not implemented by this feature.
```

### 3.5 Null Key Handling

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `NullKeyHandling` | `NullKeyBehavior` | `SeparateGroup` | How to handle items where `GroupBy` returns null. |
| `NullGroupLabel` | `string` | `"(No Value)"` | Display text for the null group. |

```csharp
public enum NullKeyBehavior
{
    /// <summary>Create a separate group for items with null keys.</summary>
    SeparateGroup,
    /// <summary>Show items with null keys ungrouped at the top (as regular data rows, no group header).</summary>
    ShowAtTop,
    /// <summary>Show items with null keys ungrouped at the bottom (as regular data rows, no group header).</summary>
    ShowAtBottom,
    /// <summary>Exclude items with null keys from display.</summary>
    Exclude
}
```

**Null Group Sort Position:** When `NullKeyBehavior = SeparateGroup`, the null group position follows `GroupOrder`:
- `Ascending`: Null group appears **first** (null < all values)
- `Descending`: Null group appears **last** (null < all values, reversed)
- `FirstOccurrence`: Null group appears where first null item occurs in source data

### 3.6 Key Comparison

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `KeyComparer` | `IEqualityComparer<TValue>?` | `null` | Custom equality comparer for group keys. If null, uses `EqualityComparer<TValue>.Default`. |

### 3.7 UI Controls

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ShowExpandCollapseAllButtons` | `bool` | `false` | Whether to show Expand All / Collapse All buttons in grid header. |

---

## 4. Context Objects

### 4.1 GroupHeaderContext

Passed to `HeaderTemplate`:

```csharp
public record GroupHeaderContext<TGridItem, TValue>(
    /// <summary>The group key value.</summary>
    TValue? Key,

    /// <summary>All items in this group.</summary>
    IReadOnlyList<TGridItem> Items,

    /// <summary>Count of items in this group.</summary>
    int Count,

    /// <summary>Whether this group is currently expanded.</summary>
    bool IsExpanded,

    /// <summary>Async delegate to toggle expand/collapse.</summary>
    Func<Task> ToggleAsync,

    /// <summary>Nesting level (0 for top-level, reserved for future nested grouping).</summary>
    int Level,

    /// <summary>Label to display for null keys.</summary>
    string NullGroupLabel
) where TGridItem : class;

### 4.2 GroupToolbarContext

Passed to `ToolbarTemplate` when grouping toolbar controls are enabled:

```csharp
public record GroupToolbarContext(
    /// <summary>Async delegate to expand all groups.</summary>
    Func<Task> ExpandAllAsync,

    /// <summary>Async delegate to collapse all groups.</summary>
    Func<Task> CollapseAllAsync,

    /// <summary>Whether any group is expanded.</summary>
    bool HasExpandedGroups,

    /// <summary>Count of expanded groups.</summary>
    int ExpandedGroupCount
);
```
```

### 4.3 GroupedRow

Discriminated union representing either a header or data row:

```csharp
public abstract record GroupedRow<TGridItem>
    where TGridItem : class;

public record GroupHeaderRow<TGridItem>(
    object? Key,           // Stored as object for coordinator compatibility (supports any TValue)
    IReadOnlyList<TGridItem> Items,
    int Count,
    bool IsExpanded,
    int Level
) : GroupedRow<TGridItem>
    where TGridItem : class;

public record DataRow<TGridItem>(
    TGridItem Item
) : GroupedRow<TGridItem>
    where TGridItem : class;

/// <summary>
/// Extension methods for GroupHeaderRow to bridge object Key to typed TValue.
/// </summary>
public static class GroupHeaderRowExtensions
{
    /// <summary>
    /// Gets the key cast to the specified type.
    /// </summary>
    public static TValue? GetTypedKey<TGridItem, TValue>(this GroupHeaderRow<TGridItem> row)
    {
        return row.Key is TValue typed ? typed : default;
    }
}
```

---

## 5. Supporting Types

### 5.1 GroupingCoordinator

Internal coordinator registered in `FeatureContext`:

```csharp
internal class GroupingCoordinator<TGridItem> : IDisposable
    where TGridItem : class
{
    private readonly Dictionary<string, IGroupingFeature<TGridItem>> _groupableColumns = new();

    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    // Note: No StateManager property - feature owns typed GroupStateManager<TValue> internally

    /// <summary>
    /// Register a column's grouping capability.
    /// Throws InvalidOperationException if columnId is already registered.
    /// </summary>
    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature);

    /// <summary>
    /// Set which column's grouping is active.
    /// Pass null to disable grouping.
    /// Throws InvalidOperationException if columnId is not null and not registered.
    /// </summary>
    public void SetActiveGrouping(string? columnId);

    /// <summary>Get total count including group headers (for virtualization).</summary>
    public int GetVirtualItemCount(IQueryable<TGridItem> items);

    /// <summary>Get items for visible range, including group headers.</summary>
    public IEnumerable<GroupedRow<TGridItem>> GetVirtualizedItems(
        IQueryable<TGridItem> items,
        int startIndex,
        int count);

    /// <summary>Transform items into grouped sequence with headers.</summary>
    public IEnumerable<GroupedRow<TGridItem>> TransformItems(IQueryable<TGridItem> items);
}
```

### 5.2 GroupStateManager

Manages expand/collapse state for groups. Uses a `HashSet<TValue>` internally with thread-safe access via `SemaphoreSlim`. Accepts an optional `IEqualityComparer<TValue>` for custom key comparison.

```csharp
public class GroupStateManager<TValue> : IDisposable
{
    public GroupStateManager(IEqualityComparer<TValue>? comparer = null);

    /// <summary>Whether any group is expanded.</summary>
    public bool HasExpandedGroups { get; }

    /// <summary>Count of expanded groups.</summary>
    public int ExpandedGroupCount { get; }

    /// <summary>Check if a specific group is expanded.</summary>
    public bool IsExpanded(TValue key);

    /// <summary>Toggle a group's expand/collapse state.</summary>
    public Task ToggleAsync(TValue key, CancellationToken ct = default);

    /// <summary>Expand a specific group.</summary>
    public Task ExpandAsync(TValue key, CancellationToken ct = default);

    /// <summary>Collapse a specific group.</summary>
    public Task CollapseAsync(TValue key, CancellationToken ct = default);

    /// <summary>Expand all groups.</summary>
    public Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken ct = default);

    /// <summary>Collapse all groups.</summary>
    public Task CollapseAllAsync(CancellationToken ct = default);

    /// <summary>Initialize state based on InitiallyExpanded setting.</summary>
    public Task InitializeAsync(IEnumerable<TValue> allKeys, bool initiallyExpanded, CancellationToken ct = default);

    public void Dispose();
}
```

**Initialization Timing:** `InitializeAsync` is called during the first `TransformItems` call when group keys become available. The feature tracks whether initialization has occurred via an internal `_isInitialized` flag. On first call, all discovered keys are initialized based on the `InitiallyExpanded` setting. This lazy initialization is required because group keys are not available at `OnAttach` time—they are derived from `Items` which flow through the grid's rendering pipeline after feature attachment.

### 5.3 IGroupingFeature Interface

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
    IEqualityComparer<object>? KeyComparerUntyped { get; }
    bool ShowExpandCollapseAllButtons { get; }

    // Optional UI template override for grid-level grouping controls
    RenderFragment<GroupToolbarContext>? ToolbarTemplate { get; }

    // State management methods (delegated from coordinator)
    /// <summary>Toggle a group's expand/collapse state.</summary>
    Task ToggleGroupAsync(object key);

    /// <summary>Check if a group is expanded.</summary>
    bool IsGroupExpanded(object key);

    /// <summary>Expand all groups.</summary>
    Task ExpandAllGroupsAsync();

    /// <summary>Collapse all groups.</summary>
    Task CollapseAllGroupsAsync();

    /// <summary>
    /// Render the group header. Called by grid for each GroupHeaderRow.
    /// The feature internally casts the object key back to TValue and renders
    /// either the custom HeaderTemplate (if provided) or the default template.
    /// This approach avoids RenderFragment covariance issues with generic type parameters.
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

### 5.4 IGridDataTransformer Interface

New interface for features that transform the grid's data source (placed in `Core/`):

```csharp
/// <summary>
/// Interface for features that transform the grid's data source.
/// Implemented by features that change the shape of data (e.g., grouping, aggregation).
/// </summary>
public interface IGridDataTransformer<TGridItem> : IColumnFeature<TGridItem>
    where TGridItem : class
{
    /// <summary>
    /// Whether this transformer is currently active.
    /// </summary>
    bool IsTransformActive { get; }

    /// <summary>
    /// Gets the coordinator key for registration in FeatureContext.
    /// </summary>
    string CoordinatorKey { get; }
}
```

### 5.5 Column Identifier

Each groupable column is registered with the coordinator using a unique identifier. The identifier is derived from the `GroupBy` expression by extracting the property name (e.g., "Category", "Status"). If a property name cannot be extracted (e.g., for complex expressions), a sequential fallback ID is generated (e.g., "GroupingColumn_1", "GroupingColumn_2").

---

### 5.6 GroupedGridDataSource

Data source wrapper that the grid iterates when grouping is active:

```csharp
public class GroupedGridDataSource<TGridItem> 
    where TGridItem : class
{
    private readonly IQueryable<TGridItem> _source;
    private readonly GroupingCoordinator<TGridItem> _coordinator;

    public GroupedGridDataSource(
        IQueryable<TGridItem> source, 
        GroupingCoordinator<TGridItem> coordinator)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
    }

    /// <summary>
    /// Returns grouped items including headers. Grid iterates this.
    /// </summary>
    public IEnumerable<GroupedRow<TGridItem>> Items => _coordinator.TransformItems(_source);

    /// <summary>
    /// Total count for virtualization (headers count as 2 rows each).
    /// </summary>
    public int VirtualItemCount => _coordinator.GetVirtualItemCount(_source);

    /// <summary>
    /// Toggle a group's expand/collapse state and notify listeners.
    /// This method is async to avoid fire-and-forget calls and to ensure
    /// state changes complete before virtualization is refreshed.
    /// </summary>
    public async Task ToggleGroupAsync(object key)
    {
        if (_coordinator.ActiveGrouping is null)
        {
            return;
        }

        await _coordinator.ActiveGrouping.ToggleGroupAsync(key);
        OnDataChanged?.Invoke();
    }

    /// <summary>
    /// Raised when group state changes (expand/collapse).
    /// </summary>
    public event Action? OnDataChanged;
}
```

**Lifecycle:** Created by the grid when it detects a `GroupingCoordinator<TGridItem>` with an active grouping. The grid binds to `Items` for row iteration and `VirtualItemCount` for virtualization.

---

## 6. Default Header Template

When `HeaderTemplate` is null, the following default is rendered:

```razor
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
```

**Features:**
- Expand/collapse chevron (▶/▼)
- Group key value (or `NullGroupLabel` if null)
- Item count with singular/plural
- Indentation for future nested grouping
- Click handler on entire row
- Focus indicator (tabindex)
- ARIA attributes for accessibility
- Hover and expanded/collapsed CSS classes

---

## 7. CSS Classes

All CSS in `wwwroot/css/qgComposable-refined-minimalism.css`:


| Class | Purpose |
|-------|---------|
| `.qg-group-header` | Group header row container |
| `.qg-group-header.expanded` | Expanded state |
| `.qg-group-header.collapsed` | Collapsed state |
| `.qg-group-header:hover` | Hover state |
| `.qg-group-header:focus` | Focus indicator |
| `.qg-group-chevron` | Expand/collapse icon |
| `.qg-group-key` | Group key text |
| `.qg-group-count` | Item count text |
| `.qg-group-controls` | Expand All / Collapse All button container |
| `.qg-expand-all` | Expand All button |
| `.qg-collapse-all` | Collapse All button |
| `.qg-grid-wrapper` | Wrapper for toolbar support |
| `.qg-group-toolbar` | Toolbar container |

### 7.1 Styling Requirements

The complete CSS implementation is in the Implementation Plan. The styles must:

- Use existing CSS variables from the design system (`--space-*`, `--color-*`, `--font-*`, etc.)
- Support hover, focus, and active states for accessibility
- Include responsive adjustments for mobile (max-width: 768px)
- Follow the design token patterns established in `qgComposable-refined-minimalism.css`

**Key dimensional requirements:**
- Group header height: `GroupHeaderSlotSpan × ItemSize` (default: 2× standard row height for virtualization alignment)
- Column spanning: `grid-column: 1 / -1`
- Level indentation: 16px per level (`padding-left: level * 16px`)

---

## 8. Virtualization Support

Since virtualization is **required**, the implementation must:

1. **Group headers count toward virtual item count**
   - Each group header counts as `GroupHeaderSlotSpan` virtual slots

2. **Collapsed groups skip their items**
   - When collapsed, group items are excluded from virtualized output

3. **Expand/collapse triggers recalculation**
   - Changing group state triggers virtualization recalculation via `RequestRefreshAsync()`

4. **Coordinator provides virtualization-compatible output**

```csharp
// GroupingCoordinator<TGridItem> methods for virtualization:

/// <summary>
/// Returns the total count including group headers (for virtualization).
/// Collapsed groups contribute GroupHeaderSlotSpan (header only).
/// Expanded groups contribute GroupHeaderSlotSpan + itemCount (header + items).
/// </summary>
public int GetVirtualItemCount(IQueryable<TGridItem> items);

/// <summary>
/// Returns items for the visible range, including group headers.
/// </summary>
public IEnumerable<GroupedRow<TGridItem>> GetVirtualizedItems(
    IQueryable<TGridItem> items,
    int startIndex,
    int count);
```

---

## 9. Sorting Behavior

When grouping is active:

| Sorting Target | Behavior |
|----------------|----------|
| **Non-grouped column** | Items within each group are sorted; group order unchanged |
| **Grouped column** | Group order changes based on sort direction |

Groups are always ordered by key (controlled by `GroupOrder` parameter).

---

## 10. Usage Example

### Basic Usage

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

### Custom Header Template

```razor
<ComposableGrid Items="@items.AsQueryable()">
    <ComposableColumn Property="x => x.Category">
        <GroupingFeature TGridItem="Product" TValue="string"
                         IsActive="true"
                         ShowExpandCollapseAllButtons="true">
            <HeaderTemplate>
                <div class="custom-group-header" @onclick="context.ToggleAsync">
                    <span class="icon">@(context.IsExpanded ? "📂" : "📁")</span>
                    <strong>@context.Key</strong>
                    <span class="badge">@context.Count</span>
                </div>
            </HeaderTemplate>
        </GroupingFeature>
    </ComposableColumn>
    <ComposableColumn Property="x => x.Name" />
</ComposableGrid>
```

### With Filtering

```razor
<ComposableGrid Items="@items.AsQueryable()">
    <ComposableColumn Property="x => x.Category">
        <GroupingFeature TGridItem="Product" TValue="string"
                         FilterBehavior="FilterGroupOrder.FilterThenGroup"
                         HideEmptyGroups="true" />
        <FilterFeature TGridItem="Product" TValue="string" />
    </ComposableColumn>
    <ComposableColumn Property="x => x.Name" />
</ComposableGrid>
```

---

## 11. File Structure

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
        ├── GroupedRow.cs                (discriminated union + extensions)
        ├── GroupedGridDataSource.cs     (data source wrapper)
        ├── Enums/
        │   ├── GroupSortDirection.cs
        │   ├── FilterGroupOrder.cs
        │   └── NullKeyBehavior.cs
        └── Components/
            └── DefaultGroupHeader.razor  (default template)
```

---



## 12. Backlog Items


| Item | Priority | Notes |
|------|----------|-------|
| Multiple grouping levels (nested) | Medium | API has `Level` property for future support |
| Full keyboard accessibility | Medium | Basic accessibility included; full navigation deferred |
| Drag-to-reorder groups | Low | Out of scope |
| Persist group state (LocalStorage) | Low | Out of scope |

---

## 13. References

- **Design Decisions:** `RowGroupingFeature_DesignDecisions.md` - Documents Q13-Q31 analysis and rationale
- Discussion: `Docs/Discussion/discussion-MudBlazorFeaturesImplementation.md`
- Pattern reference: `ExpandableRowFeature.md`
- MudBlazor docs: [MudDataGrid Grouping](https://mudblazor.com/components/datagrid)
- Migration analysis: `MudDataGrid to QuickGrid conversion.md`
