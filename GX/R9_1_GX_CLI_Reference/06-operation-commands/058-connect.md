---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.58. connect'
source_lines: 7636-7679
---

## 6.58. connect

#### Command Description

The `connect` command described in this section is used to establish a ssh session directly from CLI.

#### Command Syntax

```
connect [target-address=]<value> [user-name=]<value> [[port=]<value>]
```

#### Command Usage Details

**Table 193: connect Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 194: connect Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| target-address | The target-address which may be IPv4, IPv6 or hostname (if DNS configured). It requires connectivity. It is a mandatory attribute. | ipv4 address, ipv6 address or hostname | n/a |
| user-name | User name. | string (0..64 characters) | n/a |
| port | Optional port. By default, it is used port 22, but can be provided if needed (useful to access the shell port). | port | 22 |

#### Examples

The following command shows how to

<!-- page 338 -->

```
connect 10.41.24.55 admin port=8022
Warning: Permanently added '10.41.24.55:8022' (ED25519) to the list of known hosts.
admin@10.41.24.55's password:
```

<!-- page 339 -->
