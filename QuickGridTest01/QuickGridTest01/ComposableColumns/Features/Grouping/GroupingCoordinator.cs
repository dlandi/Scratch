using QuickGridTest01.ComposableColumns.Core.Diagnostics;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;
using QuickGridTest01.RowColumn.Core;
using System.Linq.Expressions;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed class GroupingCoordinator<TGridItem> : IDisposable
    where TGridItem : class
{
    private static readonly Func<TGridItem, TGridItem> Clone = CreateCloner();
    private readonly Dictionary<string, IGroupingFeature<TGridItem>> _registered = new(StringComparer.Ordinal);

    private readonly Dictionary<object, int> _keyToGroupId = new();
    private readonly Dictionary<int, object?> _groupIdToKey = new();
    private readonly Dictionary<int, int> _groupIdToItemCount = new();
    private int? _nullKeyGroupId;
    private int _nextGroupId = 1;

    internal IReadOnlyCollection<object?> GetKnownGroupKeys()
    {
        // Snapshot to avoid exposing internal dictionary enumerator.
        var keys = new List<object?>(_keyToGroupId.Keys);
        if (_nullKeyGroupId.HasValue)
            keys.Add(null);
        return keys;
    }

    public string? HeaderHostColumnId { get; private set; }

    public IGroupingFeature<TGridItem>? ActiveGrouping { get; private set; }

    internal IReadOnlyList<string> GetRegisteredColumnIds()
    {
        // Dictionary preserves insertion order in modern .NET.
        return _registered.Keys.ToList();
    }

    public void SetActiveGrouping(string? columnId)
    {
        if (string.IsNullOrWhiteSpace(columnId))
        {
            ActiveGrouping = null;
            QgDebugLog.Write($"Coordinator.SetActiveGrouping: disabled, coord={GetHashCode()}");
            return;
        }

        if (!_registered.TryGetValue(columnId, out var feature))
            throw new InvalidOperationException($"Grouping columnId '{columnId}' is not registered.");

        ActiveGrouping = feature;
        QgDebugLog.Write($"Coordinator.SetActiveGrouping: activeColumnId='{columnId}', coord={GetHashCode()}, host='{HeaderHostColumnId ?? "<null>"}'");
    }

    /// <summary>
    /// Gets the group key for a given groupId. Returns null if the groupId is not found or represents the null key.
    /// </summary>
    public object? GetGroupKey(int groupId)
    {
        return _groupIdToKey.TryGetValue(groupId, out var key) ? key : null;
    }

    /// <summary>
    /// Gets the item count for a given groupId. Returns 0 if the groupId is not found.
    /// </summary>
    public int GetGroupItemCount(int groupId)
    {
        return _groupIdToItemCount.TryGetValue(groupId, out var count) ? count : 0;
    }

    /// <summary>
    /// Returns whether a group is expanded, given its groupId.
    /// </summary>
    public bool IsGroupExpandedById(int groupId)
    {
        if (ActiveGrouping is null)
            return false;

        if (!_groupIdToKey.TryGetValue(groupId, out var key))
            return false;

        return ActiveGrouping.IsGroupExpanded(key);
    }

    public void RegisterColumn(string columnId, IGroupingFeature<TGridItem> feature)
    {
        ArgumentNullException.ThrowIfNull(columnId);
        ArgumentNullException.ThrowIfNull(feature);

        // Idempotent registration: first wins. Subsequent attempts are ignored.
        if (_registered.ContainsKey(columnId))
        {
            QgDebugLog.Write($"Coordinator.RegisterColumn DUPLICATE ignored: coord={GetHashCode()}, columnId='{columnId}', existingHost='{HeaderHostColumnId ?? "<null>"}', active={(ActiveGrouping is null ? "<null>" : "set")}");
            return;
        }

        _registered.Add(columnId, feature);

        var prevHost = HeaderHostColumnId;
        HeaderHostColumnId ??= columnId;

        if (!string.Equals(prevHost, HeaderHostColumnId, StringComparison.Ordinal))
        {
            QgDebugLog.Write($"Coordinator.RegisterColumn pinned host: coord={GetHashCode()}, host='{HeaderHostColumnId}', columnId='{columnId}'");
        }

        if (ActiveGrouping is null && feature.IsActive)
        {
            ActiveGrouping = feature;
            QgDebugLog.Write($"Coordinator.RegisterColumn set active: coord={GetHashCode()}, activeColumnId='{columnId}', host='{HeaderHostColumnId ?? "<null>"}'");
        }
        else
        {
            QgDebugLog.Write($"Coordinator.RegisterColumn registered: coord={GetHashCode()}, columnId='{columnId}', featureActive={feature.IsActive}, host='{HeaderHostColumnId ?? "<null>"}', active={(ActiveGrouping is null ? "<null>" : "set")}");
        }
    }

    public IQueryable<TGridItem> TransformItems(IQueryable<TGridItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (ActiveGrouping is null)
        {
            QgDebugLog.Write($"TransformItems: no active grouping, returning original items");
            return items;
        }

        if (ActiveGrouping.GroupHeaderSlotSpan < 1)
            throw new ArgumentOutOfRangeException(nameof(ActiveGrouping.GroupHeaderSlotSpan), "GroupHeaderSlotSpan must be >= 1.");

        // Materialize once; grouping is a transformation stage.
        var source = items.ToList();
        QgDebugLog.Write($"TransformItems: source.Count={source.Count}");
        
        if (source.Count == 0)
            return Array.Empty<TGridItem>().AsQueryable();

        // Runtime enforcement: grouping requires row identity.
        if (source[0] is not IRowIdentifiable)
            throw new InvalidOperationException("Active grouping requires TGridItem to implement IRowIdentifiable.");

        // Partition by key (object typed) with null handling.
        var nullKeyItems = new List<TGridItem>();
        var groups = new Dictionary<object, List<TGridItem>>();

        foreach (var item in source)
        {
            var key = ActiveGrouping.GroupByUntyped(item);
            if (key is null)
            {
                nullKeyItems.Add(item);
                continue;
            }

            if (!groups.TryGetValue(key, out var list))
            {
                list = new List<TGridItem>();
                groups.Add(key, list);
            }
            list.Add(item);
        }

        // Build output
        var output = new List<TGridItem>(source.Count + groups.Count * ActiveGrouping.GroupHeaderSlotSpan);

        // NullKeyBehavior
        if (ActiveGrouping.NullKeyBehavior == NullKeyBehavior.ShowAtTop)
            output.AddRange(nullKeyItems);

        // Determine group ordering
        IEnumerable<KeyValuePair<object, List<TGridItem>>> orderedGroups = ActiveGrouping.GroupOrder switch
        {
            GroupSortDirection.FirstOccurrence => groups, // dictionary preserves insertion order in modern .NET
            GroupSortDirection.Descending => groups.OrderByDescending(g => g.Key?.ToString(), StringComparer.Ordinal),
            _ => groups.OrderBy(g => g.Key?.ToString(), StringComparer.Ordinal)
        };

        foreach (var (key, groupItems) in orderedGroups)
        {
            var expanded = ActiveGrouping.IsGroupExpanded(key);

            var groupId = GetOrCreateGroupId(key);
            _groupIdToItemCount[groupId] = groupItems.Count;

            // marker/spacers: reuse a representative item instance to manufacture synthetic rows.
            // This avoids requiring a parameterless constructor while keeping the output as TGridItem.
            var representative = groupItems.Count > 0 ? groupItems[0] : source.First();

            output.Add(CreateSyntheticRow(representative, GroupHeaderRowId.EncodeGroupHeaderId(groupId)));

            for (int offset = 1; offset <= ActiveGrouping.GroupHeaderSlotSpan - 1; offset++)
            {
                output.Add(CreateSyntheticRow(representative, GroupHeaderRowId.EncodeGroupHeaderSpacerId(groupId, offset)));
            }

            if (expanded)
            {
                output.AddRange(groupItems);
            }
        }

        if (ActiveGrouping.NullKeyBehavior == NullKeyBehavior.ShowAtBottom)
        {
            output.AddRange(nullKeyItems);
        }
        else if (ActiveGrouping.NullKeyBehavior == NullKeyBehavior.SeparateGroup)
        {
            if (nullKeyItems.Count > 0)
            {
                var groupId = GetOrCreateGroupId(null);
                _groupIdToItemCount[groupId] = nullKeyItems.Count;

                var representative = nullKeyItems[0];

                output.Add(CreateSyntheticRow(representative, GroupHeaderRowId.EncodeGroupHeaderId(groupId)));
                for (int offset = 1; offset <= ActiveGrouping.GroupHeaderSlotSpan - 1; offset++)
                {
                    output.Add(CreateSyntheticRow(representative, GroupHeaderRowId.EncodeGroupHeaderSpacerId(groupId, offset)));
                }

                if (ActiveGrouping.IsGroupExpanded(null!))
                    output.AddRange(nullKeyItems);
            }
        }
        // Exclude => do nothing.

        QgDebugLog.Write($"TransformItems: output.Count={output.Count}, groups={groups.Count}, expanded groups with items");
        return output.AsQueryable();
    }

    private static TGridItem CreateSyntheticRow(TGridItem source, int syntheticId)
    {
        // Shallow clone to keep TGridItem type.
        // This is sufficient for virtualization placeholders where only Id is consulted.
        var clone = Clone(source);

        if (clone is not IRowIdentifiable identifiable)
            throw new InvalidOperationException("Active grouping requires TGridItem to implement IRowIdentifiable.");

        identifiable.Id = syntheticId;
        return clone;
    }

    private static Func<TGridItem, TGridItem> CreateCloner()
    {
        var instance = Expression.Parameter(typeof(TGridItem), "instance");
        var memberwiseClone = typeof(object).GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Unable to locate object.MemberwiseClone().");

        var call = Expression.Call(Expression.Convert(instance, typeof(object)), memberwiseClone);
        return Expression.Lambda<Func<TGridItem, TGridItem>>(Expression.Convert(call, typeof(TGridItem)), instance).Compile();
    }

    private int GetOrCreateGroupId(object? key)
    {
        if (key is null)
        {
            if (_nullKeyGroupId.HasValue)
                return _nullKeyGroupId.Value;

            if (_nextGroupId > 0xFFFF)
                throw new InvalidOperationException("Maximum supported group count exceeded (65535).");

            var id = _nextGroupId++;
            _nullKeyGroupId = id;
            _groupIdToKey[id] = null;
            return id;
        }

        if (_keyToGroupId.TryGetValue(key, out var existing))
            return existing;

        if (_nextGroupId > 0xFFFF)
            throw new InvalidOperationException("Maximum supported group count exceeded (65535).");

        var newId = _nextGroupId++;
        _keyToGroupId[key] = newId;
        _groupIdToKey[newId] = key;
        return newId;
    }

    public void Dispose()
    {
        _registered.Clear();
        ActiveGrouping = null;
        HeaderHostColumnId = null;
        _keyToGroupId.Clear();
        _groupIdToKey.Clear();
        _groupIdToItemCount.Clear();
        _nullKeyGroupId = null;
        _nextGroupId = 1;
    }
}
