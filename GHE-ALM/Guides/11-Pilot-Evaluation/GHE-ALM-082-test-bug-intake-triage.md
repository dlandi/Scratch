# How to Test Bug Intake and Triage

**Guide ID:** GHE-ALM-082
**Audience:** QA Manager, Engineering Manager, Project Manager
**Primary role:** QA Manager
**Classification:** Manager Performs
**Estimated time:** 60-90 minutes for the scenario, plus follow-up evidence capture
**Required permissions:** Repository: Triage on the pilot repository; Project: Write on the pilot Project
**Prerequisites:**

- The pilot is in flight under GHE-ALM-079 with at least one product, two to four repositories, and one team.
- A Bug issue type exists at the organization level, and a Bug Report issue form exists in the pilot repository.
- The pilot Project has a Bug Triage view, with `Severity`, `Priority`, `Status`, `Sprint`, `Release`, and `Product Area` fields configured.
- A non-engineer (support engineer, product owner, or pilot business user) is available to file the test bug.

**When to use this guide:** Use this guide to execute pilot scenario 3, the bug intake and triage scenario, and to record pass/fail evidence against the defect workflow criteria.

**When not to use this guide:** Do not use this guide for everyday bug filing or triage during the pilot; use GHE-ALM-014 and GHE-ALM-034. Do not use this guide if the bug form, severity, priority, or triage view is not yet in place; finish that setup first.

## Outcome

By the end of this guide, you will have produced:

- A real bug filed by a non-engineer using the Bug Report form, with severity, priority, evidence, and product area populated.
- A triage decision recorded by the manager, with the bug routed to an engineer and associated with a sprint and release.
- Evidence captured for each pilot pass criterion, ready for GHE-ALM-085.

## Before You Start

- Confirm the pilot tenant naming. This guide uses `acme-payments` as the org and `checkout-service` as the pilot repository.
- Identify one tester or non-engineer who will file the bug. They need Repository `Triage` access on `checkout-service`.
- Identify the manager who will perform triage. They need Project `Write` on the pilot Project.
- Pick a real or representative defect for the test. A trivial typo bug is acceptable; a fabricated "test bug 1" is not, because the form fields will not be exercised.
- Open a fresh pass/fail capture document or worksheet for the five pilot criteria listed under Validation Checklist.

## Steps

### Set up the scenario

1. In `acme-payments/checkout-service`, open **Issues** and confirm the **New issue** dropdown shows the **Bug Report** template. If it does not appear, stop and resolve the issue form gap before continuing.
2. Open the pilot Project, switch to the **Bug Triage** view, and confirm the view is grouped by `Severity` or `Status` and filtered to open bugs. Note the current bug count so you can verify the new bug appears.
3. Brief the non-engineer tester: they will open a bug using the form, fill every required field, attach at least one screenshot or log file, and submit. Do not coach them through the form fields; the point of the scenario is to see whether the form alone is enough.

> [SCREENSHOT: New issue picker showing the Bug Report template available in checkout-service]

### Execute the scenario as a non-engineer

4. The tester clicks **New issue**, selects **Bug Report**, and completes the form. They set a clear title such as `Checkout total ignores promo code on cart with one item`.
5. The tester fills the structured fields: reproduction steps, expected behavior, actual behavior, environment, and affected version. They pick `Severity` and `Priority` from the form dropdowns, using the illustrative 1-4 / P0-P3 scale (confirm your team's actual scale with QA leadership).
6. The tester attaches evidence in the form's file upload field or in the first comment: at least one screenshot, and a log snippet if available.
7. The tester submits the bug. Record the issue number and URL in the worksheet. The bug should land with the `bug` label and the `Bug` issue type, and it should auto-add to the pilot Project if the auto-add workflow is configured.

> [SCREENSHOT: Submitted bug as it appears on the issue page, showing fields, labels, attached screenshot, and Project association]

### Triage and route the bug

8. As the manager, open the **Bug Triage** view in the pilot Project. Confirm the new bug appears in the expected severity group. If it is missing, add it manually using **Add item** and note the gap.
9. In the Project row, set `Status` to `Triage` if it is not already, set `Owner` to the engineer who will investigate, and set `Product Area` (for example, `Checkout`).
10. Set `Sprint` to the current sprint or the next sprint, and set `Release` to the active release train (for example, `2026.05.0`). Use `sprint:@current` to filter the view if you need to confirm the sprint assignment.
11. Adjust `Severity` and `Priority` if your triage assessment differs from the tester's. Add a triage comment on the issue explaining any change. This is the manager triage decision; capture a screenshot for the worksheet.
12. Notify the assigned engineer using `@mention` in a comment, and confirm the engineer can open the issue, see all evidence, and read the triage decision without follow-up questions. Ask the engineer for a one-line confirmation in the issue or in the worksheet.

> [SCREENSHOT: Bug Triage view filtered to the new bug, showing Severity, Priority, Status, Owner, Sprint, Release, and Product Area populated]

### Record evidence per pass criterion

13. For each criterion in the Validation Checklist below, paste the relevant URL, screenshot reference, and a one-line note into the worksheet. Mark each criterion `Pass`, `Pass with caveats`, or `Fail`.
14. If any criterion fails, capture the specific gap (missing field, missing dropdown, attachment limit, view not showing the bug, sprint or release field absent). The gap, not the workaround, is what GHE-ALM-085 needs.

## Validation Checklist

- [ ] The Bug Report form captured all required fields without the tester needing engineer help.
- [ ] `Severity` and `Priority` were assigned at intake and confirmed at triage.
- [ ] At least one screenshot or log file was attached and is viewable on the issue.
- [ ] The bug is visible in the Bug Triage view in the expected severity or status group.
- [ ] The bug is associated with a sprint and a release through the `Sprint` and `Release` project fields.
- [ ] An engineer was assigned and confirmed they have enough information to act.

## Common Mistakes

- Letting the tester skip the form and file a freeform issue. The pilot must exercise the form, not bypass it.
- Coaching the tester through fields. The pilot tests whether the form is self-sufficient.
- Recording only a pass/fail label and not capturing the URL, screenshot, and worksheet note. GHE-ALM-085 needs the evidence, not just the verdict.
- Using a fabricated "test" bug with no real reproduction. Field quality cannot be judged on a stub.
- Forgetting to set `Sprint` and `Release` at triage. Without them, the defect workflow criterion fails even if everything else passes.
- Treating a missing dropdown or missing attachment field as the tester's problem. It is a form gap; record it.

## Escalation Path

- GitHub administrator: Escalate if the Bug issue type, organization issue fields, or auto-add workflow is missing or misconfigured at the org level.
- Repository administrator: Escalate if the Bug Report issue form is missing in the pilot repository, or if file uploads are blocked.
- Engineering lead: Escalate if the assigned engineer reports the bug evidence is insufficient despite the form being filled completely. That is a form quality finding, not a tester finding.
- Release manager: Escalate if the `Release` field options do not include the active release train, blocking the sprint/release association criterion.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-037 : How to Attach Evidence to a Bug
- GHE-ALM-079 : How to Run the GitHub Enterprise ALM Pilot Evaluation
- GHE-ALM-085 : How to Record Pilot Pass/Fail Evidence
