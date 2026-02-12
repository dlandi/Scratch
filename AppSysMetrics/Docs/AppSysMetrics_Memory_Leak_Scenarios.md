# AppSysMetrics Memory Leak Scenarios

**Version:** 1.0
**Date:** February 12, 2026

> This document maps all 20 memory leak scenarios from **BlazorServerMemLeakResearch01.md** to their LeakLab coverage status. For simulator implementation details, see **AppSysMetrics_SoftwareSpecification.md** §8. For the original research (root causes, repro sketches, detection tools, mitigation patterns), see **BlazorServerMemLeakResearch01.md**.

---

## Coverage Summary

| ID | Scenario | Disposition | Detail |
|---|---|---|---|
| S01 | DotNetObjectReference not disposed | **Implemented** | `S01_DotNetObjectRefSimulator` |
| S02 | JS event listeners/timers not removed | **Omitted** | Server-side mechanics identical to S01; JS-side leak requires browser |
| S03 | Event subscription to long-lived publisher | **Implemented** | `S03_EventHandlerSimulator` |
| S04 | Timer/async loop captures component | **Deferred** | Requires Blazor Server hosting (circuit lifecycle, `StateHasChanged`) |
| S05 | RenderFragment/lambda closure retains large graph | **Implemented** | `S05_ClosureCaptureSimulator` |
| S06 | Large per-circuit state retained by circuit scope | **Implemented** | `S06_LargeCircuitStateSimulator` |
| S07 | Disconnected circuit retention (false positive) | **Not applicable** | Not a true leak — by-design retention behavior |
| S08 | Per-connection state in static dictionary | **Implemented** | `S08_StaticDictionarySimulator` |
| S09 | SignalR streaming CTS accumulation | **Not implemented** | Framework bug (fixed 7.0.13) + pattern overlaps S10 |
| S10 | Middleware instance field retention | **Implemented** | `S10_MiddlewareFieldSimulator` |
| S11 | LOH pressure from huge responses | **Not applicable** | Not a true leak — LOH/GC memory management behavior |
| S12 | DI captive dependency | **Not implemented** | Requires Blazor circuit DI scope for full reproduction |
| S13 | Unbounded IMemoryCache | **Implemented** | `S13_UnboundedCacheSimulator` |
| S14 | Large session state payloads | **Deferred** | Requires full HTTP pipeline with session middleware |
| S15 | Hosted service unbounded accumulation | **Implemented** | `S15_HostedServiceSimulator` |
| S16 | Unbounded channel producer > consumer | **Implemented** | `S16_UnboundedChannelSimulator` |
| S17 | Long-lived DbContext with tracking | **Implemented** | `S17_EfCoreTrackingSimulator` |
| S18 | Streaming/IAsyncEnumerable slow clients | **Not implemented** | Requires HTTP request pipeline with active client connections |
| S19 | HttpClient/stream misuse | **Not implemented** | Native resource leak (sockets/handles), not managed heap growth |
| S20 | GCHandle/pinning/unmanaged allocations | **Not implemented** | Native/unmanaged memory outside ClrMD managed heap analysis |

**Totals:** 10 implemented, 2 deferred, 1 omitted as duplicate, 2 not applicable, 5 not implemented.

---

## Implemented Scenarios

### S01 — DotNetObjectReference Not Disposed

**Simulator:** `S01_DotNetObjectRefSimulator`
**ExpectedLeakTypes:** `DotNetObjectRefTarget`
**Volume:** 300 × 10 KB = 3 MB

Exercises the pattern where `DotNetObjectReference.Create()` wraps a payload object and the reference is never disposed. The simulator stores references in a `List<DotNetObjectReference<DotNetObjectRefTarget>>`, keeping both the wrapper and the target's `byte[] Payload` rooted. The `DotNetObjectReference` API works standalone without `IJSRuntime`, so the simulator runs in a plain `IHost` test fixture. Detection proves the pipeline identifies `DotNetObjectRefTarget` as a leak suspect and traces retention through user code.

### S03 — Event Subscription to Long-Lived Publisher

**Simulator:** `S03_EventHandlerSimulator`
**ExpectedLeakTypes:** `EventSubscriberComponent`
**Volume:** 300 × 10 KB = 3 MB

Exercises the classic managed leak where a component subscribes to a singleton's event but never unsubscribes. `SingletonEventPublisher` (registered as a DI singleton) exposes `event Action<byte[]>`. The simulator creates `EventSubscriberComponent` instances that subscribe to this event — each holds a `byte[] State` field. Because the event's delegate chain holds strong references to all subscribers, the components remain rooted even after the code that created them moves on. Reset unsubscribes all handlers.

### S05 — Lambda Closure Captures Large Graph

**Simulator:** `S05_ClosureCaptureSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** 60 × 50 KB = 3 MB

Exercises the retention pattern where C# closures capture variables by reference, keeping the captured objects alive as long as the delegate exists. The simulator stores `Action` delegates in a `List<Action>` where each lambda captures a fresh `byte[]`. The arrays are reachable only through the closure's compiler-generated display class, demonstrating that the pipeline detects `Byte[]` growth even when the retention path goes through anonymous types.

### S06 — Large Per-Circuit State

**Simulator:** `S06_LargeCircuitStateSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** 40 × 100 KB = 4 MB

Simulates the pattern where per-circuit (or per-session) state accumulates large payloads that are never pruned. The simulator appends `byte[]` payloads to a `List<byte[]>`, modeling what happens when scoped services or component state grow unbounded during a circuit's lifetime. Detection proves the pipeline catches monotonic `Byte[]` growth.

### S08 — Static Dictionary Never Cleaned

**Simulator:** `S08_StaticDictionarySimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** 80 × 50 KB = 4 MB

Exercises the pattern where per-connection (or per-request) state is stored in a `ConcurrentDictionary` with GUID keys and `byte[]` values, but entries are never removed on disconnect. This is the simplest leak pattern — a global collection that only grows. Detection proves the pipeline identifies the heap growth and traces retention through the simulator's dictionary field.

### S10 — Middleware Instance Field Retention

**Simulator:** `S10_MiddlewareFieldSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** 200 × 20 KB = 4 MB

Exercises the pattern where conventional middleware (constructed once per application lifetime) stores per-request data in instance fields. Since the middleware instance lives for the entire application, its `List<byte[]>` field grows with every simulated request. The simulator models this as a `List<byte[]>` that appends on each tick, demonstrating that the pipeline detects accumulation in long-lived singleton-like objects.

### S13 — Unbounded IMemoryCache

**Simulator:** `S13_UnboundedCacheSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** 150 × 30 KB = 4.5 MB

Exercises the pattern where `IMemoryCache` is configured without `SizeLimit` or expiration, causing unbounded growth. The simulator creates a standalone `MemoryCache` instance (not from DI) with `new MemoryCacheOptions()` and adds entries with GUID keys and `byte[]` values. The `CacheEntry` type is recognized by `IsDeveloperFacingFrameworkType`, which allows it through the framework noise filter. Detection proves the pipeline catches cache growth regardless of the cache's internal structure.

### S15 — Hosted Service Unbounded Accumulation

**Simulator:** `S15_HostedServiceSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** ~200 × 25 KB = 5 MB

Exercises the pattern where a background service (`BackgroundService` / `IHostedService`) accumulates state over its lifetime without bound. The simulator launches a `Task.Run` loop that appends `byte[]` payloads to a `List<byte[]>` at 50ms intervals, running continuously between `StartAsync` and `StopAsync`. This is one of two continuous simulators (with S16) that produce ongoing allocations rather than a fixed batch.

### S16 — Unbounded Channel Producer Outpaces Consumer

**Simulator:** `S16_UnboundedChannelSimulator`
**ExpectedLeakTypes:** `System.Byte[]`
**Volume:** ~990 × 20 KB = ~20 MB

Exercises the pattern where `Channel.CreateUnbounded<byte[]>()` is used with a fast producer (10ms delay) and a slow consumer (1000ms delay). The backlog grows rapidly because writes never block. This is the highest-volume simulator, demonstrating that the pipeline detects large-scale heap growth from queue backpressure failures. The second continuous simulator alongside S15.

### S17 — Long-Lived DbContext with Tracking

**Simulator:** `S17_EfCoreTrackingSimulator`
**ExpectedLeakTypes:** `SensorReading`
**Volume:** 600 × 5 KB = 3 MB

Exercises the pattern where a `DbContext` lives too long and change tracking accumulates entity state. The simulator creates an in-memory SQLite database via `LeakLabDbContext` (a `DbContext` subclass with `DbSet<SensorReading>`), keeps the context alive, and inserts `SensorReading` entities with tracking enabled. Since the context is never disposed and `ChangeTracker.Clear()` is never called, every inserted entity remains tracked. Detection proves the pipeline identifies `SensorReading` growth and traces retention through the simulator.

---

## Deferred Scenarios

### S04 — Timer/Async Loop Captures Component

**Research scenario:** A Blazor component starts a `Task.Run` loop or `PeriodicTimer` in `OnInitialized` that captures `this` (the component). When the user navigates away, the loop continues because `CancellationTokenSource.Cancel()` is never called in `Dispose()`. The component instance remains reachable via the running task's closure.

**Why deferred:** This scenario requires Blazor Server hosting with circuit lifecycle — the leak manifests when components are disposed during navigation but captured by still-running async state machines. LeakLab's test fixture uses a plain `IHost` without Blazor circuits, so there is no component disposal trigger and no `StateHasChanged` to call.

**What would be needed:** A test host with `AddRazorComponents().AddInteractiveServerComponents()` and browser automation (Playwright or similar) to drive navigation. The test would navigate in/out of a page repeatedly, then capture heap snapshots to verify component instances accumulate.

### S14 — Large Session State Payloads

**Research scenario:** An endpoint writes large `byte[]` blobs to `HttpContext.Session`. With the default in-memory session provider and 20-minute idle timeout, memory scales with active sessions.

**Why deferred:** This scenario requires the full ASP.NET Core HTTP pipeline with session middleware (`app.UseSession()`), HTTP context, and active requests. LeakLab's plain `IHost` fixture has no HTTP pipeline.

**What would be needed:** A `WebApplicationFactory<T>`-based test host with session middleware configured, and HTTP client calls that populate session state. The test would verify heap growth correlates with session count and that expiration eventually reclaims memory.

---

## Omitted as Duplicate

### S02 — JS Event Listeners/Timers Not Removed

**Research scenario:** A Blazor component imports a JS module via `IJSRuntime.InvokeAsync<IJSObjectReference>` and calls a function that adds DOM event listeners or timers. The component implements `IAsyncDisposable` but its `DisposeAsync` does not call the JS cleanup function or dispose the `IJSObjectReference`.

**Why omitted:** S02 is a dual-sided leak — browser (DOM listeners/timers retained by JS) and server (`IJSObjectReference` wrapper retained by .NET). On the server side, the mechanics are identical to S01: an `IAsyncDisposable` wrapper object (`IJSObjectReference`) holds a reference that is never released, keeping the wrapper and any associated payload rooted. The only aspect unique to S02 is the JS-side browser leak (retained DOM event listeners and timers), which cannot be simulated in LeakLab because the test fixture has no browser.

An S02 simulator would allocate `IJSObjectReference`-like wrappers and store them in a list without disposing — structurally identical to S01's `DotNetObjectReference<T>` pattern. It would prove nothing new about the managed heap detection pipeline. The browser-side leak (the part that makes S02 distinct) requires browser developer tools to detect and is outside the scope of ClrMD-based heap analysis.

---

## Not Applicable

### S07 — Disconnected Circuit Retention (False Positive)

**Research scenario:** Blazor Server retains disconnected circuits for a configurable retention period (default 3 minutes) and max count (default 100). Rapid page refreshes create many circuits; memory rises during the retention window, then drops after eviction plus Gen 2 GC.

**Why not applicable:** This is not a true memory leak. It is by-design retention behavior documented by Microsoft. Memory rises and then falls — the defining characteristic of a false positive. LeakLab tests true leaks (monotonic heap growth from rooted objects that are never released). Including S07 would test the GC and circuit eviction machinery, not the leak detection pipeline.

### S11 — LOH Pressure from Huge Responses (False Positive)

**Research scenario:** A controller returns a very large string (e.g., 50 million characters). The string lands on the Large Object Heap. After the response completes, the object is eligible for collection, but process memory may not drop immediately because the runtime retains committed memory and LOH collection occurs only during Gen 2 GC.

**Why not applicable:** This is not a true memory leak. The objects die (become unreachable) after the response — they are not rooted by long-lived references. The "memory not going down" symptom is LOH fragmentation and GC memory management behavior, not leaked objects. ClrMD heap analysis would correctly show these objects as unreachable between captures, producing no diff signal. LeakLab tests retention (objects that remain reachable), not GC memory management behavior.

---

## Not Implemented

### S09 — SignalR Streaming CTS Accumulation

**Research scenario:** Two sub-patterns: (1) application code creates `CancellationTokenSource` and token registrations for SignalR streaming without disposing them, causing the registration list to grow; (2) a framework-level SignalR client streaming bug (fixed in 7.0.13) caused cancellation token callback accumulation.

**Why not implemented:** The framework bug has been fixed since .NET 7.0.13 and is not reproducible on .NET 8+. The application-level pattern (accumulating `CancellationTokenRegistration` objects in a list without disposing) is structurally similar to S10 (accumulating objects in a long-lived list field). Adding a separate simulator would exercise the same detection path — a growing list in a singleton — with a different payload type. Not distinct enough to justify a separate simulator.

**Future consideration:** Could be added if CTS/registration-specific retention paths prove different enough from generic list accumulation to warrant separate coverage.

### S12 — DI Captive Dependency

**Research scenario:** A singleton service captures a scoped service via constructor injection, promoting the scoped service's lifetime to singleton. In Blazor Server, scoped services live for the circuit, and transient disposables injected into components can be held by the DI container for the circuit lifetime.

**Why not implemented:** The full captive dependency pattern requires Blazor circuit DI scope to demonstrate the circuit-long retention of scoped and transient services. LeakLab's plain `IHost` fixture provides root scope only — there are no per-circuit scopes to capture. The singleton-captures-scoped sub-pattern is partially covered by S08 (a static dictionary acting as implicit singleton state that retains per-connection data), but the DI container's role in extending service lifetimes is not exercised.

**Future consideration:** Could be added with a `WebApplicationFactory<T>`-based test host that creates scoped service providers, simulating circuit scope creation and disposal. The test would verify that a singleton holding a scoped reference prevents the scoped service graph from being collected.

### S18 — Streaming/IAsyncEnumerable Slow Clients

**Research scenario:** An endpoint returns `IAsyncEnumerable<T>`, and slow clients keep the server-side enumerator (and any captured resources like a `DbContext`) alive for the duration of the response.

**Why not implemented:** This scenario requires an active HTTP request pipeline with client connections that read slowly. The leak is not in managed heap retention per se — it is resource exhaustion from long-lived request scopes held open by slow consumers. LeakLab has no HTTP hosting, so there is no way to simulate slow client connections or request-scoped resource retention.

**Future consideration:** Could be added with a `WebApplicationFactory<T>` host and HTTP client that reads response streams with deliberate delays. The test would verify that enumerator state machines and captured `DbContext` instances accumulate proportionally to open connections.

### S19 — HttpClient/Stream Misuse

**Research scenario:** Creating a new `HttpClient` per request (instead of using `IHttpClientFactory` or a shared instance) exhausts sockets. Not disposing response streams leaks native handles.

**Why not implemented:** The primary leak in S19 is native resource exhaustion — socket handles, connection pool entries, and OS-level file descriptors. These are not managed heap objects. ClrMD's `EnumerateObjects` and `EnumerateRoots` analyze the managed heap; they do not detect native handle leaks or socket exhaustion. While `SocketsHttpHandler` instances appear on the managed heap, the actual harm (port exhaustion, `SocketException`) is in the native layer.

**Future consideration:** Would require a different detection approach — monitoring `System.Net.Http` event counters (active connections, DNS lookups) or OS handle counts via `Process.HandleCount`, rather than managed heap snapshots.

### S20 — GCHandle/Pinning/Unmanaged Allocations

**Research scenario:** `GCHandle.Alloc(buf, GCHandleType.Pinned)` without `Free()` keeps the buffer pinned and rooted. Unmanaged memory allocated via `Marshal.AllocHGlobal` or P/Invoke without corresponding `Free` leaks outside the managed heap entirely. Incorrect GC configuration (heap limits, Server vs Workstation GC) can exacerbate memory pressure.

**Why not implemented:** Like S19, the core issue involves native and unmanaged memory that ClrMD's managed heap analysis does not directly observe. While `GCHandle` leaks do keep managed objects rooted (and would appear in heap snapshots), the root cause — a missing `GCHandle.Free()` call — is an interop discipline issue rather than a managed code retention pattern. Unmanaged allocations (`AllocHGlobal`, P/Invoke) are entirely invisible to ClrMD. GC configuration tuning is an operational concern, not a detectable leak.

**Future consideration:** The `GCHandle` sub-pattern could be added as a simulator (pinned arrays retained by a list of `GCHandle` values). The unmanaged memory and GC configuration sub-patterns would require native memory diagnostics tooling beyond ClrMD's scope.
