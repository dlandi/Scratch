using AppSysMetrics.Models;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class AllocationTrackingHub
{
    private readonly object _lock = new();
    private readonly List<AllocationSnapshot> _history = [];
    private readonly int _maxHistory;

    public AllocationTrackingHub(IOptions<MetricsCollectionOptions> options)
    {
        _maxHistory = options.Value.MaxHistorySize;
    }

    public event Action<AllocationSnapshot>? OnSnapshot;

    public AllocationSnapshot? Latest { get; private set; }

    public IReadOnlyList<AllocationSnapshot> GetHistory()
    {
        lock (_lock)
        {
            return _history.ToList();
        }
    }

    internal void Publish(AllocationSnapshot snapshot)
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
