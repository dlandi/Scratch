using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherStatistics.Services;

namespace WeatherStatistics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherStatistics(
        this IServiceCollection services,
        Action<WeatherGeneratorOptions>? configure = null)
    {
        if (configure is not null)
            services.Configure(configure);
        else
            services.Configure<WeatherGeneratorOptions>(_ => { });

        services.AddSingleton<WeatherGeneratorService>();
        services.AddSingleton<IWeatherGenerator>(sp => sp.GetRequiredService<WeatherGeneratorService>());
        services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<WeatherGeneratorService>());

        return services;
    }
}
