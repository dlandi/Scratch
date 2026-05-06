# How to Find the Correct Organization, Repository, and Project

**Guide ID:** GHE-ALM-002
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per use, faster after the first two or three uses
**Required permissions:** Organization: `Member` for the target organization. Repository: `Read` on the target repository. Project: `Read` on the target Project.
**Prerequisites:**

- A GitHub Enterprise login.
- The product or release name you are trying to find work for.
- One known reference point if you can get one: an organization name, a repository name, an issue number, or a Project URL from a teammate.

**When to use this guide:** Use this guide whenever you need to land in the right planning surface and you are not already bookmarked there. Typical triggers: a new release cycle starts, you are assigned to a new product, you join a sprint review for a team you do not normally work with, or a stakeholder sends you a question and you need to confirm where it should be tracked.

**When not to use this guide:** Do not use this guide when you already have a working bookmark to the correct Project view. Do not use this guide to learn what an organization, repository, or Project is. Read GHE-ALM-001 first if those terms are unfamiliar.

## Outcome

By the end of this guide, you will have produced:

- A confirmed organization name, repository name, and Project name for the product or release you are working on.
- An open browser tab on the correct Project view, ready for backlog, sprint, bug, or release work.
- A saved bookmark or pinned tab so you do not have to repeat the search next time.

## Before You Start

- Have the product or release name written down. Names like `acme-checkout` or `2026.05.0` are easier to search for than vague descriptions.
- If you have any reference link (an issue URL, a pull request URL, a teammate's bookmark), keep it open. One reference link collapses most of this work into a single click.
- Confirm you can sign in to your GitHub Enterprise tenant. If the organization you need does not appear anywhere in your top navigation, you may not be a member yet. See the Escalation Path.

## Steps

### Find the organization

1. Sign in to your GitHub Enterprise tenant in the browser.
2. Click your profile picture in the top right, then click **Your organizations**. The page lists every organization you belong to.
3. Scan the list for the organization that owns the product. Use the worked example as a pattern: a product named `acme-checkout` typically lives in an organization named `acme-checkout`, `acme-payments`, or `acme-platform`. Organization names usually map to a product family, a business unit, or a platform, not to a single team.
4. If the list is long, type part of the name in the **Filter organizations** box. If nothing matches, you are probably not a member of the organization that owns the product. Stop here and follow the Escalation Path.
5. Click the organization name to open its landing page. You should now see organization-level tabs including **Overview**, **Repositories**, **Projects**, **Teams**, and **People**.

> [SCREENSHOT: Your organizations page with the filter box and a worked example like `acme-checkout` highlighted]

### Find the repository

6. From the organization landing page, click the **Repositories** tab. You will see every repository in the organization that you can read.
7. Use the **Find a repository** search to narrow the list. Search by product or component name first, for example `checkout-service` or `payments-api`. Repository names map to a single deployable unit or library, not to a product family.
8. If you do not know the exact repository name, sort by **Recently pushed** and look at the top entries. The repository receiving active work for the current release is almost always near the top.
9. If your tenant has the **Repository Dashboard** enabled, you can also reach it from the **Repositories** icon in the top navigation bar or by going to `github.com/repos`. The dashboard lets you filter by organization, language, and visibility, and it offers built-in views for **My contributions**, **My repositories**, **My forks**, and **Admin access**. Use it when the repository spans more than one organization you belong to, or when you need to compare several repositories side by side. For a deeper walkthrough, see GHE-ALM-003.
10. Click the repository name to open it. Confirm you are in the right place by checking the description, the most recent commit date, and the **About** panel on the right. A stale repository (no commits in months) is rarely the one you want for an active release.

> [SCREENSHOT: Repository Dashboard with the organization filter applied and a worked example repository selected]

### Find the Project

11. Decide whether the Project you want is at organization level or repository level. Cross-repository release trains, multi-team backlogs, and executive roadmaps almost always live at organization level. A Project that exists only to track one repository's internal work may live at repository level, but this is less common in the GHE-ALM model.
12. To find an organization-level Project, return to the organization landing page and click the **Projects** tab. The list shows every Project in the organization that you can read. Use the search box to filter by Project name. Project naming should follow your team's standard, typically combining a product area and a scope, for example `Checkout - 2026 Roadmap` or `Payments - Active Sprints`.
13. To find a repository-level Project, open the repository and click the **Projects** tab. The list shows Projects linked to that repository. If you see organization-level Projects listed here as well, that means an admin has already linked them to the repository, which is the recommended setup.
14. Open the candidate Project. Confirm it is the right one by checking three things: the Project description or README, the field set (a real ALM Project will have `Status`, `Priority`, `Severity`, `Sprint`, `Release`, and `Product Area` defined), and the most recently updated items. A Project with no recent activity and only default fields is usually a stub, not the live planning surface.
15. Inside the Project, click the view tabs along the top to confirm the views you expect are present: a backlog table, a current sprint board, a release roadmap, and a bug triage view. If those views are missing, you may be in a personal or experimental Project rather than the team's ALM Project. Look for a different Project, or ask the team.

> [SCREENSHOT: Organization Projects tab with the search box and a worked example Project like `Checkout - Active Sprints` highlighted]

### Confirm and save the destination

16. Once you are in the correct Project view, copy the URL.
17. Bookmark the URL in your browser. Name the bookmark using the same convention as the Project, for example `Checkout - Active Sprints`. Pin the tab if you will use it daily.
18. If your team maintains a shared landing page or wiki of Project links, add or verify your bookmark there so the next person searches for two minutes instead of twenty.

## Validation Checklist

- [ ] You can name the organization, the repository, and the Project for the product or release.
- [ ] The Project shows recent activity, has the canonical fields (`Status`, `Priority`, `Severity`, `Sprint`, `Release`, `Product Area`), and contains the views you expect.
- [ ] The repository you opened has a recent commit and matches the component you were asked about.
- [ ] You have bookmarked the Project URL or pinned the tab.
- [ ] If asked, you can paste a Project URL into chat and a teammate confirms it is the right one.

## Common Mistakes

- Treating the organization name as a product name. Organizations usually represent a business unit or platform; one organization can hold many products.
- Treating the repository as the planning surface. Repositories hold code, issues, milestones, and releases. Cross-repository planning lives in an organization-level Project.
- Opening a personal Project that has the right name but the wrong owner. Check the breadcrumb at the top of the Project page; it should show an organization name, not a personal handle.
- Stopping at the first match. Some product names appear in more than one organization, especially after reorganizations. Confirm the description and recent activity before you commit your time to that Project.
- Using a stale bookmark from a previous release cycle. Project URLs survive renames, but the team may have moved planning to a different Project for a new release. Re-verify at the start of every release.
- Searching only by exact name. Use partial matches and the **Recently pushed** sort. The right repository is often one or two characters off from what you remembered.

## Escalation Path

- GitHub administrator: Involve when the organization you need does not appear in **Your organizations**. The administrator confirms whether the organization exists and adds you as a member if appropriate. Also involve if no Project visible to you matches the product, in case the Project exists but you lack `Read` access.
- Repository administrator: Involve when you can see the organization and repository list but the specific repository you need is missing. The repository may be private to a team you have not joined.
- Engineering lead: Involve when you have found candidate repositories and Projects but cannot tell which is canonical. The engineering lead for the product confirms the live planning surface.
- Release manager: Involve when you are looking for a release-specific Project (a release train, a hotfix track) and you cannot identify which Project the release manager is using to coordinate scope.

## Related Guides

- GHE-ALM-001 : How to Navigate the GitHub Enterprise ALM Object Model
- GHE-ALM-003 : How to Use the Repository Dashboard
- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-072 : How to Request Repository Access for Project Managers and Stakeholders
