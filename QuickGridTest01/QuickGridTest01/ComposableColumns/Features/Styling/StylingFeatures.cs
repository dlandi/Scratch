using Microsoft.AspNetCore.Components.Rendering;

namespace QuickGridTest01.ComposableColumns.Features.Styling;

using ComposableColumns.Core;

/// <summary>
/// Feature that adds a tooltip to the cell.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class TooltipFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Styling;

    /// <summary>
    /// Function to generate the tooltip text from the value.
    /// </summary>
    public Func<TValue, string>? TooltipMapper { get; set; }

    /// <summary>
    /// Static tooltip text (used if TooltipMapper is not set).
    /// </summary>
    public string? Tooltip { get; set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // Nothing to do on attach
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // Nothing to clean up
    }

    public void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext)
    {
        var tooltipText = GetTooltipText(item, context);

        builder.OpenElement(sequence++, "span");
        if (!string.IsNullOrEmpty(tooltipText))
        {
            builder.AddAttribute(sequence++, "title", tooltipText);
        }
        renderNext();
        builder.CloseElement();
    }

    private string? GetTooltipText(TGridItem item, FeatureContext<TGridItem> context)
    {
        if (TooltipMapper is not null && context is FeatureContext<TGridItem, TValue> typedContext && typedContext.GetValue is not null)
        {
            var value = typedContext.GetValue(item);
            return TooltipMapper(value);
        }

        return Tooltip;
    }
}

/// <summary>
/// Feature that displays an icon based on the value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class IconFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Styling;

    /// <summary>
    /// Function to map a value to an icon CSS class.
    /// </summary>
    public Func<TValue, string>? IconMapper { get; set; }

    /// <summary>
    /// Function to map a value to an icon color.
    /// </summary>
    public Func<TValue, string>? ColorMapper { get; set; }

    /// <summary>
    /// Whether to show the value text alongside the icon.
    /// </summary>
    public bool ShowValue { get; set; } = true;

    /// <summary>
    /// Spacing between icon and value (e.g., "0.5rem").
    /// </summary>
    public string IconSpacing { get; set; } = "0.5rem";

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // Nothing to do on attach
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // Nothing to clean up
    }

    public void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext)
    {
        if (IconMapper is null || context is not FeatureContext<TGridItem, TValue> typedContext || typedContext.GetValue is null)
        {
            renderNext();
            return;
        }

        var value = typedContext.GetValue(item);
        var iconClass = IconMapper(value);
        var color = ColorMapper?.Invoke(value);

        builder.OpenElement(sequence++, "span");
        builder.AddAttribute(sequence++, "class", "icon-cell-container");
        builder.AddAttribute(sequence++, "style", "display: inline-flex; align-items: center; gap: " + IconSpacing);

        // Render icon
        builder.OpenElement(sequence++, "i");
        builder.AddAttribute(sequence++, "class", iconClass);
        if (!string.IsNullOrEmpty(color))
        {
            builder.AddAttribute(sequence++, "style", $"color: {color}");
        }
        builder.AddAttribute(sequence++, "aria-hidden", "true");
        builder.CloseElement(); // i

        // Optionally render value
        if (ShowValue)
        {
            builder.OpenElement(sequence++, "span");
            renderNext();
            builder.CloseElement();
        }

        builder.CloseElement(); // span.icon-cell-container
    }
}

/// <summary>
/// Feature that applies conditional CSS classes based on the value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class ConditionalCssFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Styling;

    /// <summary>
    /// Base CSS class always applied.
    /// </summary>
    public string? BaseClass { get; set; }

    /// <summary>
    /// List of CSS rules to evaluate.
    /// </summary>
    public List<CssRule<TValue>> Rules { get; set; } = [];

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // Nothing to do on attach
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // Nothing to clean up
    }

    public void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext)
    {
        var cssClass = BuildCssClass(item, context);

        builder.OpenElement(sequence++, "span");
        if (!string.IsNullOrEmpty(cssClass))
        {
            builder.AddAttribute(sequence++, "class", cssClass);
        }
        renderNext();
        builder.CloseElement();
    }

    private string BuildCssClass(TGridItem item, FeatureContext<TGridItem> context)
    {
        var classes = new List<string>();

        if (!string.IsNullOrEmpty(BaseClass))
        {
            classes.Add(BaseClass);
        }

        if (context is FeatureContext<TGridItem, TValue> typedContext && typedContext.GetValue is not null)
        {
            var value = typedContext.GetValue(item);
            foreach (var rule in Rules)
            {
                if (rule.Condition(value))
                {
                    classes.Add(rule.CssClass);
                    if (!rule.CombineWithOthers)
                        break;
                }
            }
        }

        return string.Join(" ", classes);
    }
}

/// <summary>
/// A rule that applies a CSS class when a condition is met.
/// </summary>
/// <typeparam name="TValue">The type of the value being evaluated.</typeparam>
public class CssRule<TValue>
{
    /// <summary>
    /// Condition that determines if this rule applies.
    /// </summary>
    public required Func<TValue, bool> Condition { get; init; }

    /// <summary>
    /// CSS class to apply when condition is true.
    /// </summary>
    public required string CssClass { get; init; }

    /// <summary>
    /// If false, stop evaluating rules after this one matches.
    /// </summary>
    public bool CombineWithOthers { get; init; } = false;
}
