namespace ResourceScheduler.Components.Models;

/// <summary>A named team of People who reserve Device-Groups. Spec 4.5.</summary>
public sealed class TestGroupDto
{
    public Guid TestGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Guid> MemberIds { get; set; } = new();
    public int Version { get; set; }
}

public sealed record TestGroupCreate(string Name, IReadOnlyList<Guid> MemberIds);

public sealed record TestGroupUpdate(string Name, IReadOnlyList<Guid> MemberIds);
