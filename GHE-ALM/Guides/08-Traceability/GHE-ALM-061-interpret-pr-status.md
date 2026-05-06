# How to Interpret Pull Request Status for Managers

**Guide ID:** GHE-ALM-061
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Reviews
**Estimated time:** 5-10 minutes per pull request
**Required permissions:** Repository: Read

**Prerequisites:**

- The repository containing the pull request is visible to you.
- You know the issue number, branch name, or PR number you want to inspect.
- You have completed GHE-ALM-060 so you can locate the PR linked to an issue.

**When to use this guide:** Use this guide when you need to know whether a unit of work is waiting on review, blocked by failing checks, ready to merge, already merged, or deployed, and you do not need to read the code itself.

**When not to use this guide:** Do not use this guide to assess code quality, perform security review, or approve a PR on behalf of an engineer. Code-level review is the engineer's job.

## Outcome

By the end of this guide, you will have produced:

- A clear status read for a single pull request: waiting, blocked, approved, merged, or deployed.
- A short escalation note (if needed) routed to the right role.

## Before You Start

- The PR URL or issue link from the work item you are tracking.
- The team's expected service level for PR review (for example, 2 business days).
- The required reviewer and required check policy for the target branch, if you have it. GHE-ALM-062 covers how to confirm that policy.

## Steps

### Open the pull request

1. From the issue, click the linked PR in the **Development** sidebar. From a Project view, click the PR row. From a search, paste the PR URL such as `https://github.example.com/acme-payments/checkout-service/pull/482`.
2. Confirm you are on the **Conversation** tab. This is the only tab a manager normally needs.

> [SCREENSHOT: PR Conversation tab with the colored state badge, branch source/target row, merge status box, checks list, reviewers sidebar, and Linked issues sidebar all visible.]

### Read the header signals

3. Read the **state badge** at the top of the page. It is one of four values:
   - **Open** (green): work in progress, not yet merged.
   - **Draft** (gray): author marks this PR as not ready for review. Code owners are not auto-requested. The PR cannot be merged in this state.
   - **Merged** (purple): the branch was merged into the target branch. Work is complete from a code-integration standpoint.
   - **Closed** (red): the PR was closed without merging. The work in this branch was abandoned or superseded.
4. Read the **branch line** under the title, formatted as `wants to merge N commits into <target> from <source>`. Confirm the target branch matches your expectation, for example `main` for trunk-based work, `release/2026.05.0` for a release branch, or `hotfix/*` for a production fix.
5. Read the **title and description**. The title should match the issue summary. The description should reference the issue with a closing keyword such as `Closes #1234`. GHE-ALM-063 covers closing keywords in detail.

### Read the merge status box

6. Locate the **merge status box** near the bottom of the Conversation tab. The box header summarizes mergeability:
   - **This branch has no conflicts with the base branch**: clean to merge from a code standpoint.
   - **This branch has conflicts that must be resolved**: the author or another engineer must rebase or merge the target branch back in. This is an engineering action, not a manager action.
   - **Review required** / **Changes requested** / **Required statuses must pass**: the box lists the specific gates that are not yet satisfied.
7. Count the **required reviews**. The box shows text such as `1 approving review by reviewers with write access is required` or `2 of 2 approving reviews`. If a code-owner review is required, it is listed separately.
8. Read the **Merge button** state. A green **Merge pull request** button means all required gates are satisfied. A grayed-out button means at least one gate is still open.

### Read the checks list

9. Below the commits list, find the **checks summary**. Each check is one of:
   - Green check: the check passed.
   - Red X: the check failed. Failed required checks block merge.
   - Yellow dot or spinner: the check is queued or in progress.
   - Skipped or neutral: the check did not run for this change.
10. Open the **Checks** tab only if you need to know which check is red. The check name (for example `build`, `unit-tests`, `lint`, `security-scan`) tells you which engineering function owns the failure. You do not need to read the log output.

### Read the reviewers and linked issues sidebars

11. In the right sidebar, read **Reviewers**. Each reviewer shows a status: requested (no symbol), approved (green check), changes requested (red X), or commented (speech bubble, not blocking). If a required reviewer or code owner is missing, the reviewer line will say `Review required`.
12. Read **Assignees** to confirm the PR has a named author or owner who can act on feedback.
13. Read **Linked issues** in the sidebar. The issues listed here will close automatically on merge if the PR body uses a closing keyword such as `Closes #1234`. An empty Linked issues sidebar is a traceability gap; see GHE-ALM-060 to remediate.

### Read the deployment block (if shown)

14. If GitHub Actions deployments are configured, a **Deployments** block appears in the Conversation timeline showing entries such as `deployed to staging` or `deployed to production` with timestamps and a link to the environment. The block tells you whether merged code has reached an environment yet. Pre-merge, this block usually shows preview or staging deployments only. GHE-ALM-066 covers how to inspect deployment history more thoroughly.

> [SCREENSHOT: Deployments block in the PR timeline showing a successful staging deployment and a pending production deployment.]

### Decide the PR's true status

15. Combine the signals into one of five status reads:
   - **Waiting on review**: Open, no conflicts, checks green, reviewers requested but not yet acted.
   - **Waiting on author**: Open, changes requested, or red checks the author must fix.
   - **Blocked**: Open, conflicts present, or required reviewer unavailable, or failing required check the author cannot resolve alone.
   - **Approved, ready to merge**: Open, all gates green, Merge button enabled, awaiting click by author or merge queue.
   - **Merged**: Merged badge present. Inspect Deployments block to see whether it has reached staging or production.

## What Good Looks Like vs. What to Escalate

| Signal | What good looks like | What to escalate, and to whom |
|---|---|---|
| PR age (Open) | Under the team's stated review SLA, for example 2 business days. | PR open more than 5 business days with no review activity. Escalate to the engineering manager or scrum master. |
| Status checks | All required checks green; failures only on optional or informational checks. | A required check has been red for more than 1 business day with no commit activity. Escalate to the engineering lead owning that check. |
| Merge conflicts | "No conflicts with the base branch." | Conflicts persisting more than 2 business days with no rebase activity. Escalate to the author and engineering lead. |
| Linked issue | At least one issue listed in **Linked issues**, ideally with a closing keyword such as `Closes #1234`. | No linked issue, or PR title references a ticket from another tracker. Escalate to the author to add the link, then verify with GHE-ALM-060. |
| Required reviewers | All required reviewers and code owners listed and acting within SLA. | Required reviewer never assigned, or reviewer out of office with no delegate. Escalate to the engineering manager to reassign. |
| Draft state | Author flips Draft to Ready when work is feedback-ready. | PR sits in Draft past sprint end with no commits. Escalate to scrum master for sprint hygiene. |
| Deployments block | Merged PRs reach staging within the team's deployment cadence. | Merged more than 1 sprint ago and never deployed. Escalate to the release manager. |

## Validation Checklist

- [ ] You can name the PR's state: Open, Draft, Merged, or Closed.
- [ ] You can name the target branch and confirm it matches expectations.
- [ ] You can state whether all required reviews are satisfied.
- [ ] You can state whether all required checks are green.
- [ ] You can name the linked issue, or note that none is linked.
- [ ] You have classified the PR as waiting, blocked, approved, merged, or deployed.

## Common Mistakes

- Reading the **Files changed** tab and trying to judge the code. That is the engineer's job. Stay on **Conversation**.
- Treating a yellow or spinning check as a failure. Yellow means in progress; wait for it to settle before escalating.
- Treating a Draft PR as overdue review. Draft means the author has not asked for review yet.
- Assuming a merged PR is deployed. Merge is code integration; deployment is a separate step visible in the Deployments block or via GHE-ALM-066.
- Escalating a non-required failed check. Read the check name and check whether the team's policy lists it as required for the target branch (see GHE-ALM-062).
- Counting comment reviews as approvals. Only the green check approval counts toward required reviews.

## Escalation Path

- GitHub administrator: Not applicable for routine PR status reads. Involve only when the PR page itself is unreachable or shows a permission error.
- Repository administrator: When required reviewer or required check policy seems wrong for the target branch. Cross-reference with GHE-ALM-073.
- Engineering lead: When a required check is red and the author has not acted, or when conflicts persist past the SLA.
- Release manager: When a merged PR has not reached the expected environment, or when a release-branch PR is at risk of slipping the release date.

## Related Guides

- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-063 : How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`
- GHE-ALM-064 : How to Use Issue and PR Timeline Events for Audit Trail
- GHE-ALM-066 : How to Review Deployment History
