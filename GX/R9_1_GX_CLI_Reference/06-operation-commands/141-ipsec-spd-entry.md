---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.141. ipsec-spd-entry'
source_lines: 12844-12900
---

## 6.141. ipsec-spd-entry

#### Command Description

These commands are used to add, edit or show ipsec Security Policy Database entry. The delete command is used to delete ipsec Security Policy Database entry.

#### Command Syntax

```
add ipsec-spd-entry-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> priority <value> [description <value>] [action <value>]
[ipsec-protocol <value>] [mode <value>] [alarm-report-control <value>] [esn <value>] [anti-replay-window <value>] [dynamic-ts <value>]
set ipsec-spd-entry-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> [priority <value>] [description <value>] [action
<value>] [ipsec-protocol <value>] [mode <value>] [alarm-report-control <value>] [esn <value>] [anti-replay-window <value>] [dynamic-ts <value>]
show ipsec-spd-entry-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> [AID] [priority] [description] [action]
[ipsec-protocol] [mode] [alarm-report-control] [esn] [oper-state] [anti-replay-window] [dynamic-ts]
delete ipsec-spd-entry-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>
```

#### Command Usage Details

**Table 370: ipsec-spd-entry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 371: ipsec-spd-entry Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | Local instance name. The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | Peer name. A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, set, show, delete |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string (length 1..64) | n/a | show |
| priority | A priority value for each SPD entry. This is used to give precedence to the SPD entries. | number (unit8) | n/a | add, set, show |
| description | User configurable label/ description. | string (length 0..128) | n/a | add, set, show |
| action | Indicates the IPsec treatment given to the IP datagrams. | protect, bypass, discard | protect | add, set, show |
| ipsec-protocol | Indicates the use of ESP or AH IPsec protocols. | ESP | ESP | add, set, show |
| mode | Indicates if the IPsec session operates in transport or tunnel mode. | tunnel, transport | tunnel | add, set, show |
| alarm-report-control | Controls the reporting of alarms for this particular object. Indicates if alarm reporting is enabled: allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed, inhibited | allowed | add, set, show |
| esn | Extended Sequence Number (ESN) support. | true, false | true | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| anti-replay-window | When action = 'protect', indicates the replay window size tolerance. | number (range 32..1024) | 64 | add, set, show |
| dynamic-ts | Indicates whether dynamic traffic selector is enabled in this SPD entry. | disabled, enabled | disabled | add, set, show |

#### Examples

This example shows how to add an IPsec SPD entry:

```
add ipsec-spd-entry-ipsec/GX2/dns priority 1 action protect
```

<!-- page 579 -->
