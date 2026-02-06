namespace WeatherStatistics.Models;

public sealed class WeatherReading
{
    public required WeatherLocation Location { get; init; }
    public required double TemperatureCelsius { get; init; }
    public required double HumidityPercent { get; init; }
    public required double WindSpeedKmh { get; init; }
    public required WeatherCondition Condition { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string Summary { get; init; }
}
