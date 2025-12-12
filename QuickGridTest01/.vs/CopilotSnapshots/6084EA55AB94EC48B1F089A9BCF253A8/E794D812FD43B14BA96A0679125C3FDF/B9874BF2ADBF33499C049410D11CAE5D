using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// A custom column that displays an icon (and optional value text) based on the cell value.
/// Supports memoization for enum domains and uses <see cref="TypeTraits{T}"/> for fast date/time formatting.
/// </summary>
/// <typeparam name="TGridItem">Row item type.</typeparam>
/// <typeparam name="TValue">Property value type.</typeparam>
public class IconColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    /// <summary>Expression selecting the value to map to an icon. Required.</summary>
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    /// <summary>Maps a value to a CSS class (e.g., "bi bi-check"). Required.</summary>
    [Parameter] public Func<TValue, string> IconMapper { get; set; } = default!;
    /// <summary>Optional color mapper for the icon.</summary>
    [Parameter] public Func<TValue, string>? ColorMapper { get; set; }
    /// <summary>Optional tooltip mapper for the icon/value.</summary>
    [Parameter] public Func<TValue, string>? TooltipMapper { get; set; }
    /// <summary>When true, renders the value text next to the icon.</summary>
    [Parameter] public bool ShowValue { get; set; } = true;

    private GridSort<TGridItem>? _sortBuilder;

    /// <inheritdoc />
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledGetter;
    private static readonly ValueKind s_kind = TypeTraits<TValue>.Kind;
    private static readonly bool s_isEnum = TypeTraits<TValue>.IsEnum;

    // Small memoization cache for enum domains to reduce repeated mappings
    private readonly Dictionary<TValue, (string icon, string? color, string? tip)> _mapCache = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException(
                $"{nameof(IconColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }

        if (IconMapper is null)
        {
            throw new InvalidOperationException(
                $"{nameof(IconColumn<TGridItem, TValue>)} requires an {nameof(IconMapper)} parameter.");
        }

        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Title is null && Property.Body is MemberExpression me)
        {
            Title = me.Member.Name;
        }

        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledGetter = Property.Compile();

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
        var value = _compiledGetter!(item);

        (string icon, string? color, string? tip) mapped;
        if (s_isEnum && value is not null)
        {
            if (!_mapCache.TryGetValue(value, out mapped))
            {
                mapped = (IconMapper(value), ColorMapper?.Invoke(value), TooltipMapper?.Invoke(value));
                _mapCache[value] = mapped;
            }
        }
        else
        {
            mapped = (IconMapper(value), ColorMapper?.Invoke(value), TooltipMapper?.Invoke(value));
        }

        int sequence = 0;

        builder.OpenElement(sequence++, "span");
        builder.SetKey(item);
        builder.AddAttribute(sequence++, "class", "icon-column-container");
        builder.AddAttribute(sequence++, "title", mapped.tip ?? value?.ToString());

        builder.OpenElement(sequence++, "i");
        builder.AddAttribute(sequence++, "class", mapped.icon);
        builder.AddAttribute(sequence++, "style", $"color: {mapped.color ?? "inherit"}; margin-right: 8px;");
        builder.AddAttribute(sequence++, "aria-hidden", "true");
        builder.CloseElement();

        if (ShowValue)
        {
            builder.OpenElement(sequence++, "span");
            builder.AddAttribute(sequence++, "class", "icon-column-value");
            if (value is null)
            {
                builder.AddContent(sequence++, string.Empty);
            }
            else if (s_kind is ValueKind.Date or ValueKind.Time or ValueKind.DateTime)
            {
                builder.AddContent(sequence++, TypeTraits<TValue>.FormatForInput(value, null, CultureInfo.InvariantCulture));
            }
            else
            {
                builder.AddContent(sequence++, value.ToString());
            }
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    private GridSort<TGridItem> BuildSort()
    {
        return GridSort<TGridItem>
            .ByAscending(Property)
            .ThenAscending(Property);
    }
}
