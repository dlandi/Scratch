namespace ResourceScheduler.Components.Models;

/// <summary>A real-world building that houses Devices. Spec 4.7.</summary>
public sealed class BuildingDto
{
    public Guid BuildingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Version { get; set; }
}

public sealed record BuildingCreate(string Name, string Address);

public sealed record BuildingUpdate(string Name, string Address);
