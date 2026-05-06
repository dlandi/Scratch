# How to Move a Bug Through the Defect Workflow

**Guide ID:** GHE-ALM-036
**Audience:** Engineering Manager, QA Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 5-10 minutes per bug; 15-20 minutes for a full board sweep
**Required permissions:** Repository: Triage (to edit issue state); Project: Write (to change project field values)
**Prerequisites:**

- The bug is already filed with `Type: Bug`, `Severity`, and `Priority` populated (see GHE-ALM-014).
- The bug is added to the organization-level ALM Project.
- The Project has a single-select `Status` field with the canonical values listed below.

**When to use this guide:** Use it whenever you, the engineering manager, or a QA manager need to advance a defect through its lifecycle stage, or to inspect the board for bugs that are stuck at a stage longer than expected.

**When not to use this guide:** Do not use it for non-defect work items such as features, requirements, or tasks. Those follow the sprint execution workflow in GHE-ALM-029.

## Outcome

By the end of this guide, you will have produced:

- A bug whose `Status` field reflects its current defect lifecycle stage.
- A short list of bugs that are stuck at one stage and need escalation.

## Before You Start

- Open the ALM Project that contains the bug.
- Confirm the Project has a `Status` field with these values: `Backlog`, `Ready`, `In Progress`, `In Review`, `Ready for QA`, `Verified`, `Done`, `Blocked`. If a value is missing, request it from the Project admin before continuing.
- Confirm you know which bug you are moving (the issue number, for example `acme-payments/checkout-service#482`).

## Defect lifecycle to Status mapping

The defect lifecycle in the evaluation document maps to the canonical `Status` field as follows. Use this table whenever you are unsure which value to set.

| Defect lifecycle stage | `Status` value | Meaning |
|---|---|---|
| New | `Backlog` | Filed but not yet triaged. |
| Triage | `Backlog` | Under triage review; severity and priority being confirmed. |
| Accepted | `Ready` | Triaged, owner assigned, ready for a sprint. |
| In Progress | `In Progress` | A developer has started work. |
| In Review | `In Review` | A pull request is open and under code review. |
| Fixed | `Ready for QA` | Code merged; awaiting QA verification. |
| Verified | `Verified` | QA has confirmed the fix in a test environment. |
| Closed | `Done` | Released and closed. |
| (any stage, blocked) | `Blocked` | Progress is held by a dependency, decision, or environment issue. |

`Blocked` is a parallel state, not a stage. When you set `Blocked`, also leave a comment naming the blocker.

## Steps

### Open the board grouped by Status

1. Open the ALM Project. Click the view tab named **Bug Workflow** (or whichever board view your team uses for defects).
2. If you are not already on a board view, click the **View options** gear icon, choose **Board** under **Layout**, and set the column field to **Status**. The board now shows one column per `Status` value.
3. Confirm the columns appear in lifecycle order: `Backlog`, `Ready`, `In Progress`, `In Review`, `Ready for QA`, `Verified`, `Done`, with `Blocked` at the end.

> [SCREENSHOT: Bug Workflow board grouped by Status with columns in lifecycle order]

### Move a bug forward

4. Find the bug card in its current column. Click the card title to open the bug.
5. In the right-hand sidebar, locate the project entry for the ALM Project and click the current `Status` value.
6. Select the next lifecycle value from the table above. For example, when triage finishes and an owner is assigned, set `Status` to `Ready`.
7. Confirm the linked project fields are correct for the new stage:
   - Moving to `Ready`: `Owner`, `Severity`, `Priority`, `Product Area` populated.
   - Moving to `In Progress`: `Sprint` populated; assignee set on the issue.
   - Moving to `In Review`: a pull request is linked using `Closes #NNN` syntax (see GHE-ALM-060).
   - Moving to `Ready for QA`: the linked pull request is merged.
   - Moving to `Verified`: a QA comment records the test environment and the test result.
   - Moving to `Done`: the bug's `Release` field matches the release where the fix shipped, and the issue is closed.
8. Save the change. The card now appears in the new column.

> [SCREENSHOT: bug issue sidebar showing Status field changed from In Review to Ready for QA]

### Move a bug backward or to Blocked

9. If QA finds the fix incomplete, open the bug, set `Status` back to `In Progress`, and add a comment explaining the failure with reproduction steps.
10. If a bug is blocked, set `Status` to `Blocked` and add a comment naming the blocker (for example: "Blocked: waiting for `acme-platform/payments-api` schema change in `2026.05.0`"). Apply an `Owner` who is responsible for clearing the blocker.

### Review the board for stuck bugs

11. Switch the view to **Table** layout grouped by `Status`. Sort each group by issue age (use the issue creation date column or filter by `updated:<2026-04-29` for items not touched in the last week).
12. Walk each column and compare it to the table below. Flag any bug that fits a "What to Escalate" row.

> [SCREENSHOT: table view grouped by Status with stale items highlighted]

## What Good Looks Like vs. What to Escalate

| `Status` column | What good looks like | What to escalate |
|---|---|---|
| `Backlog` | New bugs reach `Ready` within one business day for `Severity 1`, three days for `Severity 2`, one sprint for `Severity 3-4`. | A `Severity 1` bug older than one day with no owner. |
| `Ready` | Has `Owner`, `Severity`, `Priority`, `Product Area`, and a target `Sprint` or `Release`. | Bug ready for more than two sprints with no `Sprint` assignment. |
| `In Progress` | Has an assignee and an open pull request within three working days. | In Progress for more than five working days with no linked PR. |
| `In Review` | Linked PR has at least one approval and passing checks. | PR open more than three working days with no review. |
| `Ready for QA` | Picked up by QA within one working day for `Severity 1-2`. | Sitting more than three working days for any severity. |
| `Verified` | Closed and moved to `Done` when its `Release` ships. | Verified for a release that has already shipped but still open. |
| `Blocked` | Has a comment naming the blocker and an owner accountable for clearing it. | No blocker comment, or blocker older than one sprint. |

## Validation Checklist

- [ ] The bug's `Status` matches the actual defect lifecycle stage.
- [ ] The bug appears in the correct board column.
- [ ] Required fields for the new stage are populated (see step 7).
- [ ] If `Status` is `Blocked`, a comment names the blocker and an owner is set.
- [ ] If `Status` is `Done`, the issue is closed and the `Release` field is set.
- [ ] No `Severity 1` bug is older than one business day in `Backlog`.

## Common Mistakes

- Using a label such as `qa-ready` instead of the `Status` field. Labels do not drive the board; the board groups by `Status`.
- Setting `Status` to `Done` while the issue remains open. Close the issue when you mark it `Done`.
- Skipping `In Review` and going straight from `In Progress` to `Ready for QA`. The board loses code review traceability.
- Treating `Blocked` as a permanent home. `Blocked` is a holding state with an owner and a clearing date, not a parking lot.
- Moving a bug to `Verified` without a QA comment recording the test environment.
- Confusing the issue's open/closed state with `Status`. They are independent: only `Done` should coincide with a closed issue.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: When the `Status` field values are missing, mismatched, or out of order, request a Project admin to fix the field configuration.
- Engineering lead: When a bug sits in `In Progress` or `In Review` past the thresholds in the table above, raise it with the engineering lead for the affected `Product Area`.
- Release manager: When a bug marked `Done` has a `Release` value that does not match what actually shipped, or when a `Severity 1` bug is `Blocked` close to a release date.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-035 : How to Distinguish Severity from Priority
- GHE-ALM-037 : How to Attach Evidence to a Bug
- GHE-ALM-039 : How to Run a Weekly Bug Review
