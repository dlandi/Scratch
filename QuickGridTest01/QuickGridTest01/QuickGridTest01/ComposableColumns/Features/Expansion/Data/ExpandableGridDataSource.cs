using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Expansion.Data;

/// <summary>
/// Manages a data source with dynamic spacer row insertion for expanded overlays.
/// Wraps the original data and injects spacer rows after expanded items.
/// </summary>
/// <typeparam name="TGridItem">Entity type implementing IRowIdentifiable</typeparam>
public class ExpandableGridDataSource<TGridItem> where TGridItem : class, IRowIdentifiable, new()
{
    private readonly List<TGridItem> _originalData;
    private readonly Dictionary<int, int> _expandedRowSpans = new(); // parentId -> spacer count
    private List<TGridItem>? _cachedData;
    private bool _isDirty = true;

    public event Action? OnDataChanged;

    public ExpandableGridDataSource(IEnumerable<TGridItem> originalData)
    {
        _originalData = originalData.ToList();
    }

    public IQueryable<TGridItem> Items
    {
        get
        {
            if (_isDirty || _cachedData == null)
            {
                RebuildCache();
            }

            return _cachedData!.AsQueryable();
        }
    }

    public IReadOnlyList<TGridItem> OriginalData => _originalData;

    public IReadOnlyCollection<int> ExpandedRowIds => _expandedRowSpans.Keys;

    public bool IsExpanded(int rowId) => _expandedRowSpans.ContainsKey(rowId);

    public void ExpandRow(int rowId, int spacerCount)
    {
        if (rowId <= 0)
            throw new ArgumentOutOfRangeException(nameof(rowId), "rowId must be greater than 0.");

        if (spacerCount < 0)
            throw new ArgumentOutOfRangeException(nameof(spacerCount), "spacerCount must be greater than or equal to 0.");

        if (spacerCount == 0)
            return;

        // Deterministic behavior for repeated expands:
        // always overwrite the spacer block size for the row.
        _expandedRowSpans[rowId] = spacerCount + 1;
        MarkDirty();
    }

    public void CollapseRow(int rowId)
    {
        if (_expandedRowSpans.Remove(rowId))
        {
            MarkDirty();
        }
    }

    public void CollapseAll()
    {
        if (_expandedRowSpans.Count > 0)
        {
            _expandedRowSpans.Clear();
            MarkDirty();
        }
    }

    public void UpdateData(IEnumerable<TGridItem> newData)
    {
        _originalData.Clear();
        _originalData.AddRange(newData);
        MarkDirty();
    }

    public TGridItem? GetById(int id)
    {
        if (id <= 0) return null;
        return _originalData.FirstOrDefault(x => x.Id == id);
    }

    private void MarkDirty()
    {
        _isDirty = true;
        OnDataChanged?.Invoke();
    }

    private void RebuildCache()
    {
        _cachedData = new List<TGridItem>(_originalData.Count + _expandedRowSpans.Values.Sum());

        foreach (var item in _originalData)
        {
            _cachedData.Add(item);

            if (_expandedRowSpans.TryGetValue(item.Id, out int spacerCount))
            {
                foreach (var spacer in SpacerRowFactory.CreateSpacers<TGridItem>(item.Id, spacerCount))
                {
                    _cachedData.Add(spacer);
                }
            }
        }

        _isDirty = false;
    }
}
