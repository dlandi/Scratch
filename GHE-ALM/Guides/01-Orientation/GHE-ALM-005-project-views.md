# How to Interpret GitHub Project Views

**Guide ID:** GHE-ALM-005
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Understands
**Estimated time:** 20-30 minutes one-time read
**Required permissions:** Project: `Read` to follow along in an existing Project. None required to read this guide.
**Prerequisites:**

- Read GHE-ALM-001 so the words `Issue`, `Project`, `Field`, and `Iteration` already mean the right thing.
- Have access to at least one existing organization-level Project. A populated Project makes the worked example easier to follow.

**When to use this guide:** Read this once before you create or use any Project view. It explains what each layout shows, what it hides, and which view answers which question, so you stop fighting the wrong layout.

**When not to use this guide:** Do not use this guide to build a specific view. For backlog work use GHE-ALM-026, for sprint execution use GHE-ALM-029, for hierarchy work use GHE-ALM-018, and for charts use GHE-ALM-051.

## Outcome

By the end of this guide, you will be able to:

- Name the five view types in GitHub Projects and describe what each one is for in one sentence.
- Pick the correct layout for backlog grooming, sprint execution, release planning, program rollup, and dashboards without trial and error.
- Recognize the most common pitfalls for each layout before you spend time configuring it.

## Before You Start

- Open one of your organization Projects in a second tab. You will not change anything; you will only look.
- Identify the fields the Project already has. The minimum useful set is `Status`, `Priority`, `Sprint`, `Release`, `Owner`, `Start Date`, and `Target Date`. If several of these are missing, some views will look empty even when work is present.
- Have one current question in mind, for example "what is in the next sprint?" or "what ships in 2026.05.0?" You will use it as the worked example in step 7.

## Steps

The sections below build on each other. Read them in order. Sections 2 through 6 each cover one view type; section 7 ties the choices together.

### 1. The five view types at a glance

A GitHub Project displays the same underlying items through different layouts. The five layouts are:

| Layout | What it is | Best at | Weak at |
|---|---|---|---|
| Table | Spreadsheet of items with custom fields as columns. | Editing many fields fast, filtering, sorting, bulk triage. | Showing time, parent/child nesting, or column-flow workflow. |
| Board | Kanban columns grouped by a single-select or iteration field. | Daily execution, blocker visibility, status flow. | Editing many fields, large backlogs, multi-field comparison. |
| Roadmap | Timeline using `Start Date` and `Target Date` (or an iteration field). | Cross-release scheduling, slippage spotting, milestone alignment. | Detail editing, items without dates. |
| Hierarchy | Table with parent/child sub-issues nested inline. (GA March 19, 2026.) | Epic-to-Task rollup, scope review, program inspection. | Day-to-day status flow, time-based planning. |
| Insights | Charts and metrics over Project data. | Leadership reporting, trend tracking, scope-vs-delivery review. | Operational work; nobody plans inside Insights. |

Two facts to keep in mind. First, you can have many views per Project, each with its own layout, filter, sort, group, and visible fields. Second, the items in every view are the same items; the layouts differ in how those items are presented and what fields are required for the layout to be meaningful.

### 2. Table view

A spreadsheet: rows are items, columns are fields. You can group, sort, filter, and bulk-edit. Most managers use Table more than any other.

Use Table for:

- Backlog grooming: adjust `Priority`, `Owner`, `Sprint`, or `Release` across many items.
- Sprint planning: pull candidates, set fields before commitment.
- Bug triage: sort by `Severity` and `Created`, assign.

Look at first:

- The filter bar. An empty filter means "everything," rarely what you want.
- The grouping. `Status`, `Priority`, `Product Area`, or `Sprint` each change what reads at a glance.
- Visible fields. A Table with 25 columns is unreadable; trim to the essentials.

Common pitfalls:

- Treating Table as the only view. It is the workhorse, not the whole shop.
- Stale filter syntax. The canonical iteration filter is `sprint:@current`, not `Sprint = @current`.
- Forgetting to save. Ad-hoc filters disappear on reload until you confirm the view.

### 3. Board view

Items in columns driven by a single-select field (usually `Status`) or an iteration field (`Sprint`). Dragging between columns updates the underlying field.

Use Board for:

- The current sprint, grouped by `Status`.
- Daily standups and blocker review.
- Short, time-boxed slices where flow matters more than detail.

Look at first:

- Which field drives the columns. If columns look wrong, the column field is wrong.
- The filter. A Board without one shows every item: noise, not a sprint board.
- The `Blocked` column, if you use one. Items camped there set the next standup agenda.

Common pitfalls:

- One giant board over the whole backlog. Board is for slices, not entire programs.
- Overloading status values. Eight is a lot; ten is too many. Illustrative scale: `Backlog`, `Ready`, `In Progress`, `In Review`, `Blocked`, `Ready for Release`, `Done`.
- Prescribing a column-walk direction in standups. Teams choose; this guide does not pick one.

### 4. Roadmap view

A timeline driven by `Start Date` and `Target Date`, or by an iteration field. Drag to change dates; toggle markers for iterations and key dates.

Use Roadmap for:

- Release scheduling across weeks or quarters.
- Cross-team plans where slippage and overlap must be visible.
- Milestone and iteration alignment review with leadership.

Look at first:

- Whether items have dates. Items missing both dates (or an iteration) will not appear.
- The grouping, often by `Release` or `Owner`.
- The marker layer; without it the timeline floats with no anchors.

Common pitfalls:

- Using Roadmap as a dashboard. It shows planned dates, not health.
- Confusing it with Azure DevOps Delivery Plans. Expect fewer built-in capacity, dependency, and rollup features.
- Letting items drift because of accidental drags. Use field-level history to catch this.

### 5. Hierarchy view

A Table with parent/child sub-issues nested inline. Generally available March 19, 2026. Sub-issues nest up to eight levels deep, 100 children per parent, so a full Initiative -> Epic -> Feature -> Requirement -> Task tree fits in one place.

Use Hierarchy for:

- Epic-to-Feature decomposition review.
- Feature-to-Requirement scope checks before sprint planning.
- Program-level rollup across repositories.
- Release scope review where the parent is a Feature and the children are Tasks.

Look at first:

- Whether parents have sub-issues. An empty view usually means the work is not yet decomposed, not that the view is broken.
- Visible fields. Useful columns: `Type`, `Status`, `Priority`, `Sprint`, `Release`, `Owner`, `Target Date`. Resist adding more.
- The filter. A heavy filter can hide children whose parent does not match; verify by expanding a known parent.

Common pitfalls:

- Treating the repository folder tree as the work hierarchy. They are unrelated; see GHE-ALM-004.
- Building hierarchy by labels instead of sub-issues. Labels do not roll up.
- Running a standup from Hierarchy view. Use a Board.

### 6. Insights view

The charting surface over Project data. It produces bar, line, column, and stacked charts from your fields and filters, plus a default burn-up progress chart.

Use Insights for:

- Leadership status: open work by `Release`, `Sprint`, or `Product Area`.
- Quality posture: bugs by `Severity`.
- Delivery posture: completed vs remaining over time, open vs closed trend.

Look at first:

- The chart's filter. A "release health" chart not filtered to the release is misleading.
- The grouping field. A chart grouped by the wrong field hides the answer.
- The time window. Trend charts compress or distort depending on range.

Common pitfalls:

- Expecting Azure DevOps parity. Native velocity, sprint burndown, burnup, and cumulative flow widgets are weaker than in Azure DevOps and may need API or BI extraction for PMO reporting.
- Building dashboards on stale fields. If `Status` or `Sprint` is not maintained, the chart lies cleanly.
- Sharing one chart out of context. Pair it with a saved Table or Board so readers can drill in.

> [SCREENSHOT: a single Project sidebar showing five saved views, one of each layout, named Product Backlog (Table), Current Sprint Board (Board), Release Roadmap (Roadmap), Hierarchy View, and Executive Dashboard (Insights).]

### 7. A worked example: pick the right view for one question

Assume the organization is `acme-payments` and the Project is `Payments 2026 Plan`. A program manager asks four questions in one meeting. Use the decision rules below.

| Question | View to open | Why |
|---|---|---|
| "What is unscheduled and high priority?" | Table, filtered `no:Sprint`, grouped by `Priority`. | Bulk scanning and field edits. |
| "How is the current sprint going right now?" | Board, filtered `sprint:@current`, columns by `Status`. | Flow visibility and blockers. |
| "What is the scope of the `Self-service Refunds` Epic?" | Hierarchy, filtered to the Epic. | Parent and children together. |
| "Are we on track for `2026.05.0`?" | Roadmap grouped by `Release`, plus an Insights chart of completed vs remaining for that `Release`. | Timeline plus trend. |

The same Project answers all four questions. Nothing in the data changes; only the layout and filter change. If you find yourself answering one of these questions in the wrong layout, that is the signal to switch views, not to add more fields.

### 8. Decision rules to keep handy

- Editing many items? Table.
- Running a standup or watching flow? Board.
- Planning across weeks or releases? Roadmap.
- Reviewing parent/child scope? Hierarchy.
- Reporting to leadership? Insights, paired with a Table or Board for drill-in.
- Items not appearing where you expect? Check the filter, the grouping field, and whether the items have the field the layout requires (`Status` for Board, dates for Roadmap, sub-issues for Hierarchy).

## Validation Checklist

- [ ] You can name the five view types and give a one-sentence purpose for each.
- [ ] You can match each of the four worked-example questions to the correct layout without rereading section 7.
- [ ] You can list at least one common pitfall per layout.
- [ ] You can explain why an Insights chart for "release health" must be filtered to the release.
- [ ] You can explain why an empty Hierarchy view usually means the work has not been decomposed, not that the view is broken.

## Common Mistakes

- Building one view and trying to make it answer every question. Use multiple saved views per Project.
- Using a Roadmap on items that have no dates and assuming the view is broken.
- Using a Board over the entire backlog instead of a sprint slice.
- Treating Insights as the place to plan. Insights reports; planning happens in Table, Board, Roadmap, and Hierarchy.
- Recreating Azure DevOps Delivery Plans, velocity widgets, or cumulative flow diagrams inside Projects without checking what GitHub natively supports first.

## Escalation Path

- GitHub administrator: not applicable for view selection; escalate only if a layout (for example Hierarchy) is missing because the organization is on a release that does not yet include it.
- Repository administrator: not applicable.
- Engineering lead: when the underlying field set is too thin to support the views you need (no `Status`, no `Sprint`, no dates, no sub-issues).
- Release manager: when the `Release` field, `Target Date`, or milestone data behind the Roadmap or Insights view is incomplete or inconsistent.

## Related Guides

- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-026 : How to Use the Product Backlog View
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
