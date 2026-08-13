---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.127. ike-sa-proposal'
source_lines: 11988-12034
---

## 6.127. ike-sa-proposal

#### Command Description

This command is used to add, edit or show a common set of attributes for IKEv2 used across Management IPsec. Use the delete command to delete an ike sa proposal.

#### Command Syntax

```
add ike-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<number> integrity-algorithm <value> dh-group <value> [prf <value>]
set ike-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<number> [integrity-algorithm <value>] [dh-group <value>] [prf <value>]
show ike-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<number> [protocol-id] [integrity-algorithm] [dh-group] [prf]
delete ike-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>
```

#### Command Usage Details

**Table 341: ike-sa-proposal Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 342: ike-sa-proposal Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| number | The proposal number for the IKE SA. | number 1..max | n/a | add, set, show, delete |
| protocol-id | The protocol ID (type) for which the IKE proposal applies to. | IKE | IKE | show |
| integrity-algorithm | The cryptographic algorithm used to perform IPsec integrity protection. | none, hmac-sha2-256-128, hmac-sha2-384-192, hmac-sha2-512-256, hmac-sha1-160, hmac-sha1-96 | n/a | add, set, show |
| dh-group | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer. | dhe-2048, dhe-3072, dhe-4096, dhe-6144, dhe-8192, ecp-256, ecp-384, ecp-521, curve-25519, curve-448, ml-kem-512 | n/a | add, set, show |
| prf | A list of protocol proposals when negotiating the IKE SA + with the far-end IKE peer. | hmac-sha2-256, hmac-sha2-384, hmac-sha2-512, hmac-sha1 | n/a | add, set, show |

#### Examples

This example shows how to add an ike sa proposal:

```
add ike-sa-proposal-ipsec/GX2/1 dh-group dhe-2048 prf hmac-sha2-256 integrity-algorithm hmac-sha2-384-192
```

<!-- page 535 -->
