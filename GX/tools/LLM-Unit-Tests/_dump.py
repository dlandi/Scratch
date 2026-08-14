"""Authoring aid: dump the substance of a domain's command sections compactly.

    python _dump.py <domain> [start] [count]

Prints, per command: description, syntax, access mode, parameters that carry a
real default or enumerated values, notes and tips, and the first example. This
is the material a test is written from; quotes taken from this output are
verbatim, which matters because run_tests.py rejects any quote it cannot find
in the file.
"""
import json
import os
import re
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.dirname(HERE))
import gxpaths                                                    # noqa: E402

sys.stdout.reconfigure(encoding='utf-8', errors='replace')

domain = sys.argv[1]
start = int(sys.argv[2]) if len(sys.argv) > 2 else 0
count = int(sys.argv[3]) if len(sys.argv) > 3 else 999

recs = [json.loads(l) for l in
        open(os.path.join(gxpaths.INDEX_DIR, "commands.jsonl"), encoding="utf-8")]
sel = [r for r in recs if r["domain"] == domain and r["category"] == "operation"]
sel.sort(key=lambda r: r["file"])
print(f"### {domain}: {len(sel)} commands, showing {start}..{start + count}\n")

for r in sel[start:start + count]:
    body = open(os.path.join(gxpaths.DOCS, r["file"].replace("/", os.sep)),
                encoding="utf-8").read()
    print(f"==== {r['name']}   [{r['file'].split('/')[-1]}]  mode={r['access_mode']['raw'][:40]}")
    d = re.search(r"^#### Command Description\s*\n(.*?)(?=^#### |\Z)", body, re.S | re.M)
    if d:
        for ln in [x.strip() for x in d.group(1).split("\n") if x.strip()][:4]:
            print("  D:", ln[:330])
    s = re.search(r"^#### Command Syntax\s*\n(.*?)(?=^#### |\Z)", body, re.S | re.M)
    if s:
        for ln in [x for x in re.findall(r"```\n(.*?)```", s.group(1), re.S)[:1]
                   for x in x.split("\n") if x.strip()][:5]:
            print("  S:", ln[:210])
    for p in r["parameters"]:
        interesting = (p["default"] and p["default"] not in ("n/a", "na", "-", "")) \
            or ("," in p["values"] or "•" in p["values"] or "range" in p["values"].lower())
        if interesting:
            print(f"  P: {p['name']} = [{p['values'][:120]}] default=[{p['default'][:60]}]")
    for ln in re.findall(r"^\*\*(?:Note|Tip|Warning):\*\*(.*)$", body, re.M)[:3]:
        print("  N:", ln.strip()[:300])
    ex = re.search(r"^#### Examples?\s*\n(.*?)(?=^#### |\Z)", body, re.S | re.M)
    if ex:
        for ln in [x for x in re.findall(r"```\n(.*?)```", ex.group(1), re.S)[:1]
                   for x in x.split("\n") if x.strip()][:2]:
            print("  X:", ln[:150])
    print()
