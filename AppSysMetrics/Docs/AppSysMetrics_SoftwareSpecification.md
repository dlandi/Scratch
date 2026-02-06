# AppSysMetrics Software Specification

**Version:** 1.0
**Date:** February 6, 2026
**Target Framework:** .NET 10.0 (SDK 10.0.102)
**Solution:** `AppSysMetrics.slnx`

---

## Table of Contents

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Phase 1 — Real-Time Metrics Dashboard](#3-phase-1--real-time-metrics-dashboard)
4. [Phase 2 — Memory Diagnostics](#4-phase-2--memory-diagnostics)
5. [Project Structure](#5-project-structure)
6. [Data Models](#6-data-models)
7. [Services and Hosting](#7-services-and-hosting)
8. [UI Components](#8-ui-components)
9. [Dependency Inventory](#9-dependency-inventory)
10. [Design Decisions](#10-design-decisions)

---

## 1. Overview

### 1.1 Purpose

AppSysMetrics is a demonstration solution that provides real-time, in-process observability for .NET applications. It captures two distinct classes of runtime metrics:

- **Process-level metrics** — Working set, private memory, virtual memory, thread/handle counts, CPU utilization. These represent the OS view of the process.
- **Managed heap metrics** — GC heap size, fragmentation, generation info, allocation rate, pause time, finalization queue depth. These represent the CLR view of managed memory.

The separation matters because the two can diverge significantly. A process may hold large native buffers invisible to the GC, or the GC may report a small heap while the OS working set remains elevated due to uncommitted pages.

### 1.2 Goals

1. Provide a live dashboard that refreshes every 2 seconds with process, CPU, and GC metrics.
2. Generate controlled memory pressure via an unbounded data generator to simulate real-world leak behavior.
3. Track allocation patterns by type using in-process event tracing, without external profiling tools.
4. Offer on-demand diagnostic actions (Force GC, GC Dump capture) from the browser UI.
5. Render all visualizations as pure SVG — no JavaScript charting dependencies.

### 1.3 Non-Goals

- Production-grade telemetry export (OpenTelemetry, Prometheus, etc.)
- Multi-process or distributed monitoring
- Automated alerting or thresholds
- Authentication or multi-tenant access

---

## 2. Architecture

### 2.1 Single-Process Model

All three projects run in a single OS process. The Travelogue Blazor Server app is the host. The AppSysMetrics and WeatherStatistics libraries are loaded as in-process DLLs. This is critical: the metrics library observes the same managed heap that the weather generator pressures.

```
┌───────────────────────────────────────────────────┐
│  Travelogue Process (dotnet Travelogue.dll)       │
│                                                   │
│  ┌─────────────────┐  ┌────────────────────────┐  │
│  │ WeatherStatistics│  │    AppSysMetrics       │  │
│  │  (memory         │  │  (metrics collection,  │  │
│  │   pressure)      │  │   allocation tracking, │  │
│  │                  │  │   diagnostics)         │  │
│  └─────────────────┘  └────────────────────────┘  │
│                                                   │
│  ┌─────────────────────────────────────────────┐  │
│  │  Blazor Server (Interactive Server Mode)    │  │
│  │  Dashboard · Weather Control · Diagnostics  │  │
│  └─────────────────────────────────────────────┘  │
└───────────────────────────────────────────────────┘
```

### 2.2 Data Flow

```
PeriodicTimer (2s)          PeriodicTimer (2s)
      │                           │
      ▼                           ▼
MetricsCollector            AllocationEventListener
  ├─ Process.GetCurrentProcess()    (EventSource subscriber)
  ├─ CpuSampler                    │
  ├─ AllocationRateTracker          ▼
  └─ GC.GetGCMemoryInfo()    AllocationTrackingService
      │                           │
      ▼                           ▼
  MetricsHub                AllocationTrackingHub
  (ring buffer + event)     (ring buffer + event)
      │                           │
      ▼                           ▼
  Dashboard.razor           MemoryDiagnostics.razor
  (subscribes to OnSnapshot) (subscribes to OnSnapshot)
```

### 2.3 Threading Model

- **MetricsCollectionService** and **AllocationTrackingService** each run on their own `BackgroundService` with a `PeriodicTimer`. They never share a timer because allocation events operate at a different granularity than process/GC polling.
- **AllocationEventListener** receives callbacks on the CLR's event thread. It aggregates into a `ConcurrentDictionary` using `Interlocked` operations, avoiding locks on the hot path.
- **Hub classes** use `lock` on publish/read to protect the ring buffer. The `OnSnapshot` event is invoked outside the lock.
- **Blazor components** subscribe in `OnInitialized` and call `await InvokeAsync(StateHasChanged)` to marshal back to the sync context. All subscriptions are cleaned up in `Dispose`.

---

## 3. Phase 1 — Real-Time Metrics Dashboard

Phase 1 establishes the foundational metrics pipeline and the live dashboard.

### 3.1 Scope

| Capability | Implementation |
|---|---|
| Process memory breakdown | `Process.GetCurrentProcess()` — WorkingSet64, PrivateMemorySize64, VirtualMemorySize64, PagedMemorySize64 |
| Thread and handle counts | `Process.Threads.Count`, `Process.HandleCount` |
| CPU utilization | `TotalProcessorTime` delta / elapsed wall time / `ProcessorCount`, sampled via `Stopwatch.GetElapsedTime()` |
| GC heap overview | `GC.GetGCMemoryInfo()` — HeapSizeBytes, FragmentedBytes, MemoryLoadBytes, PauseTimePercentage |
| GC generation detail | `GcMemoryInfo.GenerationInfo` — size before/after, fragmentation before/after per generation |
| GC collection counts | `GC.CollectionCount(gen)` for Gen 0, 1, 2 |
| Allocation rate | `GC.GetTotalAllocatedBytes(precise: false)` sampled over time, computed as bytes/second |
| Memory pressure generator | Unbounded `List<WeatherReading>` that never trims — simulates a real-world memory leak |
| Dashboard visualizations | Pure SVG charts (bar, line, gauge, metric cards) rendered via `StringBuilder` and `MarkupString` |

### 3.2 Projects Introduced

**AppSysMetrics** (class library)
- `Models/` — Immutable record types for all metric snapshots
- `Collection/` — Stateful samplers (`CpuSampler`, `AllocationRateTracker`) and the orchestrating `MetricsCollector`
- `Hosting/` — `MetricsHub` (singleton event hub with ring buffer), `MetricsCollectionService` (background poller), `MetricsCollectionOptions`
- `Extensions/` — `AddAppSysMetrics()` DI registration

**WeatherStatistics** (class library)
- `Models/` — `WeatherReading` (sealed class, intentionally heap-allocated), `WeatherLocation`, `WeatherCondition`, `WeatherStats`
- `Services/` — `WeatherGeneratorService` implements both `IHostedService` and `IWeatherGenerator`. Uses `PeriodicTimer`. Maintains an unbounded `List<WeatherReading>` as the memory pressure source.
- `Extensions/` — `AddWeatherStatistics()` DI registration. Single instance pattern: one object registered as both `IHostedService` and `IWeatherGenerator`.

**Travelogue** (Blazor Server app)
- `Program.cs` — Wires `AddAppSysMetrics` (2s interval, 60-snapshot history) and `AddWeatherStatistics` (100ms interval, 5 readings/tick, no auto-start)
- `Pages/Dashboard.razor` — Route `/`, subscribes to `MetricsHub.OnSnapshot`, renders four metric panels plus weather stats
- `Pages/WeatherControl.razor` — Route `/weather`, start/stop toggle, interval slider (10–1000ms), readings-per-tick slider (1–50)
- `Charts/` — `BarChart`, `LineChart`, `GaugeChart`, `MetricCard` — all pure SVG via `BuildSvg()` methods
- `Panels/` — `ProcessMetricsPanel`, `CpuMetricsPanel`, `GcMetricsPanel`, `AllocationRatePanel`, `WeatherStatsPanel`

### 3.3 Key Design: SVG Rendering in Razor

Razor's `<text>` directive conflicts with SVG's `<text>` element. All chart components avoid this by building SVG markup in a `private string BuildSvg()` method using `StringBuilder`, then rendering via:

```razor
@((MarkupString)BuildSvg())
```

Similarly, Razor's parser interprets `<` in switch expressions as an HTML open tag. Pattern-matching expressions like `value switch { < 30 => ... }` are replaced with if/else chains in Razor files.

### 3.4 Key Design: CPU Sampling Accuracy

CPU percentage is computed using `Stopwatch.GetElapsedTime()` (not `DateTime.UtcNow`) for the wall-clock delta. This avoids clock drift and provides nanosecond-resolution timing:

```
cpuPercent = (currentCpuTime - previousCpuTime) / elapsedWallTime / processorCount * 100
```

The result is clamped to [0, 100].

### 3.5 Key Design: Memory Pressure Generator

`WeatherReading` is a `sealed class` (not a record struct) so every reading is a separate heap allocation. Each instance holds a `string Summary` property (~100 bytes of formatted text) to increase per-object cost. The `List<WeatherReading>` in `WeatherGeneratorService` is never trimmed, cleared, or bounded. At default settings (5 readings every 100ms = 50/second), the list grows indefinitely, providing a predictable memory ramp visible in the dashboard.

---

## 4. Phase 2 — Memory Diagnostics

Phase 2 adds allocation tracking by type, finalization queue monitoring, and on-demand diagnostic actions.

### 4.1 Scope

| Capability | Tier | Implementation |
|---|---|---|
| Allocation tracking by type | Tier 1 | `AllocationEventListener` subscribing to `Microsoft-Windows-DotNETRuntime` AllocationTick events |
| LOH allocation alerts | Tier 1 | Same listener, filtering `allocationKind == 1` (large object) |
| Finalization queue depth | Tier 1 | `GcMemoryInfo.FinalizationPendingCount` added to `GcMetrics` |
| Force GC with before/after | Tier 2 | `GC.Collect(2, GCCollectionMode.Forced, blocking: true)` + `GC.WaitForPendingFinalizers()`, metrics captured before and after |
| GC Dump capture | Tier 2 | Shells out to `dotnet-gcdump collect -p {pid}`, saves to configured directory |

### 4.2 Additions to AppSysMetrics Library

**New Models:**
- `AllocationTypeInfo` — type name, total bytes, allocation count, LOH flag
- `AllocationSnapshot` — top allocating types, recent LOH allocations, total tracked bytes/count

**New Collection:**
- `AllocationEventListener` (extends `EventListener`) — Subscribes to `Microsoft-Windows-DotNETRuntime` at Verbose level with `GCKeyword` (0x1). Processes `AllocationTick` events (ID 10). Aggregates by type name into `ConcurrentDictionary<string, AllocationAggregation>` using `Interlocked.Add`. Tracks LOH allocations in a bounded `ConcurrentQueue`. `CreateSnapshot()` produces a point-in-time view sorted by total bytes descending.

**New Diagnostics:**
- `DiagnosticsOptions` — configurable `GcDumpOutputDirectory` (defaults to `%TEMP%/AppSysMetrics/gcdumps`)
- `IDiagnosticsService` — `ForceGC()` returns `ForceGcResult` (before/after metrics + duration); `CaptureGcDumpAsync()` returns `GcDumpResult` (success/error + file path + size)
- `DiagnosticsService` — Force GC performs `GC.Collect(2, Forced, blocking)` twice with `WaitForPendingFinalizers()` between. GC Dump shells out to `dotnet-gcdump collect`, verifies the tool is installed, creates the output directory, and returns the `.gcdump` file path.

**New Hosting:**
- `AllocationTrackingHub` — Same ring-buffer + event pattern as `MetricsHub`, but for `AllocationSnapshot`. Separate hub because allocation events have different cadence and lifetime than the 2-second metrics polling.
- `AllocationTrackingService` — `BackgroundService` that calls `AllocationEventListener.CreateSnapshot()` on each tick and publishes via `AllocationTrackingHub`.

**Updated Models:**
- `GcMetrics` — Added `FinalizationPendingCount` property (Tier 1 in-dashboard indicator)

**Updated Collection:**
- `MetricsCollector.Collect()` — Now captures `gcInfo.FinalizationPendingCount`

**Updated DI Registration:**
- `AddAppSysMetrics()` — Registers `AllocationEventListener` (singleton), `AllocationTrackingHub` (singleton), `AllocationTrackingService` (hosted), `DiagnosticsOptions`, `IDiagnosticsService` → `DiagnosticsService` (singleton)

### 4.3 Additions to Travelogue App

**New Panels:**
- `TopAllocationsPanel` — Table of top allocating types with rank, shortened type name, total bytes, allocation count. Takes first 15 entries from `AllocationSnapshot.TopAllocatingTypes`.
- `LargeObjectAllocationsPanel` — Table of recent LOH allocations with warning styling. Shows "No large object allocations detected" when the queue is empty.
- `DiagnosticsPanel` — Two action cards: Force GC button (shows before/after heap comparison with freed bytes) and Capture GC Dump button (shows file path, size, or error message).

**Updated Panels:**
- `GcMetricsPanel` — Added Finalizers `MetricCard` with warning class when `FinalizationPendingCount > 100`.

**New Page:**
- `MemoryDiagnostics.razor` — Route `/diagnostics`. Subscribes to both `AllocationTrackingHub` and `MetricsHub`. Four-cell grid layout: `TopAllocationsPanel`, `LargeObjectAllocationsPanel`, `GcMetricsPanel`, `DiagnosticsPanel`.

**Updated Navigation:**
- `NavMenu.razor` — Added "Memory Diagnostics" link with `bi-activity` icon.

### 4.4 Key Design: EventListener vs ETW

The `AllocationEventListener` uses the in-process `System.Diagnostics.Tracing.EventListener` base class rather than out-of-process ETW or `Microsoft.Diagnostics.NETCore.Client`. Benefits:

- **No NuGet dependency** — EventListener is part of the BCL
- **Cross-platform** — Works on Windows, Linux, macOS
- **In-process** — No elevated permissions or separate collector process required
- **Low overhead** — The Verbose+GCKeyword combination targets only allocation tick events

Trade-off: EventListener receives sampled allocation ticks (approximately every 100KB of allocations), not every individual allocation. This provides a statistical view of allocation patterns, which is sufficient for identifying the dominant allocating types.

### 4.5 Key Design: GC Dump via Tool Shelling

GC dump capture shells out to `dotnet-gcdump collect` rather than using `Microsoft.Diagnostics.NETCore.Client` as a NuGet reference. Rationale:

- Avoids pulling the diagnostics client library and its transitive dependencies into the application
- The `dotnet-gcdump` tool is a well-tested, officially supported global tool
- The `.gcdump` file format is directly openable in Visual Studio's Managed Memory Analyzer
- The tool handles all the IPC complexity of attaching to a running process

The service detects missing tool installation and returns a descriptive error with the install command.

### 4.6 Key Design: Separate Allocation Hub

`AllocationTrackingHub` is a separate singleton from `MetricsHub` rather than extending `MetricsSnapshot` to include allocation data. Reasons:

- **Different cadence** — Allocation snapshots aggregate cumulative event data, which may be meaningful at intervals different from the 2-second metrics poll.
- **Different consumers** — The Dashboard page needs metrics but not allocation details. The Diagnostics page needs allocation details and optionally GC metrics.
- **Independent lifecycle** — Allocation event listening can be started/stopped independently from the metrics collection timer.

---

## 5. Project Structure

```
AppSysMetrics.slnx
│
├── AppSysMetrics/                          (Class Library — net10.0)
│   ├── AppSysMetrics.csproj
│   ├── Collection/
│   │   ├── IMetricsCollector.cs
│   │   ├── MetricsCollector.cs
│   │   ├── CpuSampler.cs
│   │   ├── AllocationRateTracker.cs
│   │   └── AllocationEventListener.cs      (Phase 2)
│   ├── Diagnostics/                        (Phase 2)
│   │   ├── DiagnosticsOptions.cs
│   │   ├── IDiagnosticsService.cs
│   │   └── DiagnosticsService.cs
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs
│   ├── Hosting/
│   │   ├── MetricsCollectionOptions.cs
│   │   ├── MetricsHub.cs
│   │   ├── MetricsCollectionService.cs
│   │   ├── AllocationTrackingHub.cs        (Phase 2)
│   │   └── AllocationTrackingService.cs    (Phase 2)
│   └── Models/
│       ├── MetricsSnapshot.cs
│       ├── ProcessMetrics.cs
│       ├── CpuMetrics.cs
│       ├── GcMetrics.cs
│       ├── GcGenerationInfo.cs
│       ├── AllocationTypeInfo.cs           (Phase 2)
│       └── AllocationSnapshot.cs           (Phase 2)
│
├── WeatherStatistics/                      (Class Library — net10.0)
│   ├── WeatherStatistics.csproj
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs
│   ├── Models/
│   │   ├── WeatherCondition.cs
│   │   ├── WeatherLocation.cs
│   │   ├── WeatherReading.cs
│   │   └── WeatherStats.cs
│   └── Services/
│       ├── IWeatherGenerator.cs
│       ├── WeatherGeneratorOptions.cs
│       └── WeatherGeneratorService.cs
│
├── Travelogue/                             (Blazor Server App — net10.0)
│   ├── Travelogue.csproj
│   ├── Program.cs
│   ├── wwwroot/
│   │   └── app.css
│   └── Components/
│       ├── _Imports.razor
│       ├── App.razor
│       ├── Routes.razor
│       ├── Layout/
│       │   ├── MainLayout.razor (+.css)
│       │   └── NavMenu.razor (+.css)
│       ├── Charts/
│       │   ├── MetricCard.razor (+.css)
│       │   ├── BarChart.razor (+.css)
│       │   ├── LineChart.razor (+.css)
│       │   └── GaugeChart.razor (+.css)
│       ├── Panels/
│       │   ├── ProcessMetricsPanel.razor
│       │   ├── CpuMetricsPanel.razor
│       │   ├── GcMetricsPanel.razor
│       │   ├── AllocationRatePanel.razor
│       │   ├── WeatherStatsPanel.razor
│       │   ├── TopAllocationsPanel.razor          (Phase 2)
│       │   ├── LargeObjectAllocationsPanel.razor  (Phase 2)
│       │   └── DiagnosticsPanel.razor             (Phase 2)
│       └── Pages/
│           ├── Dashboard.razor (+.css)
│           ├── WeatherControl.razor (+.css)
│           └── MemoryDiagnostics.razor (+.css)    (Phase 2)
│
└── Docs/
    └── AppSysMetrics_SoftwareSpecification.md
```

---

## 6. Data Models

All models are in the `AppSysMetrics.Models` namespace. All are `sealed record` types (immutable, value-equality).

### 6.1 MetricsSnapshot

The top-level container produced by `MetricsCollector.Collect()` every 2 seconds.

| Property | Type | Source |
|---|---|---|
| TimestampTicks | long | `Stopwatch.GetTimestamp()` |
| CapturedAt | DateTimeOffset | `DateTimeOffset.UtcNow` |
| Process | ProcessMetrics | `Process.GetCurrentProcess()` |
| Cpu | CpuMetrics | `CpuSampler.Sample()` |
| Gc | GcMetrics | `GC.GetGCMemoryInfo()` + `GC.CollectionCount()` |

### 6.2 ProcessMetrics

| Property | Type | Description |
|---|---|---|
| WorkingSet64 | long | Physical memory (bytes) — what Task Manager shows |
| PrivateMemorySize64 | long | Private committed memory (bytes) |
| VirtualMemorySize64 | long | Total virtual address space (bytes) |
| PagedMemorySize64 | long | Paged memory (bytes) |
| ThreadCount | int | OS thread count |
| HandleCount | int | OS handle count |

### 6.3 CpuMetrics

| Property | Type | Description |
|---|---|---|
| CpuPercentage | double | Sampled CPU % (0–100), normalized by processor count |
| TotalProcessorTime | TimeSpan | Cumulative CPU time since process start |
| ProcessorCount | int | `Environment.ProcessorCount` |

### 6.4 GcMetrics

| Property | Type | Description |
|---|---|---|
| HeapSizeBytes | long | Total managed heap size |
| FragmentedBytes | long | Fragmented bytes across all generations |
| TotalAvailableMemoryBytes | long | Total memory available to the GC |
| MemoryLoadPercent | double | `HeapSizeBytes / TotalAvailableMemoryBytes * 100` |
| TotalMemory | long | `GC.GetTotalMemory(forceFullCollection: false)` |
| TotalAllocatedBytes | long | Cumulative bytes allocated since process start |
| AllocationRateBytesPerSecond | double | Computed allocation rate |
| PauseTimePercentage | double | % of time spent in GC pauses |
| Gen0Collections | int | Gen 0 collection count |
| Gen1Collections | int | Gen 1 collection count |
| Gen2Collections | int | Gen 2 collection count |
| FinalizationPendingCount | long | Objects waiting for finalization (Phase 2) |
| GenerationInfo | IReadOnlyList\<GcGenerationInfo\> | Per-generation size/fragmentation detail |

### 6.5 GcGenerationInfo

| Property | Type | Description |
|---|---|---|
| Generation | int | 0, 1, 2, 3 (LOH), 4 (POH) |
| SizeBeforeBytes | long | Generation size before last collection |
| SizeAfterBytes | long | Generation size after last collection |
| FragmentationBeforeBytes | long | Fragmentation before last collection |
| FragmentationAfterBytes | long | Fragmentation after last collection |

### 6.6 AllocationTypeInfo (Phase 2)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from AllocationTick event |
| TotalBytes | long | Cumulative bytes allocated for this type |
| AllocationCount | int | Number of allocation ticks observed |
| IsLargeObject | bool | True if allocated on the LOH (>= 85,000 bytes) |

### 6.7 AllocationSnapshot (Phase 2)

| Property | Type | Description |
|---|---|---|
| CapturedAt | DateTimeOffset | When the snapshot was taken |
| TopAllocatingTypes | IReadOnlyList\<AllocationTypeInfo\> | Top N types by total bytes (descending) |
| RecentLargeObjectAllocations | IReadOnlyList\<AllocationTypeInfo\> | Recent LOH allocations |
| TotalTrackedBytes | long | Sum of all tracked allocation bytes |
| TotalTrackedCount | int | Sum of all tracked allocation counts |

### 6.8 WeatherStatistics Models

| Type | Kind | Key Properties |
|---|---|---|
| WeatherCondition | enum | Sunny, PartlyCloudy, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy |
| WeatherLocation | sealed record | City, Country, Latitude, Longitude |
| WeatherReading | sealed class | Location, TemperatureCelsius, HumidityPercent, WindSpeedKmh, Condition, Timestamp, Summary |
| WeatherStats | sealed record | TotalReadings, ReadingsPerLocation, ApproximateMemoryBytes, IsRunning, GenerationInterval, ReadingsPerTick |

`WeatherReading` is deliberately a `sealed class` (not a struct or record struct) to ensure each instance is a heap allocation.

---

## 7. Services and Hosting

### 7.1 DI Registration

```csharp
// Program.cs
builder.Services.AddAppSysMetrics(options =>
{
    options.CollectionInterval = TimeSpan.FromSeconds(2);
    options.MaxHistorySize = 60;
});

builder.Services.AddWeatherStatistics(options =>
{
    options.GenerationInterval = TimeSpan.FromMilliseconds(100);
    options.ReadingsPerTick = 5;
    options.AutoStart = false;
});
```

### 7.2 AppSysMetrics Service Registrations

| Service | Lifetime | Interface |
|---|---|---|
| MetricsCollector | Singleton | IMetricsCollector |
| MetricsHub | Singleton | (concrete) |
| MetricsCollectionService | Hosted | BackgroundService |
| AllocationEventListener | Singleton | (concrete) |
| AllocationTrackingHub | Singleton | (concrete) |
| AllocationTrackingService | Hosted | BackgroundService |
| DiagnosticsService | Singleton | IDiagnosticsService |
| MetricsCollectionOptions | Options | IOptions\<T\> |
| DiagnosticsOptions | Options | IOptions\<T\> |

### 7.3 WeatherStatistics Service Registrations

| Service | Lifetime | Interface |
|---|---|---|
| WeatherGeneratorService | Singleton | IWeatherGenerator, IHostedService |
| WeatherGeneratorOptions | Options | IOptions\<T\> |

Single instance pattern: one `WeatherGeneratorService` instance is registered as both `IHostedService` and `IWeatherGenerator` using factory resolution.

### 7.4 Hub Pattern

Both `MetricsHub` and `AllocationTrackingHub` follow the same pattern:

- **Ring buffer**: `List<T>` capped at `MaxHistorySize`, oldest entries removed on overflow
- **Thread safety**: `lock` on publish and read operations
- **Event**: `Action<T>? OnSnapshot` invoked after publish, outside the lock
- **Latest**: Property holding the most recent snapshot for newly subscribing components
- **GetHistory()**: Returns a copy of the ring buffer for chart rendering

### 7.5 Background Services

Both `MetricsCollectionService` and `AllocationTrackingService` use `PeriodicTimer` in `ExecuteAsync`:

```
while (await timer.WaitForNextTickAsync(stoppingToken))
{
    var snapshot = collector/listener.Collect/CreateSnapshot();
    hub.Publish(snapshot);
}
```

Exceptions within the loop are caught and logged, not propagated, to keep the service running.

---

## 8. UI Components

### 8.1 Pages

| Route | Component | Subscribes To | Description |
|---|---|---|---|
| `/` | Dashboard | MetricsHub | 4-panel grid + weather stats. Process, CPU, GC, allocation rate. |
| `/weather` | WeatherControl | IWeatherGenerator | Start/stop toggle, interval/rate sliders, latest readings grid. |
| `/diagnostics` | MemoryDiagnostics | AllocationTrackingHub, MetricsHub | Allocation types, LOH alerts, GC detail, diagnostic actions. |

### 8.2 Chart Components

| Component | Visualization | Rendering |
|---|---|---|
| BarChart | Vertical bars with labels and gridlines | SVG 400x200 viewBox, `BuildSvg()` + `MarkupString` |
| LineChart | Area-fill polyline with stroke and end-point indicator | SVG 400x180 viewBox, `BuildSvg()` + `MarkupString` |
| GaugeChart | 180-degree arc gauge, color-coded by threshold | SVG 200x130 viewBox, `BuildSvg()` + `MarkupString` |
| MetricCard | Title / value / subtitle card | Razor markup, scoped CSS |

All chart components accept parameters for data, titles, units, colors, and ranges. None use JavaScript.

### 8.3 Panel Components

| Panel | Data Source | Key Visuals |
|---|---|---|
| ProcessMetricsPanel | MetricsSnapshot | BarChart (memory breakdown), MetricCards (threads, handles, virtual) |
| CpuMetricsPanel | List\<MetricsSnapshot\> | LineChart (CPU % history), MetricCards (current, processors, total time) |
| GcMetricsPanel | MetricsSnapshot | GaugeChart (memory load %), generation table, MetricCards (collections, pause %, finalizers) |
| AllocationRatePanel | List\<MetricsSnapshot\> | LineChart (allocation rate MB/s), MetricCards (current rate, total allocated) |
| WeatherStatsPanel | IWeatherGenerator | Status indicator, MetricCards (total readings, est. memory), location grid |
| TopAllocationsPanel | AllocationSnapshot | Ranked table of types with monospace type names, bytes, counts |
| LargeObjectAllocationsPanel | AllocationSnapshot | LOH allocation table with alert styling, or "no LOH" indicator |
| DiagnosticsPanel | IDiagnosticsService | Force GC button with before/after comparison, GC Dump button with file path result |

### 8.4 Blazor Component Lifecycle Pattern

All subscribing components follow this pattern:

```csharp
protected override void OnInitialized()
{
    _latest = hub.Latest;
    hub.OnSnapshot += HandleSnapshot;
}

private async void HandleSnapshot(T snapshot)
{
    _latest = snapshot;
    try { await InvokeAsync(StateHasChanged); }
    catch (ObjectDisposedException) { }
}

public void Dispose()
{
    hub.OnSnapshot -= HandleSnapshot;
}
```

The `ObjectDisposedException` catch handles the race condition where a snapshot arrives after the component has been disposed but before the unsubscription takes effect.

---

## 9. Dependency Inventory

### 9.1 NuGet Packages

| Package | Version | Used By |
|---|---|---|
| Microsoft.Extensions.Hosting.Abstractions | 10.0.0-preview.1.25080.5 | AppSysMetrics, WeatherStatistics |
| Microsoft.Extensions.Logging.Abstractions | 10.0.0-preview.1.25080.5 | AppSysMetrics, WeatherStatistics |
| Microsoft.Extensions.Options | 10.0.0-preview.1.25080.5 | AppSysMetrics, WeatherStatistics |

Travelogue has no direct NuGet references; it receives all dependencies transitively through project references and the Web SDK.

### 9.2 CDN Resources

| Resource | Version | Purpose |
|---|---|---|
| Bootstrap | 5.3.3 | Layout grid and base component styling (via jsDelivr) |

### 9.3 External Tools

| Tool | Required By | Install Command |
|---|---|---|
| dotnet-gcdump | DiagnosticsService (GC Dump only) | `dotnet tool install --global dotnet-gcdump` |

The tool is only required for the "Capture GC Dump" feature. All other functionality works without it.

### 9.4 Runtime APIs

| API | Namespace | Purpose |
|---|---|---|
| `Process.GetCurrentProcess()` | System.Diagnostics | Process-level metrics |
| `GC.GetGCMemoryInfo()` | System | Managed heap metrics |
| `GC.GetTotalAllocatedBytes()` | System | Allocation rate tracking |
| `GC.CollectionCount()` | System | Per-generation collection counts |
| `GC.GetTotalMemory()` | System | Quick heap size estimate |
| `GC.Collect()` | System | Force GC (Tier 2) |
| `GC.WaitForPendingFinalizers()` | System | Drain finalization queue (Tier 2) |
| `Stopwatch.GetTimestamp()` | System.Diagnostics | High-resolution timing |
| `Stopwatch.GetElapsedTime()` | System.Diagnostics | Elapsed time computation |
| `EventListener` | System.Diagnostics.Tracing | Allocation event subscription |
| `Environment.ProcessorCount` | System | CPU normalization |
| `Environment.ProcessId` | System | GC dump target PID |

---

## 10. Design Decisions

### 10.1 Why sealed records for metrics?

Records provide value equality and immutable snapshots. `sealed` prevents inheritance overhead. The combination is ideal for data that's created once, published to a hub, and read by multiple consumers — no defensive copying needed.

### 10.2 Why MarkupString + StringBuilder for SVG?

Razor's parser treats `<text>` as a directive, not an HTML/SVG element. Since SVG uses `<text>` extensively for labels, axis values, and gauge readouts, the only clean solution is to build the SVG string outside of Razor's parser and inject it as raw markup. This also avoids Razor issues with `<` in switch expressions used for threshold-based coloring.

### 10.3 Why not use a JavaScript charting library?

The solution demonstrates that Blazor Server can render rich visualizations without any JavaScript interop. The SVG approach has zero JS payload, no npm dependencies, no bundling, and updates instantly via SignalR without client-side re-rendering.

### 10.4 Why an unbounded list for weather readings?

The `List<WeatherReading>` in `WeatherGeneratorService` is intentionally unbounded to simulate a real-world memory leak. This gives the dashboard something meaningful to observe — a steadily growing heap, increasing Gen 2 collections, and `WeatherReading` climbing to the top of the allocation tracking table. The user controls the leak rate via the Weather Control page.

### 10.5 Why two separate hubs?

Metrics snapshots and allocation snapshots serve different diagnostic questions. Metrics answer "how is the process doing right now?" while allocation snapshots answer "what types are consuming the most memory?" Coupling them into a single snapshot would force both collection mechanisms onto the same timer and make the API less composable for consumers that only need one view.

### 10.6 Why shell out for GC dumps instead of using the diagnostics NuGet?

`Microsoft.Diagnostics.NETCore.Client` and its transitive dependencies (`Microsoft.Diagnostics.Runtime`, etc.) add significant binary size and complexity. For a diagnostic feature used infrequently and on-demand, shelling out to an already-installed global tool is a pragmatic trade-off that keeps the library dependency graph clean.
