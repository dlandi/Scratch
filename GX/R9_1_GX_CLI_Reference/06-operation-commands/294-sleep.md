---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.294. sleep'
source_lines: 22855-22907
---

## 6.294. sleep

#### Command Description

This command is used to specify a delay for a specified amount of time. The sleep time may be an arbitrary floating point number.

#### Command Syntax

```
sleep -h
sleep <time in seconds>
```

#### Command Usage Details

**Table 687: sleep Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 688: sleep Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 689: sleep Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| time in seconds | Duration of delay in seconds. | The sleep time may be an arbitrary floating point number. | n/a |

#### Examples

This example shows how to set sleep to 1 second:

<!-- page 1131 -->

```
sleep 1
```

This example shows how to set sleep to 100 milliseconds :

```
sleep 0.1
```

<!-- page 1132 -->
