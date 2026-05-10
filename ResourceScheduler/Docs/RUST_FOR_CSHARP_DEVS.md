# Rust for C# Developers (Resource Scheduler edition)

This is a tour of our Rust crate at [src/Rust/](../src/Rust/) framed for the team's existing C# / .NET 10 mental model. It is **not** a Rust tutorial; it is a one-to-one map from idioms you will see in this codebase to their nearest C# analogues, with one "and here is where it differs" callout per section. When you want to go deeper than the bridge, the [Rust Book](https://doc.rust-lang.org/book/) is linked inline.

The Rust crate is a single binary, `resource-scheduler-api`, that implements the wire contract in [Docs/PHASE2_CONTRACT.md](PHASE2_CONTRACT.md) behind the same `IClientService` seam our C# UI already used. The C# reference implementation it mirrors is [InMemoryClientService.cs](../src/DotNet/ResourceScheduler.Components/Services/InMemoryClientService.cs); read both side-by-side as you go.

## 1. Project shape

[`Cargo.toml`](../src/Rust/Cargo.toml) is the project file. It is what `.csproj` plus `Directory.Packages.props` is in our other repos: package list, version, target edition.

```toml
[package]
name = "resource-scheduler-api"
version = "0.1.0"
edition = "2024"
publish = false

[dependencies]
axum = "0.8"
tokio = { version = "1", features = ["rt-multi-thread", "macros", "signal", "net"] }
sqlx = { version = "0.8", features = ["runtime-tokio", "sqlite", "uuid", "chrono", "migrate", "macros"] }
serde = { version = "1", features = ["derive"] }
thiserror = "2"
anyhow = "1"
```

| Cargo | .NET equivalent |
|---|---|
| `Cargo.toml` | `.csproj` + `Directory.Packages.props` |
| `Cargo.lock` | `packages.lock.json` |
| `cargo build` | `dotnet build` |
| `cargo test` | `dotnet test` |
| `cargo fmt` | `dotnet format` |
| `cargo clippy` | Roslyn analyzers |
| `target/` | `bin/` and `obj/` combined |
| `[dependencies]` | `<PackageReference>` items |
| `[dev-dependencies]` | test-project-only references |
| `features = [...]` | conditional compilation flags chosen at the consumer site |

**Surprise:** Cargo `features` are picked at the **dependency reference**, not at the package itself. The `sqlx` line above turns SQLite, chrono, uuid, and the macro layer on **for our use of sqlx**. Another crate that also depends on sqlx but does not need uuid simply does not list it. There is no NuGet-style "I happen to depend on X.SqlitePack and X.UuidPack" shape; one crate, many opt-in compile-time slices.

Reference: [Rust Book Ch. 14.3 (workspaces, manifest format)](https://doc.rust-lang.org/book/ch14-03-cargo-workspaces.html).

## 2. Module system

Modules are **directories and files**, plus `mod` declarations, not namespaces. The compiler does not auto-discover files; you have to name every child module from its parent.

[`src/lib.rs:9-15`](../src/Rust/src/lib.rs:9):

```rust
pub mod error;
pub mod extractors;
pub mod http;
pub mod models;
pub mod seed;
pub mod state;
pub mod store;
```

[`src/store/mod.rs:1-6`](../src/Rust/src/store/mod.rs:1):

```rust
pub mod buildings;
pub mod device_groups;
pub mod devices;
pub mod people;
pub mod reservations;
pub mod test_groups;
```

| Rust | C# |
|---|---|
| `pub mod foo;` in parent | adding `Foo.cs` to the project (auto in C#) |
| `mod foo;` (no `pub`) | `internal` class in a folder |
| `pub use state::AppState;` at [`lib.rs:7`](../src/Rust/src/lib.rs:7) | re-exporting a type so callers can write `using ResourceScheduler.Api;` instead of the full sub-namespace |
| `crate::error::ServiceError` | fully qualified `ResourceScheduler.Components.Services.ServiceError` |
| `super::enums::ReservationStatus` | `..\Enums\ReservationStatus` (relative) |

Two file conventions both exist:
- `foo.rs` next to a `foo/` directory: that file IS the module.
- `foo/mod.rs`: equivalent older form. We use this in [`http/mod.rs`](../src/Rust/src/http/mod.rs), [`models/mod.rs`](../src/Rust/src/models/mod.rs), [`store/mod.rs`](../src/Rust/src/store/mod.rs).

**Surprise:** Items are private to their containing module unless marked `pub`. `pub mod foo;` makes the module visible from its parent; the items inside still need their own `pub` to be reachable from outside. There is no equivalent to C#'s "internal by default and visible across files in the same assembly". If you want crate-only visibility, write `pub(crate)`.

Reference: [Rust Book Ch. 7](https://doc.rust-lang.org/book/ch07-00-managing-growing-projects-with-packages-crates-and-modules.html).

## 3. Ownership, borrowing, lifetimes

This is the one mental shift that has no C# equivalent. There is no GC. Every value has exactly one owner; passing it transfers ownership; you can lend a reference (`&T` for read, `&mut T` for write) but the compiler tracks at compile time that you do not keep two `&mut` references alive at the same time, and that any `&T` does not outlive its owner.

The signature you will see most often is in [`store/device_groups.rs:394-400`](../src/Rust/src/store/device_groups.rs:394):

```rust
async fn insert_children(
    tx: &mut sqlx::Transaction<'_, sqlx::Sqlite>,
    group_id: Uuid,
    device_ids: &[Uuid],
    connections: &[DeviceConnectionDto],
    layout: &[DeviceLayoutEntry],
) -> ServiceResult<()> {
```

Read it like this:

| Token | Meaning | C# analogue |
|---|---|---|
| `&mut sqlx::Transaction<...>` | exclusive borrow; the caller still owns the transaction, we are allowed to mutate it for the duration of this call | `ref Transaction` (but enforced everywhere, not opt-in) |
| `&[Uuid]` | shared borrow of a slice (a view over a `Vec<Uuid>` or array) | `IReadOnlyList<Guid>` |
| `'_` | "some lifetime, I do not need to name it" | no analogue; the BCL hides this with GC |
| `Uuid` (no `&`) | **moved** by value; copy or transfer of ownership | passing a `struct` by value, except `Uuid` is `Copy`, so it is duplicated rather than transferred |

The compiler will reject code that, for example, holds a `&BuildingDto` returned by a `get` call across an `await` that mutates the same row. In C# you write that bug and find it later; in Rust the borrow checker rejects it before the program runs.

**Surprise:** You will write `&mut **tx` a lot inside transaction code (see [`store/device_groups.rs:408`](../src/Rust/src/store/device_groups.rs:408)). That is not a typo. `tx` is `&mut Transaction`; sqlx wants a mutable executor (`&mut SqliteConnection`); `**tx` dereferences twice to get the connection out, and `&mut` re-borrows it for this one statement. You will write the same pattern in every transactional store fn.

Reference: [Rust Book Ch. 4 (ownership)](https://doc.rust-lang.org/book/ch04-00-understanding-ownership.html), [Ch. 10.3 (lifetimes)](https://doc.rust-lang.org/book/ch10-03-lifetime-syntax.html).

## 4. `Result<T, E>` vs exceptions

Rust has no exceptions. Failure is a return value: `Result<T, E>` is essentially `OneOf<T, E>` made into the standard fallible-call shape. The `?` operator is sugar for "if this is the `Err` arm, convert it through `From` and return it from this function."

Our error type lives in [`error.rs:21-45`](../src/Rust/src/error.rs:21):

```rust
#[derive(Debug, thiserror::Error)]
pub enum ServiceError {
    #[error("not found")]
    NotFound,
    #[error("version conflict")]
    Conflict,
    #[error("rule {rule_id}: {message}")]
    Validation { rule_id: String, message: String },
    #[error("missing or malformed If-Match header")]
    MissingIfMatch,
    #[error("bad request: {0}")]
    BadRequest(String),
    #[error(transparent)]
    Internal(#[from] anyhow::Error),
}

pub type ServiceResult<T> = Result<T, ServiceError>;
```

It maps directly onto the C# exception hierarchy in [Exceptions.cs](../src/DotNet/ResourceScheduler.Components/Models/Exceptions.cs):

| ServiceError variant | C# exception | HTTP status |
|---|---|---|
| `NotFound` | `NotFoundException` | 404 |
| `Conflict` | `ConflictException` | 409 |
| `Validation { rule_id, message }` | `ValidationException(ruleId, message)` | 400 + structured body |
| `MissingIfMatch` / `BadRequest` | (handled at the boundary) | 400 + synthetic ruleId |
| `Internal(_)` | unhandled exception | 500 |

`From<sqlx::Error> for ServiceError` at [`error.rs:56-65`](../src/Rust/src/error.rs:56) is what makes `?` so terse:

```rust
let row = sqlx::query_as::<_, BuildingDto>("SELECT ... WHERE building_id = ?")
    .bind(id)
    .fetch_optional(pool)
    .await?;            // <- `?` converts sqlx::Error into ServiceError via From
```

Mentally substitute "throw on error" for `?`. The difference is that the conversion has to be wired up explicitly via `From`; if no conversion exists, it does not compile, which is the point.

**Surprise:** `?` only works inside a function whose return type is also a `Result` (or `Option`). You cannot scatter `?` through a sync handler that returns `T`; the function signature has to be `-> ServiceResult<T>`, which is why every store fn and every handler in this crate ends in `ServiceResult<...>`.

Reference: [Rust Book Ch. 9](https://doc.rust-lang.org/book/ch09-00-error-handling.html).

## 5. Traits vs interfaces

Traits are interfaces, with one twist: you can implement them for a type **outside** the type's defining module (the "orphan rule" limits this somewhat, but for our own types we control both sides). This is how axum, sqlx, and serde extend our DTOs without inheritance.

[`error.rs:67-100`](../src/Rust/src/error.rs:67) implements `IntoResponse` for `ServiceError`:

```rust
impl IntoResponse for ServiceError {
    fn into_response(self) -> Response {
        match self {
            ServiceError::NotFound => StatusCode::NOT_FOUND.into_response(),
            ServiceError::Conflict => StatusCode::CONFLICT.into_response(),
            ServiceError::Validation { rule_id, message } => (
                StatusCode::BAD_REQUEST,
                Json(ApiErrorBody { rule_id, message }),
            ).into_response(),
            ...
        }
    }
}
```

That is the moral equivalent of an exception filter / `IExceptionHandler` in ASP.NET Core: a single seam that turns a domain error into an HTTP response. axum invokes it for every handler whose `Result` arm is `Err(ServiceError)`.

Derive macros ([`models/buildings.rs:4-6`](../src/Rust/src/models/buildings.rs:4)):

```rust
#[derive(Debug, Clone, Serialize, sqlx::FromRow)]
#[serde(rename_all = "camelCase")]
pub struct BuildingDto {
    pub building_id: Uuid,
    pub name: String,
    pub address: String,
    pub version: i32,
}
```

| Trait derived | C# analogue |
|---|---|
| `Debug` | `ToString()` for diagnostics; what `{ ... }` formatting uses |
| `Clone` | a generated copy constructor / `with` expression |
| `Serialize` (serde) | `[JsonPolymorphic]` / `[JsonPropertyName]` machinery driven by attributes |
| `Deserialize` (serde) | model binding into a record |
| `sqlx::FromRow` | EF entity configuration (column-to-property mapping) |

The custom extractor at [`extractors.rs`](../src/Rust/src/extractors.rs) is the most concrete trait-impl example to study:

```rust
pub struct IfMatch(pub i32);

impl<S: Send + Sync> FromRequestParts<S> for IfMatch {
    type Rejection = ServiceError;
    async fn from_request_parts(parts: &mut Parts, _state: &S) -> Result<Self, Self::Rejection> {
        let raw = parts.headers.get(axum::http::header::IF_MATCH)
            .ok_or(ServiceError::MissingIfMatch)?
            .to_str().map_err(|_| ServiceError::MissingIfMatch)?
            .trim().trim_matches('"');
        let value: i32 = raw.parse().map_err(|_| ServiceError::MissingIfMatch)?;
        Ok(IfMatch(value))
    }
}
```

That is exactly what an ASP.NET Core `IModelBinder` does, but you write it as a trait impl instead of registering a binder in DI. axum sees `IfMatch` in a handler signature and calls this method during request dispatch.

**Surprise:** Traits have no inheritance hierarchy you opt into at the type definition site. C# requires `class Foo : IBar`. Rust lets you write `impl IntoResponse for ServiceError` in a separate file, possibly a separate crate (subject to the orphan rule). The implication: you do not have to anticipate every interface a type might implement when you define it.

Reference: [Rust Book Ch. 10.2](https://doc.rust-lang.org/book/ch10-02-traits.html).

## 6. Pattern matching and enums

Rust enums carry data (sum types). `match` is exhaustive; the compiler will not let you forget a variant. This is C# 12+ switch expressions, more strict.

[`http/buildings.rs:32-40`](../src/Rust/src/http/buildings.rs:32):

```rust
async fn get_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
) -> ServiceResult<Json<BuildingDto>> {
    match store::get(&state.pool, id).await? {
        Some(b) => Ok(Json(b)),
        None => Err(ServiceError::NotFound),
    }
}
```

In C# 12 you would write:

```csharp
return await store.Get(id) switch
{
    BuildingDto b => Json(b),
    null => throw new NotFoundException(...)
};
```

`if let` is the one-arm shorthand ([`store/device_groups.rs:188-197`](../src/Rust/src/store/device_groups.rs:188)):

```rust
if let Some(row) = bad {
    let dev_name: String = row.get("name");
    let dev_status: String = row.get("status");
    return Err(ServiceError::validation("R5", format!(...)));
}
```

`let ... else` is the destructure-or-bail shorthand ([`store/device_groups.rs:34-36`](../src/Rust/src/store/device_groups.rs:34)):

```rust
let Some(row) = row else {
    return Ok(None);
};
```

That is the Rust equivalent of:

```csharp
if (row is not { } r) return null;
```

except `row` itself is rebound on the success path, so subsequent code uses the unwrapped value.

**Surprise:** `match` arms must cover every possibility. Add a fifth `ReservationStatus` variant and every `match` over `ReservationStatus` in the codebase becomes a compile error pointing at the missing arm. C# 12 switch expressions warn at most; Rust hard-fails. We want this. It is the reason rule-violation messages stay in sync when an enum gains a value.

Reference: [Rust Book Ch. 6 (enums)](https://doc.rust-lang.org/book/ch06-00-enums.html), [Ch. 18 (patterns)](https://doc.rust-lang.org/book/ch18-00-patterns.html).

## 7. Async

`#[tokio::main]` at [`main.rs:8`](../src/Rust/src/main.rs:8) is a macro that wraps the function body in a multi-threaded runtime startup. There is no `Program.Main` -> `WebApplication.CreateBuilder()` analogue; tokio is more like a pluggable BCL `ThreadPool`. Both axum and sqlx run on it (see the `runtime-tokio` feature on the sqlx dependency in [`Cargo.toml`](../src/Rust/Cargo.toml)).

```rust
#[tokio::main]
async fn main() -> anyhow::Result<()> {
    init_tracing();
    let state = AppState::connect(&database_url).await?;
    state.run_migrations().await?;
    ...
    axum::serve(listener, app).with_graceful_shutdown(shutdown_signal()).await?;
    Ok(())
}
```

`async fn` and `.await` look identical to C#. Two pieces of context worth carrying over:

| C# `Task<T>` | Rust `Future<Output = T>` |
|---|---|
| eagerly scheduled by default (started on call) | **lazy**: the future does nothing until something `.await`s it or hands it to a runtime via `tokio::spawn` |
| `ConfigureAwait(false)` because of `SynchronizationContext` | no sync context exists; do not look for an analogue |
| `Task.Run` for CPU work | `tokio::task::spawn_blocking` |
| `ValueTask<T>` to avoid allocations | not a thing; futures are already zero-alloc when the compiler can see through them |

axum handlers are `async fn` exactly like minimal-API delegates. Compare [`http/buildings.rs:27-30`](../src/Rust/src/http/buildings.rs:27) to a minimal API:

```rust
async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<BuildingDto>>> {
    let rows = store::list(&state.pool).await?;
    Ok(Json(rows))
}
```

```csharp
app.MapGet("/api/buildings", async (AppState state)
    => Results.Ok(await store.List(state.Pool)));
```

**Surprise:** Forgetting `.await` is a warning, not a runtime hang. The compiler tells you "this `Future` does nothing unless you `.await` it" because the value is unused. C# would happily fire-and-forget a `Task` and you would chase the bug with a profiler.

Reference: [tokio tutorial](https://tokio.rs/tokio/tutorial), [Async Book Ch. 1-3](https://rust-lang.github.io/async-book/).

## 8. axum vs ASP.NET Core minimal APIs

Routing, extractors, and DI are the three parts to compare.

[`lib.rs:17-29`](../src/Rust/src/lib.rs:17) is the equivalent of `Program.cs`:

```rust
pub fn build_app(state: AppState) -> Router {
    Router::new()
        .route("/healthz", get(healthz))
        .merge(http::buildings::router())
        .merge(http::devices::router())
        .merge(http::device_groups::router())
        ...
        .with_state(state)
        .layer(TraceLayer::new_for_http())
        .layer(cors_layer())
}
```

[`http/buildings.rs:18-25`](../src/Rust/src/http/buildings.rs:18) is the per-aggregate router (think of it as a `MapGroup` in minimal APIs):

```rust
pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/buildings", get(list).post(create))
        .route(
            "/api/buildings/{id}",
            get(get_one).put(update).delete(delete_one),
        )
}
```

| axum | minimal APIs |
|---|---|
| `Router::new().route("/x", get(handler))` | `app.MapGet("/x", handler)` |
| `.with_state(state)` | DI registration of singleton in `Program.cs` |
| `.merge(other_router)` | `app.MapGroup("/api/foo", ...)` |
| `.layer(TraceLayer)` | middleware pipeline (`app.UseSerilogRequestLogging()`) |
| `State<AppState>`, `Path<Uuid>`, `Json<T>`, our `IfMatch` | parameter binding from DI / route / body / header |

Side-by-side a real handler. axum at [`http/buildings.rs:50-58`](../src/Rust/src/http/buildings.rs:50):

```rust
async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
    Json(input): Json<BuildingUpdate>,
) -> ServiceResult<Json<BuildingDto>> {
    let row = store::update(&state.pool, id, input, version).await?;
    Ok(Json(row))
}
```

The C# we would otherwise write:

```csharp
app.MapPut("/api/buildings/{id}", async (
    Guid id,
    [FromHeader(Name="If-Match")] int version,
    BuildingUpdate input,
    AppState state) =>
{
    var row = await BuildingStore.Update(state.Pool, id, input, version);
    return Results.Ok(row);
});
```

**Surprise:** All of axum's "magic" is plain trait impls. `State<T>` is `FromRequestParts`, `Json<T>` is `FromRequest`, our `IfMatch` is the same shape ([`extractors.rs:11`](../src/Rust/src/extractors.rs:11)). There is nothing to scan, no source generator, no reflection at startup. Wrong handler signature is a compile error, not a 500 at first request.

Reference: [axum docs](https://docs.rs/axum/latest/axum/), [Tower middleware](https://docs.rs/tower/latest/tower/).

## 9. Persistence: sqlx vs EF Core

There is no LINQ, no change tracking, and no entity graph. You write parameterised SQL and bind values; sqlx maps result rows back into structs that derive `sqlx::FromRow`.

The simple case at [`store/buildings.rs:7-14`](../src/Rust/src/store/buildings.rs:7):

```rust
pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<BuildingDto>> {
    let rows = sqlx::query_as::<_, BuildingDto>(
        "SELECT building_id, name, address, version FROM buildings ORDER BY name",
    )
    .fetch_all(pool)
    .await?;
    Ok(rows)
}
```

| sqlx | EF Core |
|---|---|
| `query_as::<_, T>(sql)` | `Set<T>().FromSqlRaw(sql)` (closer to ADO.NET than to LINQ) |
| `query_scalar(sql)` | `Database.ExecuteSqlRaw` returning a single scalar |
| `.bind(value)` | parameterised `@p0` substitution |
| `.fetch_all(pool)` | `.ToListAsync()` |
| `.fetch_optional(pool)` | `.SingleOrDefaultAsync()` |
| `.fetch_one(pool)` | `.SingleAsync()` |
| `pool.begin()` | `Database.BeginTransactionAsync()` |
| `pool.begin_with("BEGIN IMMEDIATE")` | no equivalent; DbContext does not expose the SQLite begin variant |
| `sqlx::migrate!()` | `Database.Migrate()` |

Optimistic concurrency is **explicit**. There is no `[ConcurrencyCheck]`. We read the row, compare versions ourselves, and write back conditionally. Every versioned aggregate looks like [`store/buildings.rs:44-78`](../src/Rust/src/store/buildings.rs:44):

```rust
pub async fn update(pool: &SqlitePool, id: Uuid, input: BuildingUpdate, expected_version: i32)
    -> ServiceResult<BuildingDto>
{
    let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let new_version = current.version + 1;
    sqlx::query(
        "UPDATE buildings SET name = ?, address = ?, version = ? \
         WHERE building_id = ? AND version = ?",
    )
    .bind(&input.name).bind(&input.address).bind(new_version)
    .bind(id).bind(expected_version)
    .execute(pool).await?;
    ...
}
```

The transaction case at [`store/device_groups.rs:138-265`](../src/Rust/src/store/device_groups.rs:138) is worth reading once. It does five things in one transaction: read current row + version, count members (R7), check device statuses (R5), check membership clashes against other Active groups (R3), then update. It opens with `pool.begin_with("BEGIN IMMEDIATE")` instead of the default `pool.begin()`; see Section 14 for why.

Migrations live in [src/Rust/migrations/](../src/Rust/migrations/) and are picked up by `sqlx::migrate!()` at [`state.rs:42-48`](../src/Rust/src/state.rs:42). They are filename-ordered (`0001_init.sql`, `0002_devices.sql`, ...) and **content-hashed**. The hash invalidates if you edit the file, which we hit; see Section 14.

**Surprise:** No change tracking means returning the updated DTO requires you to construct it manually after the UPDATE, often by re-using fields from the row you read at the top. See the `..current` syntax at [`store/reservations.rs:266-270`](../src/Rust/src/store/reservations.rs:266) for the spread-operator-like shorthand:

```rust
Ok(ReservationDto {
    status: ReservationStatus::Confirmed,
    version: new_version,
    ..current   // copy every other field from the original
})
```

Reference: [sqlx README](https://github.com/launchbadge/sqlx).

## 10. serde vs System.Text.Json

serde is the JSON / TOML / YAML / anything-text-shaped serialiser. The two attributes you will see on every DTO are `#[derive(Serialize, Deserialize)]` and `#[serde(rename_all = "camelCase")]`.

[`models/buildings.rs:4-11`](../src/Rust/src/models/buildings.rs:4):

```rust
#[derive(Debug, Clone, Serialize, sqlx::FromRow)]
#[serde(rename_all = "camelCase")]
pub struct BuildingDto {
    pub building_id: Uuid,
    pub name: String,
    pub address: String,
    pub version: i32,
}
```

The C# client side ([`RemoteClientService.cs:23-27`](../src/DotNet/ResourceScheduler.Components/Services/RemoteClientService.cs:23)) has to agree:

```csharp
private readonly JsonSerializerOptions _json = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() }
};
```

Both sides emit camelCase keys. Both sides serialise enum **names** rather than ordinals. The Rust side gets the latter from the default serde derive on the unit-variant enums in [`models/enums.rs`](../src/Rust/src/models/enums.rs). The C# side gets it from `JsonStringEnumConverter`. If either side drops its converter, every reservation status round-trip silently breaks.

| serde attribute | System.Text.Json analogue |
|---|---|
| `#[derive(Serialize, Deserialize)]` | the type is implicitly serialisable; no attribute needed |
| `#[serde(rename_all = "camelCase")]` | `JsonNamingPolicy.CamelCase` on options |
| `#[serde(rename = "deviceGroupId")]` (per-field) | `[JsonPropertyName("deviceGroupId")]` |
| `Option<String>` | `string?` |
| `#[serde(skip_serializing_if = "Option::is_none")]` | `JsonIgnoreCondition.WhenWritingNull` |

**Surprise:** serde derives are **structural at compile time**. Adding a field to `BuildingDto` and forgetting the matching `building_id` column in SQL is caught only at runtime by sqlx; but the JSON shape changes immediately, and C# will fail to deserialise. There is no reflection cushion. Treat DTO and SQL changes as one PR.

Reference: [serde overview](https://serde.rs/).

## 11. Error library trio: `thiserror`, `anyhow`, `From`

Two error libraries appear in the same project for two different jobs.

`thiserror` at [`error.rs:21`](../src/Rust/src/error.rs:21) gives us a typed enum where each variant is a documented failure mode. The `#[error("...")]` attribute on each variant generates the `Display` impl. We use this for `ServiceError` because handlers branch on the variant to choose an HTTP status.

`anyhow` is the catch-all. It is what `dynamic` is to C#: a single `anyhow::Error` type that can wrap any other error, with a stack of context strings attached. We use it in two places:
1. The binary entry point at [`main.rs:9`](../src/Rust/src/main.rs:9) returns `anyhow::Result<()>` because at top level, the only meaningful behaviour is "log and exit".
2. The `Internal` arm of `ServiceError` at [`error.rs:43-44`](../src/Rust/src/error.rs:43) wraps an `anyhow::Error`, so anything that ends up there can carry rich context to the log line at [`error.rs:96`](../src/Rust/src/error.rs:96).

The `From<sqlx::Error> for ServiceError` impl at [`error.rs:56-65`](../src/Rust/src/error.rs:56) is what makes the `?` operator quiet; without it, every store fn would have to call `.map_err(...)` explicitly.

| Library | When to use |
|---|---|
| `thiserror` | typed errors that callers branch on. Library code, including our `ServiceError`. |
| `anyhow` | "wrap anything, attach context, return up" errors at the binary boundary, or in tests. |
| handwritten `From` impls | bridge between a third-party error type and your typed error |

**Surprise:** A library crate that exposes `anyhow::Error` to its callers is considered bad form, because callers cannot match on the cause. We follow the rule: `lib.rs` returns `ServiceResult<T>`, `main.rs` returns `anyhow::Result<T>`. If you find yourself reaching for `anyhow` inside `lib.rs`, either add a typed variant to `ServiceError` or wrap it with `.context(...)` and let `Internal` carry it.

Reference: [thiserror docs](https://docs.rs/thiserror), [anyhow docs](https://docs.rs/anyhow).

## 12. Tests

`#[tokio::test]` is `[Fact]` for async. Our tests live in [src/Rust/tests/](../src/Rust/tests/), one file per aggregate (like an xUnit project per area). Rust also lets you put unit tests inline at the bottom of a source file inside a `#[cfg(test)] mod tests { ... }` block when you need access to private items, but we have not had to reach for that yet; everything is testable from outside via the HTTP surface.

[`tests/buildings.rs:38-52`](../src/Rust/tests/buildings.rs:38):

```rust
#[tokio::test]
async fn list_starts_empty() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(Request::builder()
            .method("GET").uri("/api/buildings")
            .body(Body::empty()).unwrap())
        .await.unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    assert_eq!(body_bytes(resp).await, b"[]");
}
```

`fresh_app()` at [`tests/buildings.rs:8-12`](../src/Rust/tests/buildings.rs:8) is the Rust answer to `WebApplicationFactory<TStartup>`:

```rust
async fn fresh_app() -> axum::Router {
    let state = AppState::in_memory().await.expect("in-memory pool");
    state.run_migrations().await.expect("migrations");
    build_app(state)
}
```

`AppState::in_memory()` at [`state.rs:31-40`](../src/Rust/src/state.rs:31) opens a SQLite `:memory:` pool pinned to one connection so each test gets its own database.

`tower::ServiceExt::oneshot` at [`tests/buildings.rs:6`](../src/Rust/tests/buildings.rs:6) is the Rust analogue of `WebApplicationFactory.CreateClient().SendAsync(...)`: feed the `Router` a `Request<Body>` directly, get a `Response<Body>` back, no socket involved.

| Rust | xUnit + ASP.NET Core |
|---|---|
| `#[tokio::test]` | `[Fact]` on an `async Task` method |
| `assert_eq!(a, b)` | `Assert.Equal(a, b)` |
| `app.clone().oneshot(...)` | `factory.CreateClient().SendAsync(...)` |
| `AppState::in_memory()` | `WebApplicationFactory` with overridden `IDbContextFactory` |
| no fixtures | `IClassFixture<T>` |

**Surprise:** No fixture concept. Each `#[tokio::test]` is independent; setup is whatever helper function the test calls. There is also no `[Theory]` analogue without a third-party crate (`rstest`). When you need parameterised tests, write a loop, or a macro, or pull in `rstest`.

Reference: [Rust Book Ch. 11](https://doc.rust-lang.org/book/ch11-00-testing.html).

## 13. Cargo workflow and CI cache

Day-to-day commands, run from [src/Rust/](../src/Rust/):

| Command | What it does |
|---|---|
| `cargo build` | debug build into `target/debug/` |
| `cargo build --release` | release build into `target/release/` |
| `cargo test` | runs unit tests (in-file `#[cfg(test)]` blocks) and integration tests (in `tests/`) |
| `cargo test --all-targets` | also runs example and benchmark targets if present |
| `cargo fmt --all --check` | format check, fails if anything needs reformatting |
| `cargo clippy --all-targets -- -D warnings` | lints; we treat warnings as errors in CI |
| `cargo clean` | nuke `target/` |

`Cargo.lock` is committed (we ship a binary). Cargo resolves it on first build; CI uses it as-is. The CI cache key in [.github/workflows/build-test.yml:51](../.github/workflows/build-test.yml:51):

```yaml
key: cargo-${{ runner.os }}-${{ hashFiles('ResourceScheduler/src/Rust/Cargo.lock', 'ResourceScheduler/src/Rust/Cargo.toml', 'ResourceScheduler/src/Rust/rust-toolchain.toml') }}
```

is keyed on every input that changes the build graph. There is no `restore-keys`, on purpose: a cache miss must be a clean rebuild rather than reuse a `target/` snapshot built against a different lockfile. The comment block above that key is worth reading; it captures the lesson from a CI flake we hit early.

**Surprise:** `cargo test` runs every `#[cfg(test)] mod tests { ... }` block across the crate, plus every file in `tests/`. Adding a unit test to a `store/*.rs` file does not require touching a project file; the compiler picks it up on next build.

Reference: [Cargo Book](https://doc.rust-lang.org/cargo/).

## 14. Gotchas already hit on this codebase

Each one references the commit that introduced or fixed the issue.

### 14.1 sqlx migrations are content-hashed; do not edit applied migrations

`sqlx::migrate!()` at [`state.rs:43`](../src/Rust/src/state.rs:43) records a SHA of every migration file the **first** time it runs against a database. On subsequent runs, mismatched hashes abort startup. **Even a comment edit invalidates the hash.** This came up during the Phase 2 review when reformatting an already-applied migration would have broken every developer's local database.

The rule: once a migration has shipped, treat the file as immutable. If you need to change applied SQL, write a new `0007_*.sql` migration that does the change. Edit applied files only on a fresh database (`rm resource-scheduler.db`).

### 14.2 SQLite TEXT date storage and lex-vs-temporal ordering

SQLite has no native datetime type. We store as TEXT, and the comparison operators run on the bytes. `fmt_utc` at [`store/reservations.rs:19-21`](../src/Rust/src/store/reservations.rs:19) exists to fix this:

```rust
pub(crate) fn fmt_utc(dt: DateTime<Utc>) -> String {
    dt.format("%Y-%m-%dT%H:%M:%S%.9fZ").to_string()
}
```

sqlx's default RFC 3339 encoder picks variable precision: no fractional digits for whole seconds, otherwise milli/micro/nano. The `.` before the fraction sorts after `Z` (no fraction), so two timestamps one second apart could lex-order backwards. Forcing nine-digit fractional precision on every value makes lexicographic order match temporal order, which is what `ORDER BY start_utc` and the `<` / `>` comparisons in [`store/reservations.rs:46-51`](../src/Rust/src/store/reservations.rs:46) and the R10 / R11 overlap checks all rely on.

### 14.3 `BEGIN IMMEDIATE` for write-after-read patterns

SQLite's default transaction is **deferred**: the first read takes a SHARED lock; the first write upgrades to RESERVED; if two transactions are racing the upgrade, one fails with `SQLITE_BUSY`. Worse for us, two concurrent `activate` calls reading each other's still-Inactive state could both pass R3 and both commit, violating R1 (one Active group per device).

The fix is `pool.begin_with("BEGIN IMMEDIATE")` at [`store/device_groups.rs:149`](../src/Rust/src/store/device_groups.rs:149), which acquires the RESERVED write lock up front. Use this pattern for any handler whose reads inform a subsequent write under a domain rule. The default `pool.begin()` at [`store/device_groups.rs:272`](../src/Rust/src/store/device_groups.rs:272) is fine for `deactivate` because the read is only the version check, not a multi-row invariant.

### 14.4 Optimistic concurrency without an ORM

Every versioned aggregate explicitly does **read, compare, conditional-UPDATE** by hand. There is no `[Timestamp]` magic. The pattern is uniform across [`store/buildings.rs`](../src/Rust/src/store/buildings.rs), [`store/devices.rs`](../src/Rust/src/store/devices.rs), [`store/device_groups.rs`](../src/Rust/src/store/device_groups.rs), [`store/test_groups.rs`](../src/Rust/src/store/test_groups.rs), and [`store/reservations.rs`](../src/Rust/src/store/reservations.rs); the post-Phase-2 cleanup ([`287d9f4`](../)) aligned the four that drifted. When you add a new versioned aggregate, mirror the [`store/buildings.rs:44-78`](../src/Rust/src/store/buildings.rs:44) shape exactly:

```rust
let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
if current.version != expected_version { return Err(ServiceError::Conflict); }
let new_version = current.version + 1;
sqlx::query("UPDATE ... SET ... version = ? WHERE id = ? AND version = ?")
    .bind(new_version).bind(id).bind(expected_version)
    .execute(pool).await?;
```

The `AND version = ?` clause is belt-and-suspenders; the version compare-then-update pattern catches the race even when the row is read against the pool rather than inside a transaction. Inside a transaction (Section 14.3), it is the only line that matters.

---

If the team finds gaps in this primer, edit it in place. The point is to be the document a new C# dev opens before touching the Rust crate.
