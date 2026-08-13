---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.139. ipsec-sa-proposal'
source_lines: 12755-12805
---

## 6.139. ipsec-sa-proposal

#### Command Description

This command is used to add, edit or show an ipsec sa proposal. Use the delete command to delete an ipsec sa proposal.

#### Command Syntax

```
add ipsec-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number> dh-group <value> [integrity-algorithm
<value>]
set ipsec-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number> [integrity-algorithm <value>] [dh-group
<value>]
show ipsec-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number> [protocol-id] [integrity-algorithm]
[dh-group] [esn]
delete ipsec-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>
```

#### Command Usage Details

**Table 366: ipsec-sa-proposal Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 367: ipsec-sa-proposal Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, set, show, delete |
| number | The proposal number for the IKE SA. | number 1..max | n/a | add, set, show, delete |
| integrity-algorithm | The cryptographic algorithm used to perform IPsec integrity protection. | none, hmac-sha2-256-128, hmac-sha2-384-192, hmac-sha2-512-256, hmac-sha1-160, hmac-sha1-96 | n/a | add, set, show |
| protocol-id | The protocol ID used in the IKE SA and IPsec SA protocol proposals. | ESP | ESP | show |
| dh-group | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer. | dhe-2048, dhe-3072, dhe-4096, dhe-6144, dhe-8192, ecp-256, ecp-384, ecp-521, curve-25519, curve-448 | n/a | add, set, show |
| esn | Extended Sequence Number (ESN) support. | esn | esn | show |

#### Examples

This example shows how to add an ipsec sa proposal:

```
add ipsec-sa-proposal-ipsec/GX2/dns/1 dh-group dhe-3072 integrity-algorithm hmac-sha2-512-256
```

<!-- page 574 -->
