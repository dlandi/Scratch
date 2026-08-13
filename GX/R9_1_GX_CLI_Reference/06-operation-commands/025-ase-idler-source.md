---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.25. ase-idler-source'
source_lines: 5643-5696
---

## 6.25. ase-idler-source

#### Command Description

The commands described in this section are used to set or show the `ase-idler-source` attributes.

#### Command Syntax

```
set ase-idler-source-<name> [admin-state <value>] [label <value>] [alarm-report-control <value>] [pump-enable <value>] [target-output-power
<value>]
show ase-idler-source-<name> [supporting-card] [supporting-input-port] [supporting-output-port] [AID] [admin-state] [oper-state] [avail-state]
[label] [function] [alarm-report-control] [pump-enable] [pump-state] [target-output-power]
```

#### Command Usage Details

**Table 121: ase-idler-source Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 122: ase-idler-source Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-input-port | Input port that holds this facility. | String (length 0..64) | n/a | show |
| supporting-output-port | Output port that holds this facility. | String (length 0..64) | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| function | Functionality inherent to the ne-function object. | ase-idler-source | ase-idler-source | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| pump-enable | ASE Idler source enabling. | • enabled<br>• disabled | disabled | set, show |
| pump-state | The state of the ASE Idler pump. | • enabled<br>• disabled | disabled | show |
| target-output-power | ASE pump output power required (if manually configured). | range (-3.00..20.50) dBm | 13 | set, show |

#### Examples

The following command shows how to enable ASE idler source and how to set the target-output-power to 15dBm:

```
set ase-idler-source-1-3.1-ase pump-enable 'enabled' target-output-power '15.00'
```

<!-- page 226 -->
