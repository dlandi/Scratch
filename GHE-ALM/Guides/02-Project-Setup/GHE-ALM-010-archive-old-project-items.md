# How to Archive Completed or Old Project Items

**Guide ID:** GHE-ALM-010
**Audience:** Project Manager, Engineering Manager, Program Manager
**Primary role:** Project Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 20-minute one-time request, 15 minutes per quarterly review
**Required permissions:** Project: Admin to configure; Project: Write to view archived items and restore
**Prerequisites:**

- An organization-level GitHub Project already exists (see GHE-ALM-006).
- The Project has a defined `Status` field with a closed state such as `Done`, and items are being closed consistently.
- A Project administrator or maintainer is available to configure or update the auto-archive workflow.

**When to use this guide:** Use this guide when a Project's table, board, or roadmap views are cluttered with completed, closed, or stale items that obscure the active work, and you want to keep the historical record accessible without deleting it.

**When not to use this guide:** Do not use this guide to permanently remove items. Archive hides items from default views; it does not delete them. If you genuinely need to remove an item from the Project entirely, that is a separate action covered by Project hygiene, not auto-archive.

## Outcome

By the end of this guide, you will have produced:

- An auto-archive workflow request with a defined criterion such as "items closed and not updated in the last 3 weeks" sent to your Project administrator, or a confirmation that the existing workflow is correctly configured.
- A documented review of which items are currently archived, with restoration steps known to the team.
- A clear distinction in your team's runbook between archive (hidden, retrievable) and delete (removed, not retrievable).

## Before You Start

- Confirm who administers the target Project. Auto-archive is configured in **Workflows**, which requires Project admin access.
- Decide the archive criteria the team wants. Typical choices: closed for at least 3 weeks, or completed and not updated in the last month.
- Confirm whether your Project has reached or is approaching the 50,000-item Project limit. If yes, archiving alone will not free space; raise this with the GitHub administrator.

## Steps

### Decide the archive policy

1. Agree the archive trigger with your team. Common policies for the `acme-payments` org:
   - Items where `is:closed` and `updated:last 3 weeks` for active product Projects.
   - Items where `is:closed` and `reason:completed` and `updated:last month` for slower-moving Projects such as `acme-platform`.
2. Decide whether `reason:not planned` items should also be archived. Most teams archive these on the same schedule as completed items.
3. Decide whether merged pull requests in the Project should be archived. PR rows often clutter `Status: Done` on the board and are usually safe to archive once merged.
4. Document the chosen policy in the Project description so the team can find it later (see GHE-ALM-076 for field and label governance).

### Submit the auto-archive request

5. Open a request to your Project administrator using the **Sample Request to Send** template below. Include the Project URL, the chosen filter, and your justification.
6. Ask the administrator to navigate to the Project, open the kebab menu in the top right, choose **Workflows**, then **Auto-archive items**, click **Edit**, set the filter, and click **Save and turn on workflow**.

> [SCREENSHOT: Project Workflows panel with Auto-archive items selected and the filter field visible]

### Review the existing or new auto-archive configuration

7. Open the Project and click the kebab menu in the top right.
8. Select **Workflows**, then **Auto-archive items**.
9. Confirm the workflow is **On** and that the filter matches the agreed policy. Supported qualifiers are limited to `is:` (open, closed, merged, draft, issue, pr), `reason:` (completed, reopened, "not planned"), and `updated:` (last 14 days, last 3 weeks, last month).
10. If the filter is wrong, request a change rather than editing it yourself unless you are the Project administrator.

### Inspect and restore archived items

11. From the Project, click the kebab menu in the top right and select **Archived items**.
12. Use the text filter to find a specific item by title or number.
13. To restore one or more items, check the boxes to the left of each row, then click **Restore** above the list. The items return to the Project with their custom field values intact.
14. Record any restored items in your weekly notes so the team knows why they reappeared on the board.

## Sample Request to Send

Send this to the Project administrator (typically the GitHub administrator or DevOps lead listed for `acme-payments`):

> Subject: Auto-archive workflow request, Project `acme-payments / Delivery 2026`
>
> Hello,
>
> Please configure the auto-archive workflow on the Project at `https://github.com/orgs/acme-payments/projects/12` with the following criteria:
>
> - Filter: `is:closed reason:completed,"not planned" updated:last 3 weeks`
> - Apply to issues and pull requests.
> - Turn the workflow on after saving.
>
> Justification: The board view currently shows over 400 closed items in `Status: Done`, which makes the active sprint and roadmap views slow to scan. Archiving preserves the items and their field values; it does not delete them. The criteria match our quarterly hygiene policy documented in the Project description.
>
> Please confirm once the workflow is enabled and share a screenshot of the Workflows panel for our records.
>
> Thank you,
> [Name], Project Manager

## What Good Looks Like vs. What to Escalate

| Signal | What Good Looks Like | What to Escalate |
|---|---|---|
| Auto-archive workflow status | **On**, with a filter the team agreed to. | **Off**, or a filter no one on the team recognizes. |
| Filter content | Uses `is:closed`, a clear `reason:`, and an `updated:` window of last 14 days, last 3 weeks, or last month. | Filter is empty, archives `is:open` items, or uses an unsupported qualifier. |
| Active Project views | Board, table, and roadmap views show predominantly active or recently closed work. | Default views show hundreds of items closed months ago. |
| Archived items page | Reachable from the Project kebab menu, items have field values intact, restore works. | Items are missing fields, restore returns errors, or the page is empty when items are clearly missing from views. |
| Project item count | Well under the 50,000-item Project limit. | Approaching or above 50,000 items; archiving alone will not resolve this and warrants a separate cleanup conversation. |
| Distinction from delete | Team understands archive is reversible; delete is not. | Team members refer to archive and delete interchangeably or have used **Delete from project** for routine cleanup. |
| Documentation | Archive policy is recorded in the Project description or runbook. | No one can state the team's archive criteria when asked. |

## Validation Checklist

- [ ] The auto-archive workflow is **On** with the agreed filter.
- [ ] The filter uses only supported qualifiers (`is:`, `reason:`, `updated:`).
- [ ] The Project's default board, table, and roadmap views no longer show large volumes of stale closed work.
- [ ] The **Archived items** page is reachable from the Project kebab menu.
- [ ] At least one team member has practiced restoring an item from the archive.
- [ ] The archive policy is recorded somewhere the team can find, such as the Project description.
- [ ] The team understands that archive is retrievable and **Delete from project** is not.

## Common Mistakes

- Treating archive as deletion. Archived items keep their custom field data and can be restored from the **Archived items** page.
- Using **Delete from project** for routine cleanup of completed work. Delete is permanent and removes the item from the Project, including its field values.
- Setting `updated:last 14 days` on a slow-moving Project, which can archive items the team is still discussing.
- Forgetting that the filter applies to both issues and pull requests. Merged PRs that the team still references in standups can disappear from the board sooner than expected.
- Editing the workflow filter without informing the team, then being surprised when items appear or disappear from views.
- Assuming archive frees space against the 50,000-item Project limit. It does not.

## Escalation Path

- GitHub administrator: Involve when the Project administrator role is unclear, or when the Project is approaching the 50,000-item limit.
- Repository administrator: Not applicable. Auto-archive is a Project-level feature, not a repository feature.
- Engineering lead: Involve when the team disagrees on the archive policy, particularly the treatment of `reason:not planned` items.
- Release manager: Involve when archived items include release-tagged work that release reporting still depends on.

## Related Guides

- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-009 : How to Configure Auto-Add Workflows for Project Intake
- GHE-ALM-076 : How to Govern Project Fields and Labels
