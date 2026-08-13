---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.129. ikev2-local-instance'
source_lines: 12074-12124
---

## 6.129. ikev2-local-instance

#### Command Description

These commands are used to set and show an ikev2 local instance.

#### Command Syntax

```
set ikev2-local-instance-<name> [scope <value>] [label <value>] [admin-state <value>] [alarm-report-control <value>] [local-address <value>]
show ikev2-local-instance-<name> [host-card-encryption-capability] [scope] [host-card] [started-time] [AID] [label] [admin-state] [oper-state]
[alarm-report-control] [local-address-assignment-method] [local-address]
```

#### Command Usage Details

**Table 345: ikev2-local-instance Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 346: ikev2-local-instance Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | set, show |
| host-card-encryption-capability | Indicates whether the card on which this IKEv2 local instance is running, supports the ability to do encryption. | yes, no, unknown | unknown | show |
| scope | The scope of the IKEv2 instance for which security associations (SA) are being negotiated. | data-path-encryption, management-ipsec, local-address, local-address-assignment-method | n/a | set, show |
| host-card | The reference to the service card on which this IKEv2 protocol instance is running. | path "../../../../../equipment/card/ name" | n/a | show |
| started-time | Local system timestamp when this IKEv2 instance was started. | date-and-time | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| label | User configurable label | string | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock, maintenance, unlock | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | Inhibited | set, show |
| local-address-assignment-method | Local IP address assignment method for IKEv2 channel. | auto, manual | auto | show |
| local-address | Local IPv4 address for IKEv2 channel with prefix-length 32. | string | 0.0.0.0 | set, show |

#### Examples

This example shows how to set ikev2 local instance local address:

```
set ikev2-local-instance-1-3 local-address 1.1.180.132
```

<!-- page 538 -->
