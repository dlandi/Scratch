---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.257. raman-calibration'
source_lines: 19804-19856
---

## 6.257. raman-calibration

#### Command Description

The commands described in this section are used to add, delete, set or show the `raman-calibration` attributes.

#### Command Syntax

```
add raman-calibration-<name> [label <value>]
delete raman-calibration-<name>
set raman-calibration-<name> [label <value>]
show raman-calibration-<name> [supporting-card] [supporting-input-port] [AID] [label] [function] [calibration-state] [intermediate-results]
[gain-calibration-error] [calibrated-delta-pointloss] [last-calibration-timestamp] [additional-info]
```

#### Command Usage Details

**Table 606: raman-calibration Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 607: raman-calibration Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-input-port | Input port that holds this facility. | String (length 0..64) | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| function | Displays the NE Function object characterization:<br>• pa: Pre-amplifier.<br>• ba: Booster (booster-amplifier).<br>• inline: Inline amplifier (ILA node-types).<br>• add: Add amplifier.<br>• drop: Drop amplifier.<br>• backward-raman: Raman amplifier.<br>• edfa-tof: Erbium-Doped Fiber Amplifier/Tunable Optical Filter.<br>• edfa: Erbium-Doped Fiber Amplifier.<br>• ase-idler-source: ASE Idler source.<br>• idler: ASE Idler service (within an RD card).<br>• raman-calibration: raman calibration. | • pa<br>• ba<br>• inline<br>• add<br>• drop<br>• backward-raman<br>• edfa-tof<br>• edfa<br>• ase-idler-source<br>• idler<br>• raman-calibration | n/a | show |
| calibration-state | Displays the state of the automatic Raman gain calibration process:<br>• not-available: Raman calibration has not been triggered or no prior calibration has occurred.<br>• in-progress: Raman calibration is currently running.<br>• up-to-date: Raman calibration has completed successfully and is up-to-date.<br>• out-dated: Raman calibration out-dated due to OTS OLOS, change in delta pointloss etc.<br>• fail: Raman calibration ended with a failure. | • not-available<br>• in-progress<br>• up-to-date<br>• out-dated<br>• fail | not-available | show |
| intermediate-results | Indicates the intermediate raman calibration results. | string (length 0..1024) | "" | show |
| gain-calibration-error | Represents the residual gain error after each iteration (in dB). The value not-available indicates that is Not-available/ Not specified. | • not-available<br>• decimal64 (fraction-digits 2) | not-available | show |
| calibrated-delta-pointloss | The attribute represents the suggested delta-pointloss at the end of each iteration of the automatic Raman gain calibration (in dB). The value not-available indicates that is Not-available/ Not specified. | • not-available<br>• decimal64 (fraction-digits 2) in the range (-3..3.5dB) | not-available | show |
| last-calibration-timestamp | Time when the last time the automatic Raman gain calibration rpc was completed with or without errors. | • date-and-time<br>• never | never | show |
| additional-info | Indicates any information for troubleshooting when the calibration-state is fail or out-dated. | string (length 0..1024) | "" | show |

#### Examples

The following command shows how to view the raman calibration

```
show raman-calibration-1-8
```

<!-- page 1001 -->
