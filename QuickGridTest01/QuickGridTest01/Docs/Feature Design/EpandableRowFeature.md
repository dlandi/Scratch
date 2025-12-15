# RowExpandFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.1 (Draft) |
| Status | Design |
| Created | 2025 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Expansion` |

---

## 1. Overview

### 1.1 Purpose

`RowExpandFeature<TGridItem>` is the **base feature** for row-level expansion within the ComposableColumns architecture. It provides expandable overlay content that spans a grid row when activated, with spacer row injection to push subsequent content below the overlay.

### 1.2 Role in Architecture

This feature serves as the foundation for:
- **FormRowFeature** - Adds form semantics (draft state, validation, save/cancel)
- **NestedGridFeature** - Adds child grid rendering for master-detail patterns

```
RowExpandFeature<TGridItem>           (Base: expansion + overlay)
    ??? FormRowFeature<TGridItem>     (Extends: form semantics)
    ??? NestedGridFeature<TGridItem, TChildItem>  (Extends: child grid)
```

### 1.3 Migration Context

This feature is a port of the standalone `RowColumn<TGridItem>` component to the ComposableColumns feature architecture, enabling composition with other features like filtering, styling, and inline editing.

---

## 2. Architecture

### 2.1 Interface Implementation

```csharp
public class RowExpandFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class
{
    public int Priority => FeaturePriority.Expansion; // New priority level
}
```

### 2.2 Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|  
| Row identification | `FeatureContext.RowKey` delegate | Leverages existing ComposableGrid pattern, no interface requirement |
| Overlay rendering | CSS absolute positioning | Works within `ICellRenderFeature` model, no new interface needed |
| Spacer row injection | `ExpandableGridDataSource<T>` | Pushes rows below overlay for proper visual layout |
| State management | `RowStateManager<T>` via context services | Memory-efficient with `ConditionalWeakTable` |

### 2.3 File Structure

```
ComposableColumns/Features/Expansion/
??? Core/
?   ??? RowExpandState.cs              # Collapsed/Expanded enum
?   ??? RowDisplayContext.cs           # Context for collapsed state
?   ??? RowExpandedContext.cs          # Context for expanded state
?   ??? RowStateManager.cs             # Tracks expanded rows
?   ??? ExpandableGridDataSource.cs    # Spacer row injection
?   ??? ConcurrentExpandBehavior.cs    # Block/CollapseCurrent/AllowMultiple
??? Events/
?   ??? RowBeforeExpandEventArgs.cs
?   ??? RowExpandedEventArgs.cs
?   ??? RowCollapsedEventArgs.cs
?   ??? RowStateChangedEventArgs.cs
??? Components/
?   ??? RowCard.razor                  # Reusable card component
?   ??? RowCard.razor.css
??? RowExpandFeature.cs                # Main feature implementation
```

---

## 3. Parameters

### 3.1 Trigger & Behavior

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TriggerMode` | `RowTriggerMode` | `Button` | How expansion is triggered (Button, RowClick, Custom) |
| `ConcurrentBehavior` | `ConcurrentExpandBehavior` | `Block` | Behavior when expanding while another row is open |
| `DimInactiveRows` | `bool` | `true` | Visually dim non-expanded rows |

### 3.2 Row Span & Height

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ExpandedRowSpan` | `int` | `3` | Number of row heights the overlay spans |
| `RowHeight` | `int` | `48` | Height of each row in pixels |

### 3.3 Templates

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `DisplayTemplate` | `RenderFragment<RowDisplayContext<TGridItem>>?` | No | Content when collapsed (defaults to button) |
| `ExpandedTemplate` | `RenderFragment<RowExpandedContext<TGridItem>>` | **Yes** | Content when expanded |

### 3.4 Button Customization

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ExpandButtonText` | `string` | `"Edit"` | Button text |
| `ExpandButtonClass` | `string` | `"qg-btn qg-btn-secondary qg-btn-sm"` | Button CSS class |
| `ExpandButtonIcon` | `string?` | `"bi bi-pencil"` | Button icon class |

### 3.5 Events

| Parameter | Type | Description |
|-----------|------|-------------|
| `OnBeforeExpand` | `EventCallback<RowBeforeExpandEventArgs<TGridItem>>` | Cancel expansion |
| `OnExpanded` | `EventCallback<RowExpandedEventArgs<TGridItem>>` | After expansion |
| `OnCollapsed` | `EventCallback<RowCollapsedEventArgs<TGridItem>>` | After collapse |
| `OnStateChanged` | `EventCallback<RowStateChangedEventArgs<TGridItem>>` | Any state change |

---

## 4. Context Objects

### 4.1 RowDisplayContext

Provided to `DisplayTemplate` for custom trigger rendering:

```csharp
public class RowDisplayContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; }
    public bool IsAnyRowExpanded { get; init; }
    public bool CanExpand { get; init; }
    public Func<Task> ExpandAsync { get; init; }
}
```

### 4.2 RowExpandedContext

Provided to `ExpandedTemplate` and cascaded to children:

```csharp
public class RowExpandedContext<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; }
    public Func<Task> CollapseAsync { get; init; }
}
```

---

## 5. Integration with ComposableGrid

### 5.1 RowKey Requirement

The feature uses `FeatureContext.RowKey` to identify rows:

```csharp
private object GetItemKey(TGridItem item)
{
    return _context?.RowKey?.Invoke(item) ?? item?.GetHashCode() ?? 0;
}
```

**Usage requirement**: `ComposableGrid` must have `RowKey` parameter set:

```razor
<ComposableGrid TGridItem="Employee" Items="@employees" RowKey="e => e.Id">
```

### 5.2 Data Source Registration

The feature registers `ExpandableGridDataSource<T>` as a context service:

```csharp
public void OnAttach(FeatureContext<TGridItem> context)
{
    _context = context;
    _stateManager = new RowStateManager<TGridItem>();
    
    // Register for other features to access
    context.RegisterService(_stateManager);
}
```

---

## 6. Rendering Pipeline

### 6.1 Cell Content Flow

```csharp
public void RenderCell(
    RenderTreeBuilder builder,
    ref int sequence,
    TGridItem item,
    FeatureContext<TGridItem> context,
    Action renderNext)
{
    // Skip spacer rows
    if (IsSpacerRow(item))
    {
        RenderSpacerCell(builder, ref sequence);
        return;
    }
    
    var isExpanded = _stateManager.IsRowExpanded(item);
    
    if (isExpanded)
        RenderExpandedMode(builder, ref sequence, item);
    else
        RenderDisplayMode(builder, ref sequence, item);
}
```

### 6.2 CSS Overlay Strategy

The overlay uses CSS positioning to span the row visually:

```css
.row-overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    z-index: 10;
    background: var(--color-surface);
    border: 1px solid var(--color-border-emphasis);
    box-shadow: var(--shadow-lg);
}

.row-dimmed {
    opacity: 0.5;
    pointer-events: none;
}
```

---

## 7. Usage Example

```razor
<ComposableGrid TGridItem="Employee" Items="@employees" RowKey="e => e.Id" Class="qg-grid">
    <ComposableColumn TGridItem="Employee" TValue="int"
                      Property="@(e => e.Id)"
                      Title="ID" />
    <ComposableColumn TGridItem="Employee" TValue="string"
                      Property="@(e => e.Name)"
                      Title="Name" />
    
    <ComposableColumn TGridItem="Employee" TValue="int"
                      Property="@(e => e.Id)"
                      Title="Actions"
                      FeatureCollection="@_expandFeatures" />
</ComposableGrid>

@code {
    private IColumnFeature<Employee>[] _expandFeatures = [
        new RowExpandFeature<Employee>
        {
            TriggerMode = RowTriggerMode.Button,
            ExpandButtonText = "View",
            ExpandedRowSpan = 3,
            ExpandedTemplate = context => @<RowCard Title="@($"Employee: {context.Item.Name}")">
                <p>Details for @context.Item.Name</p>
                <button @onclick="context.CollapseAsync">Close</button>
            </RowCard>
        }
    ];
}
```

---

## 8. Open Questions

1. **Spacer row ownership**: Should `ExpandableGridDataSource` be:
   - A parameter on `ComposableGrid` (grid owns it)
   - A parameter on `RowExpandFeature` (feature owns it)
   - Auto-created when any expansion feature is detected

2. **RowKey fallback**: What happens if `RowKey` is not provided? Options:
   - Throw on attach
   - Fall back to `GetHashCode()` (current behavior)
   - Require `IRowIdentifiable` interface

3. **Multiple expansion features**: Can a column have multiple expansion features (unlikely but should be prevented)?

---

## 9. Dependencies

### 9.1 Infrastructure (to be ported)

- `TypeTraits<T>` - For type-safe operations
- `Accessors` - For compiled property access
- `SelectOption<T>` - Shared option type

### 9.2 Existing ComposableColumns

- `FeatureContext<TGridItem>` - Shared context
- `ICellRenderFeature<TGridItem>` - Render interface
- `FeaturePriority` - Priority constants (needs new `Expansion` level)