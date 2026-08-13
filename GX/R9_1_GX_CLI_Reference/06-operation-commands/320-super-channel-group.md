---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.320. super-channel-group'
source_lines: 24507-24567
---

## 6.320. super-channel-group

#### Command Description

This command is used to add, set or show super-channel-group attributes.

#### Command Syntax

```
add -m super-channel-group-<name> [label <value>] [admin-state <lock|unlock|maintenance>] [auto-in-service-enabled <value>] [valid-signal-time
<value>] [alarm-report-control <allowed|inhibited>] [line-system-mode <value>] [openwave-contention-check <value>]
```

**Note:** The add command for super-channel-group works in merge mode only. Using the -m flag performs a merge, which is the best effort add. If the target entity does not exist, it is created. If it exists, it is updated with any attributes present on the "add" command.

```
set super-channel-group [admin-state <unlock|maintenance|lock>] [alarm-report-control <allowed|inhibited>] [auto-in-service-enabled <true|false>]
[label <string>] [line-system-mode <openwave>] [valid-signal-time <value>]
show super-channel-group-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [auto-in-service-enabled] [valid-signal-time] [remaining-valid-signal-time] [alarm-report-control]
[line-system-mode] [openwave-contention-check] [expected-total-tx-power]
```

#### Command Usage Details

**Table 740: super-channel-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 1207 -->

#### Command Parameters

**Table 741: super-channel-group Command Parameters**

| Parameter | Description | Values | Default | Used In |
| --- | --- | --- | --- | --- |
| admin-state | The administrative state of the managed object. | lock, maintenance, unlock | unlock | add, set, show |
| alarm-report-control | Controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited . | allowed | add, set, show |
| auto-in-service-enabled | Auto-in-service switch for this facility. | true, false | false | add, set, show |
| label | User-defined label for the card. | String (length 0..256) | n/a | add, set, show |
| line-system-mode. | Indicates the specific mode of power control configured on the L1 transponder, and specifically, on this particular SCG port within the L1 transponder. | openwave | openwave | add, set, show |
| valid-signal-time | Configurable time that represents a detection of a valid signal. | Number (range 1..7200, minutes) | 480 | add, set, show |
| channel group | The name of the channel group | string | n/a | add, set, show, delete |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this xcon was system created or not. Only user created xcon can be user deleted. | system, user | user | show |
| command flag | -m | Merge configuration (the command will not fail if the entity already exists) |  |  |

#### Examples

This example shows how to set a super channel group attribute:

```
set super-channel-group-1-7-L1 alarm-report-control inhibited
```

<!-- page 1210 -->
