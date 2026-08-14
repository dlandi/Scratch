"""Rebuild the entire GX documentation tree from the source document, then
verify it.

    python build_all.py            # build, then run all checks
    python build_all.py --check    # run checks only, build nothing
    python build_all.py --quiet    # only show step names and failures

Exit code is non-zero if any step or check fails, so this is safe to gate on.
"""
import os
import subprocess
import sys

TOOLS = os.path.dirname(os.path.abspath(__file__))

BUILD_STEPS = [
    ("step1_split.py", "split the source into per-chapter / per-command files"),
    ("step2_privileges.py", "extract user-group tables from chapter 2"),
    ("step3_extract.py", "extract structured records, page map, table map"),
    ("step4_build_index.py", "assemble INDEX.md and the index/ files"),
    ("step5_navigation.py", "rewrite the source TOC into NAVIGATION.md"),
]

CHECKS = [
    ("check_consistency.py", "every stated number matches reality"),
    ("check_index_links.py", "every index link and record resolves"),
    ("check_navigation.py", "every NAVIGATION.md link resolves"),
]


def run(script, why, quiet):
    print(f"\n=== {script} - {why}")
    r = subprocess.run([sys.executable, os.path.join(TOOLS, script)],
                       cwd=TOOLS, capture_output=True, text=True)
    out = (r.stdout or "").rstrip()
    if out:
        if quiet:
            tail = [l for l in out.split("\n") if "FAIL" in l or "broken" in l]
            print("\n".join(tail[-6:]) if tail else out.split("\n")[-1])
        else:
            print(out)
    if r.returncode != 0:
        print((r.stderr or "").rstrip()[-2000:])
        print(f"!!! {script} FAILED (exit {r.returncode})")
    return r.returncode


def main():
    quiet = "--quiet" in sys.argv
    steps = [] if "--check" in sys.argv else BUILD_STEPS
    failed = []
    for script, why in steps + CHECKS:
        if run(script, why, quiet) != 0:
            failed.append(script)
            if (script, why) in steps:
                break                     # later steps depend on earlier output
    print()
    if failed:
        print("FAILED:", ", ".join(failed))
        return 1
    print("All steps and checks passed.")
    print("step1_split.py re-verifies that the split reassembles byte-identically "
          "to the source; see its VERIFY line above.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
