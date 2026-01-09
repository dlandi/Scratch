using System.Linq;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Demos;
using QuickGridTest01.ComposableColumns.Features.Editing;
using EditKind = QuickGridTest01.ComposableColumns.Features.Editing.EditorKind;

namespace QuickGridTest01.Pages;

public partial class ComposableNewEditorsDemo
{
    private IQueryable<EditableProduct> _editableProducts = default!;
    private IColumnFeature<EditableProduct>[] _autoNameFeatures = default!;
    private IColumnFeature<EditableProduct>[] _autoPriceFeatures = default!;
    private IColumnFeature<EditableProduct>[] _radioStatusFeatures = default!;

    protected override void OnInitialized()
    {
        _editableProducts = ComposableDemoData.GetEditableProducts();
        InitializeNewEditorFeatures();
    }

    private void InitializeNewEditorFeatures()
    {
        _autoNameFeatures =
        [
            new InlineEditingFeature<EditableProduct, string>
            {
                Editor = EditKind.Auto,
                ItemKey = p => p.Id,
                Placeholder = "Auto-detected text..."
            }
        ];

        _autoPriceFeatures =
        [
            new InlineEditingFeature<EditableProduct, decimal>
            {
                Editor = EditKind.Auto,
                ItemKey = p => p.Id
            }
        ];

        _radioStatusFeatures =
        [
            new InlineEditingFeature<EditableProduct, ProductStatus>
            {
                Editor = EditKind.RadioGroup,
                ItemKey = p => p.Id,
                OptionText = s => s switch
                {
                    ProductStatus.Active => "✅ Active",
                    ProductStatus.Discontinued => "❌ Discontinued",
                    ProductStatus.ComingSoon => "⏳ Coming Soon",
                    _ => s.ToString()
                }
            }
        ];
    }
}
