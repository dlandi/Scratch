---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.19. amplifier'
source_lines: 5307-5415
---

## 6.19. amplifier

#### Command Description

These commands are used to set or show the amplifier object attributes.

#### Command Syntax

```
set amplifier-<name> [admin-state <value>] [label <value>] [alarm-report-control <value>] [amplifier-enable <value>] [amplifier-turn-on-delay
<value>] [forced-shutdown <value>] [control-mode <value>] [gain-range-control <value>] [span-loss-control <value>] [gain-range-target <value>]
[gain-target <value>] [gain-adjustment <value>] [output-voa-attenuation <value>] [voa-control-mode <value>] [tilt-control-mode <value>]
[tilt-target <value>] [tilt-adjustment <value>] [raman-signal-gain <value>] [raman-osc-gain <value>] [olos-shutdown-soak-timer <value>]
[olos-shutdown-disable <value>] [control-speed-factor <value>]
show amplifier-<name> [supporting-card] [supporting-input-port] [supporting-output-port] [AID] [admin-state] [oper-state] [avail-state] [label]
[function] [alarm-report-control] [partner-amplifier] [amplifier-enable] [amplifier-turn-on-delay] [amplifier-turn-on-remain] [forced-shutdown]
[control-mode] [amp-control-support] [amplifier-mode] [pump-state] [actual-transmission-band] [gain-range-control] [span-loss-control]
[gain-range-target] [gain-range-actual] [gain-target] [gain-operating] [optimum-edfa-gain] [gain-adjustment] [amplifier-type] [output-power-mon]
[output-power-mon-with-ase] [input-power-mon] [output-voa-attenuation] [voa-control-mode] [output-voa-actual] [power-before-output-voa]
[interstage-support] [interstage-loss] [tilt-control-mode] [tilt-target] [tilt-adjustment] [tilt-actual] [raman-signal-gain] [raman-osc-gain]
[olos-shutdown-soak-timer] [control-state] [olos-shutdown-disable] [control-speed-factor]
```

#### Command Usage Details

**Table 108: amplifier Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

<!-- page 192 -->

**Table 109: amplifier Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Object name. | String (length 1..64) | n/a | set, show |
| supporting-card | Card that holds this object. | String | n/a | show |
| supporting-input-port | Rx (input) Port that holds this object. | String | n/a | show |
| supporting-output-port | Tx (output) Port that holds this object. | String | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| admin-state | The administrative state of the object. | lock, unlock, maintenance | unlock | show |
| oper-state | The operational state of the object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| label | User-defined label for the object. | String (length 0..256) | n/a | set, show |
| function | Describes the function of the object:<br>• 'pa' for pre-amplifierspa - for pre-amplifier<br>• 'ba' for booster (amplifiers)<br>• 'inline' for both amplifiers of ILAx card in node-type ILA. • 'add'/ 'drop' - possible to configure if card is within an ADG: only applicable for BAX.<br>• 'backward-raman': only applicable to Raman card (RPB*: raman pump backward) | pa, ba, inline,add, drop, backward-raman | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| partner-amplifier | The partner amplifier for PAx/ BAX instalments. | • not-applicable - Not Applicable/ Not specified.<br>• instance-identifier | not-applicable | show |
| amplifier-enable | Enable or disable the amplifier. Output power is dependent on:<br>• node-type ILA: existence of OSC signal<br>• node-type OADM: OMS related facilities and existence of OXcon. | enabled, disabled | disabled | set, show |
| amplifier-turn-on-delay | Allows the user to configure the timer value for the pre-amplifier of RD20TM card. The value can be within the range of 0 to 24 minutes. By default, the value is 0 minutes. | uint16 (range 0..24) | 0 | set, show |
| amplifier-turn-on-remain | Display the remaining time of the amplifier-turn-on-delay timer. This attribute is applicable to RD20TM card. | uint16 | 0 | show |
| forced-shutdown | For cards with dual-band, one amplifier can be forced to be shutdown by setting this attribute to 'true'. | true, false | false | set, show |
| control-mode | Defines whether amplifier gain is automatically set by system or manually. The attribute auto-max-pw is the auto mode targeting maximum output power. | auto-max-pw,manual | auto-max-pw | set, show |
| amp-control-support | Whether 'control-mode' can be configured as 'auto-max-pw' or not. | • auto - Manual and auto-max-pw 'control-mode' supported.<br>• manual-only - Only manual 'control-mode' supported. | auto | set, show |
| amplifier-mode | The operating mode of the amplifier (gain or power control). Only constant-gain is used. | constant-gain | constant-gain | show |
| pump-state | The amplifier's pump working status. | enabled, disabled | disabled | show |
| actual-transmission-band | Currently assigned transmission band. If amplifier is not at a degree, it will be 4.85 THz by convention. | • c-band-4.85THz - Standard C-band (4.85 THz).<br>• c-band-6.1THz - SuperC-band (6.1 THz).<br>• l-band-4.85THz - Standard L-band (4.85 THz). | c-band-4.85THz | set, show |
| gain-range-control | Control mode for the amplifier gain switch (for amplifiers with multiple gain ranges). In R6.0:<br>• if the control-mode is set to manual, the gain-range-control is automatically set to manual.<br>• if the control-mode is set to auto-max-pw, the gain-range-control is automatically set to auto. | auto, manual | auto | set, show |
| span-loss-control | Span Loss Control configuration:<br>• enabled: perform automatic Span Loss Control<br>• disabled: no Span Loss Control. This configuration is of particular relevance for very long links ( &gt; 36 dB). Only of relevance for preamplifiers and inline amplifiers (not boosters). i Note: span-loss-control must be set to enabled when Span Loss Reference is set to measured; when Span Loss Reference is set to configured, span-loss-control can only be disabled for very long links (&gt; 36 dB). When Span Loss Reference is changed from configured to measured, span-loss-control will be automatically set to enabled. | enabled, disabled | enabled | set, show |
| gain-range-target | Applicable for manual gain-range-control:<br>• standard – single range amplifier working range.<br>• low – the low range for multi working range.<br>• high – the high range for multi working range. For gain-range-control = manual, note on default value: cards with low and high range, default gain-range-target is low if the configured gain-target is within the limits of the supported-gain-range min./ max. | standard, low,high | standard | set, show |
| gain-range-actual | The current working gain range. | standard, low,high | standard | show |
| gain-target | Applicable for manual control mode. Used for setting the gain to the amplifier for constant-gain mode. | 0 .. 40 dB | 0.0 | set, show |
| gain-operating | Operating gain of the amplifier that is the actually configured gain on the amplifier. When card is plugged out, or EDFA disabled, gain-operating is 0.0. | 0 .. 40 dB | 0.0 | show |
| optimum-edfa-gain | System reports the optimum EDFA gain the required equipped EDFA has. By convention, the system reports 0 dB when the card is not required equipped. | decimal value in dB. | n/a | show |
| gain-adjustment | Applicable for auto control mode. The gain offset is defined by the user. The value is used for adjustment of gain when the amplifier is in automatic control mode, the automatically calculated gain will include offset of this attribute. Only supported on amplifiers with 'function' = 'pa' or 'inline' | [-20.00..20.00] | 0 | set, show |
| amplifier-type | Type of the amplifier HW. | • fixed-gain-EDFA<br>• variable-gain-EDFA | n/a | set, show |
| output-power-mon | Monitored aggregate signal output power [dBm]. -99.00 means no power | [-99.00..99.00] | -99 | show |
| output-power-mon-with-ase | Monitored aggregate total output power including both signal and ASE. -99.00 means no power. | [-99.00..99.00] | -99 | show |
| input-power-mon | Monitored aggregate input power.-99.00 means no power. | [-99.00..99.00] | -99 | show |
| output-voa-attenuation | Applicable for manual control mode: target VOA attenuation at output of the amplifier (line padding VOA). Applicable if the amplifier function is 'ba' or if amplifier/ supporting-card is ILAx. | [0..30]dB | 0 | set, show |
| voa-control-mode | Type of VOA control mode:<br>• manual - Manual target attenuation.<br>• constant-power - Constant Power. | • manual<br>• constant-power | constant-power | set, show |
| output-voa-actual | Actual VOA attenuation at output of the amplifier. i Note: The attribute is not-applicable whenever the card is (required equipped but) not actually equipped, otherwise, a value range between 0 and 55 dB should be reported. Applicable if the amplifier function is 'ba' or if amplifier/ supporting-card is ILAx. | • not-applicable<br>• [0..55]dB | not-applicable | show |
| power-before-output-voa | Measured optical power before output VOA [dBm]. Applicable if the amplifier function is 'ba' or if amplifier/ supporting-card is ILAx. | [-99.00..99.00] dBm | -99 dBm | show |
| interstage-support | True if interstage port is supported in this amplifier. | true, false | false | show |
| interstage-loss | Interstage loss detected by the Power Control. In R6.0, the attribute is only relevant when node-type = ILA. | decimal [dB] | 0 | set, show |
| tilt-control-mode | Specify the gain tilt control mode. Defines whether amplifier tilt is automatically set by system or configured manually by the user. i Note: When amplifier function is 'add'/ 'drop' the only option is 'manual'. | • manual – User manually controls amplifier tilt.<br>• auto – System implicitly control amplifier tilt per configured fiber parameters. • auto-planned – System implicitly controls amplifier tilt per planning tool configured parameters. | auto | set, show |
| tilt-target | Target gain tilt of the amplifier. Applicable for manual control mode. Changing the attribute: a warning "This attribute may be traffic affecting" is issued. | [-5..5] dB (dB over operating wavelength band.) | 0dB | set, show |
| tilt-adjustment | Used to offset the target tilt when tilt-control-mode = 'auto' / 'auto-planned'. The actual tilt may differ from the requested tilt-adjustment. | [-5..5] dB (dB over operating wavelength band.) | 0dB | set, show |
| tilt-actual | Spectrum Tilt (measured by the EDFA). A 0dB reading indicates: no tilt, or amplifier not available. | decimal [dB] | 0dB | show |
| raman-signal-gain | Raman Gain of C-Band (signal).<br>• If there is a fiber-connection from/to Raman, the raman-signal-gain at amplifier needs to be appropriately configured autonomously.<br>• If there is no fiber-connection from/to Raman, the user must take the raman amplifier raman-signal-gain attribute value and configure it on the amplifier (i.e. on this attribute). | [0..30]dB | n/a | set, show |
| raman-osc-gain | Required when Raman backward pumping is deployed. The value is entered by the user in case the Raman card and pre-amplifier are in different NEs, otherwise it is provided by the system. | [0..30]dB | n/a | set, show |
| olos-shutdown-soak-timer | On input OLOS, the system soaks for the specified time (in milliseconds), and if the fault still persists, it triggers the consequent action (shutdown). The range of this integer is from 0 to 2000 milliseconds. The attribute is visible at:<br>• RD20TM booster<br>• CAD10A amplifier whose function is 'drop' | uint16 (0.. 2000) | 0 | set, show |
| control-state | Indicates the current state of the power control adjustment for the preamplifier:<br>• unknown : default value, awaiting update.<br>• not-applicable : if in manual control-mode. • stopped : control loop suspended due to a fault disable condition.<br>• converged : three consecutive control cycles with no adjustments.<br>• fine-tuning : less than 0.5dB from target.<br>• adjusting : greater than 0.5dB from target. | • unknown<br>• not-applicable<br>• stopped<br>• converged<br>• fine-tuning<br>• adjusting | unknown | show |
| olos-shutdown-disable | If it is set to be true, on input OLOS, EDFA shutdown does not depend on absence of input light. It is visible at:<br>• pre-amplifier of RD20TM;<br>• CAD10A amplifier whose function is 'add'. When configured to true, the EDFA is not shut down if it has no input light. In this case, the non-service affecting alarm OLOS- SHUTDOWN-DISABLED is raised. | true, false | false | set, show |
| control-speed-factor | Control speed factor for the DGE power control algorithm. The value is conveyed to system power control on the EDFA object. | decimal64 (range: 0.01..5.00) | 1.00 | set, show |

<!-- page 204 -->

#### Examples

This example shows how to list the actual transmission band of an amplifier:

```
show amplifier actual-transmission-band
amplifier           actual-transmission-band
------------------  ------------------------
amplifier-1-1-add   c-band-6.1THz
amplifier-1-1-drop  c-band-6.1THz
amplifier-1-3-ba    c-band-6.1THz
amplifier-1-3-pa    c-band-6.1THz
amplifier-1-8-ba    c-band-4.85THz
amplifier-1-8-pa    c-band-4.85THz
```

<!-- page 205 -->
