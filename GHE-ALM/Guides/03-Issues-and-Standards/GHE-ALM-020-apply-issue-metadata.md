# How to Apply Issue Metadata Correctly

**Guide ID:** GHE-ALM-020
**Audience:** Project Manager, Engineering Manager, Product Owner
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 5-10 minutes per issue
**Required permissions:** Repository: Triage; Project: Write

**Prerequisites:**

- The issue exists in a repository that participates in the ALM Project.
- The organization has issue types and organization issue fields configured (see GHE-ALM-024).
- The ALM Project has the canonical Project fields configured.
- You know the work item's parent, target release, and product area.

**When to use this guide:** Use this guide every time you create or grade an issue that will enter sprint planning, release tracking, or any management report. It is the metadata pass that turns a raw issue into a tracked work item.

**When not to use this guide:** Do not use this guide for throwaway notes, personal reminders, or internal-only conversations that will never reach a sprint board, a milestone, or a dashboard. Those do not need the full metadata set.

## Outcome

By the end of this guide, you will have produced:

- An issue with the correct issue type, owner, labels, milestone, and Project membership.
- Populated values for every required Project field: `Status`, `Priority`, `Severity` (where applicable), `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, and `Customer Impact`.
- A work item that is clean enough to commit to a sprint, attach to a release, and report on without further cleanup.

## Before You Start

- Confirm the issue's parent (epic, feature, or requirement). Sub-issue links are part of metadata hygiene even though they are not a field.
- Decide the target release and product area before you open the issue. These two values drive most filtering.
- Check who owns the work. Do not apply metadata you cannot defend in a triage meeting.

## Steps

### Set the core issue fields

1. Open the issue in the repository. In the right-hand sidebar, locate the **Type**, **Assignees**, **Labels**, **Projects**, and **Milestone** controls. These are GitHub's built-in issue fields.
2. Set **Type** to the correct organization issue type, for example `Feature`, `Requirement`, `Bug`, `Task`, `Epic`, `Risk`, or `Change Request`. Issue types are governed at the organization level; if the type you need is missing, request it through GHE-ALM-023 rather than substituting a label.
3. Set **Assignees**. Assign the single person accountable for delivery. Avoid assigning a whole team; use a team mention in the body if many people will contribute. Unassigned work is ignored work.
4. Apply **Labels** for secondary classification only. Use labels for cross-cutting attributes such as `area:auth`, `tech-debt`, `customer-reported`, or `needs-design`. Do not use labels to encode type, severity, priority, or release; those belong in dedicated fields. See GHE-ALM-021 for label discipline.
5. Set the repository **Milestone** if the work targets a single repository's release, for example `v2026.05.0`. Cross-repository release coordination uses the Project `Release` field instead, set in the next phase.

> [SCREENSHOT: Issue sidebar showing Type, Assignees, Labels, Projects, and Milestone populated for a feature issue.]

### Add the issue to the ALM Project

6. In the issue sidebar under **Projects**, add the issue to the organization ALM Project. If auto-add is configured (GHE-ALM-009), confirm the issue arrived rather than re-adding it.
7. Open the Project from the issue sidebar. The issue's Project fields appear inline in the issue panel and in the Project table.

### Populate the canonical Project fields

8. Set `Status`. Use the workflow value that matches the work's actual state, typically one of `Triage`, `Ready`, `In Progress`, `In Review`, `Ready for QA`, `Done`. Do not skip `Triage`; that step is where metadata gets validated.
9. Set `Priority`. Use the team's illustrative scale, commonly `P0` through `P3`. Priority is business urgency, not technical impact.
10. Set `Severity` for bugs and risks. Severity is technical or user impact. The illustrative scale below is a starting point; confirm your team's actual scale with QA leadership.

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

11. Set `Effort`. Use the team's estimation unit, whether story points or t-shirt sizes. An empty `Effort` blocks sprint capacity discussion.
12. Set `Sprint`. Use the iteration the work is committed to, or leave it blank if the item is still in the backlog. The canonical filter for current iteration is `sprint:@current`.
13. Set `Release`. Use the cross-repository release train value, for example `2026.05.0` or `2026-Q3 Release`. The repository **Milestone** and the Project `Release` field are not interchangeable; set both when the work spans both views.
14. Set `Product Area`. Use the canonical taxonomy, for example `Checkout`, `Billing`, or `Identity`. Product Area is the most-used filter on every leadership view; treat it as required.
15. Set `Owner`. The Project `Owner` field tracks the single accountable person for the Project view, which is usually the same as the issue assignee but may differ for cross-team coordination items. When they differ, the Project `Owner` is the person who answers status questions in standup.
16. Set `Start Date` and `Target Date`. Use these for any item that appears on a roadmap or has a date commitment. Empty dates make the roadmap layout useless.
17. Set `Risk Level` for items that introduce delivery, security, or compliance risk. Common values are `Low`, `Medium`, `High`. Empty `Risk Level` is acceptable for routine work.
18. Set `Customer Impact` for items that are customer-visible or customer-reported. Common values are `None`, `Minor`, `Major`, `Critical`. This field drives executive reporting; do not skip it on bugs and feature work that customers will notice.

> [SCREENSHOT: Project table view showing the canonical fields populated for a single feature row.]

### Verify and link

19. Link the issue to its parent issue using the **Sub-issues** control on the parent, or confirm the parent already lists this issue. A feature without a parent epic, or a task without a parent feature or requirement, fails hierarchy hygiene (GHE-ALM-018).
20. Run a final read of the issue from top to bottom. The sidebar should show every applicable field set, the Project panel should show every canonical Project field set, and the parent link should be present.

> [SCREENSHOT: Issue page after metadata is complete, showing sidebar fields, Project fields panel, and parent link.]

## Worked example

Before metadata, an issue in `acme-payments/checkout-service` looks like this:

- Title: `Add Apple Pay to checkout`.
- Type: not set.
- Assignees: none.
- Labels: `enhancement`.
- Milestone: none.
- Projects: none.
- Body: a paragraph of context, no acceptance criteria.

This issue cannot be planned, prioritized, or reported on. After applying metadata, the same issue looks like this:

- Title: `Add Apple Pay to checkout`.
- Type: `Feature`.
- Assignees: `@jchen`.
- Labels: `area:checkout`, `customer-reported`.
- Milestone: `v2026.05.0`.
- Projects: `acme-payments ALM`.
- Parent: linked to epic `Wallet payment methods`.
- Project `Status`: `Ready`.
- Project `Priority`: `P1`.
- Project `Effort`: `5`.
- Project `Sprint`: `Sprint 27`.
- Project `Release`: `2026.05.0`.
- Project `Product Area`: `Checkout`.
- Project `Owner`: `@jchen`.
- Project `Start Date`: `2026-05-04`.
- Project `Target Date`: `2026-05-15`.
- Project `Customer Impact`: `Major`.

The issue is now ready for sprint commitment, fits cleanly on the release roadmap, and appears in every Product Area, Release, and Owner filter without manual cleanup.

## Validation Checklist

- [ ] Issue **Type** is set to a valid organization issue type.
- [ ] Issue has exactly one assignee accountable for delivery.
- [ ] Labels are secondary classifiers only, not substitutes for type, severity, priority, or release.
- [ ] Repository **Milestone** is set if the work targets a single repository release.
- [ ] Issue is a member of the ALM Project.
- [ ] `Status`, `Priority`, `Effort`, `Product Area`, and `Owner` are populated.
- [ ] `Severity` is set for bugs and risks.
- [ ] `Sprint` is set if the item is committed to an iteration, blank otherwise.
- [ ] `Release` is set for any item targeting a release train.
- [ ] `Start Date` and `Target Date` are set for roadmap items.
- [ ] `Customer Impact` is set on customer-visible work.
- [ ] Issue is linked to its parent epic, feature, or requirement.

## Common Mistakes

- Using a label such as `bug` or `feature` instead of setting **Type**. Issue types are organization-governed and reportable; labels are not.
- Setting repository **Milestone** but leaving Project `Release` blank for a cross-repository release. Reports against the Project will miss the work.
- Assigning a whole team rather than one accountable owner. The work disappears into the team and no one drives it.
- Treating `Severity` and `Priority` as the same field. Severity is impact, Priority is urgency; they are independent.
- Leaving `Product Area` blank. Almost every leadership view groups or filters by Product Area; blanks fall out of every report.
- Filling in `Effort` with a guess to clear the field. A defensible blank is better than an indefensible number.
- Filling fields once and never revisiting them. Metadata drifts; review at sprint boundaries (GHE-ALM-022) and quarterly (GHE-ALM-076).

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve when the repository is missing required labels or when the issue type you need does not exist in the organization (route through GHE-ALM-023).
- Engineering lead: Involve when ownership, effort, or parent linkage cannot be decided without engineering input.
- Release manager: Involve when `Release` or **Milestone** assignment is contested between two release trains.

## Related Guides

- GHE-ALM-021 : How to Use Labels Without Replacing Issue Types
- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
- GHE-ALM-024 : How to Define or Request Organization Issue Fields
- GHE-ALM-076 : How to Govern Project Fields and Labels
