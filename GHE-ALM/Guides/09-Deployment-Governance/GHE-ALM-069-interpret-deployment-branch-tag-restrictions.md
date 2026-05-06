# How to Interpret Deployment Branch and Tag Restrictions

**Guide ID:** GHE-ALM-069
**Audience:** Release Manager, Engineering Manager, Program Manager
**Primary role:** Release Manager
**Classification:** Manager Understands / Manager Reviews
**Estimated time:** 20-30 minutes per environment audited
**Required permissions:** Repository: `Read` to view environment settings; `Admin` is required to change them but not to interpret them.
**Prerequisites:**

- Familiarity with GitHub Environments and deployment workflows.
- A list of environments to review (typically `production`, `staging`, `uat`, `dev`).
- Knowledge of which branches and tags are used for releases (for example `main`, `release/*`, `v*`).

**When to use this guide:** Use this guide when you need to confirm that a protected environment, especially production, can only be deployed from approved branches or release tags, and when you need to translate a deployment branch/tag rule into plain English for a release governance review.

**When not to use this guide:** Do not use this guide to configure the rules. Configuration is requested through GHE-ALM-068. Do not use this guide to approve a specific deployment; that is GHE-ALM-067.

## Outcome

By the end of this guide, you will have produced:

- A written interpretation, per environment, of which branches and tags are allowed to deploy.
- A pass/fail judgment for each environment against your release governance policy.
- A short escalation list of environments that need a rule change request.

## Before You Start

- Open the repository in the browser and confirm you can reach **Settings** > **Environments**. If the **Settings** tab is not visible, you have insufficient permissions; request `Read` or higher.
- Have your release governance policy at hand. At minimum it should state which branches or tags are allowed to deploy to production, and whether staging and dev have any restriction.
- Know your repository's branch and tag naming convention. Common conventions: trunk on `main`, release branches as `release/2026.05.x`, version tags as `v2026.5.0`, hotfix branches as `hotfix/*`.

## Steps

### 1. The model: what a deployment branch and tag rule actually controls

A GitHub environment is a named deployment target (`production`, `staging`, `uat`, `dev`). When a workflow job declares `environment: production`, GitHub checks the environment's protection rules before the job runs. One of those rules is **Deployment branches and tags**. It restricts which Git ref (branch name or tag name) the workflow run was triggered from.

The rule does not restrict what the workflow does. It restricts what ref the workflow ran on. If the rule disallows the ref, the deployment job is blocked and the environment is not updated.

### 2. The three patterns you will see

Open **Settings** > **Environments** > select an environment. Find the **Deployment branches and tags** dropdown. You will see one of three values.

**No restriction (any branch or tag).** Often shown as **All branches** or equivalent. Any branch or tag in the repository can deploy to this environment. This is the default for new environments. For a production environment this is almost always wrong: it means a developer's feature branch can ship to production if a workflow is triggered against it.

**Protected branches only.** Only branches covered by a branch protection rule or a ruleset can deploy. Tags cannot deploy under this option. This is appropriate when your only deploy source is `main` or another protected long-lived branch, and you do not deploy from tags.

**Selected branches and tags.** You see an explicit list of allowed name patterns. Each entry is either a branch pattern or a tag pattern. Wildcards are supported, for example `release/*` matches `release/2026.05.x` and `release/2026.06.x`. A pattern such as `v*` on the tag side matches `v2026.5.0`, `v2026.5.1`, and so on. This option gives the most precise control and is the usual choice for production environments that release from tags.

> [SCREENSHOT: Environment settings page showing the Deployment branches and tags dropdown expanded with all three options visible.]

### 3. How to read a "Selected branches and tags" list

Each entry in the list has a ref type (Branch or Tag) and a name pattern. Read the entries together as a single allow-list. If a workflow runs from a ref that matches any entry, deployment is allowed. If it matches none, deployment is blocked.

When you see a pattern, expand it mentally:

| Pattern | Type | Matches | Does not match |
|---|---|---|---|
| `main` | Branch | `main` only | `release/2026.05.x`, `feature/x` |
| `release/*` | Branch | `release/2026.05.x`, `release/2026.06.x` | `release/2026.05/hotfix-1` (single segment only), `main` |
| `release/**` | Branch | `release/2026.05.x`, `release/2026.05/hotfix-1` | `main` |
| `v*` | Tag | `v2026.5.0`, `v1.2.3` | branch named `v-next`, tag `2026.5.0` (no `v` prefix) |
| `hotfix/*` | Branch | `hotfix/login-crash` | `main`, `release/*` |

Treat `*` as matching one path segment and `**` as matching multiple. If the rule uses only `*` and your release branches contain extra slashes, the rule will silently exclude them.

### 4. Decision rules by environment type

Use these defaults when judging whether an environment's rule is acceptable. Adjust against your team's stated policy if it differs.

| Environment | Expected restriction | Why |
|---|---|---|
| `production` | Selected branches and tags. Tags only (for example `v*`), or release branches only (for example `release/*`). Never **All branches**. | Production should be reachable only from a deliberately created release artifact. Tags are immutable; they are the strongest signal. |
| `staging`, `uat` | Selected branches and tags. Usually `main` and `release/*`. | Staging mirrors production but accepts release candidates; it should not accept arbitrary feature branches. |
| `dev`, `sandbox` | All branches, or no environment at all. | Dev environments are meant to validate work in progress. Restricting them creates friction without governance benefit. |

If production shows **All branches**, that is a finding. If production shows **Protected branches only** but your release process deploys from tags, that is also a finding because tags are not branches and will be blocked.

### 5. What the rule does not cover

The rule restricts the deploying ref. It does not enforce:

- That the ref was reviewed (covered by branch protection / rulesets, GHE-ALM-074).
- That a human approved the deployment (covered by required reviewers, GHE-ALM-067).
- That a wait timer elapsed before deployment (covered by environment protection rules, GHE-ALM-068).
- That the deployment succeeded (covered by deployment history, GHE-ALM-066).

A clean branch/tag rule does not by itself make production governed. Treat it as one of four signals on the environment.

### 6. Worked example: reviewing `production` for `acme-checkout/checkout-service`

The release manager for `acme-checkout` opens the `checkout-service` repository, goes to **Settings** > **Environments**, and selects **production**. The **Deployment branches and tags** field shows **Selected branches and tags** with two entries:

- Tag pattern: `v*`
- Branch pattern: `hotfix/*`

The release manager reads this as: "Production accepts deployments only from version tags such as `v2026.5.0` and from hotfix branches such as `hotfix/login-crash`. Any feature branch, the `main` branch on its own, and any `release/*` branch are blocked from deploying to production."

Cross-checking against `acme-checkout`'s policy, which states that production must deploy from version tags and that hotfix branches are an explicit exception, the rule passes. The release manager records the result in the governance log and moves to the next environment.

If the same review for **staging** showed **All branches**, the release manager would file a request via GHE-ALM-068 to tighten staging to `main` and `release/*`.

## Validation Checklist

- [ ] You can name, for each reviewed environment, the exact dropdown value (**All branches**, **Protected branches only**, **Selected branches and tags**).
- [ ] For every environment using **Selected branches and tags**, you have written down each pattern and its ref type.
- [ ] You have compared each environment's rule against your release governance policy and recorded pass or fail.
- [ ] You have flagged any production or staging environment showing **All branches** for follow-up.
- [ ] You have confirmed whether your release process deploys from branches, tags, or both, and that the rule type matches.

## Common Mistakes

- Reading a branch pattern as if it were a tag pattern. `v*` under Branch matches a branch literally named `v-next`; it does not match the tag `v2026.5.0`. Check the ref type column.
- Assuming `release/*` covers `release/2026.05/hotfix-1`. A single `*` matches one path segment only. Use `release/**` if your release branches contain extra slashes.
- Treating **Protected branches only** as equivalent to **Selected branches and tags** with a single branch entry. **Protected branches only** does not allow tag deployment.
- Approving an environment as governed because the branch/tag rule is correct, while ignoring missing required reviewers or absent branch protection on the deploying branch.
- Forgetting that environments are repository-scoped. Each repository's `production` environment is separate. A correctly configured rule in one repository tells you nothing about another.

## Escalation Path

- GitHub administrator: Not applicable for interpretation. Involve when an organization-wide policy on environment configuration is needed.
- Repository administrator: Involve to change an environment's deployment branch/tag rule. Submit the request through GHE-ALM-068.
- Engineering lead: Involve when the deploying ref convention is unclear (which branches and tags should the team use to release).
- Release manager: Owns the interpretation and the pass/fail judgment recorded against governance policy.

## Related Guides

- GHE-ALM-065 : How to Understand GitHub Actions and Environments at a Manager Level
- GHE-ALM-066 : How to Review Deployment History
- GHE-ALM-067 : How to Approve a Protected Deployment
- GHE-ALM-068 : How to Request Environment Protection Rules
- GHE-ALM-074 : How to Review Ruleset and Branch Protection Coverage
