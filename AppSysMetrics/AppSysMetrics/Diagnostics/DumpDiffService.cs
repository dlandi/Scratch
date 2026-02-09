using AppSysMetrics.Diagnostics.Models;
using AppSysMetrics.Models;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Computes the diff between two dump analysis results to surface memory growth (leak suspects).
/// When both dumps carry <see cref="DumpAnalysisResult.AllocationAtCapture"/>,
/// the diff is enriched with allocation throughput and per-type retention ratios.
/// Pure computation — no dependencies, no side effects.
/// </summary>
public sealed class DumpDiffService
{
    public DumpDiffResult ComputeDiff(
        DumpAnalysisResult baseline,
        DumpAnalysisResult current)
    {
        var baselineMap = baseline.TopTypes
            .ToDictionary(t => t.TypeName, t => t);

        var currentMap = current.TopTypes
            .ToDictionary(t => t.TypeName, t => t);

        // Build allocation lookup maps when both dumps have correlation data
        var hasCorrelation = baseline.AllocationAtCapture is not null
                          && current.AllocationAtCapture is not null;

        Dictionary<string, AllocationTypeInfo>? baseAllocMap = null;
        Dictionary<string, AllocationTypeInfo>? curAllocMap = null;

        if (hasCorrelation)
        {
            baseAllocMap = baseline.AllocationAtCapture!.TopAllocatingTypes
                .ToDictionary(a => a.TypeName, a => a);
            curAllocMap = current.AllocationAtCapture!.TopAllocatingTypes
                .ToDictionary(a => a.TypeName, a => a);
        }

        // Union of all type names from both dumps
        var allTypeNames = new HashSet<string>(baselineMap.Keys);
        allTypeNames.UnionWith(currentMap.Keys);

        var diffs = new List<HeapTypeDiff>();

        foreach (var typeName in allTypeNames)
        {
            var hasBaseline = baselineMap.TryGetValue(typeName, out var baselineType);
            var hasCurrent = currentMap.TryGetValue(typeName, out var currentType);

            var baselineCount = hasBaseline ? baselineType!.InstanceCount : 0;
            var currentCount = hasCurrent ? currentType!.InstanceCount : 0;
            var baselineSize = hasBaseline ? baselineType!.TotalSizeBytes : 0;
            var currentSize = hasCurrent ? currentType!.TotalSizeBytes : 0;

            var deltaSize = currentSize - baselineSize;
            var deltaCount = currentCount - baselineCount;

            var growthPercent = baselineSize > 0
                ? (deltaSize / (double)baselineSize) * 100.0
                : currentSize > 0
                    ? 100.0  // New type appeared
                    : 0.0;

            // Allocation correlation per type
            long? baseAllocBytes = null;
            long? curAllocBytes = null;
            long? allocBetween = null;
            double? retention = null;

            if (hasCorrelation)
            {
                baseAllocMap!.TryGetValue(typeName, out var baseAlloc);
                curAllocMap!.TryGetValue(typeName, out var curAlloc);

                baseAllocBytes = baseAlloc?.TotalBytes;
                curAllocBytes = curAlloc?.TotalBytes;

                if (baseAllocBytes.HasValue && curAllocBytes.HasValue)
                {
                    allocBetween = curAllocBytes.Value - baseAllocBytes.Value;

                    // Retention = how much of what was allocated actually stuck on the heap.
                    // Only meaningful when allocations occurred (throughput > 0)
                    // and the heap actually grew for this type (deltaSize > 0).
                    if (allocBetween.Value > 0 && deltaSize > 0)
                    {
                        retention = (double)deltaSize / allocBetween.Value;
                        // Cap at 1.0 — values >1 mean pre-existing objects grew (not a ratio issue)
                        if (retention > 1.0) retention = 1.0;
                    }
                    else if (allocBetween.Value > 0)
                    {
                        // Allocated but heap shrank or stayed flat — good, everything was collected
                        retention = 0.0;
                    }
                    // else: no allocations for this type between dumps → retention stays null
                }
            }

            diffs.Add(new HeapTypeDiff
            {
                TypeName = typeName,
                BaselineCount = baselineCount,
                CurrentCount = currentCount,
                DeltaCount = deltaCount,
                BaselineSizeBytes = baselineSize,
                CurrentSizeBytes = currentSize,
                DeltaSizeBytes = deltaSize,
                GrowthPercent = growthPercent,
                BaselineAllocatedBytes = baseAllocBytes,
                CurrentAllocatedBytes = curAllocBytes,
                AllocatedBetweenBytes = allocBetween,
                RetentionRatio = retention
            });
        }

        // Sort by delta size descending — biggest growers first (leak suspects)
        diffs.Sort((a, b) => b.DeltaSizeBytes.CompareTo(a.DeltaSizeBytes));

        // Compute summary allocation metrics
        long? totalAllocBetween = null;
        long? totalCollected = null;
        if (hasCorrelation)
        {
            var baseAppBytes = baseline.AllocationAtCapture!.AppTrackedBytes;
            var curAppBytes = current.AllocationAtCapture!.AppTrackedBytes;
            totalAllocBetween = curAppBytes - baseAppBytes;
            var heapDelta = current.TotalHeapBytes - baseline.TotalHeapBytes;
            totalCollected = totalAllocBetween.Value - heapDelta;
            // Floor at 0 — negative means heap grew more than allocated (pre-existing objects)
            if (totalCollected < 0) totalCollected = 0;
        }

        return new DumpDiffResult
        {
            Baseline = baseline,
            Current = current,
            TimeBetweenDumps = current.CapturedAtUtc - baseline.CapturedAtUtc,
            TypeDiffs = diffs,
            TotalHeapDelta = current.TotalHeapBytes - baseline.TotalHeapBytes,
            TotalObjectDelta = current.TotalObjectCount - baseline.TotalObjectCount,
            HasAllocationCorrelation = hasCorrelation,
            TotalAllocatedBetween = totalAllocBetween,
            TotalCollectedBetween = totalCollected
        };
    }
}
