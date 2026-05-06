# How to Create a Requirement Issue

**Guide ID:** GHE-ALM-012
**Audience:** Product Owner, Project Manager, Engineering Manager
**Primary role:** Product Owner
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes per requirement
**Required permissions:** Repository: Triage. Triage is needed to set the issue type, apply labels, set the milestone, and edit Project fields. Without Triage, you can still file the requirement; an admin or the parent's owner will need to complete the triage fields.
**Prerequisites:**

- The organization has the custom **Requirement** issue type configured (see GHE-ALM-023). If the type does not appear in the **Type** picker, stop and request it before continuing.
- A Requirement issue form exists in the target repository (see GHE-ALM-025). The form should include Acceptance Criteria as a structured field.
- The repository is added to the team's organization-level ALM Project, and the Project includes `Priority`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, and `Target Date` fields.
- The parent Epic or Feature already exists, or you know it does not yet exist and one needs to be created first (see GHE-ALM-013).

**When to use this guide:** Use this guide when a feature request has been accepted and you need to write the formal, testable specification that engineering will build and QA will verify against.

**When not to use this guide:** Do not use this guide for raw feature ideas, customer wishlists, or scope discussions. Those belong in a Feature Request (GHE-ALM-011). A feature request expresses intent; a requirement is the committed, testable specification.

## Outcome

By the end of this guide, you will have produced:

- A new GitHub issue of type **Requirement** with all structured fields populated and acceptance criteria expressed as testable statements.
- The requirement linked as a sub-issue under its parent Epic or Feature, added to the ALM Project, and assigned to a target release and product area so it is searchable and traceable.

## Before You Start

- Confirm the parent Epic or Feature ID, for example `acme-payments/checkout-service#412`. A requirement without a parent has no business context.
- Decide the product area, for example `Checkout`, `Billing`, or `Identity`.
- Draft the acceptance criteria before opening the form. Use Given/When/Then statements or a checklist of testable conditions. If you cannot write the criteria, the requirement is not yet ready to file.
- Confirm the target release, for example `2026.05.0`, or accept that triage will set it.
- Identify the engineering owner if known; otherwise the engineering lead for the product area will assign one during planning.

## Steps

### File the requirement

1. Navigate to the repository that owns the component the requirement applies to. If you are unsure which repository owns it, use the Repository Dashboard (GHE-ALM-003).
2. Click the **Issues** tab in the repository's top navigation.
3. Click **New issue**. The template chooser appears.
4. Next to **Requirement**, click **Get started**. If the Requirement form is not present in the chooser, stop and request it through GHE-ALM-025. Do not file the requirement as a blank issue, and do not reuse the Feature Request form.

> [SCREENSHOT: Issue template chooser showing the Requirement form alongside Feature Request and Bug Report, with the Requirement "Get started" button highlighted]

5. Enter a clear, specific title in the **Title** field. Use the pattern `[Product Area] Capability shall behavior under condition`. For example: `[Checkout] Apply Coupon shall reject expired codes with inline error`.
6. Fill in each form field:
   - **Parent Epic or Feature:** the issue reference of the parent, for example `acme-payments/checkout-service#412`. This will be confirmed as a sub-issue link in the next phase.
   - **Product Area:** for example `Checkout`.
   - **Description / Context:** one or two paragraphs explaining the user-facing behavior, the system boundary, and any out-of-scope items. Do not restate the feature request; state the contract engineering will build.
   - **Acceptance Criteria:** the structured, testable statements. Use Given/When/Then or a numbered checklist. Each item must be independently verifiable by QA. Examples:
     - `Given an expired coupon code, When the user clicks Apply Coupon, Then the field shows "This code has expired" and the order total is unchanged.`
     - `Given a valid coupon code, When the user clicks Apply Coupon, Then the discount is applied within 1 second and the order total updates.`
   - **Non-functional requirements:** performance, accessibility, localization, security, or compliance constraints, or `None`.
   - **Dependencies:** other requirements, services, or external systems this depends on, or `None`.
   - **Out of scope:** what this requirement explicitly does not cover. This protects against scope creep during build.
   - **Priority:** the business urgency for inclusion in a release.
   - **Target release:** the release where you expect this delivered, for example `2026.05.0`. Leave blank if planning will decide.

> [SCREENSHOT: Requirement issue form filled in with Title, Parent reference, Description, and an Acceptance Criteria field showing three Given/When/Then statements]

7. Click **Submit new issue**.

### Classify and link the requirement

After the issue is created, complete the sidebar fields and the parent link on the new issue page. If you do not have Triage permission, comment `@<product-area-team> please complete triage on this requirement` and stop here.

8. In the right sidebar, confirm **Type** is set to **Requirement**. If the form did not set it, click **Type** and choose **Requirement**. If **Requirement** is not in the list, the organization has not configured the type yet; see GHE-ALM-023.
9. Click **Labels** and apply secondary labels for cross-cutting classification, for example `area:checkout`, `compliance:pci`, or `accessibility`. Do not use a `requirement` label as a substitute for the **Requirement** type (see GHE-ALM-021).
10. Click **Assignees** and assign the engineering owner if known. If unknown, leave unassigned. The product area lead will assign during sprint planning (GHE-ALM-028).
11. Click **Milestone** and select the repository milestone for the target release, for example `2026.05.0`. If the release is not yet decided, leave blank.
12. Click **Projects** and add the issue to the team's organization-level ALM Project. The Project field panel appears once the issue is added.
13. In the Project field panel, set:
    - **Priority:** business urgency on a 1-4 / P0-P3 scale. This is a common illustrative scale; confirm your team's actual scale with QA leadership.
    - **Effort:** leave blank if the engineering team has not yet estimated. Estimation happens during refinement.
    - **Sprint:** leave blank unless this is already committed to an iteration.
    - **Release:** set the target release for cross-repository tracking, for example `2026.05.0`.
    - **Product Area:** for example `Checkout`.
    - **Owner:** the accountable product owner or engineering owner.
    - **Target Date:** set if there is a hard external deadline. Otherwise leave blank.
    - **Status:** set to `Ready for Refinement` or your team's equivalent intake status.

> [SCREENSHOT: Requirement issue page sidebar showing Type set to Requirement, Labels applied, parent Epic linked, and Project fields populated with Priority, Release, Product Area, and Owner]

14. Link the requirement under its parent Epic or Feature as a sub-issue. From the parent issue, use **Add sub-issue** and reference this requirement, or follow the full sub-issue procedure in GHE-ALM-017. Verify the sub-issue link appears on both the parent and this requirement before moving on.
15. Add a brief comment that summarizes the routing, for example: `Linked under #412 (Checkout coupon redesign). Targeted for 2026.05.0. Awaiting refinement and effort estimate from @backend-team.`

## Validation Checklist

- [ ] The new issue has type **Requirement**.
- [ ] The title follows the `[Product Area] Capability shall behavior under condition` pattern.
- [ ] Acceptance Criteria are present, structured (Given/When/Then or a numbered checklist), and each item is independently testable by QA.
- [ ] The requirement is linked as a sub-issue under its parent Epic or Feature, and the link is visible from both directions.
- [ ] The issue is added to the team's ALM Project, with `Priority`, `Release`, `Product Area`, and `Owner` populated.
- [ ] Out of scope is stated explicitly.
- [ ] Status is `Ready for Refinement` or your team's equivalent intake status.

## Common Mistakes

- Filing a requirement as a blank issue or by reusing the Feature Request form. The Requirement form exists so that acceptance criteria, non-functional constraints, and out-of-scope items are captured consistently.
- Treating the requirement as a copy of the original feature request. A feature request expresses intent; a requirement is the committed specification engineering builds against.
- Writing acceptance criteria as goals or aspirations ("the page should be fast") rather than testable statements ("the order total updates within 1 second of clicking Apply Coupon").
- Omitting the parent link, which leaves the requirement orphaned from its Epic or Feature and invisible in the Hierarchy View (GHE-ALM-018).
- Using a `requirement` label instead of the **Requirement** issue type. Issue type is governed at the organization level; labels are not a substitute.
- Setting **Effort** before refinement. Estimation belongs to engineering after they have reviewed the criteria.
- Leaving **Out of scope** blank. Implicit scope is the most common cause of mid-sprint scope disputes.

## Escalation Path

- GitHub administrator: involve when the **Requirement** issue type or required organization fields (`Priority`, `Release`, `Product Area`) are missing across the organization. See GHE-ALM-023 and GHE-ALM-024.
- Repository administrator: involve when the Requirement issue form does not exist in the repository or the form is missing the Acceptance Criteria field. See GHE-ALM-025.
- Engineering lead: involve when the requirement cannot be made testable without architectural input, or when dependencies on other services or teams cannot be resolved.
- Release manager: involve when the target release is contested or when the requirement risks displacing already-committed scope.

## Related Guides

- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-025 : How to Create or Request Issue Forms
