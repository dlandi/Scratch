---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.90. dsc-group'
source_lines: 9940-10014
---

## 6.90. dsc-group

#### Command Description

The commands described in this section are used to add, delete, set or show the `dsc-group` attributes.

#### Command Syntax

```
add dsc-group-<name> carriers <value> rate <value> [label <value>] [admin-state <value>] [alarm-report-control <value>] [instance-id <value>]
[group-id <value>] [pre-fec-q-sig-deg-threshold <value>] [pre-fec-q-sig-deg-hysteresis <value>] [post-fec-q-sig-deg-threshold <value>]
[post-fec-q-sig-deg-hysteresis <value>] [dgd-high-threshold <value>]
delete dsc-group-<name>
set dsc-group-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [carriers <value>] [rate <value>] [instance-id
<value>] [group-id <value>] [pre-fec-q-sig-deg-threshold <value>] [pre-fec-q-sig-deg-hysteresis <value>] [post-fec-q-sig-deg-threshold <value>]
[post-fec-q-sig-deg-hysteresis <value>] [dgd-high-threshold <value>]
show dsc-group-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [carriers] [rate] [instance-id] [group-id] [pre-fec-q-sig-deg-threshold]
[pre-fec-q-sig-deg-hysteresis] [post-fec-q-sig-deg-threshold] [post-fec-q-sig-deg-hysteresis] [dgd-high-threshold]
```

#### Command Usage Details

**Table 264: dsc-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 432 -->

#### Command Parameters

**Table 265: dsc-group Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The file name without the .log extension. | String (length 0...128) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| carriers | The carrier associated to this facility. Possible values can be any card/ resources/supported-carriers. | String (length: 1..32) | n/a | add, set, show |
| rate | Carried signal basic rate class. | Number (Gbit/s) | n/a | show |
| instance-id | For identifying the dsc-group logic number, is added to the dsc-group model for creation. The attribute is optional and will be automatically created if not specified. The maximum value of the instance-id will be calculated based on the capacity of the line mode and the dsc-group rate (ex: for creating an 100G dsc-group from 400G 16QAM line mode, instance can be between 1 and 4) . | uint8 (range: 1 .. max) | n/a | add, set, show |
| group-id | Optional parameter on dsc-group creation, specifies the dsc-group group number that the dsc is a member of for a given optical-carrier. If not provided, it is automatically assigned by system. (ex: for creating an 100G dsc-group from 400G 16QAM line mode, group-id can be 1/3/5/7) | uint8 (range: 1 .. max) | n/a | add, set, show |
| pre-fec-q-sig-deg-threshold | The threshold based on which the PRE-FEC-Q-SIGNAL-DEGRADE alarm is raised. 0 implies threshold crossing alarming disabled. Specific sub-range is per carrier use-case. | decimal64, fraction-digits 3, (range: 0\|5.600..9.600 dB) | n/a | add, set, show |
| pre-fec-q-sig-deg-hysteresis | Hysteresis to account for raising of the PRE-FEC-Q-SIGNAL-DEGRADE alarm. | decimal64, fraction-digits 1, (range: 0.1..1.0 dB) | 0.5 | add, set, show |
| post-fec-q-sig-deg-threshold | The threshold based on which the POST-FEC-Q-SIGNAL-DEGRADE alarm is raised. | decimal64, fraction-digits 1; range (12.5..18.0) dB | 18 | add, set, show |
| post-fec-q-sig-deg-hysteresis | Hysteresis to account for raising of the POST-FEC-Q-SIGNAL- DEGRADE alarm. | decimal64, fraction-digits 1; range (0.1.. 1.0) dB | 0.5 | add, set, show |
| dgd-high-threshold | The threshold to raise the DGD- OORH alarm (in ps). | uint16, range (25..400) ps | 100 | add, delete, set, show |

#### Examples

The following example command shows how to add an DSC group:

```
add dsc-group-1-3-1-1 rate 100 carriers 1-3-1 group-id 1 instance-id 1
```

The following example command shows how to view the parameters of one DSC group:

```
show dsc-group-1-3-1-1
```

<!-- page 437 -->
