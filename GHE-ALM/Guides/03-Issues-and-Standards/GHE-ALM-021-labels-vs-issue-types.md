# How to Use Labels Without Replacing Issue Types

**Guide ID:** GHE-ALM-021
**Audience:** Project Manager, Engineering Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 15-20 minutes per use; 30-45 minutes for a quarterly label review
**Required permissions:** Repository: Triage to apply labels; Repository: Write to create, edit, or delete labels
**Prerequisites:**

- Organization-level issue types are configured (Epic, Feature, Requirement, Task, Bug at minimum).
- Project fields for `Priority`, `Severity`, `Status`, `Sprint`, `Release`, and `Product Area` exist.
- You can locate the repository **Labels** page under the **Issues** tab.

**When to use this guide:** Use this guide when applying labels to a new issue, when reviewing a repository for label sprawl, or when a teammate proposes a new label and you need to decide whether it belongs as a label, an issue type, or a project field.

**When not to use this guide:** Do not use this guide to define organization-wide work item taxonomy. That belongs in GHE-ALM-023. Do not use it to govern field ownership; that belongs in GHE-ALM-076.

## Outcome

By the end of this guide, you will have produced:

- A correctly labeled issue or pull request that does not duplicate issue type or project field information.
- A short list of labels in your repository that should be renamed, merged, deleted, or promoted to issue types or fields.

## Before You Start

- Confirm the repository's issue type list. Open any issue and check the **Type** selector. If types are missing, stop and route through GHE-ALM-023 first.
- Confirm the project's custom fields. Open the project, then **Settings**, then **Fields**. Note what is already captured by `Priority`, `Severity`, `Status`, `Sprint`, `Release`, and `Product Area`.
- Decide your scope: a single issue, a single repository, or a cross-repository sweep.

## Steps

### Decide what belongs as a label

1. Confirm the work item category is captured by **Type**, not a label. Epic, Feature, Requirement, Task, Bug, Risk, and Change Request are issue types. Never use labels named `bug`, `feature`, `epic`, `requirement`, `task`, or `risk` to mean the same thing as the type. If a default label `bug` exists alongside the **Bug** issue type, plan to retire the label.
2. Confirm the lifecycle state is captured by `Status`, not a label. Discourage labels such as `status:in-progress`, `wip`, `blocked`, `ready-for-qa`, `done`. The board column reflects this; labels duplicate it and drift.
3. Confirm urgency and impact are captured by `Priority` and `Severity` fields, not labels. Discourage labels such as `priority:high`, `p0`, `severity:major`. Fields filter, sort, and group; labels do not roll up to insights cleanly.
4. Confirm scheduling is captured by `Sprint`, `Release`, and **Milestone**. Discourage labels such as `sprint-27`, `release-2026.05`, `next-up`.
5. Use a label only when the classification is **secondary**, **repository-scoped**, **cross-cutting**, and **does not need to roll up to project insights**. The recommended label categories are listed below.

### Apply the right labels to a single issue

6. Open the issue. In the right sidebar, click **Labels**.
7. Apply one **area** label, for example `area:checkout` or `area:billing`. Use one area label per issue. If the work crosses two areas, that is a signal the issue should be split.
8. Apply one **component** label if the codebase has component subdivisions, for example `component:payments-api` or `component:web-client`. Skip if your project uses `Product Area` as a field instead.
9. Apply situational tags that engineering and triage actually filter on:
   - `regression` for a defect against previously working behavior.
   - `customer-reported` for issues filed from a support ticket or customer escalation.
   - `needs-repro` when triage cannot confirm reproduction steps.
   - `good-first-issue` for work suitable for new contributors.
10. Do not apply labels that duplicate the **Type**, `Status`, `Priority`, `Severity`, `Sprint`, `Release`, or `Milestone` already set on the issue. If a teammate added one, remove it during your next review.

> [SCREENSHOT: Issue right sidebar showing Labels selector with `area:checkout`, `component:payments-api`, and `customer-reported` applied alongside a separate Type field set to Bug.]

### Inspect a repository for label sprawl

11. Open the repository. Click the **Issues** tab, then the **Labels** button. The Labels page lists every label, its color, description, and the count of open and closed items using it.
12. Sort by **Most issues**. Scan the top of the list for labels that look like types, statuses, priorities, severities, sprints, releases, or milestones. These are sprawl candidates.
13. Sort by **Fewest issues**. Scan for labels with zero or one item. Most are abandoned experiments and should be deleted after confirmation.
14. Identify labels missing a **Description**. A label without a description is unsafe: triagers cannot tell what it means and will apply it inconsistently. Either write a one-line description or remove the label.
15. Build a short cleanup list. For each problem label, write down the verdict: keep, rename, merge, delete, or promote to issue type / field.

> [SCREENSHOT: Repository Labels page sorted by Most issues, with annotations marking three sprawl candidates: a `bug` default label duplicating the Bug type, a `priority:high` label duplicating the Priority field, and a `status:blocked` label duplicating Status.]

### Resolve sprawl

16. For labels that duplicate **Type** (`bug`, `feature`, `enhancement`, `epic`, `requirement`, `task`): confirm the **Type** is set on each issue, then delete the label. Do not perform deletion until issue type coverage is verified, because deletion removes the label from every linked item.
17. For labels that duplicate `Status`, `Priority`, `Severity`, `Sprint`, or `Release`: confirm the corresponding project field is populated on every issue using the label, then delete the label.
18. For labels that should have been issue types or project fields (a recurring `risk` label across many repositories, a `compliance` label that filters into governance reviews): escalate. Promotion to an organization issue type goes through GHE-ALM-023. Promotion to an organization issue field goes through GHE-ALM-024. Do not create the label in 12 more repositories as a workaround.
19. For labels that should be renamed for consistency (`Area-Checkout`, `area_checkout`, `checkout-area` all meaning the same thing): on the **Labels** page, click **Edit** next to each variant and rename to the canonical form (`area:checkout`). Renaming preserves history; deletion does not.
20. Record the cleanup decisions and date in the project's governance log. The next quarterly hygiene audit (GHE-ALM-078) will check this log.

## What Good Looks Like vs. What to Escalate

| Signal | What Good Looks Like | What to Escalate |
|---|---|---|
| Issue type vs. label | Every issue has **Type** set; labels never name a work item category. | Issues rely on a `bug` or `feature` label and **Type** is empty. Route to GHE-ALM-020 and GHE-ALM-023. |
| Status duplication | `Status` field drives the board; no `status:*` labels exist. | A `wip`, `blocked`, or `done` label is in active use. Cleanup belongs in this guide; recurring drift escalates to GHE-ALM-076. |
| Priority and severity | `Priority` and `Severity` fields are populated and filter cleanly. | `p0`, `priority:high`, `sev1` labels coexist with the fields. Cleanup belongs in this guide. |
| Label descriptions | Every label has a one-line description and a stable color. | More than 10 percent of labels have no description. Escalate to repository administrator for cleanup. |
| Label volume | Under 30 labels per repository, organized into clear families (`area:*`, `component:*`, plus a small set of situational tags). | Over 60 labels, duplicates, abandoned tags. Escalate to GHE-ALM-078. |
| Cross-repo consistency | Same label families across the repositories your team owns. | Each repository invents its own area scheme. Escalate to GHE-ALM-076. |
| Label as workflow | Labels used to record cross-cutting facts (regression, customer-reported, needs-repro). | Labels used as a homegrown approval workflow (`needs-pm-review`, `approved-for-sprint`). Escalate to engineering lead and reset the workflow in fields and Status. |

## Validation Checklist

- [ ] Every issue you reviewed has **Type** set, and no label duplicates that type.
- [ ] No active label duplicates `Status`, `Priority`, `Severity`, `Sprint`, or `Release`.
- [ ] Every kept label has a one-line description.
- [ ] `area:*` and `component:*` labels follow a single naming convention across the repository.
- [ ] Sprawl candidates have a written verdict: keep, rename, merge, delete, or promote.
- [ ] Promotion candidates have a request open against GHE-ALM-023 or GHE-ALM-024.

## Common Mistakes

- Treating labels as a shadow workflow because the project lacks a needed field. Add the field instead, or request it through GHE-ALM-024.
- Deleting a label before confirming **Type** or field coverage on every linked issue. Deletion is irreversible and strips the label from every item silently.
- Creating area or component labels inconsistently across repositories. `area:checkout` in one repo and `Checkout-Area` in another defeats cross-repository reporting.
- Using color to encode meaning the description does not state. Color is for scannability; meaning lives in the description.
- Letting bots create labels (`dependencies`, `automated-pr`, scanner labels) without governance. Review bot-created labels in the same cleanup pass.
- Inventing a `priority:*` label because the **Priority** field is hidden in a saved view. Fix the view, do not work around it.

## Escalation Path

- GitHub administrator: when sprawl crosses repositories or when a label looks like it should become an organization issue type or field.
- Repository administrator: when label deletion or renaming requires Write access you do not hold.
- Engineering lead: when labels encode an undocumented engineering workflow, for example a `needs-arch-review` label that gates merges.
- Release manager: when release-themed labels (`release-2026.05`) compete with the canonical `Release` field and milestones.

## Related Guides

- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-076 : How to Govern Project Fields and Labels
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
