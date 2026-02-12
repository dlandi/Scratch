namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S05 — Lambda closures capture large objects and are stored in a long-lived collection.
/// Simulates a singleton cache of delegates (e.g., RenderFragment cache) where
/// each lambda captures a freshly-allocated byte[] array. The captured arrays
/// are reachable ONLY through the closure — no other reference exists.
/// </summary>
public sealed class S05_ClosureCaptureSimulator : LeakSimulatorBase
{
    private readonly List<Action> _cachedDelegates = [];

    public override string ScenarioId => "S05";

    public override string Description =>
        "Lambda closures capture large objects and are stored in a singleton — preventing GC";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 60 && !ct.IsCancellationRequested; i++)
        {
            var payload = new byte[50_000]; // 50KB per closure
            Array.Fill(payload, (byte)(i % 256));

            // The lambda captures 'payload' — it's only reachable through the closure
            _cachedDelegates.Add(() => _ = payload.Length);
            await Task.Delay(10, ct);
        }
    }

    public override void Reset()
    {
        _cachedDelegates.Clear();
    }
}
