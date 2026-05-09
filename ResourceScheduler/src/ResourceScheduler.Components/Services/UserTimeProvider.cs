namespace ResourceScheduler.Components.Services;

/// <summary>
/// A <see cref="TimeProvider"/> that lets the user override
/// <see cref="LocalTimeZone"/> at runtime via the header dropdown.
/// Backing time still comes from <see cref="TimeProvider.System"/>; only
/// the local-zone projection is configurable. This is intended as a
/// developer/testing aid for visualising how schedules render in other
/// zones, controlled by the <c>Features:TimeZoneSwitcher:Enabled</c>
/// flag in <c>appsettings.json</c>.
/// </summary>
public sealed class UserTimeProvider : TimeProvider
{
    private TimeZoneInfo _zone = TimeZoneInfo.Local;

    /// <summary>
    /// Fired when <see cref="SetTimeZone"/> changes the effective zone.
    /// Components that render time-sensitive data should subscribe and
    /// re-render so the change shows up without a navigation.
    /// </summary>
    public event Action? Changed;

    /// <inheritdoc />
    public override TimeZoneInfo LocalTimeZone => _zone;

    /// <inheritdoc />
    public override DateTimeOffset GetUtcNow() => System.GetUtcNow();

    /// <summary>The default (host) timezone, captured at construction.</summary>
    public TimeZoneInfo HostTimeZone { get; } = TimeZoneInfo.Local;

    /// <summary>Replace the effective zone. No-op if the id matches.</summary>
    public void SetTimeZone(TimeZoneInfo tz)
    {
        if (string.Equals(_zone.Id, tz.Id, StringComparison.Ordinal)) return;
        _zone = tz;
        Changed?.Invoke();
    }

    /// <summary>Convenience wrapper used by the dropdown.</summary>
    public void SetTimeZoneById(string id)
    {
        try
        {
            SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById(id));
        }
        catch (TimeZoneNotFoundException) { /* ignore unknown id */ }
        catch (InvalidTimeZoneException)   { /* ignore malformed */ }
    }
}

/// <summary>
/// Helpers that project UTC <see cref="DateTime"/> values into the
/// <see cref="TimeProvider.LocalTimeZone"/>. Pages should call these
/// instead of <c>DateTime.ToLocalTime()</c> so the header dropdown's
/// chosen zone is honoured (the BCL method always uses the OS zone).
/// </summary>
public static class TimeProviderExtensions
{
    /// <summary>
    /// Convert a UTC <see cref="DateTime"/> to local time in the
    /// provider's <see cref="TimeProvider.LocalTimeZone"/>. The input is
    /// treated as UTC even when its <c>Kind</c> is <c>Unspecified</c>.
    /// </summary>
    public static DateTime ToLocal(this TimeProvider time, DateTime utc)
    {
        var asUtc = utc.Kind == DateTimeKind.Utc
            ? utc
            : DateTime.SpecifyKind(utc, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(asUtc, time.LocalTimeZone);
    }
}
