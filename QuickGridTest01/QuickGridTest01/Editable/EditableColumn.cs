using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Globalization;
using QuickGridTest01.Infrastructure;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Inline-editable QuickGrid column with validation, debouncing, and commit semantics.
/// Uses compiled accessors and <see cref="TypeTraits{T}"/> for high-performance rendering and input handling.
/// </summary>
/// <typeparam name="TGridItem">Row item type.</typeparam>
/// <typeparam name="TValue">Property value type.</typeparam>
public enum EditorKind
{
    /// <summary>Automatically select editor based on <see cref="TypeTraits{T}"/> and provided options.</summary>
    Auto,
    /// <summary>Plain text input.</summary>
    Text,
    /// <summary>Numeric input.</summary>
    Number,
    /// <summary>Checkbox input.</summary>
    Checkbox,
    /// <summary>Date input (yyyy-MM-dd).</summary>
    Date,
    /// <summary>Date/time local input (yyyy-MM-ddTHH:mm).</summary>
    DateTimeLocal,
    /// <summary>Time input (HH:mm).</summary>
    Time,
    /// <summary>Multiline text input.</summary>
    TextArea,
    /// <summary>Dropdown select.</summary>
    Select,
    /// <summary>Radio button group.</summary>
    RadioGroup
}

/// <summary>
/// Represents a selectable option for select/radio editors.
/// </summary>
public record SelectOption<T>(T Value, string Text, bool Disabled = false);

/// <summary>
/// QuickGrid column that renders a value and allows inline editing with optional validation.
/// </summary>
public class EditableColumn<TGridItem, TValue> : ColumnBase<TGridItem>, IDisposable
{
    /// <summary>
    /// Expression selecting the bound property to display/edit. Required.
    /// </summary>
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>Custom validators invoked when validation runs.</summary>
    [Parameter] public List<IValidator<TValue>> Validators { get; set; } = new();

    /// <summary>Display format string applied to <see cref="IFormattable"/> values.</summary>
    [Parameter] public string? Format { get; set; }

    /// <summary>Explicit HTML input type override (e.g., "text", "number").</summary>
    [Parameter] public string? InputType { get; set; }

    /// <summary>Callback raised when a value is committed (saved or inline-commit).</summary>
    [Parameter] public EventCallback<CellValueChangedArgs<TGridItem, object>> OnValueChanged { get; set; }

    /// <summary>When true, validates on input events instead of deferred save.</summary>
    [Parameter] public bool ValidateOnChange { get; set; } = true;

    /// <summary>Optional display-only template for read mode.</summary>
    [Parameter] public RenderFragment<TValue>? DisplayTemplate { get; set; }

    /// <summary>When true, always renders the editor (no explicit edit toggle).</summary>
    [Parameter] public bool Inline { get; set; } = false;

    /// <summary>When true, applies DataAnnotations validation attributes on the bound property.</summary>
    [Parameter] public bool UseDataAnnotations { get; set; } = false;

    /// <summary>Debounce delay (ms) for oninput validation/commit in inline mode.</summary>
    [Parameter] public int DebounceMilliseconds { get; set; } = 0;

    /// <summary>When true, commits valid changes automatically in inline mode.</summary>
    [Parameter] public bool CommitOnInput { get; set; } = true;

    /// <summary>Explicit editor kind. Defaults to <see cref="EditorKind.Auto"/>.</summary>
    [Parameter] public EditorKind Editor { get; set; } = EditorKind.Auto;

    /// <summary>Explicit options for select/radio editors. When provided, forces select/radio editor.</summary>
    [Parameter] public IEnumerable<SelectOption<TValue>>? Options { get; set; }

    /// <summary>Optional function that maps option values to display text.</summary>
    [Parameter] public Func<TValue?, string>? OptionText { get; set; }

    /// <summary>When true, enum types render as select/radio (otherwise text/number).</summary>
    [Parameter] public bool EnumAsSelect { get; set; } = true;

    /// <summary>When true for string values, uses a textarea.</summary>
    [Parameter] public bool UseTextArea { get; set; } = false;

    /// <summary>Number of rows for textarea.</summary>
    [Parameter] public int TextAreaRows { get; set; } = 3;

    /// <summary>Input placeholder text.</summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>HTML input step attribute.</summary>
    [Parameter] public string? Step { get; set; }

    /// <summary>HTML input min attribute.</summary>
    [Parameter] public string? Min { get; set; }

    /// <summary>HTML input max attribute.</summary>
    [Parameter] public string? Max { get; set; }

    /// <summary>Culture override for formatting/parsing where applicable.</summary>
    [Parameter] public CultureInfo? Culture { get; set; }

    // Internal state
    private readonly Dictionary<TGridItem, EditState<TValue>> _editStates = new();
    private readonly Dictionary<TGridItem, System.Threading.Timer> _debounceTimers = new();

    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledProperty; // getter
    private Action<TGridItem, TValue>? _propertySetter; // setter
    private Func<TGridItem, string?>? _cellTextFunc;
    private GridSort<TGridItem>? _sortBuilder;
    private List<ValidationAttribute>? _dataAnnotationAttributes;
    private PropertyInfo? _boundPropertyInfo;

    // Precomputed CSS classes for hot paths
    private static readonly string CssDisplay = "editable-cell display-mode";
    private static readonly string CssEditValid = "editable-cell edit-mode";
    private static readonly string CssEditInvalid = "editable-cell edit-mode invalid";
    private static readonly string CssInlineValid = "editable-cell inline-mode";
    private static readonly string CssInlineInvalid = "editable-cell inline-mode invalid";

    // Cached enum options per TValue
    private static readonly IReadOnlyList<SelectOption<TValue>> s_enumOptions =
        TypeTraits<TValue>.IsEnum ? TypeTraits<TValue>.BuildEnumOptions() : Array.Empty<SelectOption<TValue>>();

    /// <inheritdoc />
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException($"{nameof(EditableColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }
        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Title is null && Property.Body is MemberExpression titleExpr)
        {
            Title = titleExpr.Member.Name;
        }

        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledProperty = Accessors.CreateGetter(Property);
            _propertySetter = Accessors.CreateSetter(Property);

            if (Property.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo pi)
            {
                _boundPropertyInfo = pi;
                _dataAnnotationAttributes = UseDataAnnotations
                    ? pi.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().ToList()
                    : null;
            }
            else
            {
                _boundPropertyInfo = null;
                _dataAnnotationAttributes = null;
            }

            _cellTextFunc = !string.IsNullOrEmpty(Format)
                ? item =>
                {
                    var value = _compiledProperty!(item);
                    if (value is IFormattable formattable) return formattable.ToString(Format, null);
                    return value?.ToString();
                }
                : item => _compiledProperty!(item)?.ToString();

            if (Sortable ?? false)
            {
                _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
            }
        }

        base.OnParametersSet();
    }

    /// <inheritdoc />
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var state = GetOrCreateEditState(item);

        // Inline: seed current value once
        if (Inline && _compiledProperty is not null && !state.IsInitialized)
        {
            state.BeginEdit(_compiledProperty(item));
        }

        if (Inline)
        {
            RenderInlineEditor(builder, item, state);
            return;
        }
        if (state.IsEditing) RenderEditMode(builder, item, state); else RenderDisplayMode(builder, item, state);
    }

    private void RenderDisplayMode(RenderTreeBuilder builder, TGridItem item, EditState<TValue> state)
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.SetKey(item);
        builder.AddAttribute(seq++, "class", CssDisplay);
        builder.AddAttribute(seq++, "onclick:stopPropagation", true);
        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => EnterEditMode(item)));
        builder.AddAttribute(seq++, "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => EnterEditMode(item)));
        builder.AddAttribute(seq++, "title", "Click to edit");
        if (DisplayTemplate is not null)
        {
            builder.AddContent(seq++, DisplayTemplate(_compiledProperty!(item)));
        }
        else
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "cell-value");
            builder.AddContent(seq++, _cellTextFunc!(item) ?? string.Empty);
            builder.CloseElement();
        }
        builder.OpenElement(seq++, "i");
        builder.AddAttribute(seq++, "class", "bi bi-pencil edit-icon");
        builder.CloseElement();
        builder.CloseElement();
    }

    private void RenderEditMode(RenderTreeBuilder builder, TGridItem item, EditState<TValue> state)
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.SetKey(item);
        builder.AddAttribute(seq++, "class", state.IsValid ? CssEditValid : CssEditInvalid);
        builder.AddAttribute(seq++, "onclick:stopPropagation", true);
        RenderEditor(builder, ref seq, item, state, includeAutofocus: true);
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "edit-actions");
        // Save
        builder.OpenElement(seq++, "button");
        builder.AddAttribute(seq++, "type", "button");
        builder.AddAttribute(seq++, "class", "btn-save");
        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => SaveEdit(item)));
        builder.AddAttribute(seq++, "disabled", state.IsValidating || !state.IsValid);
        builder.AddAttribute(seq++, "title", state.IsValid ? "Save" : "Fix validation errors");
        builder.OpenElement(seq++, "i");
        builder.AddAttribute(seq++, "class", "bi bi-check");
        builder.CloseElement();
        builder.CloseElement();
        // Cancel
        builder.OpenElement(seq++, "button");
        builder.AddAttribute(seq++, "type", "button");
        builder.AddAttribute(seq++, "class", "btn-cancel");
        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => CancelEdit(item)));
        builder.AddAttribute(seq++, "title", "Cancel");
        builder.OpenElement(seq++, "i");
        builder.AddAttribute(seq++, "class", "bi bi-x");
        builder.CloseElement();
        builder.CloseElement();
        builder.CloseElement();
        RenderValidationFeedback(builder, ref seq, state);
        builder.CloseElement();
    }

    private void RenderInlineEditor(RenderTreeBuilder builder, TGridItem item, EditState<TValue> state)
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.SetKey(item);
        builder.AddAttribute(seq++, "class", state.IsValid ? CssInlineValid : CssInlineInvalid);
        builder.AddAttribute(seq++, "onclick:stopPropagation", true);
        RenderEditor(builder, ref seq, item, state, includeAutofocus: false, cssClass: "edit-input seamless-input");
        RenderValidationFeedback(builder, ref seq, state);
        builder.CloseElement();
    }

    private void RenderEditor(RenderTreeBuilder builder, ref int seq, TGridItem item, EditState<TValue> state, bool includeAutofocus, string cssClass = "edit-input")
    {
        var kind = GetEffectiveEditorKind();
        switch (kind)
        {
            case EditorKind.Select:
                RenderSelect(builder, ref seq, item, state, cssClass);
                break;
            case EditorKind.RadioGroup:
                RenderRadioGroup(builder, ref seq, item, state, cssClass);
                break;
            case EditorKind.TextArea:
                RenderTextArea(builder, ref seq, item, state, cssClass);
                break;
            default:
                RenderStandardInput(builder, ref seq, item, state, includeAutofocus, cssClass, kind);
                break;
        }
    }

    private void RenderStandardInput(RenderTreeBuilder builder, ref int seq, TGridItem item, EditState<TValue> state, bool includeAutofocus, string cssClass, EditorKind kind)
    {
        var inputType = GetInputType(kind);
        builder.OpenElement(seq++, "input");
        builder.AddAttribute(seq++, "type", inputType);
        builder.AddAttribute(seq++, "class", cssClass);

        if (inputType != "checkbox")
        {
            builder.AddAttribute(seq++, "value", FormatValueForInput(state.CurrentValue, kind));
            if (!string.IsNullOrEmpty(Placeholder)) builder.AddAttribute(seq++, "placeholder", Placeholder);
            if (!string.IsNullOrEmpty(Step)) builder.AddAttribute(seq++, "step", Step);
            if (!string.IsNullOrEmpty(Min)) builder.AddAttribute(seq++, "min", Min);
            if (!string.IsNullOrEmpty(Max)) builder.AddAttribute(seq++, "max", Max);
            var eventName = ValidateOnChange ? "oninput" : "onchange";
            if (DebounceMilliseconds > 0 && ValidateOnChange)
            {
                builder.AddAttribute(seq++, eventName, EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputWithDebounce(item, e)));
            }
            else
            {
                builder.AddAttribute(seq++, eventName, EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, e)));
            }
        }
        else
        {
            var isChecked = false;
            if (state.CurrentValue is not null && TypeTraits<TValue>.Kind == ValueKind.Boolean)
            {
                isChecked = Convert.ToBoolean(state.CurrentValue, CultureInfo.InvariantCulture);
            }
            builder.AddAttribute(seq++, "checked", isChecked);
            builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, e)));
        }

        if (includeAutofocus) builder.AddAttribute(seq++, "autofocus", true);
        builder.AddAttribute(seq++, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, e => OnKeyDown(item, e)));
        builder.CloseElement();
    }

    private void RenderSelect(RenderTreeBuilder builder, ref int seq, TGridItem item, EditState<TValue> state, string cssClass)
    {
        builder.OpenElement(seq++, "select");
        builder.AddAttribute(seq++, "class", cssClass);
        if (!string.IsNullOrEmpty(Placeholder)) builder.AddAttribute(seq++, "aria-label", Placeholder);
        builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, e)));

        var opts = GetEffectiveOptions();
        foreach (var opt in opts)
        {
            builder.OpenElement(seq++, "option");
            var valStr = ToOptionValueString(opt.Value);
            builder.AddAttribute(seq++, "value", valStr);
            if (opt.Disabled) builder.AddAttribute(seq++, "disabled", true);

            var isSelected = EqualityComparer<TValue?>.Default.Equals(opt.Value, state.CurrentValue);
            if (isSelected) builder.AddAttribute(seq++, "selected", true);

            builder.AddContent(seq++, OptionText?.Invoke(opt.Value) ?? opt.Text);
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    private void RenderRadioGroup(RenderTreeBuilder builder, ref int seq, TGridItem item, EditState<TValue> state, string cssClass)
    {
        var groupName = $"rg-{Title}-{GetHashCode()}-{item?.GetHashCode()}";
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", $"radio-group {cssClass}");

        var opts = GetEffectiveOptions();
        foreach (var opt in opts)
        {
            var valStr = ToOptionValueString(opt.Value);
            var isChecked = EqualityComparer<TValue?>.Default.Equals(opt.Value, state.CurrentValue);

            builder.OpenElement(seq++, "label");
            builder.AddAttribute(seq++, "class", "radio-item");

            builder.OpenElement(seq++, "input");
            builder.AddAttribute(seq++, "type", "radio");
            builder.AddAttribute(seq++, "name", groupName);
            builder.AddAttribute(seq++, "value", valStr);
            if (isChecked) builder.AddAttribute(seq++, "checked", true);
            if (opt.Disabled) builder.AddAttribute(seq++, "disabled", true);
            builder.AddAttribute(seq++, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, e)));
            builder.CloseElement();

            builder.AddContent(seq++, OptionText?.Invoke(opt.Value) ?? opt.Text);
            builder.CloseElement(); // label
        }

        builder.CloseElement();
    }

    private void RenderTextArea(RenderTreeBuilder builder, ref int seq, TGridItem item, EditState<TValue> state, string cssClass)
    {
        builder.OpenElement(seq++, "textarea");
        builder.AddAttribute(seq++, "class", cssClass);
        builder.AddAttribute(seq++, "rows", TextAreaRows);
        if (!string.IsNullOrEmpty(Placeholder)) builder.AddAttribute(seq++, "placeholder", Placeholder);
        var eventName = ValidateOnChange ? "oninput" : "onchange";
        if (DebounceMilliseconds > 0 && ValidateOnChange)
        {
            builder.AddAttribute(seq++, eventName, EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputWithDebounce(item, e)));
        }
        else
        {
            builder.AddAttribute(seq++, eventName, EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, e)));
        }
        builder.AddContent(seq++, FormatValueForInput(state.CurrentValue, EditorKind.TextArea));
        builder.CloseElement();
    }

    private void RenderValidationFeedback(RenderTreeBuilder builder, ref int seq, EditState<TValue> state)
    {
        if (!state.IsValid && state.ErrorMessages.Any())
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "validation-errors");
            foreach (var error in state.ErrorMessages)
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "validation-error");
                builder.AddContent(seq++, error);
                builder.CloseElement();
            }
            builder.CloseElement();
        }
        if (state.IsValidating)
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "validation-loading");
            builder.AddContent(seq++, "Validating...");
            builder.CloseElement();
        }
    }

    private EditorKind GetEffectiveEditorKind()
    {
        if (Editor != EditorKind.Auto)
            return Editor;

        // Options take precedence
        if (Options is not null)
            return EditorKind.Select;

        var k = TypeTraits<TValue>.Kind;
        if (k == ValueKind.Boolean) return EditorKind.Checkbox;
        if (k == ValueKind.Date) return EditorKind.Date;
        if (k == ValueKind.Time) return EditorKind.Time;
        if (k == ValueKind.DateTime) return EditorKind.Date; // default to date unless explicitly DateTimeLocal
        if (TypeTraits<TValue>.IsEnum) return EnumAsSelect ? EditorKind.Select : EditorKind.RadioGroup;
        if (UseTextArea && k == ValueKind.String) return EditorKind.TextArea;
        if (k is ValueKind.Int32 or ValueKind.Int64 or ValueKind.Decimal or ValueKind.Double or ValueKind.Single) return EditorKind.Number;
        return EditorKind.Text;
    }

    private string GetInputType(EditorKind kind)
    {
        if (!string.IsNullOrWhiteSpace(InputType)) return InputType!;

        return kind switch
        {
            EditorKind.Number => "number",
            EditorKind.Checkbox => "checkbox",
            EditorKind.Date => "date",
            EditorKind.DateTimeLocal => "datetime-local",
            EditorKind.Time => "time",
            _ => "text"
        };
    }

    private string FormatValueForInput(TValue? value, EditorKind? kindOverride = null)
    {
        if (value is null) return string.Empty;
        var culture = Culture ?? CultureInfo.InvariantCulture;

        // Allow instance Format override when value implements IFormattable and a Format was provided
        if (!string.IsNullOrEmpty(Format) && value is IFormattable f)
        {
            return f.ToString(Format, null) ?? string.Empty;
        }

        return TypeTraits<TValue>.FormatForInput(value, kindOverride, culture);
    }

    private EditState<TValue> GetOrCreateEditState(TGridItem item)
    {
        if (!_editStates.TryGetValue(item, out var state))
        {
            state = new EditState<TValue>();
            _editStates[item] = state;
        }
        return state;
    }

    private void EnterEditMode(TGridItem item)
    {
        var state = GetOrCreateEditState(item);
        state.BeginEdit(_compiledProperty!(item));
        StateHasChanged();
    }

    private async Task OnInputChanged(TGridItem item, ChangeEventArgs e)
    {
        var state = GetOrCreateEditState(item);
        UpdateStateValueFromEvent(state, e);
        if (ValidateOnChange)
        {
            await ValidateAsync(item, state);
            if (Inline && CommitOnInput && state.IsValid && state.IsDirty && _propertySetter is not null)
            {
                await CommitValueAsync(item, state);
            }
        }
        StateHasChanged();
    }

    private Task OnInputWithDebounce(TGridItem item, ChangeEventArgs e)
    {
        var state = GetOrCreateEditState(item);
        UpdateStateValueFromEvent(state, e);
        if (_debounceTimers.TryGetValue(item, out var existing)) existing.Dispose();
        var timer = new System.Threading.Timer(_ =>
        {
            // Marshal entire debounce work to UI thread
            _ = InvokeAsync(async () =>
            {
                if (ValidateOnChange)
                {
                    await ValidateAsync(item, state);
                    if (Inline && CommitOnInput && state.IsValid && state.IsDirty && _propertySetter is not null)
                    {
                        await CommitValueAsync(item, state);
                    }
                }
                StateHasChanged();
            });
        }, null, DebounceMilliseconds, System.Threading.Timeout.Infinite);
        _debounceTimers[item] = timer;
        return Task.CompletedTask;
    }

    private async Task CommitValueAsync(TGridItem item, EditState<TValue> state)
    {
        var oldValue = _compiledProperty!(item);
        _propertySetter!(item, state.CurrentValue!);
        var propertyName = Title ?? _boundPropertyInfo?.Name ?? "Value";
        await OnValueChanged.InvokeAsync(new CellValueChangedArgs<TGridItem, object>
        {
            Item = item,
            OldValue = oldValue!,
            NewValue = state.CurrentValue!,
            PropertyName = propertyName
        });
        if (Inline) state.OriginalValue = state.CurrentValue;
        StateHasChanged();
    }

    private async Task SaveEdit(TGridItem item)
    {
        var state = GetOrCreateEditState(item);
        if (!ValidateOnChange || state.ValidationResults.Count == 0) await ValidateAsync(item, state);
        if (!state.IsValid) { StateHasChanged(); return; }
        if (state.IsDirty && _propertySetter is not null) await CommitValueAsync(item, state);
        state.CommitEdit();
        StateHasChanged();
    }

    private void CancelEdit(TGridItem item)
    {
        var state = GetOrCreateEditState(item);
        state.CancelEdit();
        StateHasChanged();
    }

    private async Task ValidateAsync(TGridItem item, EditState<TValue> state)
    {
        // Perform validation logic on UI thread to avoid dispatcher issues and concurrent state mutation
        await InvokeAsync(async () =>
        {
            state.IsValidating = true;
            state.ValidationResults.Clear();
            StateHasChanged();
            try
            {
                foreach (var validator in Validators)
                {
                    var result = await validator.ValidateAsync(state.CurrentValue);
                    state.ValidationResults.Add(result);
                }
                if (UseDataAnnotations && _dataAnnotationAttributes is not null && _boundPropertyInfo is not null)
                {
                    var ctx = new ValidationContext(item) { MemberName = _boundPropertyInfo.Name };
                    foreach (var attr in _dataAnnotationAttributes)
                    {
                        var res = attr.GetValidationResult(state.CurrentValue, ctx);
                        if (res is not null) state.ValidationResults.Add(ValidationResult.Failure(res.ErrorMessage ?? "Invalid"));
                    }
                }
            }
            finally
            {
                state.IsValidating = false;
                StateHasChanged();
            }
        });
    }

    private async Task OnKeyDown(TGridItem item, KeyboardEventArgs e)
    {
        if (Inline)
        {
            if (e.Key == "Enter" && !CommitOnInput)
            {
                await SaveEdit(item);
            }
            else if (e.Key == "Escape")
            {
                CancelEdit(item);
            }
            return;
        }
        if (e.Key == "Enter")
        {
            await SaveEdit(item);
        }
        else if (e.Key == "Escape")
        {
            CancelEdit(item);
        }
    }

    private IEnumerable<SelectOption<TValue>> GetEffectiveOptions()
    {
        if (Options is not null) return Options;
        if (TypeTraits<TValue>.IsEnum) return s_enumOptions;
        return Enumerable.Empty<SelectOption<TValue>>();
    }

    private string ToOptionValueString(TValue? value)
        => TypeTraits<TValue>.ToOptionValueString(value, Culture ?? CultureInfo.InvariantCulture);

    private void UpdateStateValueFromEvent(EditState<TValue> state, ChangeEventArgs e)
    {
        try
        {
            if (TypeTraits<TValue>.TryParseFromEventValue(e.Value, Culture ?? CultureInfo.InvariantCulture, out var parsed))
            {
                state.CurrentValue = parsed;
            }
        }
        catch { }
    }

    /// <summary>
    /// Disposes any outstanding debounce timers to avoid timer root leaks.
    /// </summary>
    public void Dispose()
    {
        foreach (var t in _debounceTimers.Values)
            t.Dispose();
        _debounceTimers.Clear();
    }
}

/// <summary>
/// Event payload describing a committed value change from the column.
/// </summary>
public class CellValueChangedArgs<TGridItem, TValue>
{
    /// <summary>The affected item (row) whose value changed.</summary>
    public TGridItem Item { get; set; } = default!;

    /// <summary>Old value captured at edit-begin or prior commit.</summary>
    public TValue OldValue { get; set; } = default!;

    /// <summary>New value after commit.</summary>
    public TValue NewValue { get; set; } = default!;

    /// <summary>Property name used for display/logging.</summary>
    public string PropertyName { get; set; } = string.Empty;
}
