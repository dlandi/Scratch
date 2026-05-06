# How to Create and Triage a Bug Report

**Guide ID:** GHE-ALM-014
**Audience:** QA Manager, Support Engineer, Engineering Manager
**Primary role:** QA Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per bug
**Required permissions:** Repository: Read or Triage. Triage role is needed to apply labels, set the milestone, and edit Project fields. Without Triage, you can still file the bug; an admin or assignee will need to complete the triage fields.
**Prerequisites:**

- A repository with the **Bug** issue type enabled and a Bug Report issue form configured (see GHE-ALM-025).
- The repository is added to the team's organization-level Project that contains Severity, Priority, Sprint, Release, and Product Area fields.
- You can sign in to GitHub Enterprise and reach the repository in question.

**When to use this guide:** Use this guide when a defect has been observed in a released, staged, or in-development build and you need to capture it as an actionable issue.

**When not to use this guide:** Do not use this guide for feature requests, change requests, or general support questions. See GHE-ALM-011 for feature requests and GHE-ALM-016 for change requests.

## Outcome

By the end of this guide, you will have produced:

- A new GitHub issue of type **Bug** with all required Bug Report form fields completed.
- A triaged bug with severity, priority, label, milestone or release field, project assignment, and target sprint set, ready to be picked up in the next bug review (GHE-ALM-039).

## Before You Start

- Confirm the product or component the defect belongs to.
- Gather the affected version or build number, the environment (for example `prod`, `staging`, `qa-eu`), and any logs, screenshots, or crash reports.
- Have a short reproduction recipe ready: what you did, what you expected, what actually happened.
- Know whether this is a regression from a previous working version and whether a workaround exists.
- Confirm the customer, internal team, or release affected.

## Steps

### Part 1: File the bug

1. Navigate to the repository where the defect lives. If you are unsure which repository owns the component, use the Repository Dashboard (GHE-ALM-003) to find it.
2. Click the **Issues** tab in the repository's top navigation.
3. Click **New issue**. The template chooser appears.

> [SCREENSHOT: Issue template chooser showing the Bug Report form alongside Feature Request and other templates, with the Bug Report "Get started" button highlighted]

4. Next to **Bug report**, click **Get started**. If your repository does not show a Bug Report form, stop and request one through GHE-ALM-025; do not file the bug as a blank issue.
5. Enter a clear, specific title in the **Title** field. Use the pattern `[Component] Short symptom under condition`. For example: `[Checkout] Apply Coupon button does nothing on Safari 17`.
6. Fill in each form field:
   - **Product/Component:** the affected module or service.
   - **Affected version:** the build or release tag where the defect was observed, for example `2026.04.2`.
   - **Environment:** for example `prod-us-east`, `staging`, or `qa-eu`.
   - **Severity:** the technical or user impact (see Part 3).
   - **Priority:** the business urgency of fixing it (see Part 3).
   - **Steps to reproduce:** numbered steps a developer can follow.
   - **Expected behavior:** what should have happened.
   - **Actual behavior:** what did happen.
   - **Logs/screenshots/crash reports:** drag and drop attachments into the field. Issue forms support file uploads.
   - **Regression:** Yes if it worked in a prior version, otherwise No. Name the last known good version if you have it.
   - **Workaround:** describe any workaround, or state `None`.
   - **Customer impact:** number of customers, severity to them, and any contractual exposure.
   - **Target release:** the release where you expect the fix, if known. Leave blank if triage will decide.

> [SCREENSHOT: Bug Report issue form filled in with Title, Severity, Priority, Steps to reproduce, and an attached screenshot file]

7. Click **Submit new issue**.

### Part 2: Triage the bug

After the issue is created, complete the triage fields in the right sidebar of the new issue page. If you do not have Triage permission, comment `@<bug-triage-team> please triage` and stop here.

8. In the right sidebar, confirm **Type** is set to **Bug**. If the form did not set it, click **Type** and choose **Bug**.
9. Click **Labels** and apply secondary classification labels such as `area:checkout`, `regression`, `customer-reported`, or `needs-repro`. Do not use labels in place of the type or severity field (see GHE-ALM-021).
10. Click **Assignees** and assign the triage owner if known. If unknown, leave unassigned for the bug triage view (GHE-ALM-034) to pick up.
11. Click **Milestone** and select the repository milestone for the target release, for example `2026.05.0`. If the fix target is not yet decided, leave the milestone empty.
12. Click **Projects** and add the issue to the team's organization-level ALM Project. The Project fields appear once the issue is added.
13. In the Project fields panel of the issue, set:
    - **Severity:** confirm the value carried over from the form, or set it now.
    - **Priority:** same.
    - **Sprint:** set to the current or next iteration if the bug must be addressed soon. Leave blank for the backlog.
    - **Release:** set the target release for cross-repository tracking.
    - **Product Area:** set to the owning product area.
    - **Status:** set to `Triage` or `New`, per your team's workflow (see GHE-ALM-036).

> [SCREENSHOT: Issue page sidebar showing Type set to Bug, Labels, Milestone, and Project fields populated with Severity, Priority, Sprint, and Release values]

14. Add a triage comment that summarizes the routing decision, for example: `Routed to @backend-team for investigation. Severity 2, Priority P1, target 2026.05.0.`

### Part 3: Score severity vs priority (worked example)

Severity describes the impact of the defect; priority describes how urgently the business needs the fix. Score them independently.

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

Worked example: the coupon code field on the checkout page silently fails for Safari 17 users. Roughly 8 percent of customers use Safari, and they can complete the order by switching browsers.

- Severity: 2. A real browser is broken, but a workaround exists.
- Priority: P1. Coupons drive conversion and the affected segment is large enough to hurt revenue this sprint, so fix in the current sprint.

The two scores can diverge: a high-severity bug for an unused feature can be low priority, and a cosmetic defect on the homepage at launch week can be high priority.

## Validation Checklist

- [ ] The new issue has type **Bug**.
- [ ] All required Bug Report form fields are filled in, including Steps to reproduce, Expected behavior, and Actual behavior.
- [ ] Severity and Priority are set as separate values.
- [ ] At least one log, screenshot, or crash report is attached, or a clear note explains why none is available.
- [ ] The issue is added to the team's ALM Project, and Sprint, Release, and Product Area fields are populated where known.
- [ ] Milestone is set if the target release is decided.
- [ ] Status is `Triage` or `New`, ready for the bug triage view.

## Common Mistakes

- Filing a bug as a blank issue instead of using the Bug Report form, which leaves engineers without structured fields.
- Using a label like `bug` in place of the **Bug** issue type. Issue type is governed at the organization level; labels are not a substitute.
- Setting Severity equal to Priority by default. Score them independently.
- Pasting a screenshot into the title or one giant unstructured comment, instead of attaching files in the dedicated form field.
- Leaving **Affected version** blank, which forces engineering to guess which build to debug.
- Assigning the bug to a developer before triage, which bypasses the bug triage view (GHE-ALM-034).

## Escalation Path

- GitHub administrator: involve when the **Bug** issue type or required organization fields (Severity, Priority, Release) are missing across repositories.
- Repository administrator: involve when the Bug Report issue form does not exist in the repository or required fields are out of date.
- Engineering lead: involve when severity, reproduction, or root-cause questions cannot be resolved during initial triage.
- Release manager: involve when the bug is a candidate for hotfix to a released version (see GHE-ALM-040).

## Related Guides

- GHE-ALM-025 : How to Create or Request Issue Forms
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-035 : How to Distinguish Severity from Priority
- GHE-ALM-037 : How to Attach Evidence to a Bug
- GHE-ALM-038 : How to Associate a Bug with a Release or Sprint
- GHE-ALM-040 : How to Handle a Hotfix Bug
