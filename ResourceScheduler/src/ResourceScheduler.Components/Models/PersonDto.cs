namespace ResourceScheduler.Components.Models;

/// <summary>A human participant in the lab. Spec 4.4.</summary>
public sealed class PersonDto
{
    public Guid PersonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public sealed record PersonCreate(string Name, string? Email);

public sealed record PersonUpdate(string Name, string? Email);
