# .NET Memory and Process Metrics Glossary

**A primer for developers using AppSysMetrics**

This glossary explains every metric captured by the AppSysMetrics library. It is organized by conceptual area so you can read it front-to-back as a .NET memory primer, or jump to a specific term when reading the dashboard.

Each entry explains what the metric measures, why it matters, and what to watch for.

> **Notation:** Properties are shown as `PropertyName` with their parent model in parentheses. For example, `WorkingSet64` (ProcessMetrics) means the `WorkingSet64` property on the `ProcessMetrics` record.

---

## Table of Contents

1. [Process Memory](#1-process-memory)
2. [CPU Utilization](#2-cpu-utilization)
3. [The GC Heap — Overview](#3-the-gc-heap--overview)
4. [GC Heap Size and Fragmentation](#4-gc-heap-size-and-fragmentation)
5. [GC Generations](#5-gc-generations)
6. [GC Collection Counts and Pauses](#6-gc-collection-counts-and-pauses)
7. [Allocation Rate and Tracking](#7-allocation-rate-and-tracking)
8. [Allocation Snapshots — Type-Level Detail](#8-allocation-snapshots--type-level-detail)
9. [The Finalization Queue](#9-the-finalization-queue)
10. [Heap Snapshots — What's on the Heap Right Now](#10-heap-snapshots--whats-on-the-heap-right-now)
11. [Heap Diff Analysis — Comparing Two Snapshots](#11-heap-diff-analysis--comparing-two-snapshots)
12. [Allocation Correlation and Retention](#12-allocation-correlation-and-retention)
13. [Diagnostics Actions](#13-diagnostics-actions)
14. [Timing and Snapshot Identity](#14-timing-and-snapshot-identity)

---

## 1. Process Memory

These metrics come from the operating system's view of the .NET process. They represent physical and virtual memory consumption as the OS sees it — which can differ significantly from what the .NET garbage collector reports.

> **Container property:** `MetricsSnapshot.Process` (type `ProcessMetrics`) holds all the properties in this section. The top-level `MetricsSnapshot` is produced by `MetricsCollector.Collect()` every 2 seconds and also carries `Cpu` (Section 2) and `Gc` (Sections 4–9).

### WorkingSet64
*ProcessMetrics · long · bytes*

**What it is:** The amount of physical RAM currently assigned to this process. This is what Windows Task Manager shows in the "Memory" column.

**Why it matters:** Working set is the most visible memory metric to operations teams and users. If your app shows 2 GB in Task Manager, this is the number they see.

**What to watch for:** Working set can be larger than the managed heap because it includes native memory (images, unmanaged buffers, memory-mapped files) and uncommitted GC segments. It can also temporarily include pages that the OS hasn't reclaimed yet even though the GC has freed the managed objects on them.

### PrivateMemorySize64
*ProcessMetrics · long · bytes*

**What it is:** The total amount of memory that this process has committed (reserved and backed by the page file or physical RAM) that cannot be shared with other processes. This includes the managed heap, native allocations, thread stacks, loaded assemblies, and JIT-compiled code.

**Why it matters:** Private memory is the most accurate measure of how much memory your process is *exclusively* consuming. Unlike working set, it doesn't fluctuate with OS paging decisions.

**What to watch for:** Steady growth in private memory without corresponding growth in the managed heap suggests native memory leaks (P/Invoke buffers, unmanaged resources not being disposed, etc.).

### VirtualMemorySize64
*ProcessMetrics · long · bytes*

**What it is:** The total virtual address space reserved by the process. This includes committed memory (backed by RAM/page file) and reserved-but-not-committed ranges.

**Why it matters:** On 32-bit processes, the virtual address space is limited to ~2 GB, and fragmentation of the virtual address space can cause `OutOfMemoryException` even when plenty of physical RAM is available. On 64-bit processes, virtual memory is essentially unlimited (128 TB), so this metric is mainly informational.

**What to watch for:** On 64-bit systems, this number is typically very large and not a concern. On 32-bit systems (rare in modern .NET), watch for it approaching the 2 GB ceiling.

### PagedMemorySize64
*ProcessMetrics · long · bytes*

**What it is:** The amount of virtual memory eligible to be written to the paging file on disk. Essentially, this is the process's contribution to the system page file commitment.

**Why it matters:** High paged memory relative to working set means the OS may be paging parts of your process to disk, which causes severe performance degradation when those pages are accessed.

**What to watch for:** If paged memory is significantly larger than working set, the system is under memory pressure and your process's pages are being swapped out.

### ThreadCount
*ProcessMetrics · int*

**What it is:** The total number of OS threads in the process, including .NET thread pool threads, finalizer thread, GC threads, and any threads created by native libraries.

**Why it matters:** Each thread consumes ~1 MB of stack space (on 64-bit). Excessive thread creation can indicate thread pool starvation (too many blocking calls), runaway `Task.Run` usage, or misbehaving libraries.

**What to watch for:** A healthy ASP.NET Core app typically has 20–50 threads. If you see hundreds of threads, investigate thread pool exhaustion — usually caused by synchronous blocking on async code (`Task.Result`, `.Wait()`).

### HandleCount
*ProcessMetrics · int*

**What it is:** The number of OS handles (file handles, socket handles, registry keys, event handles, etc.) held by the process.

**Why it matters:** Handles are finite OS resources. A steadily growing handle count usually indicates handles not being properly closed — often caused by not disposing `HttpClient`, `FileStream`, database connections, or similar `IDisposable` objects.

**What to watch for:** Steady upward trend without plateau. A typical web app might hold 200–500 handles. If you see thousands, look for undisposed `IDisposable` objects.

---

## 2. CPU Utilization

> **Container property:** `MetricsSnapshot.Cpu` (type `CpuMetrics`) holds all the properties in this section.

### CpuPercentage
*CpuMetrics · double · 0–100*

**What it is:** The percentage of available CPU time consumed by this process since the last sample, normalized by the number of logical processors. A value of 50% on a 4-core machine means the process is using the equivalent of 2 full cores.

**How it's computed:** `(deltaCpuTime / deltaWallTime) / processorCount * 100`, using high-resolution `Stopwatch` timing for accuracy.

**Why it matters:** Sustained high CPU usage can indicate tight loops, excessive GC work, or computationally expensive operations.

**What to watch for:** Brief spikes during request bursts are normal. Sustained 90%+ on a server process usually indicates a problem — either CPU-bound work that should be offloaded, or excessive Gen 2 GC collections consuming CPU.

### TotalProcessorTime
*CpuMetrics · TimeSpan*

**What it is:** The cumulative CPU time consumed by this process across all cores since it started. On a 4-core machine, 10 seconds of wall time with all cores busy would yield ~40 seconds of processor time.

**Why it matters:** This is the raw number behind `CpuPercentage`. Comparing total processor time against wall-clock uptime gives you the average CPU utilization over the process lifetime.

### ProcessorCount
*CpuMetrics · int*

**What it is:** The number of logical processors available to the process, from `Environment.ProcessorCount`. This accounts for hyper-threading: a 4-core CPU with hyper-threading reports 8.

**Why it matters:** Used to normalize CPU percentage. Without normalization, a process pinning one core on an 8-core machine would show 12.5%, which is misleading for diagnostics.

---

## 3. The GC Heap — Overview

.NET's garbage collector (GC) manages a region of memory called the **managed heap**. When you write `new MyClass()`, the object is allocated on this heap. The GC periodically scans the heap, identifies objects no longer reachable by your code, and reclaims their memory.

Understanding the GC heap requires knowing several concepts that the following sections break down:
- **Heap size** — How big is the managed heap?
- **Fragmentation** — How much space is wasted inside the heap?
- **Generations** — How does the GC organize objects by age?
- **Collection counts** — How often is the GC running?
- **Pause time** — How much time does the GC steal from your application?
- **Allocation rate** — How fast is your code creating objects?

The crucial insight: **process memory (Section 1) and heap size (this section) can diverge significantly.** A process may hold large native buffers invisible to the GC, or the GC may report a small heap while the OS working set remains elevated because the OS hasn't reclaimed decommitted pages yet.

---

## 4. GC Heap Size and Fragmentation

> **Container property:** `MetricsSnapshot.Gc` (type `GcMetrics`) holds all properties in Sections 4–6 and 9, plus `GenerationInfo` (Section 5). `GcMetrics.GenerationInfo` (type `IReadOnlyList<GcGenerationInfo>`) contains per-generation detail for generations 0–4.

### HeapSizeBytes
*GcMetrics · long · bytes*

**What it is:** The total size of the managed GC heap across all generations, as reported by `GC.GetGCMemoryInfo().HeapSizeBytes`. This includes both live objects and free space between objects (fragmentation).

**Why it matters:** This is the most important single number for managed memory health. If it grows steadily over time without stabilizing, you likely have a managed memory leak.

**What to watch for:** A healthy app's heap size follows a sawtooth pattern — it grows as objects are allocated, then drops when the GC collects. If the "valleys" (post-GC sizes) keep getting higher over time, objects are being retained that shouldn't be.

### FragmentedBytes
*GcMetrics · long · bytes*

**What it is:** The total number of bytes in the heap that are "free space" — gaps between live objects that the GC can't easily consolidate. This comes from `GC.GetGCMemoryInfo().FragmentedBytes`.

**Why it matters:** Fragmentation means the heap is larger than the live data requires. High fragmentation wastes memory and can slow GC collection because the collector has to walk over free gaps.

**What to watch for:** Some fragmentation is normal, especially in the Large Object Heap (LOH) because the LOH is not compacted by default. If fragmented bytes exceed 20–30% of heap size, you may have a fragmentation problem — often caused by allocating and freeing many large objects (>85 KB).

### TotalAvailableMemoryBytes
*GcMetrics · long · bytes*

**What it is:** The total physical memory available to the GC, as reported by the runtime. On a dedicated machine, this is roughly the total RAM. In a container, it's the container's memory limit.

**Why it matters:** The GC adjusts its aggressiveness based on how much memory is available. When available memory is scarce, the GC runs more frequently and more aggressively to keep the heap small.

### MemoryLoadPercent
*GcMetrics · double · 0–100*

**What it is:** `HeapSizeBytes / TotalAvailableMemoryBytes * 100`. Represents what fraction of available memory the managed heap is consuming.

**Why it matters:** High memory load (>80%) causes the GC to become very aggressive, running frequent Gen 2 collections that are expensive. This is the GC's "memory pressure" indicator.

**What to watch for:** Values consistently above 85% indicate your process is under memory pressure. The GC will be working overtime, consuming CPU and pausing your application.

### TotalMemory
*GcMetrics · long · bytes*

**What it is:** The value from `GC.GetTotalMemory(forceFullCollection: false)`. This is a quick estimate of the total managed memory, which may include memory that has been freed but not yet reclaimed.

**Why it matters:** This is a lighter-weight check than `GC.GetGCMemoryInfo()` and is useful for quick comparisons. However, it's less precise than `HeapSizeBytes` because it doesn't force a collection.

### SizeBeforeBytes / SizeAfterBytes
*GcGenerationInfo · long · bytes*

**What they are:** The size of a specific GC generation before and after the most recent collection that included that generation. These come from `GcMemoryInfo.GenerationInfo[n]`.

**Why they matter:** The difference between before and after shows how effective the collection was for that generation. If `SizeAfterBytes` is nearly as large as `SizeBeforeBytes`, most objects in that generation survived — they're either still referenced or pinned.

**What to watch for:** Gen 2 `SizeAfterBytes` close to `SizeBeforeBytes` means Gen 2 collections aren't freeing much memory. Combined with rising `HeapSizeBytes`, this is a strong signal of a memory leak.

### FragmentationBeforeBytes / FragmentationAfterBytes
*GcGenerationInfo · long · bytes*

**What they are:** The free space within a specific generation before and after a collection. High post-collection fragmentation in the LOH (Generation 3) is especially concerning.

**Why they matter:** Collections should reduce fragmentation by compacting. If fragmentation increases or stays high after collection, the GC is unable to consolidate memory effectively.

---

## 5. GC Generations

The .NET GC organizes the managed heap into **generations** based on object age. The key insight: most objects die young. By collecting the youngest generation frequently and the oldest generation rarely, the GC minimizes the work it has to do.

### Generation
*GcGenerationInfo · int*

**What the values mean:**

| Value | Name | What lives here | Collection frequency |
|---|---|---|---|
| 0 | Gen 0 | Newly allocated objects | Very frequent (microseconds) |
| 1 | Gen 1 | Objects that survived one Gen 0 collection | Frequent |
| 2 | Gen 2 | Long-lived objects that survived Gen 1 collection | Infrequent (expensive) |
| 3 | Large Object Heap (LOH) | Objects >= 85,000 bytes, regardless of age | Only during Gen 2 collections |
| 4 | Pinned Object Heap (POH) | Objects explicitly allocated as pinned (.NET 5+) | Only during Gen 2 collections |

**The generational hypothesis:** Most objects (temporary strings, LINQ intermediaries, short-lived DTOs) die in Gen 0 — they're created, used, and become unreachable within milliseconds. Only objects that survive Gen 0 collection get promoted to Gen 1, and only those that survive Gen 1 get promoted to Gen 2.

**Why Gen 2 collections are expensive:** Gen 2 includes the entire heap (Gen 0, Gen 1, Gen 2, LOH, POH). A Gen 2 collection must examine every live object in the process, which can take tens or hundreds of milliseconds. This is why frequent Gen 2 collections are a red flag.

### Gen0Collections / Gen1Collections / Gen2Collections
*GcMetrics · int*

**What they are:** The cumulative number of collections for each generation since the process started, from `GC.CollectionCount(gen)`.

**Why they matter:** The ratio between generations tells you about your allocation patterns:
- **Healthy ratio:** Gen 0 >> Gen 1 >> Gen 2. Example: 5000 / 500 / 5 (roughly 10:1 at each level)
- **Unhealthy:** Gen 2 collections are frequent relative to Gen 0. Example: 500 / 100 / 80 — this means the GC can't keep objects from reaching Gen 2.

**What to watch for:** Rapid growth in Gen 2 collection count indicates one or more of:
1. **Memory leak** — Objects keep accumulating in Gen 2 because they're still referenced
2. **Mid-life crisis** — Objects that should be temporary are living just long enough to get promoted to Gen 2 before dying (common with pooled/cached objects that have poor lifetime management)
3. **LOH allocations** — Each large object allocation can trigger a Gen 2 collection because the LOH is only collected during Gen 2

---

## 6. GC Collection Counts and Pauses

### PauseTimePercentage
*GcMetrics · double · percentage*

**What it is:** The percentage of wall-clock time that the process has been paused by the GC. During a GC pause, your application threads are suspended — no requests are processed, no UI updates happen.

**Why it matters:** This is the GC's direct impact on your application's responsiveness. A 5% pause time means that for every second of wall time, your application was frozen for 50 milliseconds.

**What to watch for:**
- **< 1%** — Excellent. Typical for well-tuned server apps.
- **1–5%** — Acceptable for most workloads but worth monitoring.
- **> 5%** — Problematic. Users or downstream services will notice latency spikes.
- **> 10%** — Critical. The GC is dominating your application's execution time.

High pause time correlates with frequent Gen 2 collections, large heap size, and high allocation rate. The most effective fix is usually to reduce allocation rate (object pooling, `Span<T>`, `stackalloc`) rather than tuning GC settings.

---

## 7. Allocation Rate and Tracking

### TotalAllocatedBytes
*GcMetrics · long · bytes*

**What it is:** The total number of bytes allocated on the managed heap since the process started, from `GC.GetTotalAllocatedBytes()`. This is a cumulative counter — it only goes up.

**Why it matters:** This is the denominator for understanding GC pressure. An application that allocates 1 GB/second is putting 1000x more pressure on the GC than one allocating 1 MB/second, regardless of how quickly those objects die.

### AllocationRateBytesPerSecond
*GcMetrics · double · bytes/second*

**What it is:** The rate of managed heap allocation, computed from the delta of `TotalAllocatedBytes` between two collection intervals.

**Why it matters:** Allocation rate is arguably the most actionable metric for performance optimization. Every allocation:
1. Costs time to initialize memory
2. Costs GC time to later scan and potentially collect the object
3. Costs CPU cache coherence (new allocations displace cached data)

**What to watch for:** There's no universal "good" or "bad" allocation rate — it depends on your workload. But if allocation rate is high AND you're seeing high pause times or frequent Gen 2 collections, reducing allocation rate is the first optimization target.

**Common allocation reduction techniques:**
- `ArrayPool<T>.Shared` for temporary buffers
- `Span<T>` and `stackalloc` for short-lived byte manipulation
- `string.Create()` instead of string concatenation
- Object pooling for frequently created/destroyed objects
- `ValueTask<T>` for frequently synchronous async methods

---

## 8. Allocation Snapshots — Type-Level Detail

While `AllocationRateBytesPerSecond` tells you the overall rate, allocation snapshots tell you **which types** are being allocated. This comes from the .NET runtime's `AllocationTick` events, which fire approximately every 100 KB of allocations.

### TypeName
*AllocationTypeInfo · string*

**What it is:** The fully qualified .NET type name of the objects being allocated, e.g., `System.String`, `System.Byte[]`, `MyApp.Models.OrderDto`.

**Why it matters:** Knowing *which* types dominate allocations tells you exactly where to focus optimization. If 80% of allocation bytes are `System.Byte[]`, you should look for buffer pooling opportunities. If it's your own DTO type, you might benefit from struct records or object pooling.

### TotalBytes
*AllocationTypeInfo · long · bytes*

**What it is:** The cumulative bytes allocated for this type since tracking began.

### AllocationCount
*AllocationTypeInfo · int*

**What it is:** The number of `AllocationTick` events observed for this type. Since allocation ticks fire approximately every 100 KB, this is a sampled count — not the actual number of `new` calls.

**Why it matters:** The ratio of `TotalBytes / AllocationCount` gives you the average allocation size per tick. Many small allocations (high count, low bytes-per-tick) suggest different optimization strategies than few large allocations (low count, high bytes-per-tick).

### IsLargeObject
*AllocationTypeInfo · bool*

**What it is:** True if the allocation was on the Large Object Heap (LOH), which in .NET means the object was >= 85,000 bytes (approximately 85 KB).

**Why it matters:** LOH allocations are more expensive than regular allocations because:
1. The LOH is not compacted by default (leading to fragmentation)
2. LOH allocations can trigger Gen 2 collections
3. Large objects are immediately Gen 2 (they skip Gen 0 and Gen 1)

**What to watch for:** Frequent LOH allocations, especially of types like `System.Byte[]` or `System.Char[]`, are prime candidates for `ArrayPool<T>` usage.

### TopAllocatingTypes
*AllocationSnapshot · IReadOnlyList\<AllocationTypeInfo\>*

**What it is:** The top N types ranked by total bytes allocated (descending). These are the types putting the most pressure on the GC.

### RecentLargeObjectAllocations
*AllocationSnapshot · IReadOnlyList\<AllocationTypeInfo\>*

**What it is:** A bounded queue of the most recent LOH allocations. Useful for identifying bursty large object allocation patterns.

### TotalTrackedBytes / TotalTrackedCount
*AllocationSnapshot · long / int*

**What they are:** The sum of all tracked allocation bytes and counts across all types. This is the total allocation volume being observed by the event listener.

### AppTrackedBytes / AppTrackedCount
*AllocationSnapshot · long / int*

**What they are:** Allocation bytes and counts for types that are NOT in the `AppSysMetrics.*` namespace. This isolates allocations from the application's own code and its dependencies, excluding the monitoring library's own overhead.

**Why it matters:** When measuring allocation impact, you want to know what the *application* is doing, not what the monitoring tool is adding. This split lets the dashboard report "your app allocated 500 MB" separately from "the monitoring library allocated 2 MB".

### LibraryTrackedBytes / LibraryTrackedCount
*AllocationSnapshot · long / int*

**What they are:** Allocation bytes and counts for types in the `AppSysMetrics.*` namespace — the monitoring library's own overhead.

---

## 9. The Finalization Queue

### FinalizationPendingCount
*GcMetrics · long*

**What it is:** The number of objects waiting in the finalization queue. These are objects that are no longer reachable but have finalizers (`~ClassName()` or `Finalize()`) that must run before the GC can reclaim their memory.

**Why it matters:** Finalization is a two-collection process:
1. First GC identifies the object as unreachable, but instead of reclaiming it, moves it to the finalization queue. The object is **resurrected** — it's alive again temporarily.
2. The finalizer thread runs the object's finalizer.
3. A second GC (usually the next one) finally reclaims the object.

This means objects with finalizers live **at least one collection longer** than necessary, consuming memory and GC time. A large finalization queue means many such objects are waiting.

**What to watch for:**
- **< 100** — Normal. Background finalizer thread is keeping up.
- **100–1000** — Elevated. Finalizable objects are being created faster than the finalizer thread can process them. Look for types that implement finalizers without proper `Dispose()` patterns.
- **> 1000** — Critical. The finalizer thread is overwhelmed. Common causes: not calling `Dispose()` on `SafeHandle`-based resources (database connections, file handles, graphics objects), or mass-creating objects with finalizers.

The fix is almost always to implement the `IDisposable` pattern correctly and ensure callers use `using` statements. Objects that call `GC.SuppressFinalize(this)` in `Dispose()` skip the finalization queue entirely.

---

## 10. Heap Snapshots — What's on the Heap Right Now

While allocation tracking (Section 8) tells you what's being created, a **heap snapshot** tells you what's currently alive on the managed heap. AppSysMetrics captures heap snapshots using ClrMD (`Microsoft.Diagnostics.Runtime`), which reads the GC heap directly from the CLR's internal data structures.

### TypeName
*HeapTypeInfo · string*

**What it is:** The fully qualified type name of objects found on the heap during enumeration. Unlike allocation tracking (which sees what's being created), this shows what's currently alive.

**Why it matters:** The types dominating the heap are the types consuming your application's memory. Comparing heap composition over time reveals which types are accumulating (potential leaks) vs. which are transient (healthy churn).

### InstanceCount
*HeapTypeInfo · long*

**What it is:** The number of live instances of this type found during heap enumeration.

**Why it matters:** A type with 1 million instances consuming 50 MB is very different from a type with 10 instances consuming 50 MB. Instance count helps distinguish between many small objects (potential for structural changes) and few large objects (potential for pooling or streaming).

### TotalSizeBytes
*HeapTypeInfo · long · bytes*

**What it is:** The total bytes consumed by all instances of this type on the heap, including object headers and padding.

### TotalHeapBytes
*DumpAnalysisResult · long · bytes*

**What it is:** The total size of the GC heap as measured by walking every object. This should closely match `GcMetrics.HeapSizeBytes`.

### TotalObjectCount
*DumpAnalysisResult · long*

**What it is:** The total number of live objects found on the heap.

**Why it matters:** Absolute object count gives context for GC performance. The GC must visit every live object during a collection. 10 million live objects means the GC has to do significant work even if total heap size is modest.

### UnresolvedTypeCount
*DumpAnalysisResult · int*

**What it is:** The number of types whose names could not be resolved during heap enumeration. With ClrMD, this is always 0 because ClrMD reads type metadata directly from CLR method tables. With the legacy `dotnet-gcdump` path, a .NET 8+ regression could cause UNKNOWN type names.

### TopTypes
*DumpAnalysisResult · IReadOnlyList\<HeapTypeInfo\>*

**What it is:** The top N types by total size on the heap, sorted descending. Each entry is a `HeapTypeInfo` with `TypeName`, `InstanceCount`, and `TotalSizeBytes`.

**Why it matters:** This is the curated view of what's dominating your heap. Rather than examining thousands of types, you get the biggest consumers at a glance. The number of types included is controlled by `DumpAnalyzerOptions.TopTypesCount`.

**What to watch for:** If the same type consistently dominates the top of this list and its instance count grows between snapshots, that type is a prime leak candidate. Cross-reference with `RetentionRatio` (Section 12) for confirmation.

### AllocationAtCapture
*DumpAnalysisResult · AllocationSnapshot? · nullable*

**What it is:** A snapshot of allocation tracking data captured at the same moment as the heap snapshot. This enables the correlation analysis described in Section 12.

**Why it matters:** Enriching a heap snapshot with concurrent allocation data is what powers the retention ratio analysis. Without this, you can only see what's on the heap — you can't compute how much was allocated and how much was collected.

---

## 11. Heap Diff Analysis — Comparing Two Snapshots

A single heap snapshot shows you the current state. Comparing two snapshots taken at different times reveals **what changed** — which types grew, which shrank, and by how much. This is the primary mechanism for detecting memory leaks.

### Baseline / Current
*DumpDiffResult · DumpAnalysisResult*

**What they are:** The two snapshots being compared. Baseline is the older snapshot; Current is the newer one.

### TimeBetweenDumps
*DumpDiffResult · TimeSpan*

**What it is:** The wall-clock time between the two captures. This provides context for interpreting growth — 10 MB growth over 5 seconds has very different implications than 10 MB growth over 5 hours.

### BaselineCount / CurrentCount
*HeapTypeDiff · long*

**What they are:** The absolute instance count of a specific type in the baseline and current snapshots, respectively. These come directly from the `HeapTypeInfo.InstanceCount` in each `DumpAnalysisResult`.

**Why they matter:** Absolute counts give context that delta alone cannot. A `DeltaCount` of +1,000 means something very different if `BaselineCount` was 100 (10x growth) versus 1,000,000 (0.1% growth). The absolute values anchor the delta in reality.

### BaselineSizeBytes / CurrentSizeBytes
*HeapTypeDiff · long · bytes*

**What they are:** The total bytes consumed by all instances of a specific type in the baseline and current snapshots, respectively. These come from `HeapTypeInfo.TotalSizeBytes` in each `DumpAnalysisResult`.

**Why they matter:** Like absolute counts, absolute sizes contextualize the delta. They are also the denominator for `GrowthPercent`: `(DeltaSizeBytes / BaselineSizeBytes) * 100`. A type consuming 200 MB in both snapshots is worth investigating even if its delta is 0 — it's a stable but large consumer of heap space.

### DeltaSizeBytes / DeltaCount
*HeapTypeDiff · long*

**What they are:** `CurrentSizeBytes - BaselineSizeBytes` and `CurrentCount - BaselineCount`, respectively. Positive values mean the type grew; negative values mean it shrank.

**Why they matter:** Types with large positive `DeltaSizeBytes` are your leak suspects. Types with large negative values were freed (healthy behavior). These are the primary sort keys in the diff table when allocation correlation is not available.

### GrowthPercent
*HeapTypeDiff · double*

**What it is:** `(DeltaSizeBytes / BaselineSizeBytes) * 100`. A type that went from 1 MB to 3 MB shows 200% growth.

**Why it matters:** Percentage contextualizes the delta. A 1 MB delta on a 2 MB baseline (50% growth) is far more concerning than a 1 MB delta on a 200 MB baseline (0.5% growth).

### TotalHeapDelta / TotalObjectDelta
*DumpDiffResult · long*

**What they are:** The overall change in heap size and object count between snapshots.

**What to watch for:** If `TotalHeapDelta` is positive and growing with each successive diff, the heap is trending upward — memory is accumulating faster than it's being collected.

### TypeDiffs
*DumpDiffResult · IReadOnlyList\<HeapTypeDiff\>*

**What it is:** The per-type breakdown of changes between snapshots. Sorted by retention ratio descending when allocation correlation is available, otherwise by `DeltaSizeBytes` descending.

---

## 12. Allocation Correlation and Retention

This is the most advanced diagnostic concept in AppSysMetrics. It answers the question: "Of the memory that was allocated between snapshots, how much was actually collected?"

Without correlation, you can only say "Type X grew by 500 KB." With correlation, you can say "Type X had 10 MB allocated, 9.5 MB collected, and 500 KB retained — that's healthy churn" or "Type X had 500 KB allocated and 500 KB retained — that's a 100% leak."

### HasAllocationCorrelation
*DumpDiffResult · bool*

**What it is:** True when both the baseline and current snapshots carry `AllocationAtCapture` data. When true, the diff includes per-type allocation throughput and retention ratio analysis.

### BaselineAllocatedBytes / CurrentAllocatedBytes
*HeapTypeDiff · long? · nullable*

**What they are:** The cumulative allocation bytes for this type at the time of each snapshot. These come from the `AllocationSnapshot` attached to each `DumpAnalysisResult`.

### AllocatedBetweenBytes
*HeapTypeDiff · long? · nullable*

**What it is:** `CurrentAllocatedBytes - BaselineAllocatedBytes` — the total bytes allocated for this type between the two snapshots. This is the **throughput** for this type.

**Why it matters:** Throughput is the denominator for the retention ratio. Without it, you can't tell whether heap growth represents a leak or healthy high-throughput allocation with mostly-successful collection.

### RetentionRatio
*HeapTypeDiff · double? · nullable*

**What it is:** `heapDelta / allocationThroughput`, capped at 1.0. This is the fraction of allocated bytes that remained on the heap.

| Value | Meaning | Interpretation |
|---|---|---|
| 1.0 | 100% retention | Everything allocated is still on the heap. **Leak suspect.** |
| 0.5 | 50% retention | Half the allocations were collected. Concerning. |
| 0.0 | 0% retention | Everything allocated was collected. **Healthy churn.** |
| null | No data | No allocation tracking data for this type, or zero throughput. |

**Why it matters:** Retention ratio is the single best diagnostic signal for distinguishing leaks from healthy allocation patterns. A type that allocated 100 MB and retained 100 MB is almost certainly leaking. A type that allocated 100 MB and retained 500 KB has excellent collection behavior.

**What to watch for:** Types with retention ratio >= 0.8 are flagged as leak suspects in the UI. Common causes:
- Static collections that grow without bounds (`static List<T>`, `static Dictionary<K,V>`)
- Event handlers not being unsubscribed (the handler holds a reference to the subscriber)
- Caches without expiration or size limits
- Closures capturing long-lived references in async workflows

### TotalAllocatedBetween
*DumpDiffResult · long? · nullable*

**What it is:** Total bytes allocated by the application (excluding `AppSysMetrics.*` types) between snapshots. This is the overall allocation throughput.

### TotalCollectedBetween
*DumpDiffResult · long? · nullable*

**What it is:** `TotalAllocatedBetween - TotalHeapDelta`, floored at 0. This represents the total bytes that were allocated and then successfully collected between snapshots.

**Why it matters:** This number, combined with `TotalAllocatedBetween`, gives you the **collection efficiency** of the GC: `TotalCollectedBetween / TotalAllocatedBetween * 100%`.

| Efficiency | Interpretation |
|---|---|
| >= 90% | Excellent. The GC is collecting almost everything that's allocated. |
| 80–90% | Good. Some retention, but the heap is mostly stable. |
| 50–80% | Concerning. Significant retention — investigate top retaining types. |
| < 50% | Critical. More memory is being retained than collected. Likely leak. |

---

## 13. Diagnostics Actions

These are on-demand operations triggered by the user from the browser UI, not automated metrics collection.

### Force GC

**What it does:** Calls `GC.Collect(2, GCCollectionMode.Forced, blocking: true)` twice with `GC.WaitForPendingFinalizers()` between. This forces a full blocking Gen 2 collection and processes the finalization queue.

**Why two collections?** The first collection identifies unreachable finalizable objects and moves them to the finalization queue. `WaitForPendingFinalizers()` processes the queue. The second collection reclaims those now-truly-dead objects. Without the second collection, you'd see inflated post-GC heap sizes because finalized objects haven't been reclaimed yet.

### Before / After
*ForceGcResult · GcMetrics*

**What they are:** Complete GC metrics snapshots taken immediately before and after the forced collection. The difference shows exactly how much memory the GC reclaimed.

**What to watch for:** If `After.HeapSizeBytes` is barely smaller than `Before.HeapSizeBytes` after a forced full GC, the objects on the heap are genuinely alive — they're referenced from GC roots (static fields, thread stacks, GC handles). This confirms the heap contents are not garbage; they're a true reflection of your application's live object graph.

### Duration
*ForceGcResult · TimeSpan*

**What it is:** Wall-clock time for the entire Force GC operation (both collections + finalizer processing).

**Why it matters:** This tells you how long a worst-case Gen 2 blocking collection takes for your current heap. If it's hundreds of milliseconds, that's the maximum GC pause your application could experience.

### PerformedAt
*ForceGcResult · DateTimeOffset*

**What it is:** The wall-clock time (`DateTimeOffset.UtcNow`) when the Force GC operation completed. Used for display in the UI and for temporal correlation with other metrics.

### CaptureGcDumpAsync — Heap Snapshot (ClrMD)

**What it does:** Takes an in-process snapshot of the managed heap using `DataTarget.CreateSnapshotAndAttach()` from ClrMD. This creates a copy of the process's memory (via `PssCreateSnapshot` on Windows) and enumerates all objects on the heap.

**When to use:** To see what's currently on the heap, compare snapshots over time, and detect memory leaks via the diff + correlation pipeline.

### CaptureGcDumpFileAsync — GC Dump File (dotnet-gcdump)

**What it does:** Shells out to `dotnet-gcdump collect -p {pid}` to create a `.gcdump` file on disk. This file can be opened in Visual Studio's Managed Memory Analyzer for deep inspection.

**When to use:** When you need a `.gcdump` file for offline analysis in Visual Studio or other tools.

### GcDumpResult — Return Type of Capture Operations

Both `CaptureGcDumpAsync()` and `CaptureGcDumpFileAsync()` return a `GcDumpResult` record that reports whether the capture succeeded and provides metadata about the result.

### Success
*GcDumpResult · bool*

**What it is:** `true` if the capture operation completed without error; `false` if it failed (e.g., ClrMD could not attach, `dotnet-gcdump` was not installed, or the process was in an invalid state for capture).

**Why it matters:** The UI uses this to show success/failure feedback. When `false`, the `ErrorMessage` property explains what went wrong.

### FilePath
*GcDumpResult · string? · nullable*

**What it is:** The path to the `.gcdump` file on disk for file-based captures (`CaptureGcDumpFileAsync`). `null` for in-process ClrMD snapshots (`CaptureGcDumpAsync`), since no file is created.

### ErrorMessage
*GcDumpResult · string? · nullable*

**What it is:** A human-readable description of the failure when `Success` is `false`. `null` when the operation succeeded.

**Why it matters:** Provides actionable diagnostic information. Common errors include: `dotnet-gcdump` not installed in PATH, insufficient permissions, or a timeout during the ClrMD snapshot process.

### FileSizeBytes
*GcDumpResult · long · bytes*

**What it is:** The size of the `.gcdump` file on disk. Always `0` for ClrMD in-process captures since no file is written.

### CapturedAt
*GcDumpResult · DateTimeOffset*

**What it is:** The wall-clock time when the capture operation was performed. Used for display in the UI and for correlating captures with the metrics timeline.

---

## 14. Timing and Snapshot Identity

### TimestampTicks
*MetricsSnapshot · long*

**What it is:** A high-resolution timestamp from `Stopwatch.GetTimestamp()`, used for computing precise time deltas between snapshots. This uses the CPU's performance counter (QueryPerformanceCounter on Windows), which has nanosecond-level resolution.

**Why it matters:** `DateTime.UtcNow` has ~15ms resolution on Windows. For accurate CPU percentage calculation and allocation rate computation, nanosecond timing via `Stopwatch` is essential.

### CapturedAt / CapturedAtUtc
*MetricsSnapshot, AllocationSnapshot, DumpAnalysisResult · DateTimeOffset*

**What it is:** The wall-clock time when the snapshot was taken, using `DateTimeOffset.UtcNow`. Used for display purposes and for computing `TimeBetweenDumps` in diff analysis.

### AnalyzedAtUtc
*DumpAnalysisResult · DateTimeOffset*

**What it is:** The wall-clock time when the heap analysis completed. Capture and analysis are separate steps — capture takes milliseconds (memory snapshot), while analysis (object enumeration) takes longer.

### FilePath / FileName
*DumpAnalysisResult · string*

**What they are:** For ClrMD in-process captures: `clrmd://heap_yyyyMMdd_HHmmss` (synthetic URI, no actual file). For dotnet-gcdump file captures: the actual file path on disk.

### FileSizeBytes
*DumpAnalysisResult · long · bytes*

**What it is:** The size of the `.gcdump` file on disk. Always 0 for ClrMD in-process captures (no file is written).
