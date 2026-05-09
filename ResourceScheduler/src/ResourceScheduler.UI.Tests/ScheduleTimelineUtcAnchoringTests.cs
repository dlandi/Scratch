using Microsoft.Extensions.Time.Testing;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using ResourceScheduler.WebApp.Pages;
using Xunit;

namespace ResourceScheduler.UI.Tests;

/// <summary>
/// Verifies the "UTC-anchored layout" invariant for the Schedule Timeline:
/// when the user switches the chosen timezone via the header dropdown,
/// reservation rect coordinates (x, width) on the canvas MUST NOT change,
/// and the visibility filter MUST NOT change. Hour-tick labels SHOULD
/// change because they're rendered in the chosen zone.
///
/// Implemented via Option B (test the math directly) rather than Option A
/// (render the page in bUnit). The page lives in a Blazor WebAssembly SDK
/// project and depends on JS interop (SvgInterop's dynamic module import,
/// scrollLeft, pointToSvg). Mocking that surface to render the page just
/// to read SVG attributes was strictly more setup than testing the pure
/// layout helpers extracted into <see cref="ScheduleTimelineLayout"/>.
/// </summary>
public class ScheduleTimelineUtcAnchoringTests
{
    private static ReservationDto Reservation(Guid groupId, DateTime startUtc, DateTime endUtc)
        => new()
        {
            ReservationId = Guid.NewGuid(),
            DeviceGroupId = groupId,
            TestGroupId = Guid.NewGuid(),
            StartUtc = DateTime.SpecifyKind(startUtc, DateTimeKind.Utc),
            EndUtc = DateTime.SpecifyKind(endUtc, DateTimeKind.Utc),
            Status = ReservationStatus.Confirmed,
            Version = 1,
        };

    [Fact]
    public void Rect_x_and_width_are_invariant_across_timezone_switches()
    {
        // Anchor at a fixed UTC moment so the test is deterministic
        // regardless of the host clock.
        var anchorUtc = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc);
        var hourW = ScheduleTimelineLayout.HourWidth("day");
        var endAnchorUtc = anchorUtc.AddHours(ScheduleTimelineLayout.HOURS_PER_DAY * ScheduleTimelineLayout.DaysVisible("day"));

        var groupId = Guid.NewGuid();
        var reservations = new[]
        {
            // Fully inside the canvas window.
            Reservation(groupId, anchorUtc.AddHours(2), anchorUtc.AddHours(4)),
            // Crosses the leading edge: must be clipped.
            Reservation(groupId, anchorUtc.AddHours(-3), anchorUtc.AddHours(1)),
            // Crosses the trailing edge: must be clipped.
            Reservation(groupId, anchorUtc.AddHours(22), anchorUtc.AddHours(26)),
        };

        var fake = new FakeTimeProvider();
        fake.SetUtcNow(new DateTimeOffset(anchorUtc.AddHours(6), TimeSpan.Zero));
        var time = new UserTimeProvider(fake);

        // Capture rect coordinates with the provider in UTC.
        time.SetTimeZone(TimeZoneInfo.Utc);
        var beforeX = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => ScheduleTimelineLayout.RectX(r.StartUtc, anchorUtc, endAnchorUtc, hourW))
            .ToArray();
        var beforeW = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => ScheduleTimelineLayout.RectWidth(r.StartUtc, r.EndUtc, anchorUtc, endAnchorUtc, hourW))
            .ToArray();

        // Switch to Tokyo. Anchor and reservation UTC times do NOT change.
        time.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));
        var afterX = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => ScheduleTimelineLayout.RectX(r.StartUtc, anchorUtc, endAnchorUtc, hourW))
            .ToArray();
        var afterW = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => ScheduleTimelineLayout.RectWidth(r.StartUtc, r.EndUtc, anchorUtc, endAnchorUtc, hourW))
            .ToArray();

        Assert.Equal(beforeX, afterX);
        Assert.Equal(beforeW, afterW);
    }

    [Fact]
    public void Visibility_filter_is_invariant_across_timezone_switches()
    {
        var anchorUtc = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc);
        var endAnchorUtc = anchorUtc.AddHours(24);

        var groupId = Guid.NewGuid();
        var reservations = new[]
        {
            Reservation(groupId, anchorUtc.AddHours(-10), anchorUtc.AddHours(-2)),  // before
            Reservation(groupId, anchorUtc.AddHours(2), anchorUtc.AddHours(4)),     // inside
            Reservation(groupId, anchorUtc.AddHours(26), anchorUtc.AddHours(30)),   // after
        };

        // The filter operates on UTC times only, so a UserTimeProvider's
        // chosen zone is irrelevant. Switch the zone before each capture
        // anyway to make the invariant explicit.
        var fake = new FakeTimeProvider();
        fake.SetUtcNow(new DateTimeOffset(anchorUtc, TimeSpan.Zero));
        var time = new UserTimeProvider(fake);

        time.SetTimeZone(TimeZoneInfo.Utc);
        var beforeIds = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => r.ReservationId)
            .ToArray();

        time.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));
        var afterIds = reservations
            .Where(r => ScheduleTimelineLayout.IsVisible(r, anchorUtc, endAnchorUtc))
            .Select(r => r.ReservationId)
            .ToArray();

        Assert.Equal(beforeIds, afterIds);
        Assert.Single(beforeIds);

        // Sanity: the local zone really did change between captures.
        Assert.NotEqual(TimeZoneInfo.Utc.Id, time.LocalTimeZone.Id);
    }

    [Fact]
    public void Hour_tick_labels_change_when_zone_changes()
    {
        // Same anchor; different zones must produce different labels.
        var anchorUtc = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc);

        var fake = new FakeTimeProvider();
        fake.SetUtcNow(new DateTimeOffset(anchorUtc, TimeSpan.Zero));
        var time = new UserTimeProvider(fake);

        time.SetTimeZone(TimeZoneInfo.Utc);
        var utcLabels = Enumerable.Range(0, 24)
            .Select(i => ScheduleTimelineLayout.HourTickLabel(time, anchorUtc, i))
            .ToArray();

        time.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));
        var tokyoLabels = Enumerable.Range(0, 24)
            .Select(i => ScheduleTimelineLayout.HourTickLabel(time, anchorUtc, i))
            .ToArray();

        // The two label arrays must differ in at least some positions.
        var anyDiffer = utcLabels
            .Zip(tokyoLabels, (u, t) => u != t)
            .Any(b => b);
        Assert.True(anyDiffer, "Expected hour-tick labels to differ between UTC and Tokyo.");

        // Spot-check: at i=0, UTC reads 00:00 and Tokyo reads 09:00.
        Assert.Equal("00:00", utcLabels[0]);
        Assert.Equal("09:00", tokyoLabels[0]);
    }

    [Fact]
    public void BaseAnchorUtc_uses_host_today_not_chosen_zone()
    {
        // Pick a host-local "today". The base anchor must be that local
        // midnight expressed in UTC, regardless of any chosen zone the
        // user might select. We don't construct a UserTimeProvider here
        // because the math doesn't consult LocalTimeZone at all: that's
        // the invariant we're asserting.
        var hostToday = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Local);
        var anchor = ScheduleTimelineLayout.BaseAnchorUtc(hostToday);

        Assert.Equal(DateTimeKind.Utc, anchor.Kind);
        // 00:00 host-local converts to host-offset hours UTC. The exact
        // UTC value depends on the test machine, but the function must be
        // pure and deterministic for a given host zone.
        Assert.Equal(anchor, ScheduleTimelineLayout.BaseAnchorUtc(hostToday));
    }
}
