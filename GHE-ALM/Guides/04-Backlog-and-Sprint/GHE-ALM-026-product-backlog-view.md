# How to Use the Product Backlog View

**Guide ID:** GHE-ALM-026
**Audience:** Product Owner, Engineering Manager, Project Manager
**Primary role:** Product Owner
**Classification:** Manager Performs
**Estimated time:** 30-minute one-time view setup; 30 to 45 minutes per weekly triage pass
**Required permissions:** Project: Read to view the backlog; Project: Write to save the shared view, change grouping or sorting, and edit field values during triage
**Prerequisites:**

- An organization-level GitHub Project exists for the team (see GHE-ALM-006).
- The Project has the canonical fields **Status**, **Priority**, **Product Area**, **Target Date**, and **Sprint** configured.
- New issues from team repositories are being added to the Project, either manually or through automation (see GHE-ALM-008).
- A working understanding of GitHub Project layouts and view tabs (see GHE-ALM-005).

**When to use this guide:** Use this guide to set up and run weekly triage of all work that is not yet committed to a sprint, including new feature requests, requirements awaiting refinement, and undecided bugs.

**When not to use this guide:** Do not use this guide for active sprint execution, daily standups, release-level scope reviews, or formal bug triage. Use the Current Sprint Board, the Release Roadmap, or the Bug Triage View instead.

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named "Product Backlog" filtered to exclude completed and in-flight sprint work, grouped by **Priority** or **Product Area**, and sorted by **Priority** then **Target Date**.
- A repeatable weekly triage pass that walks the view top to bottom and leaves every visible item with a current **Status**, **Priority**, **Product Area**, and target date or rejection.

## Before You Start

- Confirm the Project URL, the Project name, and the iteration field name (commonly **Sprint**, sometimes **Iteration**).
- Confirm the agreed grouping field for your team. The default is **Priority** for prioritization passes; switch to **Product Area** when the triage goal is balancing intake across components.
- Confirm you have **Write** access if you intend to save the shared view, change field values during triage, or add new fields. Otherwise, plan to ask a Project maintainer to create and save the shared view.
- Confirm the agreed definition of "backlog" with the team. The locked filter in this guide treats backlog as anything not in a closed state and not assigned to the current sprint.

## Steps

### Create or select the Product Backlog view

1. Open the Project in your browser. The view tabs appear along the top of the Project, just below the Project title.
2. If a view named **Product Backlog** already exists, click it and skip to step 7. Otherwise, click the **+** (**New view**) tab at the end of the view tab strip.
3. In the new view, click the view tab name (it defaults to a generic label) and rename it to `Product Backlog`.
4. Click the view options control next to the search bar (labeled **View options** when expanded).
5. Under **Layout**, click **Table**. The view renders as a flat table of items with one row per issue or pull request.
6. In **View options**, set the visible fields to at least: **Title**, **Status**, **Priority**, **Product Area**, **Target Date**, **Sprint**, **Owner**. Hide noisy fields such as **Linked pull requests** and **Reviewers** for this view.

> [SCREENSHOT: New Project view named "Product Backlog" with Layout set to Table and the visible field list expanded in View options]

### Filter the view to unplanned work

7. Click the filter bar at the top of the view (the input that begins with a magnifying glass).
8. Enter the locked backlog filter:

   `status:"Backlog","Triage","Ready" -sprint:@current`

   This shows items in pre-sprint statuses and excludes anything already pulled into the active iteration. If your team uses a different set of pre-sprint **Status** values, substitute them. If your iteration field is named **Iteration**, replace `sprint:` with `iteration:`.
9. Append `is:open` to hide items that have been closed without a status update:

   `status:"Backlog","Triage","Ready" -sprint:@current is:open`

10. Click the **Save changes** button on the view tab (or the down-arrow next to the view name and then **Save changes**) so the filter persists for the team.

> [SCREENSHOT: Product Backlog view with the locked filter applied and Save changes visible on the view tab]

### Group and sort for triage

11. In **View options**, set **Group by** to **Priority**. Items collapse into priority bands (for example, P0 through P3 on a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership). Empty bands appear as "No Priority" and become an explicit triage queue.
12. Set **Sort by** to **Priority** (ascending) and add a secondary sort on **Target Date** (ascending). The highest-priority items with the soonest target dates appear at the top of each group.
13. Save the view again so the grouping and sort persist.
14. Optional: clone the view as **Product Backlog by Area**, change **Group by** to **Product Area**, and save. Use the area-grouped clone for intake balancing across components such as `Checkout`, `Billing`, and `Identity`.

> [SCREENSHOT: Product Backlog view grouped by Priority with collapsible Priority bands and sort indicators on Priority and Target Date]

### Run the weekly triage pass

15. Open the **Product Backlog** view at the start of the weekly triage meeting and share your screen. Start at the top group (highest priority or first product area, depending on which clone you opened).
16. For each row, confirm the following fields are set: **Status**, **Priority**, **Product Area**, **Owner** (proposed, not committed), and **Target Date** when one is known. Edit fields inline by clicking the cell.
17. For items in the "No Priority" group, assign a **Priority** value during this pass or close the item with a reason. The "No Priority" group should be empty at the end of triage.
18. For items still in **Triage** **Status**, decide one of three outcomes during the pass: promote to **Ready** (refined enough to enter a sprint), keep in **Backlog** (valid but not refined), or close as `not planned`. Capture the decision in a short comment.
19. For items in **Ready** **Status** that are top-of-group and have a **Target Date** within the next two sprints, mark them as candidates for the next sprint planning session. Sprint assignment itself happens in GHE-ALM-028, not here.
20. At the end of the pass, scan the bottom of each group for stale items: anything with a **Target Date** in the past, anything in **Backlog** older than 90 days with no recent activity, or anything still missing **Product Area**. Close, reassign, or escalate.

## Validation Checklist

- [ ] The view is named **Product Backlog** and is saved as a shared Project view.
- [ ] The layout is **Table** and the visible fields include **Status**, **Priority**, **Product Area**, **Target Date**, **Sprint**, and **Owner**.
- [ ] The filter excludes items in the current sprint and excludes closed items.
- [ ] Group by is set to **Priority** (or **Product Area** on the area-grouped clone) and sort is **Priority**, then **Target Date**.
- [ ] The "No Priority" group is empty after each weekly triage pass.
- [ ] Every item still visible after triage has a **Status**, a **Priority**, a **Product Area**, and either a **Target Date** or a comment explaining why one is not yet known.

## Common Mistakes

- Filtering by sprint name text (for example, `sprint:"Sprint 27"`) instead of `sprint:@current`. Hard-coded names break each iteration and let in-sprint work leak into the backlog view.
- Grouping by **Status** instead of **Priority** or **Product Area**. Status grouping duplicates information already in the Current Sprint Board and hides prioritization decisions.
- Including **Done** or closed items by omitting `is:open` and the sprint exclusion. The view fills with completed work and triage signal is lost.
- Treating the Product Backlog view as a bug queue. Bugs with severity and customer impact belong in the Bug Triage View (see GHE-ALM-034); only undecided defects are appropriate here.
- Saving personal view changes as the shared view without team agreement. Coordinate filter, grouping, and sort changes with the Product Owner before clicking **Save changes**.
- Letting the "No Priority" group persist between weekly passes. Unprioritized items are the leading indicator of triage debt.

## Escalation Path

- GitHub administrator: Not applicable for routine triage. Involve only if Project access is denied across the team.
- Repository administrator: Not applicable.
- Engineering lead: Involve when the backlog grows faster than triage can clear it, when **Product Area** assignment is contested, or when high-priority items lack an engineering owner.
- Release manager: Involve when items in **Ready** with near-term **Target Date** values need to be mapped to a release window or milestone.

## Related Guides

- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-008 : How to Add Existing Issues and Pull Requests to a Project
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-034 : How to Use the Bug Triage View
