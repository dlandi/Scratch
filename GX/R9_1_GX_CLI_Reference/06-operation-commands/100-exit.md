---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.100. exit'
source_lines: 10595-10635
---

## 6.100. exit

#### Command Description

This command is used to logout of the current CLI mode. It terminates the current CLI session if in operation mode, or leaves configuration mode if in candidate mode. A confirmation prompt will be displayed, unless the -f flag is provided.

**Tip:** The keyboard shortcut Ctrl+D has the same effect as `exit -f`.

#### Command Syntax

```
exit [-h|-f]
```

#### Command Usage Details

**Table 285: exit Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 286: exit Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces the command without confirmation. |

#### Examples

This example shows how to exit without confirmation:

```
exit -f
```

<!-- page 472 -->
