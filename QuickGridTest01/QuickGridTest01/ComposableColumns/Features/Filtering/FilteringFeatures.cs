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
/// Renders a filter UI in the header with operator selection and value input.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value being filtered.</typeparam>
public class FilterFeature<TGridItem, TValue> : IColumnFeature<TGridItem>, IDisposable
{
    public int Priority => FeaturePriority.Core + 50; // After core, before formatting

    private IFilterOperator<TValue>? _selectedOperator;
    private TValue? _filterValue;
    private bool _hasFilterValue;
    private bool _showFilterDropdown;
    private CancellationTokenSource? _filterCts;
    private bool _disposed;
    private FeatureContext<TGridItem>? _context;

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
    /// CSS class for the filter container.
    /// </summary>
    public string FilterContainerClass { get; set; } = "column-filter";

    /// <summary>
    /// CSS class for the filter dropdown.
    /// </summary>
    public string FilterDropdownClass { get; set; } = "filter-dropdown";

    /// <summary>
    /// Whether the filter dropdown is currently visible.
    /// </summary>
    public bool IsDropdownVisible => _showFilterDropdown;

    /// <summary>
    /// Whether this column has an active filter.
    /// </summary>
    public bool HasActiveFilter => _selectedOperator is not null && _hasFilterValue;

    /// <summary>
    /// The current filter value.
    /// </summary>
    public TValue? CurrentFilterValue => _filterValue;

    /// <summary>
    /// The current selected operator.
    /// </summary>
    public IFilterOperator<TValue>? SelectedOperator => _selectedOperator;

    /// <summary>
    /// Callback when filter changes.
    /// </summary>
    public EventCallback<FilterChangedEventArgs<TValue>> OnFilterChanged { get; set; }

    /// <summary>
    /// Function to apply the filter to an IQueryable source.
    /// This can be used by the parent component to filter data.
    /// </summary>
    public Func<IQueryable<TGridItem>, IQueryable<TGridItem>>? GetFilteredQuery { get; private set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        _context = context;

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
        Dispose();
    }

    /// <summary>
    /// Renders the filter toggle button (for use in column headers).
    /// </summary>
    public void RenderFilterToggle(
        RenderTreeBuilder builder,
        ref int sequence,
        FeatureContext<TGridItem> context)
    {
        var baseSeq = sequence;
        sequence += 50; // Reserve space
        
        // Filter toggle button
        builder.OpenElement(baseSeq + 0, "button");
        builder.AddAttribute(baseSeq + 1, "type", "button");
        builder.AddAttribute(baseSeq + 2, "class", $"filter-toggle {(HasActiveFilter ? "active" : "")}");
        builder.AddAttribute(baseSeq + 3, "title", HasActiveFilter ? "Filter active - click to modify" : "Click to filter");
        builder.AddAttribute(baseSeq + 4, "onclick", CreateCallback<MouseEventArgs>(_ => ToggleDropdown(context)));
        builder.AddAttribute(baseSeq + 5, "onclick:stopPropagation", true);

        // Filter icon
        builder.OpenElement(baseSeq + 6, "i");
        builder.AddAttribute(baseSeq + 7, "class", HasActiveFilter ? "bi bi-funnel-fill" : "bi bi-funnel");
        builder.CloseElement();

        builder.CloseElement();

        // ALWAYS render dropdown container to maintain stable render tree
        RenderFilterDropdown(builder, baseSeq + 20, context, _showFilterDropdown);
    }

    /// <summary>
    /// Renders the complete filter UI (toggle + dropdown when open).
    /// </summary>
    public void RenderFilterUI(
        RenderTreeBuilder builder,
        ref int sequence,
        FeatureContext<TGridItem> context)
    {
        var baseSeq = sequence;
        sequence += 100; // Reserve space
        
        builder.OpenElement(baseSeq + 0, "div");
        builder.AddAttribute(baseSeq + 1, "class", FilterContainerClass);
        builder.AddAttribute(baseSeq + 2, "style", "position: relative;");

        RenderFilterToggle(builder, ref sequence, context);

        builder.CloseElement();
    }

    /// <summary>
    /// Renders an inline filter toolbar (for use outside the grid header).
    /// </summary>
    public void RenderFilterToolbar(
        RenderTreeBuilder builder,
        ref int sequence,
        FeatureContext<TGridItem> context,
        string? title = null)
    {
        var baseSeq = sequence;
        sequence += 200; // Reserve space
        
        builder.OpenElement(baseSeq + 0, "div");
        builder.AddAttribute(baseSeq + 1, "class", "filter-toolbar-item");

        // Label
        builder.OpenElement(baseSeq + 2, "label");
        builder.AddAttribute(baseSeq + 3, "class", "qg-label");
        builder.AddContent(baseSeq + 4, title ?? context.Title ?? "Filter");
        builder.CloseElement();

        // Controls row
        builder.OpenElement(baseSeq + 10, "div");
        builder.AddAttribute(baseSeq + 11, "class", "filter-toolbar-controls");

        // Operator select
        RenderOperatorSelect(builder, baseSeq + 20, context);

        // Value input
        RenderValueInput(builder, baseSeq + 60, context);

        // Clear button (always render the slot, conditionally show content)
        builder.OpenElement(baseSeq + 100, "span");
        if (HasActiveFilter)
        {
            builder.OpenElement(baseSeq + 101, "button");
            builder.AddAttribute(baseSeq + 102, "type", "button");
            builder.AddAttribute(baseSeq + 103, "class", "btn-filter-clear");
            builder.AddAttribute(baseSeq + 104, "title", "Clear filter");
            builder.AddAttribute(baseSeq + 105, "onclick", CreateAsyncCallback<MouseEventArgs>(async _ => await ClearFilterAsync(context)));

            builder.OpenElement(baseSeq + 106, "i");
            builder.AddAttribute(baseSeq + 107, "class", "bi bi-x-circle");
            builder.CloseElement();

            builder.CloseElement();
        }
        builder.CloseElement();

        builder.CloseElement(); // filter-toolbar-controls
        builder.CloseElement(); // filter-toolbar-item
    }

    private void RenderFilterDropdown(
        RenderTreeBuilder builder,
        int baseSeq,
        FeatureContext<TGridItem> context,
        bool dropdownVisible)
    {
        // ALWAYS render container to maintain stable render tree
        builder.OpenElement(baseSeq + 0, "div");
        builder.AddAttribute(baseSeq + 1, "class", FilterDropdownClass);
        builder.AddAttribute(baseSeq + 2, "style", dropdownVisible ? "" : "display: none;");
        builder.AddAttribute(baseSeq + 3, "onclick:stopPropagation", true);

        // Only render inner content if visible (for performance)
        if (dropdownVisible)
        {
            // Operator select
            RenderOperatorSelect(builder, baseSeq + 10, context);

            // Value input
            RenderValueInput(builder, baseSeq + 50, context);

            // Action buttons
            builder.OpenElement(baseSeq + 90, "div");
            builder.AddAttribute(baseSeq + 91, "class", "filter-actions");

            builder.OpenElement(baseSeq + 92, "button");
            builder.AddAttribute(baseSeq + 93, "type", "button");
            builder.AddAttribute(baseSeq + 94, "class", "btn-filter-apply");
            builder.AddAttribute(baseSeq + 95, "onclick", CreateAsyncCallback<MouseEventArgs>(async _ => await ApplyFilterAsync(context)));
            builder.AddContent(baseSeq + 96, "Apply");
            builder.CloseElement();

            builder.OpenElement(baseSeq + 97, "button");
            builder.AddAttribute(baseSeq + 98, "type", "button");
            builder.AddAttribute(baseSeq + 99, "class", "btn-filter-clear");
            builder.AddAttribute(baseSeq + 100, "onclick", CreateAsyncCallback<MouseEventArgs>(async _ => await ClearFilterAsync(context)));
            builder.AddContent(baseSeq + 101, "Clear");
            builder.CloseElement();

            builder.CloseElement(); // filter-actions
        }
        
        builder.CloseElement(); // dropdown
    }

    private void RenderOperatorSelect(
        RenderTreeBuilder builder,
        int baseSeq,
        FeatureContext<TGridItem> context)
    {
        builder.OpenElement(baseSeq + 0, "select");
        builder.AddAttribute(baseSeq + 1, "class", "qg-select filter-operator");
        builder.AddAttribute(baseSeq + 2, "value", _selectedOperator?.Name ?? "");
        builder.AddAttribute(baseSeq + 3, "onchange", CreateAsyncCallback<ChangeEventArgs>(async e => await OnOperatorChangedAsync(e, context)));

        var opIndex = 0;
        foreach (var op in Operators)
        {
            builder.OpenElement(baseSeq + 10, "option");
            builder.SetKey(opIndex);
            builder.AddAttribute(baseSeq + 11, "value", op.Name);
            builder.AddContent(baseSeq + 12, $"{op.Symbol} {op.Name}");
            builder.CloseElement();
            opIndex++;
        }

        builder.CloseElement();
    }

    private void RenderValueInput(
        RenderTreeBuilder builder,
        int baseSeq,
        FeatureContext<TGridItem> context)
    {
        var inputType = GetInputType();

        if (inputType == "checkbox")
        {
            builder.OpenElement(baseSeq + 0, "input");
            builder.AddAttribute(baseSeq + 1, "type", "checkbox");
            builder.AddAttribute(baseSeq + 2, "class", "filter-checkbox");
            builder.AddAttribute(baseSeq + 3, "checked", _hasFilterValue && _filterValue is bool b && b);
            builder.AddAttribute(baseSeq + 4, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(
                this, e => OnValueChanged(e, context)));
            builder.CloseElement();
        }
        else
        {
            builder.OpenElement(baseSeq + 0, "input");
            builder.AddAttribute(baseSeq + 1, "type", inputType);
            builder.AddAttribute(baseSeq + 2, "class", "qg-input filter-value");
            builder.AddAttribute(baseSeq + 3, "placeholder", Placeholder);
            builder.AddAttribute(baseSeq + 4, "value", _hasFilterValue && _filterValue is not null ? FormatValueForInput(_filterValue) : "");
            builder.AddAttribute(baseSeq + 5, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(
                this, e => OnValueChanged(e, context)));
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

    private void ToggleDropdown(FeatureContext<TGridItem> context)
    {
        _showFilterDropdown = !_showFilterDropdown;
        context.RequestRefresh?.Invoke();
    }

    private async Task OnOperatorChangedAsync(ChangeEventArgs e, FeatureContext<TGridItem> context)
    {
        var operatorName = e.Value?.ToString();
        _selectedOperator = Operators.FirstOrDefault(op => op.Name == operatorName);

        // Auto-apply if we have a value
        if (_hasFilterValue)
        {
            await NotifyFilterChangedAsync(context);
        }
    }

    private void OnValueChanged(ChangeEventArgs e, FeatureContext<TGridItem> context)
    {
        try
        {
            if (TypeTraits<TValue>.TryParseFromEventValue(e.Value, CultureInfo.InvariantCulture, out var parsed))
            {
                _filterValue = parsed;
                _hasFilterValue = parsed is not null;
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
        _filterCts = new CancellationTokenSource();
        var token = _filterCts.Token;

        // Capture the invoker and refresh delegate at this moment
        var invoker = context.InvokeAsync;
        var refresh = context.RequestRefresh;

        // Fire-and-forget with safety checks
        _ = DebounceAndNotifyAsync(token, invoker, refresh, context);
    }

    private async Task DebounceAndNotifyAsync(
        CancellationToken token,
        Func<Func<Task>, Task>? invoker,
        Action? refresh,
        FeatureContext<TGridItem> context)
    {
        try
        {
            await Task.Delay(DebounceMilliseconds, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (token.IsCancellationRequested || _disposed)
            return;

        // If we have an invoker, use it to marshal to UI thread
        if (invoker != null)
        {
            try
            {
                await invoker(async () =>
                {
                    if (_disposed) return;

                    // Update filter state
                    UpdateFilterQuery(context);

                    // Fire callback
                    if (OnFilterChanged.HasDelegate)
                    {
                        await OnFilterChanged.InvokeAsync(new FilterChangedEventArgs<TValue>
                        {
                            HasFilter = HasActiveFilter,
                            Operator = _selectedOperator,
                            Value = _filterValue
                        });
                    }

                    // Request refresh
                    refresh?.Invoke();
                });
            }
            catch (ObjectDisposedException)
            {
                // Component/circuit was disposed - silently ignore
            }
            catch (InvalidOperationException)
            {
                // Circuit disconnected - silently ignore
            }
        }
    }

    private async Task ApplyFilterAsync(FeatureContext<TGridItem> context)
    {
        _showFilterDropdown = false;
        await NotifyFilterChangedAsync(context);
    }

    /// <summary>
    /// Clears the current filter.
    /// </summary>
    public async Task ClearFilterAsync(FeatureContext<TGridItem> context)
    {
        _filterValue = default;
        _hasFilterValue = false;
        _showFilterDropdown = false;
        await NotifyFilterChangedAsync(context);
    }

    private async Task NotifyFilterChangedAsync(FeatureContext<TGridItem> context)
    {
        // Update the filter function
        UpdateFilterQuery(context);

        // Fire the callback
        if (OnFilterChanged.HasDelegate)
        {
            await OnFilterChanged.InvokeAsync(new FilterChangedEventArgs<TValue>
            {
                HasFilter = HasActiveFilter,
                Operator = _selectedOperator,
                Value = _filterValue
            });
        }

        // Explicitly call StateHasChanged
        context.RequestRefresh?.Invoke();
    }

    private void UpdateFilterQuery(FeatureContext<TGridItem> context)
    {
        if (context is not FeatureContext<TGridItem, TValue> typedContext)
        {
            GetFilteredQuery = null;
            return;
        }

        if (!HasActiveFilter || _selectedOperator is null || !_hasFilterValue || _filterValue is null)
        {
            GetFilteredQuery = source => source;
            return;
        }

        var propertyExpr = typedContext.PropertyExpression;
        if (propertyExpr is null)
        {
            GetFilteredQuery = source => source;
            return;
        }

        var op = _selectedOperator;
        var value = _filterValue;

        GetFilteredQuery = source => op.Apply(source, propertyExpr, value);
    }

    /// <summary>
    /// Applies the current filter to an IQueryable source.
    /// </summary>
    public IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> source)
    {
        if (GetFilteredQuery is null)
            return source;
        
        return GetFilteredQuery(source);
    }

    /// <summary>
    /// Sets the filter value programmatically.
    /// </summary>
    public void SetFilter(IFilterOperator<TValue>? op, TValue? value, FeatureContext<TGridItem> context)
    {
        _selectedOperator = op ?? Operators.FirstOrDefault();
        _filterValue = value;
        _hasFilterValue = value is not null;
        UpdateFilterQuery(context);
        context.RequestRefresh?.Invoke();
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

        // Numeric types
        if (kind is ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single)
        {
            return
            [
                new NumericEqualsOperator<TValue>(),
                new NumericNotEqualsOperator<TValue>(),
                new NumericGreaterThanOperator<TValue>(),
                new NumericGreaterThanOrEqualOperator<TValue>(),
                new NumericLessThanOperator<TValue>(),
                new NumericLessThanOrEqualOperator<TValue>()
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
    /// <summary>
    /// Whether a filter is currently active.
    /// </summary>
    public bool HasFilter { get; init; }

    /// <summary>
    /// The selected filter operator.
    /// </summary>
    public IFilterOperator<TValue>? Operator { get; init; }

    /// <summary>
    /// The current filter value.
    /// </summary>
    public TValue? Value { get; init; }
}

/// <summary>
/// Coordinator for managing multiple column filters.
/// Use this to collect and apply filters from multiple FilterFeature instances.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class FilterCoordinator<TGridItem>
{
    private readonly List<Func<IQueryable<TGridItem>, IQueryable<TGridItem>>> _filterFunctions = [];

    /// <summary>
    /// Registers a filter function.
    /// </summary>
    public void RegisterFilter(Func<IQueryable<TGridItem>, IQueryable<TGridItem>> filterFunc)
    {
        _filterFunctions.Add(filterFunc);
    }

    /// <summary>
    /// Removes a filter function.
    /// </summary>
    public void UnregisterFilter(Func<IQueryable<TGridItem>, IQueryable<TGridItem>> filterFunc)
    {
        _filterFunctions.Remove(filterFunc);
    }

    /// <summary>
    /// Clears all registered filters.
    /// </summary>
    public void ClearAll()
    {
        _filterFunctions.Clear();
    }

    /// <summary>
    /// Applies all registered filters to a queryable source.
    /// </summary>
    public IQueryable<TGridItem> ApplyFilters(IQueryable<TGridItem> source)
    {
        var result = source;
        foreach (var filter in _filterFunctions)
        {
            result = filter(result);
        }
        return result;
    }

    /// <summary>
    /// Gets the count of active filters.
    /// </summary>
    public int ActiveFilterCount => _filterFunctions.Count;
}
