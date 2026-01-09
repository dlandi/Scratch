using System;
using System.Linq;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
using QuickGridTest01.ComposableColumns.Features.Filtering;

namespace QuickGridTest01.Pages;

public partial class ComposableFilteringDemo
{
    private IQueryable<Product> _products = default!;
    private ComposableGrid<Product>? _filterGrid;

    private IColumnFeature<Product>[] _nameFilterFeatures = default!;
    private IColumnFeature<Product>[] _priceFilterFeatures = default!;
    private IColumnFeature<Product>[] _stockFilterFeatures = default!;
    private IColumnFeature<Product>[] _dateFilterFeatures = default!;
    private IColumnFeature<Product>[] _inStockFilterFeatures = default!;

    protected override void OnInitialized()
    {
        _products = ComposableDemoData.GetProducts();
        InitializeFilterFeatures();
    }

    private void InitializeFilterFeatures()
    {
        _nameFilterFeatures = [new FilterFeature<Product, string> { Placeholder = "Name..." }];
        _priceFilterFeatures = [new FilterFeature<Product, decimal> { Placeholder = "Price..." }];
        _stockFilterFeatures = [new FilterFeature<Product, int> { Placeholder = "Stock..." }];
        _dateFilterFeatures = [new FilterFeature<Product, DateTime> { Placeholder = "Date..." }];
        _inStockFilterFeatures = [new FilterFeature<Product, bool>()];
    }
}
