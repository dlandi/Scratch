using AppSysMetrics.Diagnostics.Models;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Computes the diff between two dump analysis results to surface memory growth (leak suspects).
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

            diffs.Add(new HeapTypeDiff
            {
                TypeName = typeName,
                BaselineCount = baselineCount,
                CurrentCount = currentCount,
                DeltaCount = deltaCount,
                BaselineSizeBytes = baselineSize,
                CurrentSizeBytes = currentSize,
                DeltaSizeBytes = deltaSize,
                GrowthPercent = growthPercent
            });
        }

        // Sort by delta size descending — biggest growers first (leak suspects)
        diffs.Sort((a, b) => b.DeltaSizeBytes.CompareTo(a.DeltaSizeBytes));

        return new DumpDiffResult
        {
            Baseline = baseline,
            Current = current,
            TimeBetweenDumps = current.CapturedAtUtc - baseline.CapturedAtUtc,
            TypeDiffs = diffs,
            TotalHeapDelta = current.TotalHeapBytes - baseline.TotalHeapBytes,
            TotalObjectDelta = current.TotalObjectCount - baseline.TotalObjectCount
        };
    }
}
