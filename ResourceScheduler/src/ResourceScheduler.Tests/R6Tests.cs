using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R6: every connection endpoint must be a member of the group.</summary>
public class R6Tests
{
    [Fact]
    public async Task Create_succeeds_when_connections_reference_only_members()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");

        var conn = new DeviceConnectionDto
        {
            ConnectionId = Guid.NewGuid(),
            FromDeviceId = d1.DeviceId,
            ToDeviceId = d2.DeviceId,
            Label = "LINK",
        };
        var grp = await TestFixture.NewGroupAsync(
            svc, "Linked Group",
            new[] { d1.DeviceId, d2.DeviceId },
            new[] { conn });

        Assert.Single(grp.Connections);
    }

    [Fact]
    public async Task Create_rejects_when_FromDeviceId_is_not_a_member()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");
        var orphan = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "ORPHAN");

        var conn = new DeviceConnectionDto
        {
            ConnectionId = Guid.NewGuid(),
            FromDeviceId = orphan.DeviceId,
            ToDeviceId = d1.DeviceId,
            Label = "BAD",
        };

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            svc.CreateDeviceGroupAsync(new DeviceGroupCreate(
                "Bad Group",
                new[] { d1.DeviceId, d2.DeviceId },
                new[] { conn },
                Array.Empty<DeviceLayoutEntry>())));

        Assert.Equal("R6", ex.RuleId);
        Assert.Contains("Bad Group", ex.Message);
    }

    [Fact]
    public async Task Update_rejects_when_ToDeviceId_is_not_a_member()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");
        var orphan = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "ORPHAN");

        var grp = await TestFixture.NewGroupAsync(svc, "Group X", new[] { d1.DeviceId, d2.DeviceId });

        var conn = new DeviceConnectionDto
        {
            ConnectionId = Guid.NewGuid(),
            FromDeviceId = d1.DeviceId,
            ToDeviceId = orphan.DeviceId,
            Label = "BAD",
        };

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            svc.UpdateDeviceGroupAsync(
                grp.DeviceGroupId,
                new DeviceGroupUpdate(
                    "Group X",
                    new[] { d1.DeviceId, d2.DeviceId },
                    new[] { conn },
                    Array.Empty<DeviceLayoutEntry>()),
                grp.Version));

        Assert.Equal("R6", ex.RuleId);
    }
}
