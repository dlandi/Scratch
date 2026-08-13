---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.66. current-alarms'
source_lines: 8178-8216
---

## 6.66. current-alarms

#### Command Description

The command is used to show the list of currently raised alarms.

#### Command Syntax

```
show current-alarms [number-of-alarms] [last-changed]
```

#### Command Usage Details

**Table 211: current-alarms Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 212: current-alarms Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| number-of-alarms | Number of currently raised alarms. | Number | n/a | show |
| last-changed | Timestamp of the last change in the current alarm list (either a raise or clear event). | String | n/a | show |

#### Examples

The following example shows how to view the list of currently raised alarms.

```
show current-alarms
```

<!-- page 362 -->
