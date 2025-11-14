namespace QuickGridTest01.FormattedValue.Demo.Models;

/// <summary>
/// Financial transaction model demonstrating currency and accounting formats.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Gets or sets the transaction unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the transaction date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the transaction description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the transaction amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the account balance after transaction.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the currency code (USD, EUR, GBP, etc.).
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets the transaction type (Deposit, Withdrawal, Transfer, etc.).
    /// </summary>
    public string Type { get; set; } = string.Empty;
}
