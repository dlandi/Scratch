# How to Monitor Blocked Sprint Work

**Guide ID:** GHE-ALM-031
**Audience:** Engineering Manager, Project Manager, Scrum Master
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 30-minute one-time setup, then 5 minutes daily and 20 minutes mid-sprint
**Required permissions:** Project: Write; Repository: Triage on the underlying repositories

**Prerequisites:**

- An organization Project with a current Sprint board configured (see GHE-ALM-029).
- A `Sprint` iteration field with the active sprint set as the current iteration.
- A `Status` field that includes a `Blocked` option, plus the standard execution states (`Todo`, `In Progress`, `In Review`, `Done`).
- Issue dependencies enabled on the underlying repositories (see GHE-ALM-019).

**When to use this guide:** Use this during an active sprint to surface risks before they become slips. Run the daily scan at standup and the mid-sprint risk review around day 5 of a two-week sprint.

**When not to use this guide:** Do not use this for sprint planning, sprint close-out, or backlog grooming. Those activities are covered by GHE-ALM-028, GHE-ALM-032, and GHE-ALM-026.

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named `Sprint Risk` that surfaces blocked, stale, and dependency-bound work in the active sprint.
- A repeatable daily scan routine for standup.
- A mid-sprint risk review checklist that produces a written list of items needing intervention.

## Before You Start

- Confirm the sprint name and that the iteration is marked current.
- Confirm that your team uses `Status: Blocked` as the explicit blocked state. If your team uses a label or a separate field, adapt the filter syntax accordingly.
- Have the names of the team's owners ready so you can spot missing assignees quickly.

## Steps

### Create the Sprint Risk view

1. Open the organization Project that tracks the active sprint.
2. Click the **+** tab next to the existing view tabs to add a new view.
3. Choose **Table** as the layout. Table layout exposes the most metadata at once and is the right surface for risk scanning.
4. Click the new view's tab name and rename it to `Sprint Risk`.
5. Click the **Filter** input at the top of the view and enter the base filter `sprint:@current is:open`. This scopes the view to open work in the current iteration only.
6. Click the **Group** control and group by **Status**. Grouping by status puts the `Blocked` group at the top of your view in one block.
7. Click the **Sort** control and add a secondary sort by **Updated** ascending. Oldest-touched items rise to the top of each status group, which is how you spot stale work.
8. Click the field visibility control and ensure these columns are visible: **Title**, **Status**, **Assignees**, **Priority**, **Sprint**, **Updated**, **Linked pull requests**. Hide fields that are not relevant to risk so the view stays scannable.
9. Click **Save changes** on the view tab to lock in the layout for your team.

> [SCREENSHOT: Sprint Risk view in table layout, grouped by Status, showing the Blocked group expanded at the top]

### Add the three risk filters as saved variants

10. With the `Sprint Risk` view selected, duplicate the tab by right-clicking and choosing **Duplicate view**. Rename the copy to `Sprint Risk - Blocked`.
11. On `Sprint Risk - Blocked`, change the filter to `sprint:@current status:"Blocked"`. This is the explicit-blocked signal. Save the view.
12. Duplicate the original `Sprint Risk` view again and rename to `Sprint Risk - Stale`. Set the filter to `sprint:@current is:open -status:"Done"` and confirm the secondary sort is **Updated** ascending. Items at the top of this view have not been touched recently and are your stale signal.
13. Duplicate once more and rename to `Sprint Risk - No Owner`. Set the filter to `sprint:@current is:open no:assignee`. This is the missing-owner signal.
14. Optional: duplicate a fourth time and name it `Sprint Risk - Dependencies`. Set the filter to `sprint:@current is:open` and group by **Linked pull requests** or by a custom `Blocked By` field if your team maintains one. Issue dependency relationships also render in the issue side panel; the saved view is for scanning, the side panel is for confirming the chain.

> [SCREENSHOT: View tab strip showing Sprint Risk, Sprint Risk - Blocked, Sprint Risk - Stale, Sprint Risk - No Owner, Sprint Risk - Dependencies]

### Run the daily scan during standup

15. Open `Sprint Risk - Blocked` first. For each item, ask the assignee one question: what is the unblock action, and who owns it? If no unblock action exists, the item should move to the next sprint or have its scope cut. Capture the decision in the issue as a comment.
16. Open `Sprint Risk - No Owner` next. Every item in this view is a planning gap. Assign an owner during standup or remove the item from the sprint. Do not leave items unassigned past the first standup of the sprint.
17. Open `Sprint Risk - Stale`. Look at items at the top of the list whose **Updated** date is older than two business days. For each one, ask the assignee for a one-line status. Stale and silent is the early-warning signal that the work is actually blocked but not labeled as such.
18. Close standup by recording in the sprint channel the count of blocked items, the count of unowned items, and the count of stale items. Three numbers, every standup. Trend matters more than absolute count.

### Run the mid-sprint risk review

19. Around day 5 of a two-week sprint, open `Sprint Risk - Blocked` and review every item with the assignee one-on-one or in a 20-minute sync. For each blocked item, decide: unblock today, descope, or move to next sprint using the procedure in GHE-ALM-030.
20. Open `Sprint Risk - Dependencies` and walk the dependency chains. An item that is `Blocked By` another sprint item is at risk if the upstream item is itself blocked, stale, or unowned. Two-deep chains are normal; three-deep chains in a single sprint usually mean the breakdown was wrong and the work needs to be cut.
21. For each at-risk item, post a comment on the issue summarizing the decision: keep, cut, or carry. Tag the assignee. The comment is the audit trail.
22. Update the sprint channel with the mid-sprint risk summary: total at-risk count, decisions made, and any commitment changes for the sprint review meeting (GHE-ALM-032).

> [SCREENSHOT: Issue side panel showing the Blocked By dependency relationship and a recent risk-review comment]

## Validation Checklist

- [ ] The `Sprint Risk` view and its four variants are saved on the Project and visible to the team.
- [ ] All four variants use `sprint:@current` and not a hardcoded sprint name.
- [ ] The daily scan produces three counts (blocked, unowned, stale) recorded in the team channel.
- [ ] Every item in `Sprint Risk - Blocked` has a comment naming the unblock action and the owner of that action.
- [ ] No item remains in `Sprint Risk - No Owner` after the first standup of the sprint.
- [ ] The mid-sprint review produces a written keep / cut / carry decision on each at-risk item.

## Common Mistakes

- Hardcoding the sprint name (for example `sprint:"Sprint 27"`) instead of using `sprint:@current`. The view stops working the day the sprint rolls over.
- Treating `Status: Blocked` as a parking lot. Blocked is a temporary state with a named unblock action and a named owner of that action. If neither exists, the item belongs in the backlog or the next sprint.
- Confusing stale with blocked. Stale items are silent, not labeled. Treat the `Sprint Risk - Stale` view as the early-warning system; do not wait for items to be marked blocked.
- Walking only the explicit blocked list and skipping unowned and stale items. Two of the three risk signals are quiet. The dashboard only works if you scan all three.
- Using labels such as `blocked` instead of the `Status` field. Labels are not first-class in the iteration board's grouping or sort, and the filter syntax fragments across teams. Pick `Status` and stay with it.
- Reviewing dependency chains only at sprint planning. Chains break mid-sprint when an upstream item slips; the mid-sprint check is when you catch it.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve when issue dependencies are not enabled on a repository whose work is in the sprint, or when filter syntax behaves unexpectedly across forked workflows.
- Engineering lead: Involve when an item has been blocked for more than three business days with no actionable unblock path, or when a dependency chain crosses team boundaries.
- Release manager: Involve when blocked or carried work threatens a release commitment tracked through a milestone or `Release` field.

## Related Guides

- GHE-ALM-019 : How to Use Issue Dependencies for Blocked Work
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-032 : How to Close a Sprint Review
