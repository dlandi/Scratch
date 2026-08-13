---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.46. change-ztp-mode'
source_lines: 6806-6851
---

## 6.46. change-ztp-mode

#### Command Description

This command is used to toggle the Zero Touch Provisioning (ZTP) mode, deactivating it or reactivating it. The command `change-ztp-mode enabled` starts ZTP, reverts the database to the factory default and triggers a system reboot. The command `change-ztp-mode disabled` disables ZTP and will stop ZTP if the ZTP is already in progress. This command can be executed while ZTP is running (interrupting the ZTP). The current configuration of the ztp mode can be displayed using the `show ztp`command.

#### Command Syntax

```
change-ztp-mode [-f] [ztp-mode=]<value>
```

#### Command Usage Details

**Table 165: change-ztp-mode Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 166: change-ztp-mode Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 167: change-ztp-mode Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| ZTP mode | Enable or disable ztp. | enabled, disabled | n/a |

<!-- page 290 -->

#### Examples

This example shows how to disable ZTP:

```
change-ztp-mode disabled
```

<!-- page 291 -->
