using AppSysMetrics.Diagnostics.Models;
using AppSysMetrics.LeakLab;

namespace AppSysMetrics.LeakLab.Tests.Infrastructure;

/// <summary>
/// Result of running the full 3-capture detection pipeline for a simulator.
/// </summary>
public sealed class LeakDetectionResult
{
    /// <summary>Leak suspects detected from the diff between captures 1 and 2.</summary>
    public required List<HeapTypeDiff> DiffSuspects { get; init; }

    /// <summary>Root analysis from capture 3 (may be null if no suspects were predicted).</summary>
    public RootAnalysisResult? RootAnalysis { get; init; }

    /// <summary>The simulator that was tested.</summary>
    public required ILeakSimulator Simulator { get; init; }
}
