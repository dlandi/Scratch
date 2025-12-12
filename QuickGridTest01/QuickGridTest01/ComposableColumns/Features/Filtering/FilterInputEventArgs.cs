using QuickGridTest01.Filterable;

namespace QuickGridTest01.ComposableColumns.Features.Filtering;

/// <summary>
/// Event args for when a filter is applied from the FilterInput component.
/// </summary>
/// <typeparam name="TValue">The type of the filter value.</typeparam>
public class FilterInputEventArgs<TValue>
{
    /// <summary>
    /// The filter value.
    /// </summary>
    public TValue? Value { get; init; }

    /// <summary>
    /// The selected filter operator.
    /// </summary>
    public IFilterOperator<TValue>? Operator { get; init; }

    /// <summary>
    /// Whether the filter has a value.
    /// </summary>
    public bool HasValue { get; init; }
}
