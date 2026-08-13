---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.344. telemetry'
source_lines: 25969-26007
---

## 6.344. telemetry

#### Command Description

This command is used to configure persistent and dynamic telemetry. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
show telemetry
set telemetry id <string>
```

#### Command Usage Details

**Table 791: telemetry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 792: telemetry Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| id | Persistent and dynamic telemetry. | string (length 0..4096) | n/a |

#### Examples

This example shows how to view the telemetry subscription:

```
show telemetry
```

<!-- page 1269 -->
