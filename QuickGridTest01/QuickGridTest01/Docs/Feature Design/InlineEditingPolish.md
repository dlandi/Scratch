# Inline Editing Polish Specification

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 1.1 |
| Status | ? **DESIGN APPROVED – READY FOR IMPLEMENTATION** |
| Created | 2025-12-16 |
| Updated | 2025-12-17 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Editing` |
| Branch | `Composable_WIP` |
| Key Decisions | A2.2 (Grid-cascaded stream), B2.1 (Option 5: Auto-panel + manual placement) |

> Captures **Phase A – Inline Editing polish** from the ComposableColumns parity plan. This is the next feature slated for implementation.
>
> **Major Decisions Made:**
> - **Event Mechanism (A2.2):** Grid provides `IEditEventStream` via cascading value; features opt-in with `ShowEvents=true`
> - **Placement API (B2.1):** Grid has optional `EventPanelPlacement` parameter for auto-rendering; manual placement still supported
> - See Section 8 (Key Design Decisions) for full details and Section 9 (Implementation Summary) for component list
>
> **Namespace Compliance:** All new code MUST be created within `QuickGridTest01.ComposableColumns.*` namespace hierarchy. Use existing `IValidator<T>` and `ValidationResult` from `Features.Editing.Validators.cs`.

---

## 1. Purpose

Enhance the composable inline editing experience (`InlineEditingFeature<TItem, TValue>`) with optional observability hooks (change log, analytics) and UX refinements inspired by the legacy `EditableColumn` demo, while keeping the on-blur validation philosophy.

---

## 2. Scope

- Applies to `InlineEditingFeature` and supporting validators/components within `QuickGridTest01.ComposableColumns.*`.
- Focus on telemetry/change-log hooks, validation summaries, and demo updates.
- Must preserve existing behavior: always-on editors, on-blur validation, no debounce timers.

Out of scope:
- New editor types (already delivered).
- Debounce-based auto-save (explicitly avoided per product guidance).
- Formatter/culture work (tracked separately in `FormattingParity.md`).

---

## 3. Current State

| Aspect | Legacy Demo (`EditableColumn`) | Composable Feature | Gap |
|--------|--------------------------------|--------------------|-----|
| Change Log | Real-time log panel showing edits, saves, cancels | None | No built-in event stream/logging |
| Analytics Hooks | Counters for saves/cancels/errors | None | Need optional callbacks/metrics |
| Validation Summary | Side panel showing rule descriptions / results | Not provided | Need validation summary shell component |

Conclusion: core editing works, but there is no optional logging/analytics UX equivalent to the handcrafted demo.

**Existing Infrastructure (verified in codebase):**
- `IValidator<TValue>` interface in `Features.Editing.Validators.cs` with `Name` property and `ValidateAsync` method
- `ValidationResult` class in `Features.Editing.Validators.cs` with `IsValid`, `ErrorMessage`
- Built-in validators: `RequiredStringValidator`, `StringLengthValidator`, `EmailValidator`, `PatternValidator`, etc.
- `InlineEditingFeature` already has `OnValueChanged` and `OnValidationCompleted` callbacks

---

## 4. Goals & Success Criteria

1. **Event Hooks:** Provide optional events or services to capture edit lifecycle (value change, validation result, commit/cancel) for logging/analytics.
2. **Demo Experience:** Update `ComposableColumnDemo.razor` (or new sample) to showcase change-log UI using the hooks.
3. **Validation Summary Shell:** A shared component that renders the active validation rule list plus the latest validation result for the focused cell. Purely presentational (bound to read-only data supplied by event hooks) so teams can host it wherever they like or replace it with their own UX. Supports placement options (top, right, left, bottom of the grid).

Success indicators:
- ? Developers can subscribe to edit events via feature parameters or DI services without touching legacy components.
- ? Demo shows change log similar to `EditableColumnDemo` but powered by composable features.
- ? On-blur validation remains the default; no debouncing timers introduced.
- ? Validation summary panel shows rule descriptors and results from event stream.

---

## 5. Proposed Approach

### Phase A: Core Event Infrastructure

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| A0.1 | Baseline feature tests | Create `InlineEditingFeatureTests.cs` covering existing behavior: value get/set, on-blur validation trigger, dirty tracking, editor rendering |
| A0.2 | Baseline validator tests | Ensure existing `IValidator<T>` implementations used by editing feature have unit test coverage |
| A1.1 | Enumerate existing callbacks | List current `InlineEditingFeature` events (`OnValueChanged`, `OnValidationCompleted`, etc.) and where they fire |
| A1.2 | Map lifecycle scenarios | Document edit flows (start, change, blur success/fail, cancel) with expected events |
| A1.3 | Identify gaps | Produce gap report noting missing events or payload data for commit/cancel/validation states |
| A2.1 | Draft payload contracts | Define structs/records carrying item key, old/new value, validation state, timestamps. Also define `IEditEventStream` interface, `EditEventStream` implementation, event base types (`EditEventBase`, `EditCommittedEvent`, `EditCancelledEvent`, `ValidationFailedEvent`), and `ValidationRuleDescriptor` record (`Name`, `Description`, `Severity`) for grid-level event aggregation. |
| A2.2 | Stream implementation details | Define `IEditEventStream` interface members (RecentEvents, PublishAsync, EventPublished event), `EditEventStream` class with 100-event limit, disposal semantics, and threading guidance (synchronous invocation, grid-scoped lifecycle) |
| A3.1 | Implement event publishing | Add `ShowEvents` parameter to `InlineEditingFeature`. When true, publish lifecycle events to cascaded `IEditEventStream` (commit, cancel, validation with rule descriptors). Add guards to avoid overhead when `ShowEvents=false` or stream unavailable. Keep existing `OnValueChanged`/`OnValidationCompleted` callbacks unchanged. |
| A3.2 | Callback payload tests | Unit tests verifying event payloads contain correct item key, old/new values, timestamps, property names when published to stream |
| A3.3 | Event order tests | Integration tests simulating focus ? change ? blur flows and asserting events publish to stream in expected sequence |
| A3.4 | Opt-in behavior tests | Confirm no events publish and no performance overhead when `ShowEvents=false` or stream is null |
| A3.5 | Backward-compat smoke test | Verify existing `ComposableColumnDemo` editing scenarios still work unchanged without `ShowEvents` wired |
| A3.6 | Validation event tests | Confirm validation events published to stream include rule descriptors, severity, and error messages |
| A3.7 | Telemetry safeguards | Ensure stream publishes respect on-blur policy, avoid debounce timers, and handle grid disposal correctly |
| A4.1 | Coverage matrix | Publish matrix mapping lifecycle scenarios to stream events for documentation and future regression tests |
| A5.1 | Stream usage examples | Provide sample code showing: (1) Grid auto-rendering panel with `EventPanelPlacement`, (2) Manual panel placement consuming cascaded stream, (3) Custom event viewer implementation |
| A5.2 | Grid integration | Update `ComposableGrid` to instantiate and provide `EditEventStream` via cascading value, conditionally render `<EditEventViewer>` based on `EventPanelPlacement` parameter |

### Phase B: Demo & Change-Log UI

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| B1.1 | Demo data plumbing | Extend `ComposableColumnDemo.razor(.cs)` with state to hold event stream, counters, and placement setting |
| B1.2 | Event binding | Hook demo columns to new callbacks/observer and populate the change-log store |
| B2.1 | Layout styles | Provide CSS/layout guidance: (1) Built-in styles for auto-rendered panel (`.composable-grid-with-panel`), (2) Utility classes for manual layouts (`.demo-layout-horizontal`, `.demo-layout-vertical`), (3) Responsive behavior examples |
| B3.1 | Change-log UI | Build `EditEventViewer` component: consumes `IEditEventStream` via cascading parameter, renders event log with filtering/clearing, supports both auto-render (by grid) and manual placement patterns |
| B4.1 | InlineEditorFeatures doc update | Document new callbacks, observer pattern, and demo usage |
| B4.2 | Parity spec cross-link | Update `ComposableColumnsParity_01.md` (and release notes if any) to reference the new capabilities |
| B4.3 | Parity state update | Update `ComposableColumnsParity_01.md` Current State table to mark Inline Editing gap as Closed |

### Phase C: Validation Summary Shell

| Task ID | Objective | Deliverables |
|---------|-----------|--------------|
| C1.1 | Validation event emission | Extend `ValidationFailedEvent` to include `ValidationRuleDescriptor[]` (rule name, description, severity) from validators' `Name` property. Ensure validation success events also publish to stream. |
| C1.2 | Focused cell tracking | Add mechanism to track currently focused cell for validation summary display (property name, item key) |
| C2.1 | Shell component build | Create `ValidationSummaryPanel.razor`: consumes `IEditEventStream` via cascading parameter, displays active validators for focused column, shows latest validation result, honors `Placement` parameter |
| C2.2 | Sample styling | Provide CSS for validation shell: rule list, pass/fail indicators, severity badges |
| C2.3 | Shell component tests | Unit tests verifying shell renders validation data correctly and responds to placement settings |

---

## 6. Priority & Next Steps

- **Priority:** High – designated as the next feature to implement.
- **Next Step:** Kick off Task A0.1 (baseline feature tests) then proceed through the A-series tasks.

**Recommended Implementation Order:**
```
A0.1 ? A0.2 ? A1.1-A1.3 (discovery)
       ?
A2.1-A2.2 (contracts + stream details)
       ?
A3.1 ? A3.2-A3.7 (implementation + tests)
       ?
A4.1 ? A5.1-A5.2 (documentation + grid integration)
       ?
B1.1-B1.2 ? B2.1 ? B3.1 (demo + styles + EditEventViewer)
       ?
B4.1-B4.3 (documentation updates)
       ?
C1.1-C1.2 ? C2.1-C2.3 (ValidationSummaryPanel)
```

---

## 7. Key Design Decisions

### Decision 1: Event Mechanism ? **DECIDED**

**Decision:** Grid-cascaded event stream with feature-level opt-in

**Approach:**
- `ComposableGrid` provides `IEditEventStream` via `<CascadingValue>`
- `InlineEditingFeature` adds `ShowEvents` parameter (default: `false`)
- When `ShowEvents=true`, feature publishes lifecycle events to cascaded stream
- Keep existing `OnValueChanged` and `OnValidationCompleted` callbacks unchanged

**Rationale:**
- Automatic event aggregation across columns
- Grid-scoped streams (multiple grids on one page have separate streams)
- Fine-grained opt-in per column
- Zero overhead when not used

**Reference:** See `Task B2.1 Placement API – Deep Dive.md` Option 4 architecture

---

### Decision 2: Panel Placement API ? **DECIDED**

**Decision:** Option 5 – Grid-Provided Stream + Optional Auto-Panel

**Approach:**

**Simple Path (Auto-Rendering):**
```razor
<ComposableGrid Items="@items" EventPanelPlacement="Placement.Right">
    <Columns>
        <ComposableColumn Property="@(p => p.Name)">
            <Features>
                <InlineEditingFeature ShowEvents="true" />
            </Features>
        </ComposableColumn>
    </Columns>
</ComposableGrid>
```

**Advanced Path (Manual Placement):**
```razor
<div class="demo-layout-horizontal">
    <ComposableGrid Items="@items">
        <Columns>
            <ComposableColumn Property="@(p => p.Name)">
                <Features>
                    <InlineEditingFeature ShowEvents="true" />
                </Features>
            </ComposableColumn>
        </Columns>
    </ComposableGrid>
    
    <EditEventViewer Placement="Placement.Right" />
</div>
```

**Key Features:**
- Grid adds +1 optional parameter: `EventPanelPlacement` (default: `Placement.None`)
- When `!= None`, grid auto-renders `<EditEventViewer>` in wrapper div
- Manual placement still fully supported (panel binds to cascaded stream)
- Same `Placement` enum used for both auto-rendering and styling hints

**Rationale:**
- **Progressive disclosure:** Simple for prototypes, flexible for production
- **Aligns with `RowColumn` pattern:** Auto-renders overlay like `RowColumn` does
- **Minimal bloat:** Only +1 optional parameter on grid
- **No breaking changes:** Default `None` means existing grids unchanged

**Reference:** See `Task B2.1 Placement API – Deep Dive.md` for full 5-option analysis

---

### Decision 3: Placement Enum Values ? **DECIDED**

**Values:**
```csharp
public enum Placement
{
    None,   // No auto-render; default styling for manual panels
    Top,    // Panel above grid
    Right,  // Panel to right of grid
    Bottom, // Panel below grid
    Left    // Panel to left of grid
}
```

**Dual Purpose:**
1. **On `ComposableGrid.EventPanelPlacement`:** Controls auto-rendering and wrapper layout
2. **On `EditEventViewer.Placement`:** Provides styling hints (border direction, scroll behavior)

**Future Extensions:** Can add `Overlay`, `FloatingTopRight`, `Modal`, etc. without breaking changes

---

### Decision 4: Validation Rule Descriptors ? **DECIDED**

**Decision:** Leverage existing `IValidator<T>.Name` property for rule identification

**Approach:**
- `ValidationRuleDescriptor` record: `Name` (from validator), `Description` (optional), `Severity` (enum: Info/Warning/Error)
- `ValidationFailedEvent` includes array of `ValidationRuleDescriptor` for failed rules
- Existing validators already have `Name` property (e.g., `"Required"`, `"Length(2-50)"`, `"Email"`)

**Rationale:**
- No changes required to existing validator implementations
- Rule descriptors derived at validation time from validator instances
- Severity can default to `Error` for most validators

---

## 8. Implementation Summary

### Components to Create/Modify

| Component | Change Type | Location | Description |
|-----------|-------------|----------|-------------|
| `Placement.cs` | **New** | `Features.Editing` | Enum with `None/Top/Right/Bottom/Left` |
| `IEditEventStream.cs` | **New** | `Features.Editing` | Interface with `RecentEvents`, `PublishAsync`, `EventPublished` |
| `EditEventStream.cs` | **New** | `Features.Editing` | Implementation with 100-event limit, disposal |
| `EditEventBase.cs` | **New** | `Features.Editing` | Abstract base for event payloads |
| `EditCommittedEvent.cs` | **New** | `Features.Editing` | Payload for successful commits |
| `EditCancelledEvent.cs` | **New** | `Features.Editing` | Payload for Escape-key cancellations |
| `ValidationFailedEvent.cs` | **New** | `Features.Editing` | Payload for validation failures with rule descriptors |
| `ValidationRuleDescriptor.cs` | **New** | `Features.Editing` | Record: `Name`, `Description`, `Severity` |
| `EditEventViewer.razor` | **New** | `Features.Editing` | Panel component consuming cascaded stream |
| `ValidationSummaryPanel.razor` | **New** | `Features.Editing` | Validation rules/results panel |
| `ComposableGrid.razor` | **Modify** | `Core` | Add `EventPanelPlacement` param, provide cascaded stream, conditionally render panel |
| `EditingFeatures.cs` | **Modify** | `Features.Editing` | Add `ShowEvents` param, publish to stream |
| `ComposableColumnDemo.razor` | **Modify** | `Pages` | Showcase both auto and manual placement patterns |

### Testing Strategy

| Test Type | Coverage |
|-----------|----------|
| **Unit Tests** | Stream implementation, event payload contents, disposal, ValidationRuleDescriptor creation |
| **Integration Tests** | Event order, opt-in behavior, cascading parameter binding, validation summary updates |
| **Smoke Tests** | Backward compatibility (existing demos work unchanged) |
| **UI Tests** | Auto-rendered panel positioning, manual placement layouts, validation summary display |

---

## 9. Migration Path

### For Existing Consumers

**No action required** – all changes are opt-in:

- Existing `InlineEditingFeature` usage unchanged (no `ShowEvents` param = no events)
- Existing `ComposableGrid` usage unchanged (no `EventPanelPlacement` param = no panel)

### For New Features

**Quick Start (Demos/Prototypes):**
```razor
<ComposableGrid EventPanelPlacement="Placement.Right">
    <Columns>
        <ComposableColumn Property="@(p => p.Name)">
            <Features>
                <InlineEditingFeature ShowEvents="true" />
            </Features>
        </ComposableColumn>
    </Columns>
</ComposableGrid>
```

**Production (Global Sidebar):**
```razor
<Layout>
    <Sidebar>
        <EditEventViewer />
        <ValidationSummaryPanel />
    </Sidebar>
    <Main>
        <ComposableGrid>
            <Columns>
                <ComposableColumn Property="@(p => p.Name)">
                    <Features>
                        <InlineEditingFeature ShowEvents="true" />
                    </Features>
                </ComposableColumn>
            </Columns>
        </ComposableGrid>
    </Main>
</Layout>
```

---

## 10. Appendix: File Locations

All new files created in `QuickGridTest01.ComposableColumns.Features.Editing` namespace:

```
QuickGridTest01/
??? ComposableColumns/
?   ??? Core/
?   ?   ??? ComposableGrid.razor          # Modify
?   ??? Features/
?       ??? Editing/
?           ??? EditingFeatures.cs        # Modify (InlineEditingFeature)
?           ??? Validators.cs             # Existing (IValidator<T>, ValidationResult)
?           ??? Placement.cs              # New
?           ??? IEditEventStream.cs       # New
?           ??? EditEventStream.cs        # New
?           ??? EditEventBase.cs          # New
?           ??? EditCommittedEvent.cs     # New
?           ??? EditCancelledEvent.cs     # New
?           ??? ValidationFailedEvent.cs  # New
?           ??? ValidationRuleDescriptor.cs # New
?           ??? EditEventViewer.razor     # New
?           ??? ValidationSummaryPanel.razor # New
??? Pages/
    ??? ComposableColumnDemo.razor        # Modify
