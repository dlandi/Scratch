using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Dynamic column that can be configured at runtime with property path support.
/// Supports nested properties like "Address.City" or "Company.Department.Name".
/// </summary>
public class DynamicColumn<TGridItem> : ColumnBase<TGridItem>
{
    // Column configuration
    [Parameter] public string PropertyPath { get; set; } = default!;
    [Parameter] public Type? PropertyType { get; set; }
    [Parameter] public string? Format { get; set; }
    [Parameter] public Func<object?, string>? CustomFormatter { get; set; }

    // State management
    private Func<TGridItem, object?>? _valueAccessor;
    private Func<TGridItem, string?>? _formattedValueAccessor;
    private string? _lastPropertyPath;
    private GridSort<TGridItem>? _sortBuilder;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnInitialized()
    {
        if (string.IsNullOrWhiteSpace(PropertyPath))
        {
            throw new InvalidOperationException(
                $"{nameof(DynamicColumn<TGridItem>)} requires a {nameof(PropertyPath)} parameter.");
        }

        // Build accessors BEFORE calling base.OnInitialized()
        // This ensures they're ready when QuickGrid needs them
        BuildAccessors();

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        // Rebuild accessor if property path changed
        if (_lastPropertyPath != PropertyPath)
        {
            BuildAccessors();
        }

        base.OnParametersSet();
    }

    private void BuildAccessors()
    {
        _lastPropertyPath = PropertyPath;

        try
        {
            _valueAccessor = BuildPropertyAccessor(PropertyPath);
            _formattedValueAccessor = BuildFormattedAccessor();

            if (Sortable ?? false)
            {
                _sortBuilder = BuildSort();
            }

            // Infer title from property path if not set
            if (string.IsNullOrEmpty(Title))
            {
                Title = GetDisplayName(PropertyPath);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to build accessor for property path '{PropertyPath}': {ex.Message}", ex);
        }
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        if (_formattedValueAccessor is null)
        {
            builder.AddContent(0, "[No Accessor]");
            return;
        }

        try
        {
            var formattedValue = _formattedValueAccessor(item);
            builder.AddContent(0, formattedValue ?? string.Empty);
        }
        catch (Exception ex)
        {
            builder.AddContent(0, $"[Error: {ex.Message}]");
        }
    }

    /// <summary>
    /// Builds a compiled expression accessor for the property path.
    /// Supports nested properties like "Address.City".
    /// </summary>
    private Func<TGridItem, object?> BuildPropertyAccessor(string path)
    {
        var parameter = Expression.Parameter(typeof(TGridItem), "item");
        Expression expression = parameter;

        var properties = path.Split('.');
        Type currentType = typeof(TGridItem);

        foreach (var propName in properties)
        {
            var property = currentType.GetProperty(propName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property is null)
            {
                throw new InvalidOperationException(
                    $"Property '{propName}' not found on type '{currentType.Name}'");
            }

            expression = Expression.Property(expression, property);
            currentType = property.PropertyType;

            // Store discovered property type if not explicitly set
            PropertyType ??= currentType;
        }

        // Convert to object to handle any type
        if (expression.Type.IsValueType)
        {
            expression = Expression.Convert(expression, typeof(object));
        }
        else if (expression.Type != typeof(object))
        {
            // For reference types, convert to object
            expression = Expression.Convert(expression, typeof(object));
        }

        var lambda = Expression.Lambda<Func<TGridItem, object?>>(expression, parameter);
        return lambda.Compile();
    }

    /// <summary>
    /// Builds formatted accessor that applies formatting logic with null-safety.
    /// </summary>
    private Func<TGridItem, string?> BuildFormattedAccessor()
    {
        return item =>
        {
            try
            {
                var value = _valueAccessor!(item);

                if (value is null)
                {
                    return string.Empty;
                }

                // Use custom formatter if provided
                if (CustomFormatter is not null)
                {
                    return CustomFormatter(value);
                }

                // Use format string if provided and value is IFormattable
                if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
                {
                    return formattable.ToString(Format, null);
                }

                return value.ToString();
            }
            catch (NullReferenceException)
            {
                // Handle null reference in nested property path
                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"[Error: {ex.Message}]";
            }
        };
    }

    /// <summary>
    /// Builds sort expression for the property path.
    /// </summary>
    private GridSort<TGridItem> BuildSort()
    {
        var parameter = Expression.Parameter(typeof(TGridItem), "item");
        Expression expression = parameter;

        var properties = PropertyPath.Split('.');
        var currentType = typeof(TGridItem);

        foreach (var propName in properties)
        {
            var property = currentType.GetProperty(propName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property is null)
            {
                throw new InvalidOperationException(
                    $"Property '{propName}' not found on type '{currentType.Name}'");
            }

            expression = Expression.Property(expression, property);
            currentType = property.PropertyType;
        }

        // Create lambda expression
        var lambdaType = typeof(Func<,>).MakeGenericType(typeof(TGridItem), currentType);
        var lambda = Expression.Lambda(lambdaType, expression, parameter);

        // Build sort using reflection to call generic method
        var sortMethod = typeof(GridSort<TGridItem>)
            .GetMethod(nameof(GridSort<TGridItem>.ByAscending),
                BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(currentType);

        return (GridSort<TGridItem>)sortMethod.Invoke(null, new object[] { lambda })!;
    }

    /// <summary>
    /// Gets display name from property path.
    /// "Address.City" -> "City"
    /// "CompanyName" -> "Company Name"
    /// </summary>
    private static string GetDisplayName(string path)
    {
        var lastPart = path.Split('.').Last();

        // Add spaces before capital letters
        return string.Concat(lastPart.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
    }
}