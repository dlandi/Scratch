# How to Create a Task Issue

**Guide ID:** GHE-ALM-015
**Audience:** Project Manager, Engineering Manager, Scrum Master
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 5-10 minutes per use
**Required permissions:** Repository: Triage; Project: Write

**Prerequisites:**

- A parent Feature, Requirement, or Epic issue already exists.
- The repository has the `Task` issue type enabled (set at the organization level).
- The ALM Project contains the `Status`, `Priority`, `Effort`, `Sprint`, and `Owner` fields.

**When to use this guide:** Use this guide when you need to capture a single, trackable unit of work that contributes to a larger Feature or Requirement. A Task can be implementation work (engineer hours) or non-code work such as documentation, infrastructure changes, or training delivery.

**When not to use this guide:** Do not use a Task for tiny checklist items inside another issue; use a markdown task list inside the parent issue body instead. Do not use a Task for a defect; file a Bug instead (see GHE-ALM-014).

## Outcome

By the end of this guide, you will have produced:

- A Task issue with a clear title, single-sentence acceptance criterion, owner, priority, effort estimate, and sprint assignment.
- A parent link from the Task to its Feature, Requirement, or Epic, established through the sub-issue mechanism.
- A Task that appears in the correct ALM Project view and rolls up under the parent in Hierarchy View.

## Before You Start

- Confirm the parent issue (Feature, Requirement, or Epic) exists and is in the correct repository.
- Decide who will own the Task. A Task without an `Owner` cannot be sprint-committed.
- Have a one-sentence acceptance criterion ready. If you cannot state it in one sentence, the Task is too large and should be broken down further or kept as a Feature for later decomposition.
- Confirm the Task can fit inside one sprint. If it cannot, split it.

## Steps

### Open the parent and create the sub-issue

1. Open the parent issue in the source repository, for example a Requirement in `acme-payments/checkout-service`.
2. Scroll to the **Sub-issues** section at the bottom of the parent issue description.
3. Click **Create sub-issue**. A new issue dialog opens with the parent link already established.
4. Enter the Task title using the convention `[Task] <verb-led short description>`, for example `[Task] Add idempotency key to refund endpoint` or `[Task] Draft operator runbook for refund replay`.
5. In the **Type** selector, choose **Task**.
6. In the body, write a single-sentence acceptance criterion under an `## Acceptance Criteria` heading. Example: `Refund endpoint rejects duplicate requests with the same idempotency key within a 24-hour window and returns the original response.`
7. Click **Create**. The new Task is created and linked under the parent.

> [SCREENSHOT: Parent Requirement issue with the Sub-issues section expanded and the Create sub-issue button highlighted.]

### Set issue metadata

8. On the new Task, set **Assignees** to the engineer or contributor who will perform the work. The assignee and the project field `Owner` should match.
9. Apply any secondary classification labels your team uses, for example `area:checkout` or `type:non-code`. Do not use labels to substitute for the issue type; the type is already `Task`.
10. Set the repository **Milestone** if your team scopes Tasks to release milestones. Leave blank if release scope is tracked only on the parent.

> [SCREENSHOT: Task issue sidebar showing Assignees, Labels, and the Task type badge.]

### Add the Task to the ALM Project and set project fields

11. In the Task sidebar, under **Projects**, click **Add to project** and select the ALM Project, for example `acme-payments ALM`.
12. Open the Task in the project (or use the project side panel) and set the following project fields:
    - `Status`: `Backlog` for new work, or `Ready` if the Task is groomed and ready for sprint commitment.
    - `Owner`: same person as the GitHub assignee.
    - `Priority`: `P0`, `P1`, `P2`, or `P3`. Use a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership.
    - `Effort`: story points or hour band per your team's convention.
    - `Sprint`: the iteration the Task is committed to, or leave blank if still in the backlog. Use the filter `sprint:@current` later to confirm placement.
    - `Product Area`: for example `Checkout`.
    - `Release`: the target release, for example `2026.05.0`, if known.

> [SCREENSHOT: Project side panel for the Task showing Status, Owner, Priority, Effort, Sprint, Product Area, and Release populated.]

### Verify hierarchy and visibility

13. Return to the parent issue and confirm the Task appears under **Sub-issues** with its type badge and current status.
14. Open the ALM Project's Hierarchy View and confirm the Task nests under the parent Feature or Requirement.

## Validation Checklist

- [ ] Task title starts with `[Task]` and uses a verb-led short description.
- [ ] Issue type is `Task`.
- [ ] Acceptance criterion is a single sentence under an `## Acceptance Criteria` heading.
- [ ] Parent link is visible on the Task and the Task is visible under the parent's Sub-issues section.
- [ ] `Assignees` and project field `Owner` match.
- [ ] `Priority`, `Effort`, and `Status` are set.
- [ ] Task appears in the ALM Project and nests correctly in Hierarchy View.
- [ ] Task is small enough to complete inside one sprint.

## Common Mistakes

- Creating a Task with no parent. Tasks should always link to a Feature, Requirement, or Epic so work rolls up.
- Using a Task issue for what should be a checklist line. If the item takes under an hour and does not need its own owner, sprint, or status, put it in the parent's task list instead.
- Multi-sentence or vague acceptance criteria. If the criterion needs more than one sentence, the Task is doing two things; split it.
- Setting `Owner` in the project but leaving the GitHub `Assignees` empty. Notifications and review routing rely on `Assignees`.
- Using labels such as `task` or `subtask` instead of the `Task` issue type. Issue type is the governed field; labels are for secondary classification only.
- Putting a Task directly in the current sprint without first marking `Status: Ready`. Sprint commitment requires hygiene per GHE-ALM-022.

## Escalation Path

- GitHub administrator: when the `Task` issue type is missing from the type picker, or when organization-level fields such as `Effort` or `Sprint` are not available on the Project.
- Repository administrator: when you cannot add the Task to the repository or cannot set the milestone.
- Engineering lead: when the parent Feature or Requirement does not exist, or when the Task as scoped does not fit in one sprint and needs technical decomposition.
- Release manager: when the Task affects a committed release and the `Release` field needs to change after sprint planning.

## Related Guides

- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-012 : How to Create a Requirement Issue
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-020 : How to Apply Issue Metadata Correctly
- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
