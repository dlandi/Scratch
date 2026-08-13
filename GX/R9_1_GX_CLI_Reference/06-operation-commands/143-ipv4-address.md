---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.143. ipv4-address'
source_lines: 12950-13001
---

## 6.143. ipv4-address

#### Command Description

These commands are used to add/show/delete an IPv4 address on the interface.

**Note:** To view the available interfaces, use the "?" after the command `ipv4-address-`. The system displays the object completion options.

#### Command Syntax

```
add ipv4-address-<if-name>/<ip> netmask <value>
show ipv4-address-<if-name>/<ip> [netmask] [origin]
delete ipv4-address-<if-name>/<ip>
```

#### Command Usage Details

**Table 374: ipv4-address Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |
| Pre-condition | A static IPv4 address can not be configured when DHCP is enabled on the interface. |

#### Command Parameters

**Table 375: ipv4-address Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| if-name | The interface object identifier. | String (length 0..64 characters) | n/a | add, show, delete |
| ip | The IPv4 addresses on the interface. The following addresses are disallowed from being configured: 1. Addresses beginning with 0 (current network) 2. Addresses beginning with 127 (loopback addresses) 3. Addresses beginning with 224 up to 255 (broadcast, multicast and experimental addresses) | IPv4 address | n/a | add, show |
| netmask | The subnet specified as a netmask for a particular address. Only valid netmasks are allowed to be configured. | Subnet | n/a | add, show |
| origin | IPv4 address assignment method. static: Indicates that the address has been statically\n configured - for example, using NETCONF or a Command Line Interface. dhcp: Indicates an address that has been assigned to this \n system by a DHCP server." auto-config: Indicates an address created by autoconfiguration. | static dhcp auto-config | static | show |

#### Examples

This example shows how to add and IPv4 address:

```
add ipv4-address-1-AUX-1/200.20.20.186 netmask '255.255.240.0'
```

This example shows how to configure an IPv4 DCN IP address on DCN / DCN-B:

```
add ipv4-address-DCN-B/10.220.192.213 netmask 255.255.252.0
```

<!-- page 583 -->
