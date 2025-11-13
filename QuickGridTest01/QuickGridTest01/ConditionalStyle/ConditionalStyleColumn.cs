using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace QuickGridTest01.ConditionalStyling;

public sealed class ConditionalStyleColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    [Parameter, EditorRequired] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public List<StyleRule<TValue>> Rules { get; set; } = new();
    [Parameter] public string? Format { get; set; }
    [Parameter] public bool ShowIcons { get; set; } = true;
    [Parameter] public bool ShowTooltips { get; set; } = true;
    [Parameter] public Func<TValue, string>? ValueFormatter { get; set; }
    [Parameter] public bool CombineMultipleMatches { get; set; }
    [Parameter] public string? BaseCssClass { get; set; }
    [Parameter] public RenderFragment<(TGridItem Item, TValue Value, StyleRuleResult Style)>? CellTemplate { get; set; }

    private Func<TGridItem, TValue>? _compiledAccessor;
    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, string>? _textAccessor;

    public override GridSort<TGridItem>? SortBy { get; set; }

    protected override void OnParametersSet()
    {
        if (Property == null) throw new InvalidOperationException("Property is required.");
        if (Title is null && Property.Body is MemberExpression m) Title = m.Member.Name;
        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledAccessor = Property.Compile();
            SortBy = GridSort<TGridItem>.ByAscending(Property);
            _textAccessor = BuildTextAccessor();
        }
    }

    private Func<TGridItem, string> BuildTextAccessor()
    {
        if (ValueFormatter is not null)
            return item => ValueFormatter(_compiledAccessor!(item));
        if (!string.IsNullOrEmpty(Format))
        {
            var underlying = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (!typeof(IFormattable).IsAssignableFrom(underlying))
                throw new InvalidOperationException($"Format requires IFormattable for type {typeof(TValue)}");
            return item => ((IFormattable?)(object?)_compiledAccessor!(item))?.ToString(Format, null) ?? string.Empty;
        }
        return item => _compiledAccessor!(item)?.ToString() ?? string.Empty;
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var value = _compiledAccessor!(item);
        var style = CombineMultipleMatches ? StyleRuleEvaluator.EvaluateAll(value, Rules) : StyleRuleEvaluator.Evaluate(value, Rules);
        if (CellTemplate is not null)
        {
            builder.AddContent(0, CellTemplate((item, value, style)));
            return;
        }
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", BuildCssClass(style));
        if (ShowTooltips && !string.IsNullOrEmpty(style.Tooltip)) builder.AddAttribute(seq++, "title", style.Tooltip);
        if (ShowIcons && !string.IsNullOrEmpty(style.IconClass))
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", $"cell-icon {style.IconClass}");
            builder.AddAttribute(seq++, "aria-hidden", "true");
            builder.CloseElement();
        }
        builder.OpenElement(seq++, "span");
        builder.AddAttribute(seq++, "class", "cell-value");
        builder.AddContent(seq++, _textAccessor!(item));
        builder.CloseElement();
        builder.CloseElement();
    }

    private string BuildCssClass(StyleRuleResult style)
    {
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(BaseCssClass)) parts.Add(BaseCssClass);
        if (style.HasMatch && !string.IsNullOrEmpty(style.CssClass)) parts.Add(style.CssClass);
        parts.Add("conditional-cell");
        return string.Join(" ", parts);
    }

    protected override bool IsSortableByDefault() => SortBy is not null;
}
