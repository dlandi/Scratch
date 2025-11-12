using System.Diagnostics;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Monitors virtualization performance metrics.
/// </summary>
public class VirtualizationPerformanceMonitor
{
    private readonly List<double> _renderTimes = new();
    private readonly List<double> _scrollFrameTimes = new();
    private Stopwatch _frameStopwatch = new();
    private int _frameCount = 0;
    private DateTime _lastFpsUpdate = DateTime.Now;
    private double _currentFps = 0;

    public double AverageRenderTimeMs => _renderTimes.Any() ? _renderTimes.Average() : 0;
    public double LastRenderTimeMs => _renderTimes.Any() ? _renderTimes.Last() : 0;
    public double AverageScrollFrameMs => _scrollFrameTimes.Any() ? _scrollFrameTimes.Average() : 0;
    public double CurrentFps => _currentFps;
    public int TotalFrames => _frameCount;

    public void StartRender()
    {
        _frameStopwatch = Stopwatch.StartNew();
    }

    public void EndRender()
    {
        _frameStopwatch.Stop();
        _renderTimes.Add(_frameStopwatch.Elapsed.TotalMilliseconds);
        
        // Keep last 100 measurements
        if (_renderTimes.Count > 100)
        {
            _renderTimes.RemoveAt(0);
        }
    }

    public void RecordScrollFrame()
    {
        _frameCount++;
        
        // Update FPS every second
        var now = DateTime.Now;
        var elapsed = (now - _lastFpsUpdate).TotalSeconds;
        
        if (elapsed >= 1.0)
        {
            _currentFps = _frameCount / elapsed;
            _frameCount = 0;
            _lastFpsUpdate = now;
        }
    }

    public void Reset()
    {
        _renderTimes.Clear();
        _scrollFrameTimes.Clear();
        _frameCount = 0;
        _currentFps = 0;
        _lastFpsUpdate = DateTime.Now;
    }
}

/// <summary>
/// Compares performance between virtualized and non-virtualized grids.
/// </summary>
public class VirtualizationComparison
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public int TotalCells => RowCount * ColumnCount;

    // Without virtualization
    public double NonVirtualRenderTimeMs { get; set; }
    public int NonVirtualDomElements { get; set; }
    public long NonVirtualMemoryBytes { get; set; }

    // With virtualization
    public int VisibleRows { get; set; }
    public int BufferRows { get; set; }
    public int TotalRenderedRows => VisibleRows + (BufferRows * 2);
    public double VirtualRenderTimeMs { get; set; }
    public int VirtualDomElements => TotalRenderedRows * ColumnCount;
    public long VirtualMemoryBytes { get; set; }

    // Improvements
    public double RenderTimeSpeedup => NonVirtualRenderTimeMs / VirtualRenderTimeMs;
    public double DomElementReduction => 
        ((NonVirtualDomElements - VirtualDomElements) / (double)NonVirtualDomElements) * 100;
    public double MemoryReduction =>
        ((NonVirtualMemoryBytes - VirtualMemoryBytes) / (double)NonVirtualMemoryBytes) * 100;
}

/// <summary>
/// Estimates virtualization performance for different scenarios.
/// </summary>
public static class VirtualizationEstimator
{
    public static VirtualizationComparison EstimatePerformance(
        int rowCount,
        int columnCount,
        int visibleRows = 20,
        int bufferRows = 5,
        double msPerCell = 0.05)
    {
        var totalCells = rowCount * columnCount;
        var renderedCells = (visibleRows + bufferRows * 2) * columnCount;

        return new VirtualizationComparison
        {
            RowCount = rowCount,
            ColumnCount = columnCount,
            VisibleRows = visibleRows,
            BufferRows = bufferRows,
            
            // Non-virtual estimates
            NonVirtualRenderTimeMs = totalCells * msPerCell,
            NonVirtualDomElements = totalCells,
            NonVirtualMemoryBytes = totalCells * 2048, // ~2KB per element
            
            // Virtual estimates
            VirtualRenderTimeMs = renderedCells * msPerCell,
            VirtualMemoryBytes = renderedCells * 2048
        };
    }

    public static string GetRecommendation(int rowCount)
    {
        return rowCount switch
        {
            < 100 => "❌ Virtualization unnecessary - dataset too small",
            < 500 => "⚠️ Virtualization optional - slight benefit",
            < 2000 => "✅ Virtualization recommended - noticeable improvement",
            _ => "✅✅ Virtualization essential - major performance gain"
        };
    }
}

/// <summary>
/// Tracks viewport information during scrolling.
/// </summary>
public class ViewportInfo
{
    public int FirstVisibleRow { get; set; }
    public int LastVisibleRow { get; set; }
    public int TotalRows { get; set; }
    public double ScrollPercentage { get; set; }
    public int RenderedRowCount { get; set; }

    public string FormatRange()
    {
        if (TotalRows == 0) return "0-0 of 0";
        return $"{FirstVisibleRow + 1}-{LastVisibleRow + 1} of {TotalRows}";
    }
}

/// <summary>
/// Configuration for virtualization scenarios.
/// </summary>
public class VirtualizationScenario
{
    public string Name { get; set; } = string.Empty;
    public int RowCount { get; set; }
    public bool UseVirtualization { get; set; }
    public float ItemSize { get; set; } = 40f;
    public int OverscanCount { get; set; } = 5;
    public string Description { get; set; } = string.Empty;
}
