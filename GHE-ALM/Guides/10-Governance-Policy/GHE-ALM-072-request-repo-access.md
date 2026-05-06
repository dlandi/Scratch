# How to Request Repository Access for Project Managers and Stakeholders

**Guide ID:** GHE-ALM-072
**Audience:** Project Manager, Program Manager, Engineering Manager
**Primary role:** Project Manager
**Classification:** Manager Requests
**Estimated time:** 15-20 minutes per request
**Required permissions:** None to submit the request. The recipient must be an organization owner, repository admin, or team maintainer.
**Prerequisites:**

- You know the exact organization name and repository name (or list of repositories) the user needs.
- You know the user's GitHub username and that the user has accepted the organization invitation.
- You have read GHE-ALM-071 if a team already exists for this product area.

**When to use this guide:** Use this guide when a project manager, product owner, release manager, QA stakeholder, or executive sponsor needs access to one or more repositories in your organization, and you need to send a clean request to the administrator who can grant it.

**When not to use this guide:** Do not use this guide to request changes to branch protection, rulesets, or CODEOWNERS. Use GHE-ALM-073 or GHE-ALM-075 for those. Do not use this guide to request a brand-new organization or repository structure; use GHE-ALM-070.

## Outcome

By the end of this guide, you will have produced:

- A written access request that identifies the user, the repository or team, the role, the business justification, and the duration.
- A request sent to the correct administrator (organization owner, repository admin, or team maintainer).
- A record you can reference for the next quarterly access audit.

## Before You Start

- Confirm the user has been added to the organization. Repository roles cannot be granted to a user who is not an organization member.
- Identify whether access should be granted via a team (preferred for groups of stakeholders) or directly to the individual (acceptable for short-term or single-user cases).
- Decide the least-privilege role that lets the user complete their actual work. Read the role mapping in the next section before naming a role.

## Steps

### Choose the correct role

1. Map the user's actual weekly activity to a role using the table below. Pick the lowest role that covers the activity.

| Activity the user needs to perform | Minimum role |
|---|---|
| View code, read issues and pull requests, view releases, watch Actions runs | `Read` |
| Apply labels and milestones, assign issues and pull requests, manage project metadata, hide off-topic comments, close issues without code changes | `Triage` |
| Push code to non-protected branches, open and merge pull requests, create releases, edit wiki content | `Write` |
| Configure repository settings, manage repository topics, enable Pages, manage non-destructive repository features | `Maintain` |
| Manage repository access, configure branch protection and rulesets, delete the repository, transfer ownership | `Admin` |

2. Confirm the role against these defaults for typical stakeholder profiles:
    - Project manager who triages issues and grooms the backlog: `Triage`.
    - Product owner who reviews work but does not write code: `Read` plus Project: `Write` on the relevant GitHub Project.
    - Release manager who manages milestones and releases: `Write`.
    - QA manager who triages bugs and verifies fixes: `Triage`.
    - Executive sponsor who reads dashboards: `Read`.
    - Engineering manager accountable for the repository: `Maintain`, not `Admin`, unless they also own access policy.
3. If the user only needs to interact with a GitHub Project (not the underlying repository code), request Project access in addition to repository `Read`. Project roles (`Read`, `Write`, `Admin`) are separate from repository roles.

### Decide team vs direct grant

4. Default to granting access through a team. Teams scale, survive personnel changes, and make audits easy. Request direct grants only when the user is short-term, when no team fits, or when policy requires it.
5. If a team for this product area already exists (for example, `acme-payments/checkout-pms`), name the team in your request. If no fitting team exists, request team creation under GHE-ALM-071 first, then return to this guide.

### Assemble the request

6. Capture the following fields in writing before you send anything. Missing fields are the most common reason an access request bounces back.
    - GitHub username of each user.
    - Organization name (for example, `acme-payments`).
    - Repository name or list of repositories (for example, `checkout-service`, `payments-api`). Use `org/all` only if the user genuinely needs every repository.
    - Team name if granting through a team.
    - Requested role: `Read`, `Triage`, `Write`, `Maintain`, or `Admin`.
    - Business justification: one or two sentences tied to the user's actual work.
    - Effective date and review or expiration date. Default review cadence is quarterly.
    - Approver name (the user's manager or product owner) if your organization requires manager sign-off.
7. Sanity-check the role one more time. If you wrote `Admin` or `Maintain`, justify that choice explicitly in the request. Most stakeholder requests should land at `Read` or `Triage`.

### Send the request

8. Identify the correct recipient:
    - Team membership change: team maintainer or organization owner.
    - Direct repository grant: repository admin or organization owner.
    - Cross-organization access: organization owner.
9. Send the request through your organization's standard channel (ticketing system, Slack request channel, or email). Use the template in the next section. Do not request access in a public issue; access requests can leak organizational structure.

> [SCREENSHOT: organization repository roles reference page open at the role comparison matrix]

## Sample Request to Send

Subject: Repository access request: `<user>` to `acme-payments/checkout-service` as `Triage`

Hello `<recipient>`,

Please grant the following repository access:

- User: `@jane-doe`
- Organization: `acme-payments`
- Repository: `checkout-service`
- Team (if applicable): `acme-payments/checkout-pms`
- Role: `Triage`
- Justification: Jane is the project manager for Checkout. She needs to apply labels and milestones, assign issues, and close non-engineering issues during sprint planning and backlog grooming. She does not need to push code.
- Effective date: `2026-05-06`
- Review date: `2026-08-06` (next quarterly access audit)
- Approver: `<product owner name>`

If `Triage` is the wrong fit for this activity, please tell me which role you would grant instead and I will adjust.

Thank you,
`<your name>`

## Validation Checklist

- [ ] The user appears in the repository **Settings** > **Collaborators and teams** list, or in the team that grants the access.
- [ ] The role shown matches the role you requested.
- [ ] The user can perform the intended action (apply a label, open a pull request, view a private repository) and cannot perform actions outside that role.
- [ ] The request is recorded somewhere durable (ticket number, shared tracker) so the next access audit can find it.
- [ ] The review or expiration date is on your calendar.

## Common Mistakes

- Requesting `Admin` because it is "easier." `Admin` includes deletion and access management; this is rarely what a stakeholder needs.
- Requesting `Write` for a project manager who only needs to manage labels and milestones. `Triage` covers that work without granting push access.
- Granting access directly to the user when a team already exists for the product area. Direct grants drift and are missed during audits.
- Naming a repository the user does not actually need. Scope to the specific repositories the user works in.
- Forgetting that GitHub Project access is separate from repository access. A user with repository `Read` may still need Project `Write` to update fields.
- Omitting the review date. Access without a review date becomes permanent by accident.

## Escalation Path

- GitHub administrator: Involve when the request crosses organizations, when the user is not yet an organization member, or when a custom role is needed.
- Repository administrator: Involve for direct repository grants and for any request above `Triage` on a governed repository.
- Engineering lead: Involve when the role being requested is `Maintain` or `Admin`, or when the user needs access to a sensitive code path covered by CODEOWNERS.
- Release manager: Involve when access is being requested specifically to manage milestones, releases, or release branches.

## Related Guides

- GHE-ALM-002 : How to Find the Correct Organization, Repository, and Project
- GHE-ALM-003 : How to Use the Repository Dashboard
- GHE-ALM-070 : How to Request GitHub Organization and Repository Structure
- GHE-ALM-071 : How to Request or Review Nested Teams
- GHE-ALM-076 : How to Govern Project Fields and Labels
