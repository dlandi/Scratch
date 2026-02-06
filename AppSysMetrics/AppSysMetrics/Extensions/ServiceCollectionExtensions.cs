using AppSysMetrics.Collection;
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

        services.AddSingleton<IMetricsCollector, MetricsCollector>();
        services.AddSingleton<MetricsHub>();
        services.AddHostedService<MetricsCollectionService>();

        return services;
    }
}
