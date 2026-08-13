---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.91. encryption-algorithm'
source_lines: 10015-10062
---

## 6.91. encryption-algorithm

#### Command Description

This command is used to add or show encryption-algorithm attributes. Use the delete command to remove a encryption-algorithm attributes from the configuration.

#### Command Syntax

```
add encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>/<algorithm>/<key-length>
add encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<algorithm>/<key-length>
show encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>/<algorithm>/<key-length>
show encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<algorithm>/<key-length>
delete encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>/<algorithm>/<key-length>
delete encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<algorithm>/<key-length>
```

#### Command Usage Details

**Table 266: encryption-algorithm Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 267: encryption-algorithm Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, show, delete |
| number | The proposal number for the IKE SA. | number 1..max | n/a | add, show, delete |
| algorithm | The encryption algorithm for the IKE SA. | null, aes-gcm-8, aes-gcm-12, aes-gcm-16, aes-ctr, aes-cbc, aes-ccm-8, aes-ccm-12, aes-ccm-16, chacha20-poly1305 | n/a | add, show, delete |
| key-length | The IKE SA encryption algorithm key length. | none, key-length-128, key-length-192, key-length-256 | none | add, show, delete |

#### Examples

This example shows how to add an encryption algorithm:

```
add encryption-algorithm-ipsec/GX2/1/aes-gcm-8/key-length-192
```

<!-- page 439 -->
