using AppSysMetrics.Collection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class AllocationTrackingService : BackgroundService
{
    private readonly AllocationEventListener _listener;
    private readonly AllocationTrackingHub _hub;
    private readonly MetricsCollectionOptions _options;
    private readonly ILogger<AllocationTrackingService> _logger;

    public AllocationTrackingService(
        AllocationEventListener listener,
        AllocationTrackingHub hub,
        IOptions<MetricsCollectionOptions> options,
        ILogger<AllocationTrackingService> logger)
    {
        _listener = listener;
        _hub = hub;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Allocation tracking service started.");

        using var timer = new PeriodicTimer(_options.CollectionInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var snapshot = _listener.CreateSnapshot();
                _hub.Publish(snapshot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create allocation snapshot");
            }
        }
    }

    public override void Dispose()
    {
        _listener.Dispose();
        base.Dispose();
    }
}
