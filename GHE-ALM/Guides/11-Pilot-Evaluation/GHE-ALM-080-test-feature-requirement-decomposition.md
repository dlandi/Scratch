# How to Test Feature and Requirement Decomposition

**Guide ID:** GHE-ALM-080
**Audience:** Project Manager, Engineering Manager, Product Owner
**Primary role:** Project Manager
**Classification:** Manager Performs
**Estimated time:** 45-60 minutes for a single scenario run
**Required permissions:** Repository: Triage or Write; Project: Write
**Prerequisites:**

- Pilot kickoff complete per GHE-ALM-079.
- Pilot Project exists with `Status`, `Priority`, `Sprint`, `Release`, `Product Area`, and `Owner` fields configured.
- Issue types `Feature`, `Requirement`, and `Task` are available on the pilot organization.
- Hierarchy View is enabled on the pilot Project.

**When to use this guide:** Use this guide during the pilot to run scenario 1 of the pilot scorecard, validating that GitHub Enterprise can support feature decomposition into requirements and tasks with full hierarchy visibility.

**When not to use this guide:** Do not use this guide for production work intake. For day-to-day feature, requirement, and task creation, use GHE-ALM-011, GHE-ALM-012, and GHE-ALM-015.

## Outcome

By the end of this guide, you will have produced:

- One Feature issue, two to four Requirement sub-issues, and four to eight Task sub-issues in the pilot repository.
- A Hierarchy View screenshot in the pilot Project showing the Feature, its Requirements, and its Tasks.
- A scenario 1 result row (Pass or Fail with notes and evidence links) ready to enter in the pilot scorecard per GHE-ALM-085.

## Before You Start

- Confirm the pilot Feature you will model. Use a real candidate from the pilot release if possible. Worked example: `acme-checkout` org, `checkout-service` repository, Feature "Add saved card management to checkout".
- Confirm the pilot release identifier (for example `2026.05.0`) and the active sprint (for example `Sprint 2026.18`).
- Have the pilot scorecard open in a separate tab so you can record evidence as you go.
- Note who is performing the test (the Project Manager) and who will witness the result (typically the Engineering Manager or pilot lead).

## Steps

### Set up the scenario

1. Open the pilot organization Project. Confirm you are on the saved view named **Hierarchy** or create a fresh table view named **Pilot Scenario 1** that you will switch to Hierarchy layout later.
2. In a second tab, open the pilot repository (for example `acme-checkout/checkout-service`). You will create the Feature, Requirements, and Tasks here.
3. Open the pilot scorecard. Locate the row for "Scenario 1: Feature and requirement decomposition". Record the start time and tester name.

### Create the Feature

4. In the pilot repository, click **Issues**, then **New issue**. Choose the **Feature Request** form.
5. Set the title to a clear, single-purpose statement. Example: `Add saved card management to checkout`.
6. Set **Type** to `Feature`. Set **Priority** to a value from your team scale (for example `P1`). Set **Owner**, **Product Area** (for example `Checkout`), and **Release** (for example `2026.05.0`). Leave **Sprint** unset for now.
7. Fill in the description with a one-paragraph problem statement and a bulleted acceptance criteria list with at least three items.
8. Click **Submit new issue**. Note the Feature issue number, for example `#412`.
9. Open the Feature, click the Project sidebar control, and add it to the pilot Project.

> [SCREENSHOT: New Feature issue showing Type, Priority, Owner, Product Area, Release set, with the Project sidebar showing the pilot Project added.]

### Decompose into Requirements

10. On the Feature issue, scroll to the **Sub-issues** section. Click **Add sub-issue**, then **Create sub-issue**.
11. Choose the **Requirement** form. Title the sub-issue with a noun phrase that describes a single capability. Example: `Display saved cards on checkout page`.
12. Set **Type** to `Requirement`. Set **Priority**, **Owner**, **Product Area**, and **Release** to match the parent Feature where appropriate. Add acceptance criteria.
13. Click **Create**. Confirm the new Requirement appears under **Sub-issues** on the Feature.
14. Repeat steps 10 through 13 to create at least one more Requirement, ideally three or four total. Examples: `Add new card during checkout`, `Remove a saved card`, `Mark a saved card as default`.
15. Open each Requirement and confirm it is added to the pilot Project. If auto-add is not configured, add each one manually.

### Decompose Requirements into Tasks

16. Open the first Requirement. In its **Sub-issues** section, click **Add sub-issue**, then **Create sub-issue**.
17. Choose the **Task** form. Title the Task with an imperative phrase. Example: `Render saved cards list component`.
18. Set **Type** to `Task`. Set **Sprint** to the active pilot sprint (for example `Sprint 2026.18`). Set **Owner** and **Effort** if your team uses estimates.
19. Click **Create**. Repeat for one to three Tasks per Requirement so that the Feature has between four and eight Tasks in total. Examples for `Add new card during checkout`: `Add card form UI`, `Wire card form to tokenization service`, `Add unit tests for card form`.

### Verify hierarchy visibility

20. Return to the pilot Project. Open the Hierarchy view (or switch the **Pilot Scenario 1** view layout to Hierarchy).
21. Filter or search to locate your Feature, for example `Add saved card management`. Expand the Feature row.
22. Confirm the tree shows the Feature at the top, its Requirements as children, and the Tasks as grandchildren. Confirm `Status`, `Owner`, and `Sprint` columns are populated for the Tasks and `Status`, `Owner`, and `Release` for the Feature and Requirements.

> [SCREENSHOT: Hierarchy View expanded for the pilot Feature, showing Requirement rows under the Feature and Task rows under each Requirement, with Status, Owner, and Sprint or Release columns visible.]

### Record evidence

23. Capture the Hierarchy View screenshot and save it to the pilot evidence folder named with the Feature issue number, for example `scenario-1-hierarchy-issue-412.png`.
24. In the pilot scorecard row for scenario 1, record: link to the Feature issue, link to the Hierarchy view, the screenshot filename, the count of Requirements and Tasks created, and the Pass or Fail result against each pass criterion.
25. If any pass criterion fails, write a one-sentence note describing what was missing or wrong. Hand the row to the pilot lead for review per GHE-ALM-085.

## Validation Checklist

- [ ] One Feature issue exists with `Type = Feature`, `Owner`, `Priority`, `Product Area`, and `Release` populated.
- [ ] At least two Requirement sub-issues exist under the Feature with `Type = Requirement`.
- [ ] At least four Task sub-issues exist under the Requirements with `Type = Task` and `Sprint` set.
- [ ] Hierarchy View shows the full Feature, Requirement, Task tree expanded.
- [ ] `Status` and `Owner` are visible in the Hierarchy View for every node in the tree.
- [ ] The Feature, all Requirements, and all Tasks appear in the pilot Project.
- [ ] Evidence (issue link, view link, screenshot) is recorded in the pilot scorecard scenario 1 row.

## Common Mistakes

- Creating Requirements as labels or in the issue body instead of as separate issues with `Type = Requirement`. The hierarchy will not appear in Hierarchy View.
- Forgetting to add child issues to the pilot Project when auto-add is not configured. Tasks then appear in the repository but not in Project views.
- Setting the Feature's `Sprint` field. Features and Requirements should not carry a sprint; only Tasks should. Mixing this up distorts sprint scope.
- Naming Tasks like Requirements (noun phrases describing capabilities) rather than imperative actions. The work breakdown becomes hard to schedule.
- Closing or merging the test issues mid-scenario. Leave them open until evidence is recorded so reviewers can re-open links.

## Escalation Path

- GitHub administrator: If `Feature`, `Requirement`, or `Task` issue types are missing at the organization level, escalate per GHE-ALM-023.
- Repository administrator: If Issue Forms for Feature, Requirement, or Task are missing, escalate per GHE-ALM-025.
- Engineering lead: If Hierarchy View does not show sub-issues despite correct relationships, escalate to confirm the Project has Hierarchy enabled and the items are added to the same Project.
- Release manager: Not applicable for this scenario.

## Related Guides

- GHE-ALM-079 : How to Run the GitHub Enterprise ALM Pilot Evaluation
- GHE-ALM-011 : How to Create a Feature Request Issue
- GHE-ALM-017 : How to Break Work into Sub-Issues
- GHE-ALM-018 : How to Use Hierarchy View to Review Epic-to-Task Breakdown
- GHE-ALM-085 : How to Record Pilot Pass/Fail Evidence
