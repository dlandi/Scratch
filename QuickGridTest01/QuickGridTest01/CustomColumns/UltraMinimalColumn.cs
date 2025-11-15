using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// ULTRA-MINIMAL: Test if two type parameters cause issues
/// This is as simple as MinimalTestColumn but with TValue added
/// </summary>
public class UltraMinimalColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    [Parameter] public required Func<TGridItem, TValue> GetValue { get; set; }

    private GridSort<TGridItem>? _sortBy;
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBy;
        set => _sortBy = value;
    }

    protected override void OnInitialized()
    {
        Console.WriteLine($"UltraMinimalColumn.OnInitialized - Title: {Title}");
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine($"UltraMinimalColumn.OnParametersSet - Title: {Title}");
        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        Console.WriteLine($"UltraMinimalColumn.CellContent called for Title: {Title}");
        
        var value = GetValue(item);
        var text = value?.ToString() ?? "";
        
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "style", "background-color: lime; padding: 4px;");
        builder.AddContent(2, $"ULTRA: {text}");
        builder.CloseElement();
    }
}
