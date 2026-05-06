# How to Create a Risk or Change Request Issue

**Guide ID:** GHE-ALM-016
**Audience:** Project Manager, Program Manager, Release Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per use
**Required permissions:** Repository: Triage; Project: Write
**Prerequisites:**

- The organization has the custom issue types `Risk` and `Change Request` configured. If they are missing, follow GHE-ALM-023 first.
- The ALM Project exposes the canonical fields, including `Status`, `Priority`, `Owner`, `Product Area`, `Release`, `Risk Level`, `Customer Impact`, and `Target Date`.
- You know the affected release, the responsible owner, and the baseline that the change or risk relates to.

**When to use this guide:** Use this guide to log a delivery, technical, or operational threat to a release as a Risk, or to log a controlled scope change against a baselined release as a Change Request.

**When not to use this guide:** Do not use this guide for ordinary backlog work, defects, or new feature requests. File those as Feature, Requirement, Task, or Bug per GHE-ALM-011, GHE-ALM-012, GHE-ALM-014, or GHE-ALM-015.

## Outcome

By the end of this guide, you will have produced:

- A Risk issue or Change Request issue created in the correct repository, typed correctly, and added to the ALM Project.
- A populated issue with owner, priority, release, risk or change-specific fields, and a structured description that the governance forum can review.
- A traceable link from the new issue to any parent epic, requirement, milestone, or release readiness review entry.

## Before You Start

- Confirm the affected release. For a Risk, this is the release the threat could impact. For a Change Request, this is the release whose scope is changing.
- Confirm the owner. Risks need a single accountable owner for mitigation. Change Requests need a single requester and a named approver.
- Gather facts: cause, evidence, options considered, current mitigation or workaround, decision needed, and the date by which the decision is needed.
- Verify you have at least Triage on the target repository and Write on the ALM Project.

## Steps

### Decide which type to use

1. If the item is a *threat* that has not yet caused scope change, schedule slip, or quality impact, file it as a **Risk**. A Risk is open-ended; it lives until it is closed, mitigated, accepted, or it materializes.
2. If the item is a *decision* to add, remove, or alter scope after the release baseline has been set, file it as a **Change Request**. A Change Request is short-lived; it is approved or rejected, and the result is recorded.
3. If you cannot decide, default to Risk. A Risk can spawn a Change Request later if the team chooses to act on it.

### Create the issue

4. Open the repository that owns the affected release or product area. For cross-repository releases, use the governance repository defined in your release plan.
5. Click **Issues**, then **New issue**. If a structured form for `Risk Report` or `Change Request` exists, select it. Otherwise pick the blank issue option.
6. In the **Type** selector at the top of the issue, choose `Risk` or `Change Request`. The type controls how this item appears in governance views and filters.
7. Set the title using the conventions in the next two steps.
8. For a Risk, use the pattern `Risk: <short threat statement> [<release>]`. Example: `Risk: Vendor SDK GA slip threatens checkout latency target [2026.05.0]`.
9. For a Change Request, use the pattern `CR: <short change statement> [<release>]`. Example: `CR: Add SAML SSO to acme-checkout 2026.05.0`.

> [SCREENSHOT: New issue form with the Type selector expanded, showing Risk and Change Request among the available types]

### Fill the issue body

10. For a Risk, structure the body with these sections: `Summary`, `Cause`, `Likelihood`, `Impact if it occurs`, `Current mitigation`, `Proposed mitigation`, `Owner`, `Decision needed by`. Keep each section terse.
11. For a Change Request, structure the body with these sections: `Summary of requested change`, `Justification`, `Affected scope`, `Impact assessment` (schedule, cost, quality, dependencies), `Options considered`, `Recommendation`, `Approver`, `Decision needed by`.
12. Reference any related work using `owner/repo#NNN` so it appears in the timelines of those items. For example, link the parent epic, the affected requirement, the milestone, and any defects that motivated the change.
13. Do not paste secrets, customer-identifying data, or contractual text into the issue body.

### Apply metadata

14. Set **Assignees** to the single accountable owner. For a Risk, this is the mitigation owner. For a Change Request, this is the requester until the approver acts.
15. Set the **Milestone** to the affected release if your team uses repository milestones for that release. If the release is tracked across repositories, leave the milestone blank and rely on the project `Release` field instead.
16. Add a label only for secondary classification, for example `risk:delivery`, `risk:technical`, `risk:operational`, `cr:scope-add`, `cr:scope-remove`, or `cr:scope-change`. Do not use labels to substitute for the issue type. See GHE-ALM-021.

> [SCREENSHOT: Issue sidebar showing Type set to Risk, a single assignee, milestone selected, and a risk:delivery label applied]

### Add to the ALM Project and set fields

17. In the issue sidebar, under **Projects**, add the ALM Project for the affected product or release.
18. Open the project item and set `Status` to `Triage` for a new Risk or `Pending Approval` for a new Change Request, matching your project's configured status values.
19. Set `Priority` using your team's scale (a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership).
20. Set `Owner` to the same person you assigned in step 14. Set `Product Area`, for example `Checkout` or `Billing`. Set `Release`, for example `2026.05.0`. Set `Target Date` to the decision-needed date.
21. For a **Risk**, set `Risk Level` using the locked field values (typically `Low`, `Medium`, `High`, `Critical`). Risk Level reflects likelihood multiplied by impact, not impact alone.
22. For a **Change Request**, leave `Risk Level` blank. Set `Customer Impact` to indicate whether customers will see the change (`None`, `Internal`, `External`, `Contractual`).

### Route for governance

23. For a Risk, post a comment that names the mitigation owner and the next review checkpoint, for example the next release readiness review. Cross-reference GHE-ALM-046.
24. For a Change Request, post a comment that names the approver and the deadline, and `@mention` the approver so they receive notification.
25. If the Risk or Change Request affects another team, link to the parent epic in the relevant area so it surfaces in their hierarchy view per GHE-ALM-018.

> [SCREENSHOT: ALM Project table view filtered by Type = Risk, Type = Change Request showing the new item with Status, Priority, Risk Level, and Customer Impact populated]

## Validation Checklist

- [ ] The issue type is `Risk` or `Change Request`. It is not a label, and it is not `Task` or `Bug`.
- [ ] The title follows the `Risk: ...` or `CR: ...` pattern and includes the affected release in brackets.
- [ ] The body covers the required sections for the chosen type, with no missing fields.
- [ ] The issue has a single accountable assignee.
- [ ] The issue is added to the correct ALM Project, and the project fields `Status`, `Priority`, `Owner`, `Product Area`, `Release`, and `Target Date` are set.
- [ ] For a Risk, `Risk Level` is set. For a Change Request, `Customer Impact` is set.
- [ ] The next governance checkpoint is named in a comment, and the approver or reviewer is `@mentioned` if a decision is required.

## Common Mistakes

- Filing a Risk as a Bug. Bugs describe defects already in the product. Risks describe threats that may or may not materialize.
- Filing a Change Request before the release is baselined. Pre-baseline scope decisions belong in normal sprint planning, not in change control.
- Setting Risk Level by impact only. Risk Level must combine likelihood and impact, otherwise low-likelihood high-impact items dominate the queue.
- Assigning a Risk to a team rather than a person. Risks need a single accountable owner.
- Linking the issue to the affected release only by free text in the title. Use the project `Release` field so the item appears in release filters and roadmap views.
- Using a label such as `risk` instead of the issue type `Risk`. Labels do not drive governance views.
- Embedding contract language, customer names, or pricing in the body. Reference an internal document instead.

## Escalation Path

- GitHub administrator: When the `Risk` or `Change Request` issue type is missing from the organization, or when you cannot select it in the Type field. Follow GHE-ALM-023 to request the type.
- Repository administrator: When you cannot apply the issue type or labels because you lack Triage access on the target repository.
- Engineering lead: When the Risk requires a technical mitigation decision, or when a Change Request requires effort estimation before approval.
- Release manager: When a Risk is scoring `High` or `Critical`, when a Change Request affects the release date, or when the decision-needed date is inside the next release readiness review window.

## Related Guides

- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
- GHE-ALM-023 : How to Define or Request Organization Issue Types
- GHE-ALM-046 : How to Prepare a Release Readiness Review
