using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Dynamic column with runtime property path resolution (including nested paths).
/// Pattern mirrors IconColumn: exposes a strongly typed Property delegate so QuickGrid can register the column.
/// Usage example:
/// <DynamicColumn TGridItem="Employee" TValue="string" PropertyPath="Address.City" Title="City" />
/// </summary>
public class DynamicColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    // Configuration parameters
    [Parameter] public string? PropertyPath { get; set; }   // Alternative to supplying Property directly
    [Parameter] public Func<TGridItem, TValue>? Property { get; set; } // Strongly typed accessor (preferred)
    [Parameter] public string? Format { get; set; }
    [Parameter] public Func<TValue, string>? CustomFormatter { get; set; }
    [Parameter] public bool ShowValue { get; set; } = true;

    // Internal state
    private Func<TGridItem, TValue>? _accessor; // Current effective accessor
    private string? _lastPath;
    private Func<TGridItem, string?>? _formattedAccessor;
    private GridSort<TGridItem>? _sortBuilder;
    private bool _isInitialized;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnInitialized()
    {
        _isInitialized = true;

        if (Property is null && string.IsNullOrWhiteSpace(PropertyPath))
        {
            throw new InvalidOperationException($"{nameof(DynamicColumn<TGridItem, TValue>)} requires either {nameof(Property)} or {nameof(PropertyPath)}.");
        }

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (!_isInitialized)
        {
            return;
        }

        // Decide whether we need to rebuild accessor
        bool rebuild = false;
        if (Property is not null && _accessor != Property)
        {
            _accessor = Property;
            rebuild = true;
        }
        else if (Property is null && _lastPath != PropertyPath)
        {
            _lastPath = PropertyPath;
            _accessor = BuildAccessorFromPath(PropertyPath!);
            rebuild = true;
        }

        if (rebuild)
        {
            _formattedAccessor = BuildFormattedAccessor();
            if (Sortable ?? false)
            {
                _sortBuilder = BuildSort();
            }
            if (string.IsNullOrEmpty(Title))
            {
                Title = GetDisplayName(PropertyPath ?? "");
            }
        }

        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_accessor is null)
        {
            builder.AddContent(0, "[No Accessor]");
            return;
        }

        var formatted = _formattedAccessor?.Invoke(item) ?? string.Empty;

        if (!ShowValue)
        {
            // If value should be hidden, render empty container (could style later)
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", "dynamic-column-empty");
            builder.CloseElement();
            return;
        }

        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "class", "dynamic-column-value");
        builder.AddContent(2, formatted);
        builder.CloseElement();
    }

    // Build strongly typed accessor from a dotted path (supports nested properties)
    private Func<TGridItem, TValue> BuildAccessorFromPath(string path)
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

        // Handle common flexible cases
        if (typeof(TValue) == typeof(object))
        {
            // Box any value type; pass-through reference types
            if (current.Type.IsValueType)
            {
                current = Expression.Convert(current, typeof(object));
            }
            else if (current.Type != typeof(object))
            {
                current = Expression.Convert(current, typeof(object));
            }

            var lambdaObj = Expression.Lambda<Func<TGridItem, object>>(current, parameter);
            var compiledObj = lambdaObj.Compile();
            // Wrap to match Func<TGridItem, TValue>
            return (TGridItem item) => (TValue)(object?)compiledObj(item)!;
        }

        if (typeof(TValue) == typeof(string))
        {
            // Build ToString with null safety for reference types
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
                // Value types are non-nullable here; direct ToString
                toStringExpr = Expression.Call(current, current.Type.GetMethod(nameof(object.ToString), Type.EmptyTypes)!);
            }

            var lambdaStr = Expression.Lambda<Func<TGridItem, string>>(toStringExpr, parameter);
            var compiledStr = lambdaStr.Compile();
            return (TGridItem item) => (TValue)(object)compiledStr(item);
        }

        // If destination type is assignable, cast/box if necessary
        if (typeof(TValue).IsAssignableFrom(currentType))
        {
            if (current.Type != typeof(TValue))
            {
                current = Expression.Convert(current, typeof(TValue));
            }
            var lambda = Expression.Lambda<Func<TGridItem, TValue>>(current, parameter);
            return lambda.Compile();
        }

        // Try runtime conversion using Convert.ChangeType for other convertible pairs
        var boxed = Expression.Convert(current, typeof(object));
        var changeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) })!;
        var targetTypeExpr = Expression.Constant(typeof(TValue), typeof(Type));
        var changed = Expression.Call(changeType, boxed, targetTypeExpr);
        var casted = Expression.Convert(changed, typeof(TValue));
        var lambdaFallback = Expression.Lambda<Func<TGridItem, TValue>>(casted, parameter);
        return lambdaFallback.Compile();
    }

    private Func<TGridItem, string?> BuildFormattedAccessor()
    {
        return item =>
        {
            try
            {
                var value = _accessor!(item);
                if (value is null)
                {
                    return string.Empty;
                }
                if (CustomFormatter is not null)
                {
                    return CustomFormatter(value);
                }
                if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
                {
                    return formattable.ToString(Format, null);
                }
                return value.ToString();
            }
            catch (Exception ex)
            {
                return $"[Error: {ex.Message}]";
            }
        };
    }

    private GridSort<TGridItem> BuildSort()
    {
        // Simple stable ascending sort based on the underlying value
        return GridSort<TGridItem>.ByAscending(item => _accessor!(item))
                                   .ThenAscending(item => item); // stable fallback
    }

    private static string GetDisplayName(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        var last = path.Split('.').Last();
        return string.Concat(last.Select((c, i) => i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
    }
}