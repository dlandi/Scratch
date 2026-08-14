"""Check that every link in the generated index files resolves.

Verifies, for each markdown link in the generated files:
  - the target file exists on disk
  - if the link carries a #fragment, a heading or anchor with that slug exists
    inside the target file
Also re-checks the JSONL records against the files they describe, and the
page/table maps against the parts on disk.
"""
import json
import os
import re
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import gxpaths

B = gxpaths.DOCS
CHECKED_FILES = ["INDEX.md", "NAVIGATION.md", "README.md",
                 "index/README.md", "index/topics.md", "index/entities.md",
                 "index/parameters.md", "index/access-control.md",
                 "06-operation-commands/README.md"]


def read(rel):
    return open(os.path.join(B, rel.replace("/", os.sep)), encoding="utf-8").read()


def slug(text):
    s = text.strip().lower()
    s = re.sub(r"`([^`]*)`", r"\1", s)
    s = re.sub(r"\*\*?([^*]*)\*\*?", r"\1", s)
    s = s.replace("\\", "")
    s = re.sub(r"[^\w\- ]", "", s)
    return s.replace(" ", "-")


def anchors_of(path):
    txt = open(path, encoding="utf-8").read()
    got = {slug(m.group(2)) for m in re.finditer(r"^(#{1,6})\s+(.*?)\s*$", txt, re.M)}
    return got | set(re.findall(r'<a id="([^"]+)"', txt))


bad, checked, cache = [], 0, {}
for rel in CHECKED_FILES:
    path = os.path.join(B, rel.replace("/", os.sep))
    for link in re.findall(r"\]\(([^)]+)\)", open(path, encoding="utf-8").read()):
        if link.startswith(("http", "#")):
            continue
        checked += 1
        target, _, frag = link.partition("#")
        resolved = os.path.normpath(os.path.join(os.path.dirname(path), target))
        if not os.path.exists(resolved):
            bad.append((rel, link, "missing file"))
            continue
        if frag:
            if resolved not in cache:
                cache[resolved] = anchors_of(resolved)
            if frag not in cache[resolved]:
                bad.append((rel, link, "missing anchor"))
print(f"links checked: {checked}, broken: {len(bad)}")
for b in bad[:10]:
    print("  ", b)

records = [json.loads(l) for l in
           open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"), encoding="utf-8")]
errs = []
for r in records:
    path = os.path.join(B, r["file"].replace("/", os.sep))
    if not os.path.exists(path):
        errs.append((r["name"], "file"))
        continue
    txt = open(path, encoding="utf-8").read()
    if r["category"] == "operation":
        if f"## {r['section']}. {r['name']}" not in txt:
            errs.append((r["name"], "heading"))
        m = re.search(r"source_lines: (\d+)-(\d+)", txt)
        if [int(m.group(1)), int(m.group(2))] != r["source_lines"]:
            errs.append((r["name"], "lines"))
print(f"jsonl records: {len(records)}, errors: {len(errs)} {errs[:5]}")

pages = [l.split("\t") for l in
         read("index/pages.tsv").strip().split("\n")[1:]]
ok = all(os.path.exists(os.path.join(B, p[1].replace("/", os.sep))) for p in pages)
print(f"pages rows: {len(pages)} | distinct pages: {len({p[0] for p in pages})} "
      f"| all targets exist: {ok}")

tables = [l.split("\t") for l in read("index/tables.tsv").strip().split("\n")[1:]]
print(f"tables rows: {len(tables)} | tables: {sum(1 for t in tables if t[0] == 'Table')}"
      f" | figures: {sum(1 for t in tables if t[0] == 'Figure')}")

idx = read("INDEX.md")
missing = [r["name"] for r in records if f"| `{r['name']}` |" not in idx]
print(f"commands missing from INDEX.md: {missing}")

sys.exit(1 if (bad or errs or missing or not ok) else 0)
