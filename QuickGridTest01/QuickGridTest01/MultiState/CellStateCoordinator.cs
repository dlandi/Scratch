using System.Runtime.CompilerServices;
using QuickGridTest01.MultiState.Core;

namespace QuickGridTest01.MultiState.Component;

/// <summary>
/// Manages cell states for grid items using ConditionalWeakTable for memory-efficient storage.
/// Thread-safe async state access with SemaphoreSlim.
/// </summary>
/// <typeparam name="TGridItem">The type of grid item</typeparam>
/// <typeparam name="TValue">The type of cell value</typeparam>
public class CellStateCoordinator<TGridItem, TValue> : IDisposable where TGridItem : class
{
    private readonly ConditionalWeakTable<TGridItem, MultiState<TValue>> _stateMap = new();
    private readonly SemaphoreSlim _stateLock = new(1, 1);
    private bool _disposed;

    /// <summary>
    /// Gets or creates a state for the specified grid item asynchronously.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="initialValue">The initial value if creating new state</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The state associated with the item</returns>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public async Task<MultiState<TValue>> GetOrCreateStateAsync(
        TGridItem item,
        TValue initialValue,
        CancellationToken cancellationToken = default)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        ObjectDisposedException.ThrowIf(_disposed, this);

        await _stateLock.WaitAsync(cancellationToken);
        try
        {
            if (_stateMap.TryGetValue(item, out var existingState))
            {
                return existingState;
            }

            var newState = new MultiState<TValue>
            {
                CurrentState = CellState.Reading,
                OriginalValue = initialValue,
                DraftValue = initialValue
            };

            _stateMap.Add(item, newState);
            return newState;
        }
        finally
        {
            _stateLock.Release();
        }
    }

    /// <summary>
    /// Tries to get an existing state for the specified grid item.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <param name="state">The state if found</param>
    /// <returns>True if state exists, false otherwise</returns>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public bool TryGetState(TGridItem item, out MultiState<TValue>? state)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        ObjectDisposedException.ThrowIf(_disposed, this);

        return _stateMap.TryGetValue(item, out state);
    }

    /// <summary>
    /// Removes the state for the specified grid item.
    /// </summary>
    /// <param name="item">The grid item</param>
    /// <returns>True if state was removed, false if it didn't exist</returns>
    /// <exception cref="ArgumentNullException">Thrown when item is null</exception>
    public bool RemoveState(TGridItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        ObjectDisposedException.ThrowIf(_disposed, this);

        return _stateMap.Remove(item);
    }

    /// <summary>
    /// Disposes the coordinator and releases resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _stateLock.Dispose();
        _disposed = true;
    }
}