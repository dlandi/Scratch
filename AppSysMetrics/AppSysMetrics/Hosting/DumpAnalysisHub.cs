using AppSysMetrics.Diagnostics;
using AppSysMetrics.Diagnostics.Models;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Hosting;

public sealed class DumpAnalysisHub
{
    private readonly object _lock = new();
    private readonly List<DumpAnalysisResult> _history = [];
    private readonly int _maxHistory;

    public DumpAnalysisHub(IOptions<DumpAnalyzerOptions> options)
    {
        _maxHistory = options.Value.MaxAnalysisHistory;
    }

    public event Action<DumpAnalysisResult>? OnAnalysis;
    public event Action<DumpDiffResult>? OnDiff;

    public DumpAnalysisResult? Latest { get; private set; }
    public DumpDiffResult? LatestDiff { get; private set; }

    public IReadOnlyList<DumpAnalysisResult> GetHistory()
    {
        lock (_lock)
        {
            return _history.ToList();
        }
    }

    internal void Publish(DumpAnalysisResult result)
    {
        lock (_lock)
        {
            _history.Add(result);
            if (_history.Count > _maxHistory)
                _history.RemoveAt(0);
            Latest = result;
        }
        OnAnalysis?.Invoke(result);
    }

    internal void PublishDiff(DumpDiffResult diff)
    {
        LatestDiff = diff;
        OnDiff?.Invoke(diff);
    }
}
