using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.ComposableColumns.Features.Core;

using ComposableColumns.Core;

/// <summary>
/// Feature that automatically infers the column title from the property name.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class AutoTitleFeature<TGridItem> : IColumnFeature<TGridItem>
{
    public int Priority => FeaturePriority.Core;

    /// <summary>
    /// Whether to convert PascalCase to "Pascal Case" with spaces.
    /// </summary>
    public bool SplitPascalCase { get; set; } = true;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        // Only set title if not already set
        if (!string.IsNullOrEmpty(context.Title))
            return;

        var propertyName = context.GetState<string>(FeatureStateKeys.PropertyName);
        if (string.IsNullOrEmpty(propertyName))
            return;

        context.Title = SplitPascalCase
            ? SplitPascalCaseToTitle(propertyName)
            : propertyName;
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // Nothing to clean up
    }

    private static string SplitPascalCaseToTitle(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (i > 0 && char.IsUpper(c) && !char.IsUpper(input[i - 1]))
            {
                result.Append(' ');
            }
            result.Append(c);
        }
        return result.ToString();
    }
}

/// <summary>
/// Feature that compiles a property expression for fast value access.
/// This is typically one of the first features that runs.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class CompiledAccessorFeature<TGridItem, TValue> : IColumnFeature<TGridItem>
{
    public int Priority => FeaturePriority.Infrastructure;

    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledGetter;
    private Action<TGridItem, TValue>? _compiledSetter;

    /// <summary>
    /// The property expression to compile.
    /// </summary>
    public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        CompileAccessors(context);
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        _lastProperty = null;
        _compiledGetter = null;
        _compiledSetter = null;
    }

    private void CompileAccessors(FeatureContext<TGridItem> context)
    {
        if (context is not FeatureContext<TGridItem, TValue> typedContext)
            return;

        var property = Property ?? typedContext.PropertyExpression;
        if (property is null || property == _lastProperty)
            return;

        _lastProperty = property;
        _compiledGetter = property.Compile();
        typedContext.GetValue = _compiledGetter;

        // Try to build a setter
        if (property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
        {
            if (propInfo.CanWrite)
            {
                var param = property.Parameters[0];
                var valueParam = Expression.Parameter(typeof(TValue), "value");
                var assign = Expression.Assign(memberExpr, valueParam);
                var setter = Expression.Lambda<Action<TGridItem, TValue>>(assign, param, valueParam);
                _compiledSetter = setter.Compile();
                typedContext.SetValue = _compiledSetter;
            }

            // Store property name in state
            context.SetState(FeatureStateKeys.PropertyName, propInfo.Name);
            context.SetState(FeatureStateKeys.PropertyType, typeof(TValue));
        }
    }
}

/// <summary>
/// Feature that enables sorting on the column.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value.</typeparam>
public class SortableFeature<TGridItem, TValue> : ISortingFeature<TGridItem>
{
    public int Priority => FeaturePriority.Core;

    /// <summary>
    /// Whether sorting is enabled.
    /// </summary>
    public bool IsSortable { get; set; } = true;

    /// <summary>
    /// The property expression to sort by.
    /// </summary>
    public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        context.IsSortable = IsSortable;
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        // Nothing to clean up
    }

    public Func<IQueryable<TGridItem>, bool, IOrderedQueryable<TGridItem>>? GetSortFunction()
    {
        if (!IsSortable || Property is null)
            return null;

        var compiled = Property.Compile();
        return (query, ascending) => ascending
            ? query.OrderBy(Property)
            : query.OrderByDescending(Property);
    }
}
