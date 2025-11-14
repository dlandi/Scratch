using System.Globalization;
using QuickGridTest01.FormattedValue.Core;

namespace QuickGridTest01.FormattedValue.Formatters;

/// <summary>
/// Specialized formatting functions for specific data types
/// Includes file sizes, phone numbers, sensitive data masking, and boolean displays
/// </summary>
public static class SpecializedFormatters
{
    #region File Size Formatters

    /// <summary>
    /// Formats a byte count as file size (1.46 MB, 3.2 GB, etc.)
    /// </summary>
    /// <param name="decimals">Number of decimal places (default: 2)</param>
    /// <returns>Formatter function that formats byte counts as human-readable file sizes</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.FileSize();
    /// formatter(1536)         // Returns "1.50 KB"
    /// formatter(1048576)      // Returns "1.00 MB"
    /// formatter(1073741824)   // Returns "1.00 GB"
    /// </example>
    public static Func<object?, string> FileSize(int decimals = 2)
    {
        return value =>
        {
            var number = FormatterHelpers.ToDouble(value);
            if (!number.HasValue)
                return string.Empty;

            var bytes = (long)number.Value;
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len.ToString($"F{decimals}", CultureInfo.CurrentCulture)} {sizes[order]}";
        };
    }

    #endregion

    #region Phone Number Formatters

    /// <summary>
    /// Formats a 10-digit phone number as (555) 123-4567
    /// </summary>
    /// <returns>Formatter function that formats phone numbers in US format</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.PhoneNumberUs();
    /// formatter("5551234567")        // Returns "(555) 123-4567"
    /// formatter("(555) 123-4567")    // Returns "(555) 123-4567"
    /// formatter("15551234567")       // Returns "(555) 123-4567"
    /// </example>
    public static Func<object?, string> PhoneNumberUs()
    {
        return value =>
        {
            var phone = FormatterHelpers.CleanString(
                value?.ToString(), 
                '-', '(', ')', ' ', '+'
            );
            
            if (phone.Length == 10)
                return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6)}";
            
            if (phone.Length == 11 && phone[0] == '1')
                return $"({phone.Substring(1, 3)}) {phone.Substring(4, 3)}-{phone.Substring(7)}";
            
            return phone;
        };
    }

    /// <summary>
    /// Formats an 11-digit phone number as +1 555-123-4567
    /// </summary>
    /// <returns>Formatter function that formats phone numbers in international format</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.PhoneNumberInternational();
    /// formatter("15551234567")    // Returns "+1 555-123-4567"
    /// formatter("5551234567")     // Returns "+1 555-123-4567"
    /// </example>
    public static Func<object?, string> PhoneNumberInternational()
    {
        return value =>
        {
            var phone = FormatterHelpers.CleanString(
                value?.ToString(), 
                '-', '(', ')', ' ', '+'
            );
            
            if (phone.Length == 11 && phone[0] == '1')
                return $"+{phone[0]} {phone.Substring(1, 3)}-{phone.Substring(4, 3)}-{phone.Substring(7)}";
            
            if (phone.Length == 10)
                return $"+1 {phone.Substring(0, 3)}-{phone.Substring(3, 3)}-{phone.Substring(6)}";
            
            return phone;
        };
    }

    #endregion

    #region Sensitive Data Formatters

    /// <summary>
    /// Masks a credit card number showing only last 4 digits
    /// </summary>
    /// <returns>Formatter function that masks credit card numbers</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.CreditCardMasked();
    /// formatter("4111111111111111")     // Returns "****-****-****-1111"
    /// formatter("4111-1111-1111-1111")  // Returns "****-****-****-1111"
    /// </example>
    public static Func<object?, string> CreditCardMasked()
    {
        return value =>
        {
            var card = FormatterHelpers.CleanString(
                value?.ToString(), 
                '-', ' '
            );
            
            if (card.Length >= 4)
                return $"****-****-****-{card.Substring(card.Length - 4)}";
            
            return card;
        };
    }

    /// <summary>
    /// Formats an SSN as 123-45-6789
    /// </summary>
    /// <returns>Formatter function that formats SSNs</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.SocialSecurityNumber();
    /// formatter("123456789")    // Returns "123-45-6789"
    /// formatter("123-45-6789")  // Returns "123-45-6789"
    /// </example>
    public static Func<object?, string> SocialSecurityNumber()
    {
        return value =>
        {
            var ssn = FormatterHelpers.CleanString(value?.ToString(), '-');
            
            if (ssn.Length == 9)
                return $"{ssn.Substring(0, 3)}-{ssn.Substring(3, 2)}-{ssn.Substring(5)}";
            
            return ssn;
        };
    }

    /// <summary>
    /// Masks an SSN showing only last 4 digits
    /// </summary>
    /// <returns>Formatter function that masks SSNs</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.SocialSecurityNumberMasked();
    /// formatter("123456789")    // Returns "***-**-6789"
    /// formatter("123-45-6789")  // Returns "***-**-6789"
    /// </example>
    public static Func<object?, string> SocialSecurityNumberMasked()
    {
        return value =>
        {
            var ssn = FormatterHelpers.CleanString(value?.ToString(), '-');
            
            if (ssn.Length == 9)
                return $"***-**-{ssn.Substring(5)}";
            
            return ssn;
        };
    }

    #endregion

    #region Boolean Formatters

    /// <summary>
    /// Formats a boolean as Yes/No
    /// </summary>
    /// <returns>Formatter function that formats booleans as Yes/No</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.YesNo();
    /// formatter(true)   // Returns "Yes"
    /// formatter(false)  // Returns "No"
    /// </example>
    public static Func<object?, string> YesNo()
    {
        return value => value is bool b ? (b ? "Yes" : "No") : string.Empty;
    }

    /// <summary>
    /// Formats a boolean as ✓/✗
    /// </summary>
    /// <returns>Formatter function that formats booleans as check/cross marks</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.CheckMark();
    /// formatter(true)   // Returns "✓"
    /// formatter(false)  // Returns "✗"
    /// </example>
    public static Func<object?, string> CheckMark()
    {
        return value => value is bool b ? (b ? "✓" : "✗") : string.Empty;
    }

    /// <summary>
    /// Formats a boolean as Active/Inactive
    /// </summary>
    /// <returns>Formatter function that formats booleans as Active/Inactive</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.ActiveInactive();
    /// formatter(true)   // Returns "Active"
    /// formatter(false)  // Returns "Inactive"
    /// </example>
    public static Func<object?, string> ActiveInactive()
    {
        return value => value is bool b ? (b ? "Active" : "Inactive") : string.Empty;
    }

    /// <summary>
    /// Formats a boolean as Enabled/Disabled
    /// </summary>
    /// <returns>Formatter function that formats booleans as Enabled/Disabled</returns>
    /// <example>
    /// var formatter = SpecializedFormatters.EnabledDisabled();
    /// formatter(true)   // Returns "Enabled"
    /// formatter(false)  // Returns "Disabled"
    /// </example>
    public static Func<object?, string> EnabledDisabled()
    {
        return value => value is bool b ? (b ? "Enabled" : "Disabled") : string.Empty;
    }

    #endregion
}
