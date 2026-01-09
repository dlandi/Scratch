# ComposableColumns Parity Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.2 |
| Status | ?? SPEC AUTHORING |
| Created | 2025-12-16 |
| Updated | 2025-12-17 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns` |
| Branch | `Composable_WIP` |

---

## 1. Overview

### 1.1 Purpose

This document defines the backlog feature for **ComposableColumns Parity**. The goal is to close the remaining gaps between the new composable pipeline (`ComposableGrid`, `ComposableColumn`, and feature system) and the legacy demo components (e.g., `EditableColumn`, `FormattedValueColumn`, `FilterableColumn`). Once the gaps are closed we can retire the older demo pages and unify documentation around the composable approach.

### 1.2 Scope

- Applies to all features implemented inside `QuickGridTest01.ComposableColumns.*`
- Compares against demo pages **outside** the namespace (e.g., `FormattedColumnDemo.razor`, `ConditionalStyleDemo.razor`, `FilterTest*.razor`) except `InlineEditor.razor` (intentionally naïve)
- Focuses on UX/feature parity rather than exact UI copies

### 1.3 Out of Scope

- `InlineEditor.razor` (kept only as an anti-pattern reference)
- Any QuickGrid features unrelated to composable columns

---

## 2. Current State

| Capability | Legacy Demo | Composable Feature | Gap |
|------------|-------------|--------------------|-----|
| Inline editing | `EditableColumnDemo.razor` | `InlineEditingFeature<T>` | **Closed** (event stream, change log UI) |
| Formatting | `FormattedColumnDemo.razor` (`FormattedValueColumn`) | `FormatStringFeature`, `CustomFormatterFeature`, `TooltipFeature` | Medium (runtime culture switchers, formatter catalog) |
| Styling | `ConditionalStyleDemo.razor`, `IconFeature` samples | `ConditionalCssFeature`, `IconFeature` | Closed |
| Filtering | `FilterTest4.razor`, `FilterableColumn` | `FilterFeature<T>` + `ComposableGrid` toolbar | Closed |
| Upcoming pages | See Feature Specs folder (Row expanders, memoization, etc.) | Partial | Medium |

Conclusion: filtering/styling/inline editing gaps are closed; formatting/culture switching remains the biggest delta.

---

## 3. Goals & Success Criteria

1. **Functional Parity:** Every legacy demo scenario should be reproducible using `ComposableColumn` + features without bespoke components.
2. **Documentation Alignment:** Feature specs reference composable implementations only; legacy pages become historical or are removed.
3. **Retirement Readiness:** Once parity tasks ship, the legacy demo pages can be deleted without losing showcased functionality.

Success is measured by:
- ? Demos in `ComposableColumnDemo.razor` (or dedicated composable samples) replicate each legacy feature scenario
- ? Feature specs in `Docs/Feature Design/` updated with composable architecture references
- ? Legacy pages marked for removal (tracked issue) once parity confirmed

---

## 4. Gap Breakdown

### 4.1 Inline Editing Enhancements (Minor)
- Retain **on-blur validation** (no timer/debouncer)
- Optional backlog item: change log/telemetry hook similar to `EditableColumnDemo`
- Optional backlog item: reusable UX shell for validation summary cards

### 4.2 Advanced Formatting (Medium)
- Runtime culture/date/numeric selectors
- Formatter catalog (file sizes, relative dates, durations, etc.) exposed via composable features or helper services
- Potential `FormatterFeature<T>` that accepts delegates/strategy objects

### 4.3 Remaining Demo Pages to Port
- `FormattedColumnDemo.razor`
- `ConditionalStyleDemo.razor` (mostly done but keep spec for confirmation)
- Any other feature specs referenced under `Docs/Feature Design/`

---

## 5. Work Plan (High Level)

| Phase | Objective | Key Deliverables |
|-------|-----------|------------------|
| Phase A | Inline Editing polish | Feature hooks for change log / analytics (optional) |
| Phase B | Formatting parity | Formatter feature(s), culture switch UI, demo updates |
| Phase C | Spec & demo cleanup | Update docs, retire legacy pages, ensure tests/demos run |

Each phase will receive its own execution report similar to `Phase-1-execution-report.md` once active.

---

## 6. Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Overlapping work across demos | Medium | Track via this spec; ensure each legacy page has a composable replacement story |
| Formatting scope creep | High | Prioritize formatter catalog subset (currency, numeric, date/time, relative) first |
| UX drift when retiring pages | Medium | Capture screenshots + behavior notes before removal |

---

## 7. Next Actions

1. Review existing feature specs (`Docs/Feature Design/*.md`) to enumerate remaining demo pages
2. Create execution plan for Phase B (Formatting parity) referencing this spec
3. Update backlog/issue tracker with tasks referencing `ComposableColumnsParity_01`

---

## 8. Related Documentation

### 8.1 Inline Editing (Gap Closed)

The inline editing gap has been fully closed with the implementation of the Event Stream feature:

| Document | Description |
|----------|-------------|
| [InlineEditingPolish.md](InlineEditingPolish.md) | Full specification for event stream implementation |
| [InlineEditorFeatures.md](InlineEditorFeatures.md) | InlineEditingFeature gap closure + event stream integration |
| [EditEventStreamSpec.md](EditEventStreamSpec.md) | Technical specification of IEditEventStream |
| [EditEventStreamUsageExamples.md](EditEventStreamUsageExamples.md) | Usage patterns and code samples |
| [EditEventCoverageMatrix.md](EditEventCoverageMatrix.md) | Test coverage for event scenarios |

**Key Capabilities Added:**
- `ShowEvents` parameter on `InlineEditingFeature` to enable event publishing
- `IEditEventStream` with `EventPublished` event and `RecentEvents` collection
- `EditEventViewer` component for displaying change log
- `EventPanelPlacement` parameter on `ComposableGrid` for auto-rendering
- Event types: `EditStartedEvent`, `EditCommittedEvent`, `EditCancelledEvent`, `ValidationFailedEvent`, `ValidationSucceededEvent`

**Demo Reference:** See `ComposableColumnDemo.razor` ? "Edit Event Stream Demo" section

### 8.2 Formatting (In Progress)

See [FormattingParity.md](FormattingParity.md) for the formatting gap analysis and work plan.
