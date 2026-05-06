# How to Name and Describe a GitHub ALM Project

**Guide ID:** GHE-ALM-007
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 20-minute one-time setup per Project
**Required permissions:** Project: Admin (to set the name, short description, and README)
**Prerequisites:**

- An organization-level GitHub Project shell already exists (see GHE-ALM-006).
- You know the product or program scope, the calendar year of coverage, and the owning team.
- You have a list of repositories the Project tracks, and the names of the accountable Product Owner and Engineering Manager.

**When to use this guide:** Use this when you create a new ALM Project, when you inherit a Project with a vague name or empty description, or during a quarterly governance review when names and READMEs no longer match what the Project tracks.

**When not to use this guide:** Do not use this for personal Projects or short-lived experiment Projects. Use it only for Projects that other teams, release managers, or auditors are expected to read.

## Outcome

By the end of this guide, you will have produced:

- A Project name that follows the `<Product or Program> ALM <Year>` pattern.
- A one-line short description that fits inside the Project header.
- A Project README that documents purpose, scope, owners, repositories included, decision authority, and a link to the relevant GHE-ALM guides.
- A Project icon and color that match the product area, where supported.

## Before You Start

- Confirm the canonical product or program name with your Product Owner. Do not invent abbreviations.
- Confirm the calendar year scope. ALM Projects normally cover one calendar year; multi-year Projects need a different naming variant.
- List the repositories in scope. Use `owner/repo` form, for example `acme-payments/checkout-service`.
- Identify the accountable Product Owner, Engineering Manager, and Release Manager by name and GitHub handle.
- Decide which decisions are made inside the Project (for example, sprint commitment) and which are escalated.

## Steps

### Set the Project name

1. Open the Project. From the Project header, click the project title to open the rename input.
2. Enter the name using the pattern `<Product or Program> ALM <Year>`. For example, `Edge Routing ALM 2026` or `acme-checkout ALM 2026`. Keep the name under 50 characters so it renders in the organization Projects list without truncation.
3. Press Enter to save. Confirm the new name appears in the browser tab title and in the organization's **Projects** list.

> [SCREENSHOT: Project header showing the renamed Project, with the title `acme-checkout ALM 2026` visible above the view tabs]

### Edit the short description

4. Click the **...** menu in the top-right of the Project, then click **Settings**.
5. In **Project details**, locate the **Short description** field. Enter one sentence that names the product, the year, and the work tracked. Example: `ALM tracking for the acme-checkout product across the 2026 release train, including features, requirements, bugs, and releases.`
6. Stay under 150 characters so the description renders in full on the Projects list and on linked dashboards.
7. Click **Save**.

> [SCREENSHOT: Project Settings showing the Short description field populated with the example sentence]

### Choose a Project icon and color

8. Still in **Settings**, find **Project icon** and **Project color**. Pick an icon that suggests the product area (for example, a credit card glyph for `acme-checkout`) and a color that does not collide with another active ALM Project in the same organization.
9. Save. The icon and color appear on the Projects list and in the left navigation, which helps managers identify the right Project at a glance during cross-team standups.

### Write the Project README

10. From Settings, open the **README** tab, or click **Add a README** from the Project header if no README exists.
11. Use this section structure. Keep each section short; the README is a quick reference, not a charter.

    - `## Purpose` : One paragraph stating the business outcome the Project tracks.
    - `## Scope` : Bullet list of what is in scope and a short list of what is out of scope.
    - `## Owners` : Product Owner, Engineering Manager, Release Manager, with `@handle` mentions.
    - `## Repositories` : Bullet list of `owner/repo` entries the Project covers.
    - `## Decision authority` : Who decides scope changes, sprint commitments, and release cuts inside this Project, and what escalates to a steering group.
    - `## Workflow guides` : Links to the GHE-ALM how-to guides the team follows. At minimum, link GHE-ALM-006 (create the Project), GHE-ALM-008 (add items), and GHE-ALM-077 (naming conventions).

12. Use Markdown headings, lists, and links. Avoid screenshots inside the README, since the Project UI changes more often than the README is reviewed.
13. Click **Save**.

> [SCREENSHOT: Rendered Project README showing the Purpose, Scope, Owners, Repositories, Decision authority, and Workflow guides sections]

### Verify the result

14. Return to the organization's **Projects** list. Confirm the Project appears with the new name, icon, color, and short description.
15. Open the Project in a private browser window (or ask a peer) to confirm the README is readable to someone with no prior context.

## Worked Example

Bad description, do not use:

> `Project for tracking stuff for checkout team.`

Why it fails: no product name, no year, no scope, no owners, no link to the workflow.

Good short description:

> `ALM tracking for the acme-checkout product across the 2026 release train, including features, requirements, bugs, and releases.`

Good README opening (Purpose and Scope sections only):

```text
## Purpose
Track all planned engineering work for the acme-checkout product through
the 2026 release train, from intake through release readiness.

## Scope
In scope:
- Features, requirements, tasks, and bugs for repositories listed below.
- Sprint planning and release commitment for the 2026 release train.

Out of scope:
- Production incident response (tracked in acme-payments/incident-log).
- Long-range roadmap items beyond 2026 (tracked in acme-checkout ALM 2027).
```

## Validation Checklist

- [ ] Project name matches `<Product or Program> ALM <Year>` and is under 50 characters.
- [ ] Short description is one sentence under 150 characters and names the product, year, and work tracked.
- [ ] README contains Purpose, Scope, Owners, Repositories, Decision authority, and Workflow guides sections.
- [ ] Owners are listed with GitHub `@handle` mentions, not just display names.
- [ ] Repositories are listed in `owner/repo` form.
- [ ] At least three GHE-ALM guides are linked from the README.
- [ ] Project icon and color do not duplicate another active ALM Project in the same organization.

## Common Mistakes

- Using a department name instead of a product name. The Project name should match what users build, not the team that builds it.
- Omitting the year. Without a year, retired Projects are hard to distinguish from active ones in the organization Projects list.
- Stuffing the short description with status updates. The short description is a static label, not a status board.
- Listing owners as raw text instead of `@handle`. Mentions surface the Project in the owner's notifications and make handoffs traceable.
- Linking to internal wiki pages instead of GHE-ALM guides. The GHE-ALM guides are the controlled source for ALM workflow.
- Renaming a Project without updating the README. Stale READMEs cause more confusion than missing ones.

## Escalation Path

- GitHub administrator: Involve when the organization needs a standard set of Project icons or colors reserved per product line.
- Repository administrator: Not applicable for naming and description; involve only if the README references repositories you cannot list because of access controls.
- Engineering lead: Involve when scope or decision authority is unclear, since these belong in the README and need a single accountable owner.
- Release manager: Involve to confirm the year and release train scope match the planned release calendar.

## Related Guides

- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-008 : How to Add Existing Issues and Pull Requests to a Project
- GHE-ALM-009 : How to Configure Auto-Add Workflows for Project Intake
- GHE-ALM-077 : How to Enforce Naming Conventions
- GHE-ALM-005 : How to Interpret GitHub Project Views
