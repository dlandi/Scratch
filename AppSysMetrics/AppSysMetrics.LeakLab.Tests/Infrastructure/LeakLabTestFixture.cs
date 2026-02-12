using AppSysMetrics.Diagnostics;
using AppSysMetrics.Extensions;
using AppSysMetrics.Hosting;
using AppSysMetrics.LeakLab.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace AppSysMetrics.LeakLab.Tests.Infrastructure;

/// <summary>
/// Shared xUnit collection fixture that builds a real <see cref="IHost"/>
/// with both AppSysMetrics diagnostics and LeakLab simulators registered.
///
/// Uses a plain generic host (no web hosting) — <c>CaptureGcDumpAsync</c>
/// uses in-process ClrMD, not HTTP endpoints. The host starts
/// <c>AllocationEventListener</c> and <c>MetricsCollectionService</c>
/// as hosted services.
/// </summary>
public sealed class LeakLabTestFixture : IAsyncLifetime
{
    public IHost Host { get; private set; } = null!;
    public IServiceProvider Services => Host.Services;

    public IDiagnosticsService Diagnostics =>
        Services.GetRequiredService<IDiagnosticsService>();

    public DumpAnalysisHub AnalysisHub =>
        Services.GetRequiredService<DumpAnalysisHub>();

    public LeakLabRegistry Registry =>
        Services.GetRequiredService<LeakLabRegistry>();

    public async Task InitializeAsync()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddAppSysMetrics(options =>
                {
                    options.CollectionInterval = TimeSpan.FromSeconds(1);
                    options.MaxHistorySize = 20;
                });
                services.AddLeakLab();
            })
            .Build();

        await Host.StartAsync();

        // Give hosted services a moment to initialize (AllocationEventListener, etc.)
        await Task.Delay(500);

        // Warm-up: take two throwaway captures so that host startup noise
        // (String/Char[] growth from logging, config, DI) drains before
        // the first real test. Without this, S01 (which runs first) would
        // see startup-related types dominate the diff.
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        await Diagnostics.CaptureGcDumpAsync();
        await Task.Delay(1_000);
        await Diagnostics.CaptureGcDumpAsync();
        AnalysisHub.Clear();
    }

    public async Task DisposeAsync()
    {
        await Host.StopAsync();
        Host.Dispose();
    }
}

[CollectionDefinition("LeakLab")]
public class LeakLabCollection : ICollectionFixture<LeakLabTestFixture> { }
