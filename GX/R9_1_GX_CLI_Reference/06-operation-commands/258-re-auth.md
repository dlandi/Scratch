---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.258. re-auth'
source_lines: 19857-19900
---

## 6.258. re-auth

#### Command Description

This command is used to perform a re-authentication operation of IKEv2 security associations.

#### Command Syntax

```
re-auth [ikev2-peer=]<value>
```

#### Command Usage Details

**Table 608: re-auth Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 609: re-auth Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| ikev2-peer | A reference to the IKE peer object (IKE SA). | string | n/a |

#### Examples

This example shows how to perform a re-authentication operation to ikev2-peer=ikev2-NE202:

```
re-auth ikev2-peer=ikev2-NE202
```

This example shows how to perform a re-authentication operation to ikev2-peer-ipsec/GX2:

```
re-auth ikev2-peer-ipsec/GX2
```

<!-- page 1002 -->
