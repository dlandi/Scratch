# GHE-ALM Agent Style Guide

This file captures the locked decisions for producing GHE-ALM how-to guides. All agents producing guides must read and follow this exactly.

## Source of truth

The canonical inventory of 85 guides is `GitHub_Enterprise_ALM_HowTo_Guide_Inventory.md` section 4. Use IDs from section 4. If a discrepancy ever appears between section 3 and section 4, section 4 wins.

## File layout

- Folder: `Guides/<NN-section-name>/` matching the inventory section.
- Filename: `GHE-ALM-XXX-<kebab-slug>.md`.
- Subfolders by section:
  - `01-Orientation` (4.1)
  - `02-Project-Setup` (4.2)
  - `03-Issues-and-Standards` (4.3)
  - `04-Backlog-and-Sprint` (4.4)
  - `05-Bug-Workflow` (4.5)
  - `06-Release-Management` (4.6)
  - `07-Dashboards-Reporting` (4.7)
  - `08-Traceability` (4.8)
  - `09-Deployment-Governance` (4.9)
  - `10-Governance-Policy` (4.10)
  - `11-Pilot-Evaluation` (4.11)

## Template

Use this structure exactly. Note the blank lines, especially between bold-label "frontmatter" and any list that follows.

```markdown
# How to <Activity Title>

**Guide ID:** GHE-ALM-XXX
**Audience:** <one to three role names from the canonical role list>
**Primary role:** <single role>
**Classification:** <Manager Performs | Manager Reviews | Manager Requests | Manager Understands>
**Estimated time:** <realistic, e.g., "10-15 minutes per use" or "30-minute one-time setup">
**Required permissions:** <e.g., "Repository: Triage; Project: Write" or "None">
**Prerequisites:**

- <bullet>
- <bullet>

**When to use this guide:** <one or two sentences>

**When not to use this guide:** <one or two sentences>

## Outcome

By the end of this guide, you will have produced:

- <output 1>
- <output 2>

## Before You Start

- <prerequisite>
- <data needed>
- <permission needed>

## Steps

### <Phase heading, no number>

1. <imperative step. Bold UI elements.>
2. <step>

> [SCREENSHOT: short description]

### <Next phase heading>

3. <step>
4. <step>

## Validation Checklist

- [ ] <expected result>
- [ ] <expected result>

## Common Mistakes

- <mistake>
- <mistake>

## Escalation Path

- GitHub administrator: <when to involve, or "Not applicable">
- Repository administrator: <when to involve, or "Not applicable">
- Engineering lead: <when to involve, or "Not applicable">
- Release manager: <when to involve, or "Not applicable">

## Related Guides

- GHE-ALM-XXX : <title>
- GHE-ALM-YYY : <title>
```

### Steps section rules

The Steps section uses a single continuous numbered list. When the guide has 2 or more distinct phases (for example "Create the view" then "Run a standup with the board"), use level-3 headings (`###`) without numbers as phase markers, and continue the numbered list across them:

```
### Create the view
1. Step.
2. Step.

### Run a standup with the board
3. Step.
4. Step.
```

For shorter guides without distinct phases, a single numbered list without phase headings is fine.

For Manager Understands guides, the "steps" are usually conceptual blocks rather than UI clicks. You may use numbered headings (`### 1. Concept name`) when that serves the teaching structure better. The prose inside each block may then be unnumbered.

Do NOT use numbered headings (`### 1. Title`) for Manager Performs, Manager Reviews, or Manager Requests guides.

### Required formatting

- Always blank line between any frontmatter `**X:**` line followed by a list and the list itself, especially `**Prerequisites:**`.
- Bold UI labels: `**New Project**`, `**Settings**`, `**Status**`.
- Inline code for filter expressions, naming patterns, and link syntax: `` `sprint:@current` ``, `` `Closes #1234` ``, `` `owner/repo#NNN` ``.
- Use ` : ` (space-colon-space), NOT ` — ` (em-dash), in Related Guides lines and anywhere else a separator is needed.

## Locked vocabulary

### Filter syntax

The canonical iteration filter is `sprint:@current` (lowercase fieldname:value). Other forms such as `Sprint = @current` are stale and should not appear in new guides.

### Iteration field name

Default to `Sprint`. If a guide is specifically about field naming, mention some teams use `Iteration` and the filter syntax adapts.

### Project field names (canonical)

Always use these exact names:

`Status`, `Priority`, `Severity`, `Effort`, `Sprint`, `Release`, `Product Area`, `Owner`, `Start Date`, `Target Date`, `Risk Level`, `Customer Impact`.

Do not rename, abbreviate, or substitute synonyms.

### Severity and Priority scale (illustrative)

Always label the scale as illustrative when introducing it ("a common 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership"):

| Code | Severity (impact) | Priority (urgency) |
|---|---|---|
| 1 / P0 | System down, data loss, no workaround | Fix now, hotfix candidate |
| 2 / P1 | Major feature broken, workaround painful | Fix in current sprint |
| 3 / P2 | Minor feature broken, workaround easy | Fix in next 1-2 sprints |
| 4 / P3 | Cosmetic or rare edge case | Backlog |

### Worked-example tenant naming

Use fictional `acme-*` org and product names. Do not use Nokia-specific names.

- Org: `acme-payments`, `acme-platform`, `acme-checkout`
- Repository: `checkout-service`, `payments-api`, `web-client`
- Product Area: `Checkout`, `Billing`, `Identity`
- Release: `2026.05.0`, `2026-Q3 Release`
- Sprint: `Sprint 27`, `Sprint 2026.18`

### Permission roles

Use GitHub's standard built-in role names:

- Repository: `Read`, `Triage`, `Write`, `Maintain`, `Admin`
- Organization: `Member`, `Owner`, `Project creator`
- Project: `Read`, `Write`, `Admin`

If you are not sure which role grants a specific action, name the action permission rather than guessing the role: "permission to apply labels" rather than guessing.

### Audience and primary role

For `Primary role`, pick the role that most often performs (or reviews, requests, or needs to understand) the activity in a typical week. If two roles share the work, list the one accountable for the outcome.

Canonical role list:

Project Manager, Program Manager, Engineering Manager, Release Manager, QA Manager, Product Owner, Scrum Master, Support Engineer.

### Length tolerance

- Soft target: 800 to 1500 words including the structure.
- Hard cap: 2000 words.
- Worked examples may push you up to 10 percent over the soft target. Do not exceed the hard cap.

### Screenshot markers

- Format: `> [SCREENSHOT: short description of what should be captured]`
- Manager Performs: 2 to 4 markers.
- Manager Reviews: 1 to 2 markers.
- Manager Understands: 0 to 1 markers.
- Manager Requests: 0 to 1 markers.
- Describe what the screenshot should show. Do not render the UI in prose.

### Standup direction

Do not prescribe a column-walk direction (left to right vs right to left) for standup-related guides. Teams choose. If you mention standup flow, present the direction as a team choice.

## Voice

- Imperative second-person: "Open the Project. Click **New Project**."
- Direct, terse, professional.
- No em-dashes anywhere. Use commas, semicolons, colons, or new sentences.
- No emojis or emoticons.
- No marketing language. Avoid: `seamlessly`, `powerful`, `robust`, `leverage`, `unlock`, `empower`, `cutting-edge`, `best-in-class`, `world-class`, `next-generation`, `transform`, `accelerate`, `simply`.
- No purple prose.
- No meta-commentary about the guide itself or its classification ("For an Understands guide, ...", "Because this is a checklist guide, ..."). The reader does not care; the guide just delivers content.

## Classification-specific guidance

### Manager Performs

- Click-by-click steps with bolded UI labels.
- 2 to 4 screenshot markers.
- Each phase produces something concrete the manager can point to.

### Manager Reviews

- Frame as inspection: what to open, what to look at, what good vs bad looks like, when to escalate.
- Include a "What Good Looks Like vs. What to Escalate" comparison table.
- 1 to 2 screenshot markers.

### Manager Requests

- Format the body as a request checklist (what to specify, what to send, who to send it to).
- Include a "Sample Request to Send" section before Validation Checklist with a templated message.
- 0 to 1 screenshot markers.

### Manager Understands

- Focus on mental model, glossary, decision rules.
- Include one short worked example.
- 0 to 1 screenshot markers.

## Cross-references

- Use IDs from inventory section 4. Verify each Related Guides ID exists in section 4 by Read or Grep before listing it.
- List 3 to 5 Related Guides that a reader would naturally consult before, after, or alongside this one.
- Format: `- GHE-ALM-XXX : <title from inventory>`

## What NOT to do

- Do not invent GitHub UI elements that don't exist. Verify via WebFetch.
- Do not over-explain GitHub basics. Assume the reader knows what an issue, repository, and pull request are.
- Do not write code, YAML, or shell commands unless absolutely required. Inline code for filter syntax and link syntax is fine.
- Do not write planning documents, decision logs, or analysis files on the side. Just produce the single guide file.
