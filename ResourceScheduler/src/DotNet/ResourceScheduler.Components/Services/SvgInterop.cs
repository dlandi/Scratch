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

    /// <summary>Set <c>scrollLeft</c> on the given element. Used by the
    /// schedule timeline to position the week-view canvas on today's
    /// column after the initial render.</summary>
    public async ValueTask SetScrollLeftAsync(ElementReference el, double x)
    {
        var m = await ModuleAsync();
        await m.InvokeVoidAsync("setScrollLeft", el, x);
    }

    /// <summary>
    /// Scroll the element so that the given x coordinate lands at
    /// <paramref name="fraction"/> of the visible viewport from the left.
    /// Use 0.0 to put x at the left edge, 0.25 to put it a quarter in,
    /// 0.5 to centre it. Internally consults <c>el.clientWidth</c>, which
    /// is the live viewport width including any DPI scaling.
    /// </summary>
    public async ValueTask ScrollToShowAtAsync(ElementReference el, double x, double fraction)
    {
        var m = await ModuleAsync();
        await m.InvokeVoidAsync("scrollToShowAt", el, x, fraction);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try { await _module.DisposeAsync(); } catch (JSDisconnectedException) { }
        }
    }
}
