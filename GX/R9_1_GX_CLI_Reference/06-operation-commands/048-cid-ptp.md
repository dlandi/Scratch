---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.48. cid-ptp'
source_lines: 6962-7052
---

## 6.48. cid-ptp

#### Command Description

The commands described in this section are used to manage `cid-ptp` facility and its attributes. The cid-ptp facility is created when a card supporting CableID function is created, for example, RD20TM or CAD10A. The `cid-ptp` facility supports the CableID SFP and its connection to the card via the CID port.

#### Command Syntax

```
set cid-ptp-<name> [label <value>] [admin-state <value>]
show cid-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [used]
```

#### Command Usage Details

**Table 170: cid-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 171: cid-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. The admin-state for the cid-ptp is not editable and only the value supported is unlock. The CableID operation is not affected by the cid-ptp admin-state. | unlock | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| used | It is true when CableID functionality is supported. | • true<br>• false | false | show |

#### Examples

The following command shows an example on how to view the cid-ptp attributes of RD20TM SPCID port:

```
show cid-ptp-1-4-spcid
```

The following output is retrieved:

```
  cid-ptp-1-4-spcid
  supporting-card                1-4
  supporting-port                spcid
  supporting-facilities
  supported-facilities
  AID                            '1-4-spcid'
  label                          ''
  admin-state                    unlock
  oper-state                     enabled
  avail-state                    'normal in-service'
  managed-by                     system
  used                           true
```

The following command shows an example on how to view the cid-ptp attributes of CAD10A SPU port:

```
show cid-ptp-1-6-spu
```

The following output is retrieved:

```
  cid-ptp-1-6-spu
  supporting-card              1-6
  supporting-port              spu
  supporting-facilities
  supported-facilities
  AID                          '1-6-spu'
  label                        ''
  admin-state                  unlock
  oper-state                   enabled
  avail-state                  'normal in-service'
  managed-by                   system
  used                         true
```

<!-- page 307 -->
