using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Performance benchmarking utilities for comparing optimized vs naive approaches.
/// </summary>
public static class PerformanceBenchmark
{
    /// <summary>
    /// Benchmarks property access: Reflection vs Compiled Expression vs Direct Access.
    /// </summary>
    public static BenchmarkResult BenchmarkPropertyAccess<T, TValue>(
        T item,
        Expression<Func<T, TValue>> propertyExpression,
        int iterations = 10000)
    {
        var memberExpression = propertyExpression.Body as MemberExpression
            ?? throw new ArgumentException("Expression must be a property access");

        var propertyInfo = memberExpression.Member as PropertyInfo
            ?? throw new ArgumentException("Expression must access a property");

        // Warmup
        for (int i = 0; i < 100; i++)
        {
            _ = propertyInfo.GetValue(item);
            _ = propertyExpression.Compile()(item);
        }

        // Benchmark Reflection
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            _ = propertyInfo.GetValue(item);
        }
        sw.Stop();
        var reflectionTime = sw.Elapsed.TotalMilliseconds;

        // Benchmark Compiled Expression
        var compiled = propertyExpression.Compile();
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            _ = compiled(item);
        }
        sw.Stop();
        var compiledTime = sw.Elapsed.TotalMilliseconds;

        return new BenchmarkResult
        {
            ReflectionTimeMs = reflectionTime,
            CompiledTimeMs = compiledTime,
            Iterations = iterations,
            SpeedupFactor = reflectionTime / compiledTime
        };
    }

    /// <summary>
    /// Benchmarks class string computation: Cached vs Uncached.
    /// </summary>
    public static CacheBenchmarkResult BenchmarkClassCaching(
        bool[] highlightStates,
        bool[] warningStates,
        bool[] errorStates,
        int iterations = 10000)
    {
        var cache = new Dictionary<(bool, bool, bool), string>();
        var random = new Random(42);

        // Warmup
        for (int i = 0; i < 100; i++)
        {
            var h = highlightStates[i % highlightStates.Length];
            var w = warningStates[i % warningStates.Length];
            var e = errorStates[i % errorStates.Length];
            _ = BuildClassStringUncached(h, w, e);
            _ = BuildClassStringCached(h, w, e, cache);
        }

        // Benchmark Uncached (allocates every time)
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            var h = highlightStates[i % highlightStates.Length];
            var w = warningStates[i % warningStates.Length];
            var e = errorStates[i % errorStates.Length];
            _ = BuildClassStringUncached(h, w, e);
        }
        sw.Stop();
        var uncachedTime = sw.Elapsed.TotalMilliseconds;

        // Benchmark Cached
        cache.Clear();
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            var h = highlightStates[i % highlightStates.Length];
            var w = warningStates[i % warningStates.Length];
            var e = errorStates[i % errorStates.Length];
            _ = BuildClassStringCached(h, w, e, cache);
        }
        sw.Stop();
        var cachedTime = sw.Elapsed.TotalMilliseconds;

        // Measure allocations (approximate)
        var beforeGC = GC.GetTotalMemory(true);
        for (int i = 0; i < iterations; i++)
        {
            var h = highlightStates[i % highlightStates.Length];
            var w = warningStates[i % warningStates.Length];
            var e = errorStates[i % errorStates.Length];
            _ = BuildClassStringUncached(h, w, e);
        }
        var afterGC = GC.GetTotalMemory(false);
        var uncachedAllocations = afterGC - beforeGC;

        return new CacheBenchmarkResult
        {
            UncachedTimeMs = uncachedTime,
            CachedTimeMs = cachedTime,
            Iterations = iterations,
            SpeedupFactor = uncachedTime / cachedTime,
            CacheSize = cache.Count,
            MaxCacheSize = 8, // 2^3 for 3 boolean states
            ApproximateAllocationBytes = uncachedAllocations
        };
    }

    private static string BuildClassStringUncached(bool highlight, bool warning, bool error)
    {
        var css = "cell-value";
        if (highlight) css += " cell-highlight";
        if (warning) css += " cell-warning";
        if (error) css += " cell-error";
        return css;
    }

    private static string BuildClassStringCached(
        bool highlight, 
        bool warning, 
        bool error,
        Dictionary<(bool, bool, bool), string> cache)
    {
        var key = (highlight, warning, error);
        if (!cache.TryGetValue(key, out var css))
        {
            css = "cell-value";
            if (highlight) css += " cell-highlight";
            if (warning) css += " cell-warning";
            if (error) css += " cell-error";
            cache[key] = css;
        }
        return css;
    }

    /// <summary>
    /// Measures DOM element count difference.
    /// </summary>
    public static DomCountResult CompareDomElements(int cellCount)
    {
        // Optimized: 1 element per cell
        var optimizedElements = cellCount * 1;

        // Naive: 4 elements per cell
        var naiveElements = cellCount * 4;

        return new DomCountResult
        {
            CellCount = cellCount,
            OptimizedElements = optimizedElements,
            NaiveElements = naiveElements,
            ExtraElements = naiveElements - optimizedElements,
            ReductionPercent = ((naiveElements - optimizedElements) / (double)naiveElements) * 100
        };
    }

    /// <summary>
    /// Estimates full grid render performance difference.
    /// </summary>
    public static GridRenderEstimate EstimateGridRenderPerformance(
        int rows,
        int columns,
        double reflectionTimePerCellNs = 300,
        double compiledTimePerCellNs = 15,
        double uncachedClassTimeNs = 50,
        double cachedClassTimeNs = 5,
        double domElementTimeNs = 100)
    {
        var totalCells = rows * columns;

        // Naive approach
        var naivePropertyTime = (totalCells * reflectionTimePerCellNs) / 1_000_000; // Convert to ms
        var naiveClassTime = (totalCells * uncachedClassTimeNs) / 1_000_000;
        var naiveDomTime = (totalCells * 4 * domElementTimeNs) / 1_000_000; // 4 elements per cell
        var naiveTotalMs = naivePropertyTime + naiveClassTime + naiveDomTime;

        // Optimized approach
        var optimizedPropertyTime = (totalCells * compiledTimePerCellNs) / 1_000_000;
        var optimizedClassTime = (totalCells * cachedClassTimeNs) / 1_000_000;
        var optimizedDomTime = (totalCells * 1 * domElementTimeNs) / 1_000_000; // 1 element per cell
        var optimizedTotalMs = optimizedPropertyTime + optimizedClassTime + optimizedDomTime;

        return new GridRenderEstimate
        {
            Rows = rows,
            Columns = columns,
            TotalCells = totalCells,
            NaiveRenderTimeMs = naiveTotalMs,
            OptimizedRenderTimeMs = optimizedTotalMs,
            TimeSavingsMs = naiveTotalMs - optimizedTotalMs,
            SpeedupFactor = naiveTotalMs / optimizedTotalMs
        };
    }
}

public class BenchmarkResult
{
    public double ReflectionTimeMs { get; set; }
    public double CompiledTimeMs { get; set; }
    public int Iterations { get; set; }
    public double SpeedupFactor { get; set; }
    public double ReflectionNsPerCall => (ReflectionTimeMs * 1_000_000) / Iterations;
    public double CompiledNsPerCall => (CompiledTimeMs * 1_000_000) / Iterations;
}

public class CacheBenchmarkResult
{
    public double UncachedTimeMs { get; set; }
    public double CachedTimeMs { get; set; }
    public int Iterations { get; set; }
    public double SpeedupFactor { get; set; }
    public int CacheSize { get; set; }
    public int MaxCacheSize { get; set; }
    public long ApproximateAllocationBytes { get; set; }
}

public class DomCountResult
{
    public int CellCount { get; set; }
    public int OptimizedElements { get; set; }
    public int NaiveElements { get; set; }
    public int ExtraElements { get; set; }
    public double ReductionPercent { get; set; }
}

public class GridRenderEstimate
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int TotalCells { get; set; }
    public double NaiveRenderTimeMs { get; set; }
    public double OptimizedRenderTimeMs { get; set; }
    public double TimeSavingsMs { get; set; }
    public double SpeedupFactor { get; set; }
}
