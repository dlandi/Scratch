using System.Text.RegularExpressions;
using Bunit;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class MiniTopologyTests : BunitContext
{
    private static DeviceDto MakeDevice(Guid id, DeviceStatus status = DeviceStatus.Available) =>
        new()
        {
            DeviceId = id,
            Name = $"Device-{id}",
            Status = status,
            BuildingId = Guid.NewGuid(),
        };

    [Fact]
    public void Renders_one_rect_per_member_device()
    {
        var d1 = Guid.NewGuid();
        var d2 = Guid.NewGuid();
        var d3 = Guid.NewGuid();

        var group = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new List<Guid> { d1, d2, d3 },
            Connections = new List<DeviceConnectionDto>(),
        };

        var devices = new Dictionary<Guid, DeviceDto>
        {
            [d1] = MakeDevice(d1),
            [d2] = MakeDevice(d2),
            [d3] = MakeDevice(d3),
        };

        var cut = Render<MiniTopology>(p => p
            .Add(c => c.Group, group)
            .Add(c => c.Devices, devices));

        var rectCount = Regex.Matches(cut.Markup, "<rect ").Count;
        Assert.Equal(3, rectCount);
    }

    [Fact]
    public void Renders_one_line_per_connection()
    {
        var d1 = Guid.NewGuid();
        var d2 = Guid.NewGuid();

        var group = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new List<Guid> { d1, d2 },
            Connections = new List<DeviceConnectionDto>
            {
                new()
                {
                    ConnectionId = Guid.NewGuid(),
                    FromDeviceId = d1,
                    ToDeviceId = d2,
                    Label = "link",
                },
            },
        };

        var devices = new Dictionary<Guid, DeviceDto>
        {
            [d1] = MakeDevice(d1),
            [d2] = MakeDevice(d2),
        };

        var cut = Render<MiniTopology>(p => p
            .Add(c => c.Group, group)
            .Add(c => c.Devices, devices));

        var lineCount = Regex.Matches(cut.Markup, "<line ").Count;
        Assert.Equal(1, lineCount);
    }

    [Fact]
    public void Uses_Layout_entry_when_provided_and_falls_back_for_missing_ids()
    {
        var d1 = Guid.NewGuid();
        var d2 = Guid.NewGuid();

        var group = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new List<Guid> { d1, d2 },
            Connections = new List<DeviceConnectionDto>(),
            Layout = new List<DeviceLayoutEntry>
            {
                new(d1, 0.1, 0.9),
            },
        };

        var devices = new Dictionary<Guid, DeviceDto>
        {
            [d1] = MakeDevice(d1),
            [d2] = MakeDevice(d2),
        };

        var cut = Render<MiniTopology>(p => p
            .Add(c => c.Group, group)
            .Add(c => c.Devices, devices));

        // With W=280, H=110, Pad=18, normalized (0.1, 0.9) projects to:
        //   px = 18 + 0.1 * (280 - 36) = 42.4
        //   py = 18 + 0.9 * (110 - 36) = 84.6
        // The rect is drawn at x = px - 14 = 28.4, y = py - 8 = 76.6.
        var rectMatches = Regex.Matches(
            cut.Markup,
            @"<rect\s[^>]*\bx=""(?<x>[-0-9.]+)""[^>]*\by=""(?<y>[-0-9.]+)""",
            RegexOptions.IgnoreCase);

        Assert.Equal(2, rectMatches.Count);

        var rects = new List<(double X, double Y)>();
        foreach (Match m in rectMatches)
        {
            var x = double.Parse(m.Groups["x"].Value, System.Globalization.CultureInfo.InvariantCulture);
            var y = double.Parse(m.Groups["y"].Value, System.Globalization.CultureInfo.InvariantCulture);
            rects.Add((x, y));
        }

        Assert.Contains(rects, r =>
            Math.Abs(r.X - 28.4) < 0.01 && Math.Abs(r.Y - 76.6) < 0.01);

        // d2 has no Layout entry, so it falls back to auto-layout. Just
        // confirm a second rect exists and isn't sitting on top of d1.
        Assert.Equal(2, rects.Distinct().Count());
    }

    [Fact]
    public void Skips_node_for_unknown_device_id_and_skips_connection_when_endpoint_missing()
    {
        var d1 = Guid.NewGuid();
        var d2 = Guid.NewGuid();
        var dGhost = Guid.NewGuid();

        var group = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new List<Guid> { d1, d2, dGhost },
            Connections = new List<DeviceConnectionDto>
            {
                new()
                {
                    ConnectionId = Guid.NewGuid(),
                    FromDeviceId = d1,
                    ToDeviceId = dGhost,
                    Label = "link",
                },
            },
        };

        // dGhost intentionally absent from the device dictionary.
        var devices = new Dictionary<Guid, DeviceDto>
        {
            [d1] = MakeDevice(d1),
            [d2] = MakeDevice(d2),
        };

        var cut = Render<MiniTopology>(p => p
            .Add(c => c.Group, group)
            .Add(c => c.Devices, devices));

        var rectCount = Regex.Matches(cut.Markup, "<rect ").Count;
        Assert.Equal(2, rectCount);

        var lineCount = Regex.Matches(cut.Markup, "<line ").Count;
        Assert.Equal(0, lineCount);
    }
}
