using Microsoft.AspNetCore.Components;
using QuickGridTest01.ComposableColumns.Features.Grouping.Enums;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed record GroupHeaderContext<TGridItem, TValue>(
    object? Key,
    string? ColumnId,
    int ItemCount,
    bool IsExpanded,
    GroupSortDirection GroupOrder,
    RenderFragment<GroupHeaderContext<TGridItem, TValue>>? HeaderTemplate,
    Func<Task> ToggleAsync);
