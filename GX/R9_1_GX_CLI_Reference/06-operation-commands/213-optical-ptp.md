---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.213. optical-ptp'
source_lines: 16954-17021
---

## 6.213. optical-ptp

#### Command Description

This command is used to edit, or show an optical ptp attributes.

#### Command Syntax

```
set optical-ptp-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [target-power-setting <value>] [fix-rx-attenuation
<value>] [fix-tx-attenuation <value>] [band-required <value>] [fiber-length-offset <value>]
show optical-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [ptp-type] [port-direction-convention] [target-power-setting] [laser-state]
[ase-source-connected] [actual-power-support] [power-actual-rx] [power-actual-tx] [fix-rx-attenuation] [fix-tx-attenuation <value>]
[monitoring-state] [band-required] [bands-supported] [fiber-length-offset]
```

#### Command Usage Details

**Table 512: optical-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 513: optical-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object. • allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| ptp-type | Type of Optical PTP. | • dwdm-line -- DWDM line PTP<br>• dwdm -- System side DWDM, or other filter DWDM PTP<br>• osc -- OSC PTP<br>• sposc -- SPOSC PTP<br>• ade -- ADE: Add/ Drop or Express PTP<br>• ad -- ADE: Add/ Drop PTP (no express option)<br>• fac -- BAX Facility port PTP<br>• ase-idler -- ASE Idler PTP | n/a | show |
| port-direction-convention | IOA port (PTP) direction convention. ( Only of relevance for ports exposing OTS and OMS-nim, i.e. ILA. ) | string | n/a | show |
| target-power-setting | This attribute is applicable to both HSC OLS and Standard OLS modes . It defines how the target power of the drop OXcon is determined:<br>• auto, the system automatically derives the OXcon target-actual-power-src based on pre-defined PSD (hence based on NMC width only). This attribute is applicable in HSC OLS mode.<br>• manual, the system takes the user configuration on OXcon target-output-power-src. This attribute is applicable in both HSC OLS and Standard OLS modes.<br>• auto-max, the system automatically calculates the target power of the drop OXcon according to the maximum power budget capability of the network element. This attribute is applicable to Standard OLS mode only for CDC8D6 drop ports in R9.0 | • auto<br>• manual<br>• auto-max | • auto (for RD66TM in HSC OLS mode)<br>• auto-max (for CDC8D6 only in Standard OLS mode) | set, show |
| laser-state | The emitting pump (e.g. booster) laser state. RD amplifiers: source (Tx) pump disabled. Raman modules: Pump Laser, and actual traffic emitted from dwdm-line port: sink or source. Only of relevance for DWDM line ports | disabled, enabled | disabled | show |
| ase-source-connected | Displays whether PTP is connected from an ASE Idler (connection from 'Out') or not:<br>• true: A fiber connection is provisioned between OTSCS and RD ADE port corresponding to this optical-ptp.<br>• false: there are no fiber connection is provisioned between OTSCS and RD ADE port corresponding to this optical-ptp. | • true<br>• false | false | show |
| actual-power-support | Port power monitoring support. | not-applicable - Not available or not applicable. power-rx-tx - Power actual Rx and Tx. power-rx - Power actual Rx only. ocm - OCM dependent power actual. power-tx - Power actual Tx only. | not-applicable | show |
| power-actual-rx | Optical power received, where applicable. | decimal64, with 2 fraction-digits (range: -99.00..99.00 dBm) | -99dBm | show |
| power-actual-tx | Optical power transmitted, where applicable. | decimal64, with 2 fraction-digits (range: -99.00..99.00 dBm) | -99dBm | show |
| fix-rx-attenuation | Fixed Attenuator before port Rx. 0 (dB) is equivalent to no fixed attenuator. | decimal64, fraction-digits 2; (range 0..30 dB) | 0 | set, show |
| fix-tx-attenuation | Fixed Attenuator after port Tx. 0 (dB) is equivalent to no fixed attenuator. i Note: The parameter fix-tx-attenuation is only visible when ops.port-expansion=y-cable is configured. | decimal64, fraction-digits 2 (range 0..30 dB) | 0 | set, show |
| monitoring-state | System reports this attribute, to indicate whether the optical-ptp is intended to be in use (instead of simply being pre-provisioned). When optical-ptp is created the monitoring-state needs to be calculated:<br>• disabled: for a combination of card/ptp-type.<br>• enabled: other cases. i Note: In scenarios where only the OSC path between RD sleds is provisioned (without a complete optical data path), the Control Plane (CP) alarms NO- OSPFV2-NEIGHBOR, NO-OSPFV3-NEIGHBOR, DUPLICATE-IPV6-ADDR- DETECT, and COMM- CHNL-DOWN are not reported if the optical-ptp monitoring state is disabled. i Note: monitoring-state of CDC DWDM PTP is enabled only when it is part of an OXcon path. Otherwise, it is disabled. When it is enabled, users can view alarms on the optical-PTP and all its supported facilities, most notably OLOS alarms. | enabled, disabled | enabled | show |
| band-required | Required Transmission Band(s) for the DWDM-line port.<br>• not-applicable - Required transmission band(s) not applicable.<br>• standardC-band - Required transmission band StandardC-band (4.85 THz or HSC OLS StandardC-band).<br>• superC-band - Required transmission band SuperC-band (6.1 THz).<br>• standardL-band - Required transmission band StandardL-band (HSC OLS).<br>• standardC-standardL-bands - StandardC-band (HSC OLS standardC-band) and StandardL-band for HSC OLS. | • not-applicable<br>• standardC-band<br>• superC-band<br>• standardL-band<br>• standardC-standardL-bands | n/a | set, show |
| bands-supported | List of bands supported by a card's port. Only applicable to optical dwdm(-line) and AD/ ADE ports.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz).<br>• superC-band - SuperC-band (6.1 THz). • standardL-band - Standard L-band (4.85 THz).<br>• standardC-standardL-band - Standard C or Standard L band. | • not-applicable<br>• standardC-band<br>• superC-band<br>• standardL-band<br>• standardC-standardL-bands | standardC-band | show |
| fiber-length-offset | Fiber patch cord length between the Raman DWDM port and the base card DWDM line port. | • not-applicable (corresponding to 0.50 m)<br>• decimal64, range (0..100.00) m | not-applicable | set, show |

#### Examples

This example shows how to set an optical-ptp attribute in a 1830 GX G30 node:

```
set -f optical-ptp-1-6-dwdm fix-rx-attenuation 10
```

<!-- page 820 -->
