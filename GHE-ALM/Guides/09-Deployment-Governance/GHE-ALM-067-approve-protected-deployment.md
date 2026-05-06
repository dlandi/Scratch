# How to Approve a Protected Deployment

**Guide ID:** GHE-ALM-067
**Audience:** Release Manager, Engineering Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 5-10 minutes per approval
**Required permissions:** Listed as a required reviewer on the target environment; Repository: `Read` (or higher) on the repository owning the workflow.
**Prerequisites:**

- You are configured as a required reviewer on the protected environment. If not, request this through GHE-ALM-068.
- The release readiness review for the change has been completed (GHE-ALM-046).
- You can identify the change being deployed (issue, pull request, milestone, or release tag).

**When to use this guide:** Use when you receive a notification that a workflow run is waiting on your approval to deploy to a protected environment such as `staging`, `uat`, or `production`.

**When not to use this guide:** Do not use this guide to inspect past deployments after the fact; use GHE-ALM-066 for that. Do not use this guide to configure who is allowed to approve; that is GHE-ALM-068.

## Outcome

By the end of this guide, you will have produced:

- A recorded approval (or rejection) on a protected deployment, with an optional comment captured in the workflow run history.
- A clear traceable link from the deployed change back to the issues, pull requests, and release that justified the approval.

## Before You Start

- The notification email or in-app alert that pointed you at the workflow run.
- The release ID, milestone, or tag the deployment corresponds to (for example, `2026.05.0`).
- Your release readiness checklist outcome from GHE-ALM-046.
- Confirmation from QA that validation in the prior environment (for example, `staging` before `production`) passed.

## Steps

### Open the pending deployment

1. Open the notification. From the email, click **Review deployments**. From in-app notifications, open the workflow run linked from the bell icon.
2. If you arrived from another path, navigate to the repository, click **Actions**, and open the workflow run that shows the **Waiting** status with a yellow indicator next to the environment name.
3. At the top of the run page, locate the **Review pending deployments** box. It lists each environment that is gated and the jobs awaiting approval.

> [SCREENSHOT: workflow run page showing the Review pending deployments box with the environment name and Review deployments button]

### Read the deployment context

4. Click the workflow run title to confirm the **commit SHA**, the **branch or tag** being deployed, and the **workflow name**. For a release deployment, the ref should match the release tag (for example, `v2026.05.0`).
5. Open the linked pull request or release from the run summary. Confirm the change description, linked issues (look for `Closes #NNNN`), and the milestone assignment match the scope you approved in the readiness review.
6. Cross-check the prior environment in **Environments** (left sidebar of the repository) to confirm the same SHA was deployed and verified one stage earlier. For example, before approving `production`, confirm the same SHA succeeded in `staging` and that QA signed off.

### Validate against release readiness

7. Walk through your release readiness checklist (GHE-ALM-046) and confirm: scope is complete, all required pull requests are merged, blocker bugs are closed, release notes are drafted, and the rollback plan is recorded. Stop here if any item is unresolved.
8. Confirm the deployment window. If your team uses change windows or freezes, verify the current time is inside the approved window.

### Approve, reject, or comment

9. In the **Review pending deployments** box, click **Review deployments**.
10. Select the environment(s) you intend to act on. If the run gates multiple environments and you are only authorized for one, select only that one.
11. In the comment field, record the justification. Use a short, auditable note such as `Approved per release readiness review for 2026.05.0; QA sign-off recorded on staging at SHA abc1234.` The comment is optional but strongly recommended for audit.
12. Click **Approve and deploy** to release the job, or **Reject** if readiness or window conditions are not met.

> [SCREENSHOT: review modal showing environment selection, comment field, Approve and deploy button, and Reject button]

### Confirm the result

13. After approving, the job moves from **Waiting** to **In progress**. Watch the run until the deployment job completes. The environment card on the run page shows the new active deployment.
14. After rejecting, the workflow fails. Notify the release manager and the change owner so they can address the gap and rerun the workflow when ready.
15. Open **Environments** in the repository sidebar and confirm the new deployment is listed as **Active** for the target environment. Cross-link the release record (GHE-ALM-050) so the deployment is reflected in the release closure.

> [SCREENSHOT: Environments page showing the new Active deployment for the target environment with the SHA and timestamp]

## Validation Checklist

- [ ] The workflow run status changed from **Waiting** to **In progress** (approved) or **Failed** (rejected).
- [ ] The approval comment is visible in the run timeline and attributed to your account.
- [ ] The **Environments** page shows the new deployment as **Active** for the target environment.
- [ ] The deployed commit SHA matches the release tag or pull request you validated.
- [ ] The release record (milestone or GitHub Release) reflects the deployment outcome.

## Common Mistakes

- Approving without reading the commit SHA. The run may have been re-triggered against a newer commit than the one in your readiness review.
- Approving the wrong environment when a single run gates several environments. Always tick only the environments you are authorized and ready to release to.
- Skipping the comment. Without a justification line, audit reviewers cannot tell why approval was granted.
- Approving your own deployment when self-approval is disabled. The approve action will be blocked; ask another required reviewer instead.
- Treating approval as a substitute for the release readiness review. Approval is the final gate, not the review itself.

## Escalation Path

- GitHub administrator: Not applicable for routine approvals. Escalate only if the **Review pending deployments** box does not appear when it should, indicating an environment configuration problem.
- Repository administrator: Involve when the wrong reviewers are listed, when self-approval policy needs adjustment, or when a workflow does not gate the environment it should. Use GHE-ALM-068 to request changes.
- Engineering lead: Involve when the change scope or commit SHA does not match what was reviewed, or when readiness criteria appear unmet.
- Release manager: Involve when the deployment window, rollback plan, or release tag is in doubt. The release manager owns the go/no-go decision; the approval click is the recording of that decision.

## Related Guides

- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-065 : How to Understand GitHub Actions and Environments at a Manager Level
- GHE-ALM-066 : How to Review Deployment History
- GHE-ALM-068 : How to Request Environment Protection Rules
- GHE-ALM-050 : How to Close a Release After Deployment
