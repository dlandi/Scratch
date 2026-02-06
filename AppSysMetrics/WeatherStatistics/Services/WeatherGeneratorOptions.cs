namespace WeatherStatistics.Services;

public sealed class WeatherGeneratorOptions
{
    public TimeSpan GenerationInterval { get; set; } = TimeSpan.FromMilliseconds(100);
    public int ReadingsPerTick { get; set; } = 5;
    public bool AutoStart { get; set; } = false;
}
