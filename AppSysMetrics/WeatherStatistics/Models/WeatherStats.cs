namespace WeatherStatistics.Models;

public sealed record WeatherStats
{
    public int TotalReadings { get; init; }
    public IReadOnlyDictionary<string, int> ReadingsPerLocation { get; init; } = new Dictionary<string, int>();
    public long ApproximateMemoryBytes { get; init; }
    public bool IsRunning { get; init; }
    public TimeSpan GenerationInterval { get; init; }
    public int ReadingsPerTick { get; init; }
}
