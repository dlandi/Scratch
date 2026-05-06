# How to Use Historical Charts and Burn-Up Views

**Guide ID:** GHE-ALM-053
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes per use
**Required permissions:** Project: Read to view charts; Project: Write to create or modify charts

**Prerequisites:**

- An organization-level GitHub Project with at least two weeks of item history.
- A `Status` field whose values map to Open, Completed, and Not planned.
- Familiarity with the chart-building workflow in GHE-ALM-051 and GHE-ALM-052.

**When to use this guide:** Use when you need to see how scope and completed work have moved over time, not just where they stand today. Typical uses include sprint retrospectives, mid-release health checks, and explaining scope creep to stakeholders.

**When not to use this guide:** Do not use this guide for a present-moment snapshot of work distribution by owner, severity, or product area. For that, configure a current chart using GHE-ALM-052.

## Outcome

By the end of this guide, you will have produced:

- A working burn-up chart for a sprint or release window.
- A documented reading of the scope line, completed line, and not-planned segment.
- A clear interpretation of whether the trend is healthy, scope-creeping, or stalled.

## Before You Start

- Confirm the Project contains the items you want to measure. Insights does not track archived or deleted items, so a heavily archived Project will under-report history.
- Decide the time window: a single sprint (commonly two weeks), a release window, or a quarter.
- Confirm `Status` values are stable. If `Status` was renamed mid-sprint, the historical line will show artificial discontinuities.

## Steps

### Open the default burn-up chart

1. Open the Project.
2. In the left sidebar, click **Insights**.
3. Under the chart list, select the chart named **Burn up**. This is the default historical chart shipped with every Project.

> [SCREENSHOT: Project Insights sidebar with Burn up selected and the chart rendered]

### Read the chart

4. Identify the top line, which represents total scope: every item in the filter set, including Open, Completed, and Not planned.
5. Identify the bottom line, which represents Completed work over time.
6. Identify the gap between the lines, which represents work still to do plus any items moved to Not planned.
7. Hover over a date on the X-axis to read exact counts for that day. Use this to confirm specific events such as a scope addition or a batch closure.

### Adjust the time window and filter

8. Click the date-range control above the chart. Set the start date to the sprint or release start and the end date to today, or the sprint end if reviewing a closed sprint.
9. In the filter bar, narrow to the relevant slice. Common filters: `sprint:@current`, `sprint:"Sprint 27"`, or `release:"2026.05.0"`.
10. If you want to count effort instead of items, open the chart configuration and set the Y-axis aggregation to sum the `Effort` field. Item count is the default.

> [SCREENSHOT: Burn-up chart filtered to sprint:@current with date range set to the sprint window]

### Interpret the trend

11. Compare the shape of the two lines against the patterns in the next section. Decide whether to act, escalate, or do nothing.
12. Capture the screenshot or export the underlying data via GHE-ALM-057 if you need to attach the chart to a sprint review or release readiness pack.

## Patterns to Recognize

| Pattern | What you see | What it means | Action |
|---|---|---|---|
| Healthy burn-up | Top line flat or nearly flat. Bottom line rises steadily and converges with the top by the end of the window. | Scope was stable. The team completed planned work at a predictable rate. | None. Note in the sprint review. |
| Scope creep | Top line rises during the window. Gap between lines stays the same or widens. | New items were added to the sprint or release after commitment. | Investigate the additions. Confirm they are emergencies, not undisciplined intake. See GHE-ALM-022. |
| Stalled progress | Top line flat. Bottom line flat or rising very slowly. | Work was committed but is not closing. Likely blockers, dependency chains, or Status discipline gaps. | Open the current sprint board and review blocked items. See GHE-ALM-031. |
| Late completion surge | Bottom line flat for most of the window, then jumps near the end. | Items were not transitioning to Done in real time, then got bulk-closed. | Reinforce daily Status updates. Consider whether items were truly Done when closed. |
| Rising Not planned | Stacked Not planned segment grows over the window. | Significant scope was deferred, not completed. | Confirm deferrals were intentional. Review with product owner. |

## Historical vs Current Snapshot Charts

Historical charts use **Time** as the X-axis and are derived from item state changes recorded by the Project. They answer questions about trend and trajectory.

Current snapshot charts use a non-time field (such as `Status`, `Owner`, or `Sprint`) on the X-axis. They answer questions about the present distribution of work.

Use the burn-up when you need to explain how the team got to where it is. Use a current chart from GHE-ALM-052 when you need a single-point-in-time picture for a leadership update.

## Worked Example

The Engineering Manager for `acme-checkout` opens the `Sprint 27` burn-up on the Friday of week two. The top line started at 32 items on Monday of week one and now reads 41. The bottom line reads 28. The gap of 13 includes 4 items moved to Not planned earlier in the week.

The manager interprets this as scope creep with partial recovery: 9 items were added mid-sprint, and the team absorbed roughly half the addition by deferring 4 lower-priority items. The manager flags the addition pattern in the sprint review and requests that mid-sprint additions go through the change request workflow next sprint, citing GHE-ALM-016.

## Validation Checklist

- [ ] The chart titled **Burn up** is visible under Insights.
- [ ] X-axis shows the correct date range for the sprint or release.
- [ ] Top line and bottom line are both present and labeled.
- [ ] Filter expression on the chart matches the intended scope (sprint or release).
- [ ] At least one pattern from the table above can be matched to the chart.

## Common Mistakes

- Reading the burn-up as if it were a burn-down. The bottom line rises in a burn-up; it does not fall.
- Forgetting that archived items are excluded. A Project with aggressive auto-archive will show truncated history.
- Filtering on a field that has changed values mid-window, which produces apparent discontinuities. Use stable fields like `Sprint` or `Release`.
- Confusing scope creep with healthy late discovery. Both look like a rising top line. Use the timeline of the underlying issues to distinguish.
- Treating a single chart as a verdict. Cross-check with the current sprint board and the bug triage view before escalating.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable.
- Engineering lead: Involve when stalled progress or repeat scope creep appears across two or more sprints.
- Release manager: Involve when the burn-up for a release window shows the gap widening with fewer than two sprints remaining.

## Related Guides

- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-052 : How to Configure Chart Filters and Axes
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-055 : How to Run a Monthly ALM Metrics Review
