using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Components.Services;

/// <summary>
/// Single seam between the UI and any backend. Phase 1 implements this in
/// memory; Phase 2 swaps in an HTTP client over the Rust API. The contract
/// is shaped to map cleanly to REST. See spec section 8.
/// </summary>
public interface IClientService
{
    // ---- Buildings (spec 4.7) ----
    Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default);
    Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default);
    Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default);
    Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default);
    Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default);

    // ---- Devices (spec 4.1) ----
    Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default);
    Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default);
    Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default);
    Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default);
    Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default);

    // ---- Device-Groups (spec 4.2) ----
    Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default);
    Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default);
    Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default);
    Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default);
    Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);
    Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);
    Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);

    // ---- People (spec 4.4) ----
    Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default);
    Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default);
    Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default);
    Task DeletePersonAsync(Guid id, CancellationToken ct = default);

    // ---- Test-Groups (spec 4.5) ----
    Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default);
    Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default);
    Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default);
    Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default);

    // ---- Reservations (spec 4.6) ----
    Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default);
    Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default);
    Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default);
    Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default);
}
