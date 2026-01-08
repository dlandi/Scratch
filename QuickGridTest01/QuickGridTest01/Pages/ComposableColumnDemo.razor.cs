using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
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
    private string _lastEditMessage = "";

    // Grid reference for filtering section
    private ComposableGrid<Product>? _filterGrid;
    
    // Grid reference for event stream demo section
    private ComposableGrid<EditableProduct>? _eventDemoGrid;

    // Event stream demo state
    private EventPanelPlacement _selectedPlacement = EventPanelPlacement.Right;
    private int _commitCount;
    private int _cancelCount;
    private int _validationErrorCount;

    // Feature collections for styling demo
    private IColumnFeature<Product>[] _stockFeatures = default!;
    private IColumnFeature<Product>[] _statusFeatures = default!;
    private IColumnFeature<Product>[] _priceFeatures = default!;

    // Filter feature collections - grid auto-renders toolbar when these are present
    private IColumnFeature<Product>[] _nameFilterFeatures = default!;
    private IColumnFeature<Product>[] _priceFilterFeatures = default!;
    private IColumnFeature<Product>[] _stockFilterFeatures = default!;
    private IColumnFeature<Product>[] _dateFilterFeatures = default!;
    private IColumnFeature<Product>[] _inStockFilterFeatures = default!;

    // Editing feature collections
    private IColumnFeature<EditableProduct>[] _nameEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _priceEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _stockEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _statusEditFeatures = default!;

    // New editor type demo features
    private IColumnFeature<EditableProduct>[] _autoNameFeatures = default!;
    private IColumnFeature<EditableProduct>[] _autoPriceFeatures = default!;
    private IColumnFeature<EditableProduct>[] _radioStatusFeatures = default!;
    
    // Event stream demo features (ShowEvents enabled)
    private IColumnFeature<EditableProduct>[] _eventDemoNameFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoPriceFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoStockFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoStatusFeatures = default!;

    protected override void OnInitialized()
    {
        InitializeProducts();
        InitializeFeatures();
        InitializeFilterFeatures();
        InitializeEditingFeatures();
        InitializeNewEditorFeatures();
        InitializeEventDemoFeatures();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && _eventDemoGrid?.EditEventStream is not null)
        {
            _eventDemoGrid.EditEventStream.EventPublished += HandleEventStreamPublished;
        }
    }

    private void InitializeProducts()
    {
        _products = ComposableDemoData.GetProducts();
        _editableProducts = ComposableDemoData.GetEditableProducts();
    }

    private void InitializeFilterFeatures()
    {
        // Simply add FilterFeature to columns - grid handles the rest
        _nameFilterFeatures = [new FilterFeature<Product, string> { Placeholder = "Name..." }];
        _priceFilterFeatures = [new FilterFeature<Product, decimal> { Placeholder = "Price..." }];
        _stockFilterFeatures = [new FilterFeature<Product, int> { Placeholder = "Stock..." }];
        _dateFilterFeatures = [new FilterFeature<Product, DateTime> { Placeholder = "Date..." }];
        _inStockFilterFeatures = [new FilterFeature<Product, bool>()];
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

        private void InitializeNewEditorFeatures()
        {
            // Auto-detect editor from property type
            _autoNameFeatures =
            [
                new InlineEditingFeature<EditableProduct, string>
                {
                    Editor = EditKind.Auto, // Detects Text from string
                    ItemKey = p => p.Id,
                    Placeholder = "Auto-detected text..."
                }
            ];

            _autoPriceFeatures =
            [
                new InlineEditingFeature<EditableProduct, decimal>
                {
                    Editor = EditKind.Auto, // Detects Number from decimal
                    ItemKey = p => p.Id
                }
            ];

            // RadioGroup for enum values
            _radioStatusFeatures =
            [
                new InlineEditingFeature<EditableProduct, ProductStatus>
                {
                    Editor = EditKind.RadioGroup,
                    ItemKey = p => p.Id,
                    OptionText = s => s switch
                    {
                        ProductStatus.Active => "? Active",
                        ProductStatus.Discontinued => "? Discontinued",
                        ProductStatus.ComingSoon => "? Coming Soon",
                        _ => s.ToString()
                    }
                }
            ];
        }

        private void InitializeEventDemoFeatures()
        {
            // Name with event display on edit
            _eventDemoNameFeatures =
            [
                new InlineEditingFeature<EditableProduct, string>
                {
                    Editor = EditKind.Text,
                    Placeholder = "Enter name...",
                    ItemKey = p => p.Id,
                    ShowValidationErrors = true,
                    ShowEvents = true, // Show events in the grid
                    Validators = [new RequiredStringValidator(), new StringLengthValidator { MinLength = 2, MaxLength = 50 }],
                    OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, string>>(
                        this, args => HandleValueChanged($"Name changed from '{args.OldValue}' to '{args.NewValue}'")),
                    OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, string>>(
                        this, args => HandleValidationCompleted("Name", args.IsValid, args.Errors))
                }
            ];

            // Price with event display on edit
            _eventDemoPriceFeatures =
            [
                new InlineEditingFeature<EditableProduct, decimal>
                {
                    Editor = EditKind.Currency,
                    ItemKey = p => p.Id,
                    Step = "0.01",
                    ShowValidationErrors = true,
                    ShowEvents = true, // Show events in the grid
                    Validators = [new RangeValidator<decimal> { Minimum = 0.01m, Maximum = 10000m }],
                    OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, decimal>>(
                        this, args => HandleValueChanged($"Price changed from {args.OldValue:C2} to {args.NewValue:C2}")),
                    OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, decimal>>(
                        this, args => HandleValidationCompleted("Price", args.IsValid, args.Errors))
                }
            ];

            // Stock with event display on edit
            _eventDemoStockFeatures =
            [
                new InlineEditingFeature<EditableProduct, int>
                {
                    Editor = EditKind.Number,
                    ItemKey = p => p.Id,
                    Min = "0",
                    ShowValidationErrors = true,
                    ShowEvents = true, // Show events in the grid
                    Validators = [new MinValueValidator<int> { Minimum = 0 }],
                    OnValueChanged = EventCallback.Factory.Create<ValueChangedEventArgs<EditableProduct, int>>(
                        this, args => HandleValueChanged($"Stock changed from {args.OldValue} to {args.NewValue}")),
                    OnValidationCompleted = EventCallback.Factory.Create<ValidationCompletedEventArgs<EditableProduct, int>>(
                        this, args => HandleValidationCompleted("Stock", args.IsValid, args.Errors))
                }
            ];

            // Status with event display on edit
            _eventDemoStatusFeatures =
            [
                new InlineEditingFeature<EditableProduct, ProductStatus>
                {
                    Editor = EditKind.Select,
                    ItemKey = p => p.Id,
                    ShowEvents = true, // Show events in the grid
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

    // Event counter tracking methods
    private void HandleEventStreamPublished(EditEventBase evt)
    {
        switch (evt)
        {
            case EditCommittedEvent:
                _commitCount++;
                break;
            case EditCancelledEvent:
                _cancelCount++;
                break;
            case ValidationFailedEvent:
                _validationErrorCount++;
                break;
        }
        StateHasChanged();
    }

    private void ResetEventCounters()
    {
        _commitCount = 0;
        _cancelCount = 0;
        _validationErrorCount = 0;
        _eventDemoGrid?.EditEventStream?.Clear();
        StateHasChanged();
    }

}
