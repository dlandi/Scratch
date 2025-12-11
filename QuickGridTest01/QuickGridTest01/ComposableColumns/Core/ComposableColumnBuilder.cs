using System.Linq.Expressions;

namespace QuickGridTest01.ComposableColumns.Core;

/// <summary>
/// Fluent builder for creating composable columns with features.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class ComposableColumnBuilder<TGridItem, TValue>
{
    private readonly List<IColumnFeature<TGridItem>> _features = [];
    private Expression<Func<TGridItem, TValue>>? _property;
    private string? _title;
    private string? _format;
    private Func<TValue, string>? _formatter;
    private bool _sortable;
    private string? _cssClass;

    /// <summary>
    /// Creates a new builder.
    /// </summary>
    public static ComposableColumnBuilder<TGridItem, TValue> Create() => new();

    /// <summary>
    /// Creates a new builder with the specified property.
    /// </summary>
    public static ComposableColumnBuilder<TGridItem, TValue> Create(Expression<Func<TGridItem, TValue>> property)
    {
        var builder = new ComposableColumnBuilder<TGridItem, TValue>();
        builder._property = property;
        return builder;
    }

    /// <summary>
    /// Sets the property expression for the column.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithProperty(Expression<Func<TGridItem, TValue>> property)
    {
        _property = property;
        return this;
    }

    /// <summary>
    /// Sets the column title.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithTitle(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Sets the format string.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithFormat(string format)
    {
        _format = format;
        return this;
    }

    /// <summary>
    /// Sets a custom formatter function.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithFormatter(Func<TValue, string> formatter)
    {
        _formatter = formatter;
        return this;
    }

    /// <summary>
    /// Makes the column sortable.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> Sortable(bool sortable = true)
    {
        _sortable = sortable;
        return this;
    }

    /// <summary>
    /// Sets the CSS class for the column.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithCssClass(string cssClass)
    {
        _cssClass = cssClass;
        return this;
    }

    /// <summary>
    /// Adds a feature to the column.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithFeature(IColumnFeature<TGridItem> feature)
    {
        _features.Add(feature);
        return this;
    }

    /// <summary>
    /// Adds multiple features to the column.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithFeatures(params IColumnFeature<TGridItem>[] features)
    {
        _features.AddRange(features);
        return this;
    }

    /// <summary>
    /// Adds multiple features to the column.
    /// </summary>
    public ComposableColumnBuilder<TGridItem, TValue> WithFeatures(IEnumerable<IColumnFeature<TGridItem>> features)
    {
        _features.AddRange(features);
        return this;
    }

    /// <summary>
    /// Gets the built configuration for use as component parameters.
    /// </summary>
    public ComposableColumnConfig<TGridItem, TValue> Build()
    {
        return new ComposableColumnConfig<TGridItem, TValue>
        {
            Property = _property,
            Title = _title,
            Format = _format,
            Formatter = _formatter,
            Sortable = _sortable,
            CssClass = _cssClass,
            Features = _features.ToList()
        };
    }
}

/// <summary>
/// Configuration object produced by the builder.
/// Can be passed to a ComposableColumn component.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public sealed class ComposableColumnConfig<TGridItem, TValue>
{
    public Expression<Func<TGridItem, TValue>>? Property { get; init; }
    public string? Title { get; init; }
    public string? Format { get; init; }
    public Func<TValue, string>? Formatter { get; init; }
    public bool Sortable { get; init; }
    public string? CssClass { get; init; }
    public IReadOnlyList<IColumnFeature<TGridItem>> Features { get; init; } = [];
}

/// <summary>
/// Extension methods for easy builder access.
/// </summary>
public static class ComposableColumnBuilderExtensions
{
    /// <summary>
    /// Creates a composable column builder for the specified property.
    /// </summary>
    public static ComposableColumnBuilder<TGridItem, TValue> Column<TGridItem, TValue>(
        this Expression<Func<TGridItem, TValue>> property)
    {
        return ComposableColumnBuilder<TGridItem, TValue>.Create(property);
    }
}
