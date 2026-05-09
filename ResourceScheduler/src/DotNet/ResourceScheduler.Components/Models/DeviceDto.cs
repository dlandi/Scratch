namespace ResourceScheduler.Components.Models;

/// <summary>A single physical instrument in the lab. Spec 4.1.</summary>
public sealed class DeviceDto
{
    public Guid DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceStatus Status { get; set; }

    /// <summary>Required: every Device lives in exactly one Building (R14).</summary>
    public Guid BuildingId { get; set; }

    /// <summary>
    /// Convenience field; null if unassigned. Canonical relationship lives on
    /// <see cref="DeviceGroupDto.DeviceIds"/>.
    /// </summary>
    public Guid? AssignedDeviceGroupId { get; set; }

    public int Version { get; set; }
}

public sealed record DeviceCreate(string Name, DeviceStatus Status, Guid BuildingId);

public sealed record DeviceUpdate(string Name, DeviceStatus Status, Guid BuildingId);
