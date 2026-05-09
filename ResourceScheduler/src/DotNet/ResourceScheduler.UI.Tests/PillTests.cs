// bUnit version: 2.7.2 (targets net10.0). Uses BunitContext (the modern
// successor to bUnit 1.x's TestContext) and the Render<T>(parameter
// builder) API.
using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class PillTests : BunitContext
{
    [Fact]
    public void Pill_renders_ok_class_for_Available_device()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Device, DeviceStatus.Available));

        Assert.Contains("rs-pill--ok", cut.Markup);
    }

    [Fact]
    public void Pill_renders_warn_class_for_Maintenance_device()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Device, DeviceStatus.Maintenance));

        Assert.Contains("rs-pill--warn", cut.Markup);
    }

    [Fact]
    public void Pill_renders_off_class_for_Inactive_group()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Group, DeviceGroupStatus.Inactive));

        Assert.Contains("rs-pill--off", cut.Markup);
    }

    [Fact]
    public void Pill_renders_bad_class_for_Cancelled_reservation()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Reservation, ReservationStatus.Cancelled));

        Assert.Contains("rs-pill--bad", cut.Markup);
    }

    [Fact]
    public void Pill_uses_Text_override_when_provided()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Device, DeviceStatus.Available)
            .Add(c => c.Text, "Custom Label"));

        Assert.Contains("Custom Label", cut.Markup);
    }

    [Fact]
    public void Pill_falls_back_to_enum_name_when_Text_is_null()
    {
        var cut = Render<Pill>(p => p
            .Add(c => c.Reservation, ReservationStatus.Confirmed));

        Assert.Contains("Confirmed", cut.Markup);
    }
}
