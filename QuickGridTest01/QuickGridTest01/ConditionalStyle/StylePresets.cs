using System;
using System.Collections.Generic;

namespace QuickGridTest01.ConditionalStyling;

/// <summary>
/// Pre-built styling rule sets for common scenarios.
/// These presets provide ready-to-use styling configurations that follow best practices.
/// </summary>
public static class StylePresets
{
    #region Numeric Thresholds

    /// <summary>
    /// Creates threshold-based styling for numeric values with danger/warning/success levels
    /// </summary>
    /// <param name="dangerBelow">Values below this are styled as danger (red)</param>
    /// <param name="warningBelow">Values below this (but above danger) are styled as warning (yellow)</param>
    /// <param name="successAbove">Values above this are styled as success (green)</param>
    public static List<StyleRule<decimal>> NumericThreshold(
        decimal dangerBelow,
        decimal warningBelow,
        decimal? successAbove = null)
    {
        var builder = new StyleRuleBuilder<decimal>();

        builder
            .WhenLessThan(dangerBelow, "cell-danger", priority: 30, icon: "icon-error", tooltip: $"Below threshold: {dangerBelow:N2}")
            .WhenLessThan(warningBelow, "cell-warning", priority: 20, icon: "icon-warning", tooltip: $"Below target: {warningBelow:N2}");

        if (successAbove.HasValue)
        {
            builder.WhenGreaterThan(successAbove.Value, "cell-success", priority: 10, icon: "icon-check", tooltip: $"Exceeds target: {successAbove.Value:N2}");
        }

        return builder.Build();
    }

    /// <summary>
    /// Creates threshold-based styling for integer values
    /// </summary>
    public static List<StyleRule<int>> NumericThreshold(
        int dangerBelow,
        int warningBelow,
        int? successAbove = null)
    {
        var builder = new StyleRuleBuilder<int>();

        builder
            .WhenLessThan(dangerBelow, "cell-danger", priority: 30, icon: "icon-error", tooltip: $"Critical: Below {dangerBelow}")
            .WhenLessThan(warningBelow, "cell-warning", priority: 20, icon: "icon-warning", tooltip: $"Low: Below {warningBelow}");

        if (successAbove.HasValue)
        {
            builder.WhenGreaterThan(successAbove.Value, "cell-success", priority: 10, icon: "icon-check", tooltip: $"Excellent: Above {successAbove.Value}");
        }

        return builder.Build();
    }

    /// <summary>
    /// Creates percentage-based styling with color coding
    /// Typical ranges: 0-33% = red, 33-66% = yellow, 66-100% = green
    /// </summary>
    public static List<StyleRule<decimal>> PercentageIndicator(
        decimal lowThreshold = 33m,
        decimal mediumThreshold = 66m)
    {
        return new StyleRuleBuilder<decimal>()
            .WhenLessThan(lowThreshold, "percentage-low", priority: 30, icon: "icon-arrow-down")
            .WhenBetween(lowThreshold, mediumThreshold, "percentage-medium", priority: 20, icon: "icon-minus")
            .WhenGreaterThan(mediumThreshold, "percentage-high", priority: 10, icon: "icon-arrow-up")
            .Build();
    }

    /// <summary>
    /// Creates profit/loss styling for financial data
    /// </summary>
    public static List<StyleRule<decimal>> ProfitLoss()
    {
        return new StyleRuleBuilder<decimal>()
            .WhenNegative("financial-loss", priority: 30, icon: "icon-trending-down", tooltip: "Loss")
            .WhenZero("financial-breakeven", priority: 20, icon: "icon-minus", tooltip: "Break-even")
            .WhenPositive("financial-profit", priority: 10, icon: "icon-trending-up", tooltip: "Profit")
            .Build();
    }

    /// <summary>
    /// Creates inventory level styling with stock warnings
    /// </summary>
    public static List<StyleRule<int>> InventoryLevel(
        int outOfStockThreshold = 0,
        int lowStockThreshold = 10,
        int overstockThreshold = 1000)
    {
        return new StyleRuleBuilder<int>()
            .WhenEquals(outOfStockThreshold, "stock-out", priority: 40, icon: "icon-alert-circle", tooltip: "Out of stock")
            .WhenLessThan(lowStockThreshold, "stock-low", priority: 30, icon: "icon-alert-triangle", tooltip: "Low stock - reorder needed")
            .WhenGreaterThan(overstockThreshold, "stock-high", priority: 10, icon: "icon-package", tooltip: "Overstock")
            .Build();
    }

    #endregion

    #region Status Indicators

    /// <summary>
    /// Creates status-based styling with predefined mappings
    /// Common for task status, order status, etc.
    /// </summary>
    public static List<StyleRule<string>> StatusIndicator(
        Dictionary<string, (string cssClass, string icon, string tooltip)>? mappings = null)
    {
        mappings ??= new Dictionary<string, (string, string, string)>
        {
            ["Active"] = ("status-active", "icon-check-circle", "Active"),
            ["Pending"] = ("status-pending", "icon-clock", "Pending"),
            ["Inactive"] = ("status-inactive", "icon-x-circle", "Inactive"),
            ["Completed"] = ("status-completed", "icon-check", "Completed"),
            ["Failed"] = ("status-failed", "icon-x", "Failed"),
            ["Cancelled"] = ("status-cancelled", "icon-slash", "Cancelled")
        };

        var builder = new StyleRuleBuilder<string>();
        int priority = 100;

        foreach (var (status, (cssClass, icon, tooltip)) in mappings)
        {
            builder.WhenEquals(status, cssClass, priority: priority--, icon: icon, tooltip: tooltip);
        }

        return builder.Build();
    }

    /// <summary>
    /// Creates priority-level styling (High/Medium/Low)
    /// </summary>
    public static List<StyleRule<string>> PriorityIndicator()
    {
        return new StyleRuleBuilder<string>()
            .WhenIn(new[] { "Critical", "Urgent", "High" }, "priority-high", priority: 30, icon: "icon-alert-triangle")
            .WhenIn(new[] { "Medium", "Normal" }, "priority-medium", priority: 20, icon: "icon-minus-circle")
            .WhenIn(new[] { "Low", "Minor" }, "priority-low", priority: 10, icon: "icon-circle")
            .Build();
    }

    /// <summary>
    /// Creates boolean-based styling (Yes/No, True/False, Active/Inactive)
    /// </summary>
    public static List<StyleRule<bool>> BooleanIndicator(
        string trueClass = "boolean-true",
        string falseClass = "boolean-false",
        string? trueIcon = "icon-check",
        string? falseIcon = "icon-x")
    {
        return new StyleRuleBuilder<bool>()
            .WhenTrue(trueClass, priority: 10, icon: trueIcon, tooltip: "Yes")
            .WhenFalse(falseClass, priority: 10, icon: falseIcon, tooltip: "No")
            .Build();
    }

    #endregion

    #region Performance Metrics

    /// <summary>
    /// Creates KPI (Key Performance Indicator) styling
    /// </summary>
    public static List<StyleRule<decimal>> KpiIndicator(
        decimal poorBelow = 50m,
        decimal fairBelow = 75m,
        decimal goodBelow = 90m)
    {
        return new StyleRuleBuilder<decimal>()
            .WhenLessThan(poorBelow, "kpi-poor", priority: 40, icon: "icon-trending-down", tooltip: "Needs improvement")
            .WhenLessThan(fairBelow, "kpi-fair", priority: 30, icon: "icon-minus", tooltip: "Below target")
            .WhenLessThan(goodBelow, "kpi-good", priority: 20, icon: "icon-trending-up", tooltip: "Good")
            .WhenGreaterThan(goodBelow, "kpi-excellent", priority: 10, icon: "icon-star", tooltip: "Excellent")
            .Build();
    }

    /// <summary>
    /// Creates rating-based styling (1-5 stars)
    /// </summary>
    public static List<StyleRule<decimal>> RatingIndicator()
    {
        return new StyleRuleBuilder<decimal>()
            .WhenLessThan(2m, "rating-poor", priority: 50, icon: "icon-star-off")
            .WhenBetween(2m, 3m, "rating-fair", priority: 40, icon: "icon-star-half")
            .WhenBetween(3m, 4m, "rating-good", priority: 30, icon: "icon-star")
            .WhenGreaterThan(4m, "rating-excellent", priority: 20, icon: "icon-star-full")
            .Build();
    }

    /// <summary>
    /// Creates SLA (Service Level Agreement) compliance styling
    /// </summary>
    public static List<StyleRule<decimal>> SlaCompliance()
    {
        return new StyleRuleBuilder<decimal>()
            .WhenLessThan(90m, "sla-breach", priority: 30, icon: "icon-alert-circle", tooltip: "Below SLA")
            .WhenBetween(90m, 95m, "sla-warning", priority: 20, icon: "icon-alert-triangle", tooltip: "Near SLA limit")
            .WhenGreaterThan(95m, "sla-compliant", priority: 10, icon: "icon-check-circle", tooltip: "SLA compliant")
            .Build();
    }

    #endregion

    #region Date/Time Based

    /// <summary>
    /// Creates date-based styling for overdue items
    /// </summary>
    public static List<StyleRule<DateTime?>> DateOverdue(DateTime? referenceDate = null)
    {
        var now = referenceDate ?? DateTime.Now;

        return new StyleRuleBuilder<DateTime?>()
            .When(date => date == null, "date-none", priority: 40, icon: "icon-calendar-x", tooltip: "No date set")
            .When(date => date < now, "date-overdue", priority: 30, icon: "icon-alert-circle", tooltip: "Overdue")
            .When(date => date < now.AddDays(7), "date-due-soon", priority: 20, icon: "icon-clock", tooltip: "Due soon")
            .When(date => date >= now.AddDays(7), "date-on-time", priority: 10, icon: "icon-check", tooltip: "On schedule")
            .Build();
    }

    /// <summary>
    /// Creates age-based styling (days old)
    /// </summary>
    public static List<StyleRule<int>> AgeIndicator(
        int recentDays = 7,
        int moderateDays = 30,
        int oldDays = 90)
    {
        return new StyleRuleBuilder<int>()
            .WhenLessThan(recentDays, "age-recent", priority: 10, icon: "icon-clock")
            .WhenBetween(recentDays, moderateDays, "age-moderate", priority: 20, icon: "icon-calendar")
            .WhenBetween(moderateDays, oldDays, "age-old", priority: 30, icon: "icon-calendar-x")
            .WhenGreaterThan(oldDays, "age-very-old", priority: 40, icon: "icon-archive")
            .Build();
    }

    #endregion

    #region Text/String Based

    /// <summary>
    /// Creates email validation styling
    /// </summary>
    public static List<StyleRule<string>> EmailValidation()
    {
        return new StyleRuleBuilder<string>()
            .WhenEmpty("email-empty", priority: 30, icon: "icon-mail-x", tooltip: "No email")
            .When(email => !string.IsNullOrEmpty(email) && !email.Contains("@"), 
                "email-invalid", priority: 20, icon: "icon-alert-triangle", tooltip: "Invalid email")
            .When(email => !string.IsNullOrEmpty(email) && email.Contains("@"), 
                "email-valid", priority: 10, icon: "icon-mail-check", tooltip: "Valid email")
            .Build();
    }

    /// <summary>
    /// Creates text length styling (for character limits)
    /// </summary>
    public static List<StyleRule<string>> TextLength(
        int warningLength = 80,
        int maxLength = 100)
    {
        return new StyleRuleBuilder<string>()
            .WhenEmpty("text-empty", priority: 40, icon: "icon-file-text")
            .When(text => !string.IsNullOrEmpty(text) && text.Length >= maxLength, 
                "text-too-long", priority: 30, icon: "icon-alert-circle", tooltip: "Exceeds maximum length")
            .When(text => !string.IsNullOrEmpty(text) && text.Length >= warningLength && text.Length < maxLength, 
                "text-near-limit", priority: 20, icon: "icon-alert-triangle", tooltip: "Approaching limit")
            .Build();
    }

    #endregion

    #region Health/Status Checks

    /// <summary>
    /// Creates health check styling for system monitoring
    /// </summary>
    public static List<StyleRule<string>> HealthCheck()
    {
        return new StyleRuleBuilder<string>()
            .WhenIn(new[] { "Healthy", "Ok", "Up", "Running" }, "health-good", priority: 10, icon: "icon-check-circle")
            .WhenIn(new[] { "Degraded", "Warning", "Slow" }, "health-warning", priority: 20, icon: "icon-alert-triangle")
            .WhenIn(new[] { "Unhealthy", "Error", "Down", "Stopped" }, "health-critical", priority: 30, icon: "icon-x-circle")
            .WhenIn(new[] { "Unknown", "Pending" }, "health-unknown", priority: 40, icon: "icon-help-circle")
            .Build();
    }

    /// <summary>
    /// Creates temperature-based styling (for monitoring dashboards)
    /// </summary>
    public static List<StyleRule<decimal>> Temperature(
        decimal normalMax = 75m,
        decimal warningMax = 85m,
        decimal criticalMax = 95m)
    {
        return new StyleRuleBuilder<decimal>()
            .WhenLessThan(normalMax, "temp-normal", priority: 10, icon: "icon-thermometer")
            .WhenBetween(normalMax, warningMax, "temp-warm", priority: 20, icon: "icon-alert-triangle")
            .WhenBetween(warningMax, criticalMax, "temp-hot", priority: 30, icon: "icon-alert-circle")
            .WhenGreaterThan(criticalMax, "temp-critical", priority: 40, icon: "icon-alert-octagon")
            .Build();
    }

    #endregion

    #region Custom Builders

    /// <summary>
    /// Creates a custom status indicator with user-defined mappings
    /// </summary>
    public static List<StyleRule<T>> CustomMapping<T>(
        Dictionary<T, (string cssClass, string? icon, string? tooltip)> mappings) where T : IEquatable<T>
    {
        var builder = new StyleRuleBuilder<T>();
        int priority = 100;

        foreach (var (value, (cssClass, icon, tooltip)) in mappings)
        {
            builder.WhenEquals(value, cssClass, priority: priority--, icon: icon, tooltip: tooltip);
        }

        return builder.Build();
    }

    /// <summary>
    /// Creates a simple traffic light indicator (Red/Yellow/Green)
    /// </summary>
    public static List<StyleRule<string>> TrafficLight()
    {
        return new StyleRuleBuilder<string>()
            .WhenIn(new[] { "Red", "Stop", "Critical", "Danger" }, "traffic-red", priority: 30, icon: "icon-octagon")
            .WhenIn(new[] { "Yellow", "Caution", "Warning" }, "traffic-yellow", priority: 20, icon: "icon-alert-triangle")
            .WhenIn(new[] { "Green", "Go", "Ok", "Good" }, "traffic-green", priority: 10, icon: "icon-check-circle")
            .Build();
    }

    #endregion
}
