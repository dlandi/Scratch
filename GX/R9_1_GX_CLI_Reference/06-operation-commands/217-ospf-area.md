---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.217. ospf-area'
source_lines: 17227-17272
---

## 6.217. ospf-area

#### Command Description

These commands are used to add, set, show or delete an OSPF protocol area.

#### Command Syntax

```
add ospf-area-<instance-id>/<ospf-area-id> [ospf-area-type <value>]
set ospf-area-<instance-id>/<ospf-area-id> [ospf-area-type <value>]
show ospf-area-<instance-id>/<ospf-area-id> [ospf-area-type]
delete ospf-area-<instance-id>/<ospf-area-id>
```

#### Command Usage Details

**Table 521: ospf-area Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | The OSPF instance must be created before the area can be added. |

#### Command Parameters

**Table 522: ospf-area Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF instance ID. | uint8 (range 0 .. 255) | n/a | add, set, show, delete |
| ospf-area-id | OSPF Router Area ID. | dotted-quad | n/a | add, set, show, delete |
| ospf-area-type | OSPF Router Area Type. | normal | normal | add, set, show |

<!-- page 840 -->

#### Examples

This example shows how to add an OSPF area:

```
add ospf-area-1/0.0.0.0
```

<!-- page 841 -->
