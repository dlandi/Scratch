# Phase 2 HTTP Contract

## 1. Overview

Phase 1 simulates the backend in memory behind the `IClientService` interface. Phase 2 swaps a Rust HTTP API in behind the same seam. The C# `RemoteClientService` (in `src/ResourceScheduler.Components/Services/RemoteClientService.cs`) is the client; `RemoteClientServiceTests` (in `src/ResourceScheduler.Tests/RemoteClientServiceTests.cs`) is the executable contract.

- Any change on either side that breaks this contract requires updating both `RemoteClientService` and its tests, so drift is caught early.

## 2. Conventions

- **Base URL**: configured at startup via `HttpClient.BaseAddress`. All paths in this document are relative.
- **JSON**: camelCase property names. Enums serialize as their string name (e.g. `"status": "Confirmed"`, never the underlying integer).
- **Optimistic concurrency**: aggregates with a `Version` field require an `If-Match: {integer}` header on every PUT, DELETE, and POST sub-resource verb (`activate`, `deactivate`, `confirm`, `cancel`). The server replies `409 Conflict` on mismatch.
- **People exception**: `PersonDto` has no `Version` field, so PUT and DELETE for `/api/people/{id}` do NOT carry an `If-Match` header.
- **Status codes**:

  | Code | Meaning |
  |---|---|
  | `200 OK` | Read; or POST/PUT returning the updated resource |
  | `201 Created` | POST create (body returned); `200 OK` is also accepted by the client |
  | `204 No Content` | DELETE success |
  | `400 Bad Request` | Domain rule violation, structured body required (see below) |
  | `404 Not Found` | Missing resource. GET-by-id maps to `null`; all others throw `NotFoundException` |
  | `409 Conflict` | `If-Match` version mismatch |

- **Validation error body shape (only on `400`)**:

  ```json
  { "ruleId": "R10", "message": "Group already booked." }
  ```

  Both fields required. The client maps this to a typed `ValidationException(ruleId, message)`. A `400` with an unstructured body falls through to a plain `HttpRequestException`.
- **Cancellation tokens** are honored via the standard `HttpClient` pattern. No special server-side handling is required.

## 3. Endpoints

All resource paths are kebab-case plural. CRUD verbs on aggregates with a `Version` use `If-Match` as noted; GET requests never carry `If-Match`.

### Buildings (`/api/buildings`)

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/buildings` |  |  | `BuildingDto[]` |
| GET | `/api/buildings/{id}` |  |  | `BuildingDto` or `404` |
| POST | `/api/buildings` |  | `BuildingCreate` | `BuildingDto` |
| PUT | `/api/buildings/{id}` | `If-Match: {version}` | `BuildingUpdate` | `BuildingDto` |
| DELETE | `/api/buildings/{id}` | `If-Match: {version}` |  | `204` |

### Devices (`/api/devices`)

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/devices` |  |  | `DeviceDto[]` |
| GET | `/api/devices/{id}` |  |  | `DeviceDto` or `404` |
| POST | `/api/devices` |  | `DeviceCreate` | `DeviceDto` |
| PUT | `/api/devices/{id}` | `If-Match: {version}` | `DeviceUpdate` | `DeviceDto` |
| DELETE | `/api/devices/{id}` | `If-Match: {version}` |  | `204` |

### Device-Groups (`/api/device-groups`)

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/device-groups` |  |  | `DeviceGroupDto[]` |
| GET | `/api/device-groups/{id}` |  |  | `DeviceGroupDto` or `404` |
| POST | `/api/device-groups` |  | `DeviceGroupCreate` | `DeviceGroupDto` |
| PUT | `/api/device-groups/{id}` | `If-Match: {version}` | `DeviceGroupUpdate` | `DeviceGroupDto` |
| POST | `/api/device-groups/{id}/activate` | `If-Match: {version}` | empty | `DeviceGroupDto` |
| POST | `/api/device-groups/{id}/deactivate` | `If-Match: {version}` | empty | `DeviceGroupDto` |
| DELETE | `/api/device-groups/{id}` | `If-Match: {version}` |  | `204` |

### People (`/api/people`)

PUT and DELETE here do NOT take `If-Match` (no `Version` on `PersonDto`).

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/people` |  |  | `PersonDto[]` |
| POST | `/api/people` |  | `PersonCreate` | `PersonDto` |
| PUT | `/api/people/{id}` |  | `PersonUpdate` | `PersonDto` |
| DELETE | `/api/people/{id}` |  |  | `204` |

### Test-Groups (`/api/test-groups`)

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/test-groups` |  |  | `TestGroupDto[]` |
| POST | `/api/test-groups` |  | `TestGroupCreate` | `TestGroupDto` |
| PUT | `/api/test-groups/{id}` | `If-Match: {version}` | `TestGroupUpdate` | `TestGroupDto` |
| DELETE | `/api/test-groups/{id}` | `If-Match: {version}` |  | `204` |

### Reservations (`/api/reservations`)

| Method | Path | Headers | Body | Returns |
|---|---|---|---|---|
| GET | `/api/reservations` (filter via query string) |  |  | `ReservationDto[]` |
| POST | `/api/reservations` |  | `ReservationCreate` | `ReservationDto` |
| POST | `/api/reservations/{id}/confirm` | `If-Match: {version}` | empty | `ReservationDto` |
| POST | `/api/reservations/{id}/cancel` | `If-Match: {version}` | empty | `ReservationDto` |

LIST query parameters (all optional; omit when unused, do not pass empty values):

| Parameter | Type | Notes |
|---|---|---|
| `deviceGroupId` | Guid |  |
| `testGroupId` | Guid |  |
| `fromUtc` | ISO 8601 datetime | exclusive lower bound on `EndUtc` |
| `toUtc` | ISO 8601 datetime | exclusive upper bound on `StartUtc` |
| `statusIn` | repeated parameter | one per status, e.g. `?statusIn=Pending&statusIn=Confirmed` |

Example: `/api/reservations?deviceGroupId=...&fromUtc=2026-05-09T12:00:00.0000000Z&statusIn=Pending&statusIn=Confirmed`.

## 4. DTO reference

DTOs live in `src/ResourceScheduler.Components/Models/`. Treat the C# definitions as canonical for field names and types.

| File | Description |
|---|---|
| `BuildingDto.cs` | Building aggregate, plus `BuildingCreate` / `BuildingUpdate` inputs. Has `Version`. |
| `DeviceDto.cs` | Device aggregate, plus `DeviceCreate` / `DeviceUpdate`. Has `Version` and `Status` (string enum). |
| `DeviceGroupDto.cs` | Group aggregate, plus `DeviceGroupCreate` / `DeviceGroupUpdate`, `DeviceConnectionDto`, and `DeviceLayoutEntry`. Has `Version`. |
| `PersonDto.cs` | Person aggregate, plus `PersonCreate` / `PersonUpdate`. No `Version`. |
| `TestGroupDto.cs` | Test-group aggregate, plus `TestGroupCreate` / `TestGroupUpdate`. Has `Version`. |
| `ReservationDto.cs` | Reservation aggregate, plus `ReservationCreate`, `ReservationFilter`. Has `Version` and `Status` (string enum). |

`DeviceGroupDto` carries a `Layout: List<DeviceLayoutEntry>` field that the Designer uses for canvas positions. The server should round-trip it without interpreting it. Each entry is `{ deviceId: Guid, x: double, y: double }` with `x` and `y` normalized to `[0, 1]`.

## 5. Rule catalog

Domain rules are listed in `Docs/SPECIFICATION.md` section 10 (Validation Summary). Any rule violation surfaces as `400 Bad Request` with the `{ ruleId, message }` body.

Rules currently enforced by the in-memory implementation, which the Rust server must match: **R3, R5, R6, R7, R8, R9, R10, R11, R13, R14, R15**.

The Rust server must produce the same `ruleId` values so the client can forward them to UI banners unchanged. Adding new rule ids requires extending the spec and the in-memory implementation in lockstep.

## 6. Verification

`src/ResourceScheduler.Tests/RemoteClientServiceTests.cs` is the executable contract. Those tests record outbound requests through a fake `HttpMessageHandler` and assert:

- **URL structure**: kebab-case plural paths for every list endpoint; correct id placement; correct `/activate`, `/deactivate`, `/confirm`, `/cancel` sub-resource paths.
- **HTTP method**: GET for reads, POST for creates and lifecycle verbs, PUT for updates, DELETE for deletes.
- **`If-Match` header presence**: on PUT, DELETE, and POST lifecycle verbs for versioned aggregates.
- **`If-Match` header absence**: on PUT and DELETE for `/api/people/{id}`, and on every GET.
- **Status-code translation**: `404` to `NotFoundException` (or `null` for GET-by-id), `409` to `ConflictException`, `400` with a structured body to `ValidationException(ruleId, message)`, `400` with an unstructured body to `HttpRequestException`.
- **JSON serialization**: camelCase property names; enums as string names (`"Maintenance"`, not `1`); `DeviceLayoutEntry` round-trips with `deviceId`, `x`, `y`.
- **Reservation filter query string**: `deviceGroupId`, `testGroupId`, ISO 8601 `fromUtc` / `toUtc`, repeated `statusIn` parameters; null filter emits no query string.

Any change to the contract must update those tests. They are the safety net for the Rust team.
