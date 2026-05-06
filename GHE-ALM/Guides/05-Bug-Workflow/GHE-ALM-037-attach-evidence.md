# How to Attach Evidence to a Bug

**Guide ID:** GHE-ALM-037
**Audience:** QA Manager, Engineering Manager, Support Engineer
**Primary role:** QA Manager
**Classification:** Manager Performs
**Estimated time:** 10-15 minutes per bug
**Required permissions:** Repository: Triage (to file or comment on issues); Repository: Write if you need to edit or hide other users' attachments
**Prerequisites:**

- A reproducible defect, or at least a captured artifact (screenshot, log excerpt, recording).
- The bug issue exists, or you are filing it now using a bug issue form.
- Sensitive data has been redacted from any file you plan to upload.

**When to use this guide:** Use this guide when you need to give engineering enough first-hand evidence to investigate a defect without a back-and-forth conversation. This applies whether you are filing the bug yourself, attaching to a bug filed by someone else, or adding evidence that surfaced after the bug entered triage.

**When not to use this guide:** Do not use this guide for general product feedback, feature requests, or change requests. Those follow GHE-ALM-011 (Feature Request) or GHE-ALM-016 (Risk or Change Request). Do not use this guide as a substitute for the triage and routing steps in GHE-ALM-014 (Create and Triage a Bug Report).

## Outcome

By the end of this guide, you will have produced:

- A bug issue (or comment on an existing bug) that includes the right evidence in the right place.
- Reproduction steps an engineer can follow without contacting you.
- A clean record that does not leak secrets, customer data, or internal hostnames.

## Before You Start

- The org and repo for the affected product, using the canonical naming, for example `acme-payments/checkout-service`.
- A short description of what the user did, what they expected, and what happened instead.
- The artifacts to attach: screenshots, screen recording, log excerpt, crash dump, network capture (HAR), or stack trace.
- A redaction pass: scrub access tokens, session cookies, customer email addresses, internal hostnames, and any field covered by your data-handling policy.

## Steps

### Decide where the evidence belongs

1. Choose the right surface. If the bug does not yet exist, attach evidence inside the bug issue form during creation. If the bug already exists, add evidence in a new issue comment so the timeline records who added it and when. Never edit the original issue body to insert evidence after the fact; that hides the audit trail.
2. Right-size the artifact. Screenshots, short recordings under roughly 10 MB, single-file logs, and HAR files belong as direct uploads on the issue. For multi-gigabyte logs, full database dumps, or anything that exceeds GitHub's per-file upload limit, link to the artifact in your team's log storage (for example an S3 bucket, internal artifact repository, or shared drive) and paste the link with a short description.
3. Confirm redaction before you upload anything. Once a file is attached to a public or internal repository issue, anyone with read access can download it. Redact secrets and personal data first.

### Attach evidence during initial bug filing

4. Open the repository, click **Issues**, click **New issue**, then choose the **Bug Report** form. If the form is missing, see GHE-ALM-025 to request one.
5. Fill in the structured fields: title, summary, steps to reproduce, expected vs actual behavior, environment, severity, and priority. Use the illustrative 1-4 / P0-P3 scale; confirm your team's actual scale with QA leadership.
6. Find the **Upload** field on the form. As of the March 5, 2026 issue forms update, bug forms can include a dedicated upload field that requires evidence up front. Drag screenshots, recordings, log excerpts, or crash files into the **Upload** area, or click it to open a file picker.
7. For inline screenshots in the description, paste directly from the clipboard into any text area on the form. GitHub uploads the image and inserts a Markdown reference automatically.
8. In **Steps to reproduce**, write numbered steps a stranger could follow. Include the exact URL, the account role used (not the account itself), the input values used, and the timestamp of the failure if you have it. The timestamp lets engineers correlate with server-side logs.
9. Click **Submit new issue**.

> [SCREENSHOT: Bug Report issue form with the Upload field highlighted, a screenshot file dropped into it, and Steps to reproduce filled in.]

### Attach evidence to an existing bug

10. Open the bug issue. Scroll to the comment box at the bottom.
11. Add a one-line header above the upload that names what you are attaching, for example `Repro recording from Sprint 2026.18 regression sweep` or `HAR capture from acme-checkout staging, 2026-05-06 14:22 UTC`. This makes the timeline scannable.
12. Drag and drop the file into the comment box, or click the paperclip icon to open the file picker. For clipboard images (a screenshot you just captured), click into the comment box and paste; GitHub uploads and inserts the image inline.
13. If the file is too large or sensitive for direct upload, paste a link to the file in your team's log storage and add the access path: who can open it, which bucket or share, and how long it is retained.
14. Click **Comment**.

> [SCREENSHOT: Issue comment box with a HAR file attached and a one-line header naming the source and timestamp.]

### Cover the evidence types engineers actually need

15. Reproduction steps in text. Always include them, even when a recording is attached. Engineers grep issues; they cannot grep a video.
16. Screenshots. Capture the failing screen plus one screen of context before the failure. Annotate with arrows or boxes if the failing element is small.
17. Screen recordings. Keep them under two minutes. Start the recording one step before the failure, not five steps before. Long recordings get skipped.
18. Log excerpts. Paste the relevant lines into a fenced code block with three or four lines of surrounding context. Do not paste 10,000 lines into the issue body; attach the full log as a file or link instead.
19. Crash dumps and stack traces. Attach as files. Include the build version, OS version, and the time of the crash.
20. Network captures. Save as HAR from the browser dev tools, redact the `Authorization` and `Cookie` headers, then attach.
21. Environment block. State the build, environment (`production`, `staging`, `uat`), browser or client version, and tenant or account role. This single block prevents most "cannot reproduce" responses.

### Protect sensitive data

22. Before upload, scan each artifact for: API keys, OAuth tokens, session cookies, passwords, customer names, customer email addresses, payment data, internal hostnames or IP addresses, and any field your data-handling policy classifies as restricted.
23. Redact in place. For screenshots, blur or block out the region. For logs and HAR files, run a search-and-replace pass before saving. Do not rely on cropping alone; a cropped screenshot can still contain metadata.
24. If you discover a sensitive value was uploaded after the fact, do not just edit the comment. Click the comment menu and choose **Hide**, then file a follow-up with your security team. The original upload may persist in the GitHub history; security needs to know.

> [SCREENSHOT: Comment menu showing the Hide option, used after a redaction miss is discovered.]

## Validation Checklist

- [ ] Bug issue contains reproduction steps in text, not only in a recording.
- [ ] At least one direct artifact (screenshot, log, HAR, or crash file) is attached, or a link is provided for oversized files.
- [ ] Each attachment has a one-line header naming what it is and when it was captured.
- [ ] Environment block lists build, environment, client version, and account role.
- [ ] No secrets, customer PII, or internal hostnames appear in attachments or pasted text.
- [ ] If evidence was added after the bug was filed, it appears in a comment, not edited into the issue body.

## Common Mistakes

- Pasting a 10,000-line log into the issue body. Attach the file or link to log storage instead, and quote only the relevant lines.
- Submitting a recording with no written reproduction steps. Engineers cannot search a video.
- Attaching a HAR file without redacting `Authorization` and `Cookie` headers.
- Editing the original issue body to slip evidence in later. This destroys the audit trail. Use a comment.
- Cropping a screenshot to hide a customer name without checking the rest of the image. Redact in place.
- Filing a hotfix-grade defect with only a description and no evidence. See GHE-ALM-040.

## Escalation Path

- GitHub administrator: When a sensitive file was uploaded and needs to be purged from history beyond a simple **Hide**.
- Repository administrator: When the bug form is missing an upload field or the form itself is missing.
- Engineering lead: When evidence shows a production-impacting defect that needs immediate routing rather than normal triage.
- Release manager: When the evidence indicates a defect blocks an in-flight release; coordinate with GHE-ALM-040 and the current release readiness review.

## Related Guides

- GHE-ALM-014 : How to Create and Triage a Bug Report
- GHE-ALM-025 : How to Create or Request Issue Forms
- GHE-ALM-036 : How to Move a Bug Through the Defect Workflow
- GHE-ALM-040 : How to Handle a Hotfix Bug
