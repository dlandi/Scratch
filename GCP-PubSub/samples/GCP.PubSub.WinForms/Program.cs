using GCP.PubSub;
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
