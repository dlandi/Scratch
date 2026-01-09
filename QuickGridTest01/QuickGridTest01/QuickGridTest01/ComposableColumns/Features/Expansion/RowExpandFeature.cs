using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Data;
using QuickGridTest01.ComposableColumns.Features.Expansion.Events;
using QuickGridTest01.ComposableColumns.Features.Expansion.State;

namespace QuickGridTest01.ComposableColumns.Features.Expansion;

public class RowExpandFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class, IRowIdentifiable, new()
{
    public int Priority => FeaturePriority.Expansion;

    public FeatureContext<TGridItem>? Context { get; private set; }

    public ExpandableGridDataSource<TGridItem>? DataSource { get; set; }

    public RenderFragment<RowExpandedContext<TGridItem>>? ExpandedTemplate { get; set; }

    public int ExpandedRowSpan { get; set; } = 3;

    public int RowHeight { get; set; } = 48;

    public int ExpandedHeight => ExpandedRowSpan * RowHeight;

    public RowTriggerMode TriggerMode { get; set; } = RowTriggerMode.Button;

    public ConcurrentExpandBehavior ConcurrentBehavior { get; set; } = ConcurrentExpandBehavior.CollapseCurrent;

    public bool DimInactiveRows { get; set; } = true;

    public string ExpandButtonText { get; set; } = "Expand";

    public string ExpandButtonIcon { get; set; } = "";

    public string ExpandButtonClass { get; set; } = "qg-btn qg-btn-primary qg-btn-sm";

    public EventCallback<RowBeforeExpandEventArgs<TGridItem>> OnBeforeExpand { get; set; }

    public EventCallback<RowExpandedEventArgs<TGridItem>> OnExpanded { get; set; }

    public EventCallback<RowCollapsedEventArgs<TGridItem>> OnCollapsed { get; set; }

    public EventCallback<RowStateChangedEventArgs<TGridItem>> OnStateChanged { get; set; }

    private bool _disposed;

    public void OnAttach(FeatureContext<TGridItem> context)
    {
        Context = context;

        if (context.InvokeAsync is null)
            throw new InvalidOperationException("RowExpandFeature requires FeatureContext.InvokeAsync to be set.");

        if (context.RequestRefreshAsync is null)
            throw new InvalidOperationException("RowExpandFeature requires FeatureContext.RequestRefreshAsync to be set.");

        if (context.GetService<RowStateManager<TGridItem>>() is not null)
            throw new InvalidOperationException($"{nameof(RowStateManager<TGridItem>)} is already registered for this FeatureContext.");
    }

    public void OnDetach(FeatureContext<TGridItem> context)
    {
        Dispose();
    }

    public void RenderCell(RenderTreeBuilder builder, ref int sequence, TGridItem item, FeatureContext<TGridItem> context, Action renderNext)
    {
        var state = EnsureService(context);

        if (ExpandedTemplate is null)
            throw new InvalidOperationException("RowExpandFeature requires ExpandedTemplate to be provided.");

        if (ExpandedRowSpan <= 0)
            throw new ArgumentOutOfRangeException(nameof(ExpandedRowSpan), "ExpandedRowSpan must be greater than 0.");

        if (RowHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(RowHeight), "RowHeight must be greater than 0.");

        // Skip content for spacer rows - render empty cell
        if (SpacerRowFactory.IsSpacer(item.Id))
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "row-cell row-spacer");
            builder.CloseElement();
            return;
        }

        var isExpanded = state.IsRowExpanded(item);
        var hasAnyExpanded = state.HasExpandedRows;

        // Wrapper div with state classes
        builder.OpenElement(sequence++, "div");
        builder.AddAttribute(sequence++, "class", BuildCellClass(isExpanded, hasAnyExpanded));

        if (isExpanded && state.TryGetContext(item, out var expandedContext))
        {
            // Render the expanded content
            RenderExpandedMode(builder, ref sequence, item, expandedContext!);
        }
        else
        {
            // Render display mode (trigger button)
            RenderDisplayMode(builder, ref sequence, item, hasAnyExpanded, state);
        }

        builder.CloseElement();
    }

    private void RenderDisplayMode(RenderTreeBuilder builder, ref int seq, TGridItem item, bool hasAnyExpanded, RowStateManager<TGridItem> state)
    {
        var canExpand = CanExpandRow(hasAnyExpanded);

        var displayContext = new RowDisplayContext<TGridItem>
        {
            Item = item,
            IsAnyRowExpanded = hasAnyExpanded,
            CanExpand = canExpand,
            ExpandAsync = () => ExpandRowAsync(item)
        };

        if (TriggerMode == RowTriggerMode.Button)
        {
            RenderDefaultExpandButton(builder, ref seq, displayContext);
        }
        else if (TriggerMode == RowTriggerMode.RowClick)
        {
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

    private bool CanExpandRow(bool hasAnyExpanded)
    {
        if (!hasAnyExpanded)
            return true;

        // When AllowMultiple or CollapseCurrent, other rows can still be expanded
        return ConcurrentBehavior != ConcurrentExpandBehavior.Block;
    }

    public Task ExpandRowAsync(TGridItem item, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(item);

        var context = Context ?? throw new InvalidOperationException("RowExpandFeature must be attached before use.");

        return context.InvokeAsync!(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (SpacerRowFactory.IsSpacer(item.Id))
                return;

            if (item.Id == 0)
                throw new ArgumentOutOfRangeException(nameof(item), "Row Id must be greater than 0.");

            if (ExpandedTemplate is null)
                throw new InvalidOperationException("RowExpandFeature requires ExpandedTemplate to be provided.");

            if (ExpandedRowSpan <= 0)
                throw new ArgumentOutOfRangeException(nameof(ExpandedRowSpan), "ExpandedRowSpan must be greater than 0.");

            if (RowHeight <= 0)
                throw new ArgumentOutOfRangeException(nameof(RowHeight), "RowHeight must be greater than 0.");

            var state = EnsureService(context);

            if (state.IsRowExpanded(item))
                return;

            if (ConcurrentBehavior == ConcurrentExpandBehavior.Block)
            {
                if (state.HasExpandedRows)
                    return;
            }
            else if (ConcurrentBehavior == ConcurrentExpandBehavior.CollapseCurrent)
            {
                var current = state.GetFirstExpandedRow();
                if (current is not null && !ReferenceEquals(current, item))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await CollapseRowInternalAsync(context, current, cancellationToken);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (OnBeforeExpand.HasDelegate)
            {
                var args = new RowBeforeExpandEventArgs<TGridItem> { Item = item };
                await OnBeforeExpand.InvokeAsync(args);
                if (args.Cancel)
                    return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            var expandedContext = await state.GetOrCreateContextAsync(
                item,
                collapseAsync: () => CollapseRowAsync(item, CancellationToken.None),
                cancellationToken: cancellationToken);

            DataSource?.ExpandRow(item.Id, ExpandedRowSpan);

            cancellationToken.ThrowIfCancellationRequested();
            if (OnStateChanged.HasDelegate)
            {
                await OnStateChanged.InvokeAsync(new RowStateChangedEventArgs<TGridItem>
                {
                    Item = item,
                    OldState = RowExpandedState.Collapsed,
                    NewState = RowExpandedState.Expanded
                });
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (OnExpanded.HasDelegate)
            {
                await OnExpanded.InvokeAsync(new RowExpandedEventArgs<TGridItem> { Item = item });
            }

            cancellationToken.ThrowIfCancellationRequested();
            await context.RequestRefreshAsync!();
        });
    }

    public Task CollapseRowAsync(TGridItem item, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(item);

        var context = Context ?? throw new InvalidOperationException("RowExpandFeature must be attached before use.");

        return context.InvokeAsync!(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (SpacerRowFactory.IsSpacer(item.Id))
                return;

            if (item.Id == 0)
                throw new ArgumentOutOfRangeException(nameof(item), "Row Id must be greater than 0.");

            await CollapseRowInternalAsync(context, item, cancellationToken);
        });
    }

    public Task CollapseAllAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        var context = Context ?? throw new InvalidOperationException("RowExpandFeature must be attached before use.");

        return context.InvokeAsync!(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await CollapseAllInternalAsync(context, cancellationToken);
        });
    }

    private async Task CollapseRowInternalAsync(FeatureContext<TGridItem> context, TGridItem item, CancellationToken cancellationToken)
    {
        var state = EnsureService(context);

        if (!state.IsRowExpanded(item))
            return;

        cancellationToken.ThrowIfCancellationRequested();

        await state.RemoveRowAsync(item, cancellationToken);

        DataSource?.CollapseRow(item.Id);

        cancellationToken.ThrowIfCancellationRequested();
        if (OnStateChanged.HasDelegate)
        {
            await OnStateChanged.InvokeAsync(new RowStateChangedEventArgs<TGridItem>
            {
                Item = item,
                OldState = RowExpandedState.Expanded,
                NewState = RowExpandedState.Collapsed
            });
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (OnCollapsed.HasDelegate)
        {
            await OnCollapsed.InvokeAsync(new RowCollapsedEventArgs<TGridItem> { Item = item });
        }

        cancellationToken.ThrowIfCancellationRequested();
        await context.RequestRefreshAsync!();
    }

    private async Task CollapseAllInternalAsync(FeatureContext<TGridItem> context, CancellationToken cancellationToken)
    {
        var state = EnsureService(context);

        if (!state.HasExpandedRows)
            return;

        cancellationToken.ThrowIfCancellationRequested();

        await state.ClearAllAsync(cancellationToken);

        DataSource?.CollapseAll();

        cancellationToken.ThrowIfCancellationRequested();
        await context.RequestRefreshAsync!();
    }

    private RowStateManager<TGridItem> EnsureService(FeatureContext<TGridItem> context)
    {
        var svc = context.GetService<RowStateManager<TGridItem>>();
        if (svc is not null)
            return svc;

        var created = new RowStateManager<TGridItem>();
        context.RegisterService(created);
        return created;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
    }
}
