# AppSysMetrics Implementation Plan

**Version:** 5.0
**Date:** February 9, 2026
**Target Framework:** .NET 10.0 (SDK 10.0.102)
**Package:** Razor Class Library (`Microsoft.NET.Sdk.Razor`)
**Status:** All phases complete (Phase 1–5 implemented)

> This document records the phased implementation history of AppSysMetrics — what was built in each phase, what was changed or replaced, and the design decisions made along the way. For the current-state specification of the library (architecture, models, services, UI, APIs), see **AppSysMetrics_SoftwareSpecification.md**.

---

## Table of Contents

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Phase 1 — Real-Time Metrics Dashboard](#3-phase-1--real-time-metrics-dashboard)
4. [Phase 2 — Memory Diagnostics](#4-phase-2--memory-diagnostics)
5. [Phase 3 — Razor Class Library Packaging](#5-phase-3--razor-class-library-packaging)
6. [Phase 4 — Dump Analysis and Memory Leak Detection](#6-phase-4--dump-analysis-and-memory-leak-detection)
7. [Phase 5 — ClrMD In-Process Heap Analysis and Correlation Narrative](#7-phase-5--clrmd-in-process-heap-analysis-and-correlation-narrative)
8. [Project Structure](#8-project-structure)
9. [Data Models](#9-data-models)
10. [Services and Hosting](#10-services-and-hosting)
11. [UI Components](#11-ui-components)
12. [Dependency Inventory](#12-dependency-inventory)
13. [Design Decisions](#13-design-decisions)

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
3. Offer on-demand diagnostic actions (Force GC, Heap Snapshot, GC Dump capture) from the browser UI.
4. Render all visualizations as pure SVG — no JavaScript charting dependencies.
5. Ship as a single Razor Class Library — one project reference provides both backend services and Blazor UI components.
6. Analyze and diff heap snapshots for memory leak detection with allocation/retention correlation narrative — no manual Visual Studio inspection required.
7. Capture heap snapshots in-process via ClrMD, immune to the .NET 8+ EventPipe type-name regression.

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
| **Panels** | ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel, TopAllocationsPanel, LargeObjectAllocationsPanel, DiagnosticsPanel, DumpAnalysisPanel, DumpDiffPanel, DumpHistoryPanel, MemoryHealthPanel | Drop individual panels into existing pages |
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

UI Button: "Capture Heap Snapshot"
      │
      ▼
DiagnosticsService.CaptureGcDumpAsync()
      │
      ├─ ClrMdHeapAnalyzer.CaptureAndAnalyzeAsync()
      │    └─ DataTarget.CreateSnapshotAndAttach(pid)
      │         └─ heap.EnumerateObjects() → DumpAnalysisResult
      ├─ AllocationEventListener.CreateSnapshot()  (enrichment)
      ├─ DumpAnalysisHub.Publish(result)
      └─ DumpDiffService.ComputeDiff()             (auto-diff)
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
- **ClrMdHeapAnalyzer** wraps its CPU-bound heap enumeration in `Task.Run` and serializes concurrent captures via `SemaphoreSlim(1)`. The snapshot is taken via `DataTarget.CreateSnapshotAndAttach` which uses `PssCreateSnapshot` on Windows for a fast in-memory copy.
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
- `IDiagnosticsService` — `ForceGC()` returns `ForceGcResult` (before/after metrics + duration); `CaptureGcDumpAsync()` returns `GcDumpResult` (in-process heap snapshot via ClrMD); `CaptureGcDumpFileAsync()` returns `GcDumpResult` (file-based `.gcdump` export via dotnet-gcdump)
- `DiagnosticsService` — Force GC performs `GC.Collect(2, Forced, blocking)` twice with `WaitForPendingFinalizers()` between. Heap Snapshot uses `ClrMdHeapAnalyzer` for in-process capture with allocation enrichment and auto-diff. GC Dump File shells out to `dotnet-gcdump collect` for `.gcdump` file export.

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
- `DiagnosticsPanel` — Three action groups: Force GC button (shows before/after heap comparison with freed bytes), Capture Heap Snapshot button (ClrMD in-process, shows capture time), and Capture GC Dump button (dotnet-gcdump file export, shows file path, size, or error message).
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

Phase 4 introduced dump analysis models, the diff service, the analysis hub, and the initial UI panels for memory leak detection. The original implementation used `FileSystemWatcher` + `dotnet-gcdump report` for automated file-based analysis; Phase 5 replaced that pipeline with in-process ClrMD capture (see Section 7). The models, hub, diff service, and UI components introduced here remain in the current codebase.

### 6.1 Scope

| Capability | Implementation |
|---|---|
| Heap type analysis | Top N types by total size with instance counts |
| Diff analysis | Join two analysis results on type name, compute delta size/count/growth % |
| Auto-diff | Each new snapshot is automatically diffed against the previous one |
| Manual diff | User selects any two dumps from history for comparison |
| History management | Clear all history with optional file deletion from disk |

### 6.2 Additions to AppSysMetrics Library

**New Models** (`AppSysMetrics.Diagnostics.Models`):
- `HeapTypeInfo` — Type name, instance count, total size bytes
- `DumpAnalysisResult` — Complete analysis of one dump file: file path, capture time, analysis time, file size, total heap bytes, total object count, list of top types
- `HeapTypeDiff` — Per-type diff between two analyses: baseline/current counts and sizes, delta values, growth percent
- `DumpDiffResult` — Complete diff: baseline result, current result, time between dumps, list of type diffs sorted by delta size descending, total heap/object deltas

**New Options:**
- `DumpAnalyzerOptions` — `MaxAnalysisHistory` (10), `TopTypesCount` (50)

**New Services:**
- `DumpDiffService` — Pure computation: builds dictionaries keyed by type name from both results, computes the union, calculates deltas for each type. Types only in current = new allocations (growth 100%). Types only in baseline = freed. Sorts by `DeltaSizeBytes` descending (biggest growers = leak suspects). Phase 5 extended this with allocation correlation (see Section 7.3).

**New Hosting:**
- `DumpAnalysisHub` — Ring buffer of `DumpAnalysisResult` with three events: `OnAnalysis` (new analysis), `OnDiff` (new diff), and `OnCleared` (history cleared). Follows the `MetricsHub` pattern: `object _lock`, defensive copy `GetHistory()`, `Latest`/`LatestDiff` properties. Includes a `Clear()` method that resets all state and fires `OnCleared`. Two analysis events because analysis can arrive without a diff (first dump), and diffs can be manually triggered from the UI.

**DI Registration (current state, including Phase 5 changes):**
- `AddAppSysMetrics()` — Registers `DumpAnalyzerOptions` (options), `ClrMdHeapAnalyzer` (singleton, Phase 5), `DumpDiffService` (singleton), `DumpAnalysisHub` (singleton)

### 6.3 UI Components

**Panels** (`AppSysMetrics.Components.Panels`):
- `DumpAnalysisPanel` — Shows the latest analysis: header MetricCards (heap size, object count, file name with capture time), ranked table of top 20 types with size and count
- `DumpDiffPanel` — Shows diff between two dumps: header MetricCards (heap delta, object delta, time span), 6-column diff table (type, baseline size, current size, delta size, delta count, growth %). Red for growth, green for shrinkage. Row tinting for visual emphasis.
- `DumpHistoryPanel` — Interactive table of all analyzed dumps. Click-to-select: first click = baseline ("BASE" tag), second click = current ("CUR" tag), third click = reset. Tags are based on chronological order (`CapturedAtUtc`), not click order. "Compare Selected" button triggers `DumpDiffService.ComputeDiff()` and bubbles the result via `EventCallback<DumpDiffResult>`. "Clear All" button deletes dump files from disk and fires `EventCallback OnClear`.

**View** (`AppSysMetrics.Components.Views`):
- `DumpAnalysisView` — Composite view subscribing to `DumpAnalysisHub.OnAnalysis`, `OnDiff`, and `OnCleared`, plus `MetricsHub.OnSnapshot`. Grid layout: MemoryHealthPanel (full width), DumpHistoryPanel (full width), DumpAnalysisPanel (left), DumpDiffPanel (right). Handles both auto-diff (from DiagnosticsService) and manual comparison (from history panel).

Consumers integrate the view with a thin page wrapper (e.g., `@page "/dump-analysis"`, `@rendermode InteractiveServer`, `<DumpAnalysisView />`).

### 6.4 Key Design: Dual Hub Events

`DumpAnalysisHub` has two separate events (`OnAnalysis` and `OnDiff`) rather than a single combined event because:

- The first dump produces an analysis but no diff — subscribers shouldn't receive a null diff event
- Manual comparisons from `DumpHistoryPanel` produce a diff without a new analysis — the diff panel should update without the analysis panel re-rendering
- The separation maps cleanly to the two bottom panels in `DumpAnalysisView`: each subscribes to exactly the event it needs

---

## 7. Phase 5 — ClrMD In-Process Heap Analysis and Correlation Narrative

Phase 5 replaces the external `dotnet-gcdump` tool with in-process heap analysis via ClrMD (`Microsoft.Diagnostics.Runtime`), adds allocation/retention correlation to the diff pipeline, and introduces a narrative UI that tells the memory health story at a glance.

### 7.1 Motivation

Phase 4's reliance on `dotnet-gcdump collect` + `dotnet-gcdump report` suffered from a .NET 8+ EventPipe regression (dotnet/diagnostics Issue #5116): on the second capture from the same process, `GCBulkType` events are not re-emitted, causing all type names to appear as `UNKNOWN 0x...`. ClrMD reads type metadata directly from CLR internals (method tables, DAC) and is immune to this regression.

Additionally, Phase 4's diff view showed raw numbers in a table without answering the key diagnostic question: "Is the heap healthy?" Phase 5 adds a narrative banner, collection efficiency percentage, and leak suspect call-outs.

### 7.2 Scope

| Capability | Implementation |
|---|---|
| In-process heap snapshot | `ClrMdHeapAnalyzer` using `DataTarget.CreateSnapshotAndAttach(pid)` |
| Type enumeration | `heap.EnumerateObjects()` with aggregation by `obj.Type.Name` |
| Allocation correlation | Each `DumpAnalysisResult` enriched with `AllocationSnapshot` at capture time |
| Per-type retention ratio | `DumpDiffService` computes `heapDelta / allocationThroughput` per type |
| Collection efficiency | `TotalCollectedBetween / TotalAllocatedBetween` as overall health signal |
| Leak suspect detection | Types with retention ratio >= 0.8 and positive allocation throughput |
| Narrative UI | 4-zone DumpDiffPanel: summary cards, narrative banner, leak suspects, retention-sorted table |
| App vs Library split | Allocation tracking separates `AppSysMetrics.*` namespace types from app types |
| Memory health panel | `MemoryHealthPanel` with trend detection for allocation rate and heap size |
| GC Dump file capture | `CaptureGcDumpFileAsync()` retains the original `dotnet-gcdump collect` path for `.gcdump` file export |

### 7.3 Changes to AppSysMetrics Library

**New NuGet Dependency:**
- `Microsoft.Diagnostics.Runtime` v3.1.512801 — ClrMD for in-process heap analysis

**New Service:**
- `ClrMdHeapAnalyzer` — Singleton. Wraps `DataTarget.CreateSnapshotAndAttach(Environment.ProcessId)` in `Task.Run` (CPU-bound work). Enumerates all heap objects, aggregates by type name into `Dictionary<string, (long Count, long Size)>`, returns `DumpAnalysisResult` with synthetic path `clrmd://heap_yyyyMMdd_HHmmss`. Serialized via `SemaphoreSlim(1)` to prevent concurrent captures. Uses `TopTypesCount` from `DumpAnalyzerOptions`. Type names are always resolved (no UNKNOWN entries).

**New UI Panel:**
- `MemoryHealthPanel` — Displays memory health indicators with trend detection for allocation rate and heap size.

**Modified Service — `DiagnosticsService`:**
- `CaptureGcDumpAsync()` now uses `ClrMdHeapAnalyzer` instead of shelling out to `dotnet-gcdump collect`. After capture, enriches the result with an `AllocationSnapshot` from `AllocationEventListener.CreateSnapshot()`, publishes to `DumpAnalysisHub`, and auto-diffs against the previous result.
- `CaptureGcDumpFileAsync()` — New method preserving the original `dotnet-gcdump collect` subprocess for file-based `.gcdump` export.

**Modified Service — `DumpDiffService`:**
- When both dumps carry `AllocationAtCapture`, computes per-type allocation correlation: `BaselineAllocatedBytes`, `CurrentAllocatedBytes`, `AllocatedBetweenBytes`, and `RetentionRatio` (capped at 1.0). Also computes summary fields: `TotalAllocatedBetween` (app-only bytes) and `TotalCollectedBetween` (allocated minus heap growth, floored at 0).

**Modified Models:**
- `DumpAnalysisResult` — Added `UnresolvedTypeCount` (int) and `AllocationAtCapture` (AllocationSnapshot?, nullable).
- `HeapTypeDiff` — Added `BaselineAllocatedBytes`, `CurrentAllocatedBytes`, `AllocatedBetweenBytes` (long?, nullable) and `RetentionRatio` (double?, nullable).
- `DumpDiffResult` — Added `HasAllocationCorrelation` (bool), `TotalAllocatedBetween` (long?, nullable), `TotalCollectedBetween` (long?, nullable).
- `AllocationSnapshot` — Added `AppTrackedBytes`, `AppTrackedCount`, `LibraryTrackedBytes`, `LibraryTrackedCount` for App vs Library split.
- `IDiagnosticsService` — Added `CaptureGcDumpFileAsync()` method.

**Removed Files:**
- `DumpWatcherService.cs` — FileSystemWatcher + Channel processing loop, replaced by on-demand ClrMD capture.
- `DumpAnalyzerService.cs` — Shelled out to `dotnet-gcdump report`, replaced by ClrMD.
- `DumpReportParser.cs` — Parsed fixed-width text output, only consumer was DumpAnalyzerService.

**Modified Options:**
- `DumpAnalyzerOptions` — Removed `WatchFolder`, `FileReadyTimeoutSeconds`, `FileReadyRetryDelayMs`. Retained `MaxAnalysisHistory` (hub) and `TopTypesCount` (ClrMD).

**Modified DI Registration:**
- `AddAppSysMetrics()` — Added `ClrMdHeapAnalyzer` (singleton). Removed `DumpAnalyzerService` (singleton) and `DumpWatcherService` (hosted).

### 7.4 DumpDiffPanel — 4-Zone Correlation Narrative UI

When `DumpDiffResult.HasAllocationCorrelation` is true, the `DumpDiffPanel` renders a 4-zone layout:

1. **Zone 1: Summary MetricCards** — Heap delta, object delta, time span, and an enhanced "Collected" card showing collection efficiency percentage with color-coded state (green >= 80%, yellow >= 50%, red < 50%).

2. **Zone 2: Narrative Banner** — Prose summary with colored left-border (green/yellow/red) based on overall health. Includes heap growth, allocation throughput, collected bytes, and efficiency percentage. Example: "Heap grew by 2.0 MB. 15.0 MB was allocated between snapshots, of which 13.0 MB (87%) was collected."

3. **Zone 3: Leak Suspects Call-out** — Red-themed alert box showing up to 5 types with retention ratio >= 0.8. Each suspect shows: type name, allocated bytes, retained bytes, collected bytes, and retention percentage. Example: "System.Byte[] — allocated 500 KB → retained 500 KB, collected 0 B — 100% retention".

4. **Zone 4: Full Type Diff Table** — Re-sorted by retention ratio descending (nulls last) when correlation is available. Includes conditional allocation correlation columns (allocated between, retention %).

Computed values are cached in `OnParametersSet()` matching the existing lifecycle pattern used by `DumpAnalysisPanel`.

### 7.5 Key Design: ClrMD vs dotnet-gcdump

| Aspect | dotnet-gcdump (Phase 4) | ClrMD (Phase 5) |
|---|---|---|
| Mechanism | External process via `ProcessStartInfo` | In-process via `DataTarget.CreateSnapshotAndAttach` |
| Type resolution | EventPipe `GCBulkType` events | Direct CLR method table / DAC reads |
| .NET 8+ compatibility | Broken (UNKNOWN types on repeat captures) | Fully working |
| External tool required | Yes (`dotnet tool install -g dotnet-gcdump`) | No |
| NuGet dependency | None | `Microsoft.Diagnostics.Runtime` v3.1.512801 |
| Output | `.gcdump` file on disk | In-memory `DumpAnalysisResult` |
| File export | Automatic | Separate `CaptureGcDumpFileAsync()` path retained |

### 7.6 Key Design: Enrichment Pipeline

`DiagnosticsService` orchestrates a 5-step enrichment pipeline on each heap snapshot:

1. **Capture** — `ClrMdHeapAnalyzer.CaptureAndAnalyzeAsync()` returns a `DumpAnalysisResult`
2. **Enrich** — Attaches `AllocationEventListener.CreateSnapshot()` as `AllocationAtCapture`
3. **Previous** — Reads `DumpAnalysisHub.Latest` before publishing
4. **Publish** — `DumpAnalysisHub.Publish(result)` notifies all UI panels
5. **Auto-diff** — If previous exists, `DumpDiffService.ComputeDiff()` + `DumpAnalysisHub.PublishDiff(diff)`

This pipeline was previously split across `DumpWatcherService` (for file-based captures) and was consolidated into `DiagnosticsService` as the single entry point for the ClrMD path.

### 7.7 Key Design: Retention Ratio Semantics

The per-type retention ratio is computed as `heapDelta / allocationThroughput`:

- **1.0** = 100% retention — everything allocated is still on the heap (leak suspect)
- **0.0** = 0% retention — everything allocated was collected (healthy churn)
- **null** = no allocation data or zero throughput for this type
- **Capped at 1.0** — values > 1.0 indicate pre-existing objects grew (not an allocation ratio issue)

Only types with `allocBetween > 0 && deltaSize > 0` get a non-null, non-zero retention ratio. Types where the heap shrank despite allocations get `retention = 0.0` (good — collection outpaced allocation).

---

## 8. Project Structure

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
│   │   ├── MemoryHealthPanel.razor (+.css)     (Phase 5)
│   │   ├── DumpAnalysisPanel.razor (+.css)     (Phase 4)
│   │   ├── DumpDiffPanel.razor (+.css)         (Phase 4+5)
│   │   └── DumpHistoryPanel.razor (+.css)      (Phase 4)
│   └── Views/
│       ├── MetricsDashboardView.razor (+.css)
│       ├── MemoryDiagnosticsView.razor (+.css)
│       └── DumpAnalysisView.razor (+.css)      (Phase 4)
├── Diagnostics/                            (Phase 2+4+5)
│   ├── DiagnosticsOptions.cs
│   ├── IDiagnosticsService.cs
│   ├── DiagnosticsService.cs
│   ├── ClrMdHeapAnalyzer.cs                (Phase 5)
│   ├── DumpAnalyzerOptions.cs              (Phase 4)
│   ├── DumpDiffService.cs                  (Phase 4+5)
│   └── Models/                             (Phase 4+5)
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
│   └── DumpAnalysisHub.cs                  (Phase 4)
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

## 9. Data Models

All library models are `sealed record` types (immutable, value-equality, `with`-expression support). Core metrics models are in `AppSysMetrics.Models`; dump analysis models are in `AppSysMetrics.Diagnostics.Models`; diagnostics action results (`ForceGcResult`, `GcDumpResult`) are defined alongside `IDiagnosticsService` in `AppSysMetrics.Diagnostics`.

### 9.1 MetricsSnapshot

The top-level container produced by `MetricsCollector.Collect()` every 2 seconds.

| Property | Type | Source |
|---|---|---|
| TimestampTicks | long | `Stopwatch.GetTimestamp()` |
| CapturedAt | DateTimeOffset | `DateTimeOffset.UtcNow` |
| Process | ProcessMetrics | `Process.GetCurrentProcess()` |
| Cpu | CpuMetrics | `CpuSampler.Sample()` |
| Gc | GcMetrics | `GC.GetGCMemoryInfo()` + `GC.CollectionCount()` |

### 9.2 ProcessMetrics

| Property | Type | Description |
|---|---|---|
| WorkingSet64 | long | Physical memory (bytes) — what Task Manager shows |
| PrivateMemorySize64 | long | Private committed memory (bytes) |
| VirtualMemorySize64 | long | Total virtual address space (bytes) |
| PagedMemorySize64 | long | Paged memory (bytes) |
| ThreadCount | int | OS thread count |
| HandleCount | int | OS handle count |

### 9.3 CpuMetrics

| Property | Type | Description |
|---|---|---|
| CpuPercentage | double | Sampled CPU % (0–100), normalized by processor count |
| TotalProcessorTime | TimeSpan | Cumulative CPU time since process start |
| ProcessorCount | int | `Environment.ProcessorCount` |

### 9.4 GcMetrics

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

### 9.5 GcGenerationInfo

| Property | Type | Description |
|---|---|---|
| Generation | int | 0, 1, 2, 3 (LOH), 4 (POH) |
| SizeBeforeBytes | long | Generation size before last collection |
| SizeAfterBytes | long | Generation size after last collection |
| FragmentationBeforeBytes | long | Fragmentation before last collection |
| FragmentationAfterBytes | long | Fragmentation after last collection |

### 9.6 AllocationTypeInfo (Phase 2)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from AllocationTick event |
| TotalBytes | long | Cumulative bytes allocated for this type |
| AllocationCount | int | Number of allocation ticks observed |
| IsLargeObject | bool | True if allocated on the LOH (>= 85,000 bytes) |

### 9.7 AllocationSnapshot (Phase 2+5)

| Property | Type | Description |
|---|---|---|
| CapturedAt | DateTimeOffset | When the snapshot was taken |
| TopAllocatingTypes | IReadOnlyList\<AllocationTypeInfo\> | Top N types by total bytes (descending) |
| RecentLargeObjectAllocations | IReadOnlyList\<AllocationTypeInfo\> | Recent LOH allocations |
| TotalTrackedBytes | long | Sum of all tracked allocation bytes |
| TotalTrackedCount | int | Sum of all tracked allocation counts |
| AppTrackedBytes | long | App allocations (types NOT in `AppSysMetrics.*` namespace) (Phase 5) |
| AppTrackedCount | int | App allocation count (Phase 5) |
| LibraryTrackedBytes | long | Library overhead (types in `AppSysMetrics.*` namespace) (Phase 5) |
| LibraryTrackedCount | int | Library allocation count (Phase 5) |

### 9.8 HeapTypeInfo (Phase 4)

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from ClrMD heap enumeration |
| InstanceCount | long | Number of instances of this type on the heap |
| TotalSizeBytes | long | Total bytes consumed by all instances of this type |

### 9.9 DumpAnalysisResult (Phase 4+5)

| Property | Type | Description |
|---|---|---|
| FilePath | string | Full path to `.gcdump` file, or `clrmd://heap_yyyyMMdd_HHmmss` for in-process snapshots |
| FileName | string | File name only (for display) |
| CapturedAtUtc | DateTimeOffset | When the snapshot was captured |
| AnalyzedAtUtc | DateTimeOffset | When the analysis completed |
| FileSizeBytes | long | Size of `.gcdump` file on disk (0 for ClrMD snapshots) |
| TotalHeapBytes | long | Total GC heap size |
| TotalObjectCount | long | Total GC heap object count |
| TopTypes | IReadOnlyList\<HeapTypeInfo\> | Top N types by total size, descending |
| UnresolvedTypeCount | int | Types with unresolved names (UNKNOWN 0x...). Always 0 for ClrMD. (Phase 5) |
| AllocationAtCapture | AllocationSnapshot? | Allocation snapshot at capture time for correlation analysis. Null for legacy path. (Phase 5) |

### 9.10 HeapTypeDiff (Phase 4+5)

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
| BaselineAllocatedBytes | long? | Cumulative bytes allocated at baseline dump time (Phase 5) |
| CurrentAllocatedBytes | long? | Cumulative bytes allocated at current dump time (Phase 5) |
| AllocatedBetweenBytes | long? | Bytes allocated between the two dumps (throughput) (Phase 5) |
| RetentionRatio | double? | `heapDelta / allocationThroughput` — 1.0 = leak suspect, 0.0 = healthy churn (Phase 5) |

### 9.11 DumpDiffResult (Phase 4+5)

| Property | Type | Description |
|---|---|---|
| Baseline | DumpAnalysisResult | The older dump analysis |
| Current | DumpAnalysisResult | The newer dump analysis |
| TimeBetweenDumps | TimeSpan | `Current.CapturedAtUtc - Baseline.CapturedAtUtc` |
| TypeDiffs | IReadOnlyList\<HeapTypeDiff\> | Per-type diffs, sorted by retention ratio descending when correlation available, otherwise by `DeltaSizeBytes` descending |
| TotalHeapDelta | long | `Current.TotalHeapBytes - Baseline.TotalHeapBytes` |
| TotalObjectDelta | long | `Current.TotalObjectCount - Baseline.TotalObjectCount` |
| HasAllocationCorrelation | bool | True when both dumps carry allocation snapshots (Phase 5) |
| TotalAllocatedBetween | long? | Total bytes allocated (app-only) between the two dumps (Phase 5) |
| TotalCollectedBetween | long? | Total bytes collected between dumps (allocated minus heap growth) (Phase 5) |

### 9.12 ForceGcResult (Phase 2)

Defined in `IDiagnosticsService.cs`. Returned by `IDiagnosticsService.ForceGC()`.

| Property | Type | Description |
|---|---|---|
| Before | GcMetrics | GC metrics captured before the forced collection (required) |
| After | GcMetrics | GC metrics captured after the forced collection (required) |
| Duration | TimeSpan | Wall-clock time for the GC operation (required) |
| PerformedAt | DateTimeOffset | When the operation completed (required) |

### 9.13 GcDumpResult (Phase 2)

Defined in `IDiagnosticsService.cs`. Returned by `CaptureGcDumpAsync()` and `CaptureGcDumpFileAsync()`.

| Property | Type | Description |
|---|---|---|
| Success | bool | Whether the operation completed successfully |
| FilePath | string? | Path to `.gcdump` file (null for in-process ClrMD snapshots) |
| ErrorMessage | string? | Error details when `Success` is false |
| FileSizeBytes | long | File size in bytes (0 for ClrMD snapshots) |
| CapturedAt | DateTimeOffset | When the capture was performed |

---

## 10. Services and Hosting

### 10.1 DI Registration

```csharp
// Consumer's Program.cs
builder.Services.AddAppSysMetrics(options =>
{
    options.CollectionInterval = TimeSpan.FromSeconds(2);
    options.MaxHistorySize = 60;
});
```

### 10.2 AppSysMetrics Service Registrations

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

### 10.3 Hub Pattern

Both `MetricsHub` and `AllocationTrackingHub` follow the same pattern:

- **Ring buffer**: `List<T>` capped at `MaxHistorySize`, oldest entries removed on overflow
- **Thread safety**: `lock` on publish and read operations
- **Event**: `Action<T>? OnSnapshot` invoked after publish, outside the lock
- **Latest**: Property holding the most recent snapshot for newly subscribing components
- **GetHistory()**: Returns a copy of the ring buffer for chart rendering

### 10.4 Background Services

Both `MetricsCollectionService` and `AllocationTrackingService` use `PeriodicTimer` in `ExecuteAsync`:

```
while (await timer.WaitForNextTickAsync(stoppingToken))
{
    var snapshot = collector/listener.Collect/CreateSnapshot();
    hub.Publish(snapshot);
}
```

Exceptions within the loop are caught and logged, not propagated, to keep the service running.

### 10.5 Heap Analysis Service Registrations (Phase 4+5)

| Service | Lifetime | Interface |
|---|---|---|
| ClrMdHeapAnalyzer | Singleton | (concrete) |
| DumpDiffService | Singleton | (concrete) |
| DumpAnalysisHub | Singleton | (concrete) |
| DumpAnalyzerOptions | Options | IOptions\<T\> |

### 10.6 DumpAnalysisHub Pattern (Phase 4)

`DumpAnalysisHub` follows the same ring buffer pattern as `MetricsHub` and `AllocationTrackingHub` but with three events:

- **`OnAnalysis`** — Fires when a new dump is analyzed. Subscribers receive the `DumpAnalysisResult`.
- **`OnDiff`** — Fires when a diff is computed (either auto-diff from `DiagnosticsService` or manual comparison from the UI). Subscribers receive the `DumpDiffResult`.
- **`OnCleared`** — Fires when `Clear()` is called, resetting all state.

Properties: `Latest` (most recent analysis), `LatestDiff` (most recent diff), `GetHistory()` (defensive copy of all analyses).
Methods: `Publish()`, `PublishDiff()` (internal), `Clear()` (public, resets history and fires `OnCleared`).

### 10.7 ClrMdHeapAnalyzer Pattern (Phase 5)

`ClrMdHeapAnalyzer` is an on-demand service (not a background service). It is invoked by `DiagnosticsService.CaptureGcDumpAsync()` when the user clicks "Capture Heap Snapshot":

```
DiagnosticsService.CaptureGcDumpAsync()
    → ClrMdHeapAnalyzer.CaptureAndAnalyzeAsync()
        → Task.Run(() => CaptureCore())
            → DataTarget.CreateSnapshotAndAttach(pid)
            → heap.EnumerateObjects() → aggregate by type
            → return DumpAnalysisResult
    → Enrich with AllocationSnapshot
    → DumpAnalysisHub.Publish(result)
    → DumpDiffService.ComputeDiff() + PublishDiff()
```

Concurrency is serialized via `SemaphoreSlim(1)` with a 5-second timeout. If a capture is already in progress, the request is skipped with a warning log.

---

## 11. UI Components

All chart, panel, and view components listed below are shipped in the library.

### 11.1 Component Namespaces

| Namespace | Contents |
|---|---|
| `AppSysMetrics.Components.Charts` | BarChart, LineChart, GaugeChart, MetricCard |
| `AppSysMetrics.Components.Panels` | ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel, TopAllocationsPanel, LargeObjectAllocationsPanel, DiagnosticsPanel, MemoryHealthPanel, DumpAnalysisPanel, DumpDiffPanel, DumpHistoryPanel |
| `AppSysMetrics.Components.Views` | MetricsDashboardView, MemoryDiagnosticsView, DumpAnalysisView |

### 11.2 Consumer Page Integration

The library ships views, not pages. Consumers create their own thin page wrappers:

| View Tag | Typical Route | Purpose |
|---|---|---|
| `<MetricsDashboardView />` | `/`, `/dashboard`, `/metrics` | Live process, CPU, GC, and allocation rate metrics |
| `<MemoryDiagnosticsView />` | `/diagnostics` | Allocation tracking, LOH alerts, Force GC, GC Dump capture |
| `<DumpAnalysisView />` | `/dump-analysis` | Dump file analysis, diff comparison, memory leak detection |

Each page is typically 3–6 lines: `@page`, `@rendermode`, `<PageTitle>`, and the view tag. `MetricsDashboardView` accepts a `RenderFragment? AdditionalContent` parameter for injecting app-specific panels.

### 11.3 Chart Components (AppSysMetrics library)

| Component | Visualization | Rendering |
|---|---|---|
| BarChart | Vertical bars with labels and gridlines | SVG 400x200 viewBox, `BuildSvg()` + `MarkupString` |
| LineChart | Area-fill polyline with stroke and end-point indicator | SVG 400x180 viewBox, `BuildSvg()` + `MarkupString` |
| GaugeChart | 180-degree arc gauge, color-coded by threshold | SVG 200x130 viewBox, `BuildSvg()` + `MarkupString` |
| MetricCard | Title / value / subtitle card | Razor markup, scoped CSS |

All chart components accept parameters for data, titles, units, colors, and ranges. None use JavaScript.

### 11.4 Panel Components (AppSysMetrics library)

| Panel | Data Source | Key Visuals |
|---|---|---|
| ProcessMetricsPanel | MetricsSnapshot | BarChart (memory breakdown), MetricCards (threads, handles, virtual) |
| CpuMetricsPanel | List\<MetricsSnapshot\> | LineChart (CPU % history), MetricCards (current, processors, total time) |
| GcMetricsPanel | MetricsSnapshot | GaugeChart (memory load %), generation table, MetricCards (collections, pause %, finalizers) |
| AllocationRatePanel | List\<MetricsSnapshot\> | LineChart (allocation rate MB/s), MetricCards (current rate, total allocated) |
| TopAllocationsPanel | AllocationSnapshot | Ranked table of types with monospace type names, bytes, counts |
| LargeObjectAllocationsPanel | AllocationSnapshot | LOH allocation table with alert styling, or "no LOH" indicator |
| DiagnosticsPanel | IDiagnosticsService | Force GC button with before/after comparison, Capture Heap Snapshot button (ClrMD), Capture GC Dump button (dotnet-gcdump file export with file path/size) |
| MemoryHealthPanel | MetricsSnapshot, AllocationSnapshot | Memory health indicators with trend detection for allocation rate and heap size (Phase 5) |
| DumpAnalysisPanel | DumpAnalysisResult | MetricCards (heap size, object count, file name), ranked top 20 types table, UNKNOWN type warning when present |
| DumpDiffPanel | DumpDiffResult | 4-zone layout when correlation available: summary MetricCards with collection efficiency %, narrative banner (green/yellow/red), leak suspects call-out (top 5 by retention), retention-sorted type diff table. Falls back to standard diff table when no correlation. |
| DumpHistoryPanel | IReadOnlyList\<DumpAnalysisResult\> | Interactive click-to-select table (BASE/CUR tags by chronological order), "Compare Selected" button, "Clear All" button (deletes files from disk) |

### 11.5 Composite View Components (AppSysMetrics library)

| View | Injects | Grid Content | Parameter |
|---|---|---|---|
| MetricsDashboardView | MetricsHub | MemoryHealth (full width), ProcessMetrics + CPU + GC + AllocationRate (2x2), optional full-width slot | `RenderFragment? AdditionalContent` |
| MemoryDiagnosticsView | AllocationTrackingHub, MetricsHub | MemoryHealth (full width), Diagnostics (full width), TopAllocations (full width), LOH + GC (side-by-side) | — |
| DumpAnalysisView | DumpAnalysisHub, MetricsHub | MemoryHealth (full width), DumpHistory (full width), DumpAnalysis + DumpDiff (side-by-side) | — |

### 11.6 Blazor Component Lifecycle Pattern

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

## 12. Dependency Inventory

### 12.1 NuGet Packages

| Package | Version | Used By | Purpose |
|---|---|---|---|
| Microsoft.Diagnostics.Runtime | 3.1.512801 | ClrMdHeapAnalyzer | In-process heap analysis via ClrMD (Phase 5) |

### 12.2 Framework References

| Reference | Used By | Provides |
|---|---|---|
| Microsoft.AspNetCore.App | AppSysMetrics | Razor component compilation, Hosting.Abstractions, Logging.Abstractions, Options, DI |

### 12.3 External Tools

| Tool | Required By | Install Command |
|---|---|---|
| dotnet-gcdump | DiagnosticsService.CaptureGcDumpFileAsync() (file-based GC Dump export only) | `dotnet tool install --global dotnet-gcdump` |

The tool is only required for the "Capture GC Dump" button which exports a `.gcdump` file to disk. The primary "Capture Heap Snapshot" feature uses ClrMD in-process and does not require any external tools.

### 12.4 Static Assets

| Asset | Path | Purpose |
|---|---|---|
| AppSysMetrics.css | `_content/AppSysMetrics/AppSysMetrics.css` | Shared library component styles (panels, tables, buttons, state colors) |
| AppSysMetrics.styles.css | Auto-generated by Blazor CSS isolation | Scoped component CSS for charts, panels, views |

### 12.5 Runtime APIs

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
| `DataTarget.CreateSnapshotAndAttach` | Microsoft.Diagnostics.Runtime | In-process heap snapshot via ClrMD (Phase 5) |
| `ClrHeap.EnumerateObjects` | Microsoft.Diagnostics.Runtime | Heap object enumeration (Phase 5) |
| `SemaphoreSlim` | System.Threading | Concurrency serialization for heap capture (Phase 5) |

---

## 13. Design Decisions

### 13.1 Why sealed records for metrics?

Records provide value equality and immutable snapshots. `sealed` prevents inheritance overhead. The combination is ideal for data that's created once, published to a hub, and read by multiple consumers — no defensive copying needed.

### 13.2 Why MarkupString + StringBuilder for SVG?

Razor's parser treats `<text>` as a directive, not an HTML/SVG element. Since SVG uses `<text>` extensively for labels, axis values, and gauge readouts, the only clean solution is to build the SVG string outside of Razor's parser and inject it as raw markup. This also avoids Razor issues with `<` in switch expressions used for threshold-based coloring.

### 13.3 Why not use a JavaScript charting library?

The solution demonstrates that Blazor Server can render rich visualizations without any JavaScript interop. The SVG approach has zero JS payload, no npm dependencies, no bundling, and updates instantly via SignalR without client-side re-rendering.

### 13.4 Why two separate hubs?

Metrics snapshots and allocation snapshots serve different diagnostic questions. Metrics answer "how is the process doing right now?" while allocation snapshots answer "what types are consuming the most memory?" Coupling them into a single snapshot would force both collection mechanisms onto the same timer and make the API less composable for consumers that only need one view.

### 13.5 Why ClrMD instead of dotnet-gcdump? (Phase 5)

`dotnet-gcdump` relies on EventPipe to emit `GCBulkType` events for type name resolution. A .NET 8+ regression (dotnet/diagnostics #5116) causes these events not to be re-emitted on subsequent captures from the same process, producing `UNKNOWN 0x...` type names. ClrMD (`Microsoft.Diagnostics.Runtime`) reads type metadata directly from CLR method tables and the DAC, which is immune to this regression. The trade-off is a NuGet dependency, but the gain is reliable type names on every capture and no external tool requirement for the primary heap snapshot path. The original `dotnet-gcdump collect` is retained as `CaptureGcDumpFileAsync()` for users who need `.gcdump` file export.

### 13.6 Why one library instead of separate Core + UI packages?

The Razor SDK is additive — existing C# compiles identically. A single package avoids version coordination between "core" and "UI" packages. Consumers who only need the backend can ignore the Components namespace; Razor components add negligible binary size. The `FrameworkReference` to `Microsoft.AspNetCore.App` actually simplified the `.csproj` by replacing three explicit NuGet package references.

### 13.7 Why no @page or @rendermode in library view components?

Hardcoding routes in a library claims URL paths from the consumer. Hardcoding render mode prevents consumers from choosing InteractiveServer vs InteractiveWebAssembly vs InteractiveAuto. By shipping views as plain components, the library remains composable: the consumer wraps them in their own pages with their own routing and render mode decisions. The `RenderFragment? AdditionalContent` parameter on `MetricsDashboardView` further enables extensibility without modification.

### 13.8 Why no Bootstrap dependency in library components?

Library components use zero Bootstrap CSS classes. All styling is self-contained via `AppSysMetrics.css` (shared base) and scoped `.razor.css` (per-component layout). This makes the library portable to any CSS framework or custom design system.

### 13.9 Why text parsing was replaced by ClrMD (Phase 4 → Phase 5)

Phase 4 parsed `dotnet-gcdump report` text output to avoid NuGet dependencies. Phase 5 replaced this approach with ClrMD (`Microsoft.Diagnostics.Runtime`) because the EventPipe regression in .NET 8+ made the text output unreliable (UNKNOWN type names on repeated captures). The text parser (`DumpReportParser`), its consumer (`DumpAnalyzerService`), and the file watcher (`DumpWatcherService`) were removed. The trade-off of adding a NuGet dependency was justified by reliable type resolution and the elimination of external tool requirements for the primary analysis path.

### 13.10 Why on-demand capture replaced file watching? (Phase 4 → Phase 5)

Phase 4 used `FileSystemWatcher` + `Channel<string>` to automatically process `.gcdump` files as they appeared. Phase 5 replaced this with on-demand in-process capture via `ClrMdHeapAnalyzer`, triggered by a UI button click. This is simpler (no background service, no file watching, no file-ready detection), more reliable (no race conditions with file locking or partial writes), and faster (no subprocess, no file I/O). The enrichment pipeline (allocation correlation, auto-diff) was consolidated into `DiagnosticsService` as a single entry point.

### 13.11 Why dual events on DumpAnalysisHub? (Phase 4)

`DumpAnalysisHub` separates `OnAnalysis` and `OnDiff` events because the first dump has no diff, and manual UI comparisons produce diffs without new analyses. Each bottom panel in `DumpAnalysisView` subscribes to exactly the event it needs. See Section 6.4 for details.

### 13.12 Why a third hub instead of extending existing hubs? (Phase 4)

`DumpAnalysisHub` is independent from `MetricsHub` and `AllocationTrackingHub` because heap analysis operates on a fundamentally different trigger (on-demand user action) rather than a periodic timer. The data lifecycle is different — analyses accumulate as discrete snapshots, not continuous time-series data. This follows the same separation principle described in Section 13.4 for the first two hubs.

### 13.13 Why a 4-zone narrative UI for DumpDiffPanel? (Phase 5)

Raw diff tables show numbers but don't answer "is the heap healthy?". The 4-zone layout provides progressive disclosure: executive summary (MetricCards with collection efficiency), narrative prose (colored banner), actionable alerts (leak suspects with per-type breakdown), then full detail (retention-sorted table). This mirrors how a developer would manually analyze dumps: check overall health first, look for red flags, then drill into specifics. The narrative computes `collected = allocated − heapGrowth` and `efficiency = collected / allocated`, which are the two numbers that immediately distinguish healthy churn from a leak.

### 13.14 Why enrich DumpAnalysisResult with AllocationSnapshot? (Phase 5)

Heap snapshots alone show what's on the heap but not what was allocated. By attaching an `AllocationSnapshot` at capture time, the diff service can compute per-type retention ratios: "Type X had 500 KB allocated but 500 KB retained = 100% retention = leak suspect". Without this enrichment, a type with 500 KB heap growth could be healthy (if 10 MB was allocated and 9.5 MB collected) or a leak (if only 500 KB was allocated). The `AllocationAtCapture` field is nullable to handle edge cases where the allocation listener isn't available.
