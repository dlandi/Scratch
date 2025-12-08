# QuickGrid Implementation Plan: LogViewer Migration

## Overview

This document outlines the plan to migrate the `LogViewer.razor` component from **MudDataGrid** to **QuickGrid** implementation. The goal is to prototype this in the `QuickGridTest01` project using an in-memory data model before integrating into the IPM-Site project.

---

## Phase 1: Data Model Analysis

### Original `Logs` Entity (Infinera.DTN.Logs)

Based on analysis of `LogViewer.razor`, the `Logs` entity contains the following properties:

| Property | Type | Description | Used In Grid |
|----------|------|-------------|--------------|
| `Timestamp` | `DateTime` | Log entry timestamp | ✅ Primary sort column |
| `SourceContext` | `string` | Origin/source of the log (e.g., `IPM_Site.EventLog`) | ✅ Filterable |
| `Level` | `string` | Log level: Error, Warning, Information, Verbose, Debug | ✅ Filterable, Row styling |
| `Message` | `string` | Main log message content | ✅ Truncated display, Filterable |
| `Exception` | `string?` | Exception details (if error) | ✅ Expanded view |
| `Node` | `string?` | Node identifier | Used in message parsing |
| `Template` | `string?` | Message template | Debug only |
| `Properties` | `string?` | Additional properties | Debug only |

### In-Memory Model for QuickGridTest01

```csharp
namespace QuickGridTest01.Models;

/// <summary>
/// In-memory log entry model mirroring IPM's Logs entity for QuickGrid prototyping.
/// </summary>
public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string SourceContext { get; set; } = string.Empty;
    public string Level { get; set; } = "Information";
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? Node { get; set; }
}
```

---

## Phase 2: Feature Mapping (MudDataGrid → QuickGrid)

### Grid Features Comparison

| Feature | MudDataGrid (Current) | QuickGrid (Target) | Notes |
|---------|----------------------|-------------------|-------|
| **Server-side Data** | `ServerData` callback | `ItemsProvider` | Different signature, async |
| **Virtualization** | `Virtualize` attribute | `Virtualize` attribute | ✅ Equivalent |
| **Filtering** | Built-in `Filterable` | Custom filter components | 🔧 Requires custom implementation |
| **Sorting** | Built-in `SortMode` | `Sortable` per column | ✅ Simpler API |
| **Pagination** | `MudDataGridPager` | `Paginator` component | ✅ Equivalent |
| **Hierarchy/Expand** | `HierarchyColumn` + `ChildRowContent` | ❌ Not built-in | 🔧 Custom component needed |
| **Row Styling** | `RowStyleFunc` | `RowClass` parameter | Different approach |
| **Column Resize** | `ColumnResizeMode` | CSS-based | Different approach |
| **Fixed Header** | `FixedHeader` | CSS `position: sticky` | CSS solution |
| **Custom Cell Templates** | `CellTemplate` | `ChildContent` | ✅ Equivalent |

### Critical Gap Analysis

1. **HierarchyColumn (Expandable Rows)**: QuickGrid doesn't have built-in hierarchy. Options:
   - Custom `TemplateColumn` with expand/collapse state
   - Separate detail panel component
   - CSS-based accordion pattern

2. **Advanced Filtering**: MudDataGrid has built-in filter UI. Options:
   - Use existing `FilterableColumn` from QuickGridTest01
   - Build filter toolbar above grid
   - Implement `ItemsProvider` with filter parameters

3. **Nested Grid (Error Details)**: Current implementation shows nested MudDataGrid for errors:
   - Consider collapsible detail row
   - Or separate modal/panel for error context

---

## Phase 3: Implementation Strategy

### Proposed Approach

**Step 1: Create in-memory model in QuickGridTest01**
- Decouples from database dependencies
- Enables rapid iteration and testing
- Can generate realistic sample data

**Step 2: Recreate grid as QuickGrid**
- Prototype in isolation
- Leverage existing QuickGrid components (FilterableColumn, etc.)
- Test virtualization with large datasets

**Step 3: Port back to IPM-Site**
- Replace MudDataGrid with proven QuickGrid implementation
- Reconnect to actual `logContext` database
- Minimal risk due to proven prototype

### Plan Assessment

#### Strengths ✅
1. **Isolation**: Testing in QuickGridTest01 avoids breaking production code
2. **Iterative**: Can refine UI/UX without database dependencies
3. **Reusable**: Components built here can be reused across projects
4. **Existing Assets**: QuickGridTest01 already has `FilterableColumn`, styling, etc.

#### Considerations ⚠️
1. **Hierarchy Complexity**: The expandable row feature is heavily used. Plan time for this.
2. **Message Parsing Logic**: The `GetExpandText`, `ParseMessage`, etc. methods are complex. Consider:
   - Porting these as-is initially
   - Refactoring later for cleaner separation
3. **Performance Testing**: Generate 10,000+ sample logs to validate virtualization
4. **Filter Parity**: MudDataGrid's filter operators (starts with, contains, etc.) need matching

---

## Phase 4: Proposed File Structure

```
QuickGridTest01/
├── Models/
│   └── LogEntry.cs                    # In-memory log model
├── Services/
│   └── LogDataGenerator.cs            # Sample data generation
├── Pages/
│   └── LogViewerDemo.razor            # Main demo page
├── Components/
│   └── LogViewer/
│       ├── LogGrid.razor              # QuickGrid implementation
│       ├── LogDetailPanel.razor       # Expandable detail view
│       ├── LogFilterToolbar.razor     # Filter controls
│       └── LogExporter.cs             # CSV export logic
```

---

## Phase 5: Sample Data Requirements

To properly test the implementation, sample data should include:

1. **Log Levels**: Mix of Error, Warning, Information, Verbose, Debug
2. **Source Contexts**: Various IPM modules (SmartShield, SmartOptimize, etc.)
3. **Message Types**: 
   - Short messages (< 75 chars)
   - Long messages (truncation testing)
   - Messages with special parsing (TL1, JSON, swversion)
4. **Error Entries**: Include Exception text
5. **Volume**: 10,000+ entries for virtualization testing

---

## Phase 6: Implementation Sequence

### Recommended Build Order

| Step | Task | Priority | Estimated Effort |
|------|------|----------|------------------|
| 1 | Create `LogEntry.cs` model | High | 15 min |
| 2 | Create `LogDataGenerator.cs` with realistic sample data | High | 1 hour |
| 3 | Create basic `LogViewerDemo.razor` page with QuickGrid | High | 1 hour |
| 4 | Implement sorting and virtualization | High | 2 hour |
| 5 | Add filtering using FilterableColumn | Medium | 2 hour |
| 6 | Implement expandable row functionality | Medium | 8 hours |
| 7 | Add row styling for log levels | Low | 2 hour |
| 8 | Add CSV export functionality | Low | 1 hour |
| 9 | Test with 10,000+ rows | High | 30 min |
| 10 | Port to IPM-Site | Final | 8 hours |

**Total Estimated Effort**: ~26 hours

---

## Checklist

- [ ] Create `LogEntry.cs` model in QuickGridTest01
- [ ] Create `LogDataGenerator.cs` with realistic sample data
- [ ] Create basic `LogViewerDemo.razor` page with QuickGrid
- [ ] Implement sorting and virtualization
- [ ] Add filtering using FilterableColumn
- [ ] Implement expandable row functionality
- [ ] Add row styling for log levels
- [ ] Add CSV export functionality
- [ ] Test with 10,000+ rows
- [ ] Port to IPM-Site

---

## References

- **Original Source**: `IPM-Site\Pages\Viewer\LogViewer.razor`
- **Entity Model**: `Infinera.DTN.Logs`
- **Target Project**: `QuickGridTest01` (C:\GitHub\QuickGridTest01\QuickGridTest01)
- **QuickGrid Docs**: https://aspnet.github.io/quickgrid/
- **Existing Components**: `FilterableColumn`, `FilterableGrid` in QuickGridTest01

---

## Appendix: Original MudDataGrid Configuration

```razor
<MudDataGrid @ref="logGrid" 
             ServerData="LoadServerData" 
             T="Logs" 
             RowsPerPage="10000" 
             Dense 
             Virtualize 
             SortMode="SortMode.None" 
             Filterable 
             FixedHeader 
             ColumnResizeMode="ResizeMode.Column" 
             Height="585px" 
             ShowMenuIcon="false" 
             RowStyleFunc="@RowRender">
    <Columns>
        <HierarchyColumn T="Logs" ButtonDisabledFunc="@(...)" IconSize="Size.Small" />
        <PropertyColumn Property="x => x.Timestamp" Title="Time Stamp" />
        <PropertyColumn Property="x => x.SourceContext" Title="Source Context" />
        <PropertyColumn Property="x => x.Level" Title="Level" />
        <PropertyColumn Property="x => x.Message" Title="Message">
            <CellTemplate>@((MarkupString)GetNewMessageSpan(logs.Item))</CellTemplate>
        </PropertyColumn>
    </Columns>
    <ChildRowContent>
        <!-- Expandable detail content -->
    </ChildRowContent>
    <PagerContent>
        <MudDataGridPager T="Logs" />
    </PagerContent>
</MudDataGrid>
```

### Key Methods to Port
- `LoadServerData()` - Server-side data loading with filtering/sorting
- `GetNewMessageSpan()` - Message truncation (75 chars)
- `GetExpandText()` - Expanded message formatting
- `RowRender()` - Row styling based on log level
- `ExportEvents()` - CSV export functionality


