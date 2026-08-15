"""Authoring aid: enumerate candidate clusters for multi-command tests.

    python _clusters.py            # all three bases, summary counts
    python _clusters.py aid        # AID key-path ancestry families
    python _clusters.py confusable # name pairs an operator could mix up
    python _clusters.py topic      # curated topics with enough members

A multi-command test asks one question that several command sections answer
together. Picking those sets by hand invites cherry-picking, so they are derived
mechanically from three independent bases and curated down afterwards. The basis
each cluster came from is recorded in the test's `cluster_basis` field.

  aid         Commands whose instance key path is rooted at another entity, so
              `port-<card>-<port>` is a child of `card`. This is containment: the
              cluster answers "what addresses a card, and what does a card hold".
  confusable  Command names close enough that an operator could reach for the
              wrong one. Feeds `disambiguation` questions.
  topic       The curated topics in index/topics.md, which are editorial
              groupings by subject rather than by structure.

Nothing here writes a test. It prints candidates for a human to choose from.
"""
import collections
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
import gxpaths                                                    # noqa: E402

sys.stdout.reconfigure(encoding='utf-8', errors='replace')

RECS = [json.loads(l) for l in
        open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"), encoding="utf-8")]
OPS = [r for r in RECS if r["category"] == "operation"]
BY_NAME = {r["name"]: r for r in RECS}

# A cluster smaller than this cannot carry a question that needs several
# sections; larger than this is a subject area, not a question.
MIN_MEMBERS = 3
MAX_MEMBERS = 8


# --------------------------------------------------------------- aid ancestry
def aid_families():
    """Group commands by the parent entity their key path is rooted at.

    `property-<card-name>/<property-name>` is rooted at `card`. Only `-name` and
    `-id` are stripped from the root: `-type` is meaning-bearing, and stripping
    it would merge `<card-type>`, which keys what a model of card supports, into
    `<card>`, which keys one installed card. Those are different questions.

    A root need not be a command itself. `<location>` and `<swload-state>` are
    real parents that no command addresses directly. Only truly generic
    placeholders are dropped: `<name>` heads 16 unrelated key paths and would
    produce a cluster with no subject.
    """
    generic = {"name", "index", "id"}
    fams = collections.defaultdict(set)
    for r in OPS:
        for e in r.get("entity_ids") or []:
            ph = re.findall(r"<([^>]+)>", e)
            if len(ph) < 2:                       # not a nested key path
                continue
            root = re.sub(r"-(name|id)$", "", ph[0])
            if root not in generic:
                fams[root].add(r["name"])
    return {f"hierarchy:{k}": sorted(v) for k, v in fams.items()
            if len(v) >= MIN_MEMBERS}


# ---------------------------------------------------------------- confusables
STEM_SUFFIXES = ("over", "ing", "ed", "es", "s")


def stem(tok):
    for suf in STEM_SUFFIXES:
        if len(tok) > len(suf) + 2 and tok.endswith(suf):
            return tok[:-len(suf)]
    return tok


def edit_distance(a, b):
    prev = list(range(len(b) + 1))
    for i, ca in enumerate(a, 1):
        cur = [i]
        for j, cb in enumerate(b, 1):
            cur.append(min(prev[j] + 1, cur[j - 1] + 1, prev[j - 1] + (ca != cb)))
        prev = cur
    return prev[-1]


def confusable_pairs():
    """Name pairs close enough to be reached for by mistake, with the reason.

    Four independent signals, because no single one covers the space: near
    identical spelling, one name extending another, a shared head noun under a
    different modifier, and a shared stem set where one token differs. The
    signal is reported so curation can judge whether the pair is really
    ambiguous to an operator or merely similar as a string.
    """
    names = sorted(r["name"] for r in OPS)
    pairs = {}
    for i, a in enumerate(names):
        ta, sa = a.split("-"), {stem(t) for t in a.split("-")}
        for b in names[i + 1:]:
            tb, sb = b.split("-"), {stem(t) for t in b.split("-")}
            why = None
            # Scale the spelling threshold to length. A flat distance of 2 is
            # meaningless on short names: `ace`, `add`, `adg`, `fc`, `mc`, `ne`
            # and `oc` are all within 2 of each other while sharing no meaning,
            # and they chain into one component that swamps every real family.
            shortest = min(len(a), len(b))
            if shortest >= 6 and edit_distance(a, b) <= max(1, shortest // 5):
                why = "near-identical spelling"
            elif b.startswith(a + "-") or a.startswith(b + "-"):
                why = "one name extends the other"
            elif ta[-1] == tb[-1] and ta[0] != tb[0] and len(sa | sb) <= 4:
                why = f"same head noun '{ta[-1]}', different modifier"
            elif sa != sb and len(sa & sb) >= 2 and \
                    len(sa & sb) / len(sa | sb) >= 0.5:
                why = "shared stems, one token differs"
            if why:
                pairs[(a, b)] = why
    return pairs


def confusable_families():
    """Connected components over the confusable pairs.

    Components are used rather than bare pairs because confusion is transitive
    in this corpus: `ike-sa-proposal`, `ipsec-sa-proposal` and `ipsec-sa-re-key`
    are one family an operator has to tell apart, not three separate pairs.
    """
    pairs = confusable_pairs()
    adj = collections.defaultdict(set)
    for a, b in pairs:
        adj[a].add(b)
        adj[b].add(a)
    seen, out, oversized = set(), {}, []
    for start in sorted(adj):
        if start in seen:
            continue
        comp, stack = set(), [start]
        while stack:
            n = stack.pop()
            if n in comp:
                continue
            comp.add(n)
            stack += [m for m in adj[n] if m not in comp]
        seen |= comp
        if MIN_MEMBERS <= len(comp) <= MAX_MEMBERS:
            out[f"confusable:{sorted(comp)[0]}"] = sorted(comp)
        elif len(comp) > MAX_MEMBERS:
            oversized.append(sorted(comp))
    return out, pairs, oversized


# --------------------------------------------------------------------- topics
def topic_families():
    tc = collections.defaultdict(set)
    for r in OPS:
        for t in r.get("topics") or []:
            tc[t].add(r["name"])
    return {f"topic:{k}": sorted(v) for k, v in tc.items() if len(v) >= 4}


# ---------------------------------------------------------------------- print
def show(title, fams, note=""):
    print(f"\n===== {title}: {len(fams)} clusters {note}")
    for k, v in sorted(fams.items(), key=lambda x: (-len(x[1]), x[0])):
        over = "  [OVER MAX, needs narrowing]" if len(v) > MAX_MEMBERS else ""
        print(f"  {len(v):3d}  {k}{over}")
        print(f"       {', '.join(v)}")


def main():
    which = sys.argv[1] if len(sys.argv) > 1 else "all"
    if which in ("all", "aid"):
        show("AID key-path ancestry", aid_families(),
             f"(generic roots dropped; {MIN_MEMBERS}+ members)")
    if which in ("all", "confusable"):
        fams, pairs, oversized = confusable_families()
        show("Confusable names", fams,
             f"(from {len(pairs)} candidate pairs, components of "
             f"{MIN_MEMBERS} to {MAX_MEMBERS})")
        # Never drop a component silently: an oversized one is a real family
        # that needs splitting by hand, not an absence of candidates.
        for comp in oversized:
            print(f"  {len(comp):3d}  [OVERSIZED, split by hand or ignore]")
            print(f"       {', '.join(comp)}")
        if which == "confusable":
            print("\n----- every candidate pair, with the signal that flagged it")
            for (a, b), why in sorted(pairs.items()):
                print(f"  {a:34s} {b:34s} {why}")
    if which in ("all", "topic"):
        show("Curated topics", topic_families(), "(4+ operation commands)")


if __name__ == "__main__":
    main()
