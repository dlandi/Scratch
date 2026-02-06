# AppSysMetrics Software Specification

**Version:** 4.0
**Date:** February 6, 2026
**Target Framework:** .NET 10.0 (SDK 10.0.102)
**Package:** Razor Class Library (`Microsoft.NET.Sdk.Razor`)

---

## Table of Contents

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Phase 1 — Real-Time Metrics Dashboard](#3-phase-1--real-time-metrics-dashboard)
4. [Phase 2 — Memory Diagnostics](#4-phase-2--memory-diagnostics)
5. [Phase 3 — Razor Class Library Packaging](#5-phase-3--razor-class-library-packaging)
6. [Phase 4 — Dump Analysis and Memory Leak Detection](#6-phase-4--dump-analysis-and-memory-leak-detection)
7. [Project Structure](#7-project-structure)
8. [Data Models](#8-data-models)
9. [Services and Hosting](#9-services-and-hosting)
10. [UI Components](#10-ui-components)
11. [Dependency Inventory](#11-dependency-inventory)
12. [Design Decisions](#12-design-decisions)

---

## 1. Overview

### 1.1 Purpose

AppSysMetrics is a self-contained Razor Class Library that provides real-time, in-process observability for .NET applications. A single project reference gives consumers both the metrics backend (collection, diagnostics) and a full set of Blazor UI components (charts, panels, composite dashboard views). It captures two distinct classes of runtime metrics:

- **Process-level metrics** — Working set, private memory, virtual memory, thread/handle counts, CPU utilization. These represent the OS view of the process.
- **Managed heap metrics** — GC heap size, fragmentation, generation info, allocation rate, pause time, finalization queue depth. These represent the CLR view of managed memory.

The separation matters because the two can diverge significantly. A process may hold large native buffers invisible to the GC, or the GC may report a small heap while the OS working set remains elevated due to uncommitted pages.

### 1.2 Goals

1. Provide a live dashboard that refreshes every 2 seconds with process, CPU, and GC metrics.
2. Track allocation patterns by type using in-process event tracing, without external profiling tools.
3. Offer on-demand diagnostic actions (Force GC, GC Dump capture) from the browser UI.
4. Render all visualizations as pure SVG — no JavaScript charting dependencies.
5. Ship as a single Razor Class Library — one project reference provides both backend services and Blazor UI components.
6. Automatically detect, analyze, and diff GC dump files for memory leak detection — no manual Visual Studio inspection required.

### 1.3 Non-Goals

- Production-grade telemetry export (OpenTelemetry, Prometheus, etc.)
- Multi-process or distributed monitoring
- Automated alerting or thresholds
- Authentication or multi-tenant access

---

## 2. Architecture

### 2.1 Single-Process Model

AppSysMetrics runs in-process with the host application. It observes the same managed heap, threads, and handles as the host's own workload. This is by design — the library provides self-observation, not remote monitoring.

```
┌───────────────────────────────────────────────────┐
│  Host Application Process                         │
│                                                   │
│  ┌─────────────────┐  ┌────────────────────────┐  │
│  │ Application      │  │    AppSysMetrics       │  │
│  │  workload        │  │  (metrics collection,  │  │
│  │  (observed by    │  │   allocation tracking, │  │
│  │   the library)   │  │   diagnostics,         │  │
│  │                  │  │   dump analysis,       │  │
│  │                  │  │   Blazor UI components)│  │
│  └─────────────────┘  └────────────────────────┘  │
│                                                   │
│  ┌─────────────────────────────────────────────┐  │
│  │  Blazor (Interactive Server / WebAssembly)  │  │
│  │  Dashboard · Diagnostics · Dump Analysis    │  │
│  └─────────────────────────────────────────────┘  │
└───────────────────────────────────────────────────┘
```

### 2.2 Library Packaging Model (Phase 3)

AppSysMetrics is a Razor Class Library (`Microsoft.NET.Sdk.Razor`). It ships:

| Layer | Contents | Consumer Uses |
|---|---|---|
| **Backend** | Models, Collection, Hosting, Diagnostics, Extensions | `builder.Services.AddAppSysMetrics()` |
| **Primitives** | BarChart, LineChart, GaugeChart, MetricCard | Mix-and-match in custom layouts |
| **Panels** | ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel, TopAllocationsPanel, LargeObjectAllocationsPanel, DiagnosticsPanel, DumpAnalysisPanel, DumpDiffPanel, DumpHistoryPanel | Drop individual panels into existing pages |
| **Composites** | MetricsDashboardView, MemoryDiagnosticsView, DumpAnalysisView | Full dashboard experience with one tag |
| **Stylesheet** | `_content/AppSysMetrics/AppSysMetrics.css` | Shared component styles (panels, tables, buttons) |

The host application becomes a thin wrapper: route declarations, render mode selection, and any app-specific components.

### 2.3 Data Flow

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
  MetricsDashboardView      MemoryDiagnosticsView
  (subscribes to OnSnapshot) (subscribes to OnSnapshot)

FileSystemWatcher (*.gcdump)
      │ Created event
      ▼
Channel<string> → DumpWatcherService
      │
      ├─ WaitForFileReady()
      ├─ DumpAnalyzerService.AnalyzeAsync()
      │    └─ dotnet-gcdump report → DumpReportParser
      ├─ DumpAnalysisHub.Publish(result)
      └─ DumpDiffService.ComputeDiff()
           └─ DumpAnalysisHub.PublishDiff(diff)
                  │
                  ▼
           DumpAnalysisView
           (subscribes to OnAnalysis + OnDiff)
```

### 2.4 Threading Model

- **MetricsCollectionService** and **AllocationTrackingService** each run on their own `BackgroundService` with a `PeriodicTimer`. They never share a timer because allocation events operate at a different granularity than process/GC polling.
- **AllocationEventListener** receives callbacks on the CLR's event thread. It aggregates into a `ConcurrentDictionary` using `Interlocked` operations, avoiding locks on the hot path.
- **Hub classes** use `lock` on publish/read to protect the ring buffer. The `OnSnapshot` event is invoked outside the lock.
- **DumpWatcherService** uses a `FileSystemWatcher` that raises events on a thread pool thread. Events are bridged to an `async` processing loop via `Channel<string>`, which provides natural async enumeration with cancellation token support.
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
| Dashboard visualizations | Pure SVG charts (bar, line, gauge, metric cards) rendered via `StringBuilder` and `MarkupString` |

### 3.2 Library Structure Introduced

- `Models/` — Immutable record types for all metric snapshots
- `Collection/` — Stateful samplers (`CpuSampler`, `AllocationRateTracker`) and the orchestrating `MetricsCollector`
- `Hosting/` — `MetricsHub` (singleton event hub with ring buffer), `MetricsCollectionService` (background poller), `MetricsCollectionOptions`
- `Extensions/` — `AddAppSysMetrics()` DI registration

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

### 4.3 New UI Panels

- `TopAllocationsPanel` — Table of top allocating types with rank, shortened type name, total bytes, allocation count. Takes first 15 entries from `AllocationSnapshot.TopAllocatingTypes`.
- `LargeObjectAllocationsPanel` — Table of recent LOH allocations with warning styling. Shows "No large object allocations detected" when the queue is empty.
- `DiagnosticsPanel` — Two action cards: Force GC button (shows before/after heap comparison with freed bytes) and Capture GC Dump button (shows file path, size, or error message).
- `GcMetricsPanel` — Updated with Finalizers `MetricCard` with warning class when `FinalizationPendingCount > 100`.

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

## 5. Phase 3 — Razor Class Library Packaging

Phase 3 converts AppSysMetrics from a plain class library into a Razor Class Library (RCL), packaging all UI components alongside the backend services.

### 5.1 Motivation

Before Phase 3, consuming AppSysMetrics required copying chart components, panel components, and CSS separately. After Phase 3, a consumer adds one project reference and gets both the metrics backend and the full UI:

```csharp
// Program.cs
builder.Services.AddAppSysMetrics();
```

```html
<!-- App.razor / _Host.cshtml -->
<link rel="stylesheet" href="_content/AppSysMetrics/AppSysMetrics.css" />
```

```razor
<!-- Any page -->
@page "/dashboard"
@rendermode InteractiveServer
<MetricsDashboardView />
```

### 5.2 SDK Change

The `AppSysMetrics.csproj` SDK changed from `Microsoft.NET.Sdk` to `Microsoft.NET.Sdk.Razor`. A `FrameworkReference` to `Microsoft.AspNetCore.App` replaces the three explicit NuGet package references (`Hosting.Abstractions`, `Logging.Abstractions`, `Options`), which are all included in the shared framework.

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
</Project>
```

### 5.3 Library Components

All chart and panel components live in `AppSysMetrics/Components/`:

| Component | Path | Notes |
|---|---|---|
| BarChart | Components/Charts/BarChart.razor (+.css) | Self-contained scoped CSS |
| LineChart | Components/Charts/LineChart.razor (+.css) | Self-contained scoped CSS |
| GaugeChart | Components/Charts/GaugeChart.razor (+.css) | Self-contained scoped CSS |
| MetricCard | Components/Charts/MetricCard.razor (+.css) | Self-contained scoped CSS |
| ProcessMetricsPanel | Components/Panels/ | |
| CpuMetricsPanel | Components/Panels/ | |
| GcMetricsPanel | Components/Panels/ | Scoped .razor.css for layout |
| AllocationRatePanel | Components/Panels/ | |
| TopAllocationsPanel | Components/Panels/ | |
| LargeObjectAllocationsPanel | Components/Panels/ | Scoped .razor.css for LOH alert |
| DiagnosticsPanel | Components/Panels/ | Scoped .razor.css for diagnostics layout |

### 5.4 New Composite View Components

Two composite components encapsulate the hub subscription logic and grid layout:

**`MetricsDashboardView`** (`AppSysMetrics.Components.Views`)
- Injects `MetricsHub`, subscribes to `OnSnapshot`
- Renders a 2-column grid: ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel
- Accepts `[Parameter] RenderFragment? AdditionalContent` for consumer-injected panels
- No `@page` directive — routing is the consumer's responsibility
- No `@rendermode` directive — render mode is the consumer's choice

**`MemoryDiagnosticsView`** (`AppSysMetrics.Components.Views`)
- Injects `AllocationTrackingHub` and `MetricsHub`, subscribes to both `OnSnapshot` events
- Renders a 2-column grid: TopAllocationsPanel, LargeObjectAllocationsPanel, GcMetricsPanel, DiagnosticsPanel
- No `@page` or `@rendermode` directives

Both views use `asm-` prefixed CSS class names (e.g., `.asm-grid`, `.asm-cell`, `.asm-subtitle`) in their scoped `.razor.css` files to avoid collisions with consumer stylesheets.

### 5.5 CSS Strategy

The library uses a two-tier CSS approach:

**Tier 1: Shared base stylesheet** — `AppSysMetrics/wwwroot/AppSysMetrics.css`
Served at `_content/AppSysMetrics/AppSysMetrics.css`. Contains styles shared across multiple panels:
- `.panel`, `.panel-heading`, `.panel-loading` — Panel container styles
- `.metric-row` — Flexbox row for MetricCard groups
- `.metric-ok`, `.metric-warning`, `.metric-danger` — State border colors
- `.metric-ok-text`, `.metric-danger-text` — State text colors
- `.gen-table`, `.gen-table-wrapper` — Data table styles (th, td, hover)
- `.type-name` — Monospace type name display
- `.alloc-table-wrapper`, `.alloc-summary` — Allocation table layout
- `.btn`, `.btn-warning`, `.btn-info` — Button styles

The consumer must add one `<link>` tag to include these shared styles.

**Tier 2: Scoped component CSS** — `.razor.css` per component
Automatically bundled into `AppSysMetrics.styles.css` by Blazor's CSS isolation. Contains layout-specific styles unique to each panel:
- `GcMetricsPanel.razor.css` — `.gc-top-row`, `.gc-counters` layout
- `DiagnosticsPanel.razor.css` — `.diag-actions`, `.gc-comparison`, `.dump-result` layout
- `LargeObjectAllocationsPanel.razor.css` — `.loh-alert` styling
- `MetricsDashboardView.razor.css` — `.asm-grid`, `.asm-cell`, `.asm-subtitle` layout
- `MemoryDiagnosticsView.razor.css` — Same grid layout pattern

All chart components already had self-contained scoped CSS from Phase 1.

### 5.6 Consumer Integration Pattern

A typical host application becomes a thin wrapper. Each page is a few lines: a `@page` directive, a `@rendermode` directive, and one library view tag. For example:

```razor
@page "/dashboard"
@rendermode InteractiveServer
<MetricsDashboardView />
```

The consumer controls routing, render mode, layout, and navigation. The library's composite views handle all hub subscriptions and panel arrangement internally.

### 5.7 Key Design: One Library, Not Two

The existing `AppSysMetrics.csproj` was converted in-place rather than creating a separate `AppSysMetrics.Blazor` package. Rationale:

- The Razor SDK is additive — all existing C# code compiles identically
- A single package avoids version coordination between a "core" and "UI" package
- Consumers who only need the backend can ignore the Components namespace; the Razor components add negligible binary size
- The `FrameworkReference` to `Microsoft.AspNetCore.App` replaces all three explicit NuGet packages, resulting in a cleaner `.csproj`

### 5.8 Key Design: Composite Views Without @page

The library's `MetricsDashboardView` and `MemoryDiagnosticsView` deliberately omit `@page` and `@rendermode` directives. This gives consumers full control over:

- **Route paths** — The consumer chooses where the dashboard lives (`/`, `/metrics`, `/admin/diagnostics`, etc.)
- **Render mode** — InteractiveServer, InteractiveWebAssembly, InteractiveAuto
- **Layout** — The consumer wraps the view in their own layout with their own navigation
- **Extensibility** — `RenderFragment? AdditionalContent` allows injecting app-specific panels into the dashboard grid

### 5.9 Key Design: No Bootstrap Dependency in Library

The library components use zero Bootstrap CSS classes. All panel styling is self-contained via the shared `AppSysMetrics.css` and per-component scoped CSS. This makes the library portable to any CSS framework or custom design system — Bootstrap, Tailwind, MudBlazor, Fluent UI, or bare custom CSS.

---

## 6. Phase 4 — Dump Analysis and Memory Leak Detection

Phase 4 adds automated GC dump file monitoring, analysis, and diff-based memory leak detection. When a new `.gcdump` file appears in the watch folder, it is automatically analyzed and compared against the previous dump to surface memory growth patterns.

### 6.1 Scope

| Capability | Implementation |
|---|---|
| Dump file watching | `FileSystemWatcher` on configurable folder, `Channel<string>` for async processing |
| Report parsing | Shell out to `dotnet-gcdump report {file}`, parse fixed-width text into structured models |
| Heap type analysis | Top N types by total size with instance counts |
| Diff analysis | Join two analysis results on type name, compute delta size/count/growth % |
| Auto-diff on arrival | Each new dump is automatically diffed against the previous one |
| Manual diff | User selects any two dumps from history for comparison |
| Cross-platform | Windows (NTFS + mandatory locks) and Linux (inotify + advisory locks) |

### 6.2 Additions to AppSysMetrics Library

**New Models** (`AppSysMetrics.Diagnostics.Models`):
- `HeapTypeInfo` — Type name, instance count, total size bytes (from `dotnet-gcdump report` output)
- `DumpAnalysisResult` — Complete analysis of one dump file: file path, capture time, analysis time, file size, total heap bytes, total object count, list of top types
- `HeapTypeDiff` — Per-type diff between two analyses: baseline/current counts and sizes, delta values, growth percent
- `DumpDiffResult` — Complete diff: baseline result, current result, time between dumps, list of type diffs sorted by delta size descending, total heap/object deltas

**New Options:**
- `DumpAnalyzerOptions` — `WatchFolder` (nullable, cascading default from `DiagnosticsOptions.GcDumpOutputDirectory` then temp path), `MaxAnalysisHistory` (10), `FileReadyTimeoutSeconds` (30), `FileReadyRetryDelayMs` (500), `TopTypesCount` (50)

**New Services:**
- `DumpReportParser` (static) — Pure function that parses `dotnet-gcdump report` stdout. Extracts summary lines (heap bytes, object count), finds the column header, then parses fixed-width data rows: positions [0..15] = size, [15..23] = count, [25..] = type name. Strips `[Assembly.dll]` suffixes. Skips unparseable rows gracefully.
- `DumpAnalyzerService` — Shells out to `dotnet-gcdump report "{filePath}"` using the same `ProcessStartInfo` pattern as `DiagnosticsService.CaptureGcDumpAsync()`. Cross-platform timestamp handling: falls back from `CreationTimeUtc` to `LastWriteTimeUtc` on Linux where creation time may not be available.
- `DumpDiffService` — Pure computation: builds dictionaries keyed by type name from both results, computes the union, calculates deltas for each type. Types only in current = new allocations (growth 100%). Types only in baseline = freed. Sorts by `DeltaSizeBytes` descending (biggest growers = leak suspects).

**New Hosting:**
- `DumpAnalysisHub` — Ring buffer of `DumpAnalysisResult` with two events: `OnAnalysis` (new analysis) and `OnDiff` (new diff). Follows the `MetricsHub` pattern: `object _lock`, defensive copy `GetHistory()`, `Latest`/`LatestDiff` properties. Two events because analysis can arrive without a diff (first dump), and diffs can be manually triggered from the UI.
- `DumpWatcherService` — `BackgroundService` that watches the configured dump folder using `FileSystemWatcher`. On `Created` events, file paths are written to a `Channel<string>` which decouples the synchronous FSW callback from the async analysis pipeline. The processing loop: skips already-analyzed files, waits for file readiness, runs analysis, publishes to hub, and auto-diffs against previous. On startup, scans for existing `.gcdump` files to handle app restarts.

**Updated DI Registration:**
- `AddAppSysMetrics()` — Registers `DumpAnalyzerOptions` (options), `DumpAnalyzerService` (singleton), `DumpDiffService` (singleton), `DumpAnalysisHub` (singleton), `DumpWatcherService` (hosted)

### 6.3 New UI Components

**Panels** (`AppSysMetrics.Components.Panels`):
- `DumpAnalysisPanel` — Shows the latest analysis: header MetricCards (heap size, object count, file name with capture time), ranked table of top 20 types with size and count
- `DumpDiffPanel` — Shows diff between two dumps: header MetricCards (heap delta, object delta, time span), 6-column diff table (type, baseline size, current size, delta size, delta count, growth %). Red for growth, green for shrinkage. Row tinting for visual emphasis.
- `DumpHistoryPanel` — Interactive table of all analyzed dumps. Click-to-select: first click = baseline ("BASE" tag), second click = current ("CUR" tag), third click = reset. "Compare Selected" button triggers `DumpDiffService.ComputeDiff()` and bubbles the result via `EventCallback<DumpDiffResult>`.

**View** (`AppSysMetrics.Components.Views`):
- `DumpAnalysisView` — Composite view subscribing to `DumpAnalysisHub.OnAnalysis` and `OnDiff`. Grid layout: DumpHistoryPanel (full width, top row), DumpAnalysisPanel (left), DumpDiffPanel (right). Handles both auto-diff (from watcher) and manual comparison (from history panel).

Consumers integrate the view with a thin page wrapper (e.g., `@page "/dump-analysis"`, `@rendermode InteractiveServer`, `<DumpAnalysisView />`).

### 6.4 Key Design: Cross-Platform File-Ready Detection

`dotnet-gcdump collect` writes `.gcdump` files that can take several seconds to complete. `FileSystemWatcher.Created` fires when the file *starts* being written, not when it's done. The file-ready detection uses a hybrid approach:

1. Initial 500ms delay to let the writer begin
2. Retry loop (max 30s): attempt `FileStream` open with `FileAccess.Read`, `FileShare.None`
   - **Windows**: The file is locked by `dotnet-gcdump` during writing, so `IOException` is thrown until the write completes — this is reliable
   - **Linux**: File locks are advisory, so the exclusive open may succeed even during writing — the size > 0 check catches this
3. Timeout: log warning and proceed — on Linux the file is likely ready; let `dotnet-gcdump report` decide

### 6.5 Key Design: Text Parsing Over Binary Format

`dotnet-gcdump report` has **no `--format json` flag** as of v9.0. The output is a fixed-width text table with culture-invariant formatting (`{value,15:N0}`). Parsing this text is:

- **Stable** — The format string is simple and hasn't changed across tool versions
- **Cross-platform identical** — Same output on Windows and Linux
- **Zero-dependency** — No NuGet packages needed (no TraceEvent, no ClrMD)
- **Testable** — `DumpReportParser.Parse()` is a static pure function; unit tests pass in sample strings

The alternative — reading the `.gcdump` binary format directly via `Microsoft.Diagnostics.Runtime` or the `TraceEvent` library — would require heavy dependencies and deal with an internal format subject to version changes.

### 6.6 Key Design: Channel for FileSystemWatcher Bridging

`FileSystemWatcher` raises `Created` events synchronously on a thread pool thread. Rather than performing async analysis directly in the event handler (which would require `async void` with no error propagation), the handler writes to a `Channel<string>`. The `BackgroundService.ExecuteAsync` loop reads from the channel via `await foreach (var path in channel.Reader.ReadAllAsync(stoppingToken))`, which provides:

- Natural async/await with proper cancellation
- Sequential processing (one file at a time) preventing concurrent `dotnet-gcdump report` invocations
- Error isolation: individual file failures are caught and logged without losing subsequent events
- Startup file scanning: existing `.gcdump` files are enqueued through the same channel

### 6.7 Key Design: Dual Hub Events

`DumpAnalysisHub` has two separate events (`OnAnalysis` and `OnDiff`) rather than a single combined event because:

- The first dump produces an analysis but no diff — subscribers shouldn't receive a null diff event
- Manual comparisons from `DumpHistoryPanel` produce a diff without a new analysis — the diff panel should update without the analysis panel re-rendering
- The separation maps cleanly to the two bottom panels in `DumpAnalysisView`: each subscribes to exactly the event it needs

---

## 7. Project Structure

```
AppSysMetrics/                              (Razor Class Library — net10.0)
├── AppSysMetrics.csproj                    (Sdk="Microsoft.NET.Sdk.Razor")
├── Collection/
│   ├── IMetricsCollector.cs
│   ├── MetricsCollector.cs
│   ├── CpuSampler.cs
│   ├── AllocationRateTracker.cs
│   └── AllocationEventListener.cs          (Phase 2)
├── Components/                             (Phase 3)
│   ├── _Imports.razor
│   ├── Charts/
│   │   ├── MetricCard.razor (+.css)
│   │   ├── BarChart.razor (+.css)
│   │   ├── LineChart.razor (+.css)
│   │   └── GaugeChart.razor (+.css)
│   ├── Panels/
│   │   ├── ProcessMetricsPanel.razor (+.css)
│   │   ├── CpuMetricsPanel.razor (+.css)
│   │   ├── GcMetricsPanel.razor (+.css)
│   │   ├── AllocationRatePanel.razor (+.css)
│   │   ├── TopAllocationsPanel.razor (+.css)
│   │   ├── LargeObjectAllocationsPanel.razor (+.css)
│   │   ├── DiagnosticsPanel.razor (+.css)
│   │   ├── DumpAnalysisPanel.razor (+.css)     (Phase 4)
│   │   ├── DumpDiffPanel.razor (+.css)         (Phase 4)
│   │   └── DumpHistoryPanel.razor (+.css)      (Phase 4)
│   └── Views/
│       ├── MetricsDashboardView.razor (+.css)
│       ├── MemoryDiagnosticsView.razor (+.css)
│       └── DumpAnalysisView.razor (+.css)      (Phase 4)
├── Diagnostics/                            (Phase 2+4)
│   ├── DiagnosticsOptions.cs
│   ├── IDiagnosticsService.cs
│   ├── DiagnosticsService.cs
│   ├── DumpAnalyzerOptions.cs              (Phase 4)
│   ├── DumpReportParser.cs                 (Phase 4)
│   ├── DumpAnalyzerService.cs              (Phase 4)
│   ├── DumpDiffService.cs                  (Phase 4)
│   └── Models/                             (Phase 4)
│       ├── HeapTypeInfo.cs
│       ├── DumpAnalysisResult.cs
│       ├── HeapTypeDiff.cs
│       └── DumpDiffResult.cs
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Hosting/
│   ├── MetricsCollectionOptions.cs
│   ├── MetricsHub.cs
│   ├── MetricsCollectionService.cs
│   ├── AllocationTrackingHub.cs            (Phase 2)
│   ├── AllocationTrackingService.cs        (Phase 2)
│   ├── DumpAnalysisHub.cs                  (Phase 4)
│   └── DumpWatcherService.cs               (Phase 4)
├── Models/
│   ├── MetricsSnapshot.cs
│   ├── ProcessMetrics.cs
│   ├── CpuMetrics.cs
│   ├── GcMetrics.cs
│   ├── GcGenerationInfo.cs
│   ├── AllocationTypeInfo.cs               (Phase 2)
│   └── AllocationSnapshot.cs               (Phase 2)
└── wwwroot/                                (Phase 3)
    └── AppSysMetrics.css
```

---

## 8. Data Models

All library models are `sealed record` types (immutable, value-equality). Core metrics models are in `AppSysMetrics.Models`; dump analysis models are in `AppSysMetrics.Diagnostics.Models`.

### 8.1 MetricsSnapshot

The top-level container produced by `MetricsCollector.Collect()` every 2 seconds.

| Property | Type | Source |
|---|---|---|
| TimestampTicks | long | `Stopwatch.GetTimestamp()` |
| CapturedAt | DateTimeOffset | `DateTimeOffset.UtcNow` |
| Process | ProcessMetrics | `Process.GetCurrentProcess()` |
| Cpu | CpuMetrics | `CpuSampler.Sample()` |
| Gc | GcMetrics | `GC.GetGCMemoryInfo()` + `GC.CollectionCount()` |

### 8.2 ProcessMetrics

| Property | Type | Description |
|---|---|---|
| WorkingSet64 | long | Physical memory (bytes) — what Task Manager shows |
| PrivateMemorySize64 | long | Private committed memory (bytes) |
| VirtualMemorySize64 | long | Total virtual address space (bytes) |
| PagedMemorySize64 | long | Paged memory (bytes) |
| ThreadCount | int | OS thread count |
| HandleCount | int | OS handle count |

### 8.3 CpuMetrics

| Property | Type | Description |
|---|---|---|
| CpuPercentage | double | Sampled CPU % (0–100), normalized by processor count |
| TotalProcessorTime | TimeSpan | Cumulative CPU time since process start |
| ProcessorCount | int | `Environment.ProcessorCount` |

### 8.4 GcMetrics

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

### 8.5 GcGenerationInfo

| Property | Type | Description |
|---|---|---|
| Generation | int | 0, 1, 2, 3 (LOH), 4 (POH) |
| SizeBeforeBytes | long | Generation size before last collection |
| SizeAfterBytes | long | Generation size after last collection |
| FragmentationBeforeBytes | long | Fragmentation before last collection |
| FragmentationAfterBytes | long | Fragmentation after last collection |

### 8.6 AllocationTypeInfo (Phase 2)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from AllocationTick event |
| TotalBytes | long | Cumulative bytes allocated for this type |
| AllocationCount | int | Number of allocation ticks observed |
| IsLargeObject | bool | True if allocated on the LOH (>= 85,000 bytes) |

### 8.7 AllocationSnapshot (Phase 2)

| Property | Type | Description |
|---|---|---|
| CapturedAt | DateTimeOffset | When the snapshot was taken |
| TopAllocatingTypes | IReadOnlyList\<AllocationTypeInfo\> | Top N types by total bytes (descending) |
| RecentLargeObjectAllocations | IReadOnlyList\<AllocationTypeInfo\> | Recent LOH allocations |
| TotalTrackedBytes | long | Sum of all tracked allocation bytes |
| TotalTrackedCount | int | Sum of all tracked allocation counts |

### 8.8 HeapTypeInfo (Phase 4)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from `dotnet-gcdump report` output |
| InstanceCount | long | Number of instances of this type on the heap |
| TotalSizeBytes | long | Total bytes consumed by all instances of this type |

### 8.9 DumpAnalysisResult (Phase 4)

| Property | Type | Description |
|---|---|---|
| FilePath | string | Full path to the analyzed `.gcdump` file |
| FileName | string | File name only (for display) |
| CapturedAtUtc | DateTimeOffset | When the dump was captured (from file timestamp) |
| AnalyzedAtUtc | DateTimeOffset | When the analysis completed |
| FileSizeBytes | long | Size of the `.gcdump` file on disk |
| TotalHeapBytes | long | Total GC heap size from report summary |
| TotalObjectCount | long | Total GC heap object count from report summary |
| TopTypes | IReadOnlyList\<HeapTypeInfo\> | Top N types by total size, descending |

### 8.10 HeapTypeDiff (Phase 4)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name |
| BaselineCount | long | Instance count in the baseline dump |
| CurrentCount | long | Instance count in the current dump |
| DeltaCount | long | `CurrentCount - BaselineCount` |
| BaselineSizeBytes | long | Total size in baseline dump |
| CurrentSizeBytes | long | Total size in current dump |
| DeltaSizeBytes | long | `CurrentSizeBytes - BaselineSizeBytes` |
| GrowthPercent | double | `(DeltaSizeBytes / BaselineSizeBytes) * 100` (0 if baseline is 0) |

### 8.11 DumpDiffResult (Phase 4)

| Property | Type | Description |
|---|---|---|
| Baseline | DumpAnalysisResult | The older dump analysis |
| Current | DumpAnalysisResult | The newer dump analysis |
| TimeBetweenDumps | TimeSpan | `Current.CapturedAtUtc - Baseline.CapturedAtUtc` |
| TypeDiffs | IReadOnlyList\<HeapTypeDiff\> | Per-type diffs, sorted by `DeltaSizeBytes` descending |
| TotalHeapDelta | long | `Current.TotalHeapBytes - Baseline.TotalHeapBytes` |
| TotalObjectDelta | long | `Current.TotalObjectCount - Baseline.TotalObjectCount` |

---

## 9. Services and Hosting

### 9.1 DI Registration

```csharp
// Consumer's Program.cs
builder.Services.AddAppSysMetrics(options =>
{
    options.CollectionInterval = TimeSpan.FromSeconds(2);
    options.MaxHistorySize = 60;
});
```

### 9.2 AppSysMetrics Service Registrations

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

### 9.3 Hub Pattern

Both `MetricsHub` and `AllocationTrackingHub` follow the same pattern:

- **Ring buffer**: `List<T>` capped at `MaxHistorySize`, oldest entries removed on overflow
- **Thread safety**: `lock` on publish and read operations
- **Event**: `Action<T>? OnSnapshot` invoked after publish, outside the lock
- **Latest**: Property holding the most recent snapshot for newly subscribing components
- **GetHistory()**: Returns a copy of the ring buffer for chart rendering

### 9.4 Background Services

Both `MetricsCollectionService` and `AllocationTrackingService` use `PeriodicTimer` in `ExecuteAsync`:

```
while (await timer.WaitForNextTickAsync(stoppingToken))
{
    var snapshot = collector/listener.Collect/CreateSnapshot();
    hub.Publish(snapshot);
}
```

Exceptions within the loop are caught and logged, not propagated, to keep the service running.

### 9.5 Dump Analysis Service Registrations (Phase 4)

| Service | Lifetime | Interface |
|---|---|---|
| DumpAnalyzerService | Singleton | (concrete) |
| DumpDiffService | Singleton | (concrete) |
| DumpAnalysisHub | Singleton | (concrete) |
| DumpWatcherService | Hosted | BackgroundService |
| DumpAnalyzerOptions | Options | IOptions\<T\> |

### 9.6 DumpAnalysisHub Pattern (Phase 4)

`DumpAnalysisHub` follows the same ring buffer pattern as `MetricsHub` and `AllocationTrackingHub` but with two events:

- **`OnAnalysis`** — Fires when a new dump is analyzed. Subscribers receive the `DumpAnalysisResult`.
- **`OnDiff`** — Fires when a diff is computed (either auto-diff from the watcher or manual comparison from the UI). Subscribers receive the `DumpDiffResult`.

Properties: `Latest` (most recent analysis), `LatestDiff` (most recent diff), `GetHistory()` (defensive copy of all analyses).

### 9.7 DumpWatcherService Pattern (Phase 4)

Unlike timer-based background services, `DumpWatcherService` uses event-driven processing via `Channel<string>`:

```
FileSystemWatcher (*.gcdump Created)
    → Channel<string>.Writer.TryWrite(filePath)
        → await foreach (channel.Reader.ReadAllAsync(stoppingToken))
            → WaitForFileReadyAsync → AnalyzeAsync → Publish → ComputeDiff
```

**Folder resolution chain**: `DumpAnalyzerOptions.WatchFolder` → `DiagnosticsOptions.GcDumpOutputDirectory` → `Path.Combine(Path.GetTempPath(), "AppSysMetrics", "gcdumps")`

On startup, existing `.gcdump` files are scanned and enqueued to handle application restarts. Individual file failures are caught and logged without killing the service.

---

## 10. UI Components

All chart, panel, and view components listed below are shipped in the library.

### 10.1 Component Namespaces

| Namespace | Contents |
|---|---|
| `AppSysMetrics.Components.Charts` | BarChart, LineChart, GaugeChart, MetricCard |
| `AppSysMetrics.Components.Panels` | ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel, TopAllocationsPanel, LargeObjectAllocationsPanel, DiagnosticsPanel, DumpAnalysisPanel, DumpDiffPanel, DumpHistoryPanel |
| `AppSysMetrics.Components.Views` | MetricsDashboardView, MemoryDiagnosticsView, DumpAnalysisView |

### 10.2 Consumer Page Integration

The library ships views, not pages. Consumers create their own thin page wrappers:

| View Tag | Typical Route | Purpose |
|---|---|---|
| `<MetricsDashboardView />` | `/`, `/dashboard`, `/metrics` | Live process, CPU, GC, and allocation rate metrics |
| `<MemoryDiagnosticsView />` | `/diagnostics` | Allocation tracking, LOH alerts, Force GC, GC Dump capture |
| `<DumpAnalysisView />` | `/dump-analysis` | Dump file analysis, diff comparison, memory leak detection |

Each page is typically 3–6 lines: `@page`, `@rendermode`, `<PageTitle>`, and the view tag. `MetricsDashboardView` accepts a `RenderFragment? AdditionalContent` parameter for injecting app-specific panels.

### 10.3 Chart Components (AppSysMetrics library)

| Component | Visualization | Rendering |
|---|---|---|
| BarChart | Vertical bars with labels and gridlines | SVG 400x200 viewBox, `BuildSvg()` + `MarkupString` |
| LineChart | Area-fill polyline with stroke and end-point indicator | SVG 400x180 viewBox, `BuildSvg()` + `MarkupString` |
| GaugeChart | 180-degree arc gauge, color-coded by threshold | SVG 200x130 viewBox, `BuildSvg()` + `MarkupString` |
| MetricCard | Title / value / subtitle card | Razor markup, scoped CSS |

All chart components accept parameters for data, titles, units, colors, and ranges. None use JavaScript.

### 10.4 Panel Components (AppSysMetrics library)

| Panel | Data Source | Key Visuals |
|---|---|---|
| ProcessMetricsPanel | MetricsSnapshot | BarChart (memory breakdown), MetricCards (threads, handles, virtual) |
| CpuMetricsPanel | List\<MetricsSnapshot\> | LineChart (CPU % history), MetricCards (current, processors, total time) |
| GcMetricsPanel | MetricsSnapshot | GaugeChart (memory load %), generation table, MetricCards (collections, pause %, finalizers) |
| AllocationRatePanel | List\<MetricsSnapshot\> | LineChart (allocation rate MB/s), MetricCards (current rate, total allocated) |
| TopAllocationsPanel | AllocationSnapshot | Ranked table of types with monospace type names, bytes, counts |
| LargeObjectAllocationsPanel | AllocationSnapshot | LOH allocation table with alert styling, or "no LOH" indicator |
| DiagnosticsPanel | IDiagnosticsService | Force GC button with before/after comparison, GC Dump button with file path result |
| DumpAnalysisPanel | DumpAnalysisResult | MetricCards (heap size, object count, file name), ranked top 20 types table |
| DumpDiffPanel | DumpDiffResult | MetricCards (heap delta, object delta, time span), 6-column diff table with color-coded growth/shrinkage |
| DumpHistoryPanel | IReadOnlyList\<DumpAnalysisResult\> | Interactive click-to-select table (BASE/CUR tags), "Compare Selected" button |

### 10.5 Composite View Components (AppSysMetrics library)

| View | Injects | Grid Content | Parameter |
|---|---|---|---|
| MetricsDashboardView | MetricsHub | ProcessMetrics, CPU, GC, AllocationRate (2x2) + optional full-width slot | `RenderFragment? AdditionalContent` |
| MemoryDiagnosticsView | AllocationTrackingHub, MetricsHub | TopAllocations, LOH, GC, Diagnostics (2x2) | — |
| DumpAnalysisView | DumpAnalysisHub | DumpHistory (full width), DumpAnalysis (left), DumpDiff (right) | — |

### 10.6 Blazor Component Lifecycle Pattern

All subscribing view components follow this pattern:

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

## 11. Dependency Inventory

### 11.1 NuGet Packages

AppSysMetrics has **no direct NuGet package references**. All dependencies are provided by the `Microsoft.AspNetCore.App` shared framework via `FrameworkReference`.

### 11.2 Framework References

| Reference | Used By | Provides |
|---|---|---|
| Microsoft.AspNetCore.App | AppSysMetrics | Razor component compilation, Hosting.Abstractions, Logging.Abstractions, Options, DI |

### 11.3 External Tools

| Tool | Required By | Install Command |
|---|---|---|
| dotnet-gcdump | DiagnosticsService (GC Dump capture), DumpAnalyzerService (report parsing) | `dotnet tool install --global dotnet-gcdump` |

The tool is required for both "Capture GC Dump" (Phase 2) and "Dump Analysis" (Phase 4) features. Phase 4 uses `dotnet-gcdump report` to parse heap type information from `.gcdump` files. All other functionality works without it.

### 11.4 Static Assets

| Asset | Path | Purpose |
|---|---|---|
| AppSysMetrics.css | `_content/AppSysMetrics/AppSysMetrics.css` | Shared library component styles (panels, tables, buttons, state colors) |
| AppSysMetrics.styles.css | Auto-generated by Blazor CSS isolation | Scoped component CSS for charts, panels, views |

### 11.5 Runtime APIs

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
| `FileSystemWatcher` | System.IO | File system event monitoring (Phase 4) |
| `Channel<T>` | System.Threading.Channels | Async producer-consumer bridging (Phase 4) |

---

## 12. Design Decisions

### 12.1 Why sealed records for metrics?

Records provide value equality and immutable snapshots. `sealed` prevents inheritance overhead. The combination is ideal for data that's created once, published to a hub, and read by multiple consumers — no defensive copying needed.

### 12.2 Why MarkupString + StringBuilder for SVG?

Razor's parser treats `<text>` as a directive, not an HTML/SVG element. Since SVG uses `<text>` extensively for labels, axis values, and gauge readouts, the only clean solution is to build the SVG string outside of Razor's parser and inject it as raw markup. This also avoids Razor issues with `<` in switch expressions used for threshold-based coloring.

### 12.3 Why not use a JavaScript charting library?

The solution demonstrates that Blazor Server can render rich visualizations without any JavaScript interop. The SVG approach has zero JS payload, no npm dependencies, no bundling, and updates instantly via SignalR without client-side re-rendering.

### 12.4 Why two separate hubs?

Metrics snapshots and allocation snapshots serve different diagnostic questions. Metrics answer "how is the process doing right now?" while allocation snapshots answer "what types are consuming the most memory?" Coupling them into a single snapshot would force both collection mechanisms onto the same timer and make the API less composable for consumers that only need one view.

### 12.5 Why shell out for GC dumps instead of using the diagnostics NuGet?

`Microsoft.Diagnostics.NETCore.Client` and its transitive dependencies (`Microsoft.Diagnostics.Runtime`, etc.) add significant binary size and complexity. For a diagnostic feature used infrequently and on-demand, shelling out to an already-installed global tool is a pragmatic trade-off that keeps the library dependency graph clean.

### 12.6 Why one library instead of separate Core + UI packages?

The Razor SDK is additive — existing C# compiles identically. A single package avoids version coordination between "core" and "UI" packages. Consumers who only need the backend can ignore the Components namespace; Razor components add negligible binary size. The `FrameworkReference` to `Microsoft.AspNetCore.App` actually simplified the `.csproj` by replacing three explicit NuGet package references.

### 12.7 Why no @page or @rendermode in library view components?

Hardcoding routes in a library claims URL paths from the consumer. Hardcoding render mode prevents consumers from choosing InteractiveServer vs InteractiveWebAssembly vs InteractiveAuto. By shipping views as plain components, the library remains composable: the consumer wraps them in their own pages with their own routing and render mode decisions. The `RenderFragment? AdditionalContent` parameter on `MetricsDashboardView` further enables extensibility without modification.

### 12.8 Why no Bootstrap dependency in library components?

Library components use zero Bootstrap CSS classes. All styling is self-contained via `AppSysMetrics.css` (shared base) and scoped `.razor.css` (per-component layout). This makes the library portable to any CSS framework or custom design system.

### 12.9 Why parse text output instead of the .gcdump binary format? (Phase 4)

`dotnet-gcdump report` outputs a fixed-width text table with no `--format json` option. Parsing this text is stable, cross-platform identical, and requires zero NuGet dependencies. The alternative — reading `.gcdump` files directly via `Microsoft.Diagnostics.Runtime` or `TraceEvent` — would add heavy transitive dependencies and couple the parser to an internal binary format subject to version changes. See Section 6.5 for details.

### 12.10 Why Channel\<string\> for FileSystemWatcher bridging? (Phase 4)

`FileSystemWatcher.Created` fires synchronously on a thread pool thread. Using `Channel<string>` as an intermediary provides proper async/await, natural cancellation via `ReadAllAsync(stoppingToken)`, sequential processing (preventing concurrent `dotnet-gcdump report` invocations), and error isolation per file. See Section 6.6 for details.

### 12.11 Why dual events on DumpAnalysisHub? (Phase 4)

`DumpAnalysisHub` separates `OnAnalysis` and `OnDiff` events because the first dump has no diff, and manual UI comparisons produce diffs without new analyses. Each bottom panel in `DumpAnalysisView` subscribes to exactly the event it needs. See Section 6.7 for details.

### 12.12 Why a third hub instead of extending existing hubs? (Phase 4)

`DumpAnalysisHub` is independent from `MetricsHub` and `AllocationTrackingHub` because dump analysis operates on a fundamentally different trigger (file system events) rather than a periodic timer. The data lifecycle is different — analyses accumulate as discrete events per dump file, not continuous time-series data. This follows the same separation principle described in Section 12.4 for the first two hubs.
