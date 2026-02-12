using AppSysMetrics.Diagnostics.Models;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Infrastructure;

/// <summary>
/// Static assertion helpers for leak detection integration tests.
/// Use <c>Contains</c> matching on type names for tolerance against
/// generic type parameter variations in ClrMD output.
/// </summary>
public static class LeakAssertions
{
    /// <summary>
    /// Asserts that at least one of the simulator's <see cref="ILeakSimulator.ExpectedLeakTypes"/>
    /// appears in the detected leak suspects from the diff.
    /// </summary>
    public static void AssertLeakDetected(LeakDetectionResult result)
    {
        Assert.NotEmpty(result.DiffSuspects);

        var suspectTypeNames = result.DiffSuspects
            .Select(s => s.TypeName)
            .ToHashSet(StringComparer.Ordinal);

        var expectedTypes = result.Simulator.ExpectedLeakTypes;

        var matched = expectedTypes
            .Where(expected => suspectTypeNames
                .Any(suspect => suspect.Contains(expected, StringComparison.Ordinal)
                             || expected.Contains(suspect, StringComparison.Ordinal)))
            .ToList();

        Assert.True(
            matched.Count > 0,
            $"Scenario {result.Simulator.ScenarioId}: None of the expected types " +
            $"[{string.Join(", ", expectedTypes)}] found in suspects " +
            $"[{string.Join(", ", suspectTypeNames)}]");
    }

    /// <summary>
    /// Asserts that root analysis was performed and found at least one
    /// retention path that passes through user code (non-framework).
    /// </summary>
    public static void AssertRootAnalysisHasUserCode(LeakDetectionResult result)
    {
        Assert.NotNull(result.RootAnalysis);

        var analyses = result.RootAnalysis!.TypeAnalyses;
        Assert.NotEmpty(analyses);

        var hasUserCodePath = analyses
            .Any(ta => ta.Roots.Any(r => r.HasUserCode));

        Assert.True(
            hasUserCodePath,
            $"Scenario {result.Simulator.ScenarioId}: Root analysis found no retention paths " +
            $"through user code. Analyzed types: " +
            $"[{string.Join(", ", analyses.Select(a => $"{a.TypeName} ({a.Roots.Count} roots)"))}]");
    }

    /// <summary>
    /// Asserts that at least one detected suspect has a retention ratio
    /// at or above the specified minimum, indicating a true leak.
    /// </summary>
    public static void AssertHighRetention(
        LeakDetectionResult result,
        double minimumRatio = 0.5)
    {
        var highRetention = result.DiffSuspects
            .Where(s => s.RetentionRatio.HasValue && s.RetentionRatio >= minimumRatio)
            .ToList();

        Assert.True(
            highRetention.Count > 0,
            $"Scenario {result.Simulator.ScenarioId}: No suspects with RetentionRatio >= {minimumRatio}. " +
            $"Suspects: [{string.Join(", ", result.DiffSuspects.Select(s =>
                $"{s.TypeName}={s.RetentionRatio?.ToString("F2") ?? "null"}"))}]");
    }
}
