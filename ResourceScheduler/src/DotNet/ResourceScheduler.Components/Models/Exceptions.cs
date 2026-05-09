namespace ResourceScheduler.Components.Models;

/// <summary>Base class for client-service errors that are user-actionable.</summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

/// <summary>The requested resource does not exist.</summary>
public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>Optimistic concurrency mismatch on Version.</summary>
public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

/// <summary>A business rule was violated. Carries the rule id (e.g. "R10").</summary>
public sealed class ValidationException : DomainException
{
    public string RuleId { get; }

    public ValidationException(string ruleId, string message) : base(message)
    {
        RuleId = ruleId;
    }
}
