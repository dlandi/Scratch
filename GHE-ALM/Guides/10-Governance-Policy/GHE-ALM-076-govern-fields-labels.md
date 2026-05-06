# How to Govern Project Fields and Labels

**Guide ID:** GHE-ALM-076
**Audience:** Engineering Manager, Program Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 60-minute one-time setup, then 30 minutes per quarterly review
**Required permissions:** Project: Admin (to add, rename, or delete fields); Repository: Write (to create, edit, or delete labels); Repository: Triage (to apply labels)
**Prerequisites:**

- Organization Project already exists for the product or release train.
- Canonical field list agreed with QA, product management, and release management.
- Designated owner identified for each field and label family.

**When to use this guide:** Use when standing up governance for a new ALM Project, when onboarding a second team into an existing Project, or during a quarterly hygiene cycle when field or label sprawl is suspected.

**When not to use this guide:** Do not use this guide to define organization-level issue types or organization-level issue fields. Those are governed in GHE-ALM-023 and GHE-ALM-024 and require organization owner permissions.

## Outcome

By the end of this guide, you will have produced:

- A field ownership table mapping each canonical Project field to a single accountable owner.
- A label naming standard with families, prefixes, and a do-not-use list.
- A change-control note documenting how fields and labels are added, renamed, or retired.
- A quarterly review entry recording the date, the items inspected, and any corrections applied.

## Before You Start

- The current list of fields on the Project (open the Project, then the **Settings** menu).
- The current label list for each repository feeding the Project (open the repository, then **Issues**, then **Labels**).
- The illustrative canonical field set: `Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`.
- Project: Admin permission for field changes. Repository: Write permission for label changes.

## Steps

### Establish field ownership

1. Open the Project. Click the three-dot **More options** menu in the top right, then click **Settings** to view the field list.
2. For each field on the Project, record the canonical name, the field type (single-select, iteration, date, number, text), and the team function that should own changes to it. Use the ownership table below as the starting point for `acme-payments` style products.

   | Field | Owner role | What the owner decides |
   |---|---|---|
   | Issue Type | Engineering process owner | Whether new issue types are added (Epic, Requirement, Risk, Change Request). |
   | Priority | Product management | Scale definitions and which work is P0 / P1 / P2 / P3. |
   | Severity | Engineering Manager or QA Manager | Technical impact scale and bug severity definitions. |
   | Sprint | Scrum Master or Engineering Manager | Iteration cadence, sprint length, breaks, and naming. |
   | Release | Release Manager | Active release labels, retirement of past releases. |
   | Product Area | Engineering Manager or architecture lead | Domain decomposition and area names. |
   | Risk Level | Engineering Manager or Release Manager | Risk scale and escalation thresholds. |

3. Save the ownership table in the Project description or in a `GOVERNANCE.md` file in the primary repository so reviewers can see it. Reference this table in every later field discussion.

> [SCREENSHOT: Project Settings panel showing the field list with field types beside each name]

### Define the label naming standard

4. Open the primary repository. Click **Issues**, then **Labels** above the issue list. Review the current labels.
5. Group labels into families using a `family:value` prefix pattern. Common families: `area:checkout`, `area:billing`, `type:tech-debt`, `needs:design`, `release:2026.05.0`, `risk:regulatory`. Avoid using a label where a Project field already exists. For example, do not create a `priority:p1` label when the `Priority` field already carries that data; see GHE-ALM-021.
6. Document a do-not-use list of label patterns the team has retired or banned, such as raw color labels, individual-name labels, or duplicate `bug` variants.
7. Apply the same label standard to every repository that feeds the Project. Drift between repositories is the most common source of broken cross-repository reporting.

> [SCREENSHOT: Repository Labels page showing prefixed labels grouped by family]

### Set up the change-control process

8. Document a one-page change-control note. State that any new field or new label family requires a written request to the field or label owner, a stated reason, and a target retirement date if the addition is temporary.
9. State the rename rule: rename only via the Project **Settings** for fields, or via the **Edit** action on the Labels page for labels. Renaming preserves history. Deleting and recreating does not.
10. State the retirement rule: a field that has not been set on any item in 90 days is a candidate for retirement; a label not applied to any open issue in 90 days is a candidate for retirement. The field or label owner makes the final call.

### Inspect for sprawl

11. In the Project, group the table view by each candidate field. A field that is empty on more than 30 percent of items is either misunderstood or unowned. Decide whether to make it required at intake (via issue forms, GHE-ALM-025) or to retire it.
12. On the Labels page, sort by issue count. Labels with single-digit usage and no recent activity are sprawl candidates. Labels with hundreds of issues but no documented owner are governance gaps.
13. Compare the active label list across repositories. A label that exists in three repositories but not the fourth is drift.

### Run the quarterly review

14. Schedule a 30-minute quarterly review with the field and label owners. The cadence aligns with the broader hygiene audit in GHE-ALM-078.
15. At each review, walk the field ownership table, the label families, the change-control note, and the sprawl evidence from steps 11 to 13. Record decisions: keep, rename, retire, or escalate.
16. Apply the decisions immediately while owners are in the room. Renames are safe; deletions of fields with historical data require a written sign-off from the owner.

> [SCREENSHOT: Quarterly review entry with date, items inspected, decisions, and follow-up owners]

## What Good Looks Like vs. What to Escalate

| Signal | What good looks like | What to escalate |
|---|---|---|
| Field ownership | Every field has a named owner role visible in the Project description. | Two or more owners claim the same field, or no owner can be identified. |
| Field completeness | More than 80 percent of items have the field populated where it applies. | A field is empty on most items, indicating it is misunderstood or unowned. |
| Label family discipline | Labels follow the `family:value` pattern with a documented list. | Free-form labels without family prefixes appear in more than one repository. |
| Cross-repository alignment | All feeder repositories carry the same active label families. | A label exists in some repositories but not others, breaking cross-repo reporting. |
| Field-versus-label boundary | Priority, Severity, Sprint, Release, and Product Area live in fields, not labels. | A label such as `priority:p1` competes with the `Priority` field. |
| Change control | Adds, renames, and retirements have a written record with a date and an owner. | Fields or labels appear or disappear without notice, breaking saved views. |
| Quarterly review cadence | A review entry exists for the last quarter with decisions recorded. | No review has occurred in two or more quarters. |

## Validation Checklist

- [ ] Field ownership table is published in the Project description or a `GOVERNANCE.md` file.
- [ ] Every canonical field has a single named owner role.
- [ ] Label naming standard with family prefixes is documented and applied across feeder repositories.
- [ ] Do-not-use label list is recorded.
- [ ] Change-control note covers add, rename, and retire rules.
- [ ] Most recent quarterly review entry is dated within the last 95 days.
- [ ] No label duplicates a Project field already in use.

## Common Mistakes

- Using a label as a substitute for a Project field. Reporting then has to merge two sources for the same concept.
- Renaming a field by deleting it and recreating it. History on existing items is lost.
- Letting each repository invent its own label set. Cross-repository views become unreliable.
- Allowing every team member to add new fields. Within a quarter the Project carries duplicate or near-duplicate fields.
- Setting field ownership to a person rather than a role. The owner leaves and the governance lapses.
- Skipping the quarterly review. Sprawl is invisible until reporting fails.

## Escalation Path

- GitHub administrator: when a label or field is locked at the organization level and you need an organization-scope change.
- Repository administrator: when label changes require write access you do not hold, or when you need a bulk rename across many repositories.
- Engineering lead: when ownership of a field or label family is contested between teams.
- Release manager: when changes to the `Release` field, release labels, or related fields would affect an active release train.

## Related Guides

- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-021 : How to Use Labels Without Replacing Issue Types
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-024 : How to Define or Request Organization Issue Fields
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
