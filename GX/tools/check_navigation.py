"""Check that every link in NAVIGATION.md resolves, and that every content
slice is reachable from it."""
import os
import re
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import gxpaths

OUT = gxpaths.DOCS
def gh_slug(t):
    s=t.strip().lower(); s=re.sub(r"`([^`]*)`",r"\1",s)
    s=re.sub(r"\*\*?([^*]*)\*\*?",r"\1",s); s=s.replace("\\","")
    s=re.sub(r"[^\w\- ]","",s,flags=re.UNICODE); return s.replace(" ","-")
nav=open(os.path.join(OUT,"NAVIGATION.md"),encoding="utf-8").read()
links=re.findall(r"\]\(([^)]+)\)",nav)
cache={}
bad_file=[]; bad_anchor=[]; linked=set()
for l in links:
    if l.startswith("#") or l.startswith("http"): continue
    path,_,frag=l.partition("#"); linked.add(path)
    full=os.path.join(OUT,path.replace("/",os.sep))
    if not os.path.exists(full): bad_file.append(l); continue
    if not frag: continue
    if path not in cache:
        txt=open(full,encoding="utf-8").read()
        cache[path]={gh_slug(m.group(2)) for m in re.finditer(r"^(#{1,6})\s+(.*?)\s*$",txt,re.M)}
    if frag not in cache[path]: bad_anchor.append(l)
print("links checked:",len([l for l in links if not l.startswith('#')]))
print("missing files:",len(bad_file),bad_file[:5])
print("anchors not found in target:",len(bad_anchor),bad_anchor[:5])
allparts=set()
for root,_d,fs in os.walk(OUT):
    for fn in fs:
        rel=os.path.relpath(os.path.join(root,fn),OUT).replace("\\","/")
        if fn.endswith(".md") and fn!="README.md" and rel not in ("NAVIGATION.md","INDEX.md") and not rel.startswith("index/"): allparts.add(rel)
print("content files not reachable from NAVIGATION.md:",sorted(allparts-linked))
sys.exit(1 if (bad_file or bad_anchor) else 0)
