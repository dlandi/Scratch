namespace QuickGridTest01.LogDetail.Core;

/// <summary>
/// Manages which rows are currently expanded in the log detail view.
/// Uses a simple HashSet for tracking expanded row keys since logs are read-only.
/// Allows multiple simultaneous expansions by default.
/// </summary>
/// <typeparam name="TKey">The type of key used to identify rows (typically string or int)</typeparam>
public class LogExpansionStateManager<TKey> : IDisposable where TKey : notnull
{
    private readonly HashSet<TKey> _expandedKeys = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _disposed;

    /// <summary>
    /// Gets whether any row is currently expanded.
    /// </summary>
    public bool HasExpandedRows => _expandedKeys.Count > 0;

    /// <summary>
    /// Gets the count of expanded rows.
    /// </summary>
    public int ExpandedRowCount => _expandedKeys.Count;

    /// <summary>
    /// Gets all currently expanded row keys.
    /// </summary>
    public IReadOnlyCollection<TKey> ExpandedKeys => _expandedKeys;

    /// <summary>
    /// Checks if a specific row is expanded.
    /// </summary>
    public bool IsExpanded(TKey key) => _expandedKeys.Contains(key);

    /// <summary>
    /// Expands a row by its key.
    /// </summary>
    /// <returns>True if the row was newly expanded, false if already expanded.</returns>
    public async Task<bool> ExpandAsync(TKey key, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            return _expandedKeys.Add(key);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Collapses a row by its key.
    /// </summary>
    /// <returns>True if the row was collapsed, false if it wasn't expanded.</returns>
    public async Task<bool> CollapseAsync(TKey key, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            return _expandedKeys.Remove(key);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Toggles the expansion state of a row.
    /// </summary>
    /// <returns>True if the row is now expanded, false if collapsed.</returns>
    public async Task<bool> ToggleAsync(TKey key, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_expandedKeys.Contains(key))
            {
                _expandedKeys.Remove(key);
                return false;
            }
            else
            {
                _expandedKeys.Add(key);
                return true;
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Collapses all expanded rows.
    /// </summary>
    public async Task CollapseAllAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _expandedKeys.Clear();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Expands all specified rows.
    /// </summary>
    public async Task ExpandAllAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            foreach (var key in keys)
            {
                _expandedKeys.Add(key);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _lock.Dispose();
        _expandedKeys.Clear();
        _disposed = true;
    }
}
