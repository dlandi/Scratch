---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.204. ocm-mp'
source_lines: 16269-16314
---

## 6.204. ocm-mp

#### Command Description

The commands described in this section are used to set or show the `ocm-mp` attributes.

#### Command Syntax

```
set ocm-mp-<name> [label <value>] [admin-state <value>] [ocm-enable <value>]
show ocm-mp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [ocm-enable] [monitored-port] [ad-direction] [monitoring-state]
```

#### Command Usage Details

**Table 494: ocm-mp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 495: ocm-mp Command Parameters**

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
| ocm-enable | Enables regular power monitoring. | • disabled<br>• enabled | enabled | set, show |
| monitored-port | The port that is being monitored. Can be different of supporting-port for a non-integrated OCM. • not-applicable - Not Applicable/ Not specified.<br>• instance-identifier | • not-applicable<br>• instance-identifier | not-applicable | show |
| ad-direction | Reference to the AD (coupler/ splitter) DWDM port. | • ingress | ingress | show |
| monitoring-state | System reports 'enabled' when OMS reference exists. | • enabled<br>• disabled | enabled | show |

<!-- page 754 -->
