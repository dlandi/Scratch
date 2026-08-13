---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.98. eth-zr'
source_lines: 10386-10494
---

## 6.98. eth-zr

#### Command Description

These commands are used to add/edit/show/delete an Ethernet ZR facility. The show command is used to show a Digital Coherent 400ZR interface definition. The Digital Coherent 400ZR is auto-instantiated when the ZR TOM is provisioned.

#### Command Syntax

```
add eth-zr-<name> carriers <value> rate <value> modulation-format <value> [label <value>] [admin-state <value>] [alarm-report-control <value>]
[fec-type <value>] [fdd-monitoring <value>] [fdd-threshold <value>] [fdd-clear-threshold <value>] [fed-monitoring <value>] [fed-threshold
<value>] [fed-clear-threshold <value>] [loopback-host-interface <value>] [loopback-modem-interface <value>] [lldp-transmit-interval <value>]
[loopback <value>] [lldp-admin-status <value>] [lldp-ingress-mode <value>] [lldp-egress-mode <value>]
set eth-zr-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [carriers <value>] [rate <value>] [modulation-format
<value>] [fec-type <value>] [fdd-threshold <value>] [fed-threshold <value>] [lldp-transmit-interval <value>] [loopback <value>]
[lldp-admin-status <value>] [lldp-ingress-mode <value>] [lldp-egress-mode <value>]
show eth-zr-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [carriers] [rate] [modulation-format] [fec-type] [total-time-slots] [available-time-slots]
[fdd-threshold] [fed-threshold] [loopback]
delete eth-zr-<name>
```

#### Command Usage Details

**Table 280: eth-zr Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 455 -->

#### Command Parameters

**Table 281: eth-zr Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the facility. | string (length 1..64) | n/a | add, delete |
| supporting-card | Card that holds this facility. | card | n/a | show |
| supporting-port | Ports that hold this facility. | port | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string (length 1..64) | n/a | show |
| label | User defined label. | string (length 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock, unlock, maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete. | n/a | show |
| managed-by | Describes whether this facility was system or user created. | system, user | system | show |
| alarm-report-control | Controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | allowed, inhibited | allowed | set, show |
| carriers | A list of carriers that are bound to these facilities. Possible values can be any card/resources/ supported-carriers. | string (length 1..32) | n/a | add, set,show |
| rate | Carried signal basic rate class (Gbit/s). | 400.000 Gbit/s | 400.000 Gbit/s | show |
| modulation-format | Current modulation format. | not-applicable DP-QPSK DP-16QAM DP-8QAM BPSK | n/a | add, set, show |
| interface-type | Interface type of ZR TOM:<br>• 400ZR: Media-interface 400ZR- CFEC-DP-16QAM | enumeration | 400ZR | show |
| fec-type | The FEC type. | not-applicable cfec ofec noFEC G709 i4: EFEC-I4 i7: EFEC-I7 sdfec15: 15% SDFEC-Differential sdfec15nd: 15% SDFEC-Non- Differential staircase7: 7% HDFEC Staircase ufec7: 7% UFEC | ofec | set, show |
| total-time-slots | The member of the slots to be supported as times of 100G: rate-class/100. | Integer | - | show |
| available-time-slots | A list of time-slots that are available for provisioning new services. | String (length 0..255) | - | show |
| fdd-monitoring | The configured FEC Detected Degrade (FDD) monitoring mode. | enabled, disabled | disabled | set, show |
| fdd-threshold | The threshold for FEC Detected Degrade (FDD) alarm. It is the number of slots to be supported as times of 100G: rate-class/100. Unit : Average BER | Integer decimal64 (9) range (0.000000001..0.1) | 0.0195 avg BER | set, show |
| fdd-clear-threshold | The threshold for FEC Detected Degrade (FDD) alarm clear. decimal64(9) Unit : Average BER | range (0.000000001..0.1) | 0.01062 avg BER | set, show |
| fed-monitoring | The configured FEC Detected Degrade (FED) monitoring mode. | enabled, disabled | disabled | set, show |
| fed-threshold | The threshold for FEC Excessive Degrade. Unit : Average BER | Integer decimal64(9) range (0.000000001..0.1) | 0.0206 avg BER | set, show |
| fed-clear-threshold | The threshold for FEC Excessive Degrade (FED) alarm clear. Unit : Average BER | decimal64(9) range (0.000000001..0.1) | 0.01125 avg BER | set, show |
| link-degrade-indication | The local and remote link degradation status:<br>• none: no Link degradation.<br>• local-degraded: link has local degradation.<br>• remote-degraded: link has remote degradation.<br>• local-and-remote-degraded: link has local and remote degradation. | • none<br>• local-degraded<br>• remote-degraded<br>• local-and-remote-degraded | none | show |
| loopback-host-interface | Loopback on host interface. Useful to debug on the fiber connection. | • none<br>• facility<br>• terminal | none | set, show |
| loopback-modem-interface | Loopback on modem interface. Useful to debug on the fiber connection. | • none<br>• facility<br>• terminal | none | set, show |

#### Examples

The following example shows how to view an eth-zr facility output on 1830 GX G40 environment:

```
show eth-zr-1-6-T8
  eth-zr-1-6-T8
  supporting-card             1-6
  supporting-port             T8
  supporting-facilities       optical-carrier-1-6-T8
  supported-facilities        ethernet-1-6-T8.1,ethernet-1-6-T8.2,ethernet-1-6-T8.3,ethernet-1-6-T8.4
  AID                         '1-6-T8'
  label                       ''
  admin-state                 unlock
  oper-state                  enabled
  avail-state                 'normal in-service'
  managed-by                  system
  alarm-report-control        allowed
  carriers                    1-6-T8
  interface-type              400ZR
  rate                        400.000 Gbit/s
  fec-type                    cfec
  fdd-monitoring              disabled
  fdd-threshold               0.011250000 avg BER
  fdd-clear-threshold         0.010620000 avg BER
  fed-monitoring              disabled
  fed-threshold               0.011870000 avg BER
  fed-clear-threshold         0.011250000 avg BER
  link-degrade-indication     none
  loopback-host-interface     none
  loopback-modem-interface    none
```

The following examples shows how to set eth-zr fed-threshold and fed-clear-threshold attributes on 1830 GX G40 environment:

```
set eth-zr-1-6-T8 fed-threshold 0.011870000
set eth-zr-1-6-T8 fed-clear-threshold 0.011250000
```

<!-- page 461 -->
