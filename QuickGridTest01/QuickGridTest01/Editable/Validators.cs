namespace QuickGridTest01.CustomColumns;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public static ValidationResult Success() => new() { IsValid = true };
    public static ValidationResult Failure(string errorMessage) => new() { IsValid = false, ErrorMessage = errorMessage };
}

public interface IValidator<TValue>
{
    string Name { get; }
    Task<ValidationResult> ValidateAsync(TValue? value);
}

public abstract class ValidatorBase<TValue> : IValidator<TValue>
{
    public abstract string Name { get; }
    public virtual Task<ValidationResult> ValidateAsync(TValue? value)
    {
        if (value is null)
        {
            // Null considered valid unless a specific validator overrides this (e.g. Required)
            return Task.FromResult(ValidationResult.Success());
        }
        var result = Validate(value);
        return Task.FromResult(result);
    }
    protected abstract ValidationResult Validate(TValue value);
}

// String Validators
public class RequiredStringValidator : ValidatorBase<string>
{
    public override string Name => "Required";
    public override Task<ValidationResult> ValidateAsync(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult(ValidationResult.Failure("This field is required."));
        }
        return Task.FromResult(ValidationResult.Success());
    }
    protected override ValidationResult Validate(string value) => ValidationResult.Success();
}

public class StringLengthValidator : ValidatorBase<string>
{
    public int MinLength { get; set; }
    public int MaxLength { get; set; } = int.MaxValue;
    public override string Name => $"Length({MinLength}-{MaxLength})";
    protected override ValidationResult Validate(string value)
    {
        if (value.Length < MinLength)
            return ValidationResult.Failure($"Must be at least {MinLength} characters long.");
        if (value.Length > MaxLength)
            return ValidationResult.Failure($"Must be no more than {MaxLength} characters long.");
        return ValidationResult.Success();
    }
}

public class EmailValidator : ValidatorBase<string>
{
    public override string Name => "Email";
    public override Task<ValidationResult> ValidateAsync(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Task.FromResult(ValidationResult.Success());
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, emailPattern))
            return Task.FromResult(ValidationResult.Failure("Must be a valid email address."));
        return Task.FromResult(ValidationResult.Success());
    }
    protected override ValidationResult Validate(string value) => ValidationResult.Success();
}

public class PatternValidator : ValidatorBase<string>
{
    public string Pattern { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = "Value does not match required pattern.";
    public override string Name => $"Pattern({Pattern})";
    public override Task<ValidationResult> ValidateAsync(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Task.FromResult(ValidationResult.Success());
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, Pattern))
            return Task.FromResult(ValidationResult.Failure(ErrorMessage));
        return Task.FromResult(ValidationResult.Success());
    }
    protected override ValidationResult Validate(string value) => ValidationResult.Success();
}

// Numeric Validators
public class RangeValidator<TValue> : ValidatorBase<TValue> where TValue : struct, IComparable<TValue>
{
    public TValue Minimum { get; set; }
    public TValue Maximum { get; set; }
    public override string Name => $"Range({Minimum}-{Maximum})";
    protected override ValidationResult Validate(TValue value)
    {
        if (value.CompareTo(Minimum) < 0) return ValidationResult.Failure($"Must be at least {Minimum}.");
        if (value.CompareTo(Maximum) > 0) return ValidationResult.Failure($"Must be no more than {Maximum}.");
        return ValidationResult.Success();
    }
}

public class MinValueValidator<TValue> : ValidatorBase<TValue> where TValue : struct, IComparable<TValue>
{
    public TValue Minimum { get; set; }
    public override string Name => $"Min({Minimum})";
    protected override ValidationResult Validate(TValue value)
    {
        if (value.CompareTo(Minimum) < 0) return ValidationResult.Failure($"Must be at least {Minimum}.");
        return ValidationResult.Success();
    }
}

public class MaxValueValidator<TValue> : ValidatorBase<TValue> where TValue : struct, IComparable<TValue>
{
    public TValue Maximum { get; set; }
    public override string Name => $"Max({Maximum})";
    protected override ValidationResult Validate(TValue value)
    {
        if (value.CompareTo(Maximum) > 0) return ValidationResult.Failure($"Must be no more than {Maximum}.");
        return ValidationResult.Success();
    }
}

public class PositiveNumberValidator<TValue> : ValidatorBase<TValue> where TValue : struct, IComparable<TValue>
{
    public override string Name => "Positive";
    protected override ValidationResult Validate(TValue value)
    {
        var zero = (TValue)Convert.ChangeType(0, typeof(TValue));
        if (value.CompareTo(zero) <= 0) return ValidationResult.Failure("Must be a positive number.");
        return ValidationResult.Success();
    }
}

// Date Validators
public class DateRangeValidator : ValidatorBase<DateTime>
{
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public override string Name => "DateRange";
    protected override ValidationResult Validate(DateTime value)
    {
        if (MinDate.HasValue && value < MinDate.Value)
            return ValidationResult.Failure($"Date must be on or after {MinDate.Value:yyyy-MM-dd}.");
        if (MaxDate.HasValue && value > MaxDate.Value)
            return ValidationResult.Failure($"Date must be on or before {MaxDate.Value:yyyy-MM-dd}.");
        return ValidationResult.Success();
    }
}

public class FutureDateValidator : ValidatorBase<DateTime>
{
    public override string Name => "FutureDate";
    protected override ValidationResult Validate(DateTime value)
    {
        if (value.Date <= DateTime.Today) return ValidationResult.Failure("Date must be in the future.");
        return ValidationResult.Success();
    }
}

public class PastDateValidator : ValidatorBase<DateTime>
{
    public override string Name => "PastDate";
    protected override ValidationResult Validate(DateTime value)
    {
        if (value.Date >= DateTime.Today) return ValidationResult.Failure("Date must be in the past.");
        return ValidationResult.Success();
    }
}

// Custom Validator
public class CustomValidator<TValue> : IValidator<TValue>
{
    private readonly Func<TValue?, Task<ValidationResult>> _validateFunc;
    public string Name { get; set; }
    public CustomValidator(string name, Func<TValue?, Task<ValidationResult>> validateFunc)
    {
        Name = name;
        _validateFunc = validateFunc;
    }
    public CustomValidator(string name, Func<TValue?, ValidationResult> validateFunc)
    {
        Name = name;
        _validateFunc = v => Task.FromResult(validateFunc(v));
    }
    public Task<ValidationResult> ValidateAsync(TValue? value) => _validateFunc(value);
}

public class UniqueValueValidator<TValue> : IValidator<TValue>
{
    private readonly Func<TValue?, Task<bool>> _checkUniqueAsync;
    public string Name => "Unique";
    public UniqueValueValidator(Func<TValue?, Task<bool>> checkUniqueAsync) => _checkUniqueAsync = checkUniqueAsync;
    public async Task<ValidationResult> ValidateAsync(TValue? value)
    {
        if (value is null) return ValidationResult.Success();
        var isUnique = await _checkUniqueAsync(value);
        if (!isUnique) return ValidationResult.Failure("This value already exists.");
        return ValidationResult.Success();
    }
}
