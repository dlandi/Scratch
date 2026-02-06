using AppSysMetrics.Collection;
using AppSysMetrics.Diagnostics;
using AppSysMetrics.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppSysMetrics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppSysMetrics(
        this IServiceCollection services,
        Action<MetricsCollectionOptions>? configure = null)
    {
        if (configure is not null)
            services.Configure(configure);
        else
            services.Configure<MetricsCollectionOptions>(_ => { });

        // Core metrics collection
        services.AddSingleton<IMetricsCollector, MetricsCollector>();
        services.AddSingleton<MetricsHub>();
        services.AddHostedService<MetricsCollectionService>();

        // Tier 1: Allocation tracking via EventListener
        services.AddSingleton<AllocationEventListener>();
        services.AddSingleton<AllocationTrackingHub>();
        services.AddHostedService<AllocationTrackingService>();

        // Tier 2: Diagnostics (Force GC, GC Dump)
        services.Configure<DiagnosticsOptions>(_ => { });
        services.AddSingleton<IDiagnosticsService, DiagnosticsService>();

        return services;
    }
}
