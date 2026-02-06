using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using AppSysMetrics.Models;

namespace AppSysMetrics.Collection;

public sealed class AllocationEventListener : EventListener
{
    private const int AllocationTickEventId = 10;
    private const string DotNetRuntimeProviderName = "Microsoft-Windows-DotNETRuntime";
    private const long GCKeyword = 0x1;

    private readonly ConcurrentDictionary<string, AllocationAggregation> _aggregations = new();
    private readonly ConcurrentQueue<LargeObjectRecord> _recentLargeObjects = new();
    private readonly int _maxLargeObjectRecords;
    private readonly int _topNTypes;

    public AllocationEventListener(int topNTypes = 20, int maxLargeObjectRecords = 50)
    {
        _topNTypes = topNTypes;
        _maxLargeObjectRecords = maxLargeObjectRecords;
    }

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == DotNetRuntimeProviderName)
        {
            EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)GCKeyword);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (eventData.EventId != AllocationTickEventId) return;
        if (eventData.Payload is null || eventData.Payload.Count < 4) return;

        // AllocationTick payload (.NET 5+):
        // [0] AllocationAmount (uint)
        // [1] AllocationKind (uint: 0=small, 1=large)
        // [2] ClrInstanceID (ushort)
        // [3] AllocationAmount64 (long)
        // [4] TypeID (ulong)
        // [5] TypeName (string)
        // [6] HeapIndex (uint)
        // [7] Address (ulong)
        // [8] ObjectSize (ulong)

        string typeName = "Unknown";
        if (eventData.Payload.Count > 5 && eventData.Payload[5] is string name && !string.IsNullOrEmpty(name))
        {
            typeName = name;
        }

        long allocationAmount;
        if (eventData.Payload.Count > 3 && eventData.Payload[3] is long amount64)
        {
            allocationAmount = amount64;
        }
        else
        {
            allocationAmount = Convert.ToInt64(eventData.Payload[0]);
        }

        var allocationKind = Convert.ToUInt32(eventData.Payload[1]);
        var isLarge = allocationKind == 1;

        _aggregations.AddOrUpdate(
            typeName,
            _ => new AllocationAggregation { TotalBytes = allocationAmount, Count = 1 },
            (_, existing) =>
            {
                Interlocked.Add(ref existing.TotalBytes, allocationAmount);
                Interlocked.Increment(ref existing.Count);
                return existing;
            });

        if (isLarge)
        {
            _recentLargeObjects.Enqueue(new LargeObjectRecord(
                typeName, allocationAmount, DateTimeOffset.UtcNow));

            while (_recentLargeObjects.Count > _maxLargeObjectRecords)
                _recentLargeObjects.TryDequeue(out _);
        }
    }

    public AllocationSnapshot CreateSnapshot()
    {
        var topTypes = _aggregations
            .Select(kvp => new AllocationTypeInfo
            {
                TypeName = kvp.Key,
                TotalBytes = Interlocked.Read(ref kvp.Value.TotalBytes),
                AllocationCount = kvp.Value.Count,
                IsLargeObject = false
            })
            .OrderByDescending(a => a.TotalBytes)
            .Take(_topNTypes)
            .ToList();

        var largeObjects = _recentLargeObjects
            .Reverse()
            .Select(r => new AllocationTypeInfo
            {
                TypeName = r.TypeName,
                TotalBytes = r.SizeBytes,
                AllocationCount = 1,
                IsLargeObject = true
            })
            .ToList();

        var totalBytes = _aggregations.Values.Sum(a => Interlocked.Read(ref a.TotalBytes));
        var totalCount = _aggregations.Values.Sum(a => a.Count);

        return new AllocationSnapshot
        {
            CapturedAt = DateTimeOffset.UtcNow,
            TopAllocatingTypes = topTypes,
            RecentLargeObjectAllocations = largeObjects,
            TotalTrackedBytes = totalBytes,
            TotalTrackedCount = totalCount
        };
    }

    public void Reset()
    {
        _aggregations.Clear();
        while (_recentLargeObjects.TryDequeue(out _)) { }
    }

    private sealed class AllocationAggregation
    {
        public long TotalBytes;
        public int Count;
    }

    private sealed record LargeObjectRecord(string TypeName, long SizeBytes, DateTimeOffset Timestamp);
}
