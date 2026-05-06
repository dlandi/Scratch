# How to Use the Repository Dashboard

**Guide ID:** GHE-ALM-003
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes for the first walkthrough, under 2 minutes per use after that
**Required permissions:** Organization: `Member` for any organization whose repositories you want to see. Repository: `Read` (or higher) on the repositories you expect to find. No special permission is needed to open the dashboard itself.
**Prerequisites:**

- A GitHub Enterprise login.
- Membership in at least one organization that owns repositories you care about.
- A short list of the products, components, or organizations you most often need to find. The dashboard is most useful when you arrive with a specific question rather than browsing.

**When to use this guide:** Use this guide when you need to locate a repository across an enterprise that has dozens or hundreds of them, when you want a single page that shows everything you can read or administer, or when you want to save filtered repository views you will return to weekly.

**When not to use this guide:** Do not use this guide to learn what a repository is or how it relates to organizations and Projects. Read GHE-ALM-001 first if those terms are unfamiliar. Do not use the Repository Dashboard as a release or sprint dashboard; release scope and sprint health belong in Projects, Insights, and milestones.

## Outcome

By the end of this guide, you will have produced:

- A working understanding of the four built-in dashboard views: **My contributions**, **My repositories**, **My forks**, and **Admin access**.
- At least one saved custom view that filters repositories by organization, language, or visibility for a workflow you repeat.
- A confirmed list of the repositories where you hold `Admin` access, so you know which repositories you can configure directly versus which require a request.

## Before You Start

- Note any organization names you expect to see, for example `acme-payments`, `acme-platform`, or `acme-checkout`. Filtering by organization is the fastest way to cut a long list down.
- Decide which built-in view answers your current question. Looking for a repository you have worked in recently? **My contributions**. Looking for repositories you can configure? **Admin access**. Looking for a repository you forked for a spike? **My forks**.
- If you cannot see a repository you expect, the most likely cause is missing `Read` access, not a dashboard problem. Keep GHE-ALM-072 handy if you need to request access.

## Steps

### Open the Repository Dashboard

1. Sign in to your GitHub Enterprise tenant in the browser.
2. Open the dashboard using whichever path is fastest:
   - Go directly to `github.com/repos`.
   - Click the **Repositories** icon in the top global navigation bar.
   - Open the command palette and search for **Repositories**, then select the dashboard entry.
3. Confirm you are on the dashboard by checking that the page header reads **Repositories** and that the left side shows a list of built-in views starting with **My contributions**.

> [SCREENSHOT: Repository Dashboard landing page with the built-in views list visible on the left]

### Switch between built-in views

4. Click **My contributions**. This view lists repositories you have recently committed to, opened issues in, or reviewed pull requests in. Use it as your default starting point because it surfaces work you are actively involved in.
5. Click **My repositories**. This view lists repositories you own or are explicitly a member of, regardless of recent activity. Use it when you need a complete inventory of what you have access to, not just where you have been active.
6. Click **My forks**. This view lists repositories you have forked. Use it to find personal copies created for spikes, experiments, or contribution workflows. Forks are usually not the canonical planning surface for a product.
7. Click **Admin access**. This view lists every repository where you hold the `Admin` role. Treat this as your accountability list: these are the repositories whose settings, rulesets, environments, and access lists you can change directly without filing a request.

### Filter and sort within a view

8. With any built-in view open, use the filter controls at the top of the results to narrow the list:
   - **Organization** filter: pick one organization, for example `acme-payments`, to scope the view to a single business unit.
   - **Language** filter: pick a primary language to find back-end services, front-end clients, or infrastructure repositories quickly.
   - **Visibility** filter: pick `Private`, `Internal`, or `Public` when you need to confirm exposure level, for example before discussing a repository in a public forum.
9. Sort the filtered list by relevance to push repositories you work with most often to the top. Glance at the activity indicators next to each repository to spot stale or inactive entries before you click in.
10. If a single filter is not enough, combine filters. For example, organization `acme-checkout` plus language `TypeScript` plus visibility `Internal` will isolate the internal TypeScript services owned by the Checkout business unit.

> [SCREENSHOT: Filtered Repository Dashboard with Organization, Language, and Visibility filters applied and the result list narrowed]

### Save a custom view

11. Once a filter combination produces a list you will want to return to, save it. Use the bookmark or save control on the filter bar to capture the current filter state as a named view.
12. Name the view with a convention that describes the filter, not the moment, for example `acme-checkout - Internal TypeScript` or `acme-payments - Admin access`. Avoid names like `My filter 1` that will not be meaningful to you next month.
13. Open the saved view from the views list to confirm it loads with the same filters you applied. Saved views are personal to you; they do not change what other managers see in their dashboards.
14. Repeat for the two or three filter combinations you reach for most often. A practical starter set for a manager covering one product family is: one view per organization you work in, one view limited to your `Admin access` repositories within the primary organization, and one view scoped to a specific language or component family.

### Find repositories by organization, team, or owner

15. To find every repository in a specific organization, open any built-in view and apply only the **Organization** filter. This is the fastest path when a stakeholder asks "what does `acme-platform` own?"
16. To find repositories aligned to a specific team, the dashboard's organization filter is your starting point; from there, open the organization, click **Teams**, then open the team to see its repository list. The dashboard does not filter by team directly, but the organization filter gets you most of the way there.
17. To find repositories you do not own but need to monitor, open **My contributions** with no filters first. If a repository is missing, you have not interacted with it recently. Open **My repositories** next; if it is still missing, you likely lack `Read` access. File a request using GHE-ALM-072.

### Identify your admin-access repositories

18. Open the **Admin access** view. Read the full list end to end. Each entry here is a repository whose configuration you can change directly: settings, rulesets, branch protection, environments, secrets, deploy keys, and team access.
19. For each entry, ask whether you should hold `Admin` on it. `Admin` is the broadest repository role and should be limited to repositories you are accountable for. If you see repositories where `Maintain` or `Write` would be sufficient, raise this with the GitHub administrator so the role can be adjusted.
20. Save the **Admin access** view as a named saved view, for example `My admin repos`. Re-open it at the start of each quarter as part of a permission review. Compare against the previous quarter's list to spot scope creep.

## Validation Checklist

- [ ] You can open the Repository Dashboard from `github.com/repos`, the top-navigation icon, or the command palette.
- [ ] You can switch between **My contributions**, **My repositories**, **My forks**, and **Admin access** and explain what each view shows.
- [ ] You have applied at least one combined filter (for example, organization plus language) and saved the result as a named view.
- [ ] You can name every repository in the **Admin access** view and have confirmed each one belongs there.
- [ ] You have at least one saved view that you will reuse in a recurring workflow, such as a weekly review or a release readiness check.

## Common Mistakes

- Treating the dashboard as a release or sprint dashboard. The Repository Dashboard surfaces repositories, not release scope. Release health lives in Project roadmap views, Insights charts, and milestones.
- Saving views with vague names. A view called `My filter` is invisible in three weeks; a view called `acme-checkout - Internal TypeScript` is self-explanatory.
- Confusing **My repositories** with **Admin access**. Membership in a repository does not mean you can change its settings. The **Admin access** view is the authoritative answer to "what can I configure?"
- Assuming a missing repository is a dashboard bug. The dashboard only shows what your account can read. A repository you cannot see in any view is almost always a permission issue.
- Filtering by **Visibility** and forgetting to clear the filter. A view stuck on `Private` will hide internal and public repositories until you reset it.
- Treating forks as canonical. Repositories under **My forks** are personal copies; the planning surface for a product almost always lives in the original repository under an organization.

## Escalation Path

- GitHub administrator: Involve when an organization you expect to see is missing from every view, when you suspect your `Admin access` list contains repositories you should not own, or when the dashboard itself is not loading at `github.com/repos`.
- Repository administrator: Involve when a specific repository is missing from your views and you believe you should have at least `Read` access to it. The repository administrator can grant access directly or via a team.
- Engineering lead: Involve when you can see candidate repositories but cannot tell which one is the canonical home for a product or component. The engineering lead confirms which repository is the live one.
- Release manager: Not applicable for the Repository Dashboard itself. Release coordination uses Projects, milestones, and the release roadmap, not the dashboard.

## Related Guides

- GHE-ALM-001 : How to Navigate the GitHub Enterprise ALM Object Model
- GHE-ALM-002 : How to Find the Correct Organization, Repository, and Project
- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-072 : How to Request Repository Access for Project Managers and Stakeholders
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
