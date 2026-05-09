using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R3: cannot activate a group whose member is in another active group.</summary>
public class R3Tests
{
    [Fact]
    public async Task Activate_succeeds_when_no_member_is_in_another_active_group()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");

        var groupA = await TestFixture.NewGroupAsync(svc, "Group A", new[] { d1.DeviceId });
        var groupB = await TestFixture.NewGroupAsync(svc, "Group B", new[] { d2.DeviceId });

        await svc.ActivateDeviceGroupAsync(groupA.DeviceGroupId, groupA.Version);
        var activatedB = await svc.ActivateDeviceGroupAsync(groupB.DeviceGroupId, groupB.Version);

        Assert.Equal(DeviceGroupStatus.Active, activatedB.Status);
    }

    [Fact]
    public async Task Activate_rejects_when_member_is_in_another_active_group()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var shared = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "SHARED-1");

        var groupA = await TestFixture.NewGroupAsync(svc, "Group A", new[] { shared.DeviceId });
        var groupB = await TestFixture.NewGroupAsync(svc, "Group B", new[] { shared.DeviceId });

        await svc.ActivateDeviceGroupAsync(groupA.DeviceGroupId, groupA.Version);

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ActivateDeviceGroupAsync(groupB.DeviceGroupId, groupB.Version));

        Assert.Equal("R3", ex.RuleId);
        Assert.Contains("SHARED-1", ex.Message);
    }
}
