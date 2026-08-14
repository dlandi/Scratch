"""Shared path resolution for the GX documentation toolchain.

Every path in the toolchain derives from this module, which locates itself
relative to the repository rather than hard-coding a drive letter. Move the GX
folder anywhere and the toolchain still works.

Layout assumed:

    GX/
      R9_1_GX_CLI_Command_Reference_Guide_001P4.md   <- SOURCE (never written)
      R9_1_GX_CLI_Reference/                         <- DOCS (fully generated)
        index/                                       <- INDEX_DIR
      tools/                                         <- this file lives here
        build/                                       <- BUILD (intermediates)
"""
import os
import sys

TOOLS = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(TOOLS)

SOURCE = os.path.join(ROOT, "R9_1_GX_CLI_Command_Reference_Guide_001P4.md")
SOURCE_NAME = os.path.basename(SOURCE)

DOCS = os.path.join(ROOT, "R9_1_GX_CLI_Reference")
INDEX_DIR = os.path.join(DOCS, "index")
BUILD = os.path.join(TOOLS, "build")

# Generated files that live at the root of DOCS and are NOT slices of the
# source, so every walker over DOCS must skip them.
GENERATED_AT_DOCS_ROOT = {"README.md", "NAVIGATION.md", "INDEX.md"}

# Invariants of the source document. If the source is replaced by a new
# revision these will trip, which is intentional: the boundaries below were
# derived by reading the document and must be re-derived, not guessed.
EXPECTED_PARTS = 386          # split files (content slices)
EXPECTED_PAGE_MARKERS = 652   # <!-- page N --> comments
EXPECTED_COMMANDS = 374       # "## 6.N. name" sections


def docs_path(*parts):
    return os.path.join(DOCS, *parts)


def read_source():
    """Source document as a list of lines, split on \\n with no normalization."""
    with open(SOURCE, "r", encoding="utf-8", newline="") as f:
        return f.read().split("\n")


def require_source():
    if not os.path.exists(SOURCE):
        sys.exit(f"source document not found: {SOURCE}")


def require_docs():
    if not os.path.isdir(DOCS):
        sys.exit(f"split docs not found: {DOCS}\nRun step1_split.py first.")


def part_files():
    """(relative path, first source line, last source line) for every content
    slice, ordered by position in the source. Reads the ranges back out of each
    file's frontmatter so the split stays the single source of truth."""
    import re
    out = []
    for root, _dirs, files in os.walk(DOCS):
        for fn in sorted(files):
            rel = os.path.relpath(os.path.join(root, fn), DOCS).replace("\\", "/")
            if not fn.endswith(".md") or fn == "README.md":
                continue
            if rel in GENERATED_AT_DOCS_ROOT or rel.startswith("index/"):
                continue
            with open(os.path.join(root, fn), encoding="utf-8") as fh:
                head = fh.read(400)
            m = re.search(r"^source_lines: (\d+)-(\d+)$", head, re.M)
            if not m:
                sys.exit(f"no source_lines frontmatter in {rel}")
            out.append((rel, int(m.group(1)), int(m.group(2))))
    out.sort(key=lambda p: p[1])
    return out


os.makedirs(BUILD, exist_ok=True)
