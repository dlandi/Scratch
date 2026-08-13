---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.73. db-migrate'
source_lines: 8529-8574
---

## 6.73. db-migrate

#### Command Description

The command described in this section is used to show the `db-migrate` attributes.

#### Command Syntax

```
db-migrate [-f] [type=]<value>
```

#### Command Usage Details

**Table 225: db-migrate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 226: db-migrate Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 227: db-migrate Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| type | defines the protection mode to be configured | • encryption-with-integrity<br>• encryption | n/a |

#### Examples

The following command enables integrity check for the system database, and will automatically reboot the controller.

<!-- page 375 -->

```
db-migrate -f encryption-with-integrity
```

<!-- page 376 -->
