---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.228. ots-r-auto-otdr'
source_lines: 17913-17965
---

## 6.228. ots-r-auto-otdr

#### Command Description

The commands described in this section are used to add or delete automatic OTDR `ots-r-auto-otdr` entity on ots-r containers, set or show the `ots-r-auto-otdr` attributes.

#### Command Syntax

```
add ots-r-auto-otdr-<name> [automatic-otdr <value>] [loss-calibration-by-otdr <value>]
delete ots-r-auto-otdr-<name>
set ots-r-auto-otdr-<name> [automatic-otdr <value>] [loss-calibration-by-otdr <value>]
show ots-r-auto-otdr-<name> [automatic-otdr] [external-attenuation-rx-measured] [total-reflectance-rx-measured] [loss-calibration-by-otdr]
[auto-otdr-state]
```

#### Command Usage Details

**Table 545: ots-r-auto-otdr Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 546: ots-r-auto-otdr Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| automatic-otdr | Enables/disables OTDR based automatic fiber check. On disabling, it terminates an ongoing automatic OTDR test. The attribute persists over warm/ cold restart and over SW upgrade. If the Raman card is associated with a base card that is equipped with integrated OTDR, the default value is enabled. If the NE is equipped with Raman card but not aggregated with the EDFA card, the value is disabled. If the user disables the feature while an active test is in progress, the ongoing OTDR tests is aborted and the amplifier blocked by the automatic OTDR will be allowed to be activated. If the user disables the feature while no active automatic OTDR test is in progress, no further automatic OTDR tests will be triggered until it is re-enabled. If enabled, automatic OTDR tests are triggered at the next opportunity when the pre-conditions and triggering criteria are met. | • enabled<br>• disabled | • enabled, for green field deployments<br>• disabled, for in-field SW upgrade | add, set, show |
| external-attenuation-rx-measured | Displays the attenuation (point losses) value between the span fiber and DWDM Line-In port of the Raman card, that is measured by the automatic OTDR Raman pre-check. | • not-applicable<br>• decimal64, range (0..55dB) | not-applicable | show |
| total-reflectance-rx-measured | Displays the total reflectance value between the span fiber and DWDM Line-In port of the Raman card, that is measured by the automatic OTDR Raman pre-check feature. | • not-applicable (the value will be updated once the OTDR Scan is done)<br>• decimal64, range (-99..99dB) | not-applicable | show |
| loss-calibration-by-otdr | Specifies if external-attenuation values used in Power Control come from user-configured attributes or automatically measured attributes:<br>• none: For green field deployments.<br>• rx-only: Use ots-r external-attenuation-rx-measured in pointloss calculation.<br>• tx-only: Use ots external-attenuation-tx in span loss calculation and launch power offset.<br>• tx-rx: Covers both cases of rx-only and tx-only. | • none<br>• rx-only<br>• tx-only<br>• tx-rx | none | add, set, show |
| auto-otdr-state | Displays the status of the automatic OTDR execution for the corresponding OTS-R facility:<br>• not-applicable: Hardware do not support auto otdr.<br>• not-available: Auto otdr scan has not been performed.<br>• pass: Auto otdr scan pass.<br>• in-progress: Auto otdr scan is in progress.<br>• fail: Auto otdr scan has failed<br>• aborted: Auto otdr scan aborted by end-user. | • not-applicable<br>• not-available<br>• pass<br>• in-progress<br>• fail<br>• aborted | not-applicable | show |

#### Examples

The following command shows how to enable the automatic OTDR fiber check:

```
set ots-r-auto-otdr-1-1-dwdm-line automatic-otdr enabled
```

The following command shows how to disable the automatic OTDR fiber check:

```
set ots-r-auto-otdr-1-1-dwdm-line automatic-otdr disabled
```

<!-- page 896 -->
