# LLM unit tests for the GX CLI Reference

Natural-language questions about the 1830 GX CLI, each paired with an
approximate reference answer and machine-checkable expectations. They test
whether the generated index in `../../R9_1_GX_CLI_Reference/` actually gets an
agent to the right page, and whether an answer carries the facts the document
states.

```bash
python run_tests.py              # validate the tests, then score retrieval
python run_tests.py -v           # show every test, not just failures
python run_tests.py --render     # regenerate TESTS.md for human reading
```

No third-party packages, no model required for the default run.

## Status

**Chapter 6 is complete: all 374 operation commands have at least one test.**
376 single-command tests across 14 batches, plus the 5 pilot multi-command
tests, which stay parked until the single-command set is signed off.

| Measure | Value |
| --- | --- |
| Chapter 6 coverage | 374 / 374 commands |
| Single-command tests | 376 |
| Multi-command tests | 5 (pilot) |
| Validate against the document | 381 / 381 |
| Route correctly from the index | 380 / 380 scored, 1 excluded as compound |
| Question does not name its command | 356 / 376 (94%) |
| Distinct archetypes | 15 |
| Marked `weak` (thin source section) | 30 |
| Average reference answer | 468 characters |
| Verbatim evidence quotes | 403 |

Batches are one file each in `tests/`, named `single-NN-<domains>.jsonl`, so a
batch stays separately reviewable and the runner picks up every `.jsonl` in the
directory.

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
Single-command tests must reach their one file; multi-command tests must reach
at least half the cluster, because no single query realistically surfaces every
member.

**Layer 2, facts.** Supply candidate answers as JSONL of `{"id":…, "answer":…}`
and pass `--answers`; each answer must contain every string in `expect.facts`.

Layer 3, judging prose against `approximate_answer`, is intentionally not
implemented. It needs a model at run time. The reference answers are written for
that purpose when you want it.

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

Multi-command clusters are drawn from the 16 curated domains, the topics in
`index/topics.md`, and the AID containment hierarchy. `cluster_basis` records
which of those the cluster came from.

## Remaining work

Single-command coverage of Chapter 6 is complete. What is left:

- **Multi-command tests**: 5 of a planned ~60. Held until the single-command
  set is reviewed, as agreed.
- **Chapters 3, 4 and 5**: the 21 auxiliary, navigation and piped commands have
  no tests. They are indexed and routable, just not covered here.
- **Second tests for rich commands**: `show`, `set`, `download`, `status` and
  `activate` each carry far more behaviour than one question exercises. Only
  `activate` and `optical-carrier` currently have two.
