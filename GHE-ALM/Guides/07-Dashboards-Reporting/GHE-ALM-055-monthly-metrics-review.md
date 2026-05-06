# How to Run a Monthly ALM Metrics Review

**Guide ID:** GHE-ALM-055
**Audience:** Engineering Manager, Program Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 60-90 minutes per month, plus 30 minutes pre-meeting prep
**Required permissions:** Project: Read on the organization Project; Repository: Read on contributing repositories

**Prerequisites:**

- An organization-level Project with at least three months of historical data.
- Insights charts already configured per GHE-ALM-051 and GHE-ALM-053.
- A standing monthly meeting on the calendar with engineering, product, QA, and release leads.

**When to use this guide:** Run this review once a calendar month to inspect trends across releases, product areas, defects, and sprint completion, and to confirm that the program is moving in the right direction over time.

**When not to use this guide:** Do not use this for day-to-day status. Use GHE-ALM-054 for the weekly dashboard review, which looks at current state instead of trends.

## Outcome

By the end of this guide, you will have produced:

- A signed-off monthly metrics readout covering six standard charts.
- A short written summary of trend signals, including any reversals or concerning slopes.
- A list of action items with owners and target dates, captured in the Project or in a tracked issue.
- Screenshots of each reviewed chart for the monthly governance record.

## Before You Start

- Confirm last month's action items list is reachable; you will close or roll forward each one.
- Refresh chart data the day of the meeting so the screenshots match what attendees see.
- Block 30 minutes the morning of the meeting to walk through every chart yourself before presenting.
- Have a place to capture action items, typically a recurring tracking issue labeled `governance` or a saved view.

## Steps

### Prepare the data and the deck

1. Open the organization Project, for example `acme-platform / ALM Program`.
2. Open the **Insights** tab. Confirm the six standard charts exist: Open work by **Release**, Work by **Sprint**, Bugs by **Severity**, Work by **Product Area**, Completed vs Remaining, and Open vs Closed trend. If any are missing, create them now using GHE-ALM-051; do not run the meeting with partial data.
3. For each chart, set the time window to the trailing three months at minimum. Trend reviews require at least three data points; one or two points is current state, not a trend.
4. Apply the standard filter set so the charts show in-scope work only. A typical filter is `is:issue -label:duplicate -label:invalid`. Document the filter in the chart description so future reviewers see the same numbers.
5. Capture a screenshot of each chart at full resolution. Save them to the monthly governance folder using the naming pattern `YYYY-MM-<chart-slug>.png`, for example `2026-05-bugs-by-severity.png`.

> [SCREENSHOT: Project Insights tab showing the six standard charts as tiles, with the trailing three-month window selected]

### Review each metric in the meeting

6. **Open the meeting** with last month's action item list. For each item, mark it Done, In Progress, or Rolled Forward, and assign a new target date if it is rolling forward.
7. Walk **Open work by Release**. Compare the current bar heights for each release against the prior month. Flag any release where open work grew without a corresponding scope decision. Ask the release manager to confirm scope changes are intentional.
8. Walk **Work by Sprint** as a completion-history view. For the last three to six sprints, note the completed-versus-committed ratio. A sustained ratio under roughly 70 percent indicates either over-commitment or capacity loss; ask the relevant engineering manager to comment.
9. Walk **Bugs by Severity** as a stacked bar over time. Focus on Severity 1 and 2 trends. Rising P0/P1 bug counts over two or more months is a quality signal; route to the QA Manager for root cause discussion.
10. Walk **Work by Product Area**. Identify any product area whose open count is growing faster than its closed count. Confirm with the product owner whether this reflects intentional investment or unmanaged intake.
11. Walk **Completed vs Remaining** for the in-flight release. Compare the slope of completed work against calendar time remaining. If remaining work is flat or growing, the release date is at risk; mark a follow-up for GHE-ALM-046 release readiness.
12. Walk the **Open vs Closed trend**. Open should track or trail Closed over a healthy month. If Open consistently outpaces Closed, intake exceeds throughput and the backlog is growing.

> [SCREENSHOT: A historical Bugs by Severity chart over three months with Severity 1 and 2 series highlighted]

### Capture decisions and follow up

13. For each chart that surfaced a concern, record an action item with a single owner, a one-sentence description, and a target date no later than the next monthly review.
14. Save the action items in your tracking issue or saved view. A common pattern is a recurring issue titled `Monthly ALM Review YYYY-MM` with a checklist body.
15. Attach the six chart screenshots to the tracking issue so the governance record is self-contained.
16. Send a short readout to stakeholders within one business day. Cover what changed, what is concerning, and what was decided. Do not paste the screenshots into chat; link to the tracking issue.

> [SCREENSHOT: Tracking issue with screenshots attached and an action item checklist visible]

## Validation Checklist

- [ ] All six standard charts were reviewed with a trailing window of at least three months.
- [ ] Last month's action items were each closed or explicitly rolled forward.
- [ ] Each new action item has a single named owner and a target date.
- [ ] Chart screenshots are saved with the standard filename pattern.
- [ ] A written readout was sent to stakeholders within one business day.

## Common Mistakes

- Running the review on a single month of data and calling it a trend. Trends require at least three points.
- Mixing weekly status into the monthly review. Current sprint detail belongs in GHE-ALM-054.
- Letting action items accumulate across months without owners. Untracked items defeat the governance purpose.
- Filtering out severity or product area data inconsistently month to month, which creates fake trend movement.
- Treating a one-month dip in completion as a problem. Look for the direction over three or more sprints before raising it.
- Assuming the illustrative severity scale (a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership) matches every team's labels.

## Escalation Path

- GitHub administrator: Involve when Insights charts cannot be created or saved due to organization-level Project permissions.
- Repository administrator: Involve when contributing repositories are missing from the organization Project and trend data is incomplete.
- Engineering lead: Involve when sprint completion ratios stay below the team's commitment threshold for two or more months.
- Release manager: Involve when Open work by Release or Completed vs Remaining indicates a release date is at risk.

## Related Guides

- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-053 : How to Use Historical Charts and Burn-Up Views
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-056 : How to Identify Reporting Gaps That Require BI or External Tools
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
