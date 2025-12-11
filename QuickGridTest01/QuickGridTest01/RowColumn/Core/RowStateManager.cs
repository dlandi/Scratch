using System.Runtime.CompilerServices;

namespace QuickGridTest01.RowColumn.Core;

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

    /// <summary>
    /// Gets whether any row is currently expanded.
    /// </summary>
    public bool HasExpandedRows => _expandedRows.Count > 0;

    /// <summary>
    /// Gets the count of expanded rows.
    /// </summary>
    public int ExpandedRowCount => _expandedRows.Count;

    /// <summary>
    /// Gets all currently expanded rows.
    /// </summary>
    public IReadOnlyCollection<TGridItem> ExpandedRows => _expandedRows;

    /// <summary>
    /// Checks if a specific row is expanded.
    /// </summary>
    public bool IsRowExpanded(TGridItem item) => _expandedRows.Contains(item);

    /// <summary>
    /// Gets or creates a context for the specified row.
    /// </summary>
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

    /// <summary>
    /// Tries to get an existing context for a row.
    /// </summary>
    public bool TryGetContext(TGridItem item, out RowExpandedContext<TGridItem>? context)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _contexts.TryGetValue(item, out context);
    }

    /// <summary>
    /// Removes a row from expanded state.
    /// </summary>
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

    /// <summary>
    /// Clears all expanded rows.
    /// </summary>
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

    /// <summary>
    /// Gets the first expanded row (if any).
    /// </summary>
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
