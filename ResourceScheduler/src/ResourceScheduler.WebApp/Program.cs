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
builder.Services.AddSingleton<UserTimeProvider>();
builder.Services.AddSingleton<TimeProvider>(sp => sp.GetRequiredService<UserTimeProvider>());

// Phase 1: in-memory backend simulation. Singleton so the seed survives
// the lifetime of the tab. Phase 2 will replace this registration with
// an HTTP-backed implementation against the Rust API.
builder.Services.AddSingleton<IClientService, InMemoryClientService>();

await builder.Build().RunAsync();
