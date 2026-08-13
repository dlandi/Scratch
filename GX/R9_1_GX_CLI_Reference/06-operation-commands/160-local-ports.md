---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.160. local-ports'
source_lines: 13957-14007
---

## 6.160. local-ports

#### Command Description

This command is used to add or show local ports. Use the delete command to delete local ports.

#### Command Syntax

```
add local-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
show local-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
delete local-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
```

#### Command Usage Details

**Table 408: local-ports Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 409: local-ports Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, show, delete |
| ipsec-traffic-selector-name | A unique name to identify this IPsec traffic selector entry. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| start | The values for the starting port. | all, opaque, port number | n/a | add, show, delete |
| stop | The values for the stopping port. If the stopping port is not set, the system assumes the value is 0. However, the value 0 is only accepted by the system if the starting port is set to all or opaque. | port-number | 0 | add, show, delete |

#### Examples

This example shows how to add local ports:

```
add local-ports-ipsec/GX2/tacacs/ts1/all
```

This example shows how to add an individual port start=stop=53:

```
add local-ports-ipsec/GX1/dns/ts1/53/53
```

<!-- page 629 -->
