using System.Diagnostics;
using AppSysMetrics.Diagnostics.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Shells out to <c>dotnet-gcdump report</c> and parses the output into structured analysis results.
/// </summary>
public sealed class DumpAnalyzerService
{
    private readonly DumpAnalyzerOptions _options;
    private readonly ILogger<DumpAnalyzerService> _logger;

    public DumpAnalyzerService(
        IOptions<DumpAnalyzerOptions> options,
        ILogger<DumpAnalyzerService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<DumpAnalysisResult?> AnalyzeAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Analyzing dump file: {FilePath}", filePath);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet-gcdump",
                Arguments = $"report \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            if (proc is null)
            {
                _logger.LogError(
                    "Failed to start dotnet-gcdump. Install with: dotnet tool install -g dotnet-gcdump");
                return null;
            }

            var stdout = await proc.StandardOutput.ReadToEndAsync(cancellationToken);
            var stderr = await proc.StandardError.ReadToEndAsync(cancellationToken);
            await proc.WaitForExitAsync(cancellationToken);

            if (proc.ExitCode != 0)
            {
                _logger.LogError(
                    "dotnet-gcdump report exited with code {ExitCode}: {StdErr}",
                    proc.ExitCode, stderr);
                return null;
            }

            // Determine capture time: prefer CreationTimeUtc, fall back to LastWriteTimeUtc (Linux)
            var fileInfo = new FileInfo(filePath);
            var capturedAt = fileInfo.CreationTimeUtc;

            // On Linux, CreationTimeUtc may return DateTime.MinValue or epoch if not supported
            if (capturedAt.Year < 2000)
                capturedAt = fileInfo.LastWriteTimeUtc;

            var result = DumpReportParser.Parse(
                stdout,
                filePath,
                fileInfo.Length,
                new DateTimeOffset(capturedAt, TimeSpan.Zero),
                _options.TopTypesCount);

            _logger.LogInformation(
                "Dump analysis complete: {FileName} — {HeapSize} heap, {ObjectCount} objects, {TypeCount} types",
                result.FileName,
                result.TotalHeapBytes,
                result.TotalObjectCount,
                result.TopTypes.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze dump file: {FilePath}", filePath);
            return null;
        }
    }
}
