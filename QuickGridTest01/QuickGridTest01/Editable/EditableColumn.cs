using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Globalization;

namespace QuickGridTest01.CustomColumns;

public enum EditorKind
{
    Auto,
    Text,
    Number,
    Checkbox,
    Date,
    DateTimeLocal,
    Time,
    TextArea,
    Select,
    RadioGroup
}

public record SelectOption<T>(T Value, string Text, bool Disabled = false);

public class EditableColumn<TGridItem, TValue> : ColumnBase<TGridItem>
{
    [Parameter] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;
    [Parameter] public List<IValidator<TValue>> Validators { get; set; } = new();
    [Parameter] public string? Format { get; set; }
    [Parameter] public string? InputType { get; set; }
    [Parameter] public EventCallback<CellValueChangedArgs<TGridItem, object>> OnValueChanged { get; set; }
    [Parameter] public bool ValidateOnChange { get; set; } = true;
    [Parameter] public RenderFragment<TValue>? DisplayTemplate { get; set; }
    [Parameter] public bool Inline { get; set; } = false;
    [Parameter] public bool UseDataAnnotations { get; set; } = false;
    [Parameter] public int DebounceMilliseconds { get; set; } = 0;
    [Parameter] public bool CommitOnInput { get; set; } = true;

    // New editor configuration parameters
    [Parameter] public EditorKind Editor { get; set; } = EditorKind.Auto;
    [Parameter] public IEnumerable<SelectOption<TValue>>? Options { get; set; }
    [Parameter] public Func<TValue?, string>? OptionText { get; set; }
    [Parameter] public bool EnumAsSelect { get; set; } = true;
    [Parameter] public bool UseTextArea { get; set; } = false;
    [Parameter] public int TextAreaRows { get; set; } = 3;
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public string? Step { get; set; }
    [Parameter] public string? Min { get; set; }
    [Parameter] public string? Max { get; set; }
    [Parameter] public CultureInfo? Culture { get; set; }

    private readonly Dictionary<TGridItem, EditState<TValue>> _editStates = new();
    private readonly Dictionary<TGridItem, System.Threading.Timer> _debounceTimers = new();

    private Expression<Func<TGridItem, TValue>>? _lastProperty;
    private Func<TGridItem, TValue>? _compiledProperty;
    private Action<TGridItem, TValue>? _propertySetter;
    private Func<TGridItem, string?>? _cellTextFunc;
    private GridSort<TGridItem>? _sortBuilder;
    private List<ValidationAttribute>? _dataAnnotationAttributes;
    private PropertyInfo? _boundPropertyInfo;

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnInitialized()
    {
        if (Property is null)
        {
            throw new InvalidOperationException($"{nameof(EditableColumn<TGridItem, TValue>)} requires a {nameof(Property)} parameter.");
        }
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (Title is null && Property.Body is MemberExpression titleExpr)
        {
            Title = titleExpr.Member.Name;
        }

        if (_lastProperty != Property)
        {
            _lastProperty = Property;
            _compiledProperty = Property.Compile();
            _propertySetter = BuildPropertySetter();

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

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var state = GetOrCreateEditState(item);

        // Inline mode: ensure we have captured initial model value (value types may be default/non-null so previous logic failed)
        if (Inline && _compiledProperty is not null && !state.IsInitialized)
        {
            // Capture without forcing edit semantics; treat as initialized current value but keep IsEditing true for inline rendering lifecycle
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
        builder.AddAttribute(seq++, "class", "editable-cell display-mode");
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
        builder.AddAttribute(seq++, "class", $"editable-cell edit-mode {(state.IsValid ? string.Empty : "invalid")}");
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
        // Initialization moved to CellContent; remove previous conditional
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", $"editable-cell inline-mode {(state.IsValid ? string.Empty : "invalid")}");
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
            // Checkbox uses checked and onchange
            var isChecked = false;
            if (state.CurrentValue is not null)
            {
                var underlying = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
                if (underlying == typeof(bool))
                {
                    isChecked = Convert.ToBoolean(state.CurrentValue, CultureInfo.InvariantCulture);
                }
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

        var t = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (t == typeof(bool)) return EditorKind.Checkbox;
        if (t == typeof(DateOnly)) return EditorKind.Date;
        if (t == typeof(TimeOnly)) return EditorKind.Time;
        if (t == typeof(DateTime)) return EditorKind.Date;
        if (t.IsEnum) return EnumAsSelect ? EditorKind.Select : EditorKind.RadioGroup;
        if (UseTextArea && t == typeof(string)) return EditorKind.TextArea;
        if (t == typeof(int) || t == typeof(long) || t == typeof(decimal) || t == typeof(double) || t == typeof(float)) return EditorKind.Number;
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
        var kind = kindOverride ?? GetEffectiveEditorKind();
        var culture = Culture ?? CultureInfo.InvariantCulture;

        var t = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (t == typeof(DateOnly))
        {
            var d = (DateOnly)(object)value;
            return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        if (t == typeof(TimeOnly))
        {
            var tm = (TimeOnly)(object)value;
            return tm.ToString("HH:mm", CultureInfo.InvariantCulture);
        }
        if (t == typeof(DateTime))
        {
            var dt = (DateTime)(object)value;
            return kind == EditorKind.DateTimeLocal
                ? dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture)
                : dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        if (t == typeof(int) || t == typeof(long) || t == typeof(decimal) || t == typeof(double) || t == typeof(float))
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }
        return value.ToString() ?? string.Empty;
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
        // Assume caller is already on the Blazor dispatcher (timer callback is wrapped in InvokeAsync).
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
        // Force re-render so parent change log updates immediately.
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

    private Action<TGridItem, TValue>? BuildPropertySetter()
    {
        if (Property.Body is not MemberExpression memberExpression) return null;
        var parameter = Property.Parameters[0];
        var valueParameter = Expression.Parameter(typeof(TValue), "value");
        var assign = Expression.Assign(memberExpression, valueParameter);
        return Expression.Lambda<Action<TGridItem, TValue>>(assign, parameter, valueParameter).Compile();
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

        var t = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (t.IsEnum)
        {
            var names = Enum.GetNames(t);
            var values = Enum.GetValues(t);
            var list = new List<SelectOption<TValue>>();
            int i = 0;
            foreach (var v in values)
            {
                var tv = (TValue)Convert.ChangeType(v, t, CultureInfo.InvariantCulture);
                list.Add(new SelectOption<TValue>(tv, names[i++]));
            }
            return list;
        }
        return Enumerable.Empty<SelectOption<TValue>>();
    }

    private string ToOptionValueString(TValue? value)
    {
        if (value is null) return string.Empty;
        var t = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (t.IsEnum) return value.ToString() ?? string.Empty; // use enum name
        if (t == typeof(DateOnly)) return ((DateOnly)(object)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (t == typeof(TimeOnly)) return ((TimeOnly)(object)value).ToString("HH:mm", CultureInfo.InvariantCulture);
        if (t == typeof(DateTime)) return ((DateTime)(object)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (t == typeof(int) || t == typeof(long) || t == typeof(decimal) || t == typeof(double) || t == typeof(float))
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        return value.ToString() ?? string.Empty;
    }

    private void UpdateStateValueFromEvent(EditState<TValue> state, ChangeEventArgs e)
    {
        try
        {
            var t = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (t == typeof(bool))
            {
                bool? parsedBool = null;
                if (e.Value is bool b) parsedBool = b;
                else if (e.Value is string sb)
                {
                    if (string.IsNullOrWhiteSpace(sb)) parsedBool = null;
                    else if (sb == "on") parsedBool = true; // checkbox HTML pattern
                    else if (bool.TryParse(sb, out var pb)) parsedBool = pb;
                }
                state.CurrentValue = parsedBool is null
                    ? default
                    : (TValue)(object)parsedBool.Value;
                return;
            }

            var s = e.Value?.ToString();
            if (string.IsNullOrWhiteSpace(s))
            {
                state.CurrentValue = default;
                return;
            }

            if (t.IsEnum)
            {
                try
                {
                    var ev = Enum.Parse(t, s, ignoreCase: true);
                    state.CurrentValue = (TValue)ev;
                }
                catch { state.CurrentValue = default; }
                return;
            }

            if (t == typeof(DateOnly))
            {
                if (DateOnly.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                    state.CurrentValue = (TValue)(object)d;
                else state.CurrentValue = default;
                return;
            }

            if (t == typeof(TimeOnly))
            {
                if (TimeOnly.TryParseExact(s, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tm))
                    state.CurrentValue = (TValue)(object)tm;
                else state.CurrentValue = default;
                return;
            }

            if (t == typeof(DateTime))
            {
                // accept both date and datetime-local formats
                if (DateTime.TryParseExact(s, new[] { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                    state.CurrentValue = (TValue)(object)dt;
                else state.CurrentValue = default;
                return;
            }

            if (t == typeof(int)) { state.CurrentValue = (TValue)(object)int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture); return; }
            if (t == typeof(long)) { state.CurrentValue = (TValue)(object)long.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture); return; }
            if (t == typeof(decimal)) { state.CurrentValue = (TValue)(object)decimal.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture); return; }
            if (t == typeof(double)) { state.CurrentValue = (TValue)(object)double.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture); return; }
            if (t == typeof(float)) { state.CurrentValue = (TValue)(object)float.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture); return; }

            // default: string or other convertible
            state.CurrentValue = (TValue)Convert.ChangeType(s, t, CultureInfo.InvariantCulture);
        }
        catch { }
    }
}

public class CellValueChangedArgs<TGridItem, TValue>
{
    public TGridItem Item { get; set; } = default!;
    public TValue OldValue { get; set; } = default!;
    public TValue NewValue { get; set; } = default!;
    public string PropertyName { get; set; } = string.Empty;
}
