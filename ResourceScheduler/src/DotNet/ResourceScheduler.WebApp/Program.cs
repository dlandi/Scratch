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

// Phase 1: in-memory backend simulation. Singleton so the seed survives
// the lifetime of the tab. Phase 2 will replace this registration with
// an HTTP-backed implementation against the Rust API.
builder.Services.AddSingleton<IClientService, InMemoryClientService>();

await builder.Build().RunAsync();
