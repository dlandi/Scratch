namespace Travelogue.Services;

/// <summary>
/// Deliberately leaks memory by holding strong references to allocated objects.
/// Used to exercise the GC Root Analysis pipeline with real, observable leak patterns.
/// </summary>
public sealed class MemoryLeakService
{
    private readonly Lock _lock = new();
    private readonly List<byte[]> _leakedBlobs = [];
    private readonly List<string> _leakedStrings = [];

    // ── Blob allocations ──

    /// <summary>
    /// Allocates <paramref name="count"/> byte arrays of <paramref name="sizeKb"/> KB each
    /// and retains them indefinitely. These produce <c>System.Byte[]</c> leak suspects
    /// with <c>StrongHandle</c> GC roots via this service's static-like singleton lifetime.
    /// </summary>
    public LeakAllocationResult AllocateBlobs(int count, int sizeKb)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeKb);

        var allocated = 0L;
        lock (_lock)
        {
            for (var i = 0; i < count; i++)
            {
                var blob = new byte[sizeKb * 1024];
                // Touch the array so it isn't optimized away
                Array.Fill(blob, (byte)(i % 256));
                _leakedBlobs.Add(blob);
                allocated += blob.Length;
            }
        }

        return new LeakAllocationResult(count, allocated);
    }

    // ── String allocations ──

    /// <summary>
    /// Generates <paramref name="count"/> random strings of <paramref name="length"/> characters each
    /// and retains them indefinitely. These produce <c>System.String</c> leak suspects.
    /// </summary>
    public LeakAllocationResult AllocateStrings(int count, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        var allocated = 0L;
        lock (_lock)
        {
            for (var i = 0; i < count; i++)
            {
                var s = string.Create(length, i, static (span, seed) =>
                {
                    var rng = new Random(seed ^ Environment.TickCount);
                    for (var j = 0; j < span.Length; j++)
                        span[j] = (char)rng.Next('A', 'z' + 1);
                });
                _leakedStrings.Add(s);
                allocated += s.Length * sizeof(char);
            }
        }

        return new LeakAllocationResult(count, allocated);
    }

    // ── Stats ──

    public LeakStats GetStats()
    {
        lock (_lock)
        {
            var blobBytes = _leakedBlobs.Sum(b => (long)b.Length);
            var stringBytes = _leakedStrings.Sum(s => (long)s.Length * sizeof(char));

            return new LeakStats(
                BlobCount: _leakedBlobs.Count,
                BlobBytes: blobBytes,
                StringCount: _leakedStrings.Count,
                StringBytes: stringBytes);
        }
    }

    // ── Clear ──

    /// <summary>Releases all retained references, allowing GC to reclaim the leaked objects.</summary>
    public LeakStats Clear()
    {
        lock (_lock)
        {
            var stats = GetStats();
            _leakedBlobs.Clear();
            _leakedStrings.Clear();
            return stats;
        }
    }
}

// ── Supporting records ──

public readonly record struct LeakAllocationResult(int Count, long AllocatedBytes);

public readonly record struct LeakStats(
    int BlobCount,
    long BlobBytes,
    int StringCount,
    long StringBytes)
{
    public long TotalBytes => BlobBytes + StringBytes;
    public int TotalCount => BlobCount + StringCount;
}
