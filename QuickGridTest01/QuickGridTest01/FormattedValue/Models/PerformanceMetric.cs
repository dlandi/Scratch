namespace QuickGridTest01.FormattedValue.Demo.Models;

/// <summary>
/// Performance metric model demonstrating percentage and change calculations.
/// </summary>
public class PerformanceMetric
{
    /// <summary>
    /// Gets or sets the metric name.
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current metric value.
    /// </summary>
    public decimal CurrentValue { get; set; }

    /// <summary>
    /// Gets or sets the target value to achieve.
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// Gets or sets the previous period's value.
    /// </summary>
    public decimal PreviousValue { get; set; }

    /// <summary>
    /// Gets the percentage of target achieved (0-100+).
    /// </summary>
    public decimal PercentOfTarget => TargetValue != 0
        ? (CurrentValue / TargetValue) * 100
        : 0;

    /// <summary>
    /// Gets the percentage change from previous value.
    /// </summary>
    public decimal ChangePercent => PreviousValue != 0
        ? ((CurrentValue - PreviousValue) / PreviousValue) * 100
        : 0;
}
