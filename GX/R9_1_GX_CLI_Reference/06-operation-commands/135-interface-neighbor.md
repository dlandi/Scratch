---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.135. interface-neighbor'
source_lines: 12495-12552
---

## 6.135. interface-neighbor

#### Command Description

The commands described in this section are used to set or show the `interface-neighbor` attributes.

#### Command Syntax

```
set interface-neighbor-<local-interface> [discovery-cycle-time <value>] [discovery-timeout <value>] [discovery-enabled <value>]
[alarm-report-control <value>] [label <value>]
show interface-neighbor-<local-interface> [associated-comm-channel] [discovery-cycle-time] [discovery-timeout] [discovery-enabled]
[neighbor-adjacency-state] [neighbor-ne-id] [neighbor-ne-name] [neighbor-interface-name] [neighbor-router-id] [neighbor-ipv4-address]
[neighbor-ipv6-address] [last-change-time] [alarm-report-control] [label] [AID] [oper-state]
```

#### Command Usage Details

**Table 358: interface-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 359: interface-neighbor Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| local-interface | Name of interface neighbor. | string (length 1..128) | n/a | set, show |
| associated-comm-channel | Associated communication channel of provisioned neighbor. | string (length 1..64) | n/a | show |
| discovery-cycle-time | Periodicity at which sndp discover messages will be sent. | Values in the range [30..300] seconds | 30 | set, show |
| discovery-timeout | Time after which discovery is considered as failed; when this timeout occurs, neighbor-adjacency state will transition to blackout. | Values in the range [300..1800] seconds | 300 | set, show |
| discovery-enabled | It is a switch to enable or disable discovery on the local interface. | true, false | true | set, show |
| neighbor-adjacency-state | Indicates protocol state. | blackout, discovery, holding, unknown | unknown | show |
| neighbor-ne-id | Indicates discovered neighbor ne ID. | string (length 1..128) | n/a | show |
| neighbor-ne-name | Indicates discovered neighbor ne name. | string (length 1.. 255) | n/a | show |
| neighbor-interface-name | Indicates discovered neighbor interface name. | string (length 1..128) | n/a | show |
| neighbor-router-id | Indicates discovered neighbor router ID. | string (IPv4/ IPv6 address) | n/a | show |
| neighbor-ipv4-address | Indicates discovered neighbor ipv4 address. | string (IPv4/ IPv6 address) | n/a | show |
| neighbor-ipv6-address | Indicates discovered neighbor ipv6 address. | string (IPv4/ IPv6 address) | n/a | show |
| last-change-time | Provide a timestamp indicating when the interface neighbor information was last updated. | date-and-time | n/a | show |
| oper-state | The operational state of interface-neighbor object. | enabled, disabled | disabled | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | allowed | set, show |
| label | User-configurable label. | string (length 1..255) | n/a | set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string (length 1..64) | n/a | show |

#### Examples

This example shows how to show the interface neighbour information:

```
show interface-neighbor
```

<!-- page 562 -->
