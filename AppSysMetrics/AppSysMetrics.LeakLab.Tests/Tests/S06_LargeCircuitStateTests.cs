using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// S06 — Circuit-scoped service holds large data for circuit lifetime.
/// Verifies that accumulated byte[] payloads are detected as leak suspects
/// and root analysis identifies user-code retention paths.
/// </summary>
[Collection("LeakLab")]
public sealed class S06_LargeCircuitStateTests : LeakLabTestBase
{
    public S06_LargeCircuitStateTests(LeakLabTestFixture fixture) : base(fixture) { }

    [Fact(Timeout = 120_000)]
    public async Task Simulator_Produces_Detectable_Leak()
    {
        var result = await RunDetectionPipelineAsync("S06");
        LeakAssertions.AssertLeakDetected(result);
    }

    [Fact(Timeout = 120_000)]
    public async Task Root_Analysis_Traces_To_User_Code()
    {
        var result = await RunDetectionPipelineAsync("S06");
        LeakAssertions.AssertRootAnalysisHasUserCode(result);
    }

    [Fact]
    public void ExpectedLeakTypes_Are_Specified()
    {
        var simulator = Fixture.Registry.GetSimulator("S06");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.Contains(simulator.ExpectedLeakTypes,
            t => t.Contains("Byte[]"));
    }
}
