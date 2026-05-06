# How to Export or Request Exported Project Data

**Guide ID:** GHE-ALM-057
**Audience:** Project Manager, Engineering Manager, Program Manager
**Primary role:** Project Manager
**Classification:** Manager Performs / Manager Requests
**Estimated time:** 10-20 minutes for a one-off view export; 1-2 hours of coordination for an API or BI extraction request
**Required permissions:** Project: Read for export from a view; access to a developer or DevOps engineer for API/BI extraction
**Prerequisites:**

- The Project contains the items you want to report on, with the fields you need populated.
- You know which Project view holds the right slice of data, or you are willing to create a new view first.
- For API or BI extraction, you have a developer, DevOps engineer, or BI analyst who can run GraphQL queries or build a pipeline.

**When to use this guide:** Use this guide when you need GitHub Project data outside the GitHub UI: a one-off spreadsheet for a leadership deck, a slide table for a release readiness review, or a recurring extract for a BI dashboard.

**When not to use this guide:** Do not use this guide when a Project Insights chart or a saved view in GitHub already answers the question. Do not use this guide to move sensitive data into uncontrolled spreadsheets that bypass governance.

## Outcome

By the end of this guide, you will have produced:

- A TSV or CSV file containing the items and fields visible in a chosen Project view, opened in Excel or Google Sheets.
- Or, a written extraction request to a developer or DevOps engineer covering scope, fields, cadence, and destination.

## Before You Start

- Confirm which Project and which view contain the data you need. The export reflects the current view's columns, filters, sort, and grouping.
- Confirm the canonical field names you want in the output: `Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`.
- Decide whether a one-off export is enough, or whether you need a recurring API or BI extraction.
- Know who receives the data and where it will live. Do not export sensitive issue content into uncontrolled storage.

## Steps

### Decide between view export and API extraction

1. Choose **view export** if the request is one-off, the data fits inside a single Project view, you can answer the question with the fields already on items, and the audience can read a spreadsheet.
2. Choose **API or BI extraction** if you need data from multiple Projects, custom calculations such as cycle time or lead time, historical snapshots, scheduled refresh into a dashboard, or joins with non-GitHub data such as finance or support.

### Export the current view to a file

3. Open the Project. In the view tabs at the top, click the view that contains the items and fields you want. If you do not have a suitable view, create one first using filters and column selection that match the report scope. See GHE-ALM-058 for stakeholder-ready views.
4. Switch the view layout to **Table**. The export only operates on the table layout.
5. Apply the filters, sort, and grouping you want reflected in the file. The export captures what the table currently shows.
6. Click the view name to open the view menu, or click the **...** (more options) menu next to the view tabs.
7. Select **Export view data**. GitHub generates a TSV file (tab-separated values) containing one row per item and one column per field visible in the view.
8. Save the file. Most browsers download it to your default downloads folder. The filename matches the view name.

> [SCREENSHOT: Project table view with the view menu open and **Export view data** highlighted]

### Open and clean the file

9. Open the file in Excel, Google Sheets, or Numbers. Both tools can import TSV: in Excel, use **Data > From Text/CSV** and select the tab delimiter; in Sheets, use **File > Import** and select tab.
10. Convert the file to `.xlsx` or `.csv` if your downstream consumer requires that format.
11. Remove any columns the audience does not need, and confirm date fields parsed correctly.
12. Save the working file in a controlled location: a managed SharePoint, Drive, or Confluence space. Do not email raw exports of sensitive issues.

> [SCREENSHOT: Exported TSV opened in Excel with Project columns visible]

### Request an API or BI extraction when the view export is not enough

13. Decide the scope: which Project or Projects, which fields, which item types, what date range, and any filters such as `Release` or `Product Area`.
14. Decide the cadence: one-off snapshot, daily refresh, weekly refresh, or on-demand.
15. Decide the destination: a shared spreadsheet, a database, a BI dashboard such as Power BI, Tableau, or Looker, or a data warehouse table.
16. Decide the access model: who can read the destination, and whether issue body text is included or only structured fields.
17. Send the request to your developer, DevOps engineer, or BI analyst using the sample request below.

## Sample Request to Send

Use the following template as the body of a message, ticket, or issue to your developer or DevOps contact.

```
Subject: GitHub Project data extraction request - <short name>

Project URL: https://github.com/orgs/acme-payments/projects/12
Project name: acme-payments Release 2026.05.0 ALM Project

Scope:
- Items: open and closed issues; exclude pull requests.
- Item types: Feature, Requirement, Task, Bug.
- Filter: Release = 2026.05.0 OR Sprint in (Sprint 26, Sprint 27).
- Date range for closed items: closed in last 90 days.

Fields needed (use canonical names):
- Title, Number, Repository, Assignees, State, Created, Updated, Closed
- Status, Priority, Severity, Effort, Sprint, Release, Product Area, Owner
- Start Date, Target Date
- Parent issue (for hierarchy)
- Linked pull requests (URL list)

Cadence: daily refresh at 06:00 local time.

Destination: Power BI dataset "acme-payments-alm" in workspace "Engineering Reporting".

Access: Engineering Management group; do not include issue body text.

Reason for request: weekly release health and monthly sprint completion
reporting for acme-payments leadership. Native Project Insights cannot
join across the Sprint and Release dimensions in a single chart.

Contact: <your name>, <your email>.
Needed by: <date>.
```

## Validation Checklist

- [ ] The exported file opens in Excel or Sheets and shows one row per item.
- [ ] Field columns match the view's visible columns, including custom fields.
- [ ] Filters, sort, and grouping in the exported file reflect the view at the time of export.
- [ ] Sensitive items are not exported into uncontrolled storage.
- [ ] For API requests, the scope, fields, cadence, destination, and access model are written down before the developer starts work.

## Common Mistakes

- Exporting from a board, roadmap, or insights layout. Only the table layout supports export.
- Exporting before applying the right filters, then editing the spreadsheet by hand. Filter the view first, then export.
- Treating the exported file as live data. The export is a point-in-time snapshot; rerun it when leadership asks for current numbers.
- Asking for an API extraction when a saved view and a Project Insights chart would answer the question. See GHE-ALM-051, GHE-ALM-052, and GHE-ALM-058 first.
- Sending raw exports by email to wide distribution lists. Place the file in a controlled location and share a link.
- Omitting the destination and access model from the API request. Developers will stop and ask, which delays delivery.

## Escalation Path

- GitHub administrator: when the Project itself is missing fields or items needed for the report, or when organization-level field changes are required.
- Repository administrator: not applicable.
- Engineering lead: when an API or BI extraction needs to join GitHub data with code, deployment, or other engineering datasets, or when scheduled GraphQL access requires a service account or fine-grained personal access token.
- Release manager: when the export feeds release readiness or release governance reporting and field definitions need to align with release process.

## Related Guides

- GHE-ALM-051 : How to Create and Interpret Project Insights Charts
- GHE-ALM-052 : How to Configure Chart Filters and Axes
- GHE-ALM-056 : How to Identify Reporting Gaps That Require BI or External Tools
- GHE-ALM-058 : How to Use Saved Views for Stakeholder Reporting
