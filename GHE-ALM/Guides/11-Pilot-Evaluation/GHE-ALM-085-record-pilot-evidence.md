# How to Record Pilot Pass/Fail Evidence

**Guide ID:** GHE-ALM-085
**Audience:** Project Manager, Engineering Manager, Program Manager
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 30-45 minutes per scenario, plus a 60-minute final compilation
**Required permissions:** Project: Write; Repository: Triage on the pilot repositories; permission to edit the pilot scorecard wiki page or shared document
**Prerequisites:**

- The pilot is running under GHE-ALM-079, with a Project, repositories, sprints, and a release candidate already in place.
- An evidence storage location has been agreed: a wiki page, a shared document, or a dedicated folder.
- The seven pass criteria from the pilot evaluation plan are confirmed: work hierarchy, sprint execution, bug workflow, release tracking, reporting, non-engineer usability, and governance.

**When to use this guide:** Use this guide every time you complete a pilot scenario (GHE-ALM-080 through GHE-ALM-084), and again at the end of the pilot to compile the final scorecard. Each scenario sub-guide ends with a step that says "record evidence per GHE-ALM-085"; this is that step.

**When not to use this guide:** Do not use this guide for routine sprint or release reporting after the pilot is over. For ongoing operational reporting use GHE-ALM-054 and GHE-ALM-055.

## Outcome

By the end of this guide, you will have produced:

- A populated evidence record for one or more pass criteria, with a link, screenshot, or short metric per criterion.
- A pass/fail justification for each criterion that is short, specific, and verifiable.
- A consolidated pilot scorecard that the steering group can read in fifteen minutes and use as the basis for the adoption decision.

## Before You Start

- The pilot Project, for example `acme-payments ALM Pilot`, is open in your browser.
- The pilot scorecard storage location is created and you have edit access.
- Add a Project text field named `Evidence link` if it does not already exist. This field carries the per-item evidence URL on issues you cite as proof.
- The seven pass criteria are listed in the scorecard as separate rows or sections.
- Decide the rule up front and write it at the top of the scorecard: a pass without evidence is not a pass.

## Steps

### Define the evidence record format

1. Open the pilot scorecard. Confirm it has one row per pass criterion, with columns for `Result` (Pass, Partial, Fail), `Evidence`, `Justification`, and `Owner`.
2. For each criterion, decide which evidence type fits best. Use a link to a Project view or filtered query when the evidence is "this set of items behaves correctly". Use a screenshot when the evidence is a visual layout, such as Hierarchy View or the roadmap. Use a short metric when the evidence is quantitative, such as "12 of 14 sprint items closed in iteration".
3. Standardize the evidence file naming so the steering group can find it later. Use the pattern `GHE-ALM-085-<criterion>-<artifact>.png` or `.md`, for example `GHE-ALM-085-sprint-execution-board.png`.
4. Decide where files live. A dedicated folder in the pilot wiki or a shared drive folder both work. Pick one and link it from the scorecard header.

> [SCREENSHOT: pilot scorecard table with the seven criteria rows and Result, Evidence, Justification, Owner columns visible]

### Capture per-criterion evidence

5. Capture **work hierarchy** evidence. Open the pilot Project, switch to Hierarchy View, and filter to the pilot epic. Save the view as `Pilot - Hierarchy`. Take a screenshot showing the epic, its features, requirements, tasks, and at least one sub-task chain. Paste the saved view link into the scorecard `Evidence` cell.
6. Capture **sprint execution** evidence. Open the current sprint board, filtered by `sprint:@current`. Take a screenshot at sprint close showing the **Done** column. In the scorecard, record the metric `<closed>/<committed> items closed`, for example `12/14 items closed`. Add the saved view link.
7. Capture **bug workflow** evidence. Open the bug triage view. Pick three bugs that traversed the full lifecycle from `New` through `Verified` or `Done`. Copy each issue URL into the `Evidence` cell, one per line. Optionally screenshot the timeline of one bug to demonstrate the status transitions and the linked PR.
8. Capture **release tracking** evidence. Open the release candidate's milestone page and the GitHub Release draft or published page. Capture both URLs. Take a screenshot of the roadmap layout grouped by `Release` showing the candidate's scope. Note the metric `<merged PRs>/<scoped issues>` to show traceability coverage.
9. Capture **reporting** evidence. Open Project Insights. Capture three charts that the steering group cares about: open vs closed by sprint, work by `Product Area`, and bugs by `Severity`. Save each chart and link it. If a leadership question cannot be answered with native charts, write that gap into the `Justification` cell rather than hiding it.
10. Capture **non-engineer usability** evidence. During the pilot, two pilot users who are not engineers should each log a 5-question survey response: time to find their first issue, time to file a bug, time to find sprint status, perceived clarity of fields, and one open comment. Paste the survey results into the scorecard, anonymized if needed.
11. Capture **governance** evidence. Open the rulesets page and the CODEOWNERS file for one pilot repository. Screenshot the ruleset summary showing required reviews, status checks, and protected branches. Link to a closed PR that shows the rules were enforced. Reference GHE-ALM-074 for the review approach.

> [SCREENSHOT: a representative evidence file, for example the sprint board at close with Done column populated]

### Write the pass/fail justification

12. For each criterion, write the `Justification` cell as one or two sentences in this shape: criterion verdict, the specific behavior observed, and the evidence reference. Example: "Pass. Hierarchy View rendered the full epic-to-task chain for the pilot epic, including 4 features, 11 requirements, and 27 tasks. See `Pilot - Hierarchy` saved view and `GHE-ALM-085-hierarchy-epic.png`."
13. If a criterion is `Partial`, name the specific gap and what would convert it to a pass. Example: "Partial. Insights produced sprint burn-up but did not produce a per-owner workload chart that leadership requested. Gap addressable via saved view per GHE-ALM-058 or external BI per GHE-ALM-056."
14. If a criterion is `Fail`, name the blocking behavior and the affected workflow. Do not soften the language. The steering group needs the failure to be readable in one pass.
15. Apply the rule. If a row's `Evidence` cell is empty, change `Result` to `Fail` regardless of what the team felt happened. A pass without evidence is not a pass.

### Compile the final scorecard

16. After all five pilot scenario sub-guides are complete, open the scorecard and review every row for completeness: `Result`, `Evidence`, `Justification`, and `Owner` populated.
17. Add a one-paragraph executive summary at the top of the scorecard: number of criteria passed, partial, and failed; the headline recommendation (adopt, adopt with conditions, do not adopt); and the date of compilation.
18. Add an appendix that lists every evidence artifact with its file path or URL, ordered by criterion. This is what auditors and the steering group will follow when they question a verdict.
19. Share the scorecard link with the pilot steering group and the owners of GHE-ALM-079. Schedule the adoption decision meeting only after the scorecard is shared and circulated.

> [SCREENSHOT: the final scorecard summary block with passed, partial, and failed counts and the recommendation paragraph]

## Validation Checklist

- [ ] Every pass criterion row has a non-empty `Evidence` cell.
- [ ] Every `Pass` verdict cites a link, screenshot, or metric.
- [ ] Every `Partial` and `Fail` verdict names a specific behavior, not an opinion.
- [ ] Evidence files follow the naming convention and live in the agreed location.
- [ ] The executive summary names a clear recommendation.
- [ ] The scorecard link has been shared with the steering group.

## Common Mistakes

- Recording "team agreed it works" with no link, screenshot, or metric. Treat as `Fail` and re-capture.
- Pasting a screenshot of a private browser tab that the steering group cannot reproduce. Always link the saved Project view alongside any screenshot.
- Combining several criteria into a single justification paragraph. Keep one verdict per criterion so the steering group can read row by row.
- Inflating partial results to pass to avoid awkward conversations. Partial and fail verdicts are the most valuable output of the pilot; they tell the organization what to fix before adoption.
- Storing evidence in personal folders or chat threads. Use the agreed wiki page, shared drive folder, or Project field so artifacts survive after the pilot.

## Escalation Path

- GitHub administrator: when an evidence capture requires saving an organization-level view or exporting Project data and you lack the permission.
- Repository administrator: when ruleset or CODEOWNERS evidence requires opening repository settings.
- Engineering lead: when a `Partial` or `Fail` verdict on bug workflow, traceability, or governance requires engineering interpretation before the steering meeting.
- Release manager: when release tracking evidence depends on milestone, deployment, or release-note artifacts you cannot finalize alone.

## Related Guides

- GHE-ALM-079 : How to Run the GitHub Enterprise ALM Pilot Evaluation
- GHE-ALM-080 : How to Test Feature and Requirement Decomposition
- GHE-ALM-081 : How to Test Sprint Planning and Execution
- GHE-ALM-082 : How to Test Bug Intake and Triage
- GHE-ALM-083 : How to Test Release Tracking
- GHE-ALM-084 : How to Test Leadership Dashboard Sufficiency
