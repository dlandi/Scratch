using QuickGridTest01.MultiState.Validation;
using QuickGridTest01.MultiState.Component;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Component;

public class ValidationOrchestratorTests
{
    private class TestValidator : IValidator<string>
    {
        private readonly bool _shouldPass;
        private readonly string _errorMessage;
        public bool WasCalled { get; private set; }

        public TestValidator(bool shouldPass, string errorMessage = "Test error")
        {
            _shouldPass = shouldPass;
            _errorMessage = errorMessage;
        }

        public Task<ValidationResult> ValidateAsync(string? value, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_shouldPass
                ? ValidationResult.Success()
                : ValidationResult.Failure(_errorMessage));
        }
    }

    #region ValidationOrchestrator Tests

    [Fact]
    public void Constructor_WithoutValidators_CreatesEmptyOrchestrator()
    {
        // Act
        var orchestrator = new ValidationOrchestrator<string>();

        // Assert
        Assert.Equal(0, orchestrator.ValidatorCount);
    }

    [Fact]
    public void Constructor_WithValidators_AddsThemToOrchestrator()
    {
        // Arrange
        var validators = new[]
        {
            new TestValidator(true),
            new TestValidator(true),
            new TestValidator(true)
        };

        // Act
        var orchestrator = new ValidationOrchestrator<string>(validators);

        // Assert
        Assert.Equal(3, orchestrator.ValidatorCount);
    }

    [Fact]
    public void AddValidator_IncreasesValidatorCount()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        var validator = new TestValidator(true);

        // Act
        orchestrator.AddValidator(validator);

        // Assert
        Assert.Equal(1, orchestrator.ValidatorCount);
    }

    [Fact]
    public void AddValidator_ThrowsOnNull()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => orchestrator.AddValidator(null!));
    }

    [Fact]
    public async Task ValidateAsync_ReturnsSuccess_WhenAllValidatorsPass()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));

        // Act
        var result = await orchestrator.ValidateAsync("test");

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsAllErrors_WhenMultipleValidatorsFail()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(false, "Error 1"));
        orchestrator.AddValidator(new TestValidator(false, "Error 2"));
        orchestrator.AddValidator(new TestValidator(false, "Error 3"));

        // Act
        var result = await orchestrator.ValidateAsync("test");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains("Error 1", result.Errors);
        Assert.Contains("Error 2", result.Errors);
        Assert.Contains("Error 3", result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_CollectsOnlyFailures_WhenSomeValidatorsPass()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(false, "Error A"));
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(false, "Error B"));

        // Act
        var result = await orchestrator.ValidateAsync("test");

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains("Error A", result.Errors);
        Assert.Contains("Error B", result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsSuccess_WhenNoValidators()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();

        // Act
        var result = await orchestrator.ValidateAsync("test");

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ValidateFirstFailureAsync_ReturnsSuccess_WhenAllValidatorsPass()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));

        // Act
        var result = await orchestrator.ValidateFirstFailureAsync("test");

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ValidateFirstFailureAsync_ReturnsFirstError_AndStopsValidation()
    {
        // Arrange
        var validator1 = new TestValidator(true);
        var validator2 = new TestValidator(false, "First Error");
        var validator3 = new TestValidator(false, "Second Error");

        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(validator1);
        orchestrator.AddValidator(validator2);
        orchestrator.AddValidator(validator3);

        // Act
        var result = await orchestrator.ValidateFirstFailureAsync("test");

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("First Error", result.Errors[0]);
        Assert.True(validator1.WasCalled);
        Assert.True(validator2.WasCalled);
        Assert.False(validator3.WasCalled); // Should not be called (fail-fast)
    }

    [Fact]
    public void Clear_RemovesAllValidators()
    {
        // Arrange
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));
        orchestrator.AddValidator(new TestValidator(true));

        // Act
        orchestrator.Clear();

        // Assert
        Assert.Equal(0, orchestrator.ValidatorCount);
    }

    [Fact]
    public async Task ValidateAsync_WithCancellationToken_PassesTokenToValidators()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var validator = new TestValidator(true);
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(validator);

        // Act
        await orchestrator.ValidateAsync("test", cts.Token);

        // Assert
        Assert.True(validator.WasCalled);
    }

    [Fact]
    public async Task ValidateAsync_RespectsCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var orchestrator = new ValidationOrchestrator<string>();
        orchestrator.AddValidator(new TestValidator(true));

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => orchestrator.ValidateAsync("test", cts.Token));
    }

    #endregion

    #region ValidationOrchestrationResult Tests

    [Fact]
    public void Constructor_WithNoErrors_CreatesValidResult()
    {
        // Act
        var result = new ValidationOrchestrationResult(Array.Empty<string>());

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Constructor_WithErrors_CreatesInvalidResult()
    {
        // Arrange
        var errors = new[] { "Error 1", "Error 2", "Error 3" };

        // Act
        var result = new ValidationOrchestrationResult(errors);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
    }

    [Fact]
    public void Constructor_FiltersOutEmptyErrors()
    {
        // Arrange
        var errors = new[] { "Error 1", "", "Error 2", "   ", "Error 3", null! };

        // Act
        var result = new ValidationOrchestrationResult(errors);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains("Error 1", result.Errors);
        Assert.Contains("Error 2", result.Errors);
        Assert.Contains("Error 3", result.Errors);
    }

    [Fact]
    public void GetCombinedErrorMessage_ReturnsEmptyString_WhenNoErrors()
    {
        // Arrange
        var result = new ValidationOrchestrationResult(Array.Empty<string>());

        // Act
        var message = result.GetCombinedErrorMessage();

        // Assert
        Assert.Equal(string.Empty, message);
    }

    [Fact]
    public void GetCombinedErrorMessage_JoinsErrorsWithDefaultSeparator()
    {
        // Arrange
        var errors = new[] { "Error 1", "Error 2", "Error 3" };
        var result = new ValidationOrchestrationResult(errors);

        // Act
        var message = result.GetCombinedErrorMessage();

        // Assert
        Assert.Equal("Error 1\nError 2\nError 3", message);
    }

    [Fact]
    public void GetCombinedErrorMessage_UsesCustomSeparator()
    {
        // Arrange
        var errors = new[] { "Error 1", "Error 2", "Error 3" };
        var result = new ValidationOrchestrationResult(errors);

        // Act
        var message = result.GetCombinedErrorMessage(" | ");

        // Assert
        Assert.Equal("Error 1 | Error 2 | Error 3", message);
    }

    [Fact]
    public void Errors_IsReadOnly()
    {
        // Arrange
        var errors = new[] { "Error 1" };
        var result = new ValidationOrchestrationResult(errors);

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<string>>(result.Errors);
    }

    #endregion
}