---
name: feedback-scratch-repo-write-access
description: "Full standing authorization to create, edit, delete and run anything in the Scratch repo without asking"
metadata: 
  node_type: memory
  type: feedback
  originSessionId: f13442e8-dec5-4ec4-8c1b-c40fc9395925
  modified: 2026-08-14T22:56:37.959Z
---

Dennis has granted blanket permission to create, edit, delete and run **any file
in the Scratch repo**, across every subproject in it: GX, GCP-PubSub,
ResourceScheduler and anything else under
`E:\Archive\GitHub\dlandi\Scratch`. This covers source, generated output,
scripts in any language, JSON and JSONL data, config, test fixtures and
throwaway working files.

**Why:** this is a scratch repo for iterative work, everything in it is under
git, and per-action confirmation adds friction with no safety benefit. He has
said so four separate times (through 2026-08-14), the last time noting that
asking "is just slowing us down."

**How to apply:** act directly. Do not open with permission questions, do not
hedge edits behind "shall I", and do not narrate an intention to edit before
editing. Just make the change and report what changed afterwards.

Three things this does not switch off:

- Report outcomes faithfully. Say what changed, and say plainly when something
  failed or was skipped.
- Repo conventions recorded in other memories still bind, for example
  [[feedback-central-package-management]] in GCP-PubSub. Permission to edit a
  file is not licence to ignore how that project works.
- Destructive git operations are a different risk class and are not covered:
  force push, hard reset, history rewrite and branch deletion can lose work that
  the repo cannot give back. Confirm those.

Blocking questions are still appropriate when a decision genuinely changes the
deliverable, such as scope, output format or taxonomy. They are not appropriate
for access. See [[project-gx-llm-test-suite]] for the current work in the GX
subproject.
