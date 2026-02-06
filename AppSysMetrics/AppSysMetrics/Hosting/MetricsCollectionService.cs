using AppSysMetrics.Collection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class MetricsCollectionService : BackgroundService
{
    private readonly IMetricsCollector _collector;
    private readonly MetricsHub _hub;
    private readonly MetricsCollectionOptions _options;
    private readonly ILogger<MetricsCollectionService> _logger;

    public MetricsCollectionService(
        IMetricsCollector collector,
        MetricsHub hub,
        IOptions<MetricsCollectionOptions> options,
        ILogger<MetricsCollectionService> logger)
    {
        _collector = collector;
        _hub = hub;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Metrics collection started. Interval: {Interval}",
            _options.CollectionInterval);

        CollectAndPublish();

        using var timer = new PeriodicTimer(_options.CollectionInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            CollectAndPublish();
        }
    }

    private void CollectAndPublish()
    {
        try
        {
            var snapshot = _collector.Collect();
            _hub.Publish(snapshot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect metrics snapshot");
        }
    }
}
