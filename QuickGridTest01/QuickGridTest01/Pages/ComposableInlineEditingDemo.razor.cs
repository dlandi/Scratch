using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
using QuickGridTest01.ComposableColumns.Features.Editing;
using EditKind = QuickGridTest01.ComposableColumns.Features.Editing.EditorKind;

namespace QuickGridTest01.Pages;

public partial class ComposableInlineEditingDemo
{
    private IQueryable<EditableProduct> _editableProducts = default!;
    private string _lastEditMessage = string.Empty;

    private IColumnFeature<EditableProduct>[] _nameEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _priceEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _stockEditFeatures = default!;
    private IColumnFeature<EditableProduct>[] _statusEditFeatures = default!;

    protected override void OnInitialized()
    {
        _editableProducts = ComposableDemoData.GetEditableProducts();
        InitializeEditingFeatures();
    }

    private void InitializeEditingFeatures()
    {
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
        _lastEditMessage = isValid
            ? $"{field}: Valid ✅"
            : $"{field}: Invalid - {string.Join(", ", errors)}";
        StateHasChanged();
    }
}
