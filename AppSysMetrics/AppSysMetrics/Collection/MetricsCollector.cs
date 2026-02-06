using System.Diagnostics;
using AppSysMetrics.Models;

namespace AppSysMetrics.Collection;

public sealed class MetricsCollector : IMetricsCollector
{
    private readonly CpuSampler _cpuSampler = new();
    private readonly AllocationRateTracker _allocationTracker = new();

    public MetricsSnapshot Collect()
    {
        var process = Process.GetCurrentProcess();
        process.Refresh();

        var gcInfo = GC.GetGCMemoryInfo();

        var processMetrics = new ProcessMetrics
        {
            WorkingSet64 = process.WorkingSet64,
            PrivateMemorySize64 = process.PrivateMemorySize64,
            VirtualMemorySize64 = process.VirtualMemorySize64,
            PagedMemorySize64 = process.PagedMemorySize64,
            ThreadCount = process.Threads.Count,
            HandleCount = process.HandleCount
        };

        var cpuMetrics = _cpuSampler.Sample();

        var (totalAllocated, allocationRate) = _allocationTracker.Sample();

        var generationInfo = new List<GcGenerationInfo>();
        var genInfoSpan = gcInfo.GenerationInfo;
        for (int i = 0; i < genInfoSpan.Length; i++)
        {
            var gi = genInfoSpan[i];
            generationInfo.Add(new GcGenerationInfo
            {
                Generation = i,
                SizeBeforeBytes = gi.SizeBeforeBytes,
                SizeAfterBytes = gi.SizeAfterBytes,
                FragmentationBeforeBytes = gi.FragmentationBeforeBytes,
                FragmentationAfterBytes = gi.FragmentationAfterBytes
            });
        }

        var gcMetrics = new GcMetrics
        {
            HeapSizeBytes = gcInfo.HeapSizeBytes,
            FragmentedBytes = gcInfo.FragmentedBytes,
            TotalAvailableMemoryBytes = gcInfo.TotalAvailableMemoryBytes,
            MemoryLoadPercent = gcInfo.TotalAvailableMemoryBytes > 0
                ? (double)gcInfo.MemoryLoadBytes / gcInfo.TotalAvailableMemoryBytes * 100
                : 0,
            TotalMemory = GC.GetTotalMemory(forceFullCollection: false),
            TotalAllocatedBytes = totalAllocated,
            AllocationRateBytesPerSecond = allocationRate,
            PauseTimePercentage = gcInfo.PauseTimePercentage,
            Gen0Collections = GC.CollectionCount(0),
            Gen1Collections = GC.CollectionCount(1),
            Gen2Collections = GC.CollectionCount(2),
            GenerationInfo = generationInfo
        };

        return new MetricsSnapshot
        {
            TimestampTicks = Stopwatch.GetTimestamp(),
            CapturedAt = DateTimeOffset.UtcNow,
            Process = processMetrics,
            Cpu = cpuMetrics,
            Gc = gcMetrics
        };
    }
}
