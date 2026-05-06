# How to Use Effort or Story Points in GitHub Projects

**Guide ID:** GHE-ALM-033
**Audience:** Engineering Manager, Project Manager, Scrum Master
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 20-30 minutes the first time, 5 minutes per sprint thereafter
**Required permissions:** Project: Write to set Effort on items; Project: Admin to add or rename the Effort field
**Prerequisites:**

- The organization Project has an `Effort` number field, or you have permission to add one.
- A `Sprint` iteration field exists and the team has agreed on sprint length.
- The team has agreed on a single estimation scale and what one unit represents.

**When to use this guide:** Use this guide when planning a sprint, reviewing whether the sprint is overcommitted or undercommitted, or building a chart that shows effort by sprint, owner, or product area.

**When not to use this guide:** Do not use Effort to score individual contributors, to forecast a fixed delivery date for an unestimated backlog, or to compare velocity across teams that use different scales.

## Outcome

By the end of this guide, you will have produced:

- An `Effort` value on every issue committed to the upcoming sprint.
- A sprint scope total expressed in points or hours.
- A capacity-versus-commitment statement the team can defend at sprint planning.
- An Insights chart that sums `Effort` by `Sprint`, `Status`, or `Owner`.

## Before You Start

- Confirm the canonical field name is `Effort`. Do not create parallel fields named `Story Points`, `Estimate`, or `Size`.
- Confirm the scale. Two common choices: a Fibonacci-like scale (1, 2, 3, 5, 8, 13) for relative story points, or whole-hour values for time-based teams. Both are illustrative; pick one.
- Know the team's nominal capacity for the sprint in the same units. For example, six engineers at thirty available hours each gives 180 hours, or a team that historically completes 35 points per two-week sprint has a 35-point capacity.
- Review GHE-ALM-022 to confirm sprint candidate hygiene before you estimate.

## Steps

### Add or confirm the Effort field

1. Open the organization Project. Click the view tab you use for sprint planning.
2. Open any item in the side panel. Scroll the field list. If `Effort` is present as a number field, skip to step 5.
3. If `Effort` is missing, click the project menu (three dots, top right), then **Settings**, then **+ New field**.
4. Set **Field name** to `Effort`, set **Field type** to **Number**, then click **Save**. The field now appears on every item in the Project.

> [SCREENSHOT: Project Settings showing the new field dialog with name Effort and type Number selected.]

### Estimate the sprint candidate list

5. Return to your sprint planning view. Filter to the candidate list, for example `sprint:@next` combined with `no:Effort` to see unestimated work first.
6. For each unestimated issue, open the side panel. Read the title, acceptance criteria, and any sub-issues. Confirm the work is small enough to fit in one sprint. If it is not, split it before estimating.
7. Click into the **Effort** cell. Type the agreed value (for example `3` on a Fibonacci scale, or `8` for eight hours). Press Enter.
8. If the team is estimating together, run a quick round per item. Capture the agreed number. Do not record a range; record a single value.
9. Repeat until the `no:Effort` filter shows zero items in the candidate list.

> [SCREENSHOT: Sprint planning table with the Effort column populated, filtered by sprint:@next.]

### Sum the sprint and compare to capacity

10. Group the view by `Sprint`. Most table views show a count and a sum for number fields in the group header. Read the `Effort` total for the upcoming sprint.
11. Compare the total to the team's nominal capacity. As a starting heuristic, commit to no more than 80 percent of nominal capacity to leave room for support work, code review, and the inevitable carryover from defects. Adjust the percentage as the team's actual completion data accrues.
12. If the total exceeds capacity, move the lowest-priority items out of `sprint:@next`. If the total is well under capacity, pull the next-highest-priority backlog items in and estimate them.
13. Record the final committed total in the sprint planning notes or the Sprint field description so the team can refer back to it at sprint review.

### Build an Insights chart for ongoing review

14. From the Project, click the chart icon to open **Insights**. Click **New chart**, then **Configure**.
15. Set **Layout** to a column or stacked column chart. Set **X-axis** to `Sprint`. Set **Y-axis** to `Effort` and choose **Sum** as the aggregation.
16. Optionally set **Group by** to `Status` so each sprint column splits into Done, In Progress, and the rest. Save the chart with a name such as `Effort by Sprint, grouped by Status`.
17. Open this chart at sprint review and at the start of sprint planning. Two patterns to watch: the column for the just-closed sprint is much shorter than the commitment (carryover or overcommit), or the Done segment is consistently a small fraction of the column (estimation drift).

> [SCREENSHOT: Insights chart showing Effort summed by Sprint with Status as the group-by series.]

## What Good Looks Like vs. What to Escalate

The reviewer half of this activity is reading the same chart and the sprint table with a critical eye.

| Signal | What good looks like | What to escalate |
|---|---|---|
| Coverage | Every committed item has an `Effort` value. | More than 10 percent of committed items have no `Effort`. Revisit GHE-ALM-022 hygiene before commitment. |
| Distribution | A spread of values across the scale. | Every item is the same value (for example, every item is `3`). The team is anchoring rather than estimating. |
| Sprint total | Within roughly plus or minus 15 percent of the recent rolling average. | Sudden jumps or drops without a staffing change. Suggests scope-padding, sandbagging, or an undisclosed capacity change. |
| Done segment | Done segment grows steadily across the sprint and ends near the committed total. | Done segment stays flat until the final two days, then jumps. Work is being marked Done in a batch rather than as it completes. |
| Carryover | Some carryover is normal. One or two items per sprint. | Same items carry forward across three or more sprints. Either the items are too large or they are blocked. Investigate before re-committing. |
| Capacity drift | Commitment tracks roughly with available headcount. | Commitment is flat while headcount drops, or the reverse. The team is not adjusting for capacity. |

If you see two or more escalation signals in the same sprint, raise the topic at the next retrospective rather than at sprint planning. Estimation conversations during planning eat the planning meeting.

## Validation Checklist

- [ ] `Effort` exists as a number field on the Project.
- [ ] Every issue in `sprint:@next` has an `Effort` value.
- [ ] The summed `Effort` for `sprint:@next` is recorded and compared to a stated capacity.
- [ ] An Insights chart exists for `Effort` summed by `Sprint`.
- [ ] The team can answer "how big is this sprint" with one number.

## Common Mistakes

- Treating the scale as time when the team agreed to use relative points, or the reverse. Pick one and stay with it.
- Re-estimating items mid-sprint to make the burndown look better. The estimate is set at commitment and stays.
- Comparing point velocity across teams. Points are calibrated per team and do not transfer.
- Using `Effort` as a performance metric for individuals. It will distort the estimates within one sprint.
- Adding a second number field called `Story Points` alongside `Effort`. Reporting will split and neither field will be reliable.
- Estimating everything as the same value (`3`, `5`) to move planning along. The sum is then meaningless.
- Forgetting to estimate sub-issues when the parent is the only thing assigned to the sprint. Decide one rule (estimate the parent, or estimate the leaves) and apply it consistently.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable.
- Engineering lead: Involve when sandbagging or capacity drift persists across two or more sprints, or when the team disputes the scale.
- Release manager: Involve when sprint commitment trends downward in the run-up to a release date and scope adjustment is needed.

## Related Guides

- GHE-ALM-022 : How to Manage Issue Hygiene Before Sprint Commitment
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-032 : How to Close a Sprint Review
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
