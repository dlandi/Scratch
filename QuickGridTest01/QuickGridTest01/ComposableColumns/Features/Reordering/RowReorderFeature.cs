using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Reordering;

/// <summary>
/// A column feature that enables row drag-and-drop reordering via HTML5 Drag & Drop API.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public sealed class RowReorderFeature<TGridItem> : ICellRenderFeature<TGridItem>, IDisposable
    where TGridItem : class, IRowIdentifiable
{
    private FeatureContext<TGridItem>? _context;
    private ReorderCoordinator<TGridItem>? _coordinator;
    private ReorderableDataSource<TGridItem>? _dataSource;
    private bool _disposed;
    private bool _dataSourceValidated;
    private TGridItem? _pendingDropDraggedItem; // Track dragged item for race condition handling
    private int _dragOperationId; // Unique ID for each drag operation for logging
    private readonly int _instanceId = Environment.TickCount + Random.Shared.Next(); // Unique ID for this feature instance

    /// <inheritdoc />
    public int Priority => FeaturePriority.Reordering;

    /// <summary>
    /// Gets or sets whether reordering is enabled. Default is true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets how drag reordering is triggered. Default is HandleOnly.
    /// </summary>
    public ReorderTriggerMode TriggerMode { get; set; } = ReorderTriggerMode.HandleOnly;

    /// <summary>
    /// Gets or sets whether items can be reordered across groups when grouping is active.
    /// Default is false (items can only be reordered within their group).
    /// </summary>
    public bool AllowCrossGroupReorder { get; set; } = false;

    /// <summary>
    /// Gets or sets the row height in pixels for drop position calculation. Default is 48.
    /// </summary>
    public int RowHeight { get; set; } = 48;

    /// <summary>
    /// Gets or sets the default drag handle content when no icon or template is provided. Default is "⋮⋮".
    /// </summary>
    public string DragHandleContent { get; set; } = "⋮⋮";

    /// <summary>
    /// Gets or sets the CSS class for a drag handle icon. Takes precedence over DragHandleContent.
    /// </summary>
    public string? DragHandleIcon { get; set; }

    /// <summary>
    /// Gets or sets a custom template for the drag handle. Takes precedence over icon and content.
    /// </summary>
    public RenderFragment<TGridItem>? DragHandleTemplate { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for the handle cell. Default is "reorder-handle-cell".
    /// </summary>
    public string HandleCellClass { get; set; } = "reorder-handle-cell";

    /// <summary>
    /// Gets or sets the CSS class for the drag handle. Default is "reorder-handle".
    /// </summary>
    public string HandleClass { get; set; } = "reorder-handle";

    /// <summary>
    /// Gets or sets the CSS class applied to a row while it is being dragged. Default is "reorder-dragging".
    /// </summary>
    public string DraggingRowClass { get; set; } = "reorder-dragging";

    /// <summary>
    /// Gets or sets the CSS class applied when hovering before a target row. Default is "reorder-drop-before".
    /// </summary>
    public string DragOverBeforeClass { get; set; } = "reorder-drop-before";

    /// <summary>
    /// Gets or sets the CSS class applied when hovering after a target row. Default is "reorder-drop-after".
    /// </summary>
    public string DragOverAfterClass { get; set; } = "reorder-drop-after";

    /// <summary>
    /// Gets or sets the CSS class for disabled drag handles. Default is "reorder-disabled".
    /// </summary>
    public string DisabledHandleClass { get; set; } = "reorder-disabled";

    /// <summary>
    /// Gets or sets a predicate to determine if an item can be dragged. Default allows all non-synthetic rows.
    /// </summary>
    public Func<TGridItem, bool>? CanDrag { get; set; }

    /// <summary>
    /// Gets or sets a predicate to determine if an item can be dropped on another. Default allows all valid drops.
    /// </summary>
    public Func<TGridItem, TGridItem, bool>? CanDropOn { get; set; }

    /// <summary>
    /// Event fired before a reorder operation. Can be cancelled by setting Cancel = true.
    /// </summary>
    public EventCallback<RowBeforeReorderEventArgs<TGridItem>> OnBeforeReorder { get; set; }

    /// <summary>
    /// Event fired after a successful reorder operation.
    /// </summary>
    public EventCallback<RowReorderedEventArgs<TGridItem>> OnRowReordered { get; set; }

    /// <summary>
    /// Event fired when a reorder operation is cancelled.
    /// </summary>
    public EventCallback<RowReorderCancelledEventArgs<TGridItem>> OnReorderCancelled { get; set; }

    /// <summary>
    /// Optional JS runtime for debug logging to browser console. Set this to enable detailed drag/drop logging.
    /// </summary>
    public IJSRuntime? JSRuntime { get; set; }

    /// <summary>
    /// Enables or disables debug logging to browser console. Default is false.
    /// </summary>
    public bool EnableDebugLogging { get; set; } = false;

    // Logging helper methods
    private void LogDebug(string message)
    {
        if (!EnableDebugLogging || JSRuntime is null) return;
        _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}#{_dragOperationId}] {message}");
    }

    private void LogDebugState(string eventName, TGridItem? item = null)
    {
        if (!EnableDebugLogging || JSRuntime is null) return;
        var itemInfo = item is not null ? $"Item[Id={item.Id}]" : "null";
        var draggedInfo = _coordinator?.DraggedItem is not null ? $"DraggedItem[Id={_coordinator.DraggedItem.Id}]" : "null";
        var pendingInfo = _pendingDropDraggedItem is not null ? $"PendingDrop[Id={_pendingDropDraggedItem.Id}]" : "null";
        var hoverInfo = _coordinator?.HoveredTarget is not null ? $"Hover[Id={_coordinator.HoveredTarget.Id}, Pos={_coordinator.CurrentDropPosition}]" : "null";
        var isDragging = _coordinator?.IsDragging ?? false;

            _ = JSRuntime.InvokeVoidAsync("console.log", 
                $"[Reorder@{_instanceId}#{_dragOperationId}] {eventName}: {itemInfo} | IsDragging={isDragging} | {draggedInfo} | {pendingInfo} | {hoverInfo}");
        }

        private void LogDebugError(string message)
        {
            if (!EnableDebugLogging || JSRuntime is null) return;
            _ = JSRuntime.InvokeVoidAsync("console.error", $"[Reorder@{_instanceId}#{_dragOperationId}] ERROR: {message}");
        }

        private void LogDebugWarn(string message)
        {
            if (!EnableDebugLogging || JSRuntime is null) return;
            _ = JSRuntime.InvokeVoidAsync("console.warn", $"[Reorder@{_instanceId}#{_dragOperationId}] WARN: {message}");
        }

        /// <inheritdoc />
        public void OnAttach(FeatureContext<TGridItem> context)
        {
            ArgumentNullException.ThrowIfNull(context);

        _context = context;

        // Log attachment attempt with instance ID for debugging
        if (EnableDebugLogging && JSRuntime is not null)
        {
            _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}] OnAttach called - context.Grid is {(context.Grid is null ? "NULL" : context.Grid.GetType().Name)}");
        }

        // Validate RowHeight
        if (RowHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(RowHeight), "RowHeight must be greater than zero.");

        // Get the grid and create/get coordinator
        ComposableGrid<TGridItem> grid;
        try
        {
            grid = GetGridOrThrow(context);
        }
        catch (InvalidOperationException ex)
        {
            if (EnableDebugLogging && JSRuntime is not null)
            {
                _ = JSRuntime.InvokeVoidAsync("console.error", $"[Reorder@{_instanceId}] OnAttach FAILED - GetGridOrThrow threw: {ex.Message}");
            }
            throw;
        }

        _coordinator = grid.GetOrCreateReorderCoordinator();

        if (EnableDebugLogging && JSRuntime is not null)
        {
            _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}] OnAttach - Got coordinator: {_coordinator is not null}");
        }

        // Enforce single-feature constraint (allow re-attachment of the same instance)
        if (_coordinator.Feature is not null && !ReferenceEquals(_coordinator.Feature, this))
            throw new InvalidOperationException("Only one RowReorderFeature is permitted per grid.");

        // Register this feature with the coordinator
        _coordinator.Feature = this;
        _coordinator.IsReorderingEnabled = Enabled;

        // If data source was set before attachment, register it with the coordinator now
        if (_dataSource is not null)
        {
            _coordinator.DataSource = _dataSource;
            if (EnableDebugLogging && JSRuntime is not null)
            {
                _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}] OnAttach - DataSource registered with coordinator");
            }
        }
        else
        {
            if (EnableDebugLogging && JSRuntime is not null)
            {
                _ = JSRuntime.InvokeVoidAsync("console.warn", $"[Reorder@{_instanceId}] OnAttach - DataSource is NULL (will be set via SetDataSource later)");
            }
        }

        // Subscribe to state changes for UI refresh
        _coordinator.OnStateChanged += OnCoordinatorStateChanged;

        if (EnableDebugLogging && JSRuntime is not null)
        {
            _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}] OnAttach COMPLETE - coordinator={_coordinator is not null}, dataSource={_dataSource is not null}");
        }
    }

    /// <inheritdoc />
    public void OnDetach(FeatureContext<TGridItem> context)
    {
        if (EnableDebugLogging && JSRuntime is not null)
        {
            _ = JSRuntime.InvokeVoidAsync("console.log", $"[Reorder@{_instanceId}] OnDetach called - coordinator was {(_coordinator is not null ? "set" : "null")}");
        }

        if (_coordinator is not null)
        {
            _coordinator.OnStateChanged -= OnCoordinatorStateChanged;

            if (ReferenceEquals(_coordinator.Feature, this))
            {
                _coordinator.Feature = null;
                _coordinator.IsReorderingEnabled = false;
            }
        }

        // Note: Do NOT call Dispose() here. The feature instance may be re-attached
        // to a new column (e.g., when Blazor recreates components during re-renders).
        // Keep _coordinator and _dataSource intact so they can be restored in OnAttach.
        // Dispose() should only be called for permanent cleanup when the feature is
        // no longer needed.
        _context = null;
    }

    /// <summary>
    /// Sets the data source for this feature. Called by user code.
    /// </summary>
    public void SetDataSource(ReorderableDataSource<TGridItem> dataSource)
    {
        _dataSource = dataSource;
        if (_coordinator is not null)
        {
            _coordinator.DataSource = dataSource;
        }
    }

    /// <inheritdoc />
    public void RenderCell(
        RenderTreeBuilder builder,
        ref int sequence,
        TGridItem item,
        FeatureContext<TGridItem> context,
        Action renderNext)
    {
        // Warn if coordinator is null - this indicates a render before OnAttach completed
        if (_coordinator is null && EnableDebugLogging && JSRuntime is not null)
        {
            _ = JSRuntime.InvokeVoidAsync("console.warn", $"[Reorder@{_instanceId}] RenderCell called but _coordinator is null! OnAttach may not have been called.");
        }

        // Check if feature is disabled - render empty cell
        if (!Enabled)
        {
            RenderEmptyCell(builder, ref sequence);
            return;
        }

        // Check for synthetic row - render empty cell
        if (ReorderingHelpers.IsSyntheticRow(item))
        {
            RenderEmptyCell(builder, ref sequence, "reorder-cell-empty");
            return;
        }

        // Check if dragging is allowed for this item
        var canDrag = CanDrag?.Invoke(item) ?? true;

        // Build CSS classes for cell
        var cellClass = BuildCellClass(item);

        // Open cell container
        builder.OpenElement(sequence++, "td");
        builder.AddAttribute(sequence++, "class", cellClass);

        // Drop target events are ALWAYS on the cell to accept drops when hovering anywhere over the row's handle cell
        AddDropTargetAttributes(builder, ref sequence, item);

        // Drag source events on the cell if TriggerMode is EntireRow
        if (TriggerMode == ReorderTriggerMode.EntireRow && canDrag)
        {
            AddDragSourceAttributes(builder, ref sequence, item);
        }

        // Render the drag handle
        RenderDragHandle(builder, ref sequence, item, canDrag);

        builder.CloseElement();

        // NOTE: Do NOT call renderNext() - this feature owns the entire cell content
    }

    private void RenderEmptyCell(RenderTreeBuilder builder, ref int sequence, string? additionalClass = null)
    {
        builder.OpenElement(sequence++, "td");
        var cssClass = string.IsNullOrEmpty(additionalClass) 
            ? HandleCellClass 
            : $"{HandleCellClass} {additionalClass}";
        builder.AddAttribute(sequence++, "class", cssClass);
        builder.CloseElement();
    }

    private void RenderDragHandle(RenderTreeBuilder builder, ref int sequence, TGridItem item, bool canDrag)
    {
        builder.OpenElement(sequence++, "div");

        // Build handle class
        var handleClass = canDrag ? HandleClass : $"{HandleClass} {DisabledHandleClass}";
        builder.AddAttribute(sequence++, "class", handleClass);

        // Drag source events on the handle if TriggerMode is HandleOnly
        if (canDrag && TriggerMode == ReorderTriggerMode.HandleOnly)
        {
            AddDragSourceAttributes(builder, ref sequence, item);
        }

        // ARIA attributes
        builder.AddAttribute(sequence++, "role", "button");
        builder.AddAttribute(sequence++, "aria-label", canDrag ? "Drag to reorder" : "Reordering disabled");
        builder.AddAttribute(sequence++, "tabindex", canDrag ? "0" : "-1");

        // Render handle content
        if (DragHandleTemplate is not null)
        {
            builder.AddContent(sequence++, DragHandleTemplate(item));
        }
        else if (!string.IsNullOrEmpty(DragHandleIcon))
        {
            builder.OpenElement(sequence++, "i");
            builder.AddAttribute(sequence++, "class", DragHandleIcon);
            builder.CloseElement();
        }
        else
        {
            builder.AddContent(sequence++, DragHandleContent);
        }

        builder.CloseElement();
    }

    /// <summary>
    /// Adds drag source attributes (draggable, ondragstart, ondragend) to make an element draggable.
    /// </summary>
    private void AddDragSourceAttributes(RenderTreeBuilder builder, ref int sequence, TGridItem item)
    {
        builder.AddAttribute(sequence++, "draggable", "true");
        builder.AddAttribute(sequence++, "ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragStartAsync(item, e)));
        builder.AddAttribute(sequence++, "ondragend", EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragEndAsync(item, e)));
    }

    /// <summary>
    /// Adds drop target attributes (ondragover, ondrop, ondragleave) to make an element a drop target.
    /// These are always added to the cell to allow drops when hovering anywhere over the cell.
    /// </summary>
    private void AddDropTargetAttributes(RenderTreeBuilder builder, ref int sequence, TGridItem item)
    {
        builder.AddAttribute(sequence++, "ondragover", EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragOverAsync(item, e)));
        builder.AddEventPreventDefaultAttribute(sequence++, "ondragover", true);
        builder.AddAttribute(sequence++, "ondrop", EventCallback.Factory.Create<DragEventArgs>(this, e => OnDropAsync(item, e)));
        builder.AddEventPreventDefaultAttribute(sequence++, "ondrop", true);
        builder.AddAttribute(sequence++, "ondragleave", EventCallback.Factory.Create<DragEventArgs>(this, e => OnDragLeaveAsync(item, e)));
    }

    private string BuildCellClass(TGridItem item)
    {
        var classes = new List<string> { HandleCellClass };

        if (_coordinator is null)
            return string.Join(" ", classes);

        // Is this the dragged item?
        if (_coordinator.DraggedItem is not null && _coordinator.DraggedItem.Id == item.Id)
        {
            classes.Add(DraggingRowClass);
        }

        // Is this the hover target?
        if (_coordinator.HoveredTarget is not null && _coordinator.HoveredTarget.Id == item.Id)
        {
            if (_coordinator.CurrentDropPosition == DropPosition.Before)
            {
                classes.Add(DragOverBeforeClass);
            }
            else if (_coordinator.CurrentDropPosition == DropPosition.After)
            {
                classes.Add(DragOverAfterClass);
            }
        }

        return string.Join(" ", classes);
    }

    private async Task OnDragStartAsync(TGridItem item, DragEventArgs e)
    {
        // Generate new operation ID for this drag sequence
        _dragOperationId = Environment.TickCount;

        LogDebug($"=== DRAG START === ClientX={e.ClientX}, ClientY={e.ClientY}");
        LogDebugState("OnDragStart:ENTRY", item);

        if (_coordinator is null || !Enabled)
        {
            LogDebugWarn("OnDragStart:EXIT - Coordinator null or not enabled");
            return;
        }

        // Validate DataSource on first drag attempt
        try
        {
            ValidateDataSource();
        }
        catch (Exception ex)
        {
            LogDebugError($"OnDragStart:EXIT - DataSource validation failed: {ex.Message}");
            throw;
        }

        // Synthetic rows cannot be dragged
        if (ReorderingHelpers.IsSyntheticRow(item))
        {
            LogDebugWarn("OnDragStart:EXIT - Synthetic row cannot be dragged");
            return;
        }

        // Check CanDrag predicate
        if (CanDrag?.Invoke(item) == false)
        {
            LogDebugWarn($"OnDragStart:EXIT - CanDrag predicate returned false for Item[Id={item.Id}]");
            return;
        }

        // TODO: If expansion feature is active and item is expanded, collapse first
        // (Deferred to integration phase)

        // Store dragged item for race condition handling
        _pendingDropDraggedItem = item;
        _coordinator.StartDrag(item);

        LogDebugState("OnDragStart:EXIT - Drag started successfully", item);
    }

    private Task OnDragEndAsync(TGridItem item, DragEventArgs e)
    {
        LogDebug($"=== DRAG END === ClientX={e.ClientX}, ClientY={e.ClientY}");
        LogDebugState("OnDragEnd:ENTRY", item);

        // The ondragend event fires when a drag operation ends (whether successful or not).
        // On a successful drop, ondrop fires FIRST, which clears the drag state via CancelDrag().
        // On an unsuccessful drop (cancelled or dropped outside valid target), ondrop doesn't fire.
        // 
            // Due to Blazor Server's SignalR message ordering, ondragend may be processed BEFORE ondrop.
            // To handle this, we delay clearing the state to give ondrop a chance to process first.
            // The _pendingDropDraggedItem field preserves the dragged item for ondrop to use.

            var opId = _dragOperationId; // Capture for closure
            LogDebug($"OnDragEnd: Scheduling delayed cleanup (100ms) - OpId={opId}");

            _ = Task.Delay(100).ContinueWith(async _ =>
            {
                if (JSRuntime is not null && EnableDebugLogging)
                {
                    await JSRuntime.InvokeVoidAsync("console.log", 
                        $"[Reorder#{opId}] OnDragEnd:DELAYED_CLEANUP - Clearing state. PendingDrop was {(_pendingDropDraggedItem is not null ? $"Id={_pendingDropDraggedItem.Id}" : "null")}");
                }
                _pendingDropDraggedItem = null;
                _coordinator?.CancelDrag();
            }, TaskScheduler.Default);

            LogDebugState("OnDragEnd:EXIT - Cleanup scheduled", item);
            return Task.CompletedTask;
        }

    private Task OnDragOverAsync(TGridItem item, DragEventArgs e)
    {
        // Note: Don't log every dragover as it fires very frequently
        // Only log significant state changes

        if (_coordinator is null || !_coordinator.IsDragging)
        {
            // Only log once per non-dragging hover
            if (_coordinator is null)
            {
                LogDebugWarn("OnDragOver:EXIT - Coordinator is null");
            }
            else
            {
                LogDebugWarn($"OnDragOver:EXIT - Not dragging. IsDragging={_coordinator.IsDragging}, DraggedItem={(_coordinator.DraggedItem is not null ? $"Id={_coordinator.DraggedItem.Id}" : "null")}, PendingDrop={(_pendingDropDraggedItem is not null ? $"Id={_pendingDropDraggedItem.Id}" : "null")}");
            }
            return Task.CompletedTask;
        }

        // Synthetic rows cannot be drop targets
        if (ReorderingHelpers.IsSyntheticRow(item))
        {
            LogDebug($"OnDragOver: Synthetic row - clearing hover");
            _coordinator.ClearHover();
            return Task.CompletedTask;
        }

        // Check if dragged item was filtered out during drag
        if (_dataSource is not null)
        {
            var draggedIndex = _dataSource.IndexOf(_coordinator.DraggedItem!);
            if (draggedIndex < 0)
            {
                LogDebugError("OnDragOver: Dragged item no longer in data source - cancelling drag");
                _coordinator.CancelDrag();
                return Task.CompletedTask;
            }
        }

        // Check CanDropOn predicate
        if (CanDropOn?.Invoke(_coordinator.DraggedItem!, item) == false)
        {
            LogDebug($"OnDragOver: CanDropOn predicate returned false for target Item[Id={item.Id}]");
            _coordinator.ClearHover();
            return Task.CompletedTask;
        }

        // Check grouping constraints
        if (!CanDropOnTarget(_coordinator.DraggedItem!, item))
        {
            // Log only if this is a different target than before to reduce noise
            if (_coordinator.HoveredTarget?.Id != item.Id)
            {
                LogDebug($"OnDragOver: CanDropOnTarget returned false for target Item[Id={item.Id}] (may be self-drop or group constraint)");
            }
            _coordinator.ClearHover();
            return Task.CompletedTask;
        }

        // Calculate drop position
        var position = GetDropPosition(e);

        // Log only when hover target changes
        var previousTarget = _coordinator.HoveredTarget;
        var previousPosition = _coordinator.CurrentDropPosition;

        _coordinator.UpdateHover(item, position);

        if (previousTarget?.Id != item.Id || previousPosition != position)
        {
            LogDebug($"OnDragOver: Updated hover - Target[Id={item.Id}], Position={position}, ClientY={e.ClientY}");
        }

        return Task.CompletedTask;
    }

    private async Task OnDropAsync(TGridItem item, DragEventArgs e)
    {
        LogDebug($"=== DROP === Target Item[Id={item.Id}], ClientX={e.ClientX}, ClientY={e.ClientY}");
        LogDebugState("OnDrop:ENTRY", item);

        if (_coordinator is null || _dataSource is null)
        {
            LogDebugError($"OnDrop:EXIT - Coordinator null={_coordinator is null}, DataSource null={_dataSource is null}");
            return;
        }

        // Get dragged item - prefer coordinator state, fall back to saved state for race condition handling
        var draggedItem = _coordinator.DraggedItem ?? _pendingDropDraggedItem;
        LogDebug($"OnDrop: DraggedItem resolution - Coordinator.DraggedItem={(_coordinator.DraggedItem is not null ? $"Id={_coordinator.DraggedItem.Id}" : "null")}, PendingDrop={(_pendingDropDraggedItem is not null ? $"Id={_pendingDropDraggedItem.Id}" : "null")}, Using={draggedItem?.Id.ToString() ?? "null"}");

        if (draggedItem is null)
        {
            LogDebugError("OnDrop:EXIT - No dragged item available (both coordinator and pending are null)");
            return;
        }

        // Validate drop target (same checks as OnDragOver)
        if (ReorderingHelpers.IsSyntheticRow(item))
        {
            LogDebugWarn("OnDrop:EXIT - Target is synthetic row");
            return;
        }

        if (CanDropOn?.Invoke(draggedItem, item) == false)
        {
            LogDebugWarn($"OnDrop:EXIT - CanDropOn predicate returned false");
            return;
        }

        if (!CanDropOnTarget(draggedItem, item))
        {
            LogDebugWarn($"OnDrop:EXIT - CanDropOnTarget returned false (self-drop or group constraint)");
            return;
        }

        var oldIndex = _dataSource.IndexOf(draggedItem);
        var targetIndex = _dataSource.IndexOf(item);
        var position = GetDropPosition(e);

        // Calculate new index based on drop position
        var newIndex = position == DropPosition.Before ? targetIndex : targetIndex + 1;
        if (oldIndex < newIndex)
            newIndex--; // Adjust for removal

        LogDebug($"OnDrop: Calculating move - OldIndex={oldIndex}, TargetIndex={targetIndex}, Position={position}, NewIndex={newIndex}");

        // Build event args
        var args = new RowBeforeReorderEventArgs<TGridItem>
        {
            DraggedItem = draggedItem,
            TargetItem = item,
            Position = position,
            OldIndex = oldIndex,
            NewIndex = newIndex
        };

        // Execute reorder
        LogDebug("OnDrop: Executing reorder...");
        await ExecuteReorderAsync(args);

        // Clear drag state - both coordinator and local field
        LogDebug("OnDrop: Clearing drag state");
        _pendingDropDraggedItem = null;
        _coordinator.CancelDrag();

        LogDebugState("OnDrop:EXIT - Reorder complete", item);
    }

    private Task OnDragLeaveAsync(TGridItem item, DragEventArgs e)
    {
        // Only clear hover if leaving the current target
        if (_coordinator?.HoveredTarget is not null && _coordinator.HoveredTarget.Id == item.Id)
        {
            LogDebug($"OnDragLeave: Clearing hover for Item[Id={item.Id}]");
            _coordinator.ClearHover();
        }

        return Task.CompletedTask;
    }

    private DropPosition GetDropPosition(DragEventArgs e)
    {
        // Determine position based on Y coordinate within row
        // If in upper half, drop before; if in lower half, drop after
        var yOffset = e.ClientY % RowHeight;
        return yOffset < RowHeight / 2.0 ? DropPosition.Before : DropPosition.After;
    }

    /// <summary>
    /// Validates whether the source item can be dropped on the target item.
    /// </summary>
    private bool CanDropOnTarget(TGridItem source, TGridItem target)
    {
        // Synthetic rows are never valid drop targets
        if (ReorderingHelpers.IsSyntheticRow(target))
            return false;

        // Can't drop on self
        if (source.Id == target.Id)
            return false;

        // Check grouping constraints if grouping is active
        if (_coordinator?.GroupingCoordinator?.ActiveGrouping is not null && !AllowCrossGroupReorder)
        {
            var activeGrouping = _coordinator.GroupingCoordinator.ActiveGrouping;
            var sourceGroupKey = activeGrouping.GroupByUntyped(source);
            var targetGroupKey = activeGrouping.GroupByUntyped(target);

            // Use key comparer if available, otherwise use default equality
            var keyComparer = activeGrouping.KeyComparer;
            var sameGroup = keyComparer is not null
                ? keyComparer.Equals(sourceGroupKey, targetGroupKey)
                : Equals(sourceGroupKey, targetGroupKey);

            if (!sameGroup)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Executes the reorder operation, firing appropriate events.
    /// </summary>
    private async Task<ReorderResult> ExecuteReorderAsync(RowBeforeReorderEventArgs<TGridItem> args)
    {
        LogDebug($"ExecuteReorder:ENTRY - DraggedItem[Id={args.DraggedItem.Id}] -> TargetItem[Id={args.TargetItem.Id}], Position={args.Position}");

        if (_dataSource is null)
        {
            LogDebugError("ExecuteReorder: DataSource is null");
            throw new InvalidOperationException("RowReorderFeature requires a ReorderableDataSource. Bind the grid's Items to ReorderableDataSource.Items.");
        }

        try
        {
            // Fire OnBeforeReorder event
            LogDebug("ExecuteReorder: Firing OnBeforeReorder event");
            await InvokeEventCallbackAsync(OnBeforeReorder, args);
        }
        catch (Exception ex)
        {
            // Event handler threw - cancel with reason
            LogDebugError($"ExecuteReorder: OnBeforeReorder threw exception: {ex.Message}");
            var cancelledArgs = new RowReorderCancelledEventArgs<TGridItem>
            {
                Item = args.DraggedItem,
                Reason = $"Event handler exception: {ex.Message}"
            };

            await InvokeEventCallbackAsync(OnReorderCancelled, cancelledArgs);
            return ReorderResult.Cancelled;
        }

        // Check if cancelled by event handler
        if (args.Cancel)
        {
            LogDebugWarn($"ExecuteReorder: Cancelled by OnBeforeReorder handler - Reason: {args.CancelReason}");
            var cancelledArgs = new RowReorderCancelledEventArgs<TGridItem>
            {
                Item = args.DraggedItem,
                Reason = args.CancelReason ?? "Cancelled by event handler"
            };

            await InvokeEventCallbackAsync(OnReorderCancelled, cancelledArgs);
            return ReorderResult.Cancelled;
        }

        // Execute the reorder
        try
        {
            LogDebug($"ExecuteReorder: Moving item - Position={args.Position}");
            if (args.Position == DropPosition.Before)
            {
                _dataSource.MoveItemBefore(args.DraggedItem, args.TargetItem);
            }
            else
            {
                _dataSource.MoveItemAfter(args.DraggedItem, args.TargetItem);
            }

            // Fire OnRowReordered event
            var newIndex = _dataSource.IndexOf(args.DraggedItem);
            LogDebug($"ExecuteReorder: Move complete - NewIndex={newIndex}");

            var reorderedArgs = new RowReorderedEventArgs<TGridItem>
            {
                Item = args.DraggedItem,
                OldIndex = args.OldIndex,
                NewIndex = newIndex,
                NewOrder = _dataSource.CurrentOrder
            };

            LogDebug("ExecuteReorder: Firing OnRowReordered event");
            await InvokeEventCallbackAsync(OnRowReordered, reorderedArgs);
            LogDebug("ExecuteReorder:EXIT - SUCCESS");
            return ReorderResult.Success;
        }
        catch (Exception ex)
        {
            LogDebugError($"ExecuteReorder: Move operation failed: {ex.Message}");
            var cancelledArgs = new RowReorderCancelledEventArgs<TGridItem>
            {
                Item = args.DraggedItem,
                Reason = $"Reorder failed: {ex.Message}"
            };

            await InvokeEventCallbackAsync(OnReorderCancelled, cancelledArgs);
            return ReorderResult.Failed;
        }
    }

    private void ValidateDataSource()
    {
        if (_dataSourceValidated)
            return;

        if (_dataSource is null)
            throw new InvalidOperationException("RowReorderFeature requires a ReorderableDataSource. Bind the grid's Items to ReorderableDataSource.Items.");

        _dataSourceValidated = true;
    }

    private async Task InvokeEventCallbackAsync<T>(EventCallback<T> callback, T args)
    {
        if (!callback.HasDelegate)
            return;

        if (_context?.InvokeAsync is not null)
        {
            await _context.InvokeAsync(async () => await callback.InvokeAsync(args));
        }
        else
        {
            // Assume already on UI thread
            await callback.InvokeAsync(args);
        }
    }

    private void OnCoordinatorStateChanged()
    {
        // Request UI refresh when drag state changes
        _context?.RequestRefresh?.Invoke();
    }

    private static ComposableGrid<TGridItem> GetGridOrThrow(FeatureContext<TGridItem> context)
    {
        if (context.Grid is not ComposableGrid<TGridItem> grid)
            throw new InvalidOperationException("RowReorderFeature must be used inside a ComposableGrid.");

        return grid;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_coordinator is not null)
        {
            _coordinator.OnStateChanged -= OnCoordinatorStateChanged;
        }

        _context = null;
        _coordinator = null;
        _dataSource = null;
    }
}
