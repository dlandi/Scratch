---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.202. ochm'
source_lines: 16172-16234
---

## 6.202. ochm

#### Command Description

The commands described in this section are used to set or show the `ochm` (optical channel monitoring) attributes. In ILA nodes the OCHm represents the signaled optical channel from OSC, detected at ILA OMS by using the OSC information. When either OMS **monitoring-mode** is *not-monitored* (as configured by the user), or there is no OSC messaging information, the list of OCHm facilities in an NE will be empty.

#### Command Syntax

```
set ochm-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>]
show ochm-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [direction] [power-actual] [target-actual-power] [attenuation-actual]
```

#### Command Usage Details

**Table 490: ochm Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 491: ochm Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object. • allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| direction | Ingress or Egress direction. Currently, only Egress is supported. | • ingress<br>• egress | egress | show |
| power-actual | Currently received power (dBm). The value -99dBm means that:<br>• the power not yet measured (measurement is performed by the OCM at DGE2 card), or<br>• no power detected (or no appropriate fiber-connection). For the egress direction (the only currently supported):<br>• the power actual should be calibrated at DWDM Line port output, at same reference point where the target power is defined. | n/a | -99 | show |
| target-actual-power | Target power computed by ATPS. This attribute is applicable to HSC OLS nodes. | decimal64 (range: -99.00..99.00dBm) | -99 | show |
| attenuation-actual | DGE VOA attenuation of channel. This attribute is applicable to HSC OLS nodes. | decimal64 (range: 0..55dB) | n/a | show |

<!-- page 747 -->

#### Examples

The following example shows all the ochm entities and related parameters:

```
show ochm
```

The following example shows how to retrieve the avail-state of the ochm-1-7-dwdm-line2-191968750-egress:

```
show ochm-1-7-dwdm-line2-191968750-egress avail-state
```

<!-- page 748 -->
