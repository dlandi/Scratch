using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// MINIMAL TEST COLUMN - Simplest possible custom column to verify basic rendering works
/// This eliminates all the complexity of DynamicColumn to isolate the issue
/// </summary>
public class MinimalTestColumn<TGridItem> : ColumnBase<TGridItem>
{
    [Parameter] public required Func<TGridItem, string> GetValue { get; set; }

    // .NET 9 requirement: SortBy is now abstract and must be implemented
    private GridSort<TGridItem>? _sortBy;
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBy;
        set => _sortBy = value;
    }

    protected override void OnInitialized()
    {
        Console.WriteLine($"MinimalTestColumn.OnInitialized - Title: {Title}");
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine($"MinimalTestColumn.OnParametersSet - Title: {Title}");
        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        Console.WriteLine($"MinimalTestColumn.CellContent called for Title: {Title}");

        try
        {
            var value = GetValue(item);
            Console.WriteLine($"  Value: '{value}'");

            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "style", "background-color: yellow; padding: 4px;");
            builder.AddContent(2, $"TEST: {value}");
            builder.CloseElement();

            Console.WriteLine($"  Rendered successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ERROR: {ex.Message}");
            builder.AddContent(0, $"[ERROR: {ex.Message}]");
        }
    }
}