# How to Create and Manage a Milestone

**Guide ID:** GHE-ALM-042
**Audience:** Release Manager, Engineering Manager, Project Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes for first milestone; 5 minutes per update thereafter
**Required permissions:** Repository: Triage to create, edit, close, reopen, and delete milestones; Repository: Write to assign issues and pull requests to a milestone
**Prerequisites:**

- A target release identified by name and approximate date.
- Agreement with engineering on which repository owns the release scope.
- Naming convention selected for milestones (see GHE-ALM-077).

**When to use this guide:** Use when a single repository owns the deliverable and you need a versioned scope container for issues and pull requests with a due date and progress bar. Milestones are the right tool for repository-scoped releases such as `v1.4` of `checkout-service`.

**When not to use this guide:** Do not use a single milestone to track work that spans multiple repositories. For cross-repository release trains, use a Project `Release` field as the primary tracker and one milestone per repository for the local slice. See GHE-ALM-041 for the combined pattern.

## Outcome

By the end of this guide, you will have produced:

- A repository milestone with a title, description, and due date.
- Initial issues and pull requests assigned to the milestone.
- A monitoring routine that uses the milestone progress bar to track completion.
- A clean close (or documented re-plan) when the release ships.

## Before You Start

- Confirm the repository where the milestone will live, for example `acme-checkout/checkout-service`.
- Decide the milestone name. Use one of the standard patterns: `vMajor.Minor` (`v1.0`, `v1.4`) for product versions, or `YYYY-QN Release` (`2026-Q3 Release`) for time-boxed release trains.
- Confirm the due date. Use the planned code-freeze date, not the deployment date, so the progress bar tracks engineering completion rather than rollout.
- Verify you hold at least Repository: Triage. Without it, the **New milestone** button will not appear.

## Steps

### Create the milestone

1. Open the repository in GitHub Enterprise, for example `acme-checkout/checkout-service`.
2. Click the **Issues** tab in the repository navigation.
3. Click the **Milestones** button next to the search field.
4. Click **New milestone**.
5. In **Title**, enter the milestone name following the naming convention, for example `2026-Q3 Release` or `v1.4`.
6. In **Due date**, set the planned code-freeze date.
7. In **Description**, record the release theme, scope summary, code-freeze date, target deployment date, and a link to the parent Project or release tracker. Keep it short. The description is the audit record for what this milestone was supposed to deliver.
8. Click **Create milestone**.

> [SCREENSHOT: New milestone form with Title, Due date, and Description filled in for `2026-Q3 Release`]

### Add issues and pull requests

9. From any issue or pull request in the same repository, open the right sidebar and click **Milestone**, then select the milestone you just created. Repeat for each in-scope item.
10. For bulk assignment, open the **Issues** tab, filter the list to the items you want to add, select them with the checkboxes, then click the **Milestone** dropdown above the list and choose the milestone.
11. For detailed scoping rules and the difference between `Target Release` and milestone assignment, follow GHE-ALM-043.

> [SCREENSHOT: Bulk milestone assignment from the Issues list with several issues selected]

### Monitor completion

12. Return to **Issues** then **Milestones**. Each milestone row shows the completion percentage, the count of open and closed items, and the due date.
13. Click the milestone title to open its dedicated page. Review open versus closed items, identify items without an assignee, and spot pull requests that are still in draft.
14. Run this review at least weekly for active releases, and daily in the final week before code freeze. Pair it with the release health review described in GHE-ALM-045.
15. If the progress bar is not moving in the final two weeks, escalate to the engineering lead before extending the due date. A stalled bar usually means scope is too large, not that the team needs more time.

> [SCREENSHOT: Milestone detail page showing the progress bar, open and closed counts, and the due date]

### Edit, close, reopen, or delete

16. To change the title, description, or due date, open **Issues** then **Milestones**, click **Edit** next to the milestone, make changes, then click **Save changes**. Record the reason for any due-date change in the description so the audit trail is preserved.
17. When the release is shipped and verified, open the milestone and click **Close milestone**. The completion percentage freezes at its final value and the milestone moves to the **Closed** tab.
18. If the release missed scope, close the milestone anyway when the release ships. Move the unfinished items to the next milestone first; do not leave open items attached to a closed milestone. The combined steps live in GHE-ALM-050.
19. To reopen a milestone (rare; usually only when a release was closed prematurely), open the **Closed** tab on the Milestones page, locate the milestone, and click **Reopen milestone**.
20. Avoid deleting milestones. A deleted milestone removes the historical scope record. Delete only when the milestone was created in error and contains no closed items. From the Milestones page, click **Delete** next to the milestone, then confirm.

## Validation Checklist

- [ ] The milestone appears under **Issues** then **Milestones** with the agreed title, due date, and description.
- [ ] At least one issue or pull request is assigned to the milestone, and the open/closed counts on the milestone row are non-zero.
- [ ] The naming pattern matches the team standard (`vMajor.Minor` or `YYYY-QN Release`).
- [ ] The description includes the code-freeze date, target deployment date, and a link to the parent Project or release record.
- [ ] When closed, no open items remain attached. Unfinished work has been reassigned to the next milestone.

## Common Mistakes

- Using a single milestone to track work across multiple repositories. Milestones are repository-scoped; cross-repo coordination requires a Project `Release` field.
- Setting the due date to the deployment date rather than the code-freeze date. The progress bar then implies engineering has more time than it does.
- Editing the milestone title mid-release. Downstream filters, saved searches, and release notes that reference the old title will silently break.
- Deleting a missed-scope milestone instead of closing it. Deletion removes the historical record of what was promised; closing preserves the audit trail.
- Leaving open issues attached to a closed milestone. The closed milestone will appear at less than 100 percent completion forever and will distort historical reporting.
- Treating the milestone as a status field. Status, sprint, owner, and priority all belong on the issue itself or in the Project, not in the milestone description.

## Escalation Path

- GitHub administrator: Not applicable. Milestone management does not require organization-level intervention.
- Repository administrator: Involve when you lack Triage permission, when a milestone needs to be force-deleted with attached items, or when you need to merge milestones that were created in parallel.
- Engineering lead: Involve when the progress bar stalls in the final two weeks, when scope changes mid-release require a re-baselined due date, or when items cannot be closed because of unresolved technical blockers.
- Release manager: Involve when a missed milestone affects a downstream release train, when the close decision (close-as-shipped versus close-as-missed) is contested, or when reopening a closed milestone is being considered.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-043 : How to Add Issues and Pull Requests to a Milestone
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-050 : How to Close a Release After Deployment
- GHE-ALM-077 : How to Enforce Naming Conventions
