using System.Diagnostics;
using AppSysMetrics.Collection;
using AppSysMetrics.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

public sealed class DiagnosticsService : IDiagnosticsService
{
    private readonly IMetricsCollector _collector;
    private readonly ClrMdHeapAnalyzer _heapAnalyzer;
    private readonly GcRootAnalyzer _rootAnalyzer;
    private readonly DumpAnalysisHub _hub;
    private readonly DumpDiffService _diffService;
    private readonly AllocationEventListener _allocationListener;
    private readonly DiagnosticsOptions _options;
    private readonly ILogger<DiagnosticsService> _logger;

    public DiagnosticsService(
        IMetricsCollector collector,
        ClrMdHeapAnalyzer heapAnalyzer,
        GcRootAnalyzer rootAnalyzer,
        DumpAnalysisHub hub,
        DumpDiffService diffService,
        AllocationEventListener allocationListener,
        IOptions<DiagnosticsOptions> options,
        ILogger<DiagnosticsService> logger)
    {
        _collector = collector;
        _heapAnalyzer = heapAnalyzer;
        _rootAnalyzer = rootAnalyzer;
        _hub = hub;
        _diffService = diffService;
        _allocationListener = allocationListener;
        _options = options.Value;
        _logger = logger;
    }

    public ForceGcResult ForceGC()
    {
        _logger.LogInformation("Force GC requested");

        var beforeSnapshot = _collector.Collect();
        var sw = Stopwatch.StartNew();

        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);

        sw.Stop();
        var afterSnapshot = _collector.Collect();

        _logger.LogInformation(
            "Force GC completed in {Duration}ms. Heap before: {Before}, after: {After}",
            sw.ElapsedMilliseconds,
            beforeSnapshot.Gc.HeapSizeBytes,
            afterSnapshot.Gc.HeapSizeBytes);

        return new ForceGcResult
        {
            Before = beforeSnapshot.Gc,
            After = afterSnapshot.Gc,
            Duration = sw.Elapsed,
            PerformedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<GcDumpResult> CaptureGcDumpAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Predict leak-suspect types from previous diff (if available)
            var rootTargets = PredictLeakSuspectTypes();

            // 2. Capture in-process heap snapshot via ClrMD, with optional root analysis
            var result = await _heapAnalyzer.CaptureAndAnalyzeAsync(rootTargets, cancellationToken);
            if (result is null)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = "Heap snapshot failed. Check logs for details.",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            // 3. Enrich with allocation snapshot for correlation
            var allocSnapshot = _allocationListener.CreateSnapshot();
            result = result with { AllocationAtCapture = allocSnapshot };

            // 4. Capture previous result before publishing (for diff)
            var previous = _hub.Latest;

            // 5. Publish to hub — notifies all UI panels
            _hub.Publish(result);

            // 6. Auto-diff if we have a previous result
            if (previous is not null)
            {
                var diff = _diffService.ComputeDiff(previous, result);
                _hub.PublishDiff(diff);
                _logger.LogInformation(
                    "Auto-diff computed: heap delta {HeapDelta}, {TypeCount} type diffs",
                    diff.TotalHeapDelta,
                    diff.TypeDiffs.Count);
            }

            return new GcDumpResult
            {
                Success = true,
                CapturedAt = result.CapturedAtUtc
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture heap snapshot");
            return new GcDumpResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
    }

    /// <summary>
    /// Predicts which types are likely leak suspects for the next capture
    /// by examining the previous diff's high-retention types.
    /// Returns null when no previous diff exists (first two captures).
    ///
    /// Uses a two-track approach:
    ///   Track 1 — High retention: RetentionRatio ≥ 80%. Catches pure leaks where most
    ///             allocations are retained.
    ///   Track 2 — Large absolute growth: DeltaSizeBytes is significant (≥ 20% of total
    ///             heap growth or ≥ 1 MB absolute), with positive retention and throughput.
    ///             Catches diluted leaks where a type has high framework throughput
    ///             (e.g. Byte[] shared by Kestrel pooling and user code) masking the
    ///             retention ratio, but the absolute heap growth is clearly abnormal.
    /// </summary>
    private IReadOnlyList<string>? PredictLeakSuspectTypes()
    {
        var latestDiff = _hub.LatestDiff;
        if (latestDiff is null)
            return null;

        var eligible = latestDiff.TypeDiffs
            .Where(t => t.AllocatedBetweenBytes is > 0 && t.DeltaSizeBytes > 0)
            .Where(t => !IsFrameworkOnlyType(t.TypeName))
            .ToList();

        // Track 1: High retention ratio (≥ 80%)
        var highRetention = eligible
            .Where(t => t.RetentionRatio >= 0.8);

        // Track 2: Large absolute heap growth with positive retention
        // Catches types like Byte[] where framework throughput dilutes the ratio,
        // but the absolute growth is clearly significant.
        var totalHeapDelta = Math.Max(1, latestDiff.TotalHeapDelta); // floor to avoid div-by-zero
        const long absoluteGrowthFloor = 1_048_576; // 1 MB
        const double heapShareThreshold = 0.20;     // 20% of total heap growth

        var largeAbsoluteGrowth = eligible
            .Where(t => t.RetentionRatio is > 0 and < 0.8) // Not already caught by Track 1
            .Where(t => t.DeltaSizeBytes >= absoluteGrowthFloor
                     || (totalHeapDelta > 0
                         && (double)t.DeltaSizeBytes / totalHeapDelta >= heapShareThreshold));

        // Union both tracks, deduplicate, sort, cap at 5
        var suspects = highRetention
            .Concat(largeAbsoluteGrowth)
            .DistinctBy(t => t.TypeName)
            .OrderByDescending(t => t.RetentionRatio)
            .ThenByDescending(t => t.DeltaSizeBytes)
            .Take(5)
            .Select(t => t.TypeName)
            .ToList();

        if (suspects.Count > 0)
        {
            _logger.LogInformation(
                "Predicted {Count} leak-suspect types for root analysis: {Types}",
                suspects.Count,
                string.Join(", ", suspects));
        }

        return suspects.Count > 0 ? suspects : null;
    }

    /// <summary>
    /// Returns true if a type is definitely framework-only — not user code and not a
    /// System.* container type. Framework implementation types (Microsoft.*, Internal.*, etc.)
    /// are filtered out of leak-suspect prediction to avoid noise in root analysis.
    /// System.* types (Byte[], String, List&lt;T&gt;, etc.) pass through because root analysis
    /// traces their retention paths back to user code.
    ///
    /// Dot-free type names (no namespace) are only allowed through if they match well-known
    /// primitive/array patterns (e.g. "Byte[]", "String", "Int32[]"). Other dot-free types
    /// like "ClrDacType" are framework internals that leaked into the heap without namespaces.
    /// </summary>
    private bool IsFrameworkOnlyType(string typeName)
    {
        if (_rootAnalyzer.IsUserCode(typeName))
            return false;

        if (typeName.StartsWith("System.", StringComparison.Ordinal))
            return false;

        // Dot-free type names: only allow well-known primitives and arrays through.
        // Framework internal types often appear without namespaces on the heap
        // (e.g. ClrDacType from Microsoft.Diagnostics.Runtime) — treat those as framework.
        if (!typeName.Contains('.'))
            return !IsWellKnownContainerType(typeName);

        // Developer-facing framework types pass through — their growth
        // signals developer misconfiguration (unbounded cache, long-lived DbContext, etc.)
        if (IsDeveloperFacingFrameworkType(typeName))
            return false;

        return true;
    }

    /// <summary>
    /// Returns true if a type belongs to a Microsoft.* namespace that represents
    /// developer-controlled infrastructure. Growth in these types is actionable —
    /// e.g. CacheEntry accumulation means an unbounded IMemoryCache, EntityEntry
    /// growth means a long-lived DbContext with tracking enabled.
    /// </summary>
    private static bool IsDeveloperFacingFrameworkType(string typeName)
    {
        return typeName.StartsWith("Microsoft.Extensions.Caching.", StringComparison.Ordinal)
            || typeName.StartsWith("Microsoft.EntityFrameworkCore.", StringComparison.Ordinal)
            || typeName.StartsWith("Microsoft.AspNetCore.SignalR.", StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns true if a dot-free type name is a well-known primitive, array, or collection
    /// type that could legitimately be a leak suspect (e.g. "Byte[]", "String", "Object[]").
    /// These are the types that root analysis can trace back to user code via retention paths.
    /// </summary>
    private static bool IsWellKnownContainerType(string typeName)
    {
        // Strip trailing "[]" to normalize array types: "Byte[]" → "Byte", "Byte[][]" → "Byte[]"
        var baseName = typeName;
        while (baseName.EndsWith("[]", StringComparison.Ordinal))
            baseName = baseName[..^2];

        return baseName is "Byte" or "String" or "Char" or "Int32" or "Int64"
            or "UInt32" or "UInt64" or "Int16" or "UInt16" or "Double" or "Single"
            or "Boolean" or "Object" or "IntPtr" or "UIntPtr" or "Decimal"
            or "SByte" or "Guid" or "DateTime" or "DateTimeOffset" or "TimeSpan";
    }

    public async Task<GcDumpResult> CaptureGcDumpFileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var processId = Environment.ProcessId;
            var outputDir = _options.GcDumpOutputDirectory
                ?? Path.Combine(Path.GetTempPath(), "AppSysMetrics", "gcdumps");

            Directory.CreateDirectory(outputDir);

            var fileName = $"gcdump_{processId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.gcdump";
            var filePath = Path.Combine(outputDir, fileName);

            _logger.LogInformation("Capturing GC dump for PID {Pid} to {Path}", processId, filePath);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet-gcdump",
                Arguments = $"collect -p {processId} -o \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            if (proc is null)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = "Failed to start dotnet-gcdump. Install with: dotnet tool install -g dotnet-gcdump",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            await proc.WaitForExitAsync(cancellationToken);
            var stderr = await proc.StandardError.ReadToEndAsync(cancellationToken);

            if (proc.ExitCode != 0)
            {
                return new GcDumpResult
                {
                    Success = false,
                    ErrorMessage = $"dotnet-gcdump exited with code {proc.ExitCode}: {stderr}",
                    CapturedAt = DateTimeOffset.UtcNow
                };
            }

            var fileInfo = new FileInfo(filePath);
            return new GcDumpResult
            {
                Success = true,
                FilePath = filePath,
                FileSizeBytes = fileInfo.Exists ? fileInfo.Length : 0,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture GC dump file");
            return new GcDumpResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                CapturedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
