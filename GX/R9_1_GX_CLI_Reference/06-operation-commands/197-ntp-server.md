---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.197. ntp-server'
source_lines: 15880-15928
---

## 6.197. ntp-server

#### Command Description

These commands are used to add, set or show the NTP server attributes. The delete command is used to delete an NTP server from the configuration.

#### Command Syntax

```
add ntp-server-<ip-address> [origin <value>] [auth-key-id <value>] [label <value>] [admin-state <value>] [alarm-report-control <value>]
set ntp-server-<ip-address> [origin <value>] [auth-key-id <value>] [label <value>] [admin-state <value>] [alarm-report-control <value>]
show ntp-server-<ip-address> [origin] [auth-key-id] [label] [admin-state] [oper-state] [avail-state] [alarm-report-control]
delete ntp-server-<ip-address>
```

#### Command Usage Details

**Table 480: ntp-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 481: ntp-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ip-address | NTP Server IP address. Ipv4/Ipv6/hostname supported. | IPv4 address, IPv6 address, DNS domain name | n/a | add, set, show, delete |
| origin | NTP address assignment method. A user can convert DHCP configured NTP entry into a manual configured by changing this attribute: • dhcp - Indicates NTP address that has been assigned to this system by a DHCP server.<br>• manual - Indicates the NTP address has been manually configured. | dhcp manual | manual | add, set, show |
| auth-key-id | Key ID to be used for this server. | number (uint32); not-applicable | not-applicable | add, set, show |
| label | User defined label. | string (length 0..256 characters) | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete. | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed inhibited | allowed | add, set, show |

#### Examples

This example shows how to add two NTP servers and associate a key with a configured NTP server:

```
add ntp-server-10.220.0.70
add npt-server-10.220.0.70 auth-key-id 1
```

<!-- page 730 -->
