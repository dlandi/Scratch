# How to Run the GitHub Enterprise ALM Pilot Evaluation

**Guide ID:** GHE-ALM-079
**Audience:** Engineering Manager, Program Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Performs
**Estimated time:** 8 to 10 weeks elapsed for two sprints; roughly 20 to 30 hours of manager time spread across the pilot
**Required permissions:** Organization: `Owner` or `Project creator`; Repository: `Admin` on the pilot repositories; Project: `Admin` on the pilot Project
**Prerequisites:**

- GitHub Enterprise org provisioned with the pilot team's members added.
- Issue types, project fields, issue forms, and the organization Project template defined per the Phase 1 and Phase 2 standards.
- The seven sub-guides (GHE-ALM-080 through GHE-ALM-085) reviewed by the pilot manager.

**When to use this guide:** Use this guide as the umbrella playbook when running a time-boxed evaluation pilot of GitHub Enterprise as the ALM system of record before committing to organization-wide adoption.

**When not to use this guide:** Do not use this guide for a permanent rollout, a tooling demo, or a one-team experiment that has no decision tied to it. Adoption decisions require the structured pilot scope and evidence pipeline described here.

## Outcome

By the end of this guide, you will have produced:

- A completed two-sprint pilot covering one product, two to four repositories, one team, and one release candidate.
- A pass or fail result against each of the five pilot scenarios and the seven decision criteria.
- A pilot scorecard with linked evidence (Project views, screenshots, metrics) ready for the adoption decision review.

## Before You Start

- Confirm executive sponsorship and a named decision owner for the adoption recommendation.
- Confirm the pilot team can dedicate two consecutive sprints without a major release deadline pulling focus mid-pilot.
- Identify the product, the two to four pilot repositories, the engineering team, the product owner or PM, and the candidate release.
- Reserve a shared evidence location (a Project view, a folder, or a wiki page) where the pilot scorecard and screenshots will live.
- Schedule the pilot decision review for the week after sprint two closes.

## Steps

### Define the pilot scope

1. Pick one product or application. The pilot must produce evidence about a single product end to end, not a sample across products.
2. Pick two to four repositories that belong to that product. Two is the floor for cross-repo behavior; four is the ceiling so the team is not pulled thin.
3. Pick one engineering team and one product owner or project manager. The team must be a real squad, not a volunteer cross-section, so the result reflects normal working conditions.
4. Pick two consecutive sprints (typically two weeks each) and one release candidate that lands in or shortly after sprint two.
5. Document the scope in the pilot scorecard. Use the worked example below as a template:

   - Product: `acme-checkout`.
   - Repositories: `checkout-service`, `payments-api`, `web-client`.
   - Team: Checkout Squad.
   - Product owner: one named person.
   - Sprints: `Sprint 27`, `Sprint 28`.
   - Release candidate: `2026.05.0`.

> [SCREENSHOT: pilot scorecard top section showing scope, dates, team, repos, and release]

### Stand up the pilot environment

6. Create or designate the pilot Project from the organization Project template. Do not invent fields. Use the canonical fields: `Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`.
7. Add the pilot repositories to the Project and confirm the team has Repository `Write` and Project `Write` access.
8. Confirm issue forms (feature, requirement, task, bug) are installed on each pilot repository.
9. Confirm the `Sprint` iteration field has the two pilot sprints created with correct start and end dates.
10. Walk the team through the pilot Project once, live, before sprint one starts. Do not rely on async videos for the kickoff.

### Run the five pilot scenarios

The pilot tests five scenarios. Each has a dedicated sub-guide. Run them in the order below across the two sprints. Do not collapse two scenarios into one ceremony; the evidence is per scenario.

11. Scenario 1, Feature and Requirement Decomposition. Run during sprint one planning. Follow GHE-ALM-080. The PM creates a feature, decomposes it into requirements and tasks, and walks the Hierarchy View. Pass requires feature, requirement, and task issues; full hierarchy visible; status and ownership populated.
12. Scenario 2, Sprint Planning and Execution. Run across both sprints. Follow GHE-ALM-081. The team plans `Sprint 27`, executes on the board, carries unfinished work forward, and plans `Sprint 28`. Pass requires sprint iteration, work assigned via `sprint:@current`, status flow visible, blockers visible, carryover working.
13. Scenario 3, Bug Intake and Triage. Run mid-sprint one or early sprint two. Follow GHE-ALM-082. A tester or non-engineer files a bug through the bug form. Pass requires required fields captured, severity and priority assigned, attachments working, bug visible in triage view, bug linked to release or sprint.
14. Scenario 4, Release Tracking. Run as `2026.05.0` approaches. Follow GHE-ALM-083. The team tracks scope from issues through pull requests, milestone completion, release notes, the published GitHub Release, and the deployment workflow. Pass requires `Release` field or milestone assigned, linked PRs visible, progress visible, tag created, Release published, deployment workflow visible.
15. Scenario 5, Leadership Dashboard. Run at the end of sprint two, before the decision review. Follow GHE-ALM-084. Engineering leadership reviews the executive dashboard view for release health, sprint progress, bug severity, and remaining work. Pass requires the dashboard view exists, charts cover release and sprint, bugs by severity visible, roadmap view present, data quality good enough to decide without spreadsheets.

> [SCREENSHOT: Project Hierarchy View showing one feature with requirements and tasks during scenario 1]

### Capture evidence and score the pilot

16. After each scenario, record the result in the pilot scorecard the same day. Memory decays fast; do not batch evidence at the end. Follow GHE-ALM-085 for what to capture and how.
17. For each pass criterion, capture either a Project view link, a screenshot, or a short metric. A pass without evidence is not a pass.
18. Hold a 30-minute pilot retro at the end of sprint two before the decision review. The team rates non-engineer usability and governance; these are not scenario-driven and need direct feedback.
19. Complete the Pilot Scorecard table below by mapping each criterion to its sub-guide and pass or fail answer.

#### Pilot Scorecard

| Criterion | Sub-guide | Pass / Fail Question | Result |
|---|---|---|---|
| Work hierarchy | GHE-ALM-080 | Can GitHub model the required ALM hierarchy clearly? | Pass / Fail |
| Sprint execution | GHE-ALM-081 | Can the team plan and execute sprints without workarounds? | Pass / Fail |
| Bug workflow | GHE-ALM-082 | Can bugs be logged, triaged, fixed, and verified cleanly? | Pass / Fail |
| Release tracking | GHE-ALM-083 | Can release scope and readiness be tracked end to end? | Pass / Fail |
| Reporting | GHE-ALM-084 | Are native dashboards sufficient, or is external reporting required? | Pass / Fail |
| Non-engineer usability | Pilot retro feedback | Can PMs and stakeholders use the portal without developer assistance? | Pass / Fail |
| Governance | Repository and Project review | Can standards be applied consistently across repositories and teams? | Pass / Fail |

> [SCREENSHOT: completed pilot scorecard with all seven criteria scored and evidence links]

### Convene the decision review

20. Schedule a 60-minute decision review with the executive sponsor, the engineering manager, the product owner, and a representative from QA, release, and PMO.
21. Walk the scorecard top to bottom. For each Fail or partial pass, present the evidence and the workaround cost.
22. Recommend one of three outcomes: adopt as system of record; adopt with named gaps and a remediation plan; do not adopt and document why.
23. Record the decision, the owner, and the next-step date in the pilot scorecard.

## Validation Checklist

- [ ] Pilot scope was a single product, two to four repositories, one team, two sprints, and one release candidate.
- [ ] All five scenarios were executed using sub-guides GHE-ALM-080 through GHE-ALM-084.
- [ ] Every pass criterion has linked evidence captured per GHE-ALM-085.
- [ ] The Pilot Scorecard is complete with a Pass or Fail against all seven decision criteria.
- [ ] Non-engineer usability and governance were assessed via the pilot retro, not assumed.
- [ ] A decision review was held with sponsor, engineering, product, QA, release, and PMO present.
- [ ] The adoption recommendation, owner, and next-step date are recorded.

## Common Mistakes

- Running the pilot on a low-stakes side project. The result will not predict real adoption. Pick a real product with real pressure.
- Letting the team file bugs and features outside the pilot Project. Mixed sources contaminate the evidence.
- Renaming canonical project fields mid-pilot. The reporting scenario then fails for tooling reasons rather than product reasons.
- Skipping the bug or release scenario because it did not happen naturally in the two sprints. Run them as drills if needed; the criteria still must be answered.
- Batching evidence capture to the final week. Screenshots and links are easier to gather as scenarios happen.
- Treating dashboards as a pass because charts render. The criterion is whether leadership can decide without spreadsheet reconciliation.

## Escalation Path

- GitHub administrator: when org-level fields, issue types, or rulesets need adjustment to support a scenario.
- Repository administrator: when pilot repositories need branch protections, Code Owners, or workflow access changes.
- Engineering lead: when the team cannot complete a scenario inside the sprint and a sub-guide drill is needed.
- Release manager: when scenario 4 needs a real release window protected for the pilot.

## Related Guides

- GHE-ALM-080 : How to Test Feature and Requirement Decomposition
- GHE-ALM-081 : How to Test Sprint Planning and Execution
- GHE-ALM-082 : How to Test Bug Intake and Triage
- GHE-ALM-083 : How to Test Release Tracking
- GHE-ALM-084 : How to Test Leadership Dashboard Sufficiency
- GHE-ALM-085 : How to Record Pilot Pass/Fail Evidence
