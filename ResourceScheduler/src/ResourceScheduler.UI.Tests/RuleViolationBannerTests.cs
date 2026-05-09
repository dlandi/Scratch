using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class RuleViolationBannerTests : BunitContext
{
    [Fact]
    public void Renders_rule_id_chip_when_RuleId_is_set()
    {
        var cut = Render<RuleViolationBanner>(p => p
            .Add(c => c.RuleId, "R10")
            .AddChildContent("Some message"));

        Assert.Contains("rs-banner-rule", cut.Markup);
        Assert.Contains("R10", cut.Markup);
    }

    [Fact]
    public void Skips_rule_id_chip_when_RuleId_is_null()
    {
        var cut = Render<RuleViolationBanner>(p => p
            .AddChildContent("Some message"));

        Assert.DoesNotContain("rs-banner-rule", cut.Markup);
    }

    [Fact]
    public void Applies_warn_class_when_Level_is_Warn()
    {
        var cut = Render<RuleViolationBanner>(p => p
            .Add(c => c.Level, RuleViolationBanner.BannerLevel.Warn)
            .AddChildContent("Heads up"));

        Assert.Contains("rs-banner--warn", cut.Markup);
    }

    [Fact]
    public void Applies_info_class_when_Level_is_Info()
    {
        var cut = Render<RuleViolationBanner>(p => p
            .Add(c => c.Level, RuleViolationBanner.BannerLevel.Info)
            .AddChildContent("FYI"));

        Assert.Contains("rs-banner--info", cut.Markup);
    }

    [Fact]
    public void No_level_class_when_Level_is_Bad()
    {
        var cut = Render<RuleViolationBanner>(p => p
            .Add(c => c.Level, RuleViolationBanner.BannerLevel.Bad)
            .AddChildContent("Error"));

        Assert.DoesNotContain("rs-banner--warn", cut.Markup);
        Assert.DoesNotContain("rs-banner--info", cut.Markup);
    }
}
