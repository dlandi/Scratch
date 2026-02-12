using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// S03 — Component subscribes to singleton event but never unsubscribes.
/// Verifies that leaked subscriber components are detected via the event
/// delegate chain and traced back through user code.
/// </summary>
[Collection("LeakLab")]
public sealed class S03_EventHandlerTests : LeakLabTestBase
{
    public S03_EventHandlerTests(LeakLabTestFixture fixture) : base(fixture) { }

    [Fact(Timeout = 120_000)]
    public async Task Simulator_Produces_Detectable_Leak()
    {
        var result = await RunDetectionPipelineAsync("S03");
        LeakAssertions.AssertLeakDetected(result);
    }

    [Fact(Timeout = 120_000)]
    public async Task Root_Analysis_Traces_To_User_Code()
    {
        var result = await RunDetectionPipelineAsync("S03");
        LeakAssertions.AssertRootAnalysisHasUserCode(result);
    }

    [Fact]
    public void ExpectedLeakTypes_Are_Specified()
    {
        var simulator = Fixture.Registry.GetSimulator("S03");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.Contains(simulator.ExpectedLeakTypes,
            t => t.Contains("EventSubscriberComponent"));
    }
}
