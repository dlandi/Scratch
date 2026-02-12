namespace AppSysMetrics.LeakLab;

/// <summary>
/// Registry of all registered <see cref="ILeakSimulator"/> instances.
/// Populated via DI — collects all <c>ILeakSimulator</c> registrations.
/// </summary>
public sealed class LeakLabRegistry
{
    private readonly Dictionary<string, ILeakSimulator> _simulators;

    public LeakLabRegistry(IEnumerable<ILeakSimulator> simulators)
    {
        _simulators = simulators.ToDictionary(s => s.ScenarioId, StringComparer.Ordinal);
    }

    /// <summary>Get a simulator by scenario ID. Throws if not found.</summary>
    public ILeakSimulator GetSimulator(string scenarioId) =>
        _simulators.TryGetValue(scenarioId, out var sim)
            ? sim
            : throw new KeyNotFoundException($"No simulator registered for scenario '{scenarioId}'");

    /// <summary>All registered simulators.</summary>
    public IReadOnlyList<ILeakSimulator> GetAll() => _simulators.Values.ToList();

    /// <summary>All registered scenario IDs.</summary>
    public IReadOnlyList<string> ScenarioIds => _simulators.Keys.ToList();
}
