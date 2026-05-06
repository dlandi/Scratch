# How to Manage Issue Hygiene Before Sprint Commitment

**Guide ID:** GHE-ALM-022
**Audience:** Project Manager, Engineering Manager, Scrum Master
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 30 to 60 minutes per sprint, depending on candidate count
**Required permissions:** Repository: Triage; Project: Write
**Prerequisites:**

- Sprint candidate items already added to the ALM Project.
- Project has a `Sprint` iteration field configured (see GHE-ALM-027).
- Project has `Status`, `Priority`, `Effort`, `Release`, `Product Area`, and `Owner` fields configured.

**When to use this guide:** Run this review after candidate items are tagged for the next sprint and before the sprint planning meeting where the team commits to scope.

**When not to use this guide:** Do not use this guide to triage raw inbound work, to break down epics, or to assign work mid-sprint. Hygiene is a pre-commitment gate, not a triage activity.

## Outcome

By the end of this guide, you will have produced:

- A reviewed list of sprint candidate issues with all required fields populated.
- A short list of issues deferred from the sprint because they could not be made ready.
- A clean planning view that the team can use to confirm sprint commitment.

## Before You Start

- Confirm the sprint identifier you are planning, for example `Sprint 2026.18`.
- Confirm whether the team estimates work. If yes, the `Effort` field is required.
- Confirm whether the work targets a specific release. If yes, the `Release` field is required.
- Confirm the team's escalation path for issues blocked by missing acceptance criteria or owners.

## The Hygiene Checklist

Every sprint candidate must satisfy this checklist before commitment. Treat any unchecked row as a blocker.

| Field | Required | Notes |
|---|---|---|
| Issue type | Always | Feature, Requirement, Task, Bug, Risk, or Change Request. |
| Owner | Always | Single assignee accountable for the work. |
| Priority | Always | Business urgency. Use the team's standard scale. |
| Sprint | Always | Set to the planned sprint, for example `Sprint 2026.18`. |
| Acceptance criteria | Always | Testable bullets in the issue body. Vague descriptions do not qualify. |
| Estimate (`Effort`) | If team estimates | Story points or hour bucket per the team standard. |
| Parent | Where applicable | Sub-issues must roll up to a Feature, Requirement, or Epic. |
| `Product Area` | Always | Used for ownership routing and reporting. |
| `Release` | Where applicable | Required for items targeting a specific release train. |

A common 1-4 / P0-P3 priority scale is illustrative; confirm your team's actual scale with QA leadership.

## Steps

### Build the sprint planning view

1. Open the ALM Project and select the **Sprint Planning** view, or create a new table view named **Sprint Planning** if one does not exist.
2. Apply the filter `sprint:"Sprint 2026.18"` to scope the view to the candidate sprint. Replace the value with your actual sprint name.
3. Add a secondary filter `is:open` to exclude items already closed in error.
4. Show the columns **Title**, **Type**, **Status**, **Owner**, **Priority**, `Effort`, `Product Area`, `Release`, and **Parent**. Hide unrelated columns to reduce noise.
5. Group the view by **Type** so you can spot Tasks without parents and Bugs missing severity.

> [SCREENSHOT: Sprint Planning table view with the sprint filter applied and hygiene columns visible]

### Review each candidate issue

6. Walk the table top to bottom. For each row, open the issue in a new tab.
7. Read the issue body. Confirm the **acceptance criteria** are present, testable, and specific. Replace placeholder text such as "TBD" or "see Slack" with explicit bullets.
8. Check **Type**. If the type is missing or wrong, set it from the issue header. A Task without a type is the most common defect.
9. Check **Owner**. The owner field must contain exactly one assignee. Multiple assignees indicate unclear accountability; pick the accountable person and move others to a comment thread for visibility.
10. Check **Priority**. If empty, set it based on the team's scale. If priority is `P0`, confirm the item belongs in the sprint at all rather than as a hotfix outside the sprint.
11. Check **Sprint**. Confirm the iteration value matches the planning sprint exactly. Items left on a previous sprint are not candidates.
12. Check `Effort`. If the team estimates and the field is empty, ask the assignee or tech lead for an estimate before committing the item. Do not invent an estimate.
13. Check **Parent**. Tasks and sub-issues must roll up to a Feature, Requirement, or Epic. If a Task is orphaned, either link the parent or convert the work to a standalone Requirement.
14. Check `Product Area`. If empty, set it from the repository, the linked epic, or the originating product owner.
15. Check `Release`. If the item must ship in a specific release, set the field to the release name, for example `2026.05.0`. If the item is not release-bound, leave blank.
16. Return to the planning view and confirm the row now shows green for every required column.

> [SCREENSHOT: Single issue page showing acceptance criteria, owner, priority, sprint, effort, parent, product area, and release fields populated]

### Use bulk operations for missing fields

17. Sort the planning view by `Product Area` and select all rows missing the field. Use the bulk-edit panel at the bottom of the table to set the field in one operation.
18. Repeat for any field that is missing on five or more rows. Bulk-set **Sprint**, `Release`, **Priority**, or **Owner** when the answer is the same across the selected rows.
19. Do not bulk-set acceptance criteria, **Type**, or **Parent**. These require per-issue judgment.

> [SCREENSHOT: Bulk edit panel applying Product Area to multiple selected rows]

### Handle issues that cannot be made ready

20. If an issue is missing acceptance criteria and the originator is unavailable, remove the item from the sprint by clearing the **Sprint** field. Add a comment naming the missing input and the person who owes it.
21. If a Task has no parent and the owner cannot identify one within the planning meeting, defer the Task to the next sprint and flag it in the **Backlog Hygiene** view (see GHE-ALM-026).
22. If estimation is blocked, schedule a 15-minute estimation session with the tech lead before sprint start. Items still unestimated at sprint start are not committed.
23. Record deferred items in the planning meeting notes so the team has visibility into why the sprint scope shrank.

## Validation Checklist

- [ ] Sprint planning view shows zero candidate issues with empty **Owner**, **Priority**, **Type**, or **Sprint** fields.
- [ ] Every candidate issue body contains testable acceptance criteria.
- [ ] If the team estimates, every candidate has a non-empty `Effort` value.
- [ ] Every Task in the sprint has a Parent set.
- [ ] Every release-bound issue has a `Release` value.
- [ ] Items that could not be made ready have been removed from the sprint and recorded.

## Common Mistakes

- Treating `no:Sprint` items as candidates. Hygiene applies only to items already pulled into the sprint.
- Bulk-setting `Effort` to a default value to clear the column. Estimates without owner input are unreliable and inflate planning trust.
- Accepting "see linked design doc" as acceptance criteria. The criteria must be in the issue itself so engineers and QA can read them in context.
- Allowing two assignees on an issue. Shared ownership becomes no ownership during execution.
- Setting `Release` on every item by reflex. Items not bound to a specific release should leave the field blank to avoid polluting release reporting.
- Skipping the parent check on Tasks. Orphaned Tasks distort hierarchy reports and capacity views.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: When the required `Effort`, `Release`, or `Product Area` field does not exist on the Project (see GHE-ALM-024).
- Engineering lead: When acceptance criteria require technical judgment, when estimates are missing, or when parent linkage is ambiguous.
- Release manager: When `Release` assignments conflict with the published release scope.

## Related Guides

- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-026 : How to Use the Product Backlog View
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-033 : How to Use Effort or Story Points in GitHub Projects
