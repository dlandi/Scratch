---
description: Recover the state of the GX docs work and tee up the next task
argument-hint: [optional focus, e.g. "multi tests" or "just status"]
allowed-tools: Read, Glob, Grep, Bash(git log:*), Bash(git status:*), Bash(python:*), Bash(ls:*)
---

Orient yourself in the GX CLI Reference documentation work, then hand the user a
short brief and wait for direction. Do not start editing anything.

## 1. Recover the plan

The working directory for this command is the GX project directory.
Read the session memory first. It carries the agreed plan, the decisions already
settled, and at least one dead end that was tested and should not be retried:

- `C:\Users\dland\.claude\projects\E--Archive-GitHub-dlandi-Scratch\memory\project_gx_llm_test_suite.md`
- `C:\Users\dland\.claude\projects\E--Archive-GitHub-dlandi-Scratch\memory\MEMORY.md` for anything else relevant

Treat decisions recorded there as settled. If you think one is wrong, say so in
one sentence in your brief rather than quietly doing something different.

## 2. Read the repo's own documentation

The mechanics live in the repo, not in memory. Do not re-derive them:

- `tools/README.md` - the toolchain, the two rules, the traps
- `tools/LLM-Unit-Tests/README.md` - the test format, the three layers, current coverage
- `R9_1_GX_CLI_Reference/index/README.md` - which index file answers which query shape

## 3. Confirm the actual state

Run these rather than trusting the docs to be current:

```bash
git log --oneline -5
git status --short
python tools/LLM-Unit-Tests/run_tests.py
python tools/build_all.py --check
```

The test runner should report every test valid and every test routed. The build
checker should report all checks passed. If either disagrees with what the
READMEs claim, that gap is the most important thing in your brief.

## 4. Brief the user

Six lines at most, covering:

- where the work stands, in one line
- what the memory says is next, and the batch size agreed
- anything the verification turned up that contradicts the documentation
- the single question you need answered to begin, if there is one

Then stop and wait. If the user gave a focus argument, weight the brief toward
it: $ARGUMENTS

## Working rules for whatever follows

- The source guide `R9_1_GX_CLI_Command_Reference_Guide_001P4.md` is read-only
  input. Everything under `R9_1_GX_CLI_Reference/` is generated; never hand-edit
  it, change the generator instead.
- A failing retrieval test means `tools/curated.py` is missing a search term an
  operator would type. It does not mean the test should be relaxed. Never add a
  term so generic it pollutes routing: the harness measures recall only and will
  not catch the damage.
- Evidence quotes must appear verbatim in the cited split file. Layer 0 enforces
  this, so a fabricated test cannot pass silently.
- The guide contains no procedures. Any ordering in an answer is inference and
  belongs in `inference_flags`.
