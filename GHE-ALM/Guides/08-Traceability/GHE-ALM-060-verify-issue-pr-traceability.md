# How to Verify Issue-to-Pull-Request Traceability

**Guide ID:** GHE-ALM-060
**Audience:** Project Manager, Engineering Manager, Release Manager
**Primary role:** Release Manager
**Classification:** Manager Reviews
**Estimated time:** 5 minutes per issue; 30 to 60 minutes for a release readiness review covering 20 to 50 issues
**Required permissions:** Repository: Read on each repository in scope; Project: Read on the release-tracking Project
**Prerequisites:**

- A defined scope of work to review, such as a release milestone, a sprint, or a Project view filtered to the release
- Familiarity with how your team uses issues, pull requests, and milestones
- Access to the repositories that hold both the issues and the implementing pull requests

**When to use this guide:** Use this when you need to confirm that work items in scope have visible implementation evidence, for example before a release sign-off, sprint review, audit, or compliance check.

**When not to use this guide:** Do not use this guide to assess code quality, review approval status, or build and deployment status. Those are covered by GHE-ALM-061 and GHE-ALM-062. This guide only checks that the link between an issue and its pull request exists and is correctly formed.

## Outcome

By the end of this guide, you will have:

- Confirmed that each issue in scope has at least one linked pull request, or has a documented non-code disposition such as "documentation only" or "won't fix"
- Identified issues with weak or missing traceability and recorded them for follow-up with the engineering lead
- Captured review evidence (a list, a screenshot, or an export) that can be attached to a release readiness record

## Before You Start

- Open the Project, milestone, or label query that defines the scope of issues you will review
- Have a place to record gaps: a spreadsheet, a Project field such as "Traceability Verified", or a checklist in your release ticket
- Confirm which branch the team treats as the default branch (usually `main`); auto-close behavior depends on this

## Steps

1. Open the Project, milestone, or search query that lists the issues in scope. For a release review, a Project view filtered by the release iteration or milestone is the most efficient starting point.

2. Pick the first issue and open it. In the right sidebar, locate the **Development** section. This section lists branches and pull requests that are linked to the issue, either through closing keywords in a pull request description or through manual linking.

   > [SCREENSHOT: An open issue showing the Development section in the right sidebar with one or more linked pull requests listed underneath]

3. Read each linked pull request entry under **Development**. Note its state: open, merged, or closed without merging. A merged pull request that targets the default branch is the strongest signal of completed implementation.

4. Click into the linked pull request. In the right sidebar of the pull request, find the **Linked issues** section. Confirm that the same issue appears there. The link should be visible from both directions.

5. Read the pull request description. Look for closing keywords on their own line in the form `Closes #1234`, `Fixes #1234`, or `Resolves #1234`. The full set of recognized keywords is `close`, `closes`, `closed`, `fix`, `fixes`, `fixed`, `resolve`, `resolves`, and `resolved`. These trigger automatic closure of the linked issue when the pull request merges into the default branch.

6. If the pull request lives in a different repository from the issue, the closing keyword uses cross-repository syntax such as `Closes owner/other-repo#1234`. Confirm the syntax is correct; a typo silently breaks the auto-close.

7. Distinguish a closing-keyword link from a plain mention. A line such as `See #1234` or `Refs #1234` creates a reference in the issue timeline but does not auto-close the issue and does not appear in the **Linked issues** sidebar unless someone added the link manually. Treat these as informational, not as completion evidence.

8. If the pull request was merged but the issue is still open, check whether the merge targeted the default branch. Closing keywords on pull requests that target other branches, such as a long-lived release branch, are ignored by the auto-close. The team must close the issue manually or re-merge through the default branch.

9. Record the result for the issue in your tracking sheet or Project field. Use a simple status such as Verified, Gap, or Non-code disposition.

10. Repeat for each issue in scope. For a batch review, use a Project board view filtered to the release scope and add a column or field named **Traceability Verified** so you can mark each row as you go.

   > [SCREENSHOT: A Project view filtered to a release milestone with a Traceability Verified column showing a mix of Verified and Gap values]

## Validation Checklist

- [ ] Every in-scope issue has at least one linked merged pull request, or a recorded non-code disposition
- [ ] Every linked pull request appears in the issue's **Development** section and the issue appears in the pull request's **Linked issues** section
- [ ] Closing keywords are spelled from the supported list and use correct same-repo or cross-repo syntax
- [ ] Any pull request merged into a non-default branch has a documented plan for closing its linked issues
- [ ] Gaps are recorded with the issue number, the pull request number if any, and a one-line description of the problem

## What Good Traceability Looks Like vs. What to Escalate

| Signal | Good | Escalate |
|---|---|---|
| Issue's **Development** section | Lists one or more pull requests, at least one merged into the default branch | Empty for an issue marked Done; escalate to the engineering lead |
| Pull request's **Linked issues** sidebar | Shows the originating issue | Empty, or links an unrelated tracking issue; ask the author to add the closing keyword and re-link |
| Closing keyword in PR description | A line such as `Closes #1234` or `Fixes owner/repo#1234` | The PR mentions the issue without a keyword (for example `See #1234`); ask the author to amend the description before merge |
| Issue state after PR merge | Closed automatically by the merge | PR merged days ago, issue still open; check whether the merge targeted the default branch |
| Cross-repo links | `Closes owner/repo#NNN` resolves to a real issue when clicked | Link returns 404 or points to the wrong repo; flag as a typo |
| Coverage of release scope | Every release-scoped issue has a verified link or a recorded disposition | Several issues with no linked PR and no disposition; do not sign off the release until resolved |

## Common Mistakes

- Treating a plain `#1234` mention as a completion link. Only the supported closing keywords trigger auto-close and surface in the **Linked issues** sidebar through the PR description.
- Assuming a merged PR always closes its issue. Auto-close only fires when the PR merges into the default branch.
- Reviewing only the issue side. The pull request sidebar can show links the issue side does not, especially when the PR was linked manually rather than through a keyword.
- Skipping cross-repo checks. A misspelled `owner/repo#NNN` looks fine in the PR description but never creates a link.
- Counting an open issue as Done because someone wrote "fixed in #5678" in a comment. Comments do not create traceability links.

## Escalation Path

- GitHub administrator: Not applicable for individual review findings. Involve only if the **Development** or **Linked issues** sections are missing across the entire enterprise, which would indicate a platform configuration problem.
- Repository administrator: Involve when many pull requests in the same repository are missing closing keywords, suggesting the repository needs a pull request template that prompts authors to add them.
- Engineering lead: Involve for any individual gap. The lead can ask the PR author to amend the description, add a manual link, or document a non-code disposition on the issue.
- Release manager: Involve when gaps prevent a release sign-off. The release manager owns the decision to proceed, defer, or carve out specific issues from the release scope.

## Related Guides

- GHE-ALM-061 : How to Interpret Pull Request Status for Managers
- GHE-ALM-062 : How to Verify Review and Approval Compliance
- GHE-ALM-063 : How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`
- GHE-ALM-064 : How to Use Issue and PR Timeline Events for Audit Trail
