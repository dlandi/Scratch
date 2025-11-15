using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Web.Virtualization; // Added for Virtualize<T>
using System.Linq;
using System.Collections.Generic;

namespace QuickGridTest01.Filterable;

/// <summary>
/// Wrapper for QuickGrid that coordinates filtering across multiple filterable columns.
/// </summary>
public partial class FilterableGrid<TGridItem> : ComponentBase
{
    [Parameter] public IQueryable<TGridItem>? Items { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public Virtualize<TGridItem>? Virtualize { get; set; }
    
    private readonly List<FilterableColumnBase<TGridItem>> _filterableColumns = new();
    private IQueryable<TGridItem>? _filteredItems;

    // NEW: Expose columns for external filter toolbars
    public IReadOnlyList<FilterableColumnBase<TGridItem>> Columns => _filterableColumns;

    protected override void OnInitialized()
    {
        RefreshFilteredItems();
    }

    protected override void OnParametersSet()
    {
        RefreshFilteredItems();
    }

    /// <summary>
    /// Registers a filterable column with the grid.
    /// </summary>
    internal void RegisterColumn(FilterableColumnBase<TGridItem> column)
    {
        if (!_filterableColumns.Contains(column))
        {
            _filterableColumns.Add(column);
        }
    }

    /// <summary>
    /// Called when any column's filter changes.
    /// </summary>
    internal async Task OnFilterChangedAsync()
    {
        RefreshFilteredItems();
        await InvokeAsync(StateHasChanged);
    }

    private void RefreshFilteredItems()
    {
        if (Items is null)
        {
            _filteredItems = null;
            return;
        }

        // Start with original items
        var filtered = Items;

        // Apply each active filter
        foreach (var column in _filterableColumns.Where(c => c.HasActiveFilter))
        {
            filtered = column.ApplyFilter(filtered);
        }

        _filteredItems = filtered;
    }

    /// <summary>
    /// Clears all filters in the grid.
    /// </summary>
    public async Task ClearAllFiltersAsync()
    {
        foreach (var column in _filterableColumns)
        {
            await column.ClearFilterAsync();
        }
        
        RefreshFilteredItems();
        await InvokeAsync(StateHasChanged);
    }
}
