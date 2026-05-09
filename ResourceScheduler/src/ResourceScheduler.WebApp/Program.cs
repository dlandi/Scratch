using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ResourceScheduler.Components.Services;
using ResourceScheduler.WebApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Phase 1: in-memory backend simulation. Singleton so the seed survives
// the lifetime of the tab. Phase 2 will replace this registration with
// an HTTP-backed implementation against the Rust API.
builder.Services.AddSingleton<IClientService, InMemoryClientService>();

await builder.Build().RunAsync();
