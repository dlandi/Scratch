# How to Use the Bug Triage View

**Guide ID:** GHE-ALM-034
**Audience:** Engineering Manager, QA Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 30-45 minutes per triage session; 15-minute one-time view setup
**Required permissions:** Project: Write; Repository: Triage on the underlying repositories
**Prerequisites:**

- A Project that contains the bugs you intend to triage.
- Bugs created from the standard bug issue form (see GHE-ALM-014).
- The standard project fields configured: `Status`, `Severity`, `Priority`, `Release`, `Product Area`, `Owner`.
- An organization-level issue type of `Bug` enabled, or a `Type` single-select field with a `Bug` option, so bugs can be filtered.

**When to use this guide:** Use this guide when you need a single, repeatable view that lets you walk the open bug backlog by severity and make accept, defer, duplicate, close, or route decisions. Use it for a recurring weekly bug triage meeting and for ad-hoc triage when a critical defect arrives.

**When not to use this guide:** Do not use this guide to file new bugs (see GHE-ALM-014), to define what severity and priority mean (see GHE-ALM-035), to move a single bug through its lifecycle (see GHE-ALM-036), or to plan the wider weekly bug review meeting agenda (see GHE-ALM-039).

## Outcome

By the end of this guide, you will have produced:

- A saved Project view named `Bug Triage` configured as a Table, filtered to open bugs, grouped by `Severity`, sorted by `Priority` then Created date.
- A repeatable triage session in which every open bug receives a triage decision and a routing assignment.
- An updated bug backlog where each bug has, at minimum, a `Status`, an `Owner`, and a target `Release` or an explicit deferral.

## Before You Start

- Confirm your team's `Severity` and `Priority` scales with QA leadership. The scale used in this guide is illustrative.
- Confirm the canonical project field names exist on the Project: `Status`, `Severity`, `Priority`, `Release`, `Product Area`, `Owner`.
- Identify the product area owners who should attend triage or be notified when a bug is routed to them.
- Block 30 to 45 minutes on the calendar. Do not attempt triage in the standup slot.

## Steps

### Create or select the Bug Triage view

1. Open the Project that holds your bugs, for example the `acme-payments` delivery Project.
2. In the view tabs along the top, click the plus icon to add a new view, or open the existing `Bug Triage` view if your team has already created it.
3. Click the **View options** gear icon next to the search bar, choose **Layout**, and select **Table**.
4. Click the view tab name, choose **Rename view**, and name it `Bug Triage`. Save.

> [SCREENSHOT: Project view tabs with the Bug Triage tab selected and the View options gear icon visible]

### Apply the standard filters

5. Click the search and filter bar at the top of the view. Enter the base filter for open bugs:

   ```
   is:open type:Bug
   ```

   If your organization uses a `Type` single-select field instead of organization issue types, substitute the equivalent expression, for example `` `Type:"Bug"` ``.
6. Confirm that only bug issues now appear. Closed bugs and non-bug issues should disappear.
7. Save the filter to the view by clicking **Save changes** on the view tab. The view tab dot indicator should clear.

### Group by Severity and sort by Priority

8. Click the **View options** gear icon, choose **Group**, and select `Severity`. Severity groups will appear as collapsible sections in the table.
9. Click **View options** again, choose **Sort**, and add two sort keys in this order: `Priority` ascending (so P0 sits at the top), then **Created** descending (so newest bugs sit above older ones within the same priority).
10. Click **Save changes** on the view tab.

> [SCREENSHOT: Bug Triage table grouped by Severity with Priority sort applied, showing Severity 1 group expanded at the top]

### Add slice filters for the triage meeting

11. During triage you will narrow the view repeatedly. Add these filters to the search bar one at a time as you work, then clear them. Do not save these slice filters to the view itself:

    - By release: `` `release:"2026.05.0"` `` to focus on bugs targeting the current release.
    - By product area: `` `product-area:Checkout` `` to focus on a single product area owner's queue.
    - By owner: `` `assignee:@me` `` for self-review, or `` `assignee:<github-handle>` `` to review one engineer's queue.
    - By age: `` `created:<2026-04-01` `` to find stale bugs that have lingered.
    - By regression: search for the `regression` label or your team's equivalent field value.

12. After each slice, click the **X** on the temporary filter chip to return to the saved Bug Triage view.

### Walk the view in severity order during the triage meeting

13. Open the view at the start of the meeting. Collapse all Severity groups, then expand them one at a time starting with Severity 1.
14. For each bug in the group, read the title, the reproduction summary, and the regression flag. Spend no more than two minutes per bug unless a real decision needs discussion.
15. Apply one of the five triage decisions described in the next phase, set the field values directly in the table row, and move on. Do not open the issue detail pane unless you must.

> [SCREENSHOT: A Severity 1 group expanded showing two bug rows with Status, Priority, Release, Product Area, and Owner columns visible]

### Apply a triage decision to each bug

16. **Accept.** Set `Status` to `Ready`, set `Priority` using the illustrative scale below, set `Release` to the target release, set `Product Area`, and set `Owner` to the engineer or area owner who will pick it up. Confirm `Severity` was set correctly at intake; correct it if not.
17. **Defer.** Set `Status` to `Backlog`, clear `Release` or set it to a future release, and add a short comment naming the reason for deferral. Deferred bugs remain visible in the Bug Triage view until they are fixed or closed.
18. **Duplicate.** Find the original bug, comment `` `Duplicate of #1234` `` on the newer bug, set `Status` to `Done`, then close the issue. The original bug carries the work.
19. **Close as not-a-bug.** Set `Status` to `Done`, add a comment explaining why the behavior is expected or out of scope, and close the issue. Common reasons: working as designed, user error, third-party defect, cannot reproduce.
20. **Route to product area owner.** Set `Product Area` to the correct area, set `Owner` to the area owner or `@mention` them in a comment asking for triage acceptance, and leave `Status` at `Backlog` or `Ready` depending on whether you have already accepted it.

### Locked illustrative Severity and Priority scale

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

This scale is illustrative; confirm your team's actual scale with QA leadership.

### Close out the meeting

21. After the last bug, clear any temporary slice filter so the next person opening the view sees the full backlog.
22. Note the count of bugs accepted, deferred, duplicated, closed, and routed. Capture this in the meeting notes for the weekly bug review (see GHE-ALM-039).

## Validation Checklist

- [ ] The view is named `Bug Triage`, uses the **Table** layout, and is saved.
- [ ] The saved filter on the view is `is:open type:Bug` (or your organization's equivalent type expression).
- [ ] The view is grouped by `Severity` and sorted by `Priority` then Created date.
- [ ] Severity 1 bugs appear at the top of the table when the Severity 1 group is expanded.
- [ ] Every bug walked during triage has a `Status`, `Owner`, and either a `Release` or an explicit deferral comment.
- [ ] No temporary slice filter is left applied on the saved view.

## Common Mistakes

- Saving slice filters such as `release:"2026.05.0"` to the Bug Triage view itself. The saved view should always show the full open bug backlog. Slice filters belong in the search bar for the duration of the meeting only.
- Grouping by `Priority` instead of `Severity`. Severity drives triage order because it is the technical impact you cannot negotiate; priority is the business decision you make during triage.
- Conflating severity and priority on a single bug. Set them independently. See GHE-ALM-035.
- Treating triage as a status meeting. Triage is a decision meeting. If a bug needs investigation, accept it, assign it, and move on; do the investigation outside the meeting.
- Leaving bugs without an `Owner`. An unowned bug will not move. Route to a product area owner if no individual is named.
- Closing a bug as a duplicate without linking the original with `` `Duplicate of #NNNN` ``. The link is what preserves the history.

## Escalation Path

- GitHub administrator: When the organization-level `Bug` issue type is missing or misconfigured and the `type:Bug` filter returns nothing.
- Repository administrator: When bugs are filed in a repository that is not yet added to the Project, so they do not appear in the view.
- Engineering lead: When a bug needs an owner and no product area owner has been identified, or when severity is disputed.
- Release manager: When a Severity 1 or Severity 2 bug is accepted into a release that is already in code freeze.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-035 : How to Distinguish Severity from Priority
- GHE-ALM-036 : How to Move a Bug Through the Defect Workflow
- GHE-ALM-039 : How to Run a Weekly Bug Review
