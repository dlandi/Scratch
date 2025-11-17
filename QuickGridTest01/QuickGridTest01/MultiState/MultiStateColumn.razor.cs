using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.MultiState.Component;
using QuickGridTest01.MultiState.Core;
using QuickGridTest01.MultiState.Validation;
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure; // Added for TypeTraits + Accessors

namespace QuickGridTest01.MultiState;

/// <summary>
/// A QuickGrid column that supports inline editing with multiple states (Reading, Editing, Loading) and validation.
/// Provides optimistic updates and event hooks for advanced scenarios.
/// </summary>
/// <typeparam name="TGridItem">Row item type.</typeparam>
/// <typeparam name="TValue">Property value type.</typeparam>
public class MultiStateColumn<TGridItem, TValue> : ColumnBase<TGridItem>, IDisposable
    where TGridItem : class
{
    private readonly CellStateCoordinator<TGridItem, TValue> _stateCoordinator = new();
    private Func<TGridItem, TValue>? _compiledGetter;
    private Action<TGridItem, TValue>? _compiledSetter;
    private GridSort<TGridItem>? _sortBuilder;

    #region Parameters

    /// <summary>Expression that selects the property for this column. Required.</summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    /// <summary>Validators executed during draft validation.</summary>
    [Parameter]
    public List<IValidator<TValue>>? Validators { get; set; }

    /// <summary>Asynchronous save handler (server-side validation, etc.). Return success and optional error message.</summary>
    [Parameter]
    public Func<TGridItem, TValue, Task<(bool Success, string? Error)>>? OnSaveAsync { get; set; }

    /// <summary>Raised before entering edit mode; can cancel the edit operation.</summary>
    [Parameter]
    public EventCallback<BeforeEditEventArgs<TGridItem, TValue>> OnBeforeEdit { get; set; }

    /// <summary>Raised when the draft value changes.</summary>
    [Parameter]
    public EventCallback<ValueChangingEventArgs<TGridItem, TValue>> OnValueChanging { get; set; }

    /// <summary>Raised after save completes (success or failure).</summary>
    [Parameter]
    public EventCallback<SaveResultEventArgs<TGridItem, TValue>> OnSaveResult { get; set; }

    /// <summary>Raised on state transitions (Reading, Editing, Loading).</summary>
    [Parameter]
    public EventCallback<StateTransitionEventArgs<TGridItem>> OnStateChanged { get; set; }

    /// <summary>Raised when editing is cancelled.</summary>
    [Parameter]
    public EventCallback<CancelEditEventArgs<TGridItem, TValue>> OnCancelEdit { get; set; }

    /// <summary>Optional format string used for display and editor value.</summary>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>When true, renders validation errors under the cell.</summary>
    [Parameter]
    public bool ShowValidationErrors { get; set; } = true;

    /// <summary>Placeholder text for the editor input.</summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>When true, column is read-only and does not allow editing.</summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>When true, renders as an inline editor (focus to edit, blur/Enter to save).</summary>
    [Parameter]
    public bool Inline { get; set; } = false;

    #endregion

    #region ColumnBase Implementation

    /// <inheritdoc />
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Property == null)
            throw new InvalidOperationException($"{nameof(Property)} parameter is required.");

        _compiledGetter = Accessors.CreateGetter(Property);
        _compiledSetter = Accessors.CreateSetter(Property);

        if (string.IsNullOrEmpty(Title) && Property.Body is MemberExpression member)
        {
            Title = member.Member.Name;
        }

        if (Sortable ?? false)
        {
            _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
        }
    }

    /// <inheritdoc />
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var currentValue = _compiledGetter!(item);
        var state = _stateCoordinator.TryGetState(item, out var existingState)
            ? existingState!
            : new MultiState<TValue>
            {
                CurrentState = CellState.Reading,
                OriginalValue = currentValue,
                DraftValue = currentValue
            };

        builder.OpenElement(0, "div");
        builder.SetKey(item);
        builder.AddAttribute(1, "class", $"multistate-cell state-{state.CurrentState.ToString().ToLower()}");

        if (Inline)
        {
            RenderInlineCell(builder, item, state);
        }
        else
        {
            switch (state.CurrentState)
            {
                case CellState.Reading:
                    RenderReadingState(builder, item, state);
                    break;

                case CellState.Editing:
                    RenderEditingState(builder, item, state);
                    break;

                case CellState.Loading:
                    RenderLoadingState(builder, item, state);
                    break;
            }
        }

        builder.CloseElement();
    }

    #endregion

    #region Rendering Methods

    /// <summary>Renders the inline editor input and optional validation summary.</summary>
    private void RenderInlineCell(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        var isLoading = state.CurrentState == CellState.Loading;

        builder.OpenElement(0, "input");
        builder.AddAttribute(1, "class", "cell-input" + (state.HasValidationErrors ? " invalid" : ""));
        builder.AddAttribute(2, "type", "text");
        builder.AddAttribute(3, "value", FormatValue(state.DraftValue));
        builder.AddAttribute(4, "placeholder", Placeholder);
        builder.AddAttribute(5, "disabled", isLoading || IsReadOnly);
        builder.AddAttribute(6, "onfocus", EventCallback.Factory.Create(this, () => EnterEditAsync(item)));
        builder.AddAttribute(7, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnInputChanged(item, state, e)));
        builder.AddAttribute(8, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, e => OnKeyDown(item, state, e)));
        builder.AddAttribute(9, "onblur", EventCallback.Factory.Create(this, () => SaveEditAsync(item, state)));
        builder.CloseElement();

        if (isLoading)
        {
            builder.OpenElement(10, "span");
            builder.AddAttribute(11, "class", "loading-spinner");
            builder.AddAttribute(12, "aria-hidden", "true");
            builder.AddContent(13, "\u231B");
            builder.CloseElement();
        }

        if (ShowValidationErrors && state.HasValidationErrors)
        {
            builder.OpenElement(20, "div");
            builder.AddAttribute(21, "class", "validation-errors");
            builder.AddAttribute(22, "role", "alert");
            builder.AddAttribute(23, "aria-live", "polite");
            foreach (var error in state.ValidationErrors)
            {
                builder.OpenElement(24, "div");
                builder.AddAttribute(25, "class", "validation-error");
                builder.AddContent(26, error);
                builder.CloseElement();
            }
            builder.CloseElement();
        }
    }

    /// <summary>Renders the reading (display) state with an optional edit button.</summary>
    private void RenderReadingState(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "cell-reading");

        builder.OpenElement(2, "span");
        builder.AddAttribute(3, "class", "cell-value");
        builder.AddContent(4, FormatValue(state.OriginalValue));
        builder.CloseElement();

        if (!IsReadOnly && !Inline)
        {
            builder.OpenElement(5, "button");
            builder.AddAttribute(6, "class", "btn-edit");
            builder.AddAttribute(7, "type", "button");
            builder.AddAttribute(8, "title", $"Edit {Title ?? "value"}");
            builder.AddAttribute(9, "aria-label", $"Edit {Title ?? "value"}");
            builder.AddAttribute(10, "onclick", EventCallback.Factory.Create(this, () => EnterEditAsync(item)));
            builder.OpenElement(11, "svg");
            builder.AddAttribute(12, "class", "icon-edit");
            builder.AddAttribute(13, "width", "14");
            builder.AddAttribute(14, "height", "14");
            builder.AddAttribute(15, "viewBox", "0 0 14 14");
            builder.AddMarkupContent(16, "<path d=\"M3 11h8M11.2 3.8l-6.9 6.9L3 11l.3-1.3 6.9-6.9 1-1c.4-.4 1-.4 1.4 0 .4.4.4 1 0 1.4l-1.4 1.4z\" stroke=\"currentColor\" stroke-width=\"1.3\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />");
            builder.CloseElement();
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    /// <summary>Renders the editing state with Save/Cancel actions and optional validation errors.</summary>
    private void RenderEditingState(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "cell-editing");

        builder.OpenElement(2, "input");
        builder.AddAttribute(3, "class", "cell-input" + (state.HasValidationErrors ? " invalid" : ""));
        builder.AddAttribute(4, "type", "text");
        builder.AddAttribute(5, "value", FormatValue(state.DraftValue));
        builder.AddAttribute(6, "placeholder", Placeholder);
        builder.AddAttribute(7, "aria-label", $"Edit {Title ?? "value"}");
        builder.AddAttribute(8, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this,
            e => OnInputChanged(item, state, e)));
        builder.AddAttribute(9, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this,
            e => OnKeyDown(item, state, e)));
        builder.CloseElement();

        builder.OpenElement(10, "div");
        builder.AddAttribute(11, "class", "cell-actions");

        builder.OpenElement(12, "button");
        builder.AddAttribute(13, "class", "btn-save");
        builder.AddAttribute(14, "type", "button");
        builder.AddAttribute(15, "title", state.HasValidationErrors ? "Fix validation errors to save" : $"Save {Title ?? "value"}");
        builder.AddAttribute(16, "aria-label", $"Save {Title ?? "value"}");
        builder.AddAttribute(17, "disabled", state.HasValidationErrors);
        builder.AddAttribute(18, "onclick", EventCallback.Factory.Create(this, () => SaveEditAsync(item, state)));
        builder.AddContent(19, "\u2713");
        builder.CloseElement();

        builder.OpenElement(20, "button");
        builder.AddAttribute(21, "class", "btn-cancel");
        builder.AddAttribute(22, "type", "button");
        builder.AddAttribute(23, "title", $"Cancel editing {Title ?? "value"}");
        builder.AddAttribute(24, "aria-label", $"Cancel editing {Title ?? "value"}");
        builder.AddAttribute(25, "onclick", EventCallback.Factory.Create(this, () => CancelEditAsync(item, state)));
        builder.AddContent(26, "\u2717");
        builder.CloseElement();

        builder.CloseElement();

        if (ShowValidationErrors && state.HasValidationErrors)
        {
            builder.OpenElement(27, "div");
            builder.AddAttribute(28, "class", "validation-errors");
            builder.AddAttribute(29, "role", "alert");
            builder.AddAttribute(30, "aria-live", "polite");
            foreach (var error in state.ValidationErrors)
            {
                builder.OpenElement(31, "div");
                builder.AddAttribute(32, "class", "validation-error");
                builder.AddContent(33, error);
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    /// <summary>Renders the loading state with a small inline busy indicator.</summary>
    private void RenderLoadingState(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "cell-loading");
        builder.AddAttribute(2, "role", "status");
        builder.AddAttribute(3, "aria-live", "polite");
        builder.AddAttribute(4, "aria-label", $"Saving {Title ?? "value"}");

        builder.OpenElement(5, "span");
        builder.AddAttribute(6, "class", "loading-spinner");
        builder.AddAttribute(7, "aria-hidden", "true");
        builder.AddContent(8, "\u231B");
        builder.CloseElement();

        builder.OpenElement(9, "span");
        builder.AddAttribute(10, "class", "loading-text");
        builder.AddContent(11, "Saving...");
        builder.CloseElement();

        builder.CloseElement();
    }

    #endregion

    #region Event Handlers

    /// <summary>Begins editing the current row value (fires <see cref="OnBeforeEdit"/> and <see cref="OnStateChanged"/>).</summary>
    private async Task EnterEditAsync(TGridItem item)
    {
        var currentValue = _compiledGetter!(item);

        var beforeEditArgs = new BeforeEditEventArgs<TGridItem, TValue>(item, currentValue);
        await OnBeforeEdit.InvokeAsync(beforeEditArgs);

        if (beforeEditArgs.Cancel)
            return;

        var state = await _stateCoordinator.GetOrCreateStateAsync(item, currentValue);

        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Editing);

        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Editing);
        await OnStateChanged.InvokeAsync(stateChangedArgs);

        StateHasChanged();
    }

    /// <summary>Handles text input changes by parsing and validating the draft value.</summary>
    private void OnInputChanged(TGridItem item, MultiState<TValue> state, ChangeEventArgs e)
    {
        var oldValue = state.DraftValue;
        if (TypeTraits<TValue>.TryParseFromEventValue(e.Value, CultureInfo.InvariantCulture, out var parsed))
        {
            state.DraftValue = parsed!;
        }
        var valueChangingArgs = new ValueChangingEventArgs<TGridItem, TValue>(item, oldValue!, state.DraftValue!);
        OnValueChanging.InvokeAsync(valueChangingArgs);

        _ = ValidateAsync(state);

        StateHasChanged();
    }

    /// <summary>Responds to Enter/Escape keys to save or cancel the edit.</summary>
    private async Task OnKeyDown(TGridItem item, MultiState<TValue> state, KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SaveEditAsync(item, state);
        }
        else if (e.Key == "Escape")
        {
            await CancelEditAsync(item, state);
        }
    }

    /// <summary>Saves the draft value via <see cref="OnSaveAsync"/> or by writing to the model property.</summary>
    private async Task SaveEditAsync(TGridItem item, MultiState<TValue> state)
    {
        if (!await ValidateAsync(state))
        {
            StateHasChanged();
            return;
        }

        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Loading);
        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Loading);
        await OnStateChanged.InvokeAsync(stateChangedArgs);
        StateHasChanged();

        bool success;
        string? error;

        if (OnSaveAsync != null)
        {
            (success, error) = await OnSaveAsync(item, state.DraftValue!);
        }
        else
        {
            _compiledSetter?.Invoke(item, state.DraftValue!);
            success = true;
            error = null;
        }

        var saveResultArgs = new SaveResultEventArgs<TGridItem, TValue>(item, state.DraftValue!, success, error);
        await OnSaveResult.InvokeAsync(saveResultArgs);

        if (success)
        {
            state.CommitEdit();

            oldState = state.CurrentState;
            state.TransitionTo(CellState.Reading);
            stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Reading);
            await OnStateChanged.InvokeAsync(stateChangedArgs);
        }
        else
        {
            state.TransitionTo(CellState.Editing);
            if (!string.IsNullOrEmpty(error))
            {
                state.ValidationErrors.Clear();
                state.ValidationErrors.Add(error);
            }
        }

        StateHasChanged();
    }

    /// <summary>Cancels editing and restores the original value.</summary>
    private async Task CancelEditAsync(TGridItem item, MultiState<TValue> state)
    {
        var draftValue = state.DraftValue;
        var originalValue = state.OriginalValue;

        state.CancelEdit();

        var cancelArgs = new CancelEditEventArgs<TGridItem, TValue>(item, originalValue!, draftValue!);
        await OnCancelEdit.InvokeAsync(cancelArgs);

        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Reading);
        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Reading);
        await OnStateChanged.InvokeAsync(stateChangedArgs);

        StateHasChanged();
    }

    #endregion

    #region Validation and Formatting

    /// <summary>Validates the current draft value using configured validators.</summary>
    private async Task<bool> ValidateAsync(MultiState<TValue> state)
    {
        state.ValidationErrors.Clear();

        if (Validators == null || !Validators.Any())
            return true;

        foreach (var validator in Validators)
        {
            var result = await validator.ValidateAsync(state.DraftValue);
            if (!result.IsValid && !string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                state.ValidationErrors.Add(result.ErrorMessage);
            }
        }

        return !state.HasValidationErrors;
    }

    /// <summary>
    /// Formats a value for display/editor using <see cref="Format"/> when provided, otherwise using
    /// <see cref="TypeTraits{T}.FormatForInput(T, object?, CultureInfo)"/> for stable dates/times.
    /// </summary>
    private string FormatValue(TValue? value)
    {
        if (value == null)
            return string.Empty;

        if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
            return formattable.ToString(Format, null) ?? string.Empty;

        return TypeTraits<TValue>.FormatForInput(value, null, CultureInfo.InvariantCulture);
    }

    #endregion

    #region IDisposable

    /// <summary>Disposes any transient resources allocated by the coordinator.</summary>
    public void Dispose()
    {
        _stateCoordinator?.Dispose();
    }

    #endregion
}
