---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.278. secure-entity-sa-proposal'
source_lines: 20875-20909
---

## 6.278. secure-entity-sa-proposal

#### Command Description

The command described in this section is used to show the `secure-entity-sa-proposal` attributes.

#### Command Syntax

```
show secure-entity-sa-proposal-<name>/<number> [encryption-algorithm] [encryption-key-length] [integrity-algorithm] [dh-group]
```

#### Command Usage Details

**Table 649: secure-entity-sa-proposal Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 650: secure-entity-sa-proposal Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The proposal name for the secure entity SA. | string | n/a | show |
| number | The proposal number for the secure entity SA. | 1 | 1 | show |
| encryption-algorithm | The encryption algorithm for the secure entity SA. | aes-gcm-16 | aes-gcm-16 | show |
| encryption-key-length | The secure entity SA encryption algorithm key length. | key-length-256 | key-length-256 | show |
| integrity-algorithm | Secure entity SA integrity algorithm advertised to the far-end secure entity peer. | none | none | show |
| dh-group | Secure entity SA Diffie-Hellman group advertised to the far-end secure entity peer. | ecp-521 | ecp-521 | show |

<!-- page 1047 -->
