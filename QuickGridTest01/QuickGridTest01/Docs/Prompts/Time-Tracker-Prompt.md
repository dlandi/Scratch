# Agent Task Execution Prompt

**CONFIGURATION**
| Variable | Value |
|----------|-------|
| ReportFolder | `Docs/Feature Design/Task Execution Reports/ExecutionReports-InlineEditingPolish/` |
| ExecutionReport | `Phase-C-ExecutionReport.md` |
| SpecDocument | `InlineEditingPolish.md` |
| Phase | C |

---

If you are not in 'Agent' mode, abort.

**TASK EXECUTION WITH TIME TRACKING**

**Context:** Task definitions are in {SpecDocument}. Update {ReportFolder}{ExecutionReport} as you complete each task.

**MANDATORY: Before writing any code, you MUST:**
1. Call the `run_command_in_terminal` tool with `Get-Date -Format "yyyy-MM-dd HH:mm:ss"` to get current time
2. Record the session start time

If the terminal command fails or returns no output, abort the task and notify the user.

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

**TASK LIST (Phase {Phase}):**

C1.1
C1.2
C2.1
C2.2
C2.3