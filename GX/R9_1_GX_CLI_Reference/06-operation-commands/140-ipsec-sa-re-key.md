---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.140. ipsec-sa-re-key'
source_lines: 12806-12843
---

## 6.140. ipsec-sa-re-key

#### Command Description

This command is used to add, edit or show ipsec sa re key. Use the delete command to delete ipsec sa re key.

#### Command Syntax

```
add ipsec-sa-re-key-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> [frequency <value>] [bytes <value>] [packets <value>]
set ipsec-sa-re-key-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> [frequency <value>] [bytes <value>] [packets <value>]
show ipsec-sa-re-key-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name> [frequency] [bytes] [packets]
delete ipsec-sa-re-key-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>
```

#### Command Usage Details

**Table 368: ipsec-sa-re-key Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 369: ipsec-sa-re-key Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, set, show, delete |
| frequency | The rekeying frequency for the IPsec child security association with the far-end peer. | number (range 3600..86400 seconds) | 14400 | add, set, show |
| bytes | The rekeying frequency for the IPsec child security association with the far-end peer based on amount of bytes transmitted. | number (range 1048576..max) | 1073741824 | add, set, show |
| packets | The rekeying frequency for the IPsec child security association with the far-end peer based on amount of packets transmitted. | disabled, number (range 1 .. 9223372036854775808) | disabled | add, set, show |

<!-- page 576 -->
