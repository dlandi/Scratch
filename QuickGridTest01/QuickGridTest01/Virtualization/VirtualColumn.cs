using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Column optimized for virtual scrolling scenarios with minimal per-cell overhead.
/// Compiles accessors and a formatter once and uses <see cref="TypeTraits{T}"/> for fast date/time formatting.
/// </summary>
public class VirtualColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    /// <summary>Expression selecting the value to display. Required.</summary>
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    /// <summary>Optional format string applied to <see cref="IFormattable"/> values.</summary>
    [Parameter] public string? Format { get; set; }
    /// <summary>Optional custom formatter that overrides <see cref="Format"/>.</summary>
    [Parameter] public Func<TValue, string>? CustomFormatter { get; set; }
    /// <summary>Optional CSS class applied to a wrapper span. If null/empty, renders text-only for minimal DOM.</summary>
    [Parameter] public string? CssClass { get; set; }

    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledAccessor;
    private Func<TGridItem, string>? _compiledFormatter;
    
    private GridSort<TGridItem>? _sortBuilder;

    private static readonly ValueKind s_kind = TypeTraits<TValue>.Kind;

    /// <inheritdoc />
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException(
                $"{nameof(VirtualColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }

        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Title is null && Property.Body is MemberExpression memberExpression)
        {
            Title = memberExpression.Member.Name;
        }

        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledAccessor = Property.Compile();
            _compiledFormatter = BuildFormatter();

            if (Sortable ?? false)
            {
                _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
            }
        }

        base.OnParametersSet();
    }

    /// <inheritdoc />
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_compiledFormatter is null)
            return;

        var formatted = _compiledFormatter(item);

        if (!string.IsNullOrEmpty(CssClass))
        {
            builder.OpenElement(0, "span");
            builder.SetKey(item);
            builder.AddAttribute(1, "class", CssClass);
            builder.AddContent(2, formatted);
            builder.CloseElement();
        }
        else
        {
            builder.AddContent(0, formatted);
        }
    }

    private Func<TGridItem, string> BuildFormatter()
    {
        if (CustomFormatter is not null)
        {
            return item =>
            {
                var value = _compiledAccessor!(item);
                return CustomFormatter(value);
            };
        }

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

        return item =>
        {
            var value = _compiledAccessor!(item);
            if (value is null) return string.Empty;

            if (s_kind is ValueKind.Date or ValueKind.Time or ValueKind.DateTime)
            {
                return TypeTraits<TValue>.FormatForInput(value, null, CultureInfo.InvariantCulture);
            }

            return value.ToString() ?? string.Empty;
        };
    }
}
