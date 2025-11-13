using System.Globalization;

namespace QuickGridTest01.Formatters;

/// <summary>
/// Common value formatters for use with FormattedValueColumn
/// Provides reusable, testable formatting functions for common scenarios
/// </summary>
public static class ValueFormatters
{
    #region Currency Formatters

    /// <summary>
    /// Formats a decimal as currency using the current culture
    /// </summary>
    public static Func<object?, string> Currency(int decimals = 2)
    {
        var format = $"C{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a decimal as currency using a specific culture
    /// </summary>
    public static Func<object?, string> Currency(CultureInfo culture, int decimals = 2)
    {
        var format = $"C{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, culture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a decimal as currency with accounting format (negatives in parentheses)
    /// </summary>
    public static Func<object?, string> CurrencyAccounting(int decimals = 2)
    {
        var positiveFormat = new string('0', decimals > 0 ? decimals : 1);
        if (decimals > 0)
            positiveFormat = "0." + positiveFormat;
        
        var format = $"${positiveFormat.Replace("0", "#,##0")};(${positiveFormat.Replace("0", "#,##0")})";
        
        return value => value is decimal d
            ? d.ToString(format, CultureInfo.InvariantCulture)
            : value?.ToString() ?? string.Empty;
    }

    #endregion

    #region Percentage Formatters

    /// <summary>
    /// Formats a decimal (0.0-1.0) as a percentage
    /// </summary>
    public static Func<object?, string> Percentage(int decimals = 2)
    {
        var format = $"P{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a whole number (0-100) as a percentage
    /// </summary>
    public static Func<object?, string> PercentageFromWhole(int decimals = 2)
    {
        return value =>
        {
            if (value is decimal d)
                return (d / 100m).ToString($"P{decimals}", CultureInfo.CurrentCulture);
            if (value is double dbl)
                return (dbl / 100.0).ToString($"P{decimals}", CultureInfo.CurrentCulture);
            if (value is int i)
                return (i / 100.0).ToString($"P{decimals}", CultureInfo.CurrentCulture);
            return value?.ToString() ?? string.Empty;
        };
    }

    #endregion

    #region Number Formatters

    /// <summary>
    /// Formats a number with thousands separators
    /// </summary>
    public static Func<object?, string> Number(int decimals = 2)
    {
        var format = $"N{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a number with fixed decimal places (no thousands separator)
    /// </summary>
    public static Func<object?, string> FixedPoint(int decimals = 2)
    {
        var format = $"F{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a number in scientific notation
    /// </summary>
    public static Func<object?, string> Scientific(int decimals = 2)
    {
        var format = $"E{decimals}";
        return value => value is IFormattable formattable
            ? formattable.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats large numbers in compact form (1.2M, 3.4K, etc.)
    /// </summary>
    public static Func<object?, string> CompactNumber(int decimals = 1)
    {
        return value =>
        {
            if (value is not IConvertible convertible)
                return value?.ToString() ?? string.Empty;

            var number = convertible.ToDouble(CultureInfo.InvariantCulture);
            var absNumber = Math.Abs(number);
            var sign = number < 0 ? "-" : "";

            if (absNumber >= 1_000_000_000)
                return $"{sign}{(absNumber / 1_000_000_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}B";
            if (absNumber >= 1_000_000)
                return $"{sign}{(absNumber / 1_000_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}M";
            if (absNumber >= 1_000)
                return $"{sign}{(absNumber / 1_000).ToString($"F{decimals}", CultureInfo.CurrentCulture)}K";
            
            return $"{sign}{absNumber.ToString($"F{decimals}", CultureInfo.CurrentCulture)}";
        };
    }

    #endregion

    #region Date/Time Formatters

    /// <summary>
    /// Formats a date in short format (1/15/2025)
    /// </summary>
    public static Func<object?, string> ShortDate()
    {
        return value => value is DateTime dt
            ? dt.ToString("d", CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a date in long format (Wednesday, January 15, 2025)
    /// </summary>
    public static Func<object?, string> LongDate()
    {
        return value => value is DateTime dt
            ? dt.ToString("D", CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a date and time in custom format
    /// </summary>
    public static Func<object?, string> DateTime(string format = "g")
    {
        return value => value is System.DateTime dt
            ? dt.ToString(format, CultureInfo.CurrentCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a date as ISO 8601 (2025-01-15)
    /// </summary>
    public static Func<object?, string> IsoDate()
    {
        return value => value is System.DateTime dt
            ? dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a date as relative time (Today, Yesterday, 3 days ago, etc.)
    /// </summary>
    public static Func<object?, string> RelativeDate()
    {
        return value =>
        {
            if (value is not System.DateTime dt)
                return value?.ToString() ?? string.Empty;

            var now = System.DateTime.Now.Date;
            var date = dt.Date;
            var days = (now - date).Days;

            return days switch
            {
                0 => "Today",
                1 => "Yesterday",
                -1 => "Tomorrow",
                > 0 and < 7 => $"{days} days ago",
                < 0 and > -7 => $"In {Math.Abs(days)} days",
                _ => dt.ToString("MMM d, yyyy", CultureInfo.CurrentCulture)
            };
        };
    }

    /// <summary>
    /// Formats a time duration (TimeSpan) as HH:mm:ss
    /// </summary>
    public static Func<object?, string> Duration()
    {
        return value => value is TimeSpan ts
            ? ts.ToString(@"hh\:mm\:ss")
            : value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Formats a time duration in human-readable format (1h 23m, 45s, etc.)
    /// </summary>
    public static Func<object?, string> HumanDuration()
    {
        return value =>
        {
            if (value is not TimeSpan ts)
                return value?.ToString() ?? string.Empty;

            if (ts.TotalDays >= 1)
                return $"{(int)ts.TotalDays}d {ts.Hours}h";
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            if (ts.TotalMinutes >= 1)
                return $"{(int)ts.TotalMinutes}m {ts.Seconds}s";
            
            return $"{(int)ts.TotalSeconds}s";
        };
    }

    #endregion

    #region File Size Formatters

    /// <summary>
    /// Formats a byte count as file size (1.46 MB, 3.2 GB, etc.)
    /// </summary>
    public static Func<object?, string> FileSize(int decimals = 2)
    {
        return value =>
        {
            if (value is not IConvertible convertible)
                return value?.ToString() ?? string.Empty;

            var bytes = convertible.ToInt64(CultureInfo.InvariantCulture);
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len.ToString($"F{decimals}", CultureInfo.CurrentCulture)} {sizes[order]}";
        };
    }

    #endregion

    #region Phone Number Formatters

    /// <summary>
    /// Formats a 10-digit phone number as (555) 123-4567
    /// </summary>
    public static Func<object?, string> PhoneNumberUs()
    {
        return value =>
        {
            var phone = value?.ToString()?.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "") ?? "";
            
            if (phone.Length == 10)
                return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6)}";
            
            if (phone.Length == 11 && phone[0] == '1')
                return $"({phone.Substring(1, 3)}) {phone.Substring(4, 3)}-{phone.Substring(7)}";
            
            return phone;
        };
    }

    /// <summary>
    /// Formats an 11-digit phone number as +1 555-123-4567
    /// </summary>
    public static Func<object?, string> PhoneNumberInternational()
    {
        return value =>
        {
            var phone = value?.ToString()?.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("+", "") ?? "";
            
            if (phone.Length == 11 && phone[0] == '1')
                return $"+{phone[0]} {phone.Substring(1, 3)}-{phone.Substring(4, 3)}-{phone.Substring(7)}";
            
            if (phone.Length == 10)
                return $"+1 {phone.Substring(0, 3)}-{phone.Substring(3, 3)}-{phone.Substring(6)}";
            
            return phone;
        };
    }

    #endregion

    #region Masked Data Formatters

    /// <summary>
    /// Masks a credit card number showing only last 4 digits
    /// </summary>
    public static Func<object?, string> CreditCardMasked()
    {
        return value =>
        {
            var card = value?.ToString()?.Replace("-", "").Replace(" ", "") ?? "";
            
            if (card.Length >= 4)
                return $"****-****-****-{card.Substring(card.Length - 4)}";
            
            return card;
        };
    }

    /// <summary>
    /// Formats an SSN as 123-45-6789
    /// </summary>
    public static Func<object?, string> SocialSecurityNumber()
    {
        return value =>
        {
            var ssn = value?.ToString()?.Replace("-", "") ?? "";
            
            if (ssn.Length == 9)
                return $"{ssn.Substring(0, 3)}-{ssn.Substring(3, 2)}-{ssn.Substring(5)}";
            
            return ssn;
        };
    }

    /// <summary>
    /// Masks an SSN showing only last 4 digits
    /// </summary>
    public static Func<object?, string> SocialSecurityNumberMasked()
    {
        return value =>
        {
            var ssn = value?.ToString()?.Replace("-", "") ?? "";
            
            if (ssn.Length == 9)
                return $"***-**-{ssn.Substring(5)}";
            
            return ssn;
        };
    }

    #endregion

    #region Boolean Formatters

    /// <summary>
    /// Formats a boolean as Yes/No
    /// </summary>
    public static Func<object?, string> YesNo()
    {
        return value => value is bool b ? (b ? "Yes" : "No") : string.Empty;
    }

    /// <summary>
    /// Formats a boolean as ✓/✗
    /// </summary>
    public static Func<object?, string> CheckMark()
    {
        return value => value is bool b ? (b ? "✓" : "✗") : string.Empty;
    }

    /// <summary>
    /// Formats a boolean as Active/Inactive
    /// </summary>
    public static Func<object?, string> ActiveInactive()
    {
        return value => value is bool b ? (b ? "Active" : "Inactive") : string.Empty;
    }

    #endregion

    #region Conditional Formatters

    /// <summary>
    /// Returns a CSS class based on numeric value thresholds
    /// </summary>
    public static Func<object?, string> ConditionalNumber(double lowThreshold, double highThreshold)
    {
        return value =>
        {
            if (value is not IConvertible convertible)
                return string.Empty;

            var number = convertible.ToDouble(CultureInfo.InvariantCulture);
            
            if (number < lowThreshold)
                return "value-low";
            if (number > highThreshold)
                return "value-high";
            
            return "value-normal";
        };
    }

    /// <summary>
    /// Returns a CSS class based on date (past, present, future)
    /// </summary>
    public static Func<object?, string> ConditionalDate()
    {
        return value =>
        {
            if (value is not System.DateTime dt)
                return string.Empty;

            var now = System.DateTime.Now.Date;
            var date = dt.Date;

            if (date < now)
                return "date-past";
            if (date > now)
                return "date-future";
            
            return "date-today";
        };
    }

    #endregion
}
