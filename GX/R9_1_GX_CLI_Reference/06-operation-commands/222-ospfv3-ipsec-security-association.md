---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.222. ospfv3-ipsec-security-association'
source_lines: 17505-17554
---

## 6.222. ospfv3-ipsec-security-association

#### Command Description

This command is used to add/set/show an OSPF version 3 security association. The delete command is used to delete an OSPF version 3 ipsec security association from the configuration.

#### Command Syntax

```
add ospfv3-ipsec-security-association-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> integrity-algorithm <value> [ipsec-protocol <value>]
[ipsec-mode <value>]
set ospfv3-ipsec-security-association-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> [ipsec-protocol <value>] [ipsec-mode <value>]
[integrity-algorithm <value>]
show ospfv3-ipsec-security-association-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi> [ipsec-protocol] [ipsec-mode] [integrity-algorithm]
delete ospfv3-ipsec-security-association-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi>
```

#### Command Usage Details

**Table 531: ospfv3-ipsec-security-association Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | The OSPF instance must be created before the area can be added. |

#### Command Parameters

**Table 532: ospfv3-ipsec-security-association Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF instance ID. | uint8 (range: 0 .. 255) | n/a | add, set, show, delete |
| ospf-area-id | OSPF Router Area ID. | dotted-quad | n/a | add, set, show, delete |
| ospf-if-name | Reference of the interface in OSPF area. | leafref (path "../../../../../interface/if-name") | n/a | add, set, show, delete |
| spi | A unique security parameter index (SPI) for this SA. | uint32 (range 256..4294967295) | n/a | add, set, show, delete |
| integrity-algorithm | The cryptographic algorithm used to perform IPsec integrity protection. | AUTH HMAC SHA2 256 128,A _ _ _ _ UTH HMAC SHA2 384 192, _ _ _ _ AUTH HMAC SHA2 512 256,A _ _ _ _ UTH HMAC SHA1 160, _ _ _ AUTH HMAC SHA1 96 _ _ _ | n/a | add, set, show |
| ipsec-protocol | Indicates the use of ESP or AH IPsec protocols. | ESP | ESP | add, set, show |
| ipsec-mode | Indicates IPsec mode. Only transport mode is supported. | transport | transport | add, set, show |

#### Examples

The following example shows how to add an ospfv3-ipsec-security-association:

```
add ospfv3-ipsec-security-association-1/0.0.0.0/DCN/256 integrity-algorithm AUTH_HMAC_SHA2_256_128
```

<!-- page 854 -->
