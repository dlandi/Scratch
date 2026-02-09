using System.Diagnostics;
using AppSysMetrics.Models;

namespace AppSysMetrics.Collection;

public sealed class CpuSampler
{
    private readonly Process _process = Process.GetCurrentProcess();
    private long _previousTimestamp;
    private TimeSpan _previousCpuTime;

    public CpuSampler()
    {
        _previousTimestamp = Stopwatch.GetTimestamp();
        _previousCpuTime = _process.TotalProcessorTime;
    }

    public CpuMetrics Sample()
    {
        var currentTimestamp = Stopwatch.GetTimestamp();
        _process.Refresh();
        var currentCpuTime = _process.TotalProcessorTime;

        var elapsedTime = Stopwatch.GetElapsedTime(_previousTimestamp, currentTimestamp);
        var cpuUsed = currentCpuTime - _previousCpuTime;

        double cpuPercentage = 0;
        if (elapsedTime.TotalMilliseconds > 0)
        {
            cpuPercentage = cpuUsed.TotalMilliseconds
                / (elapsedTime.TotalMilliseconds * Environment.ProcessorCount)
                * 100.0;
            cpuPercentage = Math.Clamp(cpuPercentage, 0, 100);
        }

        _previousTimestamp = currentTimestamp;
        _previousCpuTime = currentCpuTime;

        return new CpuMetrics
        {
            CpuPercentage = Math.Round(cpuPercentage, 2),
            TotalProcessorTime = currentCpuTime,
            ProcessorCount = Environment.ProcessorCount
        };
    }
}
