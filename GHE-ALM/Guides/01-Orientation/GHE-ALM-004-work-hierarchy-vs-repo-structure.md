# How to Distinguish Work Hierarchy from Repository Structure

**Guide ID:** GHE-ALM-004
**Audience:** Engineering Manager, Project Manager, Program Manager
**Primary role:** Engineering Manager
**Classification:** Manager Understands
**Estimated time:** 15-20 minutes one-time read
**Required permissions:** None
**Prerequisites:**

- Familiarity with the GitHub object model (issues, repositories, organizations).
- Optional: prior exposure to Azure DevOps Area Paths or Iteration Paths.

**When to use this guide:** Read this when you are planning ALM in GitHub and find yourself looking for a "folder of repositories", a "parent project", or a place to nest repositories under a feature. Also read this before creating new organizations, teams, or repositories to organize work.

**When not to use this guide:** Skip this if you only need to learn how to perform a specific action like creating a sub-issue or a nested team. Use the task-specific guides instead.

## Outcome

By the end of this guide, you will have produced:

- A clear mental model of the four hierarchy types in GitHub Enterprise.
- A decision rule for choosing the correct hierarchy when grouping work.
- The ability to spot and correct the common "GitHub repos as folders" mistake.

## Before You Start

- Read GHE-ALM-001 for the GitHub object model.
- Have your current org chart or product breakdown nearby for the worked example.
- No GitHub permissions are required to read this guide.

## Steps

### 1. Recognize that GitHub has four hierarchies, not one

GitHub Enterprise exposes four distinct hierarchy mechanisms. They serve different purposes and they do not nest inside one another. Treating them as one combined tree is the most common ALM mistake managers make when arriving from Azure DevOps, Jira, or a file-system mental model.

| Hierarchy Type | GitHub Mechanism | ALM Relevance |
|---|---|---|
| Work hierarchy | Issues, sub-issues, parent issues, Hierarchy View in Projects | High. The main ALM hierarchy. |
| Team hierarchy | Nested teams | Medium. Permissions and ownership. |
| Enterprise hierarchy | Enterprise account, organizations, repositories | Medium. Governance and administration. |
| Code hierarchy | Repository file tree, pull request file tree | Low for ALM. For developers and reviewers. |

The work hierarchy is where features, requirements, tasks, and bugs live. The other three hierarchies are about who owns code, who can access it, and how files are laid out inside a repository. Conflating them produces confused conversations like "where do I put the Checkout feature, in a repo or a team?"

### 2. Use the work hierarchy for ALM decomposition

The work hierarchy is built from issues and sub-issues. An issue can have up to 100 sub-issues, and the structure can nest up to eight levels deep. This is where Initiative, Epic, Feature, Requirement, Task, and Bug relationships are expressed.

```text
Initiative
  Epic
    Feature
      Requirement
        Task
        Bug
```

Visibility comes from the Hierarchy View in GitHub Projects, which renders the nested work directly in a project table. See GHE-ALM-005 for project view configuration and GHE-ALM-017 for sub-issue mechanics.

Decision rule: if the question is "what work is being done, by whom, when, and how does it roll up?", the answer lives in the work hierarchy.

### 3. Use the team hierarchy only for ownership and permissions

Nested teams group people for repository access, code ownership, review routing, and notifications. They are an HR-shaped tree, not an ALM-shaped tree.

```text
Engineering
  Platform
    DevOps
    Security
  Application Engineering
    Customer Portal
    Claims
    Reporting
```

Decision rule: if the question is "who can read or write this code, and who must review it?", the answer lives in the team hierarchy. See GHE-ALM-071 for nested team configuration.

Do not use nested teams as a substitute for Azure DevOps Area Paths unless the team structure exactly mirrors product ownership. If the team tree and the product tree diverge, use a `Product Area` field on the project instead.

### 4. Use the enterprise hierarchy for governance boundaries

The enterprise hierarchy is fixed: Enterprise account contains organizations, organizations contain repositories. There is no level above the enterprise and no level between the organization and the repository.

```text
GitHub Enterprise Account
  Organization: acme-payments
    Repository: checkout-service
    Repository: payments-api
    Repository: web-client
```

Decision rule: if the question is about billing, audit, identity, policy, or who administers what, the answer lives in the enterprise hierarchy. Use organizations to draw real governance boundaries (business unit, product line, compliance scope). Avoid creating organizations just to "group" related repositories visually.

### 5. Treat the code hierarchy as out of scope for ALM

The code hierarchy is the file tree inside a repository. It is meaningful for engineers reading code and for pull request reviewers, but it is not a planning artifact. A folder named `checkout/` inside `payments-api` is not the same thing as a Checkout feature, a Checkout team, or a Checkout product area.

Decision rule: if the question is "where does this source file live?", the answer lives in the code hierarchy. Do not try to mirror it in your ALM structure.

### 6. Apply the decision rule when something feels missing

Most "GitHub is missing X" complaints from managers come from looking in the wrong hierarchy. Use this short table when you cannot find what you expect.

| You are looking for | Wrong place to look | Correct place |
|---|---|---|
| A folder containing related repositories | Enterprise hierarchy | Organization-level Project with a `Product Area` field, plus saved views in the Repository Dashboard |
| Parent-child links between work items | Repository file tree, Teams | Sub-issues, viewed in Hierarchy View |
| A grouping of features by team | Repository name | `Owner` field on the project, or filter the project view by team |
| A roll-up of progress across repos | A single milestone | Organization-level Project with a `Release` field; milestones remain repository-scoped |
| A way to nest one repository under another | Anywhere | Does not exist. Use Projects to group cross-repo work |

If you reach for the enterprise hierarchy to solve a work-grouping problem, stop and switch to the work hierarchy. If you reach for the work hierarchy to solve an access problem, stop and switch to the team hierarchy.

> [SCREENSHOT: Hierarchy View in an organization-level Project showing an Initiative expanded to Epic, Feature, Requirement, and Task across two different repositories]

### 7. Worked example: acme-payments adds a Checkout initiative

The `acme-payments` organization has three repositories: `checkout-service`, `payments-api`, and `web-client`. A new initiative, "Single-page checkout", spans all three. A manager from an Azure DevOps background asks: "Where do I create the Checkout area, and which repository does the initiative go in?"

Apply the four hierarchies in order:

- Work hierarchy: Create one `Initiative` issue titled `Single-page checkout` in the most representative repository (for example, `checkout-service`). Add `Feature` sub-issues for the user-visible capabilities. Under each feature, add `Requirement` sub-issues, and under those, `Task` and `Bug` sub-issues. Issues that touch `payments-api` or `web-client` can be created in those repositories and added to the same organization-level Project.
- Team hierarchy: If a Checkout team owns the work, create or reuse a `acme-payments/checkout` team and grant it `Write` on the three repositories. Use code owners in each repository for review routing. Do not create a team just to label the initiative.
- Enterprise hierarchy: Do not create a new organization for Checkout. The three repositories already share the `acme-payments` organization, which is the correct governance boundary.
- Code hierarchy: A `checkout/` folder may appear inside `web-client`. That is a code layout decision for the engineers and is unrelated to the initiative's tracking.

To see the whole initiative in one place, open the organization-level Project, switch to the Hierarchy View, set `Product Area` to `Checkout`, and group by parent issue. The view spans repositories without requiring any folder, parent project, or repo nesting.

## Validation Checklist

- [ ] You can name the four GitHub hierarchies and one example use of each.
- [ ] You can state the decision rule for picking work, team, enterprise, or code hierarchy.
- [ ] You can explain why GitHub has no "folder of repositories" and what to use instead.
- [ ] You can locate a cross-repository initiative using an organization-level Project rather than a repository.
- [ ] You can distinguish a `Product Area` field from a team name and from a folder name.

## Common Mistakes

- Creating an extra organization to group related repositories. Use an organization-level Project instead.
- Using nested teams to model product structure. Use a `Product Area` field for product structure; reserve teams for permissions.
- Putting an Epic issue in a "tracking" repository that contains no code. Place parent issues in the most representative code repository and pull cross-repo children into the same Project.
- Treating a repository file folder as a feature boundary. Folders are a code layout choice, not an ALM artifact.
- Expecting milestones to span repositories. Milestones are repository-scoped; use a project `Release` field for cross-repo release tracking.
- Looking for an Azure DevOps Area Path. There is no direct equivalent; combine `Product Area`, team ownership, and labels.

## Escalation Path

- GitHub administrator: Involve when a proposed organization or team change would alter governance boundaries, billing, or identity scope.
- Repository administrator: Involve when work hierarchy decisions imply moving issues between repositories or changing repository ownership.
- Engineering lead: Involve when team hierarchy and product structure diverge enough that a `Product Area` field design is needed.
- Release manager: Not applicable for this guide.

## Related Guides

- GHE-ALM-001 : How to Navigate the GitHub Enterprise ALM Object Model
- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-071 : How to Request or Review Nested Teams
