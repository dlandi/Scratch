using System.Text.RegularExpressions;

namespace QuickGridTest01.MultiState.Validation;

/// <summary>
/// Validates that a string value is not null or whitespace.
/// </summary>
public class RequiredValidator : IValidator<string>
{
    private readonly string _errorMessage;

    public RequiredValidator(string errorMessage = "Value is required")
    {
        _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
    {
        var isValid = !string.IsNullOrWhiteSpace(value);
        return Task.FromResult(isValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(_errorMessage));
    }
}

/// <summary>
/// Validates that a string is a valid email address format.
/// </summary>
public class EmailValidator : IValidator<string>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly string _errorMessage;

    public EmailValidator(string errorMessage = "Invalid email address format")
    {
        _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult(ValidationResult.Failure("Email address cannot be empty"));
        }

        var isValid = EmailRegex.IsMatch(value);
        return Task.FromResult(isValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(_errorMessage));
    }
}

/// <summary>
/// Validates that a string matches a regex pattern.
/// </summary>
public class PatternValidator : IValidator<string>
{
    private readonly Regex _regex;
    private readonly string _errorMessage;

    public PatternValidator(string pattern, string errorMessage = "Value does not match required pattern")
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Pattern cannot be null or whitespace", nameof(pattern));

        _regex = new Regex(pattern, RegexOptions.Compiled);
        _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult(ValidationResult.Failure("Value cannot be empty"));
        }

        var isValid = _regex.IsMatch(value);
        return Task.FromResult(isValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(_errorMessage));
    }
}

/// <summary>
/// Validates string length constraints.
/// </summary>
public class LengthValidator : IValidator<string>
{
    private readonly int? _minLength;
    private readonly int? _maxLength;
    private readonly string _errorMessage;

    public LengthValidator(int? minLength = null, int? maxLength = null, string? errorMessage = null)
    {
        if (minLength.HasValue && minLength.Value < 0)
            throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length cannot be negative");

        if (maxLength.HasValue && maxLength.Value < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");

        if (minLength.HasValue && maxLength.HasValue && minLength.Value > maxLength.Value)
            throw new ArgumentException("Minimum length cannot be greater than maximum length");

        _minLength = minLength;
        _maxLength = maxLength;
        _errorMessage = errorMessage ?? GenerateDefaultMessage(minLength, maxLength);
    }

    private static string GenerateDefaultMessage(int? minLength, int? maxLength)
    {
        if (minLength.HasValue && maxLength.HasValue)
            return $"Length must be between {minLength} and {maxLength} characters";
        if (minLength.HasValue)
            return $"Length must be at least {minLength} characters";
        if (maxLength.HasValue)
            return $"Length must not exceed {maxLength} characters";
        return "Invalid length";
    }

    public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
    {
        var length = value?.Length ?? 0;

        if (_minLength.HasValue && length < _minLength.Value)
        {
            return Task.FromResult(ValidationResult.Failure(_errorMessage));
        }

        if (_maxLength.HasValue && length > _maxLength.Value)
        {
            return Task.FromResult(ValidationResult.Failure(_errorMessage));
        }

        return Task.FromResult(ValidationResult.Success());
    }
}

/// <summary>
/// Validates uniqueness using an async check function.
/// </summary>
public class UniqueValidator : IValidator<string>
{
    private readonly Func<string?, CancellationToken, Task<bool>> _checkUniqueness;
    private readonly string _errorMessage;

    public UniqueValidator(
        Func<string?, CancellationToken, Task<bool>> checkUniqueness,
        string errorMessage = "Value must be unique")
    {
        _checkUniqueness = checkUniqueness ?? throw new ArgumentNullException(nameof(checkUniqueness));
        _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    public async Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ValidationResult.Failure("Value cannot be empty");
        }

        var isUnique = await _checkUniqueness(value, cancellationToken);
        return isUnique
            ? ValidationResult.Success()
            : ValidationResult.Failure(_errorMessage);
    }
}
