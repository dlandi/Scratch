using QuickGridTest01.MultiState.Validation;
using System.Text.RegularExpressions;

namespace QuickGridTest01.MultiState.Demo;

/// <summary>
/// Custom validators for contact fields.
/// </summary>
public static class ContactValidators
{
    /// <summary>
    /// Validates that a string value is not empty or whitespace.
    /// </summary>
    public class Required : IValidator<string>
    {
        private readonly string _fieldName;

        public Required(string fieldName = "Value")
        {
            _fieldName = fieldName;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            var isValid = !string.IsNullOrWhiteSpace(value);
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure($"{_fieldName} is required"));
        }
    }

    /// <summary>
    /// Validates minimum string length.
    /// </summary>
    public class MinLength : IValidator<string>
    {
        private readonly int _minLength;
        private readonly string _fieldName;

        public MinLength(int minLength, string fieldName = "Value")
        {
            _minLength = minLength;
            _fieldName = fieldName;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            var length = value?.Length ?? 0;
            var isValid = length >= _minLength;
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure($"{_fieldName} must be at least {_minLength} characters"));
        }
    }

    /// <summary>
    /// Validates maximum string length.
    /// </summary>
    public class MaxLength : IValidator<string>
    {
        private readonly int _maxLength;
        private readonly string _fieldName;

        public MaxLength(int maxLength, string fieldName = "Value")
        {
            _maxLength = maxLength;
            _fieldName = fieldName;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            var length = value?.Length ?? 0;
            var isValid = length <= _maxLength;
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure($"{_fieldName} cannot exceed {_maxLength} characters"));
        }
    }

    /// <summary>
    /// Validates email address format.
    /// </summary>
    public class EmailFormat : IValidator<string>
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(ValidationResult.Failure("Email cannot be empty"));

            var isValid = EmailRegex.IsMatch(value);
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure("Invalid email format (expected: user@domain.com)"));
        }
    }

    /// <summary>
    /// Validates phone number format (US: 555-123-4567).
    /// </summary>
    public class PhoneFormat : IValidator<string>
    {
        private static readonly Regex PhoneRegex = new(@"^\d{3}-\d{3}-\d{4}$", RegexOptions.Compiled);

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(ValidationResult.Failure("Phone cannot be empty"));

            var isValid = PhoneRegex.IsMatch(value);
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure("Phone must be in format: 555-123-4567"));
        }
    }

    /// <summary>
    /// Validates email uniqueness using ContactService.
    /// </summary>
    public class EmailUnique : IValidator<string>
    {
        private readonly ContactService _contactService;
        private readonly int? _excludeContactId;

        public EmailUnique(ContactService contactService, int? excludeContactId = null)
        {
            _contactService = contactService;
            _excludeContactId = excludeContactId;
        }

        public async Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ValidationResult.Success(); // Let Required validator handle this

            var isUnique = await _contactService.IsEmailUniqueAsync(value, _excludeContactId);
            return isUnique
                ? ValidationResult.Success()
                : ValidationResult.Failure("Email address already exists");
        }
    }

    /// <summary>
    /// Validates that a string contains only letters and spaces.
    /// </summary>
    public class LettersOnly : IValidator<string>
    {
        private readonly string _fieldName;
        private static readonly Regex LettersRegex = new(@"^[a-zA-Z\s]+$", RegexOptions.Compiled);

        public LettersOnly(string fieldName = "Value")
        {
            _fieldName = fieldName;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(ValidationResult.Success()); // Let Required validator handle this

            var isValid = LettersRegex.IsMatch(value);
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure($"{_fieldName} can only contain letters and spaces"));
        }
    }

    /// <summary>
    /// Validates that a string matches a custom regex pattern.
    /// </summary>
    public class Pattern : IValidator<string>
    {
        private readonly Regex _regex;
        private readonly string _errorMessage;

        public Pattern(string pattern, string errorMessage)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
            _errorMessage = errorMessage;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(ValidationResult.Success());

            var isValid = _regex.IsMatch(value);
            return Task.FromResult(isValid
                ? ValidationResult.Success()
                : ValidationResult.Failure(_errorMessage));
        }
    }
}
