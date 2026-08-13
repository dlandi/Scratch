---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.20. amplifier-raman'
source_lines: 5416-5470
---

## 6.20. amplifier-raman

#### Command Description

These commands are used to set or show the amplifier object attributes.

#### Command Syntax

```
set amplifier-raman-<name> [admin-state <value>] [label <value>] [alarm-report-control <value>] [control-mode <value>] [amplifier-enable <value>]
[connected-amp-edfa-optimum-gain <value>] [target-raman-gain <value>]
show amplifier-raman-<name> [supporting-card] [supporting-input-port] [supporting-output-port] [AID] [admin-state] [oper-state] [avail-state]
[label] [function] [alarm-report-control] [control-mode] [raman-state] [amplifier-enable] [connected-amplifier] [connected-amp-edfa-optimum-gain]
[total-pump-power] [number-of-pumps] [target-raman-gain] [actual-raman-signal-gain] [actual-raman-osc-gain] [control-state]
```

#### Command Usage Details

**Table 110: amplifier-raman Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 111: amplifier-raman Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Object name. | String (length 1..64) | n/a | set, show |
| supporting-card | Card that holds this object. | String | n/a | show |
| supporting-input-port | Rx (input) Port that holds this object. | String | n/a | show |
| supporting-output-port | Tx (output) Port that holds this object. | String | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| admin-state | The administrative state of the object. | lock, unlock, maintenance | unlock | set, show |
| oper-state | The operational state of the object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| label | User-defined label for the object. | String (length 0..256) | n/a | set, show |
| function | Describes the function of the object:<br>• 'pa' for pre-amplifierspa - for pre-amplifier<br>• 'ba' for booster (amplifiers)<br>• 'inline' for both amplifiers of ILAx card in node-type ILA.<br>• 'add'/ 'drop' - possible to configure if card is within an ADG: only applicable for BAX.<br>• 'backward-raman': only applicable to Raman card (RPB*: raman pump backward) | pa, ba, inline,add, drop, backward-raman | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. • allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| control-mode | Raman Control Mode.<br>• auto: the user only needs to configure the span attributes (and optionally, the pointloss); the gain is automatically controlled.<br>• auto-planned: the user needs to configure the target Raman gain, and span attributes (and optionally, the pointloss)<br>• manual: basic manual configuration; even amplifier target-pump-power(s) should be configured. | auto, manual, auto-planned | auto | set, show |
| raman-state | State of the current Raman state/ amplifier. • disabled: Disabled local and remote Raman.<br>• disabled-from-remote: Disabled locally because of remote Raman disabled.<br>• enabled: Local Raman enabled, operating with remote Raman. | disabled,disabled-from-remote,enabled | disabled | show |
| amplifier-enable | Enable or disable the amplifier. | disable-local-and-remote, disable-local, enabled | disable-local-and-remote | set, show |
| connected-amplifier | Connected Amplifier. The system reports the degree that corresponds to the amplifier where Raman is fiber-connected to.<br>• connected-amplifier indicates the degree Raman got fiber-connected to. In ILA, this attribute is not exposed.<br>• If Raman is not fiber-connected, the system returns 'not-specified'. | • not-specified<br>• range [1..20] | not-specified | show |
| connected-amp-edfa-optimum-gain | Connected EDFA Optimum Gain. Connected EDFA Optimum Gain 0 indicates that the optimum gain is not known, in case of disaggregated Raman. The attribute is only of relevance if control-mode = 'auto', or when Raman is dis-aggregated from the connected amplifier card (i.e. no fiber-connection to the amplifier). | range [1..55dB] | 0dB | set, show |
| total-pump-power | Operating Total Pump Power. When the value of Total Pump Power is available: the raman pump power actually being sent on port 401, upstream, in the DWDM Line. -99 (-99.00) means no power (or card not actual). | range [-99 .. 99 dBm] | -99dBm | show |
| number-of-pumps | Number of pumps for the required-equipped card. This value dictates the number of pump-power objects exposed by the system. | 4, 2 | - | show |
| target-raman-gain | Indicates the target Raman gain:<br>• The target Raman gain, configurable in case the control-mode is different than auto.<br>• In case control-mode is auto, this parameter is ignored. | range [0, 5..30dB] | 0dB | set, show |
| actual-raman-signal-gain | Indicates the Raman Signal Gain. It is the actual Raman gain of C- Band (signal). Note: when Raman amplifier is disabled (or card's oper-state = disabled), the value is 0. | &lt;gain value&gt;dB | 0 | show |
| actual-raman-osc-gain | Indicates the OSC Raman gain. It is the actual Raman gain OSC. Note: when Raman amplifier is disabled (or card's oper-state = disabled), the value is 0. The value may be 0 (or residual) when OSC at connected amplifier does not transmit OSC. | &lt;gain value&gt;dB | 0 | show |
| control-state | Indicates the current state of the power control adjustment for the preamplifier:<br>• unknown : default value, awaiting update.<br>• not-applicable : if in manual control-mode.<br>• stopped : control loop suspended due to a fault disable condition.<br>• converged : three consecutive control cycles with no adjustments. • fine-tuning : less than 0.5dB from target.<br>• adjusting : greater than 0.5dB from target. | • unknown<br>• not-applicable<br>• stopped<br>• converged<br>• fine-tuning<br>• adjusting | unknown | show |

<!-- page 213 -->
