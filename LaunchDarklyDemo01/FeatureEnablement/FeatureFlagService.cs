using FeatureEnablement.Models;

namespace FeatureEnablement;

/// <summary>
/// Default implementation of the feature flag service.
/// Maintains an in-memory cache with persistence through the store.
/// </summary>
public class FeatureFlagService : IFeatureFlagService
{
    private readonly IFeatureFlagStore _store;
    private readonly Dictionary<string, FeatureFlag> _cache = new();
    private readonly object _cacheLock = new();
    private bool _initialized;

    public FeatureFlagService(IFeatureFlagStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Ensures the cache is loaded from the store. Called synchronously at startup.
    /// </summary>
    private void EnsureInitialized()
    {
        if (_initialized) return;

        lock (_cacheLock)
        {
            if (_initialized) return;

            // This is only called once at startup, before any async context
            var config = _store.LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var flag in config.Flags)
            {
                _cache[flag.Key] = flag;
            }
            _initialized = true;
        }
    }

    public bool IsEnabled(string flagKey)
    {
        EnsureInitialized();

        lock (_cacheLock)
        {
            return _cache.TryGetValue(flagKey, out var flag) && flag.IsEnabled;
        }
    }

    public IReadOnlyList<FeatureFlag> GetAllFlags()
    {
        EnsureInitialized();

        lock (_cacheLock)
        {
            // Return copies to avoid modification issues
            return _cache.Values.Select(f => new FeatureFlag
            {
                Key = f.Key,
                Name = f.Name,
                Description = f.Description,
                IsEnabled = f.IsEnabled,
                Category = f.Category,
                CreatedAt = f.CreatedAt,
                LastModifiedAt = f.LastModifiedAt
            }).ToList();
        }
    }

    public FeatureFlag? GetFlag(string flagKey)
    {
        EnsureInitialized();

        lock (_cacheLock)
        {
            return _cache.TryGetValue(flagKey, out var flag) ? flag : null;
        }
    }

    public bool SetEnabled(string flagKey, bool isEnabled)
    {
        EnsureInitialized();

        lock (_cacheLock)
        {
            if (!_cache.TryGetValue(flagKey, out var flag))
            {
                return false;
            }

            flag.IsEnabled = isEnabled;
            flag.LastModifiedAt = DateTime.UtcNow;
        }

        // Persist outside the lock, fire-and-forget
        PersistChangesAsync();
        return true;
    }

    public bool? Toggle(string flagKey)
    {
        EnsureInitialized();
        bool newState;

        lock (_cacheLock)
        {
            if (!_cache.TryGetValue(flagKey, out var flag))
            {
                return null;
            }

            flag.IsEnabled = !flag.IsEnabled;
            flag.LastModifiedAt = DateTime.UtcNow;
            newState = flag.IsEnabled;
        }

        // Persist outside the lock, fire-and-forget
        PersistChangesAsync();
        return newState;
    }

    /// <summary>
    /// Registers a new feature flag. Used during application startup.
    /// </summary>
    public void RegisterFlag(FeatureFlag flag)
    {
        EnsureInitialized();

        bool needsPersist = false;
        lock (_cacheLock)
        {
            if (!_cache.ContainsKey(flag.Key))
            {
                _cache[flag.Key] = flag;
                needsPersist = true;
            }
        }

        if (needsPersist)
        {
            PersistChangesAsync();
        }
    }

    private void PersistChangesAsync()
    {
        List<FeatureFlag> flagsCopy;
        lock (_cacheLock)
        {
            flagsCopy = _cache.Values.ToList();
        }

        var config = new FeatureFlagConfiguration
        {
            Flags = flagsCopy
        };

        // Fire and forget - don't block the UI thread
        _ = Task.Run(async () =>
        {
            try
            {
                await _store.SaveAsync(config);
            }
            catch
            {
                // Log error in production; for demo, silently ignore
            }
        });
    }
}
