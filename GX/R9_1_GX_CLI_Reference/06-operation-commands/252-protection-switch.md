---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.252. protection-switch'
source_lines: 19499-19538
---

## 6.252. protection-switch

#### Command Description

This is the operating command for protection group switching.

#### Command Syntax

```
protection-switch [-f] [operation-type=]<value> [switch-target=]<value> [protection-group=]<value>
```

#### Command Usage Details

**Table 596: protection-switch Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 597: protection-switch Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| operation-type | The type of protection switch command. | force, lockout, manual, release | n/a |
| switch-target | The target of the switch command, which is not needed for release and lockout operation. | string | n/a |
| protection-group | The target of the switch command. | string | n/a |

#### Examples

This example shows how to perform a protection switch:

```
protection-switch protection-group-test operation-type=force switch-target=protection
```

<!-- page 985 -->
