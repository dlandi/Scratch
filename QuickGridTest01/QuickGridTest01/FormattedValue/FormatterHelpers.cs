using System.Globalization;

namespace QuickGridTest01.FormattedValue.Core;

/// <summary>
/// Utility methods for building value formatters
/// Provides common patterns used across multiple formatter types
/// </summary>
public static class FormatterHelpers
{
    /// <summary>
    /// Safely converts a value to a numeric type for formatting
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Double representation or null if conversion fails</returns>
    public static double? ToDouble(object? value)
    {
        if (value is null)
            return null;

        try
        {
            return value is IConvertible convertible
                ? convertible.ToDouble(CultureInfo.InvariantCulture)
                : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Safely converts a value to decimal for currency formatting
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Decimal representation or null if conversion fails</returns>
    public static decimal? ToDecimal(object? value)
    {
        if (value is null)
            return null;

        try
        {
            return value is IConvertible convertible
                ? convertible.ToDecimal(CultureInfo.InvariantCulture)
                : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Formats a nullable value with a fallback for null
    /// </summary>
    /// <param name="value">Value to format</param>
    /// <param name="formatter">Formatting function for non-null values</param>
    /// <param name="nullDisplay">String to display for null values (default: empty string)</param>
    /// <returns>Formatted string</returns>
    public static string FormatOrDefault(object? value, Func<object, string> formatter, string nullDisplay = "")
    {
        return value is null ? nullDisplay : formatter(value);
    }

    /// <summary>
    /// Applies format string to IFormattable value with culture support
    /// </summary>
    /// <param name="value">Value to format</param>
    /// <param name="format">Format string (e.g., "C2", "N0", "P1")</param>
    /// <param name="culture">Culture info for formatting (null uses current culture)</param>
    /// <returns>Formatted string or empty if value cannot be formatted</returns>
    public static string ApplyFormat(object? value, string format, CultureInfo? culture = null)
    {
        if (value is null)
            return string.Empty;

        if (value is IFormattable formattable)
            return formattable.ToString(format, culture ?? CultureInfo.CurrentCulture);

        return value.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Cleans a string by removing specified characters (useful for phone numbers, credit cards, etc.)
    /// </summary>
    /// <param name="value">String to clean</param>
    /// <param name="charsToRemove">Characters to remove</param>
    /// <returns>Cleaned string</returns>
    public static string CleanString(string? value, params char[] charsToRemove)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var cleaned = value;
        foreach (var ch in charsToRemove)
        {
            cleaned = cleaned.Replace(ch.ToString(), string.Empty);
        }

        return cleaned;
    }

    /// <summary>
    /// Validates that a value is within an expected length range
    /// </summary>
    /// <param name="value">String to validate</param>
    /// <param name="minLength">Minimum length (inclusive)</param>
    /// <param name="maxLength">Maximum length (inclusive)</param>
    /// <returns>True if length is valid</returns>
    public static bool IsValidLength(string? value, int minLength, int maxLength)
    {
        if (value is null)
            return false;

        return value.Length >= minLength && value.Length <= maxLength;
    }

    /// <summary>
    /// Creates a formatter that chains multiple formatters together
    /// </summary>
    /// <param name="formatters">Formatters to chain (first non-empty result wins)</param>
    /// <returns>Chained formatter function</returns>
    public static Func<object?, string> ChainFormatters(params Func<object?, string>[] formatters)
    {
        return value =>
        {
            foreach (var formatter in formatters)
            {
                var result = formatter(value);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            return string.Empty;
        };
    }

    /// <summary>
    /// Creates a formatter with a fallback value for null inputs
    /// </summary>
    /// <param name="formatter">Base formatter</param>
    /// <param name="nullDisplay">String to display for null values</param>
    /// <returns>Formatter with null handling</returns>
    public static Func<object?, string> WithNullDisplay(
        Func<object?, string> formatter, 
        string nullDisplay)
    {
        return value => value is null ? nullDisplay : formatter(value);
    }
}
