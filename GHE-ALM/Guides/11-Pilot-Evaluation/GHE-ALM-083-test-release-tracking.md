# How to Test Release Tracking

**Guide ID:** GHE-ALM-083
**Audience:** Release Manager, Engineering Manager, Project Manager
**Primary role:** Release Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 60-90 minutes during the pilot release window
**Required permissions:** Repository: Triage or Write; Project: Write; Releases: permission to publish a release in the pilot repository
**Prerequisites:**

- Pilot Project from GHE-ALM-079 with the `Release` field, `Status` field, and `Sprint` field already configured.
- One pilot repository with merged pull requests for the candidate release, for example `acme-checkout/checkout-service`.
- One repository milestone, for example `2026.05.0`, attached to the pilot release scope.
- A deployment workflow in GitHub Actions that publishes to at least one environment, even if the environment is `staging` only.

**When to use this guide:** Use during the pilot evaluation when the team needs evidence that GitHub can track a release end to end, from issues through pull requests, milestone completion, release notes, the published GitHub Release, and the deployment record. This guide is the release-tracking test for pilot scenario 4.

**When not to use this guide:** Do not use this guide for routine release execution after the pilot has concluded. After adoption, run the production release flow described in GHE-ALM-046 and GHE-ALM-047.

## Outcome

By the end of this guide, you will have produced:

- A release-tracking test record showing each pass criterion as pass or fail.
- A milestone with completion percentage, linked issues, and linked pull requests visible.
- A published GitHub Release with a tag, generated or reviewed release notes, and a deployment entry tied to the same tag.
- A short note for the pilot scorecard (GHE-ALM-085) summarizing whether release governance is sufficient in GitHub.

## Before You Start

- Confirm the pilot release candidate name, for example `2026.05.0`, and the matching milestone.
- Confirm that at least three pilot issues have been completed, linked to merged pull requests via closing keywords such as `Closes #1234`, and assigned to the milestone or the `Release` field.
- Confirm that the deployment workflow has run at least once for the candidate tag, or is ready to run when the release is published.
- Open a tab on the pilot Project, the pilot repository **Issues** tab, the **Pull requests** tab, the **Milestones** page, and the **Releases** page.

## Steps

### Set up the test record

1. Open the pilot Project and create a saved view called `Release Test 2026.05.0`. Filter by `release:2026.05.0` or by milestone, group by `Status`, and show columns for issue type, linked pull requests, `Sprint`, and `Release`.
2. In a notes file or pilot scorecard row, record the candidate release name, the milestone, the tag you intend to publish, and the date of the test.

> [SCREENSHOT: Pilot Project view filtered to the candidate release, grouped by Status, showing linked pull request counts]

### Verify scope and traceability

3. In the saved view, confirm every issue in scope has either the `Release` field set to the candidate release or the milestone assigned. Mark this as pass or fail in the test record.
4. For each completed issue, open it and check the **Development** sidebar or linked pull requests block. Confirm the closing keyword pattern, for example `Closes #1234`, appears on the merged pull request. Cross-reference GHE-ALM-060 if any link is missing.
5. Open the milestone page in the repository, for example `acme-checkout/checkout-service/milestones/2026.05.0`. Record the open count, closed count, and percent complete. Mark "Release progress visible" as pass if the bar reflects reality.

### Publish the GitHub Release

6. From the repository, open **Releases** and click **Draft a new release**.
7. In **Choose a tag**, type the version, for example `v2026.05.0`, and click **Create new tag** on publish. In **Target**, choose the release branch, for example `release/2026.05.0` or `main`.
8. Enter the release title, for example `2026.05.0`. Click **Generate release notes** to populate notes from merged pull requests. Review the notes for completeness; remove or edit entries as needed.
9. Decide pre-release versus latest. For a normal pilot, leave **Set as the latest release** checked. For a release candidate, check **Set as a pre-release**.
10. Click **Publish release**. Confirm the tag now appears on the **Tags** tab and on the milestone page.

> [SCREENSHOT: Draft release page with tag, target branch, generated release notes, and the Publish release button visible]

### Verify deployment traceability

11. Open the **Actions** tab and locate the deployment workflow run triggered by the new tag, or trigger it manually if the workflow uses **workflow_dispatch**. Confirm the run references the tag from step 10.
12. Open the repository **Environments** page or the **Deployments** sidebar on the repository home. Confirm the deployment record shows the tag, the environment, the actor, and the deployment status.
13. Return to the milestone and close it once all linked issues are closed and the release is published. The milestone should now show 100 percent complete.

> [SCREENSHOT: Environments page showing the deployment entry for the published tag with status and timestamp]

### Record review evidence

14. In the test record, mark each pass criterion: `Release` field or milestone assigned, linked pull requests visible, release progress visible, Git tag created, GitHub Release published, deployment workflow visible. Capture the screenshots above for the pilot scorecard.
15. Write a one-paragraph reviewer note describing whether the release was traceable end to end without external tools, and list any gaps that would force a workaround. File this with the GHE-ALM-085 evidence pack.

## Validation Checklist

- [ ] Every in-scope issue has the `Release` field set or the milestone assigned.
- [ ] Each completed issue links to a merged pull request through a closing keyword.
- [ ] Milestone progress bar reflects the actual closed-versus-open count.
- [ ] Git tag for the candidate release exists on the **Tags** tab.
- [ ] GitHub Release is published with title, notes, and pre-release or latest designation.
- [ ] Deployment record for the tag exists in **Environments** or **Deployments**.
- [ ] Pass or fail is recorded for all six pass criteria.

## Common Mistakes

- Publishing the release before the milestone is updated, leaving milestone progress stale.
- Generating release notes from a target branch that does not contain the merged scope, producing an empty or wrong list.
- Using a tag name that does not match the deployment workflow trigger, breaking the link between the release and the deployment.
- Treating closed issues without linked pull requests as in-scope traceable items; a closing comment alone is not the same as a `Closes #NNN` reference on the merged pull request.
- Marking the pilot pass when only one of milestone or `Release` field is used and the team has not chosen which one is canonical.

## Escalation Path

- GitHub administrator: when organization-level release or tag policy blocks publishing.
- Repository administrator: when the release branch is protected and you cannot publish a tag, or when the deployment environment requires a reviewer you do not have.
- Engineering lead: when linked pull requests are missing closing keywords and the team needs to decide whether to retrofit the links.
- Release manager: accountable for the pass/fail decision on this scenario and for filing the evidence in GHE-ALM-085.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-079 : How to Run the GitHub Enterprise ALM Pilot Evaluation
- GHE-ALM-085 : How to Record Pilot Pass/Fail Evidence
