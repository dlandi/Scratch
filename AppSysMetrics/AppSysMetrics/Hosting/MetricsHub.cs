using AppSysMetrics.Models;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class MetricsHub
{
    private readonly object _lock = new();
    private readonly List<MetricsSnapshot> _history = [];
    private readonly int _maxHistory;

    public MetricsHub(IOptions<MetricsCollectionOptions> options)
    {
        _maxHistory = options.Value.MaxHistorySize;
    }

    public event Action<MetricsSnapshot>? OnSnapshot;

    public MetricsSnapshot? Latest { get; private set; }

    public IReadOnlyList<MetricsSnapshot> GetHistory()
    {
        lock (_lock)
        {
            return _history.ToList();
        }
    }

    internal void Publish(MetricsSnapshot snapshot)
    {
        lock (_lock)
        {
            _history.Add(snapshot);
            if (_history.Count > _maxHistory)
                _history.RemoveAt(0);
            Latest = snapshot;
        }
        OnSnapshot?.Invoke(snapshot);
    }
}
