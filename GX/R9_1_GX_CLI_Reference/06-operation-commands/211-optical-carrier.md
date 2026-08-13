---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.211. optical-carrier'
source_lines: 16797-16903
---

## 6.211. optical-carrier

#### Command Description

These commands are used to add, edit and show the attributes of an optical carrier. The delete command is used to delete an optical carrier from the configuration.

#### Command Syntax

```
set optical-carrier-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [frequency <value>] [frequency-offset <value>]
[tx-power <value>] [pre-fec-q-sig-deg-threshold <value>] [pre-fec-q-sig-deg-hysteresis <value>] [carrier-mode <value>] [media-interface
<value>] [grid-spacing <value>] [tx-cd <value>] [post-fec-q-sig-deg-threshold <value>] [post-fec-q-sig-deg-hysteresis <value>] [rate <value>]
[modulation-format <value>] [rx-frequency <value>] [rx-attenuation <value>] [tx-filter-roll-off <value>] [preemphasis <value>] [preemphasis-value
<value>] [cd-range-low <value>] [cd-range-high <value>] [cd-compensation-mode <value>] [cd-compensation-value <value>] [fast-sop-mode <value>]
[BICHM <value>] [propagate-shutdown <value>] [propagate-shutdown-holdoff-timer <value>] [enable-advanced-parameters <value>] [sop-data-collection
<value>] [loopback <value>]
show optical-carrier-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label]
[admin-state] [oper-state] [avail-state] [managed-by] [alarm-report-control] [frequency] [frequency-offset] [wavelength] [tx-power]
[pre-fec-q-sig-deg-threshold] [pre-fec-q-sig-deg-hysteresis] [carrier-type] [carrier-mode] [capacity] [client-mode] [baud-rate] [application]
[sop-tracking-mode] [media-interface] [grid-spacing] [spectral-bandwidth] [tx-cd] [dgd-high-threshold] [post-fec-q-sig-deg-threshold]
[post-fec-q-sig-deg-hysteresis] [rate] [modulation-format] [line-encoding] [rx-frequency] [rx-attenuation] [tx-filter-roll-off] [preemphasis]
[preemphasis-value] [cd-range-low] [cd-range-high] [cd-compensation-mode] [cd-compensation-value] [fast-sop-mode] [BICHM] [propagate-shutdown]
[propagate-shutdown-holdoff-timer] [actual-rx-frequency] [actual-frequency] [enable-advanced-parameters] [sop-data-collection] [circuit-id]
[sop-vector] [loopback]
```

#### Command Usage Details

**Table 508: optical-carrier Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 797 -->

#### Command Parameters

**Table 509: optical-carrier Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of grouping the basic optical carrier facility structure. | String (length 0...128) | n/a | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/nam e") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/n ame") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| frequency | The center frequency this carrier is tuned to. Zero means not configured. The range of the frequency depends on the module and pluggable: CHM1R:<br>• TOM-400G-C-DWDM (ZXS-C2DWDMFA-40): 191275000 to 196125000 MHz (supports fine tune, all values are supported within the range)<br>• TOM-400GXR-C-DWDM (XRCFCD400PAE5INZ): 191325000 to 196100000 MHz (step: 6.25 GHz) UTM2:<br>• TOM-100G-C-DWDM (ZXS-C2DWDMFA-10) and TOM-200G-C-DWDM (ZXS-C2DWDMFA-20): 191250000-196100000 MHz (step: 1 MHz)<br>• TOM-100GFI-C-DWDM (ZXS-C2DWDMFI-10): 191300000-196100000 MHz (step: 50 GHz)<br>• SFP+ pluggables TOM-10GMR-S-DWDM (81.71T- SPDWDM-R6 and 81.71T-SPDWDMHP-R6): 191700000-196050000 MHz (step: 50 GHz)<br>• TOM-400G-C-DWDM (ZXS-C2DWDMFA-40): 191275000-196125000 MHz (step: 1 MHz) CHM2TX: 191292500-196137500 MHz (step: 1 MHz) SPN2/SPN2C:<br>• TOM-400G-Q-DWDM (ZXS-QDDWDMFA-40): 191275000 to 196125000 MHz (step: 6.25 GHz)<br>• TOM-400GXR-Q-DWDM (XRQDCD400PAE5INZ): 191325000 to 196100000 MHz (step: 6.25 GHz) CHM7X: 190625000 - 196725000 MHz (step: 50 MHz) CHM6: 191333000 to 196117000 MHz (step: 50 MHz) CHM7: 190625000 - 196725000 MHz (step: 50 MHz) | Frequency (range: 0 \| 191275000..196125000 MHz) | m0 | set, show |
| frequency-offset | A super set range for line and client side carrier, specific sub-range is depend on application. Frequency-offset can be used for bright tuning of the wavelengths. Once set, the frequency will slowly change (over 1-10s) without affecting service. | int16 (range: -6000..6000 MHz) | 0 | set, show |
| wavelength | The wavelength of the optical carrier. | wavelength (nm) | n/a | show |
| tx-power | The optical carrier's transmit power into the fiber from the transponder's optics. The accuracy of the Tx Power can be adjusted in steps of 0.5 dBm. The range of the transmit power depends on the module and pluggable: CHM1R:<br>• TOM-400G-C-DWDM (ZXS-C2DWDMFA-40): -10 to 1<br>• TOM-400GXR-C-DWDM (XRCFCD400PAE5INZ): 0 to -16 UTM2:<br>• TOM-100G-C-DWDM (ZXS-C2DWDMFA-10) and TOM-200G-C-DWDM (ZXS-C2DWDMFA-20): -15 to 0<br>• TOM-100GFI-C-DWDM (ZXS-C2DWDMFI-10): not configurable • SFP+ pluggables TOM-10GMR-S-DWDM (81.71T-SPDWDM-R6 and 81.71T-SPDWDMHP-R6): not configurable<br>• TOM-400G-C-DWDM (ZXS-C2DWDMFA-40): -10 to +1 CHM2TX: -6 to 0 SPN2/SPN2C:<br>• TOM-400G-Q-DWDM (ZXS-QDDWDMFA-40): -10 to +1<br>• TOM-400GXR-Q-DWDM (XRQDCD400PAE5INZ): -16.00 to 0 CHM7/CHM7X: -10 to 0 | Number (range: -55.0..55.00 dBm) The range of the tx-power depends on the module and pluggable. Refer to the description for the value ranges. | CHM1R:<br>• TOM-400G-C-DWDM (ZXS-C2DWDMFA-40): 1<br>• TOM-400GXR-C-DWDM (XRCFCD400PAE5INZ): 0 UTM2: -6.0 CHM2TX: 0 SPN2/SPN2C:<br>• TOM-400G-Q-DWDM (ZXS-QDDWDMFA-40): 1<br>• TOM-400GXR-Q-DWDM (XRQDCD400PAE5INZ): 0 CHM7/7X: -2 | set, show |
| pre-fec-q-sig-deg-threshold | The threshold based on which the PRE-FEC-Q-SIGNAL- DEGRADE alarm is raised. 0 implies threshold crossing alarming disabled. Specific sub-range is per carrier use-case. | decimal64, with 2 fraction-digits (range: 0\|5.60..9.60 dB) | 6 | set, show |
| pre-fec-q-sig-deg-hysteresis | Hysteresis to account for raising of the PRE-FEC-Q- SIGNAL-DEGRADE alarm. | decimal64, with 1 fraction-digit (range: 0.1..1.0 dB) | 0.5 | set, show |
| carrier-type | The type of the carrier. | ICE6 ZR ZR+ OTN | OTN | show |
| media-interface | Media interface type of ZR tom. | 400ZR-CFEC-DP-16QAM | 400ZR-CFEC-DP-16QAM | set, show |
| capacity | The net capacity of the optical carrier. | Number (Gbps) | n/a | show |
| baud-rate | The modulated symbol rate. | Number (GBaud) | n/a | show |
| grid-spacing | Fixed Grid tunability for new 3rd party TOM (GHz). | 100 75 50 33 25 12.5 6.25 3.125 | 100 | set, show |
| spectral-bandwidth | Spectral bandwidth associated with this carrier(s). | Number (GHz). | n/a | show |
| loopback | Loopback mode. Useful to debug on the fiber connection. | none facility terminal | none | set, show |
| tx-cd | The configured transmit pre-compensation chromatic dispersion. | decimal64, with 2 fraction-digits (range: -211000.00..211000.00 ps/nm) | 0.0 | set, show |
| dgd-high-threshold | The threshold to raise the DGD-OORH alarm. | Number (range: 180..350 ps) | 300 | set, show |
| post-fec-q-sig-deg-threshold | The threshold based on which the PRE-FEC-QSIGNAL- DEGRADE alarm is raised. | Number (range: 12.5..18.0 dB) | 18 | set, show |
| post-fec-q-sig-deg-hysteresis | Hysteresis to account for raising of the PRE-FECQ- SIGNAL-DEGRADE alarm. | decimal64, with 1 fraction-digit (range: 0.1..3.0 dB) | 2.5 | set, show |
| rate | Carried signal basic rate class (in Gbit/s units). | decimal64, with 3 fraction-digits | n/a | set, show |
| modulation-format | Current modulation format. | not-applicable DP-QPSK DP-8QAM DP-16QAM BPSK | n/a | set, show |
| line-encoding | Currently line-encoding mode. | non-differential differential | non-differential | show |
| actual-frequency | A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized frequency. | Frequency (range: 0 \| 191275000..196125000 MHz) | 0 | show |
| rx-frequency | The rx laser frequency. A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized frequency (If 0, rx laser frequency is same as tx laser frequency). | Frequency (range: 0 \| 191275000..196125000 MHz) | 0 | add, set, show |
| actual-rx-frequency | A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized frequency. | Frequency (range: 0 \| 191275000..196125000 MHz) | 0 | show |
| rx-attenuation | Supports configurable optical attenuation at receiver side which is based on the hardware capability on the port. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM. | optical-power (range: 0.0..10.0 dBm) | 0.0 | show |
| tx-filter-roll-off | Transmitter filter roll off factor. For the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM, this parameter is read-only and the default value is 0.04. | decimal64, with 2 fraction-digits (range: 0.01 .. 1.0) (For FlexO/oFEC mode, the recommend range is 0.05 to 0.2) | 0.2 | set, show |
| preemphasis | Preemphasis of transmitted signal. i Note: This parameter is not configurable for the 1830 GX G30 SPN2, SPN2C, or CHM1R card with line pluggable TOM-400G-Q-DWDM. | enabled, disabled | enabled | set, show |
| preemphasis-value | Preemphasis of transmitted signal. i Note: This parameter is not configurable for the 1830 GX G30 SPN2, SPN2C, or CHM1R card with line pluggable TOM-400G-Q-DWDM. For the 1830 GX G30 SPN2, SPN2C, or CHM1R card with line pluggable TOM-400G-Q-DWDM, this parameter is read-only and the default value is 0.0. | decimal64, with 1 fraction-digit (range: 0.7..1.3) | 1.0 | set, show |
| cd-range-low | Low value of chromatic dispersion search range. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM. | int32 (ps/nm) | -45000 (the default value depends on the configured service type.) | set, show |
| cd-range-high | High value of chromatic dispersion search range. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM. | int32 (ps/nm) | -45000 (the default value depends on the configured service type.) | set, show |
| cd-compensation-mode | Chromatic dispersion compensation value source mode. | auto, manual | auto | set, show |
| cd-compensation-value | Manual chromatic dispersion compensation value. | int32 (ps/nm) | n/a | set, show |
| fast-sop-mode | Specify if enable fast SOP (state of polarization) change tracking; if enabled, the interface will tolerate very fast SOP and transient. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM. | enabled, disabled | disabled | set, show |
| BICHM | The BICHM (bit interleaved coded hybrid modulation) incremental step in 1/128 bits/symbol added to base modulation bits/symbol for the hybrid modes modulation-format.<br>• 0: Base modulation format bits/symbol;<br>• 1: 1/128 bits/symbol added to base modulation format bits/symbol;<br>• ...<br>• 127: 127/128 bits/symbol added to base modulation format bits/symbol. | int32 (range: 0..127) | 64 | set, show |
| propagate-shutdown | When the attribute value is set to yes, the transmit laser will be shutdown if the whole service of the direction has signal failure, the function mainly used in regeneration node to propagate signal failure as LOS. i Note: This parameter is not configurable for the SPN2 or SPN2C card. | enabled, disabled | disabled | set, show |
| propagate-shutdown-holdoff-timer | The hold off time of propagate shutdown. i Note: This parameter is not configurable for the SPN2 or SPN2C card. | Number (range: 0..2000 milliseconds) | 0 | set, show |
| enable-advanced-parameters | Controls enabling/disabling of configuring advanced parameters for this object. | true, false | false | set, show |
| sop-data-collection | Controls enabling/disabling sop data collection, providing the collection interval in ms. | disabled, or a number in the range of 10..500ms | disabled | set, show |

#### Examples

This example shows how to set an optical carrier administrative state to unlock in 1830 GX G40 node:

```
set optical-carrier-group1 admin-state unlock
```

This example shows how to set an optical carrier administrative state to lock in 1830 GX G30 node:

```
set optical-carrier-1-1-1 admin-state lock
```

<!-- page 807 -->
