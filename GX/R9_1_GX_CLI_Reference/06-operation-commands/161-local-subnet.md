---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.161. local-subnet'
source_lines: 14008-14051
---

## 6.161. local-subnet

#### Command Description

This command is used to add or show a local subnet. Use the delete command to delete a local subnet.

#### Command Syntax

```
add local-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
show local-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
delete local-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
```

#### Command Usage Details

**Table 410: local-subnet Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 411: local-subnet Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, delete, show |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, show, delete |
| ipsec-traffic-selector-name | A unique name to identify this IPsec traffic selector entry. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| prefix | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)). | number | n/a | add, show, delete |

#### Examples

This example shows how to add a local subnet:

```
add local-subnet-ipsec/GX2/dns/ts1/101.10.10.1/32
```

<!-- page 631 -->
