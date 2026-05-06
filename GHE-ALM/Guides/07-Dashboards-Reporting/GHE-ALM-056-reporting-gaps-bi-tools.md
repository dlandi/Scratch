# How to Identify Reporting Gaps That Require BI or External Tools

**Guide ID:** GHE-ALM-056
**Audience:** Engineering Manager, Program Manager, Project Manager
**Primary role:** Engineering Manager
**Classification:** Manager Reviews / Manager Requests
**Estimated time:** 30-45 minutes per quarterly review, plus a 15-minute check whenever a new report is requested
**Required permissions:** Project: Read on the relevant Projects; permission to file a request with the BI team or GitHub administrator
**Prerequisites:**

- Your team is using a GitHub Project for ALM tracking with `Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, and `Product Area` populated.
- You have completed at least one weekly dashboard review using GHE-ALM-054.
- You have a written list of the reports your stakeholders ask for (PMO, executive dashboard, regulatory, finance).

**When to use this guide:** Use this guide when a stakeholder asks for a chart, metric, or rollup that you cannot reproduce in Project Insights, or when you are scoping reporting needs at the start of a release, quarter, or pilot.

**When not to use this guide:** Do not use this guide to build the missing report yourself. Use it to decide whether the report should be built natively, via export, or by the BI team, and to write a clean request.

## Outcome

By the end of this guide, you will have produced:

- A written list of reports your stakeholders need, each tagged as native-fit, export-fit, or BI-fit.
- A short request to send to the BI team or GitHub administrator for any report tagged BI-fit, including data scope, metric, consumer, cadence, and export path.
- A reporting gap log you can revisit at the next quarterly review.

## Before You Start

- The list of stakeholder reporting requests you have received in the last quarter.
- Access to your organization Project and its Insights tab.
- The name and intake channel for your BI team, data platform team, or GitHub administrator.
- Knowledge of which fields in your Project are reliable enough to report on (see GHE-ALM-076).

## Steps

### Triage the request against native capability

1. Restate the request in one sentence: who is asking, what metric, what scope, what cadence. For example: "PMO needs sprint velocity per team, last 6 sprints, refreshed weekly."
2. Open the Project and check whether the metric can be answered by an existing Insights chart, a saved view, or a roadmap. Try native first. Many requests are really for charts that already exist (see GHE-ALM-051, GHE-ALM-054, GHE-ALM-055).
3. If the request maps to a native chart, send the stakeholder the saved view or chart link and stop. There is no gap.
4. If the request does not map to a native chart, score it against the gap triggers in the next phase.

### Score the request against gap triggers

5. Mark the request as a likely gap if any of the following apply:
   - It asks for **velocity, sprint burndown, burnup, cumulative flow, cycle time, or lead time**. Project Insights does not produce these in the form most PMOs expect.
   - It is a **regulatory or audit report** with a fixed schema, retention period, or signed-off format.
   - It rolls up across **multiple Projects** or across organizations (for example, portfolio-level dashboard).
   - It needs a **custom calculation** Insights cannot express (weighted scoring, blended cost, joined data with HR or finance systems).
   - It needs **historical reconstruction** beyond the Project's recorded history.
   - The consumer is an **executive dashboard** built in Power BI, Tableau, Looker, or a similar BI tool that already aggregates data from other systems.
6. Apply the decision matrix in the next phase to choose the right path.

### Apply the decision matrix

7. Use the matrix below to choose between native, export, and BI. Always start at the top row and only move down when the row above does not fit.

| Path | Use when | Cost and risk | Owner |
|---|---|---|---|
| **Native (Insights, saved views, roadmap)** | Metric exists or can be approximated by a chart, saved view, or roadmap layout in the Project. | Lowest. No new tooling. | You, with help from the project admin. |
| **Project export (CSV) plus a spreadsheet** | One-off or low-frequency report. Small data volume. Stakeholder can read a spreadsheet. Cycle time, lead time, and basic velocity can be derived from a CSV. | Low. Manual refresh. Risk of stale data if anyone treats the spreadsheet as a live system of record. | You or a delegate, using GHE-ALM-057. |
| **API or GraphQL extraction into a BI tool** | Recurring report, multi-Project rollup, joined data, executive dashboard, regulatory schema, or any case where the consumer needs scheduled refresh. | Highest. Requires BI engineering capacity, scheduling, monitoring, access governance, and ongoing maintenance. | BI team or data platform team, with you as requester. |

8. Write the chosen path next to each request in your gap log.

> [SCREENSHOT: Reporting gap log spreadsheet showing one column for the request, one for the chosen path (Native / Export / BI), and one for the owner.]

### Compare native to external fit before requesting BI work

9. Before sending a request to BI, confirm the gap is real. Use the table below as the inspection rubric. If the right column applies, escalate. If the left column applies, stay native.

| Reporting need | What good looks like in GitHub native | What to escalate to export or BI |
|---|---|---|
| Open work by status, owner, sprint, release, product area | Project table view, board view, or grouped table covers it. Insights chart by status or owner is sufficient. | Stakeholder needs the same view sliced across more than one Project, or joined to non-GitHub data. |
| Trend of open vs closed over time | Insights historical chart shows the trend (see GHE-ALM-053). | Stakeholder needs the trend recomputed against a custom snapshot date or a regulatory window. |
| Sprint burndown or burnup with team capacity overlay | Insights can approximate using a status chart filtered by `sprint:@current`. Acceptable for team standup use. | PMO requires a true burndown or burnup widget with capacity, scope-change events, and forecast lines. Native cannot produce this; route to export or BI. |
| Velocity (story points completed per sprint) | None. Insights does not compute per-sprint completed-effort sums in the form a PMO expects. | Always escalate. Export the Project to CSV and aggregate, or request a BI metric. |
| Cycle time and lead time | None natively. | Always escalate. Derive from issue and PR timestamps via API or GraphQL. |
| Cumulative flow diagram | None natively. | Always escalate. Requires daily snapshots of status counts. |
| Cross-Project portfolio rollup | Limited. Filtering by Project requires repeating views. | Escalate when more than two Projects need the same rollup. |
| Regulatory or audit report | Saved view with a frozen filter set may be acceptable for evidence capture. | Escalate when a fixed schema, retention period, or signature is required. |
| Executive dashboard combining engineering, finance, and customer data | Not in scope for native. | Always escalate to BI. |

### Scope the external request

10. For each request you have tagged as export or BI, write down five things before you send anything:
    - **Data scope:** which Projects, repositories, organizations, and time window.
    - **Metric:** the exact calculation, in plain words. For velocity: "sum of `Effort` for items where `Status = Done` and `Sprint = <sprint name>`, grouped by Sprint, last 6 sprints."
    - **Consumer:** who will read the report (PMO, exec dashboard, finance, audit) and where the report will live (Power BI workspace, Tableau site, Looker folder, shared spreadsheet).
    - **Cadence:** how often the report must refresh (one-off, weekly, daily, on demand).
    - **Export path:** your initial guess at the source (Project CSV export, REST API, GraphQL API, third-party connector). The BI team will confirm or change this.
11. Confirm the metric definition with the requesting stakeholder in writing before sending. A wrong metric definition is the most common cause of rebuilt dashboards.

## Sample Request to Send

Send this to your BI team or GitHub administrator for any request tagged BI-fit. Replace bracketed values.

> Subject: GitHub ALM reporting request: [metric name] for [consumer]
>
> Hello,
>
> The [PMO / exec team / audit team] needs a recurring report that I cannot produce from native GitHub Project Insights. Please review and advise on feasibility, owner, and timeline.
>
> - **Requesting role:** [Engineering Manager, acme-payments]
> - **Consumer:** [PMO weekly portfolio review, Power BI workspace `acme-pmo`]
> - **Metric:** [Sprint velocity. Definition: sum of `Effort` for items where `Status = Done` and `Sprint = <sprint name>`, grouped by Sprint, for the last 6 sprints, per team.]
> - **Data scope:** [Organization `acme-payments`, Projects `Checkout ALM` and `Billing ALM`, Sprints from `Sprint 22` through `Sprint 27`.]
> - **Cadence:** [Weekly refresh, Monday 08:00 local.]
> - **Suggested export path:** [GraphQL `projectV2` query against the two Projects; alternatively the Project CSV export run on a schedule. Open to your recommendation.]
> - **Why native is insufficient:** [Project Insights does not compute completed-effort sums per Sprint in a form the PMO can chart, and the report must combine two Projects.]
> - **Decision needed by:** [date]
> - **Related guides:** GHE-ALM-051, GHE-ALM-057.
>
> Thanks.

## Validation Checklist

- [ ] Each stakeholder request is tagged native, export, or BI in your gap log.
- [ ] Native-fit requests are answered with a saved view or chart link, not a new build.
- [ ] Each export-fit or BI-fit request has a written metric definition agreed by the requester.
- [ ] Each BI-fit request has been sent to the BI team using the sample request format.
- [ ] Your gap log has an owner and a review date for each open item.
- [ ] You revisit the gap log at the start of each quarter and close items that are no longer needed.

## Common Mistakes

- Routing every reporting request to BI without trying native first. Most requests can be answered by a saved view or an Insights chart.
- Asking for a velocity or burndown report without first agreeing the metric definition in writing with the stakeholder.
- Treating a one-off CSV export as a permanent reporting solution. Recurring reports need a recurring data source.
- Approving a BI build before confirming the underlying Project fields are reliable. Reports built on inconsistent `Status`, `Effort`, or `Sprint` data will be wrong regardless of the BI tool.
- Sending sensitive Project data to a BI workspace without confirming access controls match the data sensitivity.
- Reconstructing the same report in three different tools because the consumer was never named.

## Escalation Path

- GitHub administrator: when the export path requires API or GraphQL credentials, scoped tokens, or rate-limit increases.
- Repository administrator: not applicable for most reporting work; involve only when repository-scoped data is needed and current permissions block export.
- Engineering lead: when the request implies new fields or new hygiene rules in the Project that the team must maintain.
- Release manager: when the report is release-scoped and must align with the release readiness review (see GHE-ALM-046).

## Related Guides

- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-054 : How to Run a Weekly ALM Dashboard Review
- GHE-ALM-055 : How to Run a Monthly ALM Metrics Review
- GHE-ALM-057 : How to Export or Request Exported Project Data
- GHE-ALM-058 : How to Use Saved Views for Stakeholder Reporting
