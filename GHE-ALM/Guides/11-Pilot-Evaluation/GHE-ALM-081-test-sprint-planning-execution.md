# How to Test Sprint Planning and Execution

**Guide ID:** GHE-ALM-081
**Audience:** Scrum Master, Engineering Manager, Project Manager
**Primary role:** Scrum Master
**Classification:** Manager Performs
**Estimated time:** Two two-week sprints (four calendar weeks); 60 to 90 minutes of management activity per sprint
**Required permissions:** Project: Write; Repository: Triage on the pilot repositories
**Prerequisites:**

- Pilot Project exists with a `Sprint` iteration field configured for two-week iterations.
- Pilot team has been identified and has access to the Project.
- Backlog of feature, requirement, task, and bug issues is loaded into the Project with `Priority`, `Effort`, and `Owner` populated for the candidate items.
- The pilot scenario from GHE-ALM-079 is active and you are running scenario 2.

**When to use this guide:** Run this guide during the GitHub Enterprise ALM pilot when you need to validate that GitHub Projects can support a standard two-week Scrum cadence end to end. It is the sprint scenario for the pilot scorecard and feeds GHE-ALM-085.

**When not to use this guide:** Do not use this guide for routine sprint operations after the pilot has concluded; use GHE-ALM-028, GHE-ALM-029, GHE-ALM-030, and GHE-ALM-032 for ongoing sprint work.

## Outcome

By the end of this guide, you will have produced:

- Two completed two-week sprints recorded in the pilot Project, each with a planned scope, an executed board, and a closed review.
- A pass/fail record for each of the five acceptance criteria (sprint iteration exists, work assigned to sprint, board shows status flow, blocked work visible, unfinished work moved forward).
- Screenshot evidence and short notes for the pilot scorecard captured under GHE-ALM-085.

## Before You Start

- Confirm the `Sprint` iteration field shows at least three named iterations (current and next two). If only the default iterations exist, extend them so two consecutive sprints are bookable.
- Confirm the Project has a `Status` field with at least `Todo`, `In Progress`, `Blocked`, `In Review`, and `Done` options.
- Identify 8 to 15 candidate issues per sprint with owners and effort already set. Mixing one or two bugs into the candidate list is realistic.
- Open a working document or the pilot scorecard so you can paste evidence as you go.

## Steps

### Set up the sprint scenario

1. Open the pilot Project. Confirm the iteration field is named **Sprint** and the current iteration starts on the planned sprint start date. If the start date is wrong, open **Settings**, edit the **Sprint** field, and adjust the start date and duration to two weeks.
2. Create a saved view named **Pilot Sprint Planning** as a table layout, filtered by `sprint:@next` and `no:Sprint`, grouped by `Priority`, with columns for `Title`, `Owner`, `Effort`, `Status`, and `Sprint`.
3. Create a second saved view named **Pilot Sprint Board** as a board layout, grouped by `Status`, filtered by `sprint:@current`. Add a swimlane or extra filter for `Owner` if your team prefers per-person lanes.
4. Record evidence for criterion 1 ("Sprint iteration exists") by capturing the iteration field configuration screen and the two saved views.

> [SCREENSHOT: Sprint iteration field settings showing two-week duration and three named iterations]

### Plan and run sprint 1

5. Open the **Pilot Sprint Planning** view. Sort the unscheduled items by `Priority`, then `Effort`. Choose 8 to 12 items that fit the team's expected capacity for sprint 1.
6. For each chosen item, set **Sprint** to the next iteration and confirm `Owner`, `Effort`, and acceptance criteria are populated. Use bulk edit to set the iteration in one operation when possible.
7. Record evidence for criterion 2 ("Work is assigned to sprint") by capturing the planned sprint scope filtered by `sprint:@next` before the sprint starts.
8. On the sprint start date, switch to the **Pilot Sprint Board** view. Walk the team through the board at the first standup. Move items from `Todo` to `In Progress` as work begins.
9. During the sprint, exercise the `Blocked` status at least once: pick one in-flight item, set **Status** to `Blocked`, add a comment explaining the blocker, and link any blocking issue using the issue's **Dependencies** section. Capture this state for criterion 4 ("Blocked work is visible").
10. Move items through `In Review` and `Done` as pull requests merge and acceptance criteria are met. Capture a mid-sprint and end-of-sprint board screenshot for criterion 3 ("Sprint board shows status flow").

> [SCREENSHOT: Sprint board grouped by Status showing items distributed across Todo, In Progress, Blocked, In Review, and Done]

### Close sprint 1 and roll forward

11. On the sprint end date, open the **Pilot Sprint Board** and review every item. For each not-`Done` item, decide whether it moves to the next sprint, returns to the backlog, or is closed as not-needed.
12. For items moving forward, change the **Sprint** value from the closing iteration to the next iteration. For items returning to the backlog, clear the **Sprint** value. Confirm the `Status` reflects reality (still `In Progress`, still `Blocked`, etc.).
13. Record evidence for criterion 5 ("Unfinished work can be moved forward") by capturing the board before and after the rollover, plus a filter showing the moved items now appear under `sprint:@next`.
14. Hold a short sprint review and retrospective using the closed sprint board. Note anything the GitHub Project did not support cleanly; this feeds the pilot scorecard.

### Plan and run sprint 2

15. Repeat steps 5 through 11 for sprint 2. The work moved forward from sprint 1 is already on the new iteration; add fresh items from the backlog to fill remaining capacity.
16. During sprint 2, deliberately exercise at least one mid-sprint scope change: add one new urgent item to the iteration after start, or remove one item that is no longer needed. Capture before-and-after evidence.
17. At sprint 2 close, repeat steps 11 through 13 for the second rollover. The pilot now has two consecutive closed sprints with continuity.

### Record results

18. For each of the five acceptance criteria, write a short pass/fail line in the pilot scorecard with links to the saved views and screenshots: criterion 1 (iteration exists), criterion 2 (work assigned), criterion 3 (status flow), criterion 4 (blocked work visible), criterion 5 (unfinished work moved forward).
19. Note any friction observed: for example, bulk iteration edits being slow, blocked items being easy to miss without a saved filter, or effort totals being hard to read at a glance. These notes feed the adoption decision in GHE-ALM-085.

## Validation Checklist

- [ ] Two two-week iterations have started and closed inside the pilot window.
- [ ] At least 8 items per sprint were assigned to the iteration before the sprint started.
- [ ] The sprint board shows items moving through `Todo`, `In Progress`, `Blocked`, `In Review`, and `Done` over the sprint.
- [ ] At least one item was set to `Blocked` and is visible on the board and in a `status:Blocked` filter.
- [ ] Unfinished items from sprint 1 appear on sprint 2 with their state preserved.
- [ ] Each of the five acceptance criteria has a recorded pass or fail with linked evidence.

## Common Mistakes

- Treating `Status: Blocked` as a synonym for `In Progress`. Blocked items must be filterable; if they are mixed in with active work, criterion 4 fails.
- Forgetting to clear the `Sprint` field on items returned to the backlog. They will appear in `sprint:@next` and inflate the next sprint's planned scope.
- Planning all candidate items into sprint 1 without leaving capacity for the rollover from sprint 0 or for mid-sprint additions. The pilot is about validating the workflow, not maximizing throughput.
- Skipping the screenshot capture during the sprint and trying to reconstruct evidence afterward. Iteration history is preserved but board state at a point in time is not easily reproducible.
- Renaming the iteration field mid-pilot. Rename invalidates the saved filters using `sprint:@current` and `sprint:@next` and forces evidence to be recaptured.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable for sprint operations; involve only if Project access changes are needed.
- Engineering lead: Involve if effort estimates or sprint scope require negotiation with the engineering team.
- Release manager: Involve if sprint scope intersects a release window covered by GHE-ALM-083.

## Related Guides

- GHE-ALM-027 : How to Configure or Request a Sprint Iteration Field
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-085 : How to Record Pilot Pass/Fail Evidence
