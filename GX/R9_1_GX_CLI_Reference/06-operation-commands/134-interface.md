---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.134. interface'
source_lines: 12415-12494
---

## 6.134. interface

#### Command Description

These commands are used to add/set/show/delete an interface and related attributes.

#### Command Syntax

```
add interface-<if-name> if-type <value> [if-description <value>] [protection-mode <value>] [ipv4-enabled <value>] [ipv4-address-assignment-method
<value>] [ipv6-enabled <value>] [ipv6-address-assignment-method <value>] [proxy-arp-enabled <value>] [admin-state <value>] [alarm-report-control
<value>] [label <value>]
set interface-<if-name> [if-description <value>] [protection-mode <value>] [ipv4-enabled <value>] [ipv4-address-assignment-method <value>]
[ipv6-enabled <value>] [ipv6-address-assignment-method <value>] [proxy-arp-enabled <value>] [admin-state <value>] [alarm-report-control <value>]
[label <value>]
show interface-<if-name> [if-description] [if-type] [supporting-port] [backup-port] [protection-mode] [protection-state] [vrf] [ipv4-enabled]
[ipv4-address-assignment-method] [ipv6-enabled] [ipv6-address-assignment-method] [proxy-arp-enabled] [admin-state] [oper-state] [avail-state]
[alarm-report-control] [label]
delete interface-<if-name>
```

#### Command Usage Details

**Table 356: interface Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 555 -->

#### Command Parameters

**Table 357: interface Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| if-name | The interface object identifier. | String (length 0..64 characters) | n/a | add, set, show, delete |
| if-description | A textual description of the interface. | String (length 0..255 characters) | n/a | add, set, show |
| if-type | The type of the interface. ethernet: For all Ethernet-like interfaces, regardless of speed, as per RFC 3635. software-loopback: Software Loopback interface type. point-to-point: Point to point interfaces associated with control channels. ppp: RFC 1661 Point-to-Point Protocol (PPP) interface. A Link Control\n Protocol (LCP) for establishing and configuring the data-link connection and a family of Network Control Protocols (NCPs) for establishing and configuring different network-layer protocols will run over the interface. hdlc: HDLC (High-Level Data Link Control)-like interface associated with OSCX channels. | ethernet software-loopback point-to-point ppp hdlc | n/a | show |
| supporting-port | Reference to the physical port that the interface is currently mapped to. | String (length 0..64 characters) | n/a | show |
| backup-port | Reference to the physical port that supports this interface (if applicable). | String (length 0..64 characters) | n/a | show |
| protection-mode | Reference to user given protection mode for interface. unknown: Unknown/Transient protection state; output only. protected: Protected by redundant ports. unprotected: No port redundancy. | unknown protected unprotected | protected | add, set, show |
| protection-state | Reference to current state of protection of interface so by default its unknown. unknown: Unknown/Transient protection state; output only. protected: Protected by redundant ports. unprotected: No port redundancy. | unknown protected unprotected | unknown | show |
| vrf | VRF to which this interface is bound. | VRF | n/a | show |
| ipv4-enabled | Controls whether IPv4 is enabled or disabled on this interface. When IPv4 is enabled, this interface is connected to an IPv4 stack, and the interface can send\n and receive IPv4 packets. | true, false | true | add, set, show |
| ipv4-address-assignment-method | IPv4 address assignment method. | static, dhcp | static | add, set, show |
| ipv6-enabled | Controls whether IPv6 is enabled or disabled on this\n interface. When IPv6 is enabled, this interface is connected to an IPv6 stack, and the interface can send and receive IPv6 packets. | true, false | true | add, set, show |
| ipv6-address-assignment-method | IPv6 address assignment method. | static, dhcp | static | add, set, show |
| proxy-arp-enabled | Controls whether or not Proxy ARP is to be enabled on the interface. This attribute is only applicable to the DCN interface. | false, true | false | add, set, show |
| if-dhcp-relay | Enables dchp-relay function on a specific interface. It decides on which interface the DHCP/v6 relay can be run. Obeys global dhcp-relay settings. | false, true | false | add, set, show |
| admin-state | The administrative state of the managed object. | lock, unlock, maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete. | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed | add, set, show |
| label | User defined label. | string (length 0..256 characters) | n/a | add, set, show |

#### Examples

This example shows how to set an interface attribute:

```
set interface if-description localinterface
This will affect multiple 'interface' objects. Are you sure? [y/n/?]
```

This example shows how to set the protection mode of the DCN interface:

```
set interface-DCN protection-mode protected
```

This example shows how to view the details of the DCN interface:

```
show interface-DCN
```

<!-- page 559 -->
