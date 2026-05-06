# How to Run a Weekly ALM Dashboard Review

**Guide ID:** GHE-ALM-054
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 45 to 60 minutes weekly, plus 10 minutes prep
**Required permissions:** Project: Read on the ALM Project; Repository: Read on relevant repositories
**Prerequisites:**

- An organization-level ALM Project exists with `Status`, `Priority`, `Severity`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, and `Target Date` populated.
- Saved views exist for Release Roadmap, Current Sprint Board, Bug Triage, Executive Dashboard (Insights), and Hierarchy View.
- A recurring 45 to 60 minute weekly meeting is on the calendar with engineering, QA, product, and release stakeholders.

**When to use this guide:** Use this guide to run the standing weekly management review of release, sprint, defect, and roadmap health from inside GitHub Projects. The agenda repeats every week so the meeting becomes predictable and short.

**When not to use this guide:** Do not use this guide for daily standups (use GHE-ALM-029), for monthly trend reviews (use GHE-ALM-055), for sprint retrospectives (use GHE-ALM-032), or for ad hoc release readiness gates (use GHE-ALM-046).

## Outcome

By the end of this guide, you will have produced:

- A 45 to 60 minute weekly review covering five fixed views in a fixed order.
- A short written summary of decisions, escalations, and owners recorded in a tracking issue or meeting notes.
- An updated set of `Risk Level` and `Status` fields on items discussed.

## Before You Start

- Confirm the five saved views are reachable from the ALM Project sidebar.
- Confirm the current sprint name (for example `Sprint 2026.18`) and the active release (for example `2026.05.0`).
- Open the meeting tracking issue used to record outcomes, or create a new issue titled `Weekly ALM Review YYYY-MM-DD`.
- Have one note-taker assigned. Decisions made in the meeting must land in the tracking issue, not in chat.

## Steps

### Prepare the five views before the meeting

1. Ten minutes before the meeting, open the ALM Project. In the view sidebar, open each of the five saved views in a separate browser tab in this order: **Release Roadmap**, **Current Sprint Board**, **Bug Triage**, **Executive Dashboard**, **Hierarchy View**.
2. On the **Release Roadmap** tab, set the date range to cover the active release plus the next release. Confirm the roadmap is grouped by `Release` and shows `Start Date` and `Target Date` bars.
3. On the **Current Sprint Board** tab, apply the filter `sprint:@current` and group by `Status`. Confirm columns for `Backlog`, `Ready`, `In Progress`, `In Review`, `Ready for QA`, `Done`, and `Blocked` are present.
4. On the **Bug Triage** tab, apply the filter `is:open type:Bug` and group by `Severity`. Sort within each severity group by `Priority` ascending then issue age descending.
5. On the **Executive Dashboard** tab, confirm at least four charts are visible: Open vs Closed by week, Bugs by `Severity`, Sprint burn-up for the current sprint, and Work by `Release`. If any chart is missing, see GHE-ALM-051.
6. On the **Hierarchy View** tab, filter to the active release using `release:"2026.05.0"` and expand epics to one level so feature and requirement counts are visible.

> [SCREENSHOT: ALM Project sidebar showing the five saved views named in the agenda order.]

### Run the meeting in the fixed agenda order

7. **Release Roadmap (10 minutes).** Walk the active release first, then the next release. For each release, name the milestone, the `Target Date`, and any items whose `Target Date` slipped past the release `Target Date`. Highlight items with `Risk Level` set to `High` or items with no `Owner`. Escalate any release where slipped scope exceeds the team's typical late-cut tolerance. For interpretation rules, see GHE-ALM-045.
8. **Current Sprint Board (10 minutes).** Read the board grouped by `Status`. Call out the `Blocked` column item by item: who owns the unblock, what is the unblock action, what is the `Target Date` for the unblock. Call out items in `In Review` older than three days. Do not walk every card; focus on exceptions. Detailed board mechanics are in GHE-ALM-029.
9. **Bug Triage (10 minutes).** Read the Bug Triage view top-down by `Severity`. For Severity 1 and 2 bugs, confirm `Owner`, `Priority`, target `Sprint`, and target `Release` are set. For Severity 3 and 4 bugs, confirm none are older than the team's deferral threshold without a recorded decision. The triage view layout is described in GHE-ALM-034.

> [SCREENSHOT: Bug Triage view grouped by Severity with Severity 1 and 2 rows expanded.]

10. **Executive Dashboard (10 minutes).** Read the four charts in this order: Open vs Closed by week (is the gap closing or widening), Bugs by `Severity` (is Severity 1 trending up), Sprint burn-up (is the line tracking to scope), Work by `Release` (is the active release converging). Call out any chart whose direction is the opposite of expected and assign an owner to investigate. Chart authoring is covered in GHE-ALM-051.
11. **Hierarchy View (5 to 10 minutes).** Walk the active release's epics. For each epic, confirm the child feature and requirement counts have not grown since last week without a corresponding scope change record. Flag any epic with zero closed children halfway through the release window. Hierarchy View navigation is covered in GHE-ALM-018.

> [SCREENSHOT: Hierarchy View filtered to the active release, with two epics expanded to feature level.]

### Record outcomes and close the meeting

12. In the meeting tracking issue, record three lists: **Decisions** (what changed in the project, with the field and value), **Escalations** (what needs help from outside the room, with the named escalation owner), and **Follow-ups** (what someone in the room owns before next week, with `Target Date`).
13. Update fields directly in GitHub during the meeting where possible. Setting `Risk Level`, reassigning `Owner`, or changing `Status` should happen live, not in a side document. Bulk edits are faster from the table layout if many items change.
14. Post a link to the meeting tracking issue in the team channel within one hour of the meeting ending. Stale review notes lose value quickly.

## Validation Checklist

- [ ] The five views were opened in the agenda order and discussed in that order.
- [ ] Every Severity 1 and Severity 2 bug discussed has `Owner`, `Priority`, `Sprint`, and `Release` set.
- [ ] Every blocked sprint item discussed has a named unblock owner and an unblock `Target Date`.
- [ ] The meeting tracking issue contains Decisions, Escalations, and Follow-ups, each with an owner.
- [ ] Field updates made during the meeting are visible in the Project (refresh the view to confirm).
- [ ] The meeting ended on time. A weekly review consistently running over 60 minutes signals an agenda that needs trimming or a separate forum.

## Common Mistakes

- Walking every card on the sprint board. The weekly review is for exceptions, not for standup repetition.
- Discussing items that are not in the five views. If something matters, add it to the right view first, then discuss it.
- Recording decisions in chat instead of in the tracking issue. Decisions that are not written down did not happen.
- Skipping the Hierarchy View when time runs short. The hierarchy is the only view that exposes scope creep at the epic level.
- Treating the Executive Dashboard charts as decoration. Each chart should drive at least a yes/no question.
- Letting the agenda order drift. Roadmap first sets the strategic frame; sprint and bugs are tactical inside that frame.

## Escalation Path

- GitHub administrator: When a saved view is missing or the `Sprint` iteration field is misconfigured and you cannot fix it.
- Repository administrator: When a bug discussed in triage cannot be linked to a repository milestone because the milestone does not exist.
- Engineering lead: When a sprint item has been blocked for more than one week without an unblock owner, or when an epic shows zero closed children at the release midpoint.
- Release manager: When the Release Roadmap shows scope slipping past the active release `Target Date` and a release re-plan is required.

## Related Guides

- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-044 : How to Use the Release Roadmap View
- GHE-ALM-045 : How to Read Release Health from the Roadmap and Dashboard
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
