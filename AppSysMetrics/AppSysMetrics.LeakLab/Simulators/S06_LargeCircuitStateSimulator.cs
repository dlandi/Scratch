namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S06 — Circuit-scoped service holds large data that persists for circuit lifetime.
/// Simulates multiple Blazor circuits each contributing large payloads to a
/// scoped service that is never released until the circuit disconnects.
/// Without actual Blazor infrastructure, we model this as a singleton
/// accumulating byte[] payloads representing circuit state.
/// </summary>
public sealed class S06_LargeCircuitStateSimulator : LeakSimulatorBase
{
    private readonly List<byte[]> _circuitPayloads = [];

    public override string ScenarioId => "S06";

    public override string Description =>
        "Circuit-scoped service holds large data that persists for circuit lifetime";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 40 && !ct.IsCancellationRequested; i++)
        {
            var payload = new byte[100_000]; // 100KB per "circuit"
            Array.Fill(payload, (byte)(i % 256));
            lock (_circuitPayloads)
                _circuitPayloads.Add(payload);
            await Task.Delay(50, ct);
        }
    }

    public override void Reset()
    {
        lock (_circuitPayloads)
            _circuitPayloads.Clear();
    }
}
