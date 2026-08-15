"""Set up the next model run: shard the questions and print the prompt.

    python prepare_run.py run-02          # write the shards, print what to do
    python prepare_run.py run-02 --size 25

Writes `runs/<label>/questions/questions-NN.jsonl`, each holding the id and text
of up to `--size` tests and nothing else. The expected files, facts, evidence and
reference answers stay out of them, because an agent that sees those is
measuring itself.

Then dispatch one agent per shard with the prompt in `runs/prompt.md`, verbatim,
and collect each shard's answers. When they are all in:

    python prepare_run.py run-02 --collect     # concatenate and sanity check
    python compare_runs.py                     # score it beside the others
"""
import json
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
sys.path.insert(0, HERE)
import run_tests as R                                            # noqa: E402

RUNS = os.path.join(HERE, "runs")


def collect(label):
    d = os.path.join(RUNS, label)
    qdir = os.path.join(d, "questions")
    tests = {t["id"] for t in R.load_tests()}
    rows, seen = [], set()
    for fn in sorted(os.listdir(qdir)):
        if not fn.startswith("answers-"):
            continue
        for line in open(os.path.join(qdir, fn), encoding="utf-8"):
            if not line.strip():
                continue
            r = json.loads(line)
            if r["id"] in seen:
                sys.exit(f"duplicate answer for {r['id']}")
            seen.add(r["id"])
            rows.append(r)
    missing, extra = tests - seen, seen - tests
    if extra:
        sys.exit(f"answers for ids that are not tests: {sorted(extra)[:5]}")
    out = os.path.join(d, "answers.jsonl")
    with open(out, "w", encoding="utf-8") as f:
        for r in rows:
            f.write(json.dumps({"id": r["id"], "answer": r["answer"]},
                               ensure_ascii=False) + "\n")
    avg = sum(len(r["answer"]) for r in rows) // max(1, len(rows))
    print(f"wrote {out}: {len(rows)} answers, mean {avg} characters")
    if missing:
        print(f"  WARNING: {len(missing)} tests unanswered, e.g. {sorted(missing)[:5]}")
    print(f"  now write {os.path.join(d, 'run.json')} (copy run-01's and edit),"
          f"\n  then: python compare_runs.py")


def main():
    if len(sys.argv) < 2 or sys.argv[1].startswith("-"):
        sys.exit(__doc__)
    label = sys.argv[1]
    d = os.path.join(RUNS, label)
    if "--collect" in sys.argv:
        return collect(label)
    if os.path.exists(os.path.join(d, "answers.jsonl")):
        sys.exit(f"{label} already has answers.jsonl; delete it first if you mean to redo it")
    size = 25
    if "--size" in sys.argv:
        size = int(sys.argv[sys.argv.index("--size") + 1])

    tests = R.load_tests()
    qdir = os.path.join(d, "questions")
    os.makedirs(qdir, exist_ok=True)
    shards = [tests[i:i + size] for i in range(0, len(tests), size)]
    for n, sh in enumerate(shards, 1):
        with open(os.path.join(qdir, f"questions-{n:02d}.jsonl"), "w",
                  encoding="utf-8") as f:
            for t in sh:
                f.write(json.dumps({"id": t["id"], "question": t["question"]},
                                   ensure_ascii=False) + "\n")
    print(f"wrote {len(shards)} shards of up to {size} questions to {qdir}\n")
    print("Dispatch one agent per shard with runs/prompt.md verbatim, substituting:")
    print(f"   <SHARD PATH>   {os.path.join(qdir, 'questions-NN.jsonl')}")
    print(f"   <OUTPUT PATH>  {os.path.join(qdir, 'answers-NN.jsonl')}")
    print("\nThe prompt is fixed on purpose. A second run measures variance only if")
    print("the method is held constant; reword it and you have measured the prompt.")
    print(f"\nWhen every shard is in:  python prepare_run.py {label} --collect")
    return 0


if __name__ == "__main__":
    sys.exit(main())
