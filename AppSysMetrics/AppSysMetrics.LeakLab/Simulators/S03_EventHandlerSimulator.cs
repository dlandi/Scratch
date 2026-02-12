using AppSysMetrics.LeakLab.Simulators.Helpers;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S03 — Component subscribes to singleton event but never unsubscribes.
/// Creates <see cref="EventSubscriberComponent"/> instances that subscribe to
/// <see cref="SingletonEventPublisher.OnDataReceived"/>. The delegate retains
/// each component via the event's invocation list. Components are NOT stored
/// in a separate list — they are retained ONLY through the event chain.
/// </summary>
public sealed class S03_EventHandlerSimulator : LeakSimulatorBase
{
    private readonly SingletonEventPublisher _publisher;
    private readonly List<Action<byte[]>> _handlers = [];

    public S03_EventHandlerSimulator(SingletonEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public override string ScenarioId => "S03";

    public override string Description =>
        "Component subscribes to singleton event but never unsubscribes — delegate retains component";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["AppSysMetrics.LeakLab.Simulators.Helpers.EventSubscriberComponent", "System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 300 && !ct.IsCancellationRequested; i++)
        {
            var component = new EventSubscriberComponent(10_000); // 10KB state per component
            Action<byte[]> handler = component.OnData;
            _publisher.OnDataReceived += handler;

            // Keep a reference to the handler for Reset() to unsubscribe
            _handlers.Add(handler);

            await Task.Delay(5, ct);
        }
    }

    public override void Reset()
    {
        foreach (var handler in _handlers)
            _publisher.OnDataReceived -= handler;
        _handlers.Clear();
    }
}
