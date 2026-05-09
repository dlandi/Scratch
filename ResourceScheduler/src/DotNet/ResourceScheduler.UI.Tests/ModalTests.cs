using Bunit;
using Microsoft.AspNetCore.Components;
using ResourceScheduler.Components.Components;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class ModalTests : BunitContext
{
    [Fact]
    public void Renders_nothing_when_IsOpen_is_false()
    {
        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, false)
            .Add(c => c.Title, "Hidden"));

        Assert.Equal(string.Empty, cut.Markup.Trim());
        Assert.DoesNotContain("rs-modal-backdrop", cut.Markup);
    }

    [Fact]
    public void Renders_backdrop_and_dialog_when_IsOpen_is_true()
    {
        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Title, "Visible"));

        Assert.Contains("rs-modal-backdrop", cut.Markup);
        Assert.Contains("rs-modal", cut.Markup);
    }

    [Fact]
    public void Click_on_close_button_raises_OnClose()
    {
        var fired = 0;
        var cb = EventCallback.Factory.Create(this, () => fired++);

        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Title, "Closable")
            .Add(c => c.OnClose, cb));

        cut.Find(".rs-modal-close").Click();

        Assert.Equal(1, fired);
    }

    [Fact]
    public void Click_on_backdrop_raises_OnClose()
    {
        var fired = 0;
        var cb = EventCallback.Factory.Create(this, () => fired++);

        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Title, "Closable")
            .Add(c => c.OnClose, cb));

        cut.Find(".rs-modal-backdrop").Click();

        Assert.Equal(1, fired);
    }

    [Fact]
    public void Dialog_uses_stoppropagation_so_inside_clicks_do_not_bubble_to_backdrop()
    {
        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Title, "Closable")
            .Add(c => c.OnClose, EventCallback.Empty));

        // The dialog itself has no onclick handler; instead Modal applies
        // @onclick:stoppropagation="true" to the dialog so that real DOM
        // clicks inside the dialog do not bubble to the backdrop. bUnit
        // does not simulate propagation, but we can verify the marker
        // attribute is rendered.
        var dialog = cut.Find(".rs-modal");
        Assert.True(dialog.HasAttribute("blazor:onclick:stoppropagation"));
    }

    [Fact]
    public void Renders_title_and_child_content()
    {
        var cut = Render<Modal>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Title, "Hello")
            .Add(c => c.ChildContent, builder => builder.AddContent(0, "Body!")));

        Assert.Contains("Hello", cut.Markup);
        Assert.Contains("Body!", cut.Markup);
    }
}
