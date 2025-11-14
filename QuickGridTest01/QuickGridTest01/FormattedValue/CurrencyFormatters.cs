using System.Globalization;
using QuickGridTest01.FormattedValue.Core;

namespace QuickGridTest01.FormattedValue.Formatters;

/// <summary>
/// Currency formatting functions for financial data display
/// Provides culture-aware currency formatting with various styles
/// </summary>
public static class CurrencyFormatters
{
    /// <summary>
    /// Formats a numeric value as currency using the current culture
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats values as currency</returns>
    /// <example>
    /// var formatter = CurrencyFormatters.Currency();
    /// formatter(1234.56m) // Returns "$1,234.56" (in en-US culture)
    /// </example>
    public static Func<object?, string> Currency(int decimals = 2)
    {
        var format = $"C{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Formats a numeric value as currency using a specific culture
    /// </summary>
    /// <param name="culture">Culture to use for formatting</param>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats values as currency in the specified culture</returns>
    /// <example>
    /// var formatter = CurrencyFormatters.Currency(new CultureInfo("fr-FR"));
    /// formatter(1234.56m) // Returns "1 234,56 €"
    /// </example>
    public static Func<object?, string> Currency(CultureInfo culture, int decimals = 2)
    {
        var format = $"C{decimals}";
        return value => FormatterHelpers.ApplyFormat(value, format, culture);
    }

    /// <summary>
    /// Formats a numeric value as currency with accounting format (negatives in parentheses)
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats values with accounting style</returns>
    /// <example>
    /// var formatter = CurrencyFormatters.CurrencyAccounting();
    /// formatter(1234.56m)  // Returns "$1,234.56"
    /// formatter(-1234.56m) // Returns "($1,234.56)"
    /// </example>
    public static Func<object?, string> CurrencyAccounting(int decimals = 2)
    {
        return value =>
        {
            var decimalValue = FormatterHelpers.ToDecimal(value);
            if (!decimalValue.HasValue)
                return string.Empty;

            var absValue = Math.Abs(decimalValue.Value);
            var formatted = absValue.ToString($"N{decimals}", CultureInfo.CurrentCulture);
            
            if (decimalValue.Value < 0)
                return $"(${formatted})";
            
            return $"${formatted}";
        };
    }

    /// <summary>
    /// Formats a numeric value as currency with a custom symbol
    /// </summary>
    /// <param name="symbol">Currency symbol to use (e.g., "$", "€", "£")</param>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats values with custom currency symbol</returns>
    /// <example>
    /// var formatter = CurrencyFormatters.CurrencyWithSymbol("€");
    /// formatter(1234.56m) // Returns "€1,234.56"
    /// </example>
    public static Func<object?, string> CurrencyWithSymbol(string symbol, int decimals = 2)
    {
        return value =>
        {
            var decimalValue = FormatterHelpers.ToDecimal(value);
            if (!decimalValue.HasValue)
                return string.Empty;

            var formatted = decimalValue.Value.ToString($"N{decimals}", CultureInfo.CurrentCulture);
            return $"{symbol}{formatted}";
        };
    }
}
