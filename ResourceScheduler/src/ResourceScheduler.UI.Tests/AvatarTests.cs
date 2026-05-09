using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class AvatarTests : BunitContext
{
    [Fact]
    public void Renders_two_letter_initials_for_two_word_name()
    {
        var cut = Render<Avatar>(p => p
            .Add(c => c.Name, "Aoife O'Brien"));

        var textElement = cut.Find("text");
        Assert.Equal("AO", textElement.TextContent);
    }

    [Fact]
    public void Renders_first_two_letters_for_single_word_name()
    {
        var cut = Render<Avatar>(p => p
            .Add(c => c.Name, "Madonna"));

        var textElement = cut.Find("text");
        Assert.Equal("MA", textElement.TextContent);
    }

    [Fact]
    public void Honors_Size_parameter()
    {
        var cut = Render<Avatar>(p => p
            .Add(c => c.Name, "Test User")
            .Add(c => c.Size, 32));

        var svg = cut.Find("svg");
        Assert.Equal("32", svg.GetAttribute("width"));
        Assert.Equal("32", svg.GetAttribute("height"));
    }
}
