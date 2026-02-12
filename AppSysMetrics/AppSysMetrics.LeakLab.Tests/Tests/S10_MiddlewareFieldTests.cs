using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// S10 — Singleton middleware stores per-request data in instance fields.
/// Verifies that accumulated byte[] payloads are detected as leak suspects
/// and root analysis traces retention through user code.
/// </summary>
[Collection("LeakLab")]
public sealed class S10_MiddlewareFieldTests : LeakLabTestBase
{
    public S10_MiddlewareFieldTests(LeakLabTestFixture fixture) : base(fixture) { }

    [Fact(Timeout = 120_000)]
    public async Task Simulator_Produces_Detectable_Leak()
    {
        var result = await RunDetectionPipelineAsync("S10");
        LeakAssertions.AssertLeakDetected(result);
    }

    [Fact(Timeout = 120_000)]
    public async Task Root_Analysis_Traces_To_User_Code()
    {
        var result = await RunDetectionPipelineAsync("S10");
        LeakAssertions.AssertRootAnalysisHasUserCode(result);
    }

    [Fact]
    public void ExpectedLeakTypes_Are_Specified()
    {
        var simulator = Fixture.Registry.GetSimulator("S10");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.Contains(simulator.ExpectedLeakTypes,
            t => t.Contains("Byte[]"));
    }
}
