using FeatureEnablement;
using FeaturesDemo.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure feature flags
var featureFlagsPath = Path.Combine(builder.Environment.ContentRootPath, "features.json");
builder.Services.AddFeatureFlags(featureFlagsPath, flags =>
{
    flags.AddFlag(
        FeatureFlags.HeroDarkMode,
        "Hero Dark Mode",
        "Inverts the hero section color scheme from light to dark",
        defaultEnabled: false,
        category: "Visual");

    flags.AddFlag(
        FeatureFlags.CreamBackground,
        "Cream Background",
        "Changes the page background from off-white to cream color",
        defaultEnabled: false,
        category: "Visual");

    flags.AddFlag(
        FeatureFlags.EnhancedTitle,
        "Enhanced Title",
        "Changes the title from 'The Best Travel Agency' to 'The Very Best Travel Agency'",
        defaultEnabled: false,
        category: "Content");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
