using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Core;
using QuickGridTest01.ComposableColumns.Features.Formatting;
using QuickGridTest01.ComposableColumns.Features.Styling;
using QuickGridTest01.ComposableColumns.Features.Editing;
using QuickGridTest01.ComposableColumns.Features.Filtering;
using EditKind = QuickGridTest01.ComposableColumns.Features.Editing.EditorKind;

namespace QuickGridTest01.Pages;

public partial class ComposableColumnDemo
{
    private IQueryable<Product> _products = default!;
    private IQueryable<EditableProduct> _editableProducts = default!;
    private IQueryable<FeaturePriorityInfo> _featurePriorities = default!;
    private string _lastEditMessage = "";
    private string _filterMessage = "";

    // Feature collections for the demo
    private IColumnFeature<Product>[] _stockFeatures = default!;
    private IColumnFeature<Product>[] _statusFeatures = default!;
    private IColumnFeature<Product>[] _priceFeatures = default!;

    // Filter state - using simple values instead of FilterFeature POCOs
    private string? _nameFilterValue;
    private decimal _priceFilterValue;
    private int _stockFilterValue;
    private IFilterOperator<string>? _nameFilterOperator;
    private IFilterOperator<decimal>? _priceFilterOperator;
    private IFilterOperator<int>? _stockFilterOperator;
    private bool _hasPriceFilter;
    private bool _hasStockFilter;

    private bool _hasActiveFilters => 
        !string.IsNullOrEmpty(_nameFilterValue) || 
        _hasPriceFilter || 
        _hasStockFilter;

    // Filtered products query - applies filters inline
    private IQueryable<Product> FilteredProducts
    {
        get
        {
            var result = _products;
            
            // Apply name filter
            if (!string.IsNullOrEmpty(_nameFilterValue) && _nameFilterOperator is not null)
            {
                result = _nameFilterOperator.Apply(result, p => p.Name, _nameFilterValue);
            }
            
            // Apply price filter
            if (_hasPriceFilter && _priceFilterOperator is not null)
            {
                result = _priceFilterOperator.Apply(result, p => p.Price, _priceFilterValue);
            }
            
            // Apply stock filter
            if (_hasStockFilter && _stockFilterOperator is not null)
            {
                result = _stockFilterOperator.Apply(result, p => p.Stock, _stockFilterValue);
            }
            
            return result;
        }
    }

    // Editing feature collections
    private IColumnFeature<EditableProduct>[] _nameEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _priceEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _stockEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _statusEditFeatures = default!;

    protected override void OnInitialized()
    {
        InitializeProducts();
        InitializeFeatures();
        InitializeEditingFeatures();
        InitializeFeaturePriorities();
    }

    private void InitializeProducts()
    {
        _products = new List<Product>
        {
            new(1, "Widget Pro", 299.99m, 45, ProductStatus.Active, DateTime.Now.AddDays(-5)),
            new(2, "Gadget Max", 149.50m, 8, ProductStatus.Active, DateTime.Now.AddDays(-2)),
            new(3, "Tool Basic", 49.99m, 0, ProductStatus.Discontinued, DateTime.Now.AddDays(-30)),
            new(4, "Device Ultra", 599.00m, 120, ProductStatus.Active, DateTime.Now.AddDays(-1)),
            new(5, "Component X", 25.00m, 3, ProductStatus.ComingSoon, DateTime.Now.AddDays(-10)),
            new(6, "Assembly Kit", 89.99m, 67, ProductStatus.Active, DateTime.Now.AddDays(-7))
        }.AsQueryable();

        _editableProducts = new List<EditableProduct>
        {
            new() { Id = 1, Name = "Widget Pro", Price = 299.99m, Stock = 45, Status = ProductStatus.Active },
            new() { Id = 2, Name = "Gadget Max", Price = 149.50m, Stock = 8, Status = ProductStatus.Active },
            new() { Id = 3, Name = "Tool Basic", Price = 49.99m, Stock = 0, Status = ProductStatus.Discontinued },
            new() { Id = 4, Name = "Device Ultra", Price = 599.00m, Stock = 120, Status = ProductStatus.Active }
        }.AsQueryable();
    }

    // Filter event handlers
    private void OnNameFilterApplied(FilterInputEventArgs<string> args)
    {
        _nameFilterValue = args.Value;
        _nameFilterOperator = args.Operator;
        UpdateFilterMessage("Name", args.HasValue, args.Operator?.Name, args.Value);
    }

    private void OnPriceFilterApplied(FilterInputEventArgs<decimal> args)
    {
        _priceFilterValue = args.Value;
        _priceFilterOperator = args.Operator;
        _hasPriceFilter = args.HasValue;
        UpdateFilterMessage("Price", args.HasValue, args.Operator?.Name, args.HasValue ? args.Value.ToString("C2") : null);
    }

    private void OnStockFilterApplied(FilterInputEventArgs<int> args)
    {
        _stockFilterValue = args.Value;
        _stockFilterOperator = args.Operator;
        _hasStockFilter = args.HasValue;
        UpdateFilterMessage("Stock", args.HasValue, args.Operator?.Name, args.HasValue ? args.Value.ToString() : null);
    }

    private void UpdateFilterMessage(string field, bool hasValue, string? operatorName, string? value)
    {
        if (hasValue && !string.IsNullOrEmpty(value))
        {
            _filterMessage = $"Filtering {field} by {operatorName}: {value}";
        }
        else
        {
            _filterMessage = "";
        }
    }

    private void ClearAllFilters()
    {
        _nameFilterValue = null;
        _priceFilterValue = 0;
        _stockFilterValue = 0;
        _hasPriceFilter = false;
        _hasStockFilter = false;
        _filterMessage = "All filters cleared";
    }

    private void InitializeFeatures()
    {
        // Stock level with conditional CSS
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

        // Status with icon mapping
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

        // Price with tooltip
        _priceFeatures =
        [
            new FormatStringFeature<Product, decimal> { Format = "C2" },
            new TooltipFeature<Product, decimal>
            {
                TooltipMapper = price => $"Price: {price:C2} (before tax)"
            }
        ];
    }

    private void InitializeEditingFeatures()
    {
        // Name with text editing and validation
        _nameEditFeatures =
        [
            new InlineEditingFeature<EditableProduct, string>
            {
                Editor = EditKind.Text,
                Placeholder = "Enter name...",
                ItemKey = p => p.Id,
                ShowValidationErrors = true,
                Validators = [new RequiredStringValidator(), new StringLengthValidator { MinLength = 2, MaxLength = 50 }],
                OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, string>>(
                    this, args => HandleValueChanged($"Name changed from '{args.OldValue}' to '{args.NewValue}'")),
                OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, string>>(
                    this, args => HandleValidationCompleted("Name", args.IsValid, args.Errors))
            }
        ];

        // Price with currency editing and range validation
        _priceEditFeatures =
        [
            new InlineEditingFeature<EditableProduct, decimal>
            {
                Editor = EditKind.Currency,
                ItemKey = p => p.Id,
                Step = "0.01",
                ShowValidationErrors = true,
                Validators = [new RangeValidator<decimal> { Minimum = 0.01m, Maximum = 10000m }],
                OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, decimal>>(
                    this, args => HandleValueChanged($"Price changed from {args.OldValue:C2} to {args.NewValue:C2}")),
                OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, decimal>>(
                    this, args => HandleValidationCompleted("Price", args.IsValid, args.Errors))
            }
        ];

        // Stock with number editing and min value validation
        _stockEditFeatures =
        [
            new InlineEditingFeature<EditableProduct, int>
            {
                Editor = EditKind.Number,
                ItemKey = p => p.Id,
                Min = "0",
                ShowValidationErrors = true,
                Validators = [new MinValueValidator<int> { Minimum = 0 }],
                OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, int>>(
                    this, args => HandleValueChanged($"Stock changed from {args.OldValue} to {args.NewValue}")),
                OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, int>>(
                    this, args => HandleValidationCompleted("Stock", args.IsValid, args.Errors))
            }
        ];

        // Status with select dropdown - no validation needed for discrete selections
        _statusEditFeatures =
        [
            new InlineEditingFeature<EditableProduct, ProductStatus>
            {
                Editor = EditKind.Select,
                ItemKey = p => p.Id,
                OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, ProductStatus>>(
                    this, args => HandleValueChanged($"Status changed from {args.OldValue} to {args.NewValue}"))
            }
        ];
    }

    private void HandleValueChanged(string message)
    {
        _lastEditMessage = message;
        StateHasChanged();
    }

    private void HandleValidationCompleted(string field, bool isValid, List<string> errors)
    {
        if (isValid)
        {
            _lastEditMessage = $"{field}: Valid ?";
        }
        else
        {
            _lastEditMessage = $"{field}: Invalid - {string.Join(", ", errors)}";
        }
        StateHasChanged();
    }

    private void InitializeFeaturePriorities()
    {
        _featurePriorities = new List<FeaturePriorityInfo>
        {
            new("Infrastructure", 0, "Property expression, compiled accessor"),
            new("Core", 100, "Type traits, auto-title inference"),
            new("Formatting", 200, "Format string, custom formatter, culture"),
            new("Styling", 300, "Conditional CSS, icons, tooltips"),
            new("Filtering", 400, "Type-aware filters, inline filters"),
            new("Editing", 500, "Inline editing, edit state, debounce"),
            new("Validation", 600, "Validators, data annotations"),
            new("Events", 700, "Value changed, state changed, before edit"),
            new("Performance", 800, "Memoization, minimal DOM, set key"),
            new("Final", 1000, "Final wrapper features")
        }.AsQueryable();
    }

    // Demo models
    public record Product(int Id, string Name, decimal Price, int Stock, ProductStatus Status, DateTime LastUpdated);

    // Editable version with mutable properties
    public class EditableProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
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

    public record FeaturePriorityInfo(string Category, int Priority, string Description);
}
