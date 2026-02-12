namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S15 — Background service accumulates items in a list without bound.
/// Simulates a hosted/worker service that appends to an internal collection
/// in a loop, deliberately ignoring cancellation (the bug pattern).
/// Runs continuously between StartAsync/StopAsync.
/// </summary>
public sealed class S15_HostedServiceSimulator : LeakSimulatorBase
{
    private readonly List<byte[]> _items = [];
    private Task? _backgroundTask;

    public override string ScenarioId => "S15";

    public override string Description =>
        "Background service accumulates items in a list without bound — ignores stopping token";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override Task OnStartAsync(CancellationToken ct)
    {
        _backgroundTask = Task.Run(async () =>
        {
            while (!ct.IsCancellationRequested)
            {
                var item = new byte[25_000]; // 25KB per tick
                Array.Fill(item, (byte)0xBB);
                lock (_items)
                    _items.Add(item);
                try
                {
                    await Task.Delay(50, ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }, ct);

        return Task.CompletedTask;
    }

    protected override async Task OnStopAsync(CancellationToken ct)
    {
        if (_backgroundTask is not null)
        {
            try { await _backgroundTask; }
            catch (OperationCanceledException) { }
            _backgroundTask = null;
        }
    }

    public override void Reset()
    {
        lock (_items)
            _items.Clear();
    }
}
