---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.194. nmc-f'
source_lines: 15732-15791
---

## 6.194. nmc-f

#### Command Description

This command is used to show the Network Media Channel Filler (NMC-F) facility attributes.

#### Command Syntax

```
set nmc-f-<name> [admin-state <value>] [alarm-report-control <value>]
show nmc-f-<name> [admin-state] [supporting-facilities] [supported-facilities] [supporting-card] [supporting-port] [AID] [alarm-report-control]
[alloc-lower-frequency] [alloc-upper-frequency] [alloc-bandwidth] [actual-lower-frequency] [actual-upper-frequency] [actual-bandwidth]
[power-actual-tx] [monitoring-state] [power-actual-rx] [output-attenuation-compensation-actual]
```

#### Command Usage Details

**Table 474: nmc-f Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 475: nmc-f Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| alloc-lower-frequency | Allocated Lower Frequency of the NMC Filler. | value in MHz units | n/a | show |
| alloc-upper-frequency | Allocated Upper Frequency of the NMC Filler. | value in MHz units | n/a | show |
| alloc-bandwidth | Allocated Bandwidth of the NMC Filler. | value in MHz units | 75000 | show |
| actual-lower-frequency | Actual lower Frequency of the NMC Filler. | value in MHz units | n/a | show |
| actual-upper-frequency | Actual Upper Frequency of the NMC Filler. | value in MHz units | n/a | show |
| actual-bandwidth | Actual Bandwidth of the NMC Filler. | value in MHz units | 0 | show |
| power-actual-tx | Optical Power Transmitted, actual measurement. | values in the range [-99.00..99.00]dBm | -99 | show |
| monitoring-state | System reports this attribute, to indicate whether the NMC is intended to be in used. As this is system created attribute it is enabled by default. | • enabled<br>• disabled | enabled | show |
| power-actual-rx | Optical Power Received, actual measurement. | values in the range [-99.00..99.00]dBm | -99 | show |
| output-attenuation-compensation-actual | Network Media Channel attenuation adjustment applied by auto-controls to do power targeting in mux direction. | decimal64 (fraction-digits 2; range -20..20) | 0 | show |

#### Examples

This example shows how to view the NMC-F attributes from '1-3-dwdm-line-58' entity:

<!-- page 722 -->

```
show nmc-f-1-3-dwdm-line-58
```

<!-- page 723 -->
