# How to Configure or Request a Sprint Iteration Field

**Guide ID:** GHE-ALM-027
**Audience:** Project Manager, Engineering Manager, Scrum Master
**Primary role:** Project Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 20-minute one-time setup; 10-minute periodic review each quarter
**Required permissions:** Project: Admin to configure; Project: Read to review. Without Project Admin, raise a request to a project administrator.
**Prerequisites:**

- The organization-level GitHub Project exists (see GHE-ALM-006).
- A sprint cadence decision has been made by the team: duration, start day, naming pattern, and any planned breaks.
- A list of upcoming holidays or shutdown periods that should appear as breaks.

**When to use this guide:** Use this guide to stand up the `Sprint` iteration field on a Project for the first time, or to inspect an existing iteration field for configuration drift before sprint planning resumes after a long gap.

**When not to use this guide:** Do not use this guide for sprint planning itself; for selecting items into the next sprint, use GHE-ALM-028. Do not use this guide for daily standup execution; use GHE-ALM-029.

## Outcome

By the end of this guide, you will have produced:

- A `Sprint` iteration field on the Project, configured with the agreed duration, start date, naming pattern, and breaks.
- Confirmation that the canonical filter `sprint:@current` returns the active iteration on Project views.
- Either a documented self-configuration or a written request to a project administrator covering every required parameter.

## Before You Start

- Confirm the field name your team will use. Default to `Sprint`. Some teams use `Iteration`; the filter syntax adapts to the field name (lowercase fieldname colon value).
- Confirm sprint length. The default recommendation is two weeks.
- Confirm the start date of the next sprint, expressed as a calendar date.
- Confirm the naming convention. Two common patterns: `Sprint YYYY.NN` (year and ISO-week-style number, for example `Sprint 2026.18`) and short integer counters such as `Sprint 27`.
- Confirm planned breaks. Common cases include a December holiday shutdown, national holidays, and any scheduled team off-sites.
- Confirm whether you hold Project Admin on the target Project. If not, you will be filing a request rather than configuring directly.

## Steps

### Decide the iteration parameters

1. Record the **duration** in days or weeks. Two weeks is the recommended default.
2. Record the **start date** for the first iteration the field will track. Pick a date no earlier than the next planning meeting so the field begins on a clean boundary.
3. Record the **naming pattern**. Use one pattern across the Project; mixing `Sprint 27` and `Sprint 2026.18` in the same field makes filtering and reporting painful.
4. Record the **breaks**. List each break as a start date and duration. Typical examples: a one-week year-end break, a national holiday week, a one-day off-site.
5. Record the **lookahead window** you want visible. GitHub creates three initial iterations on field creation; document how many additional future iterations should be generated and maintained so planners can drag work into upcoming sprints.

### Configure the field if you hold Project Admin

6. Open the Project and switch to a **table** layout.
7. Scroll to the rightmost column header and click the plus icon to add a field.
8. Select **New field**, set the field type to **Iteration**, and set the name to `Sprint`.
9. Set the **start date** and **duration** to the recorded values. GitHub will auto-generate three initial iterations using these settings.
10. Save the field. Open the field configuration again to add or adjust the generated iterations.
11. Rename each generated iteration to follow the recorded naming pattern. Click the iteration entry and edit the label inline.
12. Insert each planned break by adding a break entry between iterations. Adjust the break dates on the calendar interface so the next iteration resumes on the correct day.
13. Add additional future iterations until the lookahead window is satisfied. New iterations inherit the most recent duration; rename them to match the pattern.

> [SCREENSHOT: Iteration field configuration panel showing duration, start date, named iterations, and an inserted break]

### Validate the field on a view

14. Return to a Project table view. Add a column for the new `Sprint` field if it is not already shown.
15. Apply the filter `sprint:@current` and confirm the view returns items in the active iteration. An empty result is expected if no items have been assigned yet; the filter should still parse without error.
16. Apply the filter `sprint:@next` and confirm the view returns the next iteration window.
17. Apply the filter `no:sprint` and confirm the view returns items that have not been placed in any iteration.

### File a request if you do not hold Project Admin

18. If you do not hold Project Admin, do not attempt to reconfigure the field through workarounds. Send the request below to the project administrator named on the Project settings page.

## Sample Request to Send

Use this template when filing a request. Fill in every bracketed value before sending.

> Subject: Request to configure Sprint iteration field on Project `[Project name]`
>
> Project URL: `[paste URL]`
>
> Please configure an iteration field on this Project with the following settings.
>
> - Field name: `Sprint`
> - Duration: `[2 weeks]`
> - Start date of first tracked iteration: `[YYYY-MM-DD]`
> - Naming pattern: `[Sprint YYYY.NN]` or `[Sprint NN]`. Apply this pattern to all generated iterations.
> - Breaks:
>   - `[YYYY-MM-DD]` to `[YYYY-MM-DD]`: `[reason, e.g., year-end shutdown]`
>   - `[YYYY-MM-DD]` to `[YYYY-MM-DD]`: `[reason]`
> - Lookahead window: maintain `[N]` future iterations beyond the current one so planners can place upcoming work.
>
> Filters that must work after configuration: `sprint:@current`, `sprint:@next`, `sprint:@previous`, and `no:sprint`.
>
> Owner for ongoing maintenance: `[name or team]`.
>
> Confirm completion by replying with the field URL and a screenshot of the configured iterations.

## What Good Looks Like vs. What to Escalate

Use the following table when reviewing an existing iteration field rather than creating a new one.

| Aspect | What good looks like | What to escalate |
|---|---|---|
| Field name | Single field named `Sprint` (or `Iteration`) on the Project | Multiple iteration fields competing for the same role; field renamed mid-cycle |
| Duration | Consistent across iterations, matching the team's published cadence | Iterations of varying length without a documented reason |
| Naming pattern | One pattern applied consistently | Mixed patterns, manual one-off names, or numbering gaps |
| Start date alignment | First iteration begins on the team's chosen weekday | Iterations starting mid-week with no rationale |
| Breaks | Holidays and shutdowns are explicit breaks, not silently absorbed into a sprint | Sprints that span a known shutdown without acknowledgement |
| Lookahead | At least two future iterations exist for planning | Only the current iteration exists, blocking next-sprint planning |
| Filter behavior | `sprint:@current` returns the active iteration on every relevant view | Filters return empty or error; views rely on hard-coded iteration names |

## Validation Checklist

- [ ] The field is named `Sprint` (or the agreed alternative) and is of type Iteration.
- [ ] Duration matches the team's documented cadence.
- [ ] Iteration names follow a single pattern.
- [ ] Every planned break is present with correct dates.
- [ ] At least two future iterations are visible beyond the current one.
- [ ] `sprint:@current`, `sprint:@next`, `sprint:@previous`, and `no:sprint` all parse and return expected results.
- [ ] The field appears on the backlog, sprint planning, and current sprint views, not only on the table where it was created.

## Common Mistakes

- Creating a second iteration field instead of editing the existing one, leaving two competing fields on the Project.
- Mixing naming patterns within the same field.
- Treating a holiday week as a normal sprint, then losing capacity tracking when half the team is unavailable.
- Allowing the lookahead to run out, so the next-sprint planning view is empty when planners arrive.
- Using stale filter syntax such as `Sprint = @current` instead of the canonical `sprint:@current`.
- Renaming the field after sprints have been logged, which breaks saved views and historical charts.

## Escalation Path

- GitHub administrator: Not applicable for iteration field configuration; the field is a Project-scoped object.
- Repository administrator: Not applicable.
- Engineering lead: Involve when the team disagrees on duration or start day, or when a break decision crosses multiple teams.
- Release manager: Involve when iteration boundaries need to align with a release train or hardening sprint that spans multiple Projects.

Project administrator: Involve when you do not hold Project Admin yourself, or when the field already exists and a destructive change (rename, duration change, deletion) is being considered.

## Related Guides

- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-028 : How to Plan the Next Sprint
- GHE-ALM-029 : How to Use the Current Sprint Board
- GHE-ALM-030 : How to Move Unfinished Work to a Later Sprint
- GHE-ALM-076 : How to Govern Project Fields and Labels
