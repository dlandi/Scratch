namespace ResourceScheduler.Components.Models;

/// <summary>Lifecycle status of a single Device. See spec 7.1.</summary>
public enum DeviceStatus
{
    Available,
    Maintenance,
    Offline,
    Retired,
}

/// <summary>Lifecycle status of a Device-Group. See spec 7.2.</summary>
public enum DeviceGroupStatus
{
    Inactive,
    Active,
}

/// <summary>Lifecycle status of a Reservation. See spec 7.3.</summary>
public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
}
