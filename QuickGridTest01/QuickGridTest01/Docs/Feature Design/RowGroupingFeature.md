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

**Priority rationale:** Grouping transforms the data shape before other features process it. Group header rows must exist before Core, Filtering, Formatting, Styling, and Editing features run.

> `FeaturePriority.Grouping = 50` must be added before `Core (100)`.

### 2.2 Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Activation pattern | **Column-first** | Feature activates when attached to a `ComposableColumn`; registers with `GroupingCoordinator` |
| Priority | **50** (before Core) | Grouping transforms data shape before other features process |
| Rendering approach | **Grid-level row interception + CSS spanning** | All artifacts within Grid/ComposableColumns namespace |
| Virtualization | **Required** | All ComposableColumn features must support virtualization (rule) |
| Grouping levels | **Single-level only** | API designed for extensibility; nested grouping deferred |
| State management | **GroupStateManager<TValue>** | Reuses `RowStateManager` pattern for expand/collapse |

### 2.3 Column-First Coordinator Pattern

```
┌─────────────────────────────────────────────────────────────────┐
│ ComposableGrid<TGridItem>                                       │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ GroupingCoordinator<TGridItem> (in FeatureContext)       │   │
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

1. `GroupingFeature<T,V>.OnAttach()` checks if `GroupingCoordinator` exists in `FeatureContext`
2. If not, creates and registers `GroupingCoordinator<TGridItem>`
3. Registers this column's grouping capability with the coordinator
4. If `IsActive = true`, sets this as the active grouping
5. Coordinator transforms grid's `Items` to include group headers
6. `ComposableGrid` discovers coordinator via `FeatureContext` and renders accordingly

### 2.4 Feature Responsibilities vs Grid Responsibilities

`GroupingFeature` is a **column feature** that coordinates grid-level behavior:

| Responsibility | Owner |
|----------------|-------|
| GroupBy expression | `GroupingFeature` |
| Header template | `GroupingFeature` |
| Expand/collapse state | `GroupStateManager<TValue>` (owned by feature) |
| Data transformation | `GroupingCoordinator` |
| Row interception | `ComposableGrid` |
| CSS styling | Global stylesheet |

---

## 3. Parameters

### 3.1 Core Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `IsActive` | `bool` | `true` | Whether this column's grouping is currently active. Only one column can have active grouping. |
| `GroupBy` | `Func<TGridItem, TValue>?` | `null` | Expression to extract group key. If null, uses column's `Property`. |
| `InitiallyExpanded` | `bool` | `true` | Whether groups start expanded or collapsed. |

### 3.2 Templates

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `HeaderTemplate` | `RenderFragment<GroupHeaderContext<TGridItem, TValue>>?` | `null` | Custom template for group headers. If null, uses full-featured default. |

### 3.3 Sorting & Ordering

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `GroupOrder` | `GroupSortDirection` | `Ascending` | How groups are ordered (Ascending, Descending, FirstOccurrence). |
| `GroupOrderComparer` | `IComparer<TValue>?` | `null` | Custom comparer for group ordering. Overrides `GroupOrder`. |

```csharp
public enum GroupSortDirection
{
    /// <summary>Groups ordered by key ascending (A-Z, 0-9). Shows ▲ chevron.</summary>
    Ascending,
    /// <summary>Groups ordered by key descending (Z-A, 9-0). Shows ▼ chevron.</summary>
    Descending,
    /// <summary>Groups ordered by first occurrence in source data.</summary>
    FirstOccurrence
}
```

### 3.4 Filtering Interaction

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `FilterBehavior` | `FilterGroupOrder` | `FilterThenGroup` | Order of filter/group operations. |
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
    /// <summary>Show items with null keys ungrouped at the top.</summary>
    ShowAtTop,
    /// <summary>Show items with null keys ungrouped at the bottom.</summary>
    ShowAtBottom,
    /// <summary>Exclude items with null keys from display.</summary>
    Exclude
}
```

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
    TValue Key,
    
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
);
```

### 4.2 GroupedRow

Discriminated union representing either a header or data row:

```csharp
public abstract record GroupedRow<TGridItem>;

public record GroupHeaderRow<TGridItem>(
    object? Key,           // Stored as object per Q21 decision
    int Count,
    bool IsExpanded,
    int Level
) : GroupedRow<TGridItem>;

public record DataRow<TGridItem>(
    TGridItem Item
) : GroupedRow<TGridItem>;

/// <summary>
/// Extension methods for GroupHeaderRow type bridging (per Q31 decision).
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

    // Note: No StateManager property - feature owns state per Q14/Q25 decisions

    /// <summary>Register a column's grouping capability.</summary>
    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature);

    /// <summary>Set which column's grouping is active.</summary>
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

Manages expand/collapse state for groups:

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
    
    /// <summary>Whether any group is expanded.</summary>
    public bool HasExpandedGroups => _expandedGroups.Count > 0;
    
    /// <summary>Count of expanded groups.</summary>
    public int ExpandedGroupCount => _expandedGroups.Count;
    
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
}
```

### 5.3 IGroupingFeature Interface

```csharp
public interface IGroupingFeature<TGridItem>
    where TGridItem : class
{
    // Configuration properties
    bool IsActive { get; }
    Func<TGridItem, object>? GroupByUntyped { get; }
    RenderFragment<GroupHeaderContext<TGridItem, object>>? HeaderTemplateUntyped { get; }
    bool InitiallyExpanded { get; }
    GroupSortDirection GroupOrder { get; }
    IComparer<object>? GroupOrderComparerUntyped { get; }
    FilterGroupOrder FilterBehavior { get; }
    bool HideEmptyGroups { get; }
    NullKeyBehavior NullKeyHandling { get; }
    string NullGroupLabel { get; }
    IEqualityComparer<object>? KeyComparerUntyped { get; }
    bool ShowExpandCollapseAllButtons { get; }

    // State management methods (per Q23 decision)
    /// <summary>Toggle a group's expand/collapse state.</summary>
    Task ToggleGroupAsync(object key);

    /// <summary>Check if a group is expanded.</summary>
    bool IsGroupExpanded(object key);

    /// <summary>Expand all groups.</summary>
    Task ExpandAllGroupsAsync();

    /// <summary>Collapse all groups.</summary>
    Task CollapseAllGroupsAsync();
}
```

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

**Group header row spanning:**

```css
.qg-group-header {
    grid-column: 1 / -1; /* Span all columns */
}
```

---

## 8. Virtualization Support

Since virtualization is **required**, the implementation must:

1. **Group headers count toward virtual item count**
   - Each group header counts as 2 virtual rows (80px = 2 × 40px slots)

2. **Collapsed groups skip their items**
   - When collapsed, group items are excluded from virtualized output

3. **Expand/collapse triggers recalculation**
   - Changing group state triggers virtualization recalculation via `RequestRefreshAsync()`

4. **Coordinator provides virtualization-compatible output**

```csharp
// GroupingCoordinator<TGridItem> methods for virtualization:

/// <summary>
/// Returns the total count including group headers (for virtualization).
/// Collapsed groups contribute 2 (header only, 80px = 2 slots).
/// Expanded groups contribute 2 + itemCount (header + items).
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

## 12. Open Questions (Require Decision)

### Q13: IGridDataTransformer Interface

The spec mentions `IGridDataTransformer<TGridItem>` but this interface doesn't exist.

#### Existing Interfaces in ComposableColumns

| Interface | Purpose | Suitable? |
|-----------|---------|-----------|
| `IColumnFeature<TGridItem>` | Base interface with Priority, OnAttach, OnDetach | ✅ Required base |
| `ICellRenderFeature<TGridItem>` | Cell rendering pipeline | ❌ Not for data transformation |
| `IHeaderRenderFeature<TGridItem>` | Header rendering | ⚠️ Possibly for Expand/Collapse buttons |
| `IValueAccessorFeature<TGridItem, TValue>` | Get/Set values | ❌ Not relevant |
| `ISortingFeature<TGridItem>` | Sort key selector | ❌ Not for data transformation |
| `IValueChangedFeature<TGridItem, TValue>` | Value change notification | ❌ Not relevant |
| `IValidationFeature<TGridItem, TValue>` | Validation | ❌ Not relevant |

**Analysis:** None of the existing interfaces handle data transformation. A new interface is needed.

| Option | Description |
|--------|-------------|
| **Create new interface** | Define `IGridDataTransformer<T>` for features that transform data |
| **Remove from signature** | Only implement `IColumnFeature<T>` |
| **Use marker interface** | Empty interface to signal coordinator pattern usage |

> **DECISION:** ✅ **Create new interface** - `IGridDataTransformer<TGridItem>`
>
> **Rationale:** Grouping fundamentally transforms the data source. A dedicated interface makes this explicit and allows the grid to discover transformers. This pattern could be reused for future features (e.g., aggregation rows).

```csharp
/// <summary>
/// Interface for features that transform the grid's data source.
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

### Q14: Type Erasure in Coordinator

The coordinator needs to work with any `TValue` but stores `GroupStateManager<object>`.

| Option | Pros | Cons |
|--------|------|------|
| **Object boxing** | Simple, no extra types | Boxing overhead for value types, type safety lost |
| **Generic coordinator** | Full type safety, no boxing | Requires `TValue` known at registration, complex coordinator management |
| **Type-safe wrapper** | Type safety internally, clean external API | Extra indirection, slightly more complex |

**Analysis:**

- **Object boxing:** Works but loses compile-time safety. `GroupStateManager<object>` would compare keys using `object.Equals()`, which works but requires careful null handling.

- **Generic coordinator:** Would require `GroupingCoordinator<TGridItem, TValue>`, but the coordinator is stored in `FeatureContext` which doesn't know `TValue`. This creates a chicken-and-egg problem.

- **Type-safe wrapper:** The feature (`GroupingFeature<TGridItem, TValue>`) knows `TValue` and can create a typed `GroupStateManager<TValue>` internally. The coordinator only needs to delegate to the active feature.

> **DECISION:** ✅ **Type-safe wrapper**
>
> **Implementation:**
> - `GroupingFeature<TGridItem, TValue>` owns a typed `GroupStateManager<TValue>` internally
> - `GroupingCoordinator<TGridItem>` (non-generic on `TValue`) delegates to the active feature
> - Coordinator uses `IGroupingFeature<TGridItem>` interface with `object`-typed accessors for cross-feature communication
> - Feature implements both typed public API and untyped internal interface
>
> **Architecture:**

```csharp
// Feature owns typed state manager
public sealed class GroupingFeature<TGridItem, TValue> : IGroupingFeature<TGridItem>
{
    private GroupStateManager<TValue>? _stateManager;

    // Typed public API
    public GroupStateManager<TValue> StateManager => _stateManager!;

    // Untyped interface for coordinator
    Func<TGridItem, object>? IGroupingFeature<TGridItem>.GroupByUntyped 
        => item => GroupBy!(item)!;
}

// Coordinator delegates to active feature
internal class GroupingCoordinator<TGridItem>
{
    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    // Coordinator doesn't manage state directly - delegates to feature
}
```

### Q15: Null Group Position in Sort Order

When `NullKeyBehavior = SeparateGroup`, where does the null group appear?

| Option | Description |
|--------|-------------|
| **Always first** | Null group at top regardless of `GroupOrder` |
| **Always last** | Null group at bottom regardless of `GroupOrder` |
| **Follow GroupOrder** | Null treated as "less than" all values (first in Ascending, last in Descending) |
| **Configurable** | Add `NullGroupPosition` parameter |

> **DECISION:** ✅ **Follow GroupOrder**
>
> **Behavior:**
> - `GroupOrder = Ascending`: Null group appears **first** (null < all values)
> - `GroupOrder = Descending`: Null group appears **last** (null < all values, reversed)
> - `GroupOrder = FirstOccurrence`: Null group appears where first null item occurs

### Q16: Multiple IsActive=true Conflict

What happens if multiple columns have `IsActive = true`?

| Option | Description |
|--------|-------------|
| **First wins** | First registered column with `IsActive=true` becomes active |
| **Last wins** | Last registered column with `IsActive=true` becomes active |
| **Throw exception** | `InvalidOperationException` at registration |
| **Log warning** | Log warning, first wins |

> **DECISION:** ✅ **First wins**
>
> **Behavior:** The first column to register with `IsActive = true` becomes the active grouping. Subsequent columns with `IsActive = true` are registered as "groupable" but not active. No exception or warning (deterministic behavior based on column order in markup).

### Q17: Expand/Collapse All Button Location

Where do the Expand All / Collapse All buttons appear?

| Option | Description |
|--------|-------------|
| **Above grid, left** | Toolbar area above grid, left-aligned |
| **Above grid, right** | Toolbar area above grid, right-aligned |
| **In grouped column header** | Inside the header cell of the grouped column |
| **Floating** | Floating button group near first group header |

> **DECISION:** ✅ **Above grid, right**
>
> **Implementation:** When `ShowExpandCollapseAllButtons = true`, the grid renders a toolbar area above the column headers with the buttons right-aligned. This keeps them visible and accessible regardless of scroll position.

```razor
<!-- Grid structure with toolbar -->
<div class="qg-grid-wrapper">
    @if (ShowGroupingControls)
    {
        <div class="qg-group-toolbar">
            <div class="qg-group-controls">
                <button class="qg-expand-all">⊞ Expand All</button>
                <button class="qg-collapse-all">⊟ Collapse All</button>
            </div>
        </div>
    }
    <div class="qg-grid">...</div>
</div>
```

### Q18: Group Header Row Height

What height should group header rows have for virtualization?

**Grid Row Height Analysis:**

From the codebase:
- `VirtualizationScenario.ItemSize` defaults to `40f` (40px)
- `VirtualScrollingDemo.razor` documents: "Must match actual row height (40px)"
- CSS typically uses 40-42px for data rows

| Option | Description |
|--------|-------------|
| **Same as data rows** | Use grid's `RowHeight` parameter (40px) |
| **Fixed height** | Hardcoded value (e.g., 40px) |
| **Configurable** | Add `GroupHeaderHeight` parameter |
| **Auto-calculated** | Measure actual rendered height |

> **DECISION:** ✅ **Fixed: 2× row height (80px)**
>
> **Rationale:** 
> - Group headers need more visual weight than data rows
> - Using exactly 2× the row height (40px × 2 = 80px) ensures alignment with virtualization
> - Virtualization can count group headers as 2 virtual rows
> - No configuration needed; consistent visual appearance

```csharp
/// <summary>
/// Group header height in pixels. Fixed at 2× the standard row height (80px)
/// to ensure proper virtualization alignment.
/// </summary>
internal const int GroupHeaderHeight = 80; // 2 × 40px standard row height
```

### Q19: ComposableGrid Integration Point

How does `ComposableGrid` know to render group headers?

**Existing Pattern: `ExpandableGridDataSource<T>`**

From the codebase, `ExpandableGridDataSource<T>` provides:
- `Items` property returns `IQueryable<T>` including spacer rows
- Spacer rows are identified by negative IDs (`IsSpacer()` check)
- `ExpandRow(rowId, spacerCount)` injects spacers
- `CollapseRow(rowId)` removes spacers
- `OnDataChanged` event for refresh

| Option | Description |
|--------|-------------|
| **Override row rendering** | Grid checks coordinator before each row |
| **Transform Items property** | Coordinator wraps `Items` with grouped sequence |
| **Render callback** | Grid calls coordinator's render method |
| **Virtual row injection** | Similar to `ExpandableGridDataSource` spacer pattern |

> **DECISION:** ✅ **Virtual row injection** (leverage existing pattern)
>
> **Implementation:** Create `GroupedGridDataSource<TGridItem>` following the `ExpandableGridDataSource<T>` pattern:
> - Wraps original `IQueryable<TGridItem>`
> - Injects group header marker items
> - Grid renders marker items as group headers
> - Handles expand/collapse by including/excluding group items
>
> **Challenge:** Group headers are not `TGridItem` instances. Solution: Use a discriminated union wrapper or marker interface that the grid can detect.

```csharp
/// <summary>
/// Wraps grid items with group headers for virtualized rendering.
/// </summary>
public class GroupedGridDataSource<TGridItem> where TGridItem : class
{
    public IQueryable<GroupedRow<TGridItem>> Items { get; }
    public void ToggleGroup(object key);
    public event Action? OnDataChanged;
}
```

### Q20: FilterBehavior vs Priority Conflict

Priority 50 runs before Filtering (150), but `FilterThenGroup` needs filtering first.

**Analysis of Options:**

| Option | How it works | Pros | Cons |
|--------|--------------|------|------|
| **Priority doesn't control data flow** | Priority determines feature initialization order, not when data transformations apply | Clean separation of concerns | Requires clear documentation |
| **Coordinator defers grouping** | Coordinator receives already-filtered `Items` from grid; filtering happens in LINQ before grouping | Simple, leverages existing data flow | Relies on grid's `Items` being pre-filtered |
| **Change priority to 175** | Grouping runs after filtering | Matches mental model | Breaks "grouping transforms data shape" rationale |
| **Dual-phase processing** | Feature registers at 50, transforms at render time | Maximum flexibility | Complex, two execution points |

**Key Insight:** The `Items` parameter passed to `ComposableGrid` is already an `IQueryable<TGridItem>`. If filtering features add `.Where()` clauses to this queryable, the grouping coordinator simply receives filtered data. Priority only affects when `OnAttach` runs, not when the data flows.

**Trade-offs:**

1. **If Priority = 50 (before Filtering):**
   - `OnAttach` runs early, coordinator is ready
   - When `FilterThenGroup`: Grid's `Items` already filtered by the time coordinator transforms
   - When `GroupThenFilter`: Coordinator groups all items, filter hides rows within groups
   - ✅ Works for both behaviors

2. **If Priority = 175 (after Filtering):**
   - `OnAttach` runs after filter feature attached
   - Less clear separation: grouping looks like it depends on filtering
   - ❌ Confusing: priority suggests dependency that doesn't exist

> **DECISION:** ✅ **Keep Priority = 50**
>
> **Rationale:** Priority controls initialization order (`OnAttach`), not data flow. The `FilterBehavior` parameter controls the actual behavior:
> - `FilterThenGroup`: Coordinator transforms already-filtered `Items` from grid
> - `GroupThenFilter`: Coordinator groups all items, filter feature hides non-matching rows within groups
>
> **Implementation detail:** The coordinator receives `IQueryable<TGridItem>` which may already have `.Where()` clauses applied by filter features. Grouping operates on whatever data flows through.

---

### Additional Clarifications (Q21-Q26)

The following clarifications are needed to avoid ambiguity during implementation:

### Q21: GroupedRow Type Parameter

`GroupHeaderRow<TGridItem, TValue>` inherits from `GroupedRow<TGridItem>`, but `TValue` is lost in the base type.

| Option | Pros | Cons |
|--------|------|------|
| **Store key as `object`** | Simple, coordinator can work with any key type | Boxing for value types, loses type safety |
| **Add `TValue` to base** | Full type safety throughout | `GroupedRow<TGridItem, TValue>` makes collections hard to type; coordinator can't hold mixed types |
| **Use pattern matching** | Type safety where needed, flexible base | Requires casting at usage sites, slightly more complex |

**Analysis:**

- The coordinator needs to work with `GroupedRow<TGridItem>` collections without knowing `TValue`
- The feature knows `TValue` and can pattern-match when rendering
- Storing key as `object` is consistent with `IGroupingFeature<TGridItem>.GroupByUntyped`

> **DECISION:** ✅ **Store key as `object`**
>
> **Implementation:** `GroupHeaderRow` stores key as `object` for coordinator compatibility:

```csharp
public abstract record GroupedRow<TGridItem>;

public record GroupHeaderRow<TGridItem>(
    object? Key,           // Stored as object for coordinator compatibility
    int Count,
    bool IsExpanded,
    int Level
) : GroupedRow<TGridItem>;

public record DataRow<TGridItem>(
    TGridItem Item
) : GroupedRow<TGridItem>;
```
>
> **Rationale:** Consistent with `IGroupingFeature<TGridItem>.GroupByUntyped` pattern. The feature can cast back to `TValue` when creating `GroupHeaderContext<TGridItem, TValue>` for template rendering.

### Q22: Virtualization Row Count for Headers

Group headers are 80px (2× row height). For virtualization counting:

| Option | Description |
|--------|-------------|
| **Count as 2 rows** | Header contributes 2 to virtual item count (consistent with height) |
| **Count as 1 row** | Header contributes 1, but with 2× height (simpler count, complex height) |

> **DECISION:** ✅ **Count as 2 rows**
>
> **Rationale:** The virtualized grid depends on a constant row height (40px). Counting headers as 2 virtual rows maintains this invariant. The virtualizer sees a flat list where each "slot" is 40px.
>
> **Implementation:**
> - Collapsed group: contributes 2 to count (header only, 80px = 2 slots)
> - Expanded group: contributes 2 + itemCount (header + items)

### Q23: State Manager Access via Interface

The coordinator needs to toggle groups, but `IGroupingFeature` doesn't expose state management.

| Option | Description |
|--------|-------------|
| **Add methods to interface** | `Task ToggleGroupAsync(object key)`, `bool IsGroupExpanded(object key)` |
| **Coordinator calls feature directly** | Cast to typed feature when needed |
| **Expose state manager** | `IGroupStateManager StateManagerUntyped { get; }` interface property |

> **DECISION:** ✅ **Add methods to interface**
>
> **Implementation:** Add state management methods directly to `IGroupingFeature<TGridItem>`:

```csharp
public interface IGroupingFeature<TGridItem>
{
    // ... existing properties ...

    /// <summary>Toggle a group's expand/collapse state.</summary>
    Task ToggleGroupAsync(object key);

    /// <summary>Check if a group is expanded.</summary>
    bool IsGroupExpanded(object key);

    /// <summary>Expand all groups.</summary>
    Task ExpandAllGroupsAsync();

    /// <summary>Collapse all groups.</summary>
    Task CollapseAllGroupsAsync();
}
```

### Q24: GroupBy Fallback to Column Property

When `GroupBy` is null, spec says "uses column's `Property`". How does the feature access it?

| Option | Pros | Cons |
|--------|------|------|
| **Via FeatureContext** | `FeatureContext<TGridItem, TValue>` already has `GetValue` | Requires typed context, which feature has access to |
| **Required parameter** | No magic, explicit API | Less convenient for common case where grouping uses column's property |
| **Injected on attach** | Column passes property during `OnAttach` | Adds complexity to attachment protocol |

**Analysis:**

From `FeatureContext.cs`:
```csharp
public class FeatureContext<TGridItem, TValue> : FeatureContext<TGridItem>
{
    public Func<TGridItem, TValue>? GetValue { get; set; }  // Already available!
}
```

The feature receives `FeatureContext<TGridItem, TValue>` during `OnAttach`, which already contains the compiled `GetValue` accessor from the column's `Property` expression. No additional mechanism needed.

> **DECISION:** ✅ **Via FeatureContext**
>
> **Implementation:** When `GroupBy` is null, use `FeatureContext<TGridItem, TValue>.GetValue`:

```csharp
public sealed class GroupingFeature<TGridItem, TValue> : IGroupingFeature<TGridItem>
{
    [Parameter] public Func<TGridItem, TValue>? GroupBy { get; set; }

    private Func<TGridItem, TValue>? _effectiveGroupBy;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // Use GroupBy if provided, otherwise fall back to column's property
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
    }
}
```
>
> **Rationale:** Leverages existing infrastructure. No new mechanisms needed. Falls back gracefully with clear error if neither is available.

### Q25: Coordinator StateManager Property Removal

Q14 decided "Type-safe wrapper" where feature owns state. But Section 5.1 still shows:
```csharp
public GroupStateManager<object>? StateManager { get; private set; }
```

| Option | Description |
|--------|-------------|
| **Remove property** | Coordinator has no StateManager; delegates to active feature |
| **Keep as untyped wrapper** | Coordinator has `IGroupStateManager` for convenience methods |

> **DECISION:** ✅ **Remove property**
>
> **Rationale:** Per Q14's decision, the feature owns the typed `GroupStateManager<TValue>`. The coordinator delegates to the active feature via `IGroupingFeature<TGridItem>` methods (added in Q23). No need for coordinator to hold state.
>
> **Updated Section 5.1:**

```csharp
internal class GroupingCoordinator<TGridItem> : IDisposable
    where TGridItem : class
{
    private readonly Dictionary<string, IGroupingFeature<TGridItem>> _groupableColumns = new();

    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    // NO StateManager property - feature owns state, coordinator delegates

    // ... rest of methods ...
}
```

### Q26: ShowAtTop/ShowAtBottom Rendering

When `NullKeyBehavior = ShowAtTop` or `ShowAtBottom`, how are ungrouped items rendered?

| Option | Description |
|--------|-------------|
| **As regular rows** | Items appear without group header, interleaved with grid |
| **As special section** | Items appear in a "(Ungrouped)" visual section |
| **With minimal header** | A header row with just the null label, no expand/collapse |

> **DECISION:** ✅ **As regular rows**
>
> **Behavior:**
> - `ShowAtTop`: Items with null keys appear as regular data rows at the top of the grid, before any group headers
> - `ShowAtBottom`: Items with null keys appear as regular data rows at the bottom of the grid, after all groups
> - No special header or visual treatment - they're just ungrouped items
>
> **Rationale:** Simplest implementation. If users want a visual section, they can use `SeparateGroup` with a custom `NullGroupLabel`.

---

### Additional Clarifications (Q27-Q31)

Final scan revealed the following items needing clarification:

### Q27: Column Identifier for Registration

The coordinator's `RegisterColumn(string columnId, ...)` method needs a column identifier to distinguish between columns.

| Option | Description |
|--------|-------------|
| **Use data model's ID property** | Derive from `TGridItem`'s ID property (e.g., `"Id"`, `"ProductId"`) |
| **Use GroupBy property name** | Extract from `GroupBy` expression (e.g., `"Category"`) |
| **Use Title** | `FeatureContext.Title` |
| **Manufacture sequential ID** | Generate sequential integer ID for each column |

**Analysis:** The data model is expected to have an ID property (either int or GUID). If the data model has no ID property, manufacture a sequential integer ID.

> **DECISION:** ✅ **Use data model's ID property with manufactured fallback**
>
> **Implementation:** 
> - Check if `TGridItem` has an `Id`, `ID`, or similar property (int or GUID)
> - If found, use that property's name as the column identifier
> - If not found, manufacture a sequential integer ID
>
> ```csharp
> private static int _columnIdCounter;
>
> private string GetColumnId()
> {
>     // Try to find ID property on TGridItem
>     var idProperty = typeof(TGridItem).GetProperty("Id") 
>                   ?? typeof(TGridItem).GetProperty("ID")
>                   ?? typeof(TGridItem).GetProperties()
>                        .FirstOrDefault(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase));
>     
>     if (idProperty != null)
>         return idProperty.Name;
>     
>     // Fallback: manufacture sequential ID
>     return $"GroupingColumn_{Interlocked.Increment(ref _columnIdCounter)}";
> }
> ```
>
> **Rationale:** The data model's ID property provides a stable, meaningful identifier that's consistent with how the grid identifies rows.

### Q28: Section 2.3 Diagram Inconsistency

The diagram on lines 98-99 says coordinator "Holds GroupStateManager<TValue>" but Q14/Q25 decided feature owns state.

| Option | Description |
|--------|-------------|
| **Update diagram** | Remove "Holds GroupStateManager" line, add "Delegates to active feature" |

> **DECISION:** ✅ **Update diagram**
>
> Updated in Section 2.3.

### Q29: Section 8 Virtualization Comment

The comment says "Collapsed groups contribute 1" but Q22 decided headers count as **2 rows**.

| Option | Description |
|--------|-------------|
| **Update comment** | Change to "Collapsed groups contribute 2 (header only, 80px = 2 slots)" |

> **DECISION:** ✅ **Update comment**
>
> Updated in Section 8.

### Q30: IGroupingCoordinator Interface

Section 8 defines `IGroupingCoordinator<TGridItem>` interface but Section 5.1 defines `GroupingCoordinator<TGridItem>` class.

| Option | Description |
|--------|-------------|
| **Keep interface** | Class implements interface for testability |
| **Remove interface** | Internal class doesn't need interface |
| **Rename** | Clarify which is authoritative |

> **DECISION:** ✅ **Remove interface**
>
> **Rationale:** Internal class doesn't need a separate interface. The class definition in Section 5.1 is authoritative. Section 8 code block updated to use class directly.

### Q31: GroupHeaderContext Type Bridge

`GroupHeaderContext<TGridItem, TValue>` uses typed `TValue Key` but `GroupHeaderRow<TGridItem>` stores `object? Key`.

| Option | Description |
|--------|-------------|
| **Document casting** | Feature casts `object` back to `TValue` when creating context |
| **Add helper method** | `GroupHeaderRow.GetTypedKey<TValue>()` |

> **DECISION:** ✅ **Add helper method**
>
> **Implementation:** Add extension method to `GroupHeaderRow`:

```csharp
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

## 13. Backlog Items


| Item | Priority | Notes |
|------|----------|-------|
| Multiple grouping levels (nested) | Medium | API has `Level` property for future support |
| Full keyboard accessibility | Medium | Basic accessibility included; full navigation deferred |
| Drag-to-reorder groups | Low | Out of scope |
| Persist group state (LocalStorage) | Low | Out of scope |

---

## 14. References

- Discussion: `Docs/Discussion/discussion-MudBlazorFeaturesImplementation.md`
- Pattern reference: `ExpandableRowFeature.md`
- MudBlazor docs: [MudDataGrid Grouping](https://mudblazor.com/components/datagrid)
- Migration analysis: `MudDataGrid to QuickGrid conversion.md`
