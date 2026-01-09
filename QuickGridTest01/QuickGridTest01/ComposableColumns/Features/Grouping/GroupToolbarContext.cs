using Microsoft.AspNetCore.Components;

namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public sealed record GroupToolbarContext(
    RenderFragment<GroupToolbarContext>? ToolbarTemplate,
    Func<Task> ExpandAllAsync,
    Func<Task> CollapseAllAsync);
