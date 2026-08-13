---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.74. db-protection-scheme'
source_lines: 8575-8620
---

## 6.74. db-protection-scheme

#### Command Description

The command described in this section is used to show the `db-protection-scheme` attributes.

#### Command Syntax

```
show db-protection-scheme [encryption-algorithm] [integrity-algorithm] [integrity-status] [mode]
```

#### Command Usage Details

**Table 228: db-protection-scheme Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 229: db-protection-scheme Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| encryption-algorithm | Encryption algorithm used for database encryption:<br>• xts-aes-256-plain64 - AES-256 encryption in XTS mode with 64-byte block size. | xts-aes-256-plain64 | n/a | show |
| integrity-algorithm | Type of integrity algorithm used for DB. | • hmac-sha2-512<br>• none | n/a | show |
| integrity-status | Indicates the status of integrity check. | • passed-on-bootup<br>• failed-on-bootup<br>• disabled | n/a | show |
| mode | Current Protection Scheme of DB. Can be changed via 'db-migrate' RPC. | • encryption-only<br>• encryption-with-integrity | n/a | show |

#### Examples

The following command shows how to check the current database protection scheme configured

```
temproot@GX> show db-protection-scheme
  db-protection-scheme
  encryption-algorithm              xts-aes-256-plain64
  integrity-algorithm               none
  integrity-status                  disabled
  mode                              encryption-only
```

<!-- page 378 -->
