using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// A data source that tracks item order for row reordering.
/// Order is tracked by Item ID using fractional double indices to enable insertion without renumbering.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class ReorderableDataSource<TGridItem> : IDisposable
    where TGridItem : class, IRowIdentifiable
{
    private readonly List<TGridItem> _originalItems;
    private List<TGridItem> _orderedItems;
    private readonly Dictionary<int, double> _orderIndices; // ItemId → OrderIndex
    private bool _disposed;

    /// <summary>
    /// Creates a new ReorderableDataSource with the specified items.
    /// Items are initially ordered by their position in the enumerable.
    /// </summary>
    /// <param name="items">The items to track.</param>
    /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
    public ReorderableDataSource(IEnumerable<TGridItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _originalItems = items.ToList();
        _orderedItems = new List<TGridItem>(_originalItems);
        _orderIndices = new Dictionary<int, double>();

        // Initialize order indices with integer values
        for (var i = 0; i < _orderedItems.Count; i++)
        {
            _orderIndices[_orderedItems[i].Id] = i;
        }
    }

    /// <summary>
    /// Gets the items in their current order as an IQueryable for binding to QuickGrid.
    /// </summary>
    public IQueryable<TGridItem> Items => _orderedItems.AsQueryable();

    /// <summary>
    /// Gets the items in their current order as a read-only list.
    /// </summary>
    public IReadOnlyList<TGridItem> CurrentOrder => _orderedItems;

    /// <summary>
    /// Raised after the order has been changed by any mutation method.
    /// </summary>
    public event Action? OnOrderChanged;

    /// <summary>
    /// Moves an item from one index to another.
    /// </summary>
    /// <param name="fromIndex">The current index of the item.</param>
    /// <param name="toIndex">The target index for the item.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when fromIndex or toIndex is out of range.</exception>
    public void MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _orderedItems.Count)
            throw new ArgumentOutOfRangeException(nameof(fromIndex), fromIndex, "Index is out of range.");
        if (toIndex < 0 || toIndex >= _orderedItems.Count)
            throw new ArgumentOutOfRangeException(nameof(toIndex), toIndex, "Index is out of range.");

        if (fromIndex == toIndex)
            return;

        var item = _orderedItems[fromIndex];

        if (toIndex == 0)
        {
            // Move to beginning
            var firstIndex = _orderIndices[_orderedItems[0].Id];
            _orderIndices[item.Id] = firstIndex - 1.0;
        }
        else if (toIndex >= _orderedItems.Count - 1)
        {
            // Move to end
            var lastIndex = _orderIndices[_orderedItems[^1].Id];
            _orderIndices[item.Id] = lastIndex + 1.0;
        }
        else
        {
            // Move between two items
            var targetItem = _orderedItems[toIndex];
            var prevItem = _orderedItems[toIndex - 1];
            
            if (fromIndex < toIndex)
            {
                // Moving down - insert after the target position item
                var targetIndex = _orderIndices[targetItem.Id];
                var nextIndex = toIndex + 1 < _orderedItems.Count 
                    ? _orderIndices[_orderedItems[toIndex + 1].Id] 
                    : targetIndex + 1.0;
                _orderIndices[item.Id] = (targetIndex + nextIndex) / 2.0;
            }
            else
            {
                // Moving up - insert before the target position item
                var prevIndex = _orderIndices[prevItem.Id];
                var targetIndex = _orderIndices[targetItem.Id];
                _orderIndices[item.Id] = (prevIndex + targetIndex) / 2.0;
            }
        }

        RebuildOrderedList();
        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Moves an item to a specific index.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="toIndex">The target index for the item.</param>
    /// <exception cref="ArgumentException">Thrown when the item is not found in the data source.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when toIndex is out of range.</exception>
    public void MoveItem(TGridItem item, int toIndex)
    {
        ArgumentNullException.ThrowIfNull(item);

        var fromIndex = IndexOf(item);
        if (fromIndex < 0)
            throw new ArgumentException("Item not found in data source.", nameof(item));

        MoveItem(fromIndex, toIndex);
    }

    /// <summary>
    /// Moves an item to immediately before the target item.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="target">The target item to insert before.</param>
    /// <exception cref="ArgumentException">Thrown when item or target is not found in the data source.</exception>
    public void MoveItemBefore(TGridItem item, TGridItem target)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(target);

        if (!_orderIndices.ContainsKey(item.Id))
            throw new ArgumentException("Item not found in data source.", nameof(item));
        if (!_orderIndices.ContainsKey(target.Id))
            throw new ArgumentException("Target not found in data source.", nameof(target));

        if (item.Id == target.Id)
            return;

        var targetIndex = _orderIndices[target.Id];

        // Find the item immediately before target (if any)
        var itemBefore = _orderedItems
            .Where(x => _orderIndices[x.Id] < targetIndex && x.Id != item.Id)
            .OrderByDescending(x => _orderIndices[x.Id])
            .FirstOrDefault();

        if (itemBefore is null)
        {
            // Insert at beginning (before target)
            _orderIndices[item.Id] = targetIndex - 1.0;
        }
        else
        {
            // Insert between itemBefore and target
            var beforeIndex = _orderIndices[itemBefore.Id];
            _orderIndices[item.Id] = (beforeIndex + targetIndex) / 2.0;
        }

        RebuildOrderedList();
        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Moves an item to immediately after the target item.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="target">The target item to insert after.</param>
    /// <exception cref="ArgumentException">Thrown when item or target is not found in the data source.</exception>
    public void MoveItemAfter(TGridItem item, TGridItem target)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(target);

        if (!_orderIndices.ContainsKey(item.Id))
            throw new ArgumentException("Item not found in data source.", nameof(item));
        if (!_orderIndices.ContainsKey(target.Id))
            throw new ArgumentException("Target not found in data source.", nameof(target));

        if (item.Id == target.Id)
            return;

        var targetIndex = _orderIndices[target.Id];

        // Find the item immediately after target (if any)
        var itemAfter = _orderedItems
            .Where(x => _orderIndices[x.Id] > targetIndex && x.Id != item.Id)
            .OrderBy(x => _orderIndices[x.Id])
            .FirstOrDefault();

        if (itemAfter is null)
        {
            // Insert at end (after target)
            _orderIndices[item.Id] = targetIndex + 1.0;
        }
        else
        {
            // Insert between target and itemAfter
            var afterIndex = _orderIndices[itemAfter.Id];
            _orderIndices[item.Id] = (targetIndex + afterIndex) / 2.0;
        }

        RebuildOrderedList();
        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Gets the index of an item in the current order.
    /// </summary>
    /// <param name="item">The item to find.</param>
    /// <returns>The index of the item, or -1 if not found.</returns>
    public int IndexOf(TGridItem item)
    {
        if (item is null)
            return -1;

        for (var i = 0; i < _orderedItems.Count; i++)
        {
            if (_orderedItems[i].Id == item.Id)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Gets the current order as a list of Item IDs.
    /// Use this to persist the order and restore it later with <see cref="SetOrderIndices"/>.
    /// </summary>
    /// <returns>A list of Item IDs in their current order.</returns>
    public IReadOnlyList<int> GetOrderIndices()
    {
        return _orderedItems.Select(x => x.Id).ToList();
    }

    /// <summary>
    /// Restores order from a previously saved list of Item IDs.
    /// </summary>
    /// <param name="indices">The list of Item IDs in the desired order.</param>
    /// <exception cref="ArgumentNullException">Thrown when indices is null.</exception>
    /// <exception cref="ArgumentException">Thrown when indices don't match current items.</exception>
    public void SetOrderIndices(IReadOnlyList<int> indices)
    {
        ArgumentNullException.ThrowIfNull(indices);

        // Validate that indices match current items
        var currentIds = _orderedItems.Select(x => x.Id).ToHashSet();
        var providedIds = indices.ToHashSet();

        if (!currentIds.SetEquals(providedIds))
            throw new ArgumentException("Indices do not match current items.", nameof(indices));

        // Rebuild order based on provided indices
        _orderIndices.Clear();
        for (var i = 0; i < indices.Count; i++)
        {
            _orderIndices[indices[i]] = i;
        }

        RebuildOrderedList();
        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Resets order to the original order when the data source was created.
    /// </summary>
    public void ResetOrder()
    {
        _orderIndices.Clear();
        for (var i = 0; i < _originalItems.Count; i++)
        {
            _orderIndices[_originalItems[i].Id] = i;
        }

        RebuildOrderedList();
        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Updates the items in the data source, optionally preserving order.
    /// New items are appended to the end. Removed items lose their order.
    /// Re-added items are treated as new items.
    /// </summary>
    /// <param name="items">The new set of items.</param>
    /// <param name="preserveOrder">If true, existing items retain their order. If false, order is reset.</param>
    /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
    public void UpdateItems(IEnumerable<TGridItem> items, bool preserveOrder = true)
    {
        ArgumentNullException.ThrowIfNull(items);

        var newItems = items.ToList();

        if (!preserveOrder)
        {
            // Replace everything and reset order
            _originalItems.Clear();
            _originalItems.AddRange(newItems);
            _orderIndices.Clear();

            for (var i = 0; i < newItems.Count; i++)
            {
                _orderIndices[newItems[i].Id] = i;
            }

            RebuildOrderedList();
            OnOrderChanged?.Invoke();
            return;
        }

        // Preserve order for existing items
        var newIds = newItems.Select(x => x.Id).ToHashSet();
        var existingIds = _orderedItems.Select(x => x.Id).ToHashSet();

        // Remove order indices for items that no longer exist
        var removedIds = existingIds.Except(newIds).ToList();
        foreach (var id in removedIds)
        {
            _orderIndices.Remove(id);
        }

        // Find max order index for appending new items
        var maxOrderIndex = _orderIndices.Count > 0 
            ? _orderIndices.Values.Max() 
            : -1.0;

        // Add order indices for new items (appended to end)
        var addedItems = newItems.Where(x => !existingIds.Contains(x.Id)).ToList();
        foreach (var item in addedItems)
        {
            maxOrderIndex += 1.0;
            _orderIndices[item.Id] = maxOrderIndex;
        }

        // Update original items reference
        _originalItems.Clear();
        _originalItems.AddRange(newItems);

        // Rebuild ordered list from new items using existing order
        _orderedItems = newItems
            .Where(x => _orderIndices.ContainsKey(x.Id))
            .OrderBy(x => _orderIndices[x.Id])
            .ToList();

        OnOrderChanged?.Invoke();
    }

    /// <summary>
    /// Rebuilds the ordered items list based on current order indices.
    /// </summary>
    private void RebuildOrderedList()
    {
        _orderedItems = _orderedItems
            .OrderBy(x => _orderIndices[x.Id])
            .ToList();
    }

    /// <summary>
    /// Releases resources used by the data source.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _orderedItems.Clear();
        _orderIndices.Clear();
        _originalItems.Clear();
    }
}
