---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.54. comm-eth'
source_lines: 7385-7492
---

## 6.54. comm-eth

#### Command Description

These commands are used to set or show the communication Ethernet port attributes. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set comm-eth-<card-name>-<port-name> [auto-negotiation <value>] [mtu <value>] [duplex-mode <value>] [rate <value>] [flow-control <value>]
[lldp-transmit-interval <value>] [lldp-mgmt-addr-if <value>] [mode <value>] [lldp-admin-status <value>]
show comm-eth-<card-name>-<port-name> [auto-negotiation] [mtu] [duplex-mode] [operational-duplex-mode] [rate] [operational-rate] [flow-control]
[operational-flow-control] [redundancy-state] [mac-address] [lldp-transmit-interval] [lldp-mgmt-addr-if] [mode] [lldp-admin-status]
```

#### Command Usage Details

**Table 184: comm-eth Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 185: comm-eth Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | Name of the card. | String (length 0..64 characters) | n/a | set, show |
| port-name | Name of the port. | String (length 0..64 characters) | n/a | set, show |
| auto-negotiation | Auto negotiation mode. | disabled enabled | enabled | set, show |
| mtu | The maximum transmission unit size in octets for the physical Ethernet port. | Integer (range: 1280..1500 octets) | 1500 | set, show |
| duplex-mode | Duplex mode. It is only valid if auto-negotiation is disabled. unknown - Link is currently disconnected or initializing. full - Full duplex. half - Half duplex. | unknown full half duplex. | full | set, show |
| operational-duplex-mode | Operational duplex mode. | unknown full half duplex. | unknown | show |
| rate | Required Ethernet rate (1/10/100/1000/10000 Mbits or maximum). It is only valid if auto-negotiation is disabled. | 1, 10, 100, 1000, 10000 Mbits, maximum | maximum | set, show |
| operational-rate | Operational Ethernet rate (1/10/100/1000/10000 Mbits or maximum). | 1, 10, 100, 1000, 10000 Mbits, maximum | unknown | show |
| flow-control | Specifies the type of flow control to be supported. Applicable when the auto-negotiation is disabled. unknown - Link is currently disconnected or initializing. disabled - No pause frames are supported. bi-directional - Symmetric flow (transmit and receive). egress-only - Transmit direction only. ingress-only - Receive direction only. | unknown disabled bi-directional egress-only ingress-only | disabled | set, show |
| operational-flow-control | Operational flow control. | unknown disabled bi-directional egress-only ingress-only | unknown | show |
| redundancy-state | Redundancy state of the comm port: none - No redundancy. active - Port is active. standby - Port is on standby. | none active standby | none | show |
| mac-address | MAC Address of the port. | String (MAC address) | 00:00:00:00:00:00 | show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| lldp-transmit-interval | The interval to transmit LLDP Tx TLVs (in seconds). | Integer in the range [1..16383] seconds | 30 seconds | set, show |
| lldp-mgmt-addr-if | Specify which interface's IP address to be used for management address. This parameter must be explicitly set by the user and is applicable when the lldp-admin-status is set to tx-only or tx-and-rx. It can be set to:<br>• 1-AUX-1<br>• 1-AUX-2<br>• DCN<br>• DCN-2 (1830 GX G30 only)<br>• 1-AUX-1-B<br>• 1-AUX-2-B<br>• DCN-B • DCN-2-B (1830 GX G30 only)<br>• L0-MGMT By default, the value is empty. If this object is not configured by the user and the loopback is configured, the system takes the loopback IP address as the default value. If this object is not configured by the user and the loopback is not configured, the system takes the DCN/AUX IP address as the default value. If both loopback and DCN/AUX have no IP address, the system does not send this TLV. | • 1-AUX-1<br>• 1-AUX-2<br>• DCN<br>• DCN-2 (G30 only)<br>• 1-AUX-1-B<br>• 1-AUX-2-B<br>• DCN-B<br>• DCN-2-B (G30 only)<br>• L0-MGMT | n/a | set, show |
| mode | Indicates the mode of operation of control channel. The mode of operation is common for comm-eth and comm-channel objects. | • L1 - L1 ETH User Channel Mode.<br>• L2: L2 AUX interface.<br>• L3 - L3 IP Mode (Default). | L3 | set, show |
| lldp-admin-status | LLDP operational mode for this port:<br>• tx-only - LLDP agent transmits LLDP frames on this port but it does not store connected remote system information.<br>• rx-only - LLDP agent receives, but it does not transmit LLDP frames on this port.<br>• tx-and-rx - LLDP agent transmits and receives LLDP frames on this port.<br>• disabled - LLDP agent does not transmit or receive LLDP frames on this port. It is not possible to modify this parameter if the mode is L1. | • tx-only<br>• rx-only<br>• tx-and-rx<br>• disabled | disabled | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |

<!-- page 331 -->

#### Examples

The following example shows how to view the attributes of all the communication Ethernet ports:

```
show comm-eth
```

The following example shows how to enable the auto-negotiation of the communication Ethernet port in 1830 GX G30 environment:

```
show comm-eth-1-11-ETH5 auto-negotiation enabled
```

The following example shows how to enable the auto-negotiation of the communication Ethernet port in 1830 GX G40 environment:

```
show comm-eth-1-13-ETH1 auto-negotiation enabled
```

The following example shows how to enable ethernet auto-negotiation mode in 1830 GX G40 environment:

```
set comm-eth auto-negotiation enabled
```

The following example shows how to enable LLDP on DCN port and sets it to tx-and-rx (LLDP agent transmits and receives LLDP frames) in 1830 GX G30 environment:

```
set comm-eth-1-5-ETH1 lldp-admin-status tx-and-rx
```

The following example shows how to enable LLDP on AUX port and sets it to tx-only in 1830 GX G30 environment:

```
set comm-eth-1-11-ETH5 lldp-admin-status tx-only
```

The following example shows how to enable LLDP on DCN port and sets it to tx-and-rx (LLDP agent transmits and receives LLDP frames) in 1830 GX G40 environment:

```
set comm-eth-1-12-ETH1 lldp-admin-status tx-and-rx
```

The following example shows how to enable LLDP on AUX port and sets it to tx-only in 1830 GX G40 environment:

```
set comm-eth-1-13-ETH4 lldp-admin-status tx-only
```

The following example shows how to set the communication Ethernet to L2 mode in 1830 GX G31 environment:

```
set comm-eth-1-13-ETH4 mode L2
```

<!-- page 332 -->
