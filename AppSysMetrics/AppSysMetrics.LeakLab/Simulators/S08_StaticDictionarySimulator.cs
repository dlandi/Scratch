using System.Collections.Concurrent;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S08 — ConcurrentDictionary entries added but never removed.
/// Simulates per-connection state stored in a static dictionary (e.g., SignalR hub)
/// where entries are added on connect but never cleaned up on disconnect.
/// </summary>
public sealed class S08_StaticDictionarySimulator : LeakSimulatorBase
{
    private readonly ConcurrentDictionary<string, byte[]> _connectionState = new();

    public override string ScenarioId => "S08";

    public override string Description =>
        "ConcurrentDictionary entries added but never removed — per-connection state leak";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 80 && !ct.IsCancellationRequested; i++)
        {
            var key = Guid.NewGuid().ToString();
            var payload = new byte[50_000]; // 50KB per entry
            Array.Fill(payload, (byte)(i % 256));
            _connectionState.TryAdd(key, payload);
            await Task.Delay(10, ct);
        }
    }

    public override void Reset()
    {
        _connectionState.Clear();
    }
}
