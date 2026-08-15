"""Authoring aid: build a multi-command test batch with the conventions enforced.

Import `Batch`, call `add()` once per test, then `write()`. Nothing is written
unless every record passes, so a batch is all-or-nothing rather than partly
broken.

    from _authoring import Batch
    b = Batch()
    b.add(id="multi-...", primary="244-pm-threshold.md", archetype="troubleshooting",
          cluster_basis="topic:...", question="...", answer="...",
          files=["244-pm-threshold.md", ...], facts=[...], route_terms=[...],
          evidence=[("244-pm-threshold.md", "verbatim quote"), ...])
    b.write("tests/multi-08-whatever.jsonl")

The checks exist because reviewing batch 1 found each of these defects in
records that had already passed every layer of run_tests.py:

  - `cluster` and `names_command` are derived from the corpus, never accepted as
    arguments. Batch 1 hand-typed 24 wrong domain slugs and one wrong
    names_command.
  - a fact must appear in the cited files and be rare enough to discriminate.
    Batch 1 shipped facts present in 243, 348 and 206 of 395 files, which a
    candidate answer would satisfy by accident.
  - every hyphenated identifier in the answer must appear in a cited file.
    Nothing checked the answer prose before, which is how a reference answer
    came to assert `default-pm-supervision`, a parameter that does not exist.
  - every cited file must actually be discussed in the answer, and the primary
    must be cited.
"""
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
sys.path.insert(0, HERE)
import gxpaths                                                    # noqa: E402
import run_tests as R                                             # noqa: E402

C = "06-operation-commands/"
RECS = {json.loads(l)["name"]: json.loads(l) for l in
        open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"), encoding="utf-8")}
BY_FILE = {r["file"]: r for r in RECS.values()}
# Keyed by file, matching run_tests.CORPUS: chapters 3 to 5 put several
# commands in one file, and keying by name counted such a file once per command
# it holds, inflating every frequency measured over it.
BODIES = {r["file"]: open(os.path.join(gxpaths.DOCS, r["file"].replace("/", os.sep)),
                          encoding="utf-8").read().lower() for r in RECS.values()}
NFILES = len(BODIES)

FACT_MAX = 0.25        # a fact in more than a quarter of the files proves nothing
MIN_MEMBERS, MAX_MEMBERS = 3, 8


def corpus_freq(s):
    return sum(1 for b in BODIES.values() if s.lower() in b)


class Batch:
    def __init__(self):
        self.tests, self.problems = [], []

    def add(self, id, primary, archetype, question, answer, files, facts,
            route_terms, evidence, cluster_basis, inference_flags=(),
            requires_decomposition=False, decomposition_note=None):
        p, files = C + primary, [C + f for f in files]
        if p not in files:
            self.problems.append(f"{id}: primary not among the cited files")
            return
        if not MIN_MEMBERS <= len(files) <= MAX_MEMBERS:
            self.problems.append(f"{id}: cluster size {len(files)}")
        if len(evidence) < 2:
            self.problems.append(f"{id}: fewer than 2 evidence quotes")
        bodies = " ".join(BODIES[BY_FILE[f]["name"]] for f in files)
        pname = BY_FILE[p]["name"]

        for f in facts:
            if f.lower() not in bodies:
                self.problems.append(f"{id}: fact {f!r} not in the cited files")
            elif corpus_freq(f) > NFILES * FACT_MAX:
                self.problems.append(
                    f"{id}: fact {f!r} too common ({corpus_freq(f)}/{NFILES})")
        # per-fact frequency is a blunt instrument on its own: it condemns `30`
        # and `false` when they are the answer, and acquits a set of ordinary
        # words that together still match a fifth of the corpus. run_tests.py
        # gates on these three; check them here so a batch fails at write time.
        if facts:
            hits = sum(1 for b in BODIES.values()
                       if all(f.lower() in b for f in facts))
            if hits > 4:
                self.problems.append(
                    f"{id}: fact set does not discriminate, {hits}/{NFILES} files satisfy it")
            if min(corpus_freq(f) for f in facts) > NFILES // 20:
                self.problems.append(f"{id}: no fact is specific to this command")
            for f in facts:
                if not R.carries(f, answer):
                    self.problems.append(
                        f"{id}: reference answer does not contain its own fact {f!r}")
            # The operator asking the question has to end up knowing what to
            # type, so the command name is a required fact, not a nicety.
            if not any(f.lower() == pname.lower() for f in facts):
                self.problems.append(
                    f"{id}: the command name {pname!r} is not among the facts")
        for ev_file, quote in evidence:
            body = open(os.path.join(gxpaths.DOCS, (C + ev_file).replace("/", os.sep)),
                        encoding="utf-8").read()
            if quote not in body:
                self.problems.append(f"{id}: quote not verbatim in {ev_file}: {quote[:50]!r}")
        for f in files:
            if BY_FILE[f]["name"].lower() not in answer.lower():
                self.problems.append(f"{id}: cited but undiscussed: {BY_FILE[f]['name']}")
        for tok in sorted({m.group(0) for m in
                           re.finditer(r"\b[a-z][a-z0-9]*(?:-[a-z0-9]+)+\b", answer.lower())}):
            if tok in R.PROSE_STOP or tok in bodies or tok.rstrip("s") in bodies:
                continue
            self.problems.append(f"{id}: identifier {tok!r} in the answer, not in a cited file")

        self.tests.append({
            "id": id, "type": "multi",
            "cluster": BY_FILE[p]["domain"],                        # derived
            "cluster_basis": cluster_basis, "primary": p,
            "archetype": archetype,
            "names_command": bool(re.search(                        # derived
                rf"\b{re.escape(pname.lower())}\b", question.lower())),
            "question": question, "approximate_answer": answer,
            "expect": {"files": files, "facts": list(facts),
                       "route_terms": list(route_terms)},
            "evidence": [{"file": C + f, "quote": q} for f, q in evidence],
            "inference_flags": list(inference_flags), "weak": False,
            **({"requires_decomposition": True,
                "decomposition_note": decomposition_note} if requires_decomposition else {}),
        })

    def write(self, path):
        if self.problems:
            for p in self.problems:
                print("  PROBLEM:", p)
            print(f"\n{len(self.problems)} problems, nothing written")
            return False
        with open(path, "w", encoding="utf-8") as f:
            for t in self.tests:
                f.write(json.dumps(t, ensure_ascii=False) + "\n")
        print(f"wrote {len(self.tests)} tests to {path}")
        return True
