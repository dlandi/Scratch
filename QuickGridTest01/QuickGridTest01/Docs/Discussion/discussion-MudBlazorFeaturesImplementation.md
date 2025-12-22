# Discussion: Remaining MudBlazor Features Implementation

## Document Information

| Attribute | Value |
|-----------|-------|
| Status | Discussion |
| Created | 2025 |
| Related | `MudDataGrid to QuickGrid conversion.md` |
| Target Framework | .NET 9 Blazor |
| Namespace | `QuickGridTest01.ComposableColumns.Features.*` |
| Encoding | UTF-8 (code page 65001) |

> **Note:** All markdown files in this project must be saved with UTF-8 encoding (code page 65001) to preserve Unicode characters.

---

## 1. Overview

This document discusses the implementation strategy for the two remaining MudBlazor DataGrid features needed to achieve full migration parity:

| Feature | Blocking Pages | MudBlazor API | Priority |
|---------|----------------|---------------|----------|
| **Row Grouping** | `Index.razor`, `Soak.razor` | `GroupBy`, `GroupTemplate`, `Groupable` | High |
| **Multi-Selection** | `Soak.razor` | `MultiSelection`, `SelectedItems`, `SelectColumn` | Medium |

### Current ComposableColumn Architecture

The existing feature pipeline provides the foundation:

```
Priority 0   → Infrastructure (property expression, compiled accessor)
Priority 100 → Core (type traits, auto-title)
Priority 150 → Filtering (FilterFeature<T,V>)
Priority 200 → Formatting
Priority 300 → Styling (TooltipFeature, CSS)
Priority 350 → Expansion (RowExpandFeature<T>)
Priority 400 → Editing (InlineEditingFeature<T,V>)
Priority ???  → Grouping (NEW)
Priority ???  → Selection (NEW)
```

---

## 2. Row Grouping Feature

### 2.1 MudBlazor Behavior Analysis

From `Index.razor` and `Soak.razor` usage patterns:

```razor
<!-- MudDataGrid grouping usage -->
<MudDataGrid Items="@items" GroupExpanded>
    <PropertyColumn Property="x => x.Category" 
                    Groupable="true" 
                    Grouping="true"
                    GroupBy="@(x => x.Category)">
        <GroupTemplate>
            <span>@context.Grouping.Key (@context.Grouping.Count() items)</span>
        </GroupTemplate>
    </PropertyColumn>
</MudDataGrid>
```

**Key MudBlazor Grouping Behaviors:**
1. `Groupable="true"` - Marks column as eligible for grouping
2. `Grouping="true"` - Activates grouping on this column
3. `GroupBy` - Expression to extract group key
4. `GroupTemplate` - Custom rendering for group header rows
5. `GroupExpanded` - Initial expanded/collapsed state
6. Multiple grouping levels supported
7. Group headers are clickable to expand/collapse

### 2.2 Design Questions & Decisions

**Q1: Grid-level vs Column-level Feature?**

| Approach | Pros | Cons |
|----------|------|------|
| **Grid-level** (`ComposableGrid` parameter) | Single grouping state, simpler coordination | Doesn't fit `IColumnFeature` pattern |
| **Column-level** (`GroupingFeature<T,V>`) | Consistent with other features | Requires cross-column coordination |
| **Hybrid** (Column marks groupable, Grid manages state) | Best of both | More complex implementation |

> **DECISION:** Column-first activation pattern.
> 
> **Rationale:** Currently, no feature is activated in the ComposableGrid unless there is an `IColumnFeature` invoked via a `ComposableColumn`. This ensures that no feature is activated unless a particular column is using it.
>
> **Implication:** The `GroupingFeature<T,V>` on a column must be the activation trigger. The grid discovers grouping capability when the feature attaches and registers itself with a shared `GroupingStateManager`.

**Q2: Priority Placement?**

| Priority | Rationale |
|----------|-----------|
| **50** (before Core) | Grouping changes what rows exist |
| **125** (before Filtering) | Groups should be filterable |
| **175** (after Filtering) | Filter first, then group results |

> **DECISION:** Priority 50 (before Core).
>
> **Rationale:** Grouping fundamentally transforms the data shape before rendering. Group header rows need to exist before other features process them.

**Q3: Group Header Row Representation?**

| Approach | Pros | Cons |
|----------|------|------|
| Virtual rows (like spacer rows) | Consistent with `RowExpandFeature` | Complex identity management |
| CSS-only (sticky headers) | Simple | Limited template flexibility |
| Data transformation | Clean separation | Requires wrapper data source |
| **In-grid rendering** | All artifacts within Grid/ComposableColumns namespace | Requires grid-level coordination |

> **DECISION:** Explore in-grid rendering with user-injectable templates.
>
> **Rationale:** Keep all artifacts within the Grid (and Composable namespace), with the option of the user injecting their own template. This avoids external data source wrappers while maintaining flexibility.

### 2.3 Column-First Grouping Architecture

Given the column-first activation pattern, here's how grouping could work:

```
┌─────────────────────────────────────────────────────────────────┐
│ ComposableGrid<TGridItem>                                       │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ GroupingCoordinator (registered on first GroupingFeature)│   │
│  │  - Tracks which columns have GroupingFeature             │   │
│  │  - Manages active grouping column                        │   │
│  │  - Holds GroupStateManager (expand/collapse per key)     │   │
│  │  - Transforms Items → grouped items + header rows        │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ComposableCol │  │ComposableCol │  │ComposableCol │         │
│  │              │  │              │  │              │         │
│  │ GroupingFeat │  │ (no grouping)│  │ GroupingFeat │         │
│  │ Property=Cat │  │              │  │ Property=Typ │         │
│  │ IsActive=true│  │              │  │ IsActive=fals│         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

**Activation Flow:**

1. `GroupingFeature<T,V>.OnAttach()` checks if `GroupingCoordinator` exists in `FeatureContext`
2. If not, creates and registers `GroupingCoordinator` (first grouping column wins)
3. Registers this column's grouping capability with the coordinator
4. Coordinator transforms grid's `Items` to include group header markers

### 2.4 Proposed API (Revised)

```csharp
// Column-level: activates grouping on this column
public class GroupingFeature<TGridItem, TValue> : IColumnFeature<TGridItem>, IGridDataTransformer<TGridItem>
{
    public int Priority => FeaturePriority.Grouping; // 50

    /// <summary>
    /// Whether this column's grouping is currently active.
    /// Only one column can have active grouping at a time.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Expression to extract the group key from each item.
    /// If null, uses the column's Property expression.
    /// </summary>
    public Func<TGridItem, TValue>? GroupBy { get; set; }

    /// <summary>
    /// Custom template for rendering group header rows.
    /// If null, uses default template showing key and count.
    /// </summary>
    public RenderFragment<GroupHeaderContext<TGridItem, TValue>>? HeaderTemplate { get; set; }

    /// <summary>
    /// Whether groups start expanded.
    /// </summary>
    public bool InitiallyExpanded { get; set; } = true;
}

// Shared coordinator (lives in FeatureContext, created by first GroupingFeature)
internal class GroupingCoordinator<TGridItem>
{
    private readonly Dictionary<string, IGroupingFeature<TGridItem>> _groupableColumns = new();
    private readonly GroupStateManager<TGridItem> _stateManager = new();

    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature);
    public void SetActiveGrouping(string? columnId);
    public IEnumerable<GroupedRow<TGridItem>> TransformItems(IQueryable<TGridItem> items);
}

// Represents either a group header or a data row
public abstract record GroupedRow<TGridItem>;
public record GroupHeaderRow<TGridItem, TValue>(TValue Key, int Count, bool IsExpanded) : GroupedRow<TGridItem>;
public record DataRow<TGridItem>(TGridItem Item) : GroupedRow<TGridItem>;

// Context for HeaderTemplate
public record GroupHeaderContext<TGridItem, TValue>(
    TValue Key,
    IReadOnlyList<TGridItem> Items,
    int Count,
    bool IsExpanded,
    Func<Task> ToggleAsync,
    int Level // For future nested grouping
);
```

### 2.5 In-Grid Rendering Approach

**Option A: Marker Interface on TGridItem**

```csharp
// Requires TGridItem to implement marker (intrusive)
public interface IGroupHeaderMarker { object GroupKey { get; } }
```

❌ **Rejected** - Too intrusive, changes user's model.

**Option B: Wrapper Data Source (like ExpandableGridDataSource)**

```csharp
// GroupingDataSource wraps items and injects header rows
public class GroupingDataSource<TGridItem> where TGridItem : class, new()
{
    public IQueryable<TGridItem> Items { get; } // Includes synthetic header items
}
```

⚠️ **Possible** - But requires `TGridItem : new()` constraint and synthetic instances.

**Option C: Grid-Level Row Interception**

```csharp
// ComposableGrid intercepts row rendering when grouping is active
// Before rendering each row, checks with GroupingCoordinator
protected override void RenderRow(RenderTreeBuilder builder, TGridItem item)
{
    if (_groupingCoordinator?.ShouldRenderGroupHeader(item, out var header) == true)
    {
        RenderGroupHeader(builder, header);
    }
    base.RenderRow(builder, item);
}
```

✅ **Preferred** - All logic stays in Grid/ComposableColumns namespace, no external wrappers.

**Option D: CSS Grid Row Spanning**

```csharp
// Group headers rendered as full-width rows using CSS grid
// Each GroupingFeature contributes to the header template
.group-header {
    grid-column: 1 / -1; /* Span all columns */
}
```

✅ **Preferred for rendering** - Clean CSS-based approach for visual spanning.

### 2.6 Recommended Approach: Option C + D Combined

1. **GroupingCoordinator** (registered in `FeatureContext`) transforms the item sequence
2. **ComposableGrid** checks each item against coordinator before rendering
3. **Group headers** are rendered as full-width rows using CSS `grid-column: 1 / -1`
4. **User templates** are optional - default template shows key + count + expand/collapse

```razor
<!-- Usage -->
<ComposableGrid Items="@items.AsQueryable()">
    <ComposableColumn Property="x => x.Category">
        <GroupingFeature IsActive="true" InitiallyExpanded="false">
            <HeaderTemplate>
                <div class="group-header">
                    <span class="expand-icon">@(context.IsExpanded ? "▼" : "▶")</span>
                    <strong>@context.Key</strong>
                    <span class="count">(@context.Count items)</span>
                </div>
            </HeaderTemplate>
        </GroupingFeature>
    </ComposableColumn>
    <ComposableColumn Property="x => x.Name" />
</ComposableGrid>
```

### 2.7 Implementation Considerations

1. **Group header rows** are not `TGridItem` instances - they're rendered by the grid itself
2. **Expand/collapse state** managed by `GroupStateManager` (keyed by group key hash)
3. **Sorting within groups** preserved - GroupingCoordinator sorts items before grouping
4. **Nested grouping** (future) - `Level` property in context, recursive group structure
5. **Virtualization** - Headers count toward virtual item count, need height calculation

### 2.8 Additional Design Questions

The following questions need decisions before creating the Feature Spec:

**Q4: Sorting interaction?**

When grouping is active, how does column sorting interact with groups?

| Option | Description |
|--------|-------------|
| **Sort within groups** | Each group's items are sorted, group order unchanged |
| **Sort groups by key** | Groups reordered by key, items within sorted separately |
| **Sort reorders everything** | Sorting disabled or reorders all items ignoring groups |
| **Configurable** | Parameter to choose behavior |

#### Discussion: MudBlazor Behavior

Based on MudBlazor documentation and typical data grid behavior:
- MudBlazor sorts **within groups** by default when a column sort is applied
- Group order is typically determined by the group key (ascending by default)
- When sorting the grouped column itself, it affects group order
- When sorting a non-grouped column, items within each group are sorted

#### Analysis

| Behavior | Use Case | Complexity |
|----------|----------|------------|
| Sort within groups | Most common expectation - user wants to find items within their category | Low |
| Sort groups by key | Natural ordering for grouped data | Low |
| Sort reorders everything | Breaks grouping mental model - confusing | N/A (rejected) |
| Configurable | Maximum flexibility but adds API surface | Medium |

**Recommendation:** Default to "sort within groups" + "groups ordered by key". This matches user expectations and MudBlazor behavior. A future `GroupSortBehavior` parameter could be added if needed.

> **DECISION:** ✅ **Sort within groups + groups ordered by key** (confirmed).
>
> **Behavior:**
> - When sorting a **non-grouped column**: Items within each group are sorted; group order unchanged
> - When sorting the **grouped column**: Group order changes based on key sort direction
> - Groups are always ordered by key (ascending by default, controllable via `GroupOrder`)

```csharp
/// <summary>
/// How sorting interacts with grouping.
/// </summary>
public enum GroupSortBehavior
{
    /// <summary>
    /// Sorting applies within each group. Group order determined by GroupOrder.
    /// This is the default and matches MudBlazor behavior.
    /// </summary>
    SortWithinGroups,

    /// <summary>
    /// Sorting the grouped column changes group order.
    /// Sorting other columns sorts within groups.
    /// </summary>
    SortGroupedColumnAffectsGroupOrder
}
```

**Q5: Filtering interaction?**

How does filtering interact with grouping?

| Option | Description |
|--------|-------------|
| **Filter before grouping** | Filter items first, then group the results |
| **Filter within groups** | Groups always exist, filtering hides items within |
| **Both supported** | Parameter to choose behavior |

#### Discussion: Feature Composition Order

In the ComposableColumn architecture, features are composed at design time via the `Features` child content or `FeatureCollection` parameter. The **priority** determines execution order:

```
Priority 50  → Grouping (transforms data shape)
Priority 150 → Filtering (filters items)
```

With Grouping at priority 50 (before Filtering at 150), the natural flow would be:
1. **Group first** → Items are organized into groups
2. **Filter second** → Filter applies within each group

However, the more intuitive UX is often:
1. **Filter first** → User sees only matching items
2. **Group second** → Matching items are grouped

#### Implementation Consideration

Both options are achievable:
- **Filter before grouping:** GroupingCoordinator receives already-filtered `IQueryable<TGridItem>`
- **Filter within groups:** GroupingCoordinator groups all items, then filtering hides non-matching rows

The priority system doesn't force one approach - it's about when the feature *processes* data, not when filtering *applies*. The GroupingCoordinator can work with pre-filtered or post-filtered data.

**Key insight:** The composition order at design time (adding features to column) doesn't determine data flow. The `IQueryable<TGridItem>` passed to the grid is already the source of truth.

> **DECISION:** ✅ **Both options supported** (confirmed).
>
> **Rationale:** Different use cases require different behaviors. Support both via a parameter.

```csharp
/// <summary>
/// When filtering and grouping are both active, determines the order of operations.
/// Default is FilterThenGroup (most intuitive UX).
/// </summary>
public FilterGroupOrder FilterBehavior { get; set; } = FilterGroupOrder.FilterThenGroup;

public enum FilterGroupOrder
{
    /// <summary>
    /// Filter items first, then group the filtered results.
    /// Empty groups (after filtering) are hidden by default.
    /// Most intuitive for users searching within data.
    /// </summary>
    FilterThenGroup,

    /// <summary>
    /// Group all items first, then apply filter within each group.
    /// Groups always exist; filtering shows/hides items within groups.
    /// Useful when group structure must remain visible.
    /// </summary>
    GroupThenFilter
}
```

**Q6: Group key equality?**

How are group keys compared for equality?

| Option | Description |
|--------|-------------|
| **Default EqualityComparer** | Use `EqualityComparer<TValue>.Default` |
| **Custom comparer parameter** | Allow `IEqualityComparer<TValue>? KeyComparer` |
| **ToString() comparison** | Compare string representations |

#### Discussion

| Option | Pros | Cons |
|--------|------|------|
| **Default EqualityComparer** | Works for most types (string, int, enum), no config needed | May not work for complex types without proper `Equals`/`GetHashCode` |
| **Custom comparer** | Full control, handles edge cases | More API surface, rarely needed |
| **ToString()** | Simple, always works | Performance overhead, loses type safety, case sensitivity issues |

**Recommendation:** Start with `EqualityComparer<TValue>.Default` as the default. Add optional `IEqualityComparer<TValue>? KeyComparer` parameter for edge cases. Avoid `ToString()` comparison.

```csharp
public IEqualityComparer<TValue>? KeyComparer { get; set; }

// Usage in GroupingCoordinator:
var comparer = feature.KeyComparer ?? EqualityComparer<TValue>.Default;
var groups = items.GroupBy(feature.GroupBy, comparer);
```

> **DECISION:** ✅ **Default EqualityComparer + optional custom comparer** (confirmed).
>
> **Implementation:**
> - Default: `EqualityComparer<TValue>.Default` (works for string, int, enum, etc.)
> - Optional: `IEqualityComparer<TValue>? KeyComparer` parameter for complex types
> - Avoid `ToString()` comparison (performance, type safety issues)

**Q7: Empty groups display?**

After filtering, what happens to groups with 0 matching items?

| Option | Description |
|--------|-------------|
| **Hide empty groups** | Groups with count=0 are not rendered |
| **Show empty groups** | Groups always shown, even with "(0 items)" |
| **Configurable** | `HideEmptyGroups` parameter |

> **DECISION:** ✅ **Configurable** with `HideEmptyGroups` parameter, defaulting to `true` (hide).
>
> **Rationale:** Empty groups add visual noise and consume screen space. Users filtering data typically want to see only relevant results. The parameter allows showing empty groups when that's meaningful (e.g., "No items in this category" feedback).

```csharp
/// <summary>
/// Whether to hide groups that have no items (after filtering).
/// Default is true (hide empty groups).
/// </summary>
public bool HideEmptyGroups { get; set; } = true;
```

**Q8: Group ordering?**

How are groups ordered in the display?

| Option | Description |
|--------|-------------|
| **By key ascending** | Alphabetical/numeric order of key values |
| **By key descending** | Reverse order |
| **First occurrence** | Order groups appear based on first item |
| **Custom comparer** | `IComparer<TValue>? GroupOrder` parameter |
| **Configurable** | Parameter for sort direction + custom comparer |

> **DECISION:** ✅ **Configurable** with sort direction enum and optional custom comparer.
>
> **Rationale:** Key ascending/descending covers most cases. Custom comparer handles edge cases. The UI should show standard sort chevrons (▲/▼) to indicate group order direction.

```csharp
/// <summary>
/// How groups are ordered. Default is ascending by key.
/// </summary>
public GroupSortDirection GroupOrder { get; set; } = GroupSortDirection.Ascending;

/// <summary>
/// Optional custom comparer for group ordering.
/// When provided, overrides GroupOrder direction.
/// </summary>
public IComparer<TValue>? GroupOrderComparer { get; set; }

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

**Q9: Null key handling?**

What happens when `GroupBy` returns `null`?

| Option | Description |
|--------|-------------|
| **Separate group** | Create "(No Value)" or "(Null)" group |
| **Exclude from grouping** | Items with null key shown ungrouped at top/bottom |
| **Throw exception** | Null keys are invalid |
| **Configurable** | `NullKeyBehavior` parameter |

> **DECISION:** ✅ **Configurable** with `NullKeyBehavior` parameter. Throwing is NOT an option.
>
> **Rationale:** Null keys are a valid data condition, not an error. Users should decide how to handle them. Default to creating a separate group since that's the most visible/debuggable behavior.

```csharp
/// <summary>
/// How to handle items where GroupBy returns null.
/// Default is to create a separate "(No Value)" group.
/// </summary>
public NullKeyBehavior NullKeyHandling { get; set; } = NullKeyBehavior.SeparateGroup;

/// <summary>
/// Display text for the null group when NullKeyHandling is SeparateGroup.
/// Default is "(No Value)".
/// </summary>
public string NullGroupLabel { get; set; } = "(No Value)";

public enum NullKeyBehavior
{
    /// <summary>Create a separate group for items with null keys.</summary>
    SeparateGroup,
    /// <summary>Show items with null keys ungrouped at the top of the grid.</summary>
    ShowAtTop,
    /// <summary>Show items with null keys ungrouped at the bottom of the grid.</summary>
    ShowAtBottom,
    /// <summary>Exclude items with null keys from display entirely.</summary>
    Exclude
}
```

**Q10: Default HeaderTemplate?**

What does the built-in default template render when no custom `HeaderTemplate` is provided?

| Element | Include? |
|---------|----------|
| Expand/collapse chevron icon | ? |
| Group key value | ? |
| Item count | ? |
| Indentation (for future nesting) | ? |
| Click handler on entire header | ? |

> **DECISION:** ✅ **Full-featured default template** - assume no custom template is provided.
>
> **Rationale:** The default must be fully functional since most users won't customize it.

**Default HeaderTemplate specification:**

| Element | Include | Implementation |
|---------|---------|----------------|
| Expand/collapse chevron | ✅ Yes | `▶` (collapsed) / `▼` (expanded), rotates on toggle |
| Group key value | ✅ Yes | `ToString()` of key, or `NullGroupLabel` if null |
| Item count | ✅ Yes | `"(N items)"` or `"(N item)"` for singular |
| Indentation | ✅ Yes | CSS `padding-left` based on `Level` (for future nesting) |
| Click handler | ✅ Yes | Entire header row is clickable to toggle |
| Focus indicator | ✅ Yes | Visible focus ring for accessibility |
| Hover state | ✅ Yes | Background color change on hover |

```razor
<!-- Default template (internal) -->
<div class="qg-group-header @(IsExpanded ? "expanded" : "collapsed")"
     style="padding-left: @(Level * 16)px"
     @onclick="ToggleAsync"
     tabindex="0"
     role="button"
     aria-expanded="@IsExpanded">
    <span class="qg-group-chevron">@(IsExpanded ? "▼" : "▶")</span>
    <span class="qg-group-key">@(Key?.ToString() ?? NullGroupLabel)</span>
    <span class="qg-group-count">(@Count @(Count == 1 ? "item" : "items"))</span>
</div>
```

**Q11: Keyboard accessibility?**

What keyboard interactions should be supported?

| Key | Action |
|-----|--------|
| `Enter` / `Space` on header | Toggle expand/collapse? |
| `Arrow Up/Down` | Navigate between groups? |
| `Home` / `End` | Jump to first/last group? |
| `Left` / `Right` on header | Collapse/expand? |

> **DECISION:** 📋 **Backlog** - Defer to future enhancement.
>
> **Rationale:** Basic accessibility (focusable headers, click to toggle) will be included in the default template. Full keyboard navigation is a polish item.

**Q12: Collapse/Expand All API?**

Should there be programmatic control to collapse or expand all groups?

| Option | Description |
|--------|-------------|
| **No API** | Only individual toggle via UI |
| **Methods on coordinator** | `CollapseAllAsync()`, `ExpandAllAsync()` |
| **Methods + UI buttons** | API + optional header buttons |

#### Discussion: Existing Functionality

We already have this pattern in `RowStateManager<TGridItem>`:

```csharp
// From ComposableColumns/Features/Expansion/State/RowStateManager.cs
public async Task ClearAllAsync(CancellationToken cancellationToken = default)
{
    // Clears all expanded rows - effectively "collapse all"
}
```

**Implementation effort:** Low - we can reuse the same pattern:

```csharp
// GroupStateManager (similar to RowStateManager)
public class GroupStateManager<TValue> : IDisposable
{
    private readonly HashSet<TValue> _expandedGroups = new();

    public async Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken ct = default);
    public async Task CollapseAllAsync(CancellationToken ct = default);
    public async Task ToggleAsync(TValue key, CancellationToken ct = default);
    public bool IsExpanded(TValue key);
}
```

**UI consideration:** Optional toolbar buttons could be added via a parameter:

```csharp
/// <summary>
/// Whether to show Expand All / Collapse All buttons in the grid header.
/// </summary>
public bool ShowExpandCollapseAllButtons { get; set; } = false;
```

> **DECISION:** ✅ **API + UI buttons** (confirmed).
>
> **Scope:**
> - **API:** `GroupStateManager<TValue>` with `ExpandAllAsync()`, `CollapseAllAsync()`, `ToggleAsync()`, `IsExpanded()`
> - **UI:** Optional toolbar buttons via `ShowExpandCollapseAllButtons` parameter (default `false`)
> - **Implementation effort:** Low - reuses existing `RowStateManager` pattern
>
> **API specification:**

```csharp
/// <summary>
/// Manages expand/collapse state for groups.
/// Reuses the pattern from RowStateManager.
/// </summary>
public class GroupStateManager<TValue> : IDisposable
{
    private readonly HashSet<TValue> _expandedGroups = new();
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
    public bool IsExpanded(TValue key) => _expandedGroups.Contains(key);

    /// <summary>Toggle a group's expand/collapse state.</summary>
    public async Task ToggleAsync(TValue key, CancellationToken ct = default);

    /// <summary>Expand a specific group.</summary>
    public async Task ExpandAsync(TValue key, CancellationToken ct = default);

    /// <summary>Collapse a specific group.</summary>
    public async Task CollapseAsync(TValue key, CancellationToken ct = default);

    /// <summary>Expand all groups.</summary>
    public async Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken ct = default);

    /// <summary>Collapse all groups.</summary>
    public async Task CollapseAllAsync(CancellationToken ct = default);
}
```

**UI buttons (when `ShowExpandCollapseAllButtons = true`):**

```razor
<!-- Rendered in grid header area -->
<div class="qg-group-controls">
    <button class="qg-expand-all" @onclick="ExpandAllAsync" title="Expand all groups">
        <span>⊞</span> Expand All
    </button>
    <button class="qg-collapse-all" @onclick="CollapseAllAsync" title="Collapse all groups">
        <span>⊟</span> Collapse All
    </button>
</div>
```

---

## 3. Multi-Selection Feature

### 3.1 MudBlazor Behavior Analysis

From `Soak.razor` usage:

```razor
<MudDataGrid Items="@items" 
             MultiSelection="true"
             @bind-SelectedItems="@selectedItems"
             SelectedItemsChanged="@OnSelectionChanged">
    <SelectColumn T="SoakItem" />
    <PropertyColumn Property="x => x.Name" />
</MudDataGrid>
```

**Key MudBlazor Selection Behaviors:**
1. `MultiSelection="true"` - Enables checkbox selection mode
2. `SelectedItems` - Two-way bound collection
3. `SelectedItemsChanged` - Callback on selection change
4. `SelectColumn` - Renders checkbox column
5. Header checkbox for select all/none
6. Shift+click for range selection
7. Works with virtualization

### 3.2 Design Questions

**Q1: Selection State Location?**

| Approach | Pros | Cons |
|----------|------|------|
| **Grid-level** (`ComposableGrid` parameter) | Natural for multi-column selection | Not a column feature |
| **SelectionFeature on dedicated column** | Fits feature pattern | Column owns grid-wide state? |
| **Hybrid** (Grid state + SelectColumn renders) | Clean separation | Two integration points |

> **Recommendation:** Hybrid - `ComposableGrid` owns `SelectedItems` state, `SelectColumnFeature` renders the checkboxes.

**Q2: Priority Placement?**

Selection is primarily a rendering concern for the checkbox column:

| Priority | Rationale |
|----------|-----------|
| **25** (very early) | Selection column should render first |
| **300** (with Styling) | It's just visual representation |

> **Recommendation:** Priority 25 for `SelectColumnFeature` - selection column typically appears first.

**Q3: Integration with Grouping?**

| Behavior | Description |
|----------|-------------|
| Select group header | Selects/deselects all items in group |
| Partial selection | Group header shows indeterminate state |
| Count display | "3 of 5 selected" in group header |

### 3.3 Proposed API

```csharp
// Grid-level selection state
public partial class ComposableGrid<TGridItem>
{
    [Parameter] public SelectionMode SelectionMode { get; set; } = SelectionMode.None;
    [Parameter] public ISet<TGridItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<ISet<TGridItem>> SelectedItemsChanged { get; set; }
    [Parameter] public EventCallback<SelectionChangedEventArgs<TGridItem>> OnSelectionChanged { get; set; }
}

public enum SelectionMode { None, Single, Multiple }

// Column feature for rendering checkboxes
public class SelectColumnFeature<TGridItem> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Selection; // 25
    
    public bool ShowHeaderCheckbox { get; set; } = true;
    public bool AllowRangeSelection { get; set; } = true;
}

// Event args
public record SelectionChangedEventArgs<TGridItem>(
    ISet<TGridItem> SelectedItems,
    IEnumerable<TGridItem> Added,
    IEnumerable<TGridItem> Removed
);
```

### 3.4 Implementation Considerations

1. **Header checkbox** needs access to all items (or page items if paginated)
2. **Shift+click range** requires tracking last clicked item
3. **Virtualization compatibility** - selection state must persist for off-screen items
4. **Keyboard navigation** - Space to toggle, Shift+Arrow for range
5. **Integration with grouping** - group header checkbox behavior

---

## 4. Implementation Order

### Recommended Sequence (Revised)

Based on the column-first activation pattern decision:

```
Phase 1: Row Grouping (unblocks Index.razor)
├── M1: Add FeaturePriority.Grouping = 50
├── M2: Create GroupingCoordinator, GroupStateManager
├── M3: Create GroupHeaderContext, GroupedRow types
├── M4: Implement GroupingFeature<T,V> with OnAttach registration
├── M5: Modify ComposableGrid to check for GroupingCoordinator
├── M6: Implement grid-level row interception (Option C)
├── M7: Add CSS for group header spanning (Option D)
├── M8: Demo page + tests
└── M9: Migrate Index.razor

Phase 2: Multi-Selection (unblocks Soak.razor)
├── M1: Add FeaturePriority.Selection = 25
├── M2: Create SelectionCoordinator (same pattern as GroupingCoordinator)
├── M3: Implement SelectionFeature<T> with OnAttach registration
├── M4: Header checkbox + range selection
├── M5: Integration with GroupingFeature (group header selection)
├── M6: Demo page + tests
└── M7: Migrate Soak.razor
```

### Key Architecture Pattern

Both features follow the same **Column-First Coordinator Pattern**:

```
┌────────────────────────────────────────────────────────────────┐
│ Pattern: Column-First Coordinator                              │
├────────────────────────────────────────────────────────────────┤
│ 1. Feature.OnAttach() checks FeatureContext for Coordinator    │
│ 2. If missing, creates and registers Coordinator               │
│ 3. Feature registers itself with Coordinator                   │
│ 4. Coordinator provides grid-level behavior coordination       │
│ 5. ComposableGrid discovers Coordinator via FeatureContext     │
│ 6. Grid renders accordingly (headers, selection UI, etc.)      │
└────────────────────────────────────────────────────────────────┘
```

### Effort Estimates

| Feature | Complexity | Effort | Dependencies |
|---------|------------|--------|--------------|
| Row Grouping | High | 3-4 days | None |
| Multi-Selection | Medium | 2-3 days | Grouping (for integration) |

---

## 5. Open Questions & Decisions

### Row Grouping

| # | Question | Decision | Rationale |
|---|----------|----------|-----------|
| 5.1 | **Virtualization support?** | ✅ **Required** | All features in the ComposableColumn namespace support virtualization. This is a rule. |
| 5.2 | **Multiple grouping levels?** | ❌ **Single-level only** | Start with single-level. Design API for extensibility but don't implement nested grouping. |
| 5.3 | **Drag-to-reorder groups?** | 📋 **Backlog** | Out of scope for initial implementation. |
| 5.4 | **Persist group state?** | 📋 **Backlog** | Not in the first release. LocalStorage/SessionStorage integration deferred. |

#### Virtualization Implications

Since virtualization is required, the implementation must:

1. **Group headers count toward virtual item count** - Headers are virtual rows with calculated heights
2. **Collapsed groups skip their items** - When a group is collapsed, its items are excluded from the virtualized item source
3. **Expand/collapse recalculates** - Changing group state triggers virtualization recalculation
4. **Sticky group headers** (optional enhancement) - Consider CSS `position: sticky` for headers during scroll

```csharp
// GroupingCoordinator must provide virtualization-compatible output
public interface IGroupingCoordinator<TGridItem>
{
    /// <summary>
    /// Returns the total count including group headers (for virtualization).
    /// </summary>
    int GetVirtualItemCount(IQueryable<TGridItem> items);

    /// <summary>
    /// Returns items for the visible range, including group headers.
    /// </summary>
    IEnumerable<GroupedRow<TGridItem>> GetVirtualizedItems(
        IQueryable<TGridItem> items, 
        int startIndex, 
        int count);
}
```

### Multi-Selection (Deferred)

> **Note:** Multi-Selection discussion is deferred until Row Grouping is complete.

| # | Question | Status |
|---|----------|--------|
| 1 | Selection persistence across pagination? | ⏳ Pending |
| 2 | Maximum selection limit? | ⏳ Pending |
| 3 | Disabled rows? | ⏳ Pending |
| 4 | Selection changed debouncing? | ⏳ Pending |

### Cross-Cutting (Deferred)

| # | Question | Status |
|---|----------|--------|
| 1 | Separate NuGet packages? | ⏳ Pending |
| 2 | CSS class naming conventions? | ⏳ Pending |
| 3 | Accessibility (ARIA)? | ⏳ Pending |

---

## 6. Row Grouping Feature Summary

### Decisions Made

| Decision | Choice |
|----------|--------|
| Activation pattern | Column-first (GroupingFeature triggers) |
| Priority | 50 (before Core) |
| Rendering approach | Grid-level row interception + CSS spanning |
| Virtualization | Required (rule) |
| Grouping levels | Single-level only |
| Sorting interaction | Sort within groups + groups ordered by key |
| Filtering interaction | Both options supported via `FilterBehavior` parameter |
| Group key equality | Default `EqualityComparer<TValue>` + optional `KeyComparer` |
| Empty groups | Configurable via `HideEmptyGroups`, default `true` |
| Group ordering | Configurable via `GroupOrder` enum + optional comparer |
| Null key handling | Configurable via `NullKeyBehavior`, default `SeparateGroup` |
| Default HeaderTemplate | Full-featured (chevron, key, count, click handler) |
| Keyboard accessibility | 📋 Backlog |
| Collapse/Expand All | API + optional UI buttons |
| Drag-to-reorder | 📋 Backlog |
| State persistence | 📋 Backlog |

### Ready for Feature Spec

The Row Grouping feature is ready to move to the next phase:

1. ✅ Design decisions documented
2. ✅ API shape defined
3. ✅ Architecture pattern established (Column-First Coordinator)
4. ✅ Scope defined (single-level, virtualization required)
5. ✅ Backlog items identified
6. ✅ **All design questions answered (Q1-Q12)**

---

## 7. Next Steps

1. ~~**Review this discussion**~~ ✅ Complete
2. ~~**Create Feature Spec**~~ ✅ Complete - `Docs/Feature Design/RowGroupingFeature.md`
3. **Create Implementation Plan** - `Docs/Feature Design/ImplementationPlans/Plan_RowGroupingFeature.md`
4. **Create Task List** - `Docs/Feature Design/Tasks/RowGroupingFeature-Tasks.md`
5. **Implement** - Follow task milestones
6. **Migrate Index.razor** - First real-world usage

---

## 8. Backlog

| Feature | Item | Priority |
|---------|------|----------|
| Row Grouping | Drag-to-reorder groups | Low |
| Row Grouping | Persist group state (LocalStorage) | Low |
| Row Grouping | Multiple grouping levels (nested) | Medium |
| Row Grouping | Full keyboard accessibility (Q11) | Medium |
| Multi-Selection | Full implementation | Medium |

---

## 9. References

- `MudDataGrid to QuickGrid conversion.md` - Migration analysis
- `ExpandableRowFeature.md` - Pattern for virtual row implementation
- `InlineEditingPolish.md` - Pattern for feature polish
- [MudBlazor DataGrid Docs](https://mudblazor.com/components/datagrid)
- [QuickGrid Source](https://github.com/dotnet/aspnetcore/tree/main/src/Components/QuickGrid)
