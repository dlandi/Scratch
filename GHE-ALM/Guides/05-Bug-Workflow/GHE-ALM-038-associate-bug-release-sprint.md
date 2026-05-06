# How to Associate a Bug with a Release or Sprint

**Guide ID:** GHE-ALM-038
**Audience:** Engineering Manager, QA Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 5-10 minutes per bug
**Required permissions:** Repository: Triage; Project: Write
**Prerequisites:**

- The bug already exists as an issue with type `Bug` (see GHE-ALM-014).
- The bug is added to the organization ALM Project.
- The Project has `Release`, `Sprint`, and `Product Area` fields configured.
- The repository has milestones defined for active and upcoming versions.

**When to use this guide:** Use this guide every time a bug enters triage and a manager needs to record where it was observed, where the fix should ship, and when the team will work on it.

**When not to use this guide:** Do not use this guide for production hotfixes that bypass the normal release train. Use GHE-ALM-040 (Hotfix Bug) instead.

## Outcome

By the end of this guide, you will have produced:

- A bug issue with `Affected Release`, `Release` (target), `Sprint`, `Product Area`, and a repository milestone populated.
- A defect record that appears correctly in the sprint board, the release roadmap, and the bug triage view.

## Before You Start

- Identify the version where the bug was reported. This becomes the affected release.
- Decide the version where the fix should ship. This becomes the target release.
- Decide whether the team will start the fix this sprint, next sprint, or later.
- Confirm the bug already has severity, priority, owner, and reproduction evidence (see GHE-ALM-014, GHE-ALM-037).

## Steps

### Open the bug and confirm it is project-attached

1. From the repository, open the **Issues** tab and select the bug. Confirm the issue type shown next to the title is `Bug`.
2. In the right sidebar, scroll to **Projects** and confirm the bug appears in the organization ALM Project. If it does not, click **Projects**, search for the ALM Project, and add it. Without this, the project fields below will not be visible.

> [SCREENSHOT: Bug issue sidebar showing Projects section with ALM Project attached]

### Set the affected release

3. In the **Projects** panel of the sidebar, expand the ALM Project entry. Locate the `Affected Release` field. If your organization uses a single `Release` field for both, see the note in Common Mistakes.
4. Click the field and select the version where the bug was observed, for example `2026.04.2`. Use the exact value from your release calendar; do not invent a new value.

### Set the target release

5. In the same project panel, locate the `Release` field. This is the canonical project field for the version where the fix should ship.
6. Click the field and select the target version, for example `2026.05.0`. If the fix has not yet been scoped, leave this blank and let the next triage pass set it. Do not guess.
7. In the right sidebar of the issue, scroll to **Milestone** and select the repository milestone that matches the target release, for example `2026.05.0`. The milestone is repository-scoped and gives engineering a per-repo cut of release scope; the project `Release` field gives the cross-repo cut.

> [SCREENSHOT: Project sidebar with Affected Release and Release fields populated, plus Milestone field set in the issue sidebar]

### Set the sprint

8. In the project panel, locate the `Sprint` field. Click it.
9. Select the iteration when the team will work the bug. Use `@current` for the active sprint, `@next` for the next planned sprint, or pick a named iteration such as `Sprint 27`. Leave the field empty for items still in the backlog.
10. If the bug is severity 1 or priority P0, also confirm with the engineering lead that the sprint has capacity. Pulling a P0 into the current sprint usually requires displacing other work.

### Set the product area

11. Locate the `Product Area` field in the project panel. Set it to the area that owns the affected component, for example `Checkout`, `Billing`, or `Identity`. This drives routing, ownership reports, and severity-by-area dashboards.

### Save and verify

12. GitHub saves field changes automatically. Refresh the issue and confirm `Affected Release`, `Release`, `Sprint`, `Product Area`, and the milestone all show the expected values.

> [SCREENSHOT: Refreshed issue showing all five associations populated]

## Worked Example

A QA engineer files a bug against the `acme-payments/checkout-service` repository. Reproduction shows the regression first appeared in version `2026.04.2`. The team agrees the fix will ship in `2026.05.0` and that work starts immediately.

The engineering manager opens the bug and sets:

- `Affected Release`: `2026.04.2`
- `Release` (target): `2026.05.0`
- Repository **Milestone**: `2026.05.0`
- `Sprint`: `Sprint 27` (the active iteration, equivalent to `sprint:@current`)
- `Product Area`: `Checkout`

The bug now appears in the current sprint board grouped under `In Progress` once an engineer picks it up, in the `2026.05.0` release roadmap row, and in the bug triage view filtered by `Affected Release: 2026.04.2`.

## Validation Checklist

- [ ] `Affected Release` reflects the version where the bug was first observed.
- [ ] `Release` reflects the target version where the fix should ship.
- [ ] Repository milestone matches the target `Release` value.
- [ ] `Sprint` reflects when the team will work the bug, or is intentionally blank.
- [ ] `Product Area` reflects the owning area.
- [ ] The bug appears in the current sprint board if `Sprint` is set to the active iteration.
- [ ] The bug appears in the release roadmap row for the target `Release`.

## Common Mistakes

- Confusing affected release with target release. Affected is where the bug was seen; target is where the fix lands. Both matter for triage.
- Setting `Release` to the version the bug was found in. Use `Affected Release` for that. The `Release` field is the target.
- Setting only the milestone without the project `Release` field. The milestone is repository-scoped and will not show in cross-repo release roadmaps.
- Setting only the project `Release` field without the milestone. Single-repo release reports and per-repo burn-down will miss the bug.
- Assigning a P0 or severity 1 bug to a future sprint by default. Critical defects belong in the active sprint or in a hotfix workflow (GHE-ALM-040).
- Filling `Sprint` with a guess when the team has not committed. Leave it blank and let sprint planning (GHE-ALM-028) place it.
- Using a label like `release-2026.05.0` instead of the structured `Release` field. Labels do not roll up in the roadmap or insights.
- Treating `Affected Release` and `Release` as the same field. Some organizations only configure one. If yours does, request the second field via GHE-ALM-024 so triage can record both signals.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: When a required milestone does not exist, ask the repository administrator to create it (see GHE-ALM-042).
- Engineering lead: When a bug needs to land in the active sprint and capacity is unclear.
- Release manager: When the proposed target release is already locked or in code freeze, or when the bug crosses repositories and needs cross-repo release coordination (see GHE-ALM-049).

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-040 : How to Handle a Hotfix Bug
- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-049 : How to Track a Cross-Repository Release
