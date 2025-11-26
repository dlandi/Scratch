using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.FormRow.Core;
using QuickGridTest01.FormRow.Events;
using QuickGridTest01.FormRow.Validation;

namespace QuickGridTest01.FormRow;

/// <summary>
/// A QuickGrid column that renders an inline form when a row is in edit mode.
/// Supports button, row-click, or custom trigger modes with configurable concurrent edit behavior.
/// </summary>
/// <typeparam name="TGridItem">The type of data item in the grid</typeparam>
public class FormRowColumn<TGridItem> : ColumnBase<TGridItem>, IDisposable
    where TGridItem : class
{
    private readonly FormRowStateManager<TGridItem> _stateManager = new();
    private readonly FormFieldValidator<TGridItem> _validator;
    private GridSort<TGridItem>? _sortBuilder;
    private bool _disposed;

    public FormRowColumn()
    {
        _validator = new FormFieldValidator<TGridItem>(true);
    }

    #region Parameters - Trigger & Behavior

    /// <summary>
    /// How form mode is triggered. Default: Button.
    /// </summary>
    [Parameter]
    public FormTriggerMode TriggerMode { get; set; } = FormTriggerMode.Button;

    /// <summary>
    /// Behavior when editing a new row while another is open. Default: Block.
    /// </summary>
    [Parameter]
    public ConcurrentEditBehavior ConcurrentEditBehavior { get; set; } = ConcurrentEditBehavior.Block;

    /// <summary>
    /// When true, non-active rows are visually dimmed. Default: true.
    /// </summary>
    [Parameter]
    public bool DimInactiveRows { get; set; } = true;

    #endregion

    #region Parameters - Templates

    /// <summary>
    /// Content shown when row is NOT in form mode.
    /// If null and TriggerMode is Button, renders default Edit button.
    /// If null and TriggerMode is RowClick, renders nothing.
    /// </summary>
    [Parameter]
    public RenderFragment<FormRowDisplayContext<TGridItem>>? DisplayTemplate { get; set; }

    /// <summary>
    /// The form layout rendered when row IS in form mode. Required.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment? FormTemplate { get; set; }

    #endregion

    #region Parameters - Button Customization

    /// <summary>
    /// Text for the edit button. Default: "Edit".
    /// </summary>
    [Parameter]
    public string EditButtonText { get; set; } = "Edit";

    /// <summary>
    /// CSS class for the edit button. Default: "qg-btn qg-btn-secondary qg-btn-sm".
    /// </summary>
    [Parameter]
    public string EditButtonClass { get; set; } = "qg-btn qg-btn-secondary qg-btn-sm";

    /// <summary>
    /// Icon class for the edit button. Default: "bi bi-pencil".
    /// </summary>
    [Parameter]
    public string? EditButtonIcon { get; set; } = "bi bi-pencil";

    #endregion

    #region Parameters - Events

    /// <summary>
    /// Called when save is requested. Return (true, null) for success.
    /// </summary>
    [Parameter]
    public Func<TGridItem, Task<(bool Success, string? Error)>>? OnSaveAsync { get; set; }

    /// <summary>
    /// Called after successful save.
    /// </summary>
    [Parameter]
    public EventCallback<FormSaveEventArgs<TGridItem>> OnSaved { get; set; }

    /// <summary>
    /// Called when edit is cancelled.
    /// </summary>
    [Parameter]
    public EventCallback<FormCancelEventArgs<TGridItem>> OnCancelled { get; set; }

    /// <summary>
    /// Called before entering form mode. Set Cancel = true to prevent.
    /// </summary>
    [Parameter]
    public EventCallback<FormBeforeEditEventArgs<TGridItem>> OnBeforeEdit { get; set; }

    /// <summary>
    /// Called when form mode state changes.
    /// </summary>
    [Parameter]
    public EventCallback<FormStateChangedEventArgs<TGridItem>> OnFormStateChanged { get; set; }

    #endregion

    #region Parameters - Validation

    /// <summary>
    /// Enable DataAnnotations validation. Default: true.
    /// </summary>
    [Parameter]
    public bool UseDataAnnotations { get; set; } = true;

    /// <summary>
    /// Validate fields on every input change. Default: true.
    /// </summary>
    [Parameter]
    public bool ValidateOnChange { get; set; } = true;

    #endregion

    #region ColumnBase Implementation

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnParametersSet()
    {
        if (FormTemplate == null)
            throw new InvalidOperationException($"{nameof(FormRowColumn<TGridItem>)} requires a {nameof(FormTemplate)} parameter.");

        if (string.IsNullOrEmpty(Title))
            Title = "Actions";

        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var isActive = _stateManager.IsRowActive(item);
        var hasAnyActive = _stateManager.HasActiveRows;

        int seq = 0;

        // Wrapper div with state classes
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", BuildCellClass(isActive, hasAnyActive));

        if (isActive && _stateManager.TryGetContext(item, out var context))
        {
            // Render the form
            RenderFormMode(builder, ref seq, item, context!);
        }
        else
        {
            // Render display mode
            RenderDisplayMode(builder, ref seq, item, hasAnyActive);
        }

        builder.CloseElement();
    }

    #endregion

    #region Rendering

    private void RenderDisplayMode(RenderTreeBuilder builder, ref int seq, TGridItem item, bool hasAnyActive)
    {
        var canEnter = CanEnterFormMode(hasAnyActive);

        var displayContext = new FormRowDisplayContext<TGridItem>
        {
            Item = item,
            IsAnyRowInFormMode = hasAnyActive,
            CanEnterFormMode = canEnter,
            EnterFormModeAsync = () => EnterFormModeAsync(item)
        };

        if (DisplayTemplate != null)
        {
            builder.AddContent(seq++, DisplayTemplate(displayContext));
        }
        else if (TriggerMode == FormTriggerMode.Button)
        {
            RenderDefaultEditButton(builder, ref seq, displayContext);
        }
        else if (TriggerMode == FormTriggerMode.RowClick)
        {
            // For row click, we render a subtle indicator or nothing
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "form-row-click-indicator");
            builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await EnterFormModeAsync(item)));
            builder.AddAttribute(seq++, "title", canEnter ? "Click to edit" : "Another row is being edited");
            builder.OpenElement(seq++, "i");
            builder.AddAttribute(seq++, "class", "bi bi-pencil-square");
            builder.CloseElement();
            builder.CloseElement();
        }
    }

    private void RenderDefaultEditButton(RenderTreeBuilder builder, ref int seq, FormRowDisplayContext<TGridItem> context)
    {
        builder.OpenElement(seq++, "button");
        builder.AddAttribute(seq++, "type", "button");
        builder.AddAttribute(seq++, "class", EditButtonClass);
        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await context.EnterFormModeAsync()));
        builder.AddAttribute(seq++, "disabled", !context.CanEnterFormMode);
        builder.AddAttribute(seq++, "title", context.CanEnterFormMode ? "Edit this row" : "Another row is being edited");

        if (!string.IsNullOrEmpty(EditButtonIcon))
        {
            builder.OpenElement(seq++, "i");
            builder.AddAttribute(seq++, "class", EditButtonIcon);
            builder.CloseElement();
        }

        if (!string.IsNullOrEmpty(EditButtonText))
        {
            builder.OpenElement(seq++, "span");
            builder.AddContent(seq++, EditButtonText);
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    private void RenderFormMode(RenderTreeBuilder builder, ref int seq, TGridItem item, FormRowContext<TGridItem> context)
    {
        // Form card overlay container
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "form-row-overlay");

        // Provide cascading context for FormField, FormActions, etc.
        builder.OpenComponent<CascadingValue<FormRowContext<TGridItem>>>(seq++);
        builder.AddComponentParameter(seq++, "Value", context);
        builder.AddComponentParameter(seq++, "ChildContent", FormTemplate);
        builder.CloseComponent();

        builder.CloseElement();
    }

    private string BuildCellClass(bool isActive, bool hasAnyActive)
    {
        var classes = new List<string> { "form-row-cell" };

        if (isActive)
        {
            classes.Add("form-active");
        }
        else if (hasAnyActive && DimInactiveRows)
        {
            classes.Add("form-dimmed");
        }

        return string.Join(" ", classes);
    }

    #endregion

    #region Form Mode Management

    private bool CanEnterFormMode(bool hasAnyActive)
    {
        if (!hasAnyActive)
            return true;

        return ConcurrentEditBehavior switch
        {
            ConcurrentEditBehavior.Block => false,
            ConcurrentEditBehavior.AllowMultiple => true,
            ConcurrentEditBehavior.CancelCurrent => true,
            ConcurrentEditBehavior.SaveCurrent => true,
            _ => false
        };
    }

    private async Task EnterFormModeAsync(TGridItem item)
    {
        // Fire before-edit event
        var beforeEditArgs = new FormBeforeEditEventArgs<TGridItem> { Item = item };
        await OnBeforeEdit.InvokeAsync(beforeEditArgs);

        if (beforeEditArgs.Cancel)
            return;

        // Handle existing active rows based on behavior
        if (_stateManager.HasActiveRows && ConcurrentEditBehavior != ConcurrentEditBehavior.AllowMultiple)
        {
            var activeRow = _stateManager.GetFirstActiveRow();
            if (activeRow != null && !ReferenceEquals(activeRow, item))
            {
                switch (ConcurrentEditBehavior)
                {
                    case ConcurrentEditBehavior.Block:
                        return;

                    case ConcurrentEditBehavior.CancelCurrent:
                        await CancelEditAsync(activeRow);
                        break;

                    case ConcurrentEditBehavior.SaveCurrent:
                        if (_stateManager.TryGetContext(activeRow, out var existingContext))
                        {
                            await SaveEditAsync(activeRow, existingContext!);
                        }
                        break;
                }
            }
        }

        // Create context for the new row
        var context = await _stateManager.GetOrCreateContextAsync(
            item,
            saveAsync: () => SaveEditAsync(item),
            cancelAsync: () => CancelEditAsync(item)
        );

        // Wire up validation callback
        context.ValidateFieldCallback = async (propertyName, value) =>
            await _validator.ValidateFieldAsync(propertyName, value, item);

        // Fire state changed event
        await OnFormStateChanged.InvokeAsync(new FormStateChangedEventArgs<TGridItem>
        {
            Item = item,
            OldState = FormRowState.Reading,
            NewState = FormRowState.Editing
        });

        await InvokeAsync(StateHasChanged);
    }

    private async Task SaveEditAsync(TGridItem item)
    {
        if (!_stateManager.TryGetContext(item, out var context) || context == null)
            return;

        // Validate all fields
        await context.ValidateAsync();

        if (context.HasErrors())
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        // Transition to saving state
        var oldState = context.State;
        context.State = FormRowState.Saving;
        context.SaveError = null;

        await OnFormStateChanged.InvokeAsync(new FormStateChangedEventArgs<TGridItem>
        {
            Item = item,
            OldState = oldState,
            NewState = FormRowState.Saving
        });

        await InvokeAsync(StateHasChanged);

        bool success;
        string? error;

        if (OnSaveAsync != null)
        {
            (success, error) = await OnSaveAsync(item);
        }
        else
        {
            // Default: commit draft values directly to item
            context.CommitDraftToItem();
            success = true;
            error = null;
        }

        if (success)
        {
            var savedValues = context.GetAllDraftValues();

            // Fire saved event
            await OnSaved.InvokeAsync(new FormSaveEventArgs<TGridItem>
            {
                Item = item,
                SavedValues = savedValues
            });

            // Exit form mode
            await _stateManager.RemoveRowAsync(item);

            await OnFormStateChanged.InvokeAsync(new FormStateChangedEventArgs<TGridItem>
            {
                Item = item,
                OldState = FormRowState.Saving,
                NewState = FormRowState.Reading
            });
        }
        else
        {
            // Stay in editing state with error
            context.State = FormRowState.Editing;
            context.SaveError = error ?? "An error occurred while saving.";
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task SaveEditAsync(TGridItem item, FormRowContext<TGridItem> context)
    {
        await SaveEditAsync(item);
    }

    private async Task CancelEditAsync(TGridItem item)
    {
        if (!_stateManager.TryGetContext(item, out var context))
            return;

        var wasDirty = context?.IsDirty() ?? false;

        // Exit form mode
        await _stateManager.RemoveRowAsync(item);

        // Fire cancelled event
        await OnCancelled.InvokeAsync(new FormCancelEventArgs<TGridItem>
        {
            Item = item,
            WasDirty = wasDirty
        });

        await OnFormStateChanged.InvokeAsync(new FormStateChangedEventArgs<TGridItem>
        {
            Item = item,
            OldState = FormRowState.Editing,
            NewState = FormRowState.Reading
        });

        await InvokeAsync(StateHasChanged);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        if (_disposed) return;
        _stateManager.Dispose();
        _disposed = true;
    }

    #endregion
}
