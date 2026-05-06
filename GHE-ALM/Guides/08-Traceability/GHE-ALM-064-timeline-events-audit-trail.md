# How to Use Issue and PR Timeline Events for Audit Trail

**Guide ID:** GHE-ALM-064
**Audience:** Engineering Manager, Release Manager, QA Manager
**Primary role:** Engineering Manager
**Classification:** Manager Reviews
**Estimated time:** 10-15 minutes per item audited
**Required permissions:** Repository: Read; Project: Read

**Prerequisites:**

- The issue or pull request URL, or the project item URL.
- Read access to the repository that owns the issue or pull request.
- Read access to the Project if you need to inspect project field history.

**When to use this guide:** Use this guide when you need to reconstruct what happened to a single issue, pull request, or project item, for compliance, audit, post-incident review, or to answer "why is this still open?"

**When not to use this guide:** Do not use this for aggregate reporting across many items. For trends and counts, use Project Insights charts instead.

## Outcome

By the end of this guide, you will have produced:

- A confirmed sequence of events showing how an issue or pull request moved from creation to its current state.
- A list of who changed what, and when.
- A clear escalation note if the trail shows missing approvals, skipped status, or unexplained reopens.

## Before You Start

- Confirm the item identifier: issue or PR number, repository, and project item link if relevant.
- Decide what question the audit must answer. Common ones: "When did this become blocked?", "Who approved the merge?", "Did Status move backwards?", "Was the closing PR linked?"
- Have your audit notes file open. Timeline events do not export from the UI; you will copy what you need.

## Steps

### Open the timeline for an issue

1. Navigate to the issue in the repository, for example `acme-payments/checkout-service` issue `#1247`.
2. Scroll the issue page from top to bottom. The timeline is the chronological strip below the issue body, interleaved with comments. Each event shows an actor avatar, an action verb, and a relative timestamp; hover the timestamp for the absolute UTC time.
3. Identify and note the event categories that matter for an audit: assignment changes, label changes, milestone changes, project additions, project field changes (including `Status`, `Sprint`, `Release`, `Priority`), sub-issue links, dependency links, branch creation, PR linkage via closing keywords, close events, and reopen events.
4. For any event you cannot explain, click the actor's name to confirm the user, and click the linked artifact (PR, branch, project) to see the source of the change.

> [SCREENSHOT: issue page scrolled to show a sequence of timeline events including a project Status change, a linked PR, and a close event]

### Open the timeline for a pull request

5. Navigate to the linked pull request, for example `checkout-service` PR `#312`.
6. Open the **Conversation** tab. The PR timeline shows opens, ready-for-review transitions, review requests, reviews (approved, changes requested, commented), check runs, force-push events, branch updates, label and milestone changes, project field changes, merge events, and close/reopen events.
7. Confirm the PR references the source issue using a closing keyword such as `Closes #1247`. If the linkage is implicit through a project rather than a closing keyword, the timeline will show a "linked an issue" event instead.
8. For governance audits, confirm that the **Merged** event was preceded by the required reviews and required status checks. If the merge happened despite a missing requirement, treat that as an escalation.

### Open the per-item timeline in the Project

9. From the issue or PR sidebar, click the project name under **Projects** to open the project item view, or open the Project and click the row to expand the item drawer.
10. In the item drawer, open the **Activity** or timeline section. This view shows project-scoped changes only: `Status`, `Sprint`, `Release`, `Priority`, `Effort`, `Owner`, custom field changes, and automation actions such as "Item moved to Done by workflow."
11. Compare the project timeline against the issue timeline. The project timeline is the source of truth for field history; the issue timeline shows project addition and removal but not every field edit.

> [SCREENSHOT: project item drawer showing the Activity section with field-level changes attributed to a user or to a workflow]

### Reconstruct the audit narrative

12. Build a short timeline in your notes: created, triaged (label and priority applied), planned (added to `Sprint 27`), started (Status moved to In Progress), implemented (PR opened and linked), reviewed (approvals recorded), merged, closed, deployed (if a deployment event is linked).
13. Flag every gap: a status that skipped a stage, a reopen without a comment, a project removal, an assignee that changed during review, or a merge that happened without the expected approver.
14. Save the audit narrative with the item URL, the date you reviewed it, and your initials. If you need to share evidence, capture screenshots of the relevant timeline segments rather than copying long quotes.

## Validation Checklist

- [ ] The issue, PR, and project item timelines have all been opened.
- [ ] Every Status change has a clear actor (user or workflow automation).
- [ ] The merge event is preceded by the required reviews and required checks.
- [ ] The closing PR is linked via a closing keyword or an explicit link event.
- [ ] Any reopen, project removal, or unexplained backwards Status move is noted.
- [ ] The audit narrative is saved with the item URL and review date.

## What Good Looks Like vs. What to Escalate

| Signal | What Good Looks Like | What to Escalate |
|---|---|---|
| Item creation and triage | Issue created, type and priority applied within hours, added to project. | Created weeks ago, no type, no priority, never added to project. |
| Status progression | Forward-only moves: Triage, Ready, In Progress, In Review, Done. | Status moved backwards repeatedly, or skipped In Review and went straight to Done. |
| PR linkage | PR references the issue with `Closes #NNN`; merge auto-closes the issue. | PR merged but issue still open with no link event. |
| Reviews | Required reviewers approved before merge; code-owner review recorded if applicable. | Merge event with no approvals, or approvals from users outside the required group. |
| Status checks | All required checks green at merge. | Merge with failing or skipped required checks. |
| Project field changes | Field changes attributed to known users or named workflows. | Field changes by an unknown bot account, or mass Status flips with no explanation. |
| Reopen events | Reopen accompanied by a comment explaining the regression. | Silent reopen, especially after a release tag. |
| Assignee continuity | Assignee stable through implementation. | Assignee changed three or more times during a single sprint. |

## Common Mistakes

- Treating the issue timeline as the full record. Project field history lives in the project item timeline.
- Confusing a "linked an issue" event with a closing-keyword link. Only closing keywords trigger automatic issue closure on merge.
- Reading the relative timestamp instead of the absolute UTC timestamp when correlating with deployments or incident timelines.
- Missing automation events. Workflow automations show as actions by an app account such as `github-actions[bot]`; treat these as intentional unless the workflow change itself is suspect.
- Auditing only the merged PR while ignoring closed-without-merge PRs that targeted the same issue.

## Escalation Path

- GitHub administrator: Involve when audit log entries are needed beyond what the timeline shows, such as permission changes, ruleset edits, or deletions. The enterprise audit log is the source for those events.
- Repository administrator: Involve when a merge bypassed required reviews or required checks, or when branch protection appears to have been temporarily relaxed.
- Engineering lead: Involve when the timeline shows backwards Status moves, repeated reopens, or a merge by an unexpected approver.
- Release manager: Involve when an item closed inside a release window has no linked PR or no deployment evidence.

## Related Guides

- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-061 : How to Interpret Pull Request Status for Managers
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-063 : How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`
- GHE-ALM-074 : How to Review Ruleset and Branch Protection Coverage
