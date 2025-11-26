using System.Runtime.CompilerServices;

namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Manages which rows are in form mode and their associated contexts.
/// Uses ConditionalWeakTable for memory-efficient storage.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
public class FormRowStateManager<TGridItem> : IDisposable where TGridItem : class
{
    private readonly ConditionalWeakTable<TGridItem, FormRowContext<TGridItem>> _contexts = new();
    private readonly HashSet<TGridItem> _activeRows = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _disposed;

    /// <summary>
    /// Gets whether any row is currently in form mode.
    /// </summary>
    public bool HasActiveRows => _activeRows.Count > 0;

    /// <summary>
    /// Gets the count of active rows.
    /// </summary>
    public int ActiveRowCount => _activeRows.Count;

    /// <summary>
    /// Gets all currently active rows.
    /// </summary>
    public IReadOnlyCollection<TGridItem> ActiveRows => _activeRows;

    /// <summary>
    /// Checks if a specific row is in form mode.
    /// </summary>
    public bool IsRowActive(TGridItem item) => _activeRows.Contains(item);

    /// <summary>
    /// Gets or creates a context for the specified row.
    /// </summary>
    public async Task<FormRowContext<TGridItem>> GetOrCreateContextAsync(
        TGridItem item,
        Func<Task> saveAsync,
        Func<Task> cancelAsync,
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

            var context = new FormRowContext<TGridItem>
            {
                Item = item,
                State = FormRowState.Editing,
                SaveAsync = saveAsync,
                CancelAsync = cancelAsync
            };

            _contexts.Add(item, context);
            _activeRows.Add(item);

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
    public bool TryGetContext(TGridItem item, out FormRowContext<TGridItem>? context)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _contexts.TryGetValue(item, out context);
    }

    /// <summary>
    /// Removes a row from form mode.
    /// </summary>
    public async Task<bool> RemoveRowAsync(TGridItem item, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _contexts.Remove(item);
            return _activeRows.Remove(item);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Clears all active rows.
    /// </summary>
    public async Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            foreach (var item in _activeRows.ToList())
            {
                _contexts.Remove(item);
            }
            _activeRows.Clear();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Gets the first active row (if any).
    /// </summary>
    public TGridItem? GetFirstActiveRow()
    {
        return _activeRows.FirstOrDefault();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _lock.Dispose();
        _activeRows.Clear();
        _disposed = true;
    }
}
