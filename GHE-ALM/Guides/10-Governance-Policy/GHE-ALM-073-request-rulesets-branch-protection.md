# How to Request Rulesets or Branch Protection

**Guide ID:** GHE-ALM-073
**Audience:** Engineering Manager, Release Manager, Program Manager
**Primary role:** Engineering Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-minute one-time request; 15 minutes per quarterly review
**Required permissions:** Repository: Read (to inspect existing rules); Repository: Admin or Organization: Owner is required to configure rules, so the manager requests the change rather than performing it.
**Prerequisites:**

- Repository or organization administrator who can implement the request.
- Agreed list of protected branch and tag patterns (typically `main`, `release/*`, `hotfix/*`, and version tags).
- CODEOWNERS coverage for sensitive paths if code-owner review will be required (see GHE-ALM-075).
- Status check names from CI workflows that must pass before merge.

**When to use this guide:** Use this guide when a new repository needs governance applied, when an existing repository has weak protection, or when a quarterly hygiene review reveals gaps. Use it whenever the repository is moving toward a release cadence that requires consistent enforcement of pull request, review, and status-check policy.

**When not to use this guide:** Do not use this guide for environment-level deployment protection (required reviewers, wait timers, deployment branch restrictions). Those belong in GHE-ALM-068. Do not use this guide for code-owner file authoring; that is GHE-ALM-075.

## Outcome

By the end of this guide, you will have produced:

- A written request to the repository or organization administrator that lists every branch pattern, tag pattern, and rule to apply.
- A comparison of current protection against the recommended baseline, with each gap flagged.
- A confirmation checklist the administrator returns once the ruleset is enforced.

## Before You Start

- Confirm whether the repository already uses classic branch protection, rulesets, or both. Rulesets are the newer model, can apply at repository or organization level, can stack, and remain visible to read-only users. Classic branch protection is the older single-rule model. New work should prefer rulesets.
- Decide the scope: repository-level ruleset for a single repository, or organization-level ruleset that targets many repositories at once. Cross-repository governance (for example, all `main` branches across `acme-payments`) is a strong reason to push the request to organization level.
- Collect the exact CI status check names. Checks must be referenced by name and the names must match the workflow job names exactly.
- Decide the bypass list. Bypass should be limited and named, not broad. Typical bypass actors are the release manager team and a designated automation account for emergency hotfixes.

## Steps

### Inspect the current state

1. Open the repository in GitHub and select **Settings**.
2. In the left navigation, open **Rules**, then **Rulesets**, to list active and disabled rulesets.
3. Open **Branches** under the same Settings area to see any classic branch protection rules still in force.
4. For each protected branch pattern, record the current rules in the comparison table below. If a rule is missing or weaker than the recommended baseline, mark it as a gap.

> [SCREENSHOT: Settings -> Rules -> Rulesets list showing active rulesets with status, target, and bypass columns]

### Compare current rules against the recommended baseline

5. Use the table below as the inspection target. The recommended baseline reflects the governance posture in evaluation document section 5.10. Adjust column three to the actual rule observed in the repository.

| Rule | What Good Looks Like | What to Escalate |
|---|---|---|
| Require pull request before merge | Active on `main`, `release/*`, `hotfix/*` | Direct push to `main` is permitted |
| Required approvals | At least 1, ideally 2 on `release/*` | Zero approvals required, or self-approval allowed |
| Dismiss stale reviews on new commits | Enabled | Approvals carry over silently after force-push or rewrite |
| Require review from Code Owners | Enabled where CODEOWNERS exists | Sensitive paths merge without owner sign-off |
| Require approval from someone other than the last pusher | Enabled | Last pusher can approve their own change |
| Require status checks to pass | All required CI jobs listed by exact name | Missing checks, or checks not listed |
| Require branches to be up to date before merge | Enabled on `release/*` | Merges proceed against stale base |
| Require all conversations resolved | Enabled | Open review threads are ignored at merge |
| Block force pushes | Enabled (default) | Force-push to `main` is permitted |
| Restrict deletions | Enabled (default) | Protected branch can be deleted |
| Restrict creations on tag patterns | Enabled for `v*.*.*` and release tags | Anyone can publish a release tag |
| Require signed commits | Enabled where signing is feasible | No signing requirement and audit asks for one |
| Linear history | Enabled where squash or rebase is the team norm | Merge commits land on `release/*` against policy |
| Bypass list | Named team or release-manager role only | "Repository admin" left as a wide bypass |

> [SCREENSHOT: comparison table marked up with current state and gaps for one repository]

### Draft and send the request

6. Use the sample request below. Fill in repository name, branch and tag patterns, required approvals, status check names, and bypass actors before sending.
7. Send the request to the repository administrator. If the request applies to more than one repository, send it to the organization owner instead and ask that the rule be implemented as an organization-level ruleset targeting the listed repositories.
8. Track the request in your governance log with a target completion date.

### Verify enforcement after implementation

9. After the administrator confirms, return to **Settings** > **Rules** > **Rulesets** and check that the new ruleset is listed with status **Active**.
10. Open a draft pull request that intentionally violates one rule, for example by skipping a required review, and confirm the merge button is blocked. Close the draft once verified.
11. Record the verification date in the governance log and notify the team that the rules are live.

## Sample Request to Send

Send the message below to the repository or organization administrator. Subject: `Ruleset request: <repo or org> protected branches`.

> Please apply the following ruleset to repository `acme-payments/checkout-service`. Implement as a repository ruleset unless an equivalent organization ruleset already covers the same patterns, in which case confirm coverage instead of duplicating.
>
> Target branches: `main`, `release/*`, `hotfix/*`. Target tags: `v*.*.*`.
>
> Branch rules:
>
> - Require a pull request before merging.
> - Required approvals: 1 on `main`, 2 on `release/*` and `hotfix/*`.
> - Dismiss stale pull request approvals when new commits are pushed.
> - Require review from Code Owners.
> - Require approval from someone other than the last pusher.
> - Require all conversations on code to be resolved.
> - Require status checks to pass: `build`, `unit-tests`, `lint`, `security-scan`. Require branches to be up to date before merging.
> - Block force pushes.
> - Restrict deletions.
> - Require signed commits (if organization signing policy is in place).
>
> Tag rules:
>
> - Restrict creations of tags matching `v*.*.*` to the release-manager team.
> - Restrict deletions and updates of release tags.
>
> Bypass list: `acme-payments/release-managers` team only. No individual user bypass.
>
> Please confirm once active and reply with the ruleset name and link.

## Validation Checklist

- [ ] Ruleset appears under **Settings** > **Rules** > **Rulesets** with status **Active**.
- [ ] Target branches and tag patterns match the request.
- [ ] Required status checks list matches the CI job names.
- [ ] Required approval count matches the requested number for each branch pattern.
- [ ] Code-owner review is required on branches that depend on it.
- [ ] Force pushes and deletions are blocked on protected branches.
- [ ] Bypass list is limited to the named team.
- [ ] A test pull request that violates one rule is blocked from merging.

## Common Mistakes

- Mixing rulesets and classic branch protection without auditing both. The most restrictive rule wins, so the result can be confusing. Migrate to rulesets and disable redundant classic rules.
- Listing required status checks by display label instead of exact job name. The check will never be found and the rule never enforces.
- Allowing self-approval by leaving "require approval from someone other than the last pusher" off. This silently defeats review governance.
- Granting bypass to a wide group such as all repository admins. Bypass should be a named, accountable team.
- Protecting `main` only and forgetting `release/*` and `hotfix/*`. Hotfix branches are where governance is most often skipped.
- Requesting signed commits before the organization has rolled out signing keys. The rule blocks every push and the team disables it under pressure.

## Escalation Path

- GitHub administrator: Escalate when the request needs an organization-level ruleset spanning multiple repositories, or when bypass policy needs to be set at organization level.
- Repository administrator: Primary owner of repository-scoped ruleset implementation and verification.
- Engineering lead: Approves required reviewer count, code-owner scope, and signed-commit policy for the team.
- Release manager: Approves rules that affect `release/*`, `hotfix/*`, and version tag patterns.

## Related Guides

- GHE-ALM-074 : How to Review Ruleset and Branch Protection Coverage
- GHE-ALM-075 : How to Request or Review CODEOWNERS-Based Review Routing
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-068 : How to Request Environment Protection Rules
- GHE-ALM-077 : How to Enforce Naming Conventions
