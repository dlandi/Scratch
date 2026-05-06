# How to Create and Interpret Project Insights Charts

**Guide ID:** GHE-ALM-051
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 30-minute one-time setup per chart, 5 minutes per weekly read
**Required permissions:** Project: Write
**Prerequisites:**

- A Project exists and contains items with populated fields for `Status`, `Sprint`, `Release`, `Product Area`, `Severity`, and `Owner`.
- You can open the Project in a browser.
- You know which management question the chart should answer before you build it.

**When to use this guide:** Use this guide when you need a chart that answers a recurring management question, such as how much work remains in the current release, how bugs are distributed across severity, or how scope is changing over time.

**When not to use this guide:** Do not use Project Insights for ad-hoc data exploration across multiple repositories with no shared Project, or for finance, headcount, or BI reporting. Those need an export or external tool. See GHE-ALM-056.

## Outcome

By the end of this guide, you will have produced:

- A clear reading of the default burn-up chart for one Project.
- One or more saved custom charts that answer a specific management question.
- A reusable interpretation pattern for trend, scope-creep, and slipping-completion signals.

## Before You Start

- Confirm the Project items have non-empty `Status`, `Sprint`, `Release`, `Product Area`, and `Severity` values. Charts grouped on empty fields produce empty or misleading bars.
- Decide the audience: leadership, release management, or your own team. Leadership charts should be few and high-signal.
- Decide whether the question is "right now" (current chart) or "over time" (historical chart). The configuration differs.
- Have the recommended chart list from section 8.7 of the GHE-ALM evaluation handy: Open work by Release, Work by Sprint, Bugs by Severity, Work by Product Area, Completed vs Remaining, Open vs Closed trend.

## Steps

### Open Insights and read the default chart

1. Open the Project in a browser. Click the graph icon in the top-right corner of the Project to open **Insights**.
2. The Insights side panel lists charts. The default chart is **Burn up**, a historical chart that plots completed and remaining work over time.
3. Read the Burn up chart in this order:
   - The total line is the sum of items in the Project. A rising total line means scope is being added (scope creep).
   - The completed line is items closed over time. A flat completed line means no work is finishing.
   - The gap between the two lines is remaining work. A gap that holds steady or widens means you will not land on the current trajectory.
4. Use the date range control above the chart to focus on the current release window or sprint window. A range that is too wide hides recent inflection points.

> [SCREENSHOT: Project Insights side panel with the default Burn up chart open and the date range control visible]

### Create a custom chart

5. In the Insights side panel, click **New chart**. A blank chart opens with the configuration panel on the right.
6. Give the chart a name that matches the management question. Use the recommended names verbatim so charts are recognizable across Projects: `Open work by Release`, `Work by Sprint`, `Bugs by Severity`, `Work by Product Area`, `Completed vs Remaining`, `Open vs Closed trend`.
7. In the **Layout** dropdown, pick the chart type. Use this mapping:

| Chart name | Chart type | X-axis | Group by | Question it answers |
|---|---|---|---|---|
| Open work by Release | Column | `Release` | `Status` | How much open work is committed to each release? |
| Work by Sprint | Column | `Sprint` | `Status` | How is each sprint loaded and how much is done? |
| Bugs by Severity | Column | `Severity` | `Status` | Where is the bug pressure concentrated? |
| Work by Product Area | Column | `Product Area` | `Status` | Which area carries the most open work? |
| Completed vs Remaining | Stacked area (historical) | Date | `Status` | Are we burning down toward zero remaining? |
| Open vs Closed trend | Line (historical) | Date | `Status` (Open vs Closed) | Is closure keeping pace with intake? |

8. Set the **X-axis** to the field shown in the table above.
9. Set **Group by** to the grouping field. Selecting `None` removes the secondary breakdown and produces a single bar per X-axis value.
10. For the Y-axis, leave the default count of items. If you track effort, switch the Y-axis to **Sum** of `Effort` so the chart reflects work size rather than item count.

### Filter the chart

11. Apply a filter to scope the chart to the work that matters. Common filters:
    - `Open work by Release`: filter `is:open` so the chart shows remaining commitments.
    - `Bugs by Severity`: filter `type:Bug is:open` to exclude features and closed bugs.
    - `Work by Sprint`: filter `sprint:@current` for the current iteration, or remove the filter to see history.
    - `Work by Product Area`: filter `is:open` for backlog pressure, drop the filter for total volume.
12. Save the chart with **Save changes**. The chart is now visible to anyone who can view the Project.

> [SCREENSHOT: New custom chart configuration panel showing Layout, X-axis, Group by, and Filter fields populated for Bugs by Severity]

### Interpret common patterns

13. Read each chart against a one-line expectation. If the chart matches, move on; if it does not, investigate.
14. Trend signals to act on:
    - **Flat completed line on Burn up.** Closure has stalled. Check for blocked items, pull-request backlog, or staffing gaps.
    - **Rising total line on Burn up.** Scope is being added after release commit. Confirm whether the additions are bug fixes, late requirements, or items that should move to the next release.
    - **Open vs Closed lines diverging.** Intake is outrunning closure. Sustained divergence means the backlog will grow until throughput changes.
    - **Bugs by Severity skewed to severity 1 / P0 or 2 / P1.** Quality is the constraint, not feature velocity. The illustrative scale (1 / P0 most severe through 4 / P3 least severe) is a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership.
    - **Work by Product Area concentrated on one area.** Either that area is under-staffed, or the release plan is unbalanced.
    - **Completed vs Remaining flat near the release date.** The release will slip on the current trajectory.

> [SCREENSHOT: Burn up chart annotated with rising total line and flat completed line indicating scope creep and stalled closure]

## Validation Checklist

- [ ] The Insights tab opens and the default Burn up chart renders for the date range you set.
- [ ] At least one custom chart is saved with a name from the recommended list.
- [ ] Each custom chart has a Layout, X-axis, Group by, and Filter that match the management question it answers.
- [ ] You can describe in one sentence what each chart should look like when the Project is healthy.
- [ ] Empty bars or a blank chart have been investigated. If the cause is empty field values on items, those items have been triaged and updated.

## Common Mistakes

- Building a chart before deciding the question. Charts without a question accumulate and stop being read.
- Grouping by a field that is empty on most items. The chart shows a large `No <Field>` bar that hides the real distribution.
- Reading a current chart as if it were historical. Current charts are a snapshot; only historical charts (such as Burn up) show change over time.
- Filtering out closed items on a trend chart. The Open vs Closed trend needs both states to be meaningful.
- Using item count when the team plans in effort. Switch the Y-axis to Sum of `Effort` for effort-based teams.
- Renaming canonical fields. Charts depend on the canonical names `Status`, `Sprint`, `Release`, `Product Area`, `Severity`, `Owner`.

## Escalation Path

- GitHub administrator: When Insights does not load, charts will not save, or permission errors block chart creation despite Project: Write access.
- Repository administrator: Not applicable. Insights is a Project-level feature.
- Engineering lead: When chart patterns indicate stalled closure, scope creep, or release slippage that requires a plan change.
- Release manager: When the Open work by Release or Completed vs Remaining chart shows the current release will not land on the target date.

## Related Guides

- GHE-ALM-052 : How to Configure Chart Filters and Axes
- GHE-ALM-053 : How to Use Historical Charts and Burn-Up Views
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-056 : How to Identify Reporting Gaps That Require BI or External Tools
