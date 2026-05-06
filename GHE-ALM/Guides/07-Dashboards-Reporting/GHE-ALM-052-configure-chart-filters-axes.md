# How to Configure Chart Filters and Axes

**Guide ID:** GHE-ALM-052
**Audience:** Project Manager, Engineering Manager, Program Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes per chart
**Required permissions:** Project: Write

**Prerequisites:**

- An organization-level GitHub Project containing the items you want to chart.
- A chart already created in the **Insights** area of the Project (see GHE-ALM-051).
- Project fields populated for the dimensions you intend to chart, for example `Status`, `Priority`, `Severity`, `Sprint`, `Release`, `Product Area`, `Owner`, `Effort`.

**When to use this guide:** Use this guide when an existing Insights chart is not answering the management question you have, or when you are building a new chart and need to set the filter, axis, grouping, and aggregation precisely.

**When not to use this guide:** Do not use this guide for first-time chart creation; start with GHE-ALM-051. Do not use this guide to compare scope or completion across time periods; use GHE-ALM-053 for historical and burn-up charts.

## Outcome

By the end of this guide, you will have produced:

- A configured Insights chart with an explicit filter, X-axis, optional Group by, and Y-axis aggregation.
- A chart whose title and configuration match a single, stated management question.
- A saved chart visible to other Project members under **Insights**.

## Before You Start

- Write down the management question the chart must answer in one sentence. Example: "How many open bugs do we have in each severity, by product area, this sprint?"
- Confirm the field names you will use match the canonical project fields (`Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`).
- Confirm you have **Project: Write** access. **Project: Read** can view charts but cannot edit them.

## Steps

### Open the chart for editing

1. Open the Project, then click the graph icon in the top-right corner of the Project to open **Insights**.
2. In the left sidebar, select the chart you want to configure, or create a new chart from a template.
3. Click **Configure** to open the chart configuration panel.

> [SCREENSHOT: Insights sidebar with a chart selected and the Configure button visible]

### Choose the chart type

4. Open the **Layout** dropdown and pick the chart type that matches the question:
   - **Column**: a snapshot count or sum across one categorical dimension. Best for "how many items per Status / Severity / Owner right now".
   - **Stacked area**: a stacked snapshot across one categorical dimension with a Group by applied. Best for "composition right now", for example open bugs by severity, stacked by product area.
   - **Line**: a snapshot trend across an ordered dimension such as `Sprint`. Best for "how does this number change as we move across sprints".
5. If you need a time-series view of open vs closed vs not planned over calendar dates, stop here and use a historical chart instead (see GHE-ALM-053).

### Set the filter

6. In the **Filter** field, enter the locked search syntax that scopes the chart. Combine clauses with spaces; all clauses are AND'd together. Common clauses:
   - `is:open` or `is:closed`
   - `type:Bug`, `type:Feature`, `type:Requirement`, `type:Task`, `type:Epic`
   - `sprint:@current`, `sprint:@next`, `sprint:@previous`
   - `release:"2026.05.0"`
   - `priority:P0`, `priority:P1`
   - `severity:1`, `severity:2`
   - `product-area:Checkout`
   - `assignee:@me` or `assignee:octocat`
   - `no:Sprint`, `no:assignee`
7. Quote any value that contains a space, for example `release:"2026 Q3 Release"`.
8. Confirm the filter resolves to a sensible row count. If the chart shows zero items, the filter is wrong before the axes are wrong.

### Set the X-axis

9. Open the **X-axis** dropdown and pick the categorical or ordered field that forms the bars, areas, or line points:
   - For "by category right now": pick `Status`, `Priority`, `Severity`, `Product Area`, `Owner`, or `Type`.
   - For "across sprints": pick `Sprint`. The X-axis will order sprints chronologically.
   - For "across releases": pick `Release`.
10. If the X-axis is a free-text or high-cardinality field such as `Title` or `Assignee`, the chart becomes unreadable. Pick a single-select, iteration, or low-cardinality field instead.

### Set the Group by

11. Open the **Group by** dropdown to add a second dimension that splits each X-axis bar or area into stacked segments. Common choices: `Owner`, `Product Area`, `Severity`, `Type`.
12. Select **None** if a single dimension is enough. Stacked area always requires a Group by; Column and Line do not.

### Set the Y-axis aggregation

13. Open the **Y-axis** dropdown to choose what is being measured:
    - **Count of items** is the default. One issue equals one unit.
    - For a quantitative roll-up, switch to a number field such as `Effort`, then pick **Sum**, **Average**, **Minimum**, or **Maximum**.
14. If the Y-axis aggregation is **Sum of Effort**, confirm `Effort` is populated on the items in scope. Items with empty `Effort` contribute zero and silently distort the total.

> [SCREENSHOT: Configuration panel showing Layout, Filter, X-axis, Group by, and Y-axis dropdowns set together]

### Name and save the chart

15. Set the chart title to restate the management question, for example **Open bugs by severity, this sprint**. A vague title is the most common reason a chart gets misread later.
16. Click **Save changes**. The chart now refreshes for every Project member with **Project: Read**.

> [SCREENSHOT: Saved chart with descriptive title visible in the Insights sidebar]

### Worked example 1: Open bugs by severity over time

17. Question: "Are open bug counts for each severity trending up or down across sprints?"
    - **Layout**: Line.
    - **Filter**: `is:open type:Bug`
    - **X-axis**: `Sprint`
    - **Group by**: `Severity`
    - **Y-axis**: Count of items
    - **Title**: **Open bugs by severity across sprints**

### Worked example 2: Effort committed by sprint

18. Question: "How much effort did the team commit to each upcoming sprint?"
    - **Layout**: Column.
    - **Filter**: `is:open -sprint:@previous`
    - **X-axis**: `Sprint`
    - **Group by**: None
    - **Y-axis**: Sum of `Effort`
    - **Title**: **Committed effort by sprint**

### Worked example 3: Throughput by Product Area

19. Question: "Which product areas are completing the most work this sprint?"
    - **Layout**: Column.
    - **Filter**: `is:closed sprint:@current`
    - **X-axis**: `Product Area`
    - **Group by**: `Type`
    - **Y-axis**: Count of items
    - **Title**: **Closed items this sprint by product area and type**

## Validation Checklist

- [ ] The chart title restates the question the chart is meant to answer.
- [ ] The **Filter** uses canonical fields and resolves to a non-zero, sensible row count.
- [ ] The **X-axis** is a low-cardinality or ordered field, not a free-text field.
- [ ] **Stacked area** charts have a **Group by** set; **Column** and **Line** charts use Group by only when a second dimension is needed.
- [ ] The **Y-axis** aggregation matches the question (count for headcount-style questions, sum for capacity, average for cycle-time-style questions).
- [ ] If the Y-axis is sum or average of a number field, that field is populated for the items in scope.
- [ ] The chart is saved and visible to other Project members.

## Common Mistakes

- Picking a high-cardinality X-axis such as `Assignee` or `Title`, producing a chart with too many bars to read.
- Forgetting that **Stacked area** requires a Group by, then concluding the chart "is broken".
- Using **Sum of Effort** when many in-scope items have an empty `Effort`, producing a misleading total.
- Mixing closed and open items in a single chart when only one or the other was intended. Add `is:open` or `is:closed` to the filter explicitly.
- Filtering by `sprint:@current` and expecting the chart to keep meaning two sprints later. The filter is dynamic; the historical record needs a historical chart (see GHE-ALM-053).
- Renaming a project field after a chart is built, which silently breaks the chart's axis or group reference.
- Using marketing-style chart titles such as "Quality dashboard" instead of the actual question being answered.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable.
- Engineering lead: Involve when chart axes require a number field (such as `Effort`) that the team has not been populating; engineering lead must decide whether to backfill or change practice.
- Release manager: Involve when chart filters reference `Release` values that are inconsistently set across repositories in a release train.

## Related Guides

- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-053 : How to Use Historical Charts and Burn-Up Views
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-058 : How to Use Saved Views for Stakeholder Reporting
