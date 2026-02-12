namespace AppSysMetrics.LeakLab;

/// <summary>
/// Contract for a memory leak simulator. Each implementation reproduces a specific
/// leak scenario from the Blazor Server memory leak research (S01–S20).
///
/// <see cref="ExpectedLeakTypes"/> lists the exact type names ClrMD will report on
/// the managed heap — this is the assertion contract for integration tests.
/// </summary>
public interface ILeakSimulator : IAsyncDisposable, IDisposable
{
    /// <summary>Scenario identifier, e.g. "S01", "S03".</summary>
    string ScenarioId { get; }

    /// <summary>Human-readable description of the leak mechanism.</summary>
    string Description { get; }

    /// <summary>
    /// Type names that should appear as leak suspects after the simulator runs.
    /// These are the exact names ClrMD reports (e.g. "System.Byte[]",
    /// "AppSysMetrics.LeakLab.Simulators.Helpers.EventSubscriberComponent").
    /// </summary>
    IReadOnlyList<string> ExpectedLeakTypes { get; }

    /// <summary>True while the simulator is actively producing leaked objects.</summary>
    bool IsRunning { get; }

    /// <summary>
    /// Activate the simulator — begins allocating and retaining objects.
    /// For batch simulators, all allocations complete before this returns.
    /// For continuous simulators (S15, S16), a background task starts and this returns immediately.
    /// </summary>
    Task StartAsync(CancellationToken ct = default);

    /// <summary>Stop the simulator. Background tasks are cancelled but retained objects remain.</summary>
    Task StopAsync(CancellationToken ct = default);

    /// <summary>
    /// Release all retained references, allowing the leaked objects to be collected.
    /// Call after <see cref="StopAsync"/> to return to a clean state.
    /// </summary>
    void Reset();
}
