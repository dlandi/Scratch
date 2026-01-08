using System.Linq;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
using QuickGridTest01.ComposableColumns.Features.Formatting;
using QuickGridTest01.ComposableColumns.Features.Styling;

namespace QuickGridTest01.Pages;

public partial class ComposableStylingDemo
{
    private IQueryable<Product> _products = default!;
    private IColumnFeature<Product>[] _stockFeatures = default!;
    private IColumnFeature<Product>[] _statusFeatures = default!;
    private IColumnFeature<Product>[] _priceFeatures = default!;

    protected override void OnInitialized()
    {
        _products = ComposableDemoData.GetProducts();
        InitializeFeatures();
    }

    private void InitializeFeatures()
    {
        _stockFeatures =
        [
            new ConditionalCssFeature<Product, int>
            {
                BaseClass = "stock-cell",
                Rules =
                [
                    new() { Condition = v => v == 0, CssClass = "stock-empty" },
                    new() { Condition = v => v < 10, CssClass = "stock-low" },
                    new() { Condition = v => v >= 10, CssClass = "stock-ok" }
                ]
            }
        ];

        _statusFeatures =
        [
            new IconFeature<Product, ProductStatus>
            {
                IconMapper = status => status switch
                {
                    ProductStatus.Active => "bi bi-check-circle-fill",
                    ProductStatus.Discontinued => "bi bi-x-circle-fill",
                    ProductStatus.ComingSoon => "bi bi-clock-fill",
                    _ => "bi bi-question-circle"
                },
                ColorMapper = status => status switch
                {
                    ProductStatus.Active => "#22c55e",
                    ProductStatus.Discontinued => "#ef4444",
                    ProductStatus.ComingSoon => "#f59e0b",
                    _ => "#6b7280"
                }
            }
        ];

        _priceFeatures =
        [
            new FormatStringFeature<Product, decimal> { Format = "C2" },
            new TooltipFeature<Product, decimal>
            {
                TooltipMapper = price => $"Price: {price:C2} (before tax)"
            }
        ];
    }
}
