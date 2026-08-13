---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.343. task'
source_lines: 25908-25968
---

## 6.343. task

#### Command Description

These commands are used to add, set, show or delete a user configurable scheduled task. The commands can define a single occurrence of a task or periodic tasks.

#### Command Syntax

```
add task-<name> command <value> [enabled <value>] [label <value>] [alarm-report-control <value>] [command-type <value>] [frequency <value>]
[number-of-runs <value>] [start-time <value>] [end-time <value>] [persistent <value>]
set task-<name> [enabled <value>] [label <value>] [alarm-report-control <value>] [command <value>] [command-type <value>] [frequency <value>]
[number-of-runs <value>] [start-time <value>] [end-time <value>] [persistent <value>]
show task-<name> [enabled] [label] [alarm-report-control] [command] [command-type] [frequency] [number-of-runs] [start-time] [end-time]
[persistent] [task-status] [next-run] [previous-run] [previous-result] [previous-output]
delete task-<name>
```

#### Command Usage Details

**Table 789: task Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 790: task Command Attributes**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Identifier of the scheduled task. | String (length 0..64 characters) | n/a | add, set, show, delete |
| enabled | Enable switch of this task. Allows a user to disable a task without deleting it. | true, false | true | add, set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| command | Command that is scheduled. The content will depend on the command-type. | String (length 1..1024) | n/a | add, set, show |
| command-type | Type of configured command. | CLI - a CLI command | cli | add, set, show |
| frequency | Frequency interval for setting up a periodic scheduled task. | String (length 0..32) '[xw] [xd] [xh] [xm] [xs]', w(eeks), d(ays), h(ours), m(inutes), s(seconds) Examples:<br>• 2w - two weeks<br>• 5d 12h - 5 days and 12 hours<br>• 1h 7m 30s - 1 hour and 7 minutes and 30 | n/a | add, set, show |
| number-of-runs | Applicable when frequency is configured. This attribute defines the number of times a periodic task is executed before stopping. | no-limit number (range 1..65535) | no-limit | add, set, show |
| start-time | Timestamp to start the task. For periodic tasks, this is the timestamp for the first trigger of the task. | String (length 5..8), never | n/a | add, set, show |
| end-time | Timestamp to stop the task. For periodic tasks, this is the timestamp for the first trigger of the task. | String (length 5..8), never | never | add, set, show |
| persistent | If true, this scheduled task will persist a system restart. | true, false | true | add, set, show |
| task-status | Current operational state of the scheduled task. scheduled: Task is enabled and will run when the time comes. disabled: Task is disabled by user. finished: Task has reached its end-time, or single occurrence task was already executed. ongoing: Task is enabled and is currently running. | scheduled disabled finished ongoing | scheduled | show |
| next-run | Next run timestamp. May be 'never' for finished tasks. | String (length 5..8), never | never | show |
| previous-run | Previous task run timestamp. | String (length 5..8), never | never | show |
| previous-result | Previous task run result. | success fail | n/a | show |
| previous-output | Output of the previous task run. | String (length: 0..1024) | n/a | show |

#### Examples

This example shows how to add a new task:

```
add task-db_backup_once command "upload database file-server=xfr1" start-time 2021-04-23T05:05:00+00:00 alarm-report-control allowed label "DB
 Backup once"
```

**Note:** This command requires that the file server xfr1 be configured before this command is used.

<!-- page 1268 -->
