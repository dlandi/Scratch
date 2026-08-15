---
name: project-gx-llm-test-suite
description: GX CLI Reference docs pipeline and LLM test suite - state as of 2026-08-15, the agreed next plan (layer 2, precision, chapters 3-5), and the authoring lessons
metadata: 
  node_type: memory
  type: project
  originSessionId: f13442e8-dec5-4ec4-8c1b-c40fc9395925
  modified: 2026-08-15T02:48:11.718Z
---

Work in `Scratch/GX` turning the Nokia 1830 GX R9.1 CLI Reference Guide into an
LLM-retrievable corpus. Branch `GX`, everything committed and pushed through
`60337db`. Working tree clean apart from `ResourceScheduler/.claude/`, which is
untracked, unrelated and should be left alone.

**Built and green:** source guide split into 386 byte-exact files, a generated
index set, a reproducible toolchain, and 441 tests (376 single covering all 374
Chapter 6 commands, 65 multi across all five cluster bases and all 16 domains).
441/441 valid, 440/440 routed, 30/30 build assertions. The repo READMEs describe
the mechanics; read those rather than re-deriving them: `GX/tools/README.md`,
`GX/tools/LLM-Unit-Tests/README.md`, `GX/R9_1_GX_CLI_Reference/index/README.md`.

## Next plan, agreed 2026-08-15, in this order

**1. Run layer 2 against a real model.** It has never been run once. It is the
cheapest way to turn a large unexercised asset into signal. Layer 2 needs an
answers file, JSONL of `{"id": ..., "answer": ...}`, passed as
`run_tests.py --answers path.jsonl`; each answer must contain every string in
that test's `expect.facts`. Expect it to immediately expose weak facts: 75 of
1,259 facts in the single tests appear in more than 25% of all 395 files, so 63
of 376 single tests would be satisfied by an answer saying almost nothing. The
65 multi tests are clean at 0, because `_authoring.py` refuses such facts; the
single tests predate that check. Fixing those 63 is part of this task.

**2. Measure precision, not just recall.** Layer 1 scores recall only. Roughly
70 vocabulary additions have been made to `curated.py` and not one is measured
for collateral damage: a term that also drags in fifty irrelevant commands makes
tests pass while degrading routing, and nothing in the harness sees it. Until
this exists, every future `curated.py` edit is unfalsifiable. Approach agreed in
outline only: sample queries that should *not* reach a topic and count false
hits. The design is open.

**3. Chapters 3, 4 and 5.** The 21 auxiliary, navigation and piped commands
still have zero tests. Note `_authoring.py` builds multi-command tests only; a
single-command batch needs a different path or an extension to it.

Also outstanding but lower value: second single-command tests for the rich
commands (`show`, `set`, `download`, `status`, `activate`).

**Honest framing of what the suite is worth right now.** No LLM has ever been
run against any of this. Layer 1 is a deterministic lexical simulation of what an
agent would do. What the tests have actually bought so far is a gap-finding
instrument for the index, and that paid: ~70 real vocabulary gaps found and
fixed, including ones nobody would guess (`fibre` reaching nothing,
`connectivity` not matching `connection`, a topic named "Database backup" with
no `backup` term). Their value to date is diagnostic, not evaluative. Do not
describe the suite as proof the documentation works.

## If you write another test batch

Use `tools/LLM-Unit-Tests/_authoring.py`. It derives `cluster` and
`names_command` from the corpus, rejects facts appearing in >25% of files,
rejects non-verbatim quotes and invented identifiers in answer prose, and writes
nothing unless the whole batch passes. It exists because reviewing batch 1 found
five defect classes that every layer of `run_tests.py` missed: fabricated
identifiers in answer prose, hand-typed metadata drift (24 of 30 wrong domain
slugs), facts with no discriminating power, archetype skew following the cluster
basis, and claims sourced from files the test does not cite.

**Reviewing a batch must not just re-run `_authoring.py`'s checks** - they were
satisfied at write time, so they prove nothing. Go where the tooling cannot:
check attribution (the prose check concatenates all cited files, so an attribute
credited to the wrong object in the cluster passes) and check every stated
default against the parameter record. Those two found the only real defects in
batch 2. Both throw false positives worth knowing: `\botdr\b` matches inside
`otdr-ptp` so match whole hyphenated tokens; command names that are ordinary
nouns (`database`, `password`) break sentence segmentation; and a default-claim
match window easily spills into the next clause.

**When retrieval fails, decide whether the index or the question is at fault**
before touching `curated.py`. Batch 2 had three failures and only one was a real
gap. The other two were questions phrased more vaguely than an operator would
phrase them. Never add a term so generic it pollutes routing, because the
harness measures recall only and would not catch the damage.

**For multi tests the signal is not pass/fail** but which cluster members stay
unreachable on a *passing* test, because a cluster's members share vocabulary
and one topic hit routes all of them. Partial routing is correct more often than
not: a cluster is structural, a question is one phrasing of one intent.

## Settled, do not re-litigate

- Cluster sources are AID key-path ancestry, confusable names and curated
  topics, all emitted by `_clusters.py`: 10 + 17 + 53 clusters. Two derivation
  traps already fixed there, do not reintroduce: a flat edit-distance threshold
  chains every 3-letter acronym into one meaningless component, so it scales
  with name length; and stripping `-type` off an AID root wrongly merges
  `<card-type>` into `<card>`.
- Multi tests reuse evidence quotes from the single tests. Safe because layer 0
  re-validates every quote against the split document on each run.
- Layer 1 requires `primary` reached plus half the remaining cluster.
- Tests needing query decomposition are excluded from the score and reported
  separately. Currently 1 of 65. Report as a finding if it exceeds ~20%.
- The guide contains no procedures. Any ordering in an answer is inference and
  belongs in `inference_flags`.

**Dead end, already tested, do not retry:** mining the document's own 574
`(p. N)` cross-references to derive clusters. Hub commands (`clear`,
`terminate`, `show`) connect everything into one 267-command component with only
2 mutual pairs. Useless for clustering.

See [[feedback-scratch-repo-write-access]] for standing permission to edit this
repo without asking.
