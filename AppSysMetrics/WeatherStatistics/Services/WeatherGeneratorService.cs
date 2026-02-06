using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherStatistics.Models;

namespace WeatherStatistics.Services;

public sealed class WeatherGeneratorService : IHostedService, IWeatherGenerator, IDisposable
{
    private static readonly WeatherLocation[] Locations =
    [
        new("Tokyo", "Japan", 35.6762, 139.6503),
        new("New York", "USA", 40.7128, -74.0060),
        new("London", "UK", 51.5074, -0.1278),
        new("Paris", "France", 48.8566, 2.3522),
        new("Sydney", "Australia", -33.8688, 151.2093),
        new("Cairo", "Egypt", 30.0444, 31.2357),
        new("Mumbai", "India", 19.0760, 72.8777),
        new("Sao Paulo", "Brazil", -23.5505, -46.6333),
        new("Moscow", "Russia", 55.7558, 37.6173),
        new("Cape Town", "South Africa", -33.9249, 18.4241),
    ];

    private static readonly WeatherCondition[] Conditions =
        Enum.GetValues<WeatherCondition>();

    private readonly List<WeatherReading> _readings = [];
    private readonly Random _random = new();
    private readonly ILogger<WeatherGeneratorService> _logger;
    private readonly object _lock = new();

    private CancellationTokenSource? _cts;
    private Task? _generationTask;
    private TimeSpan _interval;
    private int _readingsPerTick;
    private volatile bool _isRunning;

    public WeatherGeneratorService(
        IOptions<WeatherGeneratorOptions> options,
        ILogger<WeatherGeneratorService> logger)
    {
        _logger = logger;
        _interval = options.Value.GenerationInterval;
        _readingsPerTick = options.Value.ReadingsPerTick;

        if (options.Value.AutoStart)
            Start();
    }

    public bool IsRunning => _isRunning;

    public event Action<WeatherStats>? OnStatsUpdated;

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();
        return Task.CompletedTask;
    }

    public void Start()
    {
        if (_isRunning) return;

        _cts = new CancellationTokenSource();
        _isRunning = true;
        _generationTask = GenerateLoopAsync(_cts.Token);
        _logger.LogInformation("Weather generator started. Interval: {Interval}, ReadingsPerTick: {Count}",
            _interval, _readingsPerTick);
    }

    public void Stop()
    {
        if (!_isRunning) return;

        _isRunning = false;
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        _logger.LogInformation("Weather generator stopped. Total readings: {Count}", _readings.Count);
    }

    public void SetInterval(TimeSpan interval)
    {
        _interval = interval;
        if (_isRunning)
        {
            Stop();
            Start();
        }
    }

    public void SetReadingsPerTick(int count)
    {
        _readingsPerTick = Math.Max(1, count);
    }

    public WeatherStats GetStats()
    {
        lock (_lock)
        {
            var perLocation = new Dictionary<string, int>();
            foreach (var reading in _readings)
            {
                var city = reading.Location.City;
                perLocation[city] = perLocation.GetValueOrDefault(city) + 1;
            }

            return new WeatherStats
            {
                TotalReadings = _readings.Count,
                ReadingsPerLocation = perLocation,
                ApproximateMemoryBytes = _readings.Count * 200L,
                IsRunning = _isRunning,
                GenerationInterval = _interval,
                ReadingsPerTick = _readingsPerTick
            };
        }
    }

    public IReadOnlyList<WeatherReading> GetLatestReadings(int count)
    {
        lock (_lock)
        {
            var start = Math.Max(0, _readings.Count - count);
            return _readings.GetRange(start, _readings.Count - start);
        }
    }

    private async Task GenerateLoopAsync(CancellationToken ct)
    {
        try
        {
            using var timer = new PeriodicTimer(_interval);
            while (await timer.WaitForNextTickAsync(ct))
            {
                GenerateBatch();
            }
        }
        catch (OperationCanceledException)
        {
            // Expected on stop
        }
    }

    private void GenerateBatch()
    {
        lock (_lock)
        {
            for (int i = 0; i < _readingsPerTick; i++)
            {
                var location = Locations[_random.Next(Locations.Length)];
                var condition = Conditions[_random.Next(Conditions.Length)];
                var temp = Math.Round(_random.NextDouble() * 60 - 10, 1); // -10 to 50 C
                var humidity = Math.Round(_random.NextDouble() * 100, 1);
                var wind = Math.Round(_random.NextDouble() * 150, 1);

                var reading = new WeatherReading
                {
                    Location = location,
                    TemperatureCelsius = temp,
                    HumidityPercent = humidity,
                    WindSpeedKmh = wind,
                    Condition = condition,
                    Timestamp = DateTimeOffset.UtcNow,
                    Summary = $"{condition} in {location.City}, {location.Country}: " +
                              $"{temp:F1}C, {humidity:F1}% humidity, wind {wind:F1} km/h. " +
                              $"Latitude {location.Latitude:F4}, Longitude {location.Longitude:F4}. " +
                              $"Recorded at {DateTimeOffset.UtcNow:O}."
                };

                _readings.Add(reading);
            }
        }

        OnStatsUpdated?.Invoke(GetStats());
    }

    public void Dispose()
    {
        Stop();
    }
}
