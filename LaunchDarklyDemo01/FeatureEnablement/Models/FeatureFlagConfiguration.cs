namespace FeatureEnablement.Models;

/// <summary>
/// Configuration container for all feature flags, used for JSON serialization.
/// </summary>
public class FeatureFlagConfiguration
{
    /// <summary>
    /// Collection of all feature flags.
    /// </summary>
    public List<FeatureFlag> Flags { get; set; } = new();
}
