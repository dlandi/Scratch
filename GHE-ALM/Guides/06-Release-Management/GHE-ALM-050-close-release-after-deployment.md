# How to Close a Release After Deployment

**Guide ID:** GHE-ALM-050
**Audience:** Release Manager, Engineering Manager, Program Manager
**Primary role:** Release Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 30-45 minutes per release
**Required permissions:** Repository: Triage or Write (to close milestones, edit releases); Project: Write (to update Release field); Environments: read access to deployment history.
**Prerequisites:**

- Production deployment for the release has finished and the deployment job shows success.
- A GitHub Release has been drafted or published per GHE-ALM-047.
- Each contributing repository has a milestone for this release per GHE-ALM-042.
- The organization Project tracks items by `Release` field per GHE-ALM-041.

**When to use this guide:** Use this guide once production deployment for a release is complete, to close out the release record so audit, reporting, and the next release can proceed cleanly.

**When not to use this guide:** Do not use this guide for hotfix-only deployments that share a release tag (see GHE-ALM-040), or before production deployment has finished and been confirmed.

## Outcome

By the end of this guide, you will have produced:

- A GitHub Release marked as published (not draft) with final notes and assets.
- One or more closed milestones representing the released scope.
- A Project Release field state showing the release as complete.
- Deferred work re-targeted to the next release milestone or backlog.
- A deployment history record confirming production succeeded.
- A scheduled retrospective for the release.

## Before You Start

- The release tag and version, for example `2026.05.0`.
- The list of contributing repositories and their milestone names.
- Access to the organization Project that holds the `Release` field.
- The production environment name in each contributing repo, for example `production`.
- The retrospective owner and target date.

## Steps

### Confirm production deployment completed

1. Open each contributing repository. Click the **Actions** tab, then in the left sidebar click the production environment under **Environments**, or open the repository **Deployments** page directly.
2. Confirm the most recent deployment to production for the release tag shows status **Success** and that any **Required reviewers** approval is recorded. If the job is **Waiting**, **Failed**, or **In progress**, stop. Resolve the deployment before continuing.
3. Note the deployment timestamp, the actor who triggered it, and the commit SHA. Capture this for the audit record.

> [SCREENSHOT: production environment deployment history showing the release tag, success status, and approver]

### Publish the GitHub Release

4. In each contributing repository, open **Releases**. Locate the release for this version.
5. If the release is still **Draft**, open it, click **Edit release**, and confirm the **Tag**, **Title**, **Release notes**, and any attached **Assets** are final. Confirm the **Set as the latest release** checkbox is correct, and that **Set as a pre-release** is not selected for a production release.
6. Click **Publish release**. The release moves out of Draft and becomes visible on the repository **Releases** page and in the Atom feed.

> [SCREENSHOT: published release page showing tag, latest-release badge, release notes, and assets]

### Close the milestone in each repository

7. In each contributing repository, open **Issues**, then click **Milestones**. Open the milestone for this release.
8. Confirm the milestone shows 100 percent completion, or that any open issues have been intentionally re-targeted (see step 11). If open issues remain that are not yet re-targeted, do not close the milestone.
9. Click **Close milestone**. The milestone moves to the **Closed** tab and stops appearing in default issue and PR filters.

### Re-target deferred work

10. In the organization Project, open the saved view for this release. Filter by `Release: <this release>` and `Status: Todo, In Progress, Blocked` to find work that did not ship.
11. For each deferred item, decide one of: move to the next release milestone and update the `Release` field accordingly; return to backlog by clearing the `Release` field and the milestone; or close as **Won't do** if the item is no longer needed. Bulk-edit by selecting items in the Project table and using the field menu.
12. Add a comment on each re-targeted issue noting the reason it slipped, for example "Deferred from `2026.05.0` to `2026.06.0`: dependency on payments-api migration not complete." This protects the audit trail.

### Lock the Project Release field state

13. In the organization Project, open the **Release** field settings. If your team uses a status indicator on the release option (for example a `Released` or `Closed` flag), set it now. If the field is single-select and the release option is no longer in scope, mark it as the closed state your team has agreed (commonly renaming to `2026.05.0 (Released)` or moving it to a closed group).
14. Open the Project view used for executive reporting and confirm the release no longer appears in active swimlanes. If it still appears, adjust the filter to exclude closed releases.

> [SCREENSHOT: organization Project view showing the released release filtered out of active work and visible only under closed releases]

### Schedule the retrospective and record closure

15. Schedule the release retrospective with engineering, QA, and product owner participants. Target it within ten business days of production deployment.
16. Update the release tracking record (the release issue, the release page in your wiki, or the Project notes field) with: deployment timestamp, deployed commit SHA, milestone closure date, count of deferred items with their new targets, and the retrospective date.

## Validation Checklist

- [ ] Production deployment shows **Success** in each contributing repository's environment history.
- [ ] The GitHub Release is published (not draft) and tagged correctly.
- [ ] Every contributing repository's milestone for this release is closed.
- [ ] No issue still carries `Release: <this release>` with an open status, unless intentionally deferred and re-targeted.
- [ ] Each deferred item has a comment explaining why it slipped and where it moved.
- [ ] The Project executive view shows this release in the closed group, not the active group.
- [ ] Retrospective is on the calendar with the right participants.

## What Good Looks Like vs. What to Escalate

Use this table when you are reviewing another manager's release closure rather than performing it yourself.

| Area | What Good Looks Like | What to Escalate |
|---|---|---|
| Production deployment | Success status, approver recorded, commit SHA matches release tag | Failed, partial, or missing deployment record; mismatched commit SHA |
| GitHub Release | Published, latest-release badge correct, notes and assets final | Still in Draft after deployment; pre-release flag set in error; missing or placeholder notes |
| Milestone state | All contributing milestones closed; completion at 100 percent or with documented deferrals | Milestone still open with no plan; closed milestone with open critical issues still attached |
| Deferred work | Re-targeted to a named milestone or backlog with comment explaining the slip | Items left with the released `Release` value but `Status: Todo` or `In Progress`; no comment on the slip |
| Project field state | Release option marked closed or moved to closed group; executive view filters it out | Release still appearing as active in dashboards |
| Audit trail | Deployment timestamp, commit SHA, closure date, and retrospective date recorded together | No closure record; details scattered across chat or email |
| Retrospective | Scheduled within ten business days with engineering, QA, and product owner | No retrospective scheduled; only one function represented |

If two or more rows fall in the right column, return the closure to the release manager with a list of gaps before signing off.

## Common Mistakes

- Closing the milestone before re-targeting deferred work, leaving issues attached to a closed milestone with no forward path.
- Publishing the release while a production deployment job is still **Waiting** for approval or has failed.
- Leaving the GitHub Release in **Draft** after production has shipped, which blocks downstream consumers and release notes feeds.
- Clearing the `Release` field on deferred items without setting a new target, which sends them into an invisible backlog.
- Forgetting to confirm the **Set as the latest release** state, especially when a pre-release was published earlier in the cycle.
- Skipping the retrospective scheduling step, which removes the trigger for process improvement.

## Escalation Path

- GitHub administrator: When a Project field cannot be edited because of permission gaps, or when the release option needs to be archived organization-wide.
- Repository administrator: When a milestone cannot be closed because of attached issues you do not own, or when the release tag needs to be moved.
- Engineering lead: When deferred work cannot be re-targeted because no team has accepted the next-release ownership.
- Release manager: When deployment did not reach production successfully and the release should not be closed at all. Re-open GHE-ALM-046 to confirm readiness was incorrectly assessed.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-042 : How to Create and Manage a Milestone
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-066 : How to Review Deployment History
