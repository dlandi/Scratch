using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>R10: two Confirmed reservations on the same Device-Group cannot overlap.</summary>
public class R10Tests
{
    [Fact]
    public async Task Confirm_succeeds_when_reservations_on_same_group_do_not_overlap()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Group", new[] { d.DeviceId });
        grp = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);

        var pA = await TestFixture.NewPersonAsync(svc, "A");
        var pB = await TestFixture.NewPersonAsync(svc, "B");
        var teamA = await TestFixture.NewTestGroupAsync(svc, "Team A", new[] { pA.PersonId });
        var teamB = await TestFixture.NewTestGroupAsync(svc, "Team B", new[] { pB.PersonId });

        var first = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamA.TestGroupId,
            TestFixture.At(9), TestFixture.At(11));
        first = await svc.ConfirmReservationAsync(first.ReservationId, first.Version);

        // Second slot starts exactly when the first ends, so windows do not overlap.
        var second = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamB.TestGroupId,
            TestFixture.At(11), TestFixture.At(13));
        var confirmed = await svc.ConfirmReservationAsync(second.ReservationId, second.Version);

        Assert.Equal(ReservationStatus.Confirmed, confirmed.Status);
        Assert.Equal(ReservationStatus.Confirmed, first.Status);
    }

    [Fact]
    public async Task Confirm_rejects_when_reservations_on_same_group_overlap()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Group Overlap", new[] { d.DeviceId });
        grp = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);

        var pA = await TestFixture.NewPersonAsync(svc, "A");
        var pB = await TestFixture.NewPersonAsync(svc, "B");
        var teamA = await TestFixture.NewTestGroupAsync(svc, "Team A", new[] { pA.PersonId });
        var teamB = await TestFixture.NewTestGroupAsync(svc, "Team B", new[] { pB.PersonId });

        var first = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamA.TestGroupId,
            TestFixture.At(9), TestFixture.At(12));
        first = await svc.ConfirmReservationAsync(first.ReservationId, first.Version);

        var second = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamB.TestGroupId,
            TestFixture.At(11), TestFixture.At(13));

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => svc.ConfirmReservationAsync(second.ReservationId, second.Version));

        Assert.Equal("R10", ex.RuleId);
        Assert.Contains("Group Overlap", ex.Message);
    }

    [Fact]
    public async Task Confirm_ignores_non_Confirmed_reservations_when_checking_overlap()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Group", new[] { d.DeviceId });
        grp = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);

        var pA = await TestFixture.NewPersonAsync(svc, "A");
        var pB = await TestFixture.NewPersonAsync(svc, "B");
        var teamA = await TestFixture.NewTestGroupAsync(svc, "Team A", new[] { pA.PersonId });
        var teamB = await TestFixture.NewTestGroupAsync(svc, "Team B", new[] { pB.PersonId });

        // Pending overlapping reservation: should not block.
        await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamA.TestGroupId,
            TestFixture.At(9), TestFixture.At(12));

        var second = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, teamB.TestGroupId,
            TestFixture.At(11), TestFixture.At(13));
        var confirmed = await svc.ConfirmReservationAsync(second.ReservationId, second.Version);

        Assert.Equal(ReservationStatus.Confirmed, confirmed.Status);
    }
}
