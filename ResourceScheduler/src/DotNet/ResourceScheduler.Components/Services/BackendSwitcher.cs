using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Components.Services;

/// <summary>
/// Decorating <see cref="IClientService"/> that forwards every call to
/// either the in-memory simulator or the HTTP-backed Rust client based
/// on a runtime <see cref="Mode"/>. Both concrete services live as
/// singletons; this switcher just chooses which one each call hits.
///
/// Mirrors the <c>UserTimeProvider</c> pattern: components that care
/// about the active backend subscribe to <see cref="Changed"/> in
/// <c>OnInitialized</c> and unsubscribe in <c>Dispose</c>, then call
/// <see cref="StateHasChanged"/> on the event to re-fetch.
///
/// Switching backends does NOT migrate data; the two stores are
/// independent. Pages already re-fetch on navigation, so the visible
/// effect of a switch is "the next list call goes to the new backend."
/// </summary>
public sealed class BackendSwitcher : IClientService
{
    private readonly InMemoryClientService _inMemory;
    private readonly RemoteClientService _remote;

    public BackendSwitcher(InMemoryClientService inMemory, RemoteClientService remote)
    {
        _inMemory = inMemory;
        _remote = remote;
    }

    /// <summary>The currently active backend. Defaults to <see cref="BackendMode.InMemory"/>.</summary>
    public BackendMode Mode { get; private set; } = BackendMode.InMemory;

    /// <summary>Fired when <see cref="SetMode"/> changes the active backend.</summary>
    public event Action? Changed;

    /// <summary>Replace the active backend. No-op if the value is unchanged.</summary>
    public void SetMode(BackendMode mode)
    {
        if (mode == Mode) return;
        Mode = mode;
        Changed?.Invoke();
    }

    /// <summary>Resolves to the concrete service for the current <see cref="Mode"/>.</summary>
    private IClientService Current => Mode switch
    {
        BackendMode.InMemory => _inMemory,
        BackendMode.Rust     => _remote,
        _ => throw new InvalidOperationException($"Unknown backend mode: {Mode}"),
    };

    // ---- Buildings ----
    public Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default) =>
        Current.ListBuildingsAsync(ct);
    public Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default) =>
        Current.GetBuildingAsync(id, ct);
    public Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default) =>
        Current.CreateBuildingAsync(input, ct);
    public Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default) =>
        Current.UpdateBuildingAsync(id, input, version, ct);
    public Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.DeleteBuildingAsync(id, version, ct);

    // ---- Devices ----
    public Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default) =>
        Current.ListDevicesAsync(ct);
    public Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default) =>
        Current.GetDeviceAsync(id, ct);
    public Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default) =>
        Current.CreateDeviceAsync(input, ct);
    public Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default) =>
        Current.UpdateDeviceAsync(id, input, version, ct);
    public Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.DeleteDeviceAsync(id, version, ct);

    // ---- Device-Groups ----
    public Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default) =>
        Current.ListDeviceGroupsAsync(ct);
    public Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default) =>
        Current.GetDeviceGroupAsync(id, ct);
    public Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default) =>
        Current.CreateDeviceGroupAsync(input, ct);
    public Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default) =>
        Current.UpdateDeviceGroupAsync(id, input, version, ct);
    public Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.ActivateDeviceGroupAsync(id, version, ct);
    public Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.DeactivateDeviceGroupAsync(id, version, ct);
    public Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.DeleteDeviceGroupAsync(id, version, ct);

    // ---- People ----
    public Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default) =>
        Current.ListPeopleAsync(ct);
    public Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default) =>
        Current.CreatePersonAsync(input, ct);
    public Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default) =>
        Current.UpdatePersonAsync(id, input, ct);
    public Task DeletePersonAsync(Guid id, CancellationToken ct = default) =>
        Current.DeletePersonAsync(id, ct);

    // ---- Test-Groups ----
    public Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default) =>
        Current.ListTestGroupsAsync(ct);
    public Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default) =>
        Current.CreateTestGroupAsync(input, ct);
    public Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default) =>
        Current.UpdateTestGroupAsync(id, input, version, ct);
    public Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.DeleteTestGroupAsync(id, version, ct);

    // ---- Reservations ----
    public Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default) =>
        Current.ListReservationsAsync(filter, ct);
    public Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default) =>
        Current.CreateReservationAsync(input, ct);
    public Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.ConfirmReservationAsync(id, version, ct);
    public Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default) =>
        Current.CancelReservationAsync(id, version, ct);
}
