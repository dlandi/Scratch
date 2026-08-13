---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.24. ase-idler-service'
source_lines: 5593-5642
---

## 6.24. ase-idler-service

#### Command Description

The commands described in this section are used to add/delete `ase-idler-service` or set/show the `ase-idler-service` attributes.

#### Command Syntax

```
add ase-idler-service-<name> [alarm-report-control <value>] [ase-idler-enable <value>]
delete ase-idler-service-<name>
set ase-idler-service-<name> [alarm-report-control <value>] [ase-idler-enable <value>]
show ase-idler-service-<name> [supporting-card] [AID] [oper-state] [avail-state] [function] [alarm-report-control] [ase-idler-state]
[ase-idler-enable]
```

#### Command Usage Details

**Table 119: ase-idler-service Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 120: ase-idler-service Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| function | Functionality inherent to the ne-function object. In SLTE 'idler' indicates the ASE Idler functionality handled at RD WSS, though the ASE Idler signal comes from other source. | idler | idler | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| ase-idler-state | • ase-enabled: ASE idler signal filling is complete on the band spectrum.<br>• ase-partially-enabled: ASE idler signal filling is incomplete on the band spectrum. • ase-faulted: ASE idler signal is faulted.<br>• ase-disabled: ASE idler signal is completely removed from the band spectrum. | • ase-enabled<br>• ase-partially-enabled<br>• ase-faulted<br>• ase-disabled | ase-disabled | show |
| ase-idler-enable | • enabled: ASE idler signal filling on the unused and nmc-failed portions of the band spectrum is enabled.<br>• disabled: ASE idler signal filling on the unused and nmc-failed portions of the band spectrum is disabled. | • enabled<br>• disabled | disabled | add, set, show |

#### Examples

The following command shows how to enable the ASE idler:

```
set ase-idler-service-1-8 ase-idler-enable enabled
```

<!-- page 222 -->
