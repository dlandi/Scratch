using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Tests;

/// <summary>
/// Optimistic concurrency: writes that supply a stale Version fail with
/// <see cref="ConflictException"/>; successful writes increment Version.
/// </summary>
public class ConcurrencyTests
{
    [Fact]
    public async Task Building_update_increments_version_on_success()
    {
        var svc = TestFixture.NewService();
        var b = await svc.CreateBuildingAsync(new BuildingCreate("B1", "addr"));
        Assert.Equal(1, b.Version);

        var updated = await svc.UpdateBuildingAsync(
            b.BuildingId, new BuildingUpdate("B1-renamed", "addr"), b.Version);
        Assert.Equal(2, updated.Version);
    }

    [Fact]
    public async Task Building_update_with_stale_version_throws_ConflictException()
    {
        var svc = TestFixture.NewService();
        var b = await svc.CreateBuildingAsync(new BuildingCreate("B1", "addr"));
        await svc.UpdateBuildingAsync(b.BuildingId, new BuildingUpdate("B1a", "addr"), b.Version);

        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            svc.UpdateBuildingAsync(b.BuildingId, new BuildingUpdate("B1b", "addr"), b.Version));

        Assert.Contains("Building", ex.Message);
    }

    [Fact]
    public async Task Reservation_confirm_with_stale_version_throws_ConflictException()
    {
        var svc = TestFixture.NewService();
        var bldg = await TestFixture.NewBuildingAsync(svc);
        var d = await TestFixture.NewDeviceAsync(svc, bldg.BuildingId, "D-1");
        var grp = await TestFixture.NewGroupAsync(svc, "Group", new[] { d.DeviceId });
        grp = await svc.ActivateDeviceGroupAsync(grp.DeviceGroupId, grp.Version);

        var p = await TestFixture.NewPersonAsync(svc, "P");
        var team = await TestFixture.NewTestGroupAsync(svc, "Team", new[] { p.PersonId });

        var r = await TestFixture.NewReservationAsync(
            svc, grp.DeviceGroupId, team.TestGroupId,
            TestFixture.At(9), TestFixture.At(10));

        await svc.CancelReservationAsync(r.ReservationId, r.Version);

        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            svc.ConfirmReservationAsync(r.ReservationId, r.Version));

        Assert.Contains("Reservation", ex.Message);
    }
}
