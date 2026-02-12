using System.Threading.Channels;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S16 — Unbounded Channel with fast producer and slow consumer.
/// The producer writes at ~100 items/sec while the consumer reads at ~1 item/sec.
/// The channel backlog grows without bound, consuming memory proportional to
/// the rate imbalance multiplied by time.
/// </summary>
public sealed class S16_UnboundedChannelSimulator : LeakSimulatorBase
{
    private Channel<byte[]>? _channel;
    private Task? _producerTask;
    private Task? _consumerTask;

    public override string ScenarioId => "S16";

    public override string Description =>
        "Unbounded Channel with fast producer and slow consumer — channel grows without bound";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]"];

    protected override Task OnStartAsync(CancellationToken ct)
    {
        _channel = Channel.CreateUnbounded<byte[]>();

        _producerTask = Task.Run(async () =>
        {
            var i = 0;
            while (!ct.IsCancellationRequested)
            {
                var item = new byte[20_000]; // 20KB per item
                Array.Fill(item, (byte)(i++ % 256));
                try
                {
                    await _channel.Writer.WriteAsync(item, ct);
                    await Task.Delay(10, ct); // ~100 items/sec
                }
                catch (OperationCanceledException) { break; }
            }
            _channel.Writer.TryComplete();
        }, ct);

        _consumerTask = Task.Run(async () =>
        {
            try
            {
                await foreach (var _ in _channel.Reader.ReadAllAsync(ct))
                {
                    await Task.Delay(1000, ct); // ~1 item/sec — slow consumer
                }
            }
            catch (OperationCanceledException) { }
        }, ct);

        return Task.CompletedTask;
    }

    protected override async Task OnStopAsync(CancellationToken ct)
    {
        _channel?.Writer.TryComplete();

        if (_producerTask is not null)
        {
            try { await _producerTask; }
            catch (OperationCanceledException) { }
        }

        if (_consumerTask is not null)
        {
            try { await _consumerTask; }
            catch (OperationCanceledException) { }
        }
    }

    public override void Reset()
    {
        // Drain remaining items from the channel
        if (_channel is not null)
        {
            while (_channel.Reader.TryRead(out _)) { }
        }
        _channel = null;
        _producerTask = null;
        _consumerTask = null;
    }
}
