# How to Use Automatically Generated Release Notes

**Guide ID:** GHE-ALM-048
**Audience:** Release Manager, Engineering Manager, Product Owner
**Primary role:** Release Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 15-20 minutes per release
**Required permissions:** Repository: Write to draft and generate notes; Repository: Read to review notes
**Prerequisites:**

- A target repository with at least one prior tag, or an agreed first-tag baseline.
- Pull requests merged into the release branch since the previous tag.
- Optional: a `.github/release.yml` file in the default branch defining categories and exclusions.

**When to use this guide:** Use when you are about to publish a versioned GitHub Release and want a first-draft list of merged work, contributors, and a full changelog link without writing notes by hand. Also use when reviewing notes a peer or release engineer has generated.

**When not to use this guide:** Do not use this guide to author marketing copy, customer-facing announcement text, or detailed upgrade guidance. Generated notes are an engineering record, not a launch narrative. For drafting and publishing the release itself, see GHE-ALM-047.

## Outcome

By the end of this guide, you will have produced:

- A draft GitHub Release populated with auto-generated notes that list merged pull requests since the previous tag.
- A reviewed and corrected note set whose entries are traceable back to issues or work items.
- Optional: a `.github/release.yml` configuration request that groups future notes by category (for example, Features, Bug Fixes, Documentation).

## Before You Start

- Confirm the repository tag scheme. The generator compares the new tag against the previous tag, so naming must be consistent. See GHE-ALM-077 for naming conventions.
- Confirm pull requests use closing keywords such as `Closes #1234` or `Fixes #1234` so the generated notes reflect issue context. See GHE-ALM-063.
- Confirm relevant PRs carry the labels your team uses for categorization (for example, `feature`, `bug`, `documentation`). See GHE-ALM-021.
- Identify the previous tag you want to compare against. The default is the most recent tag on the same target branch.

## Steps

### Generate the notes

1. Open the repository in GitHub. On the right side of the repository home page, click **Releases**.
2. Click **Draft a new release**.
3. Under **Choose a tag**, select an existing tag or type a new tag name (for example, `v2026.05.0`) and choose **Create new tag on publish**.
4. Set **Target** to the branch the tag will be created from, typically `main` or a `release/*` branch.
5. Optional: click **Previous tag** and pick a specific earlier tag if you do not want the default auto-detected one. Use this when generating notes for a hotfix or a re-cut release.
6. Enter a **Release title**, for example `2026.05.0`.
7. Click **Generate release notes** above the description field. GitHub fills the description with the list of merged PRs since the previous tag, a **New Contributors** section if any, and a **Full Changelog** compare link.

> [SCREENSHOT: Draft release page showing tag, target branch, previous tag selector, and Generate release notes button]

### Review the generated notes

8. Read every line. Each entry shows the PR title, PR number, and author. Confirm titles are descriptive enough for a release audit. Vague titles such as `fix stuff` or `update` should be flagged back to the PR author for rewording in a future PR or annotated inline in the release body.
9. Confirm the **Full Changelog** compare link at the bottom points at the correct previous and current tags.
10. Check the **New Contributors** section for accuracy. Bot accounts (for example, dependabot) often appear and may warrant exclusion via `.github/release.yml`.
11. Compare the generated list against the milestone or Project **Release** field for the same release. Anything missing from the notes but present in the milestone is a traceability gap. See GHE-ALM-060.

> [SCREENSHOT: Generated notes body with PR list, New Contributors, and Full Changelog link]

### Correct, categorize, and save

12. Edit the note body in place. You may reorder entries, add a short summary paragraph at the top, and remove genuinely irrelevant items (for example, internal CI tweaks merged the same day).
13. If your repository has a `.github/release.yml` file, the notes are already grouped under category headings such as **Features**, **Bug Fixes**, and **Other Changes**. Verify each PR landed under the correct category. Miscategorization usually means the PR was missing a label or carried the wrong one.
14. Click **Save draft**. Do not click **Publish release** from this guide; publishing is covered in GHE-ALM-047 and requires its own review.

> [SCREENSHOT: Saved draft release card on the Releases page]

### Optional: request or update a release configuration

15. If notes arrive ungrouped or noisy, request a `.github/release.yml` from a repository administrator. The file lives on the default branch and applies to all future generated notes for that repository.
16. In the request, specify the category titles, the labels that map to each category, the catch-all (`*`) bucket, and any labels or authors to exclude (for example, `ignore-for-release`, bot accounts).

## What Good Looks Like vs. What to Escalate

| Aspect | What good looks like | What to escalate |
|---|---|---|
| PR coverage | Every merged PR since the previous tag appears, or is intentionally excluded by label. | PRs known to be merged are missing. Likely cause: wrong target branch, wrong previous tag, or excluded by configuration. |
| Issue traceability | Each PR title or body references an issue (for example, `Closes #1234`) and the issue auto-closed on merge. | PRs with no issue link, or open issues that should have closed. Escalate to engineering lead and see GHE-ALM-063. |
| Categorization | PRs land under the expected heading (Features, Bug Fixes, Documentation, Other Changes). | A bug-fix PR appearing under Features, or everything dumped into Other Changes. Escalate label hygiene to the engineering lead and see GHE-ALM-021. |
| Contributors | Human contributor list matches who actually shipped work. | Real contributors missing, or bot accounts dominating the list. Request `exclude.authors` updates to `.github/release.yml`. |
| Tag baseline | Previous tag is the immediately prior production release on the same branch. | Previous tag is from a different release line (for example, comparing a `release/2026.05` tag against a `release/2026.04` tag). Re-pick the previous tag manually. |
| Release scope | Notes match milestone scope and Project **Release** field. | Items in the milestone are missing from the notes, or notes contain work tagged for a different release. Reconcile before publishing. |

## Validation Checklist

- [ ] Generated notes were produced against the correct previous tag.
- [ ] PR list count matches the count of PRs merged into the target branch since the previous tag.
- [ ] Notes are grouped under intended categories, or the team has accepted an ungrouped flat list.
- [ ] Each entry has a meaningful title; vague titles are flagged.
- [ ] **Full Changelog** compare link resolves to the correct tag pair.
- [ ] **New Contributors** section is accurate and free of unintended bot entries.
- [ ] Draft is saved, not yet published.

## Common Mistakes

- Publishing the release immediately after generating notes without a review pass.
- Letting the generator pick the previous tag when releasing a hotfix; the auto-detected tag may be the wrong baseline.
- Treating the generated notes as customer release notes. They are an engineering inventory; customer-facing copy belongs in product or marketing.
- Excluding the entire `documentation` label from notes by default, which hides legitimate doc fixes that customers care about.
- Forgetting to add a catch-all `*` category to `.github/release.yml`. Untagged PRs then disappear from notes silently.
- Renaming or deleting the previous tag after notes were generated, which breaks the **Full Changelog** compare link.
- Creating the release on the wrong target branch, which produces an empty or wildly wrong PR list.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: When `.github/release.yml` does not exist or needs structural changes; when label-to-category mapping needs revision.
- Engineering lead: When PR titles, labels, or closing keywords are routinely missing such that generated notes are unreliable.
- Release manager: Owns the final published release notes and the decision to publish; arbitrates scope discrepancies between notes and milestone.

## Related Guides

- GHE-ALM-021 : How to Use Labels Without Replacing Issue Types
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
- GHE-ALM-063 : How to Interpret Closing Keywords such as `Closes`, `Fixes`, and `Resolves`
- GHE-ALM-077 : How to Enforce Naming Conventions
