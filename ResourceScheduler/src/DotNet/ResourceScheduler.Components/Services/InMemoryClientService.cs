using System.Collections.Concurrent;
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
        => Task.FromResult<IReadOnlyList<DeviceDto>>(_devices.Values.OrderBy(d => d.Name).Select(Clone).ToList());

    public Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_devices.TryGetValue(id, out var d) ? Clone(d) : null);

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
    // on first load. Times are anchored to local midnight so a "9" in the
    // seed renders as 9:00 local in any timezone, then converted to UTC
    // for storage. The timeline renders in local time, so this ordering
    // keeps reservation hours within the workday window (6-22) wherever
    // the host browser is.
    // Counts: 2 buildings, 24 devices (covering all DeviceStatus values),
    // 6 device-groups (4 Active, 2 Inactive drafts), 4 people, 2 test-groups,
    // 11 reservations across yesterday/today/tomorrow.
    // ============================================================
    private void Seed()
    {
        // ---- Buildings ----
        var bNorth = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "Lab North",
            Address = "1400 Industrial Pkwy\nBuilding A, Floor 3\nCambridge, MA 02139",
            Version = 1,
        };
        var bSouth = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "Lab South",
            Address = "210 Foundry Rd\nWest Wing, Bay 2\nCambridge, MA 02141",
            Version = 1,
        };
        _buildings[bNorth.BuildingId] = bNorth;
        _buildings[bSouth.BuildingId] = bSouth;

        // ---- Devices: covers all DeviceStatus values ----
        DeviceDto NewDevice(string name, DeviceStatus status, Guid buildingId) =>
            new()
            {
                DeviceId = Guid.NewGuid(),
                Name = name,
                Status = status,
                BuildingId = buildingId,
                Version = 1,
            };

        var scope01 = NewDevice("SCOPE-01",  DeviceStatus.Available,   bNorth.BuildingId);
        var scope02 = NewDevice("SCOPE-02",  DeviceStatus.Available,   bNorth.BuildingId);
        var scope03 = NewDevice("SCOPE-03",  DeviceStatus.Maintenance, bNorth.BuildingId);
        var scope04 = NewDevice("SCOPE-04",  DeviceStatus.Available,   bSouth.BuildingId);
        var awg01   = NewDevice("AWG-01",    DeviceStatus.Available,   bNorth.BuildingId);
        var awg02   = NewDevice("AWG-02",    DeviceStatus.Available,   bSouth.BuildingId);
        var awg03   = NewDevice("AWG-03",    DeviceStatus.Available,   bNorth.BuildingId);
        var dmm01   = NewDevice("DMM-01",    DeviceStatus.Available,   bNorth.BuildingId);
        var dmm02   = NewDevice("DMM-02",    DeviceStatus.Available,   bSouth.BuildingId);
        var dmm03   = NewDevice("DMM-03",    DeviceStatus.Offline,     bSouth.BuildingId);
        var dmm04   = NewDevice("DMM-04",    DeviceStatus.Available,   bSouth.BuildingId);
        var psu01   = NewDevice("PSU-01",    DeviceStatus.Available,   bNorth.BuildingId);
        var psu02   = NewDevice("PSU-02",    DeviceStatus.Available,   bSouth.BuildingId);
        var psu03   = NewDevice("PSU-03",    DeviceStatus.Available,   bSouth.BuildingId);
        var load01  = NewDevice("LOAD-01",   DeviceStatus.Available,   bSouth.BuildingId);
        var load02  = NewDevice("LOAD-02",   DeviceStatus.Available,   bSouth.BuildingId);
        var rfgen01 = NewDevice("RFGEN-01",  DeviceStatus.Available,   bNorth.BuildingId);
        var rfgen02 = NewDevice("RFGEN-02",  DeviceStatus.Maintenance, bNorth.BuildingId);
        var spec01  = NewDevice("SPEC-01",   DeviceStatus.Available,   bNorth.BuildingId);
        var vna01   = NewDevice("VNA-01",    DeviceStatus.Available,   bNorth.BuildingId);
        var refclk  = NewDevice("REF-CLK",   DeviceStatus.Available,   bNorth.BuildingId);
        var tempChm = NewDevice("TEMP-CHM-01", DeviceStatus.Available, bSouth.BuildingId);
        var daq01   = NewDevice("DAQ-01",    DeviceStatus.Available,   bSouth.BuildingId);
        var probe99 = NewDevice("PROBE-99",  DeviceStatus.Retired,     bSouth.BuildingId);

        foreach (var d in new[]
        {
            scope01, scope02, scope03, scope04,
            awg01,   awg02,   awg03,
            dmm01,   dmm02,   dmm03,   dmm04,
            psu01,   psu02,   psu03,
            load01,  load02,
            rfgen01, rfgen02, spec01,  vna01,  refclk,
            tempChm, daq01,
            probe99,
        })
            _devices[d.DeviceId] = d;

        // ---- Device-Groups: 4 Active, 2 Inactive drafts ----
        var groupAlpha = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "Bench A, Power Characterization",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new() { psu01.DeviceId, awg01.DeviceId, dmm01.DeviceId, scope01.DeviceId },
            Connections = new()
            {
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = psu01.DeviceId, ToDeviceId = awg01.DeviceId,   Label = "DC-OUT" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = awg01.DeviceId, ToDeviceId = scope01.DeviceId, Label = "CH-1"   },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = awg01.DeviceId, ToDeviceId = dmm01.DeviceId,   Label = "TRIG"   },
            },
            Version = 1,
        };
        var groupBeta = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "Bench B, Load Sweep",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new() { psu02.DeviceId, load01.DeviceId, dmm02.DeviceId, scope02.DeviceId },
            Connections = new()
            {
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = psu02.DeviceId,  ToDeviceId = load01.DeviceId, Label = "DUT" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = load01.DeviceId, ToDeviceId = dmm02.DeviceId,  Label = "SENSE" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = load01.DeviceId, ToDeviceId = scope02.DeviceId,Label = "MONITOR" },
            },
            Version = 1,
        };
        var groupGamma = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "RF Suite, 24 GHz",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new() { rfgen01.DeviceId, spec01.DeviceId, vna01.DeviceId, refclk.DeviceId },
            Connections = new()
            {
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = refclk.DeviceId,  ToDeviceId = rfgen01.DeviceId, Label = "10MHz" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = refclk.DeviceId,  ToDeviceId = spec01.DeviceId,  Label = "10MHz" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = refclk.DeviceId,  ToDeviceId = vna01.DeviceId,   Label = "10MHz" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = rfgen01.DeviceId, ToDeviceId = vna01.DeviceId,   Label = "RF-OUT" },
            },
            Version = 1,
        };
        var groupDelta = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "Rack 4, DAQ Cluster",
            Status = DeviceGroupStatus.Active,
            DeviceIds = new() { psu03.DeviceId, daq01.DeviceId, dmm04.DeviceId, scope04.DeviceId },
            Connections = new()
            {
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = psu03.DeviceId, ToDeviceId = daq01.DeviceId,   Label = "12V" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = daq01.DeviceId, ToDeviceId = dmm04.DeviceId,   Label = "AI-1" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = daq01.DeviceId, ToDeviceId = scope04.DeviceId, Label = "AI-2" },
            },
            Version = 1,
        };
        var groupDraftAudio = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "Draft, Audio Lab",
            Status = DeviceGroupStatus.Inactive,
            DeviceIds = new() { awg02.DeviceId },
            Connections = new(),
            Version = 1,
        };
        var groupDraftThermal = new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "Draft, Thermal Sweep",
            Status = DeviceGroupStatus.Inactive,
            DeviceIds = new() { tempChm.DeviceId, awg03.DeviceId, load02.DeviceId },
            Connections = new()
            {
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = awg03.DeviceId,  ToDeviceId = tempChm.DeviceId, Label = "CTRL" },
                new() { ConnectionId = Guid.NewGuid(), FromDeviceId = load02.DeviceId, ToDeviceId = tempChm.DeviceId, Label = "DUT" },
            },
            Version = 1,
        };
        _deviceGroups[groupAlpha.DeviceGroupId]        = groupAlpha;
        _deviceGroups[groupBeta.DeviceGroupId]         = groupBeta;
        _deviceGroups[groupGamma.DeviceGroupId]        = groupGamma;
        _deviceGroups[groupDelta.DeviceGroupId]        = groupDelta;
        _deviceGroups[groupDraftAudio.DeviceGroupId]   = groupDraftAudio;
        _deviceGroups[groupDraftThermal.DeviceGroupId] = groupDraftThermal;

        // Reflect back the assignment convenience field for active groups.
        void AssignTo(DeviceGroupDto g)
        {
            foreach (var did in g.DeviceIds)
                if (_devices.TryGetValue(did, out var d)) d.AssignedDeviceGroupId = g.DeviceGroupId;
        }
        AssignTo(groupAlpha);
        AssignTo(groupBeta);
        AssignTo(groupGamma);
        AssignTo(groupDelta);

        // ---- People ----
        var aoife = new PersonDto { PersonId = Guid.NewGuid(), Name = "Aoife O'Brien", Email = "aoife@lab.example" };
        var dawit = new PersonDto { PersonId = Guid.NewGuid(), Name = "Dawit Bekele",  Email = "dawit@lab.example" };
        var mei   = new PersonDto { PersonId = Guid.NewGuid(), Name = "Mei Tanaka",    Email = "mei@lab.example"   };
        var ravi  = new PersonDto { PersonId = Guid.NewGuid(), Name = "Ravi Patel",    Email = "ravi@lab.example"  };
        _people[aoife.PersonId] = aoife;
        _people[dawit.PersonId] = dawit;
        _people[mei.PersonId]   = mei;
        _people[ravi.PersonId]  = ravi;

        // ---- Test-Groups ----
        var powerTeam = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = "Power Team",
            MemberIds = new() { aoife.PersonId, dawit.PersonId },
            Version = 1,
        };
        var rfTeam = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = "RF Team",
            MemberIds = new() { mei.PersonId, ravi.PersonId, aoife.PersonId },
            Version = 1,
        };
        _testGroups[powerTeam.TestGroupId] = powerTeam;
        _testGroups[rfTeam.TestGroupId]    = rfTeam;

        // ---- Reservations across yesterday, today, tomorrow ----
        // Local midnight today, expressed in UTC. AddHours(N) on this
        // value lands on local N:00 today after ToLocalTime() round-trip,
        // independent of the host's UTC offset.
        var localNow  = _time.GetLocalNow();
        var today     = new DateTimeOffset(localNow.Date, localNow.Offset).UtcDateTime;
        var yesterday = today.AddDays(-1);
        var tomorrow  = today.AddDays(1);

        ReservationDto NewResv(Guid groupId, Guid teamId, DateTime start, DateTime end, ReservationStatus status, string? notes) =>
            new()
            {
                ReservationId = Guid.NewGuid(),
                DeviceGroupId = groupId,
                TestGroupId = teamId,
                StartUtc = start,
                EndUtc = end,
                Status = status,
                Notes = notes,
                Version = 1,
            };

        var reservations = new[]
        {
            NewResv(groupAlpha.DeviceGroupId, powerTeam.TestGroupId,
                yesterday.AddHours(9),  yesterday.AddHours(12),
                ReservationStatus.Completed, "Morning calibration run."),
            NewResv(groupBeta.DeviceGroupId, rfTeam.TestGroupId,
                yesterday.AddHours(13), yesterday.AddHours(16),
                ReservationStatus.Cancelled, "Cancelled, instrument fault."),
            NewResv(groupGamma.DeviceGroupId, rfTeam.TestGroupId,
                yesterday.AddHours(10), yesterday.AddHours(15),
                ReservationStatus.Completed, "S-parameter sweep at 24 GHz."),
            NewResv(groupAlpha.DeviceGroupId, powerTeam.TestGroupId,
                today.AddHours(12),     today.AddHours(15),
                ReservationStatus.Confirmed, "Production sweep, 1.2V to 3.3V rails."),
            NewResv(groupBeta.DeviceGroupId, rfTeam.TestGroupId,
                today.AddHours(16),     today.AddHours(18),
                ReservationStatus.Pending, "Awaiting RF team lead approval."),
            NewResv(groupGamma.DeviceGroupId, rfTeam.TestGroupId,
                today.AddHours(9),      today.AddHours(13),
                ReservationStatus.Confirmed, "RF compliance pre-scan."),
            NewResv(groupDelta.DeviceGroupId, powerTeam.TestGroupId,
                today.AddHours(10),     today.AddHours(14),
                ReservationStatus.Confirmed, "DAQ regression run."),
            NewResv(groupAlpha.DeviceGroupId, rfTeam.TestGroupId,
                tomorrow.AddHours(9),   tomorrow.AddHours(11),
                ReservationStatus.Confirmed, "Cross-team handover."),
            NewResv(groupBeta.DeviceGroupId, powerTeam.TestGroupId,
                tomorrow.AddHours(13),  tomorrow.AddHours(17),
                ReservationStatus.Pending, "Tentative load sweep."),
            NewResv(groupDelta.DeviceGroupId, powerTeam.TestGroupId,
                tomorrow.AddHours(8),   tomorrow.AddHours(12),
                ReservationStatus.Pending, "Continuation of today's DAQ run."),
            NewResv(groupGamma.DeviceGroupId, rfTeam.TestGroupId,
                tomorrow.AddHours(14),  tomorrow.AddHours(18),
                ReservationStatus.Confirmed, "Antenna characterization."),
        };
        foreach (var r in reservations)
            _reservations[r.ReservationId] = r;
    }
}
