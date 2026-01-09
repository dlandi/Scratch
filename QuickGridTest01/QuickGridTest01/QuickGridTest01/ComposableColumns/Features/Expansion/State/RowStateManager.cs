using System.Runtime.CompilerServices;
using QuickGridTest01.ComposableColumns.Features.Expansion;

namespace QuickGridTest01.ComposableColumns.Features.Expansion.State;

/// <summary>
/// Manages which rows are expanded and their associated contexts.
/// Uses ConditionalWeakTable for memory-efficient storage.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
public class RowStateManager<TGridItem> : IDisposable where TGridItem : class
{
    private readonly ConditionalWeakTable<TGridItem, RowExpandedContext<TGridItem>> _contexts = new();
    private readonly HashSet<TGridItem> _expandedRows = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _disposed;

    public bool HasExpandedRows => _expandedRows.Count > 0;

    public int ExpandedRowCount => _expandedRows.Count;

    public IReadOnlyCollection<TGridItem> ExpandedRows => _expandedRows;

    public bool IsRowExpanded(TGridItem item) => _expandedRows.Contains(item);

    public async Task<RowExpandedContext<TGridItem>> GetOrCreateContextAsync(
        TGridItem item,
        Func<Task> collapseAsync,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_contexts.TryGetValue(item, out var existing))
            {
                return existing;
            }

            var context = new RowExpandedContext<TGridItem>
            {
                Item = item,
                CollapseAsync = collapseAsync
            };

            _contexts.Add(item, context);
            _expandedRows.Add(item);

            return context;
        }
        finally
        {
            _lock.Release();
        }
    }

    public bool TryGetContext(TGridItem item, out RowExpandedContext<TGridItem>? context)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _contexts.TryGetValue(item, out context);
    }

    public async Task<bool> RemoveRowAsync(TGridItem item, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _contexts.Remove(item);
            return _expandedRows.Remove(item);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            foreach (var item in _expandedRows.ToList())
            {
                _contexts.Remove(item);
            }

            _expandedRows.Clear();
        }
        finally
        {
            _lock.Release();
        }
    }

    public TGridItem? GetFirstExpandedRow()
    {
        return _expandedRows.FirstOrDefault();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _lock.Dispose();
        _expandedRows.Clear();
        _disposed = true;
    }
}
