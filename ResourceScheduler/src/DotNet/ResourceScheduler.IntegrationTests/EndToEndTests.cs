using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;

namespace ResourceScheduler.IntegrationTests;

/// <summary>
/// End-to-end smoke tests against the real Rust binary, driven through
/// the production C# RemoteClientService. The Rust router-level tests
/// already cover server-side rule semantics; these tests prove the
/// wire format is correct on both sides and that the C# client's typed
/// exceptions surface from real HTTP responses.
/// </summary>
public sealed class EndToEndTests : IClassFixture<RustServerFixture>
{
    private readonly IClientService _client;

    public EndToEndTests(RustServerFixture fixture)
    {
        var http = new HttpClient { BaseAddress = new Uri(fixture.BaseUrl) };
        _client = new RemoteClientService(http);
    }

    [Fact]
    public async Task Buildings_round_trip_create_list_update_delete()
    {
        var created = await _client.CreateBuildingAsync(
            new BuildingCreate("Lab North", "123 Lab St"));
        Assert.NotEqual(Guid.Empty, created.BuildingId);
        Assert.Equal("Lab North", created.Name);
        Assert.Equal(1, created.Version);

        var listed = await _client.ListBuildingsAsync();
        Assert.Contains(listed, b => b.BuildingId == created.BuildingId);

        var updated = await _client.UpdateBuildingAsync(
            created.BuildingId,
            new BuildingUpdate("Lab North 2", "456 Lab St"),
            created.Version);
        Assert.Equal("Lab North 2", updated.Name);
        Assert.Equal(2, updated.Version);

        await _client.DeleteBuildingAsync(updated.BuildingId, updated.Version);

        var afterDelete = await _client.GetBuildingAsync(created.BuildingId);
        Assert.Null(afterDelete);
    }

    [Fact]
    public async Task DeleteBuilding_with_devices_throws_R15()
    {
        var b = await _client.CreateBuildingAsync(new BuildingCreate("Lab R15", "x"));
        await _client.CreateDeviceAsync(
            new DeviceCreate("Probe", DeviceStatus.Available, b.BuildingId));

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _client.DeleteBuildingAsync(b.BuildingId, b.Version));
        Assert.Equal("R15", ex.RuleId);
    }

    [Fact]
    public async Task CreateDevice_with_unknown_building_throws_R14()
    {
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _client.CreateDeviceAsync(new DeviceCreate(
                "Lonely Probe",
                DeviceStatus.Available,
                Guid.NewGuid())));
        Assert.Equal("R14", ex.RuleId);
    }

    [Fact]
    public async Task UpdatePerson_without_If_Match_succeeds()
    {
        var p = await _client.CreatePersonAsync(new PersonCreate("Alice", null));
        var updated = await _client.UpdatePersonAsync(
            p.PersonId,
            new PersonUpdate("Alicia", "alicia@example"));
        Assert.Equal("Alicia", updated.Name);
        Assert.Equal("alicia@example", updated.Email);
    }

    [Fact]
    public async Task ActivateDeviceGroup_updates_member_assigned_pointer()
    {
        var b = await _client.CreateBuildingAsync(new BuildingCreate("Lab Act", "x"));
        var d = await _client.CreateDeviceAsync(
            new DeviceCreate("D", DeviceStatus.Available, b.BuildingId));

        var g = await _client.CreateDeviceGroupAsync(new DeviceGroupCreate(
            "G",
            new[] { d.DeviceId },
            Array.Empty<DeviceConnectionDto>(),
            Array.Empty<DeviceLayoutEntry>()));

        var activated = await _client.ActivateDeviceGroupAsync(g.DeviceGroupId, g.Version);
        Assert.Equal(DeviceGroupStatus.Active, activated.Status);

        var refreshed = await _client.GetDeviceAsync(d.DeviceId);
        Assert.NotNull(refreshed);
        Assert.Equal(g.DeviceGroupId, refreshed!.AssignedDeviceGroupId);
    }

    [Fact]
    public async Task CreateReservation_with_end_at_or_before_start_throws_R13()
    {
        var (groupId, testGroupId) = await BuildActiveWorld();

        var t = new DateTime(2026, 5, 9, 12, 0, 0, DateTimeKind.Utc);
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _client.CreateReservationAsync(new ReservationCreate(
                groupId, testGroupId, t, t, null)));
        Assert.Equal("R13", ex.RuleId);
    }

    [Fact]
    public async Task ConfirmReservation_overlap_on_same_group_throws_R10()
    {
        var (groupId, teamA) = await BuildActiveWorld();
        var teamB = await _client.CreateTestGroupAsync(
            new TestGroupCreate("Team B", Array.Empty<Guid>()));

        var r1 = await _client.CreateReservationAsync(new ReservationCreate(
            groupId, teamA,
            new DateTime(2026, 5, 9, 12, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 5, 9, 13, 0, 0, DateTimeKind.Utc),
            null));
        await _client.ConfirmReservationAsync(r1.ReservationId, r1.Version);

        var r2 = await _client.CreateReservationAsync(new ReservationCreate(
            groupId, teamB.TestGroupId,
            new DateTime(2026, 5, 9, 12, 30, 0, DateTimeKind.Utc),
            new DateTime(2026, 5, 9, 13, 30, 0, DateTimeKind.Utc),
            null));

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            _client.ConfirmReservationAsync(r2.ReservationId, r2.Version));
        Assert.Equal("R10", ex.RuleId);
    }

    [Fact]
    public async Task ListReservations_filter_with_repeated_statusIn_round_trips()
    {
        var (groupId, teamId) = await BuildActiveWorld();

        var pending = await _client.CreateReservationAsync(new ReservationCreate(
            groupId, teamId,
            new DateTime(2026, 5, 9, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 5, 9, 9, 0, 0, DateTimeKind.Utc),
            null));

        var toConfirm = await _client.CreateReservationAsync(new ReservationCreate(
            groupId, teamId,
            new DateTime(2026, 5, 9, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 5, 9, 11, 0, 0, DateTimeKind.Utc),
            null));
        await _client.ConfirmReservationAsync(toConfirm.ReservationId, toConfirm.Version);

        var toCancel = await _client.CreateReservationAsync(new ReservationCreate(
            groupId, teamId,
            new DateTime(2026, 5, 9, 14, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 5, 9, 15, 0, 0, DateTimeKind.Utc),
            null));
        await _client.CancelReservationAsync(toCancel.ReservationId, toCancel.Version);

        var filtered = await _client.ListReservationsAsync(new ReservationFilter(
            DeviceGroupId: groupId,
            StatusIn: new[] { ReservationStatus.Pending, ReservationStatus.Confirmed }));

        var ids = filtered.Select(r => r.ReservationId).ToHashSet();
        Assert.Contains(pending.ReservationId, ids);
        Assert.Contains(toConfirm.ReservationId, ids);
        Assert.DoesNotContain(toCancel.ReservationId, ids);
    }

    /// <summary>
    /// Helper that creates a building, an Available device, an Active
    /// device-group containing that device, and a fresh Test-Group.
    /// Returns (deviceGroupId, testGroupId).
    /// </summary>
    private async Task<(Guid DeviceGroupId, Guid TestGroupId)> BuildActiveWorld()
    {
        var b = await _client.CreateBuildingAsync(new BuildingCreate("B", "x"));
        var d = await _client.CreateDeviceAsync(
            new DeviceCreate("D", DeviceStatus.Available, b.BuildingId));
        var g = await _client.CreateDeviceGroupAsync(new DeviceGroupCreate(
            "G",
            new[] { d.DeviceId },
            Array.Empty<DeviceConnectionDto>(),
            Array.Empty<DeviceLayoutEntry>()));
        var activated = await _client.ActivateDeviceGroupAsync(g.DeviceGroupId, g.Version);
        var team = await _client.CreateTestGroupAsync(
            new TestGroupCreate("Team", Array.Empty<Guid>()));
        return (activated.DeviceGroupId, team.TestGroupId);
    }
}
