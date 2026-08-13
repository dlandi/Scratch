---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.78. dhcp-relay'
source_lines: 8958-9011
---

## 6.78. dhcp-relay

#### Command Description

These commands allow to edit or view the dhcp relay mode and server address.

#### Command Syntax

```
set dhcp-relay [mode <value>] [server-address <value>]
show dhcp-relay [mode] [server-address]
```

#### Command Usage Details

**Table 238: dhcp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 239: dhcp-relay Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| mode | Flag indicating the dhcp-relay mode of operation. | disabled, ipv4, ipv6 | disabled | show, set |
| server-address | DHCP server ip-addresses; when enabled at least one IP address should be configured. | IPv4 or IPv6 server address | n/a | show, set |

#### Examples

This example shows how to view dhcp relay attributes:

```
show dhcp-relay
```

<!-- page 392 -->

This example shows how to set dhcp relay mode to IPv4:

```
set dhcp-relay mode ipv4
```

This example shows how to set the dhcp relay mode to IPv4 along with setting the server address:

```
set dhcp-relay mode ipv4 server-address 10.220.20.3
```

<!-- page 393 -->
