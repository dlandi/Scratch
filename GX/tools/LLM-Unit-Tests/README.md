# LLM unit tests for the GX CLI Reference

Natural-language questions about the 1830 GX CLI, each paired with an
approximate reference answer and machine-checkable expectations. They test
whether the generated index in `../../R9_1_GX_CLI_Reference/` actually gets an
agent to the right page, and whether an answer carries the facts the document
states.

```bash
python run_tests.py              # validate the tests, then score retrieval
python run_tests.py -v           # show every test, not just failures
python run_tests.py --prose      # list identifiers in answers to hand-check
python run_tests.py --render     # regenerate TESTS.md for human reading
python _clusters.py              # candidate clusters for multi-command tests
```

`_dump.py` prints the substance of a domain's sections, `_clusters.py` proposes
clusters, and `_authoring.py` builds a batch with the conventions enforced.

No third-party packages, no model required for the default run.

## Status

**Chapter 6 is complete: all 374 operation commands have at least one test.**
376 single-command tests across 14 batches, plus 65 multi-command tests: 5
pilots, 30 in batch 1 from the AID-ancestry and confusable-name bases, and 30 in
batch 2 from the topic clusters and four cross-layer chains. The multi tests
cite 187 distinct command files and reach all 16 domains.

| Measure | Value |
| --- | --- |
| Chapter 6 coverage | 374 / 374 commands |
| Single-command tests | 376 |
| Multi-command tests | 65 |
| Validate against the document | 441 / 441 |
| Route correctly from the index | 440 / 440 scored, 1 excluded as compound |
| Question does not name its command | 356 / 376 single, 55 / 65 multi |
| Distinct archetypes | 17 overall, 11 across the multi tests |
| Marked `weak` (thin source section) | 30 |
| Average reference answer | 468 characters single, 775 multi |
| Verbatim evidence quotes | 610 |
| Multi tests needing decomposition | 1 of 65 (2%) |

Batches are one file each in `tests/`, named `single-NN-<domains>.jsonl` or
`multi-NN-<basis>.jsonl`, so a batch stays separately reviewable and the runner
picks up every `.jsonl` in the directory.

## What a test looks like

```json
{
  "id": "ntp-server-dhcp-to-manual",
  "type": "single",
  "domain": "system-node-time",
  "archetype": "how-to",
  "names_command": false,
  "question": "Our time server was handed to the node by DHCP. Can I keep it but manage it ourselves?",
  "approximate_answer": "Yes. The origin attribute on ntp-server records how the address was assigned ...",
  "expect": {
    "files":  ["06-operation-commands/197-ntp-server.md"],
    "facts":  ["origin", "manual", "dhcp", "auth-key-id"],
    "route_terms": ["ntp", "server", "dhcp"]
  },
  "evidence": [
    {"file": "06-operation-commands/197-ntp-server.md",
     "quote": "A user can convert DHCP configured NTP entry into a manual configured by changing this attribute"}
  ],
  "weak": false
}
```

`evidence.quote` must appear **verbatim** in the cited file. This is the point
of the whole design: the answers are anchored in the split document, which is a
byte-exact slice of the source, not in the derived index. A test therefore
checks the index against the document rather than against itself.

## The three layers

**Layer 0, validate.** Every evidence quote is found verbatim in its file, every
expected file exists, every required fact appears somewhere in the expected
files, and ids are unique. Runs first; nothing downstream is trusted if it
fails. This is what stops a test from asserting something the guide never said.

**Layer 1, retrieval.** Given only the question text, can the index reach the
expected file? The runner reproduces what an agent does: match the question
against `topics.md` search terms, command names in `INDEX.md`, parameter names
in `parameters.md` and AID prefixes in `entities.md`. Deterministic, no model.
Single-command tests must reach their one file. Multi-command tests must reach
their `primary` file, the one the question centres on, **plus** half of the rest
of the cluster. No single query realistically surfaces every member, but
accepting any half is too weak a bar: it lets a test pass while the query misses
the very thing the question was about.

**Layer 2, facts.** Supply candidate answers as JSONL of `{"id":…, "answer":…}`
and pass `--answers`; each answer must contain every string in `expect.facts`.

Layer 3, judging prose against `approximate_answer`, is intentionally not
implemented. It needs a model at run time. The reference answers are written for
that purpose when you want it.

**What no layer checks.** Nothing validates the prose of `approximate_answer`.
Layer 0 covers the evidence quotes and the `facts` strings, so a fabricated
*quote* cannot pass, but a fabricated *sentence* can. Reviewing batch 1 found
exactly that: an answer asserted a parameter named `default-pm-supervision`,
which does not exist. The real ones are `default-data-supervision` and
`default-tca-supervision`, so the invention read perfectly next to them and
passed every layer. `--prose` exists to make that class visible: it lists
hyphenated identifiers used in an answer but absent from the files that answer
cites. It is an authoring aid, not a gate, because separating a corpus
identifier from an English compound needs judgement.

Read its output with the test type in mind. Multi-command tests cite a whole
cluster, so almost anything they mention should be in one of those files and the
signal is sharp: 3 flags across 35 tests. Single-command tests cite one file and
legitimately mention neighbouring commands, so they average 0.79 flags each and
most are cross-references rather than errors.

## What the batches found

Every batch exposed real gaps in the index rather than faults in the tests.
Across the 14 batches, **about 60 retrieval gaps** were found and fixed in
`../curated.py` by adding vocabulary an operator would actually type. A
representative sample:

- Operators say "limit" and "errored seconds", not "threshold", so the PM topic
  missed `pm-threshold`.
- "protected pair" did not match the search term `protection`; stemmed to
  `protect`.
- No route existed at all for "factory reset", "wipe", "laser", "OSNR",
  "span loss", "dispersion", "FEC", "fan", "temperature" or "serial number".
- British spelling: "fibre" did not reach anything, since every term used the
  American "fiber".
- "packet filter" and a bare "rule" did not reach `ace` or `access-rule`, and
  `authorization` was absent from the access-control topic entirely.
- `security-policies` was not in the passwords topic, so "minimum password
  length" missed the object that defines it.

Two bugs in the harness itself surfaced the same way. Command names were matched
case-sensitively against a lowercased question, so mixed-case names such as
`L2-bridge`, `ISK` and `KRK` could never match. And because Windows paths are
case-insensitive, a test citing `170-L2-bridge.md` when the file is
`170-l2-bridge.md` passed validation locally while never matching a record;
layer 0 now checks the path case against the corpus, so that cannot recur.

## Two honest limitations

**Recall only.** Layer 1 checks that the right file is reachable. It cannot
detect an over-broad search term that also drags in fifty irrelevant commands.
Adding a generic word like "node" to a topic would make tests pass while
quietly degrading precision, and nothing here would catch it. Add search terms
that an operator would actually type, and keep them specific.

**Compound questions.** `multi-node-identity-and-restart` asks three things at
once ("what is this node", "what is in it", "what will a reboot cost"). Single
shot lexical routing reaches only `restart`. It is marked
`requires_decomposition: true` and reported separately rather than counted as a
failure, because the fix belongs in the agent (split the question, then look
up), not in the index. Keep marking such cases rather than tuning terms around
them.

## Conventions

- **`names_command`**: false means the question avoids the command's name, which
  is the harder and more realistic retrieval case. Roughly 60% of tests should
  be false; a suite of questions that name their own answer proves little.
- **`archetype`**: what kind of question it is (`parameter-values`, `default`,
  `pre-condition`, `troubleshooting`, `which-command`, `enumeration`,
  `comparison`, `consequence`, `disambiguation`, `minimal-command`). Spread
  these; 374 variations of "what does X do" would be worthless.
- **`weak: true`**: the source section is too thin to support a substantial
  question (13 commands in Chapter 6 have two or fewer parameters and no
  examples). Marked rather than padded into looking substantial.
- **`primary`** on multi-command tests: the one file the question centres on.
  Required by layer 0, and layer 1 will not pass a test that misses it. Choose
  it before writing the question; if no single file is the centre, the question
  is probably compound and belongs under `requires_decomposition`.
- **`inference_flags`** on multi-command tests: anything the answer says that
  the document does not state. The guide contains **no procedures**, so any
  ordering ("first do A, then B") is inference and must be flagged.

## Adding tests

Add a batch as a new file, `tests/single-NN-<domains>.jsonl`; the runner loads
every `.jsonl` in `tests/`, so batches stay separately reviewable. Then run
`python run_tests.py`. Layer 0 will reject
a quote that is not verbatim or a fact absent from the cited files, so a
fabricated test cannot pass silently. Write the question first from the
operator's point of view, then find the answer in the file, not the reverse.

Multi-command clusters are not chosen by hand. `python _clusters.py` derives
candidates from three independent bases and prints them; `cluster_basis` records
which one a test came from.

| Basis | Clusters | What it groups |
| --- | --- | --- |
| `hierarchy:<root>` | 10 | Commands whose instance key path descends from another entity, so the cluster answers "what addresses a card, and what does a card hold" |
| `confusable:<name>` | 17 | Names close enough to reach for by mistake. Feeds `disambiguation` questions |
| `topic:<title>` | 53 | The curated topics in `index/topics.md`, grouped by subject rather than structure |

Clusters outside 3 to 8 members are printed but flagged: below that a question
does not need several sections, above it the cluster is a subject area rather
than a question. Oversized ones are never dropped silently, since an oversized
component is a real family needing a hand split, not an absence of candidates.

## Remaining work

Single-command coverage of Chapter 6 is complete. What is left:

The planned multi-command set is complete: 65 against a target of about 60,
across all five cluster bases. What is left, in the order agreed:

**1. Run layer 2 against a real model.** It has never been run once, which makes
the `facts` fields on all 441 tests an untested asset. Expect it to expose weak
facts immediately: 75 of the 1,259 facts in the single tests appear in more than
a quarter of all 395 files, so 63 of 376 single tests would be satisfied by an
answer that says almost nothing. The 65 multi tests are clean at 0, because
`_authoring.py` refuses such facts; the single tests predate that check.

**2. Measure precision, not just recall.** Layer 1 scores recall only, and about
70 vocabulary additions have been made to `curated.py` without one of them being
measured for collateral damage. A term that also drags in fifty irrelevant
commands makes tests pass while quietly degrading routing. Until something
measures this, every future `curated.py` edit is unfalsifiable. The sketch is to
sample queries that should *not* reach a topic and count false hits; the design
is open.

**3. Chapters 3, 4 and 5.** The 21 auxiliary, navigation and piped commands have
no tests. They are indexed and routable, just not covered here. Note that
`_authoring.py` builds multi-command tests only, so a single-command batch needs
a different path or an extension to it.

Lower value: second single-command tests for `show`, `set`, `download`, `status`
and `activate`, each of which carries far more behaviour than one question
exercises. Layer 3 remains unimplemented by choice.

**What this suite is and is not.** No LLM has ever been run against it. Layer 1
is a deterministic lexical simulation of what an agent would do. What the tests
have bought so far is a gap-finding instrument for the index, and that paid:
around 70 real vocabulary gaps found and fixed. Their demonstrated value is
diagnostic, not evaluative, so do not read a green run as proof that the
documentation answers questions well.

## What batch 1 found

Three retrieval gaps, all fixed in `../curated.py`:

- `nct-connection` sat only in the multi-chassis topic, so a question about
  links in the topology could not reach it despite it being in the
  `topology-discovery` domain. Fixed by topic membership, not by a new term.
- `connectivity` reached nothing: every term used `connection`, which is not a
  substring of it.
- `supports` reached nothing. The inventory topic had `supported`, so "what is
  supported" routed and "what a card supports" did not. Only the missing
  inflection was added, not the stem `support`, which would also fire on
  `supporting-facility` questions and drag 14 capability commands into optical
  and encryption queries.

Five multi tests still reach only part of their cluster, and that is correct
rather than a gap. A cluster is a structural fact about the corpus; a question
is one operator's phrasing of one intent. `multi-access-rule-object-disambiguation`
cannot reach `sw-control-rule` because the ACL vocabulary is all packet-shaped
and `sw-control-rule` is about service failure. Putting it in the ACL topic to
make the number look better would corrupt ACL routing to no benefit.

**Batch 1 is skewed toward one archetype.** 15 of its 30 tests are
`disambiguation`, because a cluster of confusable names almost forces that
question shape: if the members are alike, the useful question is which is which.
Batch 2 was planned archetype-first to correct it, choosing the target spread
before choosing clusters, and landed at 8 `troubleshooting`, 5 `pre-condition`,
4 `comparison`, 3 each of `consequence` and `enumeration`, 2 each of
`parameter-values`, `how-to` and `disambiguation`, and 1 `default`.

## What batch 2 found

One retrieval gap, in the telemetry topic. It had `subscription` but not the
verb form, so "what is currently subscribed" reached nothing, and it had no term
for `streaming` at all, which is what operators call this feature. Fixed by
replacing `subscription` with the stem `subscri` and adding `stream`.

Two other failures were the question's fault rather than the index's, and are
worth recording because the distinction matters. A question about "the reset
options" missed because the topic carries `factory reset`, and one about an
amplifier that would not turn on missed because it never said "power" although
its own answer was entirely about power. Both were reworded to what an operator
would actually type. Adding bare `reset` would have been the wrong fix: it would
pull `clear` and `database` into every PM-counter and password question, and the
harness measures recall only, so nothing here would have caught the damage.

**Reviewing a batch written with `_authoring.py` needs different checks.** The
script enforces the conventions at write time, so re-running them proves
nothing; a review has to go where the tooling cannot. Two checks earned their
keep on batch 2, neither of them cheap to automate permanently:

- **Attribution.** The prose check concatenates every cited file, so an
  attribute credited to the wrong object in the cluster passes. Checking it
  needs the answer segmented by which command each sentence is about. Beware two
  traps: `\botdr\b` matches inside `otdr-ptp`, so match whole hyphenated tokens
  only; and command names that are ordinary nouns, `database` and `password`,
  produce false segment boundaries. Batch 2 had 0 real errors and 7 such
  artifacts.
- **Every stated default against the parameter record.** 51 of batch 2's
  default claims verified; the 15 flagged were nearly all the match window
  spilling into the next clause. The one real defect it found is the kind
  nothing else would catch: an answer that said `restart` with no resource-id
  "acts on the default target", where the record says it restarts the active
  controller card. Not false, but it dropped the actual consequence from a
  `consequence` test.

`_authoring.py` encodes this README's conventions as assertions. It derives
`cluster` and `names_command` from the corpus rather than accepting typed
values, refuses any fact appearing in more than a quarter of the files, and
writes nothing unless every record in the batch passes. It rejected two records
while batch 2 was being written: a fact of `RIB`, which matches as a substring
of "describe" and "attribute" and so appears in 217 of 395 files, and an
invented identifier in an answer.
