# How to Add Issues and Pull Requests to a Milestone

**Guide ID:** GHE-ALM-043
**Audience:** Release Manager, Engineering Manager, Project Manager
**Primary role:** Release Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 10-15 minutes per use
**Required permissions:** Repository: Triage (to assign milestones); Repository: Read (to inspect)
**Prerequisites:**

- The target milestone already exists in the repository (see GHE-ALM-042).
- The release scope is defined: you know which issues and pull requests belong to this milestone.
- You can identify the correct milestone name and due date for the release.

**When to use this guide:** Use this guide when you need to bind specific issues and pull requests to a repository milestone so they appear in release scope, milestone progress charts, and release readiness reviews.

**When not to use this guide:** Do not use this guide for cross-repository release coordination; use the Project `Release` field instead (see GHE-ALM-049). Do not use it to create the milestone itself; create the milestone first using GHE-ALM-042.

## Outcome

By the end of this guide, you will have produced:

- Every in-scope issue and pull request linked to the correct milestone.
- A milestone page that reflects accurate open vs. closed counts and a meaningful completion percentage.
- A documented review pass that confirms no orphan issues remain in the release scope and no closed-but-unverified items are hiding in the milestone.

## Before You Start

- Confirm the milestone name and spelling. Milestones are repository-scoped, so the same release name may exist in multiple repositories with slight variations.
- Confirm you have Triage permission or higher on the repository. Read access cannot assign milestones.
- Have a working list of candidate issues and pull requests, ideally filtered by `Release` field, label, or saved search.

## Steps

### Assign a milestone to a single issue or pull request

1. Open the issue or pull request in the repository.
2. In the right sidebar, locate the **Milestone** section.
3. Click the gear icon next to **Milestone**.
4. In the picker, type part of the milestone name in the filter, then click the milestone to assign it. The sidebar updates immediately.
5. To remove a milestone, open the same picker and click **Clear this milestone**.

> [SCREENSHOT: issue sidebar with the Milestone section highlighted and the picker open showing a filtered milestone list]

### Bulk-assign milestones from the issues or pull requests list

6. Navigate to the repository, then click the **Issues** tab or the **Pull requests** tab.
7. Filter the list to the candidates you want to add. Useful filters include `is:open no:milestone label:release-2026.05.0`, `is:open assignee:@me`, or a saved search agreed with the team.
8. Select the checkbox to the left of each item to add. To select an entire visible page, use the checkbox in the list header.
9. Above the list, open the **Milestone** dropdown.
10. In the **Filter milestones** field, type the milestone name, then click the milestone to apply it to every selected item. The list refreshes and the items now show the milestone badge.

> [SCREENSHOT: issues list with three checkboxes selected and the Milestone dropdown open over the list, filter field showing "2026.05"]

### Add items from the milestone page itself

11. From the repository, click **Issues**, then click **Milestones**, then click the milestone name to open its page.
12. Review the open and closed counts and the completion percentage at the top of the page.
13. To add more items, return to the **Issues** or **Pull requests** tab and use the bulk-assign flow above. The milestone page does not have a built-in "add by search" picker, so the bulk flow is the canonical way to grow scope.
14. While on the milestone page, drag-and-drop items in the open list to set priority order. Prioritization is unavailable when the milestone contains more than 500 open issues; if you reach that point, split the release.

### Review pass: spot orphans and unverified closures

15. Run an issues search for in-scope work without a milestone, for example `is:issue is:open label:release-2026.05.0 no:milestone` (substitute your release label or `Release` field equivalent). Every result is an orphan that needs a milestone or an explicit deferral.
16. On the milestone page, scan the **Closed** tab for issues that closed without a linked pull request or without a Verified status. Cross-check against GHE-ALM-060 to confirm each closed issue has implementation traceability.
17. Confirm the milestone description states the release version, due date, and scope boundary so reviewers can interpret progress without context from chat.

> [SCREENSHOT: milestone page showing percent complete bar, open/closed tabs, and the description block]

## Validation Checklist

- [ ] Every issue and pull request in the agreed release scope shows the milestone badge in its sidebar.
- [ ] The milestone page open count matches the count of issues filtered by `milestone:"<name>" is:open` from the Issues tab.
- [ ] No issue with the release label or `Release` field value is missing a milestone (the `no:milestone` search returns zero results within scope).
- [ ] Closed issues on the milestone page either link to a merged PR or carry a recorded verification note.
- [ ] The milestone completion percentage moves when items close, indicating the link is real and not stale cache.

## Common Mistakes

- Assigning the milestone only to issues and forgetting the pull requests that close them. Both should carry the milestone so generated release notes and merge metrics line up.
- Using a label like `release-2026.05.0` as a substitute for the milestone. Labels do not contribute to milestone progress percentage and do not appear in milestone-scoped filters.
- Selecting all items on a filtered page and applying the wrong milestone because the filter cleared between page loads. Always re-check the filter chip before bulk-applying.
- Adding issues to a closed milestone. Closed milestones still accept assignments, but the items will look like they shipped in a past release. Reopen the milestone or pick the correct active one.
- Treating the milestone as the cross-repository release scope. Milestones are repo-scoped; for multi-repo trains, pair milestones with the Project `Release` field.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Engage when you lack Triage permission or when a needed milestone has been deleted and must be recreated with history preserved.
- Engineering lead: Engage when an orphan issue surfaces in the review pass and ownership of scope (in or out of release) is disputed.
- Release manager: Owns the final scope decision. Escalate any milestone with closed-but-unverified items so QA verification can be scheduled before the release readiness review.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-042 : How to Create and Manage a Milestone
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-049 : How to Track a Cross-Repository Release
- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability

## What Good Looks Like vs. What to Escalate

| Signal | What Good Looks Like | What to Escalate |
|---|---|---|
| Milestone scope coverage | `no:milestone` search inside the release label or `Release` field returns zero results. | Orphan issues persist after a triage cycle; release scope is ambiguous. |
| Open vs. closed counts | Open count trends down each week; closed count trends up; completion percentage moves accordingly. | Counts are static for more than two sprints; closed count grows but percentage does not. |
| Pull request linkage | Each closed issue on the milestone page links to a merged PR also on the milestone. | Closed issues have no linked PR or the PR is on a different milestone. |
| Verification status | Closed issues carry a verification note, label, or QA sign-off. | Closed issues skipped QA, or status went straight from In Progress to Done without Ready for QA. |
| Description and due date | Milestone description names the release version and due date; the due date is realistic. | Description is empty, the due date is in the past, or both. |
| Repository alignment | All repositories shipping in the release have a milestone with the same naming convention (e.g., `2026.05.0`). | Naming drifts across repositories, breaking cross-repo rollups in the org Project. |

### Worked example

For the `acme-payments` org shipping release `2026.05.0`, the release manager opens the `checkout-service` repository, runs `is:issue is:open label:release-2026.05.0 no:milestone`, finds four orphans, bulk-selects them on the Issues tab, opens the **Milestone** dropdown, types `2026.05`, and clicks `2026.05.0`. The milestone page percent-complete bar updates from 62 percent to 58 percent (denominator grew). The release manager then opens the milestone Closed tab, finds one issue closed three days ago without a linked PR, and escalates to the engineering lead to confirm whether the work shipped under a different commit or was closed in error.
