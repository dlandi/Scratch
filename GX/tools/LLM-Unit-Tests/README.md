# LLM unit tests for the GX CLI Reference

Natural-language questions about the 1830 GX CLI, each paired with an
approximate reference answer and machine-checkable expectations. They test
whether the generated index in `../../R9_1_GX_CLI_Reference/` actually gets an
agent to the right page, and whether an answer carries the facts the document
states.

For where this sits in the whole system, see
[`../../ARCHITECTURE.md`](../../ARCHITECTURE.md).

```bash
python run_tests.py              # validate the tests, then score retrieval
python run_tests.py -v           # show every test, not just failures
python run_tests.py --prose      # list identifiers in answers to hand-check
python run_tests.py --render     # regenerate TESTS.md for human reading
python precision.py              # what the index drags in, and which term did it
python compare_runs.py           # score every stored model run, and compare them
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

**Chapters 3 to 5 are covered too**, by batch 15: 16 tests over 20 of the 21
auxiliary, navigation and piped commands. See "Chapters 3 to 5" below for why
it is 16 tests and not 21, and why `?` is deliberately not among them.

| Measure | Value |
| --- | --- |
| Chapter 6 coverage | 374 / 374 commands |
| Chapters 3 to 5 coverage | 20 / 21 commands, `?` excluded by design |
| Single-command tests | 392 |
| Multi-command tests | 65 |
| Validate against the document | 457 / 457 |
| Route correctly from the index | 456 / 456 scored, 1 excluded as compound |
| Carry the required facts, Claude Opus 5, eight runs | 435/441, 434/441, 454/457, 451/457, 456/457, 454/457, 450/457, 447/457. Spread 97.8 to 99.8%; nothing fails above 50%. `compare_runs.py` reports the rate in run order |
| Question does not name its command | 372 / 392 single, 55 / 65 multi |
| Distinct archetypes | 17 overall, 11 across the multi tests |
| Marked `weak` (thin source section) | 31 |
| Average reference answer | 467 characters single, 776 multi |
| Verbatim evidence quotes | 634 |
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

It also checks that the facts are worth checking, which is a different question
from whether they are true:

- **The fact set must discriminate.** At most 4 of the 395 command files may
  contain every fact in a test. A set that half the corpus satisfies is passed
  by an answer that says almost nothing.
- **One fact must be specific**, appearing in fewer than 5% of files. Otherwise
  the test measures general fluency about the platform rather than whether the
  answer found the right command.
- **The reference answer must pass its own test.** It is the one answer known
  to be correct, so if it does not contain its own facts, no candidate answer
  will either.

Measure the set rather than each fact. Per-fact frequency, which is what
`_authoring.py` enforces at write time, is a blunt instrument used alone: it
condemns `false` and `30` when they are the entire answer to a default
question, and it acquits `equipment`, a single fact that 88 files satisfy.

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

Matching is by word boundary, not raw substring, because a fact has to mean
what it says: an operator is going to type it. A plain substring test is wrong
in both directions. It fires when it should not, since `oc` occurs inside
"block", `ILA` inside "available", `rib` inside "describe", `add` inside
"address" and `na` inside "name"; a trailing plural `s` is the one allowance,
so `reboot` still matches "reboots". And it fails when it should not, since the
guide writes `software-load` and `next-hop` where an answer writes "software
load" and "next hop", so a fact spanning more than one token also matches with
its separators flattened. Runs are stored under `runs/run-NN/`, so the tests can
be re-scored against them after an edit without paying for the model again;
`compare_runs.py` scores every stored run against the working tree and reports
each test's failure rate as a sequence in run order, because `X X . . X` and
`X X . X .` are the same rate and not the same finding. It also reports which
index each run read, so a rate assembled from two different corpora is visible
as one. See `runs/README.md`.

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

## Precision

```bash
python precision.py            # headline numbers and the worst terms
python precision.py --terms    # every search term, ranked by the noise it adds
python precision.py --dead     # terms no test question fires
```

Layer 1 scores recall. `precision.py` scores what the index drags in alongside
the answer, which is what an over-broad term in `curated.py` costs. It needs no
new data: each of the 441 questions is a real query with a known answer, so
`|expected ∩ routed| / |routed|` is a precision figure, and each routed file can
be attributed to the term, command name, parameter or AID that produced it.

**It makes a `curated.py` edit falsifiable.** A term is `necessary` when some
test has an expected file it alone reaches, so deleting it would cost recall.
A term that is never necessary and adds noise is a candidate for review. Read
that honestly: `necessary` means "needed by a question someone wrote", not
"needed". Terms exist to catch phrasings the suite does not contain, so `fibre`
is unnecessary here and still earns its keep against real operators.

**The first run found the measurement mattered more than any single term.**
Topic terms were matched as bare substrings, so `ca`, added for Certificate
Authority, fired inside "card", "scan" and "because" on 197 of the 441
questions and dragged the whole PKI topic in each time; `est` fired inside
"test", `ace` inside "interface", `led` inside "enabled". That is not only a
precision problem. **13 tests were scored as reaching the right file through a
term that had matched a fragment of an unrelated word**, so recall was partly
counterfeit.

Topic terms now match at the start of a word, which keeps the deliberate
stemming working (`protect` still catches "protection"), and terms under four
characters must match the whole word, since prefix matching does not save `ca`
from "card". Ten of the thirteen tests kept their recall honestly. The other
three were real vocabulary gaps hiding behind the accident: questions about
revocation lists and signing requests reached the PKI topic only through `ca`
firing on some unrelated word, so `revocation`, `signing request` and
`distribution point` were added. Each of those already sat in the tests'
`route_terms` field, which nothing had ever read.

| | Before | After |
| --- | --- | --- |
| Files returned per question, median | 30 | 19 |
| Questions returning more than 25 files | 60% | 30% |
| Mean precision | 0.065 | 0.099 |
| Recall | 440 / 440, 13 of them accidental | 440 / 440 |

Two terms were then removed on the tool's evidence: `what can`, a question form
rather than vocabulary, which fired on 12 questions and pulled in 14 commands
each time, and `stop`, which fired on "stop the node collecting performance
data" and pulled in the 29 CLI navigation commands. Neither cost any recall.

Ranked next are ordinary domain words: `card`, `channel`, `default`, `carrier`.
They fire because questions genuinely say them, and they are the price of topic
routing rather than a defect. Do not chase the ranking to zero.

### The acronym list, and a prototype that was measured and rejected

`99-acronyms/99-acronyms.md` is a slice of the source like any other, 797
entries, and nothing routes to it. Feeding those expansions into `curated.py`
as search terms was built, measured and **rejected**. Recorded here because the
negative result cost more to obtain than the change would have.

The rule was narrow: attach an expansion only to a topic that already carries
its acronym as a search term, so the expansion routes exactly where the acronym
does, nothing new becomes reachable and recall cannot fall. 850 definition
pairs reduced to 50 terms. Rebuilt, **every precision figure was identical to
baseline** and `run_tests.py` was unchanged at 457/457 and 456/456.

**It was rejected despite costing nothing, because it also bought nothing.**
The consumer of `topics.md` is a model, and an acronym earns a curated search
term precisely because someone recognised it as vocabulary operators type,
which makes it one of the well-known ones. So the 50 were `border gateway
protocol`, `secure shell`, `simple network management protocol`: expansions any
model already knows. The filter that rejected 770 of 850 was "no anchored
search term", and that bucket is where the acronyms needing help actually live.
`OPSM`, `GFP`, `OLS` and `AID` still route to zero files. Only three of the 49
were vendor-specific, and those were added by hand: `intelligent carrier
discovery protocol`, `inter-ne communication infrastructure`, `nodal control
and timing`. `index/README.md` now carries a row pointing at the acronym list
for the rest, which is discovery rather than routing.

Two findings are worth more than the change:

- **Anchor on a search term, never on a command name.** The first version
  accepted both. A command name is an object identifier, and its spelled-out
  form is usually the ordinary phrase the guide already uses everywhere, so it
  behaves like `card` and `channel`: `media channel`, from the `mc` command,
  added **124 irrelevant files across 6 questions**, and `NE` produced `network
  engineer`. Three of the seven command-anchored terms were the worst in the
  batch.
- **An ambiguous acronym drags unrelated vocabularies into one topic.** `PM` is
  performance monitoring here, and also phase modulation and preventive
  maintenance; `SPD` is a security policy database and a surge protective
  device. Requiring the expansion to share a content word with the topic caught
  these, at the cost of also dropping `ASE = Amplified Spontaneous Emission`.

**What could and could not be measured.** `precision.py` prices the cost, not
the benefit, because every question in the suite uses acronyms rather than
expansions, so the added terms are invisible to it by construction. The benefit
figures, 46 of 49 expansions reaching a superset of their acronym and 10
reaching nothing beforehand, come from queries written for the purpose. That is
the same blind spot as the rest of the suite, and it is why a result showing
zero cost is not on its own a reason to ship.

## One honest limitation

**Compound questions.** `multi-node-identity-and-restart` asks three things at
once ("what is this node", "what is in it", "what will a reboot cost"). Single
shot lexical routing reaches only `restart`. It is marked
`requires_decomposition: true` and reported separately rather than counted as a
failure, because the fix belongs in the agent (split the question, then look
up), not in the index. Keep marking such cases rather than tuning terms around
them.

## Conventions

- **The id is hyphenated and none of its words are decoration.** They all
  matter. `equipment-show-filter` promises three things: the object, the verb,
  and the option that narrows the listing to one card. A fact set covering the
  first two lets an answer pass having answered two thirds of its own question,
  which is what happened until `card-1-1` was added. Read the id as the
  test's contract and check the facts against every word of it, including the
  words that look like grammar: the `not` in `default-not-everything-resets` is
  the entire answer, the `vs` in `file-operation-vs-file` is what makes it a
  contrast, and the `requires` in `set-time-requires-manual-source` names the
  precondition the answer has to state.

  Some id words name a thing the facts must contain, others name the shape of
  the question and constrain what the facts have to do. Only the first kind is
  checkable as a string, so layer 0 checks the strongest instance of it: **the
  command name is a required fact on every test, and the reference answer must
  contain it.** 27 answers did not name their command at all, describing its
  attributes without ever saying which command carried them. The rest of the id
  is a reviewer's job.

  This one is not a scoring nicety. The point of the corpus is that an operator
  can ask a question in their own words and act on the answer, and an answer
  that explains `data-supervision` without ever saying `pm-control` leaves them
  with nothing to type. Naming the command is the minimum viable answer, so it
  is required of all 441 rather than of the ones that happened to have it.
- **`command`**: which command the test is about. Required only when the cited
  file holds more than one, which is true of chapters 3 to 5 and of nothing in
  chapter 6. Layer 0 checks it against the file, so it cannot name a command
  that is not there. Without it the check guessed the file's first command.
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

## What the first layer 2 run found

Run on 2026-08-14 against Claude Opus 5. Each of the 441 questions went to an
agent with the corpus and nothing else: no test file, no expected facts, no
reference answer. Answers averaged 809 characters. Stored in
`runs/2026-08-14-answers.jsonl`.

| | Score |
| --- | --- |
| Single-command, as first scored | 343 / 376 (91%) |
| Single-command, after the off-question facts were removed | 374 / 376 (99%) |
| Multi-command, as first scored | 28 / 65 (43%) |
| Multi-command, after the off-question facts were removed | 53 / 65 (82%) |
| Overall, after that repair | 427 / 441 (97%) |

The command name was then made a required fact on all 441 tests and the matcher
tightened to word boundaries. **Re-scoring the same answers gives exactly the
same 371, with an identical failing set:** not one test changed verdict. The
agent had already named the actionable command in all 248 cases where the test
had not been asking for it, so the 84% was not resting on the omission.

**The multi-command number was measuring the test, not the answer**, and the
same audit fixed it. It first looked like a scoring problem: 31 of the 46
missed facts lived only in a peripheral cluster member rather than in the
`primary` file, so the obvious move was to give layer 2 the partial credit
layer 1 already gives. That would have been the wrong fix. Auditing all 65
questions against their facts found the real cause was the same one as in the
single tests, arriving by a different route: **facts taken from cluster members
the question never touches.** `multi-ssh-which-object-holds-which-key` asks
which object holds which key and required `8022`, the SSH port.
`multi-card-running-hot` asks which dashboard shows temperature and required
`expected-fan-type`. `multi-aaa-radius-not-reached` asks what decides where
authentication is sent and required `static+rules`, an authorization mode.

30 off-question facts were removed across 30 multi tests, four of them tests
that were passing, and 6 more facts were re-encoded from a prose phrase to the
identifier an operator would actually type: `location-led` rather than "location
led test", `sc-rx` rather than "Secure Channel", `show config` rather than
"display commands". **Multi went 28/65 to 53/65 with the pass rule untouched**,
so the primary-versus-cluster allowance is not needed and layer 2 keeps
demanding every fact.

### Reading the 33 single-command failures

They were read one by one against their question, and they were not what they
looked like. **24 of the 33 failed on a fact the question never asked for.**
`security-policies-password-rules` asks the minimum password length and
required `secure-mode`, which governs whether non-secure protocols are allowed.
`user-lockout-thresholds` asks how many failed logins lock an account and
required `max-sessions`, which is concurrent sessions. `stm-types-supported`
asks which SDH rates the platform terminates and required `tti-64`, a trace
field width.

The cause is one systematic authoring habit, not 33 mistakes. Every reference
answer ends in a tail of volunteered detail: "Other settings include...", "The
same object also holds...". Facts were harvested from the whole answer, tail
included, so tests demanded material no operator asked about.

**The fix had to be found without looking at the failures**, or it would have
tuned the suite to one model run. Detecting facts that appear only in an
afterthought sentence of the reference answer finds 35 tests, and 22 of them
were passing: `ssh-port-and-enabled` asks what port SSH listens on and required
`pre-login-message`; `cli-port-and-alarm-columns` asks about alarm columns and
required `script-dir`. 60 off-question facts were removed across 50 tests,
judged from each question rather than from what any answer happened to say.

What survived is the real result. **Single-command went from 343/376 to
374/376**, and the two remaining failures are genuine: `comm-eth` was asked what
speed the ports negotiate and never mentioned `operational-rate`, and
`config-as-restore-script` said "skipping any configuration left at its default
value" rather than the guide's `non-default`. One apparent failure was a
measurement bug, now fixed: the answer gave the LOS threshold as "-23 dBm" and
the guide writes `-23dBm`.

So on single-command questions the corpus does its job. The suite had been
measuring the reference answers' habit of volunteering extra detail, and once
that is removed almost nothing is left to blame the documentation for. The
tests did not lose their teeth in the process: a whole fact set is still
satisfied by at most 4 of the 395 files, no test is passed by an empty or
generic answer, and only 7 are passed by naming the command alone, all of them
sections too thin to support more.

**Do not close this gap by relaxing the facts that this run failed.** That tunes
the suite to one model's output and destroys the instrument. The defensible fix
is the one layer 1 already made: decide what a multi test requires from the
question rather than from everything the reference answer happens to mention.

## What the second run found

Run on 2026-08-15, same model, same prompt, same sharding, with only the shard
and output paths changed. Stored in `runs/run-02/`. **422/441**, against run
01's 427/441 rescored at the same commit.

**The score went down, and the prediction that it would rise was wrong for a
useful reason.** Run 02 read the post-precision-fix `topics.md`, so it was
expected to gain. It did not, because layer 1 was already at 440/440: there was
no retrieval headroom for better routing to convert into facts. Run-to-run
variance is simply larger than that fix's effect on answers.

| | Value |
| --- | --- |
| Spread over two runs | 95.7% to 96.8% |
| Failed by every run | 10 |
| Failed by some runs | 13 |

**The 13 unstable tests are the error bar.** Any change worth less than about
1.1 points cannot be distinguished from noise by this instrument, which is worth
knowing before anyone tunes against a single number again.

**The second run changed a decision the first run had already made.**
`multi-cableid-confirm-patching` was queued as the first documentation gap to
fix, on the strength of run 01 failing it and its answer never naming `verify`.
It passes in run 02, so it is noise. `multi-node-identity-and-restart`,
`multi-cli-and-config-scope` and `multi-resource-type-defaults` are the same.
Fixing any of them would have tuned the suite to one run, which is the exact
failure the second run exists to prevent.

What survives is the real list. These 10 fail in both runs and are the corpus or
the test rather than luck:

| Test | Fact never stated |
| --- | --- |
| `multi-ipsec-policy-nesting` | `proposal` |
| `multi-fiber-connection-who-writes-it` | `NCT` |
| `multi-route-sources` | `dynamic` |
| `multi-controller-card-vs-card` | `capability` |
| `multi-otdr-locate-fiber-damage` | `automatic-otdr` |
| `multi-protection-which-member-is-live` | `y-cable` |
| `multi-restart-card-consequences` | `auto-in-service`, `controller card` |
| `multi-l1-encryption-prerequisites` | `X509v3`, `digital identity` |
| `comm-eth-lldp-and-negotiation` | `operational-rate` |
| `config-as-restore-script` | `non-default` |

Eight of the ten are multi-command tests, so that is where the remaining signal
lives. The two single-command entries are exactly the two the run-01 audit
called genuine, now confirmed against an independent run rather than resting on
one. Before treating any of the eight as a documentation gap, apply the rule the
run-01 audit established: decide what the test requires from **its question**,
not from what the reference answer happens to mention.

Run 02's agents also found nine more source-document defects, verified and
recorded in `../README.md`, plus one candidate that was checked and rejected.
Three of them turned out to be classes with members already on that list.

## Chapters 3 to 5

Batch 15, `tests/single-15-cli-and-session.jsonl`. 16 tests over the auxiliary,
navigation and piped commands. Three things about these chapters are worth
knowing before adding to them.

**They are one file per chapter, not one per command.** Chapter 6 gives layer 1
a sharp question, "did the query reach this command out of 374". Here the same
question is only "did it reach the piped-commands file", which any of the ten
piped commands satisfies. Retrieval tests over these chapters therefore measure
much less than they do over Chapter 6, and the value is almost entirely in
layer 2. Splitting the chapters per command on their H2 boundaries would fix
that and was considered; it was not done, because it changes the byte-exact
split and the invariants in `gxpaths.py` for a gain confined to 21 commands.

**Two harness bugs had to be fixed first, and neither was a relaxation.**

- `CORPUS` was keyed by command name while holding whole file bodies, so a
  chapter file was counted once per command in it. A fact unique to the piped
  file scored 10 against a `MAX_MATCHING` of 4, and **no test for chapters 4 or
  5 could pass layer 0 however well written**. The gate always meant files,
  which is what `corpus_hits` documents. Keying by file gives 377 entries and
  changed no verdict for the 441 tests that predate it.
- The command-name check read the file's *first* command, so a test about
  `sort` would have been required to name `begin`. A test in a multi-command
  file now declares `command`, and layer 0 checks the declaration against the
  file so it cannot name something that is not there.

**`?` has no test, deliberately.** The command name is a required fact and is
matched on word boundaries; `?` has none. The rule is right (an answer that
never names the command leaves the operator with nothing to type) and `?` is a
genuine exception rather than a reason to weaken it. `toc`, `top`, `include` and
`until` also have no test of their own, but each is covered inside the test for
the command it pairs with, which is how the guide itself presents them.

**One fact-level trap.** The discrimination gate matches raw substrings, so
`tic` occurs inside "static" and "diagnostic" in 187 files and `toc` in 72. A
`tic`/`toc` test needs a third, rarer fact to anchor it. This is the same
accidental-substring class already fixed in the layer 2 matcher and in layer 1
topic terms, still live in `corpus_hits`; fixing it there needs the
separator-flattening logic `carries()` has, and a naive word-boundary version
drops 50 existing tests' facts to zero. Left alone on purpose.

### What batch 15 found

Twelve of the sixteen failed retrieval on the first run, and the CLI topic in
`../curated.py` gained the vocabulary an operator would actually use for these:
`elapsed`, `keyword`, `shortcut`, `short name`, `hierarchy`, `depth`,
`previous command`, `reorder`, `mark up`, `line numbers`, `cli script` and
`starting from`. Median files per question, mean precision and the over-25
share are all unchanged, so the twelve cost nothing measurable.

Two failures were the question's fault rather than the index's, the same call
batch 2 made: "how long a command took" and "re-run something I typed earlier"
were reworded to "elapsed time" and "repeat a previous command", which is what
an operator types. A third, the `display commands` question, was reworded
rather than given a bare `script` term, which would have dragged all 29 CLI
commands into every scheduled-task question exactly as `stop` once did.

**`precision.py`'s `necessary` column cannot see a jointly necessary pair.** It
marked both `starting from` and `ending at` unnecessary, because necessity is
computed per term with the others held fixed and either alone was redundant.
Removing both broke `begin-until-retrieve-subset`; removing either would not
have. Re-check a redundant pair together before deleting on that column.
`timer` was tried and removed on the tool's evidence: it fired on retry-timer
and hold-off questions for 44 irrelevant files and no recall.

## What the third run found

Run on 2026-08-15, same model, same prompt, 19 shards because batch 15 took the
suite to 457 questions. Stored in `runs/run-03/`. **442/457.**

**Two runs were not enough, and this is the finding to keep.** The set failing
in every run drops from 10 to **7**. `multi-controller-card-vs-card`,
`multi-ipsec-policy-nesting` and `multi-route-sources` all pass in run 03: two
runs had agreed on them by chance and they were being reported as "the corpus
or the test, not luck". Anyone who had started triaging that list would have
spent a third of the effort on noise. The unstable count also went *up*, 13 to
22, so a second run narrowed the error bar less than it appeared to.

| | Value |
| --- | --- |
| Spread over three runs | 95.7% to 96.8% |
| Failed by every run | 7 |
| Failed by some runs | 22 |

The seven that survive all three:

| Test | Fact never stated |
| --- | --- |
| `multi-fiber-connection-who-writes-it` | `NCT` |
| `multi-otdr-locate-fiber-damage` | `automatic-otdr` |
| `multi-protection-which-member-is-live` | `y-cable` |
| `multi-restart-card-consequences` | `auto-in-service` |
| `multi-l1-encryption-prerequisites` | `X509v3`, `digital identity` |
| `comm-eth-lldp-and-negotiation` | `operational-rate` |
| `config-as-restore-script` | `non-default` |

### Batch 15 repeated a known authoring defect

Run 03 gave chapters 3 to 5 their first layer 2 exposure and 5 of the 16 failed,
every one on a fact that was **guide prose rather than what an operator types**:
`parent level`, `repeat the previous command`, `invert the order`,
`within quotes`, `add line numbers`. The multi tests hit this once already, when
six facts had to be re-encoded to identifiers (`location-led`, not "location led
test"). Batch 15 hit it again.

The repair followed the rule the run-01 audit established: **audit every test of
the class, not the ones that failed.** 14 of the 16 carried prose-phrase facts,
so the 9 that passed did so on phrasing luck. 12 were re-encoded, **7 of them
passing at the time**, each judged from its own question and id:

- to the flag or token an operator types: `invert the order` to `-i`,
  `repeat the previous command` to `!!`, `lines of context` to `-n`,
  `add line numbers` to `line numbers`, `within quotes` to `quotes`.
- dropped as volunteered: `wild card` (partial-keyword search, which
  `help-search-by-keyword` never promises), `CLI session window` (page sizing on
  a test about pipe ordering), `csv` / `only-values` / `keys-table` (other modes
  on a test about `display commands`), `numbered list`, `case insensitive` on
  the grep-vs-include contrast.

Runs 01 and 02 rescore unchanged at 427/441 and 422/441, and the persistent set
stayed at 7, which together confirm the repair reached only batch 15. Teeth
were re-checked after it: worst fact set matches 3 of 377 files, mean 1.12, and
no test passes on an empty answer or on the command name alone.

**One judgement call worth flagging.** `edit-vs-up-vs-top-navigation` needed an
anchor fact under the 5% threshold, and the first attempt, `hierarchy root`,
was itself guide prose picked because it was rare, which is the defect being
fixed. It was replaced with `absolute`, which is on-question because what `edit`
does, as against `up` and `top`, is take absolute or relative ids. That choice
also happens to make the test pass, so it is recorded here rather than left
silent.

## Triaging the 7, and why the resulting 0 is not a victory

The seven that failed all three runs were read against their own questions. All
seven turned out to be the defect both earlier audits found, arriving once more:
**a fact the question never asked for.** After repair, scores are 434/441,
428/441 and 448/457, and the set failing in every run is **0**.

**Do not quote that 0 as evidence the corpus answers everything.** The persistent
set was *defined* by the three runs, and this pass edited exactly that set. An
instrument adjusted after seeing which cases it flagged cannot then be cited as
clearing them. What the 0 honestly means is narrower: no test in the suite is
now failed by all three stored runs. The real check is a **fourth run against
the repaired tests**; if it produces new persistent failures, that is the
signal. Two of the seven do still fail in some run
(`multi-l1-encryption-prerequisites` in run 03,
`multi-restart-card-consequences` in run 02), so the repair did not simply
delete the evidence.

This pass is also weaker methodologically than the batch 15 audit, and the
difference is worth naming. There, the whole class was audited and 7 passing
tests were changed alongside the failures. Here only the failing seven were
edited. The mitigation is that the broad class, off-question facts in multi
tests, was already swept twice, and that one class check was run over
non-failing tests: three tests use a value of the `avail-state` enumeration as a
fact, and the two outside the seven were examined and left alone as legitimate.

| Test | Fact removed | Why the question does not ask for it |
| --- | --- | --- |
| `multi-fiber-connection-who-writes-it` | `NCT` | The question is about *fiber* links appearing unconfigured. `nct-connection` is NCT port connectivity in a multi-chassis NE, a different object class. Cluster-member fact. |
| `multi-otdr-locate-fiber-damage` | `automatic-otdr` | The question has three parts: what runs the scan, where to tune it, where the answer comes out. `automatic-otdr` is the auto-scan enable, a fourth thing. |
| `multi-protection-which-member-is-live` | `y-cable` | Asks which member is live and what settings govern switch *behaviour*. `y-cable` is a value of `protection-type`, i.e. which scheme, not how a switch behaves. |
| `multi-restart-card-consequences` | `auto-in-service` | One value in the `avail-state` enumeration carried by 49 files, lifted from the `recovery` cluster member. |
| `comm-eth-lldp-and-negotiation` | `operational-rate` | Straight from the reference answer's volunteered tail. The question asks what speed the ports negotiate, which `auto-negotiation` and `rate` answer. |
| `config-as-restore-script` | `non-default` | Prose inside the `show config` description, not a parameter and not something an operator types. |
| `multi-l1-encryption-prerequisites` | `X509v3`, `digital identity` → `local-certificate` | Both came from one descriptive sentence. What must "be in place", and what an operator acts on, is the certificate object. |

**A trap worth keeping.** `auto-in-service` is both a generic `avail-state` value
in 49 files *and* the default of `restore-from-chassis-storage` in `recovery`,
where `recovery-from-chassis-storage` asks exactly that and the fact is right.
The same token being generic in one command and precise in another is why the
frequency number alone can never decide a fact; the question decides it.

Teeth after the pass: worst fact set matches 4 of 377 files, mean 1.08, no test
passed by an empty answer.

## What the fourth run found

Run on 2026-08-15 against the repaired tests, same model, same prompt.
**444/457.** Stored in `runs/run-04/`.

**Its purpose was falsification, and the triage survived it.** Run 04's answers
were produced after the triage and had no part in writing it, so it is the
independent check the previous section said was needed. Of the seven triaged
tests only `multi-l1-encryption-prerequisites` fails, and on `secure-application`,
a fact the triage never touched. All 16 batch 15 tests pass, so the
prose-to-identifier re-encoding holds against answers it was not fitted to.

**And it showed the binary metric is misleading.** "Failed by every run" gets
mechanically harder to satisfy with each run added, so 0 is a weaker statement
at four runs than at three. The distribution is the honest picture:

| Failed in | Tests |
| --- | --- |
| 3 of 4 runs | 2: `sub-component-view`, `multi-ipsec-policy-nesting` |
| 2 of 4 runs | 9 |
| 1 of 4 runs | 18 |

`multi-ipsec-policy-nesting` is the case that makes the point. It failed runs 01
and 02, passed run 03, failed run 04. Run 03 removed it from the persistent list
as noise; at 3 of 4 it is now the joint most consistent failure in the suite.
**A test is not persistent-or-noise, it has a failure rate.** `compare_runs.py`
reported only the binary at the time; it reports the rate now, and shows it in
run order, which is what later exposed the second half of this same test's
story below.

Read `sub-component-view` and `multi-ipsec-policy-nesting` as the current best
candidates for a real gap, and triage them from their questions as before.

## The first real gap the suite has found

Triaging the two tests at 3 of 4 gave two different answers, and the second is
the first failure in this project that is neither a test defect nor noise.

**`sub-component-view` was a test defect**, the same prose class again. Its
fact `card resources` came from the description sentence "show the
sub-component details or card resources"; that is prose, not a command form,
and the question asks how to list the sub-components **of a card**, so
`card-name` is the on-question key. Re-encoded to `sub-component`, `card-name`.
This makes a 3-of-4 failure pass, which is flagged rather than buried.

**`multi-ipsec-policy-nesting` is real, and it is an index gap.** The question
asks which object decides a flow is protected and **what hangs underneath that
decision**. Answers consistently name `ipsec-spd-entry`, `protect`, `priority`
and `ipsec-traffic-selector`, and consistently miss `ipsec-sa-proposal`. The
reason is structural: **`141-ipsec-spd-entry.md` never mentions "proposal" at
all.** The nesting exists only in the child's own key path,
`ipsec-sa-proposal-<...>/<ipsec-spd-entry-name>/<number>`, so it is discoverable
from the child upward and invisible from the parent downward.

That is not something to fix by editing the test or the source. It is a missing
index view:

- `entities.md` maps an AID prefix to its command and explicitly defers
  containment, pointing at the source's MO relationship section and the `tree`
  output. There is no "what lives under X" lookup.
- The relation is nonetheless derivable from the data already in
  `commands.jsonl`: a child's full AID pattern contains its parent's key name.
  Finding it today means scanning all 294 entity patterns for your key.
- `_clusters.py` **already derives exactly this**, as its `hierarchy:<root>`
  basis over AID key-path ancestry. The derivation exists and is simply not
  emitted into the index.

**Fixed in the index**, not in the test or the source. `entities.md` now carries
a Containment section, 87 parent-to-child links across 50 parent entities,
derived from the AID key paths, and `index/README.md` routes "what lives under
X" to it. `ipsec-spd-entry` now lists `ipsec-sa-proposal` and
`ipsec-traffic-selector`.

One derivation subtlety, since it is easy to get wrong: **the immediate parent
is the second-to-last placeholder, not the first.** `_clusters.py` roots a key
path at its first placeholder, which is right for grouping a family and wrong
here; it makes `ipsec-sa-proposal` a child of `ikev2-local-instance` and leaves
`ipsec-spd-entry` with no children at all, preserving the very gap being closed.
Reusing that function unchanged would have looked correct and fixed nothing.

**Run 05 tested that prediction, and it held.** `multi-ipsec-policy-nesting`
passes, 3 of 4 becoming 3 of 5. The corroboration is worth more than the score:
the shard 01 agent reported unprompted that "the containment table in
`index\entities.md` was the key to the hierarchy questions", naming
`ipsec-spd-entry -> ipsec-sa-proposal/ipsec-traffic-selector`, without knowing
the section was new or why it existed. That is the mechanism observed rather
than inferred.

**Read it with the caveat.** One pass is one bit of evidence, and the rate
falling from 75% to 60% is arithmetic rather than independent confirmation. A
sixth run is what would settle it. After run 05 nothing in the suite fails above
60%, and five tests share that mark, four of which are the next triage
candidates: `chain-certificate-behind-encrypted-app`, `csr-gen-pending-import`,
`local-certificate-revocation-mode`, `multi-controller-card-vs-card`. Three of
the five are certificate-related, which may be a cluster rather than four
independent misses.

### The 3 of 5 is stale, and the tool now says so

`multi-ipsec-policy-nesting` should not have been left in that list of five at
all. Its three failures are runs 01, 02 and 04, every one of them produced
against an index without the Containment section. Only run 05 read the fixed
index, and it passed. The honest count is one run since the fix and one pass.

Rescoring does not catch this. It repairs the tests side of a comparison and can
do nothing about the corpus side, because a stored answer is frozen text: a run
cannot retroactively benefit from an index built after it. So a failure rate can
be assembled from two different corpora and look entirely current, which is what
this one did for a full session.

Two changes, neither of which touches scoring:

- **The rate is shown in run order.** `X X . X .` and `X X . . X` are both 3 of
  5 and are not the same finding. The set of labels this replaced made them look
  identical, and made them look identical hardest on the one test the fix had
  been built for.
- **Each run records the index it read.** `prepare_run.py --collect` writes
  `corpus.json`, a digest of `index/`, `INDEX.md` and `NAVIGATION.md`, per file
  and overall; `compare_runs.py` reports where that changed and which file moved.
  The content slices are excluded because `step1` proves them byte-exact on every
  build, so they cannot drift without the source revision changing.

It reads the files rather than git, because an agent environment need not have a
repository. `run.json` has carried a hand-typed `corpus_commit` since run 01 and
nothing ever read it; measured metadata is the point. Runs 01 to 05 cannot be
backfilled, since reconstructing an old index needs an old checkout, so the
signal starts at run 06.

**The flag it prints is about evidence, not causation.** A test whose failures
all predate the change gets marked whether or not the change had anything to do
with it, so the count of runs since is printed beside it. At one run since, that
is nearly nothing, and the mark should be read as "this rate is out of date",
never as "this was fixed".

One related defect was found while checking the test itself, and fixed. Its
required fact was the bare string `proposal`, which also matches
`ike-sa-proposal`. That object sits one level *up* from the spd entry, keyed by
local instance and peer, so an answer naming only it would contradict the
question's "what hangs underneath" and still pass. No stored run exploited the
loophole and both passing runs name `ipsec-sa-proposal` explicitly, so the fact
is now `ipsec-sa-proposal` at no cost: all five runs score identically and every
sequence in the rate table is unchanged. It is also the narrower fact, in 5 of
377 files where the whole set is still satisfied by none.

## The certificate sweep, and the sixth appearance of the prose class

The three certificate tests at 3 of 5 shared one cause, and it was the class
already found five times: a required fact the question never asks for, taken
from the reference answer's volunteered tail. `end-entity` describes a
certificate rather than naming one, `sha256` came from a "defaults worth
knowing" list, `trust-chain` from a list of read-only reporting attributes. In
each case the failing runs had answered the question correctly.

**The class was detected by position, not by failure.** Locating each fact
inside its own reference answer and flagging the ones in the last 40% turned up
ten candidates across the 27 certificate tests, of which seven were passing 5 of
5. That ordering matters: the previous triage at `e4c274a` looked only at
failures and was weaker for it.

Position only nominates, though. The question decides, and it cleared three of
the ten: `priority` on `cert-to-name-mapping` sits late but is how the node
picks between matching rules, `peer-certificate` late in
`multi-certificate-role-disambiguation` is the "which is the far end's" the
question asks for, and `ssh-known-host` is the remote host the question names.

Eight tests were edited, five of them passing at the time:

| Test | Dropped | Why |
| --- | --- | --- |
| `cdp-auto-refresh` | `last-update-result` | question asks whether it fetches on a schedule, not how the last one went |
| `csr-gen-pending-import` | `sha256`, `eccp256` | signature and key defaults; the question asks what state the node is in |
| `est-server-port-default` | `path-segment` | neither the port nor the multiplicity the question asks about |
| `isk-in-use-flags` | `KRK-name` | the signing root, not the in-use flags |
| `ssh-keygen-replaces-keys` | `show ssh-host-key` | how to view them after, not what happens to them |
| `local-certificate-revocation-mode` | `trust-chain` | a reporting attribute, not forcing revocation |
| `key-replacement-package-view` | `key replacement package` | redundant: `carries()` flattens separators, so the hyphenated fact already matches the spaced form |

`chain-certificate-behind-encrypted-app` was restructured rather than trimmed.
It required `local-certificate`, `digital identity`, `end-entity` and `X509v3`,
and **nothing in that set answered the second half of its own question**: what
has to be imported before the application comes up is `trusted-certificate`,
which was not required at all. Now `local-certificate`, `secure-application`,
`trusted-certificate`, which is the question's three objects and no prose.

**One candidate was examined and deliberately left alone.**
`multi-certificate-role-disambiguation` requires `end-entity certificate`, the
same prose token dropped from the chain test, and dropping it here takes the
fact set from 3 files to 5, past the discrimination gate. Every replacement is
prose as well, because what separates those four objects is descriptive:
`white-listed` is not in the reference answer and would fail the gate that a
reference answer must pass its own test, and rewriting the reference answer to
fit the instrument is not a repair. The token is also more defensible here than
in the chain test, where the question never asked about a certificate's position
in a hierarchy; this question explicitly asks which object is the node's own
identity as against the CA, and end-entity is the distinction. Left as it is,
and recorded rather than buried.

**Read the score movement with the caveat.** Runs now score 434, 431, 451, 447,
449, up from 434, 429, 449, 445, 446. Three tests that were failing had the
facts they failed on removed, which is an instrument adjusted after seeing what
it flagged and cannot clear itself. What makes it repair rather than tuning is
that five of the eight edits were to tests nothing had flagged, and that the
detection ran over all 27 certificate tests before any verdict was consulted.
The additions to the chain test are supplied by all five runs, so they
strengthen the test's coverage of its question and not its difficulty against
this model.

Teeth after the pass: worst fact set matched by 4 of 377 files, mean 1.07, no
test passed by an empty or generic answer, and the same 7 tests as before pass
on the command name alone, all of them sections too thin to support more.

### `multi-controller-card-vs-card` was a different defect

The fourth test at 3 of 5 was not off-question. Its missing fact `capability`
**is** what the question's third clause asks for, and it was encoded as guide
prose lifted from the description sentence "show the capability information for
supported card". The three failing runs answered correctly using "supports" and
"supported"; the two passing runs happened to write the noun. `carries()` allows
only a trailing plural `s`, so even "capabilities" would have failed. Nothing
about the corpus was involved.

Re-encoded to **`card-type`**. The question contrasts three things: the
controller, any card in the shelf, and a given *model* of card. `card-type` is
the key `supported-card` is addressed by, so it is what makes that object
model-scoped rather than a unit in a slot, and it is a typed token rather than a
description. The obvious alternative, naming a specific capability attribute
like `grid-mode-support` or `console-port-support`, was rejected as the same
defect in a new costume: the question asks what a model supports in general and
never asks about grid modes, so requiring one would be off-question.

**Two hazards worth recording.** `supported-card-mode` is the one candidate all
five runs state, and it is absent from the reference answer, so the gate
requiring a reference answer to pass its own test ruled it out. That gate did
real work here, because measuring candidates against the runs at all is how a
test gets fitted to a model. And this change makes a 3-of-5 failure pass, the
same flag carried by `sub-component-view` and `edit-vs-up-vs-top-navigation`.

**`required-type` on the same test was suspected and cleared.** It looked
off-question, since it comes from a provisioning example while the question asks
how to *see* configuration. On inspection it is the only workable discriminator
for the "any card in the shelf" clause: bare `card` appears in 147 files and
matches inside `controller-card`, `card-name` is not in the cited files at all,
and `admin-state` is in 56. At 8 files `required-type` is the sharpest thing
that names the general card object, and characterising it as the object where
provisioning happens is a fair reading of the contrast the question draws. Left
alone.

After this pass the worst rate in the suite belongs to
`multi-ipsec-policy-nesting`, whose 3 of 5 is the stale one described above.
Everything genuinely live sits at 2 of 5 or lower.

## Run 06: the falsification run

448/457. Six runs: 98.6, 98.0, 98.7, 97.8, 98.5, 98.0%, spread 97.8 to 98.7%.
Mean answer 817 characters against 810, 816, 821, 830, 819 for runs 01 to 05, so
the method held. No shard failed, and both all-multi shards, the shape that
delegated and wrote nothing in run 03 and twice in run 05, completed normally at
around 97 tool calls and nine minutes each.

This run existed to check three changes that were all made against runs 01 to
05, and therefore could not be checked by them. An edit fitted to the stored
runs passes those runs by construction.

**All three survived.**

- **`multi-ipsec-policy-nesting` passes.** Post-containment it is now 2 of 2,
  against 1 of 4 before. Run 05 alone could not distinguish the section working
  from one lucky answer; a second independent pass is a real, if small, result.
- **All eight edited certificate tests pass**, as does
  `multi-certificate-role-disambiguation`, the one the sweep deliberately left
  alone.
- **`multi-controller-card-vs-card` passes on `card-type`.**

**What that does and does not show.** A test made easier would also pass here.
The argument for each edit was that its question demanded the change, and that
argument was made and written down before this run existed. What run 06 rules
out is the specific failure of having fitted the tests to five particular
answer sets.

### What it turned up instead

Nothing fails above 50%, and the 3-of-6 tier is mostly the same class a seventh
time. Four of the six are facts their own questions never ask for:

| Test | Missing | Why it is off-question |
| --- | --- | --- |
| `console-baud-rate-default` | `local-switch` | the question asks the baud rate and whether it auto-senses |
| `equipment-policies-auto-migration` | `cable-id-control` | a third thing beyond the subtype and degree questions asked |
| `facilities-overview` | `system facilities` | prose, where `facilities` and `show facilities` are already required |
| `multi-access-rule-ordering-limits` | `permit` | the question asks which rule wins and how many groups attach, not the action |

The first two now fail every run since 03 and the third every run but one, which
is what the rate view is for: under the old binary they were invisible.

`facilities-overview` is worth a note. `system facilities` was named as a
contested prose fact by the per-fact agreement analysis described at the end of
this file, which was proposed and declined. That analysis identified it without
looking at pass or fail, three runs before it became a consistent failure.

**`multi-route-sources` is the one candidate that is not this class.** Its
question asks where to see every route the node is using "including the ones
nobody typed in by hand", so `dynamic` is squarely on-question. It fails runs
01, 02 and 06 and passes 03, 04, 05. Triage it from the corpus, not from the
fact list.

## The seventh sweep, and why one detector was not enough

Sweeping the four off-question facts run 06 exposed turned up a limit in the
instrument used for the previous six passes.

**Position alone does not find this class.** Locating each fact inside its own
reference answer and flagging the last 40% is what found the certificate
defects, but run suite-wide it flags **157 of 457 tests**, which is not a
shortlist, and it **misses `facilities-overview` entirely**, because
`system facilities` sits in that answer's opening sentence. Position finds facts
harvested from a volunteered tail. It cannot find prose lifted from a
description sentence, and both are the same defect.

**The second detector is shape, not position.** A fact that contains a space, no
hyphen or underscore, no digit, and does not begin with a CLI verb is English
prose rather than something an operator types. That is **92 facts**, a list
short enough to read.

It is worth recording what that list contained. The per-fact agreement analysis
described at the end of this file was proposed and declined; it had named
`system facilities`, `data model for system templates`, `sub-level objects`,
`temperature sensors`, `status equipment` and `controller card` as the contested
prose facts. **The shape detector finds every one of them**, without looking at
run agreement, pass or fail.

**Prose is a smell, not a verdict.** Of the 92, most are correct: they are the
question's own words (`temperature sensors` is in the question that requires
it), or the guide's own value phrasing (`Root and Intermediate`), or an
expansion the question asks for (`Enrollment over Secure Transport`).

### What was changed

Ten tests. Four are the ones run 06 exposed:

| Test | Dropped |
| --- | --- |
| `console-baud-rate-default` | `local-switch`, a per-console override of the system-wide serial switch, in a question about baud rate and auto-sensing |
| `equipment-policies-auto-migration` | `cable-id-control`, a third policy beyond the subtype and degree ones asked about |
| `multi-access-rule-ordering-limits` | `permit`, where the question asks which rule wins and how many groups attach; the reference answer itself says the outcome "lives on access-rule, not on the list" |
| `facilities-overview` | `system facilities` |

That last one turned out to head a family of five. **"Which command shows X"
questions whose third fact is the noun phrase from the description sentence**:
`facilities-overview`, `templates-overview` (`data model for system templates`),
`security-container-view` (`top level security container`), `routing-overview`
(`routing information`) and `downloads-list` (`list of downloads`). In every
case the question asks which command, and `show <thing>` answers it completely.
Four of those five were passing every run.

`l2-bridge-attributes` had `intended purpose` re-encoded to `description`, the
attribute that phrase is glossing.

`multi-route-sources` had `static route` re-encoded to `ipv4-static-route`, the
command the question's "where do the hand-typed ones get added" actually names.

### Two corrections

**`multi-route-sources` is not a corpus gap, and calling it one was wrong.**
`269-route.md` says in its description that the command shows routes "from
various sources, such as dynamic protocols and static route", and carries a
`source-protocol` attribute whose values are "OSPF, BGP, static etc." The corpus
states it plainly. The test requires the bare word `dynamic`, and three runs
answered the question correctly without using that word. That is a brittle
one-word encoding, not a documentation hole. It is left failing rather than
quietly fixed, because `source-protocol` is absent from the reference answer and
rewriting a reference answer to fit the instrument is not a repair.

**`delete-best-effort-flag` was examined and left alone.** Its `sub-level
objects` is off-question, but dropping it takes the fact set from 1 file to 5,
past the discrimination gate, exactly as with
`multi-certificate-role-disambiguation` in the certificate sweep. An
off-question fact can be the only thing giving a test teeth, and that is a
reason to leave it and say so.

### Honest weaknesses

Six of the ten edits were to tests that were failing, which is weaker than the
certificate sweep, where five of eight were to tests nothing had flagged. The
four passing-test edits all came from the one family above. The other 81 prose
facts were read and deliberately kept.

**Layer 2 cannot express a negation.** `lldp-neighbor-egress-unsupported`
requires the string `egress direction is not supported`, and its question is a
yes/no whose answer is no. Any shorter encoding either loses the denial
(`ingress` alone is satisfied by an answer that never denies egress) or becomes
vague (`not supported` matches a denial about anything). `database-clear-scope`
has the same shape with `does not wipe logs`. Both were left as they are. This
is a real limit of a string matcher, not a test defect, and the brittleness it
causes is the price.

Runs after the pass: 435, 433, 452, 450, 454, 452, spread 98.2 to 99.3%. Teeth:
worst fact set 4 of 377, mean 1.09, nothing passed by an empty, generic or
command-name-only answer, and the same 7 on the command name alone.

## Run 07: the sweep survives, and the score drops anyway

446/457. Seven runs: 98.6, 98.2, 98.9, 98.5, 99.3, 98.9, 97.6%, spread 97.6 to
99.3%. Mean answer 804 characters. No shard failed, and the all-multi pair
completed at 97 and 82 tool calls.

**All ten tests edited in the seventh sweep pass.** Nine now pass all seven
runs, and `multi-route-sources` passes here too, leaving its three failures in
runs 01, 02 and 06. Four of the ten were passing before the edit, so a weakened
test was the specific thing this run could have caught, and it did not.

**This is also the lowest score of the seven, and that is not the sweep.** Nine
tests that passed run 06 fail here: on `pm-threshold-profile`, `NCT`,
`controller card`, `secure-application`, `high order ODU` with `Super-channel`,
`clear system`, `total-ageouts`, `trap-community-string` and `attribute-value
pair`. Mostly identifiers rather than prose, spread across unrelated domains,
which reads as ordinary variance. **The honest reading of seven runs is the
spread, not the best or the latest number.**

**A third detector gap.** `attribute-value pair` is prose by any reading, and
the shape detector excluded it because it contains a hyphen. The filter treats a
hyphen as evidence of a typed identifier, and a hyphenated English compound
defeats that. Position missed prose in an opening sentence; shape misses prose
containing a hyphen. Neither detector is complete, and both are cheap, so run
both and read the union.

Run 07 is also the first run whose corpus digest could be compared with another:
it matches run 06 and is reported as unchanged, which is what makes the two
directly comparable rather than merely assumed to be.

## Triaging the seven at 3 of 7, and a matcher bug behind three of them

Seven tests sat at 3 of 7 after run 07. Triaging them found one matcher defect,
one test pointed at the wrong command, one brittle encoding, and three that
should be left alone.

### The matcher was losing `clear database`

`database-clear-scope` was failing on the fact `clear database` while its
answers said, correctly, ``clear [-f] database`` — the guide's own syntax. The
fact needs its two words adjacent, and the optional flag sits between them, so
an answer was penalised for being precise.

Scanning the suite for two-token command-form facts found 29, of which exactly
three had ever been missed, and all three are `clear`, the verb the guide writes
as `clear [-f] <object>`: `clear database` 3 of 7, `clear app` 2 of 7, `clear
system` 1 of 7. Every one of those six failures had the bracketed flag in it.

`carries()` now skips a single CLI flag between the words of a multi-token fact,
bracketed or bare, and nothing else. This is the same kind of accommodation as
the separator flattening already there: the guide writes `software-load` where
an answer writes "software load", and it writes `clear [-f] database` where the
fact says `clear database`. Verified across 7 runs and 1,733 facts, it changes
**exactly those six fact verdicts and nothing else**, and the three tests drop
to 0 failures.

That is a matcher repair, not a relaxation, and the distinction is worth
keeping: the answers already stated the fact, in the guide's own notation.

### `multi-l1-encryption-prerequisites` was pointed at the wrong command

Its question is "before layer 1 encryption will come up on a link, what has to
already be in place at each end?", and it required `secure-application`. The
corpus does not support that. `secure-application` covers an application using
an X509v3 certificate as its digital identity; `277-secure-entity.md` is the L1
object, and its parameter table names no certificate. **Neither file
cross-references the other.** The dependency was the reference answer's
inference, and its own `inference_flags` admitted the ordering was inferred
while the relevance never was.

The failing fact was a symptom: `secure-application` was the test's `primary`,
and the convention is that a test names its primary command. So the fix is
structural, not a fact edit. The test now points at `277-secure-entity.md` and
requires `secure-entity`, `remote-secure-entity`, `supporting-facility`, which
is what the question asks and what the guide states. The reference answer was
rewritten to drop the unsupported precondition and keep the other objects as
context, and an `inference_flags` entry records what was removed and why.

This is the first test found to be **asking a good question of the wrong file**,
as against a bad fact on the right one. Worth looking for again: the tell is a
reference answer whose opening sentence asserts a dependency the cited files
never state.

### `multi-restart-card-consequences`: the guide says it both ways

Required `controller card`; three runs answered "node controller". Both phrases
are in `265-restart.md`, which writes "the active controller card" in the
`resource-id` row and "node controller" in its description. The test picked one
of the guide's two names for the same thing. Re-encoded to `controller`, which
is what the question actually needs the answer to distinguish and is neutral
between the guide's own phrasings.

### Left alone, with reasons

- **`multi-ipsec-policy-nesting`.** Its three failures are runs 01, 02 and 04,
  all before the Containment section. It has passed 3 of 3 since. The rate is
  stale and only decays as runs accumulate.
- **`delete-best-effort-flag`.** `sub-level objects` is off-question, but
  dropping it takes the set from 1 file to 5, past the discrimination gate. Same
  as `multi-certificate-role-disambiguation`.
- **`multi-resource-type-defaults`.** `pm-threshold-profile` is
  on-question and correctly encoded: it is keyed by `<resource-type>` and holds
  `default-low-threshold` and `default-high-threshold`, described as "System
  defined default value". The question asks where a new resource's performance
  monitoring defaults come from, and that is the object. Three runs answer with
  `pm-profile-entry` and `pm-control-entry` and stop. **This is the most
  interesting failure left in the suite**, because the test looks right and the
  answers look reasonable.
- **`multi-route-sources`** stays as characterised above: a brittle one-word
  encoding, not a corpus gap.

Two of the four repairs turn a 3-of-7 failure into a pass, which is flagged here
as with every previous pass. Runs after triage: 435, 434, 454, 451, 456, 454,
450, spread 98.4 to 99.8%. Teeth unchanged: worst set 4 of 377, mean 1.09,
nothing passed by an empty, generic or command-name-only answer.

## Run 08: fourteen repairs hold, and the three left alone rise to the top

447/457. Eight runs: 98.6, 98.4, 99.3, 98.7, 99.8, 99.3, 98.5, 97.8%, spread
97.8 to 99.8%. No shard failed.

**All fourteen repairs from the last two passes pass**, including the widened
matcher and the re-pointed `multi-l1-encryption-prerequisites`.

Read that evidence precisely. For the re-pointed test, runs 03, 04 and 07 now
pass as well, but **that is rescoring arithmetic, not new evidence**: those
answers are being scored against different facts than before. Run 08 is the one
independent data point per repair. It is a pass in every case, which is what a
weakened test would not have produced, but it is one bit each.

### The three left alone are now the worst in the suite

That is the finding. Every test deliberately kept in the last triage has failed
again, and they now sit at the top:

| Test | Rate | Why it was left |
| --- | --- | --- |
| `delete-best-effort-flag` | 4 of 8 | its off-question fact is the only thing inside the discrimination gate |
| `multi-resource-type-defaults` | 4 of 8 | `pm-threshold-profile` is on-question and correctly encoded |
| `multi-route-sources` | 4 of 8 | `dynamic` is a brittle one-word encoding, not a corpus gap |

Two of the three are **instrument limits rather than test defects**, and they
are now the most consistent failures being measured. That is worth stating
plainly: the suite's top failures are no longer telling us about the corpus,
they are telling us about the fact matcher. `delete-best-effort-flag` needs a
fact set that both discriminates and stays on-question, and `multi-route-sources`
needs an encoding of "not typed in by hand" that survives paraphrase. Neither is
a documentation problem.

`multi-resource-type-defaults` is the exception and remains the one place where
the test looks right and the answers look reasonable.

### Other observations

Five tests that passed run 07 fail here, on `permit`/`deny`/`fail-action`,
`dynamic`, `extended-config`, `non-configuration data` and `flashing-green`,
spread across unrelated domains. Ordinary churn.

Mean answer length is 785 characters, the lowest of the eight, against a range
of 785 to 830. The last three runs read 817, 804, 785. That may be nothing, and
three points is not a trend, but it is the first monotonic movement in a figure
that had been stable, so it is worth watching.

The corpus digest is unchanged across runs 06, 07 and 08, so all three are known
to have read the same index rather than assumed to have.

## Remaining work

Single-command coverage of Chapter 6 is complete, the multi-command set is
complete at 65 across all five cluster bases, and chapters 3 to 5 are now
covered by batch 15. What is left:

**1. The two instrument limits now topping the failure list.**
`delete-best-effort-flag` needs a fact set that discriminates without an
off-question fact; `multi-route-sources` needs an encoding of "not typed in by
hand" that survives paraphrase. Both are matcher problems wearing a test's
clothes, and both are now at 4 of 8.

**2. `multi-resource-type-defaults`**, the one remaining failure where the test
looks right and the answers look reasonable. See the triage section above.

**3. Both fact detectors, run together.** Position misses prose in an opening
sentence and shape misses prose containing a hyphen, as `attribute-value pair`
showed. Neither is complete; the union is cheap.

**3. `corpus_hits` still matches raw substrings,** so `tic` registers in 187
files. Needs the separator flattening `carries()` has; a naive word-boundary fix
drops 50 tests' facts to zero.

Lower value: second single-command tests for `show`, `set`, `download`, `status`
and `activate`, each of which carries far more behaviour than one question
exercises. Layer 3 remains unimplemented by choice. `_authoring.py` still builds
multi-command tests only; batch 15 was written by hand against layer 0, which
worked but does not scale to another batch this size.

**What this suite is and is not.** Layer 1 is a deterministic lexical simulation
of what an agent would do, and layers 0 and 1 being green says nothing about
whether an answer is any good. What the tests have bought is a gap-finding
instrument: around 70 real vocabulary gaps in the index, found and fixed, and
now three scored model runs. Read the runs as diagnostic rather than as a grade.
A single run is a number about this fact list and this model on that day; the
run-01 audit showed a third of its misses were the fact list's fault, run 02
showed four of the tests it blamed on the corpus were noise, and run 03 showed
three more that *two* runs had agreed on were noise as well. Quote the 7-test
persistent set and the 95.7-96.8% spread, never a single headline figure.

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
