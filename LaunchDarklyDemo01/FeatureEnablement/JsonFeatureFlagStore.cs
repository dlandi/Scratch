using System.Text.Json;
using FeatureEnablement.Models;

namespace FeatureEnablement;

/// <summary>
/// JSON file-based implementation of feature flag persistence.
/// </summary>
public class JsonFeatureFlagStore : IFeatureFlagStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;
    private static readonly SemaphoreSlim _lock = new(1, 1);

    public JsonFeatureFlagStore(string filePath)
    {
        _filePath = filePath;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<FeatureFlagConfiguration> LoadAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (!File.Exists(_filePath))
            {
                return new FeatureFlagConfiguration();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<FeatureFlagConfiguration>(json, _jsonOptions)
                   ?? new FeatureFlagConfiguration();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task SaveAsync(FeatureFlagConfiguration configuration)
    {
        await _lock.WaitAsync();
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(configuration, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _lock.Release();
        }
    }
}
