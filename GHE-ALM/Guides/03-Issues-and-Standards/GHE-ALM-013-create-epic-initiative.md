# How to Create an Epic or Initiative Issue

**Guide ID:** GHE-ALM-013
**Audience:** Project Manager, Program Manager, Product Owner
**Primary role:** Program Manager
**Classification:** Manager Performs
**Estimated time:** 20-30 minutes per parent issue
**Required permissions:** Repository: Triage (to create issues and assign type); Project: Write (to set Project fields)
**Prerequisites:**

- Organization issue types `Epic` and `Initiative` already exist (see GHE-ALM-023).
- Target repository for the parent issue is identified.
- Organization-level GitHub Project is set up with `Status`, `Priority`, `Owner`, `Product Area`, `Release`, `Start Date`, and `Target Date` fields.
- A clear strategic intent or release theme that justifies a parent work item.

**When to use this guide:** Use when a body of work is too large for a single Feature, spans multiple sprints or releases, or needs a single anchor that holds Features, Requirements, and Tasks underneath it.

**When not to use this guide:** Do not use for a single user-facing capability that fits inside one release; create a Feature instead (see GHE-ALM-011). Do not use for a single requirement or task.

## Outcome

By the end of this guide, you will have produced:

- An Epic or Initiative parent issue in the correct repository, with the correct custom type assigned.
- A scope statement, success criteria, and in-scope/out-of-scope sections in the issue body.
- The parent issue added to the organization Project with `Status`, `Owner`, `Priority`, `Target Release`, and `Product Area` set.
- A parent ready to receive sub-issues for Features, Requirements, and Tasks.

## Before You Start

- Decide whether the work is an Epic or an Initiative. An Epic is a major capability delivered over weeks to a few months and usually fits inside one or two releases. An Initiative is a larger business objective or release theme spanning a quarter or more, and typically contains multiple Epics.
- Identify the accountable owner. One person, not a team, owns the parent.
- Identify the Product Area (for example, `Checkout`, `Billing`, `Identity`).
- Identify the Target Release if known. If the scope is still being shaped, leave Release blank and set it during the next backlog refinement.
- Draft a one-paragraph scope statement before opening the issue form. Writing it in the form leads to weak scope.

## Steps

### Choose Epic or Initiative

1. Confirm the work meets the parent threshold. A parent should hold at least three child issues (Features, Requirements, or Tasks) and represent a coherent outcome that a stakeholder can name.
2. Apply the Epic vs Initiative decision rule:
   - Pick `Epic` if the work delivers one major capability, has a single primary owner, and is expected to complete inside one to two releases.
   - Pick `Initiative` if the work spans a quarter or more, contains multiple Epics, and rolls up to a strategic objective or release theme.
3. If you are unsure, default to `Epic`. Promote it later if its scope grows beyond a release.

### Create the parent issue

4. Navigate to the repository that owns the parent work. For a cross-team Initiative, this is usually a program or planning repository (for example, `acme-platform/program-planning`). For an Epic owned by one team, use that team's primary repository (for example, `acme-payments/checkout-service`).
5. Click **Issues**, then click **New issue**.
6. If the repository shows a template chooser, select the **Epic** template or the **Initiative** template if one exists. If only blank issue and bug templates appear, click **Open a blank issue** and set the type manually in the next step.
7. In the issue sidebar, open the **Type** selector and choose `Epic` or `Initiative`. If the type does not appear in the list, the organization has not enabled it; stop and follow GHE-ALM-023 to request it.

> [SCREENSHOT: New issue form with the Type selector open and Epic and Initiative visible in the type list]

### Write the title and body

8. Write a title in the form `[Epic] <Capability outcome>` or `[Initiative] <Strategic outcome>`. Examples: `[Epic] Self-service refund flow for Checkout`, `[Initiative] 2026 Identity consolidation`. Avoid solution language (`[Epic] Build new database`) in the title.
9. In the body, use these sections in order:
   - **Scope statement.** Two to four sentences naming the outcome, the user or business problem, and the boundary.
   - **In scope.** Bulleted list of capabilities, surfaces, or workflows included.
   - **Out of scope.** Bulleted list of work that will not be done under this parent. This is the most often skipped section and the most often referenced one.
   - **Success criteria.** Three to seven measurable outcomes that decide whether the parent can be closed.
   - **Assumptions and dependencies.** Known dependencies on other Epics, teams, or releases.
   - **Open questions.** Items still being decided. Tracked here, not in chat.
10. Click **Submit new issue**.

### Set required fields and add to the Project

11. In the issue sidebar, set **Assignees** to the accountable owner.
12. Click **Projects** in the sidebar and add the issue to the organization ALM Project.
13. Open the Project view and set these fields on the row for the new issue:
    - `Status`: `Backlog` for a new parent that has not been committed; `In Progress` only when at least one child issue has started.
    - `Priority`: a value from the team's standard scale (a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership).
    - `Owner`: the same person set as Assignee on the issue.
    - `Product Area`: for example, `Checkout`, `Billing`, `Identity`.
    - `Target Release`: for example, `2026.05.0` or `2026-Q3 Release`. Leave blank if the parent is still being shaped.
    - `Start Date` and `Target Date`: best-known dates. Refine these in planning.
14. Add labels for any secondary classification your team uses (for example, `theme:platform`, `risk:high`). Do not use a label as a substitute for the Type field (see GHE-ALM-021).

> [SCREENSHOT: Project table row for the new Epic showing Status, Priority, Owner, Product Area, Target Release, Start Date, and Target Date populated]

### Prepare for work breakdown

15. Open the parent issue. In the **Sub-issues** panel, plan to add Features, Requirements, or Tasks that decompose the parent. The mechanics of adding sub-issues are covered in GHE-ALM-017.
16. Pin the parent issue in the repository if your team uses pinned issues to highlight current Epics and Initiatives.
17. Announce the parent in the next backlog refinement so child work can be linked. Share the issue URL, not a screenshot.

> [SCREENSHOT: Parent issue with the Sub-issues panel visible and the Type badge showing Epic or Initiative]

## Validation Checklist

- [ ] The issue type badge on the parent shows `Epic` or `Initiative`.
- [ ] The issue body contains scope, in scope, out of scope, success criteria, assumptions, and open questions.
- [ ] `Owner`, `Priority`, `Product Area`, and `Status` are set in the Project.
- [ ] `Target Release` is set if known, or the parent is explicitly listed for the next refinement.
- [ ] The parent appears in the organization ALM Project's backlog or roadmap view.
- [ ] At least one stakeholder outside the immediate team can find the parent by searching `is:issue type:Epic` or `is:issue type:Initiative`.

## Common Mistakes

- Using a label such as `epic` or `initiative` instead of the issue Type. Labels do not roll up in Hierarchy View and break reporting.
- Writing a title that describes a solution (`[Epic] Migrate to PostgreSQL`) instead of an outcome (`[Epic] Reduce checkout latency below 200 ms`).
- Skipping the Out of Scope and Success Criteria sections. These are the sections that prevent scope creep and resolve future arguments.
- Creating an Initiative when an Epic is sufficient. Initiatives that contain only one Epic create reporting noise.
- Assigning the parent to a team handle rather than a single owner. Parents need one accountable name.
- Adding sub-issues before the scope statement is written. The decomposition then drifts from the intent.
- Setting `Status` to `In Progress` at creation time. The parent is `In Progress` only when child work has started.

## Escalation Path

- GitHub administrator: Involve if the `Epic` or `Initiative` type does not appear in the Type selector, or if an organization-wide rename is needed.
- Repository administrator: Involve if the target repository does not have an issue template for Epic or Initiative and your team wants one added.
- Engineering lead: Involve to confirm the technical scope boundary and to commit an owner before publishing the parent.
- Release manager: Involve to confirm `Target Release` and to verify the parent fits the release train calendar.

## Related Guides

- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-012 : How to Create a Requirement Issue
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-023 : How to Define or Request Organization Issue Types
