using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;

namespace ResourceScheduler.WebApp.Pages;

/// <summary>
/// Pure layout math for <c>ScheduleTimeline.razor</c>. Extracted into a
/// static helper so the UTC-anchored layout invariant ("reservation
/// positions don't change when the user switches timezones") can be
/// verified in process without rendering the full page (which depends on
/// JS interop and a registered <c>NavigationManager</c>).
///
/// The page itself still computes these values inline for readability;
/// this file mirrors that math exactly. If the page changes, update this
/// helper in lockstep.
/// </summary>
public static class ScheduleTimelineLayout
{
    public const int HOURS_PER_DAY = 24;

    /// <summary>Pixels per hour, mirroring the page's <c>HourW</c>.</summary>
    public static int HourWidth(string view) => view == "day" ? 90 : 28;

    /// <summary>Days the canvas spans, mirroring the page's <c>DAYS_VISIBLE</c>.</summary>
    public static int DaysVisible(string view) => view == "day" ? 1 : 7;

    /// <summary>
    /// Base anchor: host's local-today midnight expressed in UTC. Note this
    /// is intentionally derived from <see cref="DateTime.Today"/> (the OS
    /// zone), not from any user-chosen zone. That's why switching the
    /// chosen zone in the header dropdown does not move reservations.
    /// </summary>
    public static DateTime BaseAnchorUtc()
        => DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Local).ToUniversalTime();

    /// <summary>Same as <see cref="BaseAnchorUtc()"/> but with an injected
    /// <c>today</c> value, for tests that want a fixed anchor.</summary>
    public static DateTime BaseAnchorUtc(DateTime hostLocalToday)
        => DateTime.SpecifyKind(hostLocalToday, DateTimeKind.Local).ToUniversalTime();

    /// <summary>
    /// X coordinate of a reservation's leading edge on the canvas, clipped
    /// to the visible window. Identical math to <c>ScheduleTimeline.razor</c>.
    /// </summary>
    public static double RectX(DateTime startUtc, DateTime anchorUtc, DateTime endAnchorUtc, int hourW)
    {
        var clipStart = startUtc < anchorUtc ? anchorUtc : startUtc;
        return (clipStart - anchorUtc).TotalHours * hourW;
    }

    /// <summary>Width of a reservation's rect, clipped to the visible window.</summary>
    public static double RectWidth(DateTime startUtc, DateTime endUtc, DateTime anchorUtc, DateTime endAnchorUtc, int hourW)
    {
        var clipStart = startUtc < anchorUtc ? anchorUtc : startUtc;
        var clipEnd = endUtc > endAnchorUtc ? endAnchorUtc : endUtc;
        return (clipEnd - clipStart).TotalHours * hourW;
    }

    /// <summary>The page's UTC-overlap visibility filter.</summary>
    public static bool IsVisible(ReservationDto r, DateTime anchorUtc, DateTime endAnchorUtc)
        => r.StartUtc < endAnchorUtc && r.EndUtc > anchorUtc;

    /// <summary>
    /// Hour-tick label for tick <paramref name="i"/> on the canvas, computed
    /// in the provider's <see cref="TimeProvider.LocalTimeZone"/>. This
    /// SHOULD change when the chosen zone changes; the rect coordinates
    /// should NOT.
    /// </summary>
    public static string HourTickLabel(TimeProvider time, DateTime anchorUtc, int i)
    {
        var tickLocal = time.ToLocal(anchorUtc.AddHours(i));
        return $"{tickLocal.Hour:D2}:00";
    }
}
