# How to Prepare a Release Readiness Review

**Guide ID:** GHE-ALM-046
**Audience:** Release Manager, Engineering Manager, QA Manager
**Primary role:** Release Manager
**Classification:** Manager Performs
**Estimated time:** 45-60 minutes the day before the release decision meeting
**Required permissions:** Project: Read (to filter views and read fields); Repository: Read (to open milestones, pull requests, and the draft release); Repository: Triage (only if you need to set or correct a `Release` field value during preparation)
**Prerequisites:**

- The release identifier exists as a value in the `Release` project field and, where used, as a repository milestone. See GHE-ALM-041.
- A current health read has been completed against the Roadmap and Insights. See GHE-ALM-045.
- A draft GitHub Release exists or is in progress for each repository in scope. See GHE-ALM-047.
- The defect severity scale and the team's waiver process are documented and accessible.

**When to use this guide:** Use this guide to assemble the readiness scorecard you will present at the go or no-go meeting for a versioned release, typically 24 to 48 hours before the planned release date.

**When not to use this guide:** Do not use it for the weekly health scan; that is GHE-ALM-045. Do not use it after the release has shipped; release closeout is GHE-ALM-050. Do not use it to draft the release notes themselves; that is GHE-ALM-047.

## Outcome

By the end of this guide, you will have produced:

- A completed readiness scorecard covering scope, traceability, validation, release notes, defects, and deployment readiness for the target release.
- A list of in-scope items that fail any criterion, with named owners and a remediation deadline.
- A recommended go, conditional go, or no-go call ready to present at the release decision meeting.

## Before You Start

- Confirm the release identifier, for example `2026.05.0`, the planned release date, and the meeting time.
- Open the organization Project for the product, with each contributing repository in adjacent tabs: for example `acme-payments/payments-api`, `acme-checkout/checkout-service`, `acme-platform/web-client`.
- Have the defect severity scale on hand. A common 1-4 / P0-P3 scale is used below; confirm your team's actual scale with QA leadership.
- Have the team's non-code disposition convention on hand: the label, comment template, or `Disposition` field used when an issue closes without a linked pull request. See GHE-ALM-060.

## Steps

### Confirm release scope

1. In the organization Project, open a table view filtered by `Release:"<your release>"`. If the filter returns zero rows, confirm the `Release` field value spelling; release scope must be readable from one filter.
2. Group the table by **Status**. Record the counts for `Done`, `In Progress`, `Blocked`, and any pre-Done statuses. The sum is the in-scope total.
3. Open each contributing repository's milestone for this release. Confirm the milestone title matches or maps to the `Release` field value. Record the open and closed counts.
4. Reconcile Project and milestone counts. Items in the Project with `Release` set but missing from the repo milestone, or vice versa, are scope drift; resolve them now or list them as scorecard exceptions.

> [SCREENSHOT: Project table filtered by Release and grouped by Status, with the Done, In Progress, and Blocked counts visible]

### Verify issue-to-pull-request traceability

5. Switch the filter to `is:closed Release:"<your release>"`. For every closed in-scope item, confirm one of two conditions per GHE-ALM-060: a linked merged pull request appears in the **Development** sidebar, or a recorded non-code disposition is present using the team's convention.
6. List every closed item that has neither. Each one is a traceability gap; assign an owner to attach the link or record the disposition before the meeting.
7. For every still-open in-scope item, confirm the latest linked pull request status. Open with no linked pull request and no disposition this close to release is a red signal; flag for the meeting.
8. Spot-check three to five high-severity or high-visibility items end to end: issue, linked pull request, merged commit, and the corresponding entry in the draft release notes. End-to-end traceability on the items leadership will ask about matters more than surface counts.

### Confirm validation status

9. Add or unhide the field the team uses to record validation, typically a `QA Status` or `Validation` single-select, or a label such as `qa:verified`. Filter to in-scope items and read the distribution.
10. Every closed in-scope item should show validation complete: `Verified`, `Passed`, or the team's equivalent. Items closed as `Done` without validation complete are exceptions unless the team's convention explicitly allows code-only closure for that issue type.
11. Confirm regression and smoke runs have completed. Read the most recent run on the release validation workflow in **Actions**. A failed or skipped required run is a hard scorecard fail until re-run and green.
12. Record any item where validation is in progress with a named QA owner and an expected completion time. Validation in progress without an owner or time is an exception.

### Confirm release notes are drafted

13. Open the draft GitHub Release for each contributing repository. Confirm the tag name and release title match the release identifier and the team's naming convention, for example `v2026.05.0`.
14. Confirm the release notes body is populated. Auto-generated notes are acceptable when every merged pull request has a meaningful title and the team's category labels are applied. See GHE-ALM-048.
15. Read the notes against the in-scope item list. Every user-visible change should appear; internal-only changes may be summarized or omitted per team convention. Note any user-visible item missing.
16. Confirm the pre-release or latest designation matches the team's convention. Confirm release assets, for example installers or signed artifacts, are attached or scheduled to attach.

> [SCREENSHOT: Draft GitHub Release page with the tag name, release title, and populated notes body visible]

### Confirm defect status

17. In the Project's **Insights** tab, open the saved chart that breaks open bugs by `Severity` and filter to the current release with an expression such as `is:issue is:open type:Bug Release:"2026.05.0"`. See GHE-ALM-051.
18. Read the open Severity 1 / P0 count. Any open Severity 1 / P0 defect is a hard scorecard fail unless a waiver is recorded by QA leadership and the release manager and attached to the readiness packet.
19. Read the open Severity 2 / P1 count. Each one needs a documented decision: fix before release, accept as known issue with a workaround documented in the release notes, or formally defer to a follow-up release with a target.
20. Confirm Severity 3 / P2 and Severity 4 / P3 counts are within the team's carry-over threshold. Above-threshold counts are not blockers but should appear on the scorecard as a note.

### Confirm deployment readiness

21. Open the deployment workflow that promotes this release in **Actions**. Confirm the most recent run targeting the release branch or tag completed successfully. A failed or skipped deployment run is a hard scorecard fail.
22. Open the relevant environments under **Settings** then **Environments**. Confirm required reviewers, wait timers, and deployment branch or tag restrictions are in place per GHE-ALM-068. Confirm the named approvers are available during the release window.
23. Confirm rollback path. The team should have a documented rollback procedure, the previous release tag identified, and the deployment workflow known to support redeploy of the previous tag.
24. Confirm external dependencies: feature flag toggles, configuration changes, database migrations, partner cutovers, customer notifications. Each one should have an owner and a planned time relative to the release.

### Compile the scorecard and recommendation

25. Fill in the scorecard below. Mark each criterion green, yellow, or red using the table rules. Take the worst color as the overall recommendation: green is go, yellow is conditional go with named exceptions, red is no-go.
26. Write a one-page summary: release identifier, planned date, recommendation, the top three exceptions with owners and remediation deadlines, and any waivers required. Attach the scorecard.
27. Distribute the scorecard and summary to the release decision meeting attendees no later than two hours before the meeting.

> [SCREENSHOT: Completed readiness scorecard table with each criterion marked and the recommendation visible]

## Release Readiness Scorecard

| Criterion | Green | Yellow | Red |
|---|---|---|---|
| Scope confirmed | Project and milestones reconcile; no late additions | One or two reconciliation gaps with owners | Material drift, or undocumented late scope |
| Issue-to-PR traceability | Every closed item has a linked merged PR or recorded disposition | One or two gaps with owners and a fix deadline | Multiple closed items lack both PR and disposition |
| Validation status | All closed items validated; required runs green | One or two items in validation with named owner and time | Validation incomplete on multiple items, or required run failed |
| Release notes drafted | Tag, title, notes, designation, and assets complete | Minor user-visible item missing or asset pending | Notes empty or missing user-visible changes; tag or title inconsistent |
| Defect status | Zero open Severity 1 / P0; each Severity 2 / P1 has a decision | Severity 2 / P1 decisions in progress with owners | Open Severity 1 / P0 without a waiver, or Severity 2 / P1 without decisions |
| Deployment readiness | Deployment workflow green; protections set; rollback documented; external dependencies owned | One external dependency unconfirmed but owner working it | Deployment failed; reviewers unavailable; no rollback; missing protections |

## Validation Checklist

- [ ] Release scope read from both the Project filter and every contributing milestone; counts reconcile or exceptions are listed.
- [ ] Every closed in-scope item has a linked merged pull request or a recorded non-code disposition; exceptions are owned.
- [ ] Validation is complete for closed items, the required test run is green, and exceptions have a named QA owner.
- [ ] Draft release notes are populated for every contributing repository with tag, title, designation, and assets confirmed.
- [ ] Open Severity 1 / P0 count is zero or covered by a recorded waiver; every open Severity 2 / P1 has a documented decision.
- [ ] Deployment workflow last run is green, environment protections are in place, rollback path is documented, external dependencies are owned.
- [ ] Scorecard and one-page summary with the recommendation have been distributed before the meeting.

## Common Mistakes

- Reading scope from the Project alone or the milestones alone. Cross-repository releases require both; reconciliation gaps are where scope drift hides.
- Counting closed items as traceable without checking the **Development** sidebar. A closed status does not prove a linked merged pull request exists.
- Treating Severity 1 / P0 as a weighted input. It is a hard gate; conditional go is not available without a recorded waiver.
- Approving auto-generated release notes without reading them. Auto-generation only works when every merged pull request title and label was correct.
- Confirming a deployment workflow exists without confirming its last run is green against the release artifact.
- Distributing the scorecard at the meeting. Attendees cannot prepare and the meeting becomes a read-aloud.

## Escalation Path

- GitHub administrator: Not applicable. Involve only if Project filters, milestones, or release pages are inaccessible or corrupted.
- Repository administrator: Involve when a milestone or draft release sits in a repository you cannot access, or when a tag needs correction and you lack the required access.
- Engineering lead: Escalate clusters of traceability, validation, or defect exceptions in a single `Product Area`. Engineering leads own remediation before the meeting.
- Release manager: Owns the final recommendation and the waiver record for any open Severity 1 / P0 defect. If you are the release manager, the scorecard recommendation is yours.

## Related Guides

- GHE-ALM-041 : How to Track a Release with Milestones and Release Fields
- GHE-ALM-045 : How to Read Release Health from the Roadmap and Dashboard
- GHE-ALM-047 : How to Draft or Review a GitHub Release
- GHE-ALM-050 : How to Close a Release After Deployment
- GHE-ALM-060 : How to Verify Issue-to-Pull-Request Traceability
