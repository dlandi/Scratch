using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ResourceScheduler.Components.Services;

/// <summary>
/// Minimal interop helper for SVG coordinate math. Lazy-loads the JS
/// module on first use and disposes it cleanly. Pages should hold one
/// instance per page (instantiated in OnAfterRenderAsync(firstRender)).
/// </summary>
public sealed class SvgInterop : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public SvgInterop(IJSRuntime js) { _js = js; }

    private async Task<IJSObjectReference> ModuleAsync()
        => _module ??= await _js.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ResourceScheduler.Components/js/svg-helpers.js");

    public readonly record struct Point(double X, double Y);

    public async ValueTask<Point> PointToSvgAsync(ElementReference svg, double clientX, double clientY)
    {
        var m = await ModuleAsync();
        return await m.InvokeAsync<Point>("pointToSvg", svg, clientX, clientY);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try { await _module.DisposeAsync(); } catch (JSDisconnectedException) { }
        }
    }
}
