---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.260. recover-mode'
source_lines: 19949-19992
---

## 6.260. recover-mode

#### Command Description

The `clear recover-mode` command, clears recover-mode flag. This action will clear the current recover-mode state of the NE, confirming the current configuration as is, re-enabling communication with the line cards and potentially reconfiguring the traffic settings. As such, it may be traffic impacting. This command has no parameters.

#### Command Syntax

```
clear [-f] recover-mode
```

#### Command Usage Details

**Table 612: clear recover-mode Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 613: clear recover-mode Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

#### Examples

This example shows how to clear the recover mode:

```
clear recover-mode
```

This example shows how to clear the recover-mode without confirmation prompt:

```
clear -f recover-mode
```

<!-- page 1005 -->
