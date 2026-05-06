# How to Use the Current Sprint Board

**Guide ID:** GHE-ALM-029
**Audience:** Scrum Master, Engineering Manager, Project Manager
**Primary role:** Scrum Master
**Classification:** Manager Performs
**Estimated time:** 10 minutes per standup; one-time 20-minute view setup
**Required permissions:** Project: Read for viewing the board; Project: Write to move items between columns, update the **Status** field, or save shared view changes
**Prerequisites:**

- An organization-level GitHub Project exists for the team (see GHE-ALM-006).
- A **Sprint** iteration field is configured on the Project with at least one current iteration (see GHE-ALM-027).
- A **Status** field exists on the Project with the values: Backlog, Ready, In Progress, In Review, Blocked, Ready for Release, Done.
- Sprint candidate work has been planned and assigned to the current iteration (see GHE-ALM-028).

**When to use this guide:** Use this guide every working day to run a standup, refresh sprint status, and surface blockers using a Project board view scoped to the current sprint.

**When not to use this guide:** Do not use this guide for backlog grooming, multi-sprint roadmap reviews, or release-level reporting; use the Product Backlog View, Roadmap, or Release dashboards instead.

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named "Current Sprint Board" filtered to the active iteration and grouped by **Status**.
- A repeatable standup process that updates the **Status** field for every committed sprint item.

## Before You Start

- Confirm the Project URL, the Project name, and the iteration field name (commonly **Sprint**, sometimes **Iteration**).
- Confirm the current sprint name and dates so you can sanity-check the filter result.
- Confirm you have **Write** access if you intend to move cards or rename the view; otherwise, plan to ask a Project maintainer to create the shared view.

## Steps

### Create or select the Current Sprint board view

1. Open the Project in your browser. The view tabs appear along the top of the Project, just below the Project title.
2. If a view named **Current Sprint** already exists, click it and skip to step 7. Otherwise, click the **+** (**New view**) tab at the end of the view tab strip.
3. In the new view, click the view tab name (it defaults to a generic label) and rename it to `Current Sprint`.
4. Click the view options control next to the search bar (labeled **View options** when expanded).
5. Under **Layout**, click **Board**. The view switches from a table to a card-based board.
6. Still in **View options**, set **Column field** to **Status**. Each **Status** value becomes a board column.

> [SCREENSHOT: New Project view named "Current Sprint" with Layout set to Board and Column field set to Status]

### Filter the board to the active sprint

7. Click the **Filter** bar at the top of the view (the input that begins with a magnifying glass).
8. Enter the filter expression for the active iteration. For a field literally named **Sprint**, use:

   `sprint:@current`

   For a field named **Iteration**, use `iteration:@current`. The board now shows only items assigned to the active iteration.
9. Optionally append common scope filters, for example:

   `sprint:@current is:open`

   to hide items already closed before standup, or `sprint:@current assignee:@me` for an individual view.
10. Click the **Save changes** button on the view tab (or the down-arrow next to the view name and then **Save changes**) so the filter, layout, and grouping persist for the team.

> [SCREENSHOT: Current Sprint board with the filter `sprint:@current` applied and Save changes visible]

### Confirm the column set matches the sprint workflow

11. Verify columns appear in this order, left to right: **Backlog**, **Ready**, **In Progress**, **In Review**, **Blocked**, **Ready for Release**, **Done**. If a value is missing, open the **Status** field configuration (Project **Settings**, **Fields**, **Status**) and add the value, or request the change from a Project maintainer.
12. If columns appear in the wrong order, drag the column headers on the board to reorder them. The order is saved with the view.

### Run a standup with the board

13. Open the **Current Sprint** view at the start of standup and share your screen.
14. Walk the board right to left, starting at **Ready for Release**, then **In Review**, then **In Progress**, then **Blocked**, then **Ready**. Discuss work that is closest to done first to flush completed items and surface review bottlenecks.
15. For each card, the assignee states: what moved since yesterday, what they will do today, and any blocker. As status changes are reported, drag the card to the new column. Dropping a card updates the **Status** field on the underlying issue.
16. For any item moved to **Blocked**, capture the blocker in an issue comment and, where appropriate, set a blocking dependency on the related issue (see GHE-ALM-031).
17. End standup at the **Ready** column. If **Ready** is empty and **In Progress** is light, pull the next-priority item from **Ready** into **In Progress** and assign an owner. If **Ready** is empty across the team, escalate to the Product Owner for re-prioritization.

> [SCREENSHOT: Standup walkthrough showing a card being dragged from In Progress to In Review]

### Recognize blocked or stale items

18. Treat any card in **Blocked** as a daily action item. Confirm an owner is named, the blocker is captured in a comment, and a target unblock date is set.
19. Treat any card in **In Progress** with no comment activity in the last two working days as stale. Ask the assignee for a status update during standup.
20. Treat any card in **In Review** older than the team's review SLA (commonly 24 to 48 hours) as a review bottleneck. Reassign reviewers or escalate to the Engineering Manager.
21. At sprint mid-point, count cards still in **Backlog** or **Ready** within the current sprint filter. If the remaining-work column count is high relative to days remaining, flag scope risk to the Product Owner and consider moving items out (see GHE-ALM-030).

## Validation Checklist

- [ ] The view is named **Current Sprint** and is saved as a shared Project view.
- [ ] The layout is **Board** and the column field is **Status**.
- [ ] The filter `sprint:@current` (or the equivalent for the iteration field name) is applied.
- [ ] Columns appear in the order: Backlog, Ready, In Progress, In Review, Blocked, Ready for Release, Done.
- [ ] Every card visible on the board belongs to the active iteration.
- [ ] After standup, every committed item has a **Status** value that reflects today's reality.

## Common Mistakes

- Filtering by sprint name text (for example, `sprint:"Sprint 27"`) instead of `sprint:@current`. Hard-coded names break the next iteration and force manual edits each sprint.
- Grouping by **Assignee** instead of **Status**. Assignee grouping hides flow problems and turns standup into a status round-robin.
- Leaving items in **In Review** after merge. The merge does not always advance **Status**; confirm that automation moves merged items to **Ready for Release** or **Done**, and update manually if it does not.
- Using the **Blocked** column as a parking lot. Blocked items must have a written blocker and an owner; otherwise they are invisible work.
- Creating a personal view but not saving it. Unsaved view changes do not persist for teammates and the next session reverts to the default layout.
- Adding items to the board mid-sprint without setting the **Sprint** field. Items without a sprint value disappear from the `sprint:@current` filter and become silent scope.

## Escalation Path

- GitHub administrator: Not applicable for daily use. Involve only if Project access is denied across the team.
- Repository administrator: Not applicable.
- Engineering lead: Involve when **In Review** items consistently exceed the review SLA, when blockers require cross-team coordination, or when sprint scope is at risk by mid-sprint.
- Release manager: Involve when items reach **Ready for Release** and need to be associated with a release tag, milestone, or deployment window.

## Related Guides

- GHE-ALM-027 : How to Configure or Request a Sprint Iteration Field
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-031 : How to Monitor Blocked Sprint Work
- GHE-ALM-032 : How to Close a Sprint Review
