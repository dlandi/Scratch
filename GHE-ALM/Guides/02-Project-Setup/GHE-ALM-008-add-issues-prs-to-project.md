# How to Add Existing Issues and Pull Requests to a Project

**Guide ID:** GHE-ALM-008
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per use
**Required permissions:** Project: Write; Repository: Read (Triage or Write to add from the repository issue list)
**Prerequisites:**

- The target Project exists at the organization level (see GHE-ALM-006).
- You can locate the issues or pull requests you want to add (you have read access to the source repository).
- You have a list, query, or set of URLs identifying the work that belongs in the Project.

**When to use this guide:** Use this guide to seed an existing organization Project with the work items that already live as issues or pull requests in one or more repositories. Use it any time you need to add a small set of items by hand, recover items that were missed by an auto-add workflow, or pull historical work into a new Project.

**When not to use this guide:** Do not use this guide to set up automatic intake of newly opened issues and pull requests. For continuous intake, configure an auto-add workflow (GHE-ALM-009). Do not use this guide to clean up old or completed items; for that, see GHE-ALM-010.

## Outcome

By the end of this guide, you will have produced:

- A Project that contains the correct initial set of issues and pull requests.
- A consistent method for adding further items as the Project evolves.
- Confidence that adding items did not move, copy, or alter the underlying issues.

## Before You Start

- Confirm the Project URL. Organization Projects live at `https://github.com/orgs/<org>/projects/<number>`.
- Know which organizations and repositories the work lives in. Items can come from any repository the Project is connected to and that you can read.
- Understand the difference between adding and moving. Adding an issue places a reference in the Project. The issue still lives in its repository. Removing an item from the Project deletes the reference, not the issue. Closing or deleting the underlying issue is a separate action in the source repository.
- Decide whether you want to add items one at a time, in bulk from a repository, or via auto-add. This guide covers the manual paths.

## Steps

### Open the Project

1. Navigate to your organization, click **Projects**, and open the target Project (for example, `acme-payments` -> Projects -> `Checkout 2026.05.0 Delivery`).
2. Pick any view: table, board, or roadmap. All four manual add methods work from any view.

> [SCREENSHOT: Organization Project open in table view, showing the bottom row with the plus icon and an empty cell next to it]

### Method 1: Paste an issue or pull request URL

3. Click the empty cell in the bottom row, next to the plus icon at the bottom of the view.
4. Paste the full URL of the issue or pull request, for example `https://github.com/acme-payments/checkout-service/issues/482`.
5. Press **Enter**. The item appears as a new row with its title, repository, and any project field defaults applied.
6. Repeat for additional URLs. This is the fastest method when you have a known list of links from a planning document, email, or chat.

### Method 2: Search by repository and issue inside the Project

7. Click the empty cell in the bottom row.
8. Type `#`. A repository picker opens listing repositories visible to the Project.
9. Select the repository (for example `acme-payments/checkout-service`). The picker switches to issues and pull requests in that repository.
10. Type part of the issue number or title to filter, then click the item to add it. Use this when you know the work exists but do not have the URL handy.

### Method 3: Bulk add from a repository via the plus button

11. Click the **+** icon in the bottom row of any view.
12. Select **Add item from repository**.
13. If needed, switch the repository using the repository selector at the top of the picker.
14. Use the checkboxes to select multiple issues or pull requests. You can use the header checkbox to select all visible items.
15. Click **Add selected items**. All selected items appear in the Project with default field values.

> [SCREENSHOT: Bulk add picker open with three issues checked in `acme-payments/checkout-service` and the Add selected items button highlighted]

### Method 4: Add from the repository's issue or pull request sidebar

16. Open the repository (for example `acme-payments/payments-api`) and navigate to **Issues** or **Pull requests**.
17. To add a single item, open the issue or pull request, find the **Projects** field in the right sidebar, click it, and select the target Project. Optionally set field values such as `Status`, `Priority`, `Sprint`, or `Release` while the panel is open.
18. To add several items at once from the list view, tick the checkbox next to each issue or pull request, click **Projects** above the list, and select the target Project. The items are added with default field values.

> [SCREENSHOT: Repository Issues list with three issues checked and the Projects dropdown open showing the target Project selected]

### Verify and tidy the additions

19. Switch the Project view to the table layout and confirm each new row shows the expected title, repository, issue type, and `Status`. New items default to `Status: No Status` unless a workflow sets it.
20. Apply any required project field values that did not carry over: `Sprint`, `Release`, `Product Area`, `Owner`, `Priority`, `Severity`, `Effort`, `Start Date`, `Target Date`. See GHE-ALM-020 for the full metadata checklist.
21. If you added an item by mistake, click the row's overflow menu and choose **Remove from project**. This deletes only the Project reference; the underlying issue or pull request is untouched.

## Validation Checklist

- [ ] Each intended issue and pull request appears in the Project at least once.
- [ ] Repository column shows the correct source repository for each item.
- [ ] No duplicates are present (search for the issue number to confirm).
- [ ] `Status` is set to a valid value for items that should appear on the sprint board or backlog view.
- [ ] Removing a test item from the Project did not close or delete the underlying issue.
- [ ] Items added from multiple repositories are visible in the Project's table view.

## Common Mistakes

- Confusing add with move. Adding an item to a Project does not move the issue from its repository. The issue continues to live in the repository where it was opened.
- Confusing remove with delete. **Remove from project** deletes the Project reference only. To delete the underlying issue, you must do that in the repository, and only with appropriate permissions.
- Adding the same item twice from different methods. The Project allows duplicate references, which inflate counts in insights. Search by issue number before adding.
- Skipping field population. Items added through the sidebar or bulk add inherit defaults only. Sprint, Release, and Priority must be set before the item shows up correctly in the sprint board, backlog, or roadmap.
- Pasting the URL of a comment, commit, or discussion. Only issue and pull request URLs are accepted in the bottom row.
- Using manual add when auto-add is already configured. New qualifying items may already be flowing in via GHE-ALM-009, so a manual add becomes a duplicate. Check the Project's workflows before bulk-adding from a repository.
- Forgetting the 50,000 item cap per Project (active and archived combined). For very large historical imports, plan archive policy first (GHE-ALM-010).

## Escalation Path

- GitHub administrator: When the repository you need to add from is not visible in the Project's repository picker, the Project may not be linked to that repository or the organization may restrict cross-repository access.
- Repository administrator: When you cannot read or triage issues in the source repository, request the appropriate role (`Read` to add via the Project, `Triage` or `Write` to add from the repository's issue or PR list).
- Engineering lead: When you are unsure whether a pull request belongs in the Project, or when an item appears to be assigned to the wrong product area, sprint, or release.
- Release manager: Not applicable.

## Related Guides

- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-007 : How to Name and Describe a GitHub ALM Project
- GHE-ALM-009 : How to Configure Auto-Add Workflows for Project Intake
- GHE-ALM-010 : How to Archive Completed or Old Project Items
- GHE-ALM-020 : How to Apply Issue Metadata Correctly
