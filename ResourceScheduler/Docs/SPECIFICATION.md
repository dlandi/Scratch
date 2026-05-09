# Resource Scheduler Specification

Version: 0.1 (Draft)
Date: 2026-05-08
Status: Phase 1 design

## 1. Purpose

Schedule shared lab equipment for use by groups of people. Equipment is
joined together physically into Device-Groups, which are reserved as
single schedulable units by Test-Groups. The application is a Blazor
WebAssembly client that simulates the backend with in-memory data in
Phase 1, with a Rust backend planned for Phase 2.

## 2. Scope

### Phase 1 (in scope)

- Blazor WebAssembly client built on **.NET 10** (target framework
  `net10.0`).
- Razor component library targeting **.NET 10** (`net10.0`) housing all
  custom components and the single `components.css` stylesheet.
- In-memory simulated backend behind a `ClientService` abstraction.
- CRUD on Devices, Device-Groups, Test-Groups, People, and Reservations.
- Enforcement of all business rules in section 5.
- SVG-first UI rendering inside the Razor component library.

### Phase 2 (planned, not in this spec)

- Replace the in-memory implementation of `ClientService` with an HTTP
  client that talks to a Rust backend.
- The interface contracts in section 8 must hold across both phases so
  that swapping implementations does not change call sites.

### Out of scope

- Authentication and authorization.
- Real-time push notifications.
- Calendar export.
- Audit logging beyond simple `Version` fields for optimistic
  concurrency.

## 3. Solution Layout

Both projects target `net10.0` and use the latest `Microsoft.NET.Sdk`
and `Microsoft.NET.Sdk.BlazorWebAssembly` SDKs available in the .NET 10
release. C# language version is the default for `net10.0`.

```
ResourceScheduler.sln
  src/
    ResourceScheduler.Components/   Razor class library  (TFM: net10.0)
      Components/                   .razor files (no per-component CSS)
      wwwroot/
        components.css              Single shared stylesheet
      Models/                       DTOs and enums
      Services/
        IClientService.cs           Contract used by the app
        InMemoryClientService.cs    Phase 1 implementation
        RemoteClientService.cs      Phase 2 HTTP implementation
      _Imports.razor
    ResourceScheduler.WebApp/       Blazor WASM project  (TFM: net10.0)
      Pages/                        Page-level routing
      Program.cs
      wwwroot/
        index.html                  Loads components.css
  Docs/
    SPECIFICATION.md                This document
```

Rules:

- The Razor library does not depend on the WASM host project.
- The WASM project depends on the Razor library and supplies the
  `IClientService` implementation through DI.
- Both `InMemoryClientService` and `RemoteClientService` live in the
  Razor library so the xUnit test project can reference them without
  pulling in the `Microsoft.NET.Sdk.BlazorWebAssembly` SDK.
- All shared CSS lives in `ResourceScheduler.Components/wwwroot/components.css`
  and is referenced once from the host's `index.html`. Blazor's per-component
  `.razor.css` isolation is not used unless a specific component requires
  it, in which case it is added as a deliberate exception.

## 4. Domain Model

### 4.1 Device

A single physical instrument or unit in the lab. Every Device is
physically located in exactly one Building (see 4.7).

| Field                    | Type             | Notes                                     |
|--------------------------|------------------|-------------------------------------------|
| DeviceId                 | Guid             | Identity                                  |
| Name                     | string           | Unique, human-readable                    |
| Status                   | DeviceStatus     | See 7.1                                   |
| BuildingId               | Guid             | Required; the Building that houses it     |
| AssignedDeviceGroupId    | Guid?            | Convenience field; null if unassigned     |
| Version                  | int              | Optimistic concurrency                    |

A Device is "assigned" when it appears in any Device-Group (active or
inactive). A Device is "deployed" when it appears in an Active
Device-Group.

### 4.2 DeviceGroup

A named, connected collection of Devices that is reserved as one unit.

| Field           | Type                       | Notes                              |
|-----------------|----------------------------|------------------------------------|
| DeviceGroupId   | Guid                       | Identity                           |
| Name            | string                     | Unique, human-readable             |
| Status          | DeviceGroupStatus          | See 7.2                            |
| DeviceIds       | List&lt;Guid&gt;           | Ordered list of member devices     |
| Connections     | List&lt;DeviceConnection&gt; | Topology among member devices    |
| Layout          | List&lt;DeviceLayoutEntry&gt; | Per-device normalized [0..1] positions on the Designer canvas. Empty falls back to auto-layout. |
| Version         | int                        | Optimistic concurrency             |

`DeviceLayoutEntry` records the saved coordinates for a single member
device. Coordinates are normalized to [0..1] so the Designer can
re-render at any canvas size without re-projecting:

```csharp
public sealed record DeviceLayoutEntry(Guid DeviceId, double X, double Y);
```

The matching `DeviceGroupCreate` and `DeviceGroupUpdate` records carry
the same payload so positions round-trip through the service:

```csharp
public sealed record DeviceGroupCreate(
    string Name,
    IReadOnlyList<Guid> DeviceIds,
    IReadOnlyList<DeviceConnection> Connections,
    IReadOnlyList<DeviceLayoutEntry> Layout);

public sealed record DeviceGroupUpdate(
    string Name,
    IReadOnlyList<Guid> DeviceIds,
    IReadOnlyList<DeviceConnection> Connections,
    IReadOnlyList<DeviceLayoutEntry> Layout);
```

### 4.3 DeviceConnection

Represents a physical link between two devices inside the same group.

| Field          | Type   | Notes                              |
|----------------|--------|------------------------------------|
| ConnectionId   | Guid   | Identity                           |
| FromDeviceId   | Guid   | Must be a member of the group      |
| ToDeviceId     | Guid   | Must be a member of the group      |
| Label          | string | Optional, free-form (e.g. "USB-3") |

Connections are directionless for scheduling purposes but a (from, to)
pair is recorded for display.

### 4.4 Person

A human participant in the lab.

| Field      | Type   | Notes                  |
|------------|--------|------------------------|
| PersonId   | Guid   | Identity               |
| Name       | string | Display name           |
| Email      | string | Optional, unique if set|

### 4.5 TestGroup

A named team of people who reserve Device-Groups together.

| Field         | Type             | Notes                          |
|---------------|------------------|--------------------------------|
| TestGroupId   | Guid             | Identity                       |
| Name          | string           | Unique                         |
| MemberIds     | List&lt;Guid&gt; | PersonIds                      |
| Version       | int              | Optimistic concurrency         |

A Person may belong to many Test-Groups.

### 4.6 Reservation


A booking of a Device-Group by a Test-Group over a time window.

| Field           | Type              | Notes                            |
|-----------------|-------------------|----------------------------------|
| ReservationId   | Guid              | Identity                         |
| DeviceGroupId   | Guid              | Target group                     |
| TestGroupId     | Guid              | Booking team                     |
| StartUtc        | DateTime (UTC)    | Inclusive                        |
| EndUtc          | DateTime (UTC)    | Exclusive                        |
| Status          | ReservationStatus | See 7.3                          |
| Notes           | string?           | Optional free text               |
| Version         | int               | Optimistic concurrency           |

The Reservation Editor supports multi-day windows: the start and end
may fall on different dates, and the conflict tests in R10 and R11
operate on the full UTC range, so multi-day windows are handled
correctly without any code changes.

### 4.7 Building

A real-world physical building that houses Devices.

| Field        | Type   | Notes                                          |
|--------------|--------|------------------------------------------------|
| BuildingId   | Guid   | Identity                                       |
| Name         | string | Unique, human-readable (e.g. "Lab North")      |
| Address      | string | Free-form mailing address; multi-line allowed  |
| Version      | int    | Optimistic concurrency                         |

Notes:

- A Building contains zero or more Devices.
- A Device belongs to exactly one Building at a time. Relocating a
  Device is modeled as updating its `BuildingId`.
- `Address` is stored as a single free-form string in Phase 1. If
  structured fields (street, city, postal code, country) are needed
  later, they will be introduced as a typed value object and migrated
  in Phase 2.

## 5. Business Rules

### 5.1 Device assignment exclusivity (real-time)

R1. A Device may be deployed in **at most one Active Device-Group** at
any moment in real time.

R2. A Device may appear in any number of **Inactive** Device-Groups
simultaneously. Inactive groups are treated as draft configurations.

R3. An Inactive Device-Group cannot be activated if any of its member
devices is already a member of another Active Device-Group.

R4. Removing a Device from an Active Device-Group is permitted. Once
removed, that Device becomes eligible to be added to another group and
that group may then be activated, subject to R3. The user is shown a
confirmation that explains the consequence; no automatic cascading is
performed.

R5. Devices in Status `Offline`, `Maintenance`, or `Retired` cannot be
members of an Active Device-Group. A group cannot be activated if it
contains such a device.

### 5.2 Device-Group integrity

R6. All `DeviceConnection` entries must reference devices that are
members of the same group.

R7. A Device-Group with zero members may exist as Inactive but cannot
be activated.

### 5.3 Reservation rules

R8. A Reservation references exactly one Device-Group and exactly one
Test-Group.

R9. The target Device-Group must be `Active` at the time the
Reservation is created or confirmed.

R10. **Device-Group exclusivity:** two `Confirmed` reservations for the
same Device-Group cannot overlap. The overlap test is:

```csharp
candidate.StartUtc < existing.EndUtc &&
candidate.EndUtc   > existing.StartUtc
```

R11. **Test-Group exclusivity:** two `Confirmed` reservations for the
same Test-Group cannot overlap, using the same overlap test.

R12. Individual People are not checked for double-booking. A Person
may belong to multiple Test-Groups and may therefore appear in
overlapping reservations.

R13. A Reservation must have `EndUtc > StartUtc`.

### 5.4 Building rules

R14. Every Device must reference an existing Building via `BuildingId`.
A Device cannot be created or updated without one.

R15. A Building cannot be deleted while any Device references it.
Devices must be reassigned to another Building or deleted first.

### 5.5 What is not enforced (deliberate)

- No automatic deactivation of a group when a member device flips to
  Offline. The system surfaces this state to the user but lets the user
  decide.
- No "move device" wizard. Moving a device from group A to group B is
  modeled as a remove from A followed by an add to B.
- No participant-level conflict detection. See R12.

## 6. Architecture

### 6.1 ClientService abstraction

`IClientService` is the single seam between the UI and any backend.
Phase 1 implements it with in-memory collections; Phase 2 implements it
with an HTTP client over the Rust API. The interface is async and
returns DTOs by value. Mutations carry a `Version` for optimistic
concurrency. See section 8 for the contract.

### 6.2 In-memory store (Phase 1)

- Backed by `ConcurrentDictionary<Guid, T>` instances per aggregate.
- Seed data is loaded once on startup with a small fixture set so the
  UI is usable without any setup.
- All write methods perform validation against the rules in section 5
  and throw a typed `DomainException` on failure that the UI maps to
  user-visible messages.
- Optimistic concurrency: a write that supplies a stale `Version`
  fails with `ConflictException`.

### 6.3 Phase 2 readiness

The `IClientService` contract is shaped to map cleanly to REST:

- One method per resource verb (List, Get, Create, Update, Delete) plus
  domain operations (Activate, Deactivate, Confirm, Cancel).
- All identifiers are `Guid`.
- All times are `DateTime` with `Kind = Utc`.
- Optional filters are passed as plain parameters or a small filter
  record, not LINQ expressions.

### 6.4 Time abstraction

- Pages and `InMemoryClientService.Seed` inject `TimeProvider` rather
  than calling `DateTime.Now`, `DateTime.UtcNow`, or `DateTime.Today`
  directly. This makes time-dependent code substitutable in tests via
  `Microsoft.Extensions.TimeProvider.Testing.FakeTimeProvider`.
- `UserTimeProvider : TimeProvider` lives in `Components/Services/`. It
  overrides `LocalTimeZone` so the header dropdown can switch zones at
  runtime, raising a `Changed` event that pages subscribe to. Both the
  `UserTimeProvider` concrete class and the `TimeProvider` abstraction
  resolve to the same singleton in DI, so consumers see the same
  instance whether they ask for the base type or the concrete one.
- `TimeProviderExtensions.ToLocal(utc)` is the helper pages use instead
  of `DateTime.ToLocalTime()`, so the chosen zone is honored everywhere
  the UI shows wall-clock times.

## 7. State Machines

### 7.1 DeviceStatus

```
Available  --> Maintenance
Available  --> Offline
Available  --> Retired
Maintenance --> Available
Offline    --> Available
Retired    --> (terminal)
```

A Device in an Active Device-Group is implicitly considered "in use"
during a Confirmed reservation but its `DeviceStatus` is independent of
reservation status.

### 7.2 DeviceGroupStatus

```
Inactive --> Active   (subject to R3, R5, R6, R7)
Active   --> Inactive (always allowed; cancels nothing automatically)
```

When deactivating a group with future Confirmed reservations, the user
is warned that those reservations will become invalid (because R9
requires Active at confirmation). The system does not auto-cancel them.

### 7.3 ReservationStatus

```
Pending   --> Confirmed (subject to R9, R10, R11)
Pending   --> Cancelled
Confirmed --> Cancelled
Confirmed --> Completed (when EndUtc <= now)
```

`Pending` reservations do not participate in conflict detection. Only
`Confirmed` reservations block other bookings.

## 8. ClientService Contract (sketch)

```csharp
public interface IClientService
{
    // Buildings
    Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default);
    Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default);
    Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default);
    Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default);
    Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default);

    // Devices
    Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default);
    Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default);
    Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default);
    Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default);
    Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default);

    // Device-Groups
    Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default);
    Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default);
    Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default);
    Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default);
    Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);
    Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);
    Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default);

    // People
    Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default);
    Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default);
    Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default);
    Task DeletePersonAsync(Guid id, CancellationToken ct = default);

    // Test-Groups
    Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default);
    Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default);
    Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default);
    Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default);

    // Reservations
    Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default);
    Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default);
    Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default);
    Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default);
}
```

`ReservationFilter` carries optional `DeviceGroupId`, `TestGroupId`,
`FromUtc`, `ToUtc`, and `StatusIn`. Concrete record types for the
`*Create` and `*Update` payloads live next to the DTOs in the
`Models` folder.

Errors are surfaced through typed exceptions:

- `NotFoundException`
- `ConflictException` (version mismatch)
- `ValidationException` (rule violation; carries which rule fired)

## 9. UI Design

### 9.1 SVG-first rendering

Where a view shows structure, status, or layout, the Razor component
library renders it with inline SVG rather than HTML/CSS box layouts.
Targets:

- **DeviceGroupCanvas:** an SVG canvas that draws each Device as a node
  and each DeviceConnection as an edge. Node fill encodes
  `DeviceStatus`; node border encodes group membership state. Used in
  both the designer and the read-only inspector.
- **ScheduleTimeline:** an SVG horizontal timeline showing
  Reservations across a selected day or week, with one row per
  Device-Group. Overlap attempts during drag are drawn as red bands.

The Schedule Timeline canvas is UTC-anchored. Reservation rectangle
positions and the NOW line are pure UTC offsets from the canvas
anchor; only the hour-tick labels and the per-block time captions are
computed from the chosen `Time.LocalTimeZone`. The day view canvas
spans 24 hours (00:00 to 24:00 UTC) at `HourW = 90 px`, and auto-scroll
positions the NOW line about 25% from the visible left edge so recent
and upcoming activity are both in view. Switching the timezone
dropdown re-labels the hour ticks and block captions but does not move
reservations, which keeps relative positions stable when previewing
other zones.
- **DevicePicker:** SVG chips with a status dot and a small lock icon
  when the device is deployed in another Active group.
- **TestGroupAvatar:** SVG cluster of initials for the Test-Group.

HTML elements are still used for forms, tables, and standard text
content where SVG would add no value. The intent is structural
visualization in SVG, not avoidance of HTML.

### 9.2 Styling

- All shared styles live in `ResourceScheduler.Components/wwwroot/components.css`.
- Class names use a `rs-` prefix to avoid collisions with any host
  styles (for example `rs-device-node`, `rs-timeline-row`).
- A small set of CSS custom properties at `:root` defines the palette
  and spacing tokens; SVG components use those tokens via `var(...)`
  on `fill` and `stroke` so theming is driven from one place.
- Per-component `.razor.css` files are not added unless a specific
  component has a real isolation need; if added it is documented
  inline in the component's file header.

### 9.3 Component catalog (Razor library)

| Component              | Purpose                                                |
|------------------------|--------------------------------------------------------|
| `BuildingList`         | Tabular list of buildings with device counts           |
| `BuildingEditor`       | Form for creating/editing a building                   |
| `DeviceList`           | Tabular list of devices with status and building filter |
| `DeviceGroupList`      | List of groups with active/inactive filter             |
| `DeviceGroupDesigner`  | Edits members and connections; embeds `DeviceGroupCanvas` |
| `DeviceGroupCanvas`    | SVG render of nodes + edges                            |
| `DevicePicker`         | SVG-chip picker for adding devices to a group          |
| `TestGroupList`        | List of test-groups                                    |
| `TestGroupEditor`      | Edits members of a test-group                          |
| `PersonList`           | People CRUD                                            |
| `ReservationList`      | Tabular reservations                                   |
| `ReservationEditor`    | Form for creating/confirming a reservation             |
| `ScheduleTimeline`     | SVG day/week view of reservations                      |
| `RuleViolationBanner`  | Renders `ValidationException` content uniformly        |

Each component takes its data through parameters and raises events for
mutations; none of them call `IClientService` directly. Page-level
components in the WASM project orchestrate service calls and pass DTOs
into library components.

### 9.4 Timezone switcher (developer aid)

- The header chip is rendered as a `<select>` when
  `Features:TimeZoneSwitcher:Enabled` is `true` in `appsettings.json`,
  and as a static label otherwise. It exists so developers can preview
  the schedule in non-host zones without changing the OS clock.
- The dropdown lists one entry per integer-hour UTC offset from -11 to
  +14, each backed by a friendly real IANA zone (Antarctica and
  `Etc/*` zones are filtered out). The host's own zone is added on top
  if it has a fractional offset, so India, Newfoundland, and similar
  zones remain selectable for the user running the app.

## 10. Validation Summary (one-page reference)

| ID  | Where enforced              | Rule                                                                              |
|-----|-----------------------------|-----------------------------------------------------------------------------------|
| R1  | Group activation, member add | Device deployed in at most one Active group                                       |
| R2  | Member add                  | Device may appear in any number of Inactive groups                                |
| R3  | Group activation            | Cannot activate if any member device is in another Active group                   |
| R4  | Member remove               | Always allowed; user warned                                                       |
| R5  | Group activation            | No Offline/Maintenance/Retired members in Active group                            |
| R6  | Group save                  | Connections must reference current members                                        |
| R7  | Group activation            | Group must have at least one member                                               |
| R8  | Reservation create          | Must reference one Device-Group and one Test-Group                                |
| R9  | Reservation confirm         | Target group must be Active                                                       |
| R10 | Reservation confirm         | No overlap with other Confirmed reservations on same Device-Group                 |
| R11 | Reservation confirm         | No overlap with other Confirmed reservations on same Test-Group                   |
| R12 | (not enforced)              | Person-level overlaps are allowed                                                 |
| R13 | Reservation create/update   | EndUtc > StartUtc                                                                 |
| R14 | Device create/update        | BuildingId must reference an existing Building                                    |
| R15 | Building delete             | Blocked while any Device references it                                            |

## 11. Open Items

These are explicitly deferred and tracked here so they are not lost:

- Whether all members of a Device-Group must reside in the same
  Building. Physically realistic, but not enforced in Phase 1 pending
  product confirmation.
- Structured address fields for Building (street, city, postal code,
  country) versus the current free-form string.
- Bulk import of devices.
- Recurring reservations.
- Phase 2: Rust backend contract document and migration plan.
- Phase 2: real authentication and per-Test-Group permissions.

### Closed

- Time-zone presentation in UI: DONE. Implemented via `TimeProvider`,
  `UserTimeProvider`, and the header dropdown described in 6.4 and 9.4.
- Designer layout positions not persisted: DONE. Captured by the
  `Layout` field on `DeviceGroupDto` and round-tripped through
  `DeviceGroupCreate` and `DeviceGroupUpdate` (see 4.2).
