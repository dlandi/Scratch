using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.Infrastructure;

namespace QuickGridTest01.ComposableColumns.Features.Filtering;

/// <summary>
/// Feature that provides filtering capability for a column.
/// When added to a ComposableColumn, the ComposableGrid will automatically
/// render a filter UI in a toolbar and apply the filter to the data.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value being filtered.</typeparam>
public class FilterFeature<TGridItem, TValue> : IGridFilterFeature<TGridItem>, IDisposable
{
    public int Priority => FeaturePriority.Filtering;

    private IFilterOperator<TValue>? _selectedOperator;
    private TValue? _filterValue;
    private bool _hasFilterValue;
    private CancellationTokenSource? _filterCts;
    private bool _disposed;
    private FeatureContext<TGridItem>? _context;
    private FeatureContext<TGridItem, TValue>? _typedContext;

    /// <summary>
    /// List of filter operators available for this column.
    /// If not set, defaults are inferred from the value type.
    /// </summary>
    public List<IFilterOperator<TValue>> Operators { get; set; } = [];

    /// <summary>
    /// Debounce delay in milliseconds for filter value changes.
    /// </summary>
    public int DebounceMilliseconds { get; set; } = 300;

    /// <summary>
    /// Placeholder text for the filter input.
    /// </summary>
    public string Placeholder { get; set; } = "Filter...";

    /// <summary>
    /// Whether this column has an active filter.
    /// </summary>
    public bool HasActiveFilter => _selectedOperator is not null && _hasFilterValue;

    /// <summary>
    /// The column title to display as label for this filter.
    /// </summary>
    public string? FilterLabel => _context?.Title;

    /// <summary>
    /// The current filter value.
    /// </summary>
    public TValue? CurrentFilterValue => _filterValue;

    /// <summary>
    /// The current selected operator.
    /// </summary>
    public IFilterOperator<TValue>? SelectedOperator => _selectedOperator;

    /// <summary>
    /// Event raised when the filter value changes.
    /// </summary>
    public event Func<Task>? OnFilterChanged;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        _context = context;

        if (context is FeatureContext<TGridItem, TValue> typedContext)
        {
            _typedContext = typedContext;
        }

        // Initialize default operators if not provided
        if (Operators.Count == 0)
        {
            Operators = GetDefaultOperators();
        }

        _selectedOperator = Operators.FirstOrDefault();

        // Store a reference to the filter function in context for external access
        context.SetState("FilterFeature", this);
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        context.SetState<FilterFeature<TGridItem, TValue>?>("FilterFeature", null);
        _context = null;
        _typedContext = null;
        Dispose();
    }

    /// <summary>
    /// Applies this filter to the given queryable.
    /// </summary>
    public IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> items)
    {
        if (!HasActiveFilter || _selectedOperator is null || _filterValue is null)
            return items;

        if (_typedContext?.PropertyExpression is null)
            return items;

        return _selectedOperator.Apply(items, _typedContext.PropertyExpression, _filterValue);
    }

    /// <summary>
    /// Clears the filter value.
    /// </summary>
    public async Task ClearFilterAsync()
    {
        _filterValue = default;
        _hasFilterValue = false;

        if (OnFilterChanged is not null)
        {
            await OnFilterChanged.Invoke();
        }
    }

    /// <summary>
    /// Renders the filter input UI for this column.
    /// Called by ComposableGrid to render the filter toolbar.
    /// </summary>
    public void RenderFilterInput(RenderTreeBuilder builder, ref int sequence)
    {
        var baseSeq = sequence;
        sequence += 200;

        builder.OpenElement(baseSeq + 0, "div");
        builder.AddAttribute(baseSeq + 1, "class", "filter-toolbar-item");

        // Label
        builder.OpenElement(baseSeq + 2, "label");
        builder.AddAttribute(baseSeq + 3, "class", "qg-label");
        builder.AddContent(baseSeq + 4, FilterLabel ?? "Filter");
        builder.CloseElement();

        // Controls row
        builder.OpenElement(baseSeq + 10, "div");
        builder.AddAttribute(baseSeq + 11, "class", "filter-toolbar-controls");

        // Operator select
        RenderOperatorSelect(builder, baseSeq + 20);

        // Value input
        RenderValueInput(builder, baseSeq + 60);

        // Clear button
        builder.OpenElement(baseSeq + 100, "span");
        builder.AddAttribute(baseSeq + 101, "style", HasActiveFilter ? "" : "display: none;");
        
        builder.OpenElement(baseSeq + 102, "button");
        builder.AddAttribute(baseSeq + 103, "type", "button");
        builder.AddAttribute(baseSeq + 104, "class", "btn-filter-clear");
        builder.AddAttribute(baseSeq + 105, "title", "Clear filter");
        builder.AddAttribute(baseSeq + 106, "onclick", CreateAsyncCallback<MouseEventArgs>(async _ => await ClearFilterAsync()));

        builder.OpenElement(baseSeq + 107, "i");
        builder.AddAttribute(baseSeq + 108, "class", "bi bi-x-circle");
        builder.CloseElement();

        builder.CloseElement(); // button
        builder.CloseElement(); // span

        builder.CloseElement(); // filter-toolbar-controls
        builder.CloseElement(); // filter-toolbar-item
    }

    private void RenderOperatorSelect(RenderTreeBuilder builder, int baseSeq)
    {
        builder.OpenElement(baseSeq + 0, "select");
        builder.AddAttribute(baseSeq + 1, "class", "qg-select filter-operator");
        builder.AddAttribute(baseSeq + 2, "value", _selectedOperator?.Name ?? "");
        builder.AddAttribute(baseSeq + 3, "onchange", CreateAsyncCallback<ChangeEventArgs>(OnOperatorChangedAsync));
        builder.AddAttribute(baseSeq + 4, "title", _selectedOperator?.Name ?? "Select operator");

        var opIndex = 0;
        foreach (var op in Operators)
        {
            builder.OpenElement(baseSeq + 10, "option");
            builder.SetKey(opIndex);
            builder.AddAttribute(baseSeq + 11, "value", op.Name);
            // Short display text for compact dropdown, full name shown on hover
            var shortName = GetShortOperatorName(op.Name);
            builder.AddContent(baseSeq + 12, $"{op.Symbol} {shortName}");
            builder.CloseElement();
            opIndex++;
        }

        builder.CloseElement();
    }

    private static string GetShortOperatorName(string name) => name switch
    {
        "Contains" => "Has",
        "Equals" => "Eq",
        "StartsWith" => "Starts",
        "EndsWith" => "Ends",
        "GreaterThan" => "GT",
        "LessThan" => "LT",
        "GreaterThanOrEqual" => "GTE",
        "LessThanOrEqual" => "LTE",
        "After" => "After",
        "Before" => "Before",
        _ => name.Length > 5 ? name[..5] : name
    };

    private void RenderValueInput(RenderTreeBuilder builder, int baseSeq)
    {
        var inputType = GetInputType();

        if (inputType == "checkbox")
        {
            builder.OpenElement(baseSeq + 0, "input");
            builder.AddAttribute(baseSeq + 1, "type", "checkbox");
            builder.AddAttribute(baseSeq + 2, "class", "filter-checkbox");
            builder.AddAttribute(baseSeq + 3, "checked", _hasFilterValue && _filterValue is bool b && b);
            builder.AddAttribute(baseSeq + 4, "onchange", CreateAsyncCallback<ChangeEventArgs>(OnCheckboxChangedAsync));
            builder.CloseElement();
        }
        else
        {
            builder.OpenElement(baseSeq + 0, "input");
            builder.AddAttribute(baseSeq + 1, "type", inputType);
            builder.AddAttribute(baseSeq + 2, "class", "qg-input filter-value");
            builder.AddAttribute(baseSeq + 3, "placeholder", Placeholder);
            builder.AddAttribute(baseSeq + 4, "value", _hasFilterValue && _filterValue is not null ? FormatValueForInput(_filterValue) : "");
            builder.AddAttribute(baseSeq + 5, "oninput", CreateCallback<ChangeEventArgs>(OnValueChanged));
            builder.CloseElement();
        }
    }

    private string GetInputType()
    {
        var kind = TypeTraits<TValue>.Kind;
        return kind switch
        {
            ValueKind.DateTime => "date",
            ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single => "number",
            ValueKind.Boolean => "checkbox",
            _ => "text"
        };
    }

    private static string FormatValueForInput(TValue value)
    {
        if (value is null) return string.Empty;
        return TypeTraits<TValue>.FormatForInput(value, null, CultureInfo.InvariantCulture);
    }

    private async Task OnOperatorChangedAsync(ChangeEventArgs e)
    {
        var operatorName = e.Value?.ToString();
        _selectedOperator = Operators.FirstOrDefault(op => op.Name == operatorName);

        if (_hasFilterValue && OnFilterChanged is not null)
        {
            await OnFilterChanged.Invoke();
        }
    }

    private async Task OnCheckboxChangedAsync(ChangeEventArgs e)
    {
        if (e.Value is bool boolVal)
        {
            _filterValue = (TValue)(object)boolVal;
            _hasFilterValue = true;
        }
        else
        {
            _filterValue = default;
            _hasFilterValue = false;
        }

        if (OnFilterChanged is not null)
        {
            await OnFilterChanged.Invoke();
        }
    }

    private void OnValueChanged(ChangeEventArgs e)
    {
        try
        {
            if (TypeTraits<TValue>.TryParseFromEventValue(e.Value, CultureInfo.InvariantCulture, out var parsed))
            {
                _filterValue = parsed;
                _hasFilterValue = parsed is not null && !EqualityComparer<TValue>.Default.Equals(parsed, default);
            }
            else
            {
                _filterValue = default;
                _hasFilterValue = false;
            }
        }
        catch
        {
            _filterValue = default;
            _hasFilterValue = false;
        }

        // Cancel any previous debounce timer
        _filterCts?.Cancel();
        _filterCts?.Dispose();
        _filterCts = new CancellationTokenSource();
        
        var token = _filterCts.Token;
        _ = DebounceAndNotifyAsync(token);
    }

    private async Task DebounceAndNotifyAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(DebounceMilliseconds, token);

            if (!token.IsCancellationRequested && !_disposed && OnFilterChanged is not null)
            {
                await OnFilterChanged.Invoke();
            }
        }
        catch (TaskCanceledException)
        {
            // Expected when debounce is cancelled
        }
    }

    private List<IFilterOperator<TValue>> GetDefaultOperators()
    {
        var kind = TypeTraits<TValue>.Kind;

        if (kind == ValueKind.String)
        {
            return
            [
                (IFilterOperator<TValue>)(object)new StringContainsOperator(),
                (IFilterOperator<TValue>)(object)new StringEqualsOperator(),
                (IFilterOperator<TValue>)(object)new StringStartsWithOperator(),
                (IFilterOperator<TValue>)(object)new StringEndsWithOperator(),
            ];
        }

        if (kind == ValueKind.DateTime)
        {
            return
            [
                (IFilterOperator<TValue>)(object)new DateEqualsOperator(),
                (IFilterOperator<TValue>)(object)new DateAfterOperator(),
                (IFilterOperator<TValue>)(object)new DateBeforeOperator(),
            ];
        }

        if (kind == ValueKind.Boolean)
        {
            return [(IFilterOperator<TValue>)(object)new BooleanEqualsOperator()];
        }

        if (kind is ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single)
        {
            return
            [
                new NumericEqualsOperator<TValue>(),
                new NumericGreaterThanOperator<TValue>(),
                new NumericLessThanOperator<TValue>(),
            ];
        }

        return [];
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _filterCts?.Cancel();
        _filterCts?.Dispose();
        OnFilterChanged = null;

        GC.SuppressFinalize(this);
    }

    private IHandleEvent? GetEventReceiver()
    {
        return _context?.GetEventReceiver();
    }

    private EventCallback<TEventArgs> CreateCallback<TEventArgs>(Action<TEventArgs> handler)
    {
        var receiver = GetEventReceiver();
        if (receiver is not null)
        {
            return EventCallback.Factory.Create(receiver, handler);
        }
        return EventCallback.Factory.Create(this, handler);
    }

    private EventCallback<TEventArgs> CreateAsyncCallback<TEventArgs>(Func<TEventArgs, Task> handler)
    {
        var receiver = GetEventReceiver();
        if (receiver is not null)
        {
            return EventCallback.Factory.Create(receiver, handler);
        }
        return EventCallback.Factory.Create(this, handler);
    }
}

/// <summary>
/// Event args for filter changed events.
/// </summary>
/// <typeparam name="TValue">The type of the filter value.</typeparam>
public class FilterChangedEventArgs<TValue>
{
    public bool HasFilter { get; init; }
    public IFilterOperator<TValue>? Operator { get; init; }
    public TValue? Value { get; init; }
}
