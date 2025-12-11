using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.RowColumn.Core;
using QuickGridTest01.RowColumn.Events;

namespace QuickGridTest01.RowColumn;

/// <summary>
/// A QuickGrid column that renders an expandable overlay when a row is activated.
/// Supports button, row-click, or custom trigger modes with configurable concurrent expand behavior.
/// </summary>
/// <typeparam name="TGridItem">The type of data item in the grid</typeparam>
public class RowColumn<TGridItem> : ColumnBase<TGridItem>, IDisposable
    where TGridItem : class
{
    private readonly RowStateManager<TGridItem> _stateManager = new();
    private GridSort<TGridItem>? _sortBuilder;
    private bool _disposed;

    #region Parameters - Trigger & Behavior

    /// <summary>
    /// How expanded mode is triggered. Default: Button.
    /// </summary>
    [Parameter]
    public RowTriggerMode TriggerMode { get; set; } = RowTriggerMode.Button;

    /// <summary>
    /// Behavior when expanding a new row while another is open. Default: Block.
    /// </summary>
    [Parameter]
    public ConcurrentExpandBehavior ConcurrentBehavior { get; set; } = ConcurrentExpandBehavior.Block;

    /// <summary>
    /// When true, non-expanded rows are visually dimmed. Default: true.
    /// </summary>
    [Parameter]
    public bool DimInactiveRows { get; set; } = true;

    #endregion

    #region Parameters - Row Span & Height

    /// <summary>
    /// Number of row heights the expanded overlay should span. Default: 3.
    /// Important for virtualization compatibility where all rows must have uniform height.
    /// </summary>
    [Parameter]
    public int ExpandedRowSpan { get; set; } = 3;

    /// <summary>
    /// Height of each row in pixels. Default: 48.
    /// Used to calculate total overlay height: ExpandedRowSpan × RowHeight.
    /// </summary>
    [Parameter]
    public int RowHeight { get; set; } = 48;

    /// <summary>
    /// Gets the calculated height of the expanded overlay in pixels.
    /// </summary>
    public int ExpandedHeight => ExpandedRowSpan * RowHeight;

    #endregion

    #region Parameters - Templates

    /// <summary>
    /// Content shown when row is NOT expanded.
    /// If null and TriggerMode is Button, renders default Edit button.
    /// If null and TriggerMode is RowClick, renders nothing.
    /// </summary>
    [Parameter]
    public RenderFragment<RowDisplayContext<TGridItem>>? DisplayTemplate { get; set; }

    /// <summary>
    /// The content rendered when row IS expanded. Required.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<RowExpandedContext<TGridItem>>? ExpandedTemplate { get; set; }

    #endregion

    #region Parameters - Button Customization

    /// <summary>
    /// Text for the expand button. Default: "Edit".
    /// </summary>
    [Parameter]
    public string ExpandButtonText { get; set; } = "Edit";

    /// <summary>
    /// CSS class for the expand button. Default: "qg-btn qg-btn-secondary qg-btn-sm".
    /// </summary>
    [Parameter]
    public string ExpandButtonClass { get; set; } = "qg-btn qg-btn-secondary qg-btn-sm";

    /// <summary>
    /// Icon class for the expand button. Default: "bi bi-pencil".
    /// </summary>
    [Parameter]
    public string? ExpandButtonIcon { get; set; } = "bi bi-pencil";

    #endregion

    #region Parameters - Events

    /// <summary>
    /// Called before expanding a row. Set Cancel = true to prevent.
    /// </summary>
    [Parameter]
    public EventCallback<RowBeforeExpandEventArgs<TGridItem>> OnBeforeExpand { get; set; }

    /// <summary>
    /// Called after a row is expanded.
    /// </summary>
    [Parameter]
    public EventCallback<RowExpandedEventArgs<TGridItem>> OnExpanded { get; set; }

    /// <summary>
    /// Called after a row is collapsed.
    /// </summary>
    [Parameter]
    public EventCallback<RowCollapsedEventArgs<TGridItem>> OnCollapsed { get; set; }

    /// <summary>
    /// Called when row state changes (expand/collapse).
    /// </summary>
    [Parameter]
    public EventCallback<RowStateChangedEventArgs<TGridItem>> OnStateChanged { get; set; }

    #endregion

    #region ColumnBase Implementation

    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBuilder;
        set => _sortBuilder = value;
    }

    protected override void OnParametersSet()
    {
        if (ExpandedTemplate == null)
            throw new InvalidOperationException($"{nameof(RowColumn<TGridItem>)} requires an {nameof(ExpandedTemplate)} parameter.");

        if (string.IsNullOrEmpty(Title))
            Title = "Actions";

        base.OnParametersSet();
    }

    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var isExpanded = _stateManager.IsRowExpanded(item);
        var hasAnyExpanded = _stateManager.HasExpandedRows;

        int seq = 0;

        // Wrapper div with state classes
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", BuildCellClass(isExpanded, hasAnyExpanded));

        if (isExpanded && _stateManager.TryGetContext(item, out var context))
        {
            // Render the expanded content
            RenderExpandedMode(builder, ref seq, item, context!);
        }
        else
        {
            // Render display mode
            RenderDisplayMode(builder, ref seq, item, hasAnyExpanded);
        }

        builder.CloseElement();
    }

    #endregion

    #region Rendering

    private void RenderDisplayMode(RenderTreeBuilder builder, ref int seq, TGridItem item, bool hasAnyExpanded)
    {
        var canExpand = CanExpandRow(hasAnyExpanded);

        var displayContext = new RowDisplayContext<TGridItem>
        {
            Item = item,
            IsAnyRowExpanded = hasAnyExpanded,
            CanExpand = canExpand,
            ExpandAsync = () => ExpandRowAsync(item)
        };

        if (DisplayTemplate != null)
        {
            builder.AddContent(seq++, DisplayTemplate(displayContext));
        }
        else if (TriggerMode == RowTriggerMode.Button)
        {
            RenderDefaultExpandButton(builder, ref seq, displayContext);
        }
        else if (TriggerMode == RowTriggerMode.RowClick)
        {
            // For row click, we render a subtle indicator
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "row-click-indicator");
            builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await ExpandRowAsync(item)));
            builder.AddAttribute(seq++, "title", canExpand ? "Click to expand" : "Another row is expanded");
            builder.OpenElement(seq++, "i");
            builder.AddAttribute(seq++, "class", "bi bi-chevron-expand");
            builder.CloseElement();
            builder.CloseElement();
        }
    }

    private void RenderDefaultExpandButton(RenderTreeBuilder builder, ref int seq, RowDisplayContext<TGridItem> context)
    {
        builder.OpenElement(seq++, "button");
        builder.AddAttribute(seq++, "type", "button");
        builder.AddAttribute(seq++, "class", ExpandButtonClass);
        builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await context.ExpandAsync()));
        builder.AddAttribute(seq++, "disabled", !context.CanExpand);
        builder.AddAttribute(seq++, "title", context.CanExpand ? "Expand this row" : "Another row is expanded");

        if (!string.IsNullOrEmpty(ExpandButtonIcon))
        {
            builder.OpenElement(seq++, "i");
            builder.AddAttribute(seq++, "class", ExpandButtonIcon);
            builder.CloseElement();
        }

        if (!string.IsNullOrEmpty(ExpandButtonText))
        {
            builder.OpenElement(seq++, "span");
            builder.AddContent(seq++, ExpandButtonText);
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    private void RenderExpandedMode(RenderTreeBuilder builder, ref int seq, TGridItem item, RowExpandedContext<TGridItem> context)
    {
        // Overlay container positioned below the row with calculated height
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "row-overlay");
        builder.AddAttribute(seq++, "style", $"height: {ExpandedHeight}px;");

        // Provide cascading context for child components
        builder.OpenComponent<CascadingValue<RowExpandedContext<TGridItem>>>(seq++);
        builder.AddComponentParameter(seq++, "Value", context);
        builder.AddComponentParameter(seq++, "ChildContent", ExpandedTemplate!(context));
        builder.CloseComponent();

        builder.CloseElement();
    }

    private string BuildCellClass(bool isExpanded, bool hasAnyExpanded)
    {
        var classes = new List<string> { "row-cell" };

        if (isExpanded)
        {
            classes.Add("row-expanded");
        }
        else if (hasAnyExpanded && DimInactiveRows)
        {
            classes.Add("row-dimmed");
        }

        return string.Join(" ", classes);
    }

    #endregion

    #region Expand/Collapse Management

    private bool CanExpandRow(bool hasAnyExpanded)
    {
        if (!hasAnyExpanded)
            return true;

        return ConcurrentBehavior switch
        {
            ConcurrentExpandBehavior.Block => false,
            ConcurrentExpandBehavior.AllowMultiple => true,
            ConcurrentExpandBehavior.CollapseCurrent => true,
            _ => false
        };
    }

    private async Task ExpandRowAsync(TGridItem item)
    {
        // Fire before-expand event
        var beforeExpandArgs = new RowBeforeExpandEventArgs<TGridItem> { Item = item };
        await OnBeforeExpand.InvokeAsync(beforeExpandArgs);

        if (beforeExpandArgs.Cancel)
            return;

        // Handle existing expanded rows based on behavior
        if (_stateManager.HasExpandedRows && ConcurrentBehavior != ConcurrentExpandBehavior.AllowMultiple)
        {
            var expandedRow = _stateManager.GetFirstExpandedRow();
            if (expandedRow != null && !ReferenceEquals(expandedRow, item))
            {
                switch (ConcurrentBehavior)
                {
                    case ConcurrentExpandBehavior.Block:
                        return;

                    case ConcurrentExpandBehavior.CollapseCurrent:
                        await CollapseRowAsync(expandedRow);
                        break;
                }
            }
        }

        // Create context for the new row
        var context = await _stateManager.GetOrCreateContextAsync(
            item,
            collapseAsync: () => CollapseRowAsync(item)
        );

        // Fire state changed event
        await OnStateChanged.InvokeAsync(new RowStateChangedEventArgs<TGridItem>
        {
            Item = item,
            OldState = RowExpandedState.Collapsed,
            NewState = RowExpandedState.Expanded
        });

        // Fire expanded event
        await OnExpanded.InvokeAsync(new RowExpandedEventArgs<TGridItem> { Item = item });

        await InvokeAsync(StateHasChanged);
    }

    private async Task CollapseRowAsync(TGridItem item)
    {
        if (!_stateManager.IsRowExpanded(item))
            return;

        // Remove from state manager
        await _stateManager.RemoveRowAsync(item);

        // Fire state changed event
        await OnStateChanged.InvokeAsync(new RowStateChangedEventArgs<TGridItem>
        {
            Item = item,
            OldState = RowExpandedState.Expanded,
            NewState = RowExpandedState.Collapsed
        });

        // Fire collapsed event
        await OnCollapsed.InvokeAsync(new RowCollapsedEventArgs<TGridItem> { Item = item });

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
