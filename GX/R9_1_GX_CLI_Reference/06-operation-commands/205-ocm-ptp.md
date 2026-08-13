---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.205. ocm-ptp'
source_lines: 16315-16380
---

## 6.205. ocm-ptp

#### Command Description

The commands described in this section are used to set or show the `ocm-ptp` attributes. The ocm-ptp facility is available for WS04S cards within a CD-AD ADG and provides dedicated OCM monitoring.

#### Command Syntax

```
set ocm-ptp-<name> [label <value>] [admin-state <value>] [ocm-enable <value>]
show ocm-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [ocm-enable] [ad-direction] [last-measurement] [adg-number] [monitoring-state]
```

#### Command Usage Details

**Table 496: ocm-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 497: ocm-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| ocm-enable | Enables regular power monitoring. | • enabled<br>• disabled | disabled | set, show |
| ad-direction | Reference to the AD (coupler/ splitter) DWDM port. | • egress<br>• ingress | ingress | show |
| last-measurement | Last OCM scan measurement date and time. ('never' is an extended part for yang:date-and-time) | • date-and-time<br>• never | never | show |
| adg-number | ADG reference. | uint8 (range 0..110) | 0 | show |
| monitoring-state | System reports 'enabled' when complete connectivity at AD is established, and OCM measurement is possible. | • enabled<br>• disabled | disabled | show |

#### Examples

The following example shows how to view all the ocm-ptp attributes:

```
show ocm-ptp-1-1.2-ocm1-in
```

The following example shows how to enable the OCM monitoring:

```
set ocm-ptp-1-1.2-ocm1-in ocm-enable enabled
```

The following example shows how to view the AD DWDM port direction:

```
show ocm-ptp-1-1.2-ocm1-in ad-direction
```

<!-- page 758 -->
