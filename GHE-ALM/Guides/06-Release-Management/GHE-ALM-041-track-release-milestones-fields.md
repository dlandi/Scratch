# How to Track a Release with Milestones and Release Fields

**Guide ID:** GHE-ALM-041
**Audience:** Release Manager, Engineering Manager, Program Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 30-45 minutes for initial setup; 10 minutes per release thereafter
**Required permissions:** Repository: Triage (to create and edit milestones); Project: Write (to set the `Release` field on items)
**Prerequisites:**

- An organization-level GitHub Project exists for the product. See GHE-ALM-006.
- A `Release` single-select field exists on the Project. If it does not, request it through GHE-ALM-024.
- Issues and pull requests for the planned release are already added to the Project.
- A naming convention for milestones and `Release` field values has been agreed with engineering and release management.

**When to use this guide:** Use this guide at the start of release planning, whenever a new release train is opened, or when scope is being assigned across one or more repositories. It is the single point where you decide whether a release is repository-scoped, cross-repository, or both, and you set up the tracking that every later release activity depends on.

**When not to use this guide:** Do not use this guide to draft the final GitHub Release artifact, write release notes, or close a release after deployment. Those activities are covered by GHE-ALM-047 and GHE-ALM-050.

## Outcome

By the end of this guide, you will have produced:

- A repository milestone for each repository that contributes scope to the release.
- A `Release` value set on every Project item that belongs to the release.
- A Project view filtered by `Release` that shows the full release scope across repositories on a single screen.
- A documented decision on whether this release is tracked by milestone, by `Release` field, or both.

## Before You Start

- Confirm the release identifier you will use, for example `2026.05.0` or `2026-Q2 Release`.
- List every repository that will contribute commits, issues, or pull requests to the release.
- Confirm the target date. Milestones support a single due date and the Project roadmap uses date fields to plot the release.
- Confirm with engineering whether the release maps to a single repository, multiple repositories, or a mix. The answer drives the choice between milestone, `Release` field, or both.

## Steps

### Decide the tracking model

1. Review the list of contributing repositories. If exactly one repository ships the release, plan to use a repository milestone as the primary tracker. If two or more repositories ship together, plan to use the Project `Release` field as the primary tracker, with a milestone in each contributing repository for repository-local progress. The `Release` field gives you cross-repository visibility; milestones give you per-repository completion percentages.
2. Record the decision in the Project description or in your release wiki so that the next release manager does not have to re-derive it. State the release identifier, the contributing repositories, the milestone name in each repository, and the `Release` field value.

### Create the repository milestone

3. Open the first contributing repository. Click the **Issues** tab, then click **Milestones**, then click **New milestone**.
4. Enter the milestone **Title** using the agreed naming pattern. Acceptable patterns include `v1.0`, `v1.1`, `2026-Q2 Release`, and `2026.05.0`. Pick one pattern per product and stay with it. Do not mix `v1.1` and `Release 1.1` in the same repository.
5. Enter a short **Description** that names the release identifier, the planned content theme, and a link to the Project view that shows the full scope. The description is the first thing a developer sees when they pick up an issue assigned to this milestone.
6. Set the **Due date** to the planned release date. Leave it blank only if the release date is genuinely undecided; an empty due date hides the milestone from roadmap and burn-down charts.
7. Click **Create milestone**.
8. Repeat steps 3 through 7 in every other repository that ships scope for this release. Use the identical milestone title in each repository so that filters and reports line up.

> [SCREENSHOT: New milestone form in a repository, with title, description, and due date filled in]

### Set the Release field on Project items

9. Open the organization Project that tracks the product. Switch to a table view.
10. Filter the table to the issues and pull requests that belong to this release. A common starting filter is `no:Release` combined with a label or sprint that identifies the release candidates.
11. Select the first item. In the side panel, set the **Release** field to the agreed value, for example `2026.Q2`, `2026.05`, or the named release train. Use the same naming pattern across all items; the `Release` field is a single-select, so a typo creates a new value.
12. Repeat for every item in scope. To move faster, group the table by the field you used to filter, then use the bulk-edit selector to set `Release` on multiple items at once.
13. Where the work item lives in a repository that has a milestone for this release, also set the repository **Milestone** field on the issue or pull request. The milestone drives the repository completion percentage; the `Release` field drives the cross-repository view.

> [SCREENSHOT: Project table grouped by Release with the side panel showing the Release field selector]

### Build the cross-repository release view

14. In the same Project, click **New view**, choose **Table** layout, and name the view `Release Scope - <release identifier>`, for example `Release Scope - 2026.05.0`.
15. Add a filter for `Release:"2026.05.0"` (substitute your release value). Group the view by **Repository** so each contributing repository becomes a section. Add columns for **Status**, **Milestone**, **Assignees**, and **Target Date**.
16. Save the view. This is the screen you will open in every release readiness review and every weekly release sync. It shows scope, owner, and status across repositories without leaving the Project.

> [SCREENSHOT: Project table view filtered by Release and grouped by Repository]

### Connect to the eventual GitHub Release

17. Confirm with the team that when the release ships, the GitHub Release artifact will be drafted in the primary repository for the train. The milestone gives you the list of merged pull requests; the `Release` field gives you the cross-repository scope. Both feed the release notes drafted in GHE-ALM-047. Do not draft the GitHub Release here; that is a separate activity performed at code freeze.

## Validation Checklist

- [ ] Each contributing repository has a milestone with the agreed title and a due date.
- [ ] Every Project item in scope has the `Release` field set to the agreed value.
- [ ] Items that live in a milestoned repository also have the repository `Milestone` field set.
- [ ] A saved Project view filtered by `Release` exists and shows the full scope grouped by repository.
- [ ] The completion percentage on each milestone updates when issues are closed.
- [ ] The decision on tracking model (milestone, `Release` field, or both) is documented in the Project description.

## Common Mistakes

- Treating the `Release` field as a substitute for milestones. The `Release` field gives you a cross-repository view but does not give you the per-repository completion percentage that milestones produce automatically. Cross-repository releases need both.
- Treating milestones as a substitute for the `Release` field. Milestones cannot be queried across repositories in a single view. A release that spans three repositories needs the `Release` field for the consolidated view.
- Inconsistent naming. `v1.1`, `Release 1.1`, and `1.1.0` are three different values to GitHub. Pick one pattern per product and enforce it through GHE-ALM-077.
- Leaving the milestone due date blank. The roadmap and progress views ignore undated milestones, which silently removes the release from leadership reporting.
- Setting the `Release` field at the issue level but forgetting linked pull requests. Pull requests are Project items too; filter for `is:pr no:Release` to find PRs that fell out of scope tracking.
- Creating the milestone in only one repository for a multi-repository release. The other repositories then show no progress, and the release looks healthier than it is.

## Escalation Path

- GitHub administrator: Involve only if the `Release` field does not exist at the organization level and you need it added across multiple Projects. See GHE-ALM-024.
- Repository administrator: Involve if you cannot create a milestone because you lack Triage permission on a contributing repository.
- Engineering lead: Involve when scope decisions are contested, when the contributing-repository list is unclear, or when a repository owner pushes back on the milestone naming pattern.
- Release manager: This is the primary role for this guide. The release manager owns the naming pattern, the cross-repository view, and the documented tracking decision.

## Related Guides

- GHE-ALM-042 : How to Create and Manage a Milestone
- GHE-ALM-043 : How to Add Issues and Pull Requests to a Milestone
- GHE-ALM-049 : How to Track a Cross-Repository Release
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-050 : How to Close a Release After Deployment
