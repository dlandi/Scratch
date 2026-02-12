using AppSysMetrics.Diagnostics.Models;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Infrastructure;

/// <summary>
/// Base class for per-simulator integration tests.
/// Provides <see cref="RunDetectionPipelineAsync"/> which executes the
/// full 3-capture pipeline: baseline → diff (with leak suspects) → root analysis.
/// </summary>
[Collection("LeakLab")]
public abstract class LeakLabTestBase
{
    protected readonly LeakLabTestFixture Fixture;

    protected LeakLabTestBase(LeakLabTestFixture fixture)
    {
        Fixture = fixture;
    }

    /// <summary>
    /// Runs the full 3-capture detection pipeline for a simulator.
    /// <list type="number">
    ///   <item>Reset simulator and clear hub state</item>
    ///   <item>Force GC to flush prior garbage</item>
    ///   <item>Capture 1: baseline heap snapshot</item>
    ///   <item>Start simulator — allocations begin</item>
    ///   <item>Wait for allocations to accumulate</item>
    ///   <item>Capture 2: diff triggers leak suspect detection</item>
    ///   <item>Pause for continuous simulators to add more</item>
    ///   <item>Capture 3: root analysis using suspects from capture 2</item>
    ///   <item>Stop simulator</item>
    /// </list>
    /// </summary>
    protected async Task<LeakDetectionResult> RunDetectionPipelineAsync(
        string scenarioId,
        TimeSpan? activationDuration = null,
        TimeSpan? interCapturePause = null)
    {
        var duration = activationDuration ?? TimeSpan.FromSeconds(5);
        var pause = interCapturePause ?? TimeSpan.FromSeconds(3);

        var simulator = Fixture.Registry.GetSimulator(scenarioId);
        simulator.Reset();
        Fixture.AnalysisHub.Clear();

        // Force GC to get a clean baseline
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);

        // Capture 1: Baseline
        var capture1 = await Fixture.Diagnostics.CaptureGcDumpAsync();
        Assert.True(capture1.Success, $"Capture 1 (baseline) failed: {capture1.ErrorMessage}");

        // Activate the simulator — creates leaked objects
        await simulator.StartAsync();

        // Wait for allocations to accumulate
        await Task.Delay(duration);

        // Capture 2: Diff — triggers LeakSuspectDetector.Detect(), stores in hub
        var capture2 = await Fixture.Diagnostics.CaptureGcDumpAsync();
        Assert.True(capture2.Success, $"Capture 2 (diff) failed: {capture2.ErrorMessage}");

        // Record suspects from the diff
        var diffSuspects = Fixture.AnalysisHub.LatestLeakSuspects?.ToList()
            ?? new List<HeapTypeDiff>();

        // Pause — for continuous simulators (S15, S16) to add more items
        await Task.Delay(pause);

        // Capture 3: Root analysis — uses suspects from capture 2 as rootTargets
        var capture3 = await Fixture.Diagnostics.CaptureGcDumpAsync();
        Assert.True(capture3.Success, $"Capture 3 (root analysis) failed: {capture3.ErrorMessage}");

        // Stop the simulator (retained objects remain until Reset)
        await simulator.StopAsync();

        return new LeakDetectionResult
        {
            DiffSuspects = diffSuspects,
            RootAnalysis = Fixture.AnalysisHub.Latest?.RootAnalysis,
            Simulator = simulator
        };
    }
}
