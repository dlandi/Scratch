using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using QuickGridTest01.CustomColumns;

namespace QuickGridTest01.FormRow.Validation;

/// <summary>
/// Validates form fields using DataAnnotations and custom validators.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item being validated</typeparam>
public class FormFieldValidator<TGridItem> where TGridItem : class
{
    private readonly Dictionary<string, List<ValidationAttribute>> _dataAnnotations = new();
    private readonly Dictionary<string, List<object>> _customValidators = new();
    private readonly bool _useDataAnnotations;

    public FormFieldValidator(bool useDataAnnotations = true)
    {
        _useDataAnnotations = useDataAnnotations;
    }

    /// <summary>
    /// Registers a property for validation, discovering DataAnnotations.
    /// </summary>
    public void RegisterProperty<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        
        if (_dataAnnotations.ContainsKey(propertyName))
            return;

        if (_useDataAnnotations && property.Body is MemberExpression memberExpr &&
            memberExpr.Member is PropertyInfo propInfo)
        {
            var attributes = propInfo
                .GetCustomAttributes(typeof(ValidationAttribute), true)
                .Cast<ValidationAttribute>()
                .ToList();
            _dataAnnotations[propertyName] = attributes;
        }
        else
        {
            _dataAnnotations[propertyName] = new List<ValidationAttribute>();
        }
    }

    /// <summary>
    /// Adds a custom validator for a property.
    /// </summary>
    public void AddValidator<TValue>(Expression<Func<TGridItem, TValue>> property, IValidator<TValue> validator)
    {
        var propertyName = GetPropertyName(property);
        
        if (!_customValidators.TryGetValue(propertyName, out var validators))
        {
            validators = new List<object>();
            _customValidators[propertyName] = validators;
        }
        
        validators.Add(validator);
    }

    /// <summary>
    /// Validates a specific field value.
    /// </summary>
    public async Task<List<string>> ValidateFieldAsync(string propertyName, object? value, TGridItem item)
    {
        var errors = new List<string>();

        // DataAnnotations validation
        if (_useDataAnnotations && _dataAnnotations.TryGetValue(propertyName, out var attributes))
        {
            var context = new ValidationContext(item) { MemberName = propertyName };
            
            foreach (var attr in attributes)
            {
                var result = attr.GetValidationResult(value, context);
                if (result != null && !string.IsNullOrEmpty(result.ErrorMessage))
                {
                    errors.Add(result.ErrorMessage);
                }
            }
        }

        // Custom validators
        if (_customValidators.TryGetValue(propertyName, out var validators))
        {
            foreach (var validator in validators)
            {
                var validatorType = validator.GetType();
                var validateMethod = validatorType.GetMethod("ValidateAsync");
                
                if (validateMethod != null)
                {
                    var task = (Task<CustomColumns.ValidationResult>?)validateMethod.Invoke(validator, new[] { value });
                    if (task != null)
                    {
                        var result = await task;
                        if (!result.IsValid && !string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            errors.Add(result.ErrorMessage);
                        }
                    }
                }
            }
        }

        return errors;
    }

    /// <summary>
    /// Validates all registered fields.
    /// </summary>
    public async Task<Dictionary<string, List<string>>> ValidateAllAsync(
        Dictionary<string, object?> fieldValues,
        TGridItem item)
    {
        var allErrors = new Dictionary<string, List<string>>();

        foreach (var kvp in fieldValues)
        {
            var errors = await ValidateFieldAsync(kvp.Key, kvp.Value, item);
            if (errors.Count > 0)
            {
                allErrors[kvp.Key] = errors;
            }
        }

        return allErrors;
    }

    /// <summary>
    /// Checks if any field has validation errors.
    /// </summary>
    public async Task<bool> IsValidAsync(Dictionary<string, object?> fieldValues, TGridItem item)
    {
        var errors = await ValidateAllAsync(fieldValues, item);
        return errors.Count == 0;
    }

    private static string GetPropertyName<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        if (property.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member.Name;
        }
        throw new ArgumentException("Expression must be a property access expression", nameof(property));
    }
}
