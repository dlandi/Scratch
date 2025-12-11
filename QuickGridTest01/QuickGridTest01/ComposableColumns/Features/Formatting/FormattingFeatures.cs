using Microsoft.AspNetCore.Components.Rendering;

namespace QuickGridTest01.ComposableColumns.Features.Formatting;

using ComposableColumns.Core;

/// <summary>
/// Feature that applies a format string to the value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class FormatStringFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Formatting;

    /// <summary>
    /// The format string to apply (e.g., "C2", "N0", "yyyy-MM-dd").
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// The format provider (culture) to use. Defaults to current culture.
    /// </summary>
    public IFormatProvider? FormatProvider { get; set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        if (context is FeatureContext<TGridItem, TValue> typedContext && !string.IsNullOrEmpty(Format))
        {
            typedContext.Format = Format;
        }
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
        if (context is not FeatureContext<TGridItem, TValue> typedContext || typedContext.GetValue is null)
        {
            renderNext();
            return;
        }

        var value = typedContext.GetValue(item);

        if (value is IFormattable formattable && !string.IsNullOrEmpty(Format))
        {
            var formatted = formattable.ToString(Format, FormatProvider);
            builder.AddContent(sequence++, formatted);
        }
        else
        {
            renderNext();
        }
    }
}

/// <summary>
/// Feature that applies a custom formatter function to the value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class CustomFormatterFeature<TGridItem, TValue> : ICellRenderFeature<TGridItem>
{
    public int Priority => FeaturePriority.Formatting;

    /// <summary>
    /// The formatter function to apply.
    /// </summary>
    public Func<TValue, string>? Formatter { get; set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        if (context is FeatureContext<TGridItem, TValue> typedContext && Formatter is not null)
        {
            typedContext.Formatter = Formatter;
        }
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
        if (Formatter is null || context is not FeatureContext<TGridItem, TValue> typedContext || typedContext.GetValue is null)
        {
            renderNext();
            return;
        }

        var value = typedContext.GetValue(item);
        var formatted = Formatter(value);
        builder.AddContent(sequence++, formatted);
    }
}
