using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class HeaderTests : BunitContext
{
    [Fact]
    public void Highlights_active_screen()
    {
        var cut = Render<Header>(p => p
            .Add(c => c.ActiveScreen, "devices"));

        var activeButtons = cut.FindAll("button.is-active");
        Assert.Single(activeButtons);
        Assert.Contains("Devices", activeButtons[0].TextContent);
    }

    [Fact]
    public void Raises_ScreenChanged_with_clicked_id()
    {
        string? captured = null;

        var cut = Render<Header>(p => p
            .Add(c => c.ScreenChanged, (string id) => captured = id));

        var buildingsButton = cut.FindAll("button.rs-nav-btn")
            .First(b => b.TextContent.Contains("Buildings"));
        buildingsButton.Click();

        Assert.Equal("buildings", captured);
    }

    [Fact]
    public void Renders_backend_dropdown_when_switcher_enabled()
    {
        var cut = Render<Header>(p => p
            .Add(c => c.CanSwitchBackend, true)
            .Add(c => c.BackendOptions, new[]
            {
                new BackendOption("InMemory", "in-memory store"),
                new BackendOption("Rust",     "rust api"),
            })
            .Add(c => c.CurrentBackendId, "Rust"));

        var select = cut.Find("select.rs-backend-select");
        Assert.Equal("Rust", select.GetAttribute("value"));
        var options = select.QuerySelectorAll("option");
        Assert.Equal(2, options.Length);
        Assert.Contains("rust api", options[1].TextContent);
    }

    [Fact]
    public void Raises_BackendChanged_with_selected_id()
    {
        string? captured = null;

        var cut = Render<Header>(p => p
            .Add(c => c.CanSwitchBackend, true)
            .Add(c => c.BackendOptions, new[]
            {
                new BackendOption("InMemory", "in-memory store"),
                new BackendOption("Rust",     "rust api"),
            })
            .Add(c => c.CurrentBackendId, "InMemory")
            .Add(c => c.BackendChanged, (string id) => captured = id));

        cut.Find("select.rs-backend-select").Change("Rust");

        Assert.Equal("Rust", captured);
    }
}
