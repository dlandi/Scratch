namespace ResourceScheduler.Components.Models;

/// <summary>
/// A booking of a Device-Group by a Test-Group over a time window. Spec 4.6.
/// All times are stored in UTC.
/// </summary>
public sealed class ReservationDto
{
    public Guid ReservationId { get; set; }
    public Guid DeviceGroupId { get; set; }
    public Guid TestGroupId { get; set; }
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public ReservationStatus Status { get; set; }
    public string? Notes { get; set; }
    public int Version { get; set; }
}

public sealed record ReservationCreate(
    Guid DeviceGroupId,
    Guid TestGroupId,
    DateTime StartUtc,
    DateTime EndUtc,
    string? Notes);

/// <summary>Optional filter for <c>ListReservationsAsync</c>.</summary>
public sealed record ReservationFilter(
    Guid? DeviceGroupId = null,
    Guid? TestGroupId = null,
    DateTime? FromUtc = null,
    DateTime? ToUtc = null,
    IReadOnlyCollection<ReservationStatus>? StatusIn = null);
