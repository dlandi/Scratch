"""Assemble the final index set from the raw extraction + curated layer."""
import json
import os
import re
from collections import defaultdict

import curated

import gxpaths

BASE, IDX, SRC_NAME = gxpaths.DOCS, gxpaths.INDEX_DIR, gxpaths.SOURCE_NAME

recs = [json.loads(l) for l in open(os.path.join(gxpaths.BUILD, "commands.raw.jsonl"),
                                    encoding="utf-8")]
by_name = {r["name"]: r for r in recs}
acl = json.load(open(os.path.join(gxpaths.BUILD, "privileges.json"), encoding="utf-8"))

# Table 33 object names that differ from the command name
OBJ_ALIAS = {"certificates": "certificate", "leds": "led",
             "community-string": "snmp-community", "tasks": "task",
             "sw-services": "sw-service", "ssh-authorized-keys": "ssh-authorized-key",
             "secure-applications": "secure-application"}
UNMAPPED_OBJ = ["asap", "command", "services", "static-route"]

# ------------------------------------------------------------------- enrichment
dom_of = {c: d for d, cs in curated.DOMAINS.items() for c in cs}
topics_of = defaultdict(list)
for title, terms, cmds in curated.TOPICS:
    for c in cmds:
        topics_of[c].append(title)

for r in recs:
    r["domain"] = dom_of[r["name"]]
    r["domain_title"] = curated.DOMAIN_TITLES[r["domain"]]
    r["topics"] = topics_of.get(r["name"], [])
    groups = {}
    if r["name"] in acl["commands"]:
        groups["execute"] = acl["commands"][r["name"]]
    obj = r["name"]
    inv = {v: k for k, v in OBJ_ALIAS.items()}
    key = obj if obj in acl["objects"] else inv.get(obj)
    if key:
        groups["object_access"] = acl["objects"][key]
    r["user_groups"] = groups
    ent = r["entity_ids"][0] if r["entity_ids"] else ""
    r["aid_prefix"] = ent.split("-<")[0] if ent else ""

order = list(curated.DOMAIN_TITLES)
recs.sort(key=lambda r: (order.index(r["domain"]), r["name"].lower()))
with open(os.path.join(IDX, "commands.jsonl"), "w", encoding="utf-8", newline="") as f:
    for r in recs:
        f.write(json.dumps(r, ensure_ascii=False) + "\n")


def W(path, text):
    with open(os.path.join(IDX, path) if not path.startswith("..") else
              os.path.join(BASE, path[3:]), "w", encoding="utf-8", newline="") as f:
        f.write(text)


def mode(r):
    a = r["access_mode"]
    if a["operational"] and a["candidate_config"]:
        m = "oper+cand"
    elif a["operational"]:
        m = "oper"
    elif a["candidate_config"]:
        m = "cand"
    else:
        return "-"
    return m + "*" if a.get("qualified") else m


def link(r, up=""):
    """Relative link to a command page. up="../" for files written into index/."""
    tgt = f"{r['file']}#{r['anchor']}" if r["category"] != "operation" else r["file"]
    return up + tgt


def esc(s):
    return s.replace("|", "\\|")


# --------------------------------------------------------------- Tier 1 INDEX.md
n_op = sum(1 for r in recs if r["category"] == "operation")
_ops = sorted(r["line_count"] for r in recs if r["category"] == "operation")
OP_MEDIAN = _ops[len(_ops) // 2]
out = [
    "# GX CLI Reference - master command index",
    "",
    f"Routing index for the 1830 GX Release 9.1 CLI Reference Guide, split from "
    f"`../{SRC_NAME}`.",
    f"**{len(recs)} commands** ({n_op} operation, "
    f"{sum(1 for r in recs if r['category'] == 'navigation')} navigation, "
    f"{sum(1 for r in recs if r['category'] == 'piped')} piped, "
    f"{sum(1 for r in recs if r['category'] == 'auxiliary')} auxiliary) "
    f"grouped into {len(curated.DOMAIN_TITLES)} functional domains.",
    "",
    "## How to use this index",
    "",
    "1. Match the user's query against the **domain headings** below, then the "
    "command rows. If the query uses vocabulary that is not a command name "
    "(\"wavelength\", \"laser shutdown\", \"upgrade\", \"loopback\"), start from "
    "[index/topics.md](index/topics.md) instead.",
    "2. Open the file in the `File` column. Every operation command is a complete, "
    "self-contained page: description, syntax, access mode, full parameter table, "
    f"and usually examples (median {OP_MEDIAN} lines, so reading the whole file "
    "is cheap).",
    "3. For a parameter or attribute name, use [index/parameters.md](index/parameters.md). "
    "For an AID like `card-1-1` or `port-1-1-DCN`, use "
    "[index/entities.md](index/entities.md).",
    "4. Page citations in the text (\"refer to pm (p. 934)\") resolve through "
    "[index/pages.tsv](index/pages.tsv); table and figure numbers through "
    "[index/tables.tsv](index/tables.tsv).",
    "",
    "This file is large; grep it for a command or domain rather than reading it "
    "end to end.",
    "",
    "`Mode` column: `oper` = Operational mode, `cand` = Candidate Configuration "
    f"mode, `-` = the source states no access mode for this command "
    f"({sum(1 for r in recs if mode(r) == '-')} commands: all 10 piped plus "
    f"{sum(1 for r in recs if mode(r) == '-' and r['category'] == 'operation')} "
    "operation commands whose section has no usage table). A trailing `*` means the source qualifies the "
    "mode (for example \"only for show command\") - check the command page. "
    "`Verbs` is empty where the command is not invoked as `<verb> <entity>`.",
    "",
    "## Domains",
    "",
]
for d in order:
    lst = [r for r in recs if r["domain"] == d]
    out.append(f"- [{curated.DOMAIN_TITLES[d]}](#{d}) - {len(lst)} commands")
out.append("")
for d in order:
    lst = [r for r in recs if r["domain"] == d]
    out += [f"## {curated.DOMAIN_TITLES[d]}", "",
            f'<a id="{d}"></a>', "",
            "| Command | What it does | Verbs | Mode | File |",
            "| --- | --- | --- | --- | --- |"]
    for r in lst:
        s = r["summary"]
        s = s[:110].rsplit(" ", 1)[0] + "..." if len(s) > 115 else s
        out.append(f"| `{esc(r['name'])}` | {esc(s)} | {'/'.join(r['verbs'][:5])} | "
                   f"{mode(r)} | [{r['file'].split('/')[-1]}]({link(r)}) |")
    out.append("")
W("../INDEX.md", "\n".join(out))

# ------------------------------------------------------------------- topics.md
grounded = json.load(open("grounding.json", encoding="utf-8")) \
    if os.path.exists("grounding.json") else {}
out = ["# Topic index - query vocabulary to commands", "",
       "Maps the words people actually use to the commands that implement them. "
       "Each entry lists **search terms** (synonyms, acronyms and phrasings that "
       "should trigger it) and the commands to open.",
       "",
       f"{len(curated.TOPICS)} topics covering "
       f"{len({c for _, _, cs in curated.TOPICS for c in cs})} distinct commands. "
       "Associations were assigned by reading each command's description in the "
       "source; they are editorial, not extracted, so treat them as routing hints "
       "and confirm against the command page.",
       ""]
for title, terms, cmds in sorted(curated.TOPICS):
    out += [f"## {title}", "",
            "*Search terms:* " + ", ".join(f"`{t}`" for t in terms), "",
            "| Command | What it does | File |", "| --- | --- | --- |"]
    for c in cmds:
        r = by_name[c]
        s = r["summary"]
        s = s[:100].rsplit(" ", 1)[0] + "..." if len(s) > 105 else s
        out.append(f"| `{esc(c)}` | {esc(s)} | [{r['file'].split('/')[-1]}]({link(r, "../")}) |")
    out.append("")
W("topics.md", "\n".join(out))

# ----------------------------------------------------------------- entities.md
ents = [r for r in recs if r["aid_prefix"]]
ents.sort(key=lambda r: r["aid_prefix"].lower())
out = ["# Entity / AID index", "",
       "Managed entities are addressed by an AID (Access Identifier) such as "
       "`card-1-1`, `port-1-1-DCN` or `odu-1-5-L1-1`. To find the command for an "
       "AID you have in hand, match the **longest AID prefix** below; the rest of "
       "the string is the instance key.",
       "",
       f"{len(ents)} of {len(recs)} commands address a named entity; the remaining "
       f"{len(recs) - len(ents)} are action commands (see the `kind` field in "
       "`commands.jsonl`).",
       "",
       "**What lives under what** is in [Containment](#containment) below, "
       "derived from the AID key paths. The source also describes it in "
       "[1.3.6 Managed Objects (MO) Relationship]"
       "(../01-introduction/01-introduction.md#136-managed-objects-mo-relationship) "
       "and shows it live in the `tree` command output, "
       "[4.5. tree](../04-navigation-and-display-commands/"
       "04-navigation-and-display-commands.md#45-tree).",
       "",
       "| AID prefix | Full pattern | Command | Domain | File |",
       "| --- | --- | --- | --- | --- |"]
for r in ents:
    pat = r["entity_ids"][0]
    out.append(f"| `{esc(r['aid_prefix'])}` | `{esc(pat)}` | `{esc(r['name'])}` | "
               f"{r['domain']} | [{r['file'].split('/')[-1]}]({link(r, "../")}) |")

# Containment, parent -> children, derived from the AID key paths.
#
# This exists because a layer 2 test found the gap it fills. An agent asked what
# hangs underneath `ipsec-spd-entry` could not find out: `141-ipsec-spd-entry.md`
# never says "proposal", and the nesting lives only inside the child's own key
# path. So the relation was discoverable child-to-parent, by reading one full
# pattern, and invisible parent-to-child without scanning all of them.
#
# The immediate parent is the SECOND TO LAST placeholder, not the first.
# `_clusters.py` roots a path at its first placeholder, which is right for
# grouping a family but wrong here: it makes `ipsec-sa-proposal` a child of
# `ikev2-local-instance` and leaves `ipsec-spd-entry` with no children at all.
# Only `-name` and `-id` are stripped, for the reason `_clusters.py` gives:
# `-type` is meaning-bearing and stripping it merges `<card-type>`, which keys
# what a model of card supports, into `<card>`, which keys one installed card.
GENERIC_KEYS = {"name", "index", "id"}


def _immediate_parent(pattern):
    ph = re.findall(r"<([^>]+)>", pattern)
    if len(ph) < 2:
        return None
    parent = re.sub(r"-(name|id)$", "", ph[-2])
    return None if parent in GENERIC_KEYS else parent


children = defaultdict(set)
for r in recs:
    for e in r["entity_ids"] or []:
        parent = _immediate_parent(e)
        if parent:
            children[parent].add(r["name"])

by_name = {r["name"]: r for r in recs}
out += ["", "## Containment", "",
        f"{sum(len(v) for v in children.values())} parent-to-child links across "
        f"{len(children)} parent entities, derived from the AID key paths rather "
        "than stated in the source. Read a row as: an instance of the parent has "
        "these beneath it, so `show <child>-<parent-key>/...` addresses one.",
        "",
        "A parent here need not be a command. `swload-state` and `location` key "
        "real levels that nothing addresses directly. Where the parent is a "
        "command it is linked.", "",
        "| Parent entity | Nested beneath it |", "| --- | --- |"]
for parent in sorted(children):
    kids = ", ".join(f"`{esc(k)}`" for k in sorted(children[parent]))
    rec = by_name.get(parent)
    label = (f"[`{esc(parent)}`]({link(rec, '../')})" if rec else f"`{esc(parent)}`")
    out.append(f"| {label} | {kids} |")

kw = [r for r in recs if r["sub_keywords"]]
out += ["", "## Sub-command keywords", "",
        "These are literal keywords a command takes rather than addressable "
        "entities, so they do not appear above. They are listed because a query "
        "may name the keyword rather than the command.", "",
        "| Keyword | Belongs to | File |", "| --- | --- | --- |"]
for r in sorted(kw, key=lambda x: x["name"]):
    for k in r["sub_keywords"]:
        out.append(f"| `{esc(k)}` | `{esc(r['name'])}` | "
                   f"[{r['file'].split('/')[-1]}]({link(r, '../')}) |")
W("entities.md", "\n".join(out) + "\n")

# --------------------------------------------------------------- parameters.md
pmap = defaultdict(list)
pdesc = {}
for r in recs:
    for p in r["parameters"]:
        nm = p["name"].strip().lower()
        # note: "parameter" is a real parameter name (pm-parameter, pm-threshold),
        # so only structural artifacts are filtered here
        if not nm or nm == "---":
            continue
        pmap[nm].append(r["name"])
        if nm not in pdesc and p["description"]:
            pdesc[nm] = p["description"]
out = ["# Parameter index", "",
       f"{len(pmap)} distinct parameter and attribute names across "
       f"{sum(len(v) for v in pmap.values())} parameter rows. Use this to answer "
       "\"which command sets *X*\". The description shown is the first one the "
       "document gives for that name; other commands may define it differently, so "
       "always confirm on the command page.",
       "",
       "| Parameter | Commands | First description |", "| --- | --- | --- |"]
for nm in sorted(pmap):
    cs = sorted(set(pmap[nm]))
    shown = ", ".join(f"`{c}`" for c in cs[:12])
    if len(cs) > 12:
        shown += f" +{len(cs) - 12} more"
    d = pdesc.get(nm, "")[:160]
    out.append(f"| `{esc(nm)}` | {esc(shown)} | {esc(d)} |")
W("parameters.md", "\n".join(out) + "\n")

# ------------------------------------------------------------ access-control.md
GROUPS = [("MA", "Monitoring Access", "Read-only across equipment and traffic model"),
          ("NA", "Network Administrator", "Read/write system, DCN, software and firmware"),
          ("SA", "Security Administrator", "Read/write all security, AAA and certificates"),
          ("PR", "Provisioning", "Facility endpoints and service provisioning"),
          ("NE", "Network Engineer", "Equipment, facility endpoints and cross-connections"),
          ("EA", "Encryption Administrator", "Data and control plane encryption"),
          ("TT", "Test and Turn up", "Turn-up and troubleshooting")]
out = ["# Access control index - user groups, objects and commands", "",
       "Derived from Chapter 2 Tables 32-34 "
       "([2.2.10 User groups and access privilege]"
       "(../02-using-the-cli/02-using-the-cli.md#2210-user-groups-and-access-privilege)). "
       "A user may belong to several groups; the highest permission wins.",
       "", "## User groups", "",
       "| Code | Group | Privilege summary |", "| --- | --- | --- |"]
for code, name, desc in GROUPS:
    out.append(f"| {code} | {name} | {desc} |")
out += ["", "## Command execution access (Table 34)", "",
        f"{len(acl['commands'])} commands have an explicit execution-access entry. "
        "Commands not listed here are governed by object access below.", "",
        "| Command | Sub-command | Conditions | Groups | Notes | File |",
        "| --- | --- | --- | --- | --- | --- |"]
for cmd in sorted(acl["commands"]):
    r = by_name.get(cmd)
    tgt = f"[{r['file'].split('/')[-1]}]({link(r, "../")})" if r else "-"
    for e in acl["commands"][cmd]:
        out.append(f"| `{esc(cmd)}` | {esc(e['sub'] or '-')} | {esc(e['cond'] or '-')} "
                   f"| {esc(e['groups'])} | {esc(e['notes'] or '-')} | {tgt} |")
out += ["", "## Data-model object access (Table 33)", "",
        f"{len(acl['objects'])} objects. `Write` lists the groups that may create, "
        "update or delete; all groups may read unless stated.", "",
        "| Object | Write access | Read access | Command page |",
        "| --- | --- | --- | --- |"]
for obj in sorted(acl["objects"]):
    tgt = OBJ_ALIAS.get(obj, obj)
    r = by_name.get(tgt)
    cell = f"[{r['file'].split('/')[-1]}]({link(r, "../")})" if r else "-"
    out.append(f"| `{esc(obj)}` | {acl['objects'][obj]['write']} | "
               f"{acl['objects'][obj]['read']} | {cell} |")
# Flag rows the PDF extraction garbled, rather than presenting them as clean data
GRP_RE = re.compile(r"^(all|[A-Z]{2}(,[A-Z]{2})*)$")
suspect = []
for cmd in sorted(acl["commands"]):
    for e in acl["commands"][cmd]:
        g = e["groups"].strip()
        if not g or not GRP_RE.match(g):
            suspect.append((cmd, e))
if suspect:
    out += ["", "## Source data quality", "",
            "The following rows are ambiguous **in the source document**: the PDF "
            "to Markdown conversion collapsed what were two table rows into one, "
            "so a single cell holds two values. They are reproduced verbatim above "
            "rather than split, because the pairing cannot be recovered with "
            "certainty. Check the guide's own Table 34 before relying on them.", "",
            "| Command | Sub-command | Conditions | Groups | Notes |",
            "| --- | --- | --- | --- | --- |"]
    for cmd, e in suspect:
        out.append(f"| `{esc(cmd)}` | {esc(e['sub'] or '-')} | {esc(e['cond'] or '-')} "
                   f"| {esc(e['groups'] or '(none given)')} | {esc(e['notes'] or '-')} |")

out += ["", f"Objects with no single matching command page: "
        f"{', '.join('`' + o + '`' for o in UNMAPPED_OBJ)} "
        "(container or model-level names rather than CLI commands).", ""]
W("access-control.md", "\n".join(out))

# ------------------------------------------------------------------- README.md
tot_params = sum(len(r["parameters"]) for r in recs)
out = f"""# Index - retrieval guide

Machine-oriented index over the split GX CLI Reference Guide. Everything here is
generated from `../{SRC_NAME}`; the content files
themselves are unmodified slices of that document.

## Which file answers which question

| The query looks like | Start here |
| --- | --- |
| "how do I configure X", command name known | [../INDEX.md](../INDEX.md) |
| Domain vocabulary, no command name ("wavelength", "upgrade", "loopback", "MACsec") | [topics.md](topics.md) |
| An attribute or parameter name ("admin-state", "tx-power") | [parameters.md](parameters.md) |
| An AID or entity string ("card-1-1", "port-1-1-DCN") | [entities.md](entities.md) |
| "what lives under X", "what does a card hold" | [entities.md](entities.md), the Containment section |
| "who is allowed to run X", user groups, privileges | [access-control.md](access-control.md) |
| A page citation from the text, "(p. 934)" | [pages.tsv](pages.tsv) |
| "Table 93", "Figure 5" | [tables.tsv](tables.tsv) |
| Anything programmatic: filtering, joins, bulk analysis | [commands.jsonl](commands.jsonl) |
| Browsing the document in reading order | [../NAVIGATION.md](../NAVIGATION.md) |

## Retrieval notes

- Operation command files are self-contained and short (median {sorted(r['line_count'] for r in recs if r['category'] == 'operation')[len([r for r in recs if r['category'] == 'operation']) // 2]} lines).
  Prefer reading the whole file over grepping fragments out of it.
- `INDEX.md` lists every command exactly once, under one primary domain.
  `topics.md` is where cross-cutting membership lives, so a command can appear in
  several topics.
- Access mode matters for answering "why did my command fail": a `cand`-only
  command must be run after `configure`, and needs `commit` to take effect.
- The guide covers four chassis variants (G31, G32, G34c, G42). Parameter ranges
  and supported cards differ per variant; check the parameter table rather than
  assuming.

## commands.jsonl fields

One JSON object per line, {len(recs)} lines.

| Field | Meaning |
| --- | --- |
| `name`, `category`, `section` | Command name; operation/navigation/piped/auxiliary; source section number |
| `file`, `anchor`, `source_lines`, `pages` | Where to read it, and where it came from in the source |
| `summary` | First substantive sentence of the description |
| `domain`, `domain_title`, `topics` | Curated classification |
| `kind`, `verbs`, `entity_ids`, `aid_prefix` | `entity` vs `action`; verbs the syntax supports; AID pattern the command addresses |
| `sub_keywords` | Literal keywords the command takes that are not addressable entities (`activate-file`, `location-led`) |
| `access_mode` | `operational` / `candidate_config` booleans, the source's own label, `qualified` when the source narrows it, plus the raw string |
| `usage_details` | Every row of the Command Usage Details table: pre-conditions, post-conditions, related commands, AID notes |
| `parameters`, `param_count` | Name, description, values, default and `used_in` verbs |
| `tables`, `page_refs` | Table numbers defined in the section; pages it cites |
| `user_groups` | Execution access (Table 34) and object access (Table 33) where defined |
| `has_examples`, `line_count` | Whether the section has worked examples; section size |

## Coverage

- {len(recs)} commands, {tot_params} parameter rows, {len(pmap)} distinct parameter names
- {len(ents)} commands address a named entity; {len(recs) - len(ents)} are action commands
- {len(acl['commands'])} commands carry explicit execution-access rules; {len(acl['objects'])} objects carry model access rules
- {len(curated.TOPICS)} topics, {sum(len(c) for _, _, c in curated.TOPICS)} topic-to-command associations
- `tables.tsv` covers 857 tables and 7 figures. The source gives Figure 7 no
  caption line (it is listed in the List of Figures and its image reference
  exists), so it has no row.
"""
W("README.md", out)
print("written:", sorted(os.listdir(IDX)))

