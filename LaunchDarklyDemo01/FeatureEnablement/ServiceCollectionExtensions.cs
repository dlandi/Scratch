using FeatureEnablement.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureEnablement;

/// <summary>
/// Extension methods for registering feature flag services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds feature flag services to the service collection with JSON file persistence.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configFilePath">Path to the JSON file for storing feature flags.</param>
    /// <param name="configureFlags">Optional action to configure initial feature flags.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddFeatureFlags(
        this IServiceCollection services,
        string configFilePath,
        Action<FeatureFlagBuilder>? configureFlags = null)
    {
        var store = new JsonFeatureFlagStore(configFilePath);
        var service = new FeatureFlagService(store);

        if (configureFlags != null)
        {
            var builder = new FeatureFlagBuilder(service);
            configureFlags(builder);
        }

        services.AddSingleton<IFeatureFlagStore>(store);
        services.AddSingleton<IFeatureFlagService>(service);
        services.AddSingleton(service); // Also register concrete type for RegisterFlag access

        return services;
    }
}

/// <summary>
/// Builder for configuring feature flags during startup.
/// </summary>
public class FeatureFlagBuilder
{
    private readonly FeatureFlagService _service;

    internal FeatureFlagBuilder(FeatureFlagService service)
    {
        _service = service;
    }

    /// <summary>
    /// Registers a feature flag with the specified configuration.
    /// If the flag already exists in the store, the existing state is preserved.
    /// </summary>
    public FeatureFlagBuilder AddFlag(
        string key,
        string name,
        string description = "",
        bool defaultEnabled = false,
        string category = "General")
    {
        var existingFlag = _service.GetFlag(key);

        if (existingFlag == null)
        {
            _service.RegisterFlag(new FeatureFlag
            {
                Key = key,
                Name = name,
                Description = description,
                IsEnabled = defaultEnabled,
                Category = category,
                CreatedAt = DateTime.UtcNow
            });
        }
        else
        {
            // Update metadata but preserve enabled state
            existingFlag.Name = name;
            existingFlag.Description = description;
            existingFlag.Category = category;
        }

        return this;
    }
}
