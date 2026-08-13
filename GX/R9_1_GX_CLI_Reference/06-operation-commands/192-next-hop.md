---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.192. next-hop'
source_lines: 15571-15619
---

## 6.192. next-hop

#### Command Description

This command is used to show the next hop in a route. Each entry represents a RIB identified by the 'name' key. All routes in a RIB belong to the same address family. For each routing instance, the system will provide one system-controlled default RIB for each supported address family.

#### Command Syntax

```
show next-hop-<rib-name>/<destination-prefix>/<interface> [next-hop-address]
```

#### Command Usage Details

**Table 470: next-hop Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 471: next-hop Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| rib-name | The name of the RIB. | String (length 1..64; pattern '([A-Za-z0-9 -.,]*)') _ | n/a | show |
| destination-prefix | IP destination prefix. | IP prefix | n/a | show |
| interface | Reference of the outgoing interface. | interface name (for example: DCN) | n/a | show |
| next-hop-address | IP address of the next-hop. | IPv4 or IPv6 address | n/a | show |

#### Examples

This example shows how to view the next-hop of the available route items:

<!-- page 705 -->

```
show next-hop
```

This example shows how to view the next-hop IP address:

```
show next-hop-IPv4/0.0.0.0/0/DCN
```

<!-- page 706 -->
