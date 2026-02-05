using FeatureEnablement.Models;

namespace FeatureEnablement;

/// <summary>
/// Service interface for feature flag operations.
/// </summary>
public interface IFeatureFlagService
{
    /// <summary>
    /// Checks if a feature flag is enabled.
    /// </summary>
    /// <param name="flagKey">The unique key of the feature flag.</param>
    /// <returns>True if enabled, false if disabled or not found.</returns>
    bool IsEnabled(string flagKey);

    /// <summary>
    /// Gets all configured feature flags.
    /// </summary>
    /// <returns>Collection of all feature flags.</returns>
    IReadOnlyList<FeatureFlag> GetAllFlags();

    /// <summary>
    /// Gets a specific feature flag by its key.
    /// </summary>
    /// <param name="flagKey">The unique key of the feature flag.</param>
    /// <returns>The feature flag, or null if not found.</returns>
    FeatureFlag? GetFlag(string flagKey);

    /// <summary>
    /// Sets the enabled state of a feature flag.
    /// </summary>
    /// <param name="flagKey">The unique key of the feature flag.</param>
    /// <param name="isEnabled">The new enabled state.</param>
    /// <returns>True if the flag was found and updated, false otherwise.</returns>
    bool SetEnabled(string flagKey, bool isEnabled);

    /// <summary>
    /// Toggles the enabled state of a feature flag.
    /// </summary>
    /// <param name="flagKey">The unique key of the feature flag.</param>
    /// <returns>The new enabled state, or null if the flag was not found.</returns>
    bool? Toggle(string flagKey);
}
