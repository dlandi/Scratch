using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>
/// Verifies that DeviceGroup Layout entries (normalized [0..1] positions
/// authored in the Designer) round-trip through the IClientService.
/// </summary>
public class LayoutRoundTripTests
{
    [Fact]
    public async Task Layout_entries_are_preserved_through_update_and_read()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");
        var d3 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-3");

        var grp = await TestFixture.NewGroupAsync(
            svc, "Layout Group",
            new[] { d1.DeviceId, d2.DeviceId, d3.DeviceId });

        var layout = new List<DeviceLayoutEntry>
        {
            new(d1.DeviceId, 0.10, 0.20),
            new(d2.DeviceId, 0.55, 0.65),
            new(d3.DeviceId, 0.90, 0.45),
        };

        var update = new DeviceGroupUpdate(
            grp.Name,
            grp.DeviceIds,
            grp.Connections,
            layout);

        await svc.UpdateDeviceGroupAsync(grp.DeviceGroupId, update, grp.Version);

        var reloaded = await svc.GetDeviceGroupAsync(grp.DeviceGroupId);
        Assert.NotNull(reloaded);
        Assert.Equal(layout.Count, reloaded!.Layout.Count);

        foreach (var expected in layout)
        {
            var actual = reloaded.Layout.FirstOrDefault(e => e.DeviceId == expected.DeviceId);
            Assert.NotNull(actual);
            Assert.Equal(expected.X, actual!.X, 9);
            Assert.Equal(expected.Y, actual.Y, 9);
        }
    }

    [Fact]
    public async Task Empty_layout_round_trips_as_empty()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");

        var grp = await TestFixture.NewGroupAsync(
            svc, "Empty Layout Group", new[] { d1.DeviceId });

        Assert.Empty(grp.Layout);

        var reloaded = await svc.GetDeviceGroupAsync(grp.DeviceGroupId);
        Assert.NotNull(reloaded);
        Assert.Empty(reloaded!.Layout);
    }
}
