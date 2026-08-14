"""Mechanical extraction layer for the GX CLI Reference index.

Produces:
  index/_commands.raw.jsonl   structured record per command (no curated fields)
  index/pages.tsv             source page number -> file
  index/tables.tsv            table/figure number -> file + caption
Verifies the derived page numbers against the document's own TOC page numbers.
"""
import json
import os
import re

import gxpaths

BASE, SRC, IDX = gxpaths.DOCS, gxpaths.SOURCE, gxpaths.INDEX_DIR
gxpaths.require_source()
gxpaths.require_docs()
os.makedirs(IDX, exist_ok=True)

with open(SRC, "r", encoding="utf-8", newline="") as f:
    lines = f.read().split("\n")

# ----------------------------------------------------------- part line ranges
parts = gxpaths.part_files()
assert len(parts) == gxpaths.EXPECTED_PARTS, len(parts)


def part_of(lineno):
    for rel, s, e in parts:
        if s <= lineno <= e:
            return rel
    raise KeyError(lineno)


# ------------------------------------------------------------------ page map
# "<!-- page N -->" marks the START of page N; content between marker N and
# marker N+1 is on page N.
marks = []                                    # (lineno, page)
for i, ln in enumerate(lines, start=1):
    m = re.match(r"^<!-- page (\d+) -->$", ln.strip())
    if m:
        marks.append((i, int(m.group(1))))
assert len(marks) == gxpaths.EXPECTED_PAGE_MARKERS


def page_at(lineno):
    """Page number the given source line sits on."""
    lo, hi, cur = 0, len(marks) - 1, 1
    while lo <= hi:
        mid = (lo + hi) // 2
        if marks[mid][0] <= lineno:
            cur = marks[mid][1]
            lo = mid + 1
        else:
            hi = mid - 1
    return cur


# Only 652 of the document's 1358 pages carry a marker, so a page->file map
# built from markers alone leaves cited pages unresolvable. Every page in the
# span [marker_page, next_marker_page) lies within that marker's line range, so
# gap pages are filled in and flagged as inferred; where a span crosses a
# section boundary, the extra files are listed too.
def first_content_line(after):
    """First non-blank line at or after `after` - a marker is often followed by
    a blank line and then the next section, so mapping the marker's own line
    would file the page under the section it ends rather than the one it opens."""
    i = after
    while i < len(lines) and not lines[i - 1].strip():
        i += 1
    return min(i, len(lines))


page_rows = []
for k, (lineno, page) in enumerate(marks):
    nxt_line = marks[k + 1][0] if k + 1 < len(marks) else len(lines)
    nxt_page = marks[k + 1][1] if k + 1 < len(marks) else page + 1
    start = first_content_line(lineno + 1)
    span = sorted({part_of(l) for l in range(start, min(nxt_line, len(lines)) + 1)}
                  or {part_of(start)},
                  key=lambda rel: next(s for r, s, e in parts if r == rel))
    for p in range(page, nxt_page):
        page_rows.append((p, span[0], "exact" if p == page else "inferred",
                          lineno, ";".join(span[1:])))
with open(os.path.join(IDX, "pages.tsv"), "w", encoding="utf-8", newline="") as fh:
    fh.write("page\tfile\tprecision\tmarker_line\talso_spans\n")
    for p, rel, prec, lineno, extra in page_rows:
        fh.write(f"{p}\t{rel}\t{prec}\t{lineno}\t{extra}\n")
print(f"page map rows: {len(page_rows)} "
      f"(exact {sum(1 for r in page_rows if r[2] == 'exact')}, "
      f"inferred {sum(1 for r in page_rows if r[2] == 'inferred')})")

# --------------------------------------------------------- tables and figures
tab_re = re.compile(r"^\*\*(Table|Figure) (\d+):\s*(.*?)\*\*\s*$")
seen = {}
for i, ln in enumerate(lines, start=1):
    m = tab_re.match(ln)
    if not m:
        continue
    kind, num, cap = m.group(1), int(m.group(2)), m.group(3)
    cap = re.sub(r"\s*\(continued\)\s*$", "", cap)
    key = (kind, num)
    if key not in seen:                       # first occurrence wins
        seen[key] = (cap, part_of(i), page_at(i), i)
with open(os.path.join(IDX, "tables.tsv"), "w", encoding="utf-8", newline="") as fh:
    fh.write("kind\tnumber\tcaption\tfile\tpage\tsource_line\n")
    for (kind, num), (cap, rel, page, i) in sorted(seen.items()):
        fh.write(f"{kind}\t{num}\t{cap}\t{rel}\t{page}\t{i}\n")
print(f"tables/figures indexed: {len(seen)}")

# ---------------------------------------------------------- command inventory
CATS = [
    ("06-operation-commands", r"^## 6\.(\d+)\.\s+(.*)$", "operation"),
    ("04-navigation-and-display-commands", r"^## 4\.(\d+)\.\s+(.*)$", "navigation"),
    ("05-piped-commands", r"^## 5\.(\d+)\.\s+(.*)$", "piped"),
]
cmds = []
for i, ln in enumerate(lines, start=1):
    rel = part_of(i)
    for prefix, pat, cat in CATS:
        if not rel.startswith(prefix):
            continue
        m = re.match(pat, ln)
        if m:
            cmds.append({"n": int(m.group(1)), "name": m.group(2).strip(),
                         "cat": cat, "start": i, "file": rel,
                         "chapter": prefix.split("-")[0]})
# section end = next command start in same chapter, else end of its part
by_cat = {}
for c in cmds:
    by_cat.setdefault(c["cat"], []).append(c)
for cat, lst in by_cat.items():
    lst.sort(key=lambda c: c["start"])
    for a, b in zip(lst, lst[1:]):
        a["end"] = b["start"] - 1
    last = lst[-1]
    last["end"] = next(e for rel, s, e in parts if rel == last["file"])
print({k: len(v) for k, v in by_cat.items()})

# auxiliary commands live in tables only, not headings
AUX = [("tic", "Starts a timer for the typed command."),
       ("toc", "Displays the elapsed time since the timer was started."),
       ("help", "Displays help for a command, container, or attribute."),
       ("?", "Contextual help: displays what can be typed at the current prompt.")]

# ----------------------------------------------------------- record extraction
NOTE_RE = re.compile(r"^\*\*(Note|Tip|Warning|Caution|Attention|Important)")


def block(text, heading):
    m = re.search(rf"^#### {heading}\s*\n(.*?)(?=^#### |\Z)", text, re.S | re.M)
    return m.group(1) if m else ""


def summarize(desc):
    for para in [p.strip() for p in desc.split("\n") if p.strip()]:
        if NOTE_RE.match(para) or para.startswith(("|", "```", "- ", "**Table")):
            continue
        para = re.sub(r"\\([<>*])", r"\1", para)
        para = re.sub(r"\s+", " ", para)
        # drop a run-in bold sub-heading, e.g. "**clear pm** The `clear pm` ..."
        para = re.sub(r"^\*\*[^*]{1,45}\*\*\s+(?=\S)", "", para).strip()
        if not para:
            continue
        sent = re.split(r"(?<=[a-z0-9\)\]`])\.\s+(?=[A-Z])", para)[0].rstrip(".")
        return (sent[:300] + "...") if len(sent) > 300 else sent
    return ""


SEP_RE = re.compile(r"^\|[\s\-|]+\|$")


def parse_tables(chunk):
    """Return [(header, rows)] for EVERY pipe table in chunk.

    Sections routinely carry more than one table (`show` has three) and tables
    are interrupted by page markers and repeated captions, so a parser that
    stops at the first blank line loses most of the content.
    """
    tables, hdr, rows, prev_pipe = [], None, [], False
    for ln in chunk.split("\n"):
        if ln.startswith("|"):
            cells = [c.strip() for c in ln.strip().strip("|").split("|")]
            if SEP_RE.match(ln.strip()) or set("".join(cells)) <= set("- "):
                prev_pipe = True
                continue
            if hdr is None:
                hdr = cells
            elif not prev_pipe and rows:
                # a pipe row after a non-pipe gap that repeats the header ends
                # the previous table only if the header actually differs
                if cells != hdr and _is_header(cells):
                    tables.append((hdr, rows))
                    hdr, rows = cells, []
                elif cells == hdr:
                    pass                       # repeated header of a continued table
                else:
                    rows.append(dict(zip(hdr, cells + [""] * (len(hdr) - len(cells)))))
            else:
                if cells == hdr:
                    pass
                else:
                    rows.append(dict(zip(hdr, cells + [""] * (len(hdr) - len(cells)))))
            prev_pipe = True
        else:
            prev_pipe = False
    if hdr is not None:
        tables.append((hdr, rows))
    return tables


HEADER_WORDS = {"parameter", "attribute", "option", "section", "command", "value",
                "values", "description", "default", "used in", "conditions",
                "sub-command", "keyword", "name", "type"}


def _is_header(cells):
    lc = [c.lower().strip() for c in cells]
    return sum(1 for c in lc if c in HEADER_WORDS) >= max(2, len(cells) - 1)


def col(row, *names):
    """Fetch a cell by any of several column names, else by position."""
    for n in names:
        for k, v in row.items():
            if k and k.strip().lower() == n:
                return v
    return ""


records = []
for c in cmds:
    body = "\n".join(lines[c["start"] - 1:c["end"]])
    desc = block(body, "Command Description")
    syn_m = re.search(r"^#### Command Syntax\s*\n(.*?)(?=^#### |\Z)", body, re.S | re.M)
    syntax = []
    if syn_m:
        for fence in re.findall(r"```\n(.*?)```", syn_m.group(1), re.S):
            syntax += [s.strip() for s in fence.split("\n") if s.strip()]
    verbs, ents = [], []
    for s in syntax:
        tok = s.split()
        if not tok or not re.match(r"^[a-z][a-z0-9-]*$", tok[0]):
            continue
        if tok[0] not in verbs:
            verbs.append(tok[0])
        if len(tok) > 1 and re.match(r"^[a-zA-Z][\w<>/-]*$", tok[1]) \
                and not tok[1].startswith(("[", "<", "-")):
            if tok[1] not in ents:
                ents.append(tok[1])

    usage_rows = []
    for hdr, rws in parse_tables(block(body, "Command Usage Details")):
        for r in rws:
            vals = list(r.values())
            label = (vals[0] if vals else "").strip()
            value = " ".join(v for v in vals[1:] if v).strip()
            if label or value:
                usage_rows.append({"section": label, "detail": value})

    MODE_RE = re.compile(r"operational\s+mode|candidate\s+configuration", re.I)
    access_raw, access_label = "", ""
    for u in usage_rows:                       # prefer the canonical label
        if u["section"].strip().lower() == "access mode":
            access_raw, access_label = u["detail"], u["section"]
            break
    if not access_raw:                         # some sections use another label
        for u in usage_rows:
            if MODE_RE.search(u["detail"]):
                access_raw, access_label = u["detail"], u["section"]
                break
    al = access_raw.lower()
    access = {"raw": access_raw, "label": access_label,
              "operational": "operational" in al,
              "candidate_config": "candidate" in al,
              "qualified": bool(re.search(r"\(only for", al))}

    params, seen_param = [], set()
    for hdr, rws in parse_tables(block(body, "Command Parameters")):
        for r in rws:
            vals = list(r.values())
            name = (col(r, "parameter", "attribute", "option", "keyword")
                    or (vals[0] if vals else "")).strip()
            # skip only rows that are themselves a repeated header, never rows
            # whose parameter simply happens to be called name/type/value
            if not name or _is_header(vals):
                continue
            # a few rows carry a note glued onto the name by the PDF extraction
            name = re.split(r"\s+i?\s*Note:", name)[0].strip()
            if len(name) > 64:
                name = name[:64].rstrip() + "..."
            desc = col(r, "description") or (vals[1] if len(vals) > 1 else "")
            used = [v.strip() for v in re.split(r"[,/]", col(r, "used in")) if v.strip()]
            key = (name.lower(), desc[:60])
            if key in seen_param:              # continued tables repeat rows
                continue
            seen_param.add(key)
            params.append({"name": name,
                           "description": re.sub(r"\s+", " ", desc)[:400],
                           "values": re.sub(r"\s+", " ", col(r, "values", "value"))[:300],
                           "default": col(r, "default"),
                           "used_in": used})

    tables = sorted({int(m) for m in re.findall(r"^\*\*Table (\d+):", body, re.M)})
    xrefs = sorted({int(m) for m in re.findall(r"\(p\. (\d+)\)", body)})
    pstart, pend = page_at(c["start"]), page_at(c["end"])
    anchor = re.sub(r"[^\w\- ]", "",
                    f"{c['chapter'].lstrip('0')}.{c['n']}. {c['name']}".lower()
                    ).replace(" ", "-")

    # An entity token is a real AID only if it is the command's own container
    # (optionally with an instance placeholder). Tokens like `activate-file`
    # under `activate`, or `candidate` under `validate`, are sub-command
    # keywords and must not be advertised as addressable entities.
    aids = [e for e in ents
            if e == c["name"] or (e.startswith(c["name"]) and "<" in e)]
    keywords = [e for e in ents if e not in aids]

    records.append({
        "name": c["name"],
        "category": c["cat"],
        "section": f"{c['chapter'].lstrip('0')}.{c['n']}",
        "file": c["file"],
        "anchor": anchor,
        "source_lines": [c["start"], c["end"]],
        "pages": [pstart, pend],
        "summary": summarize(desc),
        "kind": "entity" if aids else "action",
        "verbs": verbs,
        "entity_ids": aids,
        "sub_keywords": keywords,
        "access_mode": access,
        "param_count": len(params),
        "parameters": params,
        "usage_details": usage_rows,
        "tables": tables,
        "page_refs": xrefs,
        "has_examples": bool(re.search(r"^#### Examples?", body, re.M)),
        "line_count": c["end"] - c["start"] + 1,
    })

for name, summ in AUX:
    records.append({
        "name": name, "category": "auxiliary", "section": "3",
        "file": "03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md",
        "anchor": "3-auxiliary-and-help-commands",
        "source_lines": [2991, 3037], "pages": [95, 96], "summary": summ,
        "kind": "action", "verbs": [name], "entity_ids": [], "sub_keywords": [],
        "access_mode": {"raw": "", "label": "", "operational": True,
                        "candidate_config": True, "qualified": False},
        "param_count": 0, "parameters": [], "usage_details": [],
        "tables": [35, 36], "page_refs": [77],
        "has_examples": True, "line_count": 47,
    })

records.sort(key=lambda r: (r["category"] != "operation", r["name"]))
with open(os.path.join(gxpaths.BUILD, "commands.raw.jsonl"), "w", encoding="utf-8",
          newline="") as fh:
    for r in records:
        fh.write(json.dumps(r, ensure_ascii=False) + "\n")

print(f"records: {len(records)}")
print("  with syntax:", sum(1 for r in records if r["verbs"]))
print("  with access mode:", sum(1 for r in records if r["access_mode"]["raw"]))
print("  entity kind:", sum(1 for r in records if r["kind"] == "entity"))
print("  no summary:", [r["name"] for r in records if not r["summary"]])

# -------------------------------------------- verify pages against the doc TOC
toc = {}
for ln in lines[36:498]:
    m = re.match(r"^\s*- \[(6\.\d+)\. ([^\]]+)\]\([^)]*\) — p\. (\d+)", ln)
    if m:
        toc[m.group(1)] = int(m.group(3))
mismatch = [(r["section"], r["pages"][0], toc[r["section"]])
            for r in records if r["section"] in toc and r["pages"][0] != toc[r["section"]]]
print(f"TOC page check: {len(toc)} entries, {len(mismatch)} mismatches")
for x in mismatch[:10]:
    print("   ", x)
