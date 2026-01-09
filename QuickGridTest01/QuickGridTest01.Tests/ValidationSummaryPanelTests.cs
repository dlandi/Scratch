using QuickGridTest01.ComposableColumns.Features.Editing;
using Xunit;

namespace QuickGridTest01.Tests;

public class ValidationSummaryPanelTests
{
    #region FocusedCellInfo Tests

    [Fact]
    public void FocusedCellInfo_Constructor_SetsProperties()
    {
        // Arrange
        var propertyName = "Name";
        var itemKey = (object)42;
        var descriptors = new List<ValidationRuleDescriptor>
        {
            new("Required", "Field is required", ValidationSeverity.Error),
            new("StringLength", "Max 50 chars", ValidationSeverity.Error)
        };

        // Act
        var focusedCell = new FocusedCellInfo(propertyName, itemKey, descriptors);

        // Assert
        Assert.Equal(propertyName, focusedCell.PropertyName);
        Assert.Equal(itemKey, focusedCell.ItemKey);
        Assert.Equal(2, focusedCell.RuleDescriptors.Count);
        Assert.Equal("Required", focusedCell.RuleDescriptors[0].Name);
    }

    [Fact]
    public void FocusedCellInfo_WithNullPropertyName_IsValid()
    {
        // Arrange & Act
        var focusedCell = new FocusedCellInfo(null, 1, Array.Empty<ValidationRuleDescriptor>());

        // Assert
        Assert.Null(focusedCell.PropertyName);
    }

    #endregion

    #region ValidationRuleDescriptor Tests

    [Fact]
    public void ValidationRuleDescriptor_DefaultSeverity_IsError()
    {
        // Arrange & Act
        var descriptor = new ValidationRuleDescriptor("Required");

        // Assert
        Assert.Equal("Required", descriptor.Name);
        Assert.Null(descriptor.Description);
        Assert.Equal(ValidationSeverity.Error, descriptor.Severity);
    }

    [Fact]
    public void ValidationRuleDescriptor_WithAllProperties_SetsCorrectly()
    {
        // Arrange & Act
        var descriptor = new ValidationRuleDescriptor(
            Name: "Range",
            Description: "Value must be between 1 and 100",
            Severity: ValidationSeverity.Warning
        );

        // Assert
        Assert.Equal("Range", descriptor.Name);
        Assert.Equal("Value must be between 1 and 100", descriptor.Description);
        Assert.Equal(ValidationSeverity.Warning, descriptor.Severity);
    }

    #endregion

    #region EditEventStream FocusedCell Tests

    [Fact]
    public void EditEventStream_FocusedCell_InitiallyNull()
    {
        // Arrange
        using var stream = new EditEventStream();

        // Assert
        Assert.Null(stream.FocusedCell);
    }

    [Fact]
    public void EditEventStream_SetFocusedCell_UpdatesProperty()
    {
        // Arrange
        using var stream = new EditEventStream();
        var focusedCell = new FocusedCellInfo("Name", 1, Array.Empty<ValidationRuleDescriptor>());

        // Act
        stream.SetFocusedCell(focusedCell);

        // Assert
        Assert.NotNull(stream.FocusedCell);
        Assert.Equal("Name", stream.FocusedCell.Value.PropertyName);
    }

    [Fact]
    public void EditEventStream_SetFocusedCell_RaisesFocusedCellChanged()
    {
        // Arrange
        using var stream = new EditEventStream();
        FocusedCellInfo? receivedCell = null;
        var eventRaised = false;
        stream.FocusedCellChanged += cell =>
        {
            eventRaised = true;
            receivedCell = cell;
        };
        var focusedCell = new FocusedCellInfo("Price", 2, Array.Empty<ValidationRuleDescriptor>());

        // Act
        stream.SetFocusedCell(focusedCell);

        // Assert
        Assert.True(eventRaised);
        Assert.NotNull(receivedCell);
        Assert.Equal("Price", receivedCell.Value.PropertyName);
    }

    [Fact]
    public void EditEventStream_SetFocusedCell_ToNull_ClearsFocus()
    {
        // Arrange
        using var stream = new EditEventStream();
        stream.SetFocusedCell(new FocusedCellInfo("Name", 1, Array.Empty<ValidationRuleDescriptor>()));
        Assert.NotNull(stream.FocusedCell);

        // Act
        stream.SetFocusedCell(null);

        // Assert
        Assert.Null(stream.FocusedCell);
    }

    #endregion

    #region ValidationFailedEvent with RuleDescriptors Tests

    [Fact]
    public void ValidationFailedEvent_IncludesRuleDescriptors()
    {
        // Arrange
        var descriptors = new List<ValidationRuleDescriptor>
        {
            new("Required", null, ValidationSeverity.Error),
            new("StringLength", "2-50 chars", ValidationSeverity.Error)
        };

        var ruleResults = new List<ValidationRuleResult>
        {
            ValidationRuleResult.Success("Required"),
            ValidationRuleResult.Failure("StringLength", "Too short", ValidationSeverity.Error)
        };

        // Act
        var evt = new ValidationFailedEvent
        {
            ItemKey = 1,
            PropertyName = "Name",
            AttemptedValue = "X",
            Errors = new List<string> { "Too short" },
            RuleResults = ruleResults,
            RuleDescriptors = descriptors
        };

        // Assert
        Assert.Equal(2, evt.RuleDescriptors.Count);
        Assert.Equal("Required", evt.RuleDescriptors[0].Name);
        Assert.Equal("StringLength", evt.RuleDescriptors[1].Name);
        Assert.Equal(2, evt.RuleResults.Count);
    }

    [Fact]
    public void ValidationSucceededEvent_IncludesRuleDescriptors()
    {
        // Arrange
        var descriptors = new List<ValidationRuleDescriptor>
        {
            new("Required", null, ValidationSeverity.Error)
        };

        var ruleResults = new List<ValidationRuleResult>
        {
            ValidationRuleResult.Success("Required")
        };

        // Act
        var evt = new ValidationSucceededEvent
        {
            ItemKey = 1,
            PropertyName = "Name",
            Value = "Valid Name",
            RuleResults = ruleResults,
            RuleDescriptors = descriptors
        };

        // Assert
        Assert.Equal(1, evt.RuleDescriptors.Count);
        Assert.Equal("Required", evt.RuleDescriptors[0].Name);
        Assert.Single(evt.RuleResults);
        Assert.True(evt.RuleResults[0].IsValid);
    }

    #endregion

    #region Placement Tests

    [Theory]
    [InlineData(EventPanelPlacement.None)]
    [InlineData(EventPanelPlacement.Top)]
    [InlineData(EventPanelPlacement.Bottom)]
    [InlineData(EventPanelPlacement.Left)]
    [InlineData(EventPanelPlacement.Right)]
    public void EventPanelPlacement_AllValues_AreValid(EventPanelPlacement placement)
    {
        // Assert - just verify enum values are accessible
        Assert.True(Enum.IsDefined(typeof(EventPanelPlacement), placement));
    }

    #endregion
}
