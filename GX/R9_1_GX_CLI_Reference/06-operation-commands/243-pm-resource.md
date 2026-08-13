---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.243. pm-resource'
source_lines: 18935-18985
---

## 6.243. pm-resource

#### Command Description

These commands are used to set or show the PM configuration per resource instance.

#### Command Syntax

```
set pm-resource-<resource> [real-time-supervision <value>]
show pm-resource-<resource> [resource-type] [AID] [real-time-supervision] [real-time-data-last-reset]
```

#### Command Usage Details

**Table 578: pm-resource Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 579: pm-resource Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | Existing system resource. | Instance ID | n/a | set, show |
| resource-type | Type of resource. | Resource type | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| real-time-supervision | Real-time data supervision for this resource. | true, false | true | set, show |
| real-time-data-last-reset | Date and time of the last real time data reset for this resource. If the data was never reset, this is the date and time of this resource's creation. | date-and-time | n/a | show |

<!-- page 954 -->

This example shows how to view the pm resource information from 1830 GX G40 trib-ptp:

```
show pm-resource-trib-ptp-1-7-T9
  pm-resource-trib-ptp-1-7-T9
  pm-control-entry-trib-ptp-1-7-T9/pm-15min/ingress/near-end
  pm-control-entry-trib-ptp-1-7-T9/pm-15min/egress/near-end
  pm-control-entry-trib-ptp-1-7-T9/pm-24h/ingress/near-end
  pm-control-entry-trib-ptp-1-7-T9/pm-24h/egress/near-end
  resource-type                                                           trib-ptp
  AID                                                                     '1-7-T9'
  real-time-supervision                                                   true
```

<!-- page 955 -->
