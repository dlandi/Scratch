---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.138. ip-monitoring'
source_lines: 12703-12754
---

## 6.138. ip-monitoring

#### Command Description

These commands are used to add, edit or show Monitoring instance configuration and state. A monitoring instance allows for periodically pinging certain destinations whose result takes action on configured static-routes. Use the delete command to delete IP monitoring from the configuration.

#### Command Syntax

```
add ip-monitoring-<name> destination <value> next-hop <value> [probe-interval <value>] [drop-rate <value>] [enabled <value>] [action <value>]
[static-route <value>] [alarm-report-control <value>]
set ip-monitoring-<name> [probe-interval <value>] [destination <value>] [drop-rate <value>] [enabled <value>] [action <value>] [static-route
<value>] [next-hop <value>] [alarm-report-control <value>]
show ip-monitoring-<name> [probe-interval] [destination] [drop-rate] [enabled] [monitoring-state] [action] [static-route] [next-hop]
[alarm-report-control]
delete ip-monitoring-<name>
```

#### Command Usage Details

**Table 364: ip-monitoring Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 365: ip-monitoring Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the IP monitoring instance | String ( 1..64) | n/a | add, set, delete, show |
| destination | The remote host to monitor. | ipv4-address, ipv6-address | n/a | add, set, show |
| next-hop | Defines the exit interface to use which can be ipv4 or ipv6 source IP address. The monitoring instance will not be active until the exit interface is configured. | ipv4-address, ipv6-address | n/a | add, set, show |
| probe-interval | The time between two consecutive pings in seconds. | seconds 0...60 | 5 | add, set, show |
| drop-rate | The accepted drop rate of ping in 10% steps. | range 1..10 | 1 | add, set, show |
| enabled | Start or Stop the monitoring of the destination by setting to true or false | true, false | true | add, set, show |
| action | The action to take when the monitoring goes into 'failed' state. | none, withdraw | withdraw | add, set, show |
| static-route | The list of connected static routes for this Monitoring instance. | string 10 elements maximum | n/a | add, set, show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | allowed | add, set, show |

#### Examples

This example shows how to enable IP monitoring:

```
set ip-monitoring enabled true
```

<!-- page 572 -->
