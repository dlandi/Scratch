---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.259. re-key'
source_lines: 19901-19948
---

## 6.259. re-key

#### Command Description

This command is used to perform a re-key operation including on-demand re-keying of a data path encryption secure entity, IKEv2 peer or an IPSec Child security association (Security Policy Database entry).

#### Command Syntax

```
re-key [ipsec-security-association=]<value> | [ikev2-peer=]<value> | [secure-entity=]<value>
```

#### Command Usage Details

**Table 610: re-key Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 611: re-key Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| ipsec-security-association | Points to IPsec SPD entry object (Child SA) | string | n/a |
| ikev2-peer | A reference to the IKE peer object (IKE SA). | string | n/a |
| secure-entity | Points to secure entity object (Child SA). | string | n/a |

#### Examples

This example shows how to perform a re-key operation on secure-entity=NE202-1-4-L1-1:

```
re-key secure-entity=NE202-1-4-L1-1
```

This example shows how to perform a re-key operation on ikev2-peer-ipsec/GX :

<!-- page 1003 -->

```
re-key ikev2-peer-ipsec/GX
```

<!-- page 1004 -->
