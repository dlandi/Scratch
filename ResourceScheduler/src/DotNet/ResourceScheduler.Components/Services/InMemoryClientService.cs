using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using ResourceScheduler.Components.Models;

namespace ResourceScheduler.Components.Services;

/// <summary>
/// Phase 1 in-memory implementation of <see cref="IClientService"/>.
/// Storage is per-process; the Blazor WASM host runs single-threaded so
/// <see cref="ConcurrentDictionary{TKey,TValue}"/> is overkill but matches
/// the shape we want when Phase 2 swaps in an HTTP client.
///
/// This type lives in the Razor class library rather than the WASM host
/// project so xUnit tests can reference it without picking up the Blazor
/// WebAssembly SDK.
///
/// See Docs/SPECIFICATION.md sections 5 and 10 for the rule catalog.
/// </summary>
public sealed class InMemoryClientService : IClientService
{
    private readonly ConcurrentDictionary<Guid, BuildingDto> _buildings = new();
    private readonly ConcurrentDictionary<Guid, DeviceDto> _devices = new();
    private readonly ConcurrentDictionary<Guid, DeviceGroupDto> _deviceGroups = new();
    private readonly ConcurrentDictionary<Guid, PersonDto> _people = new();
    private readonly ConcurrentDictionary<Guid, TestGroupDto> _testGroups = new();
    private readonly ConcurrentDictionary<Guid, ReservationDto> _reservations = new();

    private readonly TimeProvider _time;

    /// <summary>
    /// Default constructor used by the existing test fixture. Hands
    /// <see cref="TimeProvider.System"/> to the parameterised overload.
    /// </summary>
    public InMemoryClientService() : this(TimeProvider.System) { }

    /// <summary>
    /// Production constructor. Blazor's DI container resolves this
    /// overload because <see cref="TimeProvider"/> is registered in
    /// <c>Program.cs</c>. Tests that want deterministic time can pass a
    /// <c>FakeTimeProvider</c> here.
    /// </summary>
    public InMemoryClientService(TimeProvider time)
    {
        _time = time;
        Seed();
    }

    // ============================================================
    // Buildings
    // ============================================================

    public Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<BuildingDto>>(_buildings.Values.OrderBy(b => b.Name).Select(Clone).ToList());

    public Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_buildings.TryGetValue(id, out var b) ? Clone(b) : null);

    public Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default)
    {
        var b = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = input.Name,
            Address = input.Address,
            Version = 1,
        };
        _buildings[b.BuildingId] = b;
        return Task.FromResult(Clone(b));
    }

    public Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default)
    {
        var b = Require(_buildings, id, "Building");
        EnsureVersion(b.Version, version, "Building");
        b.Name = input.Name;
        b.Address = input.Address;
        b.Version++;
        return Task.FromResult(Clone(b));
    }

    public Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default)
    {
        var b = Require(_buildings, id, "Building");
        EnsureVersion(b.Version, version, "Building");
        // R15: cannot delete while any Device references it.
        if (_devices.Values.Any(d => d.BuildingId == id))
            throw new ValidationException("R15", $"Building '{b.Name}' has devices assigned. Reassign or delete those devices first.");
        _buildings.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    // ============================================================
    // Devices
    // ============================================================

    public Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<DeviceDto>>(
            _devices.Values.OrderBy(d => d.Name).Select(CloneWithDerivedAssignment).ToList());

    public Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_devices.TryGetValue(id, out var d) ? CloneWithDerivedAssignment(d) : null);

    /// <summary>
    /// Returns a copy of the device with <see cref="DeviceDto.AssignedDeviceGroupId"/>
    /// derived from current Active group memberships. The stored field on
    /// the underlying record is only refreshed on Activate, so reading it
    /// directly produces stale values after Deactivate, Update, or Delete
    /// of the owning group; deriving on read closes that gap without
    /// adding write-side maintenance to four call sites. R1 guarantees
    /// at most one Active group per device, so FirstOrDefault is exact.
    /// </summary>
    private DeviceDto CloneWithDerivedAssignment(DeviceDto d)
    {
        var c = Clone(d);
        c.AssignedDeviceGroupId = _deviceGroups.Values
            .Where(g => g.Status == DeviceGroupStatus.Active && g.DeviceIds.Contains(d.DeviceId))
            .Select(g => (Guid?)g.DeviceGroupId)
            .FirstOrDefault();
        return c;
    }

    public Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default)
    {
        // R14: Building must exist.
        if (!_buildings.ContainsKey(input.BuildingId))
            throw new ValidationException("R14", "Device must reference an existing Building.");
        var d = new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = input.Name,
            Status = input.Status,
            BuildingId = input.BuildingId,
            AssignedDeviceGroupId = null,
            Version = 1,
        };
        _devices[d.DeviceId] = d;
        return Task.FromResult(Clone(d));
    }

    public Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default)
    {
        var d = Require(_devices, id, "Device");
        EnsureVersion(d.Version, version, "Device");
        if (!_buildings.ContainsKey(input.BuildingId))
            throw new ValidationException("R14", "Device must reference an existing Building.");
        d.Name = input.Name;
        d.Status = input.Status;
        d.BuildingId = input.BuildingId;
        d.Version++;
        return Task.FromResult(Clone(d));
    }

    public Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default)
    {
        var d = Require(_devices, id, "Device");
        EnsureVersion(d.Version, version, "Device");
        _devices.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    // ============================================================
    // Device-Groups
    // ============================================================

    public Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<DeviceGroupDto>>(_deviceGroups.Values.OrderBy(g => g.Name).Select(Clone).ToList());

    public Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_deviceGroups.TryGetValue(id, out var g) ? Clone(g) : null);

    public Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default)
    {
        // R6: every connection endpoint must be a member of the group.
        ValidateConnectionMembership(input.Name, input.DeviceIds, input.Connections);
        var g = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = input.Name,
            Status = DeviceGroupStatus.Inactive,
            DeviceIds = input.DeviceIds.ToList(),
            Connections = input.Connections.Select(CloneConnection).ToList(),
            Layout = input.Layout.ToList(),
            Version = 1,
        };
        _deviceGroups[g.DeviceGroupId] = g;
        return Task.FromResult(Clone(g));
    }

    public Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default)
    {
        var g = Require(_deviceGroups, id, "DeviceGroup");
        EnsureVersion(g.Version, version, "DeviceGroup");
        // R6: every connection endpoint must be a member of the group.
        ValidateConnectionMembership(input.Name, input.DeviceIds, input.Connections);
        g.Name = input.Name;
        g.DeviceIds = input.DeviceIds.ToList();
        g.Connections = input.Connections.Select(CloneConnection).ToList();
        g.Layout = input.Layout.ToList();
        g.Version++;
        return Task.FromResult(Clone(g));
    }

    public Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        var g = Require(_deviceGroups, id, "DeviceGroup");
        EnsureVersion(g.Version, version, "DeviceGroup");

        // R7: must have at least one member.
        if (g.DeviceIds.Count == 0)
            throw new ValidationException("R7", $"Device-Group '{g.Name}' has no members and cannot be activated.");

        // R5: no member may be Offline, Maintenance, or Retired.
        foreach (var did in g.DeviceIds)
        {
            if (_devices.TryGetValue(did, out var d) &&
                d.Status is DeviceStatus.Offline or DeviceStatus.Maintenance or DeviceStatus.Retired)
            {
                throw new ValidationException(
                    "R5",
                    $"Device-Group '{g.Name}' cannot be activated: member device '{d.Name}' is {d.Status}.");
            }
        }

        // R3: no member device may be in another currently-Active group.
        foreach (var other in _deviceGroups.Values)
        {
            if (other.DeviceGroupId == g.DeviceGroupId) continue;
            if (other.Status != DeviceGroupStatus.Active) continue;
            var clash = g.DeviceIds.FirstOrDefault(other.DeviceIds.Contains);
            if (clash != Guid.Empty)
            {
                var clashName = _devices.TryGetValue(clash, out var cd) ? cd.Name : clash.ToString();
                throw new ValidationException(
                    "R3",
                    $"Device-Group '{g.Name}' cannot be activated: device '{clashName}' is already deployed in active group '{other.Name}'.");
            }
        }

        g.Status = DeviceGroupStatus.Active;
        g.Version++;

        // Refresh the convenience pointer on each member device.
        foreach (var did in g.DeviceIds)
            if (_devices.TryGetValue(did, out var dd))
                dd.AssignedDeviceGroupId = g.DeviceGroupId;

        return Task.FromResult(Clone(g));
    }

    public Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        var g = Require(_deviceGroups, id, "DeviceGroup");
        EnsureVersion(g.Version, version, "DeviceGroup");
        g.Status = DeviceGroupStatus.Inactive;
        g.Version++;
        return Task.FromResult(Clone(g));
    }

    public Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        var g = Require(_deviceGroups, id, "DeviceGroup");
        EnsureVersion(g.Version, version, "DeviceGroup");
        _deviceGroups.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    // ============================================================
    // People
    // ============================================================

    public Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<PersonDto>>(_people.Values.OrderBy(p => p.Name).Select(Clone).ToList());

    public Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default)
    {
        var p = new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = input.Name,
            Email = input.Email,
        };
        _people[p.PersonId] = p;
        return Task.FromResult(Clone(p));
    }

    public Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default)
    {
        var p = Require(_people, id, "Person");
        p.Name = input.Name;
        p.Email = input.Email;
        return Task.FromResult(Clone(p));
    }

    public Task DeletePersonAsync(Guid id, CancellationToken ct = default)
    {
        if (!_people.TryRemove(id, out _))
            throw new NotFoundException($"Person {id} not found.");
        return Task.CompletedTask;
    }

    // ============================================================
    // Test-Groups
    // ============================================================

    public Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<TestGroupDto>>(_testGroups.Values.OrderBy(t => t.Name).Select(Clone).ToList());

    public Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default)
    {
        var t = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = input.Name,
            MemberIds = input.MemberIds.ToList(),
            Version = 1,
        };
        _testGroups[t.TestGroupId] = t;
        return Task.FromResult(Clone(t));
    }

    public Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default)
    {
        var t = Require(_testGroups, id, "TestGroup");
        EnsureVersion(t.Version, version, "TestGroup");
        t.Name = input.Name;
        t.MemberIds = input.MemberIds.ToList();
        t.Version++;
        return Task.FromResult(Clone(t));
    }

    public Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        var t = Require(_testGroups, id, "TestGroup");
        EnsureVersion(t.Version, version, "TestGroup");
        _testGroups.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    // ============================================================
    // Reservations
    // ============================================================

    public Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default)
    {
        IEnumerable<ReservationDto> q = _reservations.Values;
        if (filter is not null)
        {
            if (filter.DeviceGroupId is { } gid) q = q.Where(r => r.DeviceGroupId == gid);
            if (filter.TestGroupId  is { } tid) q = q.Where(r => r.TestGroupId  == tid);
            if (filter.FromUtc is { } from) q = q.Where(r => r.EndUtc   > from);
            if (filter.ToUtc   is { } to)   q = q.Where(r => r.StartUtc < to);
            if (filter.StatusIn is { Count: > 0 } statuses) q = q.Where(r => statuses.Contains(r.Status));
        }
        return Task.FromResult<IReadOnlyList<ReservationDto>>(q.OrderBy(r => r.StartUtc).Select(Clone).ToList());
    }

    public Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default)
    {
        if (input.EndUtc <= input.StartUtc)
            throw new ValidationException("R13", "Reservation end must be after start.");
        if (!_deviceGroups.ContainsKey(input.DeviceGroupId))
            throw new ValidationException("R8", "Reservation references unknown Device-Group.");
        if (!_testGroups.ContainsKey(input.TestGroupId))
            throw new ValidationException("R8", "Reservation references unknown Test-Group.");
        var r = new ReservationDto
        {
            ReservationId = Guid.NewGuid(),
            DeviceGroupId = input.DeviceGroupId,
            TestGroupId = input.TestGroupId,
            StartUtc = DateTime.SpecifyKind(input.StartUtc, DateTimeKind.Utc),
            EndUtc = DateTime.SpecifyKind(input.EndUtc, DateTimeKind.Utc),
            Status = ReservationStatus.Pending,
            Notes = input.Notes,
            Version = 1,
        };
        _reservations[r.ReservationId] = r;
        return Task.FromResult(Clone(r));
    }

    public Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default)
    {
        var r = Require(_reservations, id, "Reservation");
        EnsureVersion(r.Version, version, "Reservation");

        // R9: target Device-Group must be Active.
        if (!_deviceGroups.TryGetValue(r.DeviceGroupId, out var g))
            throw new ValidationException("R9", "Reservation references unknown Device-Group.");
        if (g.Status != DeviceGroupStatus.Active)
            throw new ValidationException(
                "R9",
                $"Reservation cannot be confirmed: Device-Group '{g.Name}' is {g.Status}.");

        // R10: no overlap with another Confirmed reservation on the same Device-Group.
        foreach (var existing in _reservations.Values)
        {
            if (existing.ReservationId == r.ReservationId) continue;
            if (existing.Status != ReservationStatus.Confirmed) continue;
            if (existing.DeviceGroupId != r.DeviceGroupId) continue;
            if (r.StartUtc < existing.EndUtc && r.EndUtc > existing.StartUtc)
            {
                throw new ValidationException(
                    "R10",
                    $"Reservation cannot be confirmed: overlaps a confirmed booking on Device-Group '{g.Name}' from {existing.StartUtc:u} to {existing.EndUtc:u}.");
            }
        }

        // R11: no overlap with another Confirmed reservation on the same Test-Group.
        var tName = _testGroups.TryGetValue(r.TestGroupId, out var t) ? t.Name : r.TestGroupId.ToString();
        foreach (var existing in _reservations.Values)
        {
            if (existing.ReservationId == r.ReservationId) continue;
            if (existing.Status != ReservationStatus.Confirmed) continue;
            if (existing.TestGroupId != r.TestGroupId) continue;
            if (r.StartUtc < existing.EndUtc && r.EndUtc > existing.StartUtc)
            {
                throw new ValidationException(
                    "R11",
                    $"Reservation cannot be confirmed: Test-Group '{tName}' already has a confirmed booking from {existing.StartUtc:u} to {existing.EndUtc:u}.");
            }
        }

        r.Status = ReservationStatus.Confirmed;
        r.Version++;
        return Task.FromResult(Clone(r));
    }

    public Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default)
    {
        var r = Require(_reservations, id, "Reservation");
        EnsureVersion(r.Version, version, "Reservation");
        r.Status = ReservationStatus.Cancelled;
        r.Version++;
        return Task.FromResult(Clone(r));
    }

    // ============================================================
    // Helpers
    // ============================================================

    private static T Require<T>(ConcurrentDictionary<Guid, T> store, Guid id, string label)
    {
        if (!store.TryGetValue(id, out var v))
            throw new NotFoundException($"{label} {id} not found.");
        return v;
    }

    private static void EnsureVersion(int current, int supplied, string label)
    {
        if (current != supplied)
            throw new ConflictException($"{label} version mismatch: stored {current}, supplied {supplied}.");
    }

    /// <summary>
    /// R6: every connection endpoint must be a member of the group's DeviceIds.
    /// </summary>
    private static void ValidateConnectionMembership(
        string groupName,
        IReadOnlyList<Guid> deviceIds,
        IReadOnlyList<DeviceConnectionDto> connections)
    {
        var members = new HashSet<Guid>(deviceIds);
        foreach (var c in connections)
        {
            if (!members.Contains(c.FromDeviceId))
                throw new ValidationException(
                    "R6",
                    $"Device-Group '{groupName}' has a connection whose FromDeviceId {c.FromDeviceId} is not a member of the group.");
            if (!members.Contains(c.ToDeviceId))
                throw new ValidationException(
                    "R6",
                    $"Device-Group '{groupName}' has a connection whose ToDeviceId {c.ToDeviceId} is not a member of the group.");
        }
    }

    // Defensive cloning so callers cannot mutate stored aggregates by holding a reference.
    private static BuildingDto Clone(BuildingDto b) => new()
    {
        BuildingId = b.BuildingId, Name = b.Name, Address = b.Address, Version = b.Version,
    };
    private static DeviceDto Clone(DeviceDto d) => new()
    {
        DeviceId = d.DeviceId, Name = d.Name, Status = d.Status,
        BuildingId = d.BuildingId, AssignedDeviceGroupId = d.AssignedDeviceGroupId, Version = d.Version,
    };
    private static DeviceConnectionDto CloneConnection(DeviceConnectionDto c) => new()
    {
        ConnectionId = c.ConnectionId == Guid.Empty ? Guid.NewGuid() : c.ConnectionId,
        FromDeviceId = c.FromDeviceId, ToDeviceId = c.ToDeviceId, Label = c.Label,
    };
    private static DeviceGroupDto Clone(DeviceGroupDto g) => new()
    {
        DeviceGroupId = g.DeviceGroupId, Name = g.Name, Status = g.Status,
        DeviceIds = g.DeviceIds.ToList(),
        Connections = g.Connections.Select(CloneConnection).ToList(),
        Layout = g.Layout.Select(e => new DeviceLayoutEntry(e.DeviceId, e.X, e.Y)).ToList(),
        Version = g.Version,
    };
    private static PersonDto Clone(PersonDto p) => new()
    {
        PersonId = p.PersonId, Name = p.Name, Email = p.Email,
    };
    private static TestGroupDto Clone(TestGroupDto t) => new()
    {
        TestGroupId = t.TestGroupId, Name = t.Name,
        MemberIds = t.MemberIds.ToList(), Version = t.Version,
    };
    private static ReservationDto Clone(ReservationDto r) => new()
    {
        ReservationId = r.ReservationId, DeviceGroupId = r.DeviceGroupId, TestGroupId = r.TestGroupId,
        StartUtc = r.StartUtc, EndUtc = r.EndUtc, Status = r.Status, Notes = r.Notes, Version = r.Version,
    };

    // ============================================================
    // Seed data: representative fixture so the UI has something to show
    // on first load. Loaded from the embedded seed-data.json, which is
    // also the source of truth for the Rust seeder; see that file for
    // counts and intent. Reservation timestamps are reconstituted at
    // seed time from (dayOffset, startHour, endHour) anchored to local
    // midnight, so a "9" in the JSON renders as 9:00 local in any
    // timezone after the round-trip through ToLocalTime().
    // ============================================================
    private void Seed()
    {
        var data = LoadSeedData();

        foreach (var b in data.Buildings)
        {
            _buildings[b.BuildingId] = new BuildingDto
            {
                BuildingId = b.BuildingId,
                Name = b.Name,
                Address = b.Address,
                Version = 1,
            };
        }

        foreach (var d in data.Devices)
        {
            _devices[d.DeviceId] = new DeviceDto
            {
                DeviceId = d.DeviceId,
                Name = d.Name,
                Status = d.Status,
                BuildingId = d.BuildingId,
                Version = 1,
            };
        }

        foreach (var g in data.DeviceGroups)
        {
            _deviceGroups[g.DeviceGroupId] = new DeviceGroupDto
            {
                DeviceGroupId = g.DeviceGroupId,
                Name = g.Name,
                Status = g.Status,
                DeviceIds = g.DeviceIds.ToList(),
                Connections = g.Connections.Select(c => new DeviceConnectionDto
                {
                    ConnectionId = c.ConnectionId,
                    FromDeviceId = c.FromDeviceId,
                    ToDeviceId = c.ToDeviceId,
                    Label = c.Label,
                }).ToList(),
                Layout = g.Layout
                    .Select(e => new DeviceLayoutEntry(e.DeviceId, e.X, e.Y))
                    .ToList(),
                Version = 1,
            };

            // Reflect the convenience pointer on each member device for
            // active groups, mirroring what ActivateDeviceGroupAsync does.
            if (g.Status == DeviceGroupStatus.Active)
            {
                foreach (var did in g.DeviceIds)
                {
                    if (_devices.TryGetValue(did, out var dev))
                    {
                        dev.AssignedDeviceGroupId = g.DeviceGroupId;
                    }
                }
            }
        }

        foreach (var p in data.People)
        {
            _people[p.PersonId] = new PersonDto
            {
                PersonId = p.PersonId,
                Name = p.Name,
                Email = p.Email,
            };
        }

        foreach (var t in data.TestGroups)
        {
            _testGroups[t.TestGroupId] = new TestGroupDto
            {
                TestGroupId = t.TestGroupId,
                Name = t.Name,
                MemberIds = t.MemberIds.ToList(),
                Version = 1,
            };
        }

        // Local midnight today, expressed in UTC. AddHours(N) on this
        // value lands on local N:00 today after ToLocalTime() round-trip,
        // independent of the host's UTC offset.
        var localNow = _time.GetLocalNow();
        var todayLocalMidnightUtc = new DateTimeOffset(localNow.Date, localNow.Offset).UtcDateTime;

        foreach (var r in data.Reservations)
        {
            var dayBase = todayLocalMidnightUtc.AddDays(r.DayOffset);
            var id = Guid.NewGuid();
            _reservations[id] = new ReservationDto
            {
                ReservationId = id,
                DeviceGroupId = r.DeviceGroupId,
                TestGroupId = r.TestGroupId,
                StartUtc = dayBase.AddHours(r.StartHour),
                EndUtc = dayBase.AddHours(r.EndHour),
                Status = r.Status,
                Notes = r.Notes,
                Version = 1,
            };
        }
    }

    private static SeedData LoadSeedData()
    {
        using var stream = typeof(InMemoryClientService).Assembly
            .GetManifestResourceStream("seed-data.json")
            ?? throw new InvalidOperationException(
                "seed-data.json was not embedded in the Components assembly. " +
                "Check the EmbeddedResource entry in ResourceScheduler.Components.csproj.");
        var data = JsonSerializer.Deserialize<SeedData>(stream, SeedJsonOptions);
        return data ?? throw new InvalidOperationException("seed-data.json deserialised to null.");
    }

    private static readonly JsonSerializerOptions SeedJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    // Wire-shape records for seed-data.json. Mirrored on the Rust side
    // by the structs in seed.rs; both load the same camelCase fields.
    private sealed record SeedData(
        SeedBuilding[] Buildings,
        SeedDevice[] Devices,
        SeedDeviceGroup[] DeviceGroups,
        SeedPerson[] People,
        SeedTestGroup[] TestGroups,
        SeedReservation[] Reservations);

    private sealed record SeedBuilding(Guid BuildingId, string Name, string Address);

    private sealed record SeedDevice(
        Guid DeviceId, string Name, DeviceStatus Status, Guid BuildingId);

    private sealed record SeedDeviceGroup(
        Guid DeviceGroupId,
        string Name,
        DeviceGroupStatus Status,
        Guid[] DeviceIds,
        SeedConnection[] Connections,
        SeedLayoutEntry[] Layout);

    private sealed record SeedConnection(
        Guid ConnectionId, Guid FromDeviceId, Guid ToDeviceId, string Label);

    private sealed record SeedLayoutEntry(Guid DeviceId, double X, double Y);

    private sealed record SeedPerson(Guid PersonId, string Name, string? Email);

    private sealed record SeedTestGroup(Guid TestGroupId, string Name, Guid[] MemberIds);

    private sealed record SeedReservation(
        Guid DeviceGroupId,
        Guid TestGroupId,
        int DayOffset,
        int StartHour,
        int EndHour,
        ReservationStatus Status,
        string? Notes);

}
