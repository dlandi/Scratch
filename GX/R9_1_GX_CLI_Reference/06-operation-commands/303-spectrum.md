---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.303. spectrum'
source_lines: 23323-23387
---

## 6.303. spectrum

#### Command Description

The commands described in this section are used to set or show the spectrum facility attributes. The spectrum facility is only instantiated by the system when the underlying server layer, OMS, monitoring-mode, is configured either as *non-intrusive* or *ila-* *with-equalization*. The object **spectrum** is only of relevance if the DGE2 is intended to be used in the NE. The existence of this MO is dependent on the OMS **monitoring-mode**. For additional details on OMS and related attributes, refer to oms (p. 777).

#### Command Syntax

```
set spectrum-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [attenuation-setting <value>]
show spectrum-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [dge-in-use] [attenuation-setting]
```

#### Command Usage Details

**Table 706: spectrum Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 707: spectrum Command Parameters**

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
| dge-in-use | Indicates if a DGE is in used for the respective DWDM line. It reports true if the corresponding OMS monitoring-mode is ila-with-equalization. | • false<br>• true | false | show |
| attenuation-setting | Unique attenuation value for entire spectrum [dB]. Editable if the attenuation-control-mode = "manual" and control-mode = "auto-max-pw". | range [0.. 30] dB | 0dB | set, show |

#### Examples

The following example shows how to view the spectrum parameters of all spectrum entities:

```
show spectrum
```

The following example shows how to view the spectrum parameters of one spectrum entity:

```
show spectrum-1-7-dwdm-line2
```

The following example shows how to lock admin-state of one spectrum entity:

```
set spectrum-1-7-dwdm-line2 admin-state lock
```

<!-- page 1153 -->
