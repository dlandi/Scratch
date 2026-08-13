---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.153. line-ptp'
source_lines: 13530-13627
---

## 6.153. line-ptp

#### Command Description

These commands are used to add/set/show/delete a line ptp.

#### Command Syntax

```
add line-ptp-<name> [label <value>] [admin-state <value>] [auto-in-service-enabled <value>] [valid-signal-time <value>] [alarm-report-control
<value>] [service-type <value>] [line-system-mode <value>] [power-threshold-low-offset <value>] [power-threshold-high-offset <value>]
set line-ptp-<name> [label <value>] [admin-state <value>] [auto-in-service-enabled <value>] [valid-signal-time <value>] [alarm-report-control
<value>] [service-type <value>] [line-system-mode <value>] [power-threshold-low-offset <value>] [power-threshold-high-offset <value>]
show line-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [auto-in-service-enabled] [valid-signal-time] [remaining-valid-signal-time] [alarm-report-control] [service-type]
[line-system-mode] [available-resources] [used-resources] [power-threshold-low] [power-threshold-low-offset] [power-threshold-high]
[power-threshold-high-offset]
delete line-ptp-<name>
```

#### Command Usage Details

**Table 395: line-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 396: line-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the facility. | String (length 0..64 characters) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | card | n/a | show |
| supporting-port | Ports that hold this facility | String (length 0..64 characters) | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| label | User defined label. | string (length 0..256 characters) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. | system, user | system | show |
| auto-in-service-enabled | Auto-in-service switch for this facility. | true, false | false | add, set, show, |
| valid-signal-time | Configurable time that represents a detection of a valid signal. Used for auto-in-service mechanism. | Number (range: 1..7200 minutes) | 480 | add, set, show |
| remaining-valid-signal-time | Actual remaining time for this facility to be automatically enabled by the auto-in-service mechanism. | Number (range: 1..7200 minutes) | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed | add, set, show |
| service-type | service-type to provision line side service. CHM1R:<br>• DP-16QAM-400G-OpenZR+<br>• DP-16QAM-400G<br>• DP-16QAM-E-400G<br>• DP-8QAM-300G<br>• DP-QPSK-200G<br>• DP-8QAM-200G<br>• DP-16QAM-200G<br>• DP-QPSK-100G<br>• DP-16QAM-400G<br>• DP-16QAM-400G-EX<br>• DP-8QAM-300G<br>• DP-8QAM-300G-EX<br>• DP-QPSK-200G<br>• DP-QPSK-200G-EX<br>• DP-QPSK-100G<br>• DP-QPSK-100G-EX CHM2TX:<br>• DP-QPSK-200G<br>• DP-16QAM-200G<br>• DP-SP16QAM-300G • DP-16QAM-400G<br>• DP-16QAM-32QAM-500G SPN2/SPN2C:<br>• DP-16QAM-400G<br>• DP-16QAM-400G-EX<br>• DP-8QAM-300G<br>• DP-16QAM-200G<br>• DP-QPSK-200G<br>• DP-QPSK-100G<br>• DP-16QAM-100G (for line XR mode) UTM2:<br>• DP-QPSK-100G<br>• DP-16QAM-200G<br>• DP-8QAM-200G<br>• OTU4<br>• OTU2<br>• OTU2e i Note: Not applicable to CHM7X. | not-applicable DP-16QAM-200G DP-QPSK-100G DP-8QAM-200G DP-16QAM-100G DP-16QAM-100G-EX DP-16QAM-400G DP-8QAM-300G DP-QPSK-200G OTU2 OTU2e DP-16QAM-E-400G DP-16QAM-400G-OpenZR+ DP-QPSK-100G-EX DP-16QAM-400G-EX DP-8QAM-300G-EX DP-QPSK-200G-EX DP-SP16QAM-300G DP-16QAM-32QAM-500G | not-applicable | add, set, show |
| line-system-mode | Indicates the specific mode of power control configured on the L1 transponder, and specifically, on this particular SCG port within\n the L1 transponder. The attribute indicates the L1 &lt;-&gt; L0 local power controls to adjust the Tx power from the L1 transponder towards the L0 line-system card (such as a WSS or Mux or Amplifier). | openwave | openwave | add, set, show |
| power-threshold-low | The default system threshold (known as 'Sensitivity') that triggers the OPR-OORL alarm (i.e., when the optical power received is below this value). Note that this is hardware dependent, based on the type of the optical transceiver (TOM). | Number (range: -55.0..55.00 dBm) | -55.0 | show |
| power-threshold-low-offset | A user configurable attribute that results in the 'effective lower threshold' based on which the system raises the OPR-OORL alarm. The effective threshold will be (threshold-low + threshold-low-offset). | Number (range: -10.00..10.00 dB) | 0.0 | add, set, show |
| power-threshold-high | The default system threshold (known as 'Overload') that triggers the OPR-OORH alarm (i.e., when the optical power received is greater than this value). Note that this is hardware dependent, based on the type of the optical transceiver (TOM). | Number (range: -55.0..55.00 dBm) | -55.0 | show |
| power-threshold-high-offset | A user configurable attribute that results in the 'effective upper threshold' based on which the system raises the OPR-OORH alarm. The effective threshold will be (threshold-high + threshold-high-offset). | Number (range: -10.00..10.00 dB) | 0.0 | add, set, show |
| available-resources | Provide an aggregate view of all available resources on the DSP. | Number(range: 0-11 for DSP connected to Line Port L1, range: 12-23 for DSP connected to Line Port L2) | Not applicable | show |
| used-resources | Provide an aggregate view of all used resources on the DSP. | Not applicable | Not applicable | show |

<!-- page 610 -->

#### Examples

This example shows how to view available resources on the line-ptp entity:

```
show line-ptp-1-4-L1 available-resources
  line-ptp-1-4-L1
  available-resources      0,1,2,3,4,5,6,7,8,9,10,11
```

This example shows how to view a line-ptp entity and the information retrieved from 1830 GX G30 environment:

```
show line-ptp-1-2-1
  line-ptp-1-2-1
  supporting-card                1-2
  supporting-port                1
  supporting-facilities
  supported-facilities
  AID                            '1-2-1'
  label                          ''
  admin-state                    unlock
  oper-state                     enabled
  avail-state                    'normal in-service'
  managed-by                     system
  auto-in-service-enabled        false
  valid-signal-time              480 minutes
  remaining-valid-signal-time    0 minutes
  alarm-report-control           allowed
  service-type                   not-applicable
  power-threshold-low            -21.00 dBm
  power-threshold-low-offset     0.00 dB
  power-threshold-high           13.00 dBm
  power-threshold-high-offset    0.00 dB
```

<!-- page 611 -->
