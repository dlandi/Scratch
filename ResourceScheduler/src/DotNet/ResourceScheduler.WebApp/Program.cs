using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ResourceScheduler.Components.Services;
using ResourceScheduler.WebApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Standard abstraction for time. Pages and services inject TimeProvider
// instead of calling DateTime.Now / UtcNow / Today directly so tests can
// substitute Microsoft.Extensions.Time.Testing.FakeTimeProvider where
// needed. See https://learn.microsoft.com/dotnet/standard/datetime/timeprovider-overview.
//
// UserTimeProvider lets the header dropdown override LocalTimeZone for
// developer/testing previews. Both registrations point to the same
// instance so existing TimeProvider injections keep working unchanged.
//
// The factory is explicit because UserTimeProvider has two constructors:
// the parameterless default and one that takes an inner TimeProvider for
// tests. Without the factory, the DI container picks the longer
// constructor and tries to resolve TimeProvider, which resolves back to
// UserTimeProvider, looping forever and locking the WASM startup on
// the loading splash.
builder.Services.AddSingleton<UserTimeProvider>(_ => new UserTimeProvider(TimeProvider.System));
builder.Services.AddSingleton<TimeProvider>(sp => sp.GetRequiredService<UserTimeProvider>());

// Both backends are registered as concrete singletons so BackendSwitcher
// can choose between them at runtime. IClientService resolves to the
// switcher; the active backend is controlled by the header dropdown
// when Features:BackendSwitcher:Enabled is true. The HttpClient used
// by the Rust client takes its base URL from Api:BaseUrl in
// appsettings.json (default 127.0.0.1:7070, where the Rust binary
// listens out of the box).
builder.Services.AddSingleton<InMemoryClientService>();
builder.Services.AddSingleton(_ =>
{
    var baseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://127.0.0.1:7070";
    return new RemoteClientService(new HttpClient { BaseAddress = new Uri(baseUrl) });
});
builder.Services.AddSingleton<BackendSwitcher>();
builder.Services.AddSingleton<IClientService>(sp => sp.GetRequiredService<BackendSwitcher>());

await builder.Build().RunAsync();
