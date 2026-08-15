"""Score every stored run against the current tests, and show where they differ.

    python compare_runs.py           # one line per run, then the disagreements
    python compare_runs.py -v        # also list every test any run failed

A single run gives a point estimate and no idea of its error bar. Storing runs
under `runs/run-NN/` and scoring them together is what turns "97%" into a range,
and shows how often each test fails rather than whether it ever did.

**A test has a failure rate, not a persistent/noise label.** This tool used to
split "failed by every run" from "failed by some", and that binary was wrong in
both directions. `multi-ipsec-policy-nesting` failed runs 01 and 02, passed run
03 and so was reported as noise, then failed run 04, which puts it among the
most consistent failures in the suite. And "failed by every run" gets harder to
satisfy with each run added, so it says less the more evidence there is. The
rate is reported instead, over the runs that answered each test.

**Every run is scored against the working tree**, never against whatever the
tests looked like on the day it was produced. That is deliberate. Run 01 scored
371/441 when it was made and 427/441 after the fact repair, and the difference
was entirely the tests demanding material the questions never asked for. Scores
from different tests commits are not comparable, so this rescores everything.

**A rate is only as current as the corpus behind it.** Rescoring fixes the
tests side and does nothing for the other one: the answers are fixed text, so a
run cannot benefit from an index built after it. `multi-ipsec-policy-nesting`
failed runs 01, 02 and 04 and passed run 05, which reads as 3-in-5 and is not,
because the Containment section that fixed it landed between run 04 and run 05.
Three of those runs were answering against an index that lacked the fix. So the
failures are shown in run order rather than as a set, and each run records the
index it read; a rate whose failures all sit before the last index change is
evidence about a corpus that no longer exists.

Adding a run:

    runs/run-NN/answers.jsonl    JSONL of {"id": ..., "answer": ...}, one per test
    runs/run-NN/run.json         label, date, model, method, and the commits
    runs/run-NN/corpus.json      index digest, written by prepare_run --collect

`corpus.json` is optional and older runs predate it. It is machine-written, and
not derived from git: an agent environment need not have a repository, and
`run.json`'s hand-typed `corpus_commit` was never read by anything, which is
how the mixed-corpus rate above went unnoticed for four runs.

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
        corpus = None
        cpath = os.path.join(d, "corpus.json")
        if os.path.exists(cpath):
            corpus = json.load(open(cpath, encoding="utf-8"))
        rows = {}
        for line in open(answers, encoding="utf-8"):
            if line.strip():
                r = json.loads(line)
                rows[r["id"]] = r["answer"]
        out.append((name, meta, rows, corpus))
    return out


def report_corpus(runs):
    """Show which index each run read, and where that changed.

    Returns the position of the newest run that read a different index than the
    run before it, or None. Everything before that position is evidence about a
    corpus that has since been edited.
    """
    if not any(c for _, _, _, c in runs):
        print("\n  No run records the index it read, so a rate here cannot tell "
              "a stale\n  failure from a current one. Runs collected from now on "
              "will: prepare_run.py\n  --collect writes corpus.json. Until then, "
              "check by hand whether the index\n  changed under any rate you are "
              "about to act on.")
        return None

    print("\n== the index each run read ==")
    boundary, boundary_name, prev = None, None, None
    for pos, (name, _, _, corpus) in enumerate(runs):
        if corpus is None:
            print(f"  {name}  (not recorded)")
            prev = None
            continue
        digest = corpus["digest"]
        if prev is None:
            print(f"  {name}  {digest}")
        elif digest == prev["digest"]:
            print(f"  {name}  {digest}   unchanged")
        else:
            changed = [f for f, h in corpus.get("files", {}).items()
                       if prev.get("files", {}).get(f) != h]
            gone = [f for f in prev.get("files", {}) if f not in corpus.get("files", {})]
            what = ", ".join(changed + [f"{f} (removed)" for f in gone]) or "?"
            print(f"  {name}  {digest}   changed: {what}")
            boundary, boundary_name = pos, name
        prev = corpus
    if boundary is not None:
        print(f"  The index last changed at {boundary_name}. A failure recorded "
              f"before it was\n  produced against an index that no longer exists.")
    return boundary


def main():
    tests = R.load_tests()
    runs = load_runs()
    if not runs:
        sys.exit(f"no runs found under {RUNS}")

    missing_by_test = collections.defaultdict(list)
    scores = []
    for name, meta, rows, _ in runs:
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

    names = [n for n, _, _, _ in runs]
    p = [passed / answered for _, _, passed, answered in scores]
    print(f"\n  spread: {min(p)*100:.1f}% to {max(p)*100:.1f}% "
          f"over {len(names)} runs")

    # A failure rate, not a persistent/unstable label. The binary this replaced
    # called `multi-ipsec-policy-nesting` noise after it failed runs 01 and 02
    # and passed run 03; it then failed run 04, making it the joint most
    # consistent failure in the suite. "Failed by every run" also gets
    # mechanically harder to satisfy with each run added, so it says less the
    # more evidence you have, which is backwards.
    #
    # The denominator is runs that ANSWERED the test, not runs. Batch 15
    # postdates runs 01 and 02, so a batch 15 test failing both runs that
    # answered it is at 100%, and that is not the same evidence as 4 of 4.
    rates = []
    for t in tests:
        tid = t["id"]
        answered_by = sum(1 for _, _, rows, _ in runs if tid in rows)
        failed_by = len(missing_by_test.get(tid, []))
        if failed_by:
            rates.append((failed_by / answered_by, failed_by, answered_by, tid))
    rates.sort(key=lambda r: (-r[0], -r[2], r[3]))

    if not rates:
        print("  no test failed in any run")
        return 0

    hard = [r for r in rates if r[0] >= 0.75]
    mid = [r for r in rates if 0.25 < r[0] < 0.75]
    once = [r for r in rates if r[0] <= 0.25]
    print(f"  {len(rates)} tests failed at least once: "
          f"{len(hard)} at 75% or more, {len(mid)} in between, {len(once)} at 25% or less")
    print("  Rate is failures / runs that answered the test. Read the top of "
          "this list as\n  the real candidates and the bottom as noise; there is "
          "no clean line between them.")

    boundary = report_corpus(runs)

    # Run order, not a set of labels. Same numerator and denominator, different
    # story: `X X . . X` is a test that keeps failing, `X X . X .` may be one
    # that was fixed. The set notation this replaced hid that distinction, and
    # hid it hardest on exactly the test the fix was built for.
    print("\n== failure rate, worst first ==")
    print(f"   columns run oldest to newest, {names[0]} to {names[-1]}: "
          f"X failed, . passed, - not answered")
    shown = rates if "-v" in sys.argv else rates[:15]
    for rate, failed, answered, tid in shown:
        failed_in = set(missing_by_test[tid])
        seq, failed_at = "", []
        for pos, (name, _, rows, _) in enumerate(runs):
            if tid not in rows:
                seq += "-"
            elif name in failed_in:
                seq += "X"
                failed_at.append(pos)
            else:
                seq += "."
        # "All failures predate the index change" is a statement about evidence,
        # not a claim the change fixed anything: a test that happened to pass in
        # the newest run gets it too. The count of runs since is what separates
        # the two, and right after a change that count is 1, which is nothing.
        stale = ""
        if boundary is not None and failed_at and max(failed_at) < boundary:
            since = sum(1 for pos, (_, _, rows, _) in enumerate(runs)
                        if pos >= boundary and tid in rows)
            stale = (f"   [failures predate the index change; "
                     f"{since} run{'s' if since != 1 else ''} since]")
        print(f"   {rate*100:3.0f}%  {failed}/{answered}  {' '.join(seq)}  "
              f"{tid}{stale}")
    if len(shown) < len(rates):
        print(f"   ... and {len(rates) - len(shown)} more at or below "
              f"{shown[-1][0]*100:.0f}%; pass -v for all")
    return 0


if __name__ == "__main__":
    sys.exit(main())
