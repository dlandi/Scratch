using System.Globalization;

namespace QuickGridTest01.FormattedValue.Formatters;

/// <summary>
/// Date and time formatting functions for temporal data display
/// Provides various date, time, and duration formatting styles
/// </summary>
public static class DateTimeFormatters
{
    /// <summary>
    /// Formats a date in short format (e.g., 1/15/2025)
    /// </summary>
    /// <returns>Formatter function that formats dates in short format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.ShortDate();
    /// formatter(new DateTime(2025, 1, 15)) // Returns "1/15/2025" (en-US)
    /// </example>
    public static Func<object?, string> ShortDate()
    {
        return value => value is DateTime dt
            ? dt.ToString("d", CultureInfo.CurrentCulture)
            : string.Empty;
    }

    /// <summary>
    /// Formats a date in long format (e.g., Wednesday, January 15, 2025)
    /// </summary>
    /// <returns>Formatter function that formats dates in long format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.LongDate();
    /// formatter(new DateTime(2025, 1, 15)) // Returns "Wednesday, January 15, 2025"
    /// </example>
    public static Func<object?, string> LongDate()
    {
        return value => value is DateTime dt
            ? dt.ToString("D", CultureInfo.CurrentCulture)
            : string.Empty;
    }

    /// <summary>
    /// Formats a date and time in custom format
    /// </summary>
    /// <param name="format">DateTime format string (default: "g" for general short)</param>
    /// <returns>Formatter function that formats dates with the specified format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.DateTime("yyyy-MM-dd HH:mm");
    /// formatter(new DateTime(2025, 1, 15, 14, 30, 0)) // Returns "2025-01-15 14:30"
    /// </example>
    public static Func<object?, string> DateTime(string format = "g")
    {
        return value => value is System.DateTime dt
            ? dt.ToString(format, CultureInfo.CurrentCulture)
            : string.Empty;
    }

    /// <summary>
    /// Formats a date as ISO 8601 (2025-01-15)
    /// </summary>
    /// <returns>Formatter function that formats dates in ISO 8601 format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.IsoDate();
    /// formatter(new DateTime(2025, 1, 15)) // Returns "2025-01-15"
    /// </example>
    public static Func<object?, string> IsoDate()
    {
        return value => value is System.DateTime dt
            ? dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : string.Empty;
    }

    /// <summary>
    /// Formats a date as relative time (Today, Yesterday, 3 days ago, etc.)
    /// </summary>
    /// <returns>Formatter function that formats dates relative to today</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.RelativeDate();
    /// formatter(DateTime.Now)                  // Returns "Today"
    /// formatter(DateTime.Now.AddDays(-1))      // Returns "Yesterday"
    /// formatter(DateTime.Now.AddDays(-3))      // Returns "3 days ago"
    /// formatter(DateTime.Now.AddDays(2))       // Returns "In 2 days"
    /// formatter(DateTime.Now.AddDays(-30))     // Returns "Jan 15, 2025"
    /// </example>
    public static Func<object?, string> RelativeDate()
    {
        return value =>
        {
            if (value is not System.DateTime dt)
                return string.Empty;

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
    /// <returns>Formatter function that formats TimeSpan as HH:mm:ss</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.Duration();
    /// formatter(new TimeSpan(2, 30, 45)) // Returns "02:30:45"
    /// formatter(new TimeSpan(0, 5, 10))  // Returns "00:05:10"
    /// </example>
    public static Func<object?, string> Duration()
    {
        return value => value is TimeSpan ts
            ? ts.ToString(@"hh\:mm\:ss")
            : string.Empty;
    }

    /// <summary>
    /// Formats a time duration in human-readable format (1h 23m, 45s, etc.)
    /// </summary>
    /// <returns>Formatter function that formats TimeSpan in human-readable form</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.HumanDuration();
    /// formatter(new TimeSpan(1, 23, 45))  // Returns "1h 23m"
    /// formatter(new TimeSpan(0, 5, 10))   // Returns "5m 10s"
    /// formatter(new TimeSpan(0, 0, 45))   // Returns "45s"
    /// formatter(new TimeSpan(25, 0, 0))   // Returns "1d 1h"
    /// </example>
    public static Func<object?, string> HumanDuration()
    {
        return value =>
        {
            if (value is not TimeSpan ts)
                return string.Empty;

            if (ts.TotalDays >= 1)
                return $"{(int)ts.TotalDays}d {ts.Hours}h";
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            if (ts.TotalMinutes >= 1)
                return $"{(int)ts.TotalMinutes}m {ts.Seconds}s";
            
            return $"{(int)ts.TotalSeconds}s";
        };
    }

    /// <summary>
    /// Formats time only (no date) in 12-hour format with AM/PM
    /// </summary>
    /// <returns>Formatter function that formats time in 12-hour format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.Time12Hour();
    /// formatter(new DateTime(2025, 1, 15, 14, 30, 0)) // Returns "2:30 PM"
    /// formatter(new DateTime(2025, 1, 15, 9, 15, 0))  // Returns "9:15 AM"
    /// </example>
    public static Func<object?, string> Time12Hour()
    {
        return value => value is System.DateTime dt
            ? dt.ToString("h:mm tt", CultureInfo.CurrentCulture)
            : string.Empty;
    }

    /// <summary>
    /// Formats time only (no date) in 24-hour format
    /// </summary>
    /// <returns>Formatter function that formats time in 24-hour format</returns>
    /// <example>
    /// var formatter = DateTimeFormatters.Time24Hour();
    /// formatter(new DateTime(2025, 1, 15, 14, 30, 0)) // Returns "14:30"
    /// formatter(new DateTime(2025, 1, 15, 9, 15, 0))  // Returns "09:15"
    /// </example>
    public static Func<object?, string> Time24Hour()
    {
        return value => value is System.DateTime dt
            ? dt.ToString("HH:mm", CultureInfo.CurrentCulture)
            : string.Empty;
    }
}
