using QuickGridTest01.RowColumn.Core;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupedGridDataSource<TGridItem>
    where TGridItem : class
{
    private readonly GroupingCoordinator<TGridItem> _coordinator;

    private IQueryable<TGridItem> _sourceItems = Array.Empty<TGridItem>().AsQueryable();
    private IQueryable<TGridItem>? _cached;
    private bool _dirty = true;

    public GroupedGridDataSource(GroupingCoordinator<TGridItem> coordinator)
    {
        _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
    }

    public event Func<Task>? OnDataChanged;

    public void SetSourceItems(IQueryable<TGridItem> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        // Always mark dirty to ensure transform runs on each render cycle.
        // The coordinator's transform may produce different results based on expansion state.
        _sourceItems = source;
        _dirty = true;
    }

    public IQueryable<TGridItem> Items
    {
        get
        {
            if (_dirty || _cached is null)
            {
                _cached = _coordinator.TransformItems(_sourceItems);
                _dirty = false;
            }

            return _cached;
        }
    }

    public async Task ToggleGroupAsync(object key)
    {
        if (_coordinator.ActiveGrouping is null)
            return;

        await _coordinator.ActiveGrouping.ToggleGroupAsync(key);
        MarkDirty();
    }

    private void MarkDirty()
    {
        _dirty = true;
        _ = OnDataChanged?.Invoke();
    }

    /// <summary>
    /// Marks the data source as dirty, causing the next access to Items to re-transform.
    /// Also invokes OnDataChanged to notify listeners (e.g., the grid) to refresh.
    /// </summary>
    public void NotifyStateChanged()
    {
        MarkDirty();
    }
}
