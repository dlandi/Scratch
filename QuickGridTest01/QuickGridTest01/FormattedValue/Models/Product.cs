namespace QuickGridTest01.FormattedValue.Demo.Models;

/// <summary>
/// Product inventory model demonstrating various numeric and specialized formats.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the product unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage (0-100).
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Gets or sets the product weight in grams.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the package file size in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the last restocked date.
    /// </summary>
    public DateTime LastRestocked { get; set; }

    /// <summary>
    /// Gets the discounted price calculated from Price and DiscountPercent.
    /// </summary>
    public decimal DiscountedPrice => Price * (1 - DiscountPercent / 100);
}
