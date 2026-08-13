---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.184. named-value-set'
source_lines: 15200-15234
---

## 6.184. named-value-set

#### Command Description

These commands are used to add/set/show and delete the `named-value-set` attributes.

#### Command Syntax

```
add named-value-set-<db-entry-name>/<named-value-set-name> [value <value>]
set named-value-set-<db-entry-name>/<named-value-set-name> [value <value>]
delete named-value-set-<db-entry-name>/<named-value-set-name>
show named-value-set-<db-entry-name>/<named-value-set-name> [value]
```

#### Command Usage Details

**Table 457: named-value-set Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 458: named-value-set Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| db-entry-name | Name of the data base entry. |  | n/a | add,set, show, delete |
| named-value-set-name | User assigned name for this named-value-set. |  | n/a | add,set, show, delete |
| value | Value item | String (length 1..1024 characters) | n/a | add,set, show, delete |

<!-- page 687 -->
