using Microsoft.Extensions.Time.Testing;
using ResourceScheduler.Components.Services;
using Xunit;

namespace ResourceScheduler.Tests;

public class UserTimeProviderTests
{
    [Fact]
    public void Default_LocalTimeZone_equals_TimeZoneInfo_Local()
    {
        var provider = new UserTimeProvider();

        Assert.Equal(TimeZoneInfo.Local.Id, provider.LocalTimeZone.Id);
    }

    [Fact]
    public void SetTimeZone_changes_LocalTimeZone_and_raises_Changed_once()
    {
        var provider = new UserTimeProvider();
        var counter = 0;
        provider.Changed += () => counter++;

        provider.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));

        Assert.Equal("Asia/Tokyo", provider.LocalTimeZone.Id);
        Assert.Equal(1, counter);
    }

    [Fact]
    public void SetTimeZone_with_same_id_is_idempotent()
    {
        var provider = new UserTimeProvider();
        var tokyo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        provider.SetTimeZone(tokyo);

        var counter = 0;
        provider.Changed += () => counter++;

        provider.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));

        Assert.Equal("Asia/Tokyo", provider.LocalTimeZone.Id);
        Assert.Equal(0, counter);
    }

    [Fact]
    public void SetTimeZoneById_picks_up_known_id_and_ignores_unknown()
    {
        var provider = new UserTimeProvider();

        provider.SetTimeZoneById("Asia/Tokyo");
        Assert.Equal("Asia/Tokyo", provider.LocalTimeZone.Id);

        // Unknown id: must not throw and must not change the zone.
        provider.SetTimeZoneById("Not/A/Zone");
        Assert.Equal("Asia/Tokyo", provider.LocalTimeZone.Id);
    }

    [Fact]
    public void GetLocalNow_uses_overridden_zone()
    {
        var fake = new FakeTimeProvider();
        // Pick a UTC moment. 2026-05-09 00:00:00 UTC.
        fake.SetUtcNow(new DateTimeOffset(2026, 5, 9, 0, 0, 0, TimeSpan.Zero));

        var provider = new UserTimeProvider(fake);

        provider.SetTimeZone(TimeZoneInfo.Utc);
        Assert.Equal(TimeSpan.Zero, provider.GetLocalNow().Offset);

        provider.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));
        Assert.Equal(TimeSpan.FromHours(9), provider.GetLocalNow().Offset);
    }

    [Fact]
    public void ToLocal_extension_uses_provider_LocalTimeZone_not_OS()
    {
        var fake = new FakeTimeProvider();
        fake.SetUtcNow(new DateTimeOffset(2026, 5, 9, 0, 0, 0, TimeSpan.Zero));
        var provider = new UserTimeProvider(fake);
        provider.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));

        // 2026-05-09 03:15 UTC == 2026-05-09 12:15 in Tokyo (UTC+9).
        var utc = new DateTime(2026, 5, 9, 3, 15, 0, DateTimeKind.Utc);
        var local = provider.ToLocal(utc);

        Assert.Equal(12, local.Hour);
        Assert.Equal(15, local.Minute);
    }
}
