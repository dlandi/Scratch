# How to Review Deployment History

**Guide ID:** GHE-ALM-066
**Audience:** Release Manager, Engineering Manager, QA Manager
**Primary role:** Release Manager
**Classification:** Manager Reviews
**Estimated time:** 5-10 minutes per environment
**Required permissions:** Repository: Read

**Prerequisites:**

- The repository uses GitHub Actions with one or more named environments (for example `staging`, `uat`, `production`).
- At least one workflow has run that targets the environment you want to inspect.
- You know the release tag, commit SHA, or pull request number you are trying to confirm.

**When to use this guide:** Use this guide to confirm whether a specific build, tag, or commit reached a given environment, and to inspect the recent deployment trail before a release sign-off, audit conversation, or incident review.

**When not to use this guide:** Do not use this guide to approve a pending protected deployment (see GHE-ALM-067) or to change protection rules (see GHE-ALM-068).

## Outcome

By the end of this guide, you will have produced:

- A confirmed answer to "did release `X` reach `staging`, `uat`, and `production`?"
- A short note recording the deployment ref, status, timestamp, and approver (if any) for each environment of interest.
- A clear escalation if any environment shows a failed, stuck, or unexpectedly inactive deployment.

## Before You Start

- Identify the repository and the environment names in scope. A typical setup is `staging`, `uat`, and `production`, but teams may use different names.
- Have the release tag (for example `v2026.05.0`) or the merge commit SHA in front of you.
- Have at least Repository `Read` access. Inspecting deployment history does not require approver rights.

## Steps

### Open the environment view

1. Open the repository in GitHub Enterprise. For example, `acme-payments/checkout-service`.
2. Click the **Settings** tab. In the left sidebar under "Code and automation", click **Environments**. This page lists every environment configured for the repository, with the name, the count of recent deployments, and any active protection rules.
3. Click the environment you want to review, for example **production**. The environment page opens with the configured protection rules at the top and the **Deployment history** section below.

> [SCREENSHOT: Environments list for a repository showing staging, uat, and production with deployment counts and protection-rule indicators]

### Read the latest deployment

4. Locate the most recent entry in **Deployment history**. Each row shows:
   - The deployment **ref**: a tag (for example `v2026.05.0`), a branch tip, or a commit SHA.
   - A **status** badge: `Active`, `Success`, `Failure`, `In progress`, `Queued`, `Waiting`, or `Inactive`. `Active` means the deployment is the current live one for that environment. `Inactive` means a later deployment has superseded it.
   - The **timestamp** of the deployment.
   - The triggering **workflow run** link, which opens the GitHub Actions run that performed the deployment.
   - The **actor** who triggered the run, and, when a required-reviewer rule applied, the **approver** who released the job.
5. Click the workflow run link in the latest row. Confirm the run completed without failed jobs, that the job tied to the environment finished with a green check, and that the listed commit SHA matches the release you expected.

### Check the recent history

6. Scroll the **Deployment history** list. Confirm:
   - The expected release ref appears at the top with status `Active` or `Success`.
   - Earlier deployments show status `Inactive` (normal: a newer deployment took over) rather than `Failure` (abnormal: the deployment did not complete).
   - Timestamps progress in the order you expect. A long gap between staging and production may indicate a stalled promotion.
7. If a deployment shows **Waiting** for more than the expected approval window, note the approver group listed and follow up using GHE-ALM-067.

### Distinguish staging vs production

8. Return to **Settings** then **Environments** and repeat steps 3 through 6 for each environment in your release path. A clean release has the same ref reaching `staging`, then `uat`, then `production`, in that time order.
9. Compare the top-of-history refs across environments. If `production` is still showing the previous tag while `staging` and `uat` show the new tag, the release has not yet been promoted to production. Record this as the current state rather than treating it as a failure.

### Confirm a specific release tag reached production

10. On the **production** environment page, search the deployment history for the tag, for example `v2026.05.0`. The matching row should show status `Active` or `Success` with a timestamp that is consistent with the release notes.
11. Click the workflow run link for that row. Confirm the run targets the `production` environment, the deployed ref matches the tag, and the listed approver matches a person authorized to sign off production deployments.

> [SCREENSHOT: Production deployment history with the active row expanded, showing ref, status, timestamp, workflow link, and approver]

## What Good Looks Like vs. What to Escalate

| Signal | What good looks like | What to escalate |
|---|---|---|
| Latest production deployment | Status `Active` or `Success`, ref matches the released tag, workflow run is green | Status `Failure`, ref does not match the released tag, or no entry for the expected tag |
| Approver on protected deployment | Listed approver is a designated release approver | No approver shown when one is required, or approver is outside the authorized group |
| Promotion order | `staging` then `uat` then `production`, same ref, in time order | Production has a newer ref than staging or uat, or environments deployed out of order |
| Waiting state | Brief, then resolves to `Success` after approval | Pending more than the agreed approval window; no responsible approver identified |
| Inactive entries | Older deployments naturally become `Inactive` when superseded | Most recent entry is `Inactive` with no `Active` deployment behind it |
| Workflow run | Linked run completed without failed jobs | Linked run failed, was cancelled, or never reached the environment job |

## Validation Checklist

- [ ] You opened **Settings** then **Environments** and identified every environment in the release path.
- [ ] You confirmed the latest deployment ref, status, timestamp, and approver for each environment.
- [ ] You confirmed the released tag appears with `Active` or `Success` status on `production` (or recorded that it has not yet been promoted).
- [ ] You followed each suspect row to the linked workflow run and confirmed the job that targets the environment succeeded.
- [ ] You captured a short note suitable for a release sign-off or audit log.

## Common Mistakes

- Treating an `Inactive` status as a failure. `Inactive` means a later deployment superseded this one.
- Reading the GitHub **Releases** page and assuming the release reached production. The Releases page records that a tag was published; only the environment deployment history confirms the tag actually deployed.
- Confusing the actor (who triggered the run) with the approver (who released a protected deployment). Both can appear on the same row.
- Looking at only the production environment. If `staging` or `uat` failed for the same ref, production may have been promoted on a stale build.
- Assuming the deployment ref matches the release tag. Always click through to the workflow run and confirm the SHA.

## Escalation Path

- GitHub administrator: Not applicable for routine review.
- Repository administrator: When deployment history is missing, environments are not visible, or you suspect protection rules were changed mid-release.
- Engineering lead: When a workflow run linked from the deployment history failed or shows unexpected jobs.
- Release manager: When the released tag did not reach `production`, when promotion order is wrong, or when an approval has been pending past the agreed window.

## Related Guides

- GHE-ALM-065 : How to Understand GitHub Actions and Environments at a Manager Level
- GHE-ALM-067 : How to Approve a Protected Deployment
- GHE-ALM-068 : How to Request Environment Protection Rules
- GHE-ALM-069 : How to Interpret Deployment Branch and Tag Restrictions
- GHE-ALM-050 : How to Close a Release After Deployment
