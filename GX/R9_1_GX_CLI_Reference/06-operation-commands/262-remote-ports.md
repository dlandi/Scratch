---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.262. remote-ports'
source_lines: 20058-20103
---

## 6.262. remote-ports

#### Command Description

This command is used to add or show a remote port. The delete command is used to delete a remote port.

#### Command Syntax

```
add remote-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
show remote-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
delete remote-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>
```

#### Command Usage Details

**Table 616: remote-ports Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 617: remote-ports Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| ipsec-spd-entry-name | A unique name to identify this SPD entry. | string (length 1..32) | n/a | add, show, delete |
| ipsec-traffic-selector-name | A unique name to identify this IPsec traffic selector entry. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, show, delete |
| start | The values for the starting port. | all, opaque, port number | n/a | add, show, delete |
| stop | The values for the stopping port. If the stopping port is not set, the system assumes the value is 0. However, the value 0 is only accepted by the system if the starting port is set to all or opaque. | port-number | 0 | add, show, delete |

#### Examples

These examples provide the commands to add remote ports:

```
add remote-ports-ipsec/GX2/protect1/ts1/49/49
add remote-ports-ipsec/GX2/protect1/ts1/all
```

<!-- page 1011 -->
