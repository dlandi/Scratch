# How to Run a Weekly Bug Review

**Guide ID:** GHE-ALM-039
**Audience:** Engineering Manager, QA Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 45-minute meeting weekly, plus 15-minute setup the first time
**Required permissions:** Project: Write; Repository: Triage on the bug-bearing repositories

**Prerequisites:**

- A GitHub Project that contains the bugs for the product or release in scope.
- The fields `Status`, `Severity`, `Priority`, `Sprint`, `Release`, `Product Area`, and `Owner` exist on the Project.
- A Bug issue type is in use across the in-scope repositories.
- The defect workflow statuses described in GHE-ALM-036 are in active use, including `Ready for QA` and `Verified`.

**When to use this guide:** Use this guide once per week to keep the bug backlog under control, surface aging defects, confirm fix-and-verify flow, and decide what to defer.

**When not to use this guide:** Do not use this guide to triage individual new bug reports as they arrive; that belongs to GHE-ALM-014. Do not use it for hotfix decisions on a live production defect; that belongs to GHE-ALM-040.

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named `Weekly Bug Review` that loads the six review slices in the agreed order.
- A recurring 45-minute weekly meeting with a fixed agenda, owner, and attendee list.
- A short set of decisions per bug reviewed: keep, defer, escalate, close, or assign owner.
- An updated set of issues reflecting those decisions in `Status`, `Sprint`, `Release`, and `Owner`.

## Before You Start

- Confirm the canonical scale your team uses. The guide assumes a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership.
- Identify the standing attendees. A typical roster is the Engineering Manager, QA Manager, Product Owner, and the Scrum Master for each in-scope team.
- Pick a recurring 45-minute slot. Place it after the daily standup and before sprint planning so decisions feed the next sprint.
- Have the Project URL ready and the date one week back. For a meeting on 2026-05-06, the cutoff date is `2026-04-29`.

## Steps

### Build the saved Project view

1. Open the Project. Click the view tab row, then click the **plus** icon at the right end of the tab strip and choose **New view**.
2. Name the view `Weekly Bug Review`. Set the layout to **Table** using the view options icon next to the search bar.
3. In the search bar, set the base filter to `type:Bug`. Every slice below will narrow this base.
4. Open **Group**. Group by `Status`. This makes the fixed-not-verified and verified slices visually distinct without retyping filters.
5. Open **Sort**. Sort by `Severity` ascending, then by `Updated` ascending. Severity 1 rises to the top; the oldest activity sits at the top within each severity band.
6. Open **Fields** and ensure the visible columns include `Title`, `Status`, `Severity`, `Priority`, `Owner`, `Sprint`, `Release`, `Product Area`, `Updated`, and `Created`.
7. Save the view. Click the view tab name and choose **Save changes**. The view is now bookmarkable.

> [SCREENSHOT: Project table view named Weekly Bug Review, grouped by Status, sorted by Severity, with the bug-only filter visible in the search bar.]

### Define the six review slices

8. Document the six saved filter strings the meeting will run, in order. Keep them in the Project description or the meeting invite body so the facilitator can paste them quickly.
9. Slice 1, new bugs since the last review. Filter: `type:Bug created:>2026-04-29 is:open`. Update the date weekly.
10. Slice 2, critical bugs still open. Filter: `type:Bug severity:1,2 is:open`.
11. Slice 3, stale bugs with no activity for seven or more days. Filter: `type:Bug is:open updated:<2026-04-29`.
12. Slice 4, deferred bugs returned to backlog this week. Filter: `type:Bug is:open no:Sprint -no:label:deferred`. Adjust to your team's deferral marker if you use a `Status` value rather than a label.
13. Slice 5, fixed but not verified. Filter: `type:Bug status:"Ready for QA"`.
14. Slice 6, verified and closed in the last week. Filter: `type:Bug is:closed status:Verified closed:>2026-04-29`.

> [SCREENSHOT: Search bar showing the slice 2 filter `type:Bug severity:1,2 is:open` applied with results visible.]

### Run the meeting

15. Open the `Weekly Bug Review` view. Confirm the Project is current and that overnight automation has not left items in an unexpected status.
16. Walk slice 1, new bugs. For each, confirm `Severity`, `Priority`, `Owner`, and target `Sprint` are set. Reject incomplete bugs back to the reporter rather than guessing fields. Do not use this slot to debug; book a follow-up if engineering needs a deeper look.
17. Walk slice 2, critical open bugs. For each Severity 1 or 2 bug, confirm there is a named owner, an active `Sprint`, and recent activity. If a Severity 1 bug has no activity in 48 hours, escalate before leaving the meeting.
18. Walk slice 3, stale bugs. For each stale item, the owner gives a one-line status. Acceptable outcomes are: pick up this sprint, defer to a later sprint with a reason, close as obsolete, or escalate. Do not allow "still investigating" without a target date.
19. Walk slice 4, deferred bugs. Confirm each deferral is intentional, that `Severity` and `Priority` reflect the reason for deferral, and that the bug carries a `Release` value if it is targeted to a future train.
20. Walk slice 5, fixed not verified. Read out the count and the oldest item. The QA Manager confirms verification capacity for the next two days. Items older than five business days in `Ready for QA` get escalated to QA leadership.
21. Walk slice 6, verified and closed last week. This is a count, not an item-by-item walk. Note the number for the weekly metric. Spot-check one or two items to confirm closure was clean: linked PR merged, `Release` populated, `Status` set to `Verified` before close.

### Record decisions and close out

22. Update each affected item in the Project as decisions are made. Change `Status`, `Owner`, `Sprint`, or `Release` directly in the table; do not capture decisions only in meeting notes.
23. At the end of the meeting, post a short summary in the team channel. Include counts for each slice, escalations raised, and the next meeting date. The Scrum Master owns the post.
24. Open the meeting invite. Update the cutoff dates in slices 1, 3, and 6 to the new "one week back" date so the next facilitator does not have to recalculate.

## Validation Checklist

- [ ] The `Weekly Bug Review` view exists, is grouped by `Status`, and is sorted by `Severity` then `Updated`.
- [ ] All six slice filters are recorded in a place the facilitator can paste from.
- [ ] Every Severity 1 or 2 open bug has an `Owner` and a `Sprint`.
- [ ] No bug remains in `Ready for QA` for more than five business days without an escalation note.
- [ ] The weekly summary post lists slice counts and any escalations.
- [ ] The cutoff dates in the saved filters are refreshed before the next meeting.

## Common Mistakes

- Treating the weekly review as a re-triage of individual bug reports. New bug intake is GHE-ALM-014; this meeting confirms the system is working.
- Allowing severity to drift to match urgency. `Severity` reflects technical impact and `Priority` reflects business urgency; see GHE-ALM-035.
- Using labels to mark deferral on some bugs and `Status` values on others. Pick one mechanism and use it consistently or the slice 4 filter will lie.
- Closing bugs directly from `In Progress` without passing through `Ready for QA` and `Verified`. This breaks slice 5 and slice 6 counts.
- Forgetting to refresh the cutoff date. A stale `created:>2026-04-29` filter will silently include or exclude items the next week.
- Capturing decisions only in meeting notes. The Project must reflect the outcome or the next review starts from the same place.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: When a bug touches a repository the standing attendees do not own, ask the repository administrator to add the correct owning team to the bug or to add a triage owner to the meeting.
- Engineering lead: When a Severity 1 bug has no activity in 48 hours, when a stale bug has no acceptable owner answer, or when fixed-not-verified items are aging past five business days.
- Release manager: When a deferred bug carries a `Release` value that the release manager has not approved, or when slice 6 spot checks reveal a bug closed without `Release` populated for a release in flight.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-035 : How to Distinguish Severity from Priority
- GHE-ALM-036 : How to Move a Bug Through the Defect Workflow
- GHE-ALM-040 : How to Handle a Hotfix Bug
