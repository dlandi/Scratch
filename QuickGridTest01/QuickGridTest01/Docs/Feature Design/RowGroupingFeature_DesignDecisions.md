# Row Grouping Feature Design Decisions

## Document Information

| Attribute | Value |
|-----------|-------|
| Parent Document | `RowGroupingFeature.md` |
| Purpose | Historical record of design decisions made during specification |
| Status | Complete - All decisions finalized |

**Note:** This document is a helper/historical record. The normative source of truth is `RowGroupingFeature.md` (and the accompanying implementation plan).

---


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

The coordinator needs to work with any `TValue` without knowing the specific type at compile time.

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
| **Same as data rows** | Use grid's row `ItemSize` (1×) |
| **Fixed height** | Hardcoded value (e.g., 80px) |
| **Configurable span** | Add `GroupHeaderSlotSpan` parameter and compute height as `GroupHeaderSlotSpan × ItemSize` |
| **Auto-calculated** | Measure actual rendered height |

> **DECISION:** ✅ **Configurable span: `GroupHeaderSlotSpan × ItemSize` (default span = 2)**
>
> **Rationale:** 
> - Group headers need more visual weight than data rows
> - Using a span expressed in row units keeps behavior and styling aligned with QuickGrid virtualization
> - Default span is 2 for visual weight, but the value is not hard-coded
> - Effective pixel height is always `GroupHeaderSlotSpan × ItemSize`

Implementation note: The feature exposes `GroupHeaderSlotSpan` (default `2`) and uses CSS variables to ensure the rendered header height matches `GroupHeaderSlotSpan × ItemSize`.

### Q18a: Virtualization Strategy for Multi-slot Headers

How should multi-slot group headers work with QuickGrid virtualization?

| Option | Description |
|--------|-------------|
| **Slot-window mapping** | Implement a windowed item provider and translate `startIndex/count` in slot space |
| **Spacer-row injection** | Represent multi-slot headers as real rows by emitting spacer rows (similar to Row Expansion) |

> **DECISION:** ✅ **Spacer-row injection**
>
> **Rationale:**
> - Aligns with existing ComposableColumns patterns (`ExpandableGridDataSource`)
> - Avoids inventing a custom virtualization integration point in `ComposableGrid`
> - Keeps QuickGrid virtualization operating over a normal flattened row sequence

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
| **Grid row interception** | Modify `ComposableGrid` to pattern-match custom row types |
| **Expansion-style marker/spacer rows** | Keep `QuickGrid.Items` typed as `TGridItem` and inject marker+spacer rows, rendered via an `ICellRenderFeature<TGridItem>` |

> **DECISION:** ✅ **Expansion-style marker/spacer rows** (leverage existing pattern)
>
> **Implementation:** Follow the proven `ComposableRowExpandDemo` approach:
> - Keep `QuickGrid.Items` typed as `IQueryable<TGridItem>`
> - Inject group header marker rows and group header spacer rows as `TGridItem` instances (identity encoded similarly to `SpacerRowFactory`)
> - Render the group header UI from a dedicated first `ComposableColumn` feature (`ICellRenderFeature<TGridItem>`) when it encounters the **FIRST** header marker row
> - Render blank output for header spacer rows
>
> **Rationale:** `ComposableGrid` does not implement a row rendering loop; QuickGrid renders rows. The expansion feature demonstrates that marker/spacer rows plus cell-pipeline rendering can achieve virtualization-aligned extra height without inventing grid-level rendering hooks.

**Additional coordination requirement (clarification):**
All columns that include grouping must register with the coordinator to form the complete set of groupable columns. The **first** grouping-enabled column is only the *header-host* for rendering; subsequent grouping columns remain selectable as group targets.

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
public abstract record GroupedRow<TGridItem>
    where TGridItem : class;

public record GroupHeaderRow<TGridItem>(
    object? Key,           // Stored as object for coordinator compatibility
    int Count,
    bool IsExpanded,
    int Level
) : GroupedRow<TGridItem>
    where TGridItem : class;

public record DataRow<TGridItem>(
    TGridItem Item
) : GroupedRow<TGridItem>
    where TGridItem : class;
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

The coordinator's `RegisterColumn(string columnId, ...)` method needs a column identifier to distinguish between groupable columns.

| Option | Description |
|--------|-------------|
| **Use GroupBy property name** | Extract from `GroupBy` expression (e.g., `"Category"`, `"Status"`) |
| **Use Title** | `FeatureContext.Title` |
| **Manufacture sequential ID** | Generate sequential integer ID for each column |

**Analysis:** The column identifier must distinguish between different groupable columns on the same grid (e.g., Column A grouping by Category vs Column B grouping by Status). The property name from the `GroupBy` expression naturally provides this distinction.

> **DECISION:** ✅ **Use GroupBy property name with manufactured fallback**
>
> **Implementation:** 
> - Extract the property name from `_effectiveGroupBy` (the resolved GroupBy expression)
> - If property name extraction fails, manufacture a sequential integer ID
>
> ```csharp
> private static int _columnIdCounter;
>
> private string GetColumnId()
> {
>     // Try to extract property name from the GroupBy expression
>     // This gives us "Category", "Status", etc.
>     var propertyName = GetPropertyNameFromDelegate(_effectiveGroupBy);
>     
>     if (!string.IsNullOrEmpty(propertyName))
>         return propertyName;
>     
>     // Fallback: manufacture sequential ID
>     return $"GroupingColumn_{Interlocked.Increment(ref _columnIdCounter)}";
> }
>
> private static string? GetPropertyNameFromDelegate<T>(Func<TGridItem, T>? func)
> {
>     // Use reflection to extract property name from delegate target
>     // Returns "Category", "Status", etc. or null if extraction fails
>     if (func?.Target is null) return null;
>     var field = func.Target.GetType().GetFields()
>         .FirstOrDefault(f => f.FieldType == typeof(Func<TGridItem, T>));
>     return field?.Name;
> }
> ```
>
> **Rationale:** The GroupBy property name (e.g., "Category") naturally identifies what the column is grouping by and distinguishes it from other groupable columns.

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

### Q32: Plan Verification Questions (Alignment with Existing ComposableColumns Features)

The following questions are used to validate that `RowGroupingFeature.md` and `Plan_RowGroupingFeature.md` are implementable within the existing ComposableColumns codebase and do not require invented APIs.

#### Q32.1 Integration model (QuickGrid + ComposableColumns)

1. Does the plan keep `QuickGrid.Items` typed as `IQueryable<TGridItem>` (no `GroupedRow<TGridItem>` / no union item type)?
2. Does the plan avoid requiring a grid-level row rendering loop (no `ComposableGrid` pattern-matching on synthetic row types), and instead use the established `ICellRenderFeature<TGridItem>` pipeline?
3. Does the plan explicitly identify the "header-host" column rule: the **first column that attaches `GroupingFeature` renders the group header UI**, while subsequent grouping-enabled columns do not render header UI?

#### Q32.2 Coordinator scope and lifecycle

4. Does the plan define a **shared coordinator scope across columns** on the same grid (so that all grouping-enabled columns can contribute to the set of groupable columns)?
5. Does the plan clearly distinguish the two roles:
   - *Groupable set*: all columns having grouping registered
   - *Active grouping*: exactly one column selected as the grouping key source
6. If the plan supports switching the active grouping column at runtime, does it do so via an explicit coordinator API + refresh (not by relying on feature re-attachment or markup reordering)?

> **DECISION (Coordinator scope):** The shared coordinator scope is **grid-scoped**, owned by `ComposableGrid<TGridItem>` and accessed via the cascaded `Grid` reference.
>
> **Rationale:** `FeatureContext<TGridItem>` is created per `ComposableColumn`, so `FeatureContext.RegisterService(...)` is column-scoped and cannot represent a cross-column coordinator. Existing cross-column coordination in this codebase (filtering) uses explicit grid methods (`Grid.RegisterFilter(...)`).

**Clarification (deterministic activation state):**

`ComposableGrid` maintains a deterministic internal state indicating whether grouping is active. This is not a "choice" or a consumer-driven toggle.

- `bool _hasGroupingFeatures` is true when one or more columns register a `GroupingFeature` with the grid-scoped coordinator.
- `bool _isGroupingActive` is true when `_hasGroupingFeatures` is true **and** `GroupingCoordinator.ActiveGrouping` is not null.

When `_isGroupingActive` is true, grouped items are derived from `FilteredItems` (Filter-then-Group) internally and bound to `QuickGrid.Items`.

#### Q32.3 Marker/spacer row contract (virtualization + identity)

7. Does the plan represent group header height using **spacer-row injection** (like `ExpandableGridDataSource`), i.e. each header occupies:
   - 1x **header marker** row (FIRST header row)
   - `GroupHeaderSlotSpan - 1` x **header spacer** rows?
8. Does the plan fully specify marker/spacer detection in terms of existing conventions (for example, `IRowIdentifiable.Id` with a `SpacerRowFactory`-like encoding), including a way to distinguish:
   - normal data rows
   - header marker rows
   - header spacer rows
9. Does the plan specify how marker/spacer rows are rendered by columns:
   - header-host column renders header UI for marker rows and blank for header spacer rows
   - all other columns render blank for marker/spacer rows?

> **Derived requirement:** Grouping must define a `SpacerRowFactory`-like id encoding helper for group header marker/spacer rows so that detection is deterministic under QuickGrid virtualization.

#### Q32.4 Interaction with existing filtering and sorting

10. Does the plan align with current filtering: when used with `ComposableGrid`, grouping receives `FilteredItems` (filter-then-group), and does it explicitly mark `GroupThenFilter` as not supported unless the grid’s data pipeline is reworked?
11. Does the plan specify how sorting is expected to behave while grouping is active, given QuickGrid sorting is column-driven (`ColumnBase.SortBy`) and grouping changes the effective row order?

> **DECISION (Sorting while grouping is active):** **Intra-group sorting only**
>
> **Rationale:** Global sorting over a flattened sequence would interleave group header marker/spacer rows with data rows and break the grouping contract. The grid-owned pipeline must remain deterministic: `Items → FilteredItems → (optional) GroupedItems`.
>
> **Behavior:**
> - When grouping is inactive, sorting behaves as it does today (QuickGrid/columns).
> - When grouping is active, any active column sort applies **only within each group’s item list**.
> - Group ordering is controlled exclusively by `GroupOrder` / `GroupOrderComparer` (or `FirstOccurrence`) and is not influenced by column sorting.

> **DECISION (Sort stage ownership - Option A):** **ComposableColumns owns the effective sort state when grouping is active**
>
> **Rationale:** Grouping injects header marker/spacer rows into the `QuickGrid.Items` sequence. Allowing QuickGrid to apply a global sort over the flattened sequence can reorder marker/spacer rows relative to data rows and break the grouping contract. Therefore, when grouping is active, the effective sort must be a deterministic, grid-owned pipeline stage.
>
> **Deterministic pipeline (identity stages permitted):**
> - `Items`: the grid input sequence.
> - `FilteredItems`: if any filter features are registered/active, this is `Items` with filters applied; otherwise **`FilteredItems = Items`**.
> - `SortedItems`: if a ComposableColumns sort is active, this is `FilteredItems` with that sort applied; otherwise **`SortedItems = FilteredItems`**.
> - `GroupedItems`: when grouping is active, this is derived from `SortedItems` by injecting header marker + spacer rows; otherwise the stage is skipped.

#### Q32.5 Refresh + async flow

12. After every expand/collapse or active-grouping change, does the plan identify a single responsible refresh trigger, with no fire-and-forget?
13. If the plan introduces an expansion-style grouped data source with an `OnDataChanged` event, does it explicitly require `ComposableGrid` to subscribe and call `InvokeAsync(StateHasChanged)` (aligned with how it internally handles filter changes), and does it avoid also calling `RequestRefreshAsync` for the same grouping state changes (prevent double-refresh loops)?
