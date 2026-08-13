---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.238. pm-control'
source_lines: 18723-18762
---

## 6.238. pm-control

#### Command Description

These commands are used to set or show configuration for currently existing resources in the system that support PM data.

#### Command Syntax

```
set pm-control <entity> data-supervision <true|false>
show pm-control
```

#### Command Usage Details

**Table 568: pm-control Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 569: pm-control Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| entity | The resource to be configured. | string | n/a | set |
| data-supervision | Real-time data supervision for this resource. | true ,false | true | set |

#### Examples

This example shows how to set pm-control on a resource:

```
set pm-control pm-resource-card-1-1 data-supervision false
```

<!-- page 944 -->
