---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.198. ntp-server-status'
source_lines: 15929-15976
---

## 6.198. ntp-server-status

#### Command Description

These commands are used to configure and show the NTP server status.

#### Command Syntax

```
show ntp-server-status-<ip-address> [refid] [stratum] [type] [when] [poll] [reach] [delay] [offset] [jitter] [auth-status] [condition]
```

#### Command Usage Details

**Table 482: ntp-server-status Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 483: ntp-server-status Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| refid | Reference clock type or address for the peer. | String (length 1..32) | n/a | show |
| stratum | Indicates the stratum of the remote peer. | Number | n/a | show |
| type | Type of the peer (local, unicast, multicast or broadcast). | String (length 1..10) | n/a | show |
| when | Indicates time elapsed since last packet was received in seconds. | Number (sec) | n/a | show |
| poll | Indicates the polling interval in seconds. | Number (sec) | n/a | show |
| reach | Indicates the reachability of the configured server. This is an 8-bit shift register with the most recent probe in the 2^0 position. The value 377 indicates that all the recent probes have been answered. | Number | n/a | show |
| delay | Delay along path to the server in milliseconds. | Number (ms) | n/a | show |
| offset | Offset of clock to the peer in milliseconds. | Number (ms) | n/a | show |
| jitter | Jitter along path to the server in milliseconds. | Number (ms) | n/a | show |
| auth-status | Authentication status of NTP server. | ok yes bad none | none | show |
| condition | Condition of NTP server. Some of possible values: sys.peer/reject/candidate/... . | String (length 1..16) | n/a | show |

#### Examples

This example shows how to view an NTP server status:

```
show ntp-server-status-172.19.13.218
```

<!-- page 732 -->
