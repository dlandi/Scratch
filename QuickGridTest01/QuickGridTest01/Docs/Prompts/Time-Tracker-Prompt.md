# Agent Task Execution Prompt

**CONFIGURATION**
| Variable | Value |
|----------|-------|
| ReportFolder | `Docs/Feature Design/Task Execution Reports/ExecutionReports-RowGroupingFeature/` |
| ExecutionReport | `RowGroupingFeature-ExecutionReport.md` |
| SpecDocument | `Docs/Feature Design/Tasks/RowGroupingFeature-Tasks.md` |

---

If you are not in 'Agent' mode, abort.

## Build Requirement (MANDATORY)

The work is **NOT complete** until the solution builds cleanly.

- You MUST run a build before finishing:
  - Prefer `run_build` (workspace build)
  - Or run: `dotnet build` on the relevant project/solution when appropriate
- If the build fails, you MUST continue debugging and fixing issues until it succeeds.
- Do not claim completion while any compilation errors remain.

## Unresolved Build Errors (Pause + Ask for Help)

If build errors cannot be resolved with reasonable local debugging (e.g., missing external dependencies, unclear intended APIs, ambiguous repo state), you MUST:
1. **Pause execution** of the task (do not continue making unrelated changes)
2. **Ask the user for guidance** with the exact error messages and the file paths involved
3. **Maintain task context** (state what you were trying to do, what you attempted, and what is blocked)

Do not guess or invent missing types/APIs to “make it compile” without user confirmation.

## Common Workspace Pitfall: Accidental Extra Root Folder

A frequent mistake is creating files under an **extra nested copy of the repo root**, e.g. creating:
- `QuickGridTest01/QuickGridTest01/QuickGridTest01/...`

instead of the correct project root:
- `QuickGridTest01/QuickGridTest01/...`

This can lead to:
- “Missing types/namespaces” or many unrelated build errors
- Difficulty locating the intended component/page under test
- Confusing build output because the wrong files are being compiled (or the intended ones are not)

**Remedy / Checks (required when builds look unexpectedly broken):**
1. Compare where existing demo pages live (e.g. `QuickGridTest01/Pages/*.razor`) and match that structure.
2. Use `file_search` for the page/component name to confirm the file is under the correct project folder.
3. If an extra nested root exists, move/delete the misplaced files and remove the extraneous folder.
4. Re-run build to confirm errors were not caused by path/layout mistakes.

**TASK EXECUTION WITH TIME TRACKING**

**Context:** Task definitions are in {SpecDocument}. Update {ReportFolder}{ExecutionReport} as you complete each task.

**Additional allowed context sources (read-only):**
- Feature specification: `Docs/Feature Design/RowGroupingFeature.md`
- Implementation plan: `Docs/Feature Design/ImplementationPlans/Plan_RowGroupingFeature.md`
- Legacy reference implementation: `RowColumn/*` (including `RowColumn.cs` and `RowColumn/Components/RowCard.razor`)
- Existing ComposableColumns implementation: `ComposableColumns/*` (especially `ComposableColumns/Core/ComposableColumn.cs`, `ComposableColumns/Core/FeatureContext.cs`, and existing features)

Use these documents/codebases for parity and interface-alignment checks while implementing tasks.

**MANDATORY: Before writing any code, you MUST:**
1. Call the `run_command_in_terminal` tool with `Get-Date -Format "yyyy-MM-dd HH:mm:ss"` to get current time
2. Record the session start time

If the terminal command fails or returns no output, abort the task and notify the user.

If `{ReportFolder}` or `{ReportFolder}{ExecutionReport}` does not exist, create it before starting task work.

**For EACH task:**

1. **BEFORE starting the task:** Call `run_command_in_terminal` with `Get-Date -Format "yyyy-MM-dd HH:mm:ss"` and store the task start time
2. **Implement the task** (write code, create files, etc.)
3. **AFTER completing the task:** Call `run_command_in_terminal` with `Get-Date -Format "yyyy-MM-dd HH:mm:ss"` to get task end time
4. **Calculate the duration** between start and end times
5. **Update ExecutionReport** with the format:

```
### Task Execution Log 
[TaskId]: Task Description
**StartTime:** [from task start time]
**End Time:** [from task end time]  
**Duration:** [calculated difference in minutes/seconds]

**Files Changed:**
- path/to/file1
- path/to/file2

**Required Artifacts/Checklists:**
- Include any checklists or interface-alignment notes required by the task’s deliverables (as specified in `{SpecDocument}`)

[Implementation details]
```

**After you have updated ExecutionReport, continue to the next task**

**AFTER all tasks are complete:**
1. Call `run_command_in_terminal` with `Get-Date -Format "yyyy-MM-dd HH:mm:ss"` to get session end time
2. Calculate total session duration from session start to session end
3. Update ExecutionReport with session summary including total duration
4. Update ExecutionReport marking tasks as `[x]`

**DO NOT skip time tracking calls. Each task MUST have start and end time recordings with calculated duration.**

**For EACH task in the task list below: before implementing, use `code_search` and `get_file` to locate existing patterns and the files under test, and then execute these tasks in order:**

---

**TASK LIST:**

M7a.P1.T1
M7a.P1.T2