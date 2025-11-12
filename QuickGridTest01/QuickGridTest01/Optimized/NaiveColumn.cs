using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Naive column implementation demonstrating common anti-patterns:
/// 1. Poor sequence number management
/// 2. No class caching (string allocation on every render)
/// 3. Excessive DOM elements
/// 4. Reflection-based property access
/// </summary>
public class NaiveColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    // Configuration
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public string? Format { get; set; }
    [Parameter] public Func<TValue, bool>? HighlightCondition { get; set; }
    [Parameter] public Func<TValue, bool>? WarningCondition { get; set; }
    [Parameter] public Func<TValue, bool>? ErrorCondition { get; set; }
    [Parameter] public bool ShowTooltip { get; set; } = false;

    // Anti-pattern #4: Using reflection instead of compiled expressions
    private PropertyInfo? _propertyInfo;
    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    
    // Sorting
    private GridSort<TGridItem>? _sortBuilder;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException(
                $"{nameof(NaiveColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        // Infer title from property if not set
        if (Title is null && Property.Body is MemberExpression memberExpression)
        {
            Title = memberExpression.Member.Name;
        }

        // Anti-pattern #4: Extract PropertyInfo for reflection
        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            
            if (Property.Body is MemberExpression member)
            {
                _propertyInfo = member.Member as PropertyInfo;
            }

            // Build sort
            if (Sortable ?? false)
            {
                _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
            }
        }

        base.OnParametersSet();
    }

    /// <summary>
    /// Anti-pattern demonstration: All four anti-patterns in one method.
    /// </summary>
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_propertyInfo is null)
            return;

        // Anti-pattern #4: Use reflection (slow!)
        var value = (TValue?)_propertyInfo.GetValue(item);
        
        // Anti-pattern #4: Format using reflection every time
        string? formattedValue;
        if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
        {
            formattedValue = formattable.ToString(Format, null);
        }
        else
        {
            formattedValue = value?.ToString();
        }

        // Evaluate conditions
        var isHighlight = value != null && (HighlightCondition?.Invoke(value) ?? false);
        var isWarning = value != null && (WarningCondition?.Invoke(value) ?? false);
        var isError = value != null && (ErrorCondition?.Invoke(value) ?? false);

        // Anti-pattern #1: Inconsistent sequence numbering
        // Using hardcoded numbers creates issues with conditional rendering
        
        // Anti-pattern #3: Excessive DOM - unnecessary wrapper elements
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "naive-cell-outer");
        
            builder.OpenElement(2, "div");
            builder.AddAttribute(3, "class", "naive-cell-middle");
            
                builder.OpenElement(4, "div");
                
                // Anti-pattern #2: Build class string every render (allocations!)
                var cssClass = "naive-cell-inner";
                if (isHighlight) cssClass += " cell-highlight";
                if (isWarning) cssClass += " cell-warning";
                if (isError) cssClass += " cell-error";
                builder.AddAttribute(5, "class", cssClass);
                
                if (ShowTooltip)
                {
                    builder.AddAttribute(6, "title", formattedValue ?? string.Empty);
                }
                
                    // Anti-pattern #3: Yet another unnecessary wrapper
                    builder.OpenElement(7, "span");
                    builder.AddAttribute(8, "class", "cell-content");
                    builder.AddContent(9, formattedValue ?? string.Empty);
                    builder.CloseElement(); // span
                
                builder.CloseElement(); // div inner
            
            builder.CloseElement(); // div middle
        
        builder.CloseElement(); // div outer
        
        // Result: 4 DOM elements per cell instead of 1!
        // Every render allocates new class string!
        // Reflection call on every property access!
        // Hardcoded sequences make conditional rendering fragile!
    }
}
