---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.269. route'
source_lines: 20436-20476
---

## 6.269. route

#### Command Description

This command is used to show the list of system routes from various sources, such as dynamic protocols and static route.

#### Command Syntax

```
show route-<rib-name>/<destination-prefix> [special-next-hop] [source-protocol]
```

#### Command Usage Details

**Table 631: route Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 632: route Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| rib-name | The name of the RIB. | String (length 1..64) | n/a | show |
| destination-prefix | IP destination prefix. | IPv4 or IPv6 address | n/a | show |
| special-next-hop | Special next hop used for IPv4, IPv6 static routes, rib:routes and IP Monitoring. Indicates the special-next-hop applicable to a route: none - no special treatment of routes, used for all for normal routes. blackhole - For the blackhole routes next-hop will not be created under route. unreachable - For the unreachable routes next-hop will not be created under route. | none blackhole unreachable | none | show |
| source-protocol | Source protocol for the route entry. | Source protocol for example OSPF, BGP, static etc. | n/a | show |

#### Examples

This example shows how to view the list of system routes:

```
show route
```

<!-- page 1026 -->
