namespace AppSysMetrics.Diagnostics.Models;

/// <summary>
/// A single GC root that retains objects of a specific type on the managed heap.
/// Roots are grouped by <see cref="RootKind"/> and <see cref="RootObjectTypeName"/>
/// with <see cref="RetainedInstanceCount"/> aggregated across all matching roots.
/// </summary>
public sealed record GcRootInfo
{
    /// <summary>Root kind from <c>ClrRootKind</c>: Stack, StrongHandle, PinnedHandle, FinalizerQueue, etc.</summary>
    public required string RootKind { get; init; }

    /// <summary>The address of the root object (for deduplication and display).</summary>
    public ulong RootAddress { get; init; }

    /// <summary>Type name of the root object itself — the object directly held by the GC root.</summary>
    public required string RootObjectTypeName { get; init; }

    /// <summary>
    /// Human-readable retention path from the root object to the target type.
    /// Example: <c>"Dictionary&lt;String,Handler&gt; → Entry[] → Handler"</c>.
    /// Null when the root directly holds the target type, or when path tracing was skipped.
    /// </summary>
    public string? RetentionPath { get; init; }

    /// <summary>Number of instances of the target type reachable from this root.</summary>
    public int RetainedInstanceCount { get; init; }

    /// <summary>
    /// True if the retention path passes through at least one user/application code type
    /// (non-framework). Used for "Just My Code" sorting — user-code paths appear first.
    /// </summary>
    public bool HasUserCode { get; init; }
}
