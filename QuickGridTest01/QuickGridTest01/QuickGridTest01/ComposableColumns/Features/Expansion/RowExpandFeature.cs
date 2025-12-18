using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
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

    public RowTriggerMode TriggerMode { get; set; } = RowTriggerMode.Button;

    public ConcurrentExpandBehavior ConcurrentBehavior { get; set; } = ConcurrentExpandBehavior.CollapseCurrent;

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
        _ = EnsureService(context);

        if (ExpandedTemplate is null)
            throw new InvalidOperationException("RowExpandFeature requires ExpandedTemplate to be provided.");

        if (ExpandedRowSpan <= 0)
            throw new ArgumentOutOfRangeException(nameof(ExpandedRowSpan), "ExpandedRowSpan must be greater than 0.");

        if (RowHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(RowHeight), "RowHeight must be greater than 0.");

        renderNext();
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
