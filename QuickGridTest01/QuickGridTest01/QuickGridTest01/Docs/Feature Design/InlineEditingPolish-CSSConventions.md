# CSS Conventions for InlineEditingPolish

> **Note:** This is a companion document to `InlineEditingPolish.md` Section 11.
> These conventions apply to tasks B2.1, B3.1, C2.1, and C2.2.

## Global Stylesheet Policy

All ComposableColumns-related CSS **MUST** be placed in the global stylesheet:

```
wwwroot/css/qgComposable-refined-minimalism.css
```

**Do NOT create scoped `.razor.css` files** for ComposableColumns components. This policy ensures:
- Consistent use of design system tokens
- No CSS isolation issues with dynamically rendered content
- Single source of truth for grid-related styles

## Before Adding New CSS

**Always check for existing classes first:**

| Pattern | Existing Classes | Location |
|---------|------------------|----------|
| Grid with panel layout | `.composable-grid-with-panel`, `.composable-grid-main` | Lines 546-560 |
| Event panel positioning | `.event-panel-top`, `.event-panel-bottom`, `.event-panel-left`, `.event-panel-right` | Lines 570-600 |
| Event viewer | `.edit-event-viewer`, `.event-viewer-header`, `.event-viewer-list` | Lines 609-690 |
| Event items | `.event-item`, `.event-item.event-committed`, `.event-item.event-failed`, etc. | Lines 691-785 |
| Badges/severity | `.qg-badge-success`, `.qg-badge-warning`, `.qg-badge-error` | In `quickgrid-refined-minimalism.css` |

## Design Tokens to Use

Always use CSS custom properties from the design system:

```css
/* Spacing */
var(--space-4, 0.5rem)
var(--space-8, 1rem)
var(--space-12, 1.5rem)

/* Colors */
var(--color-surface, #ffffff)
var(--color-canvas, #f8f9fa)
var(--color-border-default, #dee2e6)
var(--color-text-primary, #1a1a1a)
var(--color-text-secondary, #525252)
var(--color-success, #28a745)
var(--color-error, #dc3545)
var(--color-warning, #ffc107)

/* Typography */
var(--font-size-sm, 0.875rem)
var(--font-size-xs, 0.75rem)
var(--font-weight-semibold, 600)

/* Layout */
var(--card-radius, 4px)
var(--duration-fast, 150ms)
```

## Naming Convention

New classes should follow the existing patterns:
- Component container: `.{component-name}` (e.g., `.validation-summary-panel`)
- Child elements: `.{component-name}-{element}` (e.g., `.validation-summary-header`)
- State modifiers: `.{component-name}.{state}` (e.g., `.validation-rule-item.failed`)

## Task-Specific CSS Notes

| Task | CSS Guidance |
|------|--------------|
| **B2.1** | Check existing `.composable-grid-with-panel`, `.event-panel-*`. Add `.demo-layout-horizontal`, `.demo-layout-vertical` if needed. |
| **B3.1** | COMPLETED in A5.2. `EditEventViewer.razor` styles already in global CSS. |
| **C2.1** | No scoped .razor.css for `ValidationSummaryPanel.razor`. |
| **C2.2** | Add `.validation-summary-panel`, `.validation-rule-list`, `.validation-rule-item`, `.validation-result` to global CSS. |

---
