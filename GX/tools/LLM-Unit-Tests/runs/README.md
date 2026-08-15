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
```

```bash
python ../run_tests.py --answers run-01/answers.jsonl   # score one run
python ../compare_runs.py                               # score them all, and compare
```

## Adding a run

Give an agent the question ids and text, the path to `R9_1_GX_CLI_Reference/`,
and an explicit prohibition on reading anything under `tools/`. That last part
is not a formality: the tests directory holds the expected facts and the
reference answers, and an agent that reads it is measuring nothing. Shard the
441 questions so no single agent has to hold them all, write each shard's
answers out, and concatenate.

Record in `run.json` what would be needed to judge the result later: the model,
the date, how the answers were produced, and the commits the corpus and the
tests were at.

## Scores move when the tests move

Run 01 scored 371/441 on the day it was made and 427/441 a day later, without a
word of it changing. The tests had been demanding facts the questions never
asked for, and repairing that was worth 56 tests. So a score is meaningless
without the tests commit beside it, and two runs are comparable only at the same
one. `compare_runs.py` handles this by rescoring every run against the working
tree rather than trusting any stored number.

## Why more than one

One run is a point estimate. It cannot distinguish a question the corpus
genuinely cannot answer from one that happened to come out badly, and those two
want opposite responses: the first is a documentation gap to fix, the second is
noise to ignore. A second run splits them, which is why `compare_runs.py`
reports "failed by every run" separately from "failed by some".
