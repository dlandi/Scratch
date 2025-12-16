# Formatting Parity Specification (Backlog)

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.1 |
| Status | ?? BACKLOG |
| Created | 2025-12-16 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns.Features.Formatting` |
| Branch | `Composable_WIP` |

> This specification captures **Phase B – Formatting parity** from the ComposableColumns parity plan. Work is low priority until higher-value tasks complete.

---

## 1. Purpose

Bring the composable formatting story to parity with the legacy `FormattedValueColumn` demo by adding reusable formatter features, runtime culture/date/numeric switching, and updated demos that exercise the composable pipeline exclusively.

---

## 2. Scope

- Applies to formatting-related composable features (format strings, custom formatters, tooltip/icon helpers, future formatter pipeline).
- Targets demo content currently in `Pages/FormattedColumnDemo.razor` and related formatter helpers under `QuickGridTest01.FormattedValue`.
- Excludes editing, filtering, or styling (covered by other backlog specs).

Out of scope:
- Memoization/caching (tracked separately in `Memoization.md`).
- Row expansion or conditional styling (already addressed elsewhere).

---

## 3. Current State

| Area | Legacy Demo Capability | Composable Support | Gap |
|------|-----------------------|--------------------|-----|
| Formatters | 30+ formatter delegates (currency, numeric, date, duration, file size) | `FormatStringFeature`, `CustomFormatterFeature` | Missing formatter catalog & helpers |
| Culture Switching | Dropdowns for culture, date/time, numeric styles | None | No runtime culture switch UI or binding hooks |
| Demo Coverage | Dedicated `FormattedColumnDemo.razor` page | Only small samples in `ComposableColumnDemo` | Need parity demo using `ComposableColumn` |

Conclusion: everyday formatting works, but the specialized formatter library and runtime selectors have not been ported.

---

## 4. Goals & Success Criteria

1. **Formatter Feature Set** – Provide composable features (or helpers) that encapsulate the formatter catalog from the legacy demo.
2. **Runtime Switchers** – Demonstrate culture/date/numeric selection driving composable columns without bespoke components.
3. **Demo Migration** – Update `FormattedColumnDemo.razor` (or create a new composable demo) to showcase the new features; retire legacy-only components afterward.

Success is measured by:
- ? All formatter scenarios in the old demo have a composable equivalent.
- ? Culture/date/numeric dropdowns bind to composable features.
- ? Documentation references composable formatting only.

---

## 5. Proposed Approach

| Phase | Objective | Deliverables |
|-------|-----------|--------------|
| B1 | Catalog formatting helpers | Shared formatter services (currency, numeric, date, duration, file size, relative) exposed via composable features or extension methods |
| B2 | Runtime switch integration | Components or feature parameters that react to selected culture/date/numeric styles |
| B3 | Demo + docs | Composable-focused formatting demo, updated specs/docs, deprecation plan for legacy components |

---

## 6. Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Scope creep (too many formatter types) | High | Prioritize top use cases: currency, numeric, percent, date/time, relative, file size |
| Duplicate logic between helper libraries and features | Medium | Centralize formatter delegates in one service reused by features |
| Culture switching complexity | Medium | Use `CultureInfo` binding + `IFormatProvider` abstractions; avoid per-cell state |

---

## 7. Backlog Tasks (Draft)

1. Inventory formatter delegates in `QuickGridTest01.FormattedValue.*`.
2. Design composable formatter feature API (e.g., `FormatterFeature<T>` or specialized `CurrencyFormatterFeature`).
3. Implement culture/date/numeric selector component that feeds parameters into composable columns.
4. Rebuild formatting demo using `ComposableGrid` + features; remove legacy column usage.
5. Update docs (`CustomFeaturesMatrix`, `ComposableColumnsParity_01.md`) and mark legacy components for retirement.

---

## 8. Priority & Next Steps

- **Priority:** Low (execute after higher-value composable work stabilizes).
- **Immediate Next Step:** None; leave in backlog until capacity frees up or formatting parity becomes blocking.
