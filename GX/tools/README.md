# GX documentation toolchain

Generates `../R9_1_GX_CLI_Reference/` from `../R9_1_GX_CLI_Command_Reference_Guide_001P4.md`.

The source document is a 28,634-line PDF-to-Markdown conversion of the Nokia
1830 GX Release 9.1 CLI Reference Guide (1,358 pages, 374 CLI commands). This
toolchain splits it into per-command files and builds a set of indexes designed
for an LLM to search rather than for a human to browse.

## Rebuild everything

```bash
python build_all.py
```

Run it from anywhere; paths resolve from the script location, not the working
directory. It runs the five build steps in order, then three checks, and exits
non-zero if anything fails. `--check` skips the build and only verifies.
`--quiet` trims the output to step names and failures.

Python 3.12 or newer. No third-party packages.

## The two rules

**1. Everything under `../R9_1_GX_CLI_Reference/` is generated. Never edit it by
hand.** The next `build_all.py` overwrites it. To change an output, change the
step that produces it, or change `curated.py`.

**2. `../R9_1_GX_CLI_Command_Reference_Guide_001P4.md` is read-only input.** No
script writes to it. The split is verified byte-exact against it on every build:
strip the frontmatter from all 386 content slices, concatenate them in order,
and the result must equal the source byte for byte. If you ever need to change
that guarantee, you are probably solving the wrong problem.

## Layout

```
GX/
  R9_1_GX_CLI_Command_Reference_Guide_001P4.md   source, read-only
  R9_1_GX_CLI_Reference/                         generated output
  tools/                                         this directory
    build/                                       intermediates, safe to delete
```

| File | Role |
| --- | --- |
| `build_all.py` | Runs the pipeline and the checks. Start here. |
| `gxpaths.py` | Path resolution and source invariants. Imported by everything. |
| `step1_split.py` | Source to 386 content slices, plus the part manifests |
| `step2_privileges.py` | Chapter 2 Tables 32 to 34 to `build/privileges.json` |
| `step3_extract.py` | Structured record per command, page map, table map |
| `step4_build_index.py` | `INDEX.md` and everything in `index/` |
| `step5_navigation.py` | Source table of contents to `NAVIGATION.md` |
| **`curated.py`** | **The only file meant to be edited by hand.** Domains and topics. |
| `check_consistency.py` | 30 assertions: every stated number matches reality |
| `check_index_links.py` | Every link and JSONL record resolves |
| `check_navigation.py` | Every `NAVIGATION.md` link resolves |

## Pipeline

Steps must run in this order; each depends on the previous one's output.

**step1_split.py** cuts the source on H1/H2 boundaries by line number into 386
files: front matter, chapters 1 to 5, one file per chapter-6 command, acronyms.
Each file gets YAML frontmatter (`source`, `part`, `section`, `source_lines`)
and then the original bytes, unmodified. Also writes `README.md` and
`06-operation-commands/README.md`. Self-verifies byte-exactness and fails loudly
if it breaks.

**step2_privileges.py** parses the three user-group tables out of chapter 2.
Table 34's command column is only filled on each command's first row, so rows
are forward-filled. Writes `build/privileges.json`.

**step3_extract.py** reads the split files back (part ranges come from their
frontmatter, so the split stays the single source of truth) and produces:

- `build/commands.raw.jsonl`: one record per command with summary, syntax verbs,
  AID pattern, access mode, every parameter row, usage details, table numbers,
  page citations
- `index/pages.tsv`: page number to file, for all 1,358 pages
- `index/tables.tsv`: table and figure number to file, page, caption

It cross-checks its own page arithmetic against the document's table of
contents: all 374 derived page numbers must equal the page the TOC states. That
check is the reason to trust the page map.

**step4_build_index.py** joins the raw records with `curated.py` and
`build/privileges.json`, then writes `INDEX.md` and the `index/` files. Deletes
the raw intermediate when done.

**step5_navigation.py** takes the source's own table of contents and rewrites
its 457 internal anchors into relative paths, by slugging all 2,350 headings in
the document and mapping each to the file that contains it.

## Making changes

**Add or fix a topic, or move a command between domains:** edit `curated.py`,
then `python build_all.py`. `DOMAINS` must assign every command exactly once;
`step4` raises `KeyError` on a missing one and `check_consistency.py` catches
count drift. `TOPICS` entries are `(title, search_terms, commands)`. Search
terms are what a user might type; they exist so that a query using words the
document never uses ("wavelength", "laser shutdown", "loopback") still routes.

**A new revision of the source document:** the boundaries in `step1_split.py`
(`parts`, `CH6_START`, `CH6_END`) and the invariants in `gxpaths.py`
(`EXPECTED_PARTS`, `EXPECTED_PAGE_MARKERS`, `EXPECTED_COMMANDS`) are specific to
this revision. Assertions will trip on a new one. That is deliberate: re-derive
the line numbers by inspecting the new document rather than loosening the
assertions. Everything downstream of `step1` adapts automatically, because it
reads structure back out of the split files.

**Add a field to the records:** add it in `step3_extract.py` (both the main loop
and the `AUX` block, which hand-writes the four auxiliary commands that appear
only in tables), then document it in the field table `step4_build_index.py`
writes into `index/README.md`. `check_consistency.py` verifies that table stays
accurate.

## Traps

**The header-word trap.** Parameter tables are parsed by column name. It is
tempting to skip rows whose first cell looks like a column header. Do not filter
on individual words: `name` is a real parameter in 109 commands, `parameter` is
a real parameter in the PM commands, and `type`, `value` and `description` are
all real parameter names too. Filter only when the entire row reads as a header
(`_is_header`). This bug was introduced twice during development and silently
deleted about a quarter of the parameter index both times.

**Multi-table sections.** A `Command Parameters` section can hold more than one
table; 49 of them do. `show` has three. A parser that stops at the first blank
line loses most of the content. `parse_tables` returns every table in a block.

**Page markers are sparse.** Only 652 of the 1,358 pages carry a
`<!-- page N -->` marker. `pages.tsv` fills the gaps by interpolation and labels
each row `exact` or `inferred`. A marker is often followed by a blank line and
then the next section heading, so a page must be attributed to the first line of
real content after the marker, not to the marker's own line, or it lands in the
section it ended rather than the one it opened.

**Do not "fix" the source's defects in the output.** They are real and known:

- The TOC lists section 2.2.2 (PuTTY) but the body has no such heading; that
  content sits inside 2.2.1. `NAVIGATION.md` points the entry at the containing
  file and says why.
- Figure 7 has no caption line, so it has no row in `tables.tsv`, though it is
  listed in the List of Figures and its image reference exists.
- Three rows of Table 34 (`clear`/`isk`, `run`/`script task`, `default`) were
  collapsed by the PDF conversion so one cell holds two values. They are
  reproduced verbatim and flagged in a "Source data quality" section that
  `step4` generates by detection, not by hard-coding.
- Image references point at `images/figure-p*.png`, which does not exist beside
  the source either. The paths were already dangling.

Sixteen more surfaced when agents read the corpus to answer the LLM unit tests,
seven in run 01 and nine in run 02, recorded here so nobody spends the afternoon
deciding they are conversion bugs. They are in the source and they stay.

Three of them are classes rather than one-offs, which only became visible once
run 02 rediscovered them independently. Expect more members of each, and check
which class a new find belongs to before writing it up as novel:

- **The syntax line lists an attribute the parameter table omits.** Four
  instances below. An answer about such an attribute can state that it exists
  and nothing else, which is a real limit on what the corpus can support.
- **A table row is carried in from an unrelated table.** Three instances. The
  giveaway is a values column of `true, false` under a description that plainly
  describes something else.
- **A table default contradicts the section's own worked example.** Two
  instances. Neither side is checkable against the other by any script here, so
  these are found only by reading.

Found by run 01:

- `095-est.md` lists `eccp251` among the key algorithms, where the sequence and
  every other entry imply `eccp521`.
- `319-super-channel.md` gives `contention-check-status` the value `failk`,
  which is `failed` with the PDF's usual dropped characters.
- `312-status.md` says "The following Five dashboards are available" and then
  lists six, and its Command Syntax line omits `L0-spectrum` although the
  parameter table documents it.
- `014-alarm.md` demonstrates the filter `show alarm-inventory
  can-be-cleared-by-user=true`, but `016-alarm-inventory.md` never lists
  `can-be-cleared-by-user` among its parameters.
- `324-supported-gain-range.md` describes its `name` key as "Flag to enable or
  disable access control. | true, false | false", a row copied in from an
  unrelated table.
- `094-equipment-templates.md` gives `use-serdes-templates` the values
  `enabled, disabled` in the parameter table and `true`/`false` in its example.
- `148-key-replacement-package.md` has the descriptions of `key-length` and
  `key-payload` swapped: `key-length` reads "Key Payload (hex format)" and
  `key-payload` reads "Key length in bits".

Found by run 02. Every one was checked against the split file before being
listed here; a tenth candidate was rejected, see below.

- `280-security-policies.md` lists `disable-user-lockout` in the syntax at both
  the `set` and `show` forms, but gives it no parameter row, so no values and no
  default are documented.
- `273-sc-rx.md` and `274-sc-tx.md` both list `association-number`,
  `key-identifier` and `next-packet-number` in the `show` syntax and omit all
  three from the parameter table.
- `173-mka-policy.md` gives `macsec-cipher-suite` the values `true, false`, and
  `confidentiality-offset` the values `allowed, inhibited` under a description
  that says it specifies a number of octets sent in plain text.
- `313-stm.md` gives `expected-mapping-mode` the values `true, false` under a
  description saying the possible values depend on the hardware.
- `172-macsec-mka.md` merges two parameters into one row named
  `psk-lifetimepsk-expiration-warning`, so the separate `psk-lifetime` range is
  not recoverable; the syntax lists them as two attributes.
- `098-eth-zr.md` gives `fdd-threshold` the default `0.0195 avg BER` and
  `fed-threshold` `0.0206 avg BER`, where its own sample output shows
  `0.011250000` and `0.011870000`. The two clear-thresholds agree.
- `023-apply-template.md` prints the `template-type` default as
  `serdes-templage`.
- `009-activate-snapshot.md` names the attribute `db-paraphrase` in the syntax
  and `paraphrase` in the parameter table.
- `008-activate.md` notes that the `timeout` parameter "does not take effect in
  1830 GX G30 R5.1" and directs the reader to `terminate` instead. A stale
  cross-release reference inside an R9.1 guide, not a conversion fault.

Found by run 03. Runs 02 and 03 independently rediscovered most of the entries
above, which is the main reason to trust them; only these four were new.

- `001-aaa-server.md` contradicts itself on `shared-secret`: the note says the
  maximum length is 64 characters and anything longer is denied, while the
  parameter table gives `String (length 0...128)`.
- `370-validate.md` documents the syntax as
  `validate candidate <candidate> command <string>`, with no brackets, so both
  keywords are mandatory, but its only example omits both:
  `validate 'set ne altitude 600'`. Contrast the rejected `protection-switch`
  case below, where the brackets made the labels optional and the example was
  therefore correct. The bracket check is what separates the two.
- `370-validate.md` also states its description sentence twice, verbatim.
- `065-current-advanced-parameter.md` carries **unconverted DITA markup** in an
  example block: `\<codeblock id="codeblock\_azq\_wkh\_43c" class="+ topic/pre
  pr-d/codeblock "\>` wrapping the output, with the closing tag at the end. This
  is a fourth class, distinct from the three above: conversion residue rather
  than a content error. `grep -c codeblock` over the command files finds one
  other, `086-download.md`, so the class has two known members and that grep is
  the way to enumerate it.

**Rejected, do not re-report:** `252-protection-switch.md` was flagged for a
worked example that passes the group name positionally rather than as
`protection-group=`. It is correct. The syntax reads
`[operation-type=]<value> [switch-target=]<value> [protection-group=]<value>`,
where the brackets make the *label* optional, so the example matches its own
syntax. Optional-label syntax is easy to misread as a defect; check the bracket
placement before writing one up.

**Curated is not extracted.** Domain and topic assignments in `curated.py` were
made by reading all 374 command descriptions. They are editorial judgement, and
`index/topics.md` says so. Do not present them to a user as if the document
stated them.

## What the checks prove

`check_consistency.py` is the important one. It verifies, among 30 assertions:
all 1,358 pages map and all 332 cited pages resolve; indexed tables match the
List of Tables exactly; for all 374 commands the recorded verbs, table numbers,
page citations and example flags match the section body; no command loses more
than three parameter rows to the parser; every advertised AID actually belongs
to its command; domain counts match their tables; and every number written into
prose in `INDEX.md`, `index/README.md`, `index/parameters.md` and
`index/access-control.md` matches the data. If you change a generator, run it.

Current state: 30 of 30 assertions pass, 2,221 links resolve, and the split
reassembles byte-identically to the source.
