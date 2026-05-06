# How to Plan the Next Sprint

**Guide ID:** GHE-ALM-028
**Audience:** Scrum Master, Product Owner, Engineering Manager
**Primary role:** Scrum Master
**Classification:** Manager Performs
**Estimated time:** 60 to 90 minutes per sprint planning session; one-time 20-minute view setup
**Required permissions:** Project: Read to view candidate work; Project: Write to create or save the planning view, set the **Sprint** field, and update other project fields
**Prerequisites:**

- An organization-level GitHub Project exists for the team (see GHE-ALM-006).
- A **Sprint** iteration field is configured on the Project with at least one upcoming iteration defined (see GHE-ALM-027).
- A **Status** field, an **Effort** field, a **Priority** field, an **Owner** field, and a **Product Area** field exist on the Project.
- The team's effort capacity for the next sprint has been agreed (story points, hours, or whatever unit the team uses, see GHE-ALM-033).

**When to use this guide:** Use this guide once per sprint cycle, typically near the end of the current sprint, to assemble a candidate list of work for the next sprint and confirm each candidate is ready for commitment.

**When not to use this guide:** Do not use this guide for daily standup execution, mid-sprint scope changes, or release-level planning; use the Current Sprint Board, the Product Backlog View, or the Roadmap instead.

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named **Sprint Planning** filtered to next-sprint candidates and unscheduled work, grouped by **Type**.
- A vetted candidate list whose total **Effort** fits the team's agreed capacity, with each item assigned to the next iteration via the **Sprint** field.

## Before You Start

- Confirm the Project URL, the Project name, and the iteration field name (commonly **Sprint**, sometimes **Iteration**).
- Confirm the next iteration name and dates from the iteration field configuration.
- Confirm the team's effort capacity for the next sprint and the unit being used.
- Coordinate with the Product Owner so priority order is current before the planning session.

## Steps

### Open or create the Sprint Planning view

1. Open the Project in your browser. The view tabs appear along the top of the Project, just below the Project title.
2. If a view named **Sprint Planning** already exists, click it and skip to step 7. Otherwise, click the **+** (**New view**) tab at the end of the view tab strip.
3. Click the new view tab name and rename it to `Sprint Planning`.
4. Click the view options control next to the search bar. Under **Layout**, click **Table**. The table layout reads densely and is the right layout for scanning a candidate list.
5. Still in **View options**, set **Group by** to **Type**. Items will cluster under headings such as Epic, Feature, Requirement, Task, and Bug, which makes the mix of work obvious at a glance.
6. Under **Fields**, hide everything except **Title**, **Type**, **Status**, **Effort**, **Priority**, **Owner**, **Product Area**, **Sprint**, and **Parent issue**. Clean visible columns help the team focus on the data needed to commit.

> [SCREENSHOT: New Sprint Planning view set to Table layout, grouped by Type, with the visible-fields panel showing Effort, Priority, Owner, and Product Area selected]

### Filter the view to next-sprint candidates

7. Click the **Filter** bar at the top of the view.
8. Enter the locked candidate filter:

   `sprint:@next OR no:sprint`

   The view now shows items already pre-staged for the next iteration plus any item that has no sprint assignment yet, which is the full candidate pool.
9. Add `is:open` to hide already-closed work that should not enter planning:

   `sprint:@next OR no:sprint is:open`

10. Click **Save changes** on the view tab so the filter, layout, grouping, and field set persist for the team.

> [SCREENSHOT: Sprint Planning table filtered with `sprint:@next OR no:sprint`, grouped by Type, with Save changes visible]

### Sort and prioritize the candidate pool

11. Click the column header for **Priority** and set the sort to ascending so P0 and P1 work appears first within each Type group. If priority is single-select, the field's defined order controls the sort.
12. Within each Type group, scan top to bottom. Mark candidates by setting the **Sprint** field to the next iteration name on items the team intends to commit.
13. Apply selection rules in this order, top-down per group:
    - Carry-over: any item already in `sprint:@next` from a prior planning pass stays unless explicitly deferred.
    - Priority: P0 and P1 items take precedence over P2 and P3.
    - Effort fit: prefer right-sized items (1 to 5 story points, or the team's equivalent) over a single oversized item.
    - Readiness: skip items that fail issue hygiene (see step 16) and queue them for refinement instead.
14. After each selection, glance at the running total of **Effort** for items where **Sprint** is set to the next iteration. The running total is the sprint commitment estimate.

### Run the issue-hygiene gate before commitment

15. For every candidate marked into the next sprint, open the issue in a side panel and confirm each of the following is present:
    - **Acceptance criteria** in the issue body, written as a checklist or testable statements.
    - **Owner** assigned (the person accountable, not just the author).
    - **Effort** estimate set as a number, not blank and not a placeholder like `?`.
    - **Parent** linked via sub-issue relationship to the Epic, Feature, or Requirement the work belongs to.
    - **Product Area** set to a single value from the team's defined list.
    - **Target Release** set on the **Release** field if the work belongs to a planned release train.
16. Any item missing one or more of the above fails the hygiene gate. Remove it from the next sprint by clearing the **Sprint** field, label it for refinement, and route it back to the Product Owner. Do not commit unhealthy work, no matter how high the priority.

> [SCREENSHOT: Issue side panel open from the Sprint Planning view showing acceptance criteria, Owner, Effort, Parent issue, Product Area, and Release populated]

### Confirm capacity and commit

17. Sum the **Effort** values for all items where **Sprint** equals the next iteration. Compare this sum to the team's agreed capacity for the sprint.
18. If the total exceeds capacity, defer the lowest-priority items by clearing their **Sprint** field. If the total is well below capacity, pull the next candidates from the `no:sprint` group and run them through the hygiene gate (steps 15 to 16).
19. Hold the capacity conversation with the team explicitly. Confirmed capacity should account for known time off, on-call rotations, support load, and meeting overhead. The Effort total is a planning input, not a contract.
20. Once the candidate list and the capacity total are agreed, the sprint is committed. The next iteration becomes `@current` automatically when the iteration date range begins.

## Validation Checklist

- [ ] The view is named **Sprint Planning** and is saved as a shared Project view.
- [ ] The layout is **Table**, grouped by **Type**, with **Effort**, **Priority**, **Owner**, and **Product Area** visible.
- [ ] The filter `sprint:@next OR no:sprint` (with `is:open` if used) is applied.
- [ ] Every committed item has the **Sprint** field set to the next iteration.
- [ ] Every committed item passes the hygiene gate: acceptance criteria, Owner, Effort, Parent, Product Area, and Target Release where required.
- [ ] The total **Effort** of committed items does not exceed the team's agreed capacity.
- [ ] Items that failed the hygiene gate have their **Sprint** field cleared and are routed for refinement.

## Common Mistakes

- Using the stale syntax `Sprint = @next`. The locked filter syntax is `sprint:@next`; the equals form is no longer valid in Project filters.
- Filtering by hardcoded sprint name (for example, `sprint:"Sprint 27"`). Hardcoded names break next planning cycle and force manual edits.
- Skipping the `no:sprint` clause. Without it, the view hides unscheduled work and the team cannot see the full candidate pool.
- Committing items that fail the hygiene gate "to refine in sprint". Acceptance criteria and effort estimates produced mid-sprint usually become scope creep, not refinement.
- Treating the Effort sum as a commitment instead of an input to the capacity conversation. Capacity discussions must factor in time off, on-call, and support load.
- Forgetting to clear the **Sprint** field on items deferred late in the planning session. Stale assignments make the Current Sprint Board misleading on day one.
- Letting the Product Owner reorder priorities after the hygiene gate has run. Re-prioritization mid-planning forces a second pass and risks committing unvetted work.

## Escalation Path

- GitHub administrator: Not applicable for routine planning. Involve only if Project access is denied across the team.
- Repository administrator: Not applicable.
- Engineering lead: Involve when capacity is consistently exceeded, when too many candidates fail the hygiene gate, or when work breakdown decisions require architectural input.
- Release manager: Involve when next-sprint candidates are tied to an in-flight release whose target date or scope is at risk.

## Related Guides

- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
- GHE-ALM-027 : How to Configure or Request a Sprint Iteration Field
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-033 : How to Use Effort or Story Points in GitHub Projects
