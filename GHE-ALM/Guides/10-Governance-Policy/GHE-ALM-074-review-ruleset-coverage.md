# How to Review Ruleset and Branch Protection Coverage

**Guide ID:** GHE-ALM-074
**Audience:** Engineering Manager, Release Manager, Program Manager
**Primary role:** Engineering Manager
**Classification:** Manager Reviews
**Estimated time:** 15-20 minutes per repository
**Required permissions:** Repository: Read (to view active rulesets); Repository: Admin (only if you also need to edit). Read access is enough for inspection.
**Prerequisites:**

- A repository in scope, for example `acme-payments/checkout-service`.
- An agreed list of branches that must be governed: `main`, `release/*`, `hotfix/*`.
- Familiarity with your team's required checks and approval policy.

**When to use this guide:** Use this when you need to confirm that protection actually covers the branches your team treats as critical, before a release, after a repository is created, during a quarterly audit, or after an incident where an unprotected branch was used.

**When not to use this guide:** Do not use this guide to author or change rulesets. Configuration requests belong in GHE-ALM-073. CODEOWNERS-specific review routing belongs in GHE-ALM-075.

## Outcome

By the end of this guide, you will have produced:

- A short coverage finding for the repository: which critical branches are governed, which are not, and which rules are missing.
- A list of items to escalate to the repository administrator or organization administrator.

## Before You Start

- Know the repository name and organization.
- Have the team's branching standard handy: which branches map to production, which to release trains, which to hotfixes.
- Have the team's required-check names handy, for example `build`, `unit-tests`, `lint`, `security-scan`.

## Steps

### Open the repository's Rules screen

1. Navigate to the repository, for example `https://github.example.com/acme-payments/checkout-service`.
2. Click **Settings**.
3. In the left navigation, expand **Rules** and click **Rulesets**. This shows every ruleset that targets this repository, including organization-level rulesets that apply to it.
4. On the same Rules menu, also note **Rule Insights**. You will use this later to see whether rules have been bypassed.

> [SCREENSHOT: Settings > Rules > Rulesets list, showing one organization-level ruleset and one repository-level ruleset, each with Enforcement status and Targets columns.]

### Read each active ruleset

5. For each ruleset, confirm the **Enforcement status**. Only `Active` rulesets enforce. `Disabled` rulesets do not protect anything, even if the rules look correct. Treat `Evaluate` (where available) as not yet enforcing.
6. Open the ruleset and read the **Target branches** section. Confirm the `fnmatch` patterns. Examples of healthy patterns: `main`, `release/*`, `hotfix/*`. A pattern of only `main` means release and hotfix branches are uncovered.
7. Read the **Rules** section. For each critical branch you expect to be governed, confirm these rules are present:
   - Restrict deletions.
   - Block force pushes.
   - Require a pull request before merging, with a minimum number of approvals.
   - Require review from Code Owners (where CODEOWNERS exists).
   - Require status checks to pass, and verify the check names match the team's required checks.
   - Require conversation resolution before merging.
   - Require signed commits, if your organization standard requires it.
   - Restrict who can push, if direct pushes should be limited.
8. Note any rule that is present but configured loosely, for example required approvals set to `0`, or "Require status checks" enabled but no checks listed.

### Check for gaps across branch patterns

9. Build a small mental matrix: rows are critical branch patterns (`main`, `release/*`, `hotfix/*`); columns are required rules (PR required, approvals, code-owner review, required checks, force-push blocked, deletion blocked, signed commits). Mark each cell `Yes`, `No`, or `Partial` based on what you read.
10. If a branch pattern is not targeted by any active ruleset and has no legacy branch protection rule, treat it as ungoverned.
11. Open **Settings** > **Branches** in the same repository to check for any legacy branch protection rules. Branch protection rules and rulesets can coexist. The most restrictive rule applies when both target the same branch, so legacy rules can hide gaps or duplicate coverage. Capture both.

### Spot-check enforcement evidence

12. Open **Settings** > **Rules** > **Rule Insights**. Filter by branch and by actor. Look for recent **Bypass** events. A high bypass rate, or bypasses by accounts that should not have bypass rights, is a finding.
13. Open the repository's **Branches** page and confirm that `main`, the latest `release/*` branch, and any open `hotfix/*` branch each show a protection indicator. A branch with no indicator is a gap.

> [SCREENSHOT: Rule Insights filtered to the last 30 days, showing rule evaluations and any bypass events with actor and branch.]

### Record findings

14. Write a brief coverage note for the repository in this format: branch pattern, governed yes/no, missing rules, who to escalate to.
15. Decide which findings are blockers (escalate now) versus which are improvements (track in the next governance cycle).

## What Good Looks Like vs. What to Escalate

| Area | What Good Looks Like | What to Escalate |
|---|---|---|
| Coverage of `main` | Active ruleset targets `main` with required PR, required approvals, required checks, force-push blocked, deletion blocked. | `main` is not targeted, ruleset is `Disabled`, or required approvals is `0`. |
| Coverage of `release/*` | Pattern `release/*` is targeted with the same core rules as `main`. | No ruleset targets `release/*`, or only the most recent release branch is named explicitly. |
| Coverage of `hotfix/*` | Pattern `hotfix/*` is targeted, with required checks and required reviews appropriate for emergency fixes. | `hotfix/*` is ungoverned, or hotfix branches rely on a single reviewer with self-approval allowed. |
| Required status checks | Check names listed match the team's CI workflow names exactly. | "Require status checks" enabled but no checks listed, or check names no longer exist in the workflow. |
| Code-owner review | Required where CODEOWNERS covers the touched paths. See GHE-ALM-075. | Required code-owner review enabled but `CODEOWNERS` is missing, empty, or owned by archived teams. |
| Force push and deletion | Both blocked on `main`, `release/*`, and `hotfix/*`. | Force push allowed, branch deletion allowed, or rule scoped only to `main`. |
| Bypass list | Empty or limited to a small admin group with documented justification. | Long bypass list, individual user accounts, or unexplained bypass actors. |
| Rule Insights | Few or no bypass events; bypasses correspond to known incidents. | Frequent bypass events, bypasses outside incident windows, or bypasses by accounts not on the bypass list. |
| Legacy branch protection | None, or knowingly retained and consistent with rulesets. | Conflicting rules between legacy protection and rulesets, with the looser rule effectively winning on a non-overlapping branch. |
| Signed commits | Required where the organization standard requires it. | Required by policy but not enforced on the protected branches. |

## Validation Checklist

- [ ] Every critical branch pattern (`main`, `release/*`, `hotfix/*`) is targeted by at least one `Active` ruleset or branch protection rule.
- [ ] Required pull request, required approvals, and required status checks are present on each critical branch pattern.
- [ ] Force pushes and deletions are blocked on each critical branch pattern.
- [ ] Status check names listed in the ruleset still exist in the repository's workflows.
- [ ] Bypass actors, if any, are documented and limited.
- [ ] Rule Insights for the last 30 days has been reviewed for unexpected bypass events.
- [ ] Findings are recorded with a clear severity and an escalation owner.

## Common Mistakes

- Treating `Disabled` rulesets as protective. They do not enforce anything.
- Reviewing only `main` and assuming `release/*` and `hotfix/*` inherit protection. They do not unless explicitly targeted.
- Confirming "Require status checks" is on without confirming the actual check names.
- Ignoring legacy branch protection rules that overlap or conflict with rulesets.
- Skipping Rule Insights and missing a pattern of bypass events.
- Confusing organization-level rulesets (which apply broadly) with repository-level rulesets (which apply only here). Both must be considered together.

## Escalation Path

- GitHub administrator: Escalate when an organization-level ruleset is missing, misconfigured, or applied to the wrong repositories.
- Repository administrator: Escalate when a repository-level ruleset is disabled, missing critical branch patterns, or has an inappropriate bypass list.
- Engineering lead: Escalate when required status checks no longer match the actual CI workflow names, or when CODEOWNERS coverage is incomplete.
- Release manager: Escalate when `release/*` or `hotfix/*` patterns are ungoverned ahead of a release.

## Related Guides

- GHE-ALM-073 : How to Request Rulesets or Branch Protection
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-075 : How to Request or Review CODEOWNERS-Based Review Routing
- GHE-ALM-077 : How to Enforce Naming Conventions
- GHE-ALM-078 : How to Run a Quarterly ALM Hygiene Audit
