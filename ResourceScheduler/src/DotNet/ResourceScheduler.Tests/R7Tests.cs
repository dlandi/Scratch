using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R7: a Device-Group with zero members cannot be activated.</summary>
public class R7Tests
{
    [Fact]
    public async Task Activate_succeeds_when_group_has_at_least_one_member()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Solo Group", new[] { d.DeviceId });

        var activated = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);
        Assert.Equal(DeviceGroupStatus.Active, activated.Status);
    }

    [Fact]
    public async Task Activate_rejects_when_group_is_empty()
    {
        var svc = TestFixture.NewService();
        var grp = await TestFixture.NewGroupAsync(svc, "Empty Group", Array.Empty<Guid>());

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version));

        Assert.Equal("R7", ex.RuleId);
        Assert.Contains("Empty Group", ex.Message);
    }
}
