---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.358. trib-ptp'
source_lines: 26738-26834
---

## 6.358. trib-ptp

#### Command Description

These commands are used to set or show configuration of the tributary client physical termination layer which exists between the transceiver equipment and the immediate protocol layer (Ethernet, SONET/SDH, OTUk, etc.). This entity provides all physical layer configurations that are applicable to the upper-layer client. The delete command is used to remove a tributary client physical termination from the configuration.

**Note:** Operation of the ZXS-QDZRZZZZ-00 where the ambient temperature exceeds 40° C (104° F) is not supported. In such cases, the ZXS- QDZRZZZZ-00 is placed in low power mode and a CFG-MSMT alarm is raised by the management interfaces; any attempt to auto-provision or manual/pre-provision the ZXS-QDZRZZZZ-00 will be denied.

#### Command Syntax

```
add trib-ptp-<name> [label <value>] [admin-state <value>] [auto-in-service-enabled <value>] [valid-signal-time <value>] [alarm-report-control
<value>] [service-type <value>] [tributary-disable-action <value>] [tributary-disable-holdoff-timer <value>] [near-end-tda <value>]
[tda-degrade-mode <value>] [forward-defect-trigger <value>] [power-threshold-low-offset <value>] [power-threshold-high-offset <value>]
[egress-port-list <value>]
```

**Note:** The add command for trib-ptp works in merge mode only. Using the -m flag performs a merge, which is the best effort add. If the target entity does not exist, it is created. If it exists, it is updated with any attributes present on the "add" command.

```
set trib-ptp-<name> [label <value>] [admin-state <value>] [auto-in-service-enabled <value>] [valid-signal-time <value>] [alarm-report-control
<value>] [service-type <value>] [tributary-disable-action <value>] [tributary-disable-holdoff-timer <value>] [near-end-tda <value>]
[tda-degrade-mode <value>] [forward-defect-trigger <value>] [power-threshold-low-offset <value>] [power-threshold-high-offset <value>]
[egress-port-list <value>]
show trib-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [auto-in-service-enabled] [valid-signal-time] [remaining-valid-signal-time] [alarm-report-control] [service-type]
[tributary-disable-action] [tributary-disable-holdoff-timer] [near-end-tda] [tda-degrade-mode] [forward-defect-trigger] [power-threshold-low]
[power-threshold-low-offset] [power-threshold-high] [power-threshold-high-offset] [egress-port-list]
delete trib-ptp-<name>
```

**Note:** The `tributary-disable-holdoff-timer` is applicable only when the TDA action is set to `Laser-turnoff`, hence it is recommended to hold the optical signal as it is until the configured hold-off timer expires after which you can turn off the laser.

#### Command Usage Details

**Table 822: trib-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 823: trib-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the object. | String (length 0..64) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/nam e") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/n ame") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| auto-in-service-enabled | Auto-in-service switch for this facility. | true, false | n/a | add, set, show |
| valid-signal-time | Configurable time that represents a detection of a valid signal. Used for auto-in-service mechanism. | Number (range: 1..7200 minutes) | 480 | add, set, show |
| remaining-valid-signal-time | Actual remaining time for this facility to be automatically enabled by the auto-in-service mechanism. | Number (range: 1..7200 minutes) | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| service-type | The protocol type of the client that is being transported via the tributary optical transceiver module (TOM). | 100GBE 400GBE OTU4 4x100GBE not-applicable OTU2 OTU2e 1GBE 10GBE OC48 OC192 STM16 STM64 4x10G 4x10GBE 6 not-applicable | 100GBE (for 1830 GX G30) not applicable (for 1830 GX G40, except CHM6) | add, set, show |
| tributary-disable-action | Indicates what action the network element performs towards the client equipment (connected over the TOM) when a line-side failure is observed. This includes shutting off the laser or inserting an appropriate replacement signal. i Note: For more information about the actions supported for different types of services on a card, refer to 1830 GX System Description Guide. | • laser-shut-off (default)<br>• none<br>• odu-ais<br>• send-ais-1<br>• send-gais<br>• send-idles<br>• send-lf<br>• send-ms-ais<br>• send-nos for 1830 GX G30 only | laser-shut-off | add, set, show |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 6 The `not-applicable` option is not valid for CHM7X in release R9.1 because regeneration is not yet supported on CHM7X.
| tributary-disable-holdoff-ti mer | The hold off time of client shutdown or replacement signal at egress direction. 0 means holdoff functionality disabled. | uint16 (range: 0..10000 milliseconds) | 0 | add, set, show |
| near-end-tda | The switching of near end TDA. | enabled, disabled | disabled | add, set, show |
| tda-degrade-mode | The switching of defect BERSD-ODU trig ALS. | enabled, disabled | disabled | add, set, show |
| forward-defect-trigger | Indicates on the egress, if NE receives a client forward defect (e.g., LF, ODU-AIS) whether to let it flow through towards the line side (network side) or trigger an egress TDA action. | true, false | true | add, set, show |
| power-threshold-low | The default system threshold (known as 'Sensitivity') that triggers the OPR-OORL alarm (i.e., when the optical power received is below this value). Note that this is hardware dependent, based on the type of the optical transceiver (TOM). | decimal64 with 2 fraction-digits (range: -55.0..55.00 dBm) | n/a | show |
| power-threshold-low-offset | A user configurable attribute that results in the 'effective lower threshold' based on which the system raises the OPR-OORL alarm. The effective threshold will be (threshold-low + threshold-low-offset). | decimal64 with 2 fraction-digits (range: -10.00..10.00 dB) | 0.0 | add, set, show |
| power-threshold-high | The default system threshold (known as 'Overload') that triggers the OPR-OORH alarm (i.e., when the optical power received is greater than this value). Note that this is hardware dependent, based on the type of the optical transceiver (TOM). | decimal64 with 2 fraction-digits (range: -55.0..55.00 dBm) | n/a | show |
| power-threshold-high-offset | A user configurable attribute that results in the 'effective upper threshold' based on which the system raises the OPR-OORH alarm. The effective threshold will be (threshold-high + threshold-high-offset). | decimal64 with 2 fraction-digits (range: -10.00..10.00 dB) | 0.0 | add, set, show |
| 7 egress-port-list | A list of port AIDs that are bound to this trib-ptp for diverse-routing. | String with length 1..32 of a list of port AIDs that are bound to this trib-ptp for diverse-routing. | n/a | add, set, show |
| command flag | -m | Merge configuration (the command will not fail if the entity already exists) | n/a | n/a |

#### Examples

This example shows how to set trib-ptp in a 1830 GX G40 node:

```
set trib-ptp-1-5-T2 tributary-disable-action laser-shut-off
```

This example shows how to enable the trib-ptp tributary-disable-holdoff-timer in a 1830 GX G40 node:

```
set trib-ptp-1-6-T10 tributary-disable-holdoff-timer 100
```

**Note:** In case of 4x100GE, service-type of parent tribptp cannot be set to not-applicable. It is valid only for sub-tribptp; for example, if a 1-6-T1 configured 4x100GE, 1-6-T1 service-type = not-applicable cannot be set, but can be set to not-applicable for 1-6-T1x.

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 7 egress-port-list attribute is applicable to CHM7 and CHM7X currently.

<!-- page 1310 -->
