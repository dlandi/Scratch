using Microsoft.AspNetCore.Components.Rendering;

namespace QuickGridTest01.ComposableColumns.Core;

/// <summary>
/// Base interface for all column features that can be composed together.
/// Features are modular units of functionality that can be added to a ComposableColumn.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface IColumnFeature<TGridItem>
{
    /// <summary>
    /// The priority determines the order in which features are executed.
    /// Lower values execute first. Use <see cref="FeaturePriority"/> constants.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Called once when the feature is attached to a column.
    /// Use this to initialize any state or subscribe to events.
    /// </summary>
    /// <param name="context">The shared context for all features in this column.</param>
    void OnAttach(FeatureContext<TGridItem> context);

    /// <summary>
    /// Called when the feature is detached from a column (e.g., on dispose).
    /// Use this to clean up resources.
    /// </summary>
    /// <param name="context">The shared context for all features in this column.</param>
    void OnDetach(FeatureContext<TGridItem> context);

    /// <summary>
    /// Called when the column's parameters have changed.
    /// Features should update their internal state based on current property values.
    /// Default implementation does nothing.
    /// </summary>
    /// <param name="context">The shared context for all features in this column.</param>
    void OnParametersChanged(FeatureContext<TGridItem> context) { }
}

/// <summary>
/// Interface for features that participate in cell rendering.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface ICellRenderFeature<TGridItem> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Called during cell rendering. Features can add content before, after, or wrap the cell content.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    /// <param name="sequence">The current sequence number (ref to allow incrementing).</param>
    /// <param name="item">The current row item.</param>
    /// <param name="context">The shared feature context.</param>
    /// <param name="renderNext">Action to render the next feature in the pipeline (or the default content).</param>
    void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext);
}

/// <summary>
/// Interface for features that participate in header rendering.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface IHeaderRenderFeature<TGridItem> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Called during header rendering.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    /// <param name="sequence">The current sequence number.</param>
    /// <param name="context">The shared feature context.</param>
    /// <param name="renderNext">Action to render the next feature in the pipeline.</param>
    void RenderHeader(
        RenderTreeBuilder builder,
        ref int sequence,
        FeatureContext<TGridItem> context,
        Action renderNext);
}

/// <summary>
/// Interface for features that provide a value accessor.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value being accessed.</typeparam>
public interface IValueAccessorFeature<TGridItem, TValue> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Gets the value from the grid item.
    /// </summary>
    Func<TGridItem, TValue> GetValue { get; }

    /// <summary>
    /// Sets the value on the grid item (optional, for editable columns).
    /// </summary>
    Action<TGridItem, TValue>? SetValue { get; }
}

/// <summary>
/// Interface for features that provide sorting capability.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface ISortingFeature<TGridItem> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Gets the sort key selector for ascending sort.
    /// </summary>
    Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? GetSortFunction();

    /// <summary>
    /// Whether this column is sortable.
    /// </summary>
    bool IsSortable { get; }
}

/// <summary>
/// Interface for features that need to respond to value changes.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IValueChangedFeature<TGridItem, TValue> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Called when the value has changed.
    /// </summary>
    /// <param name="item">The grid item.</param>
    /// <param name="oldValue">The previous value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="context">The feature context.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnValueChangedAsync(TGridItem item, TValue oldValue, TValue newValue, FeatureContext<TGridItem> context);
}

/// <summary>
/// Interface for features that provide validation.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value being validated.</typeparam>
public interface IValidationFeature<TGridItem, TValue> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Validates the given value.
    /// </summary>
    /// <param name="item">The grid item.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="context">The feature context.</param>
    /// <returns>Validation result with any error messages.</returns>
    Task<ValidationResult> ValidateAsync(TGridItem item, TValue value, FeatureContext<TGridItem> context);
}

/// <summary>
/// Result of a validation operation.
/// </summary>
public sealed record ValidationResult
{
    public static readonly ValidationResult Success = new() { IsValid = true };

    public bool IsValid { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];

    public static ValidationResult Failure(params string[] errors) =>
        new() { IsValid = false, Errors = errors };

    public static ValidationResult Failure(IEnumerable<string> errors) =>
        new() { IsValid = false, Errors = errors.ToList() };
}

/// <summary>
/// Interface for features that provide grid-level filtering capability.
/// Features implementing this interface will be detected by ComposableGrid
/// and their filter UI will be auto-rendered in a filter toolbar.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public interface IGridFilterFeature<TGridItem> : IColumnFeature<TGridItem>
{
    /// <summary>
    /// Whether this filter currently has an active filter value.
    /// </summary>
    bool HasActiveFilter { get; }

    /// <summary>
    /// The column title to display as label for this filter.
    /// </summary>
    string? FilterLabel { get; }

    /// <summary>
    /// Applies this filter to the given queryable.
    /// </summary>
    IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> items);

    /// <summary>
    /// Clears the filter value.
    /// </summary>
    Task ClearFilterAsync();

    /// <summary>
    /// Renders the filter input UI for this column.
    /// </summary>
    void RenderFilterInput(RenderTreeBuilder builder, ref int sequence);

    /// <summary>
    /// Event raised when the filter value changes.
    /// </summary>
    event Func<Task>? OnFilterChanged;
}
