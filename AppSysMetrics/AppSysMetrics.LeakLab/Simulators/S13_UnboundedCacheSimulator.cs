using Microsoft.Extensions.Caching.Memory;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S13 — IMemoryCache with no size limit and no expiration.
/// Creates a standalone <see cref="MemoryCache"/> with no SizeLimit configured.
/// Adds entries with unique GUID keys and large byte[] values without any
/// expiration. The cache grows without bound.
/// </summary>
public sealed class S13_UnboundedCacheSimulator : LeakSimulatorBase
{
    private MemoryCache? _cache;

    public override string ScenarioId => "S13";

    public override string Description =>
        "IMemoryCache with no size limit and no expiration — cache grows without bound";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["System.Byte[]", "Microsoft.Extensions.Caching.Memory.CacheEntry"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        _cache = new MemoryCache(new MemoryCacheOptions()); // No SizeLimit

        for (var i = 0; i < 150 && !ct.IsCancellationRequested; i++)
        {
            var key = Guid.NewGuid().ToString();
            var payload = new byte[30_000]; // 30KB per entry
            Array.Fill(payload, (byte)(i % 256));
            _cache.Set(key, payload); // No expiration
            await Task.Delay(10, ct);
        }
    }

    public override void Reset()
    {
        _cache?.Dispose();
        _cache = null;
    }
}
