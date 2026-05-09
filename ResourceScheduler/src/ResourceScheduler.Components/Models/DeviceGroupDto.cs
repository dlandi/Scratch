namespace ResourceScheduler.Components.Models;

/// <summary>
/// One device's normalized [0..1] position on the Designer canvas.
/// Optional: when no entry is present for a device id, components fall
/// back to auto-layout (circular).
/// </summary>
public sealed record DeviceLayoutEntry(Guid DeviceId, double X, double Y);

/// <summary>
/// A named, connected collection of Devices reserved as one unit. Spec 4.2.
/// </summary>
public sealed class DeviceGroupDto
{
    public Guid DeviceGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceGroupStatus Status { get; set; }
    public List<Guid> DeviceIds { get; set; } = new();
    public List<DeviceConnectionDto> Connections { get; set; } = new();
    public List<DeviceLayoutEntry> Layout { get; set; } = new();
    public int Version { get; set; }
}

/// <summary>A physical link between two devices in the same group. Spec 4.3.</summary>
public sealed class DeviceConnectionDto
{
    public Guid ConnectionId { get; set; }
    public Guid FromDeviceId { get; set; }
    public Guid ToDeviceId { get; set; }
    public string Label { get; set; } = string.Empty;
}

public sealed record DeviceGroupCreate(
    string Name,
    IReadOnlyList<Guid> DeviceIds,
    IReadOnlyList<DeviceConnectionDto> Connections,
    IReadOnlyList<DeviceLayoutEntry> Layout);

public sealed record DeviceGroupUpdate(
    string Name,
    IReadOnlyList<Guid> DeviceIds,
    IReadOnlyList<DeviceConnectionDto> Connections,
    IReadOnlyList<DeviceLayoutEntry> Layout);
