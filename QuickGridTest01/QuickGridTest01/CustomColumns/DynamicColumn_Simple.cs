using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// DynamicColumn_Simple V2 - Uses Func parameter instead of expression building
/// </summary>
public class DynamicColumn_Simple<TGridItem, TValue> : ColumnBase<TGridItem>
{
    [Parameter] public required Func<TGridItem, TValue> GetValue { get; set; }
    [Parameter] public string? Format { get; set; }

    private GridSort<TGridItem>? _sortBy;
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBy;
        set => _sortBy = value;
    }

    protected override void OnInitialized()
    {
        Console.WriteLine($"\n=== DynamicColumn_Simple.OnInitialized (V2) ===");
        Console.WriteLine($"  Title: {Title}");
        base.OnInitialized();
        Console.WriteLine($"=== OnInitialized COMPLETE ===\n");
    }

    [Parameter] public string? PropertyPath { get; set; }

    private Func<TGridItem, TValue>? _accessor;

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(PropertyPath))
        {
            _accessor = BuildSimpleAccessor(PropertyPath);
        }
        base.OnParametersSet();
    }

    private Func<TGridItem, TValue> BuildSimpleAccessor(string path)
    {
        var param = Expression.Parameter(typeof(TGridItem), "item");
        var prop = Expression.Property(param, path);
        var lambda = Expression.Lambda<Func<TGridItem, TValue>>(prop, param);
        return lambda.Compile();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {

        Console.WriteLine($">>> DynamicColumn_Simple.CellContent (V2) - Title: {Title}");

        var value = _accessor != null ? _accessor(item) : GetValue(item);
        Console.WriteLine($"  Value: {value}");

        string formatted;
        if (value is null)
        {
            formatted = string.Empty;
        }
        else if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
        {
            formatted = formattable.ToString(Format, null);
        }
        else
        {
            formatted = value.ToString() ?? string.Empty;
        }

        Console.WriteLine($"  Formatted: '{formatted}'");

        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "style", "border: 2px solid blue; padding: 2px;");
        builder.AddContent(2, formatted);
        builder.CloseElement();

        Console.WriteLine($"  Rendered\n");
    }
}