# How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`

**Guide ID:** GHE-ALM-063
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Understands / Manager Reviews
**Estimated time:** 15-20 minutes to learn; 2-3 minutes per pull request to spot-check
**Required permissions:** Repository: Read (to view pull requests, issues, and timelines)
**Prerequisites:**

- You can find a repository's pull requests and linked issues.
- You know which branch is the repository's default branch (typically `main`).
- You have read GHE-ALM-060 on issue-to-PR traceability.

**When to use this guide:** Use this guide when you need to confirm that a merged pull request actually closed the issue it was meant to deliver, or when you are reviewing PR descriptions to make sure work items will close automatically on merge.

**When not to use this guide:** Do not use this guide when you only need to confirm that a PR exists for an issue. That is GHE-ALM-060. For PR review and approval compliance, use GHE-ALM-062.

## Outcome

By the end of this guide, you will be able to:

- Recognize the nine closing keywords GitHub honors.
- State the rule that determines whether auto-close fires.
- Distinguish a closing reference from a plain mention.
- Read a cross-repository closing reference.
- Spot a malformed reference that will silently fail.

## Before You Start

- Pick one merged pull request from the past sprint to use as a worked example.
- Confirm the repository's default branch name in **Settings** > **Branches**, or read it from the repository home page.
- Have one issue open that you expect that PR closed.

## Steps

### 1. The nine closing keywords

GitHub recognizes nine words as closing keywords. They are case-insensitive:

`close`, `closes`, `closed`, `fix`, `fixes`, `fixed`, `resolve`, `resolves`, `resolved`.

All nine behave identically. There is no semantic difference between `Closes #1234` and `Fixes #1234` and `Resolves #1234`. Teams sometimes adopt a convention (for example, `Fixes` for bugs and `Closes` for features) but GitHub itself does not care.

Anything else is not a closing keyword. `Addresses #1234`, `See #1234`, `Refs #1234`, `Related to #1234`, and `Part of #1234` are plain references. They create a visible link in the issue timeline but they will never close the issue automatically.

### 2. The auto-close rule

Auto-close fires only when both of these are true:

1. The closing keyword and issue reference appear in the pull request body, or in a commit included in the pull request.
2. The pull request merges into the repository's default branch.

If a pull request targets any other branch, GitHub ignores the closing keyword entirely. No link is created and merging the PR has no effect on the issue. This is the most common cause of "the PR merged but the issue is still open" confusion. Releases that flow through `develop`, `release/*`, or `hotfix/*` branches before reaching `main` will not close issues at the intermediate merge step.

### 3. Reference syntax

Same-repository reference. Use the keyword followed by `#` and the issue number:

`Fixes #1234`

Cross-repository reference. Use the keyword followed by `owner/repo#issue-number`:

`Closes acme-payments/checkout-service#412`

Multiple issues in one PR. List up to ten closing references in a single PR body. Each one needs its own keyword. The keyword does not distribute across a comma-separated list:

`Resolves #10, resolves #123, resolves acme-payments/checkout-service#412`

`Closes #10, #123, #412` will only close `#10`. The other two are plain mentions.

### 4. Pull request body versus commit message

Closing keywords work in two places: the pull request body and the commit messages of commits included in the pull request.

Pull request body. The most common pattern. The reference appears in the PR description, GitHub displays the linked issue in the PR sidebar under **Development**, and the issue closes when the PR merges into the default branch.

Commit message. If a developer writes `Fixes #1234` in a commit message and that commit reaches the default branch (through a PR merge or a direct push), the issue closes. However, the pull request that carried the commit will not appear as a linked pull request on the issue. The PR sidebar link only comes from the PR body.

The practical implication for managers: always look for the closing keyword in the PR body. A commit-message-only reference closes the issue but breaks the visible PR-to-issue link in the GitHub UI, which makes audit and traceability harder.

### 5. Plain mentions versus closing references

GitHub treats `#1234` as an issue reference whenever it appears in a PR body, commit message, or comment. A plain reference creates a backlink in the issue timeline. It does not close the issue.

Worked example. A pull request body in `acme-payments/checkout-service` reads:

> Adds the new export endpoint.
>
> Fixes #482
> Refs #501
> See acme-payments/payments-api#88

When this PR merges into `main`:

- Issue `#482` in `checkout-service` closes automatically.
- Issue `#501` in `checkout-service` stays open and gains a timeline entry referencing the PR.
- Issue `#88` in `acme-payments/payments-api` stays open and gains a timeline entry referencing the PR.

If the same PR were merged into `release/2026.05.0` instead of `main`, none of those references would create links and `#482` would stay open.

### 6. What to inspect when reviewing a pull request

Open the pull request and check four things:

1. Read the PR body. Look for one of the nine closing keywords followed by a valid issue reference.
2. Look at the **Development** section in the right sidebar. A correctly formatted closing reference produces a linked issue with a "will close" or "linked" indicator.
3. Confirm the PR's base branch. The base branch is shown at the top of the PR ("wants to merge X commits into `main` from `feature/...`"). If the base is not the default branch, auto-close will not fire.
4. After merge, confirm the issue actually closed and the issue timeline shows "closed by pull request #NNN".

> [SCREENSHOT: Pull request page showing the Development sidebar with a linked issue marked to close on merge, and the base branch label visible at the top]

## Validation Checklist

- [ ] You can name all nine closing keywords without checking.
- [ ] You can state the default-branch rule.
- [ ] You can recognize a malformed multi-issue list (`Closes #10, #11, #12`) and rewrite it correctly.
- [ ] You can recognize a cross-repo reference (`Closes acme-payments/checkout-service#412`).
- [ ] You can distinguish `Fixes #1234` from `Refs #1234` and explain the behavior of each.
- [ ] For the worked example PR you picked, you can point to the PR body line that closed the issue, or explain why the issue did not close.

## Common Mistakes

- Treating `Addresses`, `See`, `Refs`, `Related to`, or `Part of` as closing keywords. They are not. They create timeline backlinks only.
- Using one keyword to cover a list: `Closes #10, #11, #12`. Only `#10` closes. The rest are plain mentions.
- Expecting a PR merged into `release/*` or `develop` to close issues. Auto-close requires the default branch.
- Putting the closing keyword in a PR comment after the PR is opened. Closing keywords are read from the PR body and from commit messages, not from comments.
- Cross-repo references that omit the owner: `Closes checkout-service#412`. Without the owner, GitHub treats this as a same-repo reference to a non-existent issue.
- Relying on commit-message closures only. The issue will close, but the PR will not appear as a linked pull request on the issue, which damages traceability.
- Manually closing an issue when the PR is merged, then assuming auto-close worked. The audit trail will not show the PR as the closing event.

## Escalation Path

- GitHub administrator: Not applicable. Closing keyword behavior is built in and cannot be configured.
- Repository administrator: Involve when a PR repeatedly merges into a non-default branch and the team needs to align on whether `main` should remain the default, or when a CODEOWNERS or PR template change is needed to prompt developers for closing keywords.
- Engineering lead: Involve when a release branch workflow is producing systematically orphaned issues. The fix is usually a PR template that prompts for `Closes #` plus a reminder to open the closing PR against `main` after the release branch lands.
- Release manager: Involve when release notes generated from merged PRs are missing issue context. Missing closing keywords are a common cause.

## Related Guides

- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-061 : How to Interpret Pull Request Status for Managers
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-064 : How to Use Issue and PR Timeline Events for Audit Trail
