---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.31. bgp-network'
source_lines: 5998-6043
---

## 6.31. bgp-network

#### Command Description

This command is used to add/edit/show a bgp network. Use the delete command to delete a bgp network. If bgp-network managed object is created, then networks/sub-networks that must be exported to the external AS can be specified. A maximum of 100 bgp-network MOs can be configured.

**Note:** The routes to be advertised to external AS must exist in the forwarding table installed by an Interior Gateway Protocol (IGP) such as OSPF or static routes, but not BGP itself. For routes not present in IGP tables, blackhole static routes must be configured. To configure the blackhole static routes, special-next-hop parameter of static route command must be set to blackhole. For more information about configuring static routes, see (give link to static route command)

#### Command Syntax

```
add bgp-network-<instance id>/<remote-address>/<network prefix>
delete bgp-network-<instance id>/<remote-address>/<network prefix>
show bgp-network-<instance-id>/<remote-address>/<network-prefix>
```

#### Command Usage Details

**Table 133: bgp-network Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 243 -->

#### Command Parameters

**Table 134: bgp-network Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | BGP instance ID. | string (length 1...255) | n/a | add, delete, show |
| remote-address | Specifies the BGP peer address in IPv4 or IPv6 format. 0.0.0.0/0 is not supported for IPv4 and 0::0.0 is not supported for IPv6. | IPv6 or IPv4 address | n/a | add, delete, show |
| network-prefix | Specifies the network prefix. | Decimal number (1.. 64) or 128 | n/a | add, delete, show |

#### Examples

This example shows how to add a BGP network for IPv6 address format:

```
add bgp-network-10/2620:38:4001:192::65/3000::2/64
```

<!-- page 244 -->
