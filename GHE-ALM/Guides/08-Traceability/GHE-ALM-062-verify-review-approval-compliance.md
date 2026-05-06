# How to Verify Review and Approval Compliance

**Guide ID:** GHE-ALM-062
**Audience:** Engineering Manager, Release Manager, Program Manager
**Primary role:** Engineering Manager
**Classification:** Manager Reviews
**Estimated time:** 10-15 minutes per pull request, or 30-45 minutes for a release-scope spot-check
**Required permissions:** Repository: Read (Triage helps for viewing protected-branch settings; Admin or Maintain is required to read the audit log)
**Prerequisites:**

- Rulesets or branch protection are configured on the target branch (typically `main`, `release/*`, or `hotfix/*`).
- A `CODEOWNERS` file exists for the repository if your governance model requires code-owner review.
- You can list the pull requests in scope for the release (milestone, Release field, or merged-PR list).

**When to use this guide:** Use this guide before a release sign-off, during a quarterly governance audit, or when investigating a regression to confirm that the merged change went through required review. It is the inspection counterpart to GHE-ALM-073, where rules are requested, and GHE-ALM-074, where coverage is verified.

**When not to use this guide:** Do not use this guide to perform a code review. The manager checks that governance was satisfied, not that the code is correct. If the rules themselves are missing or wrong, escalate to the repository administrator instead of trying to compensate at review time.

## Outcome

By the end of this guide, you will have produced:

- A pass or fail compliance verdict for each pull request you spot-checked.
- A short list of any pull requests where required reviews, code-owner approval, or required status checks were missing, bypassed, or overridden.
- A clear next step: proceed with release, hold the release, or escalate to the repository administrator.

## Before You Start

- Identify the protected branch the PRs were merged into (`main`, `release/2026.05.0`, `hotfix/2026.05.1`).
- Confirm which rules apply to that branch. Open **Settings > Rules > Rulesets** (or **Settings > Branches** for legacy branch protection) and note the required approval count, whether code-owner review is required, and which status checks are required.
- Pull the list of merged PRs you intend to spot-check. For a release, filter the **Pull requests** tab by `is:merged base:main milestone:"2026.05.0"` or by the Release field on your project.
- If you intend to verify that no force-push or admin override occurred, request audit-log access or ask a repository administrator to run the audit query for you.

## Steps

### Confirm the rules in force

1. Open the repository in GitHub. Go to **Settings > Rules > Rulesets**.
2. Open the active ruleset that targets the branch in question (for example, the ruleset whose target is `main` or `release/*`).
3. Note these values for use during the spot-check:
   - **Require a pull request before merging** is enabled.
   - **Required approvals** count (commonly 1 or 2).
   - **Require review from Code Owners** is enabled if your model uses CODEOWNERS.
   - **Require status checks to pass** is enabled, and which checks are listed as required (for example, `build`, `unit-tests`, `lint`).
   - **Block force pushes** is enabled.
   - **Bypass list** is empty, or includes only roles you expect (for example, a release-automation app).

> [SCREENSHOT: Ruleset detail page for the protected branch, with required approvals, code-owner review, required status checks, and bypass list visible.]

### Spot-check pull requests in the release scope

4. From the repository, open **Pull requests** and filter to the merged PRs in scope. Pick a representative sample: every PR for a small release, or 5 to 10 PRs spread across the largest contributors and the most sensitive paths for a larger release.
5. For each sampled PR, open the PR page and confirm the following on the **Conversation** tab:
   - The PR is **Merged** (purple badge), not just closed.
   - The reviewer summary near the bottom shows the required approval count satisfied. The expected text is similar to "Changes approved" with the required reviewer names checked.
   - If code-owner review is required, the **Reviewers** sidebar shows the code-owner team or user with a green check next to their name. A pending code-owner request displayed as a yellow dot is a fail signal for a merged PR; investigate how it merged.
   - The **Checks** section shows all required checks with a green check. A required check missing or marked as skipped is a fail signal.
   - The linked issue appears under **Development** (or in the PR body via `Closes #NNNN`). This ties back to GHE-ALM-060 and GHE-ALM-063.
6. Open the PR's **Files changed** tab only to confirm whether the changed paths intersect a `CODEOWNERS` rule. You do not need to read the code. If a sensitive path was changed and no code owner approved, that is a fail signal.

> [SCREENSHOT: A merged pull request showing the green merge badge, the required approvals satisfied, the code-owner team approved, and all required checks passing.]

### Confirm no governance bypass occurred

7. On each sampled PR, scroll the timeline for entries such as "merged with bypass," "administrator merged," or any indication that a required review was dismissed and not re-requested. These entries are uncommon and worth flagging.
8. If you have audit-log access, open **Settings > Audit log** (organization or enterprise level depending on your role) and filter for the repository and the time window of the release. Look for events of these types:
   - `repo.override_required_status_check`
   - `protected_branch.policy_override`
   - `pull_request.bypass`
   - `git.push` against a protected branch from an unexpected actor.
   If you do not have audit-log access, ask a repository administrator to run this query and send you the result. Record the answer in your release-readiness notes.

### Record the verdict

9. For each sampled PR, mark pass or fail. A PR passes only when the merge badge is present, required approvals are satisfied, code-owner approval is present where the path requires it, all required checks are green, and no bypass appears in the timeline or audit log.
10. Summarize the spot-check in your release-readiness notes: number of PRs sampled, number of fails, list of failing PR numbers with one-line reasons, and your recommendation (proceed, hold, or escalate).

## What Good Looks Like vs. What to Escalate

| Compliance Signal | What Good Looks Like | What to Escalate |
|---|---|---|
| PR state | Purple **Merged** badge with merge commit visible. | PR shows **Closed** without a merge, or a force-merge commit appears on the protected branch outside the PR. |
| Required approvals | Reviewer summary shows the configured count satisfied with green checks. | Reviewer summary says "Review required" or shows fewer approvals than the ruleset requires for a merged PR. |
| Code-owner review | CODEOWNERS team or user listed in **Reviewers** sidebar with a green check, when changed paths match a `CODEOWNERS` entry. | Code-owner request shows a yellow dot (pending) or red X (changes requested) on a merged PR, or no code-owner was requested for a sensitive path. |
| Required status checks | All checks listed as required in the ruleset show green in the PR **Checks** section. | A required check is missing, skipped, neutral, or red on a merged PR. |
| Force-push and protected-branch integrity | No `git.push --force` events against the protected branch in the audit log; **Block force pushes** enabled in the ruleset. | Force-push events appear, or **Block force pushes** is disabled, or a non-PR commit landed directly on the protected branch. |
| Bypass list usage | Bypass list is empty or limited to expected automation accounts; no bypass entries on the PR timeline. | Human accounts appear in the bypass list, or PR timeline shows "merged with bypass" by an individual. |

## Validation Checklist

- [ ] The active ruleset for the protected branch is identified and its required approvals, code-owner setting, and required status checks are recorded.
- [ ] Each sampled merged PR shows the **Merged** badge, the required approval count satisfied, and all required checks green.
- [ ] Code-owner approval is present on every sampled PR whose changed paths match a `CODEOWNERS` rule.
- [ ] No bypass entry, dismissed-and-not-re-requested review, or unexpected administrator merge appears on the sampled PRs.
- [ ] The audit log (or an administrator's confirmation in lieu of access) shows no force-push or policy-override events against the protected branch in the release window.
- [ ] The spot-check verdict (proceed, hold, or escalate) is recorded in release-readiness notes with the sampled PR list.

## Common Mistakes

- Assuming a green merge button means governance was enforced. The merge button reflects the rules in effect at merge time; if the rules changed after merge, the green badge is not retroactive evidence. Always check the ruleset history or the audit log for changes during the release window.
- Treating a single approval as sufficient when the ruleset requires two. Read the reviewer summary count; do not count green checks visually.
- Skipping code-owner verification because no code owners were listed on the PR sidebar. The absence of a code owner on a sensitive path is itself the finding to escalate, not a reason to pass the PR.
- Confusing optional checks with required checks. Only the checks named in the ruleset count toward compliance. A green CI run that is not in the required list does not make a missing required check acceptable.
- Spot-checking only the smallest PRs. Bias the sample toward the largest, the most sensitive paths, and PRs from less-frequent contributors.
- Verifying compliance by reading code. The manager confirms that the governance process ran. If code quality is in doubt, raise it with the engineering lead through normal channels.

## Escalation Path

- GitHub administrator: When the audit log shows policy overrides, force-push events against a protected branch, or unexpected entries in a bypass list.
- Repository administrator: When the ruleset is missing a required approval count, code-owner requirement, or required status check that your governance model expects, or when a `CODEOWNERS` file is missing for sensitive paths.
- Engineering lead: When a code-owner request was pending or rejected on a merged PR, or when required reviewers were dismissed without re-request.
- Release manager: When one or more sampled PRs fail compliance and the release decision needs to weigh hold versus proceed-with-exception.

## Related Guides

- GHE-ALM-061 : How to Interpret Pull Request Status for Managers
- GHE-ALM-063 : How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`
- GHE-ALM-073 : How to Request Rulesets or Branch Protection
- GHE-ALM-074 : How to Review Ruleset and Branch Protection Coverage
- GHE-ALM-075 : How to Request or Review CODEOWNERS-Based Review Routing
