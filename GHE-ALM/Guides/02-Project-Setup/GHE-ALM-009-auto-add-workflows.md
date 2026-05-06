# How to Configure Auto-Add Workflows for Project Intake

**Guide ID:** GHE-ALM-009
**Audience:** Project Manager, Engineering Manager, Program Manager
**Primary role:** Project Manager
**Classification:** Manager Requests / Manager Reviews
**Estimated time:** 20-30 minutes to specify a request; 10-15 minutes to review an existing workflow
**Required permissions:** Project: Admin (to configure); Project: Write (to review); Repository: Read on each source repository
**Prerequisites:**

- The ALM Project shell exists at the organization level (see GHE-ALM-006).
- You know which repositories should feed the Project.
- You have agreed with engineering on the labels, issue types, or other markers that identify in-scope work.

**When to use this guide:** Use this guide when the team is adding new repositories to an existing ALM Project, when manually adding items has become a bottleneck, or when you need to review an auto-add workflow that someone else configured.

**When not to use this guide:** Do not use this guide for one-time bulk additions of historical items; auto-add only acts on items created or updated after the workflow is enabled. For backfill, use GHE-ALM-008.

## Outcome

By the end of this guide, you will have produced:

- A written request to a Project admin specifying the repository, filter, and Project for one or more auto-add workflows.
- A review note on each existing auto-add workflow recording whether it is correctly scoped, too broad, or too narrow.

## Before You Start

- The Project URL and a list of source repositories.
- The agreed in-scope filter for each repository, expressed in GitHub filter syntax (for example, `is:issue label:"in-scope"`).
- The name of the Project admin or maintainer who will configure the workflow.
- A sample of 5 to 10 recent issues per repository so you can sanity-check the filter against real items.

## Steps

### Decide what each workflow should do

1. Confirm the Project's intake policy. List the repositories in scope and, for each, the labels, issue types, or assignees that mark items as belonging in this Project. Auto-add evaluates one repository per workflow, so plan one workflow per source repository.
2. Translate each policy line into a filter expression. Auto-add supports the qualifiers `is`, `label`, `reason`, `assignee`, and `no`, plus negation with a leading hyphen. Examples: `is:issue label:"checkout"`, `is:pr -label:"dependencies"`, `is:issue assignee:@me no:label`.
3. Decide what is intentionally excluded. Write a one-line exception note per workflow, for example "Excludes Dependabot PRs via `-label:"dependencies"`" or "Excludes draft issues via `-is:draft`". Exceptions are the part reviewers most often miss.
4. Check the workflow budget. GitHub Enterprise allows up to 20 auto-add workflows per Project. If you need more than 20 source repositories, group repositories under a broader filter, split into multiple Projects, or escalate to a GitHub administrator.

### Review an existing auto-add workflow

5. Open the Project. Click the kebab menu in the top right and select **Workflows**.
6. Open **Auto-add to project**. Read the repository, the filter expression, and the on/off state.
7. Compare the filter against a sample of recent items in that repository. Issues that match the filter should already appear in the Project; issues that do not match should not. If both conditions hold, the filter is correctly scoped.
8. Check for the two common failure modes. A filter with no qualifiers (or just `is:issue`) floods the Project with every issue from the repository. A filter with several `label:` qualifiers ANDed together often misses items because authors forget one label.

> [SCREENSHOT: Project Workflows pane with the Auto-add to project workflow expanded, showing repository, filter, and on/off state]

### Capture findings for each workflow

9. For each workflow, record one row: workflow name, repository, current filter, on/off state, and a verdict of Good, Too Broad, Too Narrow, or Off When It Should Be On.
10. For any verdict other than Good, draft a follow-up request to the Project admin using the template in the next section.

## What Good Looks Like vs. What to Escalate

| Aspect | What Good Looks Like | What to Escalate |
|---|---|---|
| Repository scope | One workflow per in-scope repository, named to match the repository | Workflows pointing at archived repositories, or in-scope repositories with no workflow |
| Filter specificity | Filter combines `is:` with at least one `label:`, `assignee:`, or `no:` qualifier | Filter is empty or only `is:issue`, pulling every issue into the Project |
| Exclusions | Filter excludes known noise such as `-label:"dependencies"`, `-label:"chore"`, or bot-authored items | Project contains a steady drip of automated PRs or off-topic issues |
| Coverage | Recent in-scope items appear within minutes of creation | In-scope items routinely reach sprint planning without ever being added |
| Workflow count | Under 20 enabled workflows per Project | At or near the 20-workflow ceiling with more repositories pending |
| Naming | Workflow name identifies the repository and the filter intent | Workflow named "Workflow 2" or duplicate names across repositories |

## Common Mistakes

- Treating auto-add as a backfill. Existing items in the repository are never added, even if they match the filter; only items created or updated after the workflow is enabled are evaluated.
- Filters that use AND across many labels. `label:"checkout" label:"frontend" label:"qa-ready"` requires all three labels on every item. Authors forget one and the item never reaches the Project.
- Filters with no exclusions. A repository with active Dependabot or release-please bots will drown the Project in PRs unless `-label:"dependencies"` or similar is added.
- Forgetting to turn the workflow on after editing. The **Save and turn on workflow** button is a single action; saving without enabling leaves the filter inert.
- Pointing two workflows at the same repository with overlapping filters. Items still only appear once, but the second workflow consumes a slot from the 20-workflow budget for no benefit.
- Using auto-add for cross-repository taxonomy. Auto-add cannot filter by issue type set at the organization level if the type is not yet exposed as a `label:` or `is:` qualifier. Confirm the qualifier is supported before promising the filter to engineering.

## Sample Request to Send

Send this to the Project admin or maintainer named in your Project README. Replace the bracketed values.

```
Subject: Auto-add workflow request for [Project name]

Hello [admin name],

Please configure the following auto-add workflow on the [Project name] Project
(URL: [project URL]).

Workflow 1
- Source repository: acme-payments/checkout-service
- Filter: is:issue label:"checkout"
- Excludes: items without the checkout label, all PRs
- Reason: Route all checkout-scoped issues into the ALM Project automatically.

Workflow 2
- Source repository: acme-payments/payments-api
- Filter: is:issue,pr -label:"dependencies"
- Excludes: Dependabot PRs and any item explicitly labeled dependencies
- Reason: Capture API issues and feature PRs, suppress dependency-bump noise.

Please enable both workflows after saving and confirm by replying with the
workflow names as they appear in the Workflows pane. I will validate by
opening a test issue in each repository within 24 hours.

Thank you,
[Your name]
```

If you are also requesting changes to existing workflows, list them under a "Changes to existing workflows" heading with the current filter, the proposed filter, and the reason.

## Validation Checklist

- [ ] Each in-scope repository has exactly one auto-add workflow configured.
- [ ] Each workflow's filter has been tested against a real recent issue or PR and the item appeared in the Project.
- [ ] Each workflow's filter excludes known noise (bot PRs, dependency bumps, chore labels).
- [ ] The Project is below the 20-workflow ceiling, with headroom for the next planned repository.
- [ ] Every workflow is in the on state; none are saved-but-disabled.
- [ ] The review notes for each existing workflow are recorded with a verdict and any follow-up request.

## Escalation Path

- GitHub administrator: When the Project needs more than 20 auto-add workflows, when an organization-level qualifier (such as a custom issue type) is needed in the filter, or when a workflow must be configured by someone with elevated org access.
- Repository administrator: When the source repository's labels, issue types, or templates need to change so the filter can be expressed cleanly.
- Engineering lead: When the in-scope policy itself is unclear (which labels mean "in scope" for this Project) or when a workflow change would alter what reaches the team's sprint planning.
- Release manager: Not applicable.

## Related Guides

- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-008 : How to Add Existing Issues and Pull Requests to a Project
- GHE-ALM-010 : How to Archive Completed or Old Project Items
- GHE-ALM-076 : How to Govern Project Fields and Labels
