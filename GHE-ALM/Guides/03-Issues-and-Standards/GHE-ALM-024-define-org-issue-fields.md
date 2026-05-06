# How to Define or Request Organization Issue Fields

**Guide ID:** GHE-ALM-024
**Audience:** Engineering Manager, Program Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-minute one-time request; 15 minutes per quarterly review
**Required permissions:** Organization: `Owner` to configure. Manager typically holds `Member` and submits the request.
**Prerequisites:**

- The organization exists and uses GitHub Issues for ALM tracking.
- The organization-level GitHub Project for ALM exists, or is planned (see GHE-ALM-006).
- Issue types are defined or requested (see GHE-ALM-023).

**When to use this guide:** Use this guide when you need standard issue fields available across every repository in the organization, so reporting, planning, and governance work consistently. Submit the request once during ALM rollout, then revisit during quarterly hygiene audits.

**When not to use this guide:** Do not use this guide for project-only fields that live on a single GitHub Project. Use GHE-ALM-076 to govern fields scoped to one Project. Do not use this guide for repository labels; labels are secondary classification only.

## Outcome

By the end of this guide, you will have produced:

- A request to the organization owner that lists each issue field, type, purpose, and option set.
- An accepted definition of the canonical 12 issue fields available across every repository in the organization.
- A review checklist for confirming the fields were configured as requested.

## Before You Start

- Confirm who the organization owner is. Issue fields are defined at the organization level and require the `Owner` role to configure.
- Decide which issue types each field will be pinned to. Each field must be pinned to at least one issue type, or to "Issues without a type", to appear in the issue sidebar.
- Note GitHub limits: 25 issue fields per organization, 50 options per single-select field, 10 pinned fields per issue type, and 50 total fields per project including system fields.
- Review the canonical field list below and adjust option sets to match your team's actual scales.

## Steps

### Specify the field set

1. Open a working document or ticket where you will draft the request. Title it "Issue fields request for `acme-payments` organization" using your real organization name.
2. List the 12 canonical fields with the field type, purpose, and option set. Use the table below as your starting point. Edit option sets only where your team's existing taxonomy demands it; keep the field names exactly as written.

| Field | Type | Purpose | Option set or format |
|---|---|---|---|
| `Status` | single-select | Current workflow state of the work item. | `Backlog`, `Ready`, `In Progress`, `In Review`, `Ready for QA`, `Done` |
| `Priority` | single-select | Business urgency of fixing or delivering the item. | `P0`, `P1`, `P2`, `P3` (illustrative; confirm with engineering and QA leadership) |
| `Severity` | single-select | Technical or user impact, separate from urgency. | `1`, `2`, `3`, `4` (illustrative 1-4 scale; confirm with QA leadership) |
| `Effort` | single-select | Relative size for sprint capacity discussion. | `XS`, `S`, `M`, `L`, `XL` or numeric story points if your team prefers |
| `Sprint` | iteration (project field) | Iteration the item is committed to. Configured as a project iteration field; see GHE-ALM-027. | Iteration cadence, e.g., 2-week sprints |
| `Release` | single-select | Target release train across repositories. | `2026.05.0`, `2026-Q3 Release`, `Backlog` |
| `Product Area` | single-select | Functional area of the product the item belongs to. | `Checkout`, `Billing`, `Identity`, etc. |
| `Owner` | single-line-text | Accountable manager or product owner when the GitHub assignee is an engineer. | Free text or GitHub handle |
| `Start Date` | date | Planned start for roadmap and timeline views. | Calendar date |
| `Target Date` | date | Planned completion for roadmap and milestone tracking. | Calendar date |
| `Risk Level` | single-select | Likelihood and impact rating for risk and change-request items. | `Low`, `Medium`, `High`, `Critical` |
| `Customer Impact` | single-select | Whether the issue affects external customers. | `None`, `Internal Only`, `Limited Customers`, `Broad Customer Impact` |

3. For each field, list which issue types it should be pinned to (`Epic`, `Feature`, `Requirement`, `Task`, `Bug`, `Risk`, `Change Request`, or "Issues without a type"). Owners can pin up to 10 fields per issue type; prioritize ruthlessly.
4. Note that `Priority`, `Effort`, `Start date`, and `Target date` exist as default fields in every organization. Ask the owner to rename or extend the option sets rather than create duplicates.
5. Note that `Sprint` is a project iteration field, not an organization issue field. Submit GHE-ALM-027 separately for the iteration field on the ALM Project.

### Send the request

6. Identify the organization owner from the organization member list or your administrator contact.
7. Send the request using the template in the Sample Request to Send section. Include the field table verbatim.
8. Track the request in your normal change-control system. Issue fields apply organization-wide and affect every repository, so treat the change as governance work, not a casual ask.

### Review the configured fields

9. After the owner reports completion, navigate to the organization page, click **Settings**, then open **Planning**, then **Issue fields**.
10. Confirm each field appears with the correct name, type, and pinning. Click any field to inspect its option set.

> [SCREENSHOT: Organization Settings, Planning section, Issue fields page showing the 12 canonical fields]

11. Open one issue of each issue type in a representative repository. Confirm the pinned fields appear in the right sidebar of the issue and that values can be set.
12. Compare what you see against the What Good Looks Like vs. What to Escalate table.

## Sample Request to Send

```
To: <organization owner>
Cc: <engineering manager>, <release manager>, <QA manager>
Subject: Request: Configure organization issue fields for acme-payments

We are standardizing ALM tracking on GitHub Enterprise. Please configure the
following 12 issue fields at the organization level for acme-payments. The
goal is consistent metadata across every repository so reporting, sprint
planning, and release tracking work without per-repo custom fields.

Field set: see attached table. Field names must match exactly. Option sets
are starting points; we have validated them with engineering and QA
leadership.

Notes:
- Priority, Effort, Start Date, and Target Date already exist as defaults.
  Please rename and extend their option sets rather than create duplicates.
- Sprint is a Project iteration field, not an organization issue field. We
  will submit that request separately against the org-level ALM Project.
- Pinning: see the pinning column in the attached table. Each field is
  pinned to specific issue types so the issue sidebar stays usable.

Validation: once configured, please reply with a screenshot of
Settings > Planning > Issue fields. We will run the review checklist in
GHE-ALM-024 and confirm closure.

Requested by: <name, role>
Target completion: <date>
```

## Validation Checklist

- [ ] All 12 canonical fields are present in **Settings** > **Planning** > **Issue fields** at the organization level.
- [ ] Field names match exactly: `Status`, `Priority`, `Severity`, `Effort`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`. (`Sprint` is the iteration field on the ALM Project, not here.)
- [ ] Field types match the request: single-select for taxonomies, date for `Start Date` and `Target Date`, single-line-text for `Owner`.
- [ ] Each field is pinned to the correct issue types so it appears in the issue sidebar.
- [ ] Default fields (`Priority`, `Effort`, `Start date`, `Target date`) were renamed or extended, not duplicated.
- [ ] Option sets reflect your team's actual scales for `Priority`, `Severity`, and `Effort`.
- [ ] An issue of each type in a representative repository shows the pinned fields and accepts values.

## What Good Looks Like vs. What to Escalate

| Aspect | What Good Looks Like | What to Escalate |
|---|---|---|
| Field names | Exact match to canonical list. | Renamed, abbreviated, or pluralized differently across repos. |
| Field types | Single-select for taxonomies, date for dates, text for free-form. | A taxonomy field configured as text, blocking grouping and filtering. |
| Option sets | Match the agreed scale; no duplicate or near-duplicate options. | Stale options, ad-hoc additions, or 30+ options on one field. |
| Pinning | Field visible in the sidebar of the right issue types. | Field invisible because it was created but never pinned. |
| Defaults handling | Defaults renamed or extended in place. | Duplicate `Priority` or `Effort` field consuming one of the 25 slots. |
| Coverage | Same field set visible in every repository's issues. | One repository missing the fields, suggesting the change was scoped wrong. |
| Limits headroom | Well below 25 fields and 50 options per field. | Approaching limits, suggesting field sprawl that needs cleanup under GHE-ALM-076. |

## Common Mistakes

- Treating issue fields and project fields as interchangeable. Issue fields are organization-wide and live on every issue. Project fields exist only on a single GitHub Project. `Sprint` is a project iteration field, not an issue field.
- Creating a new `Priority` or `Effort` field instead of editing the defaults, which wastes two of the 25 available field slots.
- Defining fields without pinning them to issue types, which leaves them invisible in the issue sidebar even though the data is there.
- Approving option sets that drift from the QA team's actual severity scale, which silently corrupts release-readiness reporting.
- Letting individual repositories add per-repo labels that duplicate organization fields. Use GHE-ALM-021 to govern label use.

## Escalation Path

- GitHub administrator: when the organization owner is unavailable or when changes need enterprise-level coordination across multiple organizations.
- Repository administrator: not applicable. Issue fields are organization-scoped.
- Engineering lead: when option sets for `Severity`, `Priority`, or `Effort` need engineering or QA leadership sign-off before submission.
- Release manager: when the `Release` option set needs alignment with the active release train calendar.

## Related Guides

- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-027 : How to Configure or Request a Sprint Iteration Field
- GHE-ALM-076 : How to Govern Project Fields and Labels
