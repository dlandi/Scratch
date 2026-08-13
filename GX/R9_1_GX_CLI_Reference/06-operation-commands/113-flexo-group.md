---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.113. flexo-group'
source_lines: 11298-11347
---

## 6.113. flexo-group

#### Command Description

These commands are used to add/set/show/delete a flexo-group. Command Syntax

```
add flexo-group-<name> carriers <value> rate <value> modulation-format <value> group-id <value> [label <value>] [admin-state <value>]
[alarm-report-control <value>] [fec-type <value>]
set flexo-group-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [fec-type <value>] [group-id <value>] [expected-gid
<value>] [flexo-type <value>] [loopback <value>] [loopback-mode <value>]
show flexo-group-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [carriers] [rate] [modulation-format] [fec-type] [group-id]
delete flexo-group-<name>
```

#### Command Usage Details

**Table 312: flexo-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 313: flexo-group Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the facility. | String (length 1..64 characters) | n/a | add, delete |
| supporting-card | Card that holds this facility. | card | n/a | show |
| supporting-port | Ports that hold this facility. | port | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string (length 1..64 characters) | n/a | show |
| label | User defined label. | string (length 0..256 characters) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock, unlock, maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. | system, user | system | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed | add, set, show |
| carriers | A list of carriers that are bound to this facility. | string (length 1..32) | n/a | add, set,show |
| rate | Carried signal basic rate class (Gbit/s). | Number (Gbit/s) | n/a | add, set, show |
| modulation-format | Current modulation format. | not-applicable DP-QPSK DP-16QAM DP-8QAM BPSK | n/a | add, set, show |
| fec-type | The FEC type. | not-applicable cfec ofec noFEC G709 i4 i7 sdfec15 sdfec15nd staircase7 ufec7 | ofec | add, set, show |
| group-id | Indicates the interface group instance that the FlexO-x interface is a member of. It will be unique in the NE. | Integer (range: 1..1048575 bits) | n/a | add, set, show |

<!-- page 509 -->
