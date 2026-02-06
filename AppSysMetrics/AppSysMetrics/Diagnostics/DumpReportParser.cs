using AppSysMetrics.Diagnostics.Models;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Parses the fixed-width text output of <c>dotnet-gcdump report</c> into structured models.
/// This is a pure static class with no dependencies — designed for testability.
/// </summary>
public static class DumpReportParser
{
    /// <summary>
    /// Parses the stdout of <c>dotnet-gcdump report {file}</c>.
    /// </summary>
    public static DumpAnalysisResult Parse(
        string reportOutput,
        string filePath,
        long fileSizeBytes,
        DateTimeOffset capturedAtUtc,
        int topTypesCount = 50)
    {
        long totalHeapBytes = 0;
        long totalObjectCount = 0;
        var types = new List<HeapTypeInfo>();
        var inDataRows = false;

        var lines = reportOutput.Split('\n');

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd('\r');

            // ── Summary lines ──
            // Format: "  1,603,588,000  GC Heap bytes"
            if (line.TrimEnd().EndsWith("GC Heap bytes", StringComparison.OrdinalIgnoreCase))
            {
                totalHeapBytes = ParseLeadingNumber(line);
                continue;
            }

            // Format: "     22,000,000  GC Heap objects"
            if (line.TrimEnd().EndsWith("GC Heap objects", StringComparison.OrdinalIgnoreCase))
            {
                totalObjectCount = ParseLeadingNumber(line);
                continue;
            }

            // ── Header detection ──
            // Format: "    Object Bytes    Count  Type"
            if (line.Contains("Object Bytes") && line.Contains("Count") && line.Contains("Type"))
            {
                inDataRows = true;
                continue;
            }

            // ── Data rows ──
            // Format: "  1,603,588,000   22,000  System.String"
            // Positions: [0..15] = size (15 chars), [15..23] = count (8 chars), [25..] = type name
            if (!inDataRows)
                continue;

            if (line.Length < 25)
                continue;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var sizeStr = line[..15].Trim().Replace(",", "");
            var countStr = line[15..23].Trim().Replace(",", "");

            if (!long.TryParse(sizeStr, out var sizeBytes))
                continue;

            if (!long.TryParse(countStr, out var count))
                continue;

            var typeName = line[25..].Trim();

            // Strip assembly suffix: "System.String [System.Private.CoreLib.dll]" → "System.String"
            typeName = StripAssemblySuffix(typeName);

            if (string.IsNullOrEmpty(typeName))
                continue;

            types.Add(new HeapTypeInfo
            {
                TypeName = typeName,
                InstanceCount = count,
                TotalSizeBytes = sizeBytes
            });
        }

        // Sort by size descending, take top N
        var topTypes = types
            .OrderByDescending(t => t.TotalSizeBytes)
            .Take(topTypesCount)
            .ToList();

        return new DumpAnalysisResult
        {
            FilePath = filePath,
            FileName = Path.GetFileName(filePath),
            CapturedAtUtc = capturedAtUtc,
            AnalyzedAtUtc = DateTimeOffset.UtcNow,
            FileSizeBytes = fileSizeBytes,
            TotalHeapBytes = totalHeapBytes,
            TotalObjectCount = totalObjectCount,
            TopTypes = topTypes
        };
    }

    private static long ParseLeadingNumber(string line)
    {
        // Extract the numeric portion before the text label
        var trimmed = line.TrimStart();
        var spaceIdx = trimmed.IndexOf(' ');
        if (spaceIdx <= 0)
            return 0;

        var numStr = trimmed[..spaceIdx].Replace(",", "");
        return long.TryParse(numStr, out var value) ? value : 0;
    }

    private static string StripAssemblySuffix(string typeName)
    {
        // Pattern: "TypeName [Assembly.dll]" → "TypeName"
        var bracketIdx = typeName.LastIndexOf(" [", StringComparison.Ordinal);
        if (bracketIdx > 0 && typeName.EndsWith(']'))
        {
            return typeName[..bracketIdx].TrimEnd();
        }

        return typeName;
    }
}
