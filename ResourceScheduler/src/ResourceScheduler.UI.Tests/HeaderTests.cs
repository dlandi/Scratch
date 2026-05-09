using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
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
}
