---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.235. ping'
source_lines: 18426-18487
---

## 6.235. ping

#### Command Description

This command sends an echo message to another TCP/IP node to determine if the node is visible on the network. `ping` command uses the ICMP protocol's mandatory ECHO\_REQUEST datagram to elicit an ICMP ECHO\_RESPONSE from a host or gateway. ECHO\_REQUEST datagrams ('pings') have an IP and ICMP header, followed by a struct timeval and then an arbitrary number of 'pad' bytes used to fill out the packet.

#### Command Syntax

```
ping -h
ping [-c=<count>] [-w=<timeout>] [-s=<pktsize>] [-i=<interface> | -v=<vrf>] <ping-dest>
```

#### Command Usage Details

**Table 560: ping Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 561: ping Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -c=&lt;count&gt; | Stop after sending count ECHO REQUEST packets. With deadline option, ping waits for _ count ECHO REPLY packets, until the timeout expires. _ |
| -w=&lt;timeout&gt; | Specify a timeout, in seconds, before ping exits regardless of how many packets have been sent or received. In this case ping does not stop after count packet are sent, it waits either for deadline expire or until count probes are answered or for some error notification from network. |
| -s=&lt;pktsize&gt; | Specifies the number of octets to be sent, exclusive of all headers. Default is 56, plus 8 octets of ICMP header for a total packet size of 64 octets. |
| -i=&lt;interface&gt; | Specifies source interface. By default, the interface is selected according to the routing table. |
| -v=&lt;vrf&gt; | Specifies VRF. By default, use the MGMT VRF. i Note: The interface and VRF name parameters are mutually exclusive. |

**Table 562: ping Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| ping-dest | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | ipv4-address, ipv6-address, domain-name | n/a |

#### Examples

This example shows how to ping this IP with default options:

```
ping 192.0.2.1
```

This example shows how to ping this IP five times:

```
ping -c=5 192.0.2.2
```

This example shows how to ping this IP this IP 1 single time with a 1024 byte packet:

```
ping -c=1 -w=10 -s=1024 192.0.2.3
```

<!-- page 934 -->
