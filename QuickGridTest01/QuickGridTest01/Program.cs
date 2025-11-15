using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using QuickGridTest01.Data;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text;

namespace QuickGridTest01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Ensure console supports UTF-8 (for emoji/special chars in logs)
            Console.OutputEncoding = Encoding.UTF8;

            // Configure Serilog early
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} {Level:u3} {SourceContext} {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Replace default logging with Serilog
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            // Configure supported cultures (optional list)
            var supportedCultures = new[]
            {
                "en-US","en-GB","de-DE","fr-FR","ja-JP","es-ES"
            }.Select(c => new CultureInfo(c)).ToList();

            // Set a default (can be overridden per circuit/component)
            var defaultCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/_Host");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}