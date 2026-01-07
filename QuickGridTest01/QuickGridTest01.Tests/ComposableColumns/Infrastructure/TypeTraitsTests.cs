using System.Globalization;
using QuickGridTest01.ComposableColumns.Infrastructure;
using Xunit;

namespace QuickGridTest01.Tests.ComposableColumns.Infrastructure;

public class TypeTraitsTests
{
    private enum TestEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }

    #region Type Detection Tests

    [Fact]
    public void Kind_Boolean_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Boolean, TypeTraits<bool>.Kind);
    }

    [Fact]
    public void Kind_DateOnly_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Date, TypeTraits<DateOnly>.Kind);
    }

    [Fact]
    public void Kind_TimeOnly_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Time, TypeTraits<TimeOnly>.Kind);
    }

    [Fact]
    public void Kind_DateTime_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.DateTime, TypeTraits<DateTime>.Kind);
    }

    [Fact]
    public void Kind_Int32_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Int32, TypeTraits<int>.Kind);
    }

    [Fact]
    public void Kind_Int64_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Int64, TypeTraits<long>.Kind);
    }

    [Fact]
    public void Kind_Decimal_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Decimal, TypeTraits<decimal>.Kind);
    }

    [Fact]
    public void Kind_Double_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Double, TypeTraits<double>.Kind);
    }

    [Fact]
    public void Kind_Single_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Single, TypeTraits<float>.Kind);
    }

    [Fact]
    public void Kind_Enum_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.Enum, TypeTraits<TestEnum>.Kind);
    }

    [Fact]
    public void Kind_String_DetectedCorrectly()
    {
        Assert.Equal(ValueKind.String, TypeTraits<string>.Kind);
    }

    [Fact]
    public void Kind_CustomClass_DetectedAsOther()
    {
        Assert.Equal(ValueKind.Other, TypeTraits<TypeTraitsTests>.Kind);
    }

    #endregion

    #region Nullable Type Tests

    [Fact]
    public void IsNullable_NullableInt_ReturnsTrue()
    {
        Assert.True(TypeTraits<int?>.IsNullable);
    }

    [Fact]
    public void IsNullable_Int_ReturnsFalse()
    {
        Assert.False(TypeTraits<int>.IsNullable);
    }

    [Fact]
    public void IsNullable_NullableDateTime_ReturnsTrue()
    {
        Assert.True(TypeTraits<DateTime?>.IsNullable);
    }

    [Fact]
    public void IsNullable_DateTime_ReturnsFalse()
    {
        Assert.False(TypeTraits<DateTime>.IsNullable);
    }

    [Fact]
    public void IsNullable_String_ReturnsFalse()
    {
        // String is a reference type, not Nullable<T>
        Assert.False(TypeTraits<string>.IsNullable);
    }

    [Fact]
    public void NullableUnderlying_NullableInt_ReturnsIntType()
    {
        Assert.Equal(typeof(int), TypeTraits<int?>.NullableUnderlying);
    }

    [Fact]
    public void NullableUnderlying_Int_ReturnsNull()
    {
        Assert.Null(TypeTraits<int>.NullableUnderlying);
    }

    [Fact]
    public void NonNullableType_NullableInt_ReturnsIntType()
    {
        Assert.Equal(typeof(int), TypeTraits<int?>.NonNullableType);
    }

    [Fact]
    public void NonNullableType_Int_ReturnsIntType()
    {
        Assert.Equal(typeof(int), TypeTraits<int>.NonNullableType);
    }

    #endregion

    #region Enum Detection Tests

    [Fact]
    public void IsEnum_Enum_ReturnsTrue()
    {
        Assert.True(TypeTraits<TestEnum>.IsEnum);
    }

    [Fact]
    public void IsEnum_NullableEnum_ReturnsTrue()
    {
        Assert.True(TypeTraits<TestEnum?>.IsEnum);
    }

    [Fact]
    public void IsEnum_Int_ReturnsFalse()
    {
        Assert.False(TypeTraits<int>.IsEnum);
    }

    [Fact]
    public void IsEnum_String_ReturnsFalse()
    {
        Assert.False(TypeTraits<string>.IsEnum);
    }

    #endregion

    #region FormatForInput Tests

    [Fact]
    public void FormatForInput_Null_ReturnsEmptyString()
    {
        var result = TypeTraits<string>.FormatForInput(null, null, CultureInfo.InvariantCulture);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FormatForInput_DateOnly_UsesInvariantFormat()
    {
        var date = new DateOnly(2024, 3, 15);
        var result = TypeTraits<DateOnly>.FormatForInput(date, null, CultureInfo.CurrentCulture);
        Assert.Equal("2024-03-15", result);
    }

    [Fact]
    public void FormatForInput_TimeOnly_UsesInvariantFormat()
    {
        var time = new TimeOnly(14, 30);
        var result = TypeTraits<TimeOnly>.FormatForInput(time, null, CultureInfo.CurrentCulture);
        Assert.Equal("14:30", result);
    }

    [Fact]
    public void FormatForInput_DateTime_DefaultFormat_UsesDateOnly()
    {
        var dateTime = new DateTime(2024, 3, 15, 10, 30, 0);
        var result = TypeTraits<DateTime>.FormatForInput(dateTime, null, CultureInfo.CurrentCulture);
        Assert.Equal("2024-03-15", result);
    }

    [Fact]
    public void FormatForInput_DateTime_WithDateTimeOverride_UsesDateTimeLocalFormat()
    {
        var dateTime = new DateTime(2024, 3, 15, 10, 30, 0);
        var result = TypeTraits<DateTime>.FormatForInput(dateTime, "DateTime", CultureInfo.CurrentCulture);
        Assert.Equal("2024-03-15T10:30", result);
    }

    [Fact]
    public void FormatForInput_Int32_UsesInvariantCulture()
    {
        var result = TypeTraits<int>.FormatForInput(1234, null, CultureInfo.CurrentCulture);
        Assert.Equal("1234", result);
    }

    [Fact]
    public void FormatForInput_Int64_UsesInvariantCulture()
    {
        var result = TypeTraits<long>.FormatForInput(9876543210L, null, CultureInfo.CurrentCulture);
        Assert.Equal("9876543210", result);
    }

    [Fact]
    public void FormatForInput_Decimal_UsesInvariantCulture()
    {
        var result = TypeTraits<decimal>.FormatForInput(123.45m, null, CultureInfo.CurrentCulture);
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void FormatForInput_Double_UsesInvariantCulture()
    {
        var result = TypeTraits<double>.FormatForInput(123.456, null, CultureInfo.CurrentCulture);
        Assert.Equal("123.456", result);
    }

    [Fact]
    public void FormatForInput_Single_UsesInvariantCulture()
    {
        var result = TypeTraits<float>.FormatForInput(123.45f, null, CultureInfo.CurrentCulture);
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void FormatForInput_String_ReturnsAsIs()
    {
        var result = TypeTraits<string>.FormatForInput("Test String", null, CultureInfo.InvariantCulture);
        Assert.Equal("Test String", result);
    }

    [Fact]
    public void FormatForInput_Boolean_ReturnsToString()
    {
        var resultTrue = TypeTraits<bool>.FormatForInput(true, null, CultureInfo.InvariantCulture);
        var resultFalse = TypeTraits<bool>.FormatForInput(false, null, CultureInfo.InvariantCulture);
        Assert.Equal("True", resultTrue);
        Assert.Equal("False", resultFalse);
    }

    #endregion

    #region ToOptionValueString Tests

    [Fact]
    public void ToOptionValueString_Null_ReturnsEmptyString()
    {
        var result = TypeTraits<string>.ToOptionValueString(null, CultureInfo.InvariantCulture);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToOptionValueString_Enum_ReturnsEnumName()
    {
        var result = TypeTraits<TestEnum>.ToOptionValueString(TestEnum.First, CultureInfo.InvariantCulture);
        Assert.Equal("First", result);
    }

    [Fact]
    public void ToOptionValueString_DateOnly_UsesInvariantFormat()
    {
        var date = new DateOnly(2024, 12, 25);
        var result = TypeTraits<DateOnly>.ToOptionValueString(date, CultureInfo.CurrentCulture);
        Assert.Equal("2024-12-25", result);
    }

    [Fact]
    public void ToOptionValueString_TimeOnly_UsesInvariantFormat()
    {
        var time = new TimeOnly(23, 59);
        var result = TypeTraits<TimeOnly>.ToOptionValueString(time, CultureInfo.CurrentCulture);
        Assert.Equal("23:59", result);
    }

    [Fact]
    public void ToOptionValueString_DateTime_UsesInvariantFormat()
    {
        var dateTime = new DateTime(2024, 12, 25, 15, 30, 0);
        var result = TypeTraits<DateTime>.ToOptionValueString(dateTime, CultureInfo.CurrentCulture);
        Assert.Equal("2024-12-25", result);
    }

    [Fact]
    public void ToOptionValueString_Int32_UsesInvariantCulture()
    {
        var result = TypeTraits<int>.ToOptionValueString(42, CultureInfo.CurrentCulture);
        Assert.Equal("42", result);
    }

    [Fact]
    public void ToOptionValueString_Decimal_UsesInvariantCulture()
    {
        var result = TypeTraits<decimal>.ToOptionValueString(99.99m, CultureInfo.CurrentCulture);
        Assert.Equal("99.99", result);
    }

    #endregion

    #region TryParseFromEventValue Tests

    [Fact]
    public void TryParseFromEventValue_EmptyString_ReturnsDefaultAndTrue()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(0, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_WhitespaceString_ReturnsDefaultAndTrue()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("   ", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(0, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullString_ReturnsDefaultAndTrue()
    {
        var success = TypeTraits<int>.TryParseFromEventValue(null, CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(0, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Boolean_True_ParsesCorrectly()
    {
        var success = TypeTraits<bool>.TryParseFromEventValue(true, CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.True(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Boolean_False_ParsesCorrectly()
    {
        var success = TypeTraits<bool>.TryParseFromEventValue(false, CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.False(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Boolean_StringOn_ParsesAsTrue()
    {
        var success = TypeTraits<bool>.TryParseFromEventValue("on", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.True(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Boolean_StringTrue_ParsesAsTrue()
    {
        var success = TypeTraits<bool>.TryParseFromEventValue("true", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.True(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Boolean_StringFalse_ParsesAsFalse()
    {
        var success = TypeTraits<bool>.TryParseFromEventValue("false", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.False(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Enum_ValidName_ParsesCorrectly()
    {
        var success = TypeTraits<TestEnum>.TryParseFromEventValue("First", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(TestEnum.First, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Enum_ValidNameCaseInsensitive_ParsesCorrectly()
    {
        var success = TypeTraits<TestEnum>.TryParseFromEventValue("first", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(TestEnum.First, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Enum_InvalidName_ReturnsFalse()
    {
        var success = TypeTraits<TestEnum>.TryParseFromEventValue("Invalid", CultureInfo.InvariantCulture, out var parsed);
        Assert.False(success);
    }

    [Fact]
    public void TryParseFromEventValue_DateOnly_ValidFormat_ParsesCorrectly()
    {
        var success = TypeTraits<DateOnly>.TryParseFromEventValue("2024-03-15", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(new DateOnly(2024, 3, 15), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_DateOnly_InvalidFormat_ReturnsDefault()
    {
        var success = TypeTraits<DateOnly>.TryParseFromEventValue("15/03/2024", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(default(DateOnly), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_TimeOnly_ValidFormat_ParsesCorrectly()
    {
        var success = TypeTraits<TimeOnly>.TryParseFromEventValue("14:30", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(new TimeOnly(14, 30), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_TimeOnly_InvalidFormat_ReturnsDefault()
    {
        var success = TypeTraits<TimeOnly>.TryParseFromEventValue("2:30 PM", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(default(TimeOnly), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_DateTime_DateOnlyFormat_ParsesCorrectly()
    {
        var success = TypeTraits<DateTime>.TryParseFromEventValue("2024-03-15", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(new DateTime(2024, 3, 15), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_DateTime_DateTimeLocalFormat_ParsesCorrectly()
    {
        var success = TypeTraits<DateTime>.TryParseFromEventValue("2024-03-15T14:30", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(new DateTime(2024, 3, 15, 14, 30, 0), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Int32_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("42", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(42, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Int32_NegativeValue_ParsesCorrectly()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("-100", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(-100, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Int32_InvalidString_ReturnsFalse()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("not a number", CultureInfo.InvariantCulture, out var parsed);
        Assert.False(success);
    }

    [Fact]
    public void TryParseFromEventValue_Int64_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<long>.TryParseFromEventValue("9876543210", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(9876543210L, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Decimal_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<decimal>.TryParseFromEventValue("123.45", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(123.45m, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Decimal_InvalidString_ReturnsFalse()
    {
        var success = TypeTraits<decimal>.TryParseFromEventValue("abc", CultureInfo.InvariantCulture, out var parsed);
        Assert.False(success);
    }

    [Fact]
    public void TryParseFromEventValue_Double_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<double>.TryParseFromEventValue("123.456", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(123.456, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Single_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<float>.TryParseFromEventValue("123.45", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(123.45f, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_String_ReturnsInputAsIs()
    {
        var success = TypeTraits<string>.TryParseFromEventValue("Test String", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal("Test String", parsed);
    }

    #endregion

    #region BuildEnumOptions Tests

    [Fact]
    public void BuildEnumOptions_Enum_ReturnsAllValues()
    {
        var options = TypeTraits<TestEnum>.BuildEnumOptions();
        Assert.Equal(4, options.Count);
        Assert.Contains(options, o => o.Value.Equals(TestEnum.None));
        Assert.Contains(options, o => o.Value.Equals(TestEnum.First));
        Assert.Contains(options, o => o.Value.Equals(TestEnum.Second));
        Assert.Contains(options, o => o.Value.Equals(TestEnum.Third));
    }

    [Fact]
    public void BuildEnumOptions_Enum_ReturnsCorrectLabels()
    {
        var options = TypeTraits<TestEnum>.BuildEnumOptions();
        Assert.Contains(options, o => o.Text == "None");
        Assert.Contains(options, o => o.Text == "First");
        Assert.Contains(options, o => o.Text == "Second");
        Assert.Contains(options, o => o.Text == "Third");
    }

    [Fact]
    public void BuildEnumOptions_NonEnum_ReturnsEmpty()
    {
        var options = TypeTraits<int>.BuildEnumOptions();
        Assert.Empty(options);
    }

    [Fact]
    public void BuildEnumOptions_String_ReturnsEmpty()
    {
        var options = TypeTraits<string>.BuildEnumOptions();
        Assert.Empty(options);
    }

    [Fact]
    public void BuildEnumOptions_NullableEnum_ReturnsAllValues()
    {
        var options = TypeTraits<TestEnum?>.BuildEnumOptions();
        Assert.Equal(4, options.Count);
    }

    #endregion

    #region Nullable Type Parsing Tests

    [Fact]
    public void TryParseFromEventValue_NullableInt_EmptyString_ReturnsNull()
    {
        var success = TypeTraits<int?>.TryParseFromEventValue("", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Null(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullableInt_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<int?>.TryParseFromEventValue("42", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(42, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullableDateTime_EmptyString_ReturnsNull()
    {
        var success = TypeTraits<DateTime?>.TryParseFromEventValue("", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Null(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullableDateTime_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<DateTime?>.TryParseFromEventValue("2024-03-15", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(new DateTime(2024, 3, 15), parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullableEnum_EmptyString_ReturnsNull()
    {
        var success = TypeTraits<TestEnum?>.TryParseFromEventValue("", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Null(parsed);
    }

    [Fact]
    public void TryParseFromEventValue_NullableEnum_ValidString_ParsesCorrectly()
    {
        var success = TypeTraits<TestEnum?>.TryParseFromEventValue("First", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(TestEnum.First, parsed);
    }

    #endregion

    #region Edge Cases Tests

    [Fact]
    public void FormatForInput_NullableInt_Null_ReturnsEmptyString()
    {
        var result = TypeTraits<int?>.FormatForInput(null, null, CultureInfo.InvariantCulture);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FormatForInput_NullableInt_WithValue_FormatsCorrectly()
    {
        var result = TypeTraits<int?>.FormatForInput(42, null, CultureInfo.InvariantCulture);
        Assert.Equal("42", result);
    }

    [Fact]
    public void TryParseFromEventValue_Int32_Zero_ParsesCorrectly()
    {
        var success = TypeTraits<int>.TryParseFromEventValue("0", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(0, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_Decimal_ZeroWithDecimals_ParsesCorrectly()
    {
        var success = TypeTraits<decimal>.TryParseFromEventValue("0.00", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Equal(0.00m, parsed);
    }

    [Fact]
    public void TryParseFromEventValue_String_EmptyString_ReturnsDefault()
    {
        var success = TypeTraits<string>.TryParseFromEventValue("", CultureInfo.InvariantCulture, out var parsed);
        Assert.True(success);
        Assert.Null(parsed); // Empty string returns default (null) for string type
    }

    #endregion
}
