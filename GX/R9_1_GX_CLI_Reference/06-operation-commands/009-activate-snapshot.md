---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.9. activate-snapshot'
source_lines: 4645-4692
---

## 6.9. activate-snapshot

#### Command Description

This command is used to activate an available database snapshot.

#### Command Syntax

```
activate-snapshot db-instance <string> db-paraphrase <string> [sanity-check-override <true|false>]
```

#### Command Usage Details

**Table 85: activate-snapshot command usage**

| Section | Description |
| --- | --- |
| User Access Privilege Level | Operational mode |
| Pre-condition | A snapshot must have been taken to activate it. |
| Related Commands | take-snapshot (p. 1262) |

#### Command Parameters

**Table 86: activate-snapshot Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| db-instance | The database snapshot to be activated. | string | temp |
| paraphrase | Short description of the database to be activated. | string (length 40...200) | n/a |
| sanity-check-override | Action to override the sanity check. | true, false | false |

This example shows how to activate a database snapshot:

```
activate-snapshot sanity-check-override
```

This example shows how to activate a snapshot named onehour:

<!-- page 162 -->

```
activate-snapshot onehour
```

<!-- page 163 -->
