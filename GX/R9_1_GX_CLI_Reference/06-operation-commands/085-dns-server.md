---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.85. dns-server'
source_lines: 9450-9491
---

## 6.85. dns-server

#### Command Description

These commands are used to add, edit or show a Domain Name Server (DNS) server in the configuration. The delete command can be used to delete a DNS server from the configuration.

#### Command Syntax

```
add dns-server-<address> [origin <value>]
delete dns-server-<address>
set dns-server-<address> [origin <value>]
show dns-server-<address> [origin]
```

#### Command Usage Details

**Table 253: dns-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 254: dns-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| address | The IP address of the DNS server. | IPv4/IPv6 address. | n/a | add, set, delete, show |
| Origin | DNS address assignment method, the user can convert DHCP configured DNS entry into a manual configured by changing this attribute. | • dhcp - Indicates DNS address that has been assigned to this system by a DHCP server.<br>• manual - Indicates the DNS address has been manually configured. | manual | add, set, show |

#### Examples

This example shows how to add a DNS DHCP server:

```
add dns-server-10.100.210.243 origin dhcp
```

<!-- page 411 -->
