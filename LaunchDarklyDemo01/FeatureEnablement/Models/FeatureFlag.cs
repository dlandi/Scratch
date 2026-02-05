namespace FeatureEnablement.Models;

/// <summary>
/// Represents a feature flag with its configuration and state.
/// </summary>
public class FeatureFlag
{
    /// <summary>
    /// Unique identifier for the feature flag.
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Human-readable name for display in admin UI.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of what this feature flag controls.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Whether the feature is currently enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Optional grouping category for organizing flags in the UI.
    /// </summary>
    public string Category { get; set; } = "General";

    /// <summary>
    /// When the flag was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the flag was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
}
