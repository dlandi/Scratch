# How to Test Leadership Dashboard Sufficiency

**Guide ID:** GHE-ALM-084
**Audience:** Engineering Manager, Program Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 60-90 minutes during pilot, plus a 30-minute leadership review session
**Required permissions:** Project: Write on the pilot Project; Repository: Read on pilot repositories
**Prerequisites:**

- The pilot from GHE-ALM-079 has run for at least one full sprint with real work, real bugs, and a release in flight.
- A pilot Project exists with `Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, and `Product Area` populated for most items.
- An engineering leader (director, VP, or equivalent) has agreed to spend 30 minutes reviewing the dashboard with you.

**When to use this guide:** Use this guide as scenario 5 of the pilot evaluation, after sprint planning, bug intake, and release tracking scenarios have produced enough data for charts to be meaningful.

**When not to use this guide:** Do not use this guide for ongoing weekly dashboard reviews; that is GHE-ALM-054. Do not use it before the pilot Project has at least one full sprint of history, because empty charts cannot be evaluated.

## Outcome

By the end of this guide, you will have produced:

- An Executive Dashboard view inside the pilot Project containing the five required charts plus a roadmap layout.
- A completed pass/fail record for each acceptance criterion against the leadership review.
- An explicit list of dashboard gaps, each labelled as a content gap, a layout gap, or a fundamental tooling gap that requires BI or external reporting.

## Before You Start

- The pilot release in question has a `Release` field value such as `2026.05.0`, a target date, and at least a handful of issues assigned.
- At least one sprint (for example `Sprint 2026.18`) has closed so historical charts have something to display.
- Bugs in the pilot have `Severity` populated using the team's scale (commonly a 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership).
- You have read GHE-ALM-051 so you can build charts without help, and skimmed GHE-ALM-044 so the roadmap layout is familiar.

## Steps

### Set up the Executive Dashboard view

1. Open the pilot Project. Click the `+` next to the existing view tabs and choose **New view**.
2. Name the view `Executive Dashboard`. Set the layout to **Table** and pin it to the left of bug-triage and sprint-execution views so a leader sees it first.
3. Apply a top-line filter that matches your scenario, for example `release:"2026.05.0"`. Group by `Status`. Save the view.
4. Add a second view named `Release Roadmap`. Set the layout to **Roadmap**. Configure the date fields as `Start Date` to `Target Date`, and set the marker to `Sprint`. Group by `Release`. Confirm the pilot release `2026.05.0` and at least one prior or following release are visible on the timeline.

> [SCREENSHOT: Executive Dashboard table view filtered to the pilot release, grouped by Status, with the Release Roadmap view tab visible alongside]

### Build the five required charts

5. Open **Insights** from the left sidebar of the Project. Click **New chart**.
6. Create chart 1, **Work by Release**. Chart type: stacked column. X-axis: `Release`. Group by: `Status`. Filter: none. This answers "how much work is open, in progress, and done per release."
7. Create chart 2, **Sprint Progress (current and prior)**. Chart type: stacked column. X-axis: `Sprint`. Group by: `Status`. Filter: `sprint:@current,@previous`. This answers "are we landing what we committed to."
8. Create chart 3, **Bugs by Severity**. Chart type: column. X-axis: `Severity`. Group by: `Status`. Filter: `type:Bug is:open`. This answers "what is the open defect profile."
9. Create chart 4, **Remaining Work by Product Area**. Chart type: column. X-axis: `Product Area`. Group by: `Status`. Filter: `is:open release:"2026.05.0"`. This answers "where is the unfinished release work concentrated."
10. Create chart 5, **Burn-Up for Pilot Release**. Chart type: historical, burn-up. Filter: `release:"2026.05.0"`. Time range: from release start to today. This answers "is scope growing or stabilising."
11. Pin all five charts to the Insights page in the order above so leadership sees them top to bottom in the same sequence.

> [SCREENSHOT: Insights page showing the five pinned charts with their titles visible]

### Run the leadership review session

12. Schedule 30 minutes with the agreed engineering leader. Send the direct link to the Project, the `Executive Dashboard` view, and the Insights page in the calendar invite.
13. Open the session by stating the four questions leadership should be able to answer from the dashboard alone: release health, sprint progress, bug severity profile, and remaining work. Do not narrate; let the leader read.
14. Watch where the leader pauses, asks "where do I see X," or pulls up another tab. Each of those moments is a candidate gap. Note them verbatim.
15. At the end, ask the leader directly: "If this were the only reporting surface, could you make a release go/no-go decision tomorrow?" Record the answer.

### Record evidence and identify gaps

16. Open the pilot scorecard maintained per GHE-ALM-085. Add a row for each of the five pass criteria below and mark Pass, Partial, or Fail with one sentence of evidence.
17. For every Partial or Fail, classify the gap into one of three categories. **Content gap**: the data exists in GitHub but is not on the dashboard yet; fix by adding a chart or filter. **Layout gap**: the data and chart exist but the leader could not find or interpret them; fix by reordering, renaming, or adding a saved view. **Tooling gap**: GitHub Projects cannot answer the question at all (for example, cross-project rollups, custom calculated fields, financial views, or trend lines beyond burn-up). Tooling gaps are the inputs to GHE-ALM-056.
18. Capture screenshots of the Executive Dashboard view, the Release Roadmap, and each of the five charts. Attach them to the pilot evidence folder named in GHE-ALM-079.

## Pass Criteria

| Criterion | Pass condition |
|---|---|
| Executive Dashboard view exists | A saved view named `Executive Dashboard` is pinned in the pilot Project and reachable in one click. |
| Charts show work by release and sprint | Chart 1 (Work by Release) and chart 2 (Sprint Progress) both render with non-empty bars. |
| Bugs by severity visible | Chart 3 renders with at least one bar per active severity, and the leader can tell P0/P1 from P2/P3 at a glance. |
| Roadmap view shows timeline | The Release Roadmap view shows the pilot release and at least one neighbour release on a date axis. |
| Data quality is sufficient for decision-making | Fewer than 10 percent of items in the pilot release are missing `Status`, `Priority`, `Severity` (for bugs), `Sprint`, or `Release`. |

## Validation Checklist

- [ ] The Executive Dashboard view, Release Roadmap view, and Insights page each open from a direct URL without filtering or scrolling.
- [ ] All five required charts are pinned and render data, not empty states.
- [ ] The burn-up chart shows a visible scope line, not just a flat zero.
- [ ] The pass/fail outcome for each of the five criteria is recorded in the pilot scorecard.
- [ ] Every Partial or Fail has a gap classification of content, layout, or tooling.
- [ ] Gaps classified as tooling are listed as inputs to GHE-ALM-056.

## Common Mistakes

- Building the dashboard yourself, then walking the leader through it. Let the leader navigate. The point is to test sufficiency, not to demo.
- Treating sparse data as a dashboard failure. If `Severity` is missing on most bugs, the gap is metadata hygiene, not the chart. Note it under data quality.
- Filtering the burn-up to the wrong date range so the line looks flat. Set the range from release kickoff, not from today minus 14 days.
- Promising leadership the gaps will be fixed inside GitHub. Some gaps are real tooling gaps and need BI or export, which is the explicit purpose of GHE-ALM-056.
- Deleting charts the leader did not use during the session. Keep them; the next leader may want them.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable.
- Engineering lead: Engage when the leader cannot answer the release go/no-go question even with the full dashboard, because that is a pilot finding that must reach the adoption decision.
- Release manager: Engage when the Release Roadmap view shows missing dates or unassigned releases, because this blocks chart 1 and chart 5 from being meaningful.

## Related Guides

- GHE-ALM-079 : How to Run the GitHub Enterprise ALM Pilot Evaluation
- GHE-ALM-044 : How to Use the Release Roadmap View
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-056 : How to Identify Reporting Gaps That Require BI or External Tools
