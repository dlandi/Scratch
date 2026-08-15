"""Full consistency audit of the generated index set."""
import json
import os
import sys
import re

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import gxpaths
B = gxpaths.DOCS
S = gxpaths.SOURCE
src = open(S, encoding="utf-8").read().split("\n")
R = lambda p: open(os.path.join(B, p.replace("/", os.sep)), encoding="utf-8").read()
rs = [json.loads(l) for l in open(os.path.join(B, "index", "commands.jsonl"),
                                  encoding="utf-8")]
by = {r["name"]: r for r in rs}
fails = []


def check(cond, label, detail=""):
    print(f"  [{'PASS' if cond else 'FAIL'}] {label}" + (f" - {detail}" if detail else ""))
    if not cond:
        fails.append(label)


print("== 1. page map ==")
pg = {}
for l in R("index/pages.tsv").strip().split("\n")[1:]:
    c = l.split("\t")
    pg[int(c[0])] = c[1:]
cited = sorted({int(m) for m in re.findall(r"\(p\. (\d+)\)", "\n".join(src))})
check(len(pg) == 1358 and min(pg) == 1, "all 1358 pages mapped", f"{len(pg)} rows")
check(all(p in pg for p in cited), "every cited page resolvable",
      f"{sum(1 for p in cited if p in pg)}/{len(cited)}")
wrong = []
for p in cited:
    f = pg[p][0]
    cand = [x for x in rs if x["file"] == f]
    if cand and not any(x["pages"][0] <= p <= x["pages"][1] for x in cand):
        wrong.append((p, f))
check(not wrong, "cited page lands in a file whose page span contains it",
      f"{len(wrong)} off: {wrong[:4]}")

print("== 2. tables/figures ==")
tl = {int(l.split("\t")[1]) for l in R("index/tables.tsv").strip().split("\n")[1:]
      if l.startswith("Table")}
lot = {int(m) for m in re.findall(r"^- Table (\d+):", R("00-front-matter/03-list-of-tables.md"), re.M)}
check(tl == lot, "indexed tables match the List of Tables", f"{len(tl)} vs {len(lot)}")
for num in [93, 32, 33, 34]:
    row = [l for l in R("index/tables.tsv").split("\n") if l.startswith(f"Table\t{num}\t")]
    tgt = row[0].split("\t")[3] if row else None
    ok = tgt and f"**Table {num}:" in R(tgt)
    check(bool(ok), f"Table {num} caption present in its target file", tgt or "missing")

print("== 3. per-command field accuracy (all 374 operation commands) ==")
bad_head = bad_syn = bad_tab = bad_ex = bad_page = 0
for r in rs:
    if r["category"] != "operation":
        continue
    t = R(r["file"])
    body = t.split("\n", 6)[-1]
    if f"## {r['section']}. {r['name']}" not in t:
        bad_head += 1
    syn = re.search(r"^#### Command Syntax\s*\n(.*?)(?=^#### |\Z)", body, re.S | re.M)
    for v in r["verbs"]:
        if syn and not re.search(rf"^{re.escape(v)}\b", syn.group(1), re.M):
            bad_syn += 1
            break
    if set(r["tables"]) != {int(m) for m in re.findall(r"^\*\*Table (\d+):", body, re.M)}:
        bad_tab += 1
    if r["has_examples"] != bool(re.search(r"^#### Examples?", body, re.M)):
        bad_ex += 1
    if set(r["page_refs"]) != {int(m) for m in re.findall(r"\(p\. (\d+)\)", body)}:
        bad_page += 1
check(bad_head == 0, "section heading matches record", f"{bad_head} bad")
check(bad_syn == 0, "every recorded verb appears at the start of a syntax line", f"{bad_syn} bad")
check(bad_tab == 0, "table numbers match the section body", f"{bad_tab} bad")
check(bad_ex == 0, "has_examples matches the body", f"{bad_ex} bad")
check(bad_page == 0, "page_refs match the body", f"{bad_page} bad")

print("== 3b. summaries come from the Command Description block ==")
bad_sum = []
for r in rs:
    if r["category"] != "operation" or not r["summary"]:
        continue
    m = re.search(r"^#### Command Description\s*\n(.*?)(?=^#### |\Z)",
                  R(r["file"]), re.S | re.M)
    # normalize both sides the way summarize() does: collapse whitespace, drop
    # the backslash escapes the conversion put in front of < > and *
    norm = lambda s: re.sub(r"\s+", " ", s.replace("\\", ""))
    blk = norm(m.group(1)) if m else ""
    probe = norm(r["summary"].rstrip("."))[:60]
    if probe and probe not in blk:
        bad_sum.append(r["name"])
check(not bad_sum, "every summary is text from its own description block",
      f"{len(bad_sum)} wrong: {bad_sum[:6]}")

print("== 4. parameters ==")
short = over = 0
for r in rs:
    if r["category"] != "operation":
        continue
    m = re.search(r"^#### Command Parameters\s*\n(.*?)(?=^#### |\Z)", R(r["file"]), re.S | re.M)
    if not m:
        continue
    rows = [l for l in m.group(1).split("\n") if l.startswith("| ")]
    data = [l for l in rows
            if not set(l.replace("|", "").replace(" ", "")) <= {"-"}
            and not re.match(r"\| *(Parameter|Attribute|Option|Keyword) *\|", l, re.I)]
    if len(data) - r["param_count"] > 3:
        short += 1
    if r["param_count"] - len(data) > 3:
        over += 1
check(short == 0, "no command loses >3 parameter rows to the parser", f"{short}")
check(over == 0, "no command invents >3 parameter rows", f"{over}")
names = [p["name"] for r in rs for p in r["parameters"]]
check(all(n.strip() for n in names), "no blank parameter names")
check(max(len(n) for n in names) <= 67, "no runaway parameter names",
      f"max len {max(len(n) for n in names)}")
pmrows = re.findall(r"^\| `([^`]+)` \|", R("index/parameters.md"), re.M)
check(len(pmrows) == len(set(pmrows)), "parameters.md has no duplicate rows")
check(len(set(pmrows)) == len({n.strip().lower() for n in names}),
      "parameters.md covers every distinct name",
      f"{len(set(pmrows))} vs {len({n.strip().lower() for n in names})}")

print("== 5. entities ==")
ents = [r for r in rs if r["entity_ids"]]
for r in ents:
    pass
bad_aid = [r["name"] for r in ents
           if not (r["entity_ids"][0] == r["name"]
                   or (r["entity_ids"][0].startswith(r["name"]) and "<" in r["entity_ids"][0]))]
check(not bad_aid, "every advertised AID belongs to its command", str(bad_aid[:5]))
ent_rows = len(re.findall(r"^\| `[^`]+` \| `[^`]+` \| `[^`]+` \|", R("index/entities.md"), re.M))
check(ent_rows == len(ents), "entities.md row count matches records",
      f"{ent_rows} vs {len(ents)}")

print("== 6. cross-file consistency ==")
idx = R("INDEX.md")
check(all(f"| `{r['name']}` |" in idx for r in rs), "every command appears in INDEX.md")
for d, n in re.findall(r"\]\(#([a-z0-9-]+)\) - (\d+) commands", idx):
    sect = idx.split(f'<a id="{d}"></a>')[1].split("\n## ")[0]
    if len(re.findall(r"^\| `", sect, re.M)) != int(n):
        fails.append(f"domain count {d}")
check(not [f for f in fails if f.startswith("domain count")], "domain counts match their tables")
tp = R("index/topics.md")
tcmds = set(re.findall(r"^\| `([^`]+)` \|", tp, re.M))
check(tcmds <= set(by), "every topic row names a real command", str(sorted(tcmds - set(by))[:5]))
rd = R("index/README.md")
tot = sum(len(r["parameters"]) for r in rs)
dist = len({p["name"].strip().lower() for r in rs for p in r["parameters"]})
m = re.search(r"- (\d+) commands, (\d+) parameter rows, (\d+) distinct parameter names", rd)
check(m.groups() == (str(len(rs)), str(tot), str(dist)), "README coverage numbers current",
      f"{m.groups()} vs {(len(rs), tot, dist)}")
m2 = re.search(r"- (\d+) commands address a named entity; (\d+) are action", rd)
check(m2.groups() == (str(len(ents)), str(len(rs) - len(ents))), "README entity split current",
      f"{m2.groups()} vs {(len(ents), len(rs) - len(ents))}")
m3 = re.search(r"median (\d+) lines", rd)
ops = sorted(r["line_count"] for r in rs if r["category"] == "operation")
check(m3.group(1) == str(ops[len(ops) // 2]), "README median line count current")
m5 = re.search(r"list of (\d+) acronyms", rd)
acr = R("99-acronyms/99-acronyms.md")
n_acr = len([1 for ln in acr.split("\n")
             if (mm := re.match(r"^\|\s*([^|]+?)\s*\|\s*([^|]+?)\s*\|$", ln))
             and mm.group(1) not in ("Acronym", "---") and mm.group(2) != "---"])
check(m5 and m5.group(1) == str(n_acr), "README acronym count current",
      f"{m5.group(1) if m5 else None} vs {n_acr}")
ac = R("index/access-control.md")
m4 = re.search(r"(\d+) commands have an explicit execution-access entry", ac)
n_exec = len({r["name"] for r in rs if "execute" in r["user_groups"]})
check(m4.group(1) == str(n_exec), "access-control command count current",
      f"{m4.group(1)} vs {n_exec}")

print("== 7. markdown table integrity ==")
for f, cols in [("INDEX.md", 5), ("index/topics.md", 3), ("index/parameters.md", 3),
                ("index/entities.md", None)]:
    bad = 0
    for ln in R(f).split("\n"):
        if ln.startswith("| ") and not re.match(r"^\|[\s\-|]+\|$", ln):
            n = len(re.findall(r"(?<!\\)\|", ln)) - 1
            if cols and n != cols:
                bad += 1
    check(bad == 0, f"{f} column counts consistent", f"{bad} rows off")

print()
print("FAILURES:", len(fails), fails if fails else "none")
sys.exit(1 if fails else 0)
