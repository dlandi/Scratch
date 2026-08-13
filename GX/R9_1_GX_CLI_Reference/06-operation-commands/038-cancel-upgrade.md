---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.38. cancel-upgrade'
source_lines: 6380-6418
---

## 6.38. cancel-upgrade

#### Command Description

This command is used to cancel any active upgrade in progress.

#### Command Syntax

```
cancel-upgrade [-h]
```

#### Command Usage Details

**Table 148: cancel-upgrade Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |
| Pre-condition | An upgrade must be in progress for this command to execute successfully. |

#### Command Parameters

**Table 149: cancel-upgrade Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

The following example cancels any active upgrade:

```
cancel-upgrade
```

<!-- page 264 -->
