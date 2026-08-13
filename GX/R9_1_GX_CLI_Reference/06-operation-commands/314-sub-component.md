---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.314. sub-component'
source_lines: 24171-24218
---

## 6.314. sub-component

#### Command Description

This command is used to show the sub-component details or card resources.

#### Command Syntax

```
show sub-component-<card-name>/<sub-component-name> [AID] [description]
```

#### Command Usage Details

**Table 729: sub-component Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 730: sub-component Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | The name of the card supporting the sub-component. | string (length 0..64 characters) | n/a | show |
| sub-component-name | The name of the the sub-component. | string | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| description | A user configurable description of the sub-component | String (length 0..128) | n/a | show |

<!-- page 1189 -->

#### Examples

The following example shows how to show a card's sub-component attributes:

```
show sub-component-<card-name>/<sub-component-name> [AID] [description]
temproot@GX> show sub-component
sub-component                  AID              description
-----------------------------  ---------------  -------------------------------------------------
sub-component-1-6/dco-oec      1-6-dco-oec      DCO OEC (Optical Engine Controller) sub-component
sub-component-1-6/dco-secproc  1-6-dco-secproc  DCO Security Processor sub-component
```

<!-- page 1190 -->
