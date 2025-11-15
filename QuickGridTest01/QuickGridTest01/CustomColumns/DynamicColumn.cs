using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Dynamic column supporting either a strongly-typed Property expression (enables sorting)
/// or a dotted PropertyPath (render-only, no sorting). Mirrors the simple patterns in
/// FormattedValueColumn and MultiStateColumn to avoid QuickGrid suppression when using
/// complex, runtime-built expressions.
/// </summary>
public class DynamicColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    // Strongly-typed expression (preferred). When supplied, enables sorting.
    [Parameter] public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    // Dotted path (nested property traversal). When used, render-only (no SortBy).
    [Parameter] public string? PropertyPath { get; set; }

    [Parameter] public string? Format { get; set; }
    [Parameter] public Func<object?, string>? CustomFormatter { get; set; }

    private Func<TGridItem, TValue>? _compiledAccessor;        // For cell rendering
    private Expression<Func<TGridItem, TValue>>? _compiledExpression; // For sorting (if Property provided)
    private GridSort<TGridItem>? _sortBy;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBy;
        set => _sortBy = value;
    }

    protected override void OnParametersSet()
    {
        // Validate input
        if (Property is null && string.IsNullOrWhiteSpace(PropertyPath))
        {
            throw new InvalidOperationException($"{nameof(DynamicColumn<TGridItem, TValue>)} requires either {nameof(Property)} or {nameof(PropertyPath)}.");
        }

        if (Property is not null)
        {
            // Use provided expression directly (QuickGrid-friendly)
            if (!ReferenceEquals(_compiledExpression, Property))
            {
                _compiledExpression = Property;
                _compiledAccessor = _compiledExpression.Compile();

                if (Sortable ?? false)
                {
                    // Only assign SortBy for comparable key types
                    var keyType = _compiledExpression.Body.Type;
                    if (IsSortableType(keyType))
                    {
                        _sortBy = GridSort<TGridItem>.ByAscending(_compiledExpression);
                    }
                    else
                    {
                        _sortBy = null; // Avoid unsortable types
                    }
                }
            }
        }
        else if (PropertyPath is not null)
        {
            // Render-only path: compile a delegate, do NOT build an expression tree for QuickGrid
            _compiledAccessor = BuildDelegateFromPath(PropertyPath);
            _compiledExpression = null; // Ensure QuickGrid only sees delegate usage in CellContent
            _sortBy = null; // No sorting for path-based dynamic columns
        }

        // Derive title if absent
        if (string.IsNullOrEmpty(Title))
        {
            Title = PropertyPath is not null
                ? GetDisplayName(PropertyPath)
                : DeriveDisplayNameFromExpression(_compiledExpression);
        }

        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_compiledAccessor is null)
        {
            builder.AddContent(0, string.Empty);
            return;
        }

        var raw = _compiledAccessor(item);

        string formatted;
        if (raw is null)
        {
            formatted = string.Empty;
        }
        else if (CustomFormatter is not null)
        {
            formatted = CustomFormatter(raw);
        }
        else if (!string.IsNullOrEmpty(Format) && raw is IFormattable formattable)
        {
            formatted = formattable.ToString(Format, null) ?? string.Empty;
        }
        else
        {
            formatted = raw.ToString() ?? string.Empty;
        }

        // Minimal rendering (match FormattedValueColumn style for reliability)
        builder.AddContent(0, formatted);
    }

    private static Func<TGridItem, TValue> BuildDelegateFromPath(string path)
    {
        var parameter = Expression.Parameter(typeof(TGridItem), "item");
        Expression current = parameter;
        Type currentType = typeof(TGridItem);

        foreach (var segment in path.Split('.'))
        {
            var prop = currentType.GetProperty(segment, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop is null)
            {
                throw new InvalidOperationException($"Property '{segment}' not found on type '{currentType.Name}'.");
            }
            current = Expression.Property(current, prop);
            currentType = prop.PropertyType;
        }

        // Special cases for flexible TValue
        if (typeof(TValue) == typeof(object))
        {
            Expression boxed = current.Type.IsValueType ? Expression.Convert(current, typeof(object)) : Expression.Convert(current, typeof(object));
            var lambdaObj = Expression.Lambda<Func<TGridItem, object>>(boxed, parameter).Compile();
            return (TGridItem item) => (TValue)lambdaObj(item)!;
        }

        if (typeof(TValue) == typeof(string))
        {
            Expression toStringExpr;
            if (!current.Type.IsValueType)
            {
                var nullConst = Expression.Constant(null, current.Type);
                var isNull = Expression.Equal(current, nullConst);
                var whenNull = Expression.Constant(string.Empty);
                var toStringCall = Expression.Call(current, typeof(object).GetMethod(nameof(object.ToString))!);
                toStringExpr = Expression.Condition(isNull, whenNull, toStringCall);
            }
            else
            {
                toStringExpr = Expression.Call(current, current.Type.GetMethod(nameof(object.ToString), Type.EmptyTypes)!);
            }
            var lambdaStr = Expression.Lambda<Func<TGridItem, string>>(toStringExpr, parameter).Compile();
            return (TGridItem item) => (TValue)(object)lambdaStr(item);
        }

        // Direct assignable
        if (typeof(TValue).IsAssignableFrom(current.Type))
        {
            if (current.Type != typeof(TValue))
            {
                current = Expression.Convert(current, typeof(TValue));
            }
            return Expression.Lambda<Func<TGridItem, TValue>>(current, parameter).Compile();
        }

        // Fallback: Convert.ChangeType
        var boxedValue = Expression.Convert(current, typeof(object));
        var changeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) })!;
        var targetTypeExpr = Expression.Constant(typeof(TValue), typeof(Type));
        var changed = Expression.Call(changeType, boxedValue, targetTypeExpr);
        var casted = Expression.Convert(changed, typeof(TValue));
        return Expression.Lambda<Func<TGridItem, TValue>>(casted, parameter).Compile();
    }

    private static bool IsSortableType(Type t)
    {
        if (t == typeof(object)) return false;
        if (typeof(IComparable).IsAssignableFrom(t)) return true;
        foreach (var i in t.GetInterfaces())
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IComparable<>)) return true;
        }
        return false;
    }

    private static string GetDisplayName(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        var last = path.Split('.').Last();
        return string.Concat(last.Select((c, i) => i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
    }

    private static string DeriveDisplayNameFromExpression(Expression<Func<TGridItem, TValue>>? expr)
    {
        if (expr is null) return string.Empty;
        Expression body = expr.Body;
        var segments = new List<string>();
        while (body is MemberExpression m)
        {
            segments.Insert(0, m.Member.Name);
            body = m.Expression!;
        }
        return segments.Count > 0 ? string.Join('.', segments) : string.Empty;
    }
}