# GitHub Enterprise as an ALM Platform

**Audience:** Engineering Management  
**Date:** 2026-05-06  
**Scope:** Evaluation of GitHub Enterprise for Application Lifecycle Management, Agile planning, requirements tracking, bug tracking, release tracking, dashboards, and comparison to Azure DevOps Boards.

---

## 1. Executive Summary

GitHub Enterprise can support a substantial Application Lifecycle Management workflow, including feature requests, requirements, sprint planning, task tracking, bug tracking, release tracking, dashboards, branch governance, pull requests, CI/CD, and deployment governance.

The strongest conclusion is this:

> GitHub Enterprise is viable as an ALM platform when the organization is willing to configure a GitHub-native process model. It should not be evaluated as a direct clone of Azure DevOps Boards.

GitHub Enterprise is strongest when engineering execution is tightly connected to source code, branches, pull requests, reviews, GitHub Actions, releases, deployment environments, security controls, and repository governance.

Azure DevOps Boards remains stronger for formal out-of-the-box ALM semantics, especially work item process templates, Area Paths, Iteration Paths, sprint dashboards, velocity, burndown, cumulative flow, portfolio planning, and PMO-style reporting.

The March 2026 general availability of **Hierarchy View in GitHub Projects** materially improves GitHub Enterprise's suitability for ALM. GitHub Projects can now show nested work directly in project tables, and GitHub Issues support sub-issues up to eight levels deep. This makes Epic -> Feature -> Requirement -> Task style decomposition much more practical than older GitHub Projects models.

---

## 2. Bottom-Line Recommendation

GitHub Enterprise should be considered a credible ALM platform if the organization wants:

- Work tracking close to source code.
- Strong traceability from requirement to issue, branch, pull request, build, release, and deployment.
- Configurable rather than process-prescriptive project management.
- Organization-level planning across multiple repositories.
- A single platform for engineering execution, code review, CI/CD, security, and release packaging.
- Teams already operating primarily in GitHub.

Azure DevOps Boards should remain the preferred option if the organization requires:

- Formal Azure DevOps-style process templates such as Agile, Scrum, Basic, or CMMI.
- Native Area Path and Iteration Path governance.
- Strong built-in Scrum analytics such as velocity, sprint burndown, burnup, cumulative flow, cycle time, and lead time widgets.
- Portfolio planning and PMO reporting with minimal customization.
- A traditional ALM experience designed first for project managers and business analysts.

Recommended evaluation posture:

> Use GitHub Enterprise as the ALM platform if a pilot proves that GitHub Projects, issue types, sub-issues, hierarchy view, iteration fields, milestones, releases, and insights satisfy the organization's release governance and reporting needs.

---

## 3. Capability Summary

| ALM Capability | GitHub Enterprise Support | Assessment |
|---|---:|---|
| Feature request tracking | Strong | Supported with Issues, issue type `Feature`, issue forms, fields, labels, and Projects. |
| Requirements tracking | Strong if configured | Use custom issue type `Requirement`, issue forms, project fields, and hierarchy. |
| Epic and feature hierarchy | Stronger after March 2026 | Use sub-issues and GitHub Projects Hierarchy View. |
| Task tracking | Strong | Use issue type `Task`, sub-issues, or issue task lists for small checklist items. |
| Bug tracking | Strong | Use issue type `Bug`, bug report issue forms, severity, priority, milestones, and triage views. |
| Sprint planning | Good | Use GitHub Projects iteration fields. |
| Scrum board | Good | Use board view grouped by status and filtered to current iteration. |
| Release tracking | Good, but distributed | Use milestones, project release fields, GitHub Releases, tags, Actions, and Environments. |
| Dashboards | Moderate to good | Use Project Insights, roadmap views, repository dashboard, issue dashboard, saved views, and external reporting if needed. |
| Cross-repository planning | Good | Use organization-level Projects and custom fields. |
| Portfolio planning | Moderate | Possible through hierarchy and Projects, but Azure DevOps remains stronger out of the box. |
| CI/CD | Strong | Use GitHub Actions. |
| Deployment governance | Strong | Use GitHub Environments, approvals, branch restrictions, and deployment rules. |
| Branch and PR governance | Strong | Use rulesets, protected branches, required reviews, and required status checks. |
| Formal ALM process templates | Weak to moderate | Must be configured. GitHub does not impose Azure DevOps process semantics. |

---

## 4. Important Terminology Correction: GitHub Does Not Have Repository Folders

Some references to hierarchy in GitHub Enterprise can be misunderstood as "hierarchical folders" for repositories. GitHub Enterprise does not provide a traditional folder system for organizing repositories.

Instead, GitHub provides several distinct hierarchy mechanisms:

| Hierarchy Type | GitHub Mechanism | ALM Relevance |
|---|---|---|
| Work hierarchy | Issues, sub-issues, parent issues, Hierarchy View in Projects | High. This is the main ALM hierarchy. |
| Team hierarchy | Nested teams | Medium. Useful for permissions and ownership. |
| Enterprise hierarchy | Enterprise account -> organizations -> repositories | Medium. Useful for governance and administration. |
| Code hierarchy | Repository file tree and pull request file tree | Low for ALM. Useful for developers and reviewers. |

The ALM-relevant hierarchy is the work hierarchy:

```text
Initiative
  Epic
    Feature
      Requirement / User Story
        Task
        Bug
```

This structure should be implemented using GitHub Issues, custom issue types, sub-issues, project fields, and GitHub Projects Hierarchy View.

---

## 5. GitHub Enterprise ALM Building Blocks

### 5.1 Issues

GitHub Issues are the basic work item unit. They can represent:

- Features.
- Requirements.
- User stories.
- Tasks.
- Bugs.
- Risks.
- Change requests.
- Release work.
- Investigation items.

For ALM usage, every significant work item should be represented as an issue, not only as text in a pull request or markdown checklist.

### 5.2 Issue Types

GitHub supports organization-level issue types. The default issue types are:

- `Task`
- `Bug`
- `Feature`

Custom issue types can be added at the organization level, subject to GitHub's limits. For ALM, create a minimal standard issue type model:

| Issue Type | Purpose |
|---|---|
| `Initiative` | Large business objective or release theme. |
| `Epic` | Major capability or program-level deliverable. |
| `Feature` | User-visible capability. |
| `Requirement` | Formal business, functional, non-functional, or system requirement. |
| `Story` | Optional, only if the team uses user stories. |
| `Task` | Engineering work item. |
| `Bug` | Defect or regression. |
| `Risk` | Release, delivery, technical, or operational risk. |
| `Change Request` | Controlled scope change. |

Recommended governance rule:

> Use issue types for core ALM semantics. Use labels for secondary classification.

Do not rely only on labels for major work item categories if issue types are available.

### 5.3 Sub-Issues and Parent/Child Work Breakdown

GitHub supports sub-issues to break down larger work items. Sub-issues can themselves contain sub-issues, allowing a full hierarchy. GitHub documentation states that an issue can have up to 100 sub-issues and that issue hierarchies can nest up to eight levels deep.

Recommended hierarchy:

```text
Initiative
  Epic
    Feature
      Requirement
        Task
        Bug
```

This gives engineering managers visibility into decomposition, progress, ownership, and blocked work without losing traceability to implementation.

### 5.4 Hierarchy View in GitHub Projects

Hierarchy View in GitHub Projects reached general availability in March 2026. It allows nested work items to appear directly in project table views. This is a major improvement for ALM use cases because it allows managers and leads to view complex work breakdowns without opening each issue individually.

Use Hierarchy View for:

- Epic-to-feature planning.
- Feature-to-requirement decomposition.
- Requirement-to-task planning.
- Cross-repository work breakdown.
- Release scope review.
- Program-level status inspection.

### 5.5 Project Fields

GitHub Projects should be configured with a controlled field set. Recommended fields:

| Field | Type | Purpose |
|---|---|---|
| `Status` | Single-select | Workflow state. |
| `Type` | Issue type | Work item category. |
| `Priority` | Single-select | Business priority. |
| `Severity` | Single-select | Bug severity. |
| `Effort` / `Story Points` | Number or single-select | Planning estimate. |
| `Sprint` | Iteration | Sprint or agile cycle. |
| `Release` | Single-select or text | Cross-repository release target. |
| `Product Area` | Single-select | Product, module, or component. |
| `Owner` | Assignee or team field | Responsible person or team. |
| `Start Date` | Date | Roadmap and scheduling. |
| `Target Date` | Date | Release or delivery target. |
| `Risk Level` | Single-select | Delivery or production risk. |
| `Customer Impact` | Single-select | Business/customer impact. |

Governance warning:

> Do not recreate every Azure DevOps field. Start with the minimum field set required for planning, execution, and reporting.

### 5.6 Iteration Fields

GitHub Projects iteration fields are the best GitHub equivalent for sprints. An iteration field can associate project items with repeating blocks of time, can have custom lengths, can include breaks, and can be used to group, filter, sort, and visualize upcoming work.

Recommended configuration:

```text
Field name: Sprint
Duration: 2 weeks
Naming convention: Sprint 2026.10, Sprint 2026.11, Sprint 2026.12
Use breaks: Holidays, planned shutdowns, major offsites
```

Recommended saved filters:

```text
Current Sprint: Sprint = @current
Next Sprint Planning: Sprint = @next
Unplanned Backlog: no:Sprint
```

### 5.7 Milestones

Milestones are useful for repository-scoped release goals. They group issues and pull requests toward a defined target, usually with a due date and completion progress.

Use milestones for:

- `v1.0`
- `v1.1`
- `2026-Q2 Release`
- `May 2026 Release`
- `Hotfix 2026.05.1`

Important limitation:

> Milestones are repository-scoped. For cross-repository releases, use a Project-level `Release` field in addition to milestones.

### 5.8 GitHub Releases and Tags

GitHub Releases package versioned deliverables. A release can include release notes, contributor mentions, and binary files. Releases are connected to Git tags.

Use GitHub Releases for:

- Final release notes.
- Versioned artifacts.
- Source snapshots.
- Binary downloads.
- Release comparison.

Recommended release chain:

```text
Issue / Requirement
  -> Branch
    -> Pull Request
      -> Merge
        -> Tag
          -> GitHub Release
            -> Deployment workflow
```

### 5.9 GitHub Actions and Environments

GitHub Actions should be used for CI/CD. GitHub Environments should be used for deployment governance.

Use Environments for:

- Development.
- Test.
- Staging.
- UAT.
- Production.

Recommended environment controls:

- Required reviewers for production.
- Branch restrictions for release branches.
- Environment-specific secrets.
- Deployment history.
- Manual approval gates.
- Protection rules where required.

### 5.10 Rulesets and Branch Protection

GitHub rulesets and branch protection rules provide governance over branches and tags.

Recommended protected branches:

```text
main
release/*
hotfix/*
```

Recommended controls:

- Require pull request before merge.
- Require at least one approval.
- Require code owner review for sensitive areas.
- Require status checks to pass.
- Require branches to be up to date before merge where appropriate.
- Block force pushes.
- Restrict who can push to release branches.
- Protect release tags.
- Require signed commits if compliance requires it.

---

## 6. Recommended GitHub ALM Information Architecture

### 6.1 Enterprise and Organization Structure

Recommended model:

```text
GitHub Enterprise Account
  Organization: Business Unit or Product Line
    Repository: Application source code
    Repository: Service source code
    Repository: Infrastructure as Code
    Organization Project: Product ALM Board
```

Guidance:

- Use the enterprise account for global governance, billing, policy, audit, identity, and administration.
- Use organizations for business units, product lines, or governance boundaries.
- Use repositories for code, infrastructure, documentation, and service-specific work.
- Use organization-level Projects for cross-repository ALM tracking.

Avoid excessive organization fragmentation. Use organizations when there is a real governance, compliance, security, product, or business boundary.

### 6.2 Nested Teams

Nested teams are useful for ownership and permissions. They are not a substitute for ALM hierarchy.

Example:

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

Use nested teams for:

- Repository access.
- Code ownership.
- Review routing.
- Notifications.
- Organizational alignment.

Do not use nested teams as a substitute for Area Paths unless the team structure directly mirrors product ownership.

---

## 7. Azure DevOps to GitHub Enterprise Mapping

| Azure DevOps Concept | GitHub Enterprise Equivalent | Fit | Implementation Guidance |
|---|---|---:|---|
| Organization | Enterprise account / organization | Good | GitHub Enterprise can centrally manage organizations. |
| Project | Organization + repositories + organization Project | Partial | GitHub does not have the same project container semantics. |
| Repository | Repository | Strong | Direct match. |
| Team | GitHub Team | Strong | Use teams and nested teams for ownership and permissions. |
| Area Path | Product Area field, labels, teams, or repositories | Partial | No exact GitHub equivalent. |
| Iteration Path | Project Iteration field | Good | Works well for sprint cycles, but is not identical to Azure DevOps hierarchy. |
| Epic | Custom issue type `Epic` | Good | Use as parent issue. |
| Feature | Issue type `Feature` | Strong | Default GitHub issue type. |
| Requirement | Custom issue type `Requirement` | Good | Add as organization-level issue type. |
| User Story | Custom issue type `Story` or `Requirement` | Good | Only use if team uses story language. |
| Task | Issue type `Task`, sub-issue, or task list | Strong | Use sub-issues for trackable tasks. |
| Bug | Issue type `Bug` | Strong | Default GitHub issue type. |
| Parent/child links | Sub-issues | Good | Supports nested work breakdown. |
| Sprint backlog | Project view filtered to current iteration | Good | Use `Sprint = @current`. |
| Product backlog | Project table view | Good | Use status, priority, type, release, and product area fields. |
| Delivery Plan | Roadmap view | Partial | Useful but not as formal as Azure DevOps Delivery Plans. |
| Query | Saved view, project filter, issue search | Partial | GitHub saved views are useful but not equivalent to Azure Boards queries. |
| Dashboard | Project Insights, repository dashboard, saved views | Partial | Azure DevOps has stronger native dashboard widgets. |
| Build pipeline | GitHub Actions | Strong | Direct technical replacement for many CI scenarios. |
| Release pipeline | GitHub Actions + Environments | Strong technically | Not the same UX as Azure DevOps classic Release Pipelines. |
| Branch policy | Rulesets / branch protection | Strong | Use rulesets at repository or organization level. |
| Pull request | Pull request | Strong | Direct match. |
| Release artifact | GitHub Release + tag | Strong | Use releases and tags for versioned packages. |

---

## 8. Recommended GitHub Project Views

Create one organization-level Project per major product, platform, or release train.

### 8.1 Product Backlog

```text
Layout: Table
Filter: Status != Done
Group by: Priority or Product Area
Sort by: Priority, Target Date
```

Purpose:

- Manage unplanned or future work.
- Triage feature requests and requirements.
- Prioritize engineering intake.

### 8.2 Hierarchy View

```text
Layout: Table
Hierarchy: Enabled
Group by: Parent issue or Product Area
Visible fields: Type, Status, Priority, Sprint, Release, Owner, Target Date
```

Purpose:

- View Initiative -> Epic -> Feature -> Requirement -> Task decomposition.
- Inspect progress across nested work.
- Preserve context while navigating large programs.

### 8.3 Sprint Planning

```text
Layout: Table
Filter: Sprint = @next OR no:Sprint
Group by: Type
Visible fields: Effort, Priority, Owner, Product Area
```

Purpose:

- Select candidate work for the next sprint.
- Balance effort across team members.
- Ensure tasks are decomposed before sprint commitment.

### 8.4 Current Sprint Board

```text
Layout: Board
Filter: Sprint = @current
Columns: Status
```

Recommended status values:

```text
Backlog
Ready
In Progress
In Review
Blocked
Ready for Release
Done
```

Purpose:

- Daily execution view.
- Standup board.
- Blocker visibility.

### 8.5 Bug Triage

```text
Layout: Table
Filter: Type = Bug
Group by: Severity
Sort by: Priority, Created date
```

Purpose:

- Triage production and QA defects.
- Monitor regressions.
- Separate severity from business priority.

### 8.6 Release Roadmap

```text
Layout: Roadmap
Group by: Release
Date fields: Start Date, Target Date
Markers: Milestones, iterations, release dates
```

Purpose:

- Track planned release scope.
- Visualize delivery timeline.
- Identify risks to release readiness.

### 8.7 Executive Dashboard

```text
Layout: Insights
Charts:
  - Open work by Release
  - Work by Sprint
  - Bugs by Severity
  - Work by Product Area
  - Completed vs Remaining
  - Open vs Closed trend
```

Purpose:

- Provide leadership status.
- Track release health.
- Identify bottlenecks and scope growth.

---

## 9. Dashboards and Reporting

### 9.1 GitHub Reporting Capabilities

GitHub Enterprise reporting can be built from:

- Project Insights.
- Project table, board, and roadmap views.
- Milestone progress.
- Repository Dashboard.
- Issues Dashboard and saved views.
- Pull request dashboards.
- GitHub Actions workflow history.
- Deployment environment history.
- API or GraphQL extraction for custom dashboards.
- Third-party reporting tools where required.

Project Insights can create charts from project data and includes a default burn-up style progress chart.

### 9.2 Repository Dashboard

The Repository Dashboard became generally available in February 2026. It helps users find, filter, and save custom views of repositories.

Use it for:

- Finding repositories across a large enterprise.
- Saving repository views by product, team, or function.
- Helping engineering managers and non-engineers navigate to the right repository.
- Understanding repository landscape and ownership.

Do not treat it as the primary ALM release dashboard. Release and sprint health should be tracked through Projects, Insights, milestones, and custom dashboards.

### 9.3 Azure DevOps Reporting Advantage

Azure DevOps has stronger native reporting for formal Agile management, including:

- Velocity reports.
- Sprint burndown.
- Burnup widgets.
- Cumulative flow diagrams.
- Cycle time widgets.
- Lead time widgets.
- Analytics-backed dashboard widgets.

This is a significant difference for organizations with PMO reporting requirements.

---

## 10. Release Management Model

GitHub release management should be designed as a chain of related artifacts rather than one monolithic release object.

### 10.1 Repository-Scoped Release

Use this for a single application or service repository:

```text
Milestone: v2.4
  Issues
  Pull Requests
  Completion progress

Git tag: v2.4.0
GitHub Release: v2.4.0
  Release notes
  Binary assets
  Source snapshot
```

### 10.2 Cross-Repository Release

Use this when a release spans multiple repositories:

```text
Organization Project: Release Train 2026-Q2
  Release field: 2026.Q2
  Repositories: service-a, service-b, web-client, infrastructure
  Views: Release Roadmap, Bug Triage, Release Dashboard

Repository milestones:
  service-a: 2026.Q2
  service-b: 2026.Q2
  web-client: 2026.Q2
```

### 10.3 Deployment Governance

Use GitHub Actions and Environments:

```text
Build
  -> Unit Tests
    -> Integration Tests
      -> Package
        -> Deploy to Test
          -> Deploy to Staging
            -> Approval Gate
              -> Deploy to Production
```

Recommended production gate:

- Required reviewer from release management or engineering leadership.
- Required status checks.
- Restricted deployment branches.
- Environment-specific secrets.
- Deployment history retained.

---

## 11. Bug Tracking Model

### 11.1 Bug Issue Form

Every bug should be created from a structured issue form.

Required fields:

```text
Product / Component
Affected version
Environment
Severity
Priority
Steps to reproduce
Expected behavior
Actual behavior
Logs / screenshots / crash reports
Regression: yes/no
Workaround
Customer impact
Target release
```

Severity and priority should not be conflated:

| Field | Meaning |
|---|---|
| Severity | Technical or user impact of the defect. |
| Priority | Business urgency of fixing it. |

Example:

| Scenario | Severity | Priority |
|---|---|---|
| Production login outage | Critical | Urgent |
| Typo in admin page | Low | Low |
| Rare crash in important workflow | High | Medium or High |
| Cosmetic issue affecting major customer demo | Low | High |

### 11.2 Bug Workflow

Recommended bug workflow:

```text
New
  -> Triage
    -> Accepted
      -> In Progress
        -> In Review
          -> Fixed
            -> Verified
              -> Closed
```

For GitHub Projects, this can be represented with a `Status` field:

```text
Backlog
Ready
In Progress
In Review
Ready for QA
Verified
Done
Blocked
```

---

## 12. Branch, Pull Request, and Traceability Model

### 12.1 Branch Naming

Recommended branch naming convention:

```text
feature/1234-add-user-export
requirement/1450-support-multi-region-routing
bugfix/1250-fix-login-timeout
task/1301-refactor-order-service
release/2026.05
hotfix/2026.05.1
```

### 12.2 Pull Request Linking

Every pull request should link to one or more issues.

Recommended pull request footer:

```text
Closes #1234
Fixes #1250
Refs #1301
```

Traceability chain:

```text
Requirement
  -> Issue
    -> Branch
      -> Pull Request
        -> Review
          -> Merge
            -> Build
              -> Release
                -> Deployment
```

### 12.3 Pull Request Governance

Minimum PR governance:

- Pull request required before merge.
- Required approval count.
- Required code owner review for sensitive paths.
- Required CI status checks.
- No direct pushes to protected branches.
- Branch protection or rulesets applied consistently.

---

## 13. Step-by-Step GitHub Portal Learning Path for Non-Engineers

### Stage 1: Understand the GitHub Object Model

Learn these objects in order:

1. Enterprise account.
2. Organization.
3. Repository.
4. Issue.
5. Issue type.
6. Sub-issue.
7. Project.
8. Project field.
9. Iteration.
10. Milestone.
11. Pull request.
12. Release.
13. Action workflow.
14. Environment.

Key mental model:

```text
Azure DevOps Work Item ~= GitHub Issue
Azure DevOps Boards ~= GitHub Projects
Azure DevOps Iteration ~= GitHub Project Iteration field
Azure DevOps Release Tracking ~= GitHub Milestone + Release field + GitHub Release + Actions
```

### Stage 2: Create the Organization Project

```text
Organization
  -> Projects
    -> New project
      -> Start with Team Planning template or Table layout
```

Recommended name:

```text
Product ALM Board
```

### Stage 3: Define Issue Types

Create or standardize:

```text
Initiative
Epic
Feature
Requirement
Story
Task
Bug
Risk
Change Request
```

### Stage 4: Create Issue Forms

Create issue forms for:

```text
Feature Request
Requirement
Bug Report
Task
Change Request
```

Forms should enforce structured data entry and reduce the need for project managers to understand GitHub implementation details.

### Stage 5: Configure Project Fields

Create fields:

```text
Status
Priority
Severity
Effort
Sprint
Release
Product Area
Owner
Start Date
Target Date
Risk Level
Customer Impact
```

### Stage 6: Define Sprints

Create an iteration field:

```text
Field name: Sprint
Duration: 2 weeks
Naming: Sprint YYYY.NN
```

Use saved filters:

```text
Sprint = @current
Sprint = @next
no:Sprint
```

### Stage 7: Create Backlog and Sprint Views

Create:

```text
Product Backlog
Hierarchy View
Sprint Planning
Current Sprint Board
Bug Triage
Release Roadmap
Executive Dashboard
```

### Stage 8: Create Milestones

For each repository release:

```text
Repository
  -> Issues
    -> Milestones
      -> New milestone
```

Example:

```text
v1.0
v1.1
2026-Q2 Release
```

### Stage 9: Manage Releases

When release scope is complete:

```text
Repository
  -> Releases
    -> Draft a new release
      -> Select tag
      -> Add release notes
      -> Attach assets if needed
      -> Publish release
```

### Stage 10: Review Dashboards

Weekly review views:

- Release Roadmap.
- Executive Dashboard.
- Bug Triage.
- Current Sprint Board.
- Hierarchy View.

Monthly review views:

- Work by Release.
- Work by Product Area.
- Open vs Closed trends.
- Bugs by Severity.
- Sprint completion history.

---

## 14. Governance Recommendations

### 14.1 Field Governance

Establish field ownership. Do not allow uncontrolled custom field growth.

Recommended owners:

| Field | Owner |
|---|---|
| Issue Type | Engineering process owner |
| Priority | Product management |
| Severity | Engineering / QA |
| Sprint | Scrum master / engineering manager |
| Release | Release manager |
| Product Area | Engineering manager or architecture lead |
| Risk Level | Engineering manager / release manager |

### 14.2 Naming Conventions

Recommended naming standards:

```text
Issue title: [Area] Short description
Branch: feature/<issue-number>-short-description
Milestone: vMajor.Minor or YYYY-QN Release
Release: vMajor.Minor.Patch
Sprint: Sprint YYYY.NN
```

### 14.3 Required Issue Hygiene

Before sprint commitment, every planned work item should have:

- Issue type.
- Owner.
- Priority.
- Sprint.
- Acceptance criteria.
- Estimate, if the team estimates work.
- Parent issue, where applicable.
- Product area.
- Target release, where applicable.

Before release readiness, every release item should have:

- Linked pull request or documented non-code disposition.
- Completed review.
- Completed validation.
- Release note status.
- Deployment readiness status.

---

## 15. Gaps, Risks, and Mitigations

| Gap / Risk | Impact | Mitigation |
|---|---|---|
| No exact Azure DevOps Area Path equivalent | Product/team ownership can become inconsistent. | Use controlled `Product Area` field, labels, repositories, and team ownership. |
| No exact Azure DevOps Iteration Path hierarchy | Release/sprint nesting is less formal. | Use `Release` field plus `Sprint` iteration field. |
| Less process-prescriptive than Azure DevOps | Teams may configure Projects inconsistently. | Define organization-level ALM standards and templates. |
| Reporting weaker than Azure DevOps Boards | PMO dashboards may require additional work. | Use Project Insights first, then API/BI or third-party reporting if required. |
| Release model is distributed | Release status can be fragmented across milestones, releases, tags, and Projects. | Define a single release tracking pattern and enforce it. |
| Cross-repository milestones are awkward | Multi-repo release trains may be hard to track with milestones alone. | Use organization Projects and a `Release` project field. |
| Too many labels and fields | Data quality degrades. | Limit fields and labels. Assign ownership. Review quarterly. |
| Non-engineer learning curve | PMs may struggle with GitHub concepts. | Provide role-specific training and simplified Project views. |
| Lack of built-in capacity planning | Sprint commitments may be harder to manage. | Use Effort/Story Points field and team-specific views. |
| Governance drift | Projects diverge over time. | Use templates, rulesets, automation, and quarterly ALM audits. |

---

## 16. Pilot Evaluation Plan

Run a time-boxed pilot before committing to GitHub Enterprise as the ALM system of record.

### 16.1 Pilot Scope

Select:

- One product or application.
- Two to four repositories.
- One engineering team.
- One product owner or project manager.
- Two sprints.
- One release candidate.

### 16.2 Pilot Scenarios

The pilot should test five scenarios.

#### Scenario 1: Feature and Requirement Decomposition

Acceptance test:

```text
A project manager can create a feature, decompose it into requirements and tasks, and view the full hierarchy in GitHub Projects.
```

Pass criteria:

- Feature issue exists.
- Requirement sub-issues exist.
- Task sub-issues exist.
- Hierarchy View shows the full structure.
- Status and ownership are visible.

#### Scenario 2: Sprint Planning

Acceptance test:

```text
The team can plan a two-week sprint, assign work to the current iteration, and track execution on a board.
```

Pass criteria:

- Sprint iteration exists.
- Work is assigned to sprint.
- Sprint board shows status flow.
- Blocked work is visible.
- Unfinished work can be moved forward.

#### Scenario 3: Bug Intake and Triage

Acceptance test:

```text
A tester or non-engineer can log a bug with enough structured information for engineering triage.
```

Pass criteria:

- Bug form captures required fields.
- Severity and priority are assigned.
- Logs or screenshots can be attached.
- Bug appears in triage view.
- Bug can be associated with release or sprint.

#### Scenario 4: Release Tracking

Acceptance test:

```text
The team can track release scope from issues through pull requests, milestone completion, release notes, and published release.
```

Pass criteria:

- Release field or milestone is assigned.
- Linked PRs are visible.
- Release progress is visible.
- Git tag is created.
- GitHub Release is published.
- Deployment workflow is visible.

#### Scenario 5: Leadership Dashboard

Acceptance test:

```text
Engineering leadership can see release health, sprint progress, bug severity, and remaining work without manual spreadsheet reconciliation.
```

Pass criteria:

- Executive Dashboard view exists.
- Charts show work by release and sprint.
- Bugs by severity are visible.
- Roadmap view shows timeline.
- Data quality is sufficient for decision-making.

### 16.3 Pilot Decision Criteria

| Criterion | Pass / Fail Question |
|---|---|
| Work hierarchy | Can GitHub model the required ALM hierarchy clearly? |
| Sprint execution | Can the team plan and execute sprints without workarounds? |
| Bug workflow | Can bugs be logged, triaged, fixed, and verified cleanly? |
| Release tracking | Can release scope and readiness be tracked end to end? |
| Reporting | Are native dashboards sufficient, or is external reporting required? |
| Non-engineer usability | Can PMs and stakeholders use the portal without developer assistance? |
| Governance | Can standards be applied consistently across repositories and teams? |

---

## 17. Recommended Implementation Roadmap

### Phase 1: Foundation

- Define GitHub organization and repository structure.
- Define issue types.
- Define project fields.
- Define labels and naming standards.
- Create issue forms.
- Create organization-level Project template.

### Phase 2: Agile Planning

- Create iteration field for sprints.
- Create backlog, sprint planning, and current sprint views.
- Create Hierarchy View.
- Create bug triage view.
- Train engineering managers, project managers, and product owners.

### Phase 3: Release Governance

- Define milestone naming.
- Define release field usage.
- Define branch naming and PR linking rules.
- Configure GitHub Releases.
- Configure GitHub Actions and Environments.
- Configure production deployment approvals.

### Phase 4: Reporting

- Create Project Insights charts.
- Create release dashboard views.
- Create repository dashboard saved views.
- Identify gaps requiring API, BI, or third-party reporting.

### Phase 5: Enterprise Controls

- Apply rulesets and branch protections.
- Configure nested teams and repository access.
- Apply code owner review rules.
- Define audit and compliance monitoring.
- Review field/label usage quarterly.

---

## 18. Final Assessment

GitHub Enterprise can satisfy the requested ALM requirements if the organization adopts a deliberate GitHub-native configuration:

```text
Issue Types
  + Issue Forms
  + Sub-Issues
  + Hierarchy View
  + GitHub Projects
  + Iteration Fields
  + Milestones
  + Releases
  + Actions
  + Environments
  + Rulesets
```

This combination supports feature requests, requirements, tasks, bugs, sprint planning, multiple iterations, release tracking, dashboards, and engineering traceability.

The main evaluation risk is not whether GitHub Enterprise can track the work. It can. The main risk is whether GitHub's flexible model provides enough process structure, reporting depth, and non-engineer usability for the organization's ALM expectations.

Use GitHub Enterprise if the organization values engineering-native workflow integration and is willing to standardize configuration. Prefer Azure DevOps Boards if the organization requires formal ALM process templates, native Area/Iteration path governance, and mature out-of-the-box PMO reporting.

---

## 19. Source References

### GitHub Sources

1. GitHub Changelog, **Hierarchy view in GitHub Projects is now generally available**, March 19, 2026.  
   https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available/

2. GitHub Docs, **Adding sub-issues**.  
   https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/adding-sub-issues

3. GitHub Docs, **Managing issue types in an organization**.  
   https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/managing-issue-types-in-an-organization

4. GitHub Docs, **Managing issue fields in your organization**.  
   https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/managing-issue-fields-in-your-organization

5. GitHub Docs, **About iteration fields**.  
   https://docs.github.com/en/issues/planning-and-tracking-with-projects/understanding-fields/about-iteration-fields

6. GitHub Docs, **Best practices for Projects**.  
   https://docs.github.com/enterprise-cloud@latest/issues/planning-and-tracking-with-projects/learning-about-projects/best-practices-for-projects

7. GitHub Docs, **About insights for Projects**.  
   https://docs.github.com/en/issues/planning-and-tracking-with-projects/viewing-insights-from-your-project/about-insights-for-projects

8. GitHub Changelog, **Hierarchy view improvements and file uploads in issue forms**, March 5, 2026.  
   https://github.blog/changelog/2026-03-05-hierarchy-view-improvements-and-file-uploads-in-issue-forms/

9. GitHub Changelog, **Repository Dashboard is now generally available**, February 24, 2026.  
   https://github.blog/changelog/2026-02-24-repository-dashboard-is-now-generally-available/

10. GitHub Docs, **About milestones**.  
    https://docs.github.com/issues/using-labels-and-milestones-to-track-work/about-milestones

11. GitHub Docs, **Managing releases in a repository**.  
    https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository

12. GitHub Docs, **About rulesets**.  
    https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets

13. GitHub Docs, **Managing a branch protection rule**.  
    https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/managing-a-branch-protection-rule

### Azure DevOps Sources

14. Microsoft Learn, **About dashboards, charts, reports, and widgets**.  
    https://learn.microsoft.com/en-us/azure/devops/report/dashboards/overview?view=azure-devops

15. Microsoft Learn, **How are area and iteration paths used?**  
    https://learn.microsoft.com/en-us/azure/devops/organizations/settings/about-areas-iterations?view=azure-devops

16. Microsoft Learn, **Define iteration paths and configure team iterations**.  
    https://learn.microsoft.com/en-us/azure/devops/organizations/settings/set-iteration-paths-sprints?view=azure-devops

17. Microsoft Learn, **Analytics widgets overview for Azure DevOps**.  
    https://learn.microsoft.com/en-us/azure/devops/report/dashboards/analytics-widgets?view=azure-devops

18. Microsoft Learn, **View and configure team velocity**.  
    https://learn.microsoft.com/en-us/azure/devops/report/dashboards/team-velocity?view=azure-devops

19. Microsoft Learn, **Configure a burndown or burnup widget**.  
    https://learn.microsoft.com/en-us/azure/devops/report/dashboards/configure-burndown-burnup-widgets?view=azure-devops
