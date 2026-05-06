# How to Request or Review CODEOWNERS-Based Review Routing

**Guide ID:** GHE-ALM-075
**Audience:** Engineering Manager, Release Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-45 minutes for a request; 15-20 minutes per review pass
**Required permissions:** Repository: `Read` to inspect an existing `CODEOWNERS` file; Repository: `Admin` is required to commit a new one (the engineering owner does this), so a manager submits a request rather than editing the file directly.
**Prerequisites:**

- A repository under an `acme-*` organization where review routing is needed, for example `acme-payments/payments-api`.
- A list of sensitive paths and their accountable owners (team or user).
- Knowledge of which teams already exist in the organization (cross-ref GHE-ALM-071).

**When to use this guide:** Use this when sensitive code paths in a repository need a specific team or person automatically requested as reviewer on every pull request that touches them, or when you are inspecting an existing `CODEOWNERS` file as part of a governance review.

**When not to use this guide:** Do not use this to enforce who *must* approve before merge. The `CODEOWNERS` file only routes review requests. Required code-owner review is enforced by a repository ruleset or branch protection rule (see GHE-ALM-073).

## Outcome

By the end of this guide, you will have produced:

- A written request to the repository administrator and engineering lead specifying the path-to-owner mapping for a `CODEOWNERS` file, or
- A completed review pass over an existing `CODEOWNERS` file with findings recorded as either accepted, change-requested, or escalated.

## Before You Start

- Confirm the repository and default branch (typically `main`).
- List the sensitive paths: secrets handling, payment processing, infra-as-code, public APIs, security configuration, release tooling.
- Identify the accountable team for each path. Owners must be GitHub users, organization teams in the same org as the repository, or email addresses tied to user accounts. Teams and users must already have at least `Write` access to the repository.
- Decide which branch the file should govern. The file lives on a branch; different branches can carry different ownership rules.

## Steps

### Decide whether to request a new file or review an existing one

1. Open the repository on the GitHub Enterprise portal.
2. In the file browser on the default branch, look for `CODEOWNERS` in three locations, in order: `.github/CODEOWNERS`, repository-root `CODEOWNERS`, and `docs/CODEOWNERS`. GitHub uses the first one it finds.
3. If no file exists, follow the **Submit a request** path below.
4. If a file exists, follow the **Review an existing file** path below.

### Submit a request

5. Draft the path-to-owner mapping. Use gitignore-style globs. Each line is `<path-pattern> <owner1> <owner2> ...`. Order matters: the last matching line for a given file wins, so list general rules first and specific overrides later.
6. Cover at minimum: the catch-all `*` for default ownership, infra paths (`/infra/`, `/.github/workflows/`), security-sensitive paths (`/auth/`, `/payments/`, `/secrets/`), and release tooling (`/release/`, `/scripts/release/`).
7. Verify each owner is either an existing user (`@first.last`) or an existing org team (`@acme-payments/platform-leads`). Teams must be in the same organization as the repository and must have `Write` permission or higher.
8. Send the request using the **Sample Request to Send** template below.

### Review an existing file

9. Open the `CODEOWNERS` file on the default branch.
10. For each rule, verify three things: the path glob still matches a real directory, the named owner still exists (no archived users or deleted teams), and the team still has `Write` access.
11. Walk the path list against the **What Good Looks Like vs. What to Escalate** table.
12. Record findings as `accepted`, `change-requested`, or `escalated`. Use a pull request comment or an issue in the repository's governance tracker to capture the result.

> [SCREENSHOT: CODEOWNERS file open in the GitHub file browser with a sample rule set highlighted]

## Sample Request to Send

Send this to the repository administrator with the engineering lead in copy. Replace placeholders before sending.

```
Subject: CODEOWNERS request for acme-payments/payments-api

Repository: acme-payments/payments-api
Branch: main
File location: .github/CODEOWNERS

Requested rules (in order, last match wins):

# Default fallback
*                              @acme-payments/platform-leads

# CI and release tooling
/.github/workflows/            @acme-payments/devops
/scripts/release/              @acme-payments/release-managers

# Security-sensitive paths
/src/auth/                     @acme-payments/security
/src/secrets/                  @acme-payments/security

# Payments domain
/src/payments/                 @acme-payments/payments-core
/src/payments/refunds/         @acme-payments/payments-refunds

# Public API contracts
/api/                          @acme-payments/api-stewards

Required code-owner review enforcement:
Please pair this file with a ruleset on `main` that requires
code-owner review (see GHE-ALM-073). Without that ruleset,
this file only requests reviewers; it does not block merges.

Validation expected:
- File commits cleanly with no parser warnings.
- Opening a draft PR that touches /src/auth/ shows
  @acme-payments/security as a requested reviewer once the
  PR is marked ready.

Owner contact: <engineering lead name and handle>
Requested by: <your name and role>
Target completion: <date>
```

## What Good Looks Like vs. What to Escalate

| Aspect | What Good Looks Like | What to Escalate |
|---|---|---|
| File location | One file in `.github/CODEOWNERS`, root, or `docs/` | Multiple files in different locations causing confusion about which one wins |
| Default rule | `*` line maps to a stable team | No catch-all line, or catch-all maps to a single individual |
| Owner validity | Every `@user` and `@org/team` resolves on the org page | Archived users, deleted teams, or owners without `Write` access |
| Order | General rules first, specific paths later | Specific rule above a broader rule that overwrites it |
| Sensitive paths | Auth, payments, secrets, release tooling, workflows are explicitly owned | Security or release paths fall through to the default team |
| Parser status | No invalid-line warnings shown when viewing the file | Red error indicators in the file view, or the REST API errors list is non-empty |
| Enforcement pairing | A ruleset on protected branches requires code-owner review | File exists, no ruleset, code-owner review is only advisory |
| Draft PRs | Reviewers requested when PR moves from draft to ready | Reviewers never requested, suggesting parser failure or no matching rule |

## Validation Checklist

- [ ] `CODEOWNERS` lives in exactly one of `.github/`, repository root, or `docs/` on the default branch.
- [ ] A catch-all `*` rule exists and points to a team, not a single user.
- [ ] Every owner referenced is an existing user, an existing same-org team, or a verified email.
- [ ] Each named team has `Write` or higher repository access.
- [ ] Sensitive paths (auth, secrets, payments, release tooling, CI workflows) are explicitly owned.
- [ ] Opening a test PR that touches a covered path automatically requests the listed reviewers.
- [ ] A ruleset or branch protection rule on `main` requires code-owner review (see GHE-ALM-073), if enforcement is intended.
- [ ] No parser warnings are shown when viewing the file in the portal.

## Common Mistakes

- Treating `CODEOWNERS` as enforcement. It only requests reviewers. Without a ruleset that requires code-owner review, a PR can still merge with no code-owner approval.
- Putting specific rules above general rules. Last match wins, so a later `*` line will overwrite earlier specific rules.
- Listing a team that does not exist in the same organization, or a team with only `Read` access. The line is silently ignored.
- Using gitignore features that do not work here: `!` negation, `[abc]` character ranges, and backslash-escaped `#` patterns.
- Editing the file on a feature branch and expecting it to apply on `main`. The file is read from the branch the PR targets.
- Letting one individual own a sensitive path. When that person is on leave, no review request fires. Always prefer a team.
- Forgetting that draft PRs do not trigger review requests; the request fires when the PR is marked ready.

## Escalation Path

- GitHub administrator: when team membership across organizations needs adjustment to make a team eligible as an owner.
- Repository administrator: when the `CODEOWNERS` file needs to be created, edited, or moved, since this requires a commit to a protected branch.
- Engineering lead: when the path-to-owner mapping itself is contested, or when a sensitive path has no clear accountable team.
- Release manager: when ownership of release tooling, deployment workflows, or hotfix paths is unclear.

## Related Guides

- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-071 : How to Request or Review Nested Teams
- GHE-ALM-073 : How to Request Rulesets or Branch Protection
- GHE-ALM-077 : How to Enforce Naming Conventions
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
