using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickGridTest01.ComposableColumns.Demos;

internal static class ComposableDemoData
{
    public static IQueryable<Product> GetProducts() => new List<Product>
    {
        new(1, "Widget Pro", 299.99m, 45, ProductStatus.Active, DateTime.Now.AddDays(-5), true),
        new(2, "Gadget Max", 149.50m, 8, ProductStatus.Active, DateTime.Now.AddDays(-2), true),
        new(3, "Tool Basic", 49.99m, 0, ProductStatus.Discontinued, DateTime.Now.AddDays(-30), false),
        new(4, "Device Ultra", 599.00m, 120, ProductStatus.Active, DateTime.Now.AddDays(-1), true),
        new(5, "Component X", 25.00m, 3, ProductStatus.ComingSoon, DateTime.Now.AddDays(-10), true),
        new(6, "Assembly Kit", 89.99m, 67, ProductStatus.Active, DateTime.Now.AddDays(-7), true)
    }.AsQueryable();

    public static IQueryable<EditableProduct> GetEditableProducts() => new List<EditableProduct>
    {
        new() { Id = 1, Name = "Widget Pro", Price = 299.99m, Stock = 45, Status = ProductStatus.Active },
        new() { Id = 2, Name = "Gadget Max", Price = 149.50m, Stock = 8, Status = ProductStatus.Active },
        new() { Id = 3, Name = "Tool Basic", Price = 49.99m, Stock = 0, Status = ProductStatus.Discontinued },
        new() { Id = 4, Name = "Device Ultra", Price = 599.00m, Stock = 120, Status = ProductStatus.Active }
    }.AsQueryable();
}

public record Product(int Id, string Name, decimal Price, int Stock, ProductStatus Status, DateTime LastUpdated, bool InStock);

public class EditableProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ProductStatus Status { get; set; }
}

public enum ProductStatus
{
    Active,
    Discontinued,
    ComingSoon
}
