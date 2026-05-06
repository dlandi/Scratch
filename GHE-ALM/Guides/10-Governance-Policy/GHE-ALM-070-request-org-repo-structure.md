# How to Request GitHub Organization and Repository Structure

**Guide ID:** GHE-ALM-070
**Audience:** Engineering Manager, Program Manager, Product Owner
**Primary role:** Engineering Manager
**Classification:** Manager Requests
**Estimated time:** 30 to 60 minutes to draft, plus review cycles with the enterprise administrator
**Required permissions:** None to draft the request. Enterprise or organization administrator permission to implement.
**Prerequisites:**

- A named product, business unit, or program that needs an ALM home in GitHub.
- A short list of applications, services, or infrastructure repositories that belong to that product.
- Two or three named owners who will hold organization or repository administrator roles.
- A working understanding of the Enterprise to Organization to Repository to Project hierarchy. See GHE-ALM-001.

**When to use this guide:** Use this guide when you need to ask the enterprise administrator to create or restructure a GitHub organization, a set of repositories, or an organization-level Project to support ALM for your product. Use it before any work is created, and use it again whenever a real governance, compliance, security, or business boundary changes.

**When not to use this guide:** Do not use this guide to request access to existing repositories; use GHE-ALM-072. Do not use this guide to request team structure inside an existing organization; use GHE-ALM-071. Do not use this guide to ask for a new organization just to separate two teams that already share a product; teams are handled with nested teams and CODEOWNERS, not new organizations.

## Outcome

By the end of this guide, you will have produced:

- A written request to the enterprise administrator that names the target organization, the repositories to create, and the ALM Project to create.
- A documented justification for any new organization based on a real governance boundary.
- A list of owners and the role each owner will hold.

## Before You Start

- Confirm the product or program has a sponsor who agrees to the proposed structure.
- Check whether an existing organization already covers your business unit or product line. New organizations should be rare.
- Decide a naming pattern for repositories before submitting the request. Renames are disruptive.
- Identify the governance boundary justification: regulatory, security, contractual, or business-unit separation.

## Steps

### Decide the structure

1. Confirm the recommended pattern. Enterprise contains Organizations (one per business unit or product line). Each Organization contains Repositories (one per application, service, or infrastructure component). Each Organization contains an Organization Project (one per product ALM board) that aggregates issues across those repositories.
2. Decide whether you need a new Organization. Create a new organization only when at least one of the following applies: a regulatory or compliance boundary requires separation; a security boundary requires distinct membership; a contractual boundary with a customer or partner requires isolation; a business unit owns a separate product line with no shared engineering. If none of these apply, request repositories inside an existing organization instead.
3. List the repositories. One repository per deployable application, service, library, or infrastructure-as-code module. Avoid monorepos unless the engineering team has explicitly chosen one. Apply a consistent naming pattern such as `<product>-<component>` or `<product>-<component>-<type>`, for example `acme-checkout-service`, `acme-checkout-web-client`, `acme-checkout-iac`.
4. Decide the ALM Project. Plan one Organization Project per product. The Project will be the board, table, roadmap, and insights surface that aggregates issues from all repositories in the product. See GHE-ALM-006 for what the Project setup looks like.
5. Identify owners. Name at least two organization owners (for redundancy) and a repository administrator for each repository. Avoid naming a single person; ownership lapses when people move teams.

### Draft and submit the request

6. Open the issue tracker, ticket queue, or email channel your enterprise administrator uses for structural requests. Confirm the channel before drafting.
7. Draft the request using the template in the next section. Keep the justification specific. Vague justifications such as "we want our own space" produce org sprawl and are usually rejected.
8. Send the request to the enterprise administrator. Copy the product sponsor and the named owners.
9. Track the request to completion. Confirm the organization, repositories, and Project shell exist before any team member begins creating issues.

> [SCREENSHOT: example issue or email containing the completed request template, with org name, repository list, ALM Project name, justification, and owners visible]

## Sample Request to Send

Send a message in this shape. Replace bracketed values with your specifics. Keep the entire request in one ticket so the administrator can implement it as a unit.

```
Subject: Request: GitHub structure for [Product Name]

Target organization
- Organization name: acme-checkout
- New organization or existing: New
- Justification for new organization: Regulatory boundary. Checkout
  processes cardholder data and must remain isolated from acme-platform
  for PCI scope reasons. Confirmed with the security team on
  [date], owner [name].

Repositories to create
- acme-checkout/checkout-service (backend service)
- acme-checkout/payments-api (backend service)
- acme-checkout/web-client (frontend application)
- acme-checkout/checkout-iac (infrastructure as code)

Default repository settings requested
- Visibility: Internal
- Default branch: main
- Branch protection / ruleset coverage on main: required
  (see GHE-ALM-073 for the request)

Organization Project to create
- Project name: Checkout ALM
- Scope: All four repositories above
- Owner: Engineering Manager [name]
- Purpose: Single ALM board for the Checkout product covering
  features, requirements, tasks, bugs, and release tracking

Owners and roles
- Organization owners: [name 1], [name 2]
- Repository admins: [name] for backend, [name] for web,
  [name] for IaC
- Project admin: [name]

Out of scope of this request
- Team structure (will follow in a separate request, GHE-ALM-071)
- User access grants (will follow in a separate request,
  GHE-ALM-072)
- Branch protection rules (will follow in a separate request,
  GHE-ALM-073)

Requested completion: [date]
Requester: [your name, role]
Sponsor: [sponsor name, role]
```

## Validation Checklist

- [ ] Justification names a real governance, compliance, security, or business boundary, not preference or convenience.
- [ ] Organization name follows the enterprise naming convention.
- [ ] Repository list is complete and uses a consistent naming pattern.
- [ ] One Organization Project is requested per product, not one per repository.
- [ ] At least two organization owners are named.
- [ ] A repository administrator is named for every repository.
- [ ] Out-of-scope items are explicitly listed and pointed to follow-up requests.
- [ ] Sponsor is copied on the request.

## Common Mistakes

- Requesting a new organization to separate two teams who share a product. Use nested teams (GHE-ALM-071) inside the existing organization instead.
- Requesting one Organization Project per repository. The Project is per product, and it spans repositories.
- Requesting a monorepo and many ALM Projects. The repository structure and the ALM Project structure are independent decisions.
- Listing a single owner. The administrator will reject or delay the request, since single-owner ownership creates lockout risk.
- Bundling team structure, user access, and ruleset requests into the same ticket. Each is a separate request with a separate approver.
- Naming repositories around team names. Teams change. Name repositories around the application or service.
- Skipping the justification because the administrator "already knows the context." The justification becomes the audit record for why the structure exists.

## Escalation Path

- GitHub administrator: Owns enterprise and organization creation, including organization-level policies. Escalate here if the request stalls or if the recommended structure is rejected.
- Repository administrator: Once repositories exist, owns repository-level settings. Not involved in the initial structure request.
- Engineering lead: Confirms the application and service decomposition that drives the repository list. Involve early.
- Release manager: Confirms whether the proposed Organization Project will support cross-repository release tracking (see GHE-ALM-049). Involve before submission.

## Related Guides

- GHE-ALM-001 : How to Navigate the GitHub Enterprise ALM Object Model
- GHE-ALM-006 : How to Create and Use an Organization-Level GitHub Project
- GHE-ALM-071 : How to Request or Review Nested Teams
- GHE-ALM-072 : How to Request Repository Access for Project Managers and Stakeholders
- GHE-ALM-076 : How to Govern Project Fields and Labels
