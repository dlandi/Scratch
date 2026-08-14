"""Extract the user-group privilege tables from chapter 2 of the split docs.

Chapter 2 (section 2.2.10) carries three tables that exist nowhere else in the
document:

    Table 32  the seven user groups and what each may do
    Table 33  data-model objects -> groups with write / read access
    Table 34  CLI commands -> groups with execute access, per sub-command

Table 34's first column is only filled on a command's first row; the rows that
follow belong to the same command and are forward-filled here.

Output: tools/build/privileges.json, consumed by step4_build_index.py.
"""
import json
import os
import re

import gxpaths

gxpaths.require_docs()
CH2 = os.path.join(gxpaths.DOCS, "02-using-the-cli", "02-using-the-cli.md")
text = open(CH2, encoding="utf-8").read()


def table(number):
    """Rows of the numbered table as lists of cells, header included."""
    m = re.search(rf"^\*\*Table {number}: [^*]*\*\*\s*\n(.*?)(?=^\*\*Table |\Z)",
                  text, re.S | re.M)
    if not m:
        raise SystemExit(f"Table {number} not found in {CH2}")
    rows = []
    for ln in m.group(1).split("\n"):
        if not ln.startswith("|"):
            continue
        cells = [c.strip() for c in ln.strip().strip("|").split("|")]
        if set("".join(cells)) <= set("- "):        # separator row
            continue
        rows.append(cells)
    return rows


t33, t34 = table(33), table(34)

commands, current = {}, None
for row in t34[1:]:
    if row[0]:
        current = row[0]
    if not current:
        continue
    commands.setdefault(current, []).append({
        "sub": row[1] if len(row) > 1 else "",
        "cond": row[2] if len(row) > 2 else "",
        "groups": row[3] if len(row) > 3 else "",
        "notes": row[4] if len(row) > 4 else "",
    })

objects = {r[0]: {"write": r[1], "read": r[2]}
           for r in t33[1:] if r[0] and r[0] != "Object"}

out = os.path.join(gxpaths.BUILD, "privileges.json")
json.dump({"commands": commands, "objects": objects},
          open(out, "w", encoding="utf-8"), indent=1)

print(f"Table 33: {len(objects)} objects")
print(f"Table 34: {len(commands)} commands, "
      f"{sum(len(v) for v in commands.values())} access rows")
print(f"wrote {os.path.relpath(out, gxpaths.ROOT)}")
