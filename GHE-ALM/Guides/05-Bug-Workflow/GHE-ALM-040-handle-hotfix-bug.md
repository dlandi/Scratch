# How to Handle a Hotfix Bug

**Guide ID:** GHE-ALM-040
**Audience:** Engineering Manager, Release Manager, QA Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 20-30 minutes to file and route the hotfix; 10-15 minutes to verify ALM traceability after deployment
**Required permissions:** Repository: Triage (to file and label the bug); Project: Write (to set Severity, Priority, Sprint, Release, milestone); Repository: Write or Admin if you also create the hotfix milestone or tag

**Prerequisites:**

- Production defect has been confirmed reproducible or has confirmed customer impact.
- You know the affected release version, the affected product area, and the customer or environment where the defect surfaced.
- The repository uses semantic-style release tags (for example `v2026.05.0`) or date-style release labels (for example `2026.05.0`).
- A hotfix branching pattern is agreed with engineering, typically `hotfix/<release>-<short-slug>`.

**When to use this guide:** A defect is live in production, business impact is severe enough to bypass the next planned release, and engineering must ship a corrective build out-of-cycle while preserving the audit trail other releases provide.

**When not to use this guide:** The defect can wait for the next planned sprint or release. In that case use GHE-ALM-014 to file the bug and GHE-ALM-038 to attach it to the upcoming release.

## Outcome

By the end of this guide, you will have produced:

- A Bug issue tagged Severity 1 / P0 with explicit hotfix scope and customer impact recorded.
- A hotfix milestone (for example `2026.05.1`) containing the bug and any companion fixes scoped into the same hotfix.
- A Release field value on the Bug item pointing at the hotfix release.
- Verified traceability from Bug to hotfix branch, pull request, GitHub Release, and deployment.

## Before You Start

- Confirm with on-call or the reporting customer team that the defect is in production and reproducible.
- Identify which release tag the defect first shipped in. You will branch the hotfix from that tag, not from `main`.
- Decide who will own the post-mortem before you triage; hotfixes without a named owner tend to lose their post-mortem entirely.
- Have the canonical Severity and Priority scale available for reference (illustrative below; confirm your team's actual scale with QA leadership):

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

## Steps

### File the hotfix bug

1. Open the affected repository, click **Issues**, then click **New issue** and choose the Bug Report form.
2. Title the issue with the customer-visible symptom and the affected release, for example `Checkout coupon code returns 500 in 2026.05.0`. Avoid internal jargon in the title; the title shows up in release notes.
3. In the Bug Report body, fill **Steps to reproduce**, **Expected**, **Actual**, **Environment**, and **First seen in release**. Attach logs, request IDs, and a screenshot of the failure if available. Evidence handling rules from GHE-ALM-037 still apply.
4. In the right sidebar set **Type** to `Bug`, **Severity** to `1`, **Priority** to `P0`, and **Product Area** to the affected area (for example `Checkout`). Set **Customer Impact** to the user-facing description ("Coupon redemption failing for all users on `acme-checkout` web client").
5. Add the labels `hotfix-candidate` and `production` so the Bug Triage view (GHE-ALM-034) surfaces it immediately.

> [SCREENSHOT: New Bug Report form with Severity 1, Priority P0, Customer Impact, and hotfix-candidate label visible]

### Create or attach the hotfix milestone

6. Click **Milestones** in the repository's Issues tab. If a hotfix milestone for the affected release already exists (for example `2026.05.1`), open it. Otherwise click **New milestone**.
7. Name the milestone using your team's hotfix convention. Use `vX.Y.Z+1` style (for example `v2.7.4` if production is on `v2.7.3`) or date-style `YYYY.MM.N` (for example `2026.05.1` if production is on `2026.05.0`). Pick one convention and use it consistently across hotfixes for the same release line.
8. Set the milestone **Due date** to the planned hotfix ship date, typically within 24 to 72 hours. Add a one-line description naming the triggering Bug ID (for example "Hotfix for #4821: coupon 500 in 2026.05.0").
9. Return to the Bug issue and set **Milestone** to the hotfix milestone.

### Set the Release field and route to engineering

10. Open the ALM Project that tracks this product. Find the Bug item in the project (it should appear automatically if auto-add workflows from GHE-ALM-009 are configured; if not, add it via **Add item**).
11. Set the **Release** field to the hotfix release value (for example `2026.05.1`). Set **Sprint** to `@current` only if engineering will absorb the work into the live sprint; otherwise leave Sprint empty so sprint metrics stay clean.
12. Set **Status** to `Ready` and assign the engineer who will implement the fix. Tag the release manager and the named post-mortem owner in a comment so both are notified before work begins.
13. Confirm the engineer creates the branch from the affected release tag, not from `main`. The expected branch pattern is `hotfix/<release>-<short-slug>`, for example `hotfix/2026.05.1-coupon-fix`. Branch traceability from GHE-ALM-059 still applies.

> [SCREENSHOT: Project item sidebar showing Release set to 2026.05.1, Status Ready, hotfix milestone, Severity 1, Priority P0]

### Verify the pull request and release link

14. When the pull request opens, confirm the PR description contains a closing keyword such as `Closes #4821` linking back to the Bug. This is the same traceability check GHE-ALM-060 covers; for hotfixes it is non-negotiable.
15. Confirm the PR targets the hotfix branch's eventual merge target (typically the release branch matching the affected version, for example `release/2026.05`) and not `main`. Confirm required reviews and status checks pass per the rulesets reviewed in GHE-ALM-062.
16. After merge, confirm the engineering owner cherry-picks or forward-merges the fix into `main` so the defect does not reappear in the next planned release. Open a tracking sub-issue if the forward-merge is deferred.

### Verify release traceability after ship

17. Open the repository's **Releases** page. Confirm a draft or published release exists for the hotfix tag (for example `v2026.05.1`). Drafting and reviewing the release follows GHE-ALM-047.
18. In the release editor, click **Generate release notes** so the merged hotfix PR and its linked Bug appear in the auto-generated notes. Confirm the Bug title and PR number are present.
19. Confirm the **Target** branch for the tag matches the hotfix's merge branch (for example `release/2026.05`) and that the release is marked **Set as latest release** only if the hotfix supersedes the prior production version. If a newer minor release is already out, leave the latest designation alone and consider marking the hotfix as a pre-release for the older line per your release policy.
20. After deployment, open the protected environment's deployment history (covered by GHE-ALM-067) and confirm the hotfix tag was the deployed ref. Move the Bug item's **Status** to `Verified` once production validation is complete, then `Done` once the post-mortem is filed.

> [SCREENSHOT: Releases page showing hotfix release with auto-generated notes listing the Bug and PR]

## Validation Checklist

- [ ] Bug issue exists with Severity 1, Priority P0, Customer Impact populated, and `hotfix-candidate` label.
- [ ] Hotfix milestone exists with a name matching team convention and a due date within the agreed hotfix window.
- [ ] Project item shows Release field set to the hotfix release and an assigned owner.
- [ ] Pull request links to the Bug via `Closes #NNNN`, targets the correct release branch, and passed required checks.
- [ ] Forward-merge into `main` is complete or tracked in a sub-issue.
- [ ] GitHub Release for the hotfix tag exists, auto-generated notes include the Bug and PR, and the Target branch is correct.
- [ ] Deployment history shows the hotfix tag deployed to production; Bug Status is `Verified` then `Done`.
- [ ] Post-mortem owner is named and has a deadline.

## What Good Looks Like vs. What to Escalate

| Area | What good looks like | What to escalate |
|---|---|---|
| Hotfix scope | One Bug per hotfix milestone, plus only the minimum companion fixes required to ship the patch safely. | Scope creep: additional bugs or features merged into the hotfix milestone "while we're shipping anyway". Escalate to release manager and engineering lead before the milestone closes. |
| PR-to-Bug link | PR description contains `Closes #NNNN`, the Bug auto-closes on merge, and the Bug's timeline shows the closing event. | No linked PR, or the PR uses a free-text mention instead of a closing keyword. Escalate to the engineering owner; require the link before the release is drafted. |
| Post-mortem | Named owner, scheduled review within 5 business days of deployment, and a follow-up issue tracking remediation work. | No owner, no scheduled review, or "we already know what happened, no post-mortem needed". Escalate to engineering manager; hotfixes without post-mortems erode the audit trail and repeat. |

## Common Mistakes

- Branching the hotfix from `main` instead of from the affected release tag, which pulls in unreleased changes and forces extra QA.
- Skipping the Release field on the Bug because "the milestone already says it". Dashboards and Project Insights filter on the Release field, not the milestone, so omitting it hides the hotfix from release health views.
- Merging the hotfix into the release branch but forgetting to forward-merge into `main`, causing the defect to recur in the next planned release.
- Reusing the prior release's tag instead of creating a new patch tag (`2026.05.1`), breaking deployment history and rollback capability.
- Treating the hotfix as "done" at merge instead of at verified production deployment plus closed post-mortem.

## Escalation Path

- GitHub administrator: Not applicable for routine hotfixes. Involve only if rulesets are blocking the hotfix PR and the rule itself needs a temporary exception.
- Repository administrator: Involve if the `release/*` or `hotfix/*` branches are not covered by the rulesets reviewed in GHE-ALM-074, or if a tag protection rule prevents creating the patch tag.
- Engineering lead: Involve when scope creep is being pushed into the hotfix milestone, when the forward-merge to `main` is being skipped, or when the PR cannot find a code-owner reviewer in the hotfix window.
- Release manager: Involve to coordinate the hotfix tag, the GitHub Release draft, the deployment approval (GHE-ALM-067), and customer communication.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-038 : How to Associate a Bug with a Release or Sprint
- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-067 : How to Approve a Protected Deployment
