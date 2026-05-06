# How to Navigate the GitHub Enterprise ALM Object Model

**Guide ID:** GHE-ALM-001
**Audience:** Project Manager, Program Manager, Release Manager
**Primary role:** Project Manager
**Classification:** Manager Understands
**Estimated time:** 20-30 minutes one-time read
**Required permissions:** None for reading. Read access to at least one organization is helpful when you want to follow along in the portal.
**Prerequisites:**

- Familiarity with the general idea of an issue, a repository, and a pull request.
- A GitHub Enterprise login that lets you see at least one organization.

**When to use this guide:** Read this guide once, before any other GHE-ALM guide, so that the names you see in the portal, in reports, and in other guides map to a consistent mental model.

**When not to use this guide:** Do not use this guide to perform configuration. It teaches the model, not the clicks. For navigation tasks, use GHE-ALM-002. For project view choices, use GHE-ALM-005.

## Outcome

By the end of this guide, you will be able to:

- Name the eight ALM objects that matter to managers and explain how they relate.
- Point to the correct object when asked where work is planned, tracked, reviewed, released, or deployed.
- Trace one piece of work from intake through deployment without confusing the work hierarchy with the repository hierarchy.

## Before You Start

- Have your enterprise name, one organization name, and one repository name written down. You will use these as anchors while reading.
- Know which of your products is currently active. You will use it for the worked example in step 8.
- No special permissions are required. Read access to one organization is enough to look around.

## Steps

The sections below build on each other. Read in order.

### 1. The eight objects you need to know

GitHub Enterprise ALM rests on eight objects. Memorize them in this order, because the order reflects how work flows:

```
Enterprise Account
  Organization
    Repository
      Issue
      Pull Request (covered in later guides)
    Environment
  Project
  Milestone (lives in a Repository, but planned at Project level)
  Release (lives in a Repository, tied to a Git tag)
  Actions workflow run (lives in a Repository, deploys to an Environment)
```

Everything else in GitHub Enterprise is either a property of one of these objects, a view over them, or a permission rule that controls them.

### 2. Enterprise Account

The **Enterprise Account** is the top container. It holds billing, identity, and policy. It is where central administrators set rules that apply to every organization underneath. Managers rarely act inside the Enterprise Account, but you should know it exists, because audit reports, license counts, and policy questions live there.

### 3. Organization

An **Organization** holds repositories, teams, and organization-level **Projects**. Most ALM work is scoped to an organization. A product line, a business unit, or a major program is usually represented as one organization. When someone asks "where is the backlog?", the answer almost always begins with an organization name.

### 4. Repository

A **Repository** holds source code, issues, pull requests, releases, milestones, environments, and Actions workflows for one product or one component. A repository is the smallest unit that owns code and the issues filed against that code. A repository is not a folder. It is not a hierarchical container for other repositories. If you find yourself drawing a tree of repositories, you are probably modeling something that belongs in a Project or in the work hierarchy. See GHE-ALM-004.

### 5. Issue and the work hierarchy

An **Issue** is the unit of tracked work. Issues live inside a repository. Issues can have a custom **issue type** such as `Epic`, `Feature`, `Requirement`, `Task`, `Bug`, or `Risk`. Issues can be nested using **sub-issues**, up to eight levels deep, with up to 100 sub-issues per parent. This is where Epic to Feature to Requirement to Task decomposition lives.

The work hierarchy is independent of the repository hierarchy. A parent issue in one repository can have sub-issues in other repositories within the same organization, which is what makes cross-repository programs practical.

### 6. Project

A **Project** is the planning surface. A Project is created at organization level (or, less commonly, at user level) and pulls in issues and pull requests from one or many repositories. A Project provides table, board, roadmap, hierarchy, and insights views. Projects own custom fields such as `Status`, `Priority`, `Iteration`, `Release`, and `Target Date`. Projects are the right place to plan, prioritize, and report.

A useful rule: issues store the truth about a single piece of work. Projects store the plan that arranges many pieces of work.

### 7. Milestones, Releases, Actions, and Environments

These four objects describe how work becomes a deployed change.

- **Milestone:** a repository-scoped goal with a due date and a completion percentage. Group issues and pull requests under a milestone when you need a simple, repository-level view of "what we are trying to finish by date X."
- **Release:** a published artifact tied to a Git tag in a repository. A Release records that a specific commit was packaged and named, for example `v2.4.0`. Releases are immutable history.
- **Actions workflow run:** an automated execution defined in the repository, used for builds, tests, packaging, and deployments. Each run has logs and a status that managers can read without running anything.
- **Environment:** a named deployment target inside a repository, such as `staging` or `production`. Environments can require reviewers and can restrict which branches or tags are allowed to deploy. Approvals on environments are where managers most often act inside the deployment flow.

> [SCREENSHOT: a diagram showing Enterprise Account at the top, Organizations beneath it, Repositories beneath each Organization, and a Project spanning multiple repositories. Inside one repository, show Issues, Milestones, Releases, Environments, and Actions workflow runs as labeled boxes.]

### 8. A worked example: one feature, end to end

Follow a single feature through the model. Assume the organization is `acme-payments` and the repository is `checkout-service`.

1. **Intake.** A product owner files a feature request as an **Issue** in `acme-payments/checkout-service`, with issue type `Feature`. See GHE-ALM-011.
2. **Decomposition.** An engineering lead adds three **sub-issues** of type `Requirement` and links a fourth `Task` issue from `acme-payments/checkout-ui`. The work hierarchy now spans two repositories.
3. **Planning.** All four issues are added to the organization-level **Project** named `Payments 2026 Plan`. The Project sets `Status = Ready`, `Priority = High`, and `Iteration = Sprint 22`. See GHE-ALM-006.
4. **Targeting.** The feature is associated with a **Milestone** named `2026-Q3 Release` in `checkout-service` so the repository view also shows the deadline.
5. **Execution.** Engineers open pull requests against `checkout-service`. Pull requests reference the issues. Reviews happen on the pull requests, not on the issues.
6. **Build and verify.** Each pull request triggers an **Actions workflow run** that builds and tests the change. Managers can read run status without configuring anything.
7. **Release.** When the milestone is complete, engineering publishes a **Release** named `v2.4.0` in `checkout-service`, tied to a Git tag.
8. **Deploy.** The release tag triggers an Actions workflow that deploys to the `production` **Environment**. Because production requires reviewer approval, the release manager approves the deployment in the portal.

The same feature touched all eight objects. Plan in the Project. Track in Issues. Group toward a date in the Milestone. Review in pull requests. Build with Actions. Package as a Release. Deploy through an Environment, governed by the Enterprise and Organization.

### 9. Decision rules to keep handy

- "Where is the backlog?" Project, not Repository.
- "Where is the work item?" Issue, in a Repository.
- "Where is the deadline?" Milestone for repository-scoped dates, Project field for cross-repository dates.
- "Where is the artifact?" Release.
- "Where is the deployment?" Environment, driven by an Actions workflow run.
- "Where is the policy?" Enterprise or Organization.

## Validation Checklist

- [ ] You can list the eight objects from memory in the order Enterprise, Organization, Repository, Issue, Project, Milestone, Release, Actions, Environment.
- [ ] You can answer the six questions in step 9 without looking them up.
- [ ] You can explain why a repository is not a folder and why the work hierarchy can cross repositories.
- [ ] You can trace one of your own current features through the eight steps in the worked example.

## Common Mistakes

- Treating the Repository as the planning surface. The Project is the planning surface.
- Treating Repositories as a folder tree. They are not nested. Use the work hierarchy or a Project for grouping.
- Confusing a Milestone with a Release. A Milestone is a planned goal with a date. A Release is a published artifact tied to a Git tag.
- Asking engineers to "deploy the milestone." Deployments run from Actions into Environments, usually triggered by a Release or a branch event.
- Looking for sprints inside a Repository. Sprints are an iteration field on a Project.

## Escalation Path

- GitHub administrator: when you need a new Organization, enterprise-wide policy clarification, or audit data above the organization level.
- Repository administrator: when you need a new repository, a new Environment, or changes to required reviewers.
- Engineering lead: when issue type taxonomy, sub-issue structure, or repository scope needs to change for a product.
- Release manager: when Releases, tags, or production Environment approvals are unclear.

## Related Guides

- GHE-ALM-002 : How to Find the Correct Organization, Repository, and Project
- GHE-ALM-003 : How to Use the Repository Dashboard
- GHE-ALM-004 : How to Distinguish Work Hierarchy from Repository Structure
- GHE-ALM-005 : How to Interpret GitHub Project Views
- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
