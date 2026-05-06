# How to Use Saved Views for Stakeholder Reporting

**Guide ID:** GHE-ALM-058
**Audience:** Project Manager, Engineering Manager, Release Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 30-45 minute one-time setup per audience, 5 minutes per refresh
**Required permissions:** Project: Write
**Prerequisites:**

- An organization-level GitHub Project already populated with issues, fields, and at least one sprint or release tagged.
- Canonical project fields in place: `Status`, `Priority`, `Severity`, `Sprint`, `Release`, `Product Area`, `Owner`, `Target Date`.
- Agreement with each stakeholder audience on the slice of data they actually consume (usually one or two questions, not "everything").

**When to use this guide:** Use this guide when leadership, QA, release management, or product owners ask for a recurring view of the work and you want to give them a clean, named, shareable URL instead of a custom export, screenshot, or weekly slide.

**When not to use this guide:** Do not use saved views as a substitute for Project Insights charts when the stakeholder needs a trend or aggregate metric. Charts are covered in GHE-ALM-051. Saved views show items, not trends.

## Outcome

By the end of this guide, you will have produced:

- A set of named, audience-specific views inside a single Project, each with its own layout, filter, grouping, sort, and visible-field configuration.
- A short distribution list pairing each view name with a URL and the named stakeholder audience.
- A simple convention for who maintains each view and how often it is reviewed.

## Before You Start

- Decide which audiences you will serve first. A typical first pass covers Leadership, QA, Release Management, Product Owner, and Engineering Manager.
- For each audience, write one sentence describing the question the view answers. Examples: "Which releases are at risk this quarter?" or "Which open bugs in the current sprint are high severity?"
- Confirm Project: Write access. Anyone with Project: Read can use the views you publish; only Project: Write can create or rename them.

## Steps

### Plan the view set

1. Open the organization-level Project in GitHub.
2. For each target audience, write one line in your own notes capturing four things: audience name, single question, layout type (Table, Board, or Roadmap), and the two or three fields that audience actually cares about. Keep this list short. A reader who sees more than five or six fields will stop reading.
3. Confirm naming convention with the team. Recommended pattern: `<Audience> : <Topic>`. Examples: `Leadership : Release Health`, `QA : This Sprint`, `Release Mgmt : 2026.05.0 Scope`, `Product Owner : Backlog Top 20`. See GHE-ALM-077 for naming convention guidance.

### Create the Leadership Release Health view

4. In the Project, find the view tab strip across the top. Click the `+` next to the existing tabs to add a new view.
5. Name the view `Leadership : Release Health`.
6. Open the view options icon next to the search bar and set **Layout** to **Roadmap**.
7. In the roadmap configuration, set the date fields to `Start Date` and `Target Date`. Set the timespan to **Quarter** so leadership sees the next 90 days at a glance.
8. Set **Group by** to `Release`. This produces one swimlane per release train.
9. Open the field visibility menu and turn off every field except `Status`, `Owner`, and `Target Date`. Leadership does not need `Effort`, `Sprint`, or label noise.
10. Apply a filter that hides closed items and items with no release: `is:open has:"Release"`. Add `-status:Done` if your team uses a custom Done status.
11. Save the view. GitHub auto-saves view configuration when you make changes; confirm there is no unsaved-changes indicator on the tab.

> [SCREENSHOT: Leadership : Release Health view in Roadmap layout, grouped by Release, showing Status, Owner, and Target Date only]

### Create the QA This Sprint view

12. Click `+` on the view tab strip again. Name the view `QA : This Sprint`.
13. Set **Layout** to **Table**.
14. Apply the filter `type:Bug sprint:@current is:open`. This pulls open bugs scoped to the active iteration.
15. Set **Group by** to `Severity`. QA leads scan top-down: severity 1, then 2, then 3, then 4.
16. Sort within group by `Priority` ascending so P0 sits above P3.
17. Show only these fields: `Title`, `Severity`, `Priority`, `Status`, `Owner`, `Target Date`. Hide everything else.
18. Confirm the view loads in under three seconds. If it does not, the filter is probably too broad. Add `product-area:<your area>` to scope it.

### Create the remaining audience views

19. Repeat the create-name-configure pattern for each remaining audience. Suggested starting set:

   - `Release Mgmt : 2026.05.0 Scope`. Table layout. Filter `release:"2026.05.0"`. Group by `Status`. Visible fields: `Title`, `Status`, `Owner`, `Sprint`, `Target Date`.
   - `Product Owner : Backlog Top 20`. Table layout. Filter `is:open no:Sprint -status:Done`. Sort by `Priority` ascending, then `Target Date` ascending. Visible fields: `Title`, `Priority`, `Product Area`, `Target Date`.
   - `Engineering Mgr : Owner Load`. Board layout grouped by `Owner`. Filter `is:open sprint:@current`. Visible fields: `Title`, `Status`, `Severity`.

20. After saving each view, check that the tab order across the top reads logically. Drag tabs left or right to group views by audience.

> [SCREENSHOT: View tab strip showing the named audience views in order, with the active view highlighted]

### Share the views with stakeholders

21. Click the view tab whose URL you want to share. The browser address bar updates to a URL containing the project path and a view identifier.
22. Copy the URL. Send it to the stakeholder audience. Anyone with Project: Read on the Project will land directly on that view.
23. Maintain a short distribution list (in the Project description, a team wiki page, or a pinned message) mapping audience to view URL. Example row: `Leadership Release Health -> https://github.com/orgs/acme-payments/projects/14/views/3`. See GHE-ALM-005 for how to teach stakeholders to read a Project view they land on cold.
24. Tell each audience the refresh cadence: "This view is live. Reload at any time. The data behind it is updated as the team works the issues."

### Maintain the view set

25. Assign one named owner per view. The owner is responsible for renaming, retiring, or refiltering when the underlying field set or release changes.
26. Review the full view list once per quarter. Retire views that no audience opened in 90 days. Stale views accumulate, look authoritative, and mislead.
27. When you add a new release or sprint, audit the views that hard-code release names or iteration filters and update them. The filter `sprint:@current` updates automatically; a literal filter like `release:"2026.05.0"` does not.

## Validation Checklist

- [ ] Each audience has at least one named view following the `<Audience> : <Topic>` convention.
- [ ] Each view answers the one-line question captured during planning.
- [ ] Each view shows no more than six fields.
- [ ] Each view URL has been sent to its intended audience and tested in a private browser session by an account with Project: Read.
- [ ] Every view has a named owner recorded somewhere durable.
- [ ] No view has unsaved changes (no indicator on the tab).
- [ ] The view tab strip reads in a logical order for first-time visitors.

## Common Mistakes

- Building one mega-view that tries to satisfy every audience. Stakeholders ignore views that show fields they do not understand. Make several small views instead.
- Using literal release names in filters and forgetting to update them. Prefer `sprint:@current`, `is:open`, and grouped views over hard-coded values where possible.
- Sharing the Project URL instead of the view URL. The Project URL drops users on whichever view loads first, which is rarely the one the audience needs.
- Hiding `Status` to make the view look cleaner. Status is the one field every audience needs.
- Granting Project: Write to view consumers so they can "see what you see." Project: Read is sufficient. Write access lets stakeholders accidentally rename or delete views.
- Creating views and never reviewing them. Stale views are worse than no views.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Not applicable.
- Engineering lead: Involve when a stakeholder requests a view that requires a new project field or a change to an existing field's allowed values.
- Release manager: Involve when the Release Management view needs to track a release the team has not yet defined in the `Release` field.

## Related Guides

- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-055 : How to Run a Monthly ALM Metrics Review
- GHE-ALM-077 : How to Enforce Naming Conventions
