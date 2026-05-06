# How to Use the Release Roadmap View

**Guide ID:** GHE-ALM-044
**Audience:** Release Manager, Engineering Manager, Program Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 30-minute one-time setup, then 5-10 minutes per use
**Required permissions:** Project: Write
**Prerequisites:**

- An organization-level Project that contains the issues for the release or releases you want to visualize.
- The Project has `Start Date`, `Target Date`, `Sprint`, and `Release` fields populated on most items.
- Repository milestones exist for any version-scoped releases you want to mark.

**When to use this guide:** Use this guide when you need a single, time-based picture of release scope, schedule, and milestone alignment across the work in a Project. The roadmap layout is the right view for release planning, release readiness reviews, and quick schedule-risk scans.

**When not to use this guide:** Do not use the roadmap to run a daily standup or to triage individual bugs. Use the board view (GHE-ALM-029) for sprint execution and the bug triage view (GHE-ALM-034) for defect work.

## Outcome

By the end of this guide, you will have produced:

- A saved roadmap view in the ALM Project, grouped by `Release`, with Start Date and Target Date fields wired up.
- A configured set of markers showing iterations and milestones across the timeline.
- A repeatable visual scan that surfaces release scope, slip risk, and milestone alignment.

## Before You Start

- Confirm that issues representing release scope have a `Release` field value, for example `2026.05.0` or `2026-Q3 Release`.
- Confirm that issues have realistic `Start Date` and `Target Date` values where applicable. Items with missing dates will not render as bars on the timeline.
- Confirm that the Project's `Sprint` iteration field has been configured (see GHE-ALM-027).
- Confirm that the relevant repository milestones have due dates set, so milestone markers anchor to a point on the axis.

## Steps

### Create the roadmap view

1. Open the ALM Project. In the view tabs near the top of the page, click the **plus icon** to add a new view, or open an existing view you want to convert.
2. In the new view, click the view name dropdown and choose **Layout**, then select **Roadmap**. The view switches to a horizontal timeline with one row per item.
3. Rename the view to something durable, for example `Release Roadmap` or `2026 Release Train`. Save the view.

> [SCREENSHOT: New view with Layout menu open showing Table, Board, and Roadmap options.]

### Wire up the date fields

4. In the top right of the roadmap, click the **Date fields** button (calendar icon).
5. For **Start date**, select the `Start Date` project field. For **Target date**, select the `Target Date` project field. If you want to drive bars from sprint windows instead, select the `Sprint` iteration field for both. Items without populated dates will not appear as bars.
6. If `Start Date` or `Target Date` does not exist yet, click **New field** inside the Date fields panel, name it exactly `Start Date` or `Target Date`, and click **Save**. Then go back through items and populate the fields.

### Group by Release

7. Open the view's settings (the dropdown next to the view name) and choose **Group by**.
8. Select **Release**. Items now collapse under one band per release value, for example `2026.05.0`, `2026.06.0`, `2026-Q3 Release`. Empty release rows surface unscheduled scope.
9. Optional: choose **Sort**, then sort by `Target Date` ascending so each release band reads left to right by delivery order.

> [SCREENSHOT: Roadmap grouped by Release with three release bands visible and bars spanning the timeline.]

### Turn on markers

10. Click the **Markers** button (location icon) in the top right of the roadmap.
11. Enable **Iterations** so each `Sprint` boundary appears as a vertical line on the axis.
12. Enable **Milestones** so each repository milestone with a due date appears as a vertical marker. Hover any marker to confirm its name and date.

### Set the zoom level

13. Click the **zoom icon** (search icon) in the top right.
14. Choose **Month** for sprint-level inspection, **Quarter** for release-train planning, or **Year** for portfolio overviews. Switch zoom levels freely; the view remembers the last selection.

> [SCREENSHOT: Zoom set to Quarter with milestone and iteration markers visible across two release bands.]

### Read the roadmap for schedule risk

15. Scan each release band against the **today** line (the vertical line marking the current date). Bars that cross the line without entering an `In Progress` status indicate late starts.
16. Look for bars whose right edge sits past the next milestone marker for that release. Those items are scheduled to finish after the milestone they support.
17. Look for clusters of bars stacked on the same week inside one release. Heavy clustering near a milestone often indicates an overcommitted finish.
18. Look for items inside a release band that have no bar at all. Those items are missing `Start Date` or `Target Date` values and are invisible to the schedule.

## Validation Checklist

- [ ] The view layout is **Roadmap** and the view name reflects its purpose, for example `Release Roadmap`.
- [ ] **Date fields** are set to `Start Date` for Start date and `Target Date` for Target date.
- [ ] The view is **Grouped by** `Release` and shows one band per release value.
- [ ] **Markers** show both iterations and milestones along the date axis.
- [ ] Zoom level is set appropriately for the audience: Month for working sessions, Quarter for release reviews.
- [ ] Items with no bar can be listed and chased for missing dates.

## Common Mistakes

- Leaving items without `Start Date` or `Target Date`. Those items will not render as bars and the roadmap will silently underrepresent scope.
- Grouping by `Status` or `Sprint` instead of `Release`. The roadmap then shows execution flow, not release scope, which defeats the purpose for release planning.
- Treating the roadmap as the source of truth for release content. The roadmap visualizes whatever is in the Project; the `Release` field on each issue is still the system of record.
- Using milestone markers without due dates on the milestones. Markers will not anchor correctly and the visual cue will be missing.
- Forgetting to save the view. Configuration changes are per-view, and an unsaved roadmap reverts the next time the Project is opened.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve when milestones are missing due dates or when milestones for a release do not exist yet.
- Engineering lead: Involve when scope inside a release band is unrealistic, when items lack owners, or when bars cluster past a milestone.
- Release manager: Owns the roadmap and the conversation about slip, scope cuts, and release date changes.

## Related Guides

- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-045 : How to Read Release Health from the Roadmap and Dashboard
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-049 : How to Track a Cross-Repository Release
