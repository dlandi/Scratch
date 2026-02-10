# AppSysMetrics Software Specification

**Version:** 2.0
**Date:** February 10, 2026
**Target Framework:** .NET 10.0 (SDK 10.0.102)
**Package:** Razor Class Library (`Microsoft.NET.Sdk.Razor`)

> This document specifies the current state of the AppSysMetrics library — its architecture, data models, services, UI components, and APIs. For implementation history (phases, what changed, what was replaced), see **AppSysMetrics_ImplementationPlan.md**.

---

## Table of Contents

1. [Overview](#1-overview)
2. [Architecture](#2-architecture)
3. [Project Structure](#3-project-structure)
4. [Data Models](#4-data-models)
5. [Collection Layer](#5-collection-layer)
6. [Hosting Layer](#6-hosting-layer)
7. [Diagnostics Layer](#7-diagnostics-layer)
8. [UI Components](#8-ui-components)
9. [Consumer Integration](#9-consumer-integration)
10. [Dependencies](#10-dependencies)
11. [Design Rationale](#11-design-rationale)

---

## 1. Overview

### 1.1 Purpose

AppSysMetrics is a self-contained Razor Class Library that provides real-time, in-process observability for .NET applications. A single project reference gives consumers both the metrics backend (collection, diagnostics) and a full set of Blazor UI components (charts, panels, composite dashboard views).

It captures two distinct classes of runtime data:

- **Process-level metrics** — Working set, private memory, virtual memory, thread/handle counts, CPU utilization. These represent the OS view of the process.
- **Managed heap metrics** — GC heap size, fragmentation, generation info, allocation rate, pause time, finalization queue depth. These represent the CLR view of managed memory.

### 1.2 Goals

1. Live dashboard refreshing every 2 seconds with process, CPU, and GC metrics.
2. Allocation tracking by type using in-process event tracing, without external profiling tools.
3. On-demand diagnostic actions (Force GC, Heap Snapshot, GC Dump file export) from the browser UI.
4. In-process heap analysis via ClrMD with allocation/retention correlation and leak detection narrative.
5. GC retention path tracing from leak suspects back to user code, producing field-level ownership chains.
6. Pure SVG visualizations — no JavaScript charting dependencies.
7. Single Razor Class Library — one project reference provides both backend services and Blazor UI.

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

### 2.2 Library Layers

| Layer | Contents | Consumer Uses |
|---|---|---|
| **Backend** | Models, Collection, Hosting, Diagnostics, Extensions | `builder.Services.AddAppSysMetrics()` |
| **Primitives** | BarChart, LineChart, GaugeChart, MetricCard | Mix-and-match in custom layouts |
| **Panels** | ProcessMetricsPanel, CpuMetricsPanel, GcMetricsPanel, AllocationRatePanel, TopAllocationsPanel, LargeObjectAllocationsPanel, DiagnosticsPanel, DumpAnalysisPanel, DumpDiffPanel, DumpHistoryPanel, MemoryHealthPanel, GcRootAnalysisPanel | Drop individual panels into existing pages |
| **Composites** | MetricsDashboardView, MemoryDiagnosticsView, DumpAnalysisView | Full dashboard experience with one tag |
| **Stylesheet** | `_content/AppSysMetrics/AppSysMetrics.css` | Shared component styles (panels, tables, buttons) |

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
      ├─ PredictLeakSuspectTypes()                 (two-track: high retention OR large growth)
      ├─ ClrMdHeapAnalyzer.CaptureAndAnalyzeAsync(rootTargets)
      │    ├─ DataTarget.CreateSnapshotAndAttach(pid)
      │    ├─ heap.EnumerateObjects() → DumpAnalysisResult
      │    └─ GcRootAnalyzer.AnalyzeRoots(heap, targets)
      │         ├─ Build GC root address set
      │         ├─ Single heap pass: parent map + target instances
      │         ├─ Score instances by user-code proximity
      │         └─ Walk backward → retention paths
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

| Component | Thread Strategy | Synchronization |
|---|---|---|
| MetricsCollectionService | `BackgroundService` with `PeriodicTimer` | None (single producer) |
| AllocationTrackingService | `BackgroundService` with `PeriodicTimer` | None (single producer) |
| AllocationEventListener | CLR event thread callbacks | `ConcurrentDictionary` + `Interlocked` operations |
| MetricsHub, AllocationTrackingHub | Any thread via `Publish()` | `lock` on ring buffer; event invoked outside lock |
| DumpAnalysisHub | Any thread via `Publish()` | `lock` on ring buffer; event invoked outside lock |
| ClrMdHeapAnalyzer | `Task.Run` for CPU-bound enumeration | `SemaphoreSlim(1)` with 5-second timeout |
| GcRootAnalyzer | Called synchronously within `ClrMdHeapAnalyzer.CaptureCore()` | Per-type and global `Stopwatch` timeouts |
| Blazor components | Sync context via `InvokeAsync` | Subscribe in `OnInitialized`, unsubscribe in `Dispose` |

---

## 3. Project Structure

```
AppSysMetrics/                              (Razor Class Library — net10.0)
├── AppSysMetrics.csproj                    (Sdk="Microsoft.NET.Sdk.Razor")
├── Collection/
│   ├── IMetricsCollector.cs
│   ├── MetricsCollector.cs
│   ├── CpuSampler.cs
│   ├── AllocationRateTracker.cs
│   └── AllocationEventListener.cs
├── Components/
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
│   │   ├── MemoryHealthPanel.razor (+.css)
│   │   ├── DumpAnalysisPanel.razor (+.css)
│   │   ├── DumpDiffPanel.razor (+.css)
│   │   ├── DumpHistoryPanel.razor (+.css)
│   │   └── GcRootAnalysisPanel.razor (+.css)
│   └── Views/
│       ├── MetricsDashboardView.razor (+.css)
│       ├── MemoryDiagnosticsView.razor (+.css)
│       └── DumpAnalysisView.razor (+.css)
├── Diagnostics/
│   ├── AnalyzeMemoryLeaksAttribute.cs
│   ├── DiagnosticsOptions.cs
│   ├── IDiagnosticsService.cs
│   ├── DiagnosticsService.cs
│   ├── ClrMdHeapAnalyzer.cs
│   ├── GcRootAnalyzer.cs
│   ├── DumpAnalyzerOptions.cs
│   ├── DumpDiffService.cs
│   └── Models/
│       ├── HeapTypeInfo.cs
│       ├── DumpAnalysisResult.cs
│       ├── HeapTypeDiff.cs
│       ├── DumpDiffResult.cs
│       ├── RootAnalysisResult.cs
│       ├── TypeRootAnalysis.cs
│       └── GcRootInfo.cs
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Hosting/
│   ├── MetricsCollectionOptions.cs
│   ├── MetricsHub.cs
│   ├── MetricsCollectionService.cs
│   ├── AllocationTrackingHub.cs
│   ├── AllocationTrackingService.cs
│   └── DumpAnalysisHub.cs
├── Models/
│   ├── MetricsSnapshot.cs
│   ├── ProcessMetrics.cs
│   ├── CpuMetrics.cs
│   ├── GcMetrics.cs
│   ├── GcGenerationInfo.cs
│   ├── AllocationTypeInfo.cs
│   └── AllocationSnapshot.cs
└── wwwroot/
    └── AppSysMetrics.css
```

---

## 4. Data Models

All models are `sealed record` types (immutable, value-equality, `with`-expression support). Core metrics models are in `AppSysMetrics.Models`; dump analysis models are in `AppSysMetrics.Diagnostics.Models`; diagnostics action results are defined alongside `IDiagnosticsService` in `AppSysMetrics.Diagnostics`.

### 4.1 MetricsSnapshot

Top-level container produced by `MetricsCollector.Collect()` every 2 seconds.

| Property | Type | Source |
|---|---|---|
| TimestampTicks | long | `Stopwatch.GetTimestamp()` |
| CapturedAt | DateTimeOffset | `DateTimeOffset.UtcNow` |
| Process | ProcessMetrics | `Process.GetCurrentProcess()` |
| Cpu | CpuMetrics | `CpuSampler.Sample()` |
| Gc | GcMetrics | `GC.GetGCMemoryInfo()` + `GC.CollectionCount()` |

### 4.2 ProcessMetrics

| Property | Type | Description |
|---|---|---|
| WorkingSet64 | long | Physical memory (bytes) |
| PrivateMemorySize64 | long | Private committed memory (bytes) |
| VirtualMemorySize64 | long | Total virtual address space (bytes) |
| PagedMemorySize64 | long | Paged memory (bytes) |
| ThreadCount | int | OS thread count |
| HandleCount | int | OS handle count |

### 4.3 CpuMetrics

| Property | Type | Description |
|---|---|---|
| CpuPercentage | double | Sampled CPU % (0–100), normalized by processor count |
| TotalProcessorTime | TimeSpan | Cumulative CPU time since process start |
| ProcessorCount | int | `Environment.ProcessorCount` |

### 4.4 GcMetrics

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
| FinalizationPendingCount | long | Objects waiting for finalization |
| GenerationInfo | IReadOnlyList\<GcGenerationInfo\> | Per-generation size/fragmentation detail |

### 4.5 GcGenerationInfo

| Property | Type | Description |
|---|---|---|
| Generation | int | 0, 1, 2, 3 (LOH), 4 (POH) |
| SizeBeforeBytes | long | Generation size before last collection |
| SizeAfterBytes | long | Generation size after last collection |
| FragmentationBeforeBytes | long | Fragmentation before last collection |
| FragmentationAfterBytes | long | Fragmentation after last collection |

### 4.6 AllocationTypeInfo

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from AllocationTick event |
| TotalBytes | long | Cumulative bytes allocated for this type |
| AllocationCount | int | Number of allocation ticks observed |
| IsLargeObject | bool | True if allocated on the LOH (>= 85,000 bytes) |

### 4.7 AllocationSnapshot

| Property | Type | Description |
|---|---|---|
| CapturedAt | DateTimeOffset | When the snapshot was taken |
| TopAllocatingTypes | IReadOnlyList\<AllocationTypeInfo\> | Top N types by total bytes (descending) |
| RecentLargeObjectAllocations | IReadOnlyList\<AllocationTypeInfo\> | Recent LOH allocations |
| TotalTrackedBytes | long | Sum of all tracked allocation bytes |
| TotalTrackedCount | int | Sum of all tracked allocation counts |
| AppTrackedBytes | long | App allocations (types NOT in `AppSysMetrics.*` namespace) |
| AppTrackedCount | int | App allocation count |
| LibraryTrackedBytes | long | Library overhead (types in `AppSysMetrics.*` namespace) |
| LibraryTrackedCount | int | Library allocation count |

### 4.8 HeapTypeInfo

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully qualified type name from ClrMD heap enumeration |
| InstanceCount | long | Number of instances of this type on the heap |
| TotalSizeBytes | long | Total bytes consumed by all instances of this type |

### 4.9 DumpAnalysisResult

| Property | Type | Description |
|---|---|---|
| FilePath | string | `clrmd://heap_yyyyMMdd_HHmmss` for in-process snapshots, or `.gcdump` file path |
| FileName | string | File name only (for display) |
| CapturedAtUtc | DateTimeOffset | When the snapshot was captured |
| AnalyzedAtUtc | DateTimeOffset | When the analysis completed |
| FileSizeBytes | long | File size on disk (0 for ClrMD snapshots) |
| TotalHeapBytes | long | Total GC heap size |
| TotalObjectCount | long | Total GC heap object count |
| TopTypes | IReadOnlyList\<HeapTypeInfo\> | Top N types by total size, descending |
| UnresolvedTypeCount | int | Types with unresolved names. Always 0 for ClrMD. |
| AllocationAtCapture | AllocationSnapshot? | Allocation snapshot at capture time for correlation analysis |
| RootAnalysis | RootAnalysisResult? | GC root analysis for predicted leak-suspect types, captured during the same heap snapshot. Null when root analysis was not requested (first two captures) or failed. |

### 4.10 HeapTypeDiff

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
| BaselineAllocatedBytes | long? | Cumulative bytes allocated at baseline dump time |
| CurrentAllocatedBytes | long? | Cumulative bytes allocated at current dump time |
| AllocatedBetweenBytes | long? | Bytes allocated between the two dumps (throughput) |
| RetentionRatio | double? | `heapDelta / allocationThroughput` — 1.0 = leak suspect, 0.0 = healthy churn |

### 4.11 DumpDiffResult

| Property | Type | Description |
|---|---|---|
| Baseline | DumpAnalysisResult | The older dump analysis |
| Current | DumpAnalysisResult | The newer dump analysis |
| TimeBetweenDumps | TimeSpan | `Current.CapturedAtUtc - Baseline.CapturedAtUtc` |
| TypeDiffs | IReadOnlyList\<HeapTypeDiff\> | Per-type diffs, sorted by retention ratio descending when correlation available, otherwise by `DeltaSizeBytes` descending |
| TotalHeapDelta | long | `Current.TotalHeapBytes - Baseline.TotalHeapBytes` |
| TotalObjectDelta | long | `Current.TotalObjectCount - Baseline.TotalObjectCount` |
| HasAllocationCorrelation | bool | True when both dumps carry allocation snapshots |
| TotalAllocatedBetween | long? | Total bytes allocated (app-only) between the two dumps |
| TotalCollectedBetween | long? | Total bytes collected between dumps (allocated minus heap growth) |

### 4.12 RootAnalysisResult

Aggregate result for GC root analysis across all analyzed leak-suspect types. Attached to `DumpAnalysisResult.RootAnalysis`.

| Property | Type | Description |
|---|---|---|
| AnalyzedAtUtc | DateTimeOffset | When the root analysis was performed |
| TypeAnalyses | IReadOnlyList\<TypeRootAnalysis\> | Per-type root analysis results for each predicted leak-suspect type |
| TotalDuration | TimeSpan | Total wall-clock time for all root analysis across all types |
| WasTimedOut | bool | True if analysis was cut short by the global timeout |
| SkippedTypes | IReadOnlyList\<string\> | Type names requested but not found on the heap |

### 4.13 TypeRootAnalysis

Per-type root analysis result, containing the top roots retaining instances of a specific leak-suspect type.

| Property | Type | Description |
|---|---|---|
| TypeName | string | Fully-qualified type name that was analyzed |
| TotalRootCount | int | Total number of distinct retention paths found for this type |
| Roots | IReadOnlyList\<GcRootInfo\> | Top roots retaining this type, limited by `DumpAnalyzerOptions.MaxRootsPerType`. Sorted by `RetainedInstanceCount` descending |
| WasTruncated | bool | True if analysis was truncated due to timeout or max-root limits |
| AnalysisDuration | TimeSpan | Wall-clock time spent analyzing roots for this specific type |

### 4.14 GcRootInfo

A single retention path entry for a specific leak-suspect type. Roots are grouped by retention path with `RetainedInstanceCount` aggregated across matching instances.

| Property | Type | Description |
|---|---|---|
| RootKind | string | Root kind: `UserCode`, `StrongHandle`, `PinnedHandle`, `AsyncPinnedHandle`, `Stack`, `FinalizerQueue`, or `Unknown` |
| RootAddress | ulong | Address of the root object (for deduplication) |
| RootObjectTypeName | string | Type name of the root object — the user-code type or GC root that retains the target |
| RetentionPath | string? | Human-readable retention path (e.g., `MemoryLeakService → _leakedBlobs:List<Byte[]> → _items:Byte[][] → [*]:Byte[]`). Null when the root directly holds the target type |
| RetainedInstanceCount | int | Number of instances of the target type reachable via this path |
| HasUserCode | bool | True if the retention path passes through a user/application code type within `MaxDirectOwnershipDepth` (4) hops |

### 4.15 ForceGcResult

Defined in `IDiagnosticsService.cs`. Returned by `IDiagnosticsService.ForceGC()`.

| Property | Type | Description |
|---|---|---|
| Before | GcMetrics | GC metrics captured before the forced collection (required) |
| After | GcMetrics | GC metrics captured after the forced collection (required) |
| Duration | TimeSpan | Wall-clock time for the GC operation (required) |
| PerformedAt | DateTimeOffset | When the operation completed (required) |

### 4.16 GcDumpResult

Defined in `IDiagnosticsService.cs`. Returned by `CaptureGcDumpAsync()` and `CaptureGcDumpFileAsync()`.

| Property | Type | Description |
|---|---|---|
| Success | bool | Whether the operation completed successfully |
| FilePath | string? | Path to `.gcdump` file (null for in-process ClrMD snapshots) |
| ErrorMessage | string? | Error details when `Success` is false |
| FileSizeBytes | long | File size in bytes (0 for ClrMD snapshots) |
| CapturedAt | DateTimeOffset | When the capture was performed |

---

## 5. Collection Layer

The collection layer gathers raw metrics from the runtime and OS.

### 5.1 IMetricsCollector / MetricsCollector

`MetricsCollector` implements `IMetricsCollector` and orchestrates a single `Collect()` call that samples:

- `Process.GetCurrentProcess()` → `ProcessMetrics`
- `CpuSampler.Sample()` → `CpuMetrics`
- `GC.GetGCMemoryInfo()` + `GC.CollectionCount()` + `AllocationRateTracker.Sample()` → `GcMetrics`

Returns a complete `MetricsSnapshot` with a high-resolution timestamp.

### 5.2 CpuSampler

Computes CPU utilization as a percentage using `TotalProcessorTime` delta over `Stopwatch.GetElapsedTime()` wall-clock delta, normalized by `Environment.ProcessorCount`. Result clamped to [0, 100].

### 5.3 AllocationRateTracker

Samples `GC.GetTotalAllocatedBytes(precise: false)` and computes bytes-per-second allocation rate from the delta since the last sample.

### 5.4 AllocationEventListener

Extends `System.Diagnostics.Tracing.EventListener`. Subscribes to `Microsoft-Windows-DotNETRuntime` at Verbose level with `GCKeyword` (0x1). Processes `AllocationTick` events (ID 10).

- Aggregates by type name into `ConcurrentDictionary<string, AllocationAggregation>` using `Interlocked.Add`
- Tracks LOH allocations (>= 85 KB) in a bounded `ConcurrentQueue`
- Separates app vs library allocations by `AppSysMetrics.*` namespace prefix
- `CreateSnapshot()` produces a point-in-time `AllocationSnapshot` sorted by total bytes descending
- `Reset()` clears all aggregation state

Note: EventListener receives sampled allocation ticks (approximately every 100 KB), not every individual allocation. This provides a statistical view sufficient for identifying dominant allocating types.

---

## 6. Hosting Layer

The hosting layer manages background services, event hubs, and configuration.

### 6.1 Service Registrations

All services are registered via `AddAppSysMetrics()`:

| Service | Lifetime | Interface |
|---|---|---|
| MetricsCollector | Singleton | IMetricsCollector |
| MetricsHub | Singleton | (concrete) |
| MetricsCollectionService | Hosted | BackgroundService |
| AllocationEventListener | Singleton | (concrete) |
| AllocationTrackingHub | Singleton | (concrete) |
| AllocationTrackingService | Hosted | BackgroundService |
| GcRootAnalyzer | Singleton | (concrete) |
| ClrMdHeapAnalyzer | Singleton | (concrete) |
| DumpDiffService | Singleton | (concrete) |
| DumpAnalysisHub | Singleton | (concrete) |
| DiagnosticsService | Singleton | IDiagnosticsService |
| MetricsCollectionOptions | Options | IOptions\<T\> |
| DiagnosticsOptions | Options | IOptions\<T\> |
| DumpAnalyzerOptions | Options | IOptions\<T\> |

### 6.2 MetricsCollectionOptions

| Property | Type | Default | Description |
|---|---|---|---|
| CollectionInterval | TimeSpan | 2 seconds | Polling interval for both background services |
| MaxHistorySize | int | 60 | Ring buffer capacity (~2 minutes at 2s interval) |

### 6.3 Hub Pattern

Three hubs share the same architectural pattern:

| Hub | Data Type | Events |
|---|---|---|
| MetricsHub | MetricsSnapshot | `OnSnapshot` |
| AllocationTrackingHub | AllocationSnapshot | `OnSnapshot` |
| DumpAnalysisHub | DumpAnalysisResult | `OnAnalysis`, `OnDiff`, `OnCleared` |

Common behavior:
- **Ring buffer**: `List<T>` capped at max history, oldest entries removed on overflow
- **Thread safety**: `lock` on publish and read; events invoked outside the lock
- **Latest**: Property holding the most recent item for late-subscribing components
- **GetHistory()**: Returns a defensive copy of the ring buffer

`DumpAnalysisHub` additionally provides:
- `LatestDiff` property for the most recent `DumpDiffResult`
- `Clear()` method that resets all state and fires `OnCleared`
- `Publish()` and `PublishDiff()` (internal) for analysis and diff events

### 6.4 Background Services

Both `MetricsCollectionService` and `AllocationTrackingService` use `PeriodicTimer` in `ExecuteAsync`. Exceptions within the loop are caught and logged, not propagated, to keep the service running.

---

## 7. Diagnostics Layer

The diagnostics layer provides on-demand heap analysis and diagnostic actions.

### 7.1 IDiagnosticsService

```csharp
public interface IDiagnosticsService
{
    ForceGcResult ForceGC();
    Task<GcDumpResult> CaptureGcDumpAsync(CancellationToken cancellationToken = default);
    Task<GcDumpResult> CaptureGcDumpFileAsync(CancellationToken cancellationToken = default);
}
```

| Method | Mechanism | Returns |
|---|---|---|
| `ForceGC()` | `GC.Collect(2, Forced, blocking)` × 2 with `WaitForPendingFinalizers()` between | `ForceGcResult` with before/after `GcMetrics` |
| `CaptureGcDumpAsync()` | ClrMD in-process heap snapshot → enrichment → hub publish → auto-diff | `GcDumpResult` (no file path) |
| `CaptureGcDumpFileAsync()` | Shells out to `dotnet-gcdump collect -p {pid}` | `GcDumpResult` with `.gcdump` file path and size |

### 7.2 ClrMdHeapAnalyzer

On-demand singleton service (not a background service). Uses `DataTarget.CreateSnapshotAndAttach(Environment.ProcessId)` via `PssCreateSnapshot` on Windows. Injects `GcRootAnalyzer` for retention path analysis.

**Capture flow:**
1. Acquire `SemaphoreSlim(1)` with 5-second timeout (skip if already in progress)
2. `Task.Run(() => CaptureCore())` for CPU-bound work
3. Enumerate `heap.EnumerateObjects()`, aggregate by `obj.Type?.Name`
4. If `rootAnalysisTargets` are provided, call `GcRootAnalyzer.AnalyzeRoots(heap, targets)` within the snapshot scope
5. Return top N types by total size as `DumpAnalysisResult` with synthetic path `clrmd://heap_yyyyMMdd_HHmmss`

`CaptureAndAnalyzeAsync()` has two overloads: parameterless (no root analysis) and with optional `IReadOnlyList<string>? rootAnalysisTargets`. Type names are always resolved — no UNKNOWN entries.

### 7.3 Enrichment Pipeline

`DiagnosticsService.CaptureGcDumpAsync()` orchestrates a 6-step pipeline:

1. **Predict** — `PredictLeakSuspectTypes()` selects up to 5 leak-suspect type names from the previous diff (see §7.6)
2. **Capture + Root Analysis** — `ClrMdHeapAnalyzer.CaptureAndAnalyzeAsync(rootTargets)` → `DumpAnalysisResult` with optional `RootAnalysis` attached
3. **Enrich** — Attach `AllocationEventListener.CreateSnapshot()` as `AllocationAtCapture`
4. **Previous** — Read `DumpAnalysisHub.Latest` before publishing
5. **Publish** — `DumpAnalysisHub.Publish(result)` → notifies all UI subscribers
6. **Auto-diff** — If previous exists, `DumpDiffService.ComputeDiff()` + `DumpAnalysisHub.PublishDiff(diff)`

Root analysis requires at least 3 heap captures: the first two establish a baseline diff with leak suspect candidates; the third (and subsequent) captures analyze roots for those predicted suspects.

### 7.4 DumpDiffService

Pure computation service. Joins two `DumpAnalysisResult` on type name, computes deltas.

When both dumps carry `AllocationAtCapture`, computes per-type allocation correlation:
- `AllocatedBetweenBytes` = throughput for each type
- `RetentionRatio` = `heapDelta / allocationThroughput` (capped at 1.0)
- Summary: `TotalAllocatedBetween` (app-only), `TotalCollectedBetween` (allocated minus heap growth)

**Retention ratio semantics:**
- **1.0** = 100% retention — everything allocated is still on the heap (leak suspect)
- **0.0** = 0% retention — everything allocated was collected (healthy churn)
- **null** = no allocation data or zero throughput for this type

### 7.5 GcRootAnalyzer

Singleton service that performs reverse-reference root analysis on a live `ClrHeap` within the DataTarget snapshot scope. Exposes `public bool IsUserCode(string typeName)` for use by the prediction layer.

**Algorithm (5 phases):**

1. **Phase 1 — Build GC root address set.** `heap.EnumerateRoots()` → index every root's address, kind, and type name.

2. **Phase 2 — Single heap pass.** Enumerate all objects: collect target instances by type name, and build a `parentMap` (childAddr → parentAddr) via `EnumerateReferenceAddresses()`. User-code parent preference: when a user-code type claims parenthood of a child that already has a framework parent, the user-code type overwrites it.

3. **Phase 3 — Score and sample.** For each target type, stride-sample up to 500 instances, score each by walking up the parent map (max 10 hops) checking for user-code types. Sort by score descending, take top 50 for root walking.

4. **Phase 4 — Backward walk.** For each sampled instance, walk backward through the parent map until reaching a user-code type (primary goal) or a GC root (fallback). When user code is found, set `rootKind = "UserCode"` and `rootTypeName` to the user-code type name. Framework intermediary check: if the immediate child of the user-code type is a `Microsoft.*`/`Internal.*`/`Interop.*` type, downgrade `foundUserCode` to false (framework plumbing, not deliberate allocation). Resolve field names lazily via `EnumerateReferencesWithFields` only for objects on the final chain.

5. **Phase 5 — Group, filter, rank.** Group hits by retention path. Mark `HasUserCode` when user code appears within `MaxDirectOwnershipDepth` (4) hops. When user-code groups exist, suppress framework-only groups. Sort: user-code paths first, then by retained count descending.

**User assembly detection (two tiers):**

- **Tier 1 (Explicit):** If any loaded assembly carries `[AnalyzeMemoryLeaks]`, only attributed assemblies are user code.
- **Tier 2 (Auto):** If no assemblies have the attribute, all non-framework, non-dynamic assemblies are included. Framework prefixes: `System`, `Microsoft`, `Internal`, `Interop`, `Newtonsoft`, `netstandard`, `mscorlib`, `WindowsBase`.
- In both tiers, the AppSysMetrics assembly itself is excluded via `typeof(GcRootAnalyzer).Assembly.GetName().Name`.

### 7.6 Leak Suspect Prediction

`DiagnosticsService.PredictLeakSuspectTypes()` selects up to 5 leak-suspect types from the most recent diff using two complementary tracks:

**Track 1 — High retention:** `RetentionRatio >= 0.8`. Catches pure leaks where most allocations are retained.

**Track 2 — Large absolute growth:** `RetentionRatio > 0 and < 0.8` with `DeltaSizeBytes >= 1 MB` (absolute floor) or `DeltaSizeBytes >= 20% of TotalHeapDelta` (proportional threshold). Catches diluted leaks where framework throughput (e.g. Kestrel's MemoryPoolBlock) masks the retention ratio for shared types like `Byte[]`.

**Framework noise filtering** (`IsFrameworkOnlyType`): User-code types pass; `System.*` types pass; dot-free types pass only if they match a well-known primitive allowlist (`Byte`, `SByte`, `String`, `Char`, `Int16`, `UInt16`, `Int32`, `UInt32`, `Int64`, `UInt64`, `Double`, `Single`, `Boolean`, `Object`, `IntPtr`, `UIntPtr`, `Decimal`, `Guid`, `DateTime`, `DateTimeOffset`, `TimeSpan`, and their array variants); all other namespaced types (`Microsoft.*`, `Internal.*`, `Interop.*`) are excluded.

Both tracks are unioned, deduplicated, sorted by retention ratio descending then delta size descending, and capped at 5.

### 7.7 DiagnosticsOptions

| Property | Type | Default | Description |
|---|---|---|---|
| GcDumpOutputDirectory | string? | `%TEMP%/AppSysMetrics/gcdumps` | Output directory for `.gcdump` files |

### 7.8 DumpAnalyzerOptions

| Property | Type | Default | Description |
|---|---|---|---|
| MaxAnalysisHistory | int | 10 | Ring buffer capacity for DumpAnalysisHub |
| TopTypesCount | int | 50 | Number of top types to include in analysis results |
| MaxRootAnalysisTypes | int | 5 | Maximum leak-suspect types to analyze per capture |
| MaxRootsPerType | int | 10 | Maximum roots to report per type |
| RootAnalysisPerTypeTimeout | TimeSpan | 10 seconds | Per-type timeout to prevent a single type from blocking the pipeline |
| RootAnalysisGlobalTimeout | TimeSpan | 60 seconds | Global timeout for all root analysis |
| TraceRetentionPaths | bool | true | Whether to build the parent map and trace paths. When false, root analysis runs but skips the expensive heap-wide parent map. |
| MaxRetentionPathDepth | int | 20 | Maximum hops in backward retention path walk |

### 7.9 AnalyzeMemoryLeaksAttribute

Assembly-level attribute for explicit user-code marking during GC root analysis.

```csharp
[assembly: AppSysMetrics.Diagnostics.AnalyzeMemoryLeaks]
```

When any loaded assembly carries this attribute, only attributed assemblies are considered "user code" (Tier 1 detection). If no assemblies have the attribute, auto-discovery is used (Tier 2). See §7.5 for details.

---

## 8. UI Components

All components are shipped in the library under `AppSysMetrics.Components`.

### 8.1 Chart Components (`Components.Charts`)

| Component | Visualization | Rendering |
|---|---|---|
| BarChart | Vertical bars with labels and gridlines | SVG 400×200, `BuildSvg()` + `MarkupString` |
| LineChart | Area-fill polyline with stroke and end-point indicator | SVG 400×180, `BuildSvg()` + `MarkupString` |
| GaugeChart | 180-degree arc gauge, color-coded by threshold | SVG 200×130, `BuildSvg()` + `MarkupString` |
| MetricCard | Title / value / subtitle card | Razor markup, scoped CSS |

All chart components accept parameters for data, titles, units, colors, and ranges. None use JavaScript.

### 8.2 Panel Components (`Components.Panels`)

| Panel | Data Source | Key Visuals |
|---|---|---|
| ProcessMetricsPanel | MetricsSnapshot | BarChart (memory breakdown), MetricCards (threads, handles, virtual) |
| CpuMetricsPanel | IReadOnlyList\<MetricsSnapshot\> | LineChart (CPU % history), MetricCards (current %, processors, total time) |
| GcMetricsPanel | MetricsSnapshot | GaugeChart (memory load %), generation table, MetricCards (collections, pause %, finalizers) |
| AllocationRatePanel | IReadOnlyList\<MetricsSnapshot\> | LineChart (allocation rate MB/s), MetricCards (current rate, total allocated) |
| TopAllocationsPanel | AllocationSnapshot | Ranked table of top 15 types with monospace type names, bytes, counts |
| LargeObjectAllocationsPanel | AllocationSnapshot | LOH allocation table with alert styling, or "no LOH" indicator |
| DiagnosticsPanel | IDiagnosticsService (injected) | Force GC button (before/after comparison), Capture Heap Snapshot button (ClrMD), Capture GC Dump button (dotnet-gcdump file export) |
| MemoryHealthPanel | MetricsSnapshot + IReadOnlyList\<MetricsSnapshot\> | Primary indicators (allocation rate, heap size, Gen 2 collections) with trend detection using trailing-window comparison (5 samples, ~10s). Secondary indicators (memory load, GC pause, fragmentation, pending finalizers). |
| DumpAnalysisPanel | DumpAnalysisResult | MetricCards (heap size, object count, file name), ranked top 20 types table |
| DumpDiffPanel | DumpDiffResult | 4-zone layout when correlation available (see 8.4), standard diff table otherwise |
| DumpHistoryPanel | IReadOnlyList\<DumpAnalysisResult\> | Click-to-select table (BASE/CUR tags by chronological order), "Compare Selected" button, "Clear All" button |
| GcRootAnalysisPanel | RootAnalysisResult?, IReadOnlyList\<HeapTypeDiff\>? | Collapsible per-type sections: color-coded root kind badge (green=UserCode, red=StrongHandle, yellow=PinnedHandle, blue=Stack, orange=FinalizerQueue), root object type, monospace retention path, retained count. Cross-references with current diff's leak suspects for "confirmed" badge. Auto-expands when ≤ 3 types. |

### 8.3 Composite View Components (`Components.Views`)

| View | Injects | Grid Content | Parameter |
|---|---|---|---|
| MetricsDashboardView | MetricsHub | MemoryHealth (full width), ProcessMetrics + CPU + GC + AllocationRate (2×2), optional full-width slot | `RenderFragment? AdditionalContent` |
| MemoryDiagnosticsView | AllocationTrackingHub, MetricsHub | MemoryHealth (full width), Diagnostics (full width), TopAllocations (full width), LOH + GC (side-by-side) | — |
| DumpAnalysisView | DumpAnalysisHub, MetricsHub | MemoryHealth (full width), DumpHistory (full width), DumpAnalysis + DumpDiff (side-by-side), GcRootAnalysisPanel (full width) | — |

### 8.4 DumpDiffPanel — 4-Zone Correlation Narrative

When `DumpDiffResult.HasAllocationCorrelation` is true, the panel renders:

1. **Zone 1: Summary MetricCards** — Heap delta, object delta, time span, collection efficiency % (green ≥ 80%, yellow ≥ 50%, red < 50%)
2. **Zone 2: Narrative Banner** — Prose summary with color-coded left border. Reports heap growth, allocation throughput, collected bytes, and efficiency %.
3. **Zone 3: Leak Suspects** — Red alert box showing up to 5 types detected via two-track logic matching `DiagnosticsService.PredictLeakSuspectTypes()` (see §7.6): high retention (≥ 80%) or significant heap growth (≥ 1 MB or ≥ 20% of heap delta). Framework noise types are excluded via `IsFrameworkNoise()`. Per-suspect: type name, allocated bytes, retained bytes, collected bytes, retention %.
4. **Zone 4: Full Type Diff Table** — Sorted by retention ratio descending (nulls last). Includes allocation throughput and retention % columns.

### 8.5 Component Lifecycle Pattern

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

The `ObjectDisposedException` catch handles the race where a snapshot arrives after disposal but before unsubscription.

---

## 9. Consumer Integration

### 9.1 Setup

```csharp
// Program.cs
builder.Services.AddAppSysMetrics(options =>
{
    options.CollectionInterval = TimeSpan.FromSeconds(2);
    options.MaxHistorySize = 60;
});
```

```html
<!-- App.razor / _Host.cshtml -->
<link rel="stylesheet" href="_content/AppSysMetrics/AppSysMetrics.css" />
```

### 9.2 Page Wrappers

The library ships views, not pages. Consumers create thin page wrappers:

```razor
@page "/dashboard"
@rendermode InteractiveServer
<MetricsDashboardView />
```

Each view deliberately omits `@page` and `@rendermode`, giving consumers full control over routing and render mode.

### 9.3 CSS Strategy

**Tier 1: Shared base stylesheet** — `_content/AppSysMetrics/AppSysMetrics.css`
- `.panel`, `.panel-heading`, `.panel-loading` — Panel container styles
- `.metric-row`, `.metric-ok`, `.metric-warning`, `.metric-danger` — State styling
- `.gen-table`, `.type-name`, `.alloc-table-wrapper` — Table layout
- `.btn`, `.btn-warning`, `.btn-info` — Button styles

**Tier 2: Scoped component CSS** — Auto-bundled into `AppSysMetrics.styles.css` by Blazor CSS isolation.

All components use `asm-` prefixed class names to avoid collisions with consumer stylesheets. Zero Bootstrap dependency.

---

## 10. Dependencies

### 10.1 NuGet Packages

| Package | Version | Used By | Purpose |
|---|---|---|---|
| Microsoft.Diagnostics.Runtime | 3.1.512801 | ClrMdHeapAnalyzer | In-process heap analysis via ClrMD |

### 10.2 Framework References

| Reference | Provides |
|---|---|
| Microsoft.AspNetCore.App | Razor compilation, Hosting.Abstractions, Logging.Abstractions, Options, DI |

### 10.3 External Tools (Optional)

| Tool | Required By | Install Command |
|---|---|---|
| dotnet-gcdump | `CaptureGcDumpFileAsync()` only | `dotnet tool install --global dotnet-gcdump` |

Only required for the "Capture GC Dump" file export button. The primary "Capture Heap Snapshot" feature uses ClrMD in-process and requires no external tools.

### 10.4 Runtime APIs

| API | Purpose |
|---|---|
| `Process.GetCurrentProcess()` | Process-level metrics |
| `GC.GetGCMemoryInfo()` | Managed heap metrics |
| `GC.GetTotalAllocatedBytes()` | Allocation rate tracking |
| `GC.CollectionCount()` | Per-generation collection counts |
| `GC.GetTotalMemory()` | Quick heap size estimate |
| `GC.Collect()` / `GC.WaitForPendingFinalizers()` | Force GC |
| `Stopwatch.GetTimestamp()` / `GetElapsedTime()` | High-resolution timing |
| `EventListener` | Allocation event subscription |
| `DataTarget.CreateSnapshotAndAttach` | In-process heap snapshot (ClrMD) |
| `ClrHeap.EnumerateObjects` | Heap object enumeration (ClrMD) |
| `ClrHeap.EnumerateRoots` | GC root enumeration (ClrMD) |
| `ClrObject.EnumerateReferenceAddresses` | Parent map construction (ClrMD) |
| `ClrObject.EnumerateReferencesWithFields` | Field name resolution on retention paths (ClrMD) |

---

## 11. Design Rationale

### 11.1 Sealed records for metrics

Records provide value equality and immutable snapshots. `sealed` prevents inheritance overhead. The combination is ideal for data created once, published to a hub, and read by multiple consumers — no defensive copying needed.

### 11.2 MarkupString + StringBuilder for SVG

Razor's parser treats `<text>` as a directive, conflicting with SVG's `<text>` element. Chart components build SVG strings in `BuildSvg()` methods and inject via `@((MarkupString)BuildSvg())`. This also avoids Razor issues with `<` in switch expressions.

### 11.3 Pure SVG, no JavaScript

The library renders all visualizations as pure SVG with zero JS payload, no npm dependencies, and no bundling. Updates propagate instantly via SignalR without client-side re-rendering.

### 11.4 Separate hubs for different concerns

Three independent hubs (`MetricsHub`, `AllocationTrackingHub`, `DumpAnalysisHub`) serve different diagnostic questions at different cadences:
- Metrics: periodic 2-second polling (process health)
- Allocations: periodic 2-second snapshots from cumulative event data (type-level allocation patterns)
- Dump analysis: on-demand user action (heap state and leak detection)

Coupling them would force all onto the same timer and reduce API composability.

### 11.5 ClrMD over dotnet-gcdump

`dotnet-gcdump` relies on EventPipe `GCBulkType` events for type resolution. A .NET 8+ regression (dotnet/diagnostics #5116) causes UNKNOWN type names on repeated captures from the same process. ClrMD reads type metadata directly from CLR method tables and the DAC, which is immune to this regression. The trade-off is one NuGet dependency; the gain is reliable type names and no external tool requirement. The original `dotnet-gcdump collect` is retained as `CaptureGcDumpFileAsync()` for `.gcdump` file export.

### 11.6 Single library, not Core + UI split

The Razor SDK is additive — existing C# compiles identically. A single package avoids version coordination. Consumers who only need the backend can ignore the Components namespace. The `FrameworkReference` to `Microsoft.AspNetCore.App` replaces all explicit NuGet packages, resulting in a cleaner `.csproj`.

### 11.7 No @page or @rendermode in library views

Hardcoding routes in a library claims URL paths from the consumer. Hardcoding render mode prevents consumer choice. By shipping views as plain components, the library stays composable — consumers wrap in their own pages with their own routing, render mode, and layout decisions.

### 11.8 Zero Bootstrap dependency

Library components use no Bootstrap CSS classes. All styling is self-contained via `AppSysMetrics.css` and scoped `.razor.css` files, making the library portable to any CSS framework or custom design system.

### 11.9 Allocation enrichment for retention analysis

Heap snapshots alone show what's on the heap but not what was allocated. Attaching an `AllocationSnapshot` at capture time lets the diff service compute per-type retention ratios: a type with 500 KB heap growth could be healthy (if 10 MB allocated, 9.5 MB collected) or a leak (if only 500 KB allocated). The `AllocationAtCapture` field is nullable for edge cases.

### 11.10 4-zone narrative UI for diff analysis

Raw diff tables show numbers but don't answer "is the heap healthy?" The 4-zone layout provides progressive disclosure: executive summary (efficiency %), narrative prose (colored banner), actionable alerts (leak suspects), then full detail (retention-sorted table). The two key numbers — `collected / allocated` efficiency and per-type retention ratio — immediately distinguish healthy churn from a leak.

### 11.11 EventListener over ETW

The in-process `EventListener` base class requires no NuGet dependency, works cross-platform, needs no elevated permissions, and provides low-overhead allocation event subscription via sampled ticks (~100 KB granularity).

### 11.12 User-code parent preference in parent map

The parent map uses `TryAdd` (first-parent-wins) for performance, but framework transients can claim parenthood before the actual owner. When a user-code type tries to overwrite an existing framework parent, the overwrite is allowed. This ensures paths name the developer's class (e.g., `MemoryLeakService`) rather than framework internals (e.g., `List<T>+Enumerator`).

### 11.13 Framework intermediary check

A user-code type (like a Blazor component) may own framework objects via framework plumbing (e.g., `_renderHandle:EndpointHtmlRenderer`). The depth threshold alone can't distinguish deliberate from incidental ownership. The intermediary check examines the first hop from the user-code type: if it's `Microsoft.*`/`Internal.*`/`Interop.*`, the path is framework plumbing and downgraded to `HasUserCode = false`. If it's `System.*` (container like `List<T>`), it's direct ownership.

### 11.14 Two-track leak prediction

A single retention ratio threshold (≥ 80%) misses diluted leaks where framework throughput inflates the denominator. For example, `Byte[]` shared by Kestrel's MemoryPoolBlock (high allocation/collection churn) and a leaking service (retained) drops to ~38% retention despite real heap growth. Track 2 catches these by looking at absolute growth (≥ 1 MB or ≥ 20% of heap delta) independent of the ratio.

### 11.15 Dot-free type filtering

Some framework internal types (e.g., `ClrDacType` from `Microsoft.Diagnostics.Runtime`) appear on the heap without namespace qualifiers. The prediction filter uses a well-known primitive allowlist for dot-free types rather than allowing all through, preventing framework noise from consuming root analysis time.

### 11.16 Self-assembly exclusion

AppSysMetrics observes the same process it runs in. Without explicit exclusion, its own types (and ClrMD's) would appear as user code. `BuildUserAssemblyPrefixes()` excludes the library's own assembly name, ensuring the diagnostics tool never reports its own infrastructure as a leak suspect.
