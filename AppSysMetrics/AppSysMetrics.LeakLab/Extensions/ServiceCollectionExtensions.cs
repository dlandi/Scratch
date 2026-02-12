using AppSysMetrics.LeakLab.Simulators;
using AppSysMetrics.LeakLab.Simulators.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AppSysMetrics.LeakLab.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all LeakLab simulators and supporting infrastructure.
    /// Each simulator is a singleton — it holds state (leaked objects) across heap captures.
    /// </summary>
    public static IServiceCollection AddLeakLab(
        this IServiceCollection services,
        Action<LeakLabOptions>? configure = null)
    {
        if (configure is not null)
            services.Configure(configure);
        else
            services.Configure<LeakLabOptions>(_ => { });

        // Helper singletons needed by simulators
        services.AddSingleton<SingletonEventPublisher>();

        // Register each simulator as ILeakSimulator (collected by LeakLabRegistry)
        services.AddSingleton<ILeakSimulator, S01_DotNetObjectRefSimulator>();
        services.AddSingleton<ILeakSimulator, S03_EventHandlerSimulator>();
        services.AddSingleton<ILeakSimulator, S05_ClosureCaptureSimulator>();
        services.AddSingleton<ILeakSimulator, S06_LargeCircuitStateSimulator>();
        services.AddSingleton<ILeakSimulator, S08_StaticDictionarySimulator>();
        services.AddSingleton<ILeakSimulator, S10_MiddlewareFieldSimulator>();
        services.AddSingleton<ILeakSimulator, S13_UnboundedCacheSimulator>();
        services.AddSingleton<ILeakSimulator, S15_HostedServiceSimulator>();
        services.AddSingleton<ILeakSimulator, S16_UnboundedChannelSimulator>();
        services.AddSingleton<ILeakSimulator, S17_EfCoreTrackingSimulator>();

        // Registry collects all ILeakSimulator registrations
        services.AddSingleton<LeakLabRegistry>();

        return services;
    }
}
