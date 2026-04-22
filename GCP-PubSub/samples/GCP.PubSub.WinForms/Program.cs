using GCP.PubSub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GCP.PubSub.WinForms;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        ApplicationConfiguration.Initialize();

        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddGcpPubSub();
                services.AddTransient<MainForm>();
            })
            .Build();

        await host.StartAsync();

        var form = host.Services.GetRequiredService<MainForm>();
        Application.Run(form);

        await host.StopAsync();
    }
}
