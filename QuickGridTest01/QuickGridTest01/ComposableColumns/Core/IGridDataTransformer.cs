namespace QuickGridTest01.ComposableColumns.Core;

public interface IGridDataTransformer<TGridItem>
{
    IQueryable<TGridItem> TransformItems(IQueryable<TGridItem> items);
}
