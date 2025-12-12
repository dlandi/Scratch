using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using QuickGridTest01.CustomColumns;

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
    [Parameter] public Func<TGridItem, TValue> Property { get; set; } = default!;
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

    // State management - tracks last property to detect changes
    private Func<TGridItem, TValue>? _lastProperty;
    private bool _isInitialized;

    /// <summary>
    /// Lifecycle: Called when component is first initialized
    /// </summary>
    protected override void OnInitialized()
    {
        _isInitialized = true;

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
        // Only process after initialization
        if (!_isInitialized)
        {
            return;
        }

        // State management: Detect property changes
        if (_lastProperty != Property)
        {
            _lastProperty = Property;

            // Configure sorting if the column is sortable
            if (Sortable ?? false)
            {
                _sortBuilder = BuildSort();
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
        var value = Property(item);

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
            builder.AddContent(sequence++, value?.ToString() ?? string.Empty);
            builder.CloseElement(); // span
        }

        builder.CloseElement(); // container span
    }

    /// <summary>
    /// Builds a GridSort for this column
    /// Note: This is a simplified implementation - production code would need more robust sorting
    /// </summary>
    private GridSort<TGridItem> BuildSort()
    {
        // Create ascending sort
        return GridSort<TGridItem>
            .ByAscending(item => Property(item))
            .ThenAscending(item => item); // Stable sort
    }
}
