namespace QuickGridTest01.MultiState.Validation;

/// <summary>
/// Result of a validation operation.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Indicates if the validation passed.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// Error message if validation failed.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new() { IsValid = true };

    /// <summary>
    /// Creates a failed validation result with an error message.
    /// </summary>
    public static ValidationResult Failure(string errorMessage) => new()
    {
        IsValid = false,
        ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage))
    };
}

/// <summary>
/// Interface for value validators.
/// </summary>
/// <typeparam name="TValue">Type of value to validate</typeparam>
public interface IValidator<TValue>
{
    /// <summary>
    /// Validates a value asynchronously.
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<ValidationResult> ValidateAsync(TValue? value, CancellationToken cancellationToken = default);
}
