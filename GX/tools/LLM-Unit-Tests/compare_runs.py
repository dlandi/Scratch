"""Score every stored run against the current tests, and show where they differ.

    python compare_runs.py           # one line per run, then the disagreements
    python compare_runs.py -v        # also list every test any run failed

A single run gives a point estimate and no idea of its error bar. Storing runs
under `runs/run-NN/` and scoring them together is what turns "97%" into a range,
and separates a test the corpus consistently fails from one that merely came out
badly once.

**Every run is scored against the working tree**, never against whatever the
tests looked like on the day it was produced. That is deliberate. Run 01 scored
371/441 when it was made and 427/441 after the fact repair, and the difference
was entirely the tests demanding material the questions never asked for. Scores
from different tests commits are not comparable, so this rescores everything.

Adding a run:

    runs/run-NN/answers.jsonl    JSONL of {"id": ..., "answer": ...}, one per test
    runs/run-NN/run.json         label, date, model, method, and the commits

Generate the answers with an agent that can read `R9_1_GX_CLI_Reference/` and
nothing under `tools/`, or the expected facts leak into the thing being measured.
"""
import collections
import json
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
sys.path.insert(0, HERE)
import run_tests as R                                            # noqa: E402

RUNS = os.path.join(HERE, "runs")


def load_runs():
    out = []
    for name in sorted(os.listdir(RUNS)):
        d = os.path.join(RUNS, name)
        answers = os.path.join(d, "answers.jsonl")
        if not os.path.isdir(d) or not os.path.exists(answers):
            continue
        meta = {}
        mpath = os.path.join(d, "run.json")
        if os.path.exists(mpath):
            meta = json.load(open(mpath, encoding="utf-8"))
        rows = {}
        for line in open(answers, encoding="utf-8"):
            if line.strip():
                r = json.loads(line)
                rows[r["id"]] = r["answer"]
        out.append((name, meta, rows))
    return out


def main():
    tests = R.load_tests()
    runs = load_runs()
    if not runs:
        sys.exit(f"no runs found under {RUNS}")

    missing_by_test = collections.defaultdict(list)
    scores = []
    for name, meta, rows in runs:
        absent = [t["id"] for t in tests if t["id"] not in rows]
        passed = 0
        for t in tests:
            a = rows.get(t["id"])
            if a is None:
                continue
            miss = [f for f in t["expect"]["facts"] if not R.carries(f, a)]
            if miss:
                missing_by_test[t["id"]].append(name)
            else:
                passed += 1
        answered = len(tests) - len(absent)
        scores.append((name, meta, passed, answered))
        model = meta.get("model", "?")
        date = meta.get("date", "?")
        pct = 100 * passed / answered if answered else 0
        print(f"  {name}  {passed}/{answered} ({pct:.1f}%)   {model}, {date}"
              + (f"   [{len(absent)} unanswered]" if absent else ""))

    if len(runs) == 1:
        print("\nOne run, so no variance to report. The score above is a point "
              "estimate:\nadd runs/run-02 to find out what it is worth.")
        return 0

    names = [n for n, _, _ in runs]
    always = [t for t, who in missing_by_test.items() if len(who) == len(names)]
    sometimes = {t: who for t, who in missing_by_test.items() if len(who) < len(names)}
    p = [passed / answered for _, _, passed, answered in scores]
    print(f"\n  spread: {min(p)*100:.1f}% to {max(p)*100:.1f}% "
          f"over {len(names)} runs")
    print(f"  failed by every run:   {len(always)}   (the corpus or the test, not luck)")
    print(f"  failed by some runs:   {len(sometimes)}   (unstable, and the error bar)")

    if sometimes:
        print("\n== unstable, worth reading before trusting either verdict ==")
        for t, who in sorted(sometimes.items()):
            print(f"   {t}: failed in {', '.join(who)}")
    if "-v" in sys.argv and always:
        print("\n== failed by every run ==")
        for t in sorted(always):
            print(f"   {t}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
