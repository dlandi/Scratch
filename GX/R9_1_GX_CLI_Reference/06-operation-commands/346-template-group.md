---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.346. template-group'
source_lines: 26060-26103
---

## 6.346. template-group

#### Command Description

These commands are used to add and show the configuration that defines the data model for system template-group. The template-group represents a configuration group containing a list of template entries.

#### Command Syntax

```
add template-group-<name> [enabled <value>] [label <value>]
show template-group-<name> [enabled] [label]
delete template-group-<name>
```

#### Command Usage Details

**Table 795: template-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 796: template-group Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Represents name of the template-group | string | n/a | add, delete, show |
| enabled | enable the template-group | true, false | true (only if there is no other template-group enabled) | add, show |
| label | Represents the label to apply on the template - optional | string | n/a | add, show |

<!-- page 1272 -->

#### Example

This example shows how to add and enable a template-group:

```
add template-group-1
```

<!-- page 1273 -->
