using FeatureEnablement.Models;

namespace FeatureEnablement;

/// <summary>
/// Interface for feature flag persistence.
/// </summary>
public interface IFeatureFlagStore
{
    /// <summary>
    /// Loads feature flags from the backing store.
    /// </summary>
    /// <returns>The loaded configuration, or a new empty configuration if none exists.</returns>
    Task<FeatureFlagConfiguration> LoadAsync();

    /// <summary>
    /// Saves feature flags to the backing store.
    /// </summary>
    /// <param name="configuration">The configuration to save.</param>
    Task SaveAsync(FeatureFlagConfiguration configuration);
}
