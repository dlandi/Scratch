# Memoization Feature Specification (Backlog)

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 0.1 |
| Status | ?? BACKLOG / LOW PRIORITY |
| Created | 2025-12-16 |
| Target Framework | ASP.NET 9 Blazor Server |
| Namespace | `QuickGridTest01.ComposableColumns` |
| Branch | `Composable_WIP` |

> **Note:** This feature is considered optional and of marginal value. It remains in the backlog for completeness but is not planned for near-term work.

---

## 1. Purpose

Provide a reusable caching helper ("memoization") for composable column features that repeatedly compute the same derived value (e.g., CSS class strings, formatted text). The legacy `OptimizedColumn` demo showcased a localized cache; this spec records the idea so it is not lost, but adoption is unlikely unless profiling proves a bottleneck.

---

## 2. Current State

- Memoization exists only inside `OptimizedColumn` (`BenchmarkClassCaching` caches CSS classes for highlight/warning/error combinations).
- No composable feature currently performs enough repeated heavy computation to justify a shared cache layer.
- The `ComposableColumnDemo` simply lists `MemoizationFeature` in the "Coming Soon" bucket; no implementation work has started.

Conclusion: The benefit is unclear; caching adds state management complexity with little measured payoff.

---

## 3. Goals (If Implemented)

1. **Optional cache helper** that features can opt into when profiling shows hotspots.
2. **Bounded memory usage** (small LRU or capped dictionary) to avoid unbounded growth.
3. **Feature-level integration** so caching can be inserted before expensive formatting/styling steps.

## 4. Non-Goals

- No automatic caching for all features.
- No commitment to ship unless performance data warrants it.
- Not a replacement for expression compilation or other proven optimizations.

---

## 5. Proposed Shape (Concept Only)

| Component | Description |
|-----------|-------------|
| `IMemoizationStore` | Scoped cache interface exposed on `FeatureContext`. |
| `MemoizationFeature<TGridItem>` | Optional feature that initializes the cache and provides helper methods such as `GetOrAdd`. |
| `MemoizationKey` helpers | Static helpers ensuring keys consider item identity + feature + aspect name. |

Usage sketch:
```csharp
var memoFeature = new MemoizationFeature<Product>(capacity: 256);
memoFeature.WithCachedFormatter("PriceCss", (item, context) => BuildCss(item.Price));
```

Again, this is purely aspirational; no code should be written until a concrete need emerges.

---

## 6. Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Added state pressure | Medium | Keep caches bounded and optional |
| Complexity without benefit | High | Require profiling data before implementation |
| Thread-safety issues | Medium | Use per-render-context caches, avoid static shared dictionaries |

---

## 7. Priority & Next Steps

- **Priority:** Low / Nice-to-have only if performance profiling demonstrates repeated expensive work.
- **Next Steps:** None. Leave in backlog; revisit if future formatter/styling features expose measurable hotspots.
