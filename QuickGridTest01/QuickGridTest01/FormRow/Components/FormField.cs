using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.FormRow.Core;
using QuickGridTest01.CustomColumns;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.FormRow.Components;

/// <summary>
/// Auto-wiring form field that binds to FormRowContext.
/// Automatically discovers label, input type, and validation from property metadata.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of the property value</typeparam>
public class FormField<TGridItem, TValue> : ComponentBase where TGridItem : class
{
    private string? _propertyName;
    private string? _resolvedLabel;
    private string? _resolvedInputType;
    private string? _resolvedPlaceholder;
    private Func<TGridItem, TValue>? _compiledGetter;
    private bool _isInitialized;

    #region Parameters

    /// <summary>
    /// Cascaded form context (auto-provided by FormRowColumn).
    /// </summary>
    [CascadingParameter]
    public FormRowContext<TGridItem>? Context { get; set; }

    /// <summary>
    /// The property to bind. Required.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>
    /// Override the auto-detected label.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Placeholder text for the input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Override the auto-detected input type.
    /// </summary>
    [Parameter]
    public string? InputType { get; set; }

    /// <summary>
    /// Make the field read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Make the field disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Additional validators beyond DataAnnotations.
    /// </summary>
    [Parameter]
    public IEnumerable<IValidator<TValue>>? Validators { get; set; }

    /// <summary>
    /// Custom input template for advanced scenarios.
    /// </summary>
    [Parameter]
    public RenderFragment<FormFieldInputContext<TValue>>? InputTemplate { get; set; }

    /// <summary>
    /// CSS class applied to the field wrapper.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// When true, shows validation errors inline. Default: true.
    /// </summary>
    [Parameter]
    public bool ShowErrors { get; set; } = true;

    /// <summary>
    /// Number of rows for textarea input type.
    /// </summary>
    [Parameter]
    public int Rows { get; set; } = 3;

    /// <summary>
    /// Options for select input type (enum or custom).
    /// </summary>
    [Parameter]
    public IEnumerable<SelectOption>? SelectOptions { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        if (Property == null)
            throw new InvalidOperationException($"{nameof(FormField<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");

        if (!_isInitialized || HasPropertyChanged())
        {
            InitializeFromProperty();
            _isInitialized = true;
        }

        // Register property with context for tracking
        Context?.RegisterProperty(Property);
    }

    private bool HasPropertyChanged()
    {
        var newName = GetPropertyName(Property);
        return _propertyName != newName;
    }

    private void InitializeFromProperty()
    {
        _propertyName = GetPropertyName(Property);
        _compiledGetter = Property.Compile();

        // Resolve label
        _resolvedLabel = Label ?? GetDisplayName() ?? _propertyName;

        // Resolve input type
        _resolvedInputType = InputType ?? InferInputType();

        // Resolve placeholder
        _resolvedPlaceholder = Placeholder ?? GetPlaceholderFromAttribute();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Context == null)
        {
            builder.AddContent(0, $"FormField: No FormRowContext found for {_propertyName}");
            return;
        }

        var value = Context.GetDraft(Property);
        var errors = Context.GetErrors(Property);
        var isDirty = Context.IsDirty(Property);
        var hasErrors = errors.Any();
        var isDisabled = Disabled || Context.IsSaving;

        int seq = 0;

        // Field wrapper
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", BuildFieldClass(isDirty, hasErrors));

        // Label
        builder.OpenElement(seq++, "label");
        builder.AddAttribute(seq++, "class", "form-field-label");
        builder.AddContent(seq++, _resolvedLabel);
        if (IsRequired())
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "form-field-required");
            builder.AddAttribute(seq++, "aria-hidden", "true");
            builder.AddContent(seq++, " *");
            builder.CloseElement();
        }
        builder.CloseElement();

        // Input (custom or auto)
        if (InputTemplate != null)
        {
            var inputContext = new FormFieldInputContext<TValue>
            {
                Value = value,
                OnChange = CreateChangeHandler(),
                IsDisabled = isDisabled,
                IsReadOnly = ReadOnly,
                HasErrors = hasErrors,
                Placeholder = _resolvedPlaceholder
            };
            builder.AddContent(seq++, InputTemplate(inputContext));
        }
        else
        {
            RenderInput(builder, ref seq, value, isDisabled, hasErrors);
        }

        // Validation errors
        if (ShowErrors && hasErrors)
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "form-field-errors");
            builder.AddAttribute(seq++, "role", "alert");
            foreach (var error in errors)
            {
                builder.OpenElement(seq++, "span");
                builder.AddAttribute(seq++, "class", "form-field-error");
                builder.AddContent(seq++, error);
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        builder.CloseElement(); // wrapper div
    }

    private void RenderInput(RenderTreeBuilder builder, ref int seq, TValue? value, bool isDisabled, bool hasErrors)
    {
        var inputClass = $"form-field-input{(hasErrors ? " invalid" : "")}";

        switch (_resolvedInputType)
        {
            case "textarea":
                RenderTextarea(builder, ref seq, value, isDisabled, inputClass);
                break;

            case "select":
                RenderSelect(builder, ref seq, value, isDisabled, inputClass);
                break;

            case "checkbox":
                RenderCheckbox(builder, ref seq, value, isDisabled, inputClass);
                break;

            default:
                RenderStandardInput(builder, ref seq, value, isDisabled, inputClass);
                break;
        }
    }

    private void RenderStandardInput(RenderTreeBuilder builder, ref int seq, TValue? value, bool isDisabled, string inputClass)
    {
        builder.OpenElement(seq++, "input");
        builder.AddAttribute(seq++, "type", _resolvedInputType);
        builder.AddAttribute(seq++, "class", inputClass);
        builder.AddAttribute(seq++, "value", FormatValueForInput(value));
        builder.AddAttribute(seq++, "placeholder", _resolvedPlaceholder);
        builder.AddAttribute(seq++, "disabled", isDisabled);
        builder.AddAttribute(seq++, "readonly", ReadOnly);
        builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputChanged));
        builder.AddAttribute(seq++, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, OnBlur));
        builder.CloseElement();
    }

    private void RenderTextarea(RenderTreeBuilder builder, ref int seq, TValue? value, bool isDisabled, string inputClass)
    {
        builder.OpenElement(seq++, "textarea");
        builder.AddAttribute(seq++, "class", inputClass);
        builder.AddAttribute(seq++, "rows", Rows);
        builder.AddAttribute(seq++, "placeholder", _resolvedPlaceholder);
        builder.AddAttribute(seq++, "disabled", isDisabled);
        builder.AddAttribute(seq++, "readonly", ReadOnly);
        builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputChanged));
        builder.AddAttribute(seq++, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, OnBlur));
        builder.AddContent(seq++, value?.ToString() ?? "");
        builder.CloseElement();
    }

    private void RenderSelect(RenderTreeBuilder builder, ref int seq, TValue? value, bool isDisabled, string inputClass)
    {
        builder.OpenElement(seq++, "select");
        builder.AddAttribute(seq++, "class", inputClass);
        builder.AddAttribute(seq++, "disabled", isDisabled);
        builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputChanged));

        var options = SelectOptions ?? GetEnumOptions();
        var currentValueString = value?.ToString() ?? "";

        foreach (var option in options)
        {
            builder.OpenElement(seq++, "option");
            builder.AddAttribute(seq++, "value", option.Value);
            if (option.Value == currentValueString)
            {
                builder.AddAttribute(seq++, "selected", true);
            }
            builder.AddContent(seq++, option.Label);
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    private void RenderCheckbox(RenderTreeBuilder builder, ref int seq, TValue? value, bool isDisabled, string inputClass)
    {
        var isChecked = value is bool b && b;

        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "form-field-checkbox-wrapper");

        builder.OpenElement(seq++, "input");
        builder.AddAttribute(seq++, "type", "checkbox");
        builder.AddAttribute(seq++, "class", inputClass);
        builder.AddAttribute(seq++, "checked", isChecked);
        builder.AddAttribute(seq++, "disabled", isDisabled);
        builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnCheckboxChanged));
        builder.CloseElement();

        builder.CloseElement();
    }

    #region Event Handlers

    private void OnInputChanged(ChangeEventArgs e)
    {
        if (Context == null) return;

        var newValue = ParseValue(e.Value?.ToString());
        Context.SetDraft(Property, newValue);

        // Trigger validation
        _ = Context.ValidateFieldAsync(Property);

        StateHasChanged();
    }

    private void OnCheckboxChanged(ChangeEventArgs e)
    {
        if (Context == null) return;

        var isChecked = e.Value is bool b ? b : e.Value?.ToString() == "true";
        var newValue = (TValue)(object)isChecked;
        Context.SetDraft(Property, newValue);

        StateHasChanged();
    }

    private async Task OnBlur(FocusEventArgs e)
    {
        if (Context == null) return;
        await Context.ValidateFieldAsync(Property);
        await InvokeAsync(StateHasChanged);
    }

    private EventCallback<ChangeEventArgs> CreateChangeHandler()
    {
        return EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputChanged);
    }

    #endregion

    #region Helpers

    private string BuildFieldClass(bool isDirty, bool hasErrors)
    {
        var classes = new List<string> { "form-field" };
        
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        if (isDirty)
            classes.Add("dirty");
        if (hasErrors)
            classes.Add("has-errors");
        if (ReadOnly)
            classes.Add("readonly");
        if (Disabled || Context?.IsSaving == true)
            classes.Add("disabled");

        return string.Join(" ", classes);
    }

    private static string GetPropertyName(Expression<Func<TGridItem, TValue>> property)
    {
        if (property.Body is MemberExpression memberExpr)
            return memberExpr.Member.Name;
        throw new ArgumentException("Expression must be a property access expression", nameof(property));
    }

    private string? GetDisplayName()
    {
        if (Property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
        {
            var displayAttr = propInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name;
        }
        return null;
    }

    private string? GetPlaceholderFromAttribute()
    {
        if (Property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
        {
            var displayAttr = propInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Prompt;
        }
        return null;
    }

    private bool IsRequired()
    {
        if (Property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
        {
            return propInfo.GetCustomAttribute<RequiredAttribute>() != null;
        }
        return false;
    }

    private string InferInputType()
    {
        var type = typeof(TValue);
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType switch
        {
            _ when underlyingType == typeof(string) => "text",
            _ when underlyingType == typeof(int) || underlyingType == typeof(long) ||
                   underlyingType == typeof(short) || underlyingType == typeof(byte) => "number",
            _ when underlyingType == typeof(decimal) || underlyingType == typeof(double) ||
                   underlyingType == typeof(float) => "number",
            _ when underlyingType == typeof(bool) => "checkbox",
            _ when underlyingType == typeof(DateTime) || underlyingType == typeof(DateOnly) => "date",
            _ when underlyingType == typeof(TimeOnly) => "time",
            _ when underlyingType == typeof(DateTimeOffset) => "datetime-local",
            _ when underlyingType.IsEnum => "select",
            _ => "text"
        };
    }

    private IEnumerable<SelectOption> GetEnumOptions()
    {
        var type = typeof(TValue);
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (!underlyingType.IsEnum)
            return Enumerable.Empty<SelectOption>();

        return Enum.GetValues(underlyingType)
            .Cast<object>()
            .Select(v => new SelectOption
            {
                Value = v.ToString()!,
                Label = GetEnumDisplayName(v) ?? v.ToString()!
            });
    }

    private static string? GetEnumDisplayName(object enumValue)
    {
        var type = enumValue.GetType();
        var memberInfo = type.GetMember(enumValue.ToString()!).FirstOrDefault();
        var displayAttr = memberInfo?.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name;
    }

    private string FormatValueForInput(TValue? value)
    {
        if (value == null) return "";

        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd"),
            DateOnly d => d.ToString("yyyy-MM-dd"),
            TimeOnly t => t.ToString("HH:mm"),
            DateTimeOffset dto => dto.ToString("yyyy-MM-ddTHH:mm"),
            _ => value.ToString() ?? ""
        };
    }

    private TValue? ParseValue(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return default;

        var targetType = typeof(TValue);
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            if (underlyingType == typeof(string))
                return (TValue)(object)input;

            if (underlyingType.IsEnum)
                return (TValue)Enum.Parse(underlyingType, input);

            if (underlyingType == typeof(DateTime))
                return (TValue)(object)DateTime.Parse(input);

            if (underlyingType == typeof(DateOnly))
                return (TValue)(object)DateOnly.Parse(input);

            if (underlyingType == typeof(TimeOnly))
                return (TValue)(object)TimeOnly.Parse(input);

            return (TValue)Convert.ChangeType(input, underlyingType);
        }
        catch
        {
            return default;
        }
    }

    #endregion
}

/// <summary>
/// Context provided to custom InputTemplate.
/// </summary>
public class FormFieldInputContext<TValue>
{
    public TValue? Value { get; init; }
    public EventCallback<ChangeEventArgs> OnChange { get; init; }
    public bool IsDisabled { get; init; }
    public bool IsReadOnly { get; init; }
    public bool HasErrors { get; init; }
    public string? Placeholder { get; init; }
}

/// <summary>
/// Option for select dropdowns.
/// </summary>
public class SelectOption
{
    public string Value { get; init; } = "";
    public string Label { get; init; } = "";
}
