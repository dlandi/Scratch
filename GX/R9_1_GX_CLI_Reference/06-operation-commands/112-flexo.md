---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.112. flexo'
source_lines: 11239-11297
---

## 6.112. flexo

#### Command Description

The commands described in this section are used to set or show the `flexo` attributes.

#### Command Syntax

```
set flexo-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [resource-mode <value>] [iid <value>] [fdd-raise-threshold
<value>] [fdd-clear-threshold <value>] [fed-raise-threshold <value>] [fed-clear-threshold <value>]
show flexo-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [foic-type] [fec-type] [resource-mode] [iid] [accepted-group-id] [accepted-iid]
[fdd-raise-threshold] [fdd-clear-threshold] [fed-raise-threshold] [fed-clear-threshold]
```

#### Command Usage Details

**Table 310: flexo Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 311: flexo Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the flexo facility. | String (0...64 characters) | n/a | add, set, delete, show |
| supporting-card | Card that holds this facility. | card | n/a | show |
| supporting-port | Ports that hold this facility. | port | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| label | User defined label. | String (length 0..256 characters) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock, unlock, maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. | system, user | system | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed | add, set, show |
| foic-type | FOICx.k lanes mean using k parallel lanes to carry a FlexO-x interface, where order x signifies the interface rate in units of 100G. A unique FOICx.k identification (G.709.3 FlexO-LR and G.709.1 FlexO-SR). | foic1.2 foic1.4 foic2.4 foic2.8 foic3.6 foic4.8 foic4.16 | foic4.8 | show |
| fec-type | The FEC type. | not-applicable cfec ofec noFEC | ofec | show |
| resource-mode | Resource mode configuration to support (ADM) add-drop or (XC) add-drop with regen | ADM, XC | ADM | set, show |
| iid | Uniquely identify each member of a group and the order of each member in the group. This information is required in the reordering process. Don’t need to be sequential. | Number | n/a | add, set, show |
| accepted-group-id | The received group instance id on the FlexO interface. | Number | n/a | show |
| accepted-iid | The received iid on the FlexO interface. | Number | n/a | show |

#### Examples

The following command shows how to set the resource-mode on the FlexO object:

```
set flexo-1-5-L1-1 resource-mode 'XC'
```

<!-- page 505 -->
