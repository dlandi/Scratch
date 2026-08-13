---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.32. bootstrap'
source_lines: 6044-6076
---

## 6.32. bootstrap

#### Command Description

The command described in this section is used to bootstrap a neighbor NE by establishing a TLS connection over the OSC link and provisioning the initial administrator account on that neighbor. It is used during commissioning to remotely create the first administrator user on an OSC peer NE that has no configured users yet. The operator or NBI selects the neighbor either by supplying **local-port** (the supporting-port of an osc-eth MGMT interface, e.g. 1-1-dwdm-line1) or by supplying **neighbor-address** directly (the IPv6 link-local address of the OSC peer). When **local-port** is chosen, the system maps it to the Linux OCE netdev (OCE-\<chassis\>-\<slot\>-\<vlan-id\>) and resolves the peer IPv6 link-local address into **neighbor-address** using get\_osc\_neighbors.sh before the bootstrap request proceeds. If resolution fails, the RPC is rejected. The output is a human-readable result of the bootstrap operation, including any response data returned by the neighbor NE.

#### Command Syntax

```
bootstrap ([local-port=]<value> | [neighbor-address=]<value>) [new-admin-user=]<value> [new-password=]<value>
```

#### Command Usage Details

**Table 135: bootstrap Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 136: bootstrap Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| local-port | Supporting-port of the MGMT vrf osc-eth interface for the OSC link to the neighbor (same value as in 'show interface', e.g. 1-1-dwdm-line1). The implementation maps this to the OS OCE netdev name before calling get osc neighbors.sh. _ _ | string | n/a |
| neighbor-address | IPv6 link-local address of the OSC peer to contact (including zone), e.g. fe80::... %OCE-1-1-503. When the operator supplies this directly, the value is used as-is by USM. When local-port is chosen instead, the model fills this leaf automatically via get osc neighbors.sh -- _ _ local-port. | string (length (7..256); pattern 'fe80:.*') | n/a |
| new-admin-user | User-name of the administrator account to auto-provision on the neighbor NE. Obeys the same naming rules as a local user account. It is a mandantory attribute. | length 1..32 | n/a |
| new-password | Password for the new administrator account on the neighbor NE. Can be provided as a password hash ( format $&lt;id&gt;$&lt;salt&gt; $&lt;hash&gt;; only id 6 (SHA512) is supported; salt size is between 2 and 16 chars), or as plain text. When provided as plain text, the standard password complexity rules enforced by the system apply. | string pattern: "$6$[A-Za-z0-9./]{2,16}$[A-Za-z0-9./]+" | n/a |

<!-- page 246 -->
