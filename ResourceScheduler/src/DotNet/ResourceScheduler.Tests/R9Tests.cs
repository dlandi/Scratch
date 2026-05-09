using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R9: a reservation can only be confirmed against an Active Device-Group.</summary>
public class R9Tests
{
    [Fact]
    public async Task Confirm_succeeds_when_target_group_is_Active()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Active Group", new[] { d.DeviceId });
        grp = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);

        var person = await TestFixture.NewPersonAsync(svc, "Tester");
        var team = await TestFixture.NewTestGroupAsync(svc, "Team", new[] { person.PersonId });

        var resv = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, team.TestGroupId,
            TestFixture.At(10), TestFixture.At(11));

        var confirmed = await svc.ConfirmReservationAsync(resv.ReservationId, resv.Version);
        Assert.Equal(ReservationStatus.Confirmed, confirmed.Status);
    }

    [Fact]
    public async Task Confirm_rejects_when_target_group_is_Inactive()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        // Group is created Inactive and never activated.
        var grp = await TestFixture.NewGroupAsync(svc, "Draft Group", new[] { d.DeviceId });

        var person = await TestFixture.NewPersonAsync(svc, "Tester");
        var team = await TestFixture.NewTestGroupAsync(svc, "Team", new[] { person.PersonId });

        var resv = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, team.TestGroupId,
            TestFixture.At(10), TestFixture.At(11));

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ConfirmReservationAsync(resv.ReservationId, resv.Version));

        Assert.Equal("R9", ex.RuleId);
        Assert.Contains("Draft Group", ex.Message);
    }
}
