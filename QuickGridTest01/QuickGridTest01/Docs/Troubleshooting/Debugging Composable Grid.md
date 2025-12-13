# Debugging Composable Grid - Troubleshooting Guide

## Overview

This document catalogs the runtime errors encountered while developing the Composable Grid system for Blazor Server, along with all attempted fixes and their outcomes.

**Status: ? RESOLVED** - Root cause identified and fixed. Grid-integrated filtering now working.

---

## Error #1: Render Tree Corruption

### Error Message
```
System.ArgumentException: Attempting to return wrong pooled instance. Get/Return calls must form a stack.
   at Microsoft.AspNetCore.Components.RenderTree.StackObjectPool`1.Return(T instance)
   at Microsoft.AspNetCore.Components.RenderTree.RenderTreeDiffBuilder.AppendDiffEntriesForRange(...)
   ...
```

### Symptoms
- Error occurs when typing in filter inputs or inline editor inputs
- Circuit disconnects immediately after error
- Browser shows: `"No interop methods are registered for renderer 1"`

---

## ROOT CAUSE IDENTIFIED ?

### The Problem: Incorrect `SetKey()` Placement

In `ComposableColumn.cs`, the `CellContent` method was calling `builder.SetKey()` **before** any element was opened:

```csharp
// ? WRONG - SetKey called before OpenElement
protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
{
    if (RowKey is not null)
    {
        builder.SetKey(RowKey(item));  // ? THIS CORRUPTS THE RENDER TREE!
    }
    // ...
}
```

According to Blazor's `RenderTreeBuilder` documentation:
> **`SetKey()` must be called immediately AFTER `OpenElement()` or `OpenComponent()`**

### The Fix

Remove the incorrect `SetKey()` call:

```csharp
// ? CORRECT - No SetKey before elements
protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
{
    var cellFeatures = GetCellRenderFeatures();

    if (cellFeatures.Count == 0)
    {
        RenderDefaultCell(builder, item);
        return;
    }

    var sequence = 0;
    RenderCellPipeline(builder, ref sequence, item, cellFeatures, 0);
}
```

### File Modified
- `ComposableColumns/Core/ComposableColumn.cs` - Removed incorrect `SetKey()` call

---

## Grid-Integrated Filtering Implementation ?

### Architecture

The filtering system now uses a **grid-integrated approach** where:

1. **Columns declare filtering** via `FilterFeature<TGridItem, TValue>` in `FeatureCollection`
2. **Grid auto-detects filters** and renders a toolbar automatically
3. **Grid applies all active filters** to the source data
4. **No manual filter state management** required in parent components

### Key Components

| Component | Role |
|-----------|------|
| `IGridFilterFeature<TGridItem>` | Interface for grid-integrated filters |
| `FilterFeature<TGridItem, TValue>` | Implements filter logic and renders UI |
| `ComposableGrid` | Detects filters, renders toolbar, applies filters |
| `ComposableColumn` | Registers filter features with grid |

### Usage Example

```razor
<ComposableGrid TGridItem="Product" Items="@_products">
    <ComposableColumn TGridItem="Product" TValue="string"
                      Property="@(p => p.Name)"
                      Title="Name"
                      FeatureCollection="@_nameFilterFeatures" />
</ComposableGrid>

@code {
    // Just add FilterFeature - grid handles the rest!
    private IColumnFeature<Product>[] _nameFilterFeatures = 
        [new FilterFeature<Product, string>()];
}
```

### Key Implementation Details

1. **Filter Registration Timing**: `ComposableColumn` registers filters with grid in `OnParametersSet()` after cascading parameter is available
2. **Re-render Trigger**: `ComposableGrid.RegisterFilter()` calls `StateHasChanged()` to show toolbar
3. **Filter Application**: `ComposableGrid.FilteredItems` property applies all active filters

---

## Test Pages

| Page | Purpose | Status |
|------|---------|--------|
| `/filter-test` | Plain input + QuickGrid baseline | ? Works |
| `/filter-test-2` | FilterInput component + QuickGrid | ? Works |
| `/filter-test-3` | ComposableColumn without features | ? Works |
| `/filter-test-4` | Grid-integrated FilterFeature | ? Works |
| `/composable-demo` | Full demo with all features | ? Works |

---

## Quick Reference: Blazor RenderTreeBuilder Rules

### SetKey Placement

```csharp
// ? CORRECT - SetKey immediately after OpenElement
builder.OpenElement(0, "div");
builder.SetKey(item.Id);

// ? WRONG - SetKey before any element
builder.SetKey(item.Id);  // CRASH!
builder.OpenElement(0, "div");
```

### Best Practices

1. **Use fixed sequence numbers in conditionals**
2. **Always render containers, hide with CSS**
3. **Use `SetKey` for loops with dynamic items**
4. **Use `CancellationTokenSource` for debouncing**

---

*Last Updated: December 13, 2024*