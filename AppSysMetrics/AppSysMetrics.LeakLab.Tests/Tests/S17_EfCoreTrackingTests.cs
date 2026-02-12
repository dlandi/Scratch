using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// S17 — Long-lived EF Core DbContext with tracking enabled.
/// Verifies that tracked SensorReading entities are detected as leak suspects
/// and that root analysis traces retention back to user code.
/// </summary>
[Collection("LeakLab")]
public sealed class S17_EfCoreTrackingTests : LeakLabTestBase
{
    public S17_EfCoreTrackingTests(LeakLabTestFixture fixture) : base(fixture) { }

    [Fact(Timeout = 120_000)]
    public async Task Simulator_Produces_Detectable_Leak()
    {
        var result = await RunDetectionPipelineAsync("S17");
        LeakAssertions.AssertLeakDetected(result);
    }

    [Fact(Timeout = 120_000)]
    public async Task Root_Analysis_Traces_To_User_Code()
    {
        var result = await RunDetectionPipelineAsync("S17");
        LeakAssertions.AssertRootAnalysisHasUserCode(result);
    }

    [Fact]
    public void ExpectedLeakTypes_Are_Specified()
    {
        var simulator = Fixture.Registry.GetSimulator("S17");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.Contains(simulator.ExpectedLeakTypes,
            t => t.Contains("SensorReading"));
    }
}
