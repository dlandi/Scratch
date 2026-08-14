"""Split the GX CLI Reference Guide into per-chapter directories.

Cuts strictly on H1/H2 boundaries by line number. Content is copied
byte-for-byte; only a YAML frontmatter header is prepended to each file.
"""
import os
import re
import sys

import gxpaths

SRC, OUT, SRC_NAME = gxpaths.SOURCE, gxpaths.DOCS, gxpaths.SOURCE_NAME
gxpaths.require_source()

with open(SRC, "r", encoding="utf-8", newline="") as f:
    raw = f.read()
lines = raw.split("\n")          # keep exact; last element is trailing remainder
NLINES = len(lines)
print(f"source: {NLINES} split-lines, {len(raw)} chars")


def body(start, end):
    """1-indexed inclusive line range -> text."""
    return "\n".join(lines[start - 1:end])


def slug(text):
    s = text.lower()
    s = re.sub(r"[^a-z0-9]+", "-", s)
    return s.strip("-")


# ---------------------------------------------------------------- inventory
# H2 sections inside chapter 6 (## 6.N. name)
cmd_re = re.compile(r"^## 6\.(\d+)\.\s+(.*?)\s*$")
commands = []
for i, ln in enumerate(lines, start=1):
    m = cmd_re.match(ln)
    if m:
        commands.append({"num": int(m.group(1)), "name": m.group(2), "start": i})
assert len(commands) == gxpaths.EXPECTED_COMMANDS, len(commands)
CH6_START, CH6_END = 4013, 27831
for a, b in zip(commands, commands[1:]):
    a["end"] = b["start"] - 1
commands[-1]["end"] = CH6_END
assert commands[0]["start"] == 4019

# parts: (directory, filename, title, start, end)
parts = []
parts += [
    ("00-front-matter", "00-cover-and-legal.md", "Cover and legal notices", 1, 36),
    ("00-front-matter", "01-contents.md", "Contents", 37, 498),
    ("00-front-matter", "02-list-of-figures.md", "List of Figures", 499, 510),
    ("00-front-matter", "03-list-of-tables.md", "List of Tables", 511, 1390),
    ("00-front-matter", "04-about-this-document.md", "About this document", 1391, 1455),
    ("01-introduction", "01-introduction.md", "1. Introduction", 1456, 1825),
    ("02-using-the-cli", "02-using-the-cli.md",
     "2. Using the Command Line Interface (CLI)", 1826, 2990),
    ("03-auxiliary-and-help-commands", "03-auxiliary-and-help-commands.md",
     "3. Auxiliary and Help Commands", 2991, 3037),
    ("04-navigation-and-display-commands", "04-navigation-and-display-commands.md",
     "4. Navigation and Display Commands", 3038, 3539),
    ("05-piped-commands", "05-piped-commands.md", "5. Piped Commands", 3540, 4012),
    ("06-operation-commands", "000-overview.md", "6. Operation Commands (chapter opening)",
     CH6_START, commands[0]["start"] - 1),
]
for c in commands:
    parts.append(("06-operation-commands",
                  f"{c['num']:03d}-{slug(c['name'])}.md",
                  f"6.{c['num']}. {c['name']}",
                  c["start"], c["end"]))
parts.append(("99-acronyms", "99-acronyms.md", "Acronyms", 27832, NLINES))

# ------------------------------------------------------------- sanity checks
cursor = 1
for d, fn, title, s, e in parts:
    assert s == cursor, f"gap/overlap before {d}/{fn}: expected {cursor}, got {s}"
    assert e >= s, f"empty range {d}/{fn}"
    cursor = e + 1
assert cursor - 1 == NLINES, f"tail not covered: {cursor - 1} != {NLINES}"
names = [(d, fn) for d, fn, *_ in parts]
assert len(names) == len(set(names)), "duplicate output filenames"
print(f"{len(parts)} parts, contiguous coverage of lines 1-{NLINES}")

if "--dry-run" in sys.argv:
    sys.exit(0)

# ------------------------------------------------------------------- writing


def write(path, text):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8", newline="") as fh:
        fh.write(text)


for d, fn, title, s, e in parts:
    header = (
        "---\n"
        f"source: {SRC_NAME}\n"
        f"part: {d}\n"
        f"section: {title!r}\n"
        f"source_lines: {s}-{e}\n"
        "---\n"
        "\n"
    )
    write(os.path.join(OUT, d, fn), header + body(s, e))

# ------------------------------------------------------- generated indexes
idx = ["# 6. Operation Commands - command index", "",
       f"374 commands extracted from `{SRC_NAME}` lines {CH6_START}-{CH6_END}.",
       "Chapter opening text: [000-overview.md](000-overview.md).", "",
       "| # | Command | File | Source lines |", "| --- | --- | --- | --- |"]
for c in commands:
    fn = f"{c['num']:03d}-{slug(c['name'])}.md"
    idx.append(f"| 6.{c['num']} | `{c['name']}` | [{fn}]({fn}) | "
               f"{c['start']}-{c['end']} |")
write(os.path.join(OUT, "06-operation-commands", "README.md"), "\n".join(idx) + "\n")

man = ["# 1830 GX Release 9.1 CLI Reference Guide - split by chapter", "",
       f"Generated from `../{SRC_NAME}` (1900-003486 Revision 001, July 2026),",
       f"{NLINES - 1} lines. Each file below is a byte-exact slice of the source "
       "with a",
       "YAML frontmatter header prepended; concatenating every slice in the order",
       "listed reproduces the original document exactly.", "",
       "**This whole directory is generated. Do not edit these files by hand:**",
       "the next build overwrites them. The toolchain that produces them lives in",
       f"`../tools/` - see [tools/README.md](../tools/README.md). Rebuild with",
       "`python ../tools/build_all.py`.", "",
       "Internal anchor links (`](#...)`) are left exactly as they appear in the",
       "source and therefore do not resolve across files. Image references point at",
       "`images/figure-p*.png`, which does not exist alongside the source either.", "",
       "## Start here", "",
       "| File | Use it for |", "| --- | --- |",
       "| [INDEX.md](INDEX.md) | Master command index: 395 commands in 16 "
       "functional domains, with summaries and file paths |",
       "| [index/README.md](index/README.md) | Retrieval guide: which index file "
       "answers which kind of question |",
       "| [NAVIGATION.md](NAVIGATION.md) | The document's own table of contents, "
       "with links rewritten to the split files |", "",
       "## Parts", "",
       "| Part | File | Section | Source lines |", "| --- | --- | --- | --- |"]
for d, fn, title, s, e in parts:
    if d == "06-operation-commands" and fn != "000-overview.md":
        continue
    man.append(f"| `{d}` | [{fn}]({d}/{fn}) | {title} | {s}-{e} |")
man += ["", "Chapter 6 continues with one file per command; see",
        "[06-operation-commands/README.md](06-operation-commands/README.md)."]
write(os.path.join(OUT, "README.md"), "\n".join(man) + "\n")

# ------------------------------------------------------------- verification
rebuilt = []
for d, fn, title, s, e in parts:
    with open(os.path.join(OUT, d, fn), "r", encoding="utf-8", newline="") as fh:
        txt = fh.read()
    assert txt.startswith("---\n")
    end_of_fm = txt.index("\n---\n", 3) + len("\n---\n")
    assert txt[end_of_fm] == "\n"
    rebuilt.append(txt[end_of_fm + 1:])

joined = "\n".join(rebuilt)
print(f"rebuilt: {len(joined.split(chr(10)))} split-lines, {len(joined)} chars")
if joined == raw:
    print("VERIFY OK: reassembled output is byte-identical to the source")
else:
    print("VERIFY FAILED")
    for i, (a, b) in enumerate(zip(joined.split("\n"), lines), start=1):
        if a != b:
            print(f"first difference at line {i}\n  out: {a!r}\n  src: {b!r}")
            break
    sys.exit(1)

markers = raw.count("<!-- page ")
assert joined.count("<!-- page ") == markers
print(f"page markers preserved: {markers}")
