using AppSysMetrics.Models;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class AllocationTrackingHub
{
    private readonly object _lock = new();
    private readonly List<AllocationSnapshot> _history = [];
    private readonly int _maxHistory;
    private IReadOnlyList<AllocationSnapshot> _frozenHistory = [];

    public AllocationTrackingHub(IOptions<MetricsCollectionOptions> options)
    {
        _maxHistory = options.Value.MaxHistorySize;
    }

    public event Action<AllocationSnapshot>? OnSnapshot;

    public AllocationSnapshot? Latest { get; private set; }

    public IReadOnlyList<AllocationSnapshot> GetHistory() => _frozenHistory;

    internal void Publish(AllocationSnapshot snapshot)
    {
        lock (_lock)
        {
            _history.Add(snapshot);
            if (_history.Count > _maxHistory)
                _history.RemoveAt(0);
            Latest = snapshot;
            _frozenHistory = _history.ToList();
        }
        OnSnapshot?.Invoke(snapshot);
    }
}
