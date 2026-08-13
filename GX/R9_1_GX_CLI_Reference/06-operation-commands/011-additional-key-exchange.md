---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.11. additional-key-exchange'
source_lines: 4834-4894
---

## 6.11. additional-key-exchange

#### Command Description

Users can configure additional key exchange algorithms (for example, classic, PQC, or hybrid with an additional round) for IKE SA and apply the same algorithms to CHILD SAs. In future releases, Child SA (Phase 2) key agreement can be made independently configurable if required. Note the following for Configuring additional-key-exchange algorithms:

- Configuring the additional key exchange algorithms is optional.
- Maximum 7 rounds of additional-key-exchange can be configured. The user must choose a unique key-exchange algorithm for each round. The additional-key-exchange rounds must be configured in the same order on both IKE peers. If the configuration does not match, an IKE-CONFIG-MISMATCH alarm is raised.
- Port level key-exchange (CHILD-SA) will be derived from IKE.
- Configuring additional-key-exchange causes the IKE session to go down immediately and renegotiate the new proposal.
- To make the IKE session Post Quantum Cryptography (PQC) safe, configure either PPK or at least one post-quantum key exchange algorithm. The supported post-quantum key exchange algorithms are ml-kem-512, ml-kem-768, and ml-kem-1024. The recommended algorithm is ml-kem-1024, because it provides the highest strength.

This command is used to add, edit or show additional key exchange algorithms. Use the `delete` command to delete additional key exchange algorithms.

#### Command Syntax

```
add additional-key-exchange-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<additional-key-exchange-id> dh-group <value>
set additional-key-exchange-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<additional-key-exchange-id> [dh-group <value>]
show additional-key-exchange-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<additional-key-exchange-id> [dh-group]
delete additional-key-exchange-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<additional-key-exchange-id>
```

<!-- page 171 -->

#### Command Usage Details

**Table 91: additional-key-exchange Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 92: additional-key-exchange Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| number | The proposal number for the IKE SA. | number 1..max | n/a | add, set, show, delete |
| additional-key-exchange-id | Specifies the number of rounds of additional key exchange algorithms to be configured. | 1..7 | n/a | add, set, show |
| dh-group | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer. | dhe-2048, dhe-3072, dhe-4096, dhe-6144, dhe-8192, ecp-256, ecp-384, ecp-521, curve-25519, curve-448, ml-kem-512,ml-kem-768,ml-kem-1024 | n/a | add, set, show |

#### Examples

This example shows how to configure 7 additional rounds of key exchange algorithms on line card 1-6:

```
add additional-key-exchange-1-6/NEA/1/1 dh-group curve-25519
add additional-key-exchange-1-6/NEA/1/2 dh-group ml-kem-768
add additional-key-exchange-1-6/NEA/1/3 dh-group ml-kem-1024
add additional-key-exchange-1-6/NEA/1/4 dh-group ecp-256
add additional-key-exchange-1-6/NEA/1/5 dh-group ecp-384
add additional-key-exchange-1-6/NEA/1/6 dh-group ml-kem-512
add additional-key-exchange-1-6/NEA/1/7 dh-group curve-448
```

<!-- page 173 -->
