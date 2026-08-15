"""Measure what the index drags in, not just what it reaches.

    python precision.py             # headline numbers and the worst terms
    python precision.py --terms     # every search term, ranked by noise
    python precision.py --dead      # terms no test question fires

Layer 1 scores recall: can the question reach the right file. It cannot see an
over-broad search term that reaches the right file *and* fifty others, so about
70 vocabulary additions have gone into `../curated.py` with nothing measuring
their collateral damage. This is the other half.

The measurement needs no new data. Every one of the 441 test questions is a real
query with a known answer, so for each one:

    precision = |expected ∩ routed| / |routed|

and the routed set can be attributed to the term, command name, parameter or AID
prefix that produced each file. That gives a per-term damage figure: how many
irrelevant files this term has pulled in across the whole suite.

**A term is falsifiable when you can ask whether anything needs it.** `necessary`
is true when some test has an expected file that this term alone reaches, so
deleting the term would cost recall. A term that is never necessary and pulls in
noise is a candidate for deletion, which is the check `curated.py` edits have
never had.

Read `necessary` honestly. It means "needed by a question someone has written",
not "needed". Search terms exist to catch phrasings the suite does not contain,
so an unnecessary term may still be earning its keep against real operators. The
ranking is an argument to review a term, never a licence to delete it
mechanically.
"""
import collections
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
sys.path.insert(0, HERE)
import run_tests as R                                            # noqa: E402


class TracingIndex(R.Index):
    """route(), but recording what produced each file."""

    def route_why(self, question):
        q = question.lower()
        hits = collections.defaultdict(set)
        for terms, cmds in self.topics:
            for t in terms:
                if re.search(R.topic_pattern(t), q):
                    for c in cmds:
                        if c in self.by_name:
                            hits[self.by_name[c]["file"]].add(("topic", t))
        for name, rec in self.by_name.items():
            if len(name) > 2 and re.search(rf"\b{re.escape(name.lower())}\b", q):
                hits[rec["file"]].add(("name", name))
        for param, cmds in self.param_rows.items():
            if len(param) > 4 and re.search(rf"\b{re.escape(param)}\b", q):
                for c in cmds:
                    if c in self.by_name:
                        hits[self.by_name[c]["file"]].add(("param", param))
        for m in re.finditer(r"^\| `([^`]+)` \| `[^`]+` \| `([^`]+)` \|",
                             self.entities_md, re.M):
            if len(m.group(1)) > 3 and m.group(1).lower() in q:
                if m.group(2) in self.by_name:
                    hits[self.by_name[m.group(2)]["file"]].add(("aid", m.group(1)))
        return hits


def measure(tests, idx):
    per_term = collections.defaultdict(
        lambda: {"fires": 0, "pulled": 0, "noise": 0, "necessary": False, "topics": set()})
    sizes, precisions = [], []
    for t in tests:
        want = set(t["expect"]["files"])
        hits = idx.route_why(t["question"])
        got = set(hits)
        sizes.append(len(got))
        if got:
            precisions.append(len(want & got) / len(got))
        fired = {r for reasons in hits.values() for r in reasons if r[0] == "topic"}
        for r in fired:
            per_term[r[1]]["fires"] += 1
        for f, reasons in hits.items():
            for r in reasons:
                if r[0] != "topic":
                    continue
                per_term[r[1]]["pulled"] += 1
                if f not in want:
                    per_term[r[1]]["noise"] += 1
        # a term is necessary when it is the only thing reaching an expected file
        for f in want & got:
            reasons = hits[f]
            if len(reasons) == 1 and list(reasons)[0][0] == "topic":
                per_term[list(reasons)[0][1]]["necessary"] = True
    return per_term, sizes, precisions


def main():
    tests = R.load_tests()
    idx = TracingIndex()
    for terms, cmds in idx.topics:
        for t in terms:
            pass
    per_term, sizes, precisions = measure(tests, idx)
    all_terms = {t for terms, _ in idx.topics for t in terms}

    print(f"{len(tests)} questions against {len(idx.records)} commands, "
          f"{len(all_terms)} search terms in curated.py\n")
    s = sorted(sizes)
    print("== how much the index hands back ==")
    print(f"  files per question: median {s[len(s)//2]}, mean {sum(s)/len(s):.1f}, max {s[-1]}")
    for c in (10, 25, 50):
        n = sum(1 for x in s if x > c)
        print(f"  questions returning more than {c:2d} files: {n} ({100*n//len(s)}%)")
    print(f"  mean precision: {sum(precisions)/len(precisions):.3f}")

    ranked = sorted(per_term.items(), key=lambda kv: -kv[1]["noise"])
    dead = sorted(all_terms - {k for k, v in per_term.items() if v["fires"]})
    unnecessary = [(k, v) for k, v in ranked if not v["necessary"] and v["fires"]]

    if "--dead" in sys.argv:
        print(f"\n== {len(dead)} terms no test question fires ==")
        print("   (not evidence of a bad term: the suite may simply not ask that way)")
        for t in dead:
            print(f"   {t}")
        return 0

    print(f"\n== terms pulling in the most irrelevant files ==")
    print(f"   {'term':32} {'fires':>6} {'noise':>7} {'needed?':>8}")
    show = ranked if "--terms" in sys.argv else ranked[:20]
    for k, v in show:
        if not v["fires"]:
            continue
        print(f"   {k:32} {v['fires']:6d} {v['noise']:7d} "
              f"{'yes' if v['necessary'] else 'NO':>8}")

    print(f"\n== {len(unnecessary)} terms no test needs, ranked by the noise they add ==")
    print("   Deleting one costs no recall the suite can see. That is an argument")
    print("   to review it, not to delete it: terms exist for phrasings nobody wrote.")
    for k, v in unnecessary[:25]:
        print(f"   {k:32} fires {v['fires']:3d}, adds {v['noise']:4d} irrelevant files")
    print(f"\n   {len(dead)} further terms fire on no test question (--dead)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
