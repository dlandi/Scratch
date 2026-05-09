using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class StatusDotTests : BunitContext
{
    [Fact]
    public void StatusDot_renders_ok_class_for_Available_device()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Device, DeviceStatus.Available));

        Assert.Contains("rs-dot--ok", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_warn_class_for_Maintenance_device()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Device, DeviceStatus.Maintenance));

        Assert.Contains("rs-dot--warn", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_off_class_for_Offline_device()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Device, DeviceStatus.Offline));

        Assert.Contains("rs-dot--off", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_retired_class_for_Retired_device()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Device, DeviceStatus.Retired));

        Assert.Contains("rs-dot--retired", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_ok_class_for_Active_group()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Group, DeviceGroupStatus.Active));

        Assert.Contains("rs-dot--ok", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_off_class_for_Inactive_group()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Group, DeviceGroupStatus.Inactive));

        Assert.Contains("rs-dot--off", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_warn_class_for_Pending_reservation()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Reservation, ReservationStatus.Pending));

        Assert.Contains("rs-dot--warn", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_ok_class_for_Confirmed_reservation()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Reservation, ReservationStatus.Confirmed));

        Assert.Contains("rs-dot--ok", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_bad_class_for_Cancelled_reservation()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Reservation, ReservationStatus.Cancelled));

        Assert.Contains("rs-dot--bad", cut.Markup);
    }

    [Fact]
    public void StatusDot_renders_off_class_for_Completed_reservation()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Reservation, ReservationStatus.Completed));

        Assert.Contains("rs-dot--off", cut.Markup);
    }

    [Fact]
    public void StatusDot_inline_style_honors_Size_parameter()
    {
        var cut = Render<StatusDot>(p => p
            .Add(c => c.Device, DeviceStatus.Available)
            .Add(c => c.Size, 16));

        Assert.Contains("width:16px", cut.Markup);
        Assert.Contains("height:16px", cut.Markup);
    }
}
