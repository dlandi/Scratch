# NestedGridFeature Design Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.1 (Draft) |
| Status | Design |
| Created | 2025 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Expansion` |
| Base Class | `RowExpandFeature<TGridItem>` |

---

## 1. Overview

### 1.1 Purpose

`NestedGridFeature<TGridItem, TChildItem>` extends `RowExpandFeature<TGridItem>` to provide **master-detail grid patterns**. When a parent row is expanded, a child QuickGrid is rendered showing related items.

### 1.2 Use Cases

- Orders ? Order Lines
- Customers ? Orders
- Categories ? Products
- Departments ? Employees
- Any parent-child relationship

### 1.3 Relationship to Base

```
RowExpandFeature<TGridItem>           (Base: expansion + overlay)
    ??? NestedGridFeature<TGridItem, TChildItem>  (Extends: child grid)
```

---

## 2. Architecture

### 2.1 Class Definition

```csharp
public class NestedGridFeature<TGridItem, TChildItem> : RowExpandFeature<TGridItem>
    where TGridItem : class
    where TChildItem : class
{
    // Inherited from base:
    // - TriggerMode, ConcurrentBehavior, DimInactiveRows
    // - ExpandedRowSpan, RowHeight
    // - DisplayTemplate
    // - Events
    
    // Nested grid specific:
    [Parameter, EditorRequired]
    public Func<TGridItem, IQueryable<TChildItem>> ChildItems { get; set; }
    
    [Parameter, EditorRequired]
    public RenderFragment ChildColumns { get; set; }
}
```

### 2.2 Type Parameters

| Parameter | Description |
|-----------|-------------|
| `TGridItem` | The parent row type (e.g., `Order`) |
| `TChildItem` | The child row type (e.g., `OrderLine`) |

---

## 3. Parameters

### 3.1 Inherited from RowExpandFeature

All parameters from `RowExpandFeature<TGridItem>` are inherited.

### 3.2 Child Grid Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildItems` | `Func<TGridItem, IQueryable<TChildItem>>` | **Yes** | Function to get child items for a parent |
| `ChildColumns` | `RenderFragment` | **Yes** | Column definitions for child grid |
| `ChildGridTitle` | `string?` | No | Optional header above child grid |
| `ChildGridClass` | `string` | No | CSS class for child grid |
| `ChildRowKey` | `Func<TChildItem, object>?` | No | Row key for child grid (for nested expansion) |

### 3.3 Child Grid Behavior

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `EnableChildSorting` | `bool` | `true` | Allow sorting in child grid |
| `EnableChildPaging` | `bool` | `false` | Enable pagination in child grid |
| `ChildPageSize` | `int` | `10` | Items per page if paging enabled |
| `ShowEmptyMessage` | `bool` | `true` | Show message when no child items |
| `EmptyMessage` | `string` | `"No items"` | Message when no child items |

---

## 4. Rendering

### 4.1 Expanded Mode Override

```csharp
protected override void RenderExpandedMode(
    RenderTreeBuilder builder,
    ref int seq,
    TGridItem item,
    RowExpandedContext<TGridItem> context)
{
    var childData = ChildItems(item);
    
    builder.OpenElement(seq++, "div");
    builder.AddAttribute(seq++, "class", "nested-grid-container");
    
    // Optional title
    if (!string.IsNullOrEmpty(ChildGridTitle))
    {
        builder.OpenElement(seq++, "h4");
        builder.AddAttribute(seq++, "class", "nested-grid-title");
        builder.AddContent(seq++, ChildGridTitle);
        builder.CloseElement();
    }
    
    // Close button
    builder.OpenElement(seq++, "button");
    builder.AddAttribute(seq++, "class", "nested-grid-close");
    builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, context.CollapseAsync));
    builder.AddContent(seq++, "×");
    builder.CloseElement();
    
    // Child grid
    if (childData.Any())
    {
        builder.OpenComponent<QuickGrid<TChildItem>>(seq++);
        builder.AddComponentParameter(seq++, "Items", childData);
        builder.AddComponentParameter(seq++, "Class", ChildGridClass ?? "qg-grid nested-grid");
        builder.AddComponentParameter(seq++, "ChildContent", ChildColumns);
        builder.CloseComponent();
    }
    else if (ShowEmptyMessage)
    {
        builder.OpenElement(seq++, "p");
        builder.AddAttribute(seq++, "class", "nested-grid-empty");
        builder.AddContent(seq++, EmptyMessage);
        builder.CloseElement();
    }
    
    builder.CloseElement();
}
```

---

## 5. Usage Example

### 5.1 Basic Master-Detail

```razor
<ComposableGrid TGridItem="Order" Items="@orders" RowKey="o => o.Id" Class="qg-grid">
    <ComposableColumn TGridItem="Order" TValue="int"
                      Property="@(o => o.Id)" Title="Order #" />
    <ComposableColumn TGridItem="Order" TValue="string"
                      Property="@(o => o.CustomerName)" Title="Customer" />
    <ComposableColumn TGridItem="Order" TValue="decimal"
                      Property="@(o => o.Total)" Title="Total" Format="C2" />
    
    <ComposableColumn TGridItem="Order" TValue="int"
                      Property="@(o => o.Id)"
                      Title="Lines"
                      FeatureCollection="@_nestedFeatures" />
</ComposableGrid>

@code {
    private IColumnFeature<Order>[] _nestedFeatures = [
        new NestedGridFeature<Order, OrderLine>
        {
            TriggerMode = RowTriggerMode.Button,
            ExpandButtonText = "View Lines",
            ExpandButtonIcon = "bi bi-list-ul",
            ExpandedRowSpan = 4,
            ChildGridTitle = "Order Lines",
            ChildItems = order => order.Lines.AsQueryable(),
            ChildColumns = @<text>
                <PropertyColumn Property="@((OrderLine l) => l.ProductName)" Title="Product" />
                <PropertyColumn Property="@((OrderLine l) => l.Quantity)" Title="Qty" />
                <PropertyColumn Property="@((OrderLine l) => l.UnitPrice)" Title="Unit Price" Format="C2" />
                <PropertyColumn Property="@((OrderLine l) => l.LineTotal)" Title="Line Total" Format="C2" />
            </text>
        }
    ];
}
```

### 5.2 With Conditional Expansion

```csharp
new NestedGridFeature<Order, OrderLine>
{
    OnBeforeExpand = EventCallback.Factory.Create<RowBeforeExpandEventArgs<Order>>(this, args =>
    {
        // Only expand if order has lines
        if (!args.Item.Lines.Any())
        {
            args.Cancel = true;
        }
    }),
    // ... other parameters
}
```

---

## 6. CSS Styling

```css
.nested-grid-container {
    padding: var(--space-16);
    background: var(--color-surface);
    border: 1px solid var(--color-border-default);
    border-radius: var(--card-radius);
    position: relative;
}

.nested-grid-title {
    margin: 0 0 var(--space-12) 0;
    font-size: var(--font-size-base);
    font-weight: var(--font-weight-semibold);
    color: var(--color-text-primary);
}

.nested-grid-close {
    position: absolute;
    top: var(--space-8);
    right: var(--space-8);
    background: none;
    border: none;
    font-size: 1.5rem;
    cursor: pointer;
    color: var(--color-text-tertiary);
}

.nested-grid-close:hover {
    color: var(--color-text-primary);
}

.nested-grid {
    font-size: var(--font-size-sm);
}

.nested-grid-empty {
    color: var(--color-text-tertiary);
    font-style: italic;
    padding: var(--space-16);
    text-align: center;
}
```

---

## 7. Open Questions

### 7.1 Recursive Nesting

Should nested grids support their own `RowExpandFeature`s for infinite nesting?

```razor
<!-- Orders ? Order Lines ? Line Details? -->
<NestedGridFeature TGridItem="Order" TChildItem="OrderLine">
    <ChildColumns>
        <ComposableColumn TGridItem="OrderLine" TValue="int"
                          Property="@(l => l.Id)"
                          FeatureCollection="@_lineDetailFeatures" />  <!-- Another nested grid? -->
    </ChildColumns>
</NestedGridFeature>
```

**Considerations:**
- Complexity of state management
- Performance with deep nesting
- UX clarity (too many levels = confusing)
- Spacer row calculation becomes complex

**Recommendation:** Support 1 level initially, consider 2 levels max.

### 7.2 Child Grid Type

Should the child grid be:
- Standard `QuickGrid<TChildItem>` (current approach)
- `ComposableGrid<TChildItem>` (full feature support)
- Configurable via parameter

### 7.3 Lazy Loading

Should `ChildItems` support async loading?

```csharp
// Current: Synchronous
Func<TGridItem, IQueryable<TChildItem>> ChildItems

// Alternative: Async
Func<TGridItem, Task<IQueryable<TChildItem>>> ChildItemsAsync
```

### 7.4 Child Grid Actions

Should the child grid support:
- Inline editing (via `InlineEditingFeature`)?
- Row selection?
- Child item CRUD operations?

---

## 8. Dependencies

### 8.1 Base Feature

- `RowExpandFeature<TGridItem>` - All expansion behavior

### 8.2 Grid Components

- `QuickGrid<TChildItem>` or `ComposableGrid<TChildItem>` - Child grid rendering

### 8.3 Infrastructure

- `TypeTraits<T>` - If child columns use composable features
- `Accessors` - Compiled property access

---

## 9. Future Enhancements

| Enhancement | Description |
|-------------|-------------|
| **Lazy Loading** | Async child item loading with loading indicator |
| **Virtualization** | Virtual scrolling for large child collections |
| **Child Selection** | Row selection in child grid |
| **Child CRUD** | Add/Edit/Delete operations on child items |
| **Aggregates** | Summary row in child grid (totals, counts) |
| **Export** | Export child grid data independently |

