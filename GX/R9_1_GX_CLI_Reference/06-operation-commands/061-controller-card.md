---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.61. controller-card'
source_lines: 7813-7871
---

## 6.61. controller-card

#### Command Description

This command is used to display the configuration of a controller card.

#### Command Syntax

```
show controller-card-<name> [redundancy-status] [redundancy-standby-status] [number-of-switchover-events] [time-of-last-switchover]
[additional-details]
```

#### Command Usage Details

**Table 200: controller-card Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 201: controller-card Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| name | controller card | string | n/a |
| redundancy-status | The redundancy state of the controller card. | • active<br>• standby<br>• not-in-service | not-in-service |
| redundancy-standby-status | State of the controller redundancy. | • ready-synchronized - Standby controller is sync and ready.<br>• not-ready-synchronizing - Standby controller synchronizing data with active controller. • not-ready-synchronize-fail - Synchronization fail.<br>• lock-out - Protection in lock-out state.<br>• card-not-present - Standby card is not present. | not-ready-synchronizing |
| number-of-switchover-events | Number of times that an active controller card has switchover. Value only visible on active controller card. | uint32 | n/a |
| time-of-last-switchover | Timestamp of the last controller switchover event. Value only visible on active controller card. |  | n/a |
| additional-details | Additional details for synchronization status. | string (length 0..128) | n/a |

#### Examples

This example provide the command to show controller card information in 1830 GX G40 environment:

```
show controller-card
controller-card redundancy-status
------------------- -----------------
controller-card-1-1 active
```

This example shows how to display controller card information in 1830 GX G34c chassis, slot 12:

```
show controller-card-1-12
```

This example shows how to display the controller card(s) information:

```
show controller-card
```

<!-- page 346 -->
