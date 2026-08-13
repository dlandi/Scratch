---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.232. packaged-fw'
source_lines: 18224-18266
---

## 6.232. packaged-fw

#### Command Description

These commands are used to show the Firmware version included in this software-load. Versions for the same firmware can be different per equipment-type.

#### Command Syntax

```
show packaged-fw-<location-id>/<swload-state>/<equipment-type>/<fw-name> [fw-version]
show packaged-fw-<swload-state>/<equipment-type>/<fw-name> [fw-version]
```

#### Command Usage Details

**Table 553: packaged-fw Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 554: packaged-fw Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| location-id | Location ID (&lt;chassis-id&gt;-&lt;slot-id&gt;) of the SW load subcomponent. Software load information associated to each of the equipment. | string (length 0..64 characters) | n/a | show |
| swload-state | SW load subcomponent state. active - Active software load. inactive - Inactive software load. installable - Installable software load. | active inactive installable | n/a | show |
| equipment-type | Type of the equipment (card, etc) that will use this firmware. | String (length 0..32 characters) | n/a | show |
| fw-name | Name of the firmware. | String (length 0..32 characters) | n/a | show |
| fw-version | Included version of the firmware | String (length 0..32 characters) | n/a | show |

#### Examples

This example shows how to view all the components' Firmware version included in this software-load.

```
show packaged-fw
```

<!-- page 924 -->
