using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using QuickGridTest01.ComposableColumns.Core;

namespace QuickGridTest01.Pages;

public partial class ComposableFeatureMatrix
{
    private string _selectedCategory = "";
    private List<string> _categories = new();
    private List<ComposableFeatureRow> _features = new();
    private IQueryable<FeaturePriorityInfo> _featurePriorities = default!;

    private IQueryable<ComposableFeatureRow> FilteredFeatures => string.IsNullOrEmpty(_selectedCategory)
        ? _features.AsQueryable()
        : _features.Where(f => f.Category == _selectedCategory).AsQueryable();

    protected override void OnInitialized()
    {
        InitializeFeatures();
        InitializeFeaturePriorities();
        _categories = _features.Select(f => f.Category).Distinct().OrderBy(c => c).ToList();
    }

    private void InitializeFeatures()
    {
        // Curated list: this is intentionally authored (not reflected) because the pipeline is configurable.
        _features =
        [
            // Core / Infrastructure
            new(
                Category: "Core",
                Name: "CompiledAccessorFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Core",
                Kind: "Accessor / Infrastructure",
                Summary: "Compiles the property expression into fast get/set delegates and stores metadata (property name/type) into feature state.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Core",
                Name: "AutoTitleFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Core",
                Kind: "Core",
                Summary: "Infers the column title from the bound property name (optionally splitting PascalCase).",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Core",
                Name: "SortableFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Core",
                Kind: "Core",
                Summary: "Participates in sorting by providing a sort function based on the property expression.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),

            // Formatting
            new(
                Category: "Formatting",
                Name: "FormatStringFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Formatting",
                Kind: "Cell Render",
                Summary: "Formats values using an IFormattable format string (e.g., C2/N0/yyyy-MM-dd).",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Formatting",
                Name: "CustomFormatterFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Formatting",
                Kind: "Cell Render",
                Summary: "Formats values using a caller-provided formatter function.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),

            // Styling
            new(
                Category: "Styling",
                Name: "IconFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Styling",
                Kind: "Cell Render",
                Summary: "Maps values to icons for visual status/indicators.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Styling",
                Name: "ConditionalCssFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Styling",
                Kind: "Cell Render",
                Summary: "Applies CSS classes based on value-driven rules (e.g., stock thresholds).",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Styling",
                Name: "TooltipFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Styling",
                Kind: "Cell Render",
                Summary: "Adds tooltips to cells using configured text/formatters.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),

            // Editing
            new(
                Category: "Editing",
                Name: "InlineEditingFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Editing",
                Kind: "Cell Render",
                Summary: "Always-visible inline editor with blur-based validation, dirty tracking, and optional event publishing.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Editing",
                Name: "InlineEditingFeature (Auto)",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Editing",
                Kind: "Cell Render",
                Summary: "EditorKind.Auto selects editor type based on the property type.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Editing",
                Name: "InlineEditingFeature (RadioGroup)",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Editing",
                Kind: "Cell Render",
                Summary: "Renders enum values as radio buttons (supports OptionText).",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Editing",
                Name: "InlineEditingFeature (DataAnnotations)",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Editing",
                Kind: "Validation",
                Summary: "UseDataAnnotations discovers validation attributes on the bound property.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),
            new(
                Category: "Editing",
                Name: "InlineEditingFeature (ShowEvents)",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Editing",
                Kind: "Events",
                Summary: "Publishes edit lifecycle events to IEditEventStream for logging/viewer.",
                DemoRoutes:
                [
                    new("Composable Column", "/composable-demo")
                ]),

            // Filtering
            new(
                Category: "Filtering",
                Name: "FilterFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Filtering",
                Kind: "Grid Filter",
                Summary: "Adds per-column filtering with operators, debouncing, and integration into a grid-level filtering pipeline.",
                DemoRoutes:
                [
                    new("Filter Test 2", "/filter-test-2"),
                    new("Filter Test 3", "/filter-test-3"),
                    new("Filter Test 4", "/filter-test-4")
                ]),

            // Grouping
            new(
                Category: "Grouping",
                Name: "GroupingFeature",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Grouping",
                Kind: "Grid Transform / UI",
                Summary: "End-to-end grouping building block that coordinates grouping state, transforms the data source, and renders group headers/tooling.",
                DemoRoutes:
                [
                    new("Composable Grouping", "/composable-grouping-demo"),
                    new("Composable Grouping (Custom)", "/composable-grouping-custom-demo")
                ]),

            // Expansion
            new(
                Category: "Expansion",
                Name: "Row Expansion (Feature Set)",
                Namespace: "QuickGridTest01.ComposableColumns.Features.Expansion",
                Kind: "Grid Transform / UI",
                Summary: "Row expansion pipeline pieces (row identification, state, templates, and behaviors) used by the expandable-row demos.",
                DemoRoutes:
                [
                    new("Composable Row Expand", "/composable-row-expand-demo")
                ])
        ];
    }

    private void InitializeFeaturePriorities()
    {
        _featurePriorities = new List<FeaturePriorityInfo>
        {
            new("Infrastructure", FeaturePriority.Infrastructure, "Property expression, compiled accessor"),
            new("Grouping", FeaturePriority.Grouping, "Group state, grouped data source, group headers/tooling"),
            new("Core", FeaturePriority.Core, "Type traits, auto-title inference"),
            new("Filtering", FeaturePriority.Filtering, "Filter state, operator analysis, toolbar render/apply"),
            new("Formatting", FeaturePriority.Formatting, "Format string, custom formatter, culture"),
            new("Styling", FeaturePriority.Styling, "Conditional CSS, icons, tooltips"),
            new("Expansion", FeaturePriority.Expansion, "Row expansion identification, state, templates"),
            new("Editing", FeaturePriority.Editing, "Inline editing, edit state, debounce"),
            new("Validation", FeaturePriority.Validation, "Validators, data annotations"),
            new("Events", FeaturePriority.Events, "Value changed, state changed, before edit"),
            new("Performance", FeaturePriority.Performance, "Memoization, minimal DOM, set key"),
            new("Final", FeaturePriority.Final, "Final wrapper features")
        }.AsQueryable();
    }

    public sealed record DemoRoute(string Label, string Route);

    public sealed record ComposableFeatureRow(
        string Category,
        string Name,
        string Namespace,
        string Kind,
        string Summary,
        List<DemoRoute> DemoRoutes);

    public sealed record FeaturePriorityInfo(string Category, int Priority, string Description);
}
