---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.26. auth-key'
source_lines: 5697-5742
---

## 6.26. auth-key

#### Command Description

This command is used to add, edit or show a authorization key. Use the delete command to delete an authorization key from the configuration.

#### Command Syntax

```
add auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> key <value> [type <value>]
set auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> [type <value>] [key <value>]
show auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> [type] [key]
delete auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi>
```

#### Command Usage Details

**Table 123: auth-key Command Usage**

| Section | Description | Add | Set | Delete |
| --- | --- | --- | --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |  |  |  |

#### Command Parameters

**Table 124: auth-key Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF instance ID. | number (range 0 .. 255) | n/a | add, set, show, delete |
| ospf-area-id | OSPF Router Area ID. | string | n/a | add, set, show, delete |
| ospf-if-name | Reference of the interface in OSPF area. | string | n/a | add, set, show, delete |
| spi | A unique security parameter index (SPI) for this SA. | number (range 256..4294967295) | n/a | add, set, show |
| key | The pre-shared key for OSPFv3 IPsec integrity protection. | string (length 8..128) | n/a | add, set, show |
| type | Indicates whether the integrity key is ASCII or hexadecimal encoded. | ascii, hex | n/a | add, set, show |

#### Examples

This example shows how to add an auth-key:

```
add auth-key-1/0.0.0.0/DCN/256 key abcdefgh12345678
```

<!-- page 228 -->
