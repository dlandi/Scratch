using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickGridTest01.ConditionalStyling;

/// <summary>
/// Represents a styling rule that applies CSS classes based on a condition.
/// Rules are evaluated in priority order (highest first).
/// </summary>
/// <typeparam name="TValue">The type of value being evaluated</typeparam>
public class StyleRule<TValue>
{
    /// <summary>
    /// The condition that must be true for this rule to apply
    /// </summary>
    public Func<TValue, bool> Condition { get; set; } = _ => false;

    /// <summary>
    /// The CSS class to apply when the condition is true
    /// </summary>
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Priority for rule evaluation (higher values evaluated first)
    /// When multiple rules match, only the highest priority is applied
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Optional icon class to display alongside the value (e.g., "icon-warning")
    /// </summary>
    public string? IconClass { get; set; }

    /// <summary>
    /// Optional tooltip text to display on hover
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Whether this rule should stop further rule evaluation
    /// Default: true (first matching rule wins)
    /// </summary>
    public bool StopOnMatch { get; set; } = true;
}

/// <summary>
/// Builder for creating StyleRule instances with a fluent API
/// </summary>
/// <typeparam name="TValue">The type of value being evaluated</typeparam>
public class StyleRuleBuilder<TValue>
{
    private readonly List<StyleRule<TValue>> _rules = new();
    private int _defaultPriority = 100;

    /// <summary>
    /// Sets the default priority for subsequently added rules
    /// </summary>
    public StyleRuleBuilder<TValue> WithDefaultPriority(int priority)
    {
        _defaultPriority = priority;
        return this;
    }

    /// <summary>
    /// Adds a rule with a custom condition
    /// </summary>
    public StyleRuleBuilder<TValue> When(
        Func<TValue, bool> condition,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
    {
        _rules.Add(new StyleRule<TValue>
        {
            Condition = condition,
            CssClass = cssClass,
            Priority = priority ?? _defaultPriority,
            IconClass = icon,
            Tooltip = tooltip
        });
        return this;
    }

    /// <summary>
    /// Adds a rule for null values
    /// </summary>
    public StyleRuleBuilder<TValue> WhenNull(
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
    {
        return When(value => value == null, cssClass, priority, icon, tooltip);
    }

    /// <summary>
    /// Returns the configured rules ordered by priority
    /// </summary>
    public List<StyleRule<TValue>> Build()
    {
        return _rules.OrderByDescending(r => r.Priority).ToList();
    }
}

/// <summary>
/// Builder extensions for numeric types
/// </summary>
public static class NumericStyleRuleBuilderExtensions
{
    /// <summary>
    /// Adds a rule for values less than a threshold
    /// </summary>
    public static StyleRuleBuilder<T> WhenLessThan<T>(
        this StyleRuleBuilder<T> builder,
        T threshold,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IComparable<T>
    {
        return builder.When(
            value => value.CompareTo(threshold) < 0,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for values greater than a threshold
    /// </summary>
    public static StyleRuleBuilder<T> WhenGreaterThan<T>(
        this StyleRuleBuilder<T> builder,
        T threshold,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IComparable<T>
    {
        return builder.When(
            value => value.CompareTo(threshold) > 0,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for values within a range (inclusive)
    /// </summary>
    public static StyleRuleBuilder<T> WhenBetween<T>(
        this StyleRuleBuilder<T> builder,
        T min,
        T max,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IComparable<T>
    {
        return builder.When(
            value => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for values equal to a specific value
    /// </summary>
    public static StyleRuleBuilder<T> WhenEquals<T>(
        this StyleRuleBuilder<T> builder,
        T compareValue,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IEquatable<T>
    {
        return builder.When(
            value => value.Equals(compareValue),
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for zero values
    /// </summary>
    public static StyleRuleBuilder<T> WhenZero<T>(
        this StyleRuleBuilder<T> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IEquatable<T>
    {
        return builder.WhenEquals(default(T)!, cssClass, priority, icon, tooltip);
    }

    /// <summary>
    /// Adds a rule for negative values
    /// </summary>
    public static StyleRuleBuilder<T> WhenNegative<T>(
        this StyleRuleBuilder<T> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IComparable<T>
    {
        return builder.WhenLessThan(default(T)!, cssClass, priority, icon, tooltip);
    }

    /// <summary>
    /// Adds a rule for positive values
    /// </summary>
    public static StyleRuleBuilder<T> WhenPositive<T>(
        this StyleRuleBuilder<T> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
        where T : IComparable<T>
    {
        return builder.WhenGreaterThan(default(T)!, cssClass, priority, icon, tooltip);
    }
}

/// <summary>
/// Builder extensions for string types
/// </summary>
public static class StringStyleRuleBuilderExtensions
{
    /// <summary>
    /// Adds a rule for strings matching a pattern
    /// </summary>
    public static StyleRuleBuilder<string> WhenContains(
        this StyleRuleBuilder<string> builder,
        string substring,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return builder.When(
            value => value?.Contains(substring, comparison) ?? false,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for strings starting with a prefix
    /// </summary>
    public static StyleRuleBuilder<string> WhenStartsWith(
        this StyleRuleBuilder<string> builder,
        string prefix,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return builder.When(
            value => value?.StartsWith(prefix, comparison) ?? false,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for empty or null strings
    /// </summary>
    public static StyleRuleBuilder<string> WhenEmpty(
        this StyleRuleBuilder<string> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
    {
        return builder.When(
            value => string.IsNullOrEmpty(value),
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for strings in a set of values
    /// </summary>
    public static StyleRuleBuilder<string> WhenIn(
        this StyleRuleBuilder<string> builder,
        IEnumerable<string> values,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var valueSet = new HashSet<string>(
            values,
            comparison == StringComparison.OrdinalIgnoreCase
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal);

        return builder.When(
            value => value != null && valueSet.Contains(value),
            cssClass,
            priority,
            icon,
            tooltip);
    }
}

/// <summary>
/// Builder extensions for boolean types
/// </summary>
public static class BooleanStyleRuleBuilderExtensions
{
    /// <summary>
    /// Adds a rule for true values
    /// </summary>
    public static StyleRuleBuilder<bool> WhenTrue(
        this StyleRuleBuilder<bool> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
    {
        return builder.When(
            value => value,
            cssClass,
            priority,
            icon,
            tooltip);
    }

    /// <summary>
    /// Adds a rule for false values
    /// </summary>
    public static StyleRuleBuilder<bool> WhenFalse(
        this StyleRuleBuilder<bool> builder,
        string cssClass,
        int? priority = null,
        string? icon = null,
        string? tooltip = null)
    {
        return builder.When(
            value => !value,
            cssClass,
            priority,
            icon,
            tooltip);
    }
}

/// <summary>
/// Represents the result of style rule evaluation
/// </summary>
public class StyleRuleResult
{
    public string CssClass { get; set; } = string.Empty;
    public string? IconClass { get; set; }
    public string? Tooltip { get; set; }
    public bool HasMatch { get; set; }
}

/// <summary>
/// Evaluates style rules against values
/// </summary>
public static class StyleRuleEvaluator
{
    /// <summary>
    /// Evaluates rules and returns the result of the first matching rule
    /// </summary>
    public static StyleRuleResult Evaluate<TValue>(
        TValue value,
        IEnumerable<StyleRule<TValue>> rules)
    {
        foreach (var rule in rules.OrderByDescending(r => r.Priority))
        {
            if (rule.Condition(value))
            {
                return new StyleRuleResult
                {
                    CssClass = rule.CssClass,
                    IconClass = rule.IconClass,
                    Tooltip = rule.Tooltip,
                    HasMatch = true
                };
            }
        }

        return new StyleRuleResult { HasMatch = false };
    }

    /// <summary>
    /// Evaluates rules and returns all matching rules (for combining multiple classes)
    /// </summary>
    public static StyleRuleResult EvaluateAll<TValue>(
        TValue value,
        IEnumerable<StyleRule<TValue>> rules)
    {
        var matchingRules = rules
            .Where(r => r.Condition(value))
            .OrderByDescending(r => r.Priority)
            .ToList();

        if (!matchingRules.Any())
        {
            return new StyleRuleResult { HasMatch = false };
        }

        // Combine all CSS classes
        var cssClasses = string.Join(" ", matchingRules.Select(r => r.CssClass));

        // Use the highest priority rule's icon and tooltip
        var topRule = matchingRules.First();

        return new StyleRuleResult
        {
            CssClass = cssClasses,
            IconClass = topRule.IconClass,
            Tooltip = topRule.Tooltip,
            HasMatch = true
        };
    }
}
