using System.Linq;
using Microsoft.AspNetCore.Components;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
using QuickGridTest01.ComposableColumns.Features.Editing;
using EditKind = QuickGridTest01.ComposableColumns.Features.Editing.EditorKind;

namespace QuickGridTest01.Pages;

public partial class ComposableEditEventsDemo
{
    private IQueryable<EditableProduct> _editableProducts = default!;
    private ComposableGrid<EditableProduct>? _eventDemoGrid;

    private EventPanelPlacement _selectedPlacement = EventPanelPlacement.Right;
    private int _commitCount;
    private int _cancelCount;
    private int _validationErrorCount;

    private IColumnFeature<EditableProduct>[] _eventDemoNameFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoPriceFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoStockFeatures = default!;
    private IColumnFeature<EditableProduct>[] _eventDemoStatusFeatures = default!;

    protected override void OnInitialized()
    {
        _editableProducts = ComposableDemoData.GetEditableProducts();
        InitializeEventDemoFeatures();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && _eventDemoGrid?.EditEventStream is not null)
        {
            _eventDemoGrid.EditEventStream.EventPublished += HandleEventStreamPublished;
        }
    }

    private void InitializeEventDemoFeatures()
    {
        _eventDemoNameFeatures =
        [
            new InlineEditingFeature<EditableProduct, string>
            {
                Editor = EditKind.Text,
                ItemKey = p => p.Id,
                ShowValidationErrors = true,
                ShowEvents = true,
                Validators = [new RequiredStringValidator()]
            }
        ];

        _eventDemoPriceFeatures =
        [
            new InlineEditingFeature<EditableProduct, decimal>
            {
                Editor = EditKind.Currency,
                ItemKey = p => p.Id,
                Step = "0.01",
                ShowValidationErrors = true,
                ShowEvents = true,
                Validators = [new RangeValidator<decimal> { Minimum = 0.01m, Maximum = 10000m }]
            }
        ];

        _eventDemoStockFeatures =
        [
            new InlineEditingFeature<EditableProduct, int>
            {
                Editor = EditKind.Number,
                ItemKey = p => p.Id,
                Min = "0",
                ShowValidationErrors = true,
                ShowEvents = true,
                Validators = [new MinValueValidator<int> { Minimum = 0 }]
            }
        ];

        _eventDemoStatusFeatures =
        [
            new InlineEditingFeature<EditableProduct, ProductStatus>
            {
                Editor = EditKind.Select,
                ItemKey = p => p.Id,
                ShowEvents = true
            }
        ];
    }

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
