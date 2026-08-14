"""Generate NAVIGATION.md: the source table of contents with every internal
anchor rewritten to a relative path into the split files.

Reads the part boundaries back out of the generated files' frontmatter, so it
stays in sync with split_guide.py without duplicating the range table.
"""
import os
import re
from collections import Counter

import gxpaths

SRC, OUT, SRC_NAME = gxpaths.SOURCE, gxpaths.DOCS, gxpaths.SOURCE_NAME
GENERATED = gxpaths.GENERATED_AT_DOCS_ROOT
gxpaths.require_docs()

with open(SRC, "r", encoding="utf-8", newline="") as f:
    lines = f.read().split("\n")

# ------------------------------------------- part ranges, from the split files
parts = gxpaths.part_files()
assert len(parts) == gxpaths.EXPECTED_PARTS, len(parts)


def part_of(lineno):
    for rel, s, e in parts:
        if s <= lineno <= e:
            return rel
    raise KeyError(lineno)


# ------------------------------------------------- heading slug -> part + text
def gh_slug(text):
    """GitHub-flavored heading slug."""
    s = text.strip().lower()
    s = re.sub(r"`([^`]*)`", r"\1", s)               # strip code spans
    s = re.sub(r"\*\*?([^*]*)\*\*?", r"\1", s)       # strip emphasis
    s = s.replace("\\", "")
    s = re.sub(r"[^\w\- ]", "", s, flags=re.UNICODE)  # drop punctuation
    return s.replace(" ", "-")


anchors = {}                                  # slug -> (relpath, lineno, title)
dupes = Counter()
for i, ln in enumerate(lines, start=1):
    m = re.match(r"^(#{1,6})\s+(.*?)\s*$", ln)
    if not m:
        continue
    base = gh_slug(m.group(2))
    n = dupes[base]
    dupes[base] += 1
    slug = base if n == 0 else f"{base}-{n}"
    anchors.setdefault(slug, (part_of(i), i, m.group(2)))

# ------------------------------------------------------------ rewrite the TOC
# The source TOC lists 2.2.2 but the body has no such heading: that content sits
# inside 2.2.1 (source lines 1916-1979). Point at the containing file, no anchor.
OVERRIDES = {"222-opening-a-cli-session-using-putty":
             "02-using-the-cli/02-using-the-cli.md"}

toc_rel, toc_start, toc_end = next(p for p in parts if p[0].endswith("01-contents.md"))
link_re = re.compile(r"\]\(#([^)]+)\)")
unresolved, rewritten, out_lines = [], 0, []

for i in range(toc_start, toc_end + 1):
    ln = lines[i - 1]

    def repl(m):
        global rewritten
        slug = m.group(1)
        hit = anchors.get(slug)
        if not hit:
            unresolved.append((i, slug))
            if slug in OVERRIDES:
                rewritten += 1
                return f"]({OVERRIDES[slug]})"
            return m.group(0)
        rewritten += 1
        return f"]({hit[0]}#{slug})"

    out_lines.append(link_re.sub(repl, ln))

# drop the source "## Contents" heading; NAVIGATION.md supplies its own title
body = "\n".join(out_lines).strip("\n")
body = re.sub(r"^## Contents\s*\n+", "", body)

header = f"""# Navigation - 1830 GX Release 9.1 CLI Reference Guide

Navigable table of contents for the split document. This is a **generated**
file: it reproduces the `## Contents` section of `{SRC_NAME}`
(source lines {toc_start}-{toc_end}) with every internal anchor rewritten to a
relative path into the split files. The content files themselves are unmodified
byte-exact slices of the source, so their own anchors still do not resolve
across files - use this page to navigate instead.

Page numbers (`p. N`) refer to pages of the original 1358-page document and are
kept for cross-reference with the printed guide.

Related: [README.md](README.md) (part manifest with source line ranges),
[06-operation-commands/README.md](06-operation-commands/README.md) (index of all
374 commands). The List of Figures and List of Tables are plain text in the
source with no anchors: see
[00-front-matter/02-list-of-figures.md](00-front-matter/02-list-of-figures.md)
and [00-front-matter/03-list-of-tables.md](00-front-matter/03-list-of-tables.md).

Not listed in the source TOC:
[00-front-matter/00-cover-and-legal.md](00-front-matter/00-cover-and-legal.md)
(title page, legal notice, conformance and warranty statements) and
[00-front-matter/01-contents.md](00-front-matter/01-contents.md) (the verbatim
TOC slice this page is derived from).

**Source defect:** the original Contents lists section *2.2.2. Opening a CLI
Session using PuTTY* (p. 64), but the document body contains no such heading -
that material sits inside 2.2.1 (source lines 1916-1979). That one entry
therefore links to the containing chapter file with no anchor.

## Contents

"""

nav_path = os.path.join(OUT, "NAVIGATION.md")
with open(nav_path, "w", encoding="utf-8", newline="") as fh:
    fh.write(header + body + "\n")

print(f"headings indexed: {len(anchors)}")
print(f"links rewritten:  {rewritten}")
print(f"unresolved:       {len(unresolved)}")
for i, slug in unresolved[:20]:
    print(f"  line {i}: #{slug}")

# ------------------------------------------------------------- spot the targets
check = ["list-of-figures", "1-introduction", "132-1830-gx-g31-managed-objects-"
         "and-addressable-entities", "6291-show", "6292-show-commit", "acronyms"]
for slug in check:
    hit = anchors.get(slug)
    print(f"  {slug!r} -> {hit[0] if hit else 'MISSING'}")

# every rewritten target must exist on disk
targets = set(re.findall(r"\]\(([^)#]+)#", body))
missing = [t for t in targets if not os.path.exists(os.path.join(OUT, t))]
print(f"distinct target files: {len(targets)}, missing on disk: {len(missing)}")
assert not missing, missing
