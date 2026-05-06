# How to Use Hierarchy View to Review Epic-to-Task Breakdown

**Guide ID:** GHE-ALM-018
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Program Manager
**Classification:** Manager Performs
**Estimated time:** 15-20 minutes per use
**Required permissions:** Project: Read (to view); Project: Write (to save view changes for the team)
**Prerequisites:**

- An organization-level GitHub Project that contains issues with parent/child relationships.
- Sub-issues already created on the parent issues you want to review (see GHE-ALM-017).
- Issues populated with `Status`, `Owner`, `Sprint`, `Release`, and `Product Area` fields.

**When to use this guide:** Use Hierarchy View when you need to see how an epic decomposes into features, requirements, and tasks inside a single project table, and to review release scope or program status across nested levels of work.

**When not to use this guide:** Do not use Hierarchy View as a sprint execution board. Use the board layout grouped by `Status` for daily standups (GHE-ALM-029). Hierarchy View is for inspection and decomposition review, not column-based throughput.

## Outcome

By the end of this guide, you will have produced:

- A project table view with Hierarchy View enabled, showing parent issues with their sub-issues nested underneath.
- A reviewable picture of work decomposition for one or more epics, including `Status`, `Owner`, `Sprint`, and `Release` at every level.
- A saved view configuration the rest of the team can reuse for release scope review and program-level status inspection.

## Before You Start

- Confirm at least one epic or feature in scope has sub-issues attached. Hierarchy View only shows nesting that already exists in the data.
- Decide what you are reviewing: epic-to-feature planning, feature-to-requirement decomposition, release scope, or program status. Each leads to a different grouping choice.
- Confirm you can open the Project. If you cannot edit the view, ask the Project administrator for `Project: Write` access or work in a personal duplicate of the view.

## Steps

### Open the project and switch to a table view

1. Navigate to the organization Project that holds the epics you want to review. If you do not know which project, see GHE-ALM-002.
2. Open or create a table view. Click the **+** tab next to existing views, then choose **New view** and select the **Table** layout. Name it something specific, for example `Hierarchy - Q3 Release Scope`.
3. Confirm the layout is **Table**. Hierarchy View is a property of the table layout. It does not appear on the board or roadmap layouts.

> [SCREENSHOT: A new Project view tab being created with the Table layout selected.]

### Enable Hierarchy View

4. Click the view options gear icon next to the search bar at the top of the view.
5. In the menu that opens, locate **Show hierarchy** and toggle it on. New views created after the March 2026 GA release have this enabled by default; older views need it switched on once.
6. Confirm that parent issues now display a small disclosure triangle to the left of the title. That triangle is the expand control for sub-issues.

> [SCREENSHOT: The view options menu open with the Show hierarchy toggle enabled, and a parent issue showing its disclosure triangle.]

### Choose the columns that matter for review

7. Open the view options gear again and select the fields to display as columns. For an epic-to-task review, include `Title`, `Status`, `Owner`, `Sprint`, `Release`, `Priority`, and `Product Area`. Hide fields you will not use in this review to reduce horizontal scrolling.
8. Reorder columns by dragging their headers so `Status`, `Owner`, and `Sprint` sit immediately to the right of `Title`. These are the columns you will scan most.

### Group the hierarchy for the review you are running

9. Click the view options gear and choose **Group**. Pick the grouping that matches your review purpose:

   - For epic-to-feature planning, group by **Parent issue**. Each epic becomes a group header with its features and requirements nested underneath.
   - For release scope review, group by **Release**. Each release value, for example `2026.05.0`, becomes a section, and parent/child nesting appears within each section.
   - For program-level status inspection, group by **Product Area**. The `Checkout`, `Billing`, and `Identity` areas become headers, with the full hierarchy expanding inside each.

10. Apply a filter to keep the view focused. Useful filters include `is:open`, `release:"2026.05.0"`, `sprint:@current`, or a `parent-issue` filter naming a specific epic. Combine filters with spaces, for example `is:open release:"2026.05.0"`.

### Expand the tree and read status at every level

11. Click the disclosure triangle next to a parent issue to expand it. Sub-issues appear indented under the parent. Click again to collapse. The expand and collapse state persists across visits to the view as of the March 2026 update, so you can leave commonly inspected branches open.
12. For each parent, scan across the row and read `Status`, `Owner`, and `Sprint`. Then read the same fields on each indented child row. The pattern you are looking for is: parent in a planning status (for example `In Progress` or `Ready`), with children that have owners assigned and that resolve to either `Done` or a current/near-term `Sprint`.
13. Note any parent that is `In Progress` but whose children are mostly `Backlog` or have no owner. That signals decomposition that has not been planned into a sprint yet.
14. Note any child that has a different `Release` value from its parent. That signals scope that has slipped out of the parent's release window and may need a parent-level decision.

> [SCREENSHOT: A hierarchy grouped by Release with one epic expanded, showing four sub-issues with their Status, Owner, and Sprint values visible.]

### Save the view for reuse

15. After the columns, grouping, and filter are correct, click the small dot or unsaved indicator next to the view name and choose **Save changes to view**. The next person who opens the project sees the same configuration.
16. If this is a recurring review, duplicate the view per release or per program area. Name them clearly, for example `Hierarchy - 2026.05.0 Scope` and `Hierarchy - Identity Program`.

## Validation Checklist

- [ ] The active layout is Table and **Show hierarchy** is enabled.
- [ ] At least one parent issue shows a disclosure triangle, expands, and reveals its sub-issues indented underneath.
- [ ] Columns include `Status`, `Owner`, `Sprint`, and `Release`, and those values are readable on both parent and child rows.
- [ ] The view is grouped by `Parent issue`, `Release`, or `Product Area` matching the review you are running.
- [ ] Saved filter expressions, for example `release:"2026.05.0"` or `sprint:@current`, return results consistent with the review scope.
- [ ] The view name describes its purpose, for example `Hierarchy - Q3 Release Scope`.
- [ ] Any parent in `In Progress` whose children are unassigned or not in a sprint has been flagged for follow-up.

## Common Mistakes

- Enabling hierarchy on a board or roadmap view. Hierarchy is a property of the table layout only. Switch the layout to Table first.
- Forgetting that a parent without sub-issues looks identical to a regular issue. If you expected nesting and do not see it, the data is missing sub-issues, not the view configuration. Return to GHE-ALM-017 to add them.
- Grouping by `Status` while expecting to see decomposition. Status grouping flattens the hierarchy by status bucket and makes parent/child relationships harder to read. Group by `Parent issue`, `Release`, or `Product Area` for hierarchy review.
- Treating `Release` mismatches between parent and child as a defect in the view. The view is correct; the data needs a planning decision.
- Saving filter changes to a shared view without telling the team. Other reviewers will see your filter applied. Either save to a personal copy or announce the change.
- Confusing closed parents with completed scope. A parent issue can be closed while open children remain. Always expand a closed parent at least once during release readiness checks (see GHE-ALM-046).

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve when sub-issues span repositories the manager does not have access to and the hierarchy renders incomplete.
- Engineering lead: Involve when a parent in `In Progress` has no decomposition or when child issues have no owner during a sprint window.
- Release manager: Involve when child issues carry a `Release` value different from the parent and the parent is part of a release readiness review.

## Related Guides

- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-026 : How to Use the Product Backlog View
- GHE-ALM-080 : How to Test Feature and Requirement Decomposition
