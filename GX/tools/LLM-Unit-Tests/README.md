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
| Carry the required facts, Claude Opus 5, four runs | 434/441, 428/441, 448/457, 444/457. None fail in all four; 2 fail 3 of 4, 9 fail 2 of 4. Quote the failure rate, not the binary |
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
`compare_runs.py` scores every stored run against the working tree and separates
a test that every run fails from one that merely came out badly once. See
`runs/README.md`.

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
**A test is not persistent-or-noise, it has a failure rate**, and
`compare_runs.py` currently reports only the binary. Reporting the rate is the
obvious next improvement to it.

Read `sub-component-view` and `multi-ipsec-policy-nesting` as the current best
candidates for a real gap, and triage them from their questions as before.

## Remaining work

Single-command coverage of Chapter 6 is complete, the multi-command set is
complete at 65 across all five cluster bases, and chapters 3 to 5 are now
covered by batch 15. What is left:

**1. Report a failure rate in `compare_runs.py`,** not the persistent/unstable
binary. Run 04 showed the binary hides the most consistent failure in the
suite: `multi-ipsec-policy-nesting` sits at 3 of 4 yet run 03 had cleared it as
noise. This is a small change to the one tool everyone reads results from.

**2. Triage `sub-component-view` and `multi-ipsec-policy-nesting`,** the two at
3 of 4, from their questions as before.

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
