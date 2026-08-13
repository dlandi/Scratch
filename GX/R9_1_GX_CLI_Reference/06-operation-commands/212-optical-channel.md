---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.212. optical-channel'
source_lines: 16904-16953
---

## 6.212. optical-channel

#### Command Description

These commands are used to edit, and show optical channel attributes.

#### Command Syntax

```
set optical-channel-<name> [label <value>] [admin-state <value>]
show optical-channel-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by]
```

#### Command Usage Details

**Table 510: optical-channel Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 511: optical-channel Command Parameters**

| Parameter | Description |  | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/n ame") | n/a | show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/nam e") | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |

#### Examples

This example shows how to set the optical channel administrative state to unlock:

```
set optical-channel-channel1 admin-state unlock
```

<!-- page 810 -->
