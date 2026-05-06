# How to Draft or Review a GitHub Release

**Guide ID:** GHE-ALM-047
**Audience:** Release Manager, Engineering Manager, Product Owner
**Primary role:** Release Manager
**Classification:** Manager Performs / Manager Reviews
**Estimated time:** 20-30 minutes per release
**Required permissions:** Repository: Write to draft and publish; Repository: Read to review.
**Prerequisites:**

- Release scope is defined in a milestone or in the `Release` project field.
- Pull requests for the release are merged to the target branch.
- Tag naming convention is agreed (for example `v2026.05.0`).

**When to use this guide:** Use this when you are about to publish a versioned release in a repository, or when you need to review an existing draft or published release before it is announced or deployed.

**When not to use this guide:** Do not use this guide for milestone setup (see GHE-ALM-041) or for the post-deployment closeout (see GHE-ALM-050). Do not use it to design release notes templates; that is a one-time governance task.

## Outcome

By the end of this guide, you will have produced:

- A drafted or published GitHub Release at a specific tag, with a title, release notes, any attached assets, and a correct latest/pre-release designation.
- Or, a reviewed release record with a documented decision to approve, hold, or escalate.

## Before You Start

- Confirm the target branch and the commit that should carry the tag. For most teams this is `main` or a `release/*` branch.
- Confirm whether this release is the new latest release, a pre-release, or a back-port that should not become latest.
- Have the milestone or project filter open so you can confirm scope. The Release Readiness Review (GHE-ALM-046) should already be complete.
- Decide whether release notes will be generated automatically (see GHE-ALM-048) or written manually.

## Steps

### Drafting a release (Performs)

1. Open the repository and click the **Releases** link in the right sidebar of the repository home page, or navigate to `https://<host>/<org>/<repo>/releases`.
2. Click **Draft a new release**.
3. Open the **Choose a tag** dropdown. Select an existing tag if one already points at the release commit. Otherwise type the new tag name, for example `v2026.05.0`, and click **Create new tag on publish**.
4. If you created a new tag, set the **Target** dropdown to the branch or commit that should be tagged. Use `main` for normal releases and the appropriate `release/*` branch for maintenance releases.
5. Optionally adjust **Previous tag** from `auto` to a specific prior tag. This controls the comparison range used by auto-generated notes and the contributors list.
6. Enter the **Release title**. Use the same value as the tag for predictability, for example `v2026.05.0`, or append a short codename such as `v2026.05.0 - Checkout hardening`.

> [SCREENSHOT: Draft a new release form showing tag dropdown, target, title, and description fields]

### Adding notes, assets, and designation

7. Fill in the **Describe this release** field. Click **Generate release notes** to populate it from merged pull requests and linked issues, then edit the result. Keep a top "Highlights" section for non-technical readers and let the auto-generated "What's Changed" list follow.
8. Drag any binary assets into the **Attach binaries by dropping them here or selecting them** area. Common assets include installers, signed packages, SBOMs, and checksums. Skip this step if your delivery pipeline publishes artifacts elsewhere.
9. Set the designation:
   - Leave **Set as the latest release** selected for normal forward releases on `main`.
   - Clear **Set as the latest release** for back-ports that must not displace the current latest, then GitHub uses semantic version ordering instead.
   - Tick **Set as a pre-release** for release candidates, betas, and internal builds that should be visible but flagged.
10. If your repository uses Discussions and you want a release thread, tick **Create a discussion for this release** and pick a category.

> [SCREENSHOT: Lower half of the form showing assets area, latest/pre-release toggles, and the Save draft and Publish release buttons]

### Saving or publishing

11. Click **Save draft** if the release is not ready for public visibility. Drafts are visible only to users with write access and do not create the tag yet.
12. Click **Publish release** when the release is ready. Publishing creates the tag (if new), makes the release visible to all repository readers, and emits the release event used by downstream automations.

### Reviewing an existing release (Reviews)

13. From **Releases**, open the release you need to review. Use the pencil icon next to a release if you need to edit it; do not click the trash icon.
14. Verify the items in the table below. If anything is wrong on a draft, edit it before publication. If anything is wrong on a published release, decide whether to edit notes only, publish a follow-up patch release, or escalate.

> [SCREENSHOT: Published release page showing tag, target commit, latest badge, notes, assets, and contributors]

#### What Good Looks Like vs. What to Escalate

| Element | What Good Looks Like | What to Escalate |
|---|---|---|
| Tag | Matches naming convention; points at the intended commit on the intended branch. | Tag pushed to the wrong commit or branch; tag name does not match `v<version>` convention. |
| Title | Matches the tag, optionally with a short codename. | Title contradicts the tag or contains internal-only language. |
| Release notes | Generated from merged PRs, edited for clarity, with a Highlights section and a complete What's Changed list. | Empty notes, placeholder text, broken links, or notes that omit a known shipped change. |
| Linked issues and PRs | Notes reference the merged PRs and closed issues in the comparison range. | PRs in the range are missing from notes; linked issues are still open. |
| Assets | All expected installers, packages, SBOMs, and checksums are present and the file sizes look right. | Missing or zero-byte assets; assets from a different version. |
| Latest designation | One release is marked latest, and it is the correct one for the consuming audience. | An old release still flagged latest; a back-port flagged latest by mistake. |
| Pre-release flag | RC and beta tags carry the pre-release flag; GA tags do not. | A GA release flagged as pre-release; an RC released without the flag. |
| Draft state | Drafts exist only for in-progress work and are aged out promptly. | Multiple stale drafts with overlapping tags. |

## Validation Checklist

- [ ] The tag exists on the intended commit and the **Target** matches the agreed release branch.
- [ ] The release title matches the tag.
- [ ] Release notes are populated, edited for clarity, and reference the correct comparison range.
- [ ] All expected assets are attached and download cleanly.
- [ ] **Set as the latest release** is correct for this release type.
- [ ] **Set as a pre-release** is correct for this release type.
- [ ] The release is published (or is intentionally still a draft with an owner and a target publish date).
- [ ] The associated milestone (GHE-ALM-041) is closed or scheduled to close after deployment.

## Common Mistakes

- Publishing the release before all PRs in scope are merged, leaving the comparison range incomplete.
- Tagging `main` when the release was cut from a `release/*` branch, which produces a tag at the wrong commit.
- Leaving **Set as the latest release** selected on a back-port and demoting the true latest.
- Treating a draft as if it is published; non-admins cannot see drafts and downstream automations are not triggered until publish.
- Editing release notes after announcement instead of issuing a small follow-up release, which hides the change history.
- Attaching assets manually when the build pipeline already publishes them, creating two sources of truth.
- Forgetting to confirm the **Previous tag** when generating notes, which produces a comparison range that is too wide or too narrow.

## Escalation Path

- GitHub administrator: Tag protection rule prevents you from creating or moving the required tag.
- Repository administrator: You lack Write permission to draft, or the release commit is on a branch you cannot tag.
- Engineering lead: A merged PR is in scope but is missing from the auto-generated notes, or the release commit does not match the planned scope.
- Release manager: Latest/pre-release designation is disputed, or a published release must be retracted.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-046 : How to Prepare a Release Readiness Review
- GHE-ALM-048 : How to Use Automatically Generated Release Notes
- GHE-ALM-050 : How to Close a Release After Deployment
- GHE-ALM-066 : How to Review Deployment History
