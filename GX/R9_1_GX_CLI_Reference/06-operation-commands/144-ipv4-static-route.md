---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.144. ipv4-static-route'
source_lines: 13002-13054
---

## 6.144. ipv4-static-route

#### Command Description

These commands are used to add/show/delete a list of IPv4 static routes to the interface.

#### Command Syntax

```
add ipv4-static-route-<ipv4-destination-prefix>/<vrf> [next-hop-address <value>] [advertised <value>] [distance <value>] [interface <value>]
[origin <value>] [label <value>] [special-next-hop <value>]
set ipv4-static-route-<ipv4-destination-prefix>/<vrf> [label <value>]
show ipv4-static-route-<ipv4-destination-prefix>/<vrf> [advertised] [next-hop-address] [distance] [interface] [monitoring-state]
[monitoring-instance] [origin] [label] [special-next-hop]
delete ipv4-static-route-<ipv4-destination-prefix>/<vrf>
```

#### Command Usage Details

**Table 376: ipv4-static-route Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate configuration mode |

#### Command Parameters

**Table 377: ipv4-static-route Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ipv4-destination-prefix | IPv4 destination prefix. | IPv4 address | n/a | add, show, delete |
| vrf | VRF to which this interface is bound. | VRF | n/a | add, show, delete |
| advertised | When set to YES, the static route is advertised in the routing protocol. For OSPF, the static route will be advertised as an AS external route, if OSPF is configured as an ASBR. | true, false | false | add, show |
| next-hop-address | Next hope address. | IP address | n/a | add, show |
| distance | Distance to the next hop. | Number (range: 1..255) | 1 | add, show |
| interface | Interface associated with this static route. The VRF bound to this interface needs to match the static-route provided vrf. | Interface name | n/a | add, show |
| monitoring-state | The current state of the monitoring. unmonitored: static-route is not part of any ip monitoring instance. ok: static-route is part of an ip monitoring instance in 'ok' state. failed: static-route is part of an ip monitoring instance in 'failed' state. | unmonitored ok failed | unmonitored | show |
| monitoring-instance | Monitoring instance name, applicable only if this route is being monitored. | String (length: 0..64 characters) | n/a | show |
| origin | Route address assignment method. manual: Indicates the ipv4 route has been manually configured. dhcp: Indicates ipv4 route has been assigned to this system by a DHCP server. | manual dhcp | manual | add, show |
| label | User-defined label. | String (length 0..256) | n/a | add, set, show |
| special-next-hop | The routes to be advertised to external AS must exist in the forwarding table installed by an Interior Gateway Protocol (IGP) such as OSPF or static routes, but not BGP itself. For routes not present in IGP tables, blackhole static routes must be configured. This parameter allows you to configure blackhole static routes. | blackhole | blackhole | add, set,show |

#### Examples

This example shows how to add an ipv4 static route:

```
add ipv4-static-route-10.220.0.0/16/MGMT next-hop-address 10.220.225.165
```

<!-- page 586 -->
