using System.Globalization;
using QuickGridTest01.FormattedValue.Core;

namespace QuickGridTest01.FormattedValue.Formatters;

/// <summary>
/// Numeric formatting functions for various number display styles
/// Provides standard and specialized numeric formatting
/// </summary>
public static class NumericFormatters
{
    /// <summary>
    /// Formats a number with thousands separators
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats numbers with thousand separators</returns>
    /// <example>
    /// var formatter = NumericFormatters.Number();
    /// formatter(1234567.89) // Returns "1,234,567.89"
    /// </example>
    public static Func<object?, string> Number(int decimals = 2)
    {
        var format = $"N{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format);
    }

    /// <summary>
    /// Formats a number with fixed decimal places (no thousands separator)
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats numbers with fixed decimals</returns>
    /// <example>
    /// var formatter = NumericFormatters.FixedPoint();
    /// formatter(1234.5) // Returns "1234.50"
    /// </example>
    public static Func<object?, string> FixedPoint(int decimals = 2)
    {
        var format = $"F{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format);
    }

    /// <summary>
    /// Formats a number in scientific notation
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats numbers in scientific notation</returns>
    /// <example>
    /// var formatter = NumericFormatters.Scientific();
    /// formatter(1234567.89) // Returns "1.23E+006"
    /// </example>
    public static Func<object?, string> Scientific(int decimals = 2)
    {
        var format = $"E{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format);
    }

    /// <summary>
    /// Formats large numbers in compact form (1.2M, 3.4K, etc.)
    /// </summary>
    /// <param name="decimals">Number of decimal places for the compact value (default: 1)</param>
    /// <returns>Formatter function that formats numbers in compact notation</returns>
    /// <example>
    /// var formatter = NumericFormatters.CompactNumber();
    /// formatter(1234567)   // Returns "1.2M"
    /// formatter(1234)      // Returns "1.2K"
    /// formatter(123)       // Returns "123.0"
    /// </example>
    public static Func<object?, string> CompactNumber(int decimals = 1)
    {
        return value =>
        {
            var number = FormatterHelpers.ToDouble(value);
            if (!number.HasValue)
                return string.Empty;

            var absNumber = Math.Abs(number.Value);
            var sign = number.Value < 0 ? "-" : "";

            if (absNumber >= 1_000_000_000)
                return $"{sign}{(absNumber / 1_000_000_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}B";
            if (absNumber >= 1_000_000)
                return $"{sign}{(absNumber / 1_000_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}M";
            if (absNumber >= 1_000)
                return $"{sign}{(absNumber / 1_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}K";
            
            return $"{sign}{absNumber.ToString($"F{decimals}", CultureInfo.CurrentCulture)}";
        };
    }

    /// <summary>
    /// Formats a decimal (0.0-1.0) as a percentage
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats decimal values as percentages</returns>
    /// <example>
    /// var formatter = NumericFormatters.Percentage();
    /// formatter(0.1234) // Returns "12.34%"
    /// formatter(1.0)    // Returns "100.00%"
    /// </example>
    public static Func<object?, string> Percentage(int decimals = 2)
    {
        var format = $"P{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format);
    }

    /// <summary>
    /// Formats a whole number (0-100) as a percentage
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats whole numbers as percentages</returns>
    /// <example>
    /// var formatter = NumericFormatters.PercentageFromWhole();
    /// formatter(12.34) // Returns "12.34%"
    /// formatter(100)   // Returns "100.00%"
    /// </example>
    public static Func<object?, string> PercentageFromWhole(int decimals = 2)
    {
        return value =>
        {
            var number = FormatterHelpers.ToDouble(value);
            if (!number.HasValue)
                return string.Empty;

            return (number.Value / 100.0).ToString($"P{decimals}", CultureInfo.CurrentCulture);
        };
    }

    /// <summary>
    /// Formats a number with a custom prefix and suffix
    /// </summary>
    /// <param name="prefix">Text to prepend (e.g., "$", "±")</param>
    /// <param name="suffix">Text to append (e.g., "%", "kg", "m²")</param>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats numbers with custom prefix/suffix</returns>
    /// <example>
    /// var formatter = NumericFormatters.NumberWithPrefixSuffix("±", "kg");
    /// formatter(12.345) // Returns "±12.35kg"
    /// </example>
    public static Func<object?, string> NumberWithPrefixSuffix(
        string prefix = "", 
        string suffix = "", 
        int decimals = 2)
    {
        return value =>
        {
            var number = FormatterHelpers.ToDouble(value);
            if (!number.HasValue)
                return string.Empty;

            var formatted = number.Value.ToString($"F{decimals}", CultureInfo.CurrentCulture);
            return $"{prefix}{formatted}{suffix}";
        };
    }
}
