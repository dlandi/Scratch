# How to Track a Cross-Repository Release

**Guide ID:** GHE-ALM-049
**Audience:** Release Manager, Engineering Manager, Program Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 60-90 minutes for initial release-train setup; 15-20 minutes per weekly review
**Required permissions:** Repository: Triage in every contributing repository (to create milestones and assign scope); Project: Write on the organization Project (to set the `Release` field and configure views)
**Prerequisites:**

- An organization-level GitHub Project exists for the release train. See GHE-ALM-006.
- A `Release` single-select or text field exists on the Project. If it does not, request it through GHE-ALM-024.
- Each contributing repository is connected to the Project, with auto-add workflows set up for issues and pull requests in scope. See GHE-ALM-009.
- A naming convention for the release identifier, repository milestones, and the `Release` field value has been agreed across the contributing teams.
- A target release date is committed.

**When to use this guide:** Use this guide when a single release ships work from two or more repositories at the same time, for example a backend service, a client, and an infrastructure repository. The release train is the unit of coordination, and you need one place that shows full scope.

**When not to use this guide:** Do not use this guide for a release that is wholly contained inside one repository; a milestone alone is sufficient. See GHE-ALM-041 and GHE-ALM-042. Do not use this guide to draft the GitHub Release artifact or close the release after deployment; see GHE-ALM-047 and GHE-ALM-050.

## Outcome

By the end of this guide, you will have produced:

- A Project `Release` field value that identifies the release train, for example `2026.Q2`.
- A milestone with the same release name in every contributing repository.
- An organization Project view filtered to the release that shows scope across all contributing repositories on a single screen.
- A release roadmap view grouped by `Release` so adjacent release trains are visible alongside the current one.
- A documented readiness review checklist that confirms scope, defects, traceability, and deployment status across every contributing repository.

## Before You Start

- Confirm the release identifier you will use, for example `2026.Q2`.
- List every contributing repository. The worked example uses `acme-payments/service-a`, `acme-payments/service-b`, `acme-payments/web-client`, and `acme-payments/infrastructure`.
- Confirm which engineering owner is accountable per repository.
- Confirm the `Release` field type. A single-select field is preferred; a text field allows typos and silently splits the release into two.
- Confirm the date model. Roadmap layouts use `Start Date` and `Target Date` on items, plus the milestone due date for repository-local progress.

## Steps

### Define the release in the Project Release field

1. Open the organization Project that tracks the release train. The worked example uses a Project named `acme-payments Release Trains`. If no Project exists, create one from GHE-ALM-006 first.
2. Open **Settings**, then the **Fields** panel. Confirm a `Release` single-select field exists. If it does not, request it through GHE-ALM-024 before continuing; do not improvise with a label.
3. In the `Release` field options, click **New option** and add the release identifier, for example `2026.Q2`. Use the exact identifier engineering, release management, and product have agreed. Do not abbreviate it differently elsewhere.
4. Confirm earlier and later release identifiers exist as separate options, for example `2026.Q1` and `2026.Q3`. The roadmap groups by these values, so adjacent trains need to be present.

> [SCREENSHOT: Project Settings showing the Release single-select field with `2026.Q1`, `2026.Q2`, and `2026.Q3` as options]

### Create per-repository milestones

5. Open the first contributing repository, `acme-payments/service-a`. Click the **Issues** tab, then **Milestones**, then **New milestone**.
6. Enter the milestone **Title** as the exact release identifier, `2026.Q2`. The milestone title and the `Release` field value must match character for character so filters and reports can be cross-checked.
7. Enter a short **Description** naming the release train, the contributing repositories, and a link to the organization Project view that shows full scope.
8. Set the **Due date** to the planned release date. The due date drives the repository-local completion percentage and burn-down.
9. Click **Create milestone**.
10. Repeat steps 5 through 9 in every other contributing repository. For the worked example, create `2026.Q2` milestones in `service-b`, `web-client`, and `infrastructure`. Identical titles are mandatory; `2026 Q2` and `2026.Q2` will not group together.

> [SCREENSHOT: Milestone list in `acme-payments/service-a` showing `2026.Q2` with due date and completion bar]

### Populate the Project view

11. Return to the organization Project. Add or confirm a table view named `Release 2026.Q2 Scope`.
12. Set the filter to `release:"2026.Q2"`. Group by **Repository** so contributing repositories appear as collapsible sections. Sort within each group by **Status**, then by **Priority**.
13. Walk every issue and pull request that belongs to the release and set the **Release** field to `2026.Q2`. For items in a repository with a `2026.Q2` milestone, also set the repository **Milestone** field to `2026.Q2`. The `Release` field drives cross-repository visibility; the milestone drives per-repository completion. Both are required.
14. To move faster, filter by sprint or label, select multiple rows, and use the bulk-edit selector to set **Release** on all of them at once.
15. Spot-check the view. Confirm every contributing repository is represented, no item is missing a **Status** or **Owner**, and no item has `2026.Q2` in the `Release` field while pointing at a different milestone. Misalignment is the most common cause of a release looking green in one view and red in another.

### Build the release roadmap view

16. In the same Project, add a new view with layout **Roadmap**. Name it `Release Train Roadmap`.
17. Set the start field to `Start Date` and the target field to `Target Date`. Group the roadmap by **Release** so each train, including `2026.Q1`, `2026.Q2`, and `2026.Q3`, appears as its own swimlane.
18. Set the zoom level to **Quarter** for planning. Switch to **Month** inside the release window to see weekly slip risk.
19. Confirm the `2026.Q2` swimlane shows items from every contributing repository. If a repository is missing, its issues are likely not added to the Project; revisit the auto-add workflow in GHE-ALM-009. See GHE-ALM-044 for deeper roadmap configuration.

> [SCREENSHOT: Release Train Roadmap with swimlanes for `2026.Q1`, `2026.Q2`, and `2026.Q3`, items grouped under `2026.Q2` and color-coded by Status]

### Monitor scope across repositories

20. Schedule a weekly cross-repository scope review during the release window. Use the `Release 2026.Q2 Scope` view as the agenda.
21. For each repository group, walk the open items. Ask three questions per item: is the **Owner** still correct, is the **Status** current within seven days, and is the **Target Date** still credible. Re-assign, re-status, or escalate from the side panel.
22. Open the **Insights** tab. Pin a chart filtered by `release:"2026.Q2"` and grouped by **Status**. The chart should trend toward done. A flat or growing open count three weeks before target is scope-creep; raise it in the next review. See GHE-ALM-045.
23. Cross-check each repository milestone for `2026.Q2`. The milestone completion bar should track within ten percent of the equivalent slice of the Project chart. A large gap usually means an item is in the milestone but missing the `Release` field, or vice versa. Fix the alignment.

### Cross-repository readiness review

24. One week before the target date, run the cross-repository readiness review against the `Release 2026.Q2 Scope` view. Treat any failure as a blocker that must be resolved or formally deferred before the release decision meeting:

    - Every item in scope is in **Status** `Done`, `Verified`, or has been moved out with the `Release` field cleared or set to a future train.
    - Every contributing repository milestone for `2026.Q2` is at one hundred percent or contains only items moved out.
    - Every closed issue is linked to at least one merged pull request through `Closes`, `Fixes`, or `Resolves`. See GHE-ALM-060.
    - No open bug exists with **Severity** 1 or 2 assigned to the release. A common 1-4 / P0-P3 scale is used here; confirm your team's actual scale with QA leadership.
    - Every contributing repository has a draft GitHub Release prepared with the agreed tag. See GHE-ALM-047.
    - The deployment plan names the order in which contributing repositories deploy and the rollback procedure if one fails.

25. Capture the readiness result as a comment on the parent release-tracking issue with the `Release` field set to `2026.Q2`. The comment is the audit record that this checklist was applied. See GHE-ALM-046.

## Validation Checklist

- [ ] The Project `Release` field contains `2026.Q2` as a single-select option, not a free-text variant.
- [ ] A `2026.Q2` milestone exists in every contributing repository with the same title and the same due date.
- [ ] The `Release 2026.Q2 Scope` Project view shows items from every contributing repository when filtered by `release:"2026.Q2"`.
- [ ] The `Release Train Roadmap` view shows `2026.Q2` as a swimlane alongside adjacent release trains.
- [ ] Every item in scope has both the **Release** field and the repository **Milestone** set, where the repository has a milestone.
- [ ] The cross-repository readiness review is recorded against the release-tracking issue.

## Common Mistakes

- Using slightly different release identifiers in different places, for example `2026.Q2` in the `Release` field and `2026 Q2` in one repository milestone. The two will not group together and the release will look incomplete in one view and complete in another.
- Setting the **Release** field but forgetting the per-repository **Milestone**, or vice versa. The Project view and the repository milestone progress bar will then disagree.
- Treating the `Release` field as a label. Labels do not enforce a single value, do not appear in roadmap layouts, and cannot be filtered with `release:` syntax.
- Adding a contributing repository part-way through the release without backfilling the `Release` field on its already-open issues. The roadmap then shows the new repository as having zero scope until each issue is touched.
- Closing the cross-repository release before every contributing repository milestone is at one hundred percent or has had remaining items moved to a future release.

## Escalation Path

- GitHub administrator: Involve when the organization Project is missing the `Release` field type or when auto-add workflows cannot be configured for a contributing repository.
- Repository administrator: Involve when a contributing repository will not accept the agreed milestone naming convention or when triage permission is not available to create the milestone.
- Engineering lead: Involve when a contributing repository owner disputes the scope assigned to the release or when items are added to the `Release` field without the owner's agreement.
- Release manager: Owns this activity end to end and chairs the cross-repository readiness review.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-042 : How to Create and Manage a Milestone
- GHE-ALM-044 : How to Use the Release Roadmap View
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-050 : How to Close a Release After Deployment
