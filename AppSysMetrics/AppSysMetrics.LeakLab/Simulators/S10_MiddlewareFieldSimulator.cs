namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S10 — Middleware instance fields accumulate per-request data.
/// Simulates a singleton middleware that stores per-request payloads in a
/// list field, never releasing them. The list acts as a hidden singleton
/// that grows with every request.
/// </summary>
public sealed class S10_MiddlewareFieldSimulator : LeakSimulatorBase
{
    private readonly List<byte[]> _requestPayloads = [];

    public override string ScenarioId => "S10";

    public override string Description =>
        "Singleton middleware stores per-request data in instance fields — never released";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 200 && !ct.IsCancellationRequested; i++)
        {
            var payload = new byte[20_000]; // 20KB per "request"
            Array.Fill(payload, (byte)(i % 256));
            lock (_requestPayloads)
                _requestPayloads.Add(payload);
            await Task.Delay(5, ct);
        }
    }

    public override void Reset()
    {
        lock (_requestPayloads)
            _requestPayloads.Clear();
    }
}
