namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupStateManager<TValue>
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly HashSet<TValue> _expanded;
    private bool _defaultExpanded;

    public GroupStateManager(IEqualityComparer<TValue>? comparer = null)
    {
        _expanded = new HashSet<TValue>(comparer);
    }

    public bool HasExpandedGroups => _defaultExpanded || _expanded.Count > 0;

    public int ExpandedGroupCount => _expanded.Count;

    /// <summary>
    /// Sets the default expansion state for keys that have not been explicitly toggled.
    /// </summary>
    public void SetDefaultExpanded(bool defaultExpanded)
    {
        _defaultExpanded = defaultExpanded;
    }

    /// <summary>
    /// Sets the default expansion state and clears all toggled keys,
    /// effectively resetting all groups to the new default state.
    /// </summary>
    public void ResetToDefault(bool defaultExpanded)
    {
        _expanded.Clear();
        _defaultExpanded = defaultExpanded;
    }

    /// <summary>
    /// Returns whether a key has been explicitly toggled (vs. using default state).
    /// </summary>
    public bool HasExplicitState(TValue key) => _expanded.Contains(key);

    /// <summary>
    /// Returns whether the key is expanded.
    /// If the key has been toggled, returns the toggled state.
    /// Otherwise returns the default expansion state.
    /// </summary>
    public bool IsExpanded(TValue key)
    {
        return _defaultExpanded || _expanded.Contains(key);
    }

    public async Task InitializeAsync(IEnumerable<TValue> allKeys, bool initiallyExpanded, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(allKeys);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _defaultExpanded = initiallyExpanded;

            _expanded.Clear();
            if (initiallyExpanded)
            {
                foreach (var key in allKeys)
                    _expanded.Add(key);
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
            if (!_expanded.Add(key))
                _expanded.Remove(key);
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
            _expanded.Add(key);
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
            _expanded.Remove(key);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ExpandAllAsync(IEnumerable<TValue> allKeys, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _defaultExpanded = true;
            _expanded.Clear();

            ArgumentNullException.ThrowIfNull(allKeys);
            foreach (var key in allKeys)
                _expanded.Add(key);
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
            _defaultExpanded = false;
            _expanded.Clear();
        }
        finally
        {
            _lock.Release();
        }
    }
}
