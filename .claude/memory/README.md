# Session memory, snapshot

Claude Code keeps per-repo memory outside the working tree, at:

```
C:\Users\dland\.claude\projects\E--Archive-GitHub-dlandi-Scratch\memory\
```

That directory is not under version control by anything. These files are a
snapshot of it, committed so the project state and working agreements survive a
lost machine and are visible to anyone reading the repo.

**The live copy is the one Claude reads. This copy is inert.** Editing a file
here changes nothing about how a session behaves. To change what Claude knows,
edit the live file, then re-copy the directory here:

```bash
cp "$USERPROFILE/.claude/projects/E--Archive-GitHub-dlandi-Scratch/memory/"*.md .claude/memory/
```

Expect drift between the two. Trust the live copy on any disagreement, and
re-snapshot when a session ends on something worth keeping.

## What is here

| File | Covers |
| --- | --- |
| `MEMORY.md` | The index Claude loads each session, one line per memory |
| `project_gx_llm_test_suite.md` | GX docs pipeline and LLM test suite: state, the agreed next plan, authoring lessons, one tested dead end |
| `feedback_scratch_repo_write_access.md` | Standing permission to edit this repo without asking, and the three things that does not switch off |
| `feedback_central_package_management.md` | GCP-PubSub uses central package management: edit `Directory.Packages.props`, do not run `dotnet add package` |

## One memory is deliberately not snapshotted

`MEMORY.md` indexes a fourth memory, `project_gcp_pubsub_simple.md`, which is
absent here on purpose. It records a real GCP project id, topic and subscription
name, and the local filesystem path to an ADC credential file. None of that is a
secret and the credential itself was never in it, but the identifiers are real
and point at live cloud resources, so they stay out of git by choice.

That memory is present and unchanged in the live directory, so sessions still
have it; only the snapshot omits it. The `MEMORY.md` line pointing at it is left
alone, because that file is a faithful copy of the live index and should not be
edited to match the snapshot. If you re-run the copy command above it will bring
the file back, so delete it again afterwards.
