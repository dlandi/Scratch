using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;

namespace ResourceScheduler.UI.Tests.Fakes;

/// <summary>
/// Test double for <see cref="IClientService"/>. Every method throws
/// <see cref="NotImplementedException"/> by default; tests override only
/// the handlers they care about. Inputs to Create/Update calls are
/// recorded so tests can assert against them.
/// </summary>
public sealed class FakeClientService : IClientService
{
    public Func<BuildingCreate, Task<BuildingDto>>? OnCreateBuilding;
    public Func<Guid, BuildingUpdate, int, Task<BuildingDto>>? OnUpdateBuilding;
    public Func<DeviceCreate, Task<DeviceDto>>? OnCreateDevice;
    public Func<Guid, DeviceUpdate, int, Task<DeviceDto>>? OnUpdateDevice;
    public Func<PersonCreate, Task<PersonDto>>? OnCreatePerson;
    public Func<Guid, PersonUpdate, Task<PersonDto>>? OnUpdatePerson;

    public List<BuildingCreate> RecordedBuildingCreates { get; } = new();
    public List<DeviceCreate> RecordedDeviceCreates { get; } = new();
    public List<PersonCreate> RecordedPersonCreates { get; } = new();

    /// <summary>
    /// Returns a fake whose Create methods echo a default DTO with a
    /// freshly generated id and Version=1. Tests that don't care about
    /// the returned shape can use this as a one-liner default.
    /// </summary>
    public static FakeClientService WithStubs()
    {
        var fake = new FakeClientService();
        fake.OnCreateBuilding = input => Task.FromResult(new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = input.Name,
            Address = input.Address,
            Version = 1,
        });
        fake.OnUpdateBuilding = (id, input, version) => Task.FromResult(new BuildingDto
        {
            BuildingId = id,
            Name = input.Name,
            Address = input.Address,
            Version = version + 1,
        });
        fake.OnCreateDevice = input => Task.FromResult(new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = input.Name,
            Status = input.Status,
            BuildingId = input.BuildingId,
            Version = 1,
        });
        fake.OnUpdateDevice = (id, input, version) => Task.FromResult(new DeviceDto
        {
            DeviceId = id,
            Name = input.Name,
            Status = input.Status,
            BuildingId = input.BuildingId,
            Version = version + 1,
        });
        fake.OnCreatePerson = input => Task.FromResult(new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = input.Name,
            Email = input.Email,
        });
        fake.OnUpdatePerson = (id, input) => Task.FromResult(new PersonDto
        {
            PersonId = id,
            Name = input.Name,
            Email = input.Email,
        });
        return fake;
    }

    // ---- Buildings ----

    public Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default)
    {
        RecordedBuildingCreates.Add(input);
        if (OnCreateBuilding is null)
        {
            throw new NotImplementedException();
        }
        return OnCreateBuilding(input);
    }

    public Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default)
    {
        if (OnUpdateBuilding is null)
        {
            throw new NotImplementedException();
        }
        return OnUpdateBuilding(id, input, version);
    }

    public Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    // ---- Devices ----

    public Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default)
    {
        RecordedDeviceCreates.Add(input);
        if (OnCreateDevice is null)
        {
            throw new NotImplementedException();
        }
        return OnCreateDevice(input);
    }

    public Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default)
    {
        if (OnUpdateDevice is null)
        {
            throw new NotImplementedException();
        }
        return OnUpdateDevice(id, input, version);
    }

    public Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    // ---- Device-Groups ----

    public Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    // ---- People ----

    public Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default)
    {
        RecordedPersonCreates.Add(input);
        if (OnCreatePerson is null)
        {
            throw new NotImplementedException();
        }
        return OnCreatePerson(input);
    }

    public Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default)
    {
        if (OnUpdatePerson is null)
        {
            throw new NotImplementedException();
        }
        return OnUpdatePerson(id, input);
    }

    public Task DeletePersonAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    // ---- Test-Groups ----

    public Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    // ---- Reservations ----

    public Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default)
        => throw new NotImplementedException();
}
