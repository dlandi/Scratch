# Options / unresolved decisions still present in Plan_ExpandableRowFeature.md:
## 1.	GetRowId(context, item) misconfiguration handling
### •	If RowKey exists but does not return an int: fall back to Id or throw. (Line 248)
Answer: Fall back to Id

## 2.	Duplicate expansion feature enforcement (“sentinel”)
### •	If OnAttach(FeatureContext<TGridItem>) sees an existing RowStateManager<TGridItem> registered: throw (fail fast) or allow by sharing the service / no-op attach. (Lines 264–266)
Answer: Throw (fail fast)

## 3.	Cell render pipeline behavior (renderNext)
### •	Expansion feature as full owner of cell rendering (never call renderNext()) vs supporting a mode where it calls renderNext() (e.g., to also show the underlying value). (Lines 325–332)
Answer:

## 4.	Default close affordance implementation strategy
### •	Provide close via an inline close button rendered by the feature vs wrap expanded content in RowCard when enabled/available. Document currently says “prefer wrapping in RowCard… verify repo patterns”. (Lines 351–368)
Answer: Both Can Be true.

There needs to be a minimal RowCard component in the composable feature package that provides the basic container while allowing a user template to supply the details.
Recommendation (strict RowColumn parity)
Reuse the existing API shape from QuickGridTest01.RowColumn.Components.RowCard to avoid spec/API drift.
Create a new component under QuickGridTest01.ComposableColumns.Features.Expansion.Components with the same parameters:
Title (optional)
Class (optional)
HeaderActions (optional RenderFragment)
FooterContent (optional RenderFragment)
ShowCloseButton (bool)
OnClose (Func<Task>?) (optional)
ChildContent (RenderFragment?)
Close behavior (expansion-friendly)
When OnClose is not supplied, the default implementation should collapse via the cascaded RowExpandedContext<TGridItem>:
OnClose ??= () => Context?.CollapseAsync() ?? Task.CompletedTask;
This keeps RowCard usable both inside expansion overlays (where a cascading context exists) and outside them (where a caller can supply OnClose).
Base feature requirement: even if a user-provided ExpandedTemplate does not render its own close UI, the base expansion feature must still provide a consistent way to collapse (e.g., built-in close button in the default RowCard, or a standard close affordance rendered by the feature itself when expanded).
“User definable internal definition” clarification
Users do not override RowCard internals; they customize and/or replace it via:
ExpandedTemplate rendering entirely custom markup (full replacement)
providing HeaderActions / FooterContent / ChildContent to the packaged RowCard (parameterized customization)

## 5.	Migration strategy for legacy types
### •	Option A: move shared types and update legacy references vs Option B: duplicate types and leave legacy untouched. (Lines 411–424)
 Answer: Option A: move shared types and update legacy references

## 6.	Test strategy for UI behavior (section 8.2)
### •	Either set up bUnit (and describe it precisely) or remove/replace 8.2 with a deterministic non-bUnit strategy. Right now it says “may not be set up” and defaults to manual validation. (Lines 473–482)

 Answer: Option B.