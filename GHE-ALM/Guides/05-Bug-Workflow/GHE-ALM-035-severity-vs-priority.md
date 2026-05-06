# How to Distinguish Severity from Priority

**Guide ID:** GHE-ALM-035
**Audience:** Engineering Manager, QA Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Understands / Manager Performs
**Estimated time:** 20-minute concept review, then 2-5 minutes per bug at triage
**Required permissions:** Repository: Triage; Project: Write
**Prerequisites:**

- Familiarity with the bug intake form and the `Severity` and `Priority` project fields.
- Access to the bug triage view for your product.

**When to use this guide:** Use this guide before your first bug triage session, and any time the team disagrees about how a bug should be ranked, scheduled, or escalated.

**When not to use this guide:** Do not use this guide to set release scope, sprint capacity, or hotfix policy. Those decisions consume Severity and Priority as inputs but are governed by the release and sprint guides.

## Outcome

By the end of this guide, you will have produced:

- A shared understanding of what `Severity` and `Priority` each measure, and what they do not measure.
- A repeatable method for assigning both fields independently when triaging a bug.
- A worked matrix you can reuse to defend triage decisions in stakeholder discussions.

## Before You Start

- Confirm with QA leadership which numeric scale your team uses. The scale shown below is illustrative.
- Confirm that the project has both a `Severity` field and a `Priority` field. If only one exists, see GHE-ALM-024 to request the missing field.
- Have a recent open bug ready to triage so you can apply the model immediately.

## Steps

### 1. The Core Distinction

`Severity` and `Priority` answer two different questions about the same bug. They are set independently and they can move independently over time.

- `Severity` answers: how badly does this defect hurt a user or the system when it occurs? It is a property of the defect itself. Severity is owned by QA and engineering.
- `Priority` answers: how urgently does the business need this fixed, given everything else competing for the team's attention? It is a property of the schedule. Priority is owned by the product owner or engineering manager.

A bug can be high severity and low priority. A bug can also be low severity and high priority. Treating the two fields as the same value is the most common triage error and it produces inconsistent decisions across teams and releases.

### 2. The Illustrative Scale

The training materials use a common 1-4 / P0-P3 scale. Confirm your team's actual scale with QA leadership before applying it in production triage.

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

Read each column independently. Severity is judged against the user or system. Priority is judged against the calendar and the backlog.

### 3. Inputs to Each Field

When you set `Severity`, weigh:

- Number of users affected and whether the affected users are on a critical path.
- Loss of data, money, or compliance posture.
- Whether a workaround exists, and how painful that workaround is.
- Whether the failure is reproducible or rare.

When you set `Priority`, weigh:

- Customer commitments, contractual SLAs, and visible demos.
- Release scope and whether the release can ship without the fix.
- Cost of delay versus cost of context-switching the team away from current sprint work.
- Whether a known fix or workaround is already deployed.

A defect that locks every user out of `acme-checkout` in production is `Severity` 1 and `Priority` P0. A typo on an internal admin screen is `Severity` 4 and `Priority` P3. The interesting cases sit in between.

### 4. Worked Examples

Use these examples to calibrate. Tenant names are illustrative.

| Scenario | Severity | Priority | Why the values differ |
|---|---|---|---|
| `acme-checkout` payments page returns 500 for all users in production | 1 | P0 | Total outage; revenue stops; hotfix the same day. |
| Rare crash in the `acme-payments` refund flow, hits one customer per week, no data loss | 2 | P2 | High user impact when it occurs, but low frequency and a manual workaround exists; schedule into a normal sprint. |
| Misaligned button on the public marketing page two days before a board demo | 4 | P0 | Trivial defect, but the demo audience makes business urgency very high; fix today. |
| `acme-platform` admin console shows a stale cache value that refreshes within 30 seconds | 3 | P3 | Minor functional defect, easy workaround; backlog and address in normal grooming. |
| Memory leak in the `acme-checkout` background worker that requires a weekly restart | 2 | P1 | Major defect with operational pain, but a cron-based restart hides it from customers; commit to current sprint. |
| Typo in an error message visible only to internal staff | 4 | P3 | Cosmetic and internal-only; no business urgency. |

The pattern: `Severity` reflects what the defect does. `Priority` reflects what the business is willing to pay to make it stop.

### 5. Applying Both Fields at Triage

When a new bug arrives in the triage view, set the two fields in this order.

1. Open the bug from the triage view (see GHE-ALM-034).
2. Read the reproduction steps, customer impact, and any attached evidence. Set `Severity` based on the defect itself, ignoring the calendar.
3. Re-read the customer impact and target release. Set `Priority` based on what else the team is doing this sprint and this release.
4. If `Severity` and `Priority` are both at the top of the scale, route the bug to the hotfix path described in GHE-ALM-040 rather than the standard sprint queue.
5. If your assigned `Priority` is two or more steps away from `Severity` (for example, `Severity` 1 with `Priority` P3), add a one-line triage comment explaining why. This protects the next reviewer from assuming the bug was mis-triaged.
6. Save the item. The bug now appears correctly grouped in the triage view and weekly bug review.

> [SCREENSHOT: bug detail view with Severity and Priority fields populated, showing both values selected independently in the side panel]

### 6. When the Two Fields Drift Apart

Severity is largely fixed at the moment the defect is filed. Priority changes as the business context changes.

- A `Severity` 3 bug can become `Priority` P0 if a regulator audit, a major customer escalation, or a release demo lands on top of it.
- A `Severity` 1 bug can drop to `Priority` P2 if a workaround is shipped, customer traffic is rerouted, or the affected feature is deprecated.
- Reset `Priority` during the weekly bug review (see GHE-ALM-039) when the schedule, customer commitments, or release scope change. Do not change `Severity` unless new evidence about the defect itself arrives.

## Validation Checklist

- [ ] You can state, in one sentence each, what `Severity` and `Priority` measure on your team.
- [ ] You can produce two examples in your own backlog where the two fields disagree by at least one step, and explain why.
- [ ] Every bug you triaged today has both fields set, not just one.
- [ ] When `Severity` and `Priority` differ by two or more steps, the bug carries a triage comment explaining the gap.
- [ ] The team's hotfix routing rule is tied to the combination of both fields, not to either field alone.

## Common Mistakes

- Treating the two fields as synonyms and copying the same value into both. This collapses the model and produces inconsistent triage across reviewers.
- Letting customer noise drive `Severity`. Loud customer pressure is a `Priority` signal, not a `Severity` signal.
- Letting engineering effort drive `Severity`. A defect that is hard to fix is not automatically high severity. Effort lives in the `Effort` field.
- Inflating `Severity` to get a bug fixed faster. Use `Priority` for urgency. Severity inflation poisons release health charts.
- Failing to revisit `Priority` weekly. Stale priorities cause the team to work on bugs that no longer matter.
- Promoting any bug with `Severity` 1 directly to a hotfix without confirming `Priority` P0. Some Severity 1 defects affect features that are not in the current release scope.

## Escalation Path

- GitHub administrator: Not applicable.
- Repository administrator: Involve only if the `Severity` or `Priority` fields are missing from the project and need to be added at the organization level (see GHE-ALM-024).
- Engineering lead: Involve when the team cannot agree on `Severity` for a defect, especially around reproducibility, data loss, or compliance impact.
- Release manager: Involve when `Priority` for a defect would change the current release scope or trigger a hotfix.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-034 : How to Use the Bug Triage View
- GHE-ALM-036 : How to Move a Bug Through the Defect Workflow
- GHE-ALM-039 : How to Run a Weekly Bug Review
