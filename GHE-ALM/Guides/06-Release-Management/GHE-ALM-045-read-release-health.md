# How to Read Release Health from the Roadmap and Dashboard

**Guide ID:** GHE-ALM-045
**Audience:** Engineering Manager, Release Manager, QA Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes per release check; 5 minutes for a quick weekly scan
**Required permissions:** Project: Read (to open the Roadmap and Insights views); Repository: Read (to open milestones referenced from the Project)
**Prerequisites:**

- The release is already being tracked with a milestone, the `Release` field, or both. See GHE-ALM-041.
- A Release Roadmap view exists on the Project. See GHE-ALM-044.
- An Executive Dashboard or release-focused chart set exists in the Project's Insights tab. See GHE-ALM-051.
- Issues in scope have `Status`, `Severity`, `Start Date`, and `Target Date` populated.

**When to use this guide:** Use this guide to assess whether a release is on track, in the weeks approaching code freeze, before a release readiness review, and during the weekly ALM dashboard review.

**When not to use this guide:** Do not use it to set up the Roadmap layout or create new Insights charts; those configuration activities are covered by GHE-ALM-044 and GHE-ALM-051. Do not use it to make the formal release decision; that pass/fail checklist lives in GHE-ALM-046.

## Outcome

By the end of this guide, you will have produced:

- A current read on the five release-health signals: scope, remaining work, blocked work, defect severity, and milestone progress.
- A green, yellow, or red call on release readiness with the specific items that drove the call.
- A short list of escalations or follow-ups, with owners, ready to take into the next release sync.

## Before You Start

- Confirm the release identifier you are reviewing, for example `2026.05.0`.
- Confirm the planned target date and the planned code freeze date.
- Open the Project that tracks the release in one browser tab. Open the contributing repositories' milestones in adjacent tabs if you need per-repository percentages.
- Have your defect severity scale on hand. A common 1-4 / P0-P3 scale is used below; confirm your team's actual scale with QA leadership.

## Steps

### Open the Release Roadmap

1. Open the organization Project for the product. In the view tab strip, click the saved view named for the release roadmap, for example `Release Roadmap`. If the view does not exist, stop and follow GHE-ALM-044 first.
2. Confirm the view is grouped by **Release** so each release train appears as its own swimlane. If grouping is missing, set **Group by** to `Release`.
3. Click **Date fields** in the top right and verify the start date is `Start Date` and the target date is `Target Date`. Items missing either field will not render a bar; that is itself a metadata-hygiene signal.
4. Set the zoom level to match your review window. Use **Month** for sprint-level inspection, **Quarter** for release-level inspection, or **Year** for portfolio. Most release-health checks use **Quarter**.
5. Click **Markers** and confirm milestones, iterations, and release dates appear as vertical lines. The release date marker is the line you measure every other bar against.

> [SCREENSHOT: Release Roadmap view grouped by Release, zoomed to Quarter, with milestone and release-date markers visible]

### Read scope and remaining work

6. In the release swimlane, count the bars. Each bar is one in-scope item. Compare against the scope committed at release planning. Higher than committed is scope growth; lower is scope cut. Both deserve a note in your readiness call.
7. Switch to a table view filtered by `Release:"<your release>"` and grouped by **Status**. Read the count of `Done` items versus the total. The ratio is your release completion percentage.
8. Return to the Roadmap. Look for bars whose **Target Date** falls after the release date marker. Each such bar is remaining work that will not land on time unless re-planned. List the issue numbers for the readiness call.

### Scan for blocked work and slipping items

9. Look for items with `Status` set to **Blocked**. Switch the bar color or label field to `Status` so blocked bars stand out. Note every blocked item in the swimlane.
10. Click each blocked bar to open the side panel. Read the latest comment and the linked dependency. Confirm whether the blocker has an owner, has a date, and is being actively worked. Blocked work without an owner or date is the highest-risk pattern in the view.
11. Identify slipping items: any item whose **Target Date** has moved later during the release or whose bar extends past the release date marker. Use timeline events on the issue side panel to confirm date movement.
12. Tally slipping items by `Product Area`. A cluster of slips in one area, for example `Checkout`, points to a team or dependency problem rather than scattered execution issues.

> [SCREENSHOT: Release swimlane with blocked items highlighted and a bar extending past the release date marker]

### Read milestone progress

13. Click each milestone marker in the release swimlane to open the milestone in its repository. Each milestone shows a progress bar with completed and open issue counts and a percentage.
14. Record the percentage for each contributing repository. A release that spans `acme-payments/payments-api` and `acme-checkout/checkout-service` should show both milestones at similar thresholds; a wide gap, for example 92 percent and 41 percent, means one repository is carrying the release while another lags.
15. Compare each milestone's due date against the release date marker. A due date that has passed without the milestone closing is a hard signal the release scope is not deliverable on the current plan.

### Read defect severity from the dashboard

16. Click the **Insights** tab. Open the saved chart that breaks open bugs by `Severity`, typically `Bugs by Severity`. If it does not exist, stop and follow GHE-ALM-051.
17. Filter the chart to the current release using a filter expression such as `is:issue is:open type:Bug Release:"2026.05.0"`.
18. Read the distribution. Severity 1 / P0 and Severity 2 / P1 counts drive the readiness call. A release with any open Severity 1 / P0 defects is not shippable without an explicit waiver from QA leadership and the release manager.
19. Open the historical burn-up chart of open versus closed defects. A flat or rising open-defect line in the final two weeks is a red signal even if the absolute count looks small.

> [SCREENSHOT: Insights tab with the Bugs by Severity chart filtered to the current release]

### Make the readiness call

20. Combine the five signals using the table below. Take the worst color across the five as your overall call. Write a one-paragraph summary naming the release, the call, and the two or three items that drove it. Bring the summary into the next release sync or readiness review.

## Release Readiness Signals

| Signal | Green (on track) | Yellow (watch) | Red (escalate) |
|---|---|---|---|
| Scope | Bar count matches plan; no late additions in the last sprint | Modest scope growth or cut, owners aware | Material scope growth, undocumented additions, or large unplanned cuts |
| Remaining work | Completion percentage tracks ahead of calendar; no bars past the release date marker | Completion lags calendar by under one sprint; one or two bars past the marker | Completion lags by a sprint or more; multiple bars past the marker |
| Blocked work | No items with `Status` = **Blocked** in the release swimlane | One or two blocked items, all with named owner and date | Three or more blocked items, or any blocked item with no owner or no date |
| Defect severity | Zero open Severity 1 / P0; small and falling Severity 2 / P1 count | Zero open Severity 1 / P0; flat Severity 2 / P1 count | Any open Severity 1 / P0, or rising Severity 2 / P1 count in the final two weeks |
| Milestone progress | All contributing repository milestones above 90 percent and tracking to due date | One repository milestone trailing but recovering | Any repository milestone past its due date without closing, or a wide gap between repositories |

## Validation Checklist

- [ ] The Release Roadmap view is open, grouped by `Release`, and zoomed appropriately.
- [ ] Scope, remaining work, blocked work, defect severity, and milestone progress have each been read for the target release.
- [ ] Every blocked item in the release swimlane has been checked for owner and date.
- [ ] Every repository milestone that contributes to the release has been opened and its percentage recorded.
- [ ] The defect severity chart has been filtered to this release and the open Severity 1 / P0 and Severity 2 / P1 counts have been recorded.
- [ ] An overall green, yellow, or red readiness call has been written down with the specific items that drove the call.

## Common Mistakes

- Reading scope from the table without opening the Roadmap. The Roadmap is where late items past the release date marker become visible at a glance.
- Treating bar count alone as scope. Items missing `Start Date` or `Target Date` do not render bars and will be silently undercounted; cross-check with the table view.
- Counting blocked items without checking owner and date. Blocked items with neither are the riskiest pattern.
- Reading the defect severity chart without filtering to the release. Org-wide counts hide the release-specific picture.
- Reading one repository's milestone percentage and assuming the release is healthy. Cross-repository releases require every contributing milestone to be read.
- Calling a release green while open Severity 1 / P0 defects exist. Severity 1 / P0 is a hard gate, not a weighted input.

## Escalation Path

- GitHub administrator: Not applicable for reading the views. Involve only if the Project, Roadmap view, or Insights charts are missing or corrupted.
- Repository administrator: Involve when a milestone exists in a repository you cannot access, or when milestone due dates need to be changed and you do not have Triage access.
- Engineering lead: Escalate clusters of blocked or slipping items in a single `Product Area`, missing owners on blocked work, and any rising open-defect trend in the final two weeks.
- Release manager: Escalate any red call, any open Severity 1 / P0 defect, and any milestone past its due date. The release manager owns the go or no-go decision; this guide produces the input.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-044 : How to Use the Release Roadmap View
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
