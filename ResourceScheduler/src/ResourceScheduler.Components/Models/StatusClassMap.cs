namespace ResourceScheduler.Components.Models;

/// <summary>
/// Maps domain status enums to the short CSS-class suffixes used in
/// components.css ("ok", "warn", "off", "bad", "retired"). Mirrors the
/// helpers <c>deviceStatusToClass</c>, <c>groupStatusToClass</c>, and
/// <c>resvStatusToClass</c> in the React mockup.
/// </summary>
public static class StatusClassMap
{
    public static string ForDevice(DeviceStatus s) => s switch
    {
        DeviceStatus.Available   => "ok",
        DeviceStatus.Maintenance => "warn",
        DeviceStatus.Offline     => "off",
        DeviceStatus.Retired     => "retired",
        _ => "off",
    };

    public static string ForGroup(DeviceGroupStatus s) => s switch
    {
        DeviceGroupStatus.Active   => "ok",
        DeviceGroupStatus.Inactive => "off",
        _ => "off",
    };

    public static string ForReservation(ReservationStatus s) => s switch
    {
        ReservationStatus.Pending   => "warn",
        ReservationStatus.Confirmed => "ok",
        ReservationStatus.Cancelled => "bad",
        ReservationStatus.Completed => "off",
        _ => "off",
    };
}
