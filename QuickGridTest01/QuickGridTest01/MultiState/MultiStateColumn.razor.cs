using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.MultiState.Component;
using QuickGridTest01.MultiState.Core;
using QuickGridTest01.MultiState.Validation;
using System.Linq.Expressions;
using System.Globalization;
using QuickGridTest01.Infrastructure; // Added for TypeTraits

namespace QuickGridTest01.MultiState;

/// <summary>
/// A QuickGrid column that supports inline editing with multiple states (Reading, Editing, Loading).
/// Includes validation, event callbacks, and optimistic UI updates.
/// </summary>
/// <typeparam name="TGridItem">The type of data item displayed in the grid row</typeparam>
/// <typeparam name="TValue">The type of the property value being edited</typeparam>
public class MultiStateColumn<TGridItem, TValue> : ColumnBase<TGridItem>, IDisposable
    where TGridItem : class
{
    private readonly CellStateCoordinator<TGridItem, TValue> _stateCoordinator = new();
    private Func<TGridItem, TValue>? _compiledGetter;
    private Action<TGridItem, TValue>? _compiledSetter;
    private GridSort<TGridItem>? _sortBuilder;

    #region Parameters

    /// <summary>
    /// Gets or sets the property expression for the column.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    /// <summary>
    /// Gets or sets the list of validators for this column.
    /// </summary>
    [Parameter]
    public List<IValidator<TValue>>? Validators { get; set; }

    /// <summary>
    /// Gets or sets the save callback (returns success and optional error message).
    /// </summary>
    [Parameter]
    public Func<TGridItem, TValue, Task<(bool Success, string? Error)>>? OnSaveAsync { get; set; }

    /// <summary>
    /// Gets or sets the event callback fired before editing begins (cancellable).
    /// </summary>
    [Parameter]
    public EventCallback<BeforeEditEventArgs<TGridItem, TValue>> OnBeforeEdit { get; set; }

    /// <summary>
    /// Gets or sets the event callback fired when value is changing.
    /// </summary>
    [Parameter]
    public EventCallback<ValueChangingEventArgs<TGridItem, TValue>> OnValueChanging { get; set; }

    /// <summary>
    /// Gets or sets the event callback fired after save completes.
    /// </summary>
    [Parameter]
    public EventCallback<SaveResultEventArgs<TGridItem, TValue>> OnSaveResult { get; set; }

    /// <summary>
    /// Gets or sets the event callback fired when state transitions occur.
    /// </summary>
    [Parameter]
    public EventCallback<StateTransitionEventArgs<TGridItem>> OnStateChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback fired when edit is cancelled.
    /// </summary>
    [Parameter]
    public EventCallback<CancelEditEventArgs<TGridItem, TValue>> OnCancelEdit { get; set; }

    /// <summary>
    /// Gets or sets the format string for displaying values.
    /// </summary>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets whether to show validation errors inline.
    /// </summary>
    [Parameter]
    public bool ShowValidationErrors { get; set; } = true;

    /// <summary>
    /// Gets or sets the placeholder text for empty inputs.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets whether the column is read-only.
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// When true, renders as an inline editor (no explicit edit button). Focus to enter edit state, blur/Enter to save.
    /// </summary>
    [Parameter]
    public bool Inline { get; set; } = false;

    #endregion

    #region ColumnBase Implementation

    /// <summary>
    /// Implement abstract property from ColumnBase
    /// </summary>
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnParametersSet()
    {
        if (Property == null)
            throw new InvalidOperationException($"{nameof(Property)} parameter is required.");

        // Compile property accessor
        _compiledGetter = Property.Compile();

        // Try to compile property setter
        if (Property.Body is MemberExpression memberExpr)
        {
            var param = Expression.Parameter(typeof(TGridItem));
            var valueParam = Expression.Parameter(typeof(TValue));
            var assign = Expression.Assign(
                Expression.Property(param, memberExpr.Member.Name),
                valueParam);
            _compiledSetter = Expression.Lambda<Action<TGridItem, TValue>>(assign, param, valueParam).Compile();
        }

        // Set title if not already set
        if (string.IsNullOrEmpty(Title) && Property.Body is MemberExpression member)
        {
            Title = member.Member.Name;
        }

        // Configure sorting if the column is sortable
        if (Sortable ?? false)
        {
            _sortBuilder = GridSort<TGridItem>.ByAscending(Property);
        }
    }

    /// <summary>
    /// Implement abstract method from ColumnBase - renders the cell content
    /// </summary>
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

        builder.CloseElement(); // div
    }

    #endregion

    #region Rendering Methods

    private void RenderInlineCell(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        var isLoading = state.CurrentState == CellState.Loading;

        // The input element
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

        // Validation errors
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

    private void RenderReadingState(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "cell-reading");

        // Display value
        builder.OpenElement(2, "span");
        builder.AddAttribute(3, "class", "cell-value");
        builder.AddContent(4, FormatValue(state.OriginalValue));
        builder.CloseElement(); // span

        // Edit button removed when Inline=true
        if (!IsReadOnly && !Inline)
        {
            builder.OpenElement(5, "button");
            builder.AddAttribute(6, "class", "btn-edit");
            builder.AddAttribute(7, "type", "button");
            builder.AddAttribute(8, "title", $"Edit {Title ?? "value"}");
            builder.AddAttribute(9, "aria-label", $"Edit {Title ?? "value"}");
            builder.AddAttribute(10, "onclick", EventCallback.Factory.Create(this, () => EnterEditAsync(item)));
            // Minimal inline SVG pencil icon (stroke only, currentColor)
            builder.OpenElement(11, "svg");
            builder.AddAttribute(12, "class", "icon-edit");
            builder.AddAttribute(13, "width", "14");
            builder.AddAttribute(14, "height", "14");
            builder.AddAttribute(15, "viewBox", "0 0 14 14");
            builder.AddMarkupContent(16, "<path d=\"M3 11h8M11.2 3.8l-6.9 6.9L3 11l.3-1.3 6.9-6.9 1-1c.4-.4 1-.4 1.4 0 .4.4.4 1 0 1.4l-1.4 1.4z\" stroke=\"currentColor\" stroke-width=\"1.3\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />");
            builder.CloseElement(); // svg
            builder.CloseElement(); // button
        }

        builder.CloseElement(); // div
    }

    private void RenderEditingState(RenderTreeBuilder builder, TGridItem item, MultiState<TValue> state)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "cell-editing");

        // Input field
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
        builder.CloseElement(); // input

        // Action buttons
        builder.OpenElement(10, "div");
        builder.AddAttribute(11, "class", "cell-actions");

        // Save button
        builder.OpenElement(12, "button");
        builder.AddAttribute(13, "class", "btn-save");
        builder.AddAttribute(14, "type", "button");
        builder.AddAttribute(15, "title", state.HasValidationErrors ? "Fix validation errors to save" : $"Save {Title ?? "value"}");
        builder.AddAttribute(16, "aria-label", $"Save {Title ?? "value"}");
        builder.AddAttribute(17, "disabled", state.HasValidationErrors);
        builder.AddAttribute(18, "onclick", EventCallback.Factory.Create(this, () => SaveEditAsync(item, state)));
        builder.AddContent(19, "\u2713"); // check
        builder.CloseElement(); // button

        // Cancel button
        builder.OpenElement(20, "button");
        builder.AddAttribute(21, "class", "btn-cancel");
        builder.AddAttribute(22, "type", "button");
        builder.AddAttribute(23, "title", $"Cancel editing {Title ?? "value"}");
        builder.AddAttribute(24, "aria-label", $"Cancel editing {Title ?? "value"}");
        builder.AddAttribute(25, "onclick", EventCallback.Factory.Create(this, () => CancelEditAsync(item, state)));
        builder.AddContent(26, "\u2717"); // X
        builder.CloseElement(); // button

        builder.CloseElement(); // div.cell-actions

        // Validation errors
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
                builder.CloseElement(); // div
            }
            builder.CloseElement(); // div.validation-errors
        }

        builder.CloseElement(); // div.cell-editing
    }

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
        builder.AddContent(8, "\u231B"); // Hourglass
        builder.CloseElement(); // span

        builder.OpenElement(9, "span");
        builder.AddAttribute(10, "class", "loading-text");
        builder.AddContent(11, "Saving...");
        builder.CloseElement(); // span

        builder.CloseElement(); // div
    }

    #endregion

    #region Event Handlers

    private async Task EnterEditAsync(TGridItem item)
    {
        var currentValue = _compiledGetter!(item);

        // Fire before edit event
        var beforeEditArgs = new BeforeEditEventArgs<TGridItem, TValue>(item, currentValue);
        await OnBeforeEdit.InvokeAsync(beforeEditArgs);

        if (beforeEditArgs.Cancel)
            return;

        // Get or create state
        var state = await _stateCoordinator.GetOrCreateStateAsync(item, currentValue);

        // Transition to editing
        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Editing);

        // Fire state changed event
        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Editing);
        await OnStateChanged.InvokeAsync(stateChangedArgs);

        StateHasChanged();
    }

    private void OnInputChanged(TGridItem item, MultiState<TValue> state, ChangeEventArgs e)
    {
        var oldValue = state.DraftValue;
        if (TypeTraits<TValue>.TryParseFromEventValue(e.Value, CultureInfo.InvariantCulture, out var parsed))
        {
            state.DraftValue = parsed!;
        }
        var valueChangingArgs = new ValueChangingEventArgs<TGridItem, TValue>(item, oldValue!, state.DraftValue!);
        OnValueChanging.InvokeAsync(valueChangingArgs);

        // Validate
        _ = ValidateAsync(state);

        StateHasChanged();
    }

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

    private async Task SaveEditAsync(TGridItem item, MultiState<TValue> state)
    {
        // Validate
        if (!await ValidateAsync(state))
        {
            StateHasChanged();
            return;
        }

        // Transition to loading
        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Loading);
        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Loading);
        await OnStateChanged.InvokeAsync(stateChangedArgs);
        StateHasChanged();

        // Save
        bool success;
        string? error;

        if (OnSaveAsync != null)
        {
            (success, error) = await OnSaveAsync(item, state.DraftValue!);
        }
        else
        {
            // Default behavior: update property directly
            _compiledSetter?.Invoke(item, state.DraftValue!);
            success = true;
            error = null;
        }

        // Fire save result event
        var saveResultArgs = new SaveResultEventArgs<TGridItem, TValue>(item, state.DraftValue!, success, error);
        await OnSaveResult.InvokeAsync(saveResultArgs);

        if (success)
        {
            // Commit edit
            state.CommitEdit();

            // Transition back to reading
            oldState = state.CurrentState;
            state.TransitionTo(CellState.Reading);
            stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Reading);
            await OnStateChanged.InvokeAsync(stateChangedArgs);
        }
        else
        {
            // Stay in editing state and show error
            state.TransitionTo(CellState.Editing);
            if (!string.IsNullOrEmpty(error))
            {
                state.ValidationErrors.Clear();
                state.ValidationErrors.Add(error);
            }
        }

        StateHasChanged();
    }

    private async Task CancelEditAsync(TGridItem item, MultiState<TValue> state)
    {
        var draftValue = state.DraftValue;
        var originalValue = state.OriginalValue;

        // Cancel edit (restore original value)
        state.CancelEdit();

        // Fire cancel event
        var cancelArgs = new CancelEditEventArgs<TGridItem, TValue>(item, originalValue!, draftValue!);
        await OnCancelEdit.InvokeAsync(cancelArgs);

        // Transition back to reading
        var oldState = state.CurrentState;
        state.TransitionTo(CellState.Reading);
        var stateChangedArgs = new StateTransitionEventArgs<TGridItem>(item, oldState, CellState.Reading);
        await OnStateChanged.InvokeAsync(stateChangedArgs);

        StateHasChanged();
    }

    #endregion

    #region Validation and Formatting

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

    private string FormatValue(TValue? value)
    {
        if (value == null)
            return string.Empty;

        if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
            return formattable.ToString(Format, null);

        return value.ToString() ?? string.Empty;
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        _stateCoordinator?.Dispose();
    }

    #endregion
}
