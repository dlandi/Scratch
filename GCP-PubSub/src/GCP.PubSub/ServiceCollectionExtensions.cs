using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GCP.PubSub;

public static class ServiceCollectionExtensions
{
    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "PubSubOptions properties are simple types preserved by the linker.")]
    public static IServiceCollection AddGcpPubSub(
        this IServiceCollection services,
        Action<PubSubOptions> configure)
    {
        services.AddOptions<PubSubOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        RegisterCoreServices(services);
        return services;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026",
        Justification = "PubSubOptions properties are simple types preserved by the linker.")]
    public static IServiceCollection AddGcpPubSub(
        this IServiceCollection services,
        string configSectionPath = "PubSub")
    {
        services.AddOptions<PubSubOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        RegisterCoreServices(services);
        return services;
    }

    private static void RegisterCoreServices(IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<IPubSubPublisher, PubSubPublisher>();
        services.TryAddSingleton<IPubSubSubscriber, PubSubSubscriber>();
        services.TryAddSingleton<IPubSubAdmin, PubSubAdmin>();
    }
}
