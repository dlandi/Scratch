using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R11: two Confirmed reservations on the same Test-Group cannot overlap.</summary>
public class R11Tests
{
    [Fact]
    public async Task Confirm_succeeds_when_same_team_books_non_overlapping_groups()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");

        var grp1 = await TestFixture.NewGroupAsync(svc, "G1", new[] { d1.DeviceId });
        var grp2 = await TestFixture.NewGroupAsync(svc, "G2", new[] { d2.DeviceId });
        grp1 = await svc.ActivateDeviceGroupAsync(grp1.DeviceGroupId, grp1.Version);
        grp2 = await svc.ActivateDeviceGroupAsync(grp2.DeviceGroupId, grp2.Version);

        var person = await TestFixture.NewPersonAsync(svc, "Tester");
        var team = await TestFixture.NewTestGroupAsync(svc, "Solo Team", new[] { person.PersonId });

        var first = await TestFixture.NewReservationAsync(
            svc, grp1.DeviceGroupId, team.TestGroupId,
            TestFixture.At(9), TestFixture.At(11));
        first = await svc.ConfirmReservationAsync(first.ReservationId, first.Version);

        var second = await TestFixture.NewReservationAsync(
            svc, grp2.DeviceGroupId, team.TestGroupId,
            TestFixture.At(11), TestFixture.At(13));
        var confirmed = await svc.ConfirmReservationAsync(second.ReservationId, second.Version);

        Assert.Equal(ReservationStatus.Confirmed, confirmed.Status);
    }

    [Fact]
    public async Task Confirm_rejects_when_same_team_has_overlapping_confirmed_booking()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d1 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var d2 = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-2");

        var grp1 = await TestFixture.NewGroupAsync(svc, "G1", new[] { d1.DeviceId });
        var grp2 = await TestFixture.NewGroupAsync(svc, "G2", new[] { d2.DeviceId });
        grp1 = await svc.ActivateDeviceGroupAsync(grp1.DeviceGroupId, grp1.Version);
        grp2 = await svc.ActivateDeviceGroupAsync(grp2.DeviceGroupId, grp2.Version);

        var person = await TestFixture.NewPersonAsync(svc, "Tester");
        var team = await TestFixture.NewTestGroupAsync(svc, "Busy Team", new[] { person.PersonId });

        var first = await TestFixture.NewReservationAsync(
            svc, grp1.DeviceGroupId, team.TestGroupId,
            TestFixture.At(9), TestFixture.At(12));
        first = await svc.ConfirmReservationAsync(first.ReservationId, first.Version);

        var second = await TestFixture.NewReservationAsync(
            svc, grp2.DeviceGroupId, team.TestGroupId,
            TestFixture.At(11), TestFixture.At(13));

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ConfirmReservationAsync(second.ReservationId, second.Version));

        Assert.Equal("R11", ex.RuleId);
        Assert.Contains("Busy Team", ex.Message);
    }
}
