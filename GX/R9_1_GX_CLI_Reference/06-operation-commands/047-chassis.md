---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.47. chassis'
source_lines: 6852-6961
---

## 6.47. chassis

#### Command Description

These commands are used to add, delete, edit or show the chassis attributes.

**Note:** In 1830 GX G40, to bring up a Native ZR TOM, the user needs to set the chassis ambient temperature to 40C, see example 2. If this parameter is not set, the system will raise a port config mismatch alarm and the TOM cannot be provisioned.

#### Command Syntax

```
add chassis-<name> required-type <value> [expected-serial-number <value>] [alias-name <value>] [admin-state <value>] [alarm-report-control
<value>] [label <value>] [required-subtype <value>] [chassis-location <value>] [rack-name <value>] [position-in-rack <value>]
[expected-pem-type <value>] [expected-fan-type <value>] [pem-under-voltage-threshold <value>] [pem-over-voltage-threshold <value>]
[actual-power-draw-alarm-threshold <value>] [configured-ambient-temperature <value>] [filter-maintenance-interval <value>] [filter-insertion-date
<value>] [power-redundancy <value>] [power-limited <value>] [preferred-controller-slot <value>] [passive-shelf-detection <value>]
delete chassis-<name>
set chassis-<name> [expected-serial-number <value>] [alias-name <value>] [admin-state <value>] [alarm-report-control <value>] [label
<value>] [required-subtype <value>] [chassis-location <value>] [rack-name <value>] [position-in-rack <value>] [expected-pem-type <value>]
[expected-fan-type <value>] [pem-under-voltage-threshold <value>] [pem-over-voltage-threshold <value>] [actual-power-draw-alarm-threshold
<value>] [configured-ambient-temperature <value>] [filter-maintenance-interval <value>] [filter-insertion-date <value>] [power-redundancy
<value>] [no-switchover <value>] [power-limited <value>] [preferred-controller-slot <value>] [passive-shelf-detection <value>]
show chassis-<name> [is-node-controller] [chassis-role] [expected-serial-number] [alias-name] [AID] [admin-state] [oper-state]
[avail-state] [alarm-report-control] [label] [required-type] [required-subtype] [chassis-location] [rack-name] [position-in-rack]
[expected-pem-type] [expected-fan-type] [pem-under-voltage-threshold] [pem-over-voltage-threshold] [actual-power-draw-alarm-threshold <value>]
[total-available-power] [actual-power-draw] [reserved-power-draw] [actual-power-draw-alarm-threshold] [configured-ambient-temperature]
[filter-maintenance-interval] [filter-insertion-date] [power-redundancy] [no-switchover] [active-controller-slot] [equipment-discovery-ready]
[alarm-report-ready] [preferred-controller-slot] [power-limited]
```

<!-- page 292 -->

#### Command Usage Details

**Table 168: chassis Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 169: chassis Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Chassis name. | String (length 0..64) | n/a | add, delete, set, show |
| is-node-controller | Indicates if this chassis the node controller of this NE. | true, false | n/a | show |
| chassis-role | Identifies the role of the chassis in a multi-chassis NE. | • unknown<br>• main-chassis<br>• sub-chassis | unknown | show |
| expected-serial-number | Inform the NC the serial number of a sub-chassis. For the main-chassis, the value is auto-filled with its own serial number. | string length: 0..16 characters | n/a | add, set, show |
| alias-name | User defined alias for this entity. Must be an alphanumeric string with dash or underscore. | String (length 0..256) | n/a | add, set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 0..64) | n/a | show |
| admin-state | The administrative state of the managed object. | lock, maintenance, unlock | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | Inhibited | add, set, show |
| label | User-defined label for the card. | String (length 0..256) | n/a | add, set, show |
| required-type | Chassis type. | • G31<br>• G32<br>• G34c<br>• G42 | n/a | add, show |
| required-subtype | The subtype of the chassis. This parameter is only applicable to 1830 GX G34c. | • S<br>• X | n/a | add, set, show |
| chassis-location | User-defined location. | String (1...128) | n/a | add, set, show |
| rack-name | User-defined rack name (within the location). | String (1...128) | n/a | add, set, show |
| position-in-rack | Position of the chassis within the rack. | position in the rack (integer) | n/a | add, set, show |
| expected-pem-type | Defines what is the expected type of PEMs that this chassis will have. It is not possible to configure each PEM slot individually, as all PEMs need to be of the same type. DC - DC PEM AC-high-line - High-line (220V) AC PEM AC-low-line - Low-line (110V) AC PEM i Note: It is important to specify the PEM type as AC/DC, if not specified, the default PEM type is DC. | DC AC-high-line AC-low-line | DC | add, set, show |
| expected-fan-type | Defines what is the expected type of FANs that this chassis will have. It is not possible to configure each FAN slot individually, this needs to be done at the chassis level. This parameter is only applicable to 1830 GX G34c. single-rotar - Standard FAN type. counter-rotating - Counter rotating FAN type. (not applicable to 1830 GX G34c) This parameter is not applicable to 1830 GX G31 chassis. | single-rotar counter-rotating | counter-rotating | add, set, show |
| pem-under-voltage-threshold | Under voltage threshold on PEM input feed. | number (in Volt units) | n/a | add, set, show |
| pem-over-voltage-threshold | Over voltage threshold on PEM input feed. | number (in Volt units) | n/a | add, set, show |
| actual-power-draw-alarm-threshold | The actual power draw value at the chassis at which the PWRDRAW alarm is raised. User configured limit of power usable by this chassis. This parameter is not applicable to 1830 GX G31 chassis. | number (in Watt units) | the net PEM power | add, set, show |
| total-available-power | Total available power from the installed and active PEMs in the chassis after accounting for redundancy. | number (in Watt units) | n/a | show |
| actual-power-draw | Actual power draw on the chassis | number (in Watt units) | n/a | show |
| reserved-power-draw | Worst case power drawn by the chassis including power reserved for commons and power drawn by provisioned equipment. | number (in Watt units) | n/a | show |
| configured-ambient-temperature | Configured ambient temperature for the chassis, used to compute the FRU's power consumption. | number (in Celsius units) | n/a | add, set, show |
| filter-maintenance-interval | Configuration for the filter replacement. When the configured time interval expires, system reports an alarm indicating that dust filter needs to be replaced. This parameter is not applicable to 1830 GX G31 chassis. | • never - No removable dust filter or no replacement required.<br>• interval-1-months - 1 month interval for filter replacement. • interval-2-months - 2 months interval for filter replacement.<br>• interval-4-months - 4 months interval for filter replacement.<br>• interval-6-months - 6 months interval for filter replacement.<br>• interval-8-months - 8 months interval for filter replacement.<br>• interval-10-months - 10 months interval for filter replacement.<br>• interval-12-months - 1 year interval for filter replacement. | never | add, set, show |
| filter-insertion-date | Filter insertion date, if applicable. This parameter is not applicable to 1830 GX G31 chassis. | • date and time, if the maintenance interval is configured.<br>• never, if the maintenance interval is not configured. | never | add, set, show |
| power-redundancy | Configuration of the PEM redundancy mode. (Not applicable to 1830 GX G34c and 1830 GX G31.) one-plus-one - PEM is redundant within a bank of 2 PEMs. one-for-n - PEM is redundant against any other PEM. | one-plus-one one-for-n | one-plus-one | add, set, show |
| no-switchover | If enabled, the standby controller will be locked out from taking over the active card. This means no manual or autonomous switchover will happen. This parameter is not applicable to G31 chassis. | enabled, disabled | disabled | set, show |
| active-controller-slot | Identifies the active controller slot number. A change in this attribute allows the check of a switchover (the switchover check is not applicable to G31 chassis, as it does not support a redundant controller). | • none<br>• For 1830 GX G31: ▪ 5<br>• For 1830 GX G32: ▪ 5 ▪ 10<br>• For 1830 GX G34c: ▪ 12 ▪ 13<br>• For 1830 GX G42: ▪ 1 ▪ 3 | none | show |
| equipment-discovery-ready | Represents the equipment discovery state for the current chassis. It remains as 'false' until all equipment was discovered during startup. Equipment added after startup does not contribute to the update of this state. | true, false | false | show |
| preferred-controller-slot | Specify a controller slot as the preferred one. The active controller role reverts to the preferred-controller-slot after a reversion timer (5 minutes) has elapsed to ensure the card is stable. The reversion timer is started after the standby has reached ready-synchronized state. If the value is auto disables this preference and non-revertive behavior is maintained. | • auto<br>• controller slot name (object-name) | auto | add, set, show |
| passive-shelf-detection | RepresentsAllows the passive shelf detecntion for the current chassis. When 'true', enables the system to automatically detect the presence or absence of passive shelves, populate inventory information, and raise alarms. i Note: For AWG‑based passive OMDxx shelves using the PBAx one‑wire interface. | true, false | false | add, set |
| alarm-report-ready | Represents the alarm monitoring state for this chassis. After a system restart, alarms are kept persistent for a grace minute period, after which they will be cleared, unless they are reconfirmed. This state provides visibility whether that grace period has passed or not. When this state is true, there are no more cached alarms raised. | true, false | false | show |
| power-limited | Indicates if the chassis power consumption is limited by reducing max fan speed. i Note: This attribute is applicable only for 1830 GX G34c chassis. It is editable only when the node type is ILA. It is recommended to set the attribute on the 1830 GX G34c chassis during the commissioning phase of the network element. For details on commissioning the node, refer to the 1830 GX Commissioning Guide. | true, false | false | add, set, show |

#### Examples

The following example shows how to view the chassis-1 attributes:

```
show chassis-1
```

The following example shows how to set the chassis attributes (in the example chassis-4) configured-max-power-draw and filter-maintenance-interval:

```
set chassis-4 configured-max-power-draw 4000 filter-maintenance-interval interval-6-months
```

The following example shows how to add a 1830 GX G42 chassis:

```
add chassis-4 required-type G42 expected-serial-number MA3322090659
```

The following example shows how to delete a chassis:

```
delete chassis-4
```

<!-- page 302 -->
