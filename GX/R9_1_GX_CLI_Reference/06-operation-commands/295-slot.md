---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.295. slot'
source_lines: 22908-22986
---

## 6.295. slot

#### Command Description

These commands are used to show the slot equipment holder details.

#### Command Syntax

```
show slot-<card-name>.<slot-name> [AID] [supported-type] [installed-type] [oper-state] [avail-state] [current-equipment]
show slot-<chassis-name>-<slot-name> [AID] [supported-type] [installed-type] [oper-state] [avail-state] [current-equipment]
```

#### Command Usage Details

**Table 690: slot Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 691: slot Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | Name of the card. | String (length 0..64 characters) | n/a | show |
| chassis-name | The name of the chassis. | String. | n/a | show |
| slot-name | The name of the slot. | String. | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete. | n/a | show |
| current-equipment | Name of the equipment that is currently required in this slot. | Name of the equipment (for example, 1-1) | n/a | show |

<!-- page 1134 -->

#### Examples

This example shows how to view the information of all slots in a node:

```
show slot
```

The following output is displayed for a 1830 GX G40 node:

```
slot              AID          supported-type  installed-type  oper-state  avail-state                            current-equipment
----------------  -----------  --------------  --------------  ----------  -------------------------------------  -----------------
slot-1-1          1-1          XMM4            XMM4            enabled     normal in-service                      1-1
slot-1-2          1-2          IOPANEL         IOPANEL         enabled     normal in-service                      1-2
slot-1-3          1-3          XMM4                            enabled     partially-faulted abnormal in-service  ---
slot-1-4          1-4          CHM6                            enabled     partially-faulted abnormal in-service  1-4
slot-1-5          1-5          CHM6,gx:UCM4                    enabled     partially-faulted abnormal in-service  ---
slot-1-6          1-6          CHM6,gx:UCM4                    enabled     partially-faulted abnormal in-service  ---
slot-1-7          1-7          CHM6,gx:UCM4                    enabled     partially-faulted abnormal in-service  ---
slot-1-FAN-1      1-FAN-1      FAN             FAN             enabled     normal in-service                      1-FAN-1
slot-1-FAN-2      1-FAN-2      FAN             FAN             enabled     normal in-service                      1-FAN-2
slot-1-FAN-3      1-FAN-3      FAN             FAN             enabled     normal in-service                      1-FAN-3
slot-1-FAN-4      1-FAN-4      FAN             FAN             enabled     normal in-service                      1-FAN-4
slot-1-FAN-5      1-FAN-5      FAN             FAN             enabled     normal in-service                      1-FAN-5
slot-1-FAN-6      1-FAN-6      XMM4-FAN        XMM4-FAN        enabled     normal in-service                      1-FAN-6
slot-1-FAN-7      1-FAN-7      XMM4-FAN        XMM4-FAN        enabled     normal in-service                      1-FAN-7
slot-1-FANCTRL-1  1-FANCTRL-1  FAN-CTRL        FAN-CTRL        enabled     normal in-service                      1-FANCTRL-1
slot-1-PEM-1      1-PEM-1      PEM             PEM             enabled     normal in-service                      1-PEM-1
slot-1-PEM-2      1-PEM-2      PEM             PEM             enabled     normal in-service                      1-PEM-2
slot-1-PEM-3      1-PEM-3      PEM             PEM             enabled     normal in-service                      1-PEM-3
slot-1-PEM-4      1-PEM-4      PEM             PEM             enabled     normal in-service                      1-PEM-4
```

This example shows how to view the information of slot-1-1 in a node:

```
show slot-1-1
```

<!-- page 1135 -->
