# How to Run a Quarterly ALM Hygiene Audit

**Guide ID:** GHE-ALM-078
**Audience:** Engineering Manager, Program Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 4-6 hours per quarter, including findings write-up
**Required permissions:** Project: Admin on the audited Projects; Repository: Maintain on audited repositories; Organization: Member with read access to teams and rulesets
**Prerequisites:**

- A defined ALM standard for Project field set, label taxonomy, view layout, naming conventions, and ruleset coverage.
- An approved list of Projects, repositories, and teams in audit scope for the quarter.
- Access to the previous quarter's audit report, if one exists.

**When to use this guide:** Run this audit once per quarter to detect governance drift across Projects, repositories, fields, labels, teams, and rulesets, and to produce a remediation list with owners and deadlines.

**When not to use this guide:** Do not use this guide for incident response, sprint-level cleanup, or one-off field changes. Use the weekly dashboard review (GHE-ALM-054) for routine sprint hygiene.

## Outcome

By the end of this guide, you will have produced:

- An audit report listing each finding by category, severity, owner, and remediation deadline.
- An updated remediation tracker with issues filed for each finding requiring engineering or admin work.
- A signed-off snapshot of audit scope, methodology, and exceptions for the quarter.

## Before You Start

- Confirm the canonical field list (`Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`) and the approved label taxonomy.
- Pull the list of Projects in scope. For the worked example, audit `acme-payments`, `acme-platform`, and `acme-checkout` Projects.
- Set the audit window. Findings reference data as of the audit start date.
- Block 4-6 hours on your calendar. Schedule a 30-minute review with the engineering lead and release manager at the end.

## Steps

### Set up the audit workbook

1. Open the audit report template, or create a new one with these columns: **Category**, **Project / Repository**, **Finding**, **Severity (High / Medium / Low)**, **Owner**, **Remediation Deadline**, **Linked Issue**.
2. Record the audit window dates, the canonical field list, and the approved label taxonomy in the report header. This freezes the standard you are auditing against.

### Audit field usage drift

3. Open each Project in scope. Click **Settings**, then **Fields**.
4. Compare the Project's fields to the canonical list. Record any extra fields, missing fields, or renamed fields as findings.
5. In the Project's main table view, group by `Status`, then by `Priority`, then by `Sprint`. For each grouping, count items in the **No** *field* bucket. If more than 10 percent of open items lack `Priority`, `Owner`, `Sprint`, or `Target Date`, log a Medium finding. If more than 25 percent lack any of these, log a High finding.
6. Spot-check 10 random open issues per Project. Confirm `Severity` is set on bugs, `Effort` is set on sprint-committed work, and `Product Area` is set on every item. Record any single-select fields that contain free-text or one-off values not in the approved list.

> [SCREENSHOT: Project Settings > Fields panel showing the canonical field list]

### Audit label sprawl

7. In each repository in scope, open **Issues**, then click **Labels**.
8. Sort labels by **Most issues**. Identify labels with fewer than 5 issues attached, labels with similar names (`bug`, `Bug`, `bug-report`), and labels duplicating issue types or fields (`epic`, `feature`, `priority-high`).
9. Cross-reference against GHE-ALM-021. Any label that duplicates an issue type, a `Priority`, `Severity`, `Status`, or `Product Area` value is a label-sprawl finding. Log each one with the recommended action: delete, merge, or rename.
10. Compare the repository's label list to the approved taxonomy. Record labels that exist in only one repository but should be organization-wide, and the reverse.

### Audit stale items

11. In each Project, open the table view. Apply the filter `is:open updated:<2025-11-06`. Adjust the date so the filter selects items not updated in 180 days.
12. Group by `Status` and by `Owner`. Count stale items per owner. Log any owner with more than 10 stale items as a High finding. Log Projects where more than 5 percent of open items are stale as a Medium finding.
13. Sample 5 stale items per Project. Decide whether each should be closed, reassigned, or moved to a `Backlog (Cold)` status. Record the disposition decision in the report and create an issue per affected owner with the list of items to triage.

> [SCREENSHOT: Project filter showing `is:open updated:<DATE` with the stale results]

### Audit Project view drift

14. For each Project, open its saved views list. Compare against the standard view set: **Backlog**, **Current Sprint**, **Sprint Planning**, **Bug Triage**, **Release Roadmap**, **Hierarchy**.
15. For each standard view, confirm the layout, grouping, sorting, and filters match the approved configuration. Record views that have been renamed, deleted, or reconfigured.
16. Note any custom views that should be promoted to the standard set, and any custom views that duplicate standard views with minor variations.

### Audit permissions and team membership

17. Open the organization. Click **Teams**. For each team owning an audited repository, open its membership and child team list.
18. Compare against the approved team structure. Record any direct repository collaborators (users granted access outside a team) as a High finding. Direct collaborators bypass the team review routing intended by GHE-ALM-072.
19. For each repository in scope, open **Settings**, then **Collaborators and teams**. Record any team or user with `Admin` or `Maintain` access whose role exceeds what their function requires.
20. Spot-check 3 Projects. Open **Settings**, then **Manage access**. Record any users with `Admin` on a Project who are not Project owners or designated administrators.

### Audit ruleset and CODEOWNERS coverage

21. For each repository, open **Settings**, then **Rules**, then **Rulesets**. Confirm rulesets exist that target `main`, `release/*`, and `hotfix/*`. Cross-reference against GHE-ALM-074 to confirm the required rules are enabled (required PR, required reviews, required status checks, code-owner review where applicable).
22. Open the `CODEOWNERS` file at the repository root or in `.github/CODEOWNERS`. For each owner entry, confirm the team or user still exists and still owns that path. Record entries pointing at disbanded teams or departed users as a High finding.
23. Record repositories that lack any ruleset, lack a `CODEOWNERS` file on a path that requires code-owner review, or have rulesets in **Evaluate** mode that should be in **Active** mode.

### Compile findings, owners, and deadlines

24. For each finding, assign an owner: the Project admin for Project findings, the repository admin for ruleset and CODEOWNERS findings, the engineering manager for label and field findings, the team owner for permission findings.
25. Set remediation deadlines. High findings: 14 days. Medium findings: 30 days. Low findings: end of next quarter.
26. File one tracker issue per owner that bundles their findings. Link each tracker issue from the audit report. Use the title pattern `Hygiene Audit 2026-Q2: <owner team>`.
27. Send the audit report to the engineering lead and release manager. Schedule the 30-minute review. Record the review's exceptions, accepted risks, and revised deadlines back into the report.

> [SCREENSHOT: completed audit report header with scope, window, and finding counts]

## What Good Looks Like vs. What to Escalate

| Category | What Good Looks Like | What to Escalate |
|---|---|---|
| Field usage | Every open item has `Priority`, `Owner`, `Status`. Sprint-committed work has `Effort`, `Sprint`, `Target Date`. Bugs have `Severity`. | More than 25 percent of open items missing any required field. Free-text values in single-select fields. |
| Label sprawl | Labels match the approved taxonomy. No labels duplicate issue types or `Priority` / `Severity` values. | More than 20 percent of labels unused, duplicated, or overlapping with fields and issue types. |
| Stale items | Fewer than 5 percent of open items not updated in 180 days. Each stale item has a disposition decision. | A single owner with more than 10 stale items. Stale items in `In Progress` or `In Review` status. |
| Project view drift | All six standard views exist with approved layout and filters. Custom views are documented. | A standard view missing, renamed, or reconfigured. Critical filters such as `sprint:@current` removed. |
| Permissions | Access is granted via teams. Admin / Maintain roles match function. No direct repository collaborators. | Direct collaborators on protected repositories. Users with `Admin` on Projects without designation. |
| Ruleset coverage | `main`, `release/*`, `hotfix/*` covered with active rulesets. Required reviews and status checks enforced. | Any of these branches unprotected. Rulesets in **Evaluate** mode for a quarter or longer. |
| CODEOWNERS | File present where code-owner review is required. Every owner entry resolves to an active team or user. | Entries pointing at disbanded teams or departed users. Missing file on a repository with code-owner review required. |

## Validation Checklist

- [ ] Audit report header records scope, window, canonical field list, and approved label taxonomy.
- [ ] Every finding has a category, severity, owner, and remediation deadline.
- [ ] One tracker issue exists per owner, linked from the report.
- [ ] Engineering lead and release manager have signed off on exceptions.
- [ ] The previous quarter's High findings have either been closed or carry an explicit accepted-risk note.
- [ ] Audit report is filed in the governance repository for the current quarter.

## Common Mistakes

- Auditing against an undocumented standard. If the canonical field list and label taxonomy are not written down, the audit becomes opinion. Document the standard before auditing.
- Logging findings without owners. Findings without a named owner do not get fixed.
- Treating every drift as High severity. Reserve High for governance failures (unprotected `main`, direct collaborators on protected repos, more than 25 percent missing required fields). Most findings are Medium or Low.
- Skipping CODEOWNERS validation because the file looks present. Stale entries pointing at disbanded teams silently disable code-owner review.
- Closing the audit without a follow-up review at next quarter's start. Without follow-up, findings reopen.

## Escalation Path

- GitHub administrator: ruleset gaps spanning multiple repositories, organization-wide field or issue-type changes, direct collaborators that cannot be removed by repository admins.
- Repository administrator: per-repository ruleset and CODEOWNERS remediation, label cleanup, repository permission corrections.
- Engineering lead: disputed severity ratings, accepted-risk decisions, owner reassignment when a team has dissolved.
- Release manager: findings that affect `release/*` and `hotfix/*` branches or imminent release readiness.

## Related Guides

- GHE-ALM-021 : How to Use Labels Without Replacing Issue Types
- GHE-ALM-072 : How to Request Repository Access for Project Managers and Stakeholders
- GHE-ALM-074 : How to Review Ruleset and Branch Protection Coverage
- GHE-ALM-076 : How to Govern Project Fields and Labels
- GHE-ALM-077 : How to Enforce Naming Conventions
