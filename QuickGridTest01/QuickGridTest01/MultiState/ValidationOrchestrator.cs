using QuickGridTest01.MultiState.Validation;

namespace QuickGridTest01.MultiState.Component;

/// <summary>
/// Orchestrates multiple validators, running them in sequence and aggregating results.
/// </summary>
/// <typeparam name="TValue">The type of value to validate</typeparam>
public class ValidationOrchestrator<TValue>
{
    private readonly List<IValidator<TValue>> _validators = new();

    /// <summary>
    /// Gets the number of validators in the orchestrator.
    /// </summary>
    public int ValidatorCount => _validators.Count;

    /// <summary>
    /// Initializes a new instance of ValidationOrchestrator.
    /// </summary>
    public ValidationOrchestrator()
    {
    }

    /// <summary>
    /// Initializes a new instance of ValidationOrchestrator with initial validators.
    /// </summary>
    /// <param name="validators">Initial validators to add</param>
    public ValidationOrchestrator(IEnumerable<IValidator<TValue>> validators)
    {
        if (validators != null)
        {
            _validators.AddRange(validators);
        }
    }

    /// <summary>
    /// Adds a validator to the orchestrator.
    /// </summary>
    /// <param name="validator">The validator to add</param>
    /// <exception cref="ArgumentNullException">Thrown when validator is null</exception>
    public void AddValidator(IValidator<TValue> validator)
    {
        if (validator == null)
            throw new ArgumentNullException(nameof(validator));

        _validators.Add(validator);
    }

    /// <summary>
    /// Removes all validators from the orchestrator.
    /// </summary>
    public void Clear()
    {
        _validators.Clear();
    }

    /// <summary>
    /// Validates a value using all validators and collects all errors.
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Orchestration result with all validation errors</returns>
    public async Task<ValidationOrchestrationResult> ValidateAsync(
        TValue? value,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        foreach (var validator in _validators)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await validator.ValidateAsync(value, cancellationToken);
            if (!result.IsValid && !string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                errors.Add(result.ErrorMessage);
            }
        }

        return new ValidationOrchestrationResult(errors);
    }

    /// <summary>
    /// Validates a value using validators and stops at the first failure (fail-fast).
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Orchestration result with first error or success</returns>
    public async Task<ValidationOrchestrationResult> ValidateFirstFailureAsync(
        TValue? value,
        CancellationToken cancellationToken = default)
    {
        foreach (var validator in _validators)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await validator.ValidateAsync(value, cancellationToken);
            if (!result.IsValid)
            {
                return new ValidationOrchestrationResult(
                    string.IsNullOrWhiteSpace(result.ErrorMessage)
                        ? new[] { "Validation failed" }
                        : new[] { result.ErrorMessage });
            }
        }

        return new ValidationOrchestrationResult(Array.Empty<string>());
    }
}

/// <summary>
/// Result of validation orchestration containing aggregated errors.
/// </summary>
public class ValidationOrchestrationResult
{
    private readonly List<string> _errors;

    /// <summary>
    /// Gets whether the validation passed (no errors).
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// Gets the read-only collection of error messages.
    /// </summary>
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of ValidationOrchestrationResult.
    /// </summary>
    /// <param name="errors">Collection of error messages</param>
    public ValidationOrchestrationResult(IEnumerable<string> errors)
    {
        // Filter out null or whitespace errors
        _errors = errors?
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToList() ?? new List<string>();
    }

    /// <summary>
    /// Gets a combined error message from all errors.
    /// </summary>
    /// <param name="separator">Separator between errors (default: newline)</param>
    /// <returns>Combined error message or empty string if no errors</returns>
    public string GetCombinedErrorMessage(string separator = "\n")
    {
        return _errors.Count == 0 ? string.Empty : string.Join(separator, _errors);
    }
}