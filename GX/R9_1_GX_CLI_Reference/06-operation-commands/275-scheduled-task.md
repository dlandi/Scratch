---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.275. scheduled-task'
source_lines: 20680-20736
---

## 6.275. scheduled-task

#### Command Description

These commands are used to add/set or show a set of individual user-configurable scheduled commands. The delete command is used to delete a scheduled task from the configuration.

#### Command Syntax

```
add scheduled-task-<name> [enabled<true|false>] [command<string>] [command-type<string>] [frequency <string>] [start-time<string>] [end-time
<string>] [persistent <true|false>] [task-status <value>] [next-run <time-stamp|never>] [previous-run <time-stamp|never>] [previous-result
<value>] [previous-output <string>]
set scheduled-task-<name> [enabled<true|false>] [command<string>] [command-type<string>] [frequency <string>] [start-time<string>] [end-time
<string>] [persistent <true|false>] [task-status <value>] [next-run <time-stamp|never>] [previous-run <time-stamp|never>] [previous-result
<value>] [previous-output <string>]
show scheduled-tasks
delete scheduled-task-<name>
```

#### Command Usage Details

**Table 643: scheduled-task Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 644: scheduled-task Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Task name | string | n/a | add, set, delete |
| enabled | Enable switch of this task; allows user to disable a task without deleting it. | true, false | true | add, set |
| command | Command that is scheduled. Content will depend on the command-type. | string (length 0...1024) | n/a | add, set |
| command-type | Type of configured command. | CLI command | 1 | add, set |
| frequency | Frequency interval for setting up a periodic scheduled task. If empty (default value), represents a single-occurrence task. | w(eeks), d(ays), h(ours), m(inutes), s(seconds). | n/a | add, set |
| start-time | Timestamp to start the task. For periodic tasks, this is the timestamp for the first trigger of the task. If not provided, uses current time as star time. | date-time in the format YYYY-MM- DDThh:mm:ssZ see the set-time command for detailed information. | n/a | n/a |
| end-time | Timestamp to stop the periodic task. Not relevant for single-occurrence tasks. | date-time in the format YYYY-MM- DDThh:mm:ssZ see the set-time command for detailed information. | n/a | add, set |
| persistent | If true, this scheduled task will persist a system restart. | true, false | true | add, set |
| task-status | Current operational state of the scheduled task. | true,false | false | add, set |
| next-run | Next run timestamp. May be 'never' for finished tasks. | date-time in the format YYYY-MM- DDThh:mm:ssZ see the set-time command for detailed information. | never | add, set |
| previous-run | Previous task run timestamp. | date-time, time, never | never | add, set |
| previous-result | Previous task run result. | 1, 2 | n/a | add, set |
| previous-output | Output of the previous task run. | string (length 0..1024) | n/a | add, set |

#### Examples

This example shows how to add a scheduled task:

```
add scheduled-task
```

<!-- page 1039 -->
