# How to Request Environment Protection Rules

**Guide ID:** GHE-ALM-068
**Audience:** Release Manager, Engineering Manager, Project Manager
**Primary role:** Release Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 30-minute one-time setup request, 15 minutes per quarterly review
**Required permissions:** Repository: Read (to inspect current rules); Repository: Admin is required to apply changes and is held by the administrator who fulfils the request
**Prerequisites:**

- A target repository with at least one deployment workflow that targets a named environment.
- Knowledge of which environment names exist (for example `staging`, `uat`, `production`).
- Identified human approvers or GitHub team that should act as required reviewers.
- Branch naming convention already agreed (typically `main` and `release/*`).

**When to use this guide:** Use when production or other regulated environments need documented governance, when an audit finds that an environment has no required reviewer, or when deployment scope needs to be limited to specific branches or tags.

**When not to use this guide:** Do not use this guide to request branch protection on source branches; that belongs in GHE-ALM-073. Do not use this guide to approve a single waiting deployment; that belongs in GHE-ALM-067.

## Outcome

By the end of this guide, you will have produced:

- A written protection-rule request sent to the repository administrator covering required reviewers, wait timer, deployment branch and tag restrictions, and environment secrets policy.
- A documented baseline for the production environment that other environments can be compared against.
- A short review record (date, environment, settings observed, gaps) that you can re-run quarterly.

## Before You Start

- Confirm the repository name and the exact environment name as it appears in the workflow (`environment: production`, for example).
- List the people or GitHub team that should appear as required reviewers, and confirm each is a member of the organization.
- Decide the wait timer in minutes, if any. A 5 to 10 minute timer gives an approver time to cancel a mistaken deployment.
- Decide the allowed deployment branches and tags. The default production baseline is `main` and `release/*`.
- Decide whether environment secrets must be scoped to that environment or are acceptable as repository-level secrets.

## Steps

### Inspect the current rules

1. Open the repository in GitHub Enterprise.
2. Click **Settings**, then **Environments** in the left sidebar.
3. Open the target environment, for example **production**.
4. Record the current configuration for each of the four rule types: **Required reviewers**, **Wait timer**, **Deployment branches and tags**, and the list of **Environment secrets** and **Environment variables**.
5. Compare the recorded values against the production baseline in the next phase.

> [SCREENSHOT: Environments list under Settings, with the production environment selected and the four protection rule sections visible.]

### Apply the production baseline comparison

6. Use the table below as the baseline for any environment that deploys to production or production-like systems. Note any gap in your review record.

| Rule | Production baseline | Acceptable variation |
|---|---|---|
| Required reviewers | At least one person or team, with **Prevent self-review** enabled | Two reviewers for regulated workloads |
| Wait timer | 5 to 10 minutes | 0 minutes only if a separate change-control gate exists |
| Deployment branches and tags | Selected branches and tags, limited to `main` and `release/*` | Add `hotfix/*` if hotfix flow is documented |
| Environment secrets | Production credentials scoped to the environment, never repository-level | Shared non-secret values may live in environment variables |

### Draft and send the request

7. Use the Sample Request below. Replace each placeholder with the value from your inspection notes.
8. Send the request to the repository administrator. Copy the engineering lead and, where applicable, the security or compliance contact.
9. After the administrator confirms the change, repeat the inspection in step 1 through 4 and store the screenshot as your evidence of compliance.

### What Good Looks Like vs. What to Escalate

| Observation | What good looks like | What to escalate |
|---|---|---|
| Required reviewers on `production` | One or more named reviewers or a team; **Prevent self-review** enabled | Empty reviewer list, or a single individual with no backup |
| Wait timer | 5 to 10 minutes for production | Zero on production with no compensating change-control gate |
| Deployment branches and tags | **Selected branches and tags** restricted to `main` and `release/*` | **All branches** allowed on production, or a feature branch in the allow list |
| Environment secrets | Production credentials live as environment secrets on `production` only | Production credentials stored as repository secrets visible to any workflow |
| Implicit environment | Each environment in workflows is configured in **Settings > Environments** | Environment referenced in YAML but missing from the Environments list, leaving rules unenforced |

## Sample Request to Send

> **To:** Repository administrator, `acme-payments/checkout-service`
> **Cc:** Engineering lead, security contact
> **Subject:** Request: environment protection rules for `production`
>
> Please apply the following protection rules to the `production` environment in `acme-payments/checkout-service`. These align with the Release Management governance baseline.
>
> 1. **Required reviewers:** Add the team `@acme-payments/release-approvers` as a required reviewer. Enable **Prevent self-review**.
> 2. **Wait timer:** Set to `5` minutes.
> 3. **Deployment branches and tags:** Set to **Selected branches and tags**. Allow `main` and `release/*`. Remove any other entries.
> 4. **Environment secrets:** Move `PROD_DB_PASSWORD` and `PROD_API_TOKEN` from repository-level secrets to environment secrets scoped to `production`. Repository-level copies should be deleted after the move is verified.
>
> Please confirm completion and share a screenshot of the **Environments > production** screen so it can be filed as evidence. The change is needed before the `2026.05.0` release cut on 2026-05-13.

## Validation Checklist

- [ ] The `production` environment shows at least one required reviewer or team.
- [ ] **Prevent self-review** is enabled for `production`.
- [ ] Wait timer reflects the value you requested.
- [ ] **Deployment branches and tags** is set to **Selected branches and tags** and lists only the agreed patterns.
- [ ] Production credentials appear as environment secrets, not as repository secrets.
- [ ] A dated screenshot of the configured environment is stored in the release evidence folder.

## Common Mistakes

- Relying on **All branches** for production. Any branch that has a deploy workflow can then ship to production.
- Naming an individual instead of a team as the required reviewer. The individual becomes a single point of failure during vacation or attrition.
- Storing production secrets at the repository level. Any workflow in the repository can read them, including workflows on feature branches.
- Forgetting to disable **Prevent self-review**. The engineer who triggered the deployment can then approve it, defeating the control.
- Configuring the workflow to reference an environment but never creating the environment in **Settings**. Workflows still run, but no protection rule applies.

## Escalation Path

- GitHub administrator: When environment configuration requires organization-level policy (for example shared production approvers across many repositories).
- Repository administrator: Owns the actual configuration change and the secret rotation that follows.
- Engineering lead: Approves the list of required reviewers and the branch allow list.
- Release manager: Owns the baseline, the quarterly review, and the audit evidence.

## Related Guides

- GHE-ALM-065 : How to Understand GitHub Actions and Environments at a Manager Level
- GHE-ALM-066 : How to Review Deployment History
- GHE-ALM-067 : How to Approve a Protected Deployment
- GHE-ALM-069 : How to Interpret Deployment Branch and Tag Restrictions
- GHE-ALM-073 : How to Request Rulesets or Branch Protection
