# How to Enforce Naming Conventions

**Guide ID:** GHE-ALM-077
**Audience:** Engineering Manager, Project Manager, Release Manager
**Primary role:** Engineering Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 30-minute one-time setup, then 10 minutes per weekly sweep
**Required permissions:** Repository: Triage to rename issues and milestones; Repository: Admin to request branch/tag rulesets; Project: Write to rename iterations and releases.
**Prerequisites:**

- Your team has agreed on the conventions documented below, or your organization has published an equivalent standard.
- The Project, repository, and milestone scope for this team is already established.
- You can edit issues, milestones, and Project iteration values in the affected repositories.

**When to use this guide:** Use when you are setting up a new ALM Project, onboarding a new team to GitHub, or running a hygiene sweep where issue titles, branches, milestones, releases, or sprints are inconsistent.

**When not to use this guide:** Do not use this guide to design a brand-new naming scheme from scratch. Adopt the conventions below first; this guide is about applying and enforcing them.

## Outcome

By the end of this guide, you will have produced:

- A single-page naming standard pinned where the team works (Project README or repository wiki).
- Renamed issues, branches, milestones, releases, and iterations that did not match the standard.
- A request to repository administrators for tag and branch pattern rulesets where pattern enforcement is needed.
- A weekly review pass that catches new deviations within five working days.

## Before You Start

- Decide which Project and which repositories are in scope. Naming applies per Project / per repository, so list them explicitly.
- Confirm who owns each artifact type. Issue titles are usually the assignee or reporter; branch names are the developer; milestones and releases are the release manager; iterations are the Scrum Master.
- Have the canonical pattern table below open. You will paste it into the standard.

## Steps

### Publish the standard

1. Open the Project README, the repository wiki page, or the team handbook page that already holds operating conventions. Pick one location and link the others to it. Do not maintain two copies.
2. Paste the canonical pattern table:

   | Artifact | Pattern | Example |
   |---|---|---|
   | Issue title | `[Area] Short description` | `[Checkout] Persist guest cart across sessions` |
   | Feature branch | `feature/<issue-number>-short-description` | `feature/1842-guest-cart-persistence` |
   | Bug branch | `bugfix/<issue-number>-short-description` | `bugfix/1907-discount-rounding` |
   | Release branch | `release/<version>` | `release/2026.05` |
   | Hotfix branch | `hotfix/<version>` | `hotfix/2026.04.1` |
   | Milestone | `vMajor.Minor` or `YYYY-QN Release` | `v2026.05` or `2026-Q3 Release` |
   | Release (tag) | `vMajor.Minor.Patch` | `v2026.05.0` |
   | Sprint | `Sprint YYYY.NN` or `Sprint NN` | `Sprint 2026.18` or `Sprint 27` |

3. Add three rules to the standard so they cannot be misread later:

   - Lowercase, hyphen-separated slugs in branch names. No spaces, no underscores, no camelCase.
   - The issue number in the branch name is the GitHub issue number, not a Jira key.
   - Release tags use `v` prefix and three numeric segments. Milestones use two segments because patches roll up to the same milestone.

> [SCREENSHOT: Project README or wiki page showing the naming standard table pinned at the top.]

### Apply the standard to existing artifacts

4. Open the Project table view. Sort by **Title** and scan for issue titles that do not start with a bracketed area tag. For each, click the issue, click the title, and rename it in place. Save.
5. Open the repository **Branches** page. Filter to **Stale** and **Active**. For each branch whose name does not match the pattern, ask the assignee to rename via `git branch -m <old> <new>` and push. You do not rename other developers' active branches yourself.
6. Open **Milestones** in each in-scope repository. Click the pencil icon on any milestone whose title does not match `vMajor.Minor` or `YYYY-QN Release`, rename, and save. Existing issues remain attached.
7. Open the repository **Releases** page. For each draft or published release whose tag does not match `vMajor.Minor.Patch`, decide: if the release is published, leave the tag and add a note in the release description; if it is still a draft, edit the tag before publish. Do not delete published tags; they break consumer references.
8. Open the Project **Sprint** field configuration (Project settings, Fields, **Sprint**). Rename future iterations that do not match `Sprint YYYY.NN` or `Sprint NN`. Past iterations stay as they are; renaming history confuses charts.

> [SCREENSHOT: Repository Branches page filtered to active branches, with a non-conforming branch name highlighted.]

### Request pattern enforcement

9. Open an issue in the repository's governance or platform team queue, or send a message through your usual administrator request channel. Ask for two rulesets:

   - A **branch ruleset** on each in-scope repository that restricts branch creation to names matching `feature/*`, `bugfix/*`, `release/*`, `hotfix/*`, `main`, and any other branches your team uses. This is a pattern restriction, not a protection rule, and it stops typos at creation time.
   - A **tag ruleset** that restricts tag creation to `v[0-9]+.[0-9]+.[0-9]+` so a developer cannot accidentally publish a release tagged `2026-05-final`.

10. In the request, name the repositories, the patterns above, and the bypass list (typically nobody, or release-engineering only). Reference GHE-ALM-073 if your administrators use that intake.

> [SCREENSHOT: Repository Settings, Rules, Rulesets list with the branch and tag rulesets enabled.]

### Run the weekly review

11. Once a week, open the Project table view grouped by **Status**. Skim the first 30 to 50 active titles for the bracketed area tag. Rename outliers in place.
12. Open the repository Branches page sorted by **Newest**. Scan the most recent ten to twenty branch names. If any violate the pattern and the developer has not yet pushed code, ask them to rename. If code is pushed, accept the deviation and note the pattern reminder in their next standup.
13. Open Milestones and Releases. Confirm any new milestone or draft release matches the pattern. Rename or ask the release manager to rename before publish.

## Validation Checklist

- [ ] The naming standard is pinned in one place and linked from the Project description.
- [ ] All open issues in the Project have a bracketed area tag in the title.
- [ ] All active branches match `feature/*`, `bugfix/*`, `release/*`, or `hotfix/*`.
- [ ] All open milestones match `vMajor.Minor` or `YYYY-QN Release`.
- [ ] All draft releases use `vMajor.Minor.Patch` tags.
- [ ] Future Sprint iterations match `Sprint YYYY.NN` or `Sprint NN`.
- [ ] Branch and tag rulesets are requested or in place.

## What Good Looks Like vs. What to Escalate

| Artifact | Good | Escalate |
|---|---|---|
| Issue title | `[Billing] Refund eligibility check fails for partial captures` | `fix bug`, `URGENT - billing thing`, `RE: Slack from Tuesday` |
| Branch | `feature/2104-refund-eligibility` | `dennis-fix`, `temp`, `feature_2104_RefundEligibility` |
| Milestone | `v2026.05`, `2026-Q3 Release` | `May Release v2`, `Q3-final-FINAL`, `Sprint 12` (sprint, not milestone) |
| Release tag | `v2026.05.0`, `v2026.05.1` | `2026-05-launch`, `release-may`, `final` |
| Sprint | `Sprint 2026.18`, `Sprint 27` | `Sprint May 4 - 17`, `Current Sprint`, `Sprint Final` |

If you see escalation-column patterns more than twice in a week from the same person, raise it in a one-on-one. If you see them across the team, the standard is not pinned visibly enough; move it.

## Common Mistakes

- Renaming a published release tag to fix the pattern. This breaks consumer references and CI artifacts. Leave it; fix the next one.
- Inventing a new bracket value in an issue title every time. The set of `[Area]` values should match the **Product Area** field; do not let titles diverge from the field taxonomy.
- Treating sprint and milestone as interchangeable. Sprint is the Project iteration field; milestone is the repository release container. Mixing them breaks reporting.
- Allowing underscores or camelCase in branch names. Hyphens are easier to read in URLs and logs; pick one and hold the line.
- Asking administrators for a ruleset without listing the patterns and the bypass list. The request bounces back.

## Escalation Path

- GitHub administrator: When the branch or tag ruleset request needs organization-level approval, or when a ruleset is producing false-positive denials.
- Repository administrator: When you need a ruleset created, modified, or temporarily relaxed for a migration.
- Engineering lead: When a developer repeatedly ignores branch naming after reminders.
- Release manager: When a draft release tag needs correction before publish, or when milestone naming spans multiple repositories and needs coordination.

## Related Guides

- GHE-ALM-007 : How to Name and Describe a GitHub ALM Project
- GHE-ALM-042 : How to Create and Manage a Milestone
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-059 : How to Understand Branch Naming Conventions
- GHE-ALM-076 : How to Govern Project Fields and Labels
