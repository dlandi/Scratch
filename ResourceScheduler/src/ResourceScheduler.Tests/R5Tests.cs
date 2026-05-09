using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R5: no Offline/Maintenance/Retired members in an Active group.</summary>
public class R5Tests
{
    [Fact]
    public async Task Activate_succeeds_when_all_members_are_Available()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var ok1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "OK-1", DeviceStatus.Available);
        var ok2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "OK-2", DeviceStatus.Available);
        var grp = await TestFixture.NewGroupAsync(svc, "OK Group", new[] { ok1.DeviceId, ok2.DeviceId });

        var activated = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);
        Assert.Equal(DeviceGroupStatus.Active, activated.Status);
    }

    [Theory]
    [InlineData(DeviceStatus.Offline)]
    [InlineData(DeviceStatus.Maintenance)]
    [InlineData(DeviceStatus.Retired)]
    public async Task Activate_rejects_when_any_member_is_unavailable(DeviceStatus badStatus)
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var good = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "GOOD-1", DeviceStatus.Available);
        var bad  = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "BAD-9",  badStatus);
        var grp = await TestFixture.NewGroupAsync(svc, "Mixed Group", new[] { good.DeviceId, bad.DeviceId });

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version));

        Assert.Equal("R5", ex.RuleId);
        Assert.Contains("BAD-9", ex.Message);
    }
}
