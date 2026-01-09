using System.Linq;
using QuickGridTest01.ComposableColumns.Demos;

namespace QuickGridTest01.Pages;

public partial class ComposableBasicDemo
{
    private IQueryable<Product> _products = default!;

    protected override void OnInitialized()
    {
        _products = ComposableDemoData.GetProducts();
    }
}
