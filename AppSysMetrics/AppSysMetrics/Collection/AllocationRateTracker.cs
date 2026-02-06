using System.Diagnostics;

namespace AppSysMetrics.Collection;

public sealed class AllocationRateTracker
{
    private long _previousTimestamp;
    private long _previousAllocatedBytes;

    public AllocationRateTracker()
    {
        _previousTimestamp = Stopwatch.GetTimestamp();
        _previousAllocatedBytes = GC.GetTotalAllocatedBytes(precise: false);
    }

    public (long totalAllocatedBytes, double ratePerSecond) Sample()
    {
        var currentTimestamp = Stopwatch.GetTimestamp();
        var currentAllocated = GC.GetTotalAllocatedBytes(precise: false);

        var elapsed = Stopwatch.GetElapsedTime(_previousTimestamp, currentTimestamp);
        var allocated = currentAllocated - _previousAllocatedBytes;

        double rate = elapsed.TotalSeconds > 0
            ? allocated / elapsed.TotalSeconds
            : 0;

        _previousTimestamp = currentTimestamp;
        _previousAllocatedBytes = currentAllocated;

        return (currentAllocated, rate);
    }
}
