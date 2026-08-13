---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.136. interlaken'
source_lines: 12553-12628
---

## 6.136. interlaken

#### Command Description

The commands described in this section are used to set or show the SPN2 `interlaken` attributes.

#### Command Syntax

```
set interlaken-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [loopback <value>]
show interlaken-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [capacity] [loopback]
```

#### Command Usage Details

**Table 360: interlaken Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 361: interlaken Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| loopback | Loopback mode. Useful to debug on the fiber connection:<br>• none-Connection is not being tested.<br>• facility-Test towards facility side.<br>• terminal-Test towards terminal side. | • none<br>• facility<br>• terminal | none | set, show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system | system | show |
| capacity | Total capacity for the interlaken interface. | 500 Gbit/s | 500 Gbit/s | show |

#### Examples

The following command shows how to set the interlaken interface **label** to *interlaken\_66\_1*:

```
set interlaken-66-1-8 label interlaken_66_1
```

| The following command shows how to show the SPN2 | interlaken | attributes: |
| --- | --- | --- |
| show interlaken-66-1-8 |  |  |

The following output is displayed:

```
  interlaken-66-1-8
  supporting-card                66-1
  supporting-port                8,9
  supporting-facilities          trib-ptp-66-1-8,trib-ptp-66-1-9
  supported-facilities
  AID                            '66-1-8'
  label                          'interlaken_66_1'
  admin-state                    unlock
  oper-state                     disabled
  avail-state                    'supporting-faulted automatic out-of-service'
  managed-by                     system
  alarm-report-control           allowed
  capacity                       500.000 Gbit/s
  loopback                       none
```

<!-- page 567 -->
