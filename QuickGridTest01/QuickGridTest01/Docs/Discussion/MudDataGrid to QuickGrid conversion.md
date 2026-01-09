# MudDataGrid to QuickGrid Conversion

This document analyzes pages currently using MudDataGrid to assess their suitability for replacement with ASP.NET Core QuickGrid, considering available QuickGrid extensions.

---

## QuickGrid Extension Status

> **Note:** The "ComposableColumn Extension" column reflects features implemented in the `QuickGridTest01.ComposableColumns` namespace.

| Feature | MudDataGrid | QuickGrid (Base) | ComposableColumn Extension | Status |
|---------|-------------|------------------|----------------------------|--------|
| Basic data display | ✅ | ✅ | ✅ `ComposableColumn<T,V>` | Available |
| Sorting | ✅ | ✅ | ✅ via `SortBy` | Available |
| Filtering | ✅ | ✅ | ✅ `FilterFeature<T,V>` | **Implemented** |
| Pagination | ✅ | ✅ | ✅ via QuickGrid | Available |
| Virtualization | ✅ | ✅ | ✅ via QuickGrid | Available |
| Template columns | ✅ | ✅ | ✅ via Features pipeline | Available |
| Server-side data | ✅ | ✅ | ✅ via `ItemsProvider` | Available |
| **Cell editing** | ✅ | ❌ | ✅ `InlineEditingFeature<T,V>` | **Implemented** |
| **Hierarchy/Child rows** | ✅ | ❌ | ✅ `RowExpandFeature<T>` | **Implemented** |
| **Formatting** | ✅ | Limited | ✅ `FormattingFeatures` | **Implemented** |
| **Styling/Tooltips** | ✅ | Limited | ✅ `TooltipFeature<T,V>`, `StylingFeatures` | **Implemented** |
| **DataAnnotations validation** | ✅ | ❌ | ✅ `UseDataAnnotations` in editing | **Implemented** |
| **Custom validators** | ✅ | ❌ | ✅ `IValidator<T>` pipeline | **Implemented** |
| Row grouping | ✅ | ❌ | ⏳ | **Planned** |
| Multi-selection | ✅ | ❌ | ⏳ | **Planned** |
| Custom cell styling | ✅ | Limited | ✅ CSS class binding | Available |
| Context menus | ✅ | ❌ | ✅ Use TemplateColumn/Features | Available |

### ComposableColumn Feature Architecture

The extension is built on a composable feature pipeline (`IColumnFeature<T>`, `ICellRenderFeature<T>`) with priority-based execution:

| Priority | Category | Features |
|----------|----------|----------|
| 0 | Infrastructure | Property expression, compiled accessor |
| 100 | Core | Type traits, auto-title inference |
| 150 | Filtering | `FilterFeature<T,V>` with operators, debounce |
| 200 | Formatting | Format strings, custom formatters, culture |
| 300 | Styling | Conditional CSS, icons, `TooltipFeature<T,V>` |
| 350 | Expansion | `RowExpandFeature<T>` with spacer rows |
| 400 | Editing | `InlineEditingFeature<T,V>` with validation |

---

## MudDataGrid Usage Analysis

| Page | Grid Count | Features Used | Blocking Features | QuickGrid Candidate |
|------|------------|---------------|-------------------|---------------------|
| `Index.razor` | 1 | GroupBy, TemplateColumn, CellStyleFunc | **Row grouping** | ⏳ After grouping impl |
| `NetworkSetup.razor` | 6+ | HierarchyColumn, EditMode.Cell, nested grids | None (extensions exist) | ✅ Yes |
| `LogViewer.razor` | 2 | ServerData, HierarchyColumn, nested grids | None (extensions exist) | ✅ Yes |
| `Charts.razor` | 1 | EditMode.Cell, RowClassFunc, drag/drop | None (extensions exist) | ✅ Yes |
| `Soak.razor` | 1 | MultiSelection, Grouping | **Row grouping, Multi-selection** | ⏳ After both impl |
| `Deploy.razor` | 1 | EditMode.Cell, EditTemplate | None (extensions exist) | ✅ Yes |
| `Predict.razor` | 2 | Basic PropertyColumn | None | ✅ Yes |
| `Optimize.razor` | 2 | EditMode.Cell, context menus | None (extensions exist) | ✅ Yes |
| `SmartShield.razor` | 2 | EditMode.Cell, CommittedItemChanges | None (extensions exist) | ✅ Yes |
| `SmartCable.razor` | 1 | Basic with TemplateColumn | None | ✅ Yes |

---

## Migration Priority

### Phase 1: Ready Now (8 pages) ✅

Pages that can migrate with existing QuickGrid + extensions:

| Page | Grid(s) | Effort | Notes |
|------|---------|--------|-------|
| `Predict.razor` | 2 | Low | Simple read-only grids |
| `SmartCable.razor` | 1 | Low | Basic display with templates |
| `Charts.razor` | 1 | Medium | Uses cell editing extension |
| `Deploy.razor` | 1 | Medium | Cell editing with dropdowns |
| `Optimize.razor` | 2 | Medium | Cell editing, context via TemplateColumn |
| `SmartShield.razor` | 2 | Medium | Cell editing with validation |
| `NetworkSetup.razor` | 6+ | High | Multiple grids, hierarchy extension |
| `LogViewer.razor` | 2 | High | Server data, hierarchy, nested grids |

### Phase 2: After Row Grouping Implementation (1 page) ⏳

| Page | Blocking Feature | Notes |
|------|------------------|-------|
| `Index.razor` | Row grouping | Main home page, complex GroupTemplate |

### Phase 3: After Multi-Selection + Grouping (1 page) ⏳

| Page | Blocking Features | Notes |
|------|-------------------|-------|
| `Soak.razor` | Row grouping + Multi-selection | Both features required |

---

## Detailed Analysis by Page

### ✅ Predict.razor - READY NOW (Priority: High)

**Current features:**
- Basic data display grids
- Simple property columns
- No editing, no grouping, no hierarchy

**Migration effort:** Low
**Benefits:** Native ASP.NET Core, lighter weight, best starting point

---

### ✅ SmartCable.razor - READY NOW (Priority: High)

**Current features:**
- Basic grid with TemplateColumn
- Read-only display

**Migration effort:** Low
**Benefits:** Simple migration, good second candidate

---

### ✅ Charts.razor - READY NOW (Priority: Medium)

**Current features:**
- `SelectedItemChanged` for row selection
- `EditMode.Cell` for comments editing → **Use cell editing extension**
- `RowClassFunc` for selection highlighting → **Use CSS classes**
- Drag/drop support via custom template
- Delete button in template column

**Migration effort:** Medium
**Extension required:** Cell editing

---

### ✅ Deploy.razor - READY NOW (Priority: Medium)

**Current features:**
- `EditMode.Cell` with `EditTemplate` → **Use cell editing extension**
- Complex dropdown selection in edit mode
- Dynamic card availability logic

**Migration effort:** Medium
**Extension required:** Cell editing

```razor
<!-- MudDataGrid -->
<MudDataGrid EditMode="DataGridEditMode.Cell">
    <PropertyColumn>
        <EditTemplate>
            <MudSelect @bind-Value="context.Item.SourceCard">...</MudSelect>
        </EditTemplate>
    </PropertyColumn>
</MudDataGrid>

<!-- QuickGrid with extension -->
<QuickGrid Items="@items.AsQueryable()">
    <EditableColumn Property="x => x.SourceCard">
        <EditTemplate>
            <select @bind="context.SourceCard">...</select>
        </EditTemplate>
    </EditableColumn>
</QuickGrid>
```

---

### ✅ Optimize.razor - READY NOW (Priority: Medium)

**Current features:**
- `EditMode.Cell` for coordinate editing → **Use cell editing extension**
- Context menus via `MudMenu` → **Use TemplateColumn with custom menu**
- `@onkeypress` handling
- Complex edit templates with numeric fields

**Migration effort:** Medium
**Extension required:** Cell editing
**Note:** Context menus can be implemented via TemplateColumn with a custom dropdown/popover

---

### ✅ SmartShield.razor - READY NOW (Priority: Medium)

**Current features:**
- `EditMode.Cell` with `CommittedItemChanges` → **Use cell editing extension**
- Context menus for crossconnect operations → **Use TemplateColumn**
- Complex edit templates with validation
- `SelectedItemChanged` for selection

**Migration effort:** Medium
**Extension required:** Cell editing

---

### ✅ NetworkSetup.razor - READY NOW (Priority: Low - Complex)

**Current features:**
- Multiple nested grids (Nodes → Degrees, Links → Carriers) → **Use hierarchy extension**
- `HierarchyColumn` with `ButtonDisabledFunc` → **Use hierarchy extension**
- `ChildRowContent` for expandable rows → **Use hierarchy extension**
- Inline cell editing → **Use cell editing extension**
- `EditTemplate` with custom inputs

**Migration effort:** High (6+ grids)
**Extensions required:** Hierarchy, Cell editing

```razor
<!-- MudDataGrid -->
<MudDataGrid EditMode="DataGridEditMode.Cell">
    <HierarchyColumn ButtonDisabledFunc="@(x => ...)" />
    <ChildRowContent>
        <MudDataGrid Items="@node.Item.Degrees" />
    </ChildRowContent>
</MudDataGrid>

<!-- QuickGrid with hierarchy extension -->
<QuickGrid Items="@nodes.AsQueryable()">
    <HierarchyColumn CanExpand="@(x => x.Degrees.Any())">
        <ChildContent>
            <QuickGrid Items="@context.Degrees.AsQueryable()">...</QuickGrid>
        </ChildContent>
    </HierarchyColumn>
</QuickGrid>
```

---

### ✅ LogViewer.razor - READY NOW (Priority: Low - Complex)

**Current features:**
- Server-side data loading with `ServerData` → **QuickGrid supports ItemsProvider**
- Hierarchical rows → **Use hierarchy extension**
- Nested grids in `ChildRowContent` → **Use hierarchy extension**
- Custom `RowStyleFunc` → **Use CSS class binding**
- Pager integration → **Use Paginator component**

**Migration effort:** High
**Extensions required:** Hierarchy

---

### ⏳ Index.razor - WAITING ON ROW GROUPING

**Blocking feature:** Row grouping with `GroupBy` and `GroupTemplate`

**Current features:**
- Row grouping with `GroupBy` and `GroupTemplate` → **⏳ Extension needed**
- Complex `CellStyleFunc` for dynamic styling → **Use CSS class binding**
- Rich `TemplateColumn` with MudBadge, MudProgressLinear
- Custom tooltip integration
- SVG rendering in cells

**Migration effort:** Medium (after grouping extension)
**Extension required:** Row grouping

```razor
<!-- MudDataGrid -->
<MudDataGrid Items="displayedLinks" GroupExpanded>
    <PropertyColumn Groupable="true" Grouping GroupBy="@groupBy">
        <GroupTemplate>
            <span>@context.Grouping.Key</span>
        </GroupTemplate>
    </PropertyColumn>
</MudDataGrid>

<!-- QuickGrid with grouping extension (planned) -->
<QuickGrid Items="@links.AsQueryable()" GroupBy="@(x => x.LinkName)">
    <GroupHeader>
        <span>@context.Key</span>
    </GroupHeader>
    <PropertyColumn Property="x => x.FiberPair" />
</QuickGrid>
```

---

### ⏳ Soak.razor - WAITING ON GROUPING + MULTI-SELECT

**Blocking features:** 
- Row grouping with `GroupBy` and `GroupTemplate` → **⏳ Extension needed**
- `MultiSelection` with `SelectedItems` binding → **⏳ Extension needed**

**Current features:**
- `MultiSelection` with `SelectedItems` binding
- Row grouping with `GroupBy` and `GroupTemplate`
- `SelectedItemsChanged` callback
- `SelectColumn` for checkboxes

**Migration effort:** Medium (after both extensions)
**Extensions required:** Row grouping, Multi-selection

```razor
<!-- MudDataGrid -->
<MudDataGrid MultiSelection SelectedItems="selectedSoaks" SelectedItemsChanged="@SoaksChanged">
    <SelectColumn />
    <PropertyColumn Groupable="true" Grouping="true" GroupBy="x => x.Description">
        <GroupTemplate>...</GroupTemplate>
    </PropertyColumn>
</MudDataGrid>

<!-- QuickGrid with extensions (planned) -->
<QuickGrid Items="@soaks.AsQueryable()" 
           GroupBy="@(x => x.Description)"
           SelectionMode="SelectionMode.Multiple"
           @bind-SelectedItems="selectedSoaks">
    <SelectColumn />
    <PropertyColumn Property="x => x.Node" />
</QuickGrid>
```

---

## Migration Roadmap

```
Phase 1 (Now)                    Phase 2                      Phase 3
─────────────────────────────────────────────────────────────────────────
✅ Predict.razor                 ⏳ Index.razor               ⏳ Soak.razor
✅ SmartCable.razor                 (after row grouping)        (after grouping
✅ Charts.razor                                                  + multi-select)
✅ Deploy.razor
✅ Optimize.razor
✅ SmartShield.razor
✅ NetworkSetup.razor
✅ LogViewer.razor
─────────────────────────────────────────────────────────────────────────
     8 pages ready                    1 page                      1 page
```

---

## QuickGrid Migration Checklist

1. **Add package reference:**
   ```xml
   <PackageReference Include="Microsoft.AspNetCore.Components.QuickGrid" Version="8.0.*" />
   ```

2. **Add extension packages (as needed):**
   ```xml
   <!-- Cell editing extension -->
   <PackageReference Include="QuickGrid.Extensions.Editing" Version="x.x.x" />
   <!-- Hierarchy extension -->
   <PackageReference Include="QuickGrid.Extensions.Hierarchy" Version="x.x.x" />
   ```

3. **Update imports:**
   ```razor
   @using Microsoft.AspNetCore.Components.QuickGrid
   ```

4. **Convert grid syntax:**
   ```razor
   <!-- Before (MudDataGrid) -->
   <MudDataGrid Items="@items" Dense Bordered>
       <PropertyColumn Property="x => x.Name" Title="Name" />
   </MudDataGrid>

   <!-- After (QuickGrid) -->
   <QuickGrid Items="@items.AsQueryable()" Class="table-dense table-bordered">
       <PropertyColumn Property="x => x.Name" Title="Name" />
   </QuickGrid>
   ```

5. **Handle pagination:**
   ```razor
   <QuickGrid Items="@items.AsQueryable()" Pagination="@pagination">
       ...
   </QuickGrid>
   <Paginator State="@pagination" />

   @code {
       PaginationState pagination = new() { ItemsPerPage = 10 };
   }
   ```

6. **Replace MudBlazor styling with CSS:**
   - Create QuickGrid-specific CSS classes
   - Map `CellStyle`/`HeaderStyle` to CSS classes
   - Use `Class` parameter instead of inline styles

7. **Convert cell editing (with extension):**
   ```razor
   <!-- Before -->
   <PropertyColumn>
       <EditTemplate>
           <MudTextField @bind-Value="context.Item.Name" />
       </EditTemplate>
   </PropertyColumn>

   <!-- After -->
   <EditableColumn Property="x => x.Name" />
   ```

8. **Convert hierarchy (with extension):**
   ```razor
   <!-- Before -->
   <HierarchyColumn />
   <ChildRowContent>@context.Item.Details</ChildRowContent>

   <!-- After -->
   <HierarchyColumn>
       <ChildContent>@context.Details</ChildContent>
   </HierarchyColumn>
   ```

---

*Document extracted from ClientCodeTree.md for focused migration planning*
