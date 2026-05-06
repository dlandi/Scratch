# How to Request or Review Nested Teams

**Guide ID:** GHE-ALM-071
**Audience:** Engineering Manager, Program Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-minute one-time request; 15 minutes per quarterly review
**Required permissions:** Organization: Member (to view team pages); Organization: Owner or team maintainer (to create or edit teams)
**Prerequisites:**

- A defined product or platform decomposition (areas, components, sub-systems).
- Names of the engineering managers, tech leads, or owners accountable for each area.
- Repository list for the relevant org, with the access level each team needs per repository.

**When to use this guide:** Use when the org needs a team hierarchy for permissions, code review routing, ownership signals, and notification scope, or when reviewing whether the existing team tree still matches product ownership.

**When not to use this guide:** Do not use this guide to model work hierarchy. Epics, features, requirements, and tasks belong in issues and sub-issues, not in teams. See GHE-ALM-004.

## Outcome

By the end of this guide, you will have produced:

- A request to the organization owner or team maintainer specifying the team tree, parent and child relationships, repository access per team, team visibility, and team maintainers.
- A reviewed team tree where every team has a clear owner, parent, repository permission set, and purpose, with anomalies flagged for cleanup.

## Before You Start

- Confirm which organization holds the repositories. Nested teams live inside one organization; they do not span organizations.
- List each team's intended purpose: permission grouping, code review routing via CODEOWNERS, or notification target.
- Decide team visibility per team. Visible teams can be mentioned by any org member. Secret teams are restricted to members and org owners and cannot be nested.
- Have the repository access matrix ready: for each team, which repository, which role (`Read`, `Triage`, `Write`, `Maintain`, or `Admin`).

## Steps

### Draft the team tree

1. Sketch the hierarchy on paper or in a diagram tool. Use product or platform ownership as the shape, not reporting lines. A typical pattern:

   ```
   Engineering
     Platform
       DevOps
       Security
     Application Engineering
       Customer Portal
       Checkout
       Identity
     QA
   ```

2. For each node, write the team slug (lowercase, hyphenated), display name, parent team, visibility, and one-sentence purpose. Keep the tree shallow. Three levels is usually enough; four is the practical limit before the tree becomes hard to read.
3. For each team, list the repositories it should access and the role it needs on each. Prefer the lowest role that lets the team do its job. Code review routing via CODEOWNERS only needs `Read`.
4. Identify two team maintainers per team where possible. A single maintainer is a continuity risk.

### Decide what each team is for

5. Tag each team with one or more of these purposes: permission grouping, code review routing, ownership signal, notification target. If a team has no purpose on this list, remove it from the tree.
6. Confirm child teams inherit parent repository access. If `Engineering` has `Read` on `checkout-service`, every descendant automatically has `Read`. Grant additional access only on the child team that needs more.
7. Confirm `@org/team-name` mentions on a parent notify all descendant members. Use this for cross-cutting announcements; do not abuse it for daily noise.

### Send the request (Manager Requests path)

8. Compose the request to the organization owner using the template in the next section. Include the full tree, repository-to-team-to-role matrix, visibility, and maintainers.
9. Send the request via the channel your org uses for governance changes (ticket, issue in a governance repo, or email distribution).

> [SCREENSHOT: organization Teams page showing an existing nested tree with parent and child teams expanded]

### Review an existing team tree (Manager Reviews path)

10. Open the organization. Click **Teams**. Expand the tree fully.
11. For each team, open the team page and check four things: parent team, members, repositories and role per repository, and team maintainers.
12. Compare against the comparison table in the "What Good Looks Like vs. What to Escalate" section below. Flag anomalies in a tracking issue.
13. Sample one or two CODEOWNERS files in critical repositories. Confirm the teams listed in CODEOWNERS still exist and still have at least `Read` access on the repository.

## What Good Looks Like vs. What to Escalate

| Aspect | What Good Looks Like | What to Escalate |
|---|---|---|
| Tree shape | Mirrors product ownership: platform, application areas, QA, release. Three levels deep. | Mirrors HR org chart, contains personal teams, or exceeds four levels. |
| Team purpose | Each team has a stated purpose: permissions, review routing, ownership, or notifications. | Teams exist with no documented purpose or no recent activity. |
| Parent and child | Children scope or subdivide a parent's domain. | Children duplicate the parent or have unrelated scope. |
| Visibility | Default is visible. Secret teams used only for sensitive partnerships. | Most teams are secret with no documented reason. |
| Repository access | Lowest necessary role per team. Inheritance from parent is intentional. | Many teams hold `Admin` on production repos. Inheritance grants access nobody noticed. |
| Maintainers | Two maintainers per team where headcount allows. | Single maintainer or no maintainer named. |
| CODEOWNERS alignment | Teams referenced in CODEOWNERS exist and have repo `Read`. | CODEOWNERS references missing teams, or teams without access to the repo. |
| Membership | Members are current org members in the right product area. | Former employees, external collaborators (not allowed in teams), or empty teams. |

## Sample Request to Send

Send this request to the organization owner or designated team maintainer.

```
Subject: Request: Create or update nested teams for acme-platform

Org: acme-platform
Visibility default: visible (call out exceptions)

Team tree:

  engineering (parent of: platform, application-engineering, qa, release)
    platform (parent of: devops, security)
      devops
      security
    application-engineering (parent of: customer-portal, checkout, identity)
      customer-portal
      checkout
      identity
    qa
    release

Per-team purpose:

  engineering            : org-wide notifications, baseline Read access
  platform               : platform ownership signal, CODEOWNERS routing
  devops                 : Maintain on infra repos, CODEOWNERS owner
  security               : review routing for security-sensitive paths
  application-engineering: product ownership signal
  customer-portal        : Write on web-client, CODEOWNERS owner
  checkout               : Write on checkout-service, payments-api, CODEOWNERS owner
  identity               : Write on identity-service, CODEOWNERS owner
  qa                     : Triage across product repos
  release                : Maintain on release-tooling, deployment approver

Repository access (additive over inherited Read):

  checkout-service : checkout=Write, devops=Maintain, security=Read
  payments-api     : checkout=Write, devops=Maintain, security=Read
  web-client       : customer-portal=Write, devops=Maintain
  identity-service : identity=Write, security=Read, devops=Maintain
  release-tooling  : release=Maintain, devops=Admin

Maintainers (two per team where possible): listed in attached spreadsheet.

Please confirm visibility, parent-child relationships, and repo permissions
before applying. CODEOWNERS updates will follow in a separate request
(see GHE-ALM-075).
```

## Validation Checklist

- [ ] Every team has a parent (or is a top-level team) and a one-sentence purpose.
- [ ] No team exceeds four levels of nesting.
- [ ] Each team has at least one named maintainer; two where possible.
- [ ] Repository access uses the lowest role that supports the team's purpose.
- [ ] Inherited access from parent teams is intentional, not accidental.
- [ ] Visibility default is visible; secret teams have a documented reason.
- [ ] Teams referenced in CODEOWNERS exist in the org and have at least `Read` on the relevant repository.
- [ ] No team is empty or contains only former employees.
- [ ] Team tree shape reflects product or platform ownership, not the HR org chart.

## Common Mistakes

- Modeling work hierarchy as teams. Epics, features, requirements, and tasks live in issues and sub-issues. Teams are for people, permissions, and review routing.
- Building a team tree that mirrors the HR reporting structure. Build it around product and code ownership instead.
- Granting `Admin` to broad parent teams. Inheritance pushes that role to every descendant, including new teams added later.
- Creating one team per person or per project. Teams are durable ownership units; transient projects belong in issues, milestones, and project items.
- Using secret teams as the default. Secret teams cannot be nested and hide ownership from the org.
- Letting CODEOWNERS drift. A team referenced in CODEOWNERS but missing from the org silently disables the required-review behavior on that path.
- Naming teams after technologies that change. Prefer ownership names (`checkout`, `identity`) over stack names (`react-team`, `kafka-team`).

## Escalation Path

- GitHub administrator: When team creation, deletion, or visibility changes are needed at the organization or enterprise level.
- Repository administrator: When repository access for a team needs to be added, downgraded, or revoked.
- Engineering lead: When the proposed team tree does not match the actual code ownership boundaries.
- Release manager: Not applicable, unless a release-approver team is part of the request.

## Related Guides

- GHE-ALM-004 : How to Distinguish Work Hierarchy from Repository Structure
- GHE-ALM-070 : How to Request GitHub Organization and Repository Structure
- GHE-ALM-072 : How to Request Repository Access for Project Managers and Stakeholders
- GHE-ALM-075 : How to Request or Review CODEOWNERS-Based Review Routing
