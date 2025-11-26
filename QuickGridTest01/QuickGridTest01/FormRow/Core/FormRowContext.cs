using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Context provided to FormTemplate and cascaded to FormField components.
/// Manages draft state, validation, and form actions.
/// </summary>
/// <typeparam name="TGridItem">The type of data item being edited</typeparam>
public class FormRowContext<TGridItem> where TGridItem : class
{
    private readonly Dictionary<string, object?> _draftValues = new();
    private readonly Dictionary<string, object?> _originalValues = new();
    private readonly Dictionary<string, List<string>> _fieldErrors = new();
    private readonly Dictionary<string, Func<object?, object?>> _compiledGetters = new();
    private readonly Dictionary<string, Action<object?, object?>> _compiledSetters = new();

    /// <summary>
    /// The data item being edited.
    /// </summary>
    public TGridItem Item { get; init; } = default!;

    /// <summary>
    /// Current state of the form (Reading, Editing, Saving).
    /// </summary>
    public FormRowState State { get; internal set; } = FormRowState.Editing;

    /// <summary>
    /// Convenience property: true when State == Saving.
    /// </summary>
    public bool IsSaving => State == FormRowState.Saving;

    /// <summary>
    /// Error message from last failed save attempt.
    /// </summary>
    public string? SaveError { get; internal set; }

    /// <summary>
    /// Validates and saves the form.
    /// </summary>
    public Func<Task> SaveAsync { get; init; } = default!;

    /// <summary>
    /// Cancels editing and reverts to original values.
    /// </summary>
    public Func<Task> CancelAsync { get; init; } = default!;

    /// <summary>
    /// Internal callback for validation.
    /// </summary>
    internal Func<string, object?, Task<List<string>>>? ValidateFieldCallback { get; set; }

    #region Draft State Management

    /// <summary>
    /// Gets the draft value for a property.
    /// </summary>
    public TValue? GetDraft<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        
        if (_draftValues.TryGetValue(propertyName, out var value))
        {
            return (TValue?)value;
        }

        // Initialize from item if not yet tracked
        var getter = GetOrCreateGetter(property);
        var originalValue = (TValue?)getter(Item);
        _originalValues[propertyName] = originalValue;
        _draftValues[propertyName] = originalValue;
        
        return originalValue;
    }

    /// <summary>
    /// Sets the draft value for a property.
    /// </summary>
    public void SetDraft<TValue>(Expression<Func<TGridItem, TValue>> property, TValue? value)
    {
        var propertyName = GetPropertyName(property);
        
        // Ensure original is captured
        if (!_originalValues.ContainsKey(propertyName))
        {
            var getter = GetOrCreateGetter(property);
            _originalValues[propertyName] = getter(Item);
        }
        
        _draftValues[propertyName] = value;
    }

    /// <summary>
    /// Returns true if the specified property has been modified.
    /// </summary>
    public bool IsDirty<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        
        if (!_draftValues.TryGetValue(propertyName, out var draft))
            return false;
            
        if (!_originalValues.TryGetValue(propertyName, out var original))
            return false;

        return !EqualityComparer<object>.Default.Equals(draft, original);
    }

    /// <summary>
    /// Returns true if any field has been modified.
    /// </summary>
    public bool IsDirty()
    {
        foreach (var kvp in _draftValues)
        {
            if (_originalValues.TryGetValue(kvp.Key, out var original))
            {
                if (!EqualityComparer<object>.Default.Equals(kvp.Value, original))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets all draft values as a dictionary.
    /// </summary>
    internal Dictionary<string, object?> GetAllDraftValues() => new(_draftValues);

    /// <summary>
    /// Commits draft values to the item.
    /// </summary>
    internal void CommitDraftToItem()
    {
        foreach (var kvp in _draftValues)
        {
            if (_compiledSetters.TryGetValue(kvp.Key, out var setter))
            {
                setter(Item, kvp.Value);
            }
        }
        
        // Update originals to match committed values
        foreach (var kvp in _draftValues)
        {
            _originalValues[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Reverts draft values to original values.
    /// </summary>
    internal void RevertDraft()
    {
        _draftValues.Clear();
        foreach (var kvp in _originalValues)
        {
            _draftValues[kvp.Key] = kvp.Value;
        }
        _fieldErrors.Clear();
        SaveError = null;
    }

    #endregion

    #region Validation

    /// <summary>
    /// Gets validation errors for a specific property.
    /// </summary>
    public IReadOnlyList<string> GetErrors<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        return _fieldErrors.TryGetValue(propertyName, out var errors) 
            ? errors.AsReadOnly() 
            : Array.Empty<string>();
    }

    /// <summary>
    /// Returns true if the specified property has validation errors.
    /// </summary>
    public bool HasErrors<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        return _fieldErrors.TryGetValue(propertyName, out var errors) && errors.Count > 0;
    }

    /// <summary>
    /// Returns true if any field has validation errors.
    /// </summary>
    public bool HasErrors()
    {
        return _fieldErrors.Values.Any(e => e.Count > 0);
    }

    /// <summary>
    /// Validates all tracked fields.
    /// </summary>
    public async Task ValidateAsync()
    {
        if (ValidateFieldCallback == null)
            return;

        foreach (var propertyName in _draftValues.Keys.ToList())
        {
            var value = _draftValues[propertyName];
            var errors = await ValidateFieldCallback(propertyName, value);
            _fieldErrors[propertyName] = errors;
        }
    }

    /// <summary>
    /// Validates a specific field.
    /// </summary>
    public async Task ValidateFieldAsync<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        if (ValidateFieldCallback == null)
            return;

        var propertyName = GetPropertyName(property);
        var value = _draftValues.TryGetValue(propertyName, out var v) ? v : default;
        var errors = await ValidateFieldCallback(propertyName, value);
        _fieldErrors[propertyName] = errors;
    }

    /// <summary>
    /// Sets errors for a property (used internally by validation).
    /// </summary>
    internal void SetErrors(string propertyName, List<string> errors)
    {
        _fieldErrors[propertyName] = errors;
    }

    /// <summary>
    /// Clears all validation errors.
    /// </summary>
    internal void ClearErrors()
    {
        _fieldErrors.Clear();
        SaveError = null;
    }

    #endregion

    #region Expression Helpers

    private static string GetPropertyName<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        if (property.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member.Name;
        }
        throw new ArgumentException("Expression must be a property access expression", nameof(property));
    }

    private Func<object?, object?> GetOrCreateGetter<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        
        if (_compiledGetters.TryGetValue(propertyName, out var existing))
            return existing;

        var compiled = property.Compile();
        Func<object?, object?> getter = obj => compiled((TGridItem)obj!);
        _compiledGetters[propertyName] = getter;

        // Also create setter while we're at it
        if (property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
        {
            Action<object?, object?> setter = (obj, val) => propInfo.SetValue(obj, val);
            _compiledSetters[propertyName] = setter;
        }

        return getter;
    }

    /// <summary>
    /// Registers a property for tracking (used by FormField to ensure property is tracked).
    /// </summary>
    internal void RegisterProperty<TValue>(Expression<Func<TGridItem, TValue>> property)
    {
        var propertyName = GetPropertyName(property);
        
        if (!_draftValues.ContainsKey(propertyName))
        {
            var getter = GetOrCreateGetter(property);
            var value = getter(Item);
            _originalValues[propertyName] = value;
            _draftValues[propertyName] = value;
        }
    }

    #endregion
}
