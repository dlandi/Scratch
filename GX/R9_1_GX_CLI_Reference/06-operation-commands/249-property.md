---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.249. property'
source_lines: 19347-19386
---

## 6.249. property

#### Command Description

These commands are used to set or show a type specific property, auto instantiated by the system, but configurable by the user.

#### Command Syntax

```
show property-<card-name>/<property-name> [value] [description]
set property-<card-name>/<property-name> [value <value>]
```

#### Command Usage Details

**Table 591: property Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 592: property Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card name | The name of the card the property applies to. | string | n/a | set, show |
| property-name | The property to be set. Supported values are fast-client-recovery and max-packet length.<br>• fast-client-recovery - Indicates if fast client signal recovery is enabled or disabled in case of client signal failures. • max-packet-length - Indicates the maximum packet length supported by the CHM6 / CHM6L DCO. | • fast-client-recovery - disabled, enabled.<br>• max-packet-length - 1518 to 18000. | • fast-client-recovery - disabled<br>• max-packet-length - 1518 | set, show |

#### Examples

This example shows how to enable fast client recovery with known and expected firmware version:

```
set property-1-5/fast-client-recovery value enabled
```

<!-- page 976 -->
