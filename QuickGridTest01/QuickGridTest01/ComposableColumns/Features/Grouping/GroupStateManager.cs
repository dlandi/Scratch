namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupStateManager<TValue>
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly HashSet<TValue> _toggledKeys;
    private bool _defaultExpanded;

    public GroupStateManager(IEqualityComparer<TValue>? comparer = null)
    {
        _toggledKeys = new HashSet<TValue>(comparer);
    }

    public bool HasExpandedGroups => _defaultExpanded || _toggledKeys.Count > 0;

    public int ExpandedGroupCount => _toggledKeys.Count;

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
        _toggledKeys.Clear();
        _defaultExpanded = defaultExpanded;
    }

    /// <summary>
    /// Returns whether a key has been explicitly toggled (vs. using default state).
    /// </summary>
    public bool HasExplicitState(TValue key) => _toggledKeys.Contains(key);

    /// <summary>
    /// Returns whether the key is expanded.
    /// If the key has been toggled, returns the toggled state.
    /// Otherwise returns the default expansion state.
    /// </summary>
    public bool IsExpanded(TValue key)
    {
        if (_toggledKeys.Contains(key))
        {
            // Key has been toggled: if default is expanded, being in set means collapsed; vice versa
            return !_defaultExpanded;
        }
        return _defaultExpanded;
    }

    public async Task InitializeAsync(IEnumerable<TValue> allKeys, bool initiallyExpanded, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(allKeys);

        await _lock.WaitAsync(cancellationToken);
        try
        {
            _toggledKeys.Clear();
            _defaultExpanded = initiallyExpanded;
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
            // Toggle: if key is in set, remove it (revert to default); if not, add it (deviate from default)
            if (!_toggledKeys.Add(key))
            {
                _toggledKeys.Remove(key);
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
            if (_defaultExpanded)
            {
                // Default is expanded, so remove from toggled set to get expanded
                _toggledKeys.Remove(key);
            }
            else
            {
                // Default is collapsed, so add to toggled set to get expanded
                _toggledKeys.Add(key);
            }
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
            if (_defaultExpanded)
            {
                // Default is expanded, so add to toggled set to get collapsed
                _toggledKeys.Add(key);
            }
            else
            {
                // Default is collapsed, so remove from toggled set to get collapsed
                _toggledKeys.Remove(key);
            }
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
            _toggledKeys.Clear();
            _defaultExpanded = true;
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
            _toggledKeys.Clear();
            _defaultExpanded = false;
        }
        finally
        {
            _lock.Release();
        }
    }
}
