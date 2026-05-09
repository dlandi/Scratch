// Note on project layout: InMemoryClientService.cs lives in the
// ResourceScheduler.Components Razor class library rather than the
// Blazor WASM host project. The implementation was moved out of the
// host so this xUnit project can reference it without dragging in the
// Microsoft.NET.Sdk.BlazorWebAssembly SDK, which has runtime test
// loading issues on net10.0. See InMemoryClientService.cs header for
// the same note. Tests construct a fresh service per case and build
// their fixtures through IClientService rather than relying on Seed().

using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;

namespace ResourceScheduler.Tests;

/// <summary>
/// Per-test fixture helpers. Each test should new-up a fresh
/// <see cref="InMemoryClientService"/> via <see cref="NewService"/>
/// so the seed data is isolated and predictable.
/// </summary>
internal static class TestFixture
{
    public static InMemoryClientService NewService() => new();

    public static async Task<BuildingDto> NewBuildingAsync(IClientService svc, string name = "Test Building")
        => await svc.CreateBuildingAsync(new BuildingCreate(name, "123 Test Lane"));

    public static async Task<DeviceDto> NewDeviceAsync(
        IClientService svc, Guid buildingId, string name = "DEV-X",
        DeviceStatus status = DeviceStatus.Available)
        => await svc.CreateDeviceAsync(new DeviceCreate(name, status, buildingId));

    public static async Task<DeviceGroupDto> NewGroupAsync(
        IClientService svc, string name, IReadOnlyList<Guid> deviceIds,
        IReadOnlyList<DeviceConnectionDto>? connections = null)
        => await svc.CreateDeviceGroupAsync(new DeviceGroupCreate(
            name, deviceIds, connections ?? Array.Empty<DeviceConnectionDto>(),
            Array.Empty<DeviceLayoutEntry>()));

    public static async Task<PersonDto> NewPersonAsync(IClientService svc, string name = "Person X")
        => await svc.CreatePersonAsync(new PersonCreate(name, null));

    public static async Task<TestGroupDto> NewTestGroupAsync(
        IClientService svc, string name, IReadOnlyList<Guid> memberIds)
        => await svc.CreateTestGroupAsync(new TestGroupCreate(name, memberIds));

    public static async Task<ReservationDto> NewReservationAsync(
        IClientService svc, Guid groupId, Guid teamId, DateTime startUtc, DateTime endUtc)
        => await svc.CreateReservationAsync(new ReservationCreate(groupId, teamId, startUtc, endUtc, null));

    /// <summary>
    /// A Utc-anchored timestamp at a fixed offset from a known epoch, so
    /// reservation tests do not race the wall clock.
    /// </summary>
    public static DateTime At(int hours, int minutes = 0)
        => new DateTime(2030, 1, 1, hours, minutes, 0, DateTimeKind.Utc);
}
