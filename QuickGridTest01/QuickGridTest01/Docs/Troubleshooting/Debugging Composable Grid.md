# Debugging Composable Grid - Troubleshooting Guide

## Overview

This document catalogs the runtime errors encountered while developing the Composable Grid system for Blazor Server, along with all attempted fixes and their outcomes.

**Status: ? RESOLVED** - Root cause identified and fixed.

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

Calling `SetKey()` at the wrong time corrupts the internal render tree structure, causing the "pooled instance" error when Blazor tries to diff the trees.

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
- `ComposableColumns/Core/ComposableColumn.cs` - Removed incorrect `SetKey()` call from `CellContent()` method

---

## Isolation Testing Process

We used a systematic isolation approach to find the root cause:

| Test | Components Used | Result |
|------|-----------------|--------|
| Filter Test 1 | Plain `<input>` + `QuickGrid` | ? Works |
| Filter Test 2 | `FilterInput.razor` + `QuickGrid` | ? Works |
| Filter Test 3 | `FilterInput.razor` + `ComposableGrid` + `ComposableColumn` (no features) | ? Failed ? ? Fixed |

This process eliminated:
- ? QuickGrid itself
- ? Blazor Server's IQueryable handling
- ? `FilterInput.razor` component
- ? POCO-based features
- ? **Identified `ComposableColumn.CellContent()` as the culprit**

---

## Previous Attempted Fixes (Before Root Cause Found)

These fixes were not the root cause but may still provide value:

### Attempt 1-6: Various Fixes
- Fixed sequence numbers
- CSS hiding for conditional elements
- CancellationTokenSource for debouncing
- Simplified bindings
- Exception handling

**Note:** While not the root cause, some of these are still best practices and remain in the codebase.

---

## Quick Reference: Blazor RenderTreeBuilder Rules

### SetKey Placement

```csharp
// ? CORRECT - SetKey immediately after OpenElement
builder.OpenElement(0, "div");
builder.SetKey(item.Id);
builder.AddAttribute(1, "class", "my-class");
builder.CloseElement();

// ? WRONG - SetKey before any element
builder.SetKey(item.Id);  // CRASH!
builder.OpenElement(0, "div");
```

### Other Best Practices

1. **Use fixed sequence numbers in conditionals**
2. **Always render containers, hide with CSS**
3. **Use `SetKey` for loops with dynamic items**
4. **Use `CancellationTokenSource` for debouncing (not `Timer`)**

---

## Test Pages Created

| Page | Purpose | Status |
|------|---------|--------|
| `/filter-test` | Plain input + QuickGrid | ? Works |
| `/filter-test-2` | FilterInput + QuickGrid | ? Works |
| `/filter-test-3` | FilterInput + ComposableGrid + ComposableColumn | ? Works (after fix) |

---

*Last Updated: December 13, 2024*
*Resolution: Removed incorrect `SetKey()` call in `ComposableColumn.CellContent()`*