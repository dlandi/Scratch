# How to Create and Use an Organization-Level GitHub Project

**Guide ID:** GHE-ALM-006
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Performs / Manager Requests
**Estimated time:** 30-minute one-time setup
**Required permissions:** Organization membership with the Project creator role, or Organization Owner. If you do not have project creation rights, send a request to an organization owner using the template at the end of this guide.
**Prerequisites:**

- You know which GitHub organization will own the project.
- You have agreed on a working name for the product, release, or program the project will track.
- You have read GHE-ALM-005 (How to Interpret GitHub Project Views) so you understand table, board, roadmap, and hierarchy views.
- You know which repositories will feed work into this project.

**When to use this guide:** Use this guide when you need a new ALM tracking surface that spans more than one repository, for example a product line, a release train, or a program that pulls issues from several repos.

**When not to use this guide:** Do not use this guide for a project that lives inside a single repository and will never need cross-repo visibility. A repository-level project is sufficient for that case. Also skip this guide if a suitable organization project already exists; use GHE-ALM-002 to find it instead.

## Outcome

By the end of this guide, you will have produced:

- An empty organization-level GitHub Project shell, owned by the correct organization, with a working name and a chosen starting layout (Table, Board, or Roadmap, or a template-based layout).
- A project URL you can share with stakeholders for follow-on configuration in GHE-ALM-007 through GHE-ALM-010.

## Before You Start

- Confirm the exact organization name (for example `nokia-platform-eng`). Creating the project under the wrong organization is the most common rework cause.
- Decide the starting layout: **Table** for backlog work, **Board** for sprint flow, **Roadmap** for release timelines. You can add other views later.
- Decide whether to start from a blank layout or from a template. Templates are useful when your organization has a standard ALM project layout already published.
- Have a one-line project description ready. You will refine the full name and README in GHE-ALM-007.
- Confirm your permission level. In GitHub, click your profile picture, choose **Your organizations**, click the organization name, then check whether **New project** appears on the **Projects** tab. If it does not, you do not have creation rights.

## Steps

1. In the top-right corner of any GitHub page, click your profile picture, then click **Your organizations**.
2. Click the name of the organization that should own the new project.
3. On the organization page, click the **Projects** tab.

> [SCREENSHOT: Organization landing page with the Projects tab highlighted in the top navigation row]

4. Click **New project** in the top-right of the Projects list. A project creation dialog opens.
5. Under **Start from scratch**, choose one of the blank layouts: **Table**, **Board**, or **Roadmap**. To use a pre-built layout instead, scroll to **Templates** and pick a built-in template (for example **Team planning**, **Feature release**, **Bug tracker**, **Iterative development**, **Product launch**, or **Team retrospective**) or an organization-published template if your organization owners have created one.
6. If you selected a template, review the **Fields**, **Views**, **Workflows**, and **Insights** preview shown on the right side of the dialog. These are what the template will create for you. If anything looks wrong for your use case, pick a different template or fall back to a blank layout.

> [SCREENSHOT: New project dialog showing the Start from scratch options on the left and the Templates section below]

7. In the **Project name** field, enter your working project name. Use the convention from GHE-ALM-007 if it is already published. A safe interim pattern is:

   ```
   <Product or Program> ALM <Year>
   ```

   For example: `Edge Routing ALM 2026`. You can rename later.
8. Optional. If you want the project to start with existing work loaded, click **Import items from repository** and choose the repository to import from. Note that the chosen repository becomes the project's default repository. If you are unsure, skip this; you can add items later using GHE-ALM-008.
9. Click **Create project**. GitHub creates the project and opens it in the view you selected.

> [SCREENSHOT: Newly created empty project showing the chosen layout and the project name in the page header]

10. Copy the project URL from the browser address bar. It will look like `https://github.com/orgs/<org>/projects/<number>`. Save it where your team will find it (a team wiki, a pinned chat message, or the relevant repository README).
11. Click the gear icon (**Settings**) in the top-right of the project. Confirm that **Visibility** matches your intent. Organization projects default to **Private**. Leave it private unless you have an explicit reason and authority to make it public.

> [SCREENSHOT: Project settings panel showing the Visibility section]

12. Stop here. Do not configure fields, custom views, or workflows in this guide. Continue with GHE-ALM-007 to set the formal name and description, then GHE-ALM-008 to populate the project, then GHE-ALM-009 for auto-add intake.

### If you cannot create the project yourself

If **New project** is not visible, or you receive a permission error, you do not have project creation rights in that organization. Send a request to an organization owner. Use this template:

```
Subject: Request to create organization-level GitHub Project

Organization: <org name, for example nokia-platform-eng>
Requested project name: <working name, for example Edge Routing ALM 2026>
Starting layout: <Table | Board | Roadmap | Template name>
Purpose: <one sentence, for example "Track epics, requirements, sprints, and
releases for the Edge Routing product line in 2026.">
Repositories that will feed this project: <repo1, repo2, repo3>
Visibility: Private
Project admins to add after creation: <github-handle-1>, <github-handle-2>
Reason I cannot create it myself: I do not have the Project creator role
in this organization.
```

Once the owner creates the shell and grants you admin access on the project, return to this guide and pick up from step 10.

## Validation Checklist

- [ ] The project URL begins with `https://github.com/orgs/<org>/projects/` (organization-scoped), not `https://github.com/<user>/projects/` (user-scoped) or a repository-scoped URL.
- [ ] The project opens in the layout you selected (Table, Board, Roadmap, or template layout).
- [ ] The project name matches the agreed working pattern.
- [ ] The project is private unless you confirmed otherwise with an organization owner.
- [ ] You have saved the project URL where your team can find it.
- [ ] You have not yet added items, fields, or custom views; those steps belong to later guides.

## Common Mistakes

- Creating the project under your personal user account instead of the organization. The URL will start with `/users/` or your handle. Delete it and start over inside the correct organization.
- Picking a template and then fighting its built-in fields. If the template does not match your ALM model, start from a blank layout instead.
- Importing items from a repository at creation time without realizing that repository is now set as the project's default. If the wrong default is set, change it later in project **Settings**.
- Renaming the project repeatedly during setup. Pick a working name now and finalize it in GHE-ALM-007.
- Setting visibility to **Public** to make sharing easier. Organization ALM projects almost always contain internal planning data; keep them private.
- Creating duplicate projects because you could not find an existing one. Run GHE-ALM-002 first.

## Escalation Path

- GitHub administrator: Involve the enterprise GitHub administrator if the organization itself is missing, if the **Projects** tab is hidden by organization policy, or if no one in the organization has the Project creator role.
- Repository administrator: Not applicable for project shell creation. Repository admins become relevant in GHE-ALM-008 (adding items) and GHE-ALM-009 (auto-add workflows).
- Engineering lead: Involve the engineering lead to confirm scope when the project will span repositories owned by multiple engineering teams.
- Release manager: Involve the release manager when the project will be the primary surface for release tracking, so the release fields and milestones are aligned from day one (covered in GHE-ALM-041).

## Related Guides

- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-007 : How to Name and Describe a GitHub ALM Project
- GHE-ALM-008 : How to Add Existing Issues and Pull Requests to a Project
- GHE-ALM-009 : How to Configure Auto-Add Workflows for Project Intake
- GHE-ALM-026 : How to Use the Product Backlog View
