# Porting MudBlazor `MudDataGrid` to ASP.NET Core QuickGrid - Notes
===============================

 **Heads‑up about feature parity:** QuickGrid intentionally focuses on core grid scenarios. Advanced behaviors like **hierarchical rows (row detail expanders)** are *not* in scope today, and there’s a long‑standing backlog request to add row expansion. You can still achieve a similar UX with inline toggles/modals, but it won’t be the same as MudBlazor’s `HierarchyColumn`. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/), [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/46356)

***

## 1) One‑for‑one mapping from MudDataGrid → QuickGrid

Below are the main concepts from your MudBlazor grid, with their QuickGrid equivalents and notes.

*   **Server data**
    *   **MudBlazor:** `ServerData="LoadServerData"`
    *   **QuickGrid:** `ItemsProvider="LoadServerData"` — a delegate that receives a `GridItemsProviderRequest<T>` (paging/virtualization start/size + sort) and returns `GridItemsProviderResult<T>` (`Items` + `TotalItemCount`). Use `request.ApplySorting(...)` or `request.GetSortByProperties()` to translate sort rules for your backend. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-10.0), [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.quickgrid.griditemsproviderrequest-1?view=aspnetcore-9.0), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/sorting/)

*   **Hierarchy (expandable detail rows)**
    *   **MudBlazor:** `<HierarchyColumn/>` + `<ChildRowContent>`
    *   **QuickGrid:** **No built‑in** row detail/expander today. Implement alternatives: inline expand/collapse inside a `TemplateColumn`, open a modal, or show details in a panel beneath the grid tied to the selected row. (If true master‑detail rows are required, consider a commercial grid such as DevExpress/Telerik/Syncfusion.) [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/46356), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/), [\[docs.devexpress.com\]](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.DetailRowTemplate), [\[telerik.com\]](https://www.telerik.com/blazor-ui/documentation/components/grid/export/csv), [\[blazor.syn...fusion.com\]](https://blazor.syncfusion.com/documentation/datagrid/detail-template)

*   **Virtualization**
    *   **MudBlazor:** `Virtualize`
    *   **QuickGrid:** `Virtualize="true"` and **fixed row height required** (`ItemSize=...`). Be careful with variable‑height content (e.g., multi‑line messages). Use truncation to keep heights consistent if you virtualize. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-10.0), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/virtualizing/)

*   **Paging**
    *   **MudBlazor:** `<MudDataGridPager/>`
    *   **QuickGrid:** Create a `PaginationState` and add `<Paginator State="...">`. You can also make a custom pager or omit paging if you use virtualization. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/paging/)

*   **Sorting**
    *   **MudBlazor:** built‑in
    *   **QuickGrid:** Enable per column (`Sortable="true"`). When using `ItemsProvider`, read `request` and apply sort on the server. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/sorting/)

*   **Filtering**
    *   **MudBlazor:** built‑in filters
    *   **QuickGrid:** No built‑in filtering API; implement your own filter UI (often in `<ColumnOptions>`) and apply filters to `IQueryable` or pass filters through `ItemsProvider` to the backend. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/filtering/)

*   **Row styling (`RowRender`)**
    *   **MudBlazor:** `RowStyleFunc`
    *   **QuickGrid:** No row‑level callback. Use CSS and/or render conditional classes inside `TemplateColumn`s (per cell). Full row background by condition is limited; you can do global `tr` styling or experimental CSS selectors (e.g., `:has()`), but there’s no official per‑row class hook yet. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/styling/), [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/answers/questions/1653107/how-to-set-quickgrid-row-colour-in-blazor-webassem), [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/45657)

*   **Column resizing / fixed header**
    *   **MudBlazor:** `ColumnResizeMode="ResizeMode.Column"` and `FixedHeader`
    *   **QuickGrid:** Column widths and sticky headers are done via CSS. Column resize handlers aren’t part of QuickGrid’s goals; stick to CSS sizing. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/styling/), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/)

*   **CSV export**
    *   **MudBlazor:** you wrote `ExportEvents()`
    *   **QuickGrid:** No built‑in export; implement via a server endpoint or JS interop to download CSV. (There’s an open request to add export to QuickGrid.) [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/43499)

***

## 2) A working QuickGrid port (Logs)

> The sample below wires:
>
> *   server‑side data via `ItemsProvider`;
> *   truncation (`GetNewMessageSpan` → `Truncate`) and inline expand (`GetExpandText`);
> *   conditional visual cue for log level;
> *   paging with `<Paginator>`;
> *   optional virtualization (commented out if message text varies);
> *   **CSV export** via a minimal API.

### Component: `LogsGrid.razor`

```razor
@using Microsoft.AspNetCore.Components.QuickGrid
@inject NavigationManager Nav

<QuickGrid TGridItem="Logs"
           @ref="_grid"
           ItemsProvider="LoadServerData"
           Class="table table-striped table-hover"
           ItemKey="@(log => log.Id)"
           Theme="default"
           /* Virtualize="true" ItemSize="28"  <-- only if row height is fixed */>

    <!-- Timestamp -->
    <PropertyColumn TGridItem="Logs" Property="@(x => x.Timestamp)" Title="Time Stamp" Sortable="true" />

    <!-- SourceContext -->
    <PropertyColumn TGridItem="Logs" Property="@(x => x.SourceContext)" Title="Source Context" Sortable="true">
        <ColumnOptions>
            <!-- example filter UI placed in the column options popup -->
            <div class="p-2">
                <input type="search" class="form-control form-control-sm"
                       @bind="_sourceContextFilter" placeholder="Filter by source..." />
            </div>
        </ColumnOptions>
    </PropertyColumn>

    <!-- Level (with inline badge color) -->
    <TemplateColumn TGridItem="Logs" Title="Level" Sortable="true" SortBy="@(log => log.Level)">
        <ChildContent Context="log">
            <span class="level-badge @GetLevelClass(log.Level)">@log.Level</span>
        </ChildContent>
    </TemplateColumn>

    <!-- Message with truncation + inline expand -->
    <TemplateColumn TGridItem="Logs" Title="Message">
        <ChildContent Context="log">
            <div class="message-cell">
                @if (!_expandedRowKeys.Contains(log.Id))
                {
                    <span>@Truncate(log.Message, 75)</span>
                    <button class="btn btn-link btn-sm"
                            @onclick="() => ToggleExpand(log.Id)">
                        More
                    </button>
                }
                else
                {
                    <pre class="expanded-text">@GetExpandText(log)</pre>
                    <button class="btn btn-link btn-sm"
                            @onclick="() => ToggleExpand(log.Id)">
                        Less
                    </button>
                }
            </div>
        </ChildContent>

        <!-- Optional per-column options to filter by message substring -->
        <ColumnOptions>
            <div class="p-2">
                <input type="search" class="form-control form-control-sm"
                       @bind="_messageFilter" placeholder="Filter message..." />
            </div>
        </ColumnOptions>
    </TemplateColumn>
</QuickGrid>

<!-- Built-in pagination UI -->
<Paginator State="@_pagination" />

<!-- Toolbar -->
<div class="mt-2 d-flex gap-2">
    <button class="btn btn-outline-secondary btn-sm" @onclick="ExportEvents">
        Export CSV
    </button>
    <button class="btn btn-outline-secondary btn-sm" @onclick="ClearFilters">
        Clear Filters
    </button>
</div>

@code {
    private QuickGrid<Logs>? _grid;
    private PaginationState _pagination = new() { ItemsPerPage = 50 };
    private HashSet<string> _expandedRowKeys = new();

    // Filters bound in ColumnOptions
    private string? _sourceContextFilter;
    private string? _messageFilter;

    // ItemsProvider = server data loader with paging/sorting
    private async ValueTask<GridItemsProviderResult<Logs>> LoadServerData(GridItemsProviderRequest<Logs> request)
    {
        // Translate sort to backend parameters (property names + direction)
        var sortPairs = request.GetSortByProperties(); // name + dir
        // Build query DTO for server:
        var query = new LogsQuery
        {
            Skip = request.StartIndex,
            Take = request.Count ?? _pagination.ItemsPerPage,
            SourceContextContains = _sourceContextFilter,
            MessageContains = _messageFilter,
            Sort = sortPairs.Select(p => new SortPair(p.PropertyName, p.Direction == SortDirection.Ascending)).ToList()
        };

        // Call your backend (Db/Minimal API) to fetch items + total count
        var result = await LogsDataService.QueryAsync(query);

        // Update paginator total count
        await _pagination.SetTotalItemCountAsync(result.TotalItemCount);

        return new GridItemsProviderResult<Logs>(result.Items, result.TotalItemCount);
    }

    private void ToggleExpand(string id)
    {
        if (!_expandedRowKeys.Add(id))
            _expandedRowKeys.Remove(id);
    }

    // Message truncation (formerly GetNewMessageSpan)
    private static string Truncate(string? text, int max)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        return text.Length <= max ? text : text.Substring(0, max) + "…";
    }

    // Expanded text formatting (formerly GetExpandText)
    private static string GetExpandText(Logs log)
        => $"{log.Message}\n\n[{log.Level}] {log.Timestamp:u} {log.SourceContext}";

    private string GetLevelClass(string level) => level switch
    {
        "Error" or "Fatal" => "level-error",
        "Warn"            => "level-warn",
        _                 => "level-info"
    };

    private void ClearFilters()
    {
        _sourceContextFilter = null;
        _messageFilter = null;
        _ = _grid?.RefreshDataAsync();
    }

    private void ExportEvents()
    {
        // Navigate to a server endpoint that returns CSV for current filters/sort
        var url = $"/api/logs/export?source={Uri.EscapeDataString(_sourceContextFilter ?? "")}" +
                  $"&message={Uri.EscapeDataString(_messageFilter ?? "")}";
        Nav.NavigateTo(url, forceLoad: true);
    }
}
```

> **Why `ItemsProvider`?** This is the recommended way to do server‑side paging/sorting, and it exposes the same concepts you had in MudBlazor’s `ServerData`. Use `GridItemsProviderRequest<T>` to read paging/sort and return `GridItemsProviderResult<T>` with **total count** so the paginator/virtualizer knows how much data exists. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-10.0), [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.quickgrid.griditemsproviderrequest-1?view=aspnetcore-9.0)

### CSS (row cues, sticky header, widths)

```css
/* LogsGrid.razor.css - scoped */
::deep thead {
  position: sticky; top: 0; background: #f9fafb; z-index: 1;
}

/* cell-level badges for log level */
::deep .level-badge.level-error { color: #b00020; font-weight: 600; }
::deep .level-badge.level-warn  { color: #b36b00; font-weight: 600; }
::deep .level-badge.level-info  { color: #0b5ed7; }

/* message cell—prevent wrapping for virtualization; allow wrapping if you disable it */
::deep .message-cell { white-space: nowrap; max-width: 520px; overflow: hidden; text-overflow: ellipsis; }
::deep .expanded-text { white-space: pre-wrap; }

/* example: set column widths via header/column classes if needed */
```

> Styling in QuickGrid is largely done by CSS; you can make headers sticky, set widths, etc., with scoped styles or themes. There’s no built‑in column resize like MudBlazor—this is by design. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/styling/), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/)

***

## 3) Implementing your key methods in QuickGrid terms

### A) `LoadServerData()` → `ItemsProvider`

Use `GridItemsProviderRequest<T>` to read `StartIndex`, `Count`, and sort info. Inside, pass filter values to your backend and **return total**. You can obtain sort pairs with `request.GetSortByProperties()` or apply sorting to `IQueryable` with `request.ApplySorting(queryable)`. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.quickgrid.griditemsproviderrequest-1?view=aspnetcore-9.0), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/sorting/)

### B) `GetNewMessageSpan()` (truncate 75 chars)

Keep it as a helper (`Truncate(string, 75)`) and render in a `TemplateColumn`. If you virtualize, truncation helps keep row height constant. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-10.0), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/virtualizing/)

### C) `GetExpandText()` (expanded formatting)

Render inside the same `TemplateColumn` when expanded. Since QuickGrid lacks `ChildRowContent`, you can either:

*   render expanded text **inline** in the cell (as shown),
*   open a **modal**, or
*   navigate to a **detail** view.

(Commercial grids have native detail templates if you want that UX out‑of‑the‑box.) [\[docs.devexpress.com\]](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.DetailRowTemplate), [\[telerik.com\]](https://www.telerik.com/blazor-ui/documentation/components/grid/export/csv), [\[blazor.syn...fusion.com\]](https://blazor.syncfusion.com/documentation/datagrid/detail-template)

### D) `RowRender()` (row styling by log level)

QuickGrid doesn’t provide a per‑row style callback, so use **CSS + per‑cell badges**, or apply conditional markup in your `TemplateColumn`. Global `tr` styling is possible, but conditional row styling per item is limited and typically done via per‑cell content/styles. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/styling/), [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/answers/questions/1653107/how-to-set-quickgrid-row-colour-in-blazor-webassem)

### E) `ExportEvents()` (CSV)

QuickGrid has **no built‑in export**, so create a **server endpoint** that returns a CSV built from the current filters/sort. Trigger navigation to that endpoint to download (as shown in `ExportEvents()`). See the GitHub feature discussion and community articles for QuickGrid + CSV patterns. [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/43499), [\[c-sharpcorner.com\]](https://www.c-sharpcorner.com/article/blazor-and-net-8-quickgrid-implementation-with-csv-data-export/)

**Minimal API example (Program.cs):**

```csharp
app.MapGet("/api/logs/export", async (HttpContext ctx, ILogsRepository repo) =>
{
    var source = ctx.Request.Query["source"].ToString();
    var message = ctx.Request.Query["message"].ToString();

    var logs = await repo.FilterAsync(source, message);
    ctx.Response.ContentType = "text/csv";
    ctx.Response.Headers.Append("Content-Disposition", "attachment; filename=logs.csv");

    await using var writer = new StreamWriter(ctx.Response.Body);
    await writer.WriteLineAsync("Timestamp,SourceContext,Level,Message");
    foreach (var l in logs)
    {
        var msg = l.Message?.Replace("\"", "\"\"") ?? "";
        await writer.WriteLineAsync($"{l.Timestamp:u},{Quote(l.SourceContext)},{l.Level},{Quote(msg)}");
    }

    static string Quote(string? s) => $"\"{(s ?? "").Replace("\"", "\"\"")}\"";
});
```

> Many developers use a simple CSV writer or `CsvHelper`, then return `File(...)` from a controller or minimal API. The download pattern is standard in Blazor—navigate to an endpoint that streams the file. [\[stackoverflow.com\]](https://stackoverflow.com/questions/74532660/asp-net-blazor-server-how-to-export-csv-file-csv-created-by-csvhelper)

***

## 4) Optional: Pagination vs. Virtualization

QuickGrid offers both. Use **paging** via `PaginationState` and `<Paginator/>` for variable‑height content (expanded messages). Use **virtualization** only if **every row has fixed height**, otherwise scroll jitter can occur. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/paging/), [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/virtualizing/)

***

## 5) Filtering UX

QuickGrid deliberately leaves filtering to your UI. Place inputs in `<ColumnOptions>` popovers or above the grid, bind to component fields, and apply them in your backend via `ItemsProvider` or locally via `IQueryable.Where(...)`. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/filtering/)

***

## 6) Server‑side pattern (ItemsProvider request → backend)

If you need **true server‑side paging/sorting/filtering**, wire your `ItemsProvider` to pass `Skip`, `Take`, **SortBy** (property + direction), and your filter values to your API, and **return `{ Items, TotalItemCount }`**. This is the recommended pattern for remote data with QuickGrid. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.quickgrid.griditemsproviderrequest-1?view=aspnetcore-9.0), [\[mzansibytes.com\]](https://mzansibytes.com/2023/10/10/blazor-quickgrid-with-webassembly-and-api-backend-complete-example-part-3/)

***

## 7) Where QuickGrid differs most from MudBlazor

*   **No row‑detail / hierarchy column:** you’ll emulate expansion via cell templates or modals. (Backlog exists, but not part of current releases.) [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/46356)
*   **Styling is CSS‑first:** sticky headers, widths, zebra stripes, etc., are done via scoped CSS or themes. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/styling/)
*   **Advanced features (column drag‑reorder, hierarchical rows):** intentionally out of scope. [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/)
*   **Export:** roll your own (CSV/PDF) via endpoints or client‑side JS. [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/43499)

***

## 8) Suggested data model

```csharp
public sealed class Logs
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // for ItemKey
    public DateTime Timestamp { get; set; }
    public string SourceContext { get; set; } = "";
    public string Level { get; set; } = "";
    public string Message { get; set; } = "";
}
```

> Bind `ItemKey` to a unique ID so QuickGrid can maintain row identity across reloads. [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-10.0)

***

## 9) If you truly need row detail

If your UX requires **expandable detail rows** across all columns, consider a grid with built‑in **DetailRowTemplate** (e.g., DevExpress `DxGrid`, Telerik Grid, Syncfusion Grid). They implement master‑detail and export natively. [\[docs.devexpress.com\]](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.DetailRowTemplate), [\[telerik.com\]](https://www.telerik.com/blazor-ui/documentation/components/grid/export/csv), [\[blazor.syn...fusion.com\]](https://blazor.syncfusion.com/documentation/datagrid/detail-template)

***

### Quick checklist before you implement

*   Will you use **paging** instead of virtualization (because expanded messages vary height)? [\[aspnet.github.io\]](https://aspnet.github.io/quickgridsamples/virtualizing/)
*   What’s your **hosting model** (Server/WASM)? (Affects download strategy for CSV.) [\[stackoverflow.com\]](https://stackoverflow.com/questions/74532660/asp-net-blazor-server-how-to-export-csv-file-csv-created-by-csvhelper)
*   Do you need **true row‑detail** or is **inline cell expansion**/modal acceptable? [\[github.com\]](https://github.com/dotnet/aspnetcore/issues/46356)
*   How large can the dataset get and what are sort/filter requirements server‑side? [\[learn.microsoft.com\]](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.quickgrid.griditemsproviderrequest-1?view=aspnetcore-9.0)

If you share your backend shape for `LoadServerData` (DTOs and endpoint signatures) and confirm whether inline expansion is OK, I’ll tailor the `ItemsProvider` + API code to your exact filters and add an **EF Core** example with `ApplySorting`/`Where` for log level and text filters.
