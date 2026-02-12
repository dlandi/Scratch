using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// S13 — IMemoryCache with no size limit and no expiration.
/// Verifies that cached byte[] entries and CacheEntry objects are detected
/// as leak suspects. CacheEntry is a developer-facing framework type
/// recognized by <c>TypeClassification.IsDeveloperFacingFrameworkType</c>.
/// </summary>
[Collection("LeakLab")]
public sealed class S13_UnboundedCacheTests : LeakLabTestBase
{
    public S13_UnboundedCacheTests(LeakLabTestFixture fixture) : base(fixture) { }

    [Fact(Timeout = 120_000)]
    public async Task Simulator_Produces_Detectable_Leak()
    {
        var result = await RunDetectionPipelineAsync("S13");
        LeakAssertions.AssertLeakDetected(result);
    }

    [Fact(Timeout = 120_000)]
    public async Task Root_Analysis_Traces_To_User_Code()
    {
        var result = await RunDetectionPipelineAsync("S13");
        LeakAssertions.AssertRootAnalysisHasUserCode(result);
    }

    [Fact]
    public void ExpectedLeakTypes_Are_Specified()
    {
        var simulator = Fixture.Registry.GetSimulator("S13");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.Contains(simulator.ExpectedLeakTypes,
            t => t.Contains("Byte[]") || t.Contains("CacheEntry"));
    }
}
