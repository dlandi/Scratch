using AppSysMetrics.LeakLab.Simulators.Helpers;
using Microsoft.JSInterop;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S01 — DotNetObjectReference&lt;T&gt; not disposed.
/// Creates <see cref="DotNetObjectRefTarget"/> instances wrapped in
/// <see cref="DotNetObjectReference{T}"/>. The DotNetObjectReference holds
/// a strong internal reference that prevents GC of the target, even if
/// no other reference exists. The wrapper works standalone without IJSRuntime.
/// </summary>
public sealed class S01_DotNetObjectRefSimulator : LeakSimulatorBase
{
    private readonly List<DotNetObjectReference<DotNetObjectRefTarget>> _leakedRefs = [];

    public override string ScenarioId => "S01";

    public override string Description =>
        "DotNetObjectReference<T> not disposed — strong internal reference prevents GC";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["AppSysMetrics.LeakLab.Simulators.Helpers.DotNetObjectRefTarget", "System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        for (var i = 0; i < 300 && !ct.IsCancellationRequested; i++)
        {
            var target = new DotNetObjectRefTarget(10_000); // 10KB payload per target
            var objRef = DotNetObjectReference.Create(target);
            _leakedRefs.Add(objRef);
            await Task.Delay(5, ct);
        }
    }

    public override void Reset()
    {
        foreach (var objRef in _leakedRefs)
            objRef.Dispose();
        _leakedRefs.Clear();
    }
}
