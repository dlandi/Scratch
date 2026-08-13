---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.223. otdr'
source_lines: 17555-17622
---

## 6.223. otdr

#### Command Description

The commands described in this section are used to add, delete, set or show the OTDR function.

#### Command Syntax

```
add otdr-<name> [admin-state <value>] [label <value>] [alarm-report-control <value>]
delete otdr-<name>
set otdr-<name> [admin-state <value>] [label <value>] [alarm-report-control <value>]
show otdr-<name> [supporting-card] [supporting-input-port] [supporting-output-port] [AID] [admin-state] [oper-state] [avail-state] [label]
[alarm-report-control] [otdr-state] [otdr-measurement-time] [otdr-file-prefix-requested] [otdr-error] [otdr-laser-state] [otdr-measurement-port]
[otdr-measurement-direction] [otdr-ongoing-measurement-profile]
```

#### Command Usage Details

**Table 533: otdr Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 534: otdr Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

<!-- page 855 -->

**Table 535: otdr Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-input-port | Input port that holds this facility. | String (length 0..64) | n/a | show |
| supporting-output-port | Output port that holds this facility. | String (length 0..64) | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| otdr-state | Indicates the current status of the OTDR. The status change will trigger change notification: not-available – Status is not available idle – Idle status measuring – Measurement is ongoing finished – Measurement has completed fail – Measurement has failed | • not-available<br>• idle<br>• measuring<br>• finished<br>• fail | not-available | show |
| otdr-measurement-time | Indicates the time remaining in current measurement running. | integer | 0 | show |
| otdr-file-prefix-requested | Indicates the requested file name prefix for RD66 and D2ILA OTDR test results. Synced from otdr-file-prefix. Only applicable for RD66 and D2ILA cards. | string (length 0..256) | n/a | show |
| otdr-error | Error message produced when the measurement ends with error. | string | n/a | show |
| otdr-laser-state | Indicates the current status of the OTDR laser. | • not-available<br>• enabled<br>• disabled | not-available | show |
| otdr-measurement-port | Indicates the OTDR port number where a measurement is currently taking place.<br>• 0 - indicates that the card is not measuring any port;<br>• non-zero - indicates the OTDR port number where a measurement is currently taking place | string (port object) | 0 | show |
| otdr-measurement-direction | Indicates the Scan direction:<br>• not-available: Indicates scan is not running.<br>• tx: Indicates scan is running in the tx direction.<br>• rx: Indicates scan is running in the rx direction. | • not-available<br>• tx<br>• rx | not-available | show |
| otdr-ongoing-measurement-profile | Displays which pre-defined OTDR measurement profile is in progress:<br>• none: Indicates automatic otdr scan is not running.<br>• short: Indicates baseline otdr scan is running for short distance.<br>• medium: Indicates baseline otdr scan is running for medium distance. • long: Indicates baseline otdr scan is running for long distance.<br>• raman-precheck1: Indicates raman-precheck otdr first scan running.<br>• raman-precheck2: Indicates raman-precheck otdr second scan running.<br>• raman-precheck3: Indicates raman-precheck otdr third scan running. | • none<br>• short<br>• medium<br>• long<br>• raman-precheck1<br>• raman-precheck2<br>• raman-precheck3 | none | show |

#### Examples

This example shows how to list OTDR entities and display their attributes:

```
show otdr
```

<!-- page 860 -->
