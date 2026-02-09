using WeatherStatistics.Models;

namespace WeatherStatistics.Services;

public interface IWeatherGenerator
{
    bool IsRunning { get; }
    void Start();
    void Stop();
    void SetInterval(TimeSpan interval);
    void SetReadingsPerTick(int count);
    WeatherStats GetStats();
    IReadOnlyList<WeatherReading> GetLatestReadings(int count);
    int TrimToLatest();
    event Action<WeatherStats>? OnStatsUpdated;
}
