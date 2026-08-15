"""Run the LLM unit tests against the GX documentation index.

    python run_tests.py                 # validate + retrieval (no model needed)
    python run_tests.py --validate      # only check the tests themselves
    python run_tests.py --answers a.jsonl   # also score key facts in answers
    python run_tests.py --prose         # list identifiers in answers to hand-check
    python run_tests.py --render        # write TESTS.md for human reading
    python run_tests.py -v              # show every test, not just failures

Three layers, in increasing cost:

  0. VALIDATE   Every evidence quote appears verbatim in the file it cites, and
                every expected file exists. This guards against a test that
                asserts something the document does not say. It runs first and
                nothing else is trusted if it fails.
  1. RETRIEVAL  Given only the question text, does the index route to the
                expected file? Simulates what an agent does: match the question
                against topics.md search terms, INDEX.md command names,
                parameters.md parameter names and entities.md AID prefixes.
                Deterministic, no model required. A single-command test must
                reach its one file; a multi-command test must reach its
                `primary` file plus half of the rest of the cluster.
  2. FACTS      Given a candidate answer, does it contain the required facts?
                Needs an answers file: JSONL of {"id": ..., "answer": ...}.

Layer 3, judging prose against `approximate_answer`, is deliberately not
implemented here: it needs a model at run time. The reference answers are in
the test files for that purpose.
"""
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
import gxpaths                                                    # noqa: E402

DOCS = gxpaths.DOCS
TESTS = os.path.join(HERE, "tests")


def load_tests():
    out = []
    for fn in sorted(os.listdir(TESTS)):
        if fn.endswith(".jsonl"):
            for n, line in enumerate(open(os.path.join(TESTS, fn), encoding="utf-8"), 1):
                if line.strip():
                    try:
                        out.append(json.loads(line))
                    except json.JSONDecodeError as e:
                        sys.exit(f"{fn}:{n}: bad JSON: {e}")
    return out


def read(rel):
    path = os.path.join(DOCS, rel.replace("/", os.sep))
    with open(path, encoding="utf-8") as f:
        return f.read()


KNOWN_FILES = {json.loads(l)["file"] for l in
               open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"),
                    encoding="utf-8")}

# Every command body, lowercased, for the discrimination checks below.
#
# Keyed by FILE, not by command name. Chapter 6 is one file per command so the
# two are the same there, but chapters 3 to 5 hold 4, 7 and 10 commands in a
# single file each. Keying by name counted those files once per command they
# hold, so a fact unique to the piped-commands file scored 10 against a
# MAX_MATCHING of 4 and no test for those chapters could ever pass layer 0.
# The gate always meant files, which is what corpus_hits documents and what
# the README states. Correcting it changes no verdict for the 441 tests that
# predate it.
CORPUS = {}
COMMANDS_IN = {}
for _l in open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"), encoding="utf-8"):
    _r = json.loads(_l)
    CORPUS[_r["file"]] = read(_r["file"]).lower()
    COMMANDS_IN.setdefault(_r["file"], []).append(_r["name"])
NFILES = len(CORPUS)

MAX_MATCHING = 4        # files a whole fact set may match: 1% of the corpus
ANCHOR_MAX = NFILES // 20   # one fact must be rarer than 5% of the corpus


def corpus_hits(facts):
    """How many command files contain every fact. The discrimination measure."""
    low = [f.lower() for f in facts]
    return sum(1 for b in CORPUS.values() if all(f in b for f in low))


def fact_freq(fact):
    f = fact.lower()
    return sum(1 for b in CORPUS.values() if f in b)


# --------------------------------------------------------------- layer 0
def validate(tests):
    problems = []
    seen = set()
    for t in tests:
        tid = t.get("id", "<no id>")
        if tid in seen:
            problems.append((tid, "duplicate id"))
        seen.add(tid)
        for field in ("question", "approximate_answer", "expect", "evidence"):
            if not t.get(field):
                problems.append((tid, f"missing {field}"))
        # multi tests name the file the question centres on, so scoring can
        # require it rather than accepting any half of the cluster
        if t.get("type") == "multi":
            primary = t.get("primary")
            if not primary:
                problems.append((tid, "multi test missing primary"))
            elif primary not in t.get("expect", {}).get("files", []):
                problems.append((tid, f"primary not in expect.files: {primary}"))
        for rel in t.get("expect", {}).get("files", []):
            if not os.path.exists(os.path.join(DOCS, rel.replace("/", os.sep))):
                problems.append((tid, f"expected file missing: {rel}"))
            elif rel not in KNOWN_FILES:
                # Windows paths are case-insensitive, so a wrong-case path still
                # opens here but would fail on Linux and never matches a record
                problems.append((tid, f"path case does not match the corpus: {rel}"))
        for ev in t.get("evidence", []):
            rel, quote = ev.get("file", ""), ev.get("quote", "")
            try:
                body = read(rel)
            except OSError:
                problems.append((tid, f"evidence file missing: {rel}"))
                continue
            if quote not in body:
                problems.append((tid, f"evidence quote not found verbatim in {rel}: "
                                      f"{quote[:60]!r}"))
        # a fact that appears in no expected file is almost certainly wrong
        facts = t.get("expect", {}).get("facts", [])
        for fact in facts:
            bodies = " ".join(read(r) for r in t["expect"]["files"]
                              if os.path.exists(os.path.join(DOCS, r.replace("/", os.sep))))
            if fact.lower() not in bodies.lower():
                problems.append((tid, f"fact not present in expected files: {fact!r}"))
        # ... and a fact set that half the corpus satisfies proves nothing when
        # a candidate answer contains it. Measure the set, not each fact: the
        # per-fact frequency `_authoring.py` checks calls `false` and `30` weak
        # when they are the whole answer to a default question, and passes
        # `show equipment`, whose single fact 88 of 395 files satisfy.
        if facts:
            hits = corpus_hits(facts)
            if hits > MAX_MATCHING:
                problems.append((tid, f"fact set does not discriminate: {hits} of "
                                      f"{NFILES} command files satisfy all of {facts}"))
            rarest = min(fact_freq(f) for f in facts)
            if rarest > ANCHOR_MAX:
                problems.append((tid, f"no fact is specific to this command: the rarest "
                                      f"appears in {rarest} of {NFILES} files"))
            # the reference answer is the one answer known to be right, so it
            # must pass the test's own layer 2. 22 of them did not: the guide
            # writes a range as `1..110` and the answer as "1 to 110", which
            # would have failed every candidate answer too.
            missing = [f for f in facts
                       if not carries(f, t.get("approximate_answer", ""))]
            if missing:
                problems.append((tid, f"reference answer does not contain its own "
                                      f"facts: {missing}"))
        # A test id is hyphenated and none of its words are decoration:
        # `equipment-show-filter` promises the object, the verb and the option
        # that narrows the listing, and an answer covering two of the three has
        # answered two thirds of the question. The first word is the one that
        # can be checked mechanically. 27 answers described a command's
        # attributes without ever naming the command, so nothing in the test
        # separated a right answer from one about a different object.
        key = t.get("primary") if t.get("type") == "multi" \
            else (t.get("expect", {}).get("files") or [None])[0]
        here = COMMANDS_IN.get(key, [])
        # A file holding several commands cannot say by itself which one the
        # test is about, and guessing picked the first one in the file, so a
        # test about `sort` would have been required to name `begin`. Such a
        # test declares `command`, and the declaration is checked against the
        # file so it cannot name something that is not there.
        declared = t.get("command")
        if declared and declared not in here:
            problems.append((tid, f"declared command {declared!r} is not in {key}"))
            name = None
        elif declared:
            name = declared
        elif len(here) > 1:
            problems.append((tid, f"{key} holds {len(here)} commands, so the test "
                                  f"must declare which one with 'command'"))
            name = None
        else:
            name = here[0] if here else None
        if name and not carries(name, t.get("approximate_answer", "")):
            problems.append((tid, f"reference answer never names the command it is "
                                  f"about: {name}"))
    return problems


# ------------------------------------------------- authoring aid: prose check
# English compounds that look like corpus identifiers but are not. Kept small
# and explicit: anything added here stops being checked, so add a word only
# after confirming the corpus really has no such identifier.
PROSE_STOP = {
    "read-only", "read-write", "far-side", "operator-driven", "type-keyed",
    "instance-keyed", "add-and-drop", "end-entity", "service-level", "per-card",
    "per-instance", "per-session", "per-type", "per-node", "per-parameter",
    "type-wide", "protocol-wide", "well-known", "roll-back", "hand-entered",
    "multi-chassis", "zero-touch", "self-contained", "non-standard", "x509v3",
    "sub-ports", "sub-components",
}


def prose(tests):
    """Flag identifiers asserted in approximate_answer but absent from the cited files.

    Layer 0 validates evidence quotes and the `facts` strings, but never the
    reference answer's prose. That gap is real: batch 1 shipped a fabricated
    parameter name, `default-pm-supervision`, which reads plausibly next to the
    genuine `default-data-supervision` and passed every layer. This is an
    authoring aid rather than a gate, because distinguishing a corpus identifier
    from an English compound needs judgement and a false failure here would be
    worse than the miss.
    """
    hits = []
    for t in tests:
        bodies = " ".join(read(f) for f in t["expect"]["files"]
                          if os.path.exists(os.path.join(DOCS, f.replace("/", os.sep)))).lower()
        toks = {m.group(0) for m in
                re.finditer(r"\b[a-z][a-z0-9]*(?:-[a-z0-9]+)+\b",
                            t.get("approximate_answer", "").lower())}
        for tok in sorted(toks):
            # a trailing plural of a real identifier is prose, not a claim
            if tok in PROSE_STOP or tok in bodies or tok.rstrip("s") in bodies:
                continue
            hits.append((t["id"], tok))
    return hits


# --------------------------------------------------------------- layer 1
def topic_pattern(term):
    """A topic search term matches at the start of a word, never inside one.

    The terms in `curated.py` are deliberately stemmed, so `protect` has to go
    on catching "protection" and "protected" and `subscri` has to catch
    "subscription" and "subscribed". That is why this matches a word prefix
    rather than a whole word.

    Terms shorter than four characters must be the whole word, because prefix
    matching does not save them: `ca`, added for Certificate Authority, fired
    inside "card", "carrier", "scan" and "because" on 197 of the 441 test
    questions and dragged the entire PKI topic in each time. `est` fired inside
    "test" and "latest", `ace` inside "interface", `led` inside "enabled".
    Matching topic terms as bare substrings cost 13 tests their honesty: they
    were scored as reaching the right file through a term that had fired on a
    fragment of an unrelated word.
    """
    t = re.escape(term.lower())
    tail = r"(?![a-z0-9])" if len(term) < 4 else ""
    return r"(?<![a-z0-9])" + t + tail


class Index:
    """The retrieval surface an agent actually has."""

    def __init__(self):
        self.index_md = read("INDEX.md")
        self.topics_md = read("index/topics.md")
        self.params_md = read("index/parameters.md")
        self.entities_md = read("index/entities.md")
        self.records = [json.loads(l) for l in
                        open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"),
                             encoding="utf-8")]
        self.by_name = {r["name"]: r for r in self.records}
        # topic section -> (search terms, commands)
        self.topics = []
        for block in self.topics_md.split("\n## ")[1:]:
            terms = re.findall(r"`([^`]+)`", block.split("\n")[2]) \
                if "*Search terms:*" in block else []
            cmds = re.findall(r"^\| `([^`]+)` \|", block, re.M)
            self.topics.append((terms, cmds))
        self.param_rows = {m.group(1).lower(): re.findall(r"`([^`]+)`", m.group(2))
                           for m in re.finditer(r"^\| `([^`]+)` \| ([^|]*) \|",
                                                self.params_md, re.M)}

    def route(self, question):
        """Files an agent could reach from this question alone."""
        q = question.lower()
        hits = set()
        why = []
        for terms, cmds in self.topics:                    # topic vocabulary
            if any(re.search(topic_pattern(t), q) for t in terms):
                for c in cmds:
                    if c in self.by_name:
                        hits.add(self.by_name[c]["file"])
                why.append("topics")
        for name, rec in self.by_name.items():             # command named outright
            # match case-insensitively: the corpus has mixed-case command names
            # such as L2-bridge, ISK and KRK
            if len(name) > 2 and re.search(rf"\b{re.escape(name.lower())}\b", q):
                hits.add(rec["file"])
                why.append("name")
        for param, cmds in self.param_rows.items():        # parameter named
            if len(param) > 4 and re.search(rf"\b{re.escape(param)}\b", q):
                for c in cmds:
                    if c in self.by_name:
                        hits.add(self.by_name[c]["file"])
                why.append("param")
        for m in re.finditer(r"^\| `([^`]+)` \| `[^`]+` \| `([^`]+)` \|",
                             self.entities_md, re.M):      # AID prefix
            if len(m.group(1)) > 3 and m.group(1).lower() in q:
                if m.group(2) in self.by_name:
                    hits.add(self.by_name[m.group(2)]["file"])
                    why.append("aid")
        return hits, sorted(set(why))


def retrieval(tests, idx, verbose):
    passed = failed = 0
    decomp = []
    for t in tests:
        want = set(t["expect"]["files"])
        got, why = idx.route(t["question"])
        found = want & got
        # single: the one file must be reachable. multi: the primary file, the
        # one the question centres on, plus half of the rest. No single query
        # realistically surfaces every member, but reaching any half is too weak
        # a bar: it passes when the query misses the very thing it asked about.
        if t["type"] == "single":
            ok, detail = len(found) >= len(want), ""
        else:
            primary = t["primary"]
            rest = want - {primary}
            got_primary = primary in got
            got_rest = len(found & rest)
            need_rest = (len(rest) + 1) // 2
            ok = got_primary and got_rest >= need_rest
            detail = (f" primary={'hit' if got_primary else 'MISS'}"
                      f" rest={got_rest}/{len(rest)} need {need_rest}")
        if not ok and t.get("requires_decomposition"):
            decomp.append(t["id"])
            if verbose:
                print(f"  [DECOMP] {t['id']}  ({len(found)}/{len(want)}) "
                      f"compound question, needs splitting before lookup")
            continue
        passed, failed = (passed + ok, failed + (not ok))
        if verbose or not ok:
            mark = "PASS" if ok else "FAIL"
            print(f"  [{mark}] {t['id']}  ({len(found)}/{len(want)} via "
                  f"{','.join(why) or 'nothing'}){detail}")
            if not ok:
                print(f"         Q: {t['question'][:96]}")
                for miss in sorted(want - got):
                    print(f"         unreachable: {miss}")
    if decomp:
        print(f"  {len(decomp)} compound question(s) excluded, they need the agent to "
              f"split the query first: {', '.join(decomp)}")
    return passed, failed


# --------------------------------------------------------------- layer 2
def _flat(s):
    return re.sub(r"[^a-z0-9]+", " ", s.lower()).strip()


def carries(fact, answer):
    """Does this answer state this fact?

    A fact has to mean what it says, because an operator reading the answer is
    going to type it. Two rules, and both exist because a plain substring test
    gets this wrong in opposite directions.

    It fires when it should not: `oc` is satisfied by "block", `ILA` by
    "available", `rib` by "describe", `add` by "address", `na` by "name". So a
    fact must start and end on a word boundary. The single exception is a
    trailing plural `s`, which is why `reboot` still matches "reboots"; any
    other trailing letter is a different word.

    It fails when it should not: the guide writes `software-load` and
    `next-hop`, an answer writes "software load" and "next hop", and the fact
    is stated either way. So a fact spanning more than one token also matches
    with its separators flattened.
    """
    if _boundary(fact, answer):
        return True
    if not re.search(r"[^A-Za-z0-9]", fact.strip()):
        return False
    if _boundary(fact, answer, flat=True):
        return True
    # ... and a value written against its unit closes up either way: the guide
    # writes `-23dBm`, an answer writes "-23 dBm", and flattening leaves
    # "23dbm" against "23 dbm". Comparing with the separators deleted rather
    # than spaced catches it. Held to 5 characters so this cannot rescue a
    # short token by gluing it to its neighbour.
    squash = lambda s: re.sub(r"[^a-z0-9]+", "", s.lower())
    return len(squash(fact)) >= 5 and squash(fact) in squash(answer)


def _boundary(fact, answer, flat=False):
    f, a = (_flat(fact), _flat(answer)) if flat else (fact.lower(), answer.lower())
    if not f:
        return False
    tail = r"(?![0-9])" if f[-1].isdigit() else r"s?(?![A-Za-z0-9])"
    return bool(re.search(r"(?<![A-Za-z0-9])" + re.escape(f) + tail, a))


def facts(tests, answers, verbose):
    passed = failed = skipped = 0
    for t in tests:
        ans = answers.get(t["id"])
        if ans is None:
            skipped += 1
            continue
        missing = [f for f in t["expect"]["facts"] if not carries(f, ans)]
        ok = not missing
        passed, failed = (passed + ok, failed + (not ok))
        if verbose or not ok:
            print(f"  [{'PASS' if ok else 'FAIL'}] {t['id']}"
                  + (f"  missing: {missing}" if missing else ""))
    return passed, failed, skipped


def render(tests):
    out = ["# GX CLI Reference - LLM unit tests", "",
           f"{sum(1 for t in tests if t['type'] == 'single')} single-command and "
           f"{sum(1 for t in tests if t['type'] == 'multi')} multi-command tests. "
           "Generated for reading; the machine-readable source is in `tests/`.", ""]
    for kind, title in (("single", "Single-command tests"),
                        ("multi", "Multi-command tests")):
        out += [f"## {title}", ""]
        for t in [x for x in tests if x["type"] == kind]:
            out += [f"### {t['id']}", "",
                    f"*{t.get('domain') or t.get('cluster')} / {t['archetype']}"
                    + ("  (weak: the source section is thin)" if t.get("weak") else "")
                    + "*", "",
                    f"**Q.** {t['question']}", "",
                    f"**A.** {t['approximate_answer']}", "",
                    "Source: " + ", ".join(f"`{f}`" for f in t["expect"]["files"]), ""]
            if t.get("inference_flags"):
                out += ["Not stated by the document: "
                        + " ".join(t["inference_flags"]), ""]
    path = os.path.join(HERE, "TESTS.md")
    open(path, "w", encoding="utf-8").write("\n".join(out))
    print(f"wrote {path}")


def main():
    verbose = "-v" in sys.argv
    tests = load_tests()
    print(f"loaded {len(tests)} tests "
          f"({sum(1 for t in tests if t['type'] == 'single')} single, "
          f"{sum(1 for t in tests if t['type'] == 'multi')} multi)")

    print("\n== layer 0: validate tests against the document ==")
    problems = validate(tests)
    for tid, msg in problems:
        print(f"  [FAIL] {tid}: {msg}")
    print(f"  {len(tests) - len({p[0] for p in problems})}/{len(tests)} tests valid")
    if problems:
        return 1

    if "--prose" in sys.argv:
        print("\n== authoring aid: identifiers in answers, not in the cited files ==")
        hits = prose(tests)
        for tid, tok in hits:
            print(f"  [CHECK] {tid}: {tok!r}")
        print(f"  {len(hits)} to review by hand (not a pass/fail gate)")

    if "--render" in sys.argv:
        render(tests)

    if "--validate" in sys.argv:
        return 0

    print("\n== layer 1: retrieval from the index ==")
    idx = Index()
    p, f = retrieval(tests, idx, verbose)
    print(f"  {p}/{p + f} routed to the expected file(s)")

    rc = 1 if f else 0
    if "--answers" in sys.argv:
        path = sys.argv[sys.argv.index("--answers") + 1]
        answers = {}
        for line in open(path, encoding="utf-8"):
            if line.strip():
                r = json.loads(line)
                answers[r["id"]] = r["answer"]
        print("\n== layer 2: key facts in candidate answers ==")
        p2, f2, sk = facts(tests, answers, verbose)
        print(f"  {p2}/{p2 + f2} answers carried every required fact ({sk} not answered)")
        rc = rc or (1 if f2 else 0)
    return rc


if __name__ == "__main__":
    sys.exit(main())
