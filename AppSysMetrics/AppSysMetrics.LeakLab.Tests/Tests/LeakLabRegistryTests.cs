using AppSysMetrics.LeakLab.Tests.Infrastructure;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Tests;

/// <summary>
/// Contract tests for <see cref="LeakLabRegistry"/> — verifies all simulators
/// are registered, resolve by ID, and have valid metadata.
/// No heap captures — these are fast unit-like checks.
/// </summary>
[Collection("LeakLab")]
public sealed class LeakLabRegistryTests
{
    private readonly LeakLabTestFixture _fixture;

    public LeakLabRegistryTests(LeakLabTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Registry_Contains_All_10_Simulators()
    {
        var registry = _fixture.Registry;
        Assert.Equal(10, registry.GetAll().Count);
    }

    [Theory]
    [InlineData("S01")]
    [InlineData("S03")]
    [InlineData("S05")]
    [InlineData("S06")]
    [InlineData("S08")]
    [InlineData("S10")]
    [InlineData("S13")]
    [InlineData("S15")]
    [InlineData("S16")]
    [InlineData("S17")]
    public void Registry_Resolves_Simulator_By_Id(string scenarioId)
    {
        var simulator = _fixture.Registry.GetSimulator(scenarioId);
        Assert.NotNull(simulator);
        Assert.Equal(scenarioId, simulator.ScenarioId);
    }

    [Theory]
    [InlineData("S01")]
    [InlineData("S03")]
    [InlineData("S05")]
    [InlineData("S06")]
    [InlineData("S08")]
    [InlineData("S10")]
    [InlineData("S13")]
    [InlineData("S15")]
    [InlineData("S16")]
    [InlineData("S17")]
    public void Simulator_Has_Valid_Metadata(string scenarioId)
    {
        var simulator = _fixture.Registry.GetSimulator(scenarioId);
        Assert.False(string.IsNullOrWhiteSpace(simulator.Description),
            $"{scenarioId}: Description is empty");
        Assert.NotEmpty(simulator.ExpectedLeakTypes);
        Assert.All(simulator.ExpectedLeakTypes, t =>
            Assert.False(string.IsNullOrWhiteSpace(t),
                $"{scenarioId}: ExpectedLeakTypes contains empty entry"));
    }

    [Fact]
    public void Registry_Throws_For_Unknown_Scenario()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _fixture.Registry.GetSimulator("S99"));
    }

    [Theory]
    [InlineData("S01")]
    [InlineData("S03")]
    [InlineData("S05")]
    [InlineData("S06")]
    [InlineData("S08")]
    [InlineData("S10")]
    [InlineData("S13")]
    [InlineData("S15")]
    [InlineData("S16")]
    [InlineData("S17")]
    public void Simulator_Starts_Not_Running(string scenarioId)
    {
        var simulator = _fixture.Registry.GetSimulator(scenarioId);
        Assert.False(simulator.IsRunning);
    }
}
