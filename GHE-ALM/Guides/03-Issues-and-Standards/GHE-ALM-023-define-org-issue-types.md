# How to Define or Request Organization Issue Types

**Guide ID:** GHE-ALM-023
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Program Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-minute one-time request, 15-minute periodic review
**Required permissions:** Organization: `Owner` to configure. `Member` is sufficient to request and to review the resulting taxonomy.
**Prerequisites:**

- An organization where ALM work will be tracked, for example `acme-payments`.
- Agreement with engineering and QA leadership on the work item taxonomy your program needs.
- Read access to existing issues so you can confirm which types are currently in use.

**When to use this guide:** Use this guide when a new organization is being stood up for ALM, when the current type list is missing categories your program needs (Epic, Requirement, Risk, Change Request), or when you want to confirm the type list is still correct during a quarterly hygiene review.

**When not to use this guide:** Do not use this guide for repository-level labels or for project field design. Labels are covered in GHE-ALM-021. Project fields such as `Priority` or `Severity` are covered in GHE-ALM-024.

## Outcome

By the end of this guide, you will have produced:

- A written request to your organization owner specifying which issue types to add, edit, disable, or delete, with a description and color for each new type.
- A reviewed taxonomy where the visible issue types in **Settings** > **Planning** > **Issue types** match the canonical ALM list.

## Before You Start

- Confirm who the organization owner is. Only an organization owner can change issue types.
- Pull the current list of types from **Settings** > **Planning** > **Issue types** so your request explicitly says what to keep, edit, or remove.
- Decide on the canonical type list with engineering and QA leadership before sending the request. Changing the list repeatedly creates churn in reports and dashboards.

## Steps

### Confirm the canonical type list for your program

1. Open the canonical ALM type list and confirm each entry against your program's actual work intake. The recommended list is: `Initiative`, `Epic`, `Feature`, `Requirement`, `Story` (optional), `Task`, `Bug`, `Risk`, `Change Request`.
2. Decide whether `Story` belongs in your taxonomy. Teams that decompose a Feature directly into Tasks should leave `Story` out. Teams that need a user-facing increment between Feature and Task should include it.
3. Note that GitHub's three default types are `Task`, `Bug`, and `Feature`. Everything else on the canonical list must be added.
4. Decide a one-line description for each type so future contributors apply them consistently. Sample descriptions appear in the request template below.
5. Decide a color for each type. Use distinct colors for high-frequency types (`Bug`, `Task`, `Feature`) and reserve a strong contrast color for governance types (`Risk`, `Change Request`).

### Inspect the current taxonomy before requesting changes

6. As an organization member, open the organization, click **Settings**. If **Settings** is not visible, open the **More** menu first.
7. In the left navigation, expand **Planning** and click **Issue types**.
8. Read each row. Record name, description, color, and enabled state. This is the baseline you will compare your canonical list against.

> [SCREENSHOT: Organization Settings, Planning, Issue types page showing the current list with default Task, Bug, Feature]

### Build and send the request

9. Use the request template in the **Sample Request to Send** section below. Fill in your organization name, the canonical list, the changes you want (Add, Edit, Disable, Delete), and a target date.
10. Send the request to the organization owner. Cc the engineering manager and QA lead so the type list is not changed without their visibility.
11. After the owner confirms changes, return to **Settings** > **Planning** > **Issue types** and verify each requested type is present, has the agreed description and color, and is enabled.

### Review and maintain the taxonomy

12. During quarterly hygiene work (see GHE-ALM-078), reopen the issue types page and apply the **What Good Looks Like vs. What to Escalate** table below.
13. If a type is unused for a full quarter, raise the question in the next governance review whether to disable it. Do not request immediate deletion; disabling preserves history.
14. Treat the issue type list as a versioned governance artifact. Record the canonical list and any changes in the same place you keep field governance notes (see GHE-ALM-076).

## What Good Looks Like vs. What to Escalate

| Aspect | What Good Looks Like | What to Escalate |
|---|---|---|
| Coverage | All canonical types exist: `Initiative`, `Epic`, `Feature`, `Requirement`, `Task`, `Bug`, `Risk`, `Change Request`. | One or more canonical types missing, especially `Risk` or `Change Request`. |
| Naming | Each type uses the exact agreed name with consistent capitalization. | Variants such as `epic`, `EPIC`, or `Risk Item` instead of `Risk`. |
| Description | Every type has a one-line description that distinguishes it from adjacent types. | Empty descriptions or descriptions copied across types. |
| Color | Each type has a distinct color. Governance types stand out from execution types. | Two types sharing a color, or all types using the default color. |
| Usage | Default types `Task`, `Bug`, `Feature` are used as defined; custom types appear on real issues. | Custom types created but never applied; managers using labels to substitute for missing types. |
| Limit headroom | Active list is well under the 25-type maximum. | The list is approaching 25 active types, indicating sprawl. |
| Disabled vs. deleted | Retired types are disabled rather than deleted, preserving historical references. | Types deleted while issues still reference them, breaking reporting continuity. |

## Sample Request to Send

Send a message such as the following to the organization owner.

> Subject: Request to configure organization issue types for `acme-payments`
>
> Hello,
>
> We are aligning the `acme-payments` organization to the GHE-ALM standard issue type taxonomy. As organization owner, you are the only role that can edit this list. Please apply the changes below at **Settings** > **Planning** > **Issue types** in the `acme-payments` organization.
>
> Canonical list to support after this change:
>
> - `Initiative` : Multi-release program-level deliverable that groups Epics across products.
> - `Epic` : Major capability delivered across one or more releases. Parent of Features.
> - `Feature` : User-visible capability delivered within a release. Parent of Requirements or Tasks.
> - `Requirement` : Formal requirement with acceptance criteria. Implementable within a sprint.
> - `Task` : Implementation or non-code work item, usually a child of a Feature or Requirement.
> - `Bug` : Defect against released or in-progress functionality.
> - `Risk` : Identified delivery, quality, security, or compliance risk requiring tracking.
> - `Change Request` : Controlled scope change against an in-flight Feature or release.
>
> Requested changes:
>
> - Keep enabled: `Task`, `Bug`, `Feature` (defaults).
> - Add: `Initiative`, `Epic`, `Requirement`, `Risk`, `Change Request`. Description and color for each shown above; please pick distinct colors and confirm before saving.
> - Edit: none at this time.
> - Disable: none at this time.
> - Delete: none. We prefer disabling over deleting to preserve historical references.
>
> Optional: We are not requesting `Story` at this time. We will revisit if a team adopts a Feature-to-Story-to-Task breakdown.
>
> Target date: <date>. Please reply once changes are saved so we can verify them.
>
> Thank you.

> [SCREENSHOT: Create new type dialog with name, description, and color fields populated for Requirement]

## Validation Checklist

- [ ] Every canonical type appears in **Settings** > **Planning** > **Issue types** for the target organization.
- [ ] Each type has the agreed name, description, and color.
- [ ] Default types `Task`, `Bug`, and `Feature` remain enabled.
- [ ] Custom types `Initiative`, `Epic`, `Requirement`, `Risk`, and `Change Request` are enabled.
- [ ] No type appears twice under different capitalizations.
- [ ] The active type count is below the 25-type platform limit, with headroom.
- [ ] The organization owner has confirmed the changes by reply.

## Common Mistakes

- Treating issue types as labels. Labels are repository-scoped secondary classification and do not appear in the issue type filter; see GHE-ALM-021.
- Asking for type deletion instead of disabling. Deletion is permanent and breaks historical reporting.
- Adding overlapping types such as both `Defect` and `Bug`. Pick one and stick to it.
- Letting each team request its own custom types. Type sprawl approaching the 25-type cap forces a painful cleanup.
- Forgetting to send the agreed description and color with the request. The owner then has to invent them, and consistency suffers.
- Confusing organization issue types (this guide) with organization issue fields such as `Priority` and `Severity`, which are covered in GHE-ALM-024.

## Escalation Path

- GitHub administrator: Involve when the organization owner cannot be identified, or when an enterprise-level policy may affect what types are permitted.
- Repository administrator: Not applicable. Issue types are organization-scoped.
- Engineering lead: Involve before sending the request to confirm the canonical list reflects how engineering decomposes work.
- Release manager: Involve before sending the request to confirm `Risk` and `Change Request` definitions match release governance practice.

## Related Guides

- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-016 : How to Create a Risk or Change Request Issue
- GHE-ALM-024 : How to Define or Request Organization Issue Fields
- GHE-ALM-025 : How to Create or Request Issue Forms
- GHE-ALM-076 : How to Govern Project Fields and Labels
