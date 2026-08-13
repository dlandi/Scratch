---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.281. security-policy-database'
source_lines: 21035-21077
---

## 6.281. security-policy-database

#### Command Description

These commands are used to add, edit or show the security database. The delete command is used to delete a security policy database.

#### Command Syntax

```
add security-policy-database-<ikev2-local-instance>/<ikev2-peer-name> [associated-secure-entity <value>]
set security-policy-database-<ikev2-local-instance>/<ikev2-peer-name> [associated-secure-entity <value>]
show security-policy-database-<ikev2-local-instance>/<ikev2-peer-name> [associated-secure-entity]
delete security-policy-database-<ikev2-local-instance>/<ikev2-peer-name>
```

#### Command Usage Details

**Table 654: security-policy-database Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 655: security-policy-database Command Attributes**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance | The name (ID) of the local IKE protocol daemon instance. | string | n/a | add, set, show, delete |
| ikev2-peer | A reference to the IKE peer object (IKE SA). | string | n/a | add, set, show, delete |
| associated-secure-entity | List of all SPD entries associated with this far-end peer for which IKE negotiates security associations (keys). | string | n/a | add, set, show |

#### Examples

This example shows how to add a security policy database in 1830 GX G40:

```
add security-policy-database-ikev2-local-instance-1-6/ikev2-NE202-1-4 associated-secure-entity secure-entity-1-6-L1-1
```

<!-- page 1062 -->
