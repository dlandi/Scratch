# How to Close a Sprint Review

**Guide ID:** GHE-ALM-032
**Audience:** Scrum Master, Engineering Manager, Project Manager
**Primary role:** Scrum Master
**Classification:** Manager Performs
**Estimated time:** 45-60 minutes per sprint
**Required permissions:** Repository: Triage; Project: Write
**Prerequisites:**

- A current sprint exists in the project `Sprint` field with items assigned.
- Items have `Status`, `Owner`, and (where used) `Effort` populated.
- The next sprint iteration exists so unfinished work can be rolled forward.

**When to use this guide:** Use this guide on the last day of the sprint, immediately before or after the team's Sprint Review and Sprint Retrospective meetings, to classify every committed item, capture sprint metrics, and record retro outcomes.

**When not to use this guide:** Do not use this guide for mid-sprint check-ins (use GHE-ALM-029) or for triaging individual blocked items in flight (use GHE-ALM-031).

## Outcome

By the end of this guide, you will have produced:

- A current-sprint board where every item has a final, accurate `Status`.
- A documented count of completed, completed-late, carried-forward, returned-to-backlog, and not-needed items.
- An Insights snapshot of the sprint's burn-up and completion distribution.
- A retro note recorded against the sprint, capturing what to keep and what to change.

## Before You Start

- Confirm the sprint end date and the start of the next iteration. The `sprint:@previous` filter only resolves to this sprint after the next iteration begins.
- Have the team's working agreement on hand. You need it to apply consistent rules for "completed" versus "completed late."
- Open the `Effort` field convention if your team estimates (see GHE-ALM-033). Sprint metrics by points only work when estimates were entered before sprint start.
- Identify where retro notes live. A pinned discussion, a recurring issue, or a project README block all work; pick one and stay consistent across sprints.

## Steps

### Open the Current Sprint board and freeze the picture

1. Open the organization or repository project that holds the sprint.
2. Open the **Current Sprint** board view, which is grouped by `Status`. If you do not have one, see GHE-ALM-029.
3. Apply the filter `sprint:@current` to confirm you are looking at the closing sprint and nothing else.
4. Take a snapshot of the board before any classification work, so the team can compare against the start-of-sprint plan during the review meeting.

> [SCREENSHOT: Current Sprint board grouped by Status, filtered to sprint:@current, showing all columns at end of sprint]

### Classify every committed item

5. Walk the board column by column. For each item, set the final `Status` value using the categories below. Do not leave anything in an in-flight column at sprint close.
6. Use these five outcomes:
   - **Completed in sprint.** Work meets the team's Definition of Done and was finished within the sprint window. Set `Status` to `Done`.
   - **Completed but late.** Work meets Definition of Done but slipped past the sprint end date. Set `Status` to `Done` and add the label `late-completion` (or your team's equivalent) so the metric is recoverable later.
   - **Not completed, carried forward.** Work is partially done and will continue. Move it to the next iteration using GHE-ALM-030. Leave `Status` in its current in-flight column so the next sprint inherits accurate state.
   - **Not completed, returned to backlog.** Work is no longer urgent or has lost its owner. Clear the `Sprint` field and set `Status` to `Backlog`.
   - **Not completed, closed as not needed.** Work is obsolete, duplicated, or descoped. Close the issue with reason **Not planned** and add a comment explaining the decision.
7. For every item that did not complete, add a one-line comment on the issue stating why. Future sprint planning depends on this signal.

> [SCREENSHOT: Issue close dialog with "Not planned" reason selected and comment explaining descope]

### Capture sprint metrics from Insights

8. Open the project's **Insights** tab.
9. Open or create a **Sprint Burn-up** historical chart filtered to `sprint:@current`. Confirm the completed line meets the scope line, or note the gap. See GHE-ALM-051 for chart configuration.
10. Open or create a **Sprint Outcome** distribution chart with X-axis `Status` and filter `sprint:@current`. Record the counts:
    - Completed (including late).
    - Carried forward.
    - Returned to backlog.
    - Closed as not needed.
11. If your team uses estimates, also record completed `Effort` versus committed `Effort`. This is your delivered velocity for the sprint.
12. Export the chart image or copy the numbers into the sprint's retro note. Insights data is live and will shift as items move; the snapshot is your record.

> [SCREENSHOT: Project Insights view with Burn-up chart on left and Sprint Outcome distribution on right, filtered to sprint:@current]

### Record review and retro outcomes

13. Open your team's retro location (pinned discussion, recurring issue, or project README block).
14. Add a dated entry containing:
    - Sprint name and dates.
    - Committed item count and committed effort.
    - Completed item count and completed effort.
    - Carryover count and carryover reasons (one line each).
    - Top three Keep, Stop, Start items from the retro discussion.
    - Action items with an owner and a target sprint.
15. Link the retro entry from the project description or pinned view so the next sprint planning session (GHE-ALM-028) can find it without searching.

### Switch the board to the next sprint

16. Once the next iteration has started, change the board filter from `sprint:@current` to `sprint:@previous` to confirm the closed sprint still reads correctly. The `@previous` reference is how you audit a closed sprint after the iteration boundary moves.
17. Update any saved view or bookmark that pointed at the just-closed sprint so it now points at the new current sprint.

## Validation Checklist

- [ ] Every item that was in the sprint at start has a final `Status` (Done, Backlog, or closed as Not planned), or has been moved to a later sprint.
- [ ] No items remain in `In Progress`, `In Review`, or `Ready for QA` against the closed sprint.
- [ ] Insights charts filtered to `sprint:@current` (or `sprint:@previous` after the boundary) match the manual counts in the retro note.
- [ ] Carryover items appear in the next sprint with their original `Owner` and parent links intact.
- [ ] Retro note exists, is dated, and is linked from the project so it can be found next sprint.

## Common Mistakes

- Closing items as `Done` when they did not meet Definition of Done. This inflates velocity and hides quality debt. Use `late-completion` labels or carry the item forward instead.
- Bulk-deleting carryover items to "clean up" the closed sprint. The history is what makes Insights useful; carry items forward, do not delete them.
- Running the close before the next iteration starts and then trying to use `sprint:@previous`. The filter only resolves once the new iteration begins. Until then, name the sprint explicitly.
- Recording retro outcomes in chat or meeting notes that are not linked to the project. The next planning session loses the signal.
- Treating carryover as automatically bad. One or two carried items can reflect honest scope. A pattern of large carryover is the signal worth acting on.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable for a normal close. Involve if the `Sprint` iteration field needs structural change (length, naming, breaks) for the next planning cycle.
- Engineering lead: Involve when the same items carry forward two or more sprints in a row, or when a closed sprint shows zero completed items.
- Release manager: Involve when carryover or descoped work affects a tracked release milestone or `Release` field assignment.

## Related Guides

- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-031 : How to Monitor Blocked Sprint Work
- GHE-ALM-033 : How to Use Effort or Story Points in GitHub Projects
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
