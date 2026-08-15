# Stored model runs

Candidate answers from a real model, kept so the tests can be re-scored against
them for free after an edit. Layer 2 is the only layer that costs money to
exercise, so throwing the answers away and paying again is the one avoidable
expense here.

```
runs/
  run-01/
    answers.jsonl    JSONL of {"id": ..., "answer": ...}, one line per test
    run.json         label, date, model, method, and the commits it was made at
    corpus.json      digest of the index this run read, written by --collect
```

`run.json` is written by hand and holds the prose. `corpus.json` is measured and
holds the facts a comparison depends on; see "The index moves too" below.

```bash
python ../run_tests.py --answers run-01/answers.jsonl   # score one run
python ../compare_runs.py                               # score them all, and compare
```

## Adding a run

Give an agent the question ids and text, the path to `R9_1_GX_CLI_Reference/`,
and an explicit prohibition on reading anything under `tools/`. That last part
is not a formality: the tests directory holds the expected facts and the
reference answers, and an agent that reads it is measuring nothing. Shard the
457 questions so no single agent has to hold them all, write each shard's
answers out, and concatenate.

`prepare_run.py` does the sharding and, on `--collect`, the concatenation and
the corpus digest. Record in `run.json` what would be needed to judge the result
later: the model, the date, and how the answers were produced.

## Scores move when the tests move

Run 01 scored 371/441 on the day it was made and 427/441 a day later, without a
word of it changing. The tests had been demanding facts the questions never
asked for, and repairing that was worth 56 tests. So a score is meaningless
without the tests commit beside it, and two runs are comparable only at the same
one. `compare_runs.py` handles this by rescoring every run against the working
tree rather than trusting any stored number.

## The index moves too

Rescoring fixes one side of that and not the other. The tests can be re-run
against stored answers; the answers themselves are frozen text, so a run can
never benefit from an index built after it.

This was learned on `multi-ipsec-policy-nesting`, which failed runs 01, 02 and
04 and passed run 05. As a rate that is 3 in 5. It is not: the Containment
section in `entities.md` was added between run 04 and run 05 precisely because
that test kept failing, so three of those five runs were answering against an
index that lacked the fix. The rate mixed two corpora and read as current.

So `--collect` writes `corpus.json`, a SHA-256 digest of `index/`, `INDEX.md`
and `NAVIGATION.md`, per file and overall. `compare_runs.py` reads it and says
which runs read a different index, and which file changed. The content slices
are excluded on purpose: `step1` verifies on every build that they reassemble
byte-exactly to the source, so they cannot drift without the source revision
changing, which is a larger event than this file describes.

Deliberately not derived from git. Not every environment that runs this has a
repository. `run.json` has carried a hand-typed `corpus_commit` since run 01 and
nothing ever read it, which is how the mixed-corpus rate survived four runs.

Runs 01 to 05 predate `corpus.json` and cannot be backfilled, because
reconstructing an old index means checking out an old commit, which is the
dependency being avoided. Their `corpus_note` fields describe the changes in
prose. The measured signal starts at run 06.

## Why more than one

One run is a point estimate. It cannot distinguish a question the corpus
genuinely cannot answer from one that happened to come out badly, and those two
want opposite responses: the first is a documentation gap to fix, the second is
noise to ignore.

More runs split them, but not into two piles. `compare_runs.py` used to report
"failed by every run" separately from "failed by some", and that binary was
wrong in both directions: it called `multi-ipsec-policy-nesting` noise after one
passing run, and "failed by every run" gets mechanically harder to satisfy with
each run added, so it says less the more evidence there is. A test has a failure
rate, over the runs that answered it.

The rate is shown as a sequence in run order, oldest to newest, because the same
rate can mean opposite things. `X X . . X` is a test that keeps failing.
`X X . X .` may be one that was fixed, and that is the shape the ipsec test had.
