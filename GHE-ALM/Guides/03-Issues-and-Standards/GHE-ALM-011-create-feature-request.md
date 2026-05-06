# How to Create a Feature Request Issue

**Guide ID:** GHE-ALM-011
**Audience:** Product Owner, Project Manager, Engineering Manager
**Primary role:** Product Owner
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per use
**Required permissions:** Repository: Triage (to apply issue type, labels, and assignee); Project: Write (to set Project fields such as `Priority`, `Owner`, `Product Area`, and `Release`)
**Prerequisites:**

- The target repository has the **Feature Request** issue form configured. If the form is missing, see GHE-ALM-025.
- The organization has the `Feature` issue type enabled. If it is not, see GHE-ALM-023.
- The ALM Project has fields for `Priority`, `Owner`, `Product Area`, and `Release`. If any are missing, see GHE-ALM-024.
- You can identify the right repository and Project for this product. If not, see GHE-ALM-002.

**When to use this guide:** Use this when a stakeholder asks for new product capability that is larger than a single task and needs prioritization, sizing, and a target release. The output is a structured intake item, not a finished design.

**When not to use this guide:** Do not use this for defects (use GHE-ALM-014), implementation tasks under an existing feature (use GHE-ALM-015), or program-level themes that span multiple features (use GHE-ALM-013).

## Outcome

By the end of this guide, you will have produced:

- A new issue in the correct repository with issue type set to `Feature`.
- A populated Feature Request form covering the problem, business justification, and acceptance criteria.
- An item added to the ALM Project with `Priority`, `Owner`, `Product Area`, and `Release` set, ready for triage.

## Before You Start

- Confirm the repository slug, for example `acme-payments/checkout-service`.
- Confirm the ALM Project name, for example `acme-payments ALM`.
- Have the requester's name, the business problem, and at least a draft of acceptance criteria written down.
- Decide a target release if one is known, for example `2026.05.0`. If unknown, leave `Release` empty and triage will set it.
- Decide a `Product Area`, for example `Checkout`, `Billing`, or `Identity`.

## Steps

### Open the Feature Request form

1. Navigate to the target repository, for example `acme-payments/checkout-service`.
2. Click the **Issues** tab.
3. Click **New issue**. The repository's issue form picker appears.
4. Find the **Feature Request** entry in the list and click its **Get started** button. If only a blank issue option is offered, the form is not configured; stop here and see GHE-ALM-025.

> [SCREENSHOT: Issue form picker with the Feature Request form highlighted]

### Fill in the form body

5. Enter a title using the pattern `Feature: <capability> for <user or system>`, for example `Feature: Saved payment methods for returning checkout users`. Avoid solution-specific titles like `Add dropdown to checkout`.
6. In **Problem statement**, describe the user or business problem in two to four sentences. State who is affected and what they cannot do today.
7. In **Proposed capability**, describe what the user should be able to do after the feature ships. Keep this outcome-focused, not implementation-focused.
8. In **Business justification**, state the value: revenue, retention, compliance, support cost reduction, or strategic commitment. Reference the requester or sponsoring stakeholder by name.
9. In **Acceptance criteria**, list testable conditions as a checklist. Each item should be independently verifiable. A typical feature has three to seven criteria. Example:
   - [ ] Returning user sees their saved payment methods at checkout.
   - [ ] User can remove a saved payment method from their account settings.
   - [ ] Saved payment methods are scoped to a single account and never shared.
10. In **Out of scope**, list anything a reader might assume is included but is not. This prevents scope drift during refinement.
11. In **Target release**, enter the planned release if known, for example `2026.05.0`. Leave blank if triage will set it.
12. In **Product area**, select the product area, for example `Checkout`.

### Set issue type, assignee, and labels

13. In the right sidebar, open the **Type** control and select `Feature`. The type appears next to the title once saved.
14. Open **Assignees** and assign the owner accountable for shepherding this feature through refinement. This is usually the Product Owner. Engineering ownership is set later through the Project `Owner` field.
15. Open **Labels** and apply secondary labels only if your team uses them for routing, for example `area:checkout` or `customer-reported`. Do not use a label to substitute for the issue type. See GHE-ALM-021.
16. Open **Projects** and add the issue to the ALM Project, for example `acme-payments ALM`. The issue now has Project fields available.

> [SCREENSHOT: Issue sidebar showing Type set to Feature, Assignees set, and the ALM Project added]

### Set Project fields for triage readiness

17. Click **Submit new issue**. The issue is created and assigned a number, for example `#482`.
18. In the right sidebar under the ALM Project, set **Priority** using your team's scale. A common 1-4 / P0-P3 scale is illustrative; confirm your team's actual scale with QA leadership and product leadership. For new features that are not yet prioritized, set `P3` or leave blank so triage can rank it.
19. Set **Owner** to the engineering owner who will lead delivery once prioritized. If unknown, leave empty for triage.
20. Set **Product Area** to match what you entered in the form body, for example `Checkout`. Keeping form text and Project field aligned matters for reporting.
21. Set **Release** to the target release if known, for example `2026.05.0`. Leave empty if triage will assign it.
22. Leave `Sprint`, `Effort`, `Start Date`, and `Target Date` empty. These are set during sprint planning and refinement, not at intake.

> [SCREENSHOT: Project fields panel showing Priority, Owner, Product Area, and Release populated]

### Hand off for triage

23. Add a single comment that names the triage forum and date, for example `Submitting to Checkout triage on 2026-05-08.` This makes the handoff explicit in the issue history.
24. If your team uses a triage view filter, confirm the issue appears there. A typical filter is `is:open type:Feature no:assignee` or `Status:Triage`. Adjust to match your Project.

## Validation Checklist

- [ ] Issue title follows `Feature: <capability> for <user or system>` and is not solution-specific.
- [ ] Issue type is `Feature`.
- [ ] Problem statement, proposed capability, business justification, and acceptance criteria are all present and non-empty.
- [ ] Acceptance criteria are written as testable checkboxes.
- [ ] Issue is added to the correct ALM Project.
- [ ] `Priority`, `Owner`, `Product Area`, and `Release` are set or intentionally left blank for triage.
- [ ] Issue appears in the team's triage view or filter.

## Common Mistakes

- Writing the title as a solution, for example `Add dropdown to checkout`, instead of a capability or outcome. Refactoring the title later breaks links and search.
- Using a label such as `feature` instead of setting the `Feature` issue type. Reporting and hierarchy queries depend on the type, not the label.
- Putting acceptance criteria into the body as prose. Use a checkbox list so progress is trackable and each criterion is individually verifiable.
- Setting `Sprint` or `Effort` at intake. These belong to refinement and sprint planning, and setting them early skews backlog reports.
- Filing the feature in the wrong repository because the requester sent a link to a related repo. Confirm the product area and the owning team before you click **New issue**.
- Skipping the Project add. An issue without the ALM Project will not appear in backlog, sprint, or roadmap views.

## Escalation Path

- GitHub administrator: Involve only if the `Feature` issue type is missing organization-wide or you cannot add issues to the organization Project. See GHE-ALM-023.
- Repository administrator: Involve if the Feature Request issue form is missing or broken. See GHE-ALM-025.
- Engineering lead: Involve to confirm the engineering `Owner` and to validate that acceptance criteria are technically meaningful before triage.
- Release manager: Involve before setting `Release` if you are aiming at a release that is already in scope-lock or hardening.

## Related Guides

- GHE-ALM-012 : How to Create a Requirement Issue
- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-025 : How to Create or Request Issue Forms
