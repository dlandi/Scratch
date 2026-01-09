namespace QuickGridTest01.ComposableColumns.Core;

internal interface IColumnFeatureProvider<TGridItem>
{
    IReadOnlyList<IColumnFeature<TGridItem>> GetAllFeatures();
}
