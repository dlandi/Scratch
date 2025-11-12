using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Column optimized for virtual scrolling scenarios.
/// Key optimizations:
/// - Lightweight rendering (minimal overhead per cell)
/// - No per-item state tracking
/// - Efficient compiled accessors
/// - Pre-computed formatting
/// </summary>
public class VirtualColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    // Configuration
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public string? Format { get; set; }
    [Parameter] public Func<TValue, string>? CustomFormatter { get; set; }
    [Parameter] public string? CssClass { get; set; }

    // Compiled accessors (critical for virtualization performance)
    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledAccessor;
    private Func<TGridItem, string>? _compiledFormatter;
    
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
                $"{nameof(VirtualColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
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

        // Compile accessors once (not on every render!)
        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledAccessor = Property.Compile();
            _compiledFormatter = BuildFormatter();

            // Build sort
            if (Sortable ?? false)
            {
                _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
            }
        }

        base.OnParametersSet();
    }

    /// <summary>
    /// Ultra-lightweight cell rendering for virtualization.
    /// Critical: This is called frequently during scrolling.
    /// Must be as fast as possible.
    /// </summary>
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_compiledFormatter is null)
            return;

        // Direct formatted access - no allocations, no computation
        var formatted = _compiledFormatter(item);

        // Minimal DOM
        if (!string.IsNullOrEmpty(CssClass))
        {
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", CssClass);
            builder.AddContent(2, formatted);
            builder.CloseElement();
        }
        else
        {
            // Even more minimal - just text
            builder.AddContent(0, formatted);
        }
    }

    /// <summary>
    /// Builds a compiled formatter function.
    /// Called once during initialization, not on every render.
    /// </summary>
    private Func<TGridItem, string> BuildFormatter()
    {
        // Use custom formatter if provided
        if (CustomFormatter is not null)
        {
            return item =>
            {
                var value = _compiledAccessor!(item);
                return CustomFormatter(value);
            };
        }

        // Use format string if provided
        if (!string.IsNullOrEmpty(Format))
        {
            return item =>
            {
                var value = _compiledAccessor!(item);
                if (value is IFormattable formattable)
                {
                    return formattable.ToString(Format, null) ?? string.Empty;
                }
                return value?.ToString() ?? string.Empty;
            };
        }

        // Default ToString
        return item =>
        {
            var value = _compiledAccessor!(item);
            return value?.ToString() ?? string.Empty;
        };
    }
}
