# How to Break Work into Sub-Issues

**Guide ID:** GHE-ALM-017
**Audience:** Project Manager, Engineering Manager, Product Owner
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per parent issue
**Required permissions:** Repository: Triage on the parent repository and on any repository where sub-issues live (Triage permits creating and editing issues; some teams require Write).
**Prerequisites:**

- A parent issue already exists. The parent is typically an Initiative, Epic, Feature, or Requirement created via GHE-ALM-011, GHE-ALM-012, or GHE-ALM-013.
- The repositories that will hold sub-issues are part of the same GitHub organization as the parent.
- You know roughly how the parent decomposes. You do not need a complete breakdown before starting; sub-issues can be added incrementally.

**When to use this guide:** Use this guide whenever you need to decompose a parent work item into smaller, trackable units, or when a previously independent issue should be re-parented under an Epic, Feature, or Requirement so the hierarchy reflects reality.

**When not to use this guide:** Do not use sub-issues to model dependency relationships between unrelated work items. Use GHE-ALM-019 (Issue Dependencies) for blocked-by and blocking links. Do not use sub-issues as a substitute for project field metadata; sub-issues express parent-child decomposition only.

## Outcome

By the end of this guide, you will have produced:

- A parent issue with one or more sub-issues attached, visible in the parent's Sub-issues panel.
- A traceable work breakdown where each sub-issue links back to its parent and shows progress contribution.
- A hierarchy that supports cross-repository programs, with sub-issues placed in whichever repository will deliver the work.

## Before You Start

- Confirm the parent issue ID and URL, for example `acme-payments/checkout-service#412`.
- Decide whether each sub-issue belongs in the same repository as the parent or in a different repository in the same organization.
- Have a draft list of sub-issue titles. Short, outcome-oriented titles work better than implementation notes.
- Know the limits: a parent issue can hold up to 100 sub-issues, and the hierarchy can nest up to 8 levels deep. Keep the tree shallow; three to four levels (Initiative, Epic, Feature, Task) is typical.

## Steps

### Create new sub-issues from the parent

1. Open the parent issue in the browser, for example the Epic `acme-payments/checkout-service#412`.
2. Scroll to the bottom of the issue description. Locate the **Create sub-issue** button.
3. Click **Create sub-issue**. A dialog opens.
4. Enter the sub-issue **Title**. Use an outcome phrase such as `Wire 3DS challenge response into checkout flow`.
5. Optionally add the description, **Issue type** (for example, `Task` or `Feature`), **Assignees**, **Labels**, **Projects**, and **Milestones**. You can leave these blank and apply metadata later via GHE-ALM-020.
6. If you plan to add several sub-issues in sequence, select **Create more sub-issues**. The dialog will reopen after each save.
7. Click **Create**. The sub-issue appears in the parent's **Sub-issues** panel with its issue number, title, and status.

> [SCREENSHOT: Parent issue showing the Create sub-issue button and the resulting Sub-issues panel with several child entries]

### Place a sub-issue in a different repository

8. On the parent issue, click the dropdown triangle next to **Create sub-issue**.
9. Click **Add existing issue**.
10. In the picker, click the back arrow to switch repositories. Select the target repository within the same organization, for example `acme-payments/payments-api`.
11. Create the sub-issue there if it does not yet exist (open a new tab, file the issue using the standard form per GHE-ALM-015), then return to the parent and use **Add existing issue** to attach it.
12. The sub-issue now shows with the cross-repository reference syntax `acme-payments/payments-api#88` in the parent's panel.

### Convert an existing issue into a sub-issue

Use this when an issue was filed independently and should now sit under a parent, for example a bug that turned out to be the first observable symptom of a larger Epic.

13. Open the intended parent issue.
14. Click the dropdown triangle next to **Create sub-issue**, then click **Add existing issue**.
15. In the **Search issues** field, search by issue number or title. Suggestions appear from the current repository; switch repositories with the back arrow when needed.
16. Select the issue to attach. It is now a sub-issue of this parent. The original issue's URL and number do not change.
17. If the issue was previously a sub-issue of a different parent, attaching it here moves it. An issue has at most one parent at a time.

> [SCREENSHOT: Add existing issue dialog showing the repository switcher and search-by-number field]

### Re-parent or detach a sub-issue

18. To remove a sub-issue from a parent, open the parent issue, find the entry in the **Sub-issues** panel, and use the row menu to **Remove sub-issue**. The child issue itself is not closed or deleted; only the parent link is removed.
19. To re-parent, open the new parent and use **Add existing issue** to attach the orphaned issue. Adding it under a new parent automatically replaces any prior parent link.

## Validation Checklist

- [ ] The parent issue's **Sub-issues** panel lists every child you created or attached, with correct titles and issue numbers.
- [ ] Each sub-issue's page shows a parent reference linking back to the parent issue.
- [ ] Cross-repository sub-issues display the `owner/repo#NNN` form, and clicking the reference opens the issue in its home repository.
- [ ] The hierarchy depth from the top-level Initiative to the deepest sub-issue is no more than 8 levels.
- [ ] The parent does not exceed 100 direct sub-issues. If it does, split into intermediate Features.
- [ ] No sub-issue is attached to two parents.

## Common Mistakes

- Treating sub-issues as a tagging mechanism. If two unrelated issues share a sub-issue link, the work breakdown becomes misleading. Use labels (GHE-ALM-021) or project fields for grouping.
- Creating sub-issues with vague titles like `Backend work` or `Phase 2`. Each sub-issue should describe an outcome a single owner can deliver.
- Building a tree wider than 100 children. The UI silently caps at 100; later additions will fail. Introduce an intermediate Feature level and redistribute.
- Nesting too deep. Beyond four or five levels the hierarchy becomes unreadable in the Hierarchy View (GHE-ALM-018) and harder to roll up in dashboards.
- Forgetting that a sub-issue can only have one parent. Attaching to a new parent silently moves the child; verify the previous parent still reflects the intended scope.
- Confusing sub-issues with task list checkboxes in the issue body. Markdown task lists do not create traceable work items and do not appear in Hierarchy View.

## Escalation Path

- GitHub administrator: Involve when sub-issue creation fails for permission reasons across multiple repositories, or when an organization-wide policy is needed for hierarchy depth.
- Repository administrator: Involve when you lack Triage on a target repository and need access to attach sub-issues there.
- Engineering lead: Involve when the decomposition exceeds 100 sub-issues per parent and the team needs to agree on intermediate Feature boundaries.
- Release manager: Not applicable.

## Related Guides

- GHE-ALM-013 : How to Create an Epic or Initiative Issue
- GHE-ALM-015 : How to Create a Task Issue
- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-019 : How to Use Issue Dependencies for Blocked Work
- GHE-ALM-020 : How to Apply Issue Metadata Correctly
