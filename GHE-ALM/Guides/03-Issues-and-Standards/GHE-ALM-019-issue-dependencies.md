# How to Use Issue Dependencies for Blocked Work

**Guide ID:** GHE-ALM-019
**Audience:** Project Manager, Engineering Manager, Scrum Master
**Primary role:** Project Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 5-10 minutes per use
**Required permissions:** Repository: Triage (to add dependencies on issues you do not own); Project: Read (to inspect blocked work in a Project view)
**Prerequisites:**

- The blocking and blocked issues already exist as GitHub issues.
- You know the issue numbers or titles you plan to link.
- The issues are in repositories you can access.

**When to use this guide:** Use when one issue cannot start or finish until another issue is delivered, and you need that constraint to be visible to planners, the assigned engineer, and reviewers without relying on a comment or label.

**When not to use this guide:** Do not use issue dependencies to model parent/child decomposition (an epic broken into tasks). That is a sub-issue relationship, covered in GHE-ALM-017. Dependencies express sequence between peers, not hierarchy.

## Outcome

By the end of this guide, you will have produced:

- An explicit `Blocked by` or `Blocks` link between two issues, visible in the **Relationships** section of each issue.
- A blocked indicator that surfaces on the issue, the repository Issues list, and any Project view that contains the issue.
- A repeatable inspection step you can run during sprint planning, standup, or sprint review to find blocked work and decide when to escalate.

## Before You Start

- Confirm the issue you want to mark as blocked actually depends on the other issue's outcome, not just shares context with it. A dependency is a sequencing constraint: issue B cannot start (or cannot finish) until issue A is done.
- Decide whether the relationship is `Blocked by` (this issue waits on another) or `Blocks` (this issue is the gate for another). Pick the direction that reads naturally on the issue you are editing. The link appears bidirectionally on both issues regardless of which side you create it from.
- For cross-repository dependencies, confirm the other repository is in the same organization or accessible to you. Use the `owner/repo#NNN` form when searching.

## Steps

### Mark an issue as blocked or blocking (Performs)

1. Open the issue that should carry the dependency. From the repository, click **Issues**, then click the issue title.
2. In the right sidebar, locate the **Relationships** section.
3. To record a dependency on another issue, click **Mark as blocked by**. To record that this issue is the gate for another, click **Mark as blocking**.
4. In the dialog, search by issue title or paste the reference in `owner/repo#NNN` form. Select one or more issues. Confirm the selection to add the link.
5. Verify the linked issue now appears under **Relationships** with the correct label (`Blocked by` or `Blocks`).

> [SCREENSHOT: Issue right sidebar showing the Relationships section with Mark as blocked by and Mark as blocking actions]

### Confirm the link is bidirectional

6. Open the other issue in the dependency. Confirm its **Relationships** section now shows the inverse link automatically. If you marked issue B as blocked by issue A, issue A should now show that it blocks issue B.
7. If the inverse link does not appear, refresh the issue. If it still does not appear, the original add did not save; repeat step 3.

### Remove or change a dependency (Performs)

8. On either issue in the relationship, open the **Relationships** section.
9. Use **Change blocked by** or **Change blocking** depending on the link type. Deselect the issue you no longer want linked, and confirm.
10. Verify the link is gone from both issues. Removing on one side removes on both.

> [SCREENSHOT: Change blocked by dialog with one linked issue selected for removal]

### Inspect blocked work in a Project view (Reviews)

11. Open the GitHub Project that holds the work, for example the `acme-payments` ALM Project. Switch to the table or board view used for sprint execution (covered in GHE-ALM-029).
12. Look for the blocked indicator on issue rows or cards. GitHub renders a `Blocked` icon on items whose `Blocked by` dependencies are still open. The icon also appears on the repository Issues list.
13. Click an issue showing the indicator. Read its **Relationships** section to see exactly which open issues are blocking it. Note the assignee and `Status` of the blocker.
14. Compare what you see against the table below to decide whether to leave the work in place, move it, or escalate.

> [SCREENSHOT: Project board card or table row showing the Blocked icon next to an issue title]

## What Good Looks Like vs. What to Escalate

| Signal | What good looks like | What to escalate |
|---|---|---|
| Blocker age | Blocker was opened or last updated within the last few days; assignee is active. | Blocker is open with no activity for a full sprint or longer. |
| Blocker ownership | Blocker has an assignee, `Owner`, and a `Sprint` or `Target Date`. | Blocker has no assignee, no sprint, and no target date. |
| Blocker repository | Blocker lives in the same product area or a known partner repository. | Blocker is in a third-party or unfamiliar repository, with no visible owner from your team or partner team. |
| Direction of pain | One issue is blocked by one open issue. | A single issue is blocked by three or more open issues, or a chain of dependencies extends across more than two issues. |
| Status alignment | Blocked issue is parked in a `Blocked` or backlog `Status`, not committed for this sprint. | Blocked issue is sitting in `In Progress` or `Ready for QA` while a `Blocked by` dependency is still open. |
| Sprint commitment | Blocker is also in the current sprint and on track. | Blocker is in a future sprint or has no sprint, while the blocked issue is committed to the current sprint. |

If you see escalation signals during sprint planning, do not commit the blocked issue. If you see them mid-sprint, work with the Scrum Master to move the blocked issue using GHE-ALM-030 and raise the blocker to the responsible engineering lead.

## Validation Checklist

- [ ] Both linked issues show the matching relationship in their **Relationships** section (`Blocked by` on one, `Blocks` on the other).
- [ ] A blocked indicator appears on the blocked issue in the repository Issues list and in the Project view.
- [ ] Each open blocker has an assignee and a `Status` value other than empty.
- [ ] No issue committed to the current sprint is blocked by an issue that has no sprint and no target date.
- [ ] Removed dependencies are gone from both issues, not just one.

## Common Mistakes

- Using a dependency to model decomposition. A feature broken into tasks is a sub-issue relationship (GHE-ALM-017), not a `Blocks` link. Dependencies are sequence between peers; sub-issues are hierarchy between parent and child.
- Leaving a `Blocked` `Status` on the issue after the blocker is closed. Open the blocked issue, confirm the blocker is closed, and move `Status` back to `Ready` or `In Progress` so the board reflects reality.
- Treating a label such as `blocked` as a substitute for the dependency link. Labels do not carry the link; reviewers cannot click through to see what is blocking the work. Use the dependency, then optionally add a label for filtering.
- Adding a dependency on a closed issue. The relationship is allowed, but it produces no useful blocked indicator. Close the loop by removing the link or replacing it with the open issue that actually gates the work.
- Forgetting that the link is bidirectional. You only need to add it on one side. Adding it on both sides does not create a stronger link; it creates duplicate edits.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve when a dependency you need to add lives in a repository you cannot access; request `Triage` access.
- Engineering lead: Involve when an open blocker has no assignee, has been stale for a sprint, or sits across an organizational boundary.
- Release manager: Involve when blocked work is committed to the current release and the blocker is not on track to close before code freeze.

## Related Guides

- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-031 : How to Monitor Blocked Sprint Work
