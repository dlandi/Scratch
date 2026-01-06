using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using QuickGridTest01.ComposableColumns.Features.Editing;

namespace QuickGridTest01.ComposableColumns.Core;

/// <summary>
/// A column that composes multiple features together.
/// This is the main entry point for the composable column system.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the property value for this column.</typeparam>
public class ComposableColumn<TGridItem, TValue> : ColumnBase<TGridItem>, IDisposable, IColumnFeatureProvider<TGridItem>
    where TGridItem : class
{
    [CascadingParameter]
    public Func<TGridItem, object>? RowKey { get; set; }

    [CascadingParameter]
    public ComposableGrid<TGridItem>? Grid { get; set; }

    [CascadingParameter]
    public IEditEventStream? EditEventStream { get; set; }

    private readonly List<IColumnFeature<TGridItem>> _features = [];
    private FeatureContext<TGridItem, TValue>? _context;
    private bool _initialized;
    private bool _disposed;
    private GridSort<TGridItem>? _sortBy;
    private Expression<Func<TGridItem, TValue>>? _lastProperty;

    // Cached sorted lists for rendering
    private List<ICellRenderFeature<TGridItem>>? _cellRenderFeatures;

    /// <summary>
    /// The property expression that identifies which property of the model to display.
    /// </summary>
    [Parameter]
    public Expression<Func<TGridItem, TValue>>? Property { get; set; }

    /// <summary>
    /// Format string for displaying the value.
    /// </summary>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>
    /// Custom formatter function for displaying the value.
    /// </summary>
    [Parameter]
    public Func<TValue, string>? Formatter { get; set; }

    /// <summary>
    /// Child content containing feature components.
    /// </summary>
    [Parameter]
    public RenderFragment? Features { get; set; }

    /// <summary>
    /// Child content containing feature components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent
    {
        get => Features;
        set => Features = value;
    }

    /// <summary>
    /// Collection of features to add programmatically.
    /// </summary>
    [Parameter]
    public IEnumerable<IColumnFeature<TGridItem>>? FeatureCollection { get; set; }

    /// <summary>
    /// Gets or sets the sort expression for this column.
    /// </summary>
    public override GridSort<TGridItem>? SortBy
    {
        get => _sortBy;
        set => _sortBy = value;
    }

    /// <summary>
    /// Gets the feature context for this column.
    /// </summary>
    public FeatureContext<TGridItem, TValue> Context => _context ??= CreateContext();

    /// <summary>
    /// Adds a feature to this column.
    /// </summary>
    public ComposableColumn<TGridItem, TValue> AddFeature(IColumnFeature<TGridItem> feature)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ComposableColumn<TGridItem, TValue>));

        _features.Add(feature);
        InvalidateFeatureCache();
        return this;
    }

    /// <summary>
    /// Removes a feature from this column.
    /// </summary>
    public bool RemoveFeature(IColumnFeature<TGridItem> feature)
    {
        var removed = _features.Remove(feature);
        if (removed)
        {
            feature.OnDetach(Context);
            InvalidateFeatureCache();
        }
        return removed;
    }

    /// <summary>
    /// Gets all features of a specific type.
    /// </summary>
    public IEnumerable<TFeature> GetFeatures<TFeature>() where TFeature : IColumnFeature<TGridItem>
    {
        return _features.OfType<TFeature>();
    }

    public IReadOnlyList<IColumnFeature<TGridItem>> GetAllFeatures() => _features;

    /// <summary>
    /// Gets the first feature of a specific type, or null if not found.
    /// </summary>
    public TFeature? GetFeature<TFeature>() where TFeature : class, IColumnFeature<TGridItem>
    {
        return _features.OfType<TFeature>().FirstOrDefault();
    }

    protected override void OnParametersSet()
    {
        // Update context with current parameters
        Context.PropertyExpression = Property;
        Context.Format = Format;
        Context.Formatter = Formatter;
        Context.Title = Title;

        // Ensure features can access the owning ComposableGrid without reflection.
        Context.Grid = Grid;

        // Compile property accessor if property changed
        if (Property is not null && Property != _lastProperty)
        {
            _lastProperty = Property;
            Context.GetValue = Property.Compile();

            // Create setter if possible
            if (Property.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo propInfo)
            {
                Context.SetState(FeatureStateKeys.PropertyName, propInfo.Name);
                Context.SetState(FeatureStateKeys.PropertyType, typeof(TValue));

                // Bill setter expression
                if (propInfo.CanWrite)
                {
                    var param = Property.Parameters[0];
                    var valueParam = Expression.Parameter(typeof(TValue), "value");
                    var assign = Expression.Assign(memberExpr, valueParam);
                    var setter = Expression.Lambda<Action<TGridItem, TValue>>(assign, param, valueParam);
                    Context.SetValue = setter.Compile();
                }
            }

            // Set up sorting if enabled
            if (Sortable == true)
            {
                _sortBy = GridSort<TGridItem>.ByAscending(Property);
                Context.IsSortable = true;
            }
        }

        // Add features from FeatureCollection parameter (only once)
        if (FeatureCollection is not null && !_initialized)
        {
            foreach (var feature in FeatureCollection)
            {
                if (!_features.Contains(feature))
                {
                    _features.Add(feature);
                }
            }
            InvalidateFeatureCache();
        }

        // Initialize features if not done yet
        if (!_initialized)
        {
            Grid?.RegisterColumn(this);
            Initialize();
            _initialized = true;
        }
        else
        {
            // Notify features that parameters have changed so they can update their state
            foreach (var feature in _features)
            {
                feature.OnParametersChanged(Context);
            }
        }

        // Register filter features with grid (may not be available on first pass)
        RegisterFilterFeaturesWithGrid();
    }

    private void Initialize()
    {
        // Add core-owned features before any user features attach. These participate in the render pipeline
        // for all columns and enable cross-column behaviors without runtime feature injection.
        _features.Insert(0, new GroupingSyntheticBlankingFeature<TGridItem, TValue>());
        _features.Insert(1, new ComposableColumns.Features.Grouping.Components.GroupHeaderHostFeature<TGridItem>());

        // Attach non-core features first so they can populate FeatureContext state
        // (e.g., grouping columns set Grouping.ColumnId and register with the coordinator).
        foreach (var feature in _features.Where(f => f.Priority != FeaturePriority.Grouping).ToList())
        {
            feature.OnAttach(Context);
        }

        // Then attach grouping features (core-owned + grouping features) so they can read coordinator/state.
        foreach (var feature in _features.Where(f => f.Priority == FeaturePriority.Grouping).ToList())
        {
            feature.OnAttach(Context);
        }
    }

    private bool _filtersRegistered;

    private void RegisterFilterFeaturesWithGrid()
    {
        // Only register once, and only if Grid is available
        if (_filtersRegistered || Grid is null)
            return;

        foreach (var feature in _features)
        {
            if (feature is IGridFilterFeature<TGridItem> filterFeature)
            {
                Grid.RegisterFilter(filterFeature);
            }
        }

        _filtersRegistered = true;
    }

    private FeatureContext<TGridItem, TValue> CreateContext()
    {
        var context = new FeatureContext<TGridItem, TValue>
        {
            Column = this,
            EventReceiver = this,
            Title = Title,
            IsSortable = Sortable == true,
            RequestRefresh = StateHasChanged,
            RequestRefreshAsync = () => InvokeAsync(StateHasChanged),
            InvokeAsync = async action => await InvokeAsync(action)
        };

        // Register the EditEventStream service if available
        if (EditEventStream is not null)
        {
            context.RegisterService(EditEventStream);
        }

        return context;
    }

    private void InvalidateFeatureCache()
    {
        _cellRenderFeatures = null;
    }

    private List<ICellRenderFeature<TGridItem>> GetCellRenderFeatures()
    {
        return _cellRenderFeatures ??= _features
            .OfType<ICellRenderFeature<TGridItem>>()
            .OrderBy(f => f.Priority)
            .ToList();
    }

    /// <inheritdoc />
    protected override void CellContent(RenderTreeBuilder builder, TGridItem item)
    {
        var cellFeatures = GetCellRenderFeatures();

        if (cellFeatures.Count == 0)
        {
            // No cell render features - just render the formatted value
            RenderDefaultCell(builder, item);
            return;
        }

        // Build the render pipeline (features wrap each other)
        var sequence = 0;
        RenderCellPipeline(builder, ref sequence, item, cellFeatures, 0);
    }

    private void RenderCellPipeline(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        List<ICellRenderFeature<TGridItem>> features,
        int index)
    {
        if (index >= features.Count)
        {
            // End of pipeline - render default content
            RenderDefaultCell(builder, item);
            return;
        }

        var feature = features[index];
        var seq = sequence; // Capture for closure
        var nextIndex = index + 1;

        feature.RenderCell(builder, ref sequence, item, Context, () =>
        {
            var innerSeq = seq + 100; // Leave room for feature content
            RenderCellPipeline(builder, ref innerSeq, item, features, nextIndex);
        });
    }

    private void RenderDefaultCell(RenderTreeBuilder builder, TGridItem item)
    {
        var formattedValue = Context.GetFormattedValue(item);
        builder.AddContent(0, formattedValue);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        Grid?.UnregisterColumn(this);

        // Unregister filter features and detach all features
        foreach (var feature in _features)
        {
            // Unregister filter features from the grid
            if (feature is IGridFilterFeature<TGridItem> filterFeature && Grid is not null)
            {
                Grid.UnregisterFilter(filterFeature);
            }

            feature.OnDetach(Context);
        }

        _features.Clear();
        _context?.Clear();

        GC.SuppressFinalize(this);
    }
}
