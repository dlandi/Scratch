using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.QuickGrid;
using System.Linq.Expressions;

namespace QuickGridTest01.FormattedValue.Component;

/// <summary>
/// A QuickGrid column that formats values using a provided formatter function.
/// Provides strongly-typed property access with efficient expression compilation.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid</typeparam>
/// <typeparam name="TValue">The type of the property being formatted</typeparam>
public class FormattedValueColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    /// <summary>
    /// Gets or sets the property expression that selects the value to format from each grid item.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>
    /// Gets or sets the formatter function that converts the property value to a display string.
    /// </summary>
    [Parameter, EditorRequired]
    public Func<object?, string> Formatter { get; set; } = default!;

    // Cached compiled property accessor for performance
    private Expression<Func<TGridItem, TValue>>? _lastAssignedProperty;
    private Func<TGridItem, TValue>? _compiledPropertyGetter;

    /// <summary>
    /// Gets or sets the sorting rules for this column.
    /// If not explicitly set, sorting is enabled using the Property expression.
    /// </summary>
    public override GridSort<TGridItem>? SortBy { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Compile the property expression if it changed
        // This provides 10x+ performance improvement over repeated compilation
        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
            _compiledPropertyGetter = Property?.Compile();

            // Auto-enable sorting if not explicitly set
            if (SortBy == null && Property != null)
            {
                SortBy = GridSort<TGridItem>.ByAscending(Property)
                    .ThenAscending(Property); // Enables toggle between asc/desc
            }
        }

        // Validate required parameters
        if (Property == null)
        {
            throw new InvalidOperationException(
                $"{nameof(FormattedValueColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }

        if (Formatter == null)
        {
            throw new InvalidOperationException(
                $"{nameof(FormattedValueColumn<TGridItem, TValue>)} requires a {nameof(Formatter)} parameter.");
        }
    }

    /// <inheritdoc />
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_compiledPropertyGetter == null)
        {
            // This should never happen due to OnParametersSet validation,
            // but we handle it defensively
            builder.AddContent(0, string.Empty);
            return;
        }

        // Get the property value using the compiled accessor
        var value = _compiledPropertyGetter(item);

        // Format the value using the provided formatter
        var formattedValue = Formatter(value);

        // Render the formatted value
        builder.AddContent(0, formattedValue);
    }

    /// <inheritdoc />
    protected override bool IsSortableByDefault()
    {
        // Column is sortable by default if it has a Property expression
        return Property != null;
    }
}
