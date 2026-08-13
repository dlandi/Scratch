---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.289. set-time'
source_lines: 21734-21786
---

## 6.289. set-time

#### Command Description

The `set-time` command changes the system time. The command is only applicable when time-source is manual (e.g. NTP is not enabled). The new time is provided with the format derived from ISO 8601, with the combined date and time (2017-01-12T10:22:25Z), where 'Z' represents UTC timezone. A non-UTC timezone is also allowed by providing +/-hh:mm instead of 'Z'. The current system time can be retrieved with command `time`. This command provides the time based on the system configured timezone, not the timezone used in the `set-time` command.

#### Command Syntax

```
set-time -h
set-time [new-time=]<value>
```

#### Command Usage Details

**Table 673: set-time Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 674: set-time Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

<!-- page 1088 -->

**Table 675: set-time Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| new-time | Time to set in the system. | string | n/a |

#### Examples

This example shows how to set a new time in UTC:

```
set-time 2021-02-06T11:16:58Z
```

This example shows how to set a new time in timezone GMT+03:

```
set-time 2021-04-01T18:46:44+03:00
```

<!-- page 1089 -->
