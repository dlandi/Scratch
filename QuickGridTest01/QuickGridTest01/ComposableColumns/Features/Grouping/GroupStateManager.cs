namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupStateManager<TValue>
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly HashSet<TValue> _expandedKeys;

    public GroupStateManager(IEqualityComparer<TValue>? comparer = null)
    {
        _expandedKeys = new HashSet<TValue>(comparer);
    }

    public bool HasExpandedGroups => _expandedKeys.Count > 0;

    public int ExpandedGroupCount => _expandedKeys.Count;

    public bool IsExpanded(TValue key) => _expandedKeys.Contains(key);

    public async Task InitializeAsync(IEnumerable<TValue> allKeys, bool initiallyExpanded, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(allKeys);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _expandedKeys.Clear();

            if (!initiallyExpanded)
                return;

            foreach (var key in allKeys)
            {
                _expandedKeys.Add(key);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ToggleAsync(TValue key, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (!_expandedKeys.Add(key))
            {
                _expandedKeys.Remove(key);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ExpandAsync(TValue key, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _expandedKeys.Add(key);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task CollapseAsync(TValue key, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _expandedKeys.Remove(key);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(allKeys);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            foreach (var key in allKeys)
            {
                _expandedKeys.Add(key);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task CollapseAllAsync(CancellationToken cancellationToken = default)
    {
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
}
