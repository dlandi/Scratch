---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.355. traceroute'
source_lines: 26569-26623
---

## 6.355. traceroute

#### Command Description

This command is used to track the route packets taken from an IP network on their way to a given host. Traceroute uses the IP protocol's time to live (TTL) field and attempts to elicit an ICMP TIME\_EXCEEDED response from each gateway along the path to the host. After the trip time, some additional annotation can be printed: !H, !N, or !P (host, network or protocol unreachable), !S (source route failed), !F (fragmentation needed), !X (communication administratively prohibited), !V (host precedence violation), !C (precedence cutoff in effect), or !\<num\> (ICMP unreachable code \<num\>).

#### Command Syntax

```
traceroute [-m=<hopcnt>] [-w=<timeout>] [-i=<interface> | -v=<vrf>] <tr-dest> [pktsize]
```

#### Command Usage Details

**Table 816: traceroute Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 817: traceroute Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| -m=&lt;hopcnt&gt; | Specifies the maximum number of hops (max time-to-live value) traceroute will probe. | uint8 (range 1..255) | 30 |
| -w=&lt;timeout&gt; | Specifies the timeout, in seconds, before traceroute exits. | uint16 (range 1..10) | 2 |
| -i=&lt;interface&gt; | Specifies source interface. | leafref (path "/ioa-ne:ne/ioa-ne:system/ioa-ne:networking/ioa-ne:interface/ioa-ne:if-name") | By default, the interface is selected according to the routing table. |
| -v=&lt;vrf&gt; | Specifies VRF. VRF is to be used. If not provided, defaults to MGMT. i Note: The interface and VRF name parameters are mutually exclusive. | string | MGMT |
| pktsize | Specifies the total size of the probing packet. | uint16 (60 bytes for IPv4) | 60 |
| tr-dest | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | IPv4 address, IPv6 address, domain-name | n/a |

#### Examples

This example shows how to perform a traceroute for this IP with default parameters:

```
traceroute 1.2.3.4
```

This example shows how to perform a traceroute for this IP with max of 10 hops and timeout 3 seconds:

```
traceroute -m=10 -w=3 1.2.3.4
```

This example shows how to perform a traceroute for this IP with packet size of 100 bytes:

```
traceroute 1.2.3.4 100
```

<!-- page 1297 -->
