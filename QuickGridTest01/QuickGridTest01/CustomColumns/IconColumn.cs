using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// A custom column that displays an icon based on the cell value.
/// Demonstrates: ColumnBase extension, custom rendering, state management, lifecycle methods.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row</typeparam>
/// <typeparam name="TValue">The type of the property value</typeparam>
public class IconColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    // Column configuration parameters
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public Func<TValue, string> IconMapper { get; set; } = default!;
    [Parameter] public Func<TValue, string>? ColorMapper { get; set; }
    [Parameter] public Func<TValue, string>? TooltipMapper { get; set; }
    [Parameter] public bool ShowValue { get; set; } = true;

    // Sorting support
    private GridSort<TGridItem>? _sortBuilder;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    // State management - compile accessor and detect changes
    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledGetter;
    private static readonly ValueKind s_kind = TypeTraits<TValue>.Kind;

    /// <summary>
    /// Lifecycle: Called when component is first initialized
    /// </summary>
    protected override void OnInitialized()
    {
        // Validate required parameters
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

    /// <summary>
    /// Lifecycle: Called when parameters are set/updated
    /// This is where we detect parameter changes and rebuild state
    /// </summary>
    protected override void OnParametersSet()
    {
        // Set title from property name if not provided
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

    /// <summary>
    /// Core rendering method - defines how cell content is rendered
    /// </summary>
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        // Extract the value from the item
        var value = _compiledGetter!(item);

        // Get mapped values
        var icon = IconMapper(value);
        var color = ColorMapper?.Invoke(value) ?? "inherit";
        var tooltip = TooltipMapper?.Invoke(value) ?? value?.ToString();

        // Build the cell content
        int sequence = 0;

        // Container span
        builder.OpenElement(sequence++, "span");
        builder.AddAttribute(sequence++, "class", "icon-column-container");
        builder.AddAttribute(sequence++, "title", tooltip);

        // Icon element
        builder.OpenElement(sequence++, "i");
        builder.AddAttribute(sequence++, "class", icon);
        builder.AddAttribute(sequence++, "style", $"color: {color}; margin-right: 8px;");
        builder.AddAttribute(sequence++, "aria-hidden", "true");
        builder.CloseElement(); // i

        // Optional value text
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
            builder.CloseElement(); // span
        }

        builder.CloseElement(); // container span
    }

    /// <summary>
    /// Builds a GridSort for this column
    /// </summary>
    private GridSort<TGridItem> BuildSort()
    {
        // Create ascending sort
        return GridSort<TGridItem>
            .ByAscending(Property)
            .ThenAscending(Property);
    }
}
