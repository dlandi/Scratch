---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.248. profile-control'
source_lines: 19232-19346
---

## 6.248. profile-control

#### Command Description

The `profile-control` command allows the user to read or write per-slice power or attenuation profiles to/from to write the correct number of data to the database (DB). The data to write to the DB can be retrieved from HW in case of power-profile or set by user in case of attenuation profile. This commands applies only to RD20TM operating in **card-mode** = *slte* or *slte-backhaul*. In case of RD20TM operating in **card-mode** = *slte*, the power profile and OCM power are defined at the output of the booster (before the EVOA), not at the DWDM line out port. The OMS Attenuation informative string is formatted as follows:

- Array of attenuation is '\<StartFreq\>,\<SlotWidth\>,\<AttnValue1\>,\<AttnValue2\>,\<AttnValue3\>,…,\<AttnValueN\>': **▪**\<StartFreq\> in MHz, e.g. 191300000. **▪**\<SlotWidth\> in GHz, e.g. 6.25. **▪**AttnValue is fixed to positive float with two decimals and range is 0 - 25.50. **▪**N depends on spectrum width and slot granularity. For standardC-band with slot granularity of 6.25, N is 776.

The NMC Attenuation informative string is formatted as follows:

- Array of attenuation is '\<StartFreq\>,\<SlotWidth\>,\<AttnValue1\>,\<AttnValue2\>,\<AttnValue3\>,…,\<AttnValueN\>': **▪**\<StartFreq\> in MHz will be lower frequecy of NMC. **▪**\<SlotWidth\> in GHz, e.g. 6.25. **▪**AttnValue can be positive, negative of float with two decimals and range is -10 - 25.50. **▪**N cannot exceed 32. **▪**The Attenuation values can be any of 'ww.f', 'w.ff', 'w.f', 'w'.

**OMS power data** Power values for standardC-band range at 6.25 GHz granularity are obtained from HW.

#### Command Syntax

```
profile-control [type=]<value> [entity=]<value> [direction=]<value> [[profile-data=]<value>]
```

<!-- page 971 -->

#### Command Usage Details

**Table 589: profile-control Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 590: profile-control Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| type | Type of control to be considered:<br>• write-attenuation-profile - writes an attenuation-profile for an entity that supports it.<br>• read-attenuation-profile - reads an attenuation-profile for an entity that supports it.<br>• create-power-profile - creates a power-profile snapshot for an entity that supports it.<br>• read-power-profile - reads a power-profile snapshot for an entity that supports it.<br>• read-ocm-power - Reads the OCM data from OMS entity. | • write-attenuation-profile<br>• read-attenuation-profile<br>• create-power-profile<br>• read-power-profile<br>• read-ocm-power | n/a |
| entity | Reference to an entity to which the profile-control applies. It may be different depending on &lt;type&gt;. | instance-identifier | n/a |
| direction | Direction associated with the entity.<br>• tx - Transmit.<br>• rx - Receive. Only applicable for some type of control requests. For power-profile, only tx is supported. | • tx<br>• rx | n/a |
| profile-data | Profile data to be inputted. The details are specific of the type of profile being considered, and only for 'write' requests. It is not used with power-profile type. | string | n/a |

#### Examples

This example shows how to write attenuations for OMS dwdm-line:

```
profile-control write-attenuation-profile oms-1-8-dwdm-line tx
 profile-data=191300000,6.25,1.4,1.4,1.4,1.19,1.19,1.2,1.19,1.14,1.15,1.18,1.21,1.16,1.2,1.19,1.22,1.19,1.2,1.25,1.2,1.21,1.16,1.22,1.22,1.2,1.1
6,1.15,1.19,1.21,1.23,1.19,1.16,1.19,1.19,1.21,1.25,1.22,1.22,1.25,1.26,1.22,1.25,1.27,1.3,1.27,1.28,1.3,1.26,1.27,1.21,1.25,1.27,1.19,1.26,1.21
,1.29,1.29,1.24,1.24,1.27,1.28,1.28,1.24,1.17,1.23,1.24,1.24,1.24,1.24,1.24,1.23,1.2,1.19,1.14,1.11,1.17,1.15,1.2,1.19,1.2,1.15,1.14,1.14,1.14,1
.1,1.09,1.14,1.14,1.12,1.14,1.11,1.09,1.1,1.07,1.15,1.13,1.06,1.12,1.14,1.15,1.16,1.13,1.17,1.15,1.16,1.15,1.12,1.18,1.07,1.08,1.11,1.12,1.15,1.
14,1.15,1.15,1.12,1.06,1.04,1.14,1.14,1.15,1.14,1.1,1.16,1.15,1.14,1.1,1.1,1.15,1.15,1.17,1.18,1.16,1.13,1.12,1.18,1.18,1.15,1.07,1.06,1.07,1.09
,1.09,1.09,1.06,1.08,1.15,1.12,1.15,1.13,1.12,1.16,1.14,1.15,1.12,1.09,1.12,1.13,1.14,1.11,1.05,1.11,1.09,1.14,1.1,1.1,1.16,1.11,1.19,1.2,1.18,1
.1,1.13,1.16,1.2,1.18,1.23,1.2,1.22,1.23,1.17,1.25,1.21,1.23,1.26,1.23,1.21,1.23,1.27,1.3,1.24,1.18,1.17,1.24,1.28,1.26,1.32,1.3,1.27,1.3,1.26,1
.3,1.25,1.25,1.32,1.27,1.27,1.33,1.29,1.3,1.28,1.28,1.3,1.29,1.22,1.28,1.27,1.24,1.27,1.29,1.3,1.33,1.35,1.36,1.29,1.25,1.25,1.23,1.23,1.18,1.23
,1.26,1.2,1.27,1.26,1.23,1.23,1.22,1.27,1.22,1.25,1.27,1.24,1.27,1.29,1.26,1.29,1.26,1.28,1.27,1.24,1.31,1.3,1.27,1.28,1.24,1.23,1.28,1.25,1.29,
1.28,1.27,1.28,1.3,1.29,1.33,1.22,1.31,1.3,1.32,1.35,1.36,1.32,1.32,1.35,1.31,1.25,1.29,1.34,1.32,1.31,1.34,1.3,1.3,1.31,1.34,1.35,1.29,1.28,1.3
3,1.4,1.41,1.39,1.39,1.38,1.35,1.36,1.4,1.4,1.43,1.4,1.36,1.39,1.43,1.41,1.4,1.37,1.44,1.43,1.45,1.45,1.41,1.44,1.45,1.47,1.49,1.5,1.48,1.52,1.4
9,1.47,1.46,1.44,1.45,1.4,1.46,1.49,1.49,1.48,1.51,1.52,1.49,1.45,1.47,1.51,1.53,1.54,1.45,1.48,1.49,1.52,1.46,1.46,1.45,1.47,1.52,1.49,1.52,1.5
1,1.47,1.53,1.52,1.55,1.51,1.51,1.54,1.54,1.52,1.54,1.56,1.53,1.52,1.46,1.42,1.5,1.49,1.48,1.47,1.49,1.47,1.47,1.44,1.43,1.46,1.43,1.38,1.44,1.4
5,1.38,1.4,1.43,1.4,1.46,1.48,1.45,1.45,1.36,1.42,1.44,1.46,1.46,1.4,1.43,1.42,1.42,1.38,1.42,1.39,1.38,1.38,1.41,1.36,1.37,1.4,1.43,1.44,1.42,1
.41,1.43,1.44,1.39,1.42,1.43,1.44,1.44,1.43,1.43,1.41,1.41,1.38,1.42,1.43,1.4,1.39,1.42,1.42,1.43,1.41,1.42,1.39,1.4,1.46,1.45,1.34,1.43,1.43,1.
41,1.41,1.41,1.48,1.49,1.46,1.47,1.43,1.45,1.46,1.48,1.43,1.46,1.47,1.5,1.51,1.52,1.56,1.6,1.56,1.53,1.57,1.59,1.54,1.57,1.56,1.6,1.59,1.57,1.59
,1.61,1.6,1.61,1.58,1.56,1.62,1.53,1.53,1.62,1.62,1.67,1.65,1.62,1.65,1.7,1.66,1.61,1.66,1.66,1.67,1.63,1.68,1.68,1.66,1.66,1.7,1.6,1.67,1.7,1.6
6,1.6,1.67,1.62,1.57,1.64,1.67,1.7,1.63,1.63,1.64,1.58,1.64,1.7,1.56,1.61,1.6,1.65,1.65,1.6,1.61,1.65,1.58,1.64,1.64,1.63,1.64,1.63,1.69,1.7,1.7
1,1.65,1.62,1.67,1.67,1.7,1.66,1.64,1.71,1.73,1.7,1.73,1.7,1.67,1.74,1.71,1.72,1.71,1.69,1.69,1.7,1.68,1.65,1.73,1.73,1.76,1.76,1.72,1.75,1.76,1
.68,1.79,1.73,1.72,1.78,1.77,1.76,1.79,1.77,1.77,1.78,1.76,1.71,1.72,1.71,1.7,1.75,1.72,1.74,1.75,1.78,1.77,1.77,1.72,1.72,1.68,1.66,1.6,1.72,1.
69,1.67,1.64,1.67,1.67,1.61,1.54,1.61,1.58,1.59,1.63,1.6,1.5,1.55,1.5,1.52,1.54,1.5,1.51,1.47,1.45,1.4,1.39,1.37,1.4,1.34,1.27,1.25,1.27,1.23,1.
21,1.2,1.12,1.05,1.08,1.09,1.12,1.1,1.06,1.05,0.98,0.95,0.99,0.98,0.93,0.95,0.81,0.86,0.86,0.84,0.81,0.83,0.78,0.76,0.79,0.73,0.72,0.75,0.76,0.6
2,0.66,0.65,0.61,0.62,0.64,0.61,0.52,0.45,0.51,0.54,0.54,0.42,0.45,0.45,0.48,0.5,0.44,0.41,0.42,0.36,0.42,0.39,0.31,0.28,0.45,0.38,0.33,0.36,0.2
5,0.27,0.31,0.32,0.33,0.32,0.26,0.29,0.27,0.17,0.19,0.22,0.21,0.25,0.23,0.21,0.2,0.2,0.09,0.11,0.21,0.17,0.22,0.28,0.24,0.22,0.18,0.16,0.17,0.21
,0.13,0.11,0.17,0.12,0.16,0.22,0.16,0.15,0.16,0.16,0.16,0.22,0.18,0.2,0.13,0.23,0.28,0.3,0.23,0.25,0.34,0.33,0.34,0.31,0.29,0.3,0.31,0.43,0.45,0
.44,0.47,0.52,0.58,0.58,0.58,0.61,0.56,0.52,0.62,0.57,0.68,0.74,0.74,0.77,0.73,0.72,0.73,0.71,0.86,0.83,0.79,0.74,0.86,0.93,0.91,0.94,0.96,1.01,
1.02,1.05,1.03,0.96,1.07,1.04,1.05,1.13,1.13,1.11,1.16,1.2,1.16,1.19,1.45,1.45
```

This example shows how to write attenuations for NMC:

```
profile-control write-attenuation-profile nmc-RD20-1-1 tx
 profile-data=192783000,6.25,-0.5,-0.45,-0.4,-0.2,-0.2,-0.2,-0.1,-0.1,-0.1,0,0,0,0.1,0.1,0.1,0.2,0.2,0.2,0,-0.05,-0.1
```

This example shows how to read attenuations for NMC:

```
profile-control read-attenuation-profile nmc-RD20-1-1 tx
```

This example shows how to create power-profile for OMS dwdm-line:

```
profile-control create-power-profile oms-1-8-dwdm-line tx
```

The following command shows how to read power-profile for OMS dwdm-line:

```
profile-control read-power-profile oms-1-8-dwdm-line tx
```

The following command shows how to read OCM power for OMS dwdm-line tx:

```
profile-control read-ocm-power oms-1-8-dwdm-line tx
```

The following command shows how to read OCM power for OMS dwdm-line rx:

```
profile-control read-ocm-power oms-1-8-dwdm-line rx
```

<!-- page 974 -->
