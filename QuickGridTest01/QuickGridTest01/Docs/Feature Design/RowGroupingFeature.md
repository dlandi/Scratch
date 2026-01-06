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

##### 2.5.1.3.1 Encoding contract (normative)

Grouping must use a deterministic, reversible integer encoding for synthetic row ids that:

- always produces **negative** ids for synthetic grouping rows
- supports extracting `groupId` and (for spacer rows) `offset`
- supports unambiguous detection of marker vs spacer rows
- avoids collisions within the supported range

**Bit layout** (31-bit payload + negative sign):

- Synthetic ids are negative. The payload is stored in the low 31 bits of the absolute value.
- Payload format (from most-significant to least-significant bits):

  - bits 30..24 (7 bits): `kind`
    - `0x01` = marker row
    - `0x02` = spacer row
  - bits 23..8 (16 bits): `groupId`
    - valid range: `1..65535`
  - bits 7..0 (8 bits): `offset`
    - marker rows use `0`
    - spacer rows use `1..255` and must satisfy `offset <= GroupHeaderSlotSpan - 1`

**Encoding rules:**

- Marker id: `id = -((kind << 24) | (groupId << 8) | 0)` with `kind = 0x01`
- Spacer id: `id = -((kind << 24) | (groupId << 8) | offset)` with `kind = 0x02`

**Validation / error behavior:**

- For invalid inputs to encoding methods (out of range), the helper must throw `ArgumentOutOfRangeException`.
- For decode methods (`GetGroupId`, `GetSpacerOffset`) called on non-synthetic ids, the helper must throw `ArgumentException`.
- For detection methods (`IsGroupingSynthetic`, `IsGroupHeaderMarker`, `IsGroupHeaderSpacer`) invalid ids must return `false`.

**Capacity note:** With this encoding, the maximum supported group count per grouped data source instance is 65,535 groups.

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
4. The coordinator therefore represents the complete set of *groupable columns* (used by the grid toolbar UI to offer "Group by" options).
5. The **first column** that registers a `GroupingFeature` becomes the "Group Header Column" (header-host). It is responsible only for rendering the group header UI. Subsequent grouping-enabled columns do not render the header UI.
6. If `IsActive = true`, the column requests activation. If multiple columns have `IsActive = true`, the first one wins (deterministic based on column order in markup).
7. The grid wraps the grid's `Items` via a grid-owned `GroupedGridDataSource<TGridItem>` that emits **only `TGridItem`** instances.
8. The transformed sequence injects **group header marker rows** and **group header spacer rows** into `IQueryable<TGridItem>`.
9. The header-host column uses an `ICellRenderFeature<TGridItem>` to:
   - Render the group header UI only for the **FIRST** header marker row
   - Render blank content for header spacer rows
   - Render blank content for normal data rows

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
| Data transformation | `GroupingCoordinator` + grid-owned `GroupedGridDataSource<TGridItem>` |
| Sorting while grouping active | `ComposableGrid` pipeline + grouping transform (intra-group only) |
| Row interception | **None required** (QuickGrid renders rows; features render cells) |
| CSS styling | Global stylesheet |

### 2.4.1 Sorting semantics while grouping is active

Grouping introduces a deterministic data pipeline stage (`FilteredItems → GroupedItems`). Because group headers are represented as marker/spacer rows, **global sorting over the flattened sequence is not permitted**.

**Rule:** When grouping is active, column sorting applies **within each group only**.

- Group ordering is controlled exclusively by `GroupOrder` / `GroupOrderComparer` (or `FirstOccurrence`).
- Sorting does not re-order groups.

#### 2.4.1.1 Concrete sorting suppression mechanism (normative)

This codebase currently supports **two** sorting concepts:

1. **QuickGrid column sorting** via `ColumnBase.SortBy` (driven today by `ComposableColumn.Sortable` setting `SortBy`).
2. **ComposableColumns sorting** via `ISortingFeature<TGridItem>` (returns a sort function over `IQueryable<TGridItem>`).

When grouping is active, **QuickGrid column sorting must be disabled**, otherwise QuickGrid may reorder the flattened sequence (including marker/spacer rows), violating the grouping identity contract.

**Normative suppression rule:**

- When grouping is active (grid-scoped coordinator has `ActiveGrouping != null`), `ComposableColumn.SortBy` must be `null` for all columns (or otherwise not supplied to QuickGrid).
- Sorting state is owned by `ComposableGrid<TGridItem>` and materialized as `SortedItems` using the active `ISortingFeature<TGridItem>` state.
- Grouping consumes `SortedItems` as the per-group item ordering.

**UI implication (deterministic):** while grouping is active, the QuickGrid sort UI (click-to-sort headers) is disabled.

### 2.4.2 Deterministic data pipeline (Filter → Sort → Group)

`ComposableGrid<TGridItem>` defines a deterministic pipeline. Stages may be identity transforms when the corresponding features are not present/active.

- `Items`: original grid input.
- `FilteredItems`: derived from `Items` when filter features exist/are active; otherwise `FilteredItems = Items`.
- `SortedItems`: derived from `FilteredItems` when a ComposableColumns sort is active; otherwise `SortedItems = FilteredItems`.
- `GroupedItems`: derived from `SortedItems` when grouping is active by injecting group header marker + spacer rows.

When grouping is active, `QuickGrid.Items` binds to `GroupedItems`.

#### Pipeline invariants (stage aliasing)

The following invariants remove conditional/indecisive language. Each stage has a single defined input and may be an identity transform when its corresponding feature set is absent/inactive.

- `FilteredItems` always consumes `Items`. If no filters are present/active, `FilteredItems = Items`.
- `SortedItems` always consumes `FilteredItems`. If no ComposableColumns sort is active, `SortedItems = FilteredItems`.
- When grouping is inactive, `ItemsForQuickGrid = SortedItems`.
- When grouping is active, `ItemsForQuickGrid = GroupedItems(SortedItems)`.

#### 2.4.2.1 Stage ownership (normative)

To ensure deterministic behavior and to prevent tasks from inventing pipeline glue, the pipeline stages are owned by the following components:

- `Items` (input): provided to `ComposableGrid<TGridItem>` by the consumer.
- `FilteredItems`: owned by `ComposableGrid<TGridItem>` (existing filtering integration).
- `SortedItems`: owned by `ComposableGrid<TGridItem>` (ComposableColumns sort stage; independent of QuickGrid global sort).
- `GroupedItems`: owned by the grouping integration (`GroupedGridDataSource<TGridItem>` + `GroupingCoordinator<TGridItem>`), consuming `SortedItems` as input.

#### 2.4.2.2 Sorting authority when grouping is active (normative)

When grouping is active, **QuickGrid must not apply a global sort over the flattened `ItemsForQuickGrid` sequence**, because that could reorder marker/spacer rows relative to data rows and break the grouping contract.

Therefore:

1. `ComposableGrid<TGridItem>` owns the effective sort stage (`SortedItems`).
2. When grouping is active, the grid must ensure QuickGrid does not apply a global sort by not providing QuickGrid sort state that would reorder the flattened sequence.
3. Any active sort is applied as **intra-group sorting** during grouping transformation:
   - The grouping transform orders groups per `GroupOrder` / `GroupOrderComparer`.
   - Within each group, item ordering uses the current ComposableColumns sort state (the same logic that produced `SortedItems` when grouping is inactive).

**Implication for implementation tasks:** tasks must modify `ComposableGrid<TGridItem>` to make the sort state a grid-owned pipeline stage and to prevent QuickGrid from applying global sorting when grouping is active.

### 2.5 Integration Model (No Grid Row Hooks)

Grouping must integrate using the same model proven by `ComposableRowExpandDemo.razor`:

1. **QuickGrid renders rows** from an `IQueryable<TGridItem>`.
2. Grouping transforms the `Items` sequence by injecting marker/spacer rows that are still `TGridItem` instances.
3. A dedicated first `ComposableColumn` renders the group header UI using an `ICellRenderFeature<TGridItem>`.

No custom row renderer hooks are required (or permitted). QuickGrid still renders rows directly from `TGridItem`.

#### 2.5.0 Minimal grid markup participation (CSS variables only)

The grid may add a minimal wrapper/attributes in `ComposableGrid` markup **solely** to provide CSS variables used for sizing/alignment.

**Normative rule:** This is limited to **styling support** (e.g., setting `--qg-item-size` from QuickGrid `ItemSize`, and `--qg-group-header-slot-span` from the active grouping feature). It must not introduce any new grid-level row interception or alternative row rendering paths.

This enables default styles to compute header/overlay sizing consistently with virtualization.

#### 2.5.0.1 Coordinator storage + access (Filtering pattern, normative)

To remain consistent with existing feature integration (notably filtering), the grouping coordinator is **grid-owned** and accessed via the cascaded `ComposableGrid<TGridItem>` instance.

- `ComposableGrid<TGridItem>` must own a private field: `_groupingCoordinator`
- `ComposableGrid<TGridItem>` must expose an `internal` API: `GetOrCreateGroupingCoordinator()` returning `GroupingCoordinator<TGridItem>`
- `GroupingFeature<TGridItem, TValue>.OnAttach(...)` must call `grid.GetOrCreateGroupingCoordinator()` and register itself

This avoids attempting to use `FeatureContext.RegisterService(...)`, which is column-scoped.

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

**Enforcement (Expansion pattern, normative):** If grouping is active and `TGridItem` does not implement `IRowIdentifiable`, `GroupingFeature<TGridItem, TValue>.OnAttach(...)` must throw `InvalidOperationException`.
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

The header UI is emitted from the **header-host column's** `ICellRenderFeature<TGridItem>` using the **same overlay positioning model as `RowExpandFeature`** (rendered from within the first column's cell surface, but visually spanning the grid).

##### 2.5.2.1 Rendering responsibility split (normative)

Because the coordinator is not generic over `TValue`, the header-host column feature must not attempt to invoke typed templates directly.

Responsibilities are split as follows:

- **Header-host column cell feature:**
  - Detects marker/spacer vs data rows via `GroupHeaderRowId`.
  - Owns overlay placement (RowExpandFeature-style).
  - Delegates actual UI rendering to the active grouping feature.

- **Active `GroupingFeature<TGridItem, TValue>` (via `IGroupingFeature<TGridItem>`):**
  - Owns template selection and rendering.
  - If `HeaderTemplate` is provided, renders it.
  - Otherwise renders the default header UI.

##### 2.5.2.2 Grid toolbar: Grouping section location + frequency (normative)

Grid toolbar: Grouping section controls (Group By selector, Expand All / Collapse All, and any future controls):

- Render **once per grid** (not once per group).
- Are rendered in the grid's toolbar region (Grouping section), **closest to the grid**.
  - When filter UI is present, the grouping toolbar appears **below** the filtering UI.
  - The grouping toolbar does **not** scroll with virtualized grid content.

If the consumer provides `ToolbarTemplate`, the active grouping feature renders it; otherwise the feature renders the default grouping toolbar UI.

**Implementation note (normative):** Because the grid toolbar: Grouping section is rendered in the grid toolbar (not in a marker-row overlay), it must not depend on the presence of any particular group header row in the current virtualization viewport.

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
│    └─ Calls Grid.RegisterColumn(this)                                       │
│       └─ (Optional early path) Grid detects grouping features and registers │
│          them with the grid-owned GroupingCoordinator (idempotent)          │
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
│    ├─ Validate context (InvokeAsync required)                               │
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
│    │  └─ Idempotent: ignores duplicate registrations for the same columnId  │
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
│    │  └─ Feature initializes expand/collapse state via `GroupStateManager.InitializeAsync(allKeys, InitiallyExpanded)`
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
│     │  └─ Else: render blank                                                  │
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
| Grouped data source `OnDataChanged` triggers full re-render | Single refresh authority for grouping state changes (avoids double-refresh loops) |

## 2.6.9 Addendum — Early + Idempotent Grouping Registration (normative)

### Motivation

In practice, `ComposableGrid<TGridItem>` may evaluate its `ItemsForQuickGrid` binding during a render pass **before** all columns have completed feature attachment.
This can cause grouping to be observed as inactive during the first render if registration happens only inside `GroupingFeature.OnAttach`.

To keep grouping deterministic while avoiding render/refresh loops, grouping registration is defined as a **dual-path** operation:

- an **early grid-level registration** path (preferred)
- a **feature attach-time registration** path (required for completeness)

### 2.6.9.1 Dual-path registration (required)

Grouping columns may be registered with the grid-owned `GroupingCoordinator<TGridItem>` from either (or both) of the following points:

1. **Grid-level (early) registration**
   - During column registration (`ComposableGrid.RegisterColumn(...)`) the grid may detect grouping features attached to that column and register them immediately.
   - This is the preferred path because it can establish `HeaderHostColumnId` and (when `IsActive=true`) `ActiveGrouping` before `ItemsForQuickGrid` is evaluated.

2. **Feature attach-time registration**
   - `GroupingFeature<TGridItem, TValue>.OnAttach(...)` must also register its grouping capability with the coordinator (filter-registration pattern), because this is the canonical feature lifecycle hook.

**Compatibility rule:** Implementations must be correct if **both** paths call registration for the same column.

### 2.6.9.2 Idempotent coordinator registration (required)

`GroupingCoordinator<TGridItem>.RegisterColumn(columnId, feature)` must be **idempotent**.

- If `columnId` is registered for the first time, the coordinator records the feature.
- If `columnId` is registered again later, the coordinator must **ignore** the subsequent registration (no-op).
- The coordinator must not throw solely due to duplicate registration attempts for the same `columnId`.

**First-wins rules (unchanged):**

- `HeaderHostColumnId` is pinned to the first registered grouping column.
- `ActiveGrouping` is pinned to the first registered grouping column with `IsActive = true`.

### 2.6.9.3 Refresh rule when grouping becomes active (required)

If grouping becomes active only after a render pass has already bound `QuickGrid.Items` (i.e., `ItemsForQuickGrid` was evaluated while inactive), the grid may perform **at most one** deterministic refresh to rebind `ItemsForQuickGrid`.

- This must not become a render loop.
- Group expand/collapse refresh authority remains `GroupedGridDataSource.OnDataChanged -> grid InvokeAsync(StateHasChanged)`.

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
| `ToolbarTemplate` | `RenderFragment<GroupToolbarContext>?` | `null` | Custom template for grouping controls rendered in the grid toolbar (e.g., Group By selector, Expand All / Collapse All). If null, the grid renders the default grouping toolbar UI when enabled. |

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

**Runtime behavior (normative):** If `FilterBehavior = FilterGroupOrder.GroupThenFilter` is configured, `GroupingFeature<TGridItem, TValue>.OnAttach(...)` must throw `NotSupportedException` with a message indicating that `GroupThenFilter` is not supported in the current `ComposableGrid` integration model.
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
| `ShowExpandCollapseAllButtons` | `bool` | `false` | Whether to show Expand All / Collapse All buttons in the grid toolbar (below filter UI when present). |

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

### 4.3 Row rendering model (no union row type)

This feature does not introduce a `GroupedRow<TGridItem>` union type.

**Normative rule:** Group headers are represented as **synthetic `TGridItem` instances** (marker + spacer rows) identified by `IRowIdentifiable.Id` using `GroupHeaderRowId`. All non-synthetic rows are normal data rows.

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
    /// Idempotent: ignores duplicate registrations for the same columnId.
    /// </summary>
    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature);

    /// <summary>
    /// Set which column's grouping is active.
    /// Pass null to disable grouping.
    /// Throws InvalidOperationException if columnId is not null and not registered.
    /// </summary>
    public void SetActiveGrouping(string? columnId);

    /// <summary>
    /// Transform items into a flattened `IQueryable<TGridItem>` sequence containing:
    /// - group header marker rows
    /// - group header spacer rows
    /// - normal data rows
    /// </summary>
    public IQueryable<TGridItem> TransformItems(IQueryable<TGridItem> items);
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
    /// Render the group header. Called by the header-host column cell feature for each group header marker row.
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
    /// Returns grouped items including marker/spacer header rows. Grid binds `QuickGrid.Items` to this.
    /// </summary>
    public IQueryable<TGridItem> Items => _coordinator.TransformItems(_source);

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

**Lifecycle:** Created by the grid when it detects a `GroupingCoordinator<TGridItem>` with an active grouping. The grid binds to `Items` for row iteration.

#### 5.6.1 Ownership + caching rules (normative)

- `GroupedGridDataSource<TGridItem>` is **grid-owned** and **grid-scoped**.
- The grid holds a single instance (field) and **reuses** it across renders while the active grouping and upstream source sequence are stable.
- The grid **recreates** the instance only when one of the following changes occurs:
  1. Grouping transitions from inactive -> active (first activation)
  2. Grouping transitions from active -> inactive (instance is disposed/cleared)
  3. The active grouping column changes (coordinator `ActiveGrouping` changes)
  4. The upstream bound source for grouping changes (the queryable instance provided to the data source changes)

#### 5.6.2 Event subscription rules (single refresh authority)

- `GroupedGridDataSource<TGridItem>.OnDataChanged` is the **only** refresh signal for grouping state changes (expand/collapse, expand all, collapse all).
- When the grid creates the data source, it must subscribe exactly once:
  - handler: `() => InvokeAsync(StateHasChanged)`
- When the grid replaces or disables the data source, it must unsubscribe (or dispose the data source that owns the event invocation list) before dropping the reference.

#### 5.6.3 Disposal rules

- When grouping becomes inactive, or when the grid is disposed, the grid must:
  1. unsubscribe from `OnDataChanged`
  2. dispose the current `GroupedGridDataSource<TGridItem>` (if it is `IDisposable`; otherwise set it to null)
  3. clear any cached references so the old instance is eligible for GC

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
   - Changing group state triggers virtualization recalculation via the grouped data source's `OnDataChanged` event.
   - The grid receives `OnDataChanged` and calls `InvokeAsync(StateHasChanged)`.
   - Grouping state changes must not also call `FeatureContext.RequestRefreshAsync()` (single refresh authority; avoids double-refresh loops).

4. **Virtualization-compatible output (marker/spacer model)**

QuickGrid virtualization operates over a flat sequence of fixed-height rows. Grouping preserves this by representing header height as additional fixed-height rows (marker + spacer rows).

- Each group header consumes `GroupHeaderSlotSpan` virtual slots:
  - 1 marker row
  - `GroupHeaderSlotSpan - 1` spacer rows
- Collapsed groups emit only their header slots.
- Expanded groups emit header slots + all group items.

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
        ├── GroupHeaderRowId.cs          (marker/spacer row id encoding + helpers)
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

- Discussion: `Docs/Discussion/discussion-MudBlazorFeaturesImplementation.md`
- Pattern reference: `ExpandableRowFeature.md`
- MudBlazor docs: [MudDataGrid Grouping](https://mudblazor.com/components/datagrid)
- Migration analysis: `MudDataGrid to QuickGrid conversion.md`
