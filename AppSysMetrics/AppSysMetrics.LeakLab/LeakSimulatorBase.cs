namespace AppSysMetrics.LeakLab;

/// <summary>
/// Abstract base class providing lifecycle management for leak simulators.
/// Handles <see cref="CancellationTokenSource"/> creation, linking, and disposal.
/// Subclasses override <see cref="OnStartAsync"/> and optionally <see cref="OnStopAsync"/>.
/// </summary>
public abstract class LeakSimulatorBase : ILeakSimulator
{
    private volatile bool _isRunning;
    private CancellationTokenSource? _cts;

    public abstract string ScenarioId { get; }
    public abstract string Description { get; }
    public abstract IReadOnlyList<string> ExpectedLeakTypes { get; }

    public bool IsRunning => _isRunning;

    /// <summary>Token that is cancelled when <see cref="StopAsync"/> is called.</summary>
    protected CancellationToken StoppingToken => _cts?.Token ?? CancellationToken.None;

    public async Task StartAsync(CancellationToken ct = default)
    {
        if (_isRunning) return;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _isRunning = true;
        await OnStartAsync(_cts.Token);
    }

    public async Task StopAsync(CancellationToken ct = default)
    {
        if (!_isRunning) return;

        try { _cts?.Cancel(); }
        catch (ObjectDisposedException) { }

        _isRunning = false;
        await OnStopAsync(ct);
    }

    /// <summary>Override to release all retained references.</summary>
    public virtual void Reset() { }

    /// <summary>
    /// Perform leak-producing allocations. For batch simulators, allocate everything
    /// before returning. For continuous simulators, start a background task and return immediately.
    /// </summary>
    protected abstract Task OnStartAsync(CancellationToken ct);

    /// <summary>Override for cleanup when stopping (e.g. awaiting background tasks).</summary>
    protected virtual Task OnStopAsync(CancellationToken ct) => Task.CompletedTask;

    public void Dispose()
    {
        if (_isRunning)
            StopAsync().GetAwaiter().GetResult();
        _cts?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isRunning)
            await StopAsync();
        _cts?.Dispose();
        GC.SuppressFinalize(this);
    }
}
