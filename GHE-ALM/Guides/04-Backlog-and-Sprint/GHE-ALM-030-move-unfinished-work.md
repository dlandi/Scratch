# How to Move Unfinished Work to a Later Sprint

**Guide ID:** GHE-ALM-030
**Audience:** Scrum Master, Engineering Manager, Project Manager
**Primary role:** Scrum Master
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes at sprint close; 5 minutes for an ad-hoc mid-sprint scope cut
**Required permissions:** Project: Write to update the **Sprint** field on items; Repository: Triage on the underlying repositories to close issues that will not be carried forward
**Prerequisites:**

- An organization-level GitHub Project exists for the team (see GHE-ALM-006).
- A **Sprint** iteration field is configured on the Project, with the current iteration and at least one future iteration defined (see GHE-ALM-027).
- A **Status** field exists on the Project with at least the values: Backlog, Ready, In Progress, In Review, Blocked, Done.
- The current sprint board is in active use (see GHE-ALM-029).

**When to use this guide:** Use this guide at sprint close to roll incomplete work forward with intent, and mid-sprint when scope must be reduced because of an outage, a re-prioritization, or a major estimate miss.

**When not to use this guide:** Do not use this guide to move every open item forward without classifying it. Silent rollover hides scope problems and undermines planning data. Do not use this guide for release-level scope changes; update the **Release** field and milestones via GHE-ALM-041 instead.

## Outcome

By the end of this guide, you will have produced:

- A reviewed list of every unfinished item in the closing sprint.
- An explicit per-item disposition: moved to the next sprint, returned to the backlog, deferred to a later named sprint, or closed as not needed.
- The **Sprint** field updated on every unfinished item so the next sprint planning view (see GHE-ALM-028) starts from a clean state.

## Before You Start

- Confirm the closing sprint name (for example `Sprint 27`) and the next sprint name (for example `Sprint 28`).
- Confirm the iteration field name on the Project; default is **Sprint**, some teams use **Iteration**.
- Have the sprint review notes or the standup notes available so you know which items were intentionally dropped, which were blocked, and which were genuinely just not started.
- Decide ahead of time who has authority to close items as not needed. The Product Owner usually owns this call.

## Steps

### Identify the unfinished work

1. Open the Project and click the table view used for sprint execution. If no table view is scoped to the active sprint, use the **Current Sprint** view (see GHE-ALM-029) and switch its layout to **Table** for this exercise.
2. Click the **Filter** bar at the top of the view and enter:

   `sprint:@current is:open`

   This returns every Project item still assigned to the active iteration that has not been closed. Closed items already counted toward sprint completion and do not need to move.
3. Add `-status:Done` if your team marks items **Done** in the Project before closing the underlying issue, so cards awaiting closure do not appear on the rollover list.
4. Click the column header on **Status** and choose **Group by Status**. The unfinished items now group into In Progress, In Review, Blocked, Ready, and any other in-flight values, which makes classification faster.

> [SCREENSHOT: Project table view filtered to `sprint:@current is:open` and grouped by Status, showing several unfinished items across In Progress, In Review, and Blocked]

### Classify each item

5. Walk the grouped list with the team during the sprint review or with the Product Owner immediately after. For each item, agree on one of four dispositions:

   - **Move to next sprint:** Work is genuinely in flight and the team will finish it in the next iteration. Assign to the next sprint.
   - **Return to the backlog:** Work was started or queued but is not the right next thing. Clear the **Sprint** value so the item returns to the unscheduled backlog and competes for capacity in future planning.
   - **Defer to a later named sprint:** Work depends on something landing in two or three sprints. Assign to the specific future iteration so the dependency is recorded.
   - **Close as not needed:** Work has been overtaken, descoped, or duplicates other work. Close the underlying issue with a short comment that explains why.

6. Record the disposition in a column you can sort on. The simplest approach is to use the **Status** field: leave items keeping their current status if they are moving to the next sprint, set status back to **Backlog** for items returning to the backlog, and close items that will not be done.

### Update the Sprint field on a single item

7. To update one item, click anywhere in its **Sprint** cell in the table view. A dropdown opens listing the configured iterations with their date ranges.
8. Select the target iteration: the next iteration for "move forward", a specific later iteration for "defer", or click the **Clear** option (or press **Backspace**) to empty the **Sprint** value and return the item to the backlog.
9. The row updates immediately and the filter `sprint:@current is:open` removes the item from the view, confirming the change.

### Bulk-update the Sprint field on multiple items

10. To update many items at once, click the leftmost cell of the first row in the group, then hold **Shift** and click the leftmost cell of the last row to select a contiguous range. Use **Ctrl+Click** (or **Cmd+Click** on macOS) to add or remove individual rows from the selection.
11. Right-click anywhere in the highlighted selection. A context menu appears with field-edit options.
12. Choose **Edit Sprint** (the menu item names the iteration field as it is configured on your Project, so it may read **Edit Iteration**). A picker opens listing the configured iterations.
13. Select the target iteration. Every selected item updates to that iteration in one operation. To return a selected range to the backlog, choose the **Clear** option in the same picker so the **Sprint** value is emptied on all selected items.

> [SCREENSHOT: Multi-row selection in the Project table with the right-click context menu open and "Edit Sprint" highlighted]

### Handle the special cases

14. For **blocked** items, do not blindly roll them forward to the next sprint without addressing the block. Either resolve the dependency as part of planning the next sprint (see GHE-ALM-031), defer the item to a later iteration where the dependency will be in place, or return it to the backlog with a comment naming the blocker.
15. For items that **partially completed** (some sub-issues done, some not), close the parent only when every sub-issue is closed. If the parent must stay open, move the parent and the open sub-issues forward together; closed sub-issues stay closed and remain countable toward the sprint that finished them.
16. For items being **closed as not needed**, open each issue, add a one-line comment naming the reason ("descoped at sprint 27 review, requirement withdrawn"), and close the issue. The closed item drops out of `is:open` filters automatically.

### Confirm the sprint is clean

17. Re-run the filter `sprint:@current is:open` on the table view. The result should be empty if every unfinished item received a disposition. Any rows still showing represent items the team has not yet decided about.
18. Run `sprint:@next is:open` to preview the next sprint. The list should now include the items moved forward in step 13 plus any new work already planned in GHE-ALM-028.

> [SCREENSHOT: Empty result for `sprint:@current is:open` after rollover, with a small banner or message indicating no items match]

## Validation Checklist

- [ ] The filter `sprint:@current is:open` returns zero items after the rollover is complete.
- [ ] Every unfinished item from the closing sprint has either a new **Sprint** value, an empty **Sprint** value (returned to backlog), or a closed state (closed as not needed).
- [ ] The filter `sprint:@next is:open` shows the items moved forward in addition to any pre-planned next-sprint work.
- [ ] Items that were blocked at sprint close have either a resolved dependency, a deferred sprint assignment, or a backlog return with the blocker recorded in a comment.
- [ ] Items closed as not needed have a comment recording why they were dropped.

## Common Mistakes

- Selecting every open item and bulk-moving them all to the next sprint without classification. This guarantees the next sprint starts overcommitted and hides the fact that scope was wrong.
- Leaving the **Sprint** field set to the closed iteration on items that are being returned to the backlog. Stale iteration values pollute velocity and burndown reporting.
- Closing the parent of a partially complete epic to "tidy up" before all sub-issues are resolved. Open sub-issues with closed parents are easy to lose.
- Moving a blocked item forward unchanged. The next sprint inherits the block and the same conversation repeats.
- Updating the **Sprint** field but forgetting to also update **Status** back to **Ready** or **Backlog** for items that are no longer actively in progress. The next sprint board then shows them as **In Progress** on day one.

## Escalation Path

- GitHub administrator: Not applicable for routine rollover. Involve only if the iteration field itself is misconfigured (missing future iterations, wrong dates) and you cannot edit it.
- Repository administrator: Not applicable.
- Engineering lead: Involve when an item that the team believed would finish has slipped two sprints in a row. The pattern usually signals an estimate, dependency, or staffing issue that planning alone cannot solve.
- Release manager: Involve when deferred items are tied to a committed release date. A rollover that pushes work past the release window requires a release scope decision (see GHE-ALM-041).

## Related Guides

- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-031 : How to Monitor Blocked Sprint Work
- GHE-ALM-032 : How to Close a Sprint Review
