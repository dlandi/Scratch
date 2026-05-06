# How to Create or Request Issue Forms

**Guide ID:** GHE-ALM-025
**Audience:** Project Manager, Engineering Manager, QA Manager
**Primary role:** Engineering Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30 to 60 minutes to draft a request; 1 to 2 hours for the repository administrator to implement and verify
**Required permissions:** None to draft and request the forms. Repository `Write` or `Admin` is required to commit the YAML files to `.github/ISSUE_TEMPLATE/`. Organization `Owner` is needed if the linked issue types or fields do not yet exist (see GHE-ALM-023 and GHE-ALM-024).
**Prerequisites:**

- Organization issue types are defined or scheduled (Feature, Requirement, Bug, Task, Risk, Change Request). See GHE-ALM-023.
- Organization issue fields exist for `Priority`, `Severity`, `Effort`, `Release`, `Product Area`, `Start Date`, `Target Date`. See GHE-ALM-024.
- Target repository is identified (the one where managers and contributors will file work).
- Naming standard for forms is agreed (lowercase, hyphenated, e.g., `feature-request.yml`).

**When to use this guide:** When every Feature Request, Requirement, Bug Report, Task, and Change Request must arrive with the same structured fields, or when free-text issues are producing inconsistent intake.

**When not to use this guide:** To define what fields should exist at the organization level (use GHE-ALM-024) or to file a single issue (use GHE-ALM-011, GHE-ALM-012, or GHE-ALM-014).

## Outcome

By the end of this guide, you will have produced:

- A written request to the repository administrator naming the five forms to create and the required fields per form.
- A review record confirming that each form renders correctly, populates the right issue type, and writes to the right project fields.
- A short escalation note for any field that cannot be wired because the underlying organization issue type or field is missing.

## Before You Start

- Confirm the repository (e.g., `acme-checkout/checkout-service`), or whether forms should live in a shared `.github` repository at the organization level.
- Confirm the team's Severity and Priority scale with QA leadership.
- List any team-specific fields the organization has not yet created. Those go through GHE-ALM-024.
- Decide whether blank issues should be disabled in the chooser. Most ALM repositories disable them.

## Steps

### Specify the five canonical forms

1. Confirm the five forms in scope: Feature Request, Requirement, Bug Report, Task, Change Request. Each maps to a single organization issue type.
2. For each form, list the required fields, field types (`input`, `textarea`, `dropdown`, `checkboxes`, `markdown`, file upload), and which are mandatory. The next phase is the contract.
3. Decide which project fields each form should populate (for example, `Priority`, `Severity`, `Product Area`, `Release`, `Sprint`, `Effort`, `Owner`).
4. Decide which labels each form should auto-apply (for example, `needs-triage` on Bug Report, `intake` on Feature Request).

### Define required fields per form

5. Use the lists below as the request contract. Mandatory fields are marked with an asterisk; managers must not accept a delivered form that omits any starred field. Severity 1-4 and Priority P0-P3 are illustrative; confirm the actual scale with QA leadership.

**Feature Request** (issue type: Feature)

- Title* (input), Problem statement* (textarea), Proposed outcome* (textarea), Product Area* (dropdown), Priority* (dropdown), Target Release (input, maps to `Release`), Acceptance criteria* (textarea, bulleted and testable), Stakeholders (input, GitHub handles).

**Requirement** (issue type: Requirement)

- Title* (input, one requirement per issue), Source* (dropdown: customer, regulatory, internal, contractual), Requirement statement* (textarea), Parent feature or epic* (input, `owner/repo#NNN`), Product Area* (dropdown), Priority* (dropdown), Acceptance criteria* (textarea, each criterion independently verifiable), Verification method* (dropdown: test, inspection, demo, analysis).

**Bug Report** (issue type: Bug)

- Title* (input, symptom-led), Product / Component* (dropdown), Affected version* (input), Environment* (dropdown: Dev, QA, Staging, UAT, Production), Severity* (dropdown), Priority* (dropdown), Steps to reproduce* (textarea, numbered), Expected behavior* (textarea), Actual behavior* (textarea), Logs or screenshots* (textarea with drag-and-drop file upload, GA March 2026), Regression* (dropdown: Yes / No / Unknown), Workaround (textarea), Customer impact* (textarea), Target release (input, maps to `Release`).

**Task** (issue type: Task)

- Title* (input, verb-led), Parent issue* (input, `owner/repo#NNN`), Description* (textarea), Effort* (dropdown), Owner (input), Sprint (input, maps to `Sprint`).

**Change Request** (issue type: Change Request)

- Title* (input), Change type* (dropdown: scope add, scope remove, date change, owner change, other), Affected work item* (input, `owner/repo#NNN`), Reason* (textarea: customer, contractual, regulatory, technical, capacity), Impact assessment* (textarea: schedule, scope, cost, risk), Requested decision date* (input, ISO date), Approver* (input, release manager or product owner handle), Risk Level* (dropdown: Low, Medium, High, Critical).

### Write the request

6. Combine the form list, field requirements, project field mappings, labels, and chooser configuration into a single request to the repository administrator. Use the **Sample Request to Send** below.
7. Send the request via your normal channel for repository changes. Include a target date.

### Review what comes back

8. When the administrator reports the forms are live, open the repository, click **Issues**, then **New issue**. Confirm all five forms appear in the chooser and that blank issues are disabled if requested.
9. Open each form. Confirm every starred field is present and marked required, dropdowns contain the agreed values, and Bug Report accepts file uploads.
10. File one test issue per form. Confirm each lands with the correct issue type, labels, and project field values. Close the test issues with a comment noting they were validation runs.
11. Use the next section to score each form as ready or escalate.

> [SCREENSHOT: Issue chooser screen showing all five forms and blank-issue option suppressed, with one form expanded to show required-field markers.]

## What Good Looks Like vs. What to Escalate

| Aspect | What good looks like | What to escalate |
|---|---|---|
| Form availability | All five forms appear in the chooser; blank issue option is suppressed if requested. | Any of the five forms missing, or blank issues still permitted. |
| Required fields | Every starred field in the tables above is present and enforced. | Any starred field missing, optional, or renamed. |
| Issue type wiring | Each form sets the correct organization issue type on creation. | Issue type is unset, wrong, or defaults to a generic value. |
| Project field population | Forms populate `Priority`, `Severity`, `Product Area`, `Release`, `Sprint`, `Effort` where applicable. | Form fields collect values but do not flow to project fields, leaving project views empty. |
| Label routing | `needs-triage` is applied to Bug Report and `intake` to Feature Request, per request. | Bugs land without `needs-triage`, breaking the bug triage view (GHE-ALM-034). |
| File uploads | Bug Report accepts logs and screenshots via drag-and-drop. | Upload rejected, attachments stripped, or only image types allowed. |
| Naming and chooser order | Form names are clear, sentence case, and ordered Feature, Requirement, Bug, Task, Change Request. | Cryptic filenames or confusing order in the chooser. |
| Cross-repo consistency | The same five forms exist in every repository in scope, with identical required fields. | Form drift between repositories breaks roll-up reporting. |

## Sample Request to Send

> Subject: Request to add five issue forms to `<owner/repo>`
>
> Please add the following issue forms under `.github/ISSUE_TEMPLATE/` in `<owner/repo>`:
>
> 1. `feature-request.yml` (issue type: Feature)
> 2. `requirement.yml` (issue type: Requirement)
> 3. `bug-report.yml` (issue type: Bug)
> 4. `task.yml` (issue type: Task)
> 5. `change-request.yml` (issue type: Change Request)
>
> Required fields per form are listed in GHE-ALM-025, "Define required fields per form." Mandatory fields must be enforced as required in the form schema.
>
> Each form should:
>
> - Set the matching organization issue type on creation.
> - Populate project fields `Priority`, `Severity`, `Product Area`, `Release`, `Sprint`, `Effort`, `Owner` from the form values where the form collects them.
> - Apply labels: `needs-triage` for Bug Report, `intake` for Feature Request.
> - Allow file uploads on Bug Report (logs, screenshots).
>
> Also add `.github/ISSUE_TEMPLATE/config.yml` with `blank_issues_enabled: false`, and contact links for security reports and customer support so those routes do not file issues.
>
> Target date: `<YYYY-MM-DD>`. Please reply on this thread when forms are live; I will validate per GHE-ALM-025 and confirm.

## Validation Checklist

- [ ] All five forms appear in the **New issue** chooser in the target repository.
- [ ] Blank issues are disabled if that was requested.
- [ ] Every starred field in the tables above is present and enforced as required.
- [ ] Each form sets the correct organization issue type on creation.
- [ ] Project fields are populated by form values where applicable.
- [ ] Bug Report accepts file uploads.
- [ ] One test issue per form was filed and lands in the expected project view.
- [ ] Test issues were closed with a comment noting they were validation runs.

## Common Mistakes

- Requesting forms before the underlying organization issue types and fields exist. The form will save free text but fail to populate project fields, breaking dashboards.
- Treating labels as a substitute for fields. Severity and Priority must be project fields, not labels. See GHE-ALM-021.
- Allowing each repository to invent its own field names. Stick to the canonical names: `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`.
- Skipping the `config.yml` file. Without it, contributors can still file blank issues that bypass the forms.
- Letting cryptic filenames into the chooser. The chooser shows the `name:` value to users; keep it human-readable.
- Forgetting cross-repository consistency. The same five forms must live in every repository in the ALM scope, otherwise insights and roadmap views diverge.

## Escalation Path

- GitHub administrator: When organization issue types or fields are missing and must be created before forms can wire to them.
- Repository administrator: For all changes inside `.github/ISSUE_TEMPLATE/`, including form creation, edits, and the chooser config file.
- Engineering lead: When the requested fields conflict with engineering tooling or with an existing intake automation.
- Release manager: When Change Request fields, approvers, or risk levels need adjustment to match the release governance model.

## Related Guides

- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-012 : How to Create a Requirement Issue
- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-024 : How to Define or Request Organization Issue Fields
