---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.145. ipv6-address'
source_lines: 13055-13105
---

## 6.145. ipv6-address

#### Command Description

These commands are used to add/show/delete an IPv6 address to the interface.

**Note:** To view the available interfaces, use the "?" after the command `ipv6-address-`. The system displays the object completion options.

#### Command Syntax

```
add ipv6-address-<if-name>/<ip> prefix-length <value>
show ipv6-address-<if-name>/<ip> [prefix-length] [origin]
delete ipv6-address-<if-name>/<ip>
```

#### Command Usage Details

**Table 378: ipv6-address Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration mode |

#### Command Parameters

**Table 379: ipv6-address Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| if-name | The interface object identifier. | String (length 0..64 characters) | n/a | add, show, delete |
| ip | The IPv6 address on the interface. | IPv6 address. | n/a | add, show |
| prefix-length | The length of the subnet prefix. Only valid prefixes are allowed to be configured. i Note: IPv6 /127 subnet is currently supported on unprotected DCN-A (DCN) interfaces and does not apply to broadcast links with larger subnets. | Number (range 1..128) | n/a | add, show |
| origin | IPv6 address assignment method. static: Indicates that the address has been statically\n configured - for example, using NETCONF or a Command Line Interface. dhcp: Indicates an address that has been assigned to this \n system by a DHCP server." auto-config: Indicates an address created by autoconfiguration. | static dhcp auto-config | static | show |

#### Examples

This example shows how to add an ipv6 address:

```
add ipv6-address-1-AUX-1/AAAA::186 prefix-length 10
```

This example shows how to configure an IPv6 DCN IP address on DCN / DCN-B:

```
add ipv6-address-DCN-B/2620:38:4001:192:10:220:192:213 prefix-length 64
```

<!-- page 588 -->
