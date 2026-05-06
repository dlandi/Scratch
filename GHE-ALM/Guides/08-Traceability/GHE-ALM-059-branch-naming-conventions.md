# How to Understand Branch Naming Conventions

**Guide ID:** GHE-ALM-059
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Understands / Manager Reviews
**Estimated time:** 10-15 minutes to learn, 1-2 minutes per use
**Required permissions:** Repository: `Read`
**Prerequisites:**

- A repository or Project that contains active branches.
- Familiarity with GitHub Issues numbering.
- Awareness of your team's adopted naming convention (or the recommended convention below).

**When to use this guide:** Use when a developer mentions a branch in a standup, when a pull request title references a branch, when a release manager asks which work landed on `release/2026.05`, or when you need to confirm that engineering activity matches a tracked issue.

**When not to use this guide:** Do not use this guide to set or enforce naming rules at the repository level. Naming enforcement belongs in rulesets and team policy. See GHE-ALM-073 and GHE-ALM-077.

## Outcome

By the end of this guide, you will have produced:

- A reliable mental model for reading any well-formed branch name.
- The ability to navigate from a branch name to the originating issue in under a minute.
- A short escalation list for branches that do not match the convention.

## Before You Start

- Confirm the recommended branch naming pattern your organization has adopted. The patterns below match the GHE-ALM evaluation reference model.
- Have the GitHub repository URL ready, for example `https://github.com/acme-payments/checkout-service`.
- Know which branch holds production code (typically `main`) and which prefixes are reserved for release work (`release/*`, `hotfix/*`).

## Steps

### 1. The mental model

A branch name is a label on a line of work. In a healthy repository it tells you three things at a glance:

- **What kind of work it is.** The prefix (`feature/`, `bugfix/`, `task/`, `requirement/`, `release/`, `hotfix/`) maps to a work item type or to a release artifact.
- **Which tracked item it belongs to.** The issue number embedded in the branch (for example `1234`) ties the branch to an issue you can open in GitHub.
- **What it is about, in plain English.** The short description after the issue number is for humans skimming the branch list.

A branch name is a pointer, not a record. The authoritative record is the issue. If a branch name and an issue title disagree, trust the issue.

### 2. Recommended naming patterns

These are the patterns referenced by the GHE-ALM evaluation. Confirm with your release manager which patterns your team uses.

| Pattern | Used for | Example |
|---|---|---|
| `feature/<issue#>-short-desc` | Implementing a feature | `feature/1234-add-user-export` |
| `requirement/<issue#>-short-desc` | Implementing a tracked requirement | `requirement/1450-support-multi-region-routing` |
| `bugfix/<issue#>-short-desc` | Fixing a defect | `bugfix/1250-fix-login-timeout` |
| `task/<issue#>-short-desc` | A standalone task or chore | `task/1301-refactor-order-service` |
| `release/<version>` | Cutting or stabilizing a release | `release/2026.05` |
| `hotfix/<version>` | Patching a released version | `hotfix/2026.05.1` |

Issue-linked branches always carry an issue number. Release and hotfix branches carry a version, not an issue number, because they represent a release line rather than a single work item.

### 3. How to read a branch name in three parts

Take `feature/1234-add-user-export`:

- `feature` is the prefix. This branch represents new functionality, not a fix.
- `1234` is the issue number. The branch should map to issue `#1234` in this repository, or to `owner/repo#1234` if cross-repository.
- `add-user-export` is the human-readable slug. It is informational only.

Take `release/2026.05`:

- `release` is the prefix. This is a release stabilization branch, not a feature branch.
- `2026.05` is the version. There is no issue number, and that is correct.

### 4. Trace a branch to its issue

Use this short routine when a branch name shows up in a standup, PR title, or deployment log.

1. Read the prefix. If it is `release/` or `hotfix/`, skip to step 4. Otherwise continue.
2. Pull the issue number out of the branch name. In `bugfix/1250-fix-login-timeout` the number is `1250`.
3. In your browser, navigate to the issue: `https://github.com/<org>/<repo>/issues/1250`. Or paste `#1250` into the repository's issue search.
4. For a `release/<version>` or `hotfix/<version>` branch, open the matching milestone or the Project filtered by `Release` field equal to that version. The release branch's scope is the set of issues in that milestone or release.

> [SCREENSHOT: Repository branch list with a `feature/<issue#>-short-desc` branch highlighted next to its linked issue in a side panel]

### 5. Worked example

A standup mentions: "I merged `bugfix/1250-fix-login-timeout` last night, blocked on `feature/1234-add-user-export` review."

You read it as:

- A bug fix for issue `#1250` was merged. To confirm scope and severity, open issue `#1250` and check `Severity`, `Priority`, and the linked PR.
- A feature for issue `#1234` is awaiting review. Open the issue, scroll to the Development panel, click the linked pull request, and check the PR's status. See GHE-ALM-061 for how to interpret PR status.

You did not need to read any code to understand the standup.

### 6. What good looks like vs. what to escalate

| Signal | What good looks like | What to escalate, and to whom |
|---|---|---|
| Prefix | Branch starts with `feature/`, `requirement/`, `bugfix/`, `task/`, `release/`, or `hotfix/`. | Branch starts with `dev/`, a person's name, a date, or no prefix. Raise with engineering lead. |
| Issue number on work branch | Issue number is present, numeric, and resolves to an open or recently closed issue. | Issue number is missing, made up, or points to an unrelated issue. Raise with the branch author and engineering lead. |
| Slug | Slug is short, lowercase, hyphenated, and recognizable. | Slug is empty, all numeric, or contains personal commentary. Note for hygiene review (GHE-ALM-078). |
| Release branches | `release/<version>` and `hotfix/<version>` follow the agreed version scheme. | A release branch uses a name that does not match a known release. Raise with the release manager. |
| Pull request linkage | The PR opened from this branch references the same issue using `Closes #1234`, `Fixes #1234`, or `Resolves #1234`. | The PR has no linked issue, or the PR's linked issue does not match the branch's number. See GHE-ALM-060. |
| Long-lived branches | Feature and bugfix branches close within a sprint or two. | A `feature/` or `bugfix/` branch has been open for several sprints. Raise as a sprint hygiene item. |

### 7. Common edge cases

- **Cross-repository work.** If the work is tracked in another repository, the issue reference uses `owner/repo#NNN`. The branch will still typically use just the local issue number.
- **Spike, research, or exploration branches.** These may use a `task/` prefix or a separate `spike/` prefix if the team has adopted one. Confirm with your engineering lead which prefixes are sanctioned.
- **Renamed or rebased branches.** A branch name can change. Always trust the linked issue and the PR over the branch name.
- **Personal forks.** Contributors working from forks may not follow your convention. The PR opened against your repository should still link to a tracked issue.

## Validation Checklist

- [ ] You can identify the prefix, issue number, and slug in any well-formed branch name.
- [ ] You can navigate from a branch name to the originating issue in your repository in under one minute.
- [ ] You can identify a `release/<version>` or `hotfix/<version>` branch and locate the matching milestone or `Release` field value.
- [ ] You know which engineering owner to escalate non-conforming branch names to.

## Common Mistakes

- Treating a branch name as authoritative. The issue is the source of truth.
- Assuming any branch starting with a number is a bug. The number identifies the issue, not the issue type.
- Reading the slug to determine scope. The slug is a hint; the issue carries the acceptance criteria.
- Equating the absence of an issue number on a `release/<version>` branch with a problem. Release branches do not need an issue number.
- Trying to enforce naming by hand. Enforcement belongs in rulesets (GHE-ALM-073) and naming policy (GHE-ALM-077).

## Escalation Path

- GitHub administrator: Not applicable for branch reading. Involve only when discussing organization-wide naming policy.
- Repository administrator: Involve when branches that bypass the convention are being merged into protected branches.
- Engineering lead: Involve when individual contributors repeatedly ignore the naming convention or when the issue number on a branch does not match its work.
- Release manager: Involve when a `release/<version>` or `hotfix/<version>` branch does not match the published release schedule.

## Related Guides

- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-061 : How to Interpret Pull Request Status for Managers
- GHE-ALM-073 : How to Request Rulesets or Branch Protection
- GHE-ALM-077 : How to Enforce Naming Conventions
