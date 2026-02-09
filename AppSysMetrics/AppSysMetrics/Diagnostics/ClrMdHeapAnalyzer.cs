using AppSysMetrics.Diagnostics.Models;
using Microsoft.Diagnostics.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// In-process heap analyzer using ClrMD (<c>Microsoft.Diagnostics.Runtime</c>).
/// Creates a snapshot of the current process via <see cref="DataTarget.CreateSnapshotAndAttach"/>
/// and enumerates heap objects to build type-level aggregates.
///
/// This replaces <c>dotnet-gcdump</c> which suffers from a .NET 8+ EventPipe regression
/// (dotnet/diagnostics #5116) where type names are not re-emitted on subsequent captures.
/// ClrMD reads type metadata directly from CLR internals (method tables, DAC) and is immune
/// to the EventPipe bug.
/// </summary>
public sealed class ClrMdHeapAnalyzer
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly int _topTypesCount;
    private readonly ILogger<ClrMdHeapAnalyzer> _logger;

    public ClrMdHeapAnalyzer(
        IOptions<DumpAnalyzerOptions> options,
        ILogger<ClrMdHeapAnalyzer> logger)
    {
        _topTypesCount = options.Value.TopTypesCount;
        _logger = logger;
    }

    /// <summary>
    /// Captures an in-process heap snapshot and returns structured analysis results.
    /// Serialized via semaphore — only one capture at a time.
    /// </summary>
    public async Task<DumpAnalysisResult?> CaptureAndAnalyzeAsync(
        CancellationToken cancellationToken = default)
    {
        if (!await _gate.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
        {
            _logger.LogWarning("Heap capture skipped — another capture is already in progress");
            return null;
        }

        try
        {
            _logger.LogInformation("Starting in-process heap snapshot via ClrMD");

            var result = await Task.Run(() => CaptureCore(), cancellationToken);

            if (result is not null)
            {
                _logger.LogInformation(
                    "Heap snapshot complete: {HeapSize} heap, {ObjectCount} objects, {TypeCount} types",
                    result.TotalHeapBytes,
                    result.TotalObjectCount,
                    result.TopTypes.Count);
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Heap capture was cancelled");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture heap snapshot via ClrMD");
            return null;
        }
        finally
        {
            _gate.Release();
        }
    }

    private DumpAnalysisResult? CaptureCore()
    {
        var pid = Environment.ProcessId;
        var capturedAt = DateTimeOffset.UtcNow;
        var tag = capturedAt.ToString("yyyyMMdd_HHmmss");

        using var dataTarget = DataTarget.CreateSnapshotAndAttach(pid);
        using var runtime = dataTarget.ClrVersions[0].CreateRuntime();

        var heap = runtime.Heap;

        // Aggregate: type name → (instance count, total size)
        var typeMap = new Dictionary<string, (long Count, long Size)>(4096);
        long totalObjects = 0;
        long totalHeapBytes = 0;

        foreach (var obj in heap.EnumerateObjects())
        {
            if (!obj.IsValid)
                continue;

            var size = (long)obj.Size;
            totalObjects++;
            totalHeapBytes += size;

            var typeName = obj.Type?.Name ?? "UNKNOWN";

            if (typeMap.TryGetValue(typeName, out var agg))
            {
                typeMap[typeName] = (agg.Count + 1, agg.Size + size);
            }
            else
            {
                typeMap[typeName] = (1, size);
            }
        }

        // Build top types list, sorted by size descending
        var topTypes = typeMap
            .Select(kvp => new HeapTypeInfo
            {
                TypeName = kvp.Key,
                InstanceCount = kvp.Value.Count,
                TotalSizeBytes = kvp.Value.Size
            })
            .OrderByDescending(t => t.TotalSizeBytes)
            .Take(_topTypesCount)
            .ToList();

        return new DumpAnalysisResult
        {
            FilePath = $"clrmd://heap_{tag}",
            FileName = $"heap_{tag}",
            CapturedAtUtc = capturedAt,
            AnalyzedAtUtc = DateTimeOffset.UtcNow,
            FileSizeBytes = 0, // No file — in-process snapshot
            TotalHeapBytes = totalHeapBytes,
            TotalObjectCount = totalObjects,
            TopTypes = topTypes,
            UnresolvedTypeCount = 0 // ClrMD always resolves types
        };
    }
}
