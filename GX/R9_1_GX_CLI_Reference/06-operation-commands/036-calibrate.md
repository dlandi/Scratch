---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.36. calibrate'
source_lines: 6275-6320
---

## 6.36. calibrate

#### Command Description

The command described in this section is used to calibrate the Raman gain.

#### Command Syntax

```
calibrate [type=]<value> [trigger=]<value> [entity=]<value>
```

#### Command Usage Details

**Table 143: calibrate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 144: calibrate Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| type | Type of calibration:<br>• raman: triggers calibration on raman cards. | • raman | n/a |
| trigger | Action triggered:<br>• start: starts the Raman gain calibration for the OTS span.<br>• stop: stops the Raman gain calibration for the OTS span. | • start<br>• stop | n/a |
| entity | Select the entity to be calibrated. The type of entity depends on type parameter. For raman calibration, the entity may be an OTS-R entity. | • instance-identifier | n/a |

#### Examples

The following command shows how to trigger an automatic Raman gain calibration:

```
calibrate raman start ots-r-1-1-dwdm-line
```

The following command shows how to stop an automatic Raman gain calibration:

```
calibrate raman stop ots-r-1-1-dwdm-line
```

<!-- page 261 -->
