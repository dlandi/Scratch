---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.41. carrier-neighbor'
source_lines: 6550-6589
---

## 6.41. carrier-neighbor

#### Command Description

This command is used to show a Local carrier instance that has discovered this neighbor node. Each carrier can discover up to one node. It is possible for multiple collocated carriers to discover the same node multiple times (each time connected to a different remote carrier.

#### Command Syntax

```
show carrier-neighbor-<local-carrier >[last-update] [age] [local-carrier-id] [ne-id] [ne-name] [ne-type] [remote-carrier-id]
[ipv4-loopback-address] [ipv6-loopback-address]
```

#### Command Usage Details

**Table 154: carrier-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 155: carrier-neighbor Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| local-carrier | Local carrier instance that has discovered this neighbor node | string length 0..16 | n/a |
| last-update | Time of the last update | date-time in the format YYYY-MM-DDThh: mm:ssZ) see the set-time command for detailed information. | n/a |
| age | Hardware version of this FRU. | string | n/a |
| local-carrier-id | AID of local carrier | string | n/a |
| ne-id | Id of the remote network element. | string length 0..256 | n/a |
| ne-name | Neighbor ne-name | string length 0..256 | n/a |
| ne-type | Type of the remote network element | string length 0..64 | n/a |
| remote-carrier-id | AID of the remote carrier connected to the local carrier. Implies a specific remote port id | string length 0..64 | n/a |
| ipv4-loopback-address | IPv4 loopback address of the neighbor; may be empty if not configured. | loopback address | n/a |
| ipv6-loopback-address | IPv6 loopback address of the neighbor; may be empty if not configured. | loopback address | n/a |

<!-- page 279 -->
