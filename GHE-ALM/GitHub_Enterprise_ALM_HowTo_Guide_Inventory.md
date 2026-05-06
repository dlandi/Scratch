# GitHub Enterprise ALM Workflow: Non-Technical Manager How-To Guide Inventory

**Audience:** Engineering Management  
**Date:** 2026-05-06  
**Purpose:** Identify every GitHub Enterprise ALM workflow activity that should have a role-specific "How To" guide for a non-technical manager, project manager, product owner, release manager, QA manager, or engineering manager.

---

## 1. Scope

This document identifies the training and operational guides needed to support a non-technical manager using GitHub Enterprise as an ALM platform.

The source workflow assumes GitHub Enterprise is being used for:

- Feature request tracking.
- Requirements tracking.
- Epic, feature, requirement, task, and bug hierarchy.
- Scrum and sprint planning.
- Bug intake and triage.
- Release planning and release tracking.
- Dashboards and reporting.
- Branch, pull request, release, and deployment traceability.
- Governance through project standards, teams, rulesets, and deployment controls.

This is not a set of full step-by-step manuals. It is a complete guide inventory: each row identifies a discrete activity that needs a future "How To" guide.

---

## 2. Guide Classification

Each required guide is classified by who needs it and how directly the manager performs the activity.

| Classification | Meaning |
|---|---|
| Manager Performs | A non-technical manager can perform the activity directly in the GitHub portal after training. |
| Manager Reviews | A non-technical manager needs to inspect, validate, or approve the result but usually does not configure it. |
| Manager Requests | A non-technical manager needs to know what to request from GitHub administrators, repository administrators, DevOps engineers, or engineering leads. |
| Manager Understands | A non-technical manager needs conceptual fluency to interpret status, ask the right questions, and avoid process errors. |

---

## 3. Minimum Recommended How-To Guide Set

These are the core guides required before onboarding non-technical managers to the GitHub Enterprise ALM workflow.

| Priority | Guide ID | How-To Guide Title | Classification |
|---:|---|---|---|
| 1 | GHE-ALM-001 | How to Navigate the GitHub Enterprise ALM Object Model | Manager Understands |
| 2 | GHE-ALM-002 | How to Find the Correct Organization, Repository, and Project | Manager Performs |
| 3 | GHE-ALM-006 | How to Create and Use an Organization-Level GitHub Project | Manager Performs |
| 4 | GHE-ALM-011 | How to Create a Feature Request Issue | Manager Performs |
| 5 | GHE-ALM-012 | How to Create a Requirement Issue | Manager Performs |
| 6 | GHE-ALM-014 | How to Create and Triage a Bug Report | Manager Performs |
| 7 | GHE-ALM-017 | How to Break Work into Sub-Issues | Manager Performs |
| 8 | GHE-ALM-018 | How to Use Hierarchy View to Review Epic-to-Task Breakdown | Manager Performs |
| 9 | GHE-ALM-026 | How to Use the Product Backlog View | Manager Performs |
| 10 | GHE-ALM-028 | How to Plan the Next Sprint | Manager Performs |
| 11 | GHE-ALM-029 | How to Use the Current Sprint Board | Manager Performs |
| 12 | GHE-ALM-030 | How to Move Unfinished Work to a Later Sprint | Manager Performs |
| 13 | GHE-ALM-034 | How to Use the Bug Triage View | Manager Performs |
| 14 | GHE-ALM-041 | How to Track a Release with Milestones and Release Fields | Manager Performs |
| 15 | GHE-ALM-045 | How to Read Release Health from the Roadmap and Dashboard | Manager Performs |
| 16 | GHE-ALM-051 | How to Create and Interpret Project Insights Charts | Manager Performs |
| 17 | GHE-ALM-060 | How to Verify Issue-to-Pull-Request Traceability | Manager Reviews |
| 18 | GHE-ALM-079 | How to Run the GitHub Enterprise ALM Pilot Evaluation | Manager Performs |

---

## 4. Complete How-To Guide Inventory

### 4.1 Orientation and Navigation

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-001 | How to Navigate the GitHub Enterprise ALM Object Model | Learn how Enterprise Accounts, Organizations, Repositories, Issues, Projects, Milestones, Releases, Actions, and Environments fit together. | Manager Understands | Enterprise account, Organization, Repository, Issue, Project | Manager can explain where work is planned, tracked, reviewed, released, and deployed. |
| GHE-ALM-002 | How to Find the Correct Organization, Repository, and Project | Locate the correct GitHub organization, repository, and ALM Project for a product or release. | Manager Performs | Organization navigation, repository list, Project list, Repository Dashboard | Manager lands in the correct planning surface without developer assistance. |
| GHE-ALM-003 | How to Use the Repository Dashboard | Use the Repository Dashboard to find repositories, identify repositories with admin access, and save useful repository views. | Manager Performs | Repository Dashboard | Manager can locate relevant repositories across the enterprise. |
| GHE-ALM-004 | How to Distinguish Work Hierarchy from Repository Structure | Understand the difference between issue hierarchy, nested teams, enterprise hierarchy, and repository file trees. | Manager Understands | Sub-issues, nested teams, enterprise/org/repo model | Manager avoids treating GitHub repositories as folder hierarchies. |
| GHE-ALM-005 | How to Interpret GitHub Project Views | Understand table, board, roadmap, hierarchy, and insights views. | Manager Understands | GitHub Projects views | Manager can select the correct view for backlog, sprint, release, or dashboard work. |

---

### 4.2 Project Setup and Project Structure

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-006 | How to Create and Use an Organization-Level GitHub Project | Create a GitHub Project at organization level and choose table, board, roadmap, or template-based setup. | Manager Performs / Manager Requests | Organization Projects | Project shell exists for ALM tracking. |
| GHE-ALM-007 | How to Name and Describe a GitHub ALM Project | Apply naming standards and project description conventions. | Manager Performs | Project name, project README/description | Project purpose and scope are clear to stakeholders. |
| GHE-ALM-008 | How to Add Existing Issues and Pull Requests to a Project | Add items manually, by URL, by search, from repository issue lists, or in bulk. | Manager Performs | Project item add, repository search | Project contains the correct initial work items. |
| GHE-ALM-009 | How to Configure Auto-Add Workflows for Project Intake | Configure auto-add workflows so qualifying issues and PRs are added automatically to the Project. | Manager Requests / Manager Reviews | Project workflows, auto-add | New qualifying issues and PRs flow into the ALM Project automatically. |
| GHE-ALM-010 | How to Archive Completed or Old Project Items | Configure or review auto-archive behavior for stale, closed, or completed work. | Manager Requests / Manager Reviews | Project workflows, auto-archive | Project views remain usable without deleting historical records. |

---

### 4.3 Issue Types, Issue Fields, and Work Item Standards

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-011 | How to Create a Feature Request Issue | Create a feature request and assign issue type, priority, owner, product area, release, and acceptance criteria. | Manager Performs | Issues, issue type Feature, issue fields | Feature request is ready for triage and prioritization. |
| GHE-ALM-012 | How to Create a Requirement Issue | Create a formal requirement with structured fields and acceptance criteria. | Manager Performs | Issues, custom issue type Requirement, issue forms | Requirement is visible, searchable, and traceable. |
| GHE-ALM-013 | How to Create an Epic or Initiative Issue | Create a parent issue that represents a major capability, release theme, initiative, or program-level deliverable. | Manager Performs | Issues, custom issue types, sub-issues | Parent work item exists for work breakdown. |
| GHE-ALM-014 | How to Create and Triage a Bug Report | File a bug using the bug form, assign severity and priority, and route it into triage. | Manager Performs | Issue type Bug, issue forms, fields, labels | Bug report has enough structured information for engineering action. |
| GHE-ALM-015 | How to Create a Task Issue | Create task issues for implementation or non-code work and associate them with a parent feature or requirement. | Manager Performs | Issue type Task, sub-issues | Trackable task is created and placed in the right hierarchy. |
| GHE-ALM-016 | How to Create a Risk or Change Request Issue | Create and classify a release risk, delivery risk, or controlled scope change. | Manager Performs | Custom issue types Risk / Change Request | Risk or change request enters governance workflow. |
| GHE-ALM-017 | How to Break Work into Sub-Issues | Add sub-issues under an initiative, epic, feature, or requirement. | Manager Performs | Sub-issues, parent issue | Work breakdown is visible and traceable. |
| GHE-ALM-018 | How to Use Hierarchy View to Review Epic-to-Task Breakdown | Enable or use Hierarchy View to inspect nested issue relationships. | Manager Performs | GitHub Projects Hierarchy View | Manager can review full work decomposition in a project table. |
| GHE-ALM-019 | How to Use Issue Dependencies for Blocked Work | Mark issues as blocked by or blocking other work. | Manager Performs / Manager Reviews | Issue dependencies | Blocked work is explicit and visible. |
| GHE-ALM-020 | How to Apply Issue Metadata Correctly | Set issue type, assignee, labels, milestone, project fields, sprint, release, product area, and priority. | Manager Performs | Issue metadata, project fields | Work item is clean enough for planning and reporting. |
| GHE-ALM-021 | How to Use Labels Without Replacing Issue Types | Apply labels for secondary classification without undermining issue type governance. | Manager Performs / Manager Reviews | Labels, issue types | Labels remain useful and do not become an uncontrolled process substitute. |
| GHE-ALM-022 | How to Manage Issue Hygiene Before Sprint Commitment | Verify that planned work has owner, priority, acceptance criteria, sprint, estimate, parent, and target release where required. | Manager Performs | Issues, project fields | Sprint candidate work is ready for commitment. |
| GHE-ALM-023 | How to Define or Request Organization Issue Types | Request or configure standard issue types such as Epic, Requirement, Risk, and Change Request. | Manager Requests / Manager Reviews | Organization issue types | Organization-level issue type taxonomy supports ALM. |
| GHE-ALM-024 | How to Define or Request Organization Issue Fields | Request or configure standard fields such as Priority, Severity, Effort, Release, Product Area, Start Date, and Target Date. | Manager Requests / Manager Reviews | Organization issue fields | Structured metadata is available across repositories. |
| GHE-ALM-025 | How to Create or Request Issue Forms | Create or request Feature Request, Requirement, Bug Report, Task, and Change Request forms. | Manager Requests / Manager Reviews | Issue forms, issue templates | Non-technical users can submit structured issues consistently. |

---

### 4.4 Backlog, Sprint, and Agile Execution

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-026 | How to Use the Product Backlog View | Review unplanned, future, or unscheduled work by status, priority, product area, or target date. | Manager Performs | Project table view, filters, grouping, sorting | Backlog is visible and manageable. |
| GHE-ALM-027 | How to Configure or Request a Sprint Iteration Field | Create or request a recurring Sprint iteration field with length, naming convention, and breaks. | Manager Requests / Manager Reviews | Project iteration field | Project can support sprint-based planning. |
| GHE-ALM-028 | How to Plan the Next Sprint | Select work for the next sprint using `Sprint = @next`, `no:Sprint`, priority, effort, and readiness. | Manager Performs | Iteration field, sprint planning view | Sprint candidate list is prepared. |
| GHE-ALM-029 | How to Use the Current Sprint Board | Use board view grouped by Status to run standups and track execution. | Manager Performs | Project board view, Status field | Sprint status is visible day to day. |
| GHE-ALM-030 | How to Move Unfinished Work to a Later Sprint | Move incomplete work from the current sprint to a future iteration. | Manager Performs | Iteration field, grouping, bulk edits | Unfinished work is rolled forward cleanly. |
| GHE-ALM-031 | How to Monitor Blocked Sprint Work | Identify blocked items, stale items, dependency chains, and missing owners. | Manager Performs | Status field, dependencies, filters | Sprint risks are visible early. |
| GHE-ALM-032 | How to Close a Sprint Review | Review completed, not completed, blocked, and moved-forward work at sprint end. | Manager Performs | Current sprint board, insights, filters | Sprint outcome is recorded and understood. |
| GHE-ALM-033 | How to Use Effort or Story Points in GitHub Projects | Enter and interpret estimates for sprint planning and team capacity discussion. | Manager Performs / Manager Reviews | Number fields, single-select fields, project charts | Sprint scope can be discussed quantitatively. |

---

### 4.5 Bug Intake, Triage, and Defect Workflow

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-034 | How to Use the Bug Triage View | Review open bugs by severity, priority, age, release, product area, and owner. | Manager Performs | Project table view, filters, grouping | Bug backlog is triaged systematically. |
| GHE-ALM-035 | How to Distinguish Severity from Priority | Apply technical/user impact separately from business urgency. | Manager Understands / Manager Performs | Severity field, Priority field | Bug triage decisions are consistent. |
| GHE-ALM-036 | How to Move a Bug Through the Defect Workflow | Move a bug from New/Triage through Ready, In Progress, Ready for QA, Verified, and Done. | Manager Performs / Manager Reviews | Status field, project board/table | Bug status reflects the real defect lifecycle. |
| GHE-ALM-037 | How to Attach Evidence to a Bug | Attach logs, screenshots, crash reports, and reproduction information. | Manager Performs | Issue forms, file uploads, issue comments | Engineers receive enough evidence for investigation. |
| GHE-ALM-038 | How to Associate a Bug with a Release or Sprint | Assign target release, affected release, sprint, milestone, and product area. | Manager Performs | Milestones, Release field, Sprint field | Defect impact is visible in sprint and release tracking. |
| GHE-ALM-039 | How to Run a Weekly Bug Review | Review new, critical, stale, deferred, fixed, and verified defects. | Manager Performs | Bug triage view, charts, filters | Bug backlog remains controlled. |
| GHE-ALM-040 | How to Handle a Hotfix Bug | Identify a production defect, assign urgency, link to hotfix milestone or release field, and verify release traceability. | Manager Performs / Manager Reviews | Bug issues, milestones, Release field, PR links | Emergency fix is tracked without bypassing ALM visibility. |

---

### 4.6 Release Planning, Milestones, Roadmaps, and GitHub Releases

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-041 | How to Track a Release with Milestones and Release Fields | Use repository milestones for repo-scoped releases and Project Release fields for cross-repository release trains. | Manager Performs | Milestones, Release project field | Release scope is visible and controlled. |
| GHE-ALM-042 | How to Create and Manage a Milestone | Create a milestone, set due date, add issues/PRs, and monitor completion. | Manager Performs | Repository milestones | Milestone represents a versioned release target. |
| GHE-ALM-043 | How to Add Issues and Pull Requests to a Milestone | Assign release-scope work items to the correct repository milestone. | Manager Performs / Manager Reviews | Milestones, issues, PRs | Release scope is linked to tracked work. |
| GHE-ALM-044 | How to Use the Release Roadmap View | Use a roadmap layout with start dates, target dates, iterations, milestones, and release grouping. | Manager Performs | Project roadmap layout | Release timeline and scope are visible. |
| GHE-ALM-045 | How to Read Release Health from the Roadmap and Dashboard | Interpret release scope, blocked work, defect severity, remaining work, and milestone progress. | Manager Performs | Roadmap view, insights, milestones | Engineering manager can assess release readiness. |
| GHE-ALM-046 | How to Prepare a Release Readiness Review | Confirm completed issues, linked PRs, validation status, release notes, defects, and deployment readiness. | Manager Performs | Projects, milestones, PR links, releases | Release decision meeting has a structured checklist. |
| GHE-ALM-047 | How to Draft or Review a GitHub Release | Draft a release, select or create a tag, add release title, release notes, assets, and pre-release/latest designation. | Manager Performs / Manager Reviews | GitHub Releases, tags | Versioned release package is ready or reviewed. |
| GHE-ALM-048 | How to Use Automatically Generated Release Notes | Generate or review release notes from merged pull requests and issue links. | Manager Performs / Manager Reviews | GitHub Releases, generated release notes | Release notes are traceable to completed work. |
| GHE-ALM-049 | How to Track a Cross-Repository Release | Use an organization Project, Release field, repo milestones, and release roadmap across multiple repositories. | Manager Performs | Organization Project, Release field, milestones | Multi-repository release train is coordinated. |
| GHE-ALM-050 | How to Close a Release After Deployment | Confirm release published, deployment completed, milestone closed, dashboard updated, and deferred work moved. | Manager Performs / Manager Reviews | Releases, milestones, deployments, project fields | Release record is complete and auditable. |

---

### 4.7 Dashboards, Insights, Metrics, and Reporting

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-051 | How to Create and Interpret Project Insights Charts | Create charts for work by release, sprint, product area, owner, severity, or status. | Manager Performs | Project Insights | Manager can create and read project charts. |
| GHE-ALM-052 | How to Configure Chart Filters and Axes | Configure chart layout, filters, X-axis, grouping, and numeric aggregation. | Manager Performs | Project Insights configuration | Chart reflects the intended management question. |
| GHE-ALM-053 | How to Use Historical Charts and Burn-Up Views | Use historical charts to review open/completed/not-planned trends and burn-up progress. | Manager Performs | Project Insights historical charts | Manager can spot trend, bottleneck, or scope-growth signals. |
| GHE-ALM-054 | How to Run a Weekly ALM Dashboard Review | Review release roadmap, sprint board, bug triage, executive dashboard, and hierarchy view. | Manager Performs | Project views and insights | Weekly management review has a repeatable agenda. |
| GHE-ALM-055 | How to Run a Monthly ALM Metrics Review | Review work by release, product area, open/closed trends, bugs by severity, and sprint completion history. | Manager Performs | Insights, saved views, dashboards | Monthly governance and trend review is standardized. |
| GHE-ALM-056 | How to Identify Reporting Gaps That Require BI or External Tools | Determine when Project Insights are insufficient and external reporting is needed. | Manager Reviews / Manager Requests | Project data, export, API/GraphQL, BI tools | Reporting gaps are explicit rather than hidden. |
| GHE-ALM-057 | How to Export or Request Exported Project Data | Export project data or request API/BI extraction for leadership reports. | Manager Performs / Manager Requests | Project export, API/GraphQL | Data can be reused for external reporting. |
| GHE-ALM-058 | How to Use Saved Views for Stakeholder Reporting | Create or use simplified views for leadership, QA, release management, or product owners. | Manager Performs | Project saved views, filters | Stakeholders see the right slice of data. |

---

### 4.8 Branches, Pull Requests, Code Review, and Traceability

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-059 | How to Understand Branch Naming Conventions | Read branch names such as `feature/1234-add-user-export` and connect them to work items. | Manager Understands / Manager Reviews | Branches, naming conventions | Manager can trace branch names back to issues. |
| GHE-ALM-060 | How to Verify Issue-to-Pull-Request Traceability | Confirm that pull requests link to issues using references or closing keywords. | Manager Reviews | Issues, Pull Requests, linked PRs | Work item has visible implementation traceability. |
| GHE-ALM-061 | How to Interpret Pull Request Status for Managers | Read PR state, reviews, checks, mergeability, linked issues, and deployment signals without reviewing code. | Manager Reviews | Pull Requests, checks, reviews | Manager understands whether work is waiting, blocked, approved, or merged. |
| GHE-ALM-062 | How to Verify Review and Approval Compliance | Confirm that required approvals, code-owner reviews, and required checks were satisfied. | Manager Reviews | Branch protection, rulesets, PR reviews, status checks | Governance compliance is visible before release. |
| GHE-ALM-063 | How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves` | Understand how pull requests automatically close linked issues when merged. | Manager Understands / Manager Reviews | PR body, commit messages, issue links | Manager can validate automatic issue closure behavior. |
| GHE-ALM-064 | How to Use Issue and PR Timeline Events for Audit Trail | Read timeline events showing project changes, status changes, PR links, and workflow automation. | Manager Reviews | Issue timeline, PR timeline, Project timeline events | Manager can audit how an item moved through the workflow. |

---

### 4.9 Deployment Governance, Environments, and Release Approval

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-065 | How to Understand GitHub Actions and Environments at a Manager Level | Learn what workflows, environments, jobs, approvals, and deployment history mean. | Manager Understands | GitHub Actions, Environments | Manager can interpret deployment status without writing YAML. |
| GHE-ALM-066 | How to Review Deployment History | Inspect current and previous deployments for an environment. | Manager Reviews | Deployments, Environments | Manager can confirm whether a release reached staging, UAT, or production. |
| GHE-ALM-067 | How to Approve a Protected Deployment | Approve a deployment job when required reviewer rules apply. | Manager Performs | Environment required reviewers | Manager can act as release approver when designated. |
| GHE-ALM-068 | How to Request Environment Protection Rules | Request required reviewers, wait timers, deployment branch/tag restrictions, and environment secrets policy. | Manager Requests / Manager Reviews | Environments, protection rules | Production deployment governance is documented and applied. |
| GHE-ALM-069 | How to Interpret Deployment Branch and Tag Restrictions | Understand which branches or tags are allowed to deploy to protected environments. | Manager Understands / Manager Reviews | Deployment branch/tag rules | Manager can verify release governance assumptions. |

---

### 4.10 Governance, Teams, Permissions, and Policy Controls

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-070 | How to Request GitHub Organization and Repository Structure | Specify the organization, repository, and Project structure needed for ALM. | Manager Requests | Enterprise account, organizations, repositories, Projects | Administrators can implement a coherent structure. |
| GHE-ALM-071 | How to Request or Review Nested Teams | Define team hierarchy for ownership, review routing, and access control. | Manager Requests / Manager Reviews | Nested teams, team permissions | Team structure aligns with product ownership. |
| GHE-ALM-072 | How to Request Repository Access for Project Managers and Stakeholders | Request appropriate read, triage, write, maintain, or admin access. | Manager Requests | Repository permissions, organization teams | Users have enough access without overprivileging. |
| GHE-ALM-073 | How to Request Rulesets or Branch Protection | Request required PRs, reviews, status checks, code-owner reviews, signed commits, force-push blocking, and tag protection. | Manager Requests / Manager Reviews | Rulesets, branch protection | Repository governance is enforced consistently. |
| GHE-ALM-074 | How to Review Ruleset and Branch Protection Coverage | Verify that `main`, `release/*`, and `hotfix/*` branches are governed. | Manager Reviews | Rulesets, protected branches | Critical branches are protected. |
| GHE-ALM-075 | How to Request or Review CODEOWNERS-Based Review Routing | Understand code-owner review behavior and request ownership files for sensitive paths. | Manager Requests / Manager Reviews | CODEOWNERS, PR review requests | Sensitive code paths require appropriate reviewers. |
| GHE-ALM-076 | How to Govern Project Fields and Labels | Establish field ownership, label discipline, quarterly cleanup, and change control. | Manager Performs / Manager Reviews | Project fields, labels | Metadata remains reliable for reporting. |
| GHE-ALM-077 | How to Enforce Naming Conventions | Apply naming conventions for issues, branches, milestones, releases, and sprints. | Manager Performs / Manager Reviews | Issues, branches, milestones, releases, iterations | Work items and release artifacts are searchable and consistent. |
| GHE-ALM-078 | How to Run a Quarterly ALM Hygiene Audit | Review field usage, label sprawl, stale items, project drift, permissions, and view consistency. | Manager Performs / Manager Reviews | Projects, fields, labels, teams, rulesets | Governance drift is identified and corrected. |

---

### 4.11 Pilot Evaluation Activities

| Guide ID | How-To Guide Title | Activity Covered | Classification | Primary GitHub Features | Output of the Exercise |
|---|---|---|---|---|---|
| GHE-ALM-079 | How to Run the GitHub Enterprise ALM Pilot Evaluation | Execute a two-sprint pilot with one product, two to four repositories, one team, and one release candidate. | Manager Performs | Projects, issues, iterations, milestones, insights | Pilot produces evidence for adoption decision. |
| GHE-ALM-080 | How to Test Feature and Requirement Decomposition | Create a feature, decompose it into requirements and tasks, and verify hierarchy visibility. | Manager Performs | Issues, sub-issues, Hierarchy View | Pilot validates work breakdown capability. |
| GHE-ALM-081 | How to Test Sprint Planning and Execution | Plan and run a two-week sprint in GitHub Projects. | Manager Performs | Iteration fields, board view | Pilot validates Agile planning fit. |
| GHE-ALM-082 | How to Test Bug Intake and Triage | File, triage, route, and track bugs through the defect workflow. | Manager Performs | Bug forms, severity, priority, triage view | Pilot validates defect workflow. |
| GHE-ALM-083 | How to Test Release Tracking | Track release scope from issues through pull requests, milestones, release notes, GitHub Release, and deployment. | Manager Performs / Manager Reviews | Milestones, releases, PR links, deployment history | Pilot validates release governance. |
| GHE-ALM-084 | How to Test Leadership Dashboard Sufficiency | Determine whether native GitHub views and insights are sufficient for leadership reporting. | Manager Performs / Manager Reviews | Insights, roadmap, dashboard views | Pilot identifies dashboard gaps. |
| GHE-ALM-085 | How to Record Pilot Pass/Fail Evidence | Capture pass/fail results against hierarchy, sprint execution, bug workflow, release tracking, reporting, usability, and governance criteria. | Manager Performs | Pilot scorecard, Project views, screenshots, metrics | Adoption decision is evidence-based. |

---

## 5. Recommended Training Sequence

The guides should not be written or delivered in alphabetical order. They should be delivered in the order a manager would experience the workflow.

### Wave 1: Orientation and Navigation

1. GHE-ALM-001 — How to Navigate the GitHub Enterprise ALM Object Model.
2. GHE-ALM-002 — How to Find the Correct Organization, Repository, and Project.
3. GHE-ALM-003 — How to Use the Repository Dashboard.
4. GHE-ALM-005 — How to Interpret GitHub Project Views.

### Wave 2: Work Intake and Work Breakdown

1. GHE-ALM-011 — How to Create a Feature Request Issue.
2. GHE-ALM-012 — How to Create a Requirement Issue.
3. GHE-ALM-013 — How to Create an Epic or Initiative Issue.
4. GHE-ALM-017 — How to Break Work into Sub-Issues.
5. GHE-ALM-018 — How to Use Hierarchy View to Review Epic-to-Task Breakdown.
6. GHE-ALM-020 — How to Apply Issue Metadata Correctly.

### Wave 3: Project Views and Sprint Execution

1. GHE-ALM-006 — How to Create and Use an Organization-Level GitHub Project.
2. GHE-ALM-026 — How to Use the Product Backlog View.
3. GHE-ALM-027 — How to Configure or Request a Sprint Iteration Field.
4. GHE-ALM-028 — How to Plan the Next Sprint.
5. GHE-ALM-029 — How to Use the Current Sprint Board.
6. GHE-ALM-030 — How to Move Unfinished Work to a Later Sprint.
7. GHE-ALM-032 — How to Close a Sprint Review.

### Wave 4: Bug Management

1. GHE-ALM-014 — How to Create and Triage a Bug Report.
2. GHE-ALM-034 — How to Use the Bug Triage View.
3. GHE-ALM-035 — How to Distinguish Severity from Priority.
4. GHE-ALM-036 — How to Move a Bug Through the Defect Workflow.
5. GHE-ALM-039 — How to Run a Weekly Bug Review.

### Wave 5: Release Management

1. GHE-ALM-041 — How to Track a Release with Milestones and Release Fields.
2. GHE-ALM-042 — How to Create and Manage a Milestone.
3. GHE-ALM-044 — How to Use the Release Roadmap View.
4. GHE-ALM-045 — How to Read Release Health from the Roadmap and Dashboard.
5. GHE-ALM-046 — How to Prepare a Release Readiness Review.
6. GHE-ALM-047 — How to Draft or Review a GitHub Release.
7. GHE-ALM-050 — How to Close a Release After Deployment.

### Wave 6: Dashboards and Reporting

1. GHE-ALM-051 — How to Create and Interpret Project Insights Charts.
2. GHE-ALM-052 — How to Configure Chart Filters and Axes.
3. GHE-ALM-053 — How to Use Historical Charts and Burn-Up Views.
4. GHE-ALM-054 — How to Run a Weekly ALM Dashboard Review.
5. GHE-ALM-056 — How to Identify Reporting Gaps That Require BI or External Tools.

### Wave 7: Traceability and Governance

1. GHE-ALM-059 — How to Understand Branch Naming Conventions.
2. GHE-ALM-060 — How to Verify Issue-to-Pull-Request Traceability.
3. GHE-ALM-061 — How to Interpret Pull Request Status for Managers.
4. GHE-ALM-062 — How to Verify Review and Approval Compliance.
5. GHE-ALM-067 — How to Approve a Protected Deployment.
6. GHE-ALM-073 — How to Request Rulesets or Branch Protection.
7. GHE-ALM-078 — How to Run a Quarterly ALM Hygiene Audit.

### Wave 8: Pilot Execution

1. GHE-ALM-079 — How to Run the GitHub Enterprise ALM Pilot Evaluation.
2. GHE-ALM-080 — How to Test Feature and Requirement Decomposition.
3. GHE-ALM-081 — How to Test Sprint Planning and Execution.
4. GHE-ALM-082 — How to Test Bug Intake and Triage.
5. GHE-ALM-083 — How to Test Release Tracking.
6. GHE-ALM-084 — How to Test Leadership Dashboard Sufficiency.
7. GHE-ALM-085 — How to Record Pilot Pass/Fail Evidence.

---

## 6. Role-Based Guide Assignments

### Project Manager / Program Manager

Required guides:

- Orientation and navigation: GHE-ALM-001 through GHE-ALM-005.
- Project setup and views: GHE-ALM-006 through GHE-ALM-010.
- Feature, requirement, and task intake: GHE-ALM-011 through GHE-ALM-025.
- Sprint execution: GHE-ALM-026 through GHE-ALM-033.
- Dashboards: GHE-ALM-051 through GHE-ALM-058.
- Pilot execution: GHE-ALM-079 through GHE-ALM-085.

### Engineering Manager

Required guides:

- All project manager guides.
- Bug workflow: GHE-ALM-034 through GHE-ALM-040.
- Release governance: GHE-ALM-041 through GHE-ALM-050.
- Traceability: GHE-ALM-059 through GHE-ALM-064.
- Governance: GHE-ALM-070 through GHE-ALM-078.

### Release Manager

Required guides:

- GHE-ALM-041 through GHE-ALM-050.
- GHE-ALM-065 through GHE-ALM-069.
- GHE-ALM-060 through GHE-ALM-064.
- GHE-ALM-054 through GHE-ALM-058.

### QA Manager

Required guides:

- GHE-ALM-014.
- GHE-ALM-034 through GHE-ALM-040.
- GHE-ALM-045.
- GHE-ALM-046.
- GHE-ALM-051 through GHE-ALM-055.

### GitHub Administrator / DevOps Lead

Manager-facing review or request guides:

- GHE-ALM-009.
- GHE-ALM-010.
- GHE-ALM-023 through GHE-ALM-025.
- GHE-ALM-027.
- GHE-ALM-068 through GHE-ALM-075.

---

## 7. Guides That Should Include Screenshots

The following guides should be written with annotated screenshots because the workflow is portal-heavy and the target audience is non-technical.

| Guide ID | Screenshot Need |
|---|---|
| GHE-ALM-002 | Organization, repository, Project, and Repository Dashboard navigation. |
| GHE-ALM-003 | Repository Dashboard saved views and Admin Access view. |
| GHE-ALM-006 | Organization Project creation flow. |
| GHE-ALM-008 | Adding issues/PRs to a Project. |
| GHE-ALM-011 through GHE-ALM-016 | Issue creation forms for each work item type. |
| GHE-ALM-017 | Sub-issue creation and parent issue layout. |
| GHE-ALM-018 | Hierarchy View enabled in a Project table. |
| GHE-ALM-026 through GHE-ALM-030 | Backlog, sprint planning, current sprint board, and iteration movement. |
| GHE-ALM-034 through GHE-ALM-040 | Bug form, bug triage table, and defect status workflow. |
| GHE-ALM-041 through GHE-ALM-050 | Milestones, release roadmap, release drafting, and deployment review. |
| GHE-ALM-051 through GHE-ALM-053 | Project Insights chart creation and configuration. |
| GHE-ALM-060 through GHE-ALM-064 | Linked issue/PR traceability and timeline events. |
| GHE-ALM-066 through GHE-ALM-067 | Deployment history and approval flow. |
| GHE-ALM-073 through GHE-ALM-075 | Governance screens for branch/ruleset/code-owner review. |

---

## 8. Guides That Require Administrator Participation

These guides should not be written as if every non-technical manager can complete them directly. They require elevated GitHub permissions or coordination with an administrator.

| Guide ID | Activity | Required Elevated Role |
|---|---|---|
| GHE-ALM-009 | Configure auto-add workflow. | Project admin or project maintainer. |
| GHE-ALM-010 | Configure auto-archive workflow. | Project admin or project maintainer. |
| GHE-ALM-023 | Define organization issue types. | Organization owner. |
| GHE-ALM-024 | Define organization issue fields. | Organization owner. |
| GHE-ALM-025 | Create issue forms in repositories. | Repository write/admin access. |
| GHE-ALM-027 | Create or modify Project iteration field. | Project admin or project maintainer. |
| GHE-ALM-068 | Configure environment protection rules. | Repository admin. |
| GHE-ALM-070 | Establish organization/repository structure. | Enterprise or organization administrator. |
| GHE-ALM-071 | Configure nested teams. | Organization owner or team maintainer. |
| GHE-ALM-072 | Grant repository/project access. | Organization owner, repository admin, or team maintainer. |
| GHE-ALM-073 | Configure rulesets or branch protection. | Repository admin or organization admin. |
| GHE-ALM-075 | Configure CODEOWNERS and required code-owner review. | Repository admin and engineering owner. |

---

## 9. Guides That Should Be Written as Manager Checklists Rather Than Configuration Manuals

The following activities are important for managers, but the manager usually should not perform the underlying configuration.

| Guide ID | Recommended Format | Reason |
|---|---|---|
| GHE-ALM-023 | Request checklist | Issue types affect organization-wide taxonomy. |
| GHE-ALM-024 | Request checklist | Issue fields affect reporting and should be governed. |
| GHE-ALM-068 | Request/review checklist | Deployment protection is security-sensitive. |
| GHE-ALM-073 | Request/review checklist | Branch and tag rules affect engineering workflow. |
| GHE-ALM-075 | Request/review checklist | CODEOWNERS requires engineering ownership decisions. |
| GHE-ALM-056 | Decision checklist | External reporting may require architecture, data, or licensing decisions. |
| GHE-ALM-057 | Request checklist | API/BI export is usually technical. |

---

## 10. Guide Template Recommendation

Every future "How To" guide should use the same structure.

```text
# How to [perform activity]

Audience:
Primary role:
Estimated time:
Required permissions:
Prerequisites:
When to use this guide:
When not to use this guide:

## Outcome

By the end of this guide, the user will have produced:

- [Output artifact 1]
- [Output artifact 2]

## Before You Start

- [Prerequisite]
- [Data required]
- [Permission required]

## Steps

1. [Step]
2. [Step]
3. [Step]

## Validation Checklist

- [ ] Expected result is visible.
- [ ] Required fields are populated.
- [ ] Correct Project view shows the item.
- [ ] Required stakeholders can see the result.

## Common Mistakes

- [Mistake]
- [Mistake]

## Escalation Path

- GitHub administrator:
- Repository administrator:
- Engineering lead:
- Release manager:

## Related Guides

- GHE-ALM-XXX
- GHE-ALM-YYY
```

---

## 11. Completeness Checklist

A GitHub Enterprise ALM training package for non-technical managers is complete only when guides exist for all of the following workflow categories.

| Workflow Category | Required Guide Coverage | Complete? |
|---|---|---|
| GitHub navigation | Enterprise, organization, repository, Project, Repository Dashboard | No |
| Project setup | Organization Project, fields, views, automation, archiving | No |
| Work item intake | Feature, requirement, epic, task, risk, change request | No |
| Bug intake | Bug form, triage, evidence, severity, priority | No |
| Work hierarchy | Sub-issues, parent/child structure, Hierarchy View | No |
| Sprint planning | Iteration field, sprint planning, current sprint, rollover | No |
| Release planning | Milestones, Release field, roadmap, readiness review | No |
| GitHub Releases | Tags, release notes, assets, publish/review | No |
| Reporting | Insights, charts, dashboards, reporting gaps, exports | No |
| Traceability | Issue-to-branch, issue-to-PR, PR-to-release, timeline events | No |
| Deployment governance | Environments, approvals, deployment history, protection rules | No |
| GitHub governance | Teams, permissions, rulesets, CODEOWNERS, naming conventions | No |
| Pilot evaluation | Five pilot scenarios, scorecard, adoption decision | No |

---

## 12. Source Reference Map

The following references were used to validate that the identified activities map to current GitHub Enterprise capabilities and portal workflows.

### GitHub Projects and Planning

- GitHub Docs, "Planning and tracking with Projects"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects

- GitHub Docs, "Creating a project"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/creating-projects/creating-a-project

- GitHub Docs, "Adding items to your project"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-items-in-your-project/adding-items-to-your-project

- GitHub Docs, "Changing the layout of a view"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/customizing-views-in-your-project/changing-the-layout-of-a-view

- GitHub Docs, "Customizing the roadmap layout"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/customizing-views-in-your-project/customizing-the-roadmap-layout

- GitHub Docs, "About iteration fields"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/understanding-fields/about-iteration-fields

- GitHub Docs, "About insights for Projects"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/viewing-insights-from-your-project/about-insights-for-projects

- GitHub Docs, "Configuring charts"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/viewing-insights-from-your-project/configuring-charts

- GitHub Docs, "Adding items automatically"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/automating-your-project/adding-items-automatically

- GitHub Docs, "Using the built-in automations"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/automating-your-project/using-the-built-in-automations

- GitHub Docs, "Archiving items automatically"  
  https://docs.github.com/en/issues/planning-and-tracking-with-projects/automating-your-project/archiving-items-automatically

### GitHub Issues and Work Hierarchy

- GitHub Docs, "About issues"  
  https://docs.github.com/en/issues/tracking-your-work-with-issues/learning-about-issues/about-issues

- GitHub Docs, "Managing issue types in an organization"  
  https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/managing-issue-types-in-an-organization

- GitHub Docs, "Managing issue fields in your organization"  
  https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/managing-issue-fields-in-your-organization

- GitHub Docs, "Configuring issue templates for your repository"  
  https://docs.github.com/en/communities/using-templates-to-encourage-useful-issues-and-pull-requests/configuring-issue-templates-for-your-repository

- GitHub Changelog, "Hierarchy view in GitHub Projects is now generally available"  
  https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available/

- GitHub Changelog, "Hierarchy view improvements and file uploads in issue forms"  
  https://github.blog/changelog/2026-03-05-hierarchy-view-improvements-and-file-uploads-in-issue-forms/

### Repository Navigation

- GitHub Changelog, "Repository Dashboard is now generally available"  
  https://github.blog/changelog/2026-02-24-repository-dashboard-is-now-generally-available/

### Releases and Deployment

- GitHub Docs, "Managing releases in a repository"  
  https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository

- GitHub Docs, "Automatically generated release notes"  
  https://docs.github.com/en/repositories/releasing-projects-on-github/automatically-generated-release-notes

- GitHub Docs, "Managing environments for deployment"  
  https://docs.github.com/actions/deployment/targeting-different-environments/using-environments-for-deployment

- GitHub Docs, "Deployments and environments"  
  https://docs.github.com/en/actions/reference/workflows-and-actions/deployments-and-environments

### Branch, Pull Request, and Governance

- GitHub Docs, "Linking a pull request to an issue"  
  https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/linking-a-pull-request-to-an-issue

- GitHub Docs, "Using keywords in issues and pull requests"  
  https://docs.github.com/en/get-started/writing-on-github/working-with-advanced-formatting/using-keywords-in-issues-and-pull-requests

- GitHub Docs, "About rulesets"  
  https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets

- GitHub Docs, "Available rules for rulesets"  
  https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/available-rules-for-rulesets

- GitHub Docs, "Managing a branch protection rule"  
  https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/managing-a-branch-protection-rule

- GitHub Docs, "About code owners"  
  https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-code-owners

---

## 13. Final Recommendation

The GitHub Enterprise ALM workflow requires a structured training package for non-technical managers. The complete inventory contains **85 distinct how-to guides**.

The first training release should not attempt to produce all 85 guides. It should produce the minimum viable set of 18 core guides listed in Section 3, then expand through the training waves in Section 5.

The highest-risk areas for non-technical manager adoption are:

1. Confusing repositories, Projects, issues, milestones, and releases.
2. Misusing labels instead of issue types and fields.
3. Failing to maintain metadata hygiene.
4. Losing traceability between requirements, issues, pull requests, releases, and deployments.
5. Expecting Azure DevOps-style reports without configuring GitHub views and insights.
6. Treating admin-only governance activities as routine project-manager activities.

A disciplined guide set will make GitHub Enterprise operationally usable for project managers and engineering managers without requiring them to become software developers.
