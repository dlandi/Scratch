---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.263. remote-subnet'
source_lines: 20104-20147
---

## 6.263. remote-subnet

#### Command Description

This command is used to add or show a remote subnet. The delete command is used to delete a remote subnet.

#### Command Syntax

```
add remote-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
show remote-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
delete remote-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>
```

#### Command Usage Details

**Table 618: remote-subnet Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 619: remote-subnet Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, delete, show |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, show, delete |
| ipsec-traffic-selector-name | A unique name to identify this IPsec traffic selector entry. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| prefix | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)). | number | n/a | add, show, delete |

#### Examples

This example shows how to add a remote subnet:

```
add remote-subnet-ipsec/GX2/dns/ts1/102.20.20.2/32
```

<!-- page 1013 -->
