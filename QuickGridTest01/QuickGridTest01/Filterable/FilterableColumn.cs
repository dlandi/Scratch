using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web; // Added for MouseEventArgs
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure; // TypeTraits

namespace QuickGridTest01.Filterable;

public abstract class FilterableColumnBase<TGridItem> : ColumnBase<TGridItem>
{
    [CascadingParameter] internal FilterableGrid<TGridItem>? FilterableGrid { get; set; }
    [Parameter] public EventCallback<object?> OnFilterChanged { get; set; }
    public abstract bool HasActiveFilter { get; }
    public abstract IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> items);
    public abstract Task ClearFilterAsync();

    public virtual void BuildFilterToolbar(RenderTreeBuilder builder) { builder.AddContent(0, Title); }
    public virtual void ToggleFilter() { }
}

public class FilterableColumn<TGridItem, TValue> : FilterableColumnBase<TGridItem>
{
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public List<IFilterOperator<TValue>> FilterOperators { get; set; } = new();
    [Parameter] public string? Format { get; set; }
    [Parameter] public bool ShowFilterInHeader { get; set; } = true;
    
    private IFilterOperator<TValue>? _selectedOperator;
    private TValue? _filterValue;
    private bool _hasFilterValue; // tracks if user set a value
    private bool _showFilterUI;
    
    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledProperty;
    private Func<TGridItem, string?>? _cellTextFunc;
    private GridSort<TGridItem>? _sortBuilder;

    // Cached type info via TypeTraits
    private static readonly ValueKind s_kind = TypeTraits<TValue>.Kind;
    private static readonly bool s_isEnum = TypeTraits<TValue>.IsEnum;

    public override bool HasActiveFilter => _selectedOperator is not null && _hasFilterValue;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException($"{nameof(FilterableColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }

        if (!FilterOperators.Any())
        {
            FilterOperators = GetDefaultOperators();
        }

        _selectedOperator = FilterOperators.FirstOrDefault();
        _hasFilterValue = false;
        FilterableGrid?.RegisterColumn(this);
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (Title is null && Property.Body is MemberExpression memberExpression)
        {
            Title = memberExpression.Member.Name;
        }

        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledProperty = Property.Compile();

            if (!string.IsNullOrEmpty(Format))
            {
                _cellTextFunc = item =>
                {
                    var value = _compiledProperty!(item);
                    if (value is IFormattable formattable)
                    {
                        return formattable.ToString(Format, null);
                    }
                    return value?.ToString();
                };
            }
            else
            {
                _cellTextFunc = item => _compiledProperty!(item)?.ToString();
            }

            if (Sortable ?? false)
            {
                _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
            }
        }

        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_cellTextFunc is null) return;
        var text = _cellTextFunc(item);
        builder.AddContent(0, text ?? string.Empty);
    }

    internal void RenderFilterUI(RenderTreeBuilder builder, ref int sequence)
    {
        builder.OpenElement(sequence++, "span");
        builder.AddAttribute(sequence++, "class", $"filter-toggle {(HasActiveFilter ? "active" : "")}");
        builder.AddAttribute(sequence++, "style", "position:relative; display:inline-flex; align-items:center; gap:.35rem; padding:.25rem .5rem;");
        builder.AddAttribute(sequence++, "role", "button");
        builder.AddAttribute(sequence++, "tabindex", "0");
        builder.AddAttribute(sequence++, "onclick:stopPropagation", true);
        builder.AddAttribute(sequence++, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, e => { if (e.Key == "Enter" || e.Key == " ") ToggleFilterUI(); }));
        builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create(this, ToggleFilterUI));

        builder.OpenElement(sequence++, "i");
        builder.AddAttribute(sequence++, "class", HasActiveFilter ? "bi bi-funnel-fill" : "bi bi-funnel");
        builder.CloseElement();
        builder.OpenElement(sequence++, "span");
        builder.AddAttribute(sequence++, "class", "filter-title");
        builder.AddContent(sequence++, Title);
        builder.CloseElement();
        builder.CloseElement();

        if (_showFilterUI)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "filter-dropdown");
            builder.AddAttribute(sequence++, "style", "position:absolute; z-index:1000; background:white; border:1px solid #ced4da; padding:.5rem; border-radius:.25rem; box-shadow:0 4px 16px rgba(0,0,0,.15);");
            builder.AddAttribute(sequence++, "onclick:stopPropagation", true);

            builder.OpenElement(sequence++, "select");
            builder.AddAttribute(sequence++, "class", "filter-operator");
            builder.AddAttribute(sequence++, "style", "margin-right:.5rem;");
            builder.AddAttribute(sequence++, "value", _selectedOperator?.Name ?? "");
            builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnOperatorChangedAsync));
            foreach (var op in FilterOperators)
            {
                builder.OpenElement(sequence++, "option");
                builder.AddAttribute(sequence++, "value", op.Name);
                builder.AddContent(sequence++, $"{op.Symbol} {op.Name}");
                builder.CloseElement();
            }
            builder.CloseElement();

            RenderValueInput(builder, ref sequence);

            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "filter-actions");
            builder.AddAttribute(sequence++, "style", "margin-top:.5rem; display:flex; gap:.5rem; justify-content:flex-end;");
            builder.OpenElement(sequence++, "button");
            builder.AddAttribute(sequence++, "type", "button");
            builder.AddAttribute(sequence++, "class", "btn-filter-apply");
            builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create(this, ApplyFilterAsync));
            builder.AddContent(sequence++, "Apply");
            builder.CloseElement();
            builder.OpenElement(sequence++, "button");
            builder.AddAttribute(sequence++, "type", "button");
            builder.AddAttribute(sequence++, "class", "btn-filter-clear");
            builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create(this, ClearFilterAsync));
            builder.AddContent(sequence++, "Clear");
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
        }
    }

    public override void BuildFilterToolbar(RenderTreeBuilder builder)
    {
        int seq = 0;
        // Wrapper group
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "ft-group");

        // Label using design system style
        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "class", "qg-label");
        builder.AddContent(seq++, Title);
        builder.CloseElement();

        // Controls row
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "ft-controls");

        // Operator select
        builder.OpenElement(seq++, "select");
        builder.AddAttribute(seq++, "class", "qg-select ft-operator");
        builder.AddAttribute(seq++, "value", _selectedOperator?.Name ?? "");
        builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnOperatorChangedAsync));
        foreach (var op in FilterOperators)
        {
            builder.OpenElement(seq++, "option");
            builder.AddAttribute(seq++, "value", op.Name);
            builder.AddContent(seq++, $"{op.Symbol} {op.Name}");
            builder.CloseElement();
        }
        builder.CloseElement();

        // Value input
        RenderValueInput(builder, ref seq);

        builder.CloseElement(); // ft-controls
        builder.CloseElement(); // ft-group
    }

    public override void ToggleFilter() => ToggleFilterUI();

    internal void RenderFilterUI(RenderTreeBuilder builder)
    { int seq = 0; RenderFilterUI(builder, ref seq); }

    private void RenderValueInput(RenderTreeBuilder builder, ref int sequence)
    {
        var inputType = GetInputType();
        if (inputType == "checkbox")
        {
            builder.OpenElement(sequence++, "input");
            builder.AddAttribute(sequence++, "type", "checkbox");
            builder.AddAttribute(sequence++, "class", "filter-value filter-checkbox");
            if (_hasFilterValue && _filterValue is not null && _filterValue is bool b && b)
            {
                builder.AddAttribute(sequence++, "checked", true);
            }
            builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChangedAsync));
            builder.CloseElement();
            return;
        }
        builder.OpenElement(sequence++, "input");
        builder.AddAttribute(sequence++, "type", inputType);
        builder.AddAttribute(sequence++, "class", "qg-input filter-value");
        builder.AddAttribute(sequence++, "placeholder", "Filter value...");
        if (_hasFilterValue && _filterValue is not null)
        {
            builder.AddAttribute(sequence++, "value", FormatValueForInput(_filterValue));
        }
        builder.AddAttribute(sequence++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChangedAsync));
        builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChangedAsync));
        builder.CloseElement();
    }

    private string GetInputType()
    {
        // Map ValueKind to HTML input type
        return s_kind switch
        {
            ValueKind.DateTime => "date",
            ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single => "number",
            ValueKind.Boolean => "checkbox",
            _ => "text"
        };
    }

    private string FormatValueForInput(TValue value)
    {
        // Reuse TypeTraits formatting for date/date-time
        if (value is null) return string.Empty;
        return TypeTraits<TValue>.FormatForInput(value, null, CultureInfo.InvariantCulture);
    }

    private void ToggleFilterUI()
    {
        _showFilterUI = !_showFilterUI;
        StateHasChanged();
    }

    // Now async to auto-apply
    private async Task OnOperatorChangedAsync(ChangeEventArgs e)
    {
        var operatorName = e.Value?.ToString();
        _selectedOperator = FilterOperators.FirstOrDefault(op => op.Name == operatorName);
        if (FilterableGrid is not null)
        {
            await FilterableGrid.OnFilterChangedAsync();
        }
        else
        {
            await OnFilterChanged.InvokeAsync(_hasFilterValue ? _filterValue : null);
            StateHasChanged();
        }
    }

    private async Task OnValueChangedAsync(ChangeEventArgs e)
    {
        try
        {
            // Parse using TypeTraits
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

        if (FilterableGrid is not null)
        {
            await FilterableGrid.OnFilterChangedAsync();
        }
        else
        {
            await OnFilterChanged.InvokeAsync(_hasFilterValue ? _filterValue : null);
            StateHasChanged();
        }
    }

    private async Task ApplyFilterAsync()
    {
        _showFilterUI = false;
        await OnFilterChanged.InvokeAsync(_hasFilterValue ? _filterValue : null);
        if (FilterableGrid is not null) await FilterableGrid.OnFilterChangedAsync(); else StateHasChanged();
    }

    public override async Task ClearFilterAsync()
    {
        _filterValue = default;
        _hasFilterValue = false;
        _showFilterUI = false;
        await OnFilterChanged.InvokeAsync(null);
        if (FilterableGrid is not null) await FilterableGrid.OnFilterChangedAsync(); else StateHasChanged();
    }

    public override IQueryable<TGridItem> ApplyFilter(IQueryable<TGridItem> items)
    {
        if (!HasActiveFilter || _selectedOperator is null || !_hasFilterValue || _filterValue is null) return items;
        return _selectedOperator.Apply(items, Property, _filterValue);
    }

    private List<IFilterOperator<TValue>> GetDefaultOperators()
    {
        // Build default operator set based on ValueKind
        if (s_kind == ValueKind.String)
        {
            return new List<IFilterOperator<TValue>>
            {
                (IFilterOperator<TValue>)(object)new StringContainsOperator(),
                (IFilterOperator<TValue>)(object)new StringEqualsOperator(),
                (IFilterOperator<TValue>)(object)new StringStartsWithOperator(),
                (IFilterOperator<TValue>)(object)new StringEndsWithOperator(),
            };
        }
        if (s_kind == ValueKind.DateTime)
        {
            return new List<IFilterOperator<TValue>>
            {
                (IFilterOperator<TValue>)(object)new DateEqualsOperator(),
                (IFilterOperator<TValue>)(object)new DateAfterOperator(),
                (IFilterOperator<TValue>)(object)new DateBeforeOperator(),
            };
        }
        if (s_kind == ValueKind.Boolean)
        {
            return new List<IFilterOperator<TValue>> { (IFilterOperator<TValue>)(object)new BooleanEqualsOperator() };
        }
        // Numeric comparable value kinds
        if (s_kind is ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single)
        {
            return new List<IFilterOperator<TValue>>
            {
                CreateNumericOperator<NumericEqualsOperator<TValue>>(),
                CreateNumericOperator<NumericNotEqualsOperator<TValue>>(),
                CreateNumericOperator<NumericGreaterThanOperator<TValue>>(),
                CreateNumericOperator<NumericGreaterThanOrEqualOperator<TValue>>(),
                CreateNumericOperator<NumericLessThanOperator<TValue>>(),
                CreateNumericOperator<NumericLessThanOrEqualOperator<TValue>>()
            };
        }
        return new List<IFilterOperator<TValue>>();
    }

    private IFilterOperator<TValue> CreateNumericOperator<TOperator>() where TOperator : IFilterOperator<TValue>, new() => new TOperator();
}
