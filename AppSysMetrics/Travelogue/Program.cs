using AppSysMetrics.Extensions;
using WeatherStatistics.Extensions;
using Travelogue.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAppSysMetrics(options =>
{
    options.CollectionInterval = TimeSpan.FromSeconds(2);
    options.MaxHistorySize = 60;
});

builder.Services.AddWeatherStatistics(options =>
{
    options.GenerationInterval = TimeSpan.FromMilliseconds(100);
    options.ReadingsPerTick = 5;
    options.AutoStart = false;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
