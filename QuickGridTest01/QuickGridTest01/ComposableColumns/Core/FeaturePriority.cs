namespace QuickGridTest01.ComposableColumns.Core;

/// <summary>
/// Constants for feature execution priority.
/// Lower values execute first in the render pipeline.
/// </summary>
public static class FeaturePriority
{
    /// <summary>
    /// Infrastructure features that must run first (e.g., property expression, compiled accessor).
    /// </summary>
    public const int Infrastructure = 0;

    /// <summary>
    /// Core features like type traits, auto-title inference.
    /// </summary>
    public const int Core = 100;

    /// <summary>
    /// Filtering features (filter state, filter application).
    /// </summary>
    public const int Filtering = 150;

    /// <summary>
    /// Formatting features (format string, custom formatter, culture).
    /// </summary>
    public const int Formatting = 200;

    /// <summary>
    /// Styling features (conditional CSS, icons, tooltips).
    /// </summary>
    public const int Styling = 300;

    /// <summary>
    /// Editing features (inline editing, edit state, debounce).
    /// </summary>
    public const int Editing = 400;

    /// <summary>
    /// Validation features (validators, data annotations).
    /// </summary>
    public const int Validation = 500;

    /// <summary>
    /// Event features (value changed, state changed, before edit).
    /// </summary>
    public const int Events = 600;

    /// <summary>
    /// Performance optimization features (memoization, minimal DOM, set key).
    /// </summary>
    public const int Performance = 700;

    /// <summary>
    /// Final wrapper features that should run last.
    /// </summary>
    public const int Final = 1000;
}
