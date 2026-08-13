---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.142. ipsec-traffic-selector'
source_lines: 12901-12949
---

## 6.142. ipsec-traffic-selector

#### Command Description

This command is used to add, edit or show ipsec traffic selector. Use the delete command to delete ipsec traffic selector.

#### Command Syntax

```
add ipsec-traffic-selector-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>
[next-layer-protocol <value>]
set ipsec-traffic-selector-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>
[next-layer-protocol <value>]
show ipsec-traffic-selector-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name> [AID]
[next-layer-protocol]
delete ipsec-traffic-selector-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>
```

#### Command Usage Details

**Table 372: ipsec-traffic-selector Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 373: ipsec-traffic-selector Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, delete, show |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, set, show, delete |
| ipsec-traffic-selector-name | A unique name to identify this IPsec traffic selector entry. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| next-layer-protocol | Indicates the inner protocol (upper layer), obtained from the IPv4 protocol or the IPv6 next header field. | any (value 0), opaque (value 255), number (range 0-255) | any | add, set, show |

#### Examples

This example shows how to add a traffic selector:

```
add ipsec-traffic-selector-ipsec/GX2/Radius/ts1 next-layer-protocol 17
```

<!-- page 581 -->
