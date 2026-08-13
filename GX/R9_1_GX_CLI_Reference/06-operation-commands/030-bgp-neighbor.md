---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.30. bgp-neighbor'
source_lines: 5934-5997
---

## 6.30. bgp-neighbor

#### Command Description

This command is used to add/edit/show a BGP neighbor.

#### Command Syntax

```
add bgp-neighbor-<instance-id>/<remote-address> peer-as <value> [afi-safis <value>] [enabled <value>] [description <value>] [secure-session
<value>] [password <value>] [connect-retry-interval <value>] [hold-time <value>] [keepalive-interval <value>]
delete bgp-neighbor-<instance-id>/<remote-address>
set bgp-neighbor-<instance-id>/<remote-address> [enabled <value>] [description <value>] [secure-session <value>] [password <value>]
[connect-retry-interval <value>] [hold-time <value>] [keepalive-interval <value>]
show bgp-neighbor-<instance-id>/<remote-address> [enabled] [peer-as] [description] [afi-safis <value>] [secure-session] [password]
[connect-retry-interval] [hold-time] [keepalive-interval] [negotiated-hold-time] [session-state] [known-errors]
```

#### Command Usage Details

**Table 131: bgp-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 132: bgp-neighbor Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | Name of instance. | string (length 1...64) | n/a | add, set, delete, show |
| remote-address | Address of the BGP peer. | IPv4 or IPv6 address | n/a | add, set, delete, show |
| peer-as | AS number of the peer. | number (1 .. 4294967295) | n/a | add, set, show |
| enabled | Whether the BGP peer is enabled. In cases where the enabled leaf is set to false, the local system must not initiate connections to the neighbor, and must not respond to TCP connections attempts from the neighbor. If the state of the BGP session is ESTABLISHED at the time that this leaf is set to false, the BGP session must be ceased. | true, false | true | add, set, show |
| description | An optional textual description (intended primarily for use with a peer or group). | string (0..128) | n/a | add, set, show |
| afi-safi | Specifies the afi-safi value. GNE only exports and imports IPv4 or IPv6 unicast with afi-safi value set to IPv4 unicast or IPv6 unicast. _ _ | • IPv4-unicast<br>• IPv6-unicast | • IPv4-unicast, if remote address is set to IPv4<br>• IPv6-unicast, if remote address is set to IPv6 | add, set, show |
| secure-session | Authentication method of the session to the peer. | none, TCP-MD5 | none | add, set, show |
| password | Password as TCP-MD5 authentication key in ASCII format. | string (0..80) | n/a | add, set, show |
| connect-retry-interval | Time interval in seconds between attempts to establish a session with the peer. | seconds (1..65535) | 120 | add, set, show |
| hold-time | Time interval in seconds that a BGP session will be considered active in the absence of keepalive or other messages from the peer. The hold-time is typically set to 3x the keepalive-interval | seconds (3..65535) | 90 | add, set, show |
| keepalive-interval | Time interval in seconds between transmission of keepalive messages to the neighbor. Typically set to 1/3 the hold-time. | seconds (range 1..21845) | 30 | add, set, show |
| negotiated-hold-time | Negotiated hold time between two BGP neighbors. | seconds (range 0..65535) | 0 | show |
| session-state | Current BGP Session state in ASCII format. | Idle - Idle state Connect - Connect state Active - Active state OpenSent - OpenSent state OpenConfirm - OpenConfirm state Established - Established state Close - Close state | Idle | show |
| known-errors | Current BGP Session state errors if any ASCII format. | string (length 0..256) | n/a | show |

#### Examples

The following example shows how to show bgp neighbor:

<!-- page 241 -->

```
show bgp-neighbor
bgp-neighbor enabled peer-as description secure-session connect-retry-interval (seconds) hold-time (seconds) keepalive-interval (seconds)
 negotiated-hold-time (seconds) session-state known-errors
----------------------------- ------- ------- ----------- -------------- -------------------------------- -------------------
 ---------------------------- ------------------------------ ------------- ------------
bgp-neighbor-10/10.220.142.86 true 1 none 120 90 30 90 Established
```

<!-- page 242 -->
