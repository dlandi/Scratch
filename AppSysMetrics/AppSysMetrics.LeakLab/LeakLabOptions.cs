namespace AppSysMetrics.LeakLab;

/// <summary>
/// Configurable defaults for leak simulators. Simulators use these values
/// unless overridden by scenario-specific requirements.
/// </summary>
public sealed class LeakLabOptions
{
    /// <summary>Default allocation chunk size in bytes per simulator tick.</summary>
    public int DefaultChunkSizeBytes { get; set; } = 50_000;

    /// <summary>Default interval between allocation ticks.</summary>
    public TimeSpan DefaultTickInterval { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>Number of allocation ticks per StartAsync call (controls total volume).</summary>
    public int DefaultTickCount { get; set; } = 200;
}
